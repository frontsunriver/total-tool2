package com.innomalist.taxi.driver.activities.travel

import android.Manifest
import android.content.Context
import android.content.DialogInterface
import android.content.Intent
import android.content.pm.PackageManager
import android.location.Location
import android.location.LocationListener
import android.location.LocationManager
import android.net.Uri
import android.os.Bundle
import android.view.View
import android.view.ViewGroup
import android.view.WindowManager
import androidx.appcompat.app.AlertDialog
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.databinding.DataBindingUtil
import androidx.transition.TransitionManager
import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.GoogleMap
import com.google.android.gms.maps.OnMapReadyCallback
import com.google.android.gms.maps.SupportMapFragment
import com.google.android.gms.maps.model.BitmapDescriptorFactory
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.Marker
import com.google.android.gms.maps.model.MarkerOptions
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import com.google.android.material.snackbar.Snackbar
import com.google.android.material.textfield.TextInputEditText
import com.google.maps.android.PolyUtil
import com.innomalist.taxi.common.activities.chat.ChatActivity
import com.innomalist.taxi.common.interfaces.AlertDialogEvent
import com.innomalist.taxi.common.location.MapHelper.centerLatLngsInMap
import com.innomalist.taxi.common.models.Request
import com.innomalist.taxi.common.networking.socket.Cancel
import com.innomalist.taxi.common.networking.socket.CurrentRequestResult
import com.innomalist.taxi.common.networking.socket.GetCurrentRequestInfo
import com.innomalist.taxi.common.networking.socket.interfaces.EmptyClass
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import com.innomalist.taxi.common.networking.socket.interfaces.SocketNetworkDispatcher
import com.innomalist.taxi.common.utils.AlertDialogBuilder
import com.innomalist.taxi.common.utils.AlertDialogBuilder.show
import com.innomalist.taxi.common.utils.AlerterHelper
import com.innomalist.taxi.common.utils.DistanceFormatter
import com.innomalist.taxi.common.utils.LoadingDialog
import com.innomalist.taxi.common.utils.LocationHelper.Companion.distFrom
import com.innomalist.taxi.driver.R
import com.innomalist.taxi.driver.activities.waitingForPayment.WaitingForPaymentActivity
import com.innomalist.taxi.driver.databinding.ActivityTravelBinding
import com.innomalist.taxi.driver.networking.http.LocationUpdate
import com.innomalist.taxi.driver.networking.socket.Arrived
import com.innomalist.taxi.driver.networking.socket.Finish
import com.innomalist.taxi.driver.networking.socket.FinishResult
import com.innomalist.taxi.driver.networking.socket.Start
import com.innomalist.taxi.driver.ui.DriverBaseActivity
import java.text.NumberFormat
import java.util.*
import kotlin.collections.ArrayList

class TravelActivity : DriverBaseActivity(), OnMapReadyCallback, LocationListener {
    private var gMap: GoogleMap? = null
    private var currentLocation: LatLng? = null
    lateinit var binding: ActivityTravelBinding
    private var pointMarkers: MutableList<Marker> = ArrayList()
    private var driverMarker: Marker? = null
    private var locationManager: LocationManager? = null
    private var geoLog: MutableList<LatLng> = ArrayList()
    private val permissionPhoneCallRequestCode = 400

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = DataBindingUtil.setContentView(this, R.layout.activity_travel)
        val mapFragment = (supportFragmentManager.findFragmentById(R.id.map) as SupportMapFragment?)!!
        mapFragment.getMapAsync(this)
        window.addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON)
        SocketNetworkDispatcher.instance.onPaid = {
            finish()
        }
        SocketNetworkDispatcher.instance.onCancel = {
            val req = travel!!
            req.status = Request.Status.RiderCanceled
            travel = req
            refreshPage()
        }

        locationManager = getSystemService(Context.LOCATION_SERVICE) as LocationManager
        binding.slideStart.setOnSlideCompleteListener { startTravel() }
        binding.slideFinish.setOnSlideCompleteListener { finishTravel() }
        binding.slideCall.setOnSlideCompleteListener { callRider() }
        binding.slideArrived.setOnSlideCompleteListener {
            LoadingDialog.display(this)
            Arrived().execute<Request> {
                LoadingDialog.hide()
                when (it) {
                    is RemoteResponse.Success -> {
                        travel = it.body
                        refreshPage()
                    }

                    is RemoteResponse.Error -> {
                        it.error.showAlert(this)
                    }
                }

            }
        }
        binding.chatButton.setOnClickListener { onChatButtonClicked() }
        binding.slideCancel.setOnSlideCompleteListener {
            run {
                LoadingDialog.display(this)
                Cancel().execute<EmptyClass> {
                    LoadingDialog.hide()
                    when (it) {
                        is RemoteResponse.Success -> {
                            val request = travel
                            request!!.status = Request.Status.DriverCanceled
                            travel = request
                            refreshPage()
                        }
                        is RemoteResponse.Error -> {
                            it.error.showAlert(this)
                        }
                    }
                }
            }
        }
        Timer().schedule(object : TimerTask() {
            override fun run() {
                runOnUiThread {
                    val request = travel
                    val timeStamp = if (request!!.startTimestamp == null) (request.etaPickup
                            ?: 0) else (request.startTimestamp ?: 0) + (request.durationBest
                            ?: 0) * 1000
                    val seconds = (timeStamp - Date().time) / 1000
                    if (seconds <= 0) binding.etaText.setText(R.string.eta_soon) else binding.etaText.text = String.format(Locale.getDefault(), "%02d:%02d", seconds / 60, seconds % 60)
                }
            }
        }, 0, 1000)
    }

    override fun onReconnected() {
        super.onReconnected()
        requestRefresh()
    }

    override fun onResume() {
        super.onResume()
        // window.setFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS, WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS)
        if (ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION) == PackageManager.PERMISSION_GRANTED || ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_COARSE_LOCATION) == PackageManager.PERMISSION_GRANTED) {
            locationManager!!.requestLocationUpdates(LocationManager.GPS_PROVIDER, 5000, 20f, this)
        }
        SocketNetworkDispatcher.instance.onNewMessage = {
            Snackbar.make(binding.root, it.content, Snackbar.LENGTH_SHORT).show()
        }
        requestRefresh()

    }

    private fun requestRefresh() {
        GetCurrentRequestInfo().execute<CurrentRequestResult> {
            when (it) {
                is RemoteResponse.Success -> {
                    travel = it.body.request
                    val format: NumberFormat = NumberFormat.getCurrencyInstance()
                    format.currency = Currency.getInstance(it.body.request.currency)
                    binding.costText.text = format.format((it.body.request.costBest ?: 0.0) - (it.body.request.providerShare ?: 0.0))
                    refreshPage()
                }

                is RemoteResponse.Error -> {
                    finish()
                }
            }
        }
    }

    override fun onPause() {
        locationManager!!.removeUpdates(this)
        super.onPause()
    }

    override fun onMapReady(googleMap: GoogleMap) {
        gMap = googleMap
        gMap!!.isTrafficEnabled = true
    }

    override fun onLocationChanged(location: Location) {
        val latLng = LatLng(location.latitude, location.longitude)
        LocationUpdate(preferences.token!!, latLng, true).execute<EmptyClass> {
            when (it) {
                is RemoteResponse.Success -> {

                }

                is RemoteResponse.Error -> {

                }
            }

        }
        geoLog.add(LatLng(location.latitude, location.longitude))
        currentLocation = LatLng(location.latitude, location.longitude)
        refreshPage()
        val request = travel
        val destination: LatLng = if (request!!.startTimestamp == null || request.points.count() < 2) request.points[0] else request.points[1]
        val distance = distFrom(latLng, destination)
        binding.distanceText.text = DistanceFormatter.format(distance)
    }

    override fun onProviderDisabled(provider: String) {}
    override fun onProviderEnabled(provider: String) {}
    override fun onStatusChanged(provider: String, status: Int, extras: Bundle) {}
    private fun startTravel() {
        LoadingDialog.display(this)
        Start().execute<Request> {
            LoadingDialog.hide()
            when (it) {
                is RemoteResponse.Success -> {
                    travel = it.body
                    refreshPage()
                }

                is RemoteResponse.Error -> {
                    it.error.showAlert(this)
                }
            }
        }
    }

    private fun refreshPage() {
        val request = travel
        if (currentLocation != null) {
            if (driverMarker == null) {
                driverMarker = gMap!!.addMarker(MarkerOptions().position(currentLocation!!).icon(BitmapDescriptorFactory.fromResource(R.drawable.marker_taxi)))
            } else {
                driverMarker!!.position = currentLocation!!
            }
        }
        when (request!!.status) {
            Request.Status.DriverAccepted -> {
                pointMarkers.forEach { it.remove() }
                val marker = gMap!!.addMarker(MarkerOptions()
                        .position(request.points[0])
                        .icon(BitmapDescriptorFactory.fromResource(R.drawable.marker_pickup)))
                pointMarkers.add(marker)
                if (driverMarker == null) gMap!!.animateCamera(CameraUpdateFactory.newLatLngZoom(request.points[0], 16f)) else {
                    val locations: MutableList<LatLng> = ArrayList()
                    locations.add(request.points[0])
                    locations.add(driverMarker!!.position)
                    centerLatLngsInMap(gMap!!, locations, true)
                }
            }
            Request.Status.DriverCanceled, Request.Status.RiderCanceled -> show(this@TravelActivity, getString(R.string.service_canceled), AlertDialogBuilder.DialogButton.OK, AlertDialogEvent { finish() })
            Request.Status.Started -> {
                pointMarkers.forEach { it.remove() }
                request.points.indices.drop(1).forEach {
                    pointMarkers.add(
                            gMap!!.addMarker(MarkerOptions()
                                    .position(request.points[it])
                                    .icon(BitmapDescriptorFactory.fromResource(R.drawable.marker_destination))
                            )
                    )
                }
                val positions = ArrayList<LatLng>()
                for (marker in pointMarkers)
                    positions.add((marker.position))
                if (driverMarker != null)
                    positions.add(driverMarker!!.position)
                when (positions.count()) {
                    0 -> { }
                    1 -> {
                        gMap!!.animateCamera(CameraUpdateFactory.newLatLngZoom(positions[0], 16f))
                    }
                    else -> {
                        centerLatLngsInMap(gMap!!, positions, true)
                    }
                }
                TransitionManager.beginDelayedTransition((binding.root as ViewGroup))
                binding.slideStart.visibility = View.GONE
                binding.slideArrived.visibility = View.GONE
                binding.slideCancel.visibility = View.GONE
                binding.slideFinish.visibility = View.VISIBLE
                binding.contactPanel.visibility = View.GONE
                binding.chatButton.visibility = View.GONE
            }
            Request.Status.WaitingForPostPay -> {
                val intent = Intent(this, WaitingForPaymentActivity::class.java)
                startActivity(intent)
            }
            Request.Status.Finished, Request.Status.WaitingForReview, null -> {
                finish()
            }
            Request.Status.Requested, Request.Status.NotFound, Request.Status.NoCloseFound, Request.Status.Found, Request.Status.Expired -> {
                AlerterHelper.showError(this, "An unknown Trip status: ${request.status!!.name}")
            }
            Request.Status.Arrived -> {
                binding.slideStart.visibility = View.VISIBLE
                binding.slideCancel.visibility = View.VISIBLE
                binding.slideFinish.visibility = View.GONE
                binding.slideArrived.visibility = View.GONE
            }
            Request.Status.WaitingForPrePay -> TODO()
            Request.Status.Booked -> TODO()
        }
    }

    private fun finishTravel() {
        var encodedPoly = ""
        if (geoLog.size > 0) encodedPoly = PolyUtil.encode(PolyUtil.simplify(geoLog, 10.0))
        if (travel!!.confirmationCode == null) {
            callFinish(travel!!.distanceReal!!.toInt(), encodedPoly)
        } else {
            showConfirmationDialog(encodedPoly)
        }
    }

    private fun showConfirmationDialog(path: String) {
        val builder = MaterialAlertDialogBuilder(this)
                .setTitle(getString(R.string.delivery_verify_code_title))
                .setMessage(getString(R.string.delivery_verify_code_message))
                .setView(R.layout.dialog_input)
                .setPositiveButton(R.string.alert_ok) { dialog: DialogInterface, _: Int ->
                    val dlg = dialog as AlertDialog
                    val txt = dlg.findViewById<TextInputEditText>(R.id.text1)
                    callFinish(travel!!.distanceReal!!.toInt(), path, txt!!.text.toString().toInt())

                }
        builder.show()
    }

    private fun callFinish(distanceReal: Int, path: String, confirmationCode: Int? = null) {
        LoadingDialog.display(this)
        Finish(confirmationCode, distanceReal, path).execute<FinishResult> {
            LoadingDialog.hide()
            when (it) {
                is RemoteResponse.Success -> {
                    runOnUiThread {
                        val req = travel!!
                        req.status = if (it.body.status) Request.Status.Finished else Request.Status.WaitingForPostPay
                        travel = req
                        refreshPage()
                    }
                }

                is RemoteResponse.Error -> {
                    it.error.showAlert(this)
                }
            }
        }
    }

    private fun callRider() {
        if (ContextCompat.checkSelfPermission(this, Manifest.permission.CALL_PHONE) == PackageManager.PERMISSION_GRANTED) {
            val intent = Intent(Intent.ACTION_CALL)
            intent.data = Uri.parse("tel:+" + travel!!.rider!!.mobileNumber)
            intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK
            startActivity(intent)
        } else {
            ActivityCompat.requestPermissions(this, arrayOf(Manifest.permission.CALL_PHONE), permissionPhoneCallRequestCode)
        }
    }

    override fun onRequestPermissionsResult(requestCode: Int, permissions: Array<out String>, grantResults: IntArray) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        if (requestCode == permissionPhoneCallRequestCode) {
            if (!grantResults.contains(-1)) {
                callRider()
            }
        }
    }

    private fun onChatButtonClicked() {
        val intent = Intent(this@TravelActivity, ChatActivity::class.java)
        intent.putExtra("app", "driver")
        startActivity(intent)
    }
}
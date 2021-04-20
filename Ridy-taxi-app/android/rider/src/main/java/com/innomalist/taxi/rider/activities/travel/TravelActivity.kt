package com.innomalist.taxi.rider.activities.travel

import android.Manifest.permission.CALL_PHONE
import android.annotation.SuppressLint
import android.app.Activity
import android.content.Intent
import android.content.pm.PackageManager
import android.net.Uri
import android.os.Bundle
import android.view.View
import android.view.ViewGroup
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.databinding.DataBindingUtil
import androidx.transition.TransitionManager
import com.google.android.gms.maps.GoogleMap
import com.google.android.gms.maps.OnMapReadyCallback
import com.google.android.gms.maps.SupportMapFragment
import com.google.android.gms.maps.model.BitmapDescriptorFactory
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.Marker
import com.google.android.gms.maps.model.MarkerOptions
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import com.google.android.material.snackbar.Snackbar
import com.innomalist.taxi.common.activities.chargeAccount.ChargeAccountActivity
import com.innomalist.taxi.common.activities.chat.ChatActivity
import com.innomalist.taxi.common.interfaces.AlertDialogEvent
import com.innomalist.taxi.common.location.MapHelper.centerLatLngsInMap
import com.innomalist.taxi.common.models.Coupon
import com.innomalist.taxi.common.models.Request
import com.innomalist.taxi.common.models.Review
import com.innomalist.taxi.common.models.Service
import com.innomalist.taxi.common.networking.socket.Cancel
import com.innomalist.taxi.common.networking.socket.CurrentRequestResult
import com.innomalist.taxi.common.networking.socket.GetCurrentRequestInfo
import com.innomalist.taxi.common.networking.socket.interfaces.EmptyClass
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import com.innomalist.taxi.common.networking.socket.interfaces.SocketNetworkDispatcher
import com.innomalist.taxi.common.utils.AlertDialogBuilder
import com.innomalist.taxi.common.utils.AlertDialogBuilder.show
import com.innomalist.taxi.common.utils.AlerterHelper.showInfo
import com.innomalist.taxi.common.utils.LoadingDialog
import com.innomalist.taxi.rider.R
import com.innomalist.taxi.rider.activities.coupon.CouponActivity
import com.innomalist.taxi.rider.activities.travel.adapters.TravelTabsViewPagerAdapter
import com.innomalist.taxi.rider.activities.travel.fragments.ReviewDialog
import com.innomalist.taxi.rider.activities.travel.fragments.ReviewDialog.OnReviewFragmentInteractionListener
import com.innomalist.taxi.rider.databinding.ActivityTravelBinding
import com.innomalist.taxi.rider.networking.socket.EnableVerification
import com.innomalist.taxi.rider.networking.socket.ReviewDriver
import com.innomalist.taxi.rider.ui.RiderBaseActivity
import java.text.NumberFormat
import java.util.*
import kotlin.collections.ArrayList

class TravelActivity : RiderBaseActivity(), OnMapReadyCallback, OnReviewFragmentInteractionListener {
    lateinit var binding: ActivityTravelBinding
    private var pointMarkers: MutableList<Marker> = ArrayList()
    private var driverMarker: Marker? = null
    private var driverLocation: LatLng? = null
    private var gMap: GoogleMap? = null
    private var travelTabsViewPagerAdapter: TravelTabsViewPagerAdapter? = null
    private val permissionPhoneCallRequestCode = 400

    @SuppressLint("CheckResult")
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = DataBindingUtil.setContentView(this, R.layout.activity_travel)
        (supportFragmentManager.findFragmentById(R.id.map) as SupportMapFragment).getMapAsync(this)
        binding.slideCancel.setOnSlideCompleteListener {
            LoadingDialog.display(this)
            Cancel().execute<EmptyClass> {
                LoadingDialog.hide()
                when (it) {
                    is RemoteResponse.Success -> {
                        updateTravelStatus(Request.Status.RiderCanceled)
                        refreshPage()
                    }

                    is RemoteResponse.Error -> {
                        it.error.showAlert(this)
                    }
                }
            }
        }
        binding.slideCall.setOnSlideCompleteListener { callDriver() }
        binding.enableVerificationButton.setOnClickListener { onEnableVerification() }
        binding.chatButton.setOnClickListener {
            val intent = Intent(this@TravelActivity, ChatActivity::class.java)
            intent.putExtra("app", "rider")
            startActivity(intent)
        }
        SocketNetworkDispatcher.instance.onStarted = {
            travel = it
            showInfo(this@TravelActivity, getString(R.string.message_travel_started))
            refreshPage()
        }
        SocketNetworkDispatcher.instance.onArrived = {
            val req = travel!!
            req.status = Request.Status.Arrived
            travel = req
            refreshPage()
        }
        SocketNetworkDispatcher.instance.onTravelInfo = {
            this.driverLocation = it
            refreshPage()
        }
        SocketNetworkDispatcher.instance.onCancel = {
            updateTravelStatus(Request.Status.DriverCanceled)
            refreshPage()
        }
        travelTabsViewPagerAdapter = TravelTabsViewPagerAdapter(supportFragmentManager, this@TravelActivity, travel!!)
        binding.viewpager.adapter = travelTabsViewPagerAdapter
        binding.tabLayout.setupWithViewPager(binding.viewpager)
        val request = travel
        if (request!!.rating != null) {
            travelTabsViewPagerAdapter!!.deletePage(2)
            val tab = binding.tabLayout.getTabAt(0)
            tab?.select()
        }
    }

    override fun onResume() {
        super.onResume()
        if (gMap != null) {
            requestRefresh()
        }
        SocketNetworkDispatcher.instance.onNewMessage = {
            Snackbar.make(binding.root, it.content, Snackbar.LENGTH_SHORT).setAction("View") {
                val intent = Intent(this@TravelActivity, ChatActivity::class.java)
                intent.putExtra("app", "rider")
                startActivity(intent)
            }.show()
        }
        SocketNetworkDispatcher.instance.onFinished = {
            travel!!.costAfterCoupon = it.remainingAmount
            val req = travel!!
            req.status = if (it.paid) (if (travelTabsViewPagerAdapter!!.count == 2) Request.Status.Finished else Request.Status.WaitingForReview) else Request.Status.WaitingForPostPay
            travel = req
            refreshPage()
        }
    }

    override fun onReconnected() {
        super.onReconnected()
        requestRefresh()
    }

    private fun requestRefresh() {
        GetCurrentRequestInfo().execute<CurrentRequestResult> {
            when (it) {
                is RemoteResponse.Success -> {
                    travel = it.body.request
                    driverLocation = it.body.driverLocation
                    refreshPage()
                }

                is RemoteResponse.Error -> {
                    finish()
                }
            }
        }
    }

    private fun refreshPage() {
        val request = travel
        if (request!!.service != null && request.service!!.canEnableVerificationCode) {
            binding.enableVerificationButton.visibility = View.VISIBLE
        } else {
            binding.enableVerificationButton.visibility = View.GONE
        }
        when (request.status) {
            Request.Status.DriverAccepted, Request.Status.Started -> {
                pointMarkers.forEach { it.remove() }
                pointMarkers = ArrayList()
                for (id in request.points.indices) {
                    val marker = gMap!!.addMarker(MarkerOptions()
                            .position(request.points[id])
                            .icon(BitmapDescriptorFactory.fromResource(if(id == 0) R.drawable.marker_pickup else R.drawable.marker_destination)))
                    pointMarkers.add(marker)
                }
                var points = request.points
                if (driverLocation != null) {
                    points = points.plus(driverLocation!!)
                    if (driverMarker == null) {
                        driverMarker = gMap!!.addMarker(MarkerOptions().position(driverLocation!!).icon(BitmapDescriptorFactory.fromResource(R.drawable.marker_taxi)))
                    } else {
                        driverMarker!!.position = driverLocation
                    }
                }
                centerLatLngsInMap(gMap!!, points, true)
                if (request.status == Request.Status.Started) {
                    TransitionManager.beginDelayedTransition((binding.root as ViewGroup))
                    binding.slideCall.visibility = View.GONE
                    binding.chatButton.visibility = View.GONE
                    binding.slideCancel.visibility = View.GONE
                }
            }
            Request.Status.Arrived -> {
                showInfo(this, getString(R.string.message_driver_arrived))
            }

            Request.Status.DriverCanceled, Request.Status.RiderCanceled -> show(this@TravelActivity, getString(R.string.service_canceled), AlertDialogBuilder.DialogButton.OK, AlertDialogEvent { finish() })
            Request.Status.WaitingForPostPay -> {
                if (request.service?.paymentMethod != Service.PaymentMethod.OnlyCash) {
                    val intent = Intent(this, ChargeAccountActivity::class.java)
                    intent.putExtra("defaultAmount", travel!!.costAfterCoupon)
                    intent.putExtra("currency", travel!!.currency)
                    startActivity(intent)
                } else {
                    MaterialAlertDialogBuilder(this)
                            .setTitle(R.string.message_rider_finished_title)
                            .setMessage(R.string.message_finished_rider_content_pay_in_cash)
                            .setPositiveButton(R.string.notification_finished_action) { _, _ ->
                                requestRefresh()
                            }
                            .show()
                }
            }
            Request.Status.WaitingForReview -> {
                if (supportFragmentManager.findFragmentByTag("fragment_review_travel") == null) {
                    val reviewDialog: ReviewDialog = ReviewDialog.newInstance()
                    reviewDialog.isCancelable = false
                    reviewDialog.show(supportFragmentManager, "fragment_review_travel")
                }
            }
            Request.Status.Finished -> {
                finish()
            }

            else -> {
                show(this, "Unknown event found: ${getString(request.status!!.localizedDescription)}", AlertDialogBuilder.DialogButton.OK, null)
            }
        }
    }

    private fun updateTravelStatus(status: Request.Status) {
        val request = travel
        request!!.status = status
        travel = request
    }

    fun onChargeAccountClicked() {
        val intent = Intent(this@TravelActivity, ChargeAccountActivity::class.java)
        intent.putExtra("defaultAmount", travel!!.costAfterCoupon)
        intent.putExtra("currency", travel!!.currency)
        startActivity(intent)
    }

    fun onApplyCouponClicked() {
        val intent = Intent(this@TravelActivity, CouponActivity::class.java)
        intent.putExtra("select_mode", true)
        startActivityForResult(intent, ACTIVITY_COUPON)
    }

    override fun onBackPressed() {}

    override fun onMapReady(googleMap: GoogleMap) {
        gMap = googleMap
        gMap!!.isTrafficEnabled = true
        gMap!!.setMaxZoomPreference(17f)
        refreshPage()
    }

    override fun onReviewTravelClicked(review: Review) {
        LoadingDialog.display(this)
        ReviewDriver(review).execute<EmptyClass> {
            LoadingDialog.hide()
            when (it) {
                is RemoteResponse.Success -> {
                    if (travel!!.status == Request.Status.WaitingForReview) {
                        updateTravelStatus(Request.Status.Finished)
                        refreshPage()
                        return@execute
                    }
                    showInfo(this@TravelActivity, getString(R.string.message_review_sent))
                    travelTabsViewPagerAdapter!!.deletePage(2)
                    val tab = binding.tabLayout.getTabAt(0)
                    tab?.select()
                }

                is RemoteResponse.Error -> {
                    it.error.showAlert(this)
                }
            }

        }
    }

    private fun callDriver() {
        if (ContextCompat.checkSelfPermission(this, CALL_PHONE) == PackageManager.PERMISSION_GRANTED) {
            val intent = Intent(Intent.ACTION_CALL)
            intent.data = Uri.parse("tel:+" + travel!!.driver!!.mobileNumber)
            intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK
            startActivity(intent)
        } else {
            ActivityCompat.requestPermissions(this, arrayOf(CALL_PHONE), permissionPhoneCallRequestCode)
        }
    }


    override fun onRequestPermissionsResult(requestCode: Int, permissions: Array<out String>, grantResults: IntArray) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        if (requestCode == permissionPhoneCallRequestCode) {
            if (!grantResults.contains(-1)) {
                callDriver()
            }
        }
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (requestCode == ACTIVITY_COUPON) {
            if (resultCode == Activity.RESULT_OK) {
                val coupon = data!!.getSerializableExtra("coupon") as Coupon
                var message = ""
                val format: NumberFormat = NumberFormat.getCurrencyInstance()
                format.currency = Currency.getInstance(travel!!.currency)
                travelTabsViewPagerAdapter!!.statisticsFragment!!.onUpdatePrice(data.getDoubleExtra("costAfterCoupon", travel!!.costAfterCoupon!!))
                if (coupon.flatDiscount == 0.0 && coupon.discountPercent != 0) message = getString(R.string.message_coupon_applied, "${coupon.discountPercent}%")
                if (coupon.flatDiscount != 0.0 && coupon.discountPercent == 0) message = getString(R.string.message_coupon_applied, format.format(coupon.flatDiscount))
                if (coupon.flatDiscount != 0.0 && coupon.discountPercent != 0) message = getString(R.string.message_coupon_applied, "${format.format(coupon.flatDiscount)} & ${coupon.discountPercent}%")
                if (message == "") return
                showInfo(this@TravelActivity, message)
            }
        }
    }

    private fun onEnableVerification() {
        EnableVerification().execute<Int> {
            when (it) {
                is RemoteResponse.Success -> {
                    show(this@TravelActivity, getString(R.string.confirmation_code_message, it.body), AlertDialogBuilder.DialogButton.OK, null)
                }

                is RemoteResponse.Error -> {
                    it.error.showAlert(this)
                }
            }

        }
    }

    companion object {
        private const val ACTIVITY_COUPON = 700
    }
}
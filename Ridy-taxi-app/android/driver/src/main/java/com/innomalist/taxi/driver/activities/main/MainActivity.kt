package com.innomalist.taxi.driver.activities.main

import android.Manifest
import android.annotation.SuppressLint
import android.app.Activity
import android.app.NotificationChannel
import android.app.NotificationManager
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
import android.location.Location
import android.location.LocationListener
import android.location.LocationManager
import android.media.AudioAttributes
import android.net.Uri
import android.os.Build
import android.os.Bundle
import android.provider.Settings
import android.util.Log
import android.util.TypedValue
import android.view.MenuItem
import android.view.View
import android.view.WindowManager
import android.widget.ImageView
import android.widget.TextView
import android.widget.Toast
import androidx.core.app.ActivityCompat
import androidx.core.view.GravityCompat
import androidx.databinding.DataBindingUtil
import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.GoogleMap
import com.google.android.gms.maps.OnMapReadyCallback
import com.google.android.gms.maps.SupportMapFragment
import com.google.android.gms.maps.model.*
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import com.innomalist.taxi.common.activities.chargeAccount.ChargeAccountActivity
import com.innomalist.taxi.common.activities.transactions.TransactionsActivity
import com.innomalist.taxi.common.activities.travels.TravelsActivity
import com.innomalist.taxi.common.models.Request
import com.innomalist.taxi.common.networking.socket.CurrentRequestResult
import com.innomalist.taxi.common.networking.socket.GetCurrentRequestInfo
import com.innomalist.taxi.common.networking.socket.interfaces.EmptyClass
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import com.innomalist.taxi.common.networking.socket.interfaces.SocketNetworkDispatcher
import com.innomalist.taxi.common.utils.AlertDialogBuilder
import com.innomalist.taxi.common.utils.AlerterHelper.showInfo
import com.innomalist.taxi.common.utils.CommonUtils
import com.innomalist.taxi.common.utils.DataBinder.setMedia
import com.innomalist.taxi.common.utils.LoadingDialog
import com.innomalist.taxi.driver.R
import com.innomalist.taxi.driver.activities.about.AboutActivity
import com.innomalist.taxi.driver.activities.main.adapters.RequestsFragmentPagerAdapter
import com.innomalist.taxi.driver.activities.main.fragments.RequestFragment.OnFragmentInteractionListener
import com.innomalist.taxi.driver.activities.statistics.StatisticsActivity
import com.innomalist.taxi.driver.activities.travel.TravelActivity
import com.innomalist.taxi.driver.databinding.ActivityMainBinding
import com.innomalist.taxi.driver.networking.http.LocationUpdate
import com.innomalist.taxi.driver.networking.socket.AcceptOrder
import com.innomalist.taxi.driver.networking.socket.GetAvailableRequests
import com.innomalist.taxi.driver.networking.socket.UpdateStatus
import com.innomalist.taxi.driver.ui.DriverBaseActivity
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch


enum class LocationState {
    OK,
    LocationDisabled,
    PermissionNotAsked,
    PermissionDenied
}

class MainActivity : DriverBaseActivity(), OnMapReadyCallback, LocationListener, OnFragmentInteractionListener {
    private var mMap: GoogleMap? = null
    private var markersLocations: List<Marker> = listOf()
    private lateinit var binding: ActivityMainBinding
    private lateinit var requestCardsAdapter: RequestsFragmentPagerAdapter
    private var mapFragment: SupportMapFragment? = null
    private val requestLocationCode = 432

    @SuppressLint("CheckResult")
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = DataBindingUtil.setContentView(this@MainActivity, R.layout.activity_main)
        createNotificationChannels()
        mapFragment = supportFragmentManager.findFragmentById(R.id.map) as SupportMapFragment
        requestCardsAdapter = RequestsFragmentPagerAdapter(supportFragmentManager, ArrayList())
        binding.requestsViewPager.adapter = requestCardsAdapter
        binding.requestsViewPager.offscreenPageLimit = 3
        window.addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON)
        binding.buttonEnableLocation.setOnClickListener { startActivity(Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS)) }
        binding.buttonEnablePermission.setOnClickListener {
            val permissions = arrayOf(Manifest.permission.ACCESS_COARSE_LOCATION, Manifest.permission.ACCESS_FINE_LOCATION)
            /*if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q) {
                permissions = permissions.plus(Manifest.permission.ACCESS_BACKGROUND_LOCATION)
            }*/
            MaterialAlertDialogBuilder(this@MainActivity)
                    .setTitle(R.string.message_gps_title)
                    .setMessage(getString(R.string.message_driver_location_permission))
                    .setPositiveButton(R.string.alert_ok) { _, _ ->
                        ActivityCompat.requestPermissions(this@MainActivity, permissions, requestLocationCode)
                    }
                    .setNegativeButton(R.string.alert_cancel) { _, _ ->

                    }
                    .show()
            return@setOnClickListener
        }
        binding.buttonOpenLocationSettings.setOnClickListener { startActivity(Intent(Settings.ACTION_APPLICATION_DETAILS_SETTINGS, Uri.parse("package:$packageName"))) }
        setSupportActionBar(binding.appbar)
        val actionBar = supportActionBar
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            val value = TypedValue()
            theme.resolveAttribute(R.attr.colorPrimary, value, true)
            window.navigationBarColor = value.data
        }
        if (actionBar != null) {
            actionBar.setHomeAsUpIndicator(R.drawable.menu)
            actionBar.setDisplayHomeAsUpEnabled(true)
        }
        binding.navigationView.setNavigationItemSelectedListener { menuItem: MenuItem ->
            binding.drawerLayout.closeDrawers()
            when (menuItem.itemId) {
                R.id.nav_item_travels -> startActivity(Intent(this@MainActivity, TravelsActivity::class.java))
                R.id.nav_item_statistics -> startActivity(Intent(this@MainActivity, StatisticsActivity::class.java))
                R.id.nav_item_charge_account -> startActivityForResult(Intent(this@MainActivity, ChargeAccountActivity::class.java), ACTIVITY_WALLET)
                R.id.nav_item_transactions -> startActivity(Intent(this@MainActivity, TransactionsActivity::class.java))
                R.id.nav_item_about -> startActivity(Intent(this@MainActivity, AboutActivity::class.java))
                R.id.nav_item_exit -> logout()
                else -> Toast.makeText(this@MainActivity, menuItem.title, Toast.LENGTH_SHORT).show()
            }
            true
        }
        fillInfo()
        checkPermissions()
        SocketNetworkDispatcher.instance.onCancelRequest = {
            val position = requestCardsAdapter.getPositionWithTravelId(it.toLong())
            if (position >= 0) requestCardsAdapter.remove(position)
        }
        SocketNetworkDispatcher.instance.onNewRequest = {
            requestCardsAdapter.add(it)
            requestCardsAdapter.notifyDataSetChanged()
        }
        GetCurrentRequestInfo().execute<CurrentRequestResult> {
            when (it) {
                is RemoteResponse.Success -> {
                    if (it.body.request.status != Request.Status.WaitingForReview) {
                        val intent = Intent(this@MainActivity, TravelActivity::class.java)
                        travel = it.body.request
                        startActivityForResult(intent, ACTIVITY_TRAVEL)
                    }
                }

                is RemoteResponse.Error -> {
                }
            }
        }
        binding.switchConnection.setOnClickListener { switchClicked() }
    }

    @SuppressLint("MissingPermission")
    override fun onMapReady(googleMap: GoogleMap) {
        mMap = googleMap
        mMap!!.uiSettings.isMapToolbarEnabled = false
        mMap!!.isMyLocationEnabled = true
        mMap!!.uiSettings.isMyLocationButtonEnabled = true
        val locationManager = (this.getSystemService(Context.LOCATION_SERVICE) as LocationManager)
        locationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 5000, 5f, this)
        getLastKnownLocation()
        if (resources.getBoolean(R.bool.isNightMode)) {
            val success = mMap!!.setMapStyle(
                    MapStyleOptions.loadRawResourceStyle(
                            this, R.raw.map_night))
            if (!success) Log.e("MapsActivityRaw", "Style parsing failed.")
        }
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        when (item.itemId) {
            android.R.id.home -> binding.drawerLayout.openDrawer(GravityCompat.START)
        }
        return super.onOptionsItemSelected(item)
    }

    override fun onRequestPermissionsResult(requestCode: Int, permissions: Array<out String>, grantResults: IntArray) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        checkPermissions()
    }

    private fun requestRefresh() {
        GetAvailableRequests().executeArray<Request> {
            when (it) {
                is RemoteResponse.Success -> {
                    binding.switchConnection.isChecked = true
                    requestCardsAdapter = RequestsFragmentPagerAdapter(supportFragmentManager, it.body)
                    binding.requestsViewPager.adapter = requestCardsAdapter
                    binding.requestsViewPager.offscreenPageLimit = 3
                }

                is RemoteResponse.Error -> {
                    binding.switchConnection.isChecked = false
                }
            }
        }
    }

    private fun checkPermissions() {
        if (!CommonUtils.isGPSEnabled(this@MainActivity)) {
            binding.locationState = LocationState.LocationDisabled
            return
        }
        if (ActivityCompat.checkSelfPermission(this@MainActivity, Manifest.permission.ACCESS_COARSE_LOCATION) == PackageManager.PERMISSION_GRANTED && CommonUtils.isGPSEnabled(this@MainActivity)) {
            binding.locationState = LocationState.OK
            runOnUiThread {
                mapFragment!!.getMapAsync(this)
            }
            requestRefresh()
            return
        }
        binding.locationState = LocationState.PermissionNotAsked
    }

    override fun onReconnected() {
        super.onReconnected()
        checkPermissions()
    }

    private fun switchClicked() {
        if (binding.switchConnection.isChecked && CommonUtils.currentLocation == null && mMap!!.myLocation == null) {
            AlertDialogBuilder.show(this@MainActivity, "Your exact current location is yet to be determined. Please try again after a few seconds.", AlertDialogBuilder.DialogButton.OK, null)
            binding.switchConnection.isChecked = false
            return
        }
        binding.switchConnection.isEnabled = false
        GlobalScope.launch(Dispatchers.Main) {
            UpdateStatus(binding.switchConnection.isChecked).execute<EmptyClass> {
                binding.switchConnection.isEnabled = true
                when (it) {
                    is RemoteResponse.Success -> {
                        if (binding.switchConnection.isChecked)
                            LocationUpdate(preferences.token!!, if (CommonUtils.currentLocation != null) CommonUtils.currentLocation!! else LatLng(mMap!!.myLocation.latitude, mMap!!.myLocation.longitude), false).execute<EmptyClass> {
                                requestRefresh()
                            }
                    }

                    is RemoteResponse.Error -> {
                        binding.switchConnection.isChecked = !binding.switchConnection.isChecked
                    }
                }
            }
        }
    }

    private fun getLastKnownLocation() {
        if (ActivityCompat.checkSelfPermission(this@MainActivity, Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(this@MainActivity, Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            return
        }
        val manager = applicationContext.getSystemService(Context.LOCATION_SERVICE) as LocationManager
        val providers: List<String>
        providers = manager.getProviders(true)
        var bestLocation: Location? = null
        for (provider in providers) {
            val l = manager.getLastKnownLocation(provider) ?: continue
            if (bestLocation == null || l.accuracy < bestLocation.accuracy) {
                bestLocation = l
            }
        }
        if(bestLocation == null)
            return
        val latLng = LatLng(bestLocation.latitude, bestLocation.longitude)
        if (binding.switchConnection.isChecked) {
            LocationUpdate(preferences.token!!, latLng, false).execute<EmptyClass> {

            }
        }
        mMap!!.animateCamera(CameraUpdateFactory.newLatLngZoom(latLng, 16f))
    }

    private fun fillInfo() {
        try {
            val name: String
            val driver = preferences.driver!!
            name = if ((driver.firstName == null || driver.firstName!!.isEmpty()) && (driver.lastName == null || driver.lastName!!.isEmpty())) driver.mobileNumber.toString() else driver.firstName + " " + driver.lastName
            val header = binding.navigationView.getHeaderView(0)
            (header.findViewById<View>(R.id.navigation_header_name) as TextView).text = name
            (header.findViewById<View>(R.id.navigation_header_charge) as TextView).text = driver.mobileNumber.toString()
            val imageView = header.findViewById<ImageView>(R.id.navigation_header_image)
            setMedia(imageView, driver.media)
        } catch (ignored: Exception) {
        }
    }

    private fun logout() {
        preferences.clearPreferences()
        finish()
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        when (requestCode) {
            ACTIVITY_PROFILE -> {
                if (resultCode == Activity.RESULT_OK) showInfo(this@MainActivity, getString(R.string.info_edit_profile_success))
                fillInfo()
            }
            ACTIVITY_WALLET -> {
                if (resultCode == Activity.RESULT_OK) showInfo(this@MainActivity, getString(R.string.account_charge_success))
                fillInfo()
            }
            ACTIVITY_TRAVEL -> {
            }
        }
    }

    override fun onLocationChanged(location: Location) {
        CommonUtils.currentLocation = LatLng(location.latitude, location.longitude)
        if (binding.switchConnection.isChecked) {
            LocationUpdate(preferences.token!!, CommonUtils.currentLocation!!, false).execute<EmptyClass> {
                when (it) {
                    is RemoteResponse.Success -> {
                        val cameraUpdate = CameraUpdateFactory.newLatLngZoom(CommonUtils.currentLocation, if (mMap!!.cameraPosition.zoom > 5) mMap!!.cameraPosition.zoom else 16f)
                        mMap!!.animateCamera(cameraUpdate)
                    }
                    is RemoteResponse.Error -> {

                    }
                }
            }
        }
    }

    override fun onStatusChanged(s: String, i: Int, bundle: Bundle) {}
    override fun onProviderEnabled(s: String) {}
    override fun onProviderDisabled(s: String) {}
    override fun onAccept(request: Request) {
        LoadingDialog.display(this)
        AcceptOrder(request.id!!.toLong()).execute<Request> {
            LoadingDialog.hide()
            when (it) {
                is RemoteResponse.Success -> {
                    val intentTravel = Intent(this@MainActivity, TravelActivity::class.java)
                    travel = it.body
                    startActivityForResult(intentTravel, ACTIVITY_TRAVEL)
                }

                is RemoteResponse.Error -> {
                    it.error.showAlert(this@MainActivity)
                }
            }
        }
        removeMarkers()
        while (requestCardsAdapter.count > 0) requestCardsAdapter.remove(0)
    }


    override fun onDecline(request: Request) {
        val position = requestCardsAdapter.getPositionWithTravelId(request.id!!)
        if (position >= 0) requestCardsAdapter.remove(position)
        requestCardsAdapter.notifyDataSetChanged()
    }

    override fun onVisible(request: Request) {
        for(point in request.points) {
            markersLocations = markersLocations.plus(mMap!!.addMarker(MarkerOptions()
                    .position(point)
                    .icon(BitmapDescriptorFactory.fromResource(R.drawable.marker_pickup))))
        }
        mMap!!.setPadding(0, 0, 0, 850)
        val builder = LatLngBounds.Builder()
        for (location in request.points) builder.include(location)
        val bounds = builder.build()
        val cu = CameraUpdateFactory.newLatLngBounds(bounds, 150)
        mMap!!.animateCamera(cu)
        //((RequestFragment)requestCardsAdapter.getFragment(travel,requestCardsAdapter.getPositionWithTravelId(travel.getId()))).locationChanged(markerDriver.getPosition());
    }

    private fun createNotificationChannels() {
        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.O) {
            return
        }
        val sound = Uri.parse("android.resource://" + applicationContext.packageName + "/" + R.raw.notification)

        val notificationManager = applicationContext.getSystemService(NOTIFICATION_SERVICE) as NotificationManager

        NotificationChannel("request", "Requests", NotificationManager.IMPORTANCE_HIGH).let {
            it.enableLights(true)
            it.description = "New trip requests notification"
            val audioAttributes = AudioAttributes.Builder()
                    .setContentType(AudioAttributes.CONTENT_TYPE_SONIFICATION)
                    .setUsage(AudioAttributes.USAGE_NOTIFICATION)
                    .build()
            it.setSound(sound, audioAttributes)
            notificationManager.createNotificationChannel(it)
        }

        NotificationChannel("message", "Message", NotificationManager.IMPORTANCE_HIGH).let {
            it.enableLights(true)
            it.description = "In-App Chat messages"
            val audioAttributes = AudioAttributes.Builder()
                    .setContentType(AudioAttributes.CONTENT_TYPE_SONIFICATION)
                    .setUsage(AudioAttributes.USAGE_ALARM)
                    .build()
            it.setSound(sound, audioAttributes)
            notificationManager.createNotificationChannel(it)
        }

        NotificationChannel("paid", "Payment", NotificationManager.IMPORTANCE_HIGH).let {
            it.enableLights(true)
            it.description = "Service Payment notification"
            val audioAttributes = AudioAttributes.Builder()
                    .setContentType(AudioAttributes.CONTENT_TYPE_SONIFICATION)
                    .setUsage(AudioAttributes.USAGE_ALARM)
                    .build()
            it.setSound(sound, audioAttributes)
            notificationManager.createNotificationChannel(it)
        }
    }

    override fun onInvisible(request: Request) {
        removeMarkers()
    }

    private fun removeMarkers() {
        markersLocations.forEach {
            it.remove()
        }
        markersLocations.dropWhile { markersLocations.count() > 0 }
        mMap!!.setPadding(0, 0, 0, 0)
    }

    companion object {
        const val ACTIVITY_PROFILE = 11
        const val ACTIVITY_WALLET = 12
        const val ACTIVITY_TRAVEL = 14
    }
}
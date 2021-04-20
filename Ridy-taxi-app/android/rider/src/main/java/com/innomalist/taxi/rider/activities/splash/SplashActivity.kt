package com.innomalist.taxi.rider.activities.splash

import android.Manifest.permission.ACCESS_COARSE_LOCATION
import android.Manifest.permission.ACCESS_FINE_LOCATION
import android.annotation.SuppressLint
import android.app.Activity
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
import android.location.Location
import android.location.LocationListener
import android.location.LocationManager
import android.os.Bundle
import android.os.Handler
import android.view.View
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.databinding.DataBindingUtil
import com.firebase.ui.auth.AuthUI
import com.firebase.ui.auth.AuthUI.IdpConfig.PhoneBuilder
import com.google.android.gms.location.LocationServices
import com.google.android.gms.maps.model.LatLng
import com.google.android.libraries.places.api.Places
import com.google.firebase.auth.FirebaseAuth
import com.google.firebase.messaging.FirebaseMessaging
import com.innomalist.taxi.common.components.BaseActivity
import com.innomalist.taxi.common.interfaces.AlertDialogEvent
import com.innomalist.taxi.common.networking.socket.interfaces.Namespace
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import com.innomalist.taxi.common.networking.socket.interfaces.SocketNetworkDispatcher
import com.innomalist.taxi.common.utils.AlertDialogBuilder
import com.innomalist.taxi.common.utils.AlertDialogBuilder.DialogResult
import com.innomalist.taxi.common.utils.AlertDialogBuilder.show
import com.innomalist.taxi.common.utils.AlerterHelper.showError
import com.innomalist.taxi.common.utils.CommonUtils.isInternetDisabled
import com.innomalist.taxi.common.utils.LocationHelper.Companion.latLngToDoubleArray
import com.innomalist.taxi.rider.R
import com.innomalist.taxi.rider.activities.main.MainActivity
import com.innomalist.taxi.rider.databinding.ActivitySplashBinding
import com.innomalist.taxi.rider.networking.http.Login
import com.innomalist.taxi.rider.networking.http.LoginResult

class SplashActivity : BaseActivity(), LocationListener {
    lateinit var binding: ActivitySplashBinding
    private var signInRequestCode = 123
    private var locationTimeoutHandler: Handler? = null
    private var locationManager: LocationManager? = null
    var currentLocation: LatLng? = null
    private var isErrored = false
    private val permissionLocationRequestCode = 321
    private var goingToOpen = false

    override fun onCreate(savedInstanceState: Bundle?) {
        showConnectionDialog = false
        immersiveScreen = true
        super.onCreate(savedInstanceState)
        Places.initialize(applicationContext, getString(R.string.google_maps_key))
        Places.createClient(this)
        binding = DataBindingUtil.setContentView(this@SplashActivity, R.layout.activity_splash)
        binding.loginButton.setOnClickListener {
            startActivityForResult(
                    AuthUI.getInstance()
                            .createSignInIntentBuilder()
                            .setAvailableProviders(listOf(PhoneBuilder().build()))
                            .setTheme(currentTheme)
                            .build(),
                    signInRequestCode)
        }
    }

    private fun checkPermissions() {
        if (isInternetDisabled(this)) {
            show(this, getString(R.string.message_enable_wifi), AlertDialogBuilder.DialogButton.CANCEL_RETRY, AlertDialogEvent { result: DialogResult ->
                if (result == DialogResult.RETRY) {
                    checkPermissions()
                } else {
                    finishAffinity()
                }
            })
            return
        }
        if (ContextCompat.checkSelfPermission(this@SplashActivity, ACCESS_FINE_LOCATION) == PackageManager.PERMISSION_GRANTED) {
            requestLocation()
        } else {
            ActivityCompat.requestPermissions(this@SplashActivity, arrayOf(ACCESS_FINE_LOCATION, ACCESS_COARSE_LOCATION), permissionLocationRequestCode)
        }
    }

    @SuppressLint("MissingPermission")
    private fun requestLocation() {
        LocationServices.getFusedLocationProviderClient(this@SplashActivity).lastLocation.addOnSuccessListener(this@SplashActivity) { location: Location? ->
            tryConnect()
            if (location != null) {
                currentLocation = LatLng(location.latitude, location.longitude)
            }
        }.addOnFailureListener {
            tryConnect()
        }
    }

    override fun onRequestPermissionsResult(requestCode: Int, permissions: Array<out String>, grantResults: IntArray) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        if(requestCode == permissionLocationRequestCode && !grantResults.contains(-1)) {
            requestLocation()
        } else {
            tryConnect()
        }
    }

    @SuppressLint("MissingPermission")
    private fun searchCurrentLocation() {
        locationManager = this.getSystemService(Context.LOCATION_SERVICE) as LocationManager
        locationManager!!.requestLocationUpdates(LocationManager.GPS_PROVIDER, 100, 1f, this)
    }

    private fun startMainActivity(latLng: LatLng) {
        if (goingToOpen) return
        goingToOpen = true
        val intent = Intent(this@SplashActivity, MainActivity::class.java)
        val array = latLngToDoubleArray(latLng)
        intent.putExtra("currentLocation", array)
        //intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
        startActivity(intent)
    }

    private fun tryConnect() {
        val token = preferences.token
        if (token != null && token.isNotEmpty()) {
            FirebaseMessaging.getInstance().token.addOnCompleteListener { fb ->
                runOnUiThread {
                    SocketNetworkDispatcher.instance.connect(Namespace.Rider, token, fb.result
                            ?: "") {
                        when (it) {
                            is RemoteResponse.Success -> {
                                if (ActivityCompat.checkSelfPermission(this, ACCESS_FINE_LOCATION) == PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(this, ACCESS_COARSE_LOCATION) == PackageManager.PERMISSION_GRANTED) {
                                    runOnUiThread {
                                        locationTimeoutHandler = Handler()
                                        locationTimeoutHandler!!.postDelayed({
                                            locationManager!!.removeUpdates(this@SplashActivity)
                                            if (currentLocation == null) {
                                                val location = getString(R.string.default_location).split(",").toTypedArray()
                                                val lat = location[0].toDouble()
                                                val lng = location[1].toDouble()
                                                currentLocation = LatLng(lat, lng)
                                            }
                                            if (isErrored) return@postDelayed
                                            startMainActivity(currentLocation!!)
                                        }, 5000)
                                        searchCurrentLocation()
                                    }
                                } else {
                                    val location = getString(R.string.default_location).split(",").toTypedArray()
                                    startMainActivity(LatLng(location[0].toDouble(), location[1].toDouble()))
                                }
                            }

                            is RemoteResponse.Error -> {
                                runOnUiThread {
                                    isErrored = true
                                    goToLoginMode()
                                    showError(this, it.error.rawValue)
                                }

                            }
                        }
                    }

                }
            }
            goToLoadingMode()
        } else {
            goToLoginMode()
        }
    }


    override fun onResume() {
        super.onResume()
        checkPermissions()
    }

    private fun tryLogin(firebaseToken: String) {
        isErrored = false
        Login(firebaseToken).execute<LoginResult> {
            when (it) {
                is RemoteResponse.Success -> {
                    preferences.rider = it.body.user
                    preferences.token = it.body.token
                    tryConnect()
                }

                is RemoteResponse.Error -> {
                    showError(this, it.error.localizedDescription)
                }
            }

        }
    }

    private fun goToLoadingMode() {
        binding.loginButton.visibility = View.GONE
        binding.progressBar.visibility = View.VISIBLE
    }

    private fun goToLoginMode() {
        binding.loginButton.visibility = View.VISIBLE
        binding.progressBar.visibility = View.GONE
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (requestCode == signInRequestCode) {
            if (resultCode == Activity.RESULT_OK) {
                goToLoadingMode()
                FirebaseAuth.getInstance().currentUser!!.getIdToken(false).addOnCompleteListener {
                    tryLogin(it.result!!.token!!)
                }
                return
            }
        }
        showError(this@SplashActivity, getString(R.string.login_failed))
        goToLoginMode()
    }

    override fun onLocationChanged(location: Location) {
        currentLocation = LatLng(location.latitude, location.longitude)
    }

    override fun onStatusChanged(s: String, i: Int, bundle: Bundle) {}
    override fun onProviderEnabled(s: String) {}
    override fun onProviderDisabled(s: String) {}
}
package com.innomalist.taxi.common.utils

import android.content.Context
import android.location.LocationManager
import android.net.ConnectivityManager
import android.os.CountDownTimer
import androidx.appcompat.app.AppCompatActivity
import com.google.android.gms.maps.model.LatLng
import com.innomalist.taxi.common.models.Driver
import com.innomalist.taxi.common.models.Rider

object CommonUtils {
    const val MY_PERMISSIONS_REQUEST_STORAGE = 2
    var currentLocation: LatLng? = null
    @JvmStatic
    fun isInternetDisabled(activity: AppCompatActivity): Boolean {
        var haveConnectedWifi = false
        var haveConnectedMobile = false
        val cm = activity.getSystemService(Context.CONNECTIVITY_SERVICE) as ConnectivityManager
        val netInfo = cm.allNetworkInfo
        for (ni in netInfo) {
            if (ni.typeName.equals("WIFI", ignoreCase = true)) if (ni.isConnected) haveConnectedWifi = true
            if (ni.typeName.equals("MOBILE", ignoreCase = true)) if (ni.isConnected) haveConnectedMobile = true
        }
        return !haveConnectedWifi && !haveConnectedMobile
    }

    @JvmStatic
    fun isGPSEnabled(activity: AppCompatActivity): Boolean {
        val locationManager = activity.getSystemService(Context.LOCATION_SERVICE) as LocationManager
        return locationManager.isProviderEnabled(LocationManager.GPS_PROVIDER)
    }
}
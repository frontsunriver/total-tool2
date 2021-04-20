package com.innomalist.taxi.common.utils

import android.Manifest
import android.content.Context
import android.content.pm.PackageManager
import android.location.Location
import android.location.LocationListener
import android.location.LocationManager
import androidx.core.app.ActivityCompat
import com.google.android.gms.maps.model.LatLng

class LocationHelper {
    companion object {
        @JvmStatic
        fun distFrom(latLng1: LatLng, latLng2: LatLng): Int {
            val locationA = Location("LocationA")
            locationA.latitude = latLng1.latitude
            locationA.longitude = latLng1.longitude
            val locationB = Location("LocationB")
            locationB.latitude = latLng2.latitude
            locationB.longitude = latLng2.longitude
            locationA.distanceTo(locationB)
            return locationA.distanceTo(locationB).toInt()
        }

        @JvmStatic
        fun latLngToDoubleArray(position: LatLng): DoubleArray {
            return doubleArrayOf(position.latitude, position.longitude)
        }

        @JvmStatic
        fun doubleArrayToLatLng(position: DoubleArray): LatLng {
            return LatLng(position[0], position[1])
        }
    }
}
package com.innomalist.taxi.rider.networking.socket

import com.google.android.gms.maps.model.LatLng
import com.innomalist.taxi.common.models.ServiceCategory
import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest
import com.squareup.moshi.JsonClass
import org.json.JSONArray
import org.json.JSONObject

class CalculateFare(locations: List<LatLng>) : SocketRequest() {
    init {
        val arr = JSONArray()
        for(loc in locations) {
            val obj = JSONObject()
            obj.put("x", loc.longitude)
            obj.put("y", loc.latitude)
            arr.put(obj)
        }
        this.params = arrayOf(arr)
    }
}

@JsonClass(generateAdapter = true)
data class CalculateFareResult(
        val categories: List<ServiceCategory>,
        val distance: Int,
        val duration: Int,
        val currency: String
)
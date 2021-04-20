package com.innomalist.taxi.rider.networking.socket

import com.innomalist.taxi.common.models.Review
import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest
import com.innomalist.taxi.common.utils.Adapters
import org.json.JSONObject

class ReviewDriver(review: Review): SocketRequest() {
    init {
        val obj = JSONObject(Adapters.moshi.adapter<Review>(Review::class.java).toJson(review))
        this.params = arrayOf(obj)
    }
}
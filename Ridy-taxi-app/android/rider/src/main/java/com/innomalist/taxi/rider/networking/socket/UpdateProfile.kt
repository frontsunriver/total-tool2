package com.innomalist.taxi.rider.networking.socket

import com.innomalist.taxi.common.models.Rider
import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest
import com.innomalist.taxi.common.utils.Adapters
import org.json.JSONObject

class UpdateProfile(user: Rider): SocketRequest() {
    init {
        val obj = JSONObject(Adapters.moshi.adapter<Rider>(Rider::class.java).toJson(user))
        this.params = arrayOf(obj)
    }
}
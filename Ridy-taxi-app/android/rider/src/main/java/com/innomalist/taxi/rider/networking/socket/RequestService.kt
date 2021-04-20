package com.innomalist.taxi.rider.networking.socket

import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest
import com.innomalist.taxi.common.utils.Adapters
import com.innomalist.taxi.rider.models.RequestDTO
import org.json.JSONObject

class RequestService(requestDto: RequestDTO): SocketRequest() {
    init {
        val dto = JSONObject(Adapters.moshi.adapter<RequestDTO>(RequestDTO::class.java).toJson(requestDto))
        this.params = arrayOf(dto)
    }
}



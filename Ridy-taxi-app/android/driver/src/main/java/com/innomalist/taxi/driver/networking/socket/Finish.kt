package com.innomalist.taxi.driver.networking.socket

import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest
import com.innomalist.taxi.common.utils.Adapters
import org.json.JSONObject

class Finish(confirmationCode: Int?, distance: Int, log: String) : SocketRequest() {
    init {
        val dto = FinishDto(confirmationCode ?: 0, distance, log)
        val obj = JSONObject(Adapters.moshi.adapter<FinishDto>(FinishDto::class.java).toJson(dto))
        this.params = arrayOf(obj)
    }
}

data class FinishResult(
        val status: Boolean
)

data class FinishDto(
        val confirmationCode: Int,
        val distance: Int,
        val log: String
)
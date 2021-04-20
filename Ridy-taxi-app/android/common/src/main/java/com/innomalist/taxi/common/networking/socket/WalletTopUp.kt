package com.innomalist.taxi.common.networking.socket

import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest
import com.innomalist.taxi.common.utils.Adapters
import org.json.JSONObject

class WalletTopUp(gatewayId: Int, currency: String, token: String, amount: Double, pin: Int? = null, otp: Int? = null, transactionId: String? = null) : SocketRequest() {
    init {
        val dto = TopUpDto(gatewayId, amount, currency, token, pin, otp, transactionId)
        val obj = JSONObject(Adapters.moshi.adapter<TopUpDto>(TopUpDto::class.java).toJson(dto))
        this.params = arrayOf(obj)
    }
}

data class TopUpDto (
    val gatewayId: Int,
    val amount: Double,
    val currency: String,
    val token: String,
    val pin: Int? = null,
    val otp: Int? = null,
    val transactionId: String? = null
)

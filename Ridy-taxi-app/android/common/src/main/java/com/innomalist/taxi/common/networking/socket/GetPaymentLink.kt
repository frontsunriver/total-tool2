package com.innomalist.taxi.common.networking.socket

import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest
import com.innomalist.taxi.common.utils.Adapters
import org.json.JSONObject

class GetPaymentLink(gatewayId: Int, currency: String, amount: Double, serverUrl: String): SocketRequest() {
    init {
        val dto = GetPaymentLinkDTO(gatewayId, amount, currency, serverUrl)
        val obj = JSONObject(Adapters.moshi.adapter(GetPaymentLinkDTO::class.java).toJson(dto))
        this.params = arrayOf(obj)
    }
}

data class GetPaymentLinkDTO (
        val gatewayId: Int,
        val amount: Double,
        val currency: String,
        val serverUrl: String
)

data class GetPaymentLinkResult (
        val url: String
)
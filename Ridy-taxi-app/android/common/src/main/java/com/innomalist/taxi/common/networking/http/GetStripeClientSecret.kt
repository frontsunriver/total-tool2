package com.innomalist.taxi.common.networking.http

import com.innomalist.taxi.common.models.Rider
import com.innomalist.taxi.common.networking.http.interfaces.HTTPRequest

class GetStripeClientSecret(gatewayId: Int, amount: Int, currency: String): HTTPRequest() {
    override val path: String = "stripe_client_secret"
    init {
        this.params = mapOf("gatewayId" to gatewayId.toString(), "amount" to amount, "currency" to currency)
    }
}

data class StripeClientSecretEndpointResult(
        val clientSecret: String
)
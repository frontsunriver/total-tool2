package com.innomalist.taxi.common.models

import com.squareup.moshi.Json

data class PaymentGateway(
    var id: Int,
    var title: String?,
    var type: PaymentGatewayType,
    var publicKey: String? = null,
    var privateKey: String? = null
)
enum class PaymentGatewayType (val rawValue: String) {
    @Json(name="stripe")
    Stripe("stripe"),
    @Json(name="braintree")
    Braintree("braintree"),
    @Json(name = "paypal")
    PayPal("paypal"),
    @Json(name = "paytm")
    Paytm("paytm"),
    @Json(name = "razorpay")
    Razorpay("razorpay"),
    @Json(name = "paystack")
    Paystack("paystack"),
    @Json(name = "payu")
    PayU("payu"),
    @Json(name = "instamojo")
    Instamojo("instamojo"),
    @Json(name="flutterwave")
    Flutterwave("flutterwave"),
    @Json(name="paygate")
    PayGate("paygate"),
    @Json( name = "mips")
    MIPS("mips"),
    @Json( name = "link")
    CustomLink("link");

    companion object {
        operator fun invoke(rawValue: String) = values().firstOrNull { it.rawValue == rawValue }
    }
}

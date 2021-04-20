package com.innomalist.taxi.rider.networking.socket

import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest
import com.squareup.moshi.JsonClass

class ApplyCoupon(code: String) : SocketRequest() {
    init {
        this.params = arrayOf(code)
    }
}
@JsonClass(generateAdapter = true)
data class ApplyCouponResponse(
    val costAfterCoupon: Double? = 0.0
)
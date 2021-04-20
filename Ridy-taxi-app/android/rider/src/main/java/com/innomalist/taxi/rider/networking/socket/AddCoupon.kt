package com.innomalist.taxi.rider.networking.socket

import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest

class AddCoupon(code: String): SocketRequest() {
    init {
        this.params = arrayOf(code)
    }
}
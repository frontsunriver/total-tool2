package com.innomalist.taxi.driver.networking.socket

import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest

class AcceptOrder(requestId: Long) : SocketRequest() {
    init {
        this.params = arrayOf(requestId)
    }
}
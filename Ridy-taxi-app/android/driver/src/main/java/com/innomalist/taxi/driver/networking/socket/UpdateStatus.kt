package com.innomalist.taxi.driver.networking.socket

import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest

class UpdateStatus(turnOnline: Boolean): SocketRequest() {
    init {
        this.params = arrayOf(turnOnline)
    }
}
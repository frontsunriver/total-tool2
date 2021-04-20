package com.innomalist.taxi.rider.networking.socket

import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest

class DeleteAddress(id: Int): SocketRequest() {
    init {
        this.params = arrayOf(id)
    }
}
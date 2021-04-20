package com.innomalist.taxi.rider.networking.socket

import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest
import java.nio.Buffer

class UpdateProfileImage(data: ByteArray): SocketRequest() {
    init {
        this.params = arrayOf(data)
    }
}
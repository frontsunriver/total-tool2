package com.innomalist.taxi.common.networking.socket

import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest

class SendMessage(content: String) : SocketRequest() {
    init {
        this.params = arrayOf(content)
    }
}
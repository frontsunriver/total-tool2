package com.innomalist.taxi.common.networking.socket

import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest

class HideHistoryItem(requestId: Long) : SocketRequest() {
    init {
        this.params = arrayOf(requestId)
    }
}
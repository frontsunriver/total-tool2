package com.innomalist.taxi.rider.networking.socket

import com.google.gson.internal.LinkedHashTreeMap
import com.innomalist.taxi.common.models.Address
import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest
import com.innomalist.taxi.common.utils.Adapters
import org.json.JSONObject

class UpsertAddress(address: Address): SocketRequest() {
    init {
        val add = JSONObject(Adapters.moshi.adapter<Address>(Address::class.java).toJson(address))
        this.params = arrayOf(add)
    }
}
package com.innomalist.taxi.common.networking.socket

import com.google.android.gms.maps.model.LatLng
import com.innomalist.taxi.common.models.Request
import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest
import com.squareup.moshi.JsonClass

class GetCurrentRequestInfo: SocketRequest()

@JsonClass(generateAdapter = true)
data class CurrentRequestResult(
        val request: Request,
        val driverLocation: LatLng?
)
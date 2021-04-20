package com.innomalist.taxi.rider.models

import com.google.android.gms.maps.model.LatLng
import com.squareup.moshi.JsonClass

@JsonClass(generateAdapter = true)
data class RequestDTO(
        var locations: Array<LocationWithName>,
        var services: Array<OrderedService>,
        var intervalMinutes: Int = 0
)

@JsonClass(generateAdapter = true)
data class OrderedService(
        var serviceId: Long,
        var quantity: Int
)

@JsonClass(generateAdapter = true)
data class LocationWithName(
        var loc: LatLng,
        var add: String
)
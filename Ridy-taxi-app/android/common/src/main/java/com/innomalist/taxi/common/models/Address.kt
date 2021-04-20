package com.innomalist.taxi.common.models

import com.google.android.gms.maps.model.LatLng
import com.squareup.moshi.JsonClass

@JsonClass(generateAdapter = true)
data class Address(
        var address: String? = null,
        var location: LatLng? = null,
        var id: Int = 0,
        var title: String? = null
)
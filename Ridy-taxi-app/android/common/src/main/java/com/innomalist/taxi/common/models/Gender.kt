package com.innomalist.taxi.common.models

import com.squareup.moshi.Json

enum class Gender {
    @Json(name = "unknown")
    unknown,
    @Json(name = "male")
    male,
    @Json(name = "female")
    female
}
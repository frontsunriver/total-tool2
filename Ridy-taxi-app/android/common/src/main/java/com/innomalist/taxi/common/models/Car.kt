package com.innomalist.taxi.common.models

import com.squareup.moshi.JsonClass

@JsonClass(generateAdapter = true)
class Car {
    var media: Media? = null
    var id = 0
    var title: String? = null

}
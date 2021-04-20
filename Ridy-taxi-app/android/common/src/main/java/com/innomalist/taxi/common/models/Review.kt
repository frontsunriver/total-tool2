package com.innomalist.taxi.common.models

import com.squareup.moshi.JsonClass

@JsonClass(generateAdapter = true)
data class Review(val score: Int, val review: String, val id: Int)
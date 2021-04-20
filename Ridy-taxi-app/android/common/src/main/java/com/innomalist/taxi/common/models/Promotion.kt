package com.innomalist.taxi.common.models

import com.squareup.moshi.JsonClass
import java.sql.Timestamp
import java.util.*

@JsonClass(generateAdapter = true)
class Promotion {
    var startTimestamp: Long = 0
    var expirationTimestamp: Long = 0
    var description: String? = null
    var media: Media? = null
    var id = 0
    var title: String? = null
    var daysLeft = 0
        get() {
            val date = Date()
            field = ((expirationTimestamp - date.time) / (1000 * 60 * 60 * 24)).toInt()
            return field
        }

}
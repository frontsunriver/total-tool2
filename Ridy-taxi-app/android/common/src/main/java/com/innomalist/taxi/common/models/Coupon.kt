package com.innomalist.taxi.common.models

import com.squareup.moshi.JsonClass
import java.io.Serializable
import java.sql.Timestamp
import java.util.*

@JsonClass(generateAdapter = true)
class Coupon : Serializable {
    var isEnabled: Boolean = true
    var manyUsersCanUse = 0
    var manyTimesUserCanUse = 0
    var flatDiscount: Double = 0.0
    var code: String? = null
    var description: String? = null
    var id = 0
    var title: String? = null
    var startTimestamp: Long = 0
    var expirationTimestamp: Long = 0
    var discountPercent = 0
    var isFirstTravelOnly: Boolean = false
    var daysLeft = 0
        get() {
            field = ((expirationTimestamp - Date().time) / (1000 * 60 * 60 * 24)).toInt()
            return field
        }

}
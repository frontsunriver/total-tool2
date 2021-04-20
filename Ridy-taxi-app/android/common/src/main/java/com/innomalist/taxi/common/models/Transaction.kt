package com.innomalist.taxi.common.models

import com.squareup.moshi.JsonClass
import java.sql.Timestamp
import java.text.SimpleDateFormat
import java.util.*

@JsonClass(generateAdapter = true)
data class Transaction(
        var amount: Double,
        val currency: String,
        var documentNumber: String?,
        var transactionTime: Long,
        var details: String?,
        var id: Long,
        var transactionType: String
) {
    val day: String
        get() {
            val date = Date(transactionTime)
            return SimpleDateFormat("dd", Locale.getDefault()).format(date)
        }
    val month: String
        get() {
            val date = Date(transactionTime)
            return SimpleDateFormat("MMM", Locale.getDefault()).format(date)
        }
}
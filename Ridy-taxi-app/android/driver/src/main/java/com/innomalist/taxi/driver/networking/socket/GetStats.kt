package com.innomalist.taxi.driver.networking.socket

import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest
import com.squareup.moshi.Json
import com.squareup.moshi.JsonClass

class GetStats(queryType: QueryType) : SocketRequest() {
    init {
        this.params = arrayOf(queryType.value)
    }
}

@JsonClass(generateAdapter = true)
data class StatisticsResult(
        var currency: String,
        var dataset: List<DataPoint>
)

@JsonClass(generateAdapter = true)
data class DataPoint(
        var name: String,
        var current: String,
        var earning: Float,
        var count: String,
        var distance: String,
        var time: String
)

enum class QueryType(val value: String) {
    @Json(name = "daily")
    Daily("daily"),
    @Json(name = "weekly")
    Weekly("weekly"),
    @Json(name = "monthly")
    Monthly("monthly");

    companion object {
        operator fun get(code: String): QueryType {
            for (s in values()) {
                if (s.value == code) return s
            }
            return Daily
        }
    }
}
package com.innomalist.taxi.common.models

import com.squareup.moshi.Json
import com.squareup.moshi.JsonClass
import java.io.Serializable
@JsonClass(generateAdapter = true)
class Media(var address: String, var pathType: PathType) : Serializable {
    var id = 0

    enum class PathType {
        @Json(name="relative")
        relative,
        @Json(name="absolute")
        absolute
    }

    var privacyLevel: String? = null
    var title: String? = null

}
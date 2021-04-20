package com.innomalist.taxi.common.utils

import com.google.android.gms.maps.model.LatLng
import com.google.gson.*
import java.lang.reflect.Type

class LatLngDeserializer : JsonDeserializer<LatLng?>, JsonSerializer<LatLng> {
    override fun deserialize(json: JsonElement, typeOfT: Type, context: JsonDeserializationContext): LatLng? {
        val jsonObject = json.asJsonObject
        return if (jsonObject["y"] != null) LatLng(
                jsonObject["y"].asDouble,
                jsonObject["x"].asDouble) else {
            if (jsonObject["latitude"] != null) {
                LatLng(
                        jsonObject["latitude"].asDouble,
                        jsonObject["longitude"].asDouble)
            } else {
                null
            }
        }
    }

    override fun serialize(src: LatLng, typeOfSrc: Type, context: JsonSerializationContext): JsonElement {
        val jsonObject = JsonObject()
        jsonObject.addProperty("x", src.longitude)
        jsonObject.addProperty("y", src.latitude)
        return jsonObject
    }
}
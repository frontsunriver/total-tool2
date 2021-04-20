package com.innomalist.taxi.common.utils

import com.google.android.gms.maps.model.LatLng
import com.squareup.moshi.FromJson
import com.squareup.moshi.JsonClass
import com.squareup.moshi.Moshi
import com.squareup.moshi.ToJson
import com.squareup.moshi.kotlin.reflect.KotlinJsonAdapterFactory
import org.json.JSONObject


class Adapters {
    companion object {
        val moshi: Moshi = Moshi.Builder().add(LatLngAdapter()).add(KotlinJsonAdapterFactory()).build()

    }
}

class LatLngAdapter {
    @ToJson
    fun toJson(card: LatLng): MutableMap<String, Double> {
        /*val obj = JSONObject()
        obj.put("x", card.longitude)
        obj.put("y", card.latitude)
        return obj.toString()*/
        val m = LinkedHashMap<String, Double>()
        m.put("x", card.longitude)
        m.put("y", card.latitude)
        return m
    }

    @FromJson
    fun fromJson(map: MutableMap<String, Double>): LatLng {
        //val obj = JSONObject(card)
        return LatLng(map.get("y") as Double, map.get("x") as Double)
    }
}

@JsonClass(generateAdapter = true)
data class CoordinateXY(
        val x: Double,
        val y: Double
)
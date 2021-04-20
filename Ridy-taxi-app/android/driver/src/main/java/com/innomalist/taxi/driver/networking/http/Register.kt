package com.innomalist.taxi.driver.networking.http

import com.innomalist.taxi.common.models.Driver
import com.innomalist.taxi.common.networking.http.interfaces.HTTPRequest
import com.innomalist.taxi.common.utils.Adapters
import org.json.JSONObject

class Register(jwtToken: String, driver: Driver) : HTTPRequest() {
    override val path: String = "driver/register"

    init {
        val mapped = Adapters.moshi.adapter<Driver>(Driver::class.java).toJsonValue(driver)!!
        this.params = mapOf("token" to jwtToken, "driver" to mapped)
    }
}
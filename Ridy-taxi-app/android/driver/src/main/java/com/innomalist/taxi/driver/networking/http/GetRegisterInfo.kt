package com.innomalist.taxi.driver.networking.http

import com.innomalist.taxi.common.models.Driver
import com.innomalist.taxi.common.models.Service
import com.innomalist.taxi.common.networking.http.interfaces.HTTPRequest
import com.squareup.moshi.Json
import com.squareup.moshi.JsonClass

class GetRegisterInfo(jwtToken: String): HTTPRequest() {
    override val path: String = "driver/get"
    init {
        this.params = mapOf("token" to jwtToken)
    }
}
@JsonClass(generateAdapter = true)
data class RegistrationInfo(
    val driver: Driver,
    val services: List<Service>
)
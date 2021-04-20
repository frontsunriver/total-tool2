package com.innomalist.taxi.rider.networking.http

import com.innomalist.taxi.common.models.Rider
import com.innomalist.taxi.common.networking.http.interfaces.HTTPRequest

class Login(fireBaseToken: String): HTTPRequest() {
    override val path: String = "rider/login"
    init {
        this.params = mapOf("token" to fireBaseToken)
    }
}

data class LoginResult(
    val token: String,
    val user: Rider
)
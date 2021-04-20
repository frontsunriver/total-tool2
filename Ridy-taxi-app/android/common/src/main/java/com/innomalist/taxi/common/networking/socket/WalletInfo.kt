package com.innomalist.taxi.common.networking.socket

import com.innomalist.taxi.common.models.PaymentGateway
import com.innomalist.taxi.common.models.WalletItem
import com.innomalist.taxi.common.networking.socket.interfaces.SocketRequest
import com.squareup.moshi.JsonClass

class WalletInfo : SocketRequest()

@JsonClass(generateAdapter = true)
data class WalletInfoResult(
    val gateways: List<PaymentGateway>,
    val wallet: List<WalletItem>
    )
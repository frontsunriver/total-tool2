package com.innomalist.taxi.common.models

import com.innomalist.taxi.common.Config
import com.squareup.moshi.JsonClass
import com.stfalcon.chatkit.commons.models.IUser

@JsonClass(generateAdapter = true)
data class Rider(
        var id: Long = 0,
        var firstName: String? = null,
        var lastName: String? = null,
        var media: Media? = null,
        var mobileNumber: Long = 0,
        var status: String? = null,
        var email: String? = null,
        var gender: Gender? = null,
        val wallet: List<WalletItem>? = null,
        var address: String? = null
) : IUser {
    override fun getId(): String {
        return id.toString()
    }

    override fun getName(): String {
        return lastName!!
    }

    override fun getAvatar(): String? {
        return if (media == null) {
            null
        } else {
            "${Config.Backend}${media!!.address}"
        }
    }
}
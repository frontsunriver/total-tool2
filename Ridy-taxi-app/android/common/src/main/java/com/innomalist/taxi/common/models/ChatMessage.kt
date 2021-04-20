package com.innomalist.taxi.common.models

import com.squareup.moshi.JsonClass
import com.stfalcon.chatkit.commons.models.IMessage
import com.stfalcon.chatkit.commons.models.IUser
import java.util.*

@JsonClass(generateAdapter = true)
data class ChatMessage(
        var id: Long = 0,
        val sentBy: String? = null,
        val sentAt: Long? = null,
        var state: String? = null,
        var request: Request? = null,
        val content: String
) : IMessage {

    override fun getId(): String {
        return id.toString()
    }

    override fun getText(): String {
        return content
    }

    override fun getUser(): IUser {
        return if (sentBy == "d") {
            request!!.driver!!
        } else {
            request!!.rider!!
        }
    }

    override fun getCreatedAt(): Date {
        return Date(sentAt!!)
    }

}
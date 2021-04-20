package com.innomalist.taxi.common.models

import com.innomalist.taxi.common.Config
import com.squareup.moshi.Json
import com.squareup.moshi.JsonClass
import com.stfalcon.chatkit.commons.models.IUser
import java.sql.Timestamp

@JsonClass(generateAdapter = true)
data class Driver(
    var accountNumber: String?,
    var bankName: String?,
    var bankRoutingNumber: String?,
    var bankSwift: String?,
    var media: Media? = null,
    var carMedia: Media? = null,
    var carPlate: String? = null,
    var address: String? = null,
    var gender: Gender? = null,
    val rating: Int? = null,
    var firstName: String? = null,
    var lastName: String? = null,
    var carColor: String? = null,
    var certificateNumber: String? = null,
    var carProductionYear: String? = null,
    val id: Long? = null,
    val mobileNumber: Long? = null,
    val car: Car? = null,
    var email: String? = null,
    var status: Status? = null,
    val documentsNote: String? = null,
    val wallet: List<WalletItem>? = null,
    val documents: List<Media>? = null,
    var services: List<Service>? = null): IUser {


    override fun getId(): String {
        return id.toString()
    }

    override fun getName(): String {
        return lastName!!
    }

    override fun getAvatar(): String? {
        return if (media == null) null else "${Config.Backend}${media!!.address}"
    }

    enum class Status(val value: String) {
        @Json(name="offline")Offline("offline"), @Json(name="online")Online("online"), @Json(name="in service")InService("in service"), @Json(name="blocked")Blocked("blocked"), @Json(name="pending approval")PendingApproval("pending approval"), @Json(name="waiting documents")WaitingDocuments("waiting documents"), @Json(name="soft reject")SoftReject("soft reject"), @Json(name="hard reject")HardReject("hard reject");

        companion object {
            operator fun get(code: String): Status {
                for (s in values()) {
                    if (s.value == code) return s
                }
                return Offline
            }
        }
    }
}
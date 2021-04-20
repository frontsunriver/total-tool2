package com.innomalist.taxi.common.models

import com.google.android.gms.maps.model.LatLng
import com.innomalist.taxi.common.R
import com.squareup.moshi.Json
import com.squareup.moshi.JsonClass

@JsonClass(generateAdapter = true)
data class Request(
        var driver: Driver? = null,
        var rider: Rider? = null,
        var cost: Double? = null,
        var startTimestamp: Long? = 0,
        var log: String? = null,
        var distanceBest: Int? = null,
        var rating: Int? = null,
        var isHidden: Int? = null,
        var addresses: List<String> = ArrayList(),
        var points: List<LatLng> = ArrayList(),
        var finishTimestamp: Long? = null,
        var requestTimestamp: Long? = null,
        var etaPickup: Long? = 0,
        var durationBest: Int? = null,
        var costBest: Double? = null,
        var costAfterCoupon: Double? = null,
        var providerShare: Double? = 0.0,
        var currency: String? = null,
        var durationReal: Long? = null,
        var distanceReal: Long? = null,
        var id: Long? = null,
        var status: Status? = null,
        var imageUrl: String? = null,
        var service: Service? = null,
        var confirmationCode: Int? = null
) {
    enum class Status {
        @Json(name = "Requested")
        Requested,
        @Json(name = "NotFound")
        NotFound,
        @Json(name = "NoCloseFound")
        NoCloseFound,
        @Json(name = "Found")
        Found,
        @Json(name = "DriverAccepted")
        DriverAccepted,
        @Json(name = "Arrived")
        Arrived,
        @Json(name = "WaitingForPrePay")
        WaitingForPrePay,
        @Json(name = "RiderCanceled")
        RiderCanceled,
        @Json(name = "DriverCanceled")
        DriverCanceled,
        @Json(name = "WaitingForPostPay")
        WaitingForPostPay,
        @Json(name = "WaitingForReview")
        WaitingForReview,
        @Json(name = "Started")
        Started,
        @Json(name = "Booked")
        Booked,
        @Json(name = "Expired")
        Expired,
        @Json(name = "Finished")
        Finished;

        val localizedDescription: Int
            get() {
                return when (this) {
                    Requested -> R.string.request_status_requested
                    NotFound -> R.string.request_status_not_found
                    NoCloseFound -> R.string.request_status_no_close_found
                    Found -> R.string.request_status_found
                    DriverAccepted -> R.string.request_status_driver_accepted
                    Arrived -> R.string.request_status_arrived
                    WaitingForPrePay -> R.string.request_status_waiting_for_pre_pay
                    RiderCanceled -> R.string.request_status_rider_canceled
                    DriverCanceled -> R.string.request_status_driver_canceled
                    WaitingForPostPay -> R.string.request_status_waiting_for_post_pay
                    WaitingForReview -> R.string.request_status_waiting_for_review
                    Started -> R.string.request_status_started
                    Booked -> R.string.request_status_booked
                    Expired -> R.string.request_status_expired
                    Finished -> R.string.request_status_finished
                }
            }
    }

}



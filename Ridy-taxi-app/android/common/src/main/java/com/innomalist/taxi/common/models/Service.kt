package com.innomalist.taxi.common.models

import com.squareup.moshi.JsonClass
import java.io.Serializable

@JsonClass(generateAdapter = true)
data class Service(
        val serviceCategory: ServiceCategory?,
        val media: Media?,
        val availableTimeFrom: String?,
        val perHundredMeters: Double,
        val availableTimeTo: String?,
        val perMinuteDrive: Double,
        val rangeMinusPercent: Int?,
        val rangePlusPercent: Int?,
        val baseFare: Double,
        val id: Long = 0,
        val title: String?,
        val perMinuteWait: Double,
        val minimumFee: Double?,
        val cost: Double?,
        val canEnableVerificationCode: Boolean,
        val distanceFeeMode: DistanceFee,
        val feeEstimationMode: FeeEstimationMode,
        val paymentMethod: PaymentMethod,
        val paymentTime: PaymentTime,
        val quantityMode: QuantityMode,
        val bookingMode: BookingMode
) : Serializable {
    enum class DistanceFee {
        None, PickupToDestination
    }

    enum class FeeEstimationMode {
        Static, Dynamic, Ranged, RangedStrict, Disabled
    }

    enum class PaymentMethod {
        CashCredit, OnlyCredit, OnlyCash
    }

    enum class PaymentTime {
        PrePay, PostPay
    }

    enum class QuantityMode {
        Singular, Multiple
    }

    enum class BookingMode {
        OnlyNow, Time, DateTime, DateTimeAbosoluteHour
    }
}
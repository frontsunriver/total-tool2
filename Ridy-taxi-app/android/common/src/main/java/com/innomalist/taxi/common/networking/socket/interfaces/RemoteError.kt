package com.innomalist.taxi.common.networking.socket.interfaces

import android.content.Context
import android.view.View
import com.google.android.material.snackbar.Snackbar
import com.innomalist.taxi.common.R
import com.innomalist.taxi.common.utils.AlerterHelper
import com.squareup.moshi.Json
import com.squareup.moshi.JsonClass

@JsonClass(generateAdapter = true)
data class RemoteError(
        var status: ErrorStatus,
        var message: String? = null) {
        fun showAlert(context: Context) {
            val message = when(this.status) {
                ErrorStatus.Unknown -> "Unknown Error: ${this.message}"
                else -> context.getString(this.status.localizedDescription)
            }
            AlerterHelper.showError(context, message)
        }
}

enum class ErrorStatus {
    @Json(name = "DistanceCalculationFailed")
    DistanceCalculationFailed,
    @Json(name = "DriversUnavailable")
    DriversUnavailable,
    @Json(name = "ConfirmationCodeRequired")
    ConfirmationCodeRequired,
    @Json(name = "ConfirmationCodeInvalid")
    ConfirmationCodeInvalid,
    @Json(name = "OrderAlreadyTaken")
    OrderAlreadyTaken,
    @Json(name = "CreditInsufficient")
    CreditInsufficient,
    @Json(name = "CouponUsed")
    CouponUsed,
    @Json(name = "CouponExpired")
    CouponExpired,
    @Json(name = "CouponInvalid")
    CouponInvalid,
    @Json(name = "Unknown")
    Unknown,
    @Json(name = "Networking")
    Networking,
    @Json(name = "FailedEncoding")
    FailedEncoding,
    @Json(name = "FailedToVerify")
    FailedToVerify,
    @Json(name = "RegionUnsupported")
    RegionUnsupported,
    @Json(name = "NoServiceInRegion")
    NoServiceInRegion,
    @Json(name = "PINCodeRequired")
    PINCodeRequired,
    @Json(name = "OTPCodeRequired")
    OTPCodeRequired;

    val localizedDescription: Int
        get() {
            return when (this) {
                DistanceCalculationFailed -> R.string.socket_error_distance_calculation_failed
                DriversUnavailable -> R.string.socket_error_drivers_unavailable
                ConfirmationCodeRequired -> R.string.socket_error_confirm_code_required
                ConfirmationCodeInvalid -> R.string.socket_error_confirm_code_invalid
                OrderAlreadyTaken -> R.string.socket_error_order_already_taken
                Unknown -> R.string.socket_error_unknown
                Networking -> R.string.socket_error_networking
                FailedEncoding -> R.string.socket_error_failed_encoding
                FailedToVerify -> R.string.socket_error_failed_to_verify
                RegionUnsupported -> R.string.socket_error_region_unsupported
                NoServiceInRegion -> R.string.socket_error_no_service_provided_in_region
                CreditInsufficient -> R.string.socket_error_credit_insufficient
                CouponUsed -> R.string.socket_error_coupon_used
                CouponExpired -> R.string.socket_error_coupon_expired
                CouponInvalid -> R.string.socket_error_coupon_invalid
                PINCodeRequired -> R.string.socket_error_pin_required
                OTPCodeRequired -> R.string.socket_error_otp_required
            }
        }


}

package com.innomalist.taxi.common.utils

import java.text.DecimalFormat
import java.text.NumberFormat
import java.util.*
import kotlin.math.floor

object DistanceFormatter {
    private const val METERS_IN_ONE_MILE = 1609.0
    private const val METERS_IN_ONE_FOOT = 0.3048
    private const val FEET_IN_ONE_MILE = 5280.0
    private lateinit var decimalFormat: DecimalFormat


    fun format(distanceInMeters: Int, realTime: Boolean = false): String {
        val locale: Locale = Locale.getDefault()
        return format(distanceInMeters, realTime, locale)
    }

    /**
     * Format distance for display using specified distance units.
     *
     * @param distanceInMeters the actual distance in meters.
     * @param realTime boolean flag for navigation vs. list view.
     * @param units miles or kilometers.
     * @return distance string formatted according to the rules of the formatter.
     */
    fun format(distanceInMeters: Int, realTime: Boolean, units: DistanceUnits?): String {
        return format(distanceInMeters, realTime, Locale.getDefault(), units)
    }

    /**
     * Format distance for display using specified locale.
     *
     * @param distanceInMeters the actual distance in meters.
     * @param realTime boolean flag for navigation vs. list view.
     * @param locale Locale that defines the number format for displaying distance.
     * @return distance string formatted according to the rules of the formatter.
     */
    fun format(distanceInMeters: Int, realTime: Boolean, locale: Locale): String {
        return if (useMiles(locale)) {
            format(distanceInMeters, realTime, locale, DistanceUnits.MILES)
        } else {
            format(distanceInMeters, realTime, locale, DistanceUnits.KILOMETERS)
        }
    }

    /**
     * Format distance for display using specified locale and units.
     *
     * @param distanceInMeters the actual distance in meters.
     * @param realTime boolean flag for navigation vs. list view.
     * @param locale Locale that defines the number format for displaying distance.
     * @param units miles or kilometers.
     * @return distance string formatted according to the rules of the formatter.
     */
    fun format(distanceInMeters: Int, realTime: Boolean, locale: Locale?,
               units: DistanceUnits?): String {
        decimalFormat = NumberFormat.getNumberInstance(locale!!) as DecimalFormat
        decimalFormat.applyPattern("#.#")
        return if (distanceInMeters == 0) {
            ""
        } else when (units) {
            DistanceUnits.MILES -> formatMiles(distanceInMeters, realTime)
            DistanceUnits.KILOMETERS -> formatKilometers(distanceInMeters, realTime)
            else -> ""
        }
    }

    private fun formatMiles(distanceInMeters: Int, realTime: Boolean): String {
        val distanceInFeet = distanceInMeters / METERS_IN_ONE_FOOT
        return when {
            distanceInFeet < 10 -> {
                formatDistanceLessThanTenFeet(distanceInFeet, realTime)
            }
            distanceInFeet < FEET_IN_ONE_MILE / 10 -> {
                formatDistanceOverTenFeet(distanceInFeet)
            }
            else -> {
                formatDistanceInMiles(distanceInMeters)
            }
        }
    }

    private fun formatKilometers(distanceInMeters: Int, realTime: Boolean): String {
        return when {
            distanceInMeters >= 100 -> {
                formatDistanceInKilometers(distanceInMeters)
            }
            distanceInMeters > 10 -> {
                formatDistanceOverTenMeters(distanceInMeters)
            }
            else -> {
                formatShortMeters(distanceInMeters, realTime)
            }
        }
    }

    private fun useMiles(locale: Locale): Boolean {
        return locale == Locale.US || locale == Locale.UK
    }

    private fun formatDistanceOverTenMeters(distanceInMeters: Int): String {
        return java.lang.String.format(Locale.getDefault(), "%s m", distanceInMeters)
    }

    private fun formatShortMeters(distanceInMeters: Int, realTime: Boolean): String {
        return if (realTime) {
            "now"
        } else {
            formatDistanceOverTenMeters(distanceInMeters)
        }
    }

    private fun formatDistanceInKilometers(distanceInMeters: Int): String {
        val value: String = decimalFormat.format(distanceInMeters.toFloat() / 1000)
        return java.lang.String.format(Locale.getDefault(), "%s km", value)
    }

    private fun formatDistanceLessThanTenFeet(distanceInFeet: Double, realTime: Boolean): String {
        return if (realTime) {
            "now"
        } else {
            java.lang.String.format(Locale.getDefault(), "%d ft", floor(distanceInFeet).toInt())
        }
    }

    private fun formatDistanceOverTenFeet(distanceInFeet: Double): String {
        val roundedDistanceInFeet = roundDownToNearestTen(distanceInFeet)
        return java.lang.String.format(Locale.getDefault(), "%d ft", roundedDistanceInFeet)
    }

    private fun formatDistanceInMiles(distanceInMeters: Int): String {
        return java.lang.String.format(Locale.getDefault(), "%s mi",
                decimalFormat.format(distanceInMeters / METERS_IN_ONE_MILE))
    }

    private fun roundDownToNearestTen(distance: Double): Int {
        return floor(distance / 10).toInt() * 10
    }

    enum class DistanceUnits {
        MILES,
        KILOMETERS
    }
}
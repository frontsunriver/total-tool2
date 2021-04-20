package com.innomalist.taxi.common.utils

import java.util.regex.Pattern

object Validators {
    @JvmStatic
    fun validateEmailAddress(emailAddress: String?): Boolean {
        val regexPattern = Pattern.compile("^[(a-zA-Z-0-9-_+.)]+@[(a-z-A-z)]+\\.[(a-zA-z)]{2,3}$")
        val regMatcher = regexPattern.matcher(emailAddress ?: "")
        return regMatcher.matches()
    }
}
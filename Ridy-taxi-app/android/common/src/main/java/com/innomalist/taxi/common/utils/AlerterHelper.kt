package com.innomalist.taxi.common.utils

import android.content.Context
import androidx.appcompat.app.AppCompatActivity
import com.innomalist.taxi.common.R
import com.tapadoo.alerter.Alerter

object AlerterHelper {
    @JvmStatic
    fun showError(context: Context?, message: String?) {
        Alerter.create((context as AppCompatActivity?)!!)
                .setTitle(R.string.error)
                .setText(message!!)
                .setIcon(R.drawable.ic_error)
                .setBackgroundColorRes(R.color.accent_red)
                .show()
    }

    @JvmStatic
    fun showWarning(context: Context?, message: String?) {
        Alerter.create((context as AppCompatActivity?)!!)
                .setTitle(R.string.warning)
                .setText(message!!)
                .setIcon(R.drawable.ic_warning)
                .setBackgroundColorRes(R.color.accent_orange)
                .show()
    }

    @JvmStatic
    fun showInfo(context: Context?, message: String?) {
        Alerter.create((context as AppCompatActivity?)!!)
                .setTitle(R.string.info)
                .setText(message!!)
                .setIcon(R.drawable.ic_info)
                .setBackgroundColorRes(R.color.accent_cyan)
                .show()
    }
}
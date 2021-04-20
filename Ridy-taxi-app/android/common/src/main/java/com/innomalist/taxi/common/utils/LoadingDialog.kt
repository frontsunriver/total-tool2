package com.innomalist.taxi.common.utils

import android.app.Activity
import androidx.appcompat.app.AlertDialog
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import com.innomalist.taxi.common.R
import java.lang.Exception

object LoadingDialog {
    var dialog: AlertDialog? = null

    fun display(context: Activity) {
        if(dialog != null) {
            return
        }
        try {
            dialog = MaterialAlertDialogBuilder(context)
                    .setView(R.layout.dialog_loading)
                    .setCancelable(false)
                    .show()
        } catch (exception: Exception) {

        }
    }

    fun hide() {
        if (dialog != null) {
            dialog!!.dismiss()
            dialog = null
        }
    }
}
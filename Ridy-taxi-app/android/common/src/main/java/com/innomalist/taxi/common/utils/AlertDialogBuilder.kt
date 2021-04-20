package com.innomalist.taxi.common.utils

import android.content.Context
import android.content.DialogInterface
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import com.innomalist.taxi.common.R
import com.innomalist.taxi.common.interfaces.AlertDialogEvent

object AlertDialogBuilder {
    @JvmOverloads
    fun show(context: Context, message: String?, title: String? = context.getString(R.string.message_default_title), button: DialogButton = DialogButton.OK, event: AlertDialogEvent?) {
        val materialDialogBuilder = MaterialAlertDialogBuilder(context)
                .setTitle(title)
                .setMessage(message)
        if (button == DialogButton.OK || button == DialogButton.OK_CANCEL) {
            val positiveText = context.getString(R.string.alert_ok)
            materialDialogBuilder.setPositiveButton(positiveText) { _: DialogInterface?, _: Int -> event?.onAnswerDialog(DialogResult.OK) }
        }
        if (button == DialogButton.OK_CANCEL || button == DialogButton.CANCEL_RETRY) {
            val negativeText = context.getString(R.string.alert_cancel)
            materialDialogBuilder.setNegativeButton(negativeText, null)
        }
        if (button == DialogButton.CANCEL_RETRY) {
            val positiveText = context.getString(R.string.alert_retry)
            materialDialogBuilder.setPositiveButton(positiveText) { _: DialogInterface?, _: Int -> event?.onAnswerDialog(DialogResult.RETRY) }
        }
        materialDialogBuilder.setOnCancelListener { event?.onAnswerDialog (DialogResult.CANCEL) }
        materialDialogBuilder.show()
    }

    @JvmStatic
    fun show(context: Context, message: String?, button: DialogButton, event: AlertDialogEvent?) {
        show(context, message, context.getString(R.string.message_default_title), button, event)
    }

    enum class DialogButton {
        OK_CANCEL, OK, CANCEL_RETRY
    }

    enum class DialogResult {
        CANCEL, OK, RETRY
    }
}
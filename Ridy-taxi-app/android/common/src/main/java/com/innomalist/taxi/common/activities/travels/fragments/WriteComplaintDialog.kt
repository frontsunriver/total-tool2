package com.innomalist.taxi.common.activities.travels.fragments

import android.app.Dialog
import android.content.Context
import android.content.DialogInterface
import android.os.Bundle
import android.text.Editable
import android.text.TextWatcher
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.appcompat.app.AlertDialog
import androidx.databinding.DataBindingUtil
import androidx.fragment.app.DialogFragment
import com.innomalist.taxi.common.R
import com.innomalist.taxi.common.databinding.DialogWriteComplaintBinding

class WriteComplaintDialog : DialogFragment() {
    lateinit var binding: DialogWriteComplaintBinding
    private var mListener: OnWriteComplaintInteractionListener? = null
    fun onCreateDialogView(inflater: LayoutInflater?, container: ViewGroup?, savedInstanceState: Bundle?): View {
        binding = DataBindingUtil.inflate(inflater!!, R.layout.dialog_write_complaint, container, false)
        return binding.root
    }

    override fun onCreateDialog(savedInstanceState: Bundle?): Dialog {
        val alertDialogBuilder = AlertDialog.Builder(activity!!)
        alertDialogBuilder.setTitle(R.string.write_complaint)
        val view = onCreateDialogView(activity!!.layoutInflater, null, null)
        onViewCreated(view, null)
        alertDialogBuilder.setView(view)
        binding.textContent.addTextChangedListener(object : TextWatcher {
            override fun onTextChanged(s: CharSequence, start: Int, before: Int, count: Int) {
                val dialog = dialog as AlertDialog?
                dialog!!.getButton(AlertDialog.BUTTON_POSITIVE).isEnabled = s.toString().trim { it <= ' ' }.isNotEmpty()
            }

            override fun beforeTextChanged(s: CharSequence, start: Int, count: Int, after: Int) {}
            override fun afterTextChanged(s: Editable) {}
        })
        alertDialogBuilder.setPositiveButton(getString(R.string.alert_ok)) { _: DialogInterface?, _: Int ->
            val event = Complaint(binding.textSubject.text.toString(), binding.textContent.text.toString())
            mListener!!.onSaveComplaintClicked(event)
        }
        alertDialogBuilder.setNegativeButton(getString(R.string.alert_cancel)) { _: DialogInterface?, _: Int ->
            dialog?.dismiss()
        }
        return alertDialogBuilder.create()
    }

    override fun onAttach(context: Context) {
        super.onAttach(context)
        mListener = if (context is OnWriteComplaintInteractionListener) {
            context
        } else {
            throw RuntimeException("$context must implement onEditAddressInteractionListener")
        }
    }

    override fun onResume() {
        super.onResume()
        val dialog = dialog as AlertDialog?
        dialog!!.getButton(AlertDialog.BUTTON_POSITIVE).isEnabled = false
    }

    override fun onDetach() {
        super.onDetach()
        mListener = null
    }

    interface OnWriteComplaintInteractionListener {
        fun onSaveComplaintClicked(event: Complaint)
    }
}

data class Complaint(
        val subject: String,
        val content: String
)
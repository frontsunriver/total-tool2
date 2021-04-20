package com.innomalist.taxi.rider.activities.addresses.fragments

import android.app.Dialog
import android.content.Context
import android.content.DialogInterface
import android.os.Bundle
import android.text.Editable
import android.text.TextWatcher
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import androidx.appcompat.app.AlertDialog
import androidx.databinding.DataBindingUtil
import androidx.fragment.app.DialogFragment
import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.GoogleMap
import com.google.android.gms.maps.OnMapReadyCallback
import com.google.android.gms.maps.SupportMapFragment
import com.innomalist.taxi.common.models.Address
import com.innomalist.taxi.common.utils.Adapters
import com.innomalist.taxi.rider.R
import com.innomalist.taxi.rider.databinding.FragmentEditAddressBinding
import java.lang.Exception

class EditAddressDialog : DialogFragment(), OnMapReadyCallback {
    lateinit var binding: FragmentEditAddressBinding
    private var address: Address? = null
    var googleMap: GoogleMap? = null
    var mapFragment: SupportMapFragment? = null
    private var mListener: OnEditAddressInteractionListener? = null
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        if (arguments != null) {
            val str = requireArguments().getString(ARG_ADDRESS)
            address = Adapters.moshi.adapter<Address>(Address::class.java).fromJson(str!!)
        }
    }

    private fun onCreateDialogView(inflater: LayoutInflater?): View {
        binding = DataBindingUtil.inflate(inflater!!, R.layout.fragment_edit_address, null, false)
        binding.address = address
        return binding.root
    }

    override fun onCreateDialog(savedInstanceState: Bundle?): Dialog {
        val alertDialogBuilder = AlertDialog.Builder(requireActivity())
        if (address!!.id != 0) alertDialogBuilder.setTitle(R.string.edit_address_dialog_title) else alertDialogBuilder.setTitle(R.string.add_address_dialog_title)
        val view = onCreateDialogView(requireActivity().layoutInflater)
        onViewCreated(view, null)
        alertDialogBuilder.setView(view)
        mapFragment = requireActivity().supportFragmentManager.findFragmentById(R.id.map) as SupportMapFragment?
        mapFragment!!.getMapAsync(this)
        binding.textTitle.addTextChangedListener(object : TextWatcher {
            override fun onTextChanged(s: CharSequence, start: Int, before: Int, count: Int) {
                val dialog = dialog as AlertDialog?
                dialog!!.getButton(AlertDialog.BUTTON_POSITIVE).isEnabled = s.toString().trim { it <= ' ' }.isNotEmpty()
            }

            override fun beforeTextChanged(s: CharSequence, start: Int, count: Int, after: Int) {}
            override fun afterTextChanged(s: Editable) {}
        })
        alertDialogBuilder.setPositiveButton(R.string.alert_ok) { _: DialogInterface?, _: Int ->
            address!!.title = binding.textTitle.text.toString()
            address!!.address = binding.textAddress.text.toString()
            address!!.location = googleMap!!.cameraPosition.target
            mListener!!.onSaveButtonClicked(address!!)
        }
        alertDialogBuilder.setNegativeButton(R.string.alert_cancel) { dialog: DialogInterface?, _: Int ->
            dialog?.dismiss()
        }
        return alertDialogBuilder.create()
    }

    override fun onAttach(context: Context) {
        super.onAttach(context)
        mListener = if (context is OnEditAddressInteractionListener) {
            context
        } else {
            throw RuntimeException("$context must implement onEditAddressInteractionListener")
        }
    }

    override fun onDetach() {
        super.onDetach()
        mListener = null
    }

    override fun onPause() {
        if (mapFragment != null && activity != null) requireActivity().supportFragmentManager.beginTransaction().remove(mapFragment!!).commitAllowingStateLoss()
        super.onPause()
    }

    override fun onResume() {
        super.onResume()
        val dialog = dialog as AlertDialog?
        dialog!!.getButton(AlertDialog.BUTTON_POSITIVE).isEnabled = false
    }

    override fun onMapReady(googleMap: GoogleMap) {
        this.googleMap = googleMap
        if (address!!.location != null) googleMap.animateCamera(CameraUpdateFactory.newLatLngZoom(address!!.location, 16f))
    }

    interface OnEditAddressInteractionListener {
        fun onSaveButtonClicked(address: Address)
    }

    companion object {
        private const val ARG_ADDRESS = "address"
        fun newInstance(param1: Address): EditAddressDialog {
            try {
                val fragment = EditAddressDialog()
                val args = Bundle()
                val str = Adapters.moshi.adapter<Address>(Address::class.java).toJson(param1)
                args.putString(ARG_ADDRESS, str)
                fragment.arguments = args
                return fragment
            } catch (exception: Exception) {
                Log.e("unable to start dialog", exception.stackTrace.toString())
                return EditAddressDialog()
            }


        }
    }
}
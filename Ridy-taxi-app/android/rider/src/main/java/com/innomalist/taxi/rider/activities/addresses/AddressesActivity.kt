package com.innomalist.taxi.rider.activities.addresses

import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import androidx.databinding.DataBindingUtil
import androidx.recyclerview.widget.LinearLayoutManager
import com.google.android.gms.maps.model.LatLng
import com.innomalist.taxi.common.interfaces.AlertDialogEvent
import com.innomalist.taxi.common.models.Address
import com.innomalist.taxi.common.networking.socket.interfaces.EmptyClass
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import com.innomalist.taxi.common.utils.AlertDialogBuilder
import com.innomalist.taxi.common.utils.AlertDialogBuilder.DialogResult
import com.innomalist.taxi.common.utils.AlertDialogBuilder.show
import com.innomalist.taxi.common.utils.LocationHelper.Companion.doubleArrayToLatLng
import com.innomalist.taxi.rider.R
import com.innomalist.taxi.rider.activities.addresses.adapters.AddressesRecyclerViewAdapter
import com.innomalist.taxi.rider.activities.addresses.adapters.AddressesRecyclerViewAdapter.OnAddressItemInteractionListener
import com.innomalist.taxi.rider.activities.addresses.fragments.EditAddressDialog
import com.innomalist.taxi.rider.activities.addresses.fragments.EditAddressDialog.OnEditAddressInteractionListener
import com.innomalist.taxi.rider.databinding.ActivityAddressesBinding
import com.innomalist.taxi.rider.networking.socket.DeleteAddress
import com.innomalist.taxi.rider.networking.socket.GetAddresses
import com.innomalist.taxi.rider.networking.socket.UpsertAddress
import com.innomalist.taxi.rider.ui.RiderBaseActivity
import com.tylersuehr.esr.ContentItemLoadingStateFactory
import com.tylersuehr.esr.EmptyStateRecyclerView
import com.tylersuehr.esr.ImageTextStateDisplay

class AddressesActivity : RiderBaseActivity(), OnEditAddressInteractionListener {
    lateinit var binding: ActivityAddressesBinding
    var currentLocation: LatLng? = null
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = DataBindingUtil.setContentView(this@AddressesActivity, R.layout.activity_addresses)
        if(intent.getDoubleArrayExtra("currentLocation") != null) {
            currentLocation = doubleArrayToLatLng(intent.getDoubleArrayExtra("currentLocation")!!)
        }
        initializeToolbar(getString(R.string.drawer_favorite_locations))
        binding.recyclerView.setStateDisplay(EmptyStateRecyclerView.STATE_LOADING, ContentItemLoadingStateFactory.newListLoadingState(this))
        binding.recyclerView.setStateDisplay(EmptyStateRecyclerView.STATE_EMPTY, ImageTextStateDisplay(this, com.innomalist.taxi.common.R.drawable.empty_state, getString(com.innomalist.taxi.common.R.string.empty_state_title), getString(com.innomalist.taxi.common.R.string.empty_state_message)))
        binding.recyclerView.setStateDisplay(EmptyStateRecyclerView.STATE_ERROR, ImageTextStateDisplay(this, com.innomalist.taxi.common.R.drawable.empty_state, getString(com.innomalist.taxi.common.R.string.empty_state_error_title), getString(com.innomalist.taxi.common.R.string.empty_state_error_message)))
        binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_LOADING)
        refreshAddresses()

    }

    fun refreshAddresses() {
        GetAddresses().executeArray<Address> {
            when(it) {
                is RemoteResponse.Success -> {
                    if (it.body.isEmpty()) {
                        binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_EMPTY)
                        return@executeArray
                    }
                    binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_OK)
                    val addressesRecyclerViewAdapter = AddressesRecyclerViewAdapter(it.body, object : OnAddressItemInteractionListener {
                        override fun onEdit(address: Address) {
                            showEditAddressDialog(address)
                        }

                        override fun onDelete(address: Address) {
                            show(this@AddressesActivity, getString(R.string.question_delete_address), AlertDialogBuilder.DialogButton.OK_CANCEL, AlertDialogEvent { result: DialogResult ->
                                run {
                                    if (result == DialogResult.OK) {
                                        DeleteAddress(address.id).execute<EmptyClass> {
                                            refreshAddresses()
                                        }
                                    }
                                }
                            })
                        }
                    })
                    val llm = LinearLayoutManager(this@AddressesActivity)
                    llm.orientation = LinearLayoutManager.VERTICAL
                    llm.isAutoMeasureEnabled = false
                    binding.recyclerView.setHasFixedSize(true)
                    binding.recyclerView.layoutManager = llm
                    binding.recyclerView.adapter = addressesRecyclerViewAdapter
                }

                is RemoteResponse.Error -> {
                    it.error.showAlert(this)
                }
            }
        }
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        menuInflater.inflate(R.menu.actionbar_add, menu)
        return super.onCreateOptionsMenu(menu)
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        val address = Address()
        address.location = currentLocation
        showEditAddressDialog(address)
        return super.onOptionsItemSelected(item)
    }

    fun showEditAddressDialog(address: Address) {
        val fm = supportFragmentManager
        val editNameDialogFragment: EditAddressDialog = EditAddressDialog.newInstance(address)
        editNameDialogFragment.show(fm, "fragment_edit_name")
    }

    override fun onSaveButtonClicked(address: Address) {
        UpsertAddress(address).execute<EmptyClass> {
            when(it) {
                is RemoteResponse.Success -> {
                    refreshAddresses()
                }

                is RemoteResponse.Error -> {
                    it.error.showAlert(this)
                }
            }
        }
    }

    private interface EditAddressResult {
        fun onAddressEdited(address: Address)
    }
}
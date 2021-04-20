package com.innomalist.taxi.common.activities.travels

import android.os.Bundle
import androidx.databinding.DataBindingUtil
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.innomalist.taxi.common.R
import com.innomalist.taxi.common.activities.travels.adapters.TravelsRecyclerViewAdapter
import com.innomalist.taxi.common.activities.travels.adapters.TravelsRecyclerViewAdapter.OnTravelItemInteractionListener
import com.innomalist.taxi.common.activities.travels.fragments.Complaint
import com.innomalist.taxi.common.activities.travels.fragments.WriteComplaintDialog
import com.innomalist.taxi.common.activities.travels.fragments.WriteComplaintDialog.OnWriteComplaintInteractionListener
import com.innomalist.taxi.common.components.BaseActivity
import com.innomalist.taxi.common.databinding.ActivityTravelsBinding
import com.innomalist.taxi.common.interfaces.AlertDialogEvent
import com.innomalist.taxi.common.models.Request
import com.innomalist.taxi.common.networking.socket.GetRequestHistory
import com.innomalist.taxi.common.networking.socket.HideHistoryItem
import com.innomalist.taxi.common.networking.socket.WriteComplaint
import com.innomalist.taxi.common.networking.socket.interfaces.EmptyClass
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import com.innomalist.taxi.common.utils.AlertDialogBuilder
import com.innomalist.taxi.common.utils.AlertDialogBuilder.DialogResult
import com.innomalist.taxi.common.utils.AlerterHelper
import com.tylersuehr.esr.ContentItemLoadingStateFactory
import com.tylersuehr.esr.EmptyStateRecyclerView
import com.tylersuehr.esr.ImageTextStateDisplay

class TravelsActivity : BaseActivity(), OnWriteComplaintInteractionListener {
    private var lastSelectedTravelId: Long = 0
    lateinit var binding: ActivityTravelsBinding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = DataBindingUtil.setContentView(this@TravelsActivity, R.layout.activity_travels)
        initializeToolbar(getString(R.string.drawer_travels))
        binding.recyclerView.setStateDisplay(EmptyStateRecyclerView.STATE_LOADING, ContentItemLoadingStateFactory.newListLoadingState(this))
        binding.recyclerView.setStateDisplay(EmptyStateRecyclerView.STATE_EMPTY, ImageTextStateDisplay(this, R.drawable.empty_state, getString(R.string.empty_state_title), getString(R.string.empty_state_message)))
        binding.recyclerView.setStateDisplay(EmptyStateRecyclerView.STATE_ERROR, ImageTextStateDisplay(this, R.drawable.empty_state, getString(R.string.empty_state_error_title), getString(R.string.empty_state_error_message)))
        binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_LOADING)
        refreshRequests()

    }

    fun refreshRequests() {
        binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_LOADING)
        GetRequestHistory().executeArray<Request> {
            when(it) {
                is RemoteResponse.Success -> {
                    if (it.body.isEmpty()) {
                        binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_EMPTY)
                        return@executeArray
                    }
                    binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_OK)
                    loadList(it.body)
                }

                is RemoteResponse.Error -> {
                    binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_ERROR)
                }
            }
        }
    }

    private fun loadList(requests: ArrayList<Request>?) {
        if (requests == null) return
        val adapter = TravelsRecyclerViewAdapter(this@TravelsActivity, requests, object : OnTravelItemInteractionListener {
            override fun onHideTravel(request: Request) {
                AlertDialogBuilder.show(this@TravelsActivity, getString(R.string.question_hide_travel), AlertDialogBuilder.DialogButton.OK_CANCEL, AlertDialogEvent { result: DialogResult ->
                    if (result != DialogResult.OK) {
                        return@AlertDialogEvent
                    }
                    HideHistoryItem(request.id!!).execute<EmptyClass> {
                        AlerterHelper.showInfo(this@TravelsActivity, getString(R.string.info_travel_hidden))
                        refreshRequests()
                    }
                })
            }

            override fun onWriteComplaint(request: Request) {
                lastSelectedTravelId = request.id!!.toLong()
                val fm = supportFragmentManager
                WriteComplaintDialog().show(fm, "fragment_complaint")
            }
        })
        val llm = LinearLayoutManager(this@TravelsActivity)
        llm.orientation = RecyclerView.VERTICAL
        binding.recyclerView.setHasFixedSize(true)
        binding.recyclerView.layoutManager = llm
        binding.recyclerView.adapter = adapter
    }

    override fun onSaveComplaintClicked(event: Complaint) {
        WriteComplaint(lastSelectedTravelId, event.subject, event.content).execute<EmptyClass> {
            AlerterHelper.showInfo(this@TravelsActivity, getString(R.string.message_complaint_sent))
            refreshRequests()
        }
    }
}
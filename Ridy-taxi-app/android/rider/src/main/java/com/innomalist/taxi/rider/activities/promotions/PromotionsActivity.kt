package com.innomalist.taxi.rider.activities.promotions

import android.os.Bundle
import androidx.databinding.DataBindingUtil
import androidx.recyclerview.widget.LinearLayoutManager
import com.innomalist.taxi.common.components.BaseActivity
import com.innomalist.taxi.common.models.Promotion
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import com.innomalist.taxi.common.utils.AlerterHelper
import com.innomalist.taxi.rider.R
import com.innomalist.taxi.rider.activities.promotions.adapters.PromotionsRecyclerViewAdapter
import com.innomalist.taxi.rider.databinding.ActivityPromotionsBinding
import com.innomalist.taxi.rider.networking.socket.GetPromotions
import com.tylersuehr.esr.ContentItemLoadingStateFactory
import com.tylersuehr.esr.EmptyStateRecyclerView
import com.tylersuehr.esr.ImageTextStateDisplay

class PromotionsActivity : BaseActivity() {
    lateinit var binding: ActivityPromotionsBinding
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = DataBindingUtil.setContentView(this@PromotionsActivity, R.layout.activity_promotions)
        initializeToolbar(getString(R.string.drawer_promotions))
        binding.recyclerView.setStateDisplay(EmptyStateRecyclerView.STATE_LOADING, ContentItemLoadingStateFactory.newListLoadingState(this))
        binding.recyclerView.setStateDisplay(EmptyStateRecyclerView.STATE_EMPTY, ImageTextStateDisplay(this, com.innomalist.taxi.common.R.drawable.empty_state, getString(R.string.empty_state_title), getString(R.string.empty_state_message)))
        binding.recyclerView.setStateDisplay(EmptyStateRecyclerView.STATE_ERROR, ImageTextStateDisplay(this, com.innomalist.taxi.common.R.drawable.empty_state, getString(R.string.empty_state_error_title), getString(R.string.empty_state_error_message)))
        binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_LOADING)
        refreshPromotions()
    }

    private fun refreshPromotions() {
        GetPromotions().executeArray<Promotion> {
            when(it) {
                is RemoteResponse.Success -> {
                    if (it.body.isEmpty()) {
                        binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_EMPTY)
                        return@executeArray
                    }
                    binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_OK)
                    val promotionsRecyclerViewAdapter = PromotionsRecyclerViewAdapter(it.body)
                    val llm = LinearLayoutManager(this@PromotionsActivity)
                    llm.orientation = LinearLayoutManager.VERTICAL
                    binding.recyclerView.setHasFixedSize(true)
                    binding.recyclerView.layoutManager = llm
                    binding.recyclerView.adapter = promotionsRecyclerViewAdapter
                }

                is RemoteResponse.Error -> {
                    AlerterHelper.showError(this, it.error.message)
                }
            }

        }

    }
}
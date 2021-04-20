package com.innomalist.taxi.common.activities.transactions

import android.os.Bundle
import androidx.databinding.DataBindingUtil
import androidx.recyclerview.widget.LinearLayoutManager
import com.innomalist.taxi.common.R
import com.innomalist.taxi.common.activities.transactions.adapters.TransactionsRecyclerViewAdapter
import com.innomalist.taxi.common.components.BaseActivity
import com.innomalist.taxi.common.databinding.ActivityTransactionsBinding
import com.innomalist.taxi.common.models.Transaction
import com.innomalist.taxi.common.networking.socket.GetTransactions
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import com.tylersuehr.esr.ContentItemLoadingStateFactory
import com.tylersuehr.esr.EmptyStateRecyclerView
import com.tylersuehr.esr.ImageTextStateDisplay

class TransactionsActivity : BaseActivity() {
    lateinit var binding: ActivityTransactionsBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = DataBindingUtil.setContentView(this@TransactionsActivity, R.layout.activity_transactions)
        initializeToolbar(R.string.drawer_transactions)
        binding.recyclerView.setStateDisplay(EmptyStateRecyclerView.STATE_LOADING, ContentItemLoadingStateFactory.newListLoadingState(this))
        binding.recyclerView.setStateDisplay(EmptyStateRecyclerView.STATE_EMPTY, ImageTextStateDisplay(this, R.drawable.empty_state, getString(R.string.empty_state_title), getString(R.string.empty_state_message)))
        binding.recyclerView.setStateDisplay(EmptyStateRecyclerView.STATE_ERROR, ImageTextStateDisplay(this, R.drawable.empty_state, getString(R.string.empty_state_error_title), getString(R.string.empty_state_error_message)))
        binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_LOADING)
        GetTransactions().executeArray<Transaction> {
            when(it) {
                is RemoteResponse.Success -> {
                    if (it.body.isEmpty()) {
                        binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_EMPTY)
                        return@executeArray
                    }
                    binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_OK)
                    val transactionsRecyclerViewAdapter = TransactionsRecyclerViewAdapter(it.body)
                    val llm = LinearLayoutManager(this@TransactionsActivity)
                    llm.orientation = LinearLayoutManager.VERTICAL
                    binding.recyclerView.setHasFixedSize(true)
                    binding.recyclerView.layoutManager = llm
                    binding.recyclerView.adapter = transactionsRecyclerViewAdapter
                }

                is RemoteResponse.Error -> {
                    binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_ERROR)
                }
            }

        }
    }
}
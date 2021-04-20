package com.innomalist.taxi.common.activities.transactions.adapters

import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.innomalist.taxi.common.databinding.ItemTransactionBinding
import com.innomalist.taxi.common.models.Transaction
import java.text.NumberFormat
import java.util.*

class TransactionsRecyclerViewAdapter(private val transactions: List<Transaction>) : RecyclerView.Adapter<TransactionsRecyclerViewAdapter.ViewHolder>() {
    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val layoutInflater = LayoutInflater.from(parent.context)
        val itemBinding = ItemTransactionBinding.inflate(layoutInflater, parent, false)
        return ViewHolder(itemBinding)
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val transaction = transactions[position]
        holder.bind(transaction)
    }

    override fun getItemCount(): Int {
        return transactions.size
    }

    class ViewHolder(var binding: ItemTransactionBinding) : RecyclerView.ViewHolder(binding.root) {
        fun bind(transaction: Transaction?) {
            binding.item = transaction
            val format: NumberFormat = NumberFormat.getCurrencyInstance()
            format.currency = Currency.getInstance(transaction!!.currency)
            binding.textAmount.text = format.format(transaction.amount)
            binding.executePendingBindings()
        }

    }

}
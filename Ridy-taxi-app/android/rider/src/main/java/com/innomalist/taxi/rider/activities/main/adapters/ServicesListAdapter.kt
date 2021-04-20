package com.innomalist.taxi.rider.activities.main.adapters

import android.view.LayoutInflater
import android.view.MotionEvent
import android.view.ViewGroup
import androidx.recyclerview.selection.ItemDetailsLookup
import androidx.recyclerview.selection.SelectionTracker
import androidx.recyclerview.widget.RecyclerView
import com.innomalist.taxi.common.models.Service
import com.innomalist.taxi.rider.databinding.ItemServiceBinding
import java.text.NumberFormat
import java.util.*

class ServicesListAdapter(private val services: List<Service>,
                          private var distance: Int = 0,
                          private var duration: Int = 0,
                          private var currency: String) : RecyclerView.Adapter<ServicesListAdapter.ViewHolder>() {
    var tracker: SelectionTracker<Long>? = null

    init {
        setHasStableIds(true)
    }

    class ViewHolder(var binding: ItemServiceBinding) : RecyclerView.ViewHolder(binding.root) {
        fun bind(service: Service, distance: Int, duration: Int, currency: String, selected: Boolean = false) {
            binding.item = service
            binding.rootView.alpha = if (selected) 1.0f else 0.7f
            updatePrice(service, distance, duration, currency)
            binding.executePendingBindings()
        }

        fun getItemDetails(): ItemDetailsLookup.ItemDetails<Long> = object : ItemDetailsLookup.ItemDetails<Long>() {
            override fun getSelectionKey(): Long? {
                return itemId
            }

            override fun getPosition(): Int {
                return adapterPosition
            }

            override fun inSelectionHotspot(e: MotionEvent): Boolean {
                return true
            }
        }

        private fun updatePrice(service: Service, distance: Int, duration: Int, currency: String) {
            val formatter: NumberFormat = NumberFormat.getCurrencyInstance()
            formatter.currency = Currency.getInstance(currency)
            val cost: Double = service.cost!!
            when (service.feeEstimationMode) {
                Service.FeeEstimationMode.Disabled -> binding.textCost.text = "-"
                Service.FeeEstimationMode.Static -> binding.textCost.text = formatter.format(cost)
                Service.FeeEstimationMode.Dynamic -> binding.textCost.text = "~${formatter.format(cost)}"
                Service.FeeEstimationMode.Ranged -> {
                    val cMinus = cost - (cost * (service.rangeMinusPercent ?: 0) / 100)
                    val cPlus = cost + (cost * (service.rangePlusPercent ?: 0) / 100)
                    binding.textCost.text = "${formatter.format(cMinus)}~${formatter.format(cPlus)}"
                }
                Service.FeeEstimationMode.RangedStrict -> {
                    val cMinus = cost - (cost * (service.rangeMinusPercent ?: 0) / 100)
                    val cPlus = cost + (cost * (service.rangePlusPercent ?: 0) / 100)
                    binding.textCost.text = "${formatter.format(cMinus)}-${formatter.format(cPlus)}"
                }
            }
        }
    }

    override fun getItemId(position: Int): Long {
        return services[position].id
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val layoutInflater = LayoutInflater.from(parent.context)
        val itemBinding = ItemServiceBinding.inflate(layoutInflater, parent, false)
        itemBinding.textCost.isSelected = true
        return ViewHolder(itemBinding)
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val service = services[position]
        tracker?.let {
            holder.bind(service, distance, duration, currency ,it.isSelected(service.id))
        }
    }

    override fun getItemCount(): Int {
        return services.size
    }

}
package com.innomalist.taxi.rider.activities.coupon.adapters

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.innomalist.taxi.common.models.Coupon
import com.innomalist.taxi.rider.databinding.ItemCouponBinding

class CouponsRecyclerViewAdapter(private val coupons: List<Coupon?>?, isEditMode: Boolean, val listener: OnCouponItemInteractionListener) : RecyclerView.Adapter<CouponsRecyclerViewAdapter.ViewHolder>() {
    var isEditMode = false
    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val layoutInflater = LayoutInflater.from(parent.context)
        val itemBinding = ItemCouponBinding.inflate(layoutInflater, parent, false)
        if (!isEditMode) itemBinding.buttonSelect.visibility = View.INVISIBLE
        return ViewHolder(itemBinding)
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val coupon = coupons!![position]!!
        holder.bind(coupon, listener)
    }

    override fun getItemCount(): Int {
        return coupons!!.size
    }

    interface OnCouponItemInteractionListener {
        fun onSelect(coupon: Coupon)
    }

    class ViewHolder(var binding: ItemCouponBinding) : RecyclerView.ViewHolder(binding.root) {
        fun bind(coupon: Coupon, listener: OnCouponItemInteractionListener) {
            binding.item = coupon
            binding.buttonSelect.setOnClickListener { listener.onSelect(coupon) }
            binding.executePendingBindings()
        }

    }

    init {
        this.isEditMode = isEditMode
    }
}
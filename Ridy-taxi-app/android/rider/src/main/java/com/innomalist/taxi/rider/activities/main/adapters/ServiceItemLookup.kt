package com.innomalist.taxi.rider.activities.main.adapters

import android.view.MotionEvent
import androidx.recyclerview.selection.ItemDetailsLookup
import androidx.recyclerview.widget.RecyclerView

class ServiceItemLookup(private val recyclerView: RecyclerView) :
        ItemDetailsLookup<Long>() {
    override fun getItemDetails(event: MotionEvent): ItemDetails<Long>? {
        val view = recyclerView.findChildViewUnder(event.x, event.y)
        if (view != null) {
            return (recyclerView.getChildViewHolder(view) as ServicesListAdapter.ViewHolder)
                    .getItemDetails()
        }
        return null
    }
}
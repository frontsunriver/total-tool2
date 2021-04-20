package com.innomalist.taxi.rider.ui

import android.content.Context
import android.graphics.Rect
import android.util.TypedValue
import android.view.View
import androidx.recyclerview.widget.RecyclerView
import androidx.recyclerview.widget.RecyclerView.ItemDecoration

class SmartPaddingForLinearSnapHelper(context: Context, screenWidth: Int) : ItemDecoration() {
    private var PADDING_IN_DIPS = 8
    private val mPadding: Int
    override fun getItemOffsets(outRect: Rect, view: View, parent: RecyclerView, state: RecyclerView.State) {
        val itemPosition = parent.getChildAdapterPosition(view)
        if (itemPosition == RecyclerView.NO_POSITION) {
            return
        }
        if (itemPosition == 0) {
            outRect.left = mPadding
        }
        val adapter = parent.adapter
        if (adapter != null && itemPosition == adapter.itemCount - 1) {
            outRect.right = mPadding
        }
    }

    init {
        val metrics = context.resources.displayMetrics
        PADDING_IN_DIPS = screenWidth / 2
        mPadding = TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, PADDING_IN_DIPS.toFloat(), metrics).toInt()
    }
}
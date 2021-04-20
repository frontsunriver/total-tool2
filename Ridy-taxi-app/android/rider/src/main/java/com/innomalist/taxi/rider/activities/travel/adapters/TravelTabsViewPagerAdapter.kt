package com.innomalist.taxi.rider.activities.travel.adapters

import android.content.Context
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentManager
import androidx.fragment.app.FragmentPagerAdapter
import com.innomalist.taxi.common.R
import com.innomalist.taxi.common.models.Request
import com.innomalist.taxi.rider.activities.travel.fragments.TabDriverInfoFragment
import com.innomalist.taxi.rider.activities.travel.fragments.TabReviewFragment
import com.innomalist.taxi.rider.activities.travel.fragments.TabStatisticsFragment
import java.util.*

class TravelTabsViewPagerAdapter(manager: FragmentManager, private val context: Context, private val request: Request) : FragmentPagerAdapter(manager, BEHAVIOR_RESUME_ONLY_CURRENT_FRAGMENT) {
    private val arrayIndexes = ArrayList(listOf(0, 1, 2))
    var statisticsFragment: TabStatisticsFragment? = null
    override fun getItem(position: Int): Fragment {
        return when (position) {
            0 -> TabDriverInfoFragment.newInstance(request)
            1 -> {
                statisticsFragment = TabStatisticsFragment.newInstance()
                statisticsFragment!!
            }
            2 -> TabReviewFragment()
            else -> TabDriverInfoFragment.newInstance(request)
        }
    }

    override fun getCount(): Int {
        return arrayIndexes.size
    }

    override fun getPageTitle(position: Int): CharSequence? {
        return when (position) {
            0 -> context.getString(R.string.tab_driver_info)
            1 -> context.getString(R.string.tab_statistics)
            2 -> context.getString(R.string.tab_review)
            else -> context.getString(R.string.tab_driver_info)
        }
    }

    fun deletePage(position: Int) { // Remove the corresponding item in the data set
        arrayIndexes.removeAt(position)
        // Notify the adapter that the data set is changed
        notifyDataSetChanged()
    }

}
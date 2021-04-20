package com.innomalist.taxi.driver.activities.main.adapters

import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentManager
import com.innomalist.taxi.common.models.Request
import com.innomalist.taxi.common.ui.ArrayFragmentPagerAdapter
import com.innomalist.taxi.driver.activities.main.fragments.RequestFragment
import java.util.*

class RequestsFragmentPagerAdapter(fm: FragmentManager?, requests: ArrayList<Request>) : ArrayFragmentPagerAdapter<Request?>(fm, requests) {
    fun getPositionWithTravelId(travelId: Long): Int {
        for (i in 0 until count) {
            if (getItem(i)!!.id == travelId) {
                return i
            }
        }
        return -1
    }

    override fun getFragment(item: Request?, position: Int): Fragment? { // return RequestCardFragment.newInstance(item);
        return RequestFragment.newInstance(item)
    }
}
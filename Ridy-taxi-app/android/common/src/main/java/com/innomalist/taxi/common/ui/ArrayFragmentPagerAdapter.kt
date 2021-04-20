package com.innomalist.taxi.common.ui

import android.annotation.SuppressLint
import android.util.Log
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentManager
import androidx.fragment.app.FragmentTransaction

/**
 * ViewPager adapter that handles Fragments and items.
 * You can use this adapter if there is a few number of pages and no need to save states (or implement by yourself).
 * Subclasses of this class just need to implement getFragment() and return a fragment associated with position and item.
 * @param <T> item type
</T> */
abstract class ArrayFragmentPagerAdapter<T> : ArrayPagerAdapter<T> {
    private var fragmentManager: FragmentManager? = null
    private var currentTransaction: FragmentTransaction? = null
    private var currentPrimaryItem: Fragment? = null

    constructor(fm: FragmentManager?, items: List<T>?) : super(items) {
        fragmentManager = fm
    }

    /**
     * Return the Fragment associated with a specified position and item.
     * @param item item of this page.
     * @param position position of this page.
     * @return fragment that represent this page.
     */
    abstract fun getFragment(item: T?, position: Int): Fragment?

    @SuppressLint("CommitTransaction")
    override fun instantiateItem(container: ViewGroup, position: Int): Any {
        if (currentTransaction == null) {
            currentTransaction = fragmentManager!!.beginTransaction()
        }
        // Do we already have this fragment?
        val item = getItemWithId(position)
        // Do we already have this fragment?
        val name = makeFragmentName(item.id)
        var fragment = fragmentManager!!.findFragmentByTag(name)
        if (fragment != null) {
            if (DEBUG) Log.d(TAG, "Attaching item #$item: f=$fragment")
            currentTransaction!!.attach(fragment)
        } else {
            fragment = getFragment(item.item, position)
            if (DEBUG) Log.d(TAG, "Adding item #$item: f=$fragment")
            currentTransaction!!.add(container.id, fragment!!,
                    makeFragmentName(item.id))
        }
        if (fragment !== currentPrimaryItem) {
            fragment!!.setMenuVisibility(false)
            fragment.userVisibleHint = false
        }
        return super.instantiateItem(container, position)
    }

    @SuppressLint("CommitTransaction")
    override fun destroyItem(container: ViewGroup, position: Int, `object`: Any) {
        if (currentTransaction == null) {
            currentTransaction = fragmentManager!!.beginTransaction()
        }
        val name = makeFragmentName((`object` as IdentifiedItem<*>).id)
        val f = fragmentManager!!.findFragmentByTag(name)
        if (f != null) {
            if (DEBUG) Log.d(TAG, "Detaching item #" + getItemId(position) + ": f=" + `object`
                    + " v=" + f.view)
            currentTransaction!!.detach(f)
        }
    }

    @SuppressLint("CommitTransaction")
    @Throws(IndexOutOfBoundsException::class)
    override fun remove(position: Int) {
        if (currentTransaction == null) {
            currentTransaction = fragmentManager!!.beginTransaction()
        }
        val name = makeFragmentName(getItemId(position))
        val f = fragmentManager!!.findFragmentByTag(name)
        if (f != null) {
            if (DEBUG) Log.d(TAG, "Removing item #" + getItemId(position) + ": f=" + f
                    + " v=" + f.view)
            currentTransaction!!.remove(f)
        }
        super.remove(position)
    }

    @SuppressLint("CommitTransaction")
    override fun clear() {
        if (currentTransaction == null) {
            currentTransaction = fragmentManager!!.beginTransaction()
        }
        val fragments = fragmentManager!!.fragments
        if (fragments != null) {
            for (fragment in fragments) {
                if (fragment != null) {
                    currentTransaction!!.remove(fragment)
                }
            }
        }
        super.clear()
    }

    override fun setPrimaryItem(container: ViewGroup, position: Int, `object`: Any) {
        val fragment = fragmentManager!!.findFragmentByTag(makeFragmentName(getItemId(position)))
        if (fragment !== currentPrimaryItem) {
            if (currentPrimaryItem != null) {
                currentPrimaryItem!!.setMenuVisibility(false)
                currentPrimaryItem!!.userVisibleHint = false
            }
            if (fragment != null) {
                fragment.setMenuVisibility(true)
                fragment.userVisibleHint = true
            }
            currentPrimaryItem = fragment
        }
    }

    override fun finishUpdate(container: ViewGroup) {
        if (currentTransaction != null) {
            currentTransaction!!.commitAllowingStateLoss()
            currentTransaction = null
            fragmentManager!!.executePendingTransactions()
        }
    }

    override fun isViewFromObject(view: View, `object`: Any): Boolean {
        for (fragment in fragmentManager!!.fragments) {
            if (fragment != null) {
                val v = fragment.view
                if (v != null && v === view && makeFragmentName((`object` as IdentifiedItem<*>).id) == fragment.tag) {
                    return true
                }
            }
        }
        return false
    }

    private fun getItemId(position: Int): Long {
        return if (count > position) {
            getItemWithId(position).id
        } else -1
    }

    companion object {
        private const val TAG = "FragmentPagerAdapter"
        private const val DEBUG = true
        private fun makeFragmentName(id: Long): String {
            return "android:switcher:$id"
        }
    }
}
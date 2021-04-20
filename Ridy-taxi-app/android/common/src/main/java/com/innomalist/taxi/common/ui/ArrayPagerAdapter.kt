package com.innomalist.taxi.common.ui

import android.util.SparseBooleanArray
import android.view.ViewGroup
import androidx.viewpager.widget.PagerAdapter
import java.util.*

/**
 * ViewPager adapter that contain items passed from outside.
 * This is base class of other ArrayPagerAdapters in this library.
 *
 * @param <T> item type
</T> */
abstract class ArrayPagerAdapter<T> @JvmOverloads constructor(items: List<T>? = ArrayList()) : PagerAdapter() {
    var itemsWithId: ArrayList<IdentifiedItem<T>>
        private set
    private val lock = Any()
    private val identifiedItemFactory: IdentifiedItemFactory<T>

    @SafeVarargs
    constructor(vararg items: T) : this(ArrayList<T>(Arrays.asList(*items))) {
    }

    override fun instantiateItem(container: ViewGroup, position: Int): Any {
        return itemsWithId[position]
    }

    /**
     * Adds the specified item at the end of the array.
     *
     * @param item The item to add at the end of the array.
     */
    fun add(item: T) {
        synchronized(lock) { itemsWithId.add(identifiedItemFactory.create(item)) }
        notifyDataSetChanged()
    }

    /**
     * Adds the specified item at the specified position of the array.
     *
     * @param item The item to add at the specified position of the array.
     */
    fun add(index: Int, item: T) {
        synchronized(lock) { itemsWithId.add(index, identifiedItemFactory.create(item)) }
        itemPositionChangeChecked = SparseBooleanArray(itemsWithId.size)
        notifyDataSetChanged()
    }

    /**
     * Adds the specified items at the end of the array.
     *
     * @param items The items to add at the end of the array.
     */
    fun addAll(vararg items: T) {
        synchronized(lock) { itemsWithId.addAll(identifiedItemFactory.createList(*items)) }
        itemPositionChangeChecked = SparseBooleanArray(itemsWithId.size)
        notifyDataSetChanged()
    }

    /**
     * Adds the specified items at the end of the array.
     *
     * @param items The items to add at the end of the array.
     */
    fun addAll(items: List<T>?) {
        synchronized(lock) { itemsWithId.addAll(identifiedItemFactory.createList(items!!)) }
        itemPositionChangeChecked = SparseBooleanArray(itemsWithId.size)
        notifyDataSetChanged()
    }

    /**
     * Removes the specified item from the array.
     *
     * @param position The item position to be removed
     * @return true if this items was modified by this operation, false otherwise.
     * @throws IndexOutOfBoundsException if position < 0 || position >= getCount()
     */
    @Throws(IndexOutOfBoundsException::class)
    open fun remove(position: Int) {
        synchronized(lock) { itemsWithId.removeAt(position) }
        itemPositionChangeChecked = SparseBooleanArray(itemsWithId.size)
        notifyDataSetChanged()
    }

    /**
     * Remove all elements from the list.
     */
    open fun clear() {
        synchronized(lock) { itemsWithId.clear() }
        itemPositionChangeChecked = SparseBooleanArray(itemsWithId.size)
        notifyDataSetChanged()
    }

    /**
     * Get the data item associated with the specified position in the data set.
     *
     * @param position Position of the item whose data we want within the adapter's
     * data set.
     * @return The data at the specified position.
     */
    fun getItem(position: Int): T {
        return itemsWithId[position].item
    }

    /**
     * Returns the position of the specified item in the array.
     *
     * @param item The item to retrieve the position of.
     * @return The position of the specified item.
     */
    fun getPosition(item: T): Int {
        for (i in itemsWithId.indices) {
            if (itemsWithId[i].item === item) {
                return i
            }
        }
        return -1
    }

    /**
     * {@inheritDoc}
     */
    override fun getItemPosition(item: Any): Int {
        return if (!itemsWithId.contains(item)) {
            POSITION_NONE
        } else if (itemPositionChangeChecked.size() != itemsWithId.size) {
            val newPos = itemsWithId.indexOf(item)
            val ret = if (itemPositionChangeChecked[newPos]) POSITION_UNCHANGED else newPos
            itemPositionChangeChecked.put(newPos, true)
            ret
        } else {
            POSITION_UNCHANGED
        }
    }

    /**
     * {@inheritDoc}
     */
    override fun getCount(): Int {
        return itemsWithId.size
    }

    fun getItemWithId(position: Int): IdentifiedItem<T> {
        return itemsWithId[position]
    }

    fun getItems(): ArrayList<T> {
        val list = ArrayList<T>()
        for (item in itemsWithId) {
            list.add(item.item)
        }
        return list
    }

    fun setItems(items: List<T>?) {
        itemsWithId = identifiedItemFactory.createList(items!!)
        notifyDataSetChanged()
    }

    private var itemPositionChangeChecked = SparseBooleanArray()

    internal class IdentifiedItemFactory<T>(private var lastId: Long) {
        fun create(item: T): IdentifiedItem<T> {
            return IdentifiedItem(lastId++, item)
        }

        fun createList(items: List<T>): ArrayList<IdentifiedItem<T>> {
            val list: ArrayList<IdentifiedItem<T>> = ArrayList()
            for (item in items) {
                list.add(create(item))
            }
            return list
        }

        @SafeVarargs
        fun createList(vararg items: T): Collection<IdentifiedItem<T>> {
            return createList(ArrayList(Arrays.asList(*items)))
        }

    }

    class IdentifiedItem<T>(var id: Long, var item: T) {
        override fun toString(): String {
            return "IdentifiedItem{" +
                    "id=" + id +
                    ", item=" + item +
                    '}'
        }

    }

    init {
        identifiedItemFactory = IdentifiedItemFactory(0)
        itemsWithId = identifiedItemFactory.createList(items!!)
    }
}
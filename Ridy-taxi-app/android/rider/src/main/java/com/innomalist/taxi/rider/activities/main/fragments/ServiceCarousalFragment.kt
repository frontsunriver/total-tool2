package com.innomalist.taxi.rider.activities.main.fragments

import android.content.Context
import android.os.Bundle
import android.view.Gravity
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.recyclerview.selection.SelectionPredicates
import androidx.recyclerview.selection.SelectionTracker
import androidx.recyclerview.selection.StableIdKeyProvider
import androidx.recyclerview.selection.StorageStrategy
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import androidx.recyclerview.widget.SnapHelper
import com.innomalist.taxi.common.models.Service
import com.innomalist.taxi.common.utils.Adapters
import com.innomalist.taxi.rider.activities.main.adapters.ServiceItemLookup
import com.innomalist.taxi.rider.activities.main.adapters.ServicesListAdapter
import com.innomalist.taxi.rider.ui.gravitySnapHelper.GravitySnapHelper
import com.squareup.moshi.Types
import kotlin.collections.ArrayList

class ServiceCarousalFragment : Fragment() {
    private var services: List<Service>? = null
    private var distance: Int = 0
    private var duration: Int = 0
    private var currency: String = ""
    private var mListener: OnServicesCarousalFragmentListener? = null
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        if (arguments != null) {
            val str = requireArguments().getString(ARG_SERVICES)
            val type = Types.newParameterizedType(List::class.java, Service::class.java)
            val adapter = Adapters.moshi.adapter<List<Service>>(type)
            services = adapter.fromJson(str!!)
            distance = requireArguments().getInt(ARG_DISTANCE)
            duration = requireArguments().getInt(ARG_DURATION)
            currency = requireArguments().getString(ARG_CURRENCY, "")
        }
    }

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        val recyclerView = RecyclerView(requireContext())
        recyclerView.layoutManager = LinearLayoutManager(context, LinearLayoutManager.HORIZONTAL, false)
        val snapHelper: SnapHelper = GravitySnapHelper(Gravity.START)
        snapHelper.attachToRecyclerView(recyclerView)
        val adapter = ServicesListAdapter(services ?: ArrayList(), distance, duration, currency)
        recyclerView.adapter = adapter
        val tracker = SelectionTracker.Builder(
                "mySelection",
                recyclerView,
                StableIdKeyProvider(recyclerView),
                ServiceItemLookup(recyclerView),
                StorageStrategy.createLongStorage()
        ).withSelectionPredicate(SelectionPredicates.createSelectSingleAnything()).build()
        tracker.addObserver(object : SelectionTracker.SelectionObserver<Long>() {
            override fun onSelectionChanged() {
                if (tracker.hasSelection()) {
                    mListener?.onServiceSelected(services!!.first { it.id == tracker.selection.first() }, currency)
                } else {
                    mListener?.onServiceSelected(null, currency)
                }
                super.onSelectionChanged()

            }
        })
        if (services!!.isNotEmpty())
            tracker.select(services!!.first().id)
        adapter.tracker = tracker
        return recyclerView
    }

    override fun onAttach(context: Context) {
        super.onAttach(context)
        mListener = if (context is OnServicesCarousalFragmentListener) {
            context
        } else {
            throw RuntimeException("$context must implement OnServicesCarousalFragmentListener")
        }
    }

    override fun onDetach() {
        super.onDetach()
        mListener = null
    }

    interface OnServicesCarousalFragmentListener {
        fun onServiceSelected(service: Service?, currency: String)
    }

    companion object {
        private const val ARG_SERVICES = "services"
        private const val ARG_CURRENCY = "currency"
        private const val ARG_DISTANCE = "distance"
        private const val ARG_DURATION = "duration"
        fun newInstance(services: List<Service>, distance: Int, duration: Int, currency: String): ServiceCarousalFragment {
            val fragment = ServiceCarousalFragment()
            val args = Bundle()
            val type = Types.newParameterizedType(List::class.java, Service::class.java)
            val adapter = Adapters.moshi.adapter<List<Service>>(type)
            val str = adapter.toJson(services)
            args.putString(ARG_SERVICES, str)
            args.putString(ARG_CURRENCY, currency)
            args.putInt(ARG_DISTANCE, distance)
            args.putInt(ARG_DURATION, duration)
            fragment.arguments = args
            return fragment
        }
    }
}
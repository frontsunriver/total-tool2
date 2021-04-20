package com.innomalist.taxi.rider.activities.travel.fragments

import android.annotation.SuppressLint
import android.content.Context
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.databinding.DataBindingUtil
import com.google.android.gms.maps.model.LatLng
import com.innomalist.taxi.common.components.BaseFragment
import com.innomalist.taxi.common.networking.socket.interfaces.SocketNetworkDispatcher
import com.innomalist.taxi.common.utils.TravelRepository
import com.innomalist.taxi.common.utils.TravelRepository.get
import com.innomalist.taxi.rider.R
import com.innomalist.taxi.rider.activities.travel.TravelActivity
import com.innomalist.taxi.rider.activities.travel.fragments.ReviewDialog.OnReviewFragmentInteractionListener
import com.innomalist.taxi.rider.databinding.FragmentTravelStatsBinding
import java.text.NumberFormat
import java.util.*

class TabStatisticsFragment : BaseFragment() {
    private var binding: FragmentTravelStatsBinding? = null

    @SuppressLint("CheckResult")
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        Timer().schedule(object : TimerTask() {
            override fun run() {
                val activity = activity ?: return
                activity.runOnUiThread {
                    val request = get(activity.applicationContext, TravelRepository.AppType.RIDER)
                    var timestamp: Long = 0
                    if (request.startTimestamp == null) {
                        if (request.etaPickup != null) {
                            timestamp = request.etaPickup!!
                        }
                    } else {
                        timestamp = request.startTimestamp!! + request.durationBest!! * 1000
                    }
                    val seconds = (timestamp - Date().time) / 1000
                    if(binding == null) {
                        return@runOnUiThread
                    }
                    if (seconds <= 0) binding!!.etaText.setText(R.string.eta_soon) else binding!!.etaText.text = String.format(Locale.getDefault(), "%02d:%02d", seconds / 60, seconds % 60)
                }
            }
        }, 0, 1000)
    }

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        binding = DataBindingUtil.inflate(inflater, R.layout.fragment_travel_stats, container, false)
        val request = get(context, TravelRepository.AppType.RIDER)
        val format: NumberFormat = NumberFormat.getCurrencyInstance()
        format.currency = Currency.getInstance(request.currency)
        binding!!.costText.text = format.format(request.costAfterCoupon)
        binding!!.applyCouponButton.setOnClickListener { (requireActivity() as TravelActivity).onApplyCouponClicked() }
        binding!!.chargeAccountButton.setOnClickListener { (requireActivity() as TravelActivity).onChargeAccountClicked() }
        return binding!!.root
    }

    fun onUpdatePrice(price: Double) {
        val request = get(context, TravelRepository.AppType.RIDER)
        val format: NumberFormat = NumberFormat.getCurrencyInstance()
        format.currency = Currency.getInstance(request.currency)
        binding?.costText?.text = format.format(price)
    }

    companion object {
        fun newInstance(): TabStatisticsFragment {
            return TabStatisticsFragment()
        }
    }
}
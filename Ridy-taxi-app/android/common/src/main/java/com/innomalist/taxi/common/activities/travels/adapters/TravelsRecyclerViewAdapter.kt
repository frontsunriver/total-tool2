package com.innomalist.taxi.common.activities.travels.adapters

import android.content.Context
import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.innomalist.taxi.common.databinding.ItemTravelBinding
import com.innomalist.taxi.common.models.Request
import com.innomalist.taxi.common.utils.DistanceFormatter
import java.text.NumberFormat
import java.text.SimpleDateFormat
import java.util.*

class TravelsRecyclerViewAdapter(private val context: Context, private val requests: List<Request>, val listener: OnTravelItemInteractionListener) : RecyclerView.Adapter<TravelsRecyclerViewAdapter.ViewHolder>() {
    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val layoutInflater = LayoutInflater.from(parent.context)
        val itemBinding = ItemTravelBinding.inflate(layoutInflater, parent, false)
        return ViewHolder(itemBinding)
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val request = requests[position]
        holder.bind(request, listener, context)
    }

    override fun getItemCount(): Int {
        return requests.size
    }

    interface OnTravelItemInteractionListener {
        fun onHideTravel(request: Request)
        fun onWriteComplaint(request: Request)
    }

    class ViewHolder(var binding: ItemTravelBinding) : RecyclerView.ViewHolder(binding.root) {
        fun bind(request: Request, listener: OnTravelItemInteractionListener, context: Context) {
            binding.item = request
            binding.buttonHideTravel.setOnClickListener { listener.onHideTravel(request) }
            binding.buttonComplaint.setOnClickListener { listener.onWriteComplaint(request) }
            binding.textFrom.isSelected = true
            binding.textTo.isSelected = true
            if (request.requestTimestamp != null) {
                val dateRequest = Date()
                dateRequest.time = request.requestTimestamp!!
                binding.textRequestDate.text = SimpleDateFormat("MMM d, yyyy", Locale.getDefault()).format(dateRequest)
                binding.textRequestTime.text = SimpleDateFormat("hh:mm aaa", Locale.getDefault()).format(dateRequest)
            }
            if (request.finishTimestamp != null) {
                val dateFinish = Date()
                dateFinish.time = request.finishTimestamp!!
                binding.textFinishDate.text = SimpleDateFormat("MMM d, yyyy", Locale.getDefault()).format(dateFinish)
                binding.textFinishTime.text = SimpleDateFormat("hh:mm aaa", Locale.getDefault()).format(dateFinish)
            }
            val format: NumberFormat = NumberFormat.getCurrencyInstance()
            format.currency = Currency.getInstance(request.currency)
            binding.textDetailsCost.text = format.format(request.costAfterCoupon)
            binding.textDetailsDistance.text = DistanceFormatter.format(request.distanceBest!!)
            /*var mapUrl = "https://maps.googleapis.com/maps/api/staticmap?size=600x400&language=${Locale.getDefault().displayLanguage}&${request.points.joinToString("&") { "markers=color:blue|label:${request.points.indexOf(it) + 1}|${it.latitude},${it.longitude}" }}&key=${context.getString(R.string.google_maps_key)}"
            if (request.log != null && request.log!!.isNotEmpty()) mapUrl += "&path=weight:3|color:orange|enc:" + request.log
            request.imageUrl = mapUrl*/
            binding.buttonComplaint.tag = request.id
            binding.buttonHideTravel.tag = request.id
            val res = context.resources
            if (request.status != null) {
                binding.textStatus.text = res.getString(request.status!!.localizedDescription)
            }
            binding.executePendingBindings()
        }

    }

}
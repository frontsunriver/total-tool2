package com.innomalist.taxi.rider.activities.travel.fragments

import android.app.Dialog
import android.content.Context
import android.content.DialogInterface
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.RatingBar
import android.widget.RatingBar.OnRatingBarChangeListener
import androidx.appcompat.app.AlertDialog
import androidx.databinding.DataBindingUtil
import androidx.fragment.app.DialogFragment
import com.innomalist.taxi.common.models.Review
import com.innomalist.taxi.rider.R
import com.innomalist.taxi.rider.databinding.FragmentReviewBinding

class ReviewDialog : DialogFragment() {
    lateinit var binding: FragmentReviewBinding
    private var mListener: OnReviewFragmentInteractionListener? = null

    private fun onCreateDialogView(inflater: LayoutInflater?, container: ViewGroup?): View {
        binding = DataBindingUtil.inflate(inflater!!, R.layout.fragment_review, container, false)
        return binding.root
    }

    override fun onCreateDialog(savedInstanceState: Bundle?): Dialog {
        val alertDialogBuilder = AlertDialog.Builder(requireContext())
        alertDialogBuilder.setTitle(R.string.review_dialog_title)
        val view = onCreateDialogView(requireActivity().layoutInflater, null)
        onViewCreated(view, null)
        alertDialogBuilder.setView(view)
        binding.ratingBar.onRatingBarChangeListener = OnRatingBarChangeListener { _: RatingBar?, _: Float, _: Boolean ->
            val dialog = dialog as AlertDialog?
            dialog!!.getButton(AlertDialog.BUTTON_POSITIVE).isEnabled = true
        }
        alertDialogBuilder.setPositiveButton(getString(R.string.alert_ok)) { _: DialogInterface?, _: Int -> mListener!!.onReviewTravelClicked(Review(binding.ratingBar.rating.toInt() * 20, if(binding.reviewText.text != null) binding.reviewText.text.toString() else "", 0)) }
        return alertDialogBuilder.create()
    }

    override fun onAttach(context: Context) {
        super.onAttach(context)
        mListener = if (context is OnReviewFragmentInteractionListener) {
            context
        } else {
            throw RuntimeException("$context must implement onEditAddressInteractionListener")
        }
    }

    override fun onResume() {
        super.onResume()
        val dialog = dialog as AlertDialog?
        dialog!!.getButton(AlertDialog.BUTTON_POSITIVE).isEnabled = false
    }

    override fun onDetach() {
        super.onDetach()
        mListener = null
    }

    interface OnReviewFragmentInteractionListener {
        fun onReviewTravelClicked(review: Review)
    }

    companion object {
        fun newInstance(): ReviewDialog { /*Bundle args = new Bundle();
        args.putSerializable(ARG_ADDRESS, param1);
        fragment.setArguments(args);*/
            return ReviewDialog()
        }
    }
}
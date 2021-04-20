package com.innomalist.taxi.rider.activities.coupon

import android.app.Activity
import android.content.DialogInterface
import android.content.Intent
import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import androidx.appcompat.app.AlertDialog
import androidx.databinding.DataBindingUtil
import androidx.recyclerview.widget.LinearLayoutManager
import com.google.android.material.datepicker.MaterialTextInputPicker
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import com.google.android.material.textfield.TextInputEditText
import com.innomalist.taxi.common.components.BaseActivity
import com.innomalist.taxi.common.models.Coupon
import com.innomalist.taxi.common.networking.socket.interfaces.EmptyClass
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import com.innomalist.taxi.common.utils.AlerterHelper
import com.innomalist.taxi.rider.R
import com.innomalist.taxi.rider.activities.coupon.adapters.CouponsRecyclerViewAdapter
import com.innomalist.taxi.rider.activities.coupon.adapters.CouponsRecyclerViewAdapter.OnCouponItemInteractionListener
import com.innomalist.taxi.rider.databinding.ActivityCouponBinding
import com.innomalist.taxi.rider.networking.socket.AddCoupon
import com.innomalist.taxi.rider.networking.socket.ApplyCoupon
import com.innomalist.taxi.rider.networking.socket.ApplyCouponResponse
import com.innomalist.taxi.rider.networking.socket.GetCoupons
import com.tylersuehr.esr.ContentItemLoadingStateFactory
import com.tylersuehr.esr.EmptyStateRecyclerView
import com.tylersuehr.esr.ImageTextStateDisplay

class CouponActivity : BaseActivity() {
    lateinit var binding: ActivityCouponBinding
    var coupon: Coupon? = null
    var isEditMode = false
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        isEditMode = intent.getBooleanExtra("select_mode", false)
        binding = DataBindingUtil.setContentView(this@CouponActivity, R.layout.activity_coupon)
        initializeToolbar(getString(R.string.drawer_coupons))
        binding.recyclerView.setStateDisplay(EmptyStateRecyclerView.STATE_LOADING, ContentItemLoadingStateFactory.newListLoadingState(this))
        binding.recyclerView.setStateDisplay(EmptyStateRecyclerView.STATE_EMPTY, ImageTextStateDisplay(this, com.innomalist.taxi.common.R.drawable.empty_state, getString(com.innomalist.taxi.common.R.string.empty_state_title), getString(com.innomalist.taxi.common.R.string.empty_state_message)))
        binding.recyclerView.setStateDisplay(EmptyStateRecyclerView.STATE_ERROR, ImageTextStateDisplay(this, com.innomalist.taxi.common.R.drawable.empty_state, getString(com.innomalist.taxi.common.R.string.empty_state_error_title), getString(com.innomalist.taxi.common.R.string.empty_state_error_message)))
        binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_LOADING)
        refreshCoupons()
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        menuInflater.inflate(R.menu.actionbar_add, menu)
        return super.onCreateOptionsMenu(menu)
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        MaterialAlertDialogBuilder(this)
                .setTitle(R.string.coupon_dialog_title)
                .setMessage(R.string.coupon_dialog_message)
                .setView(R.layout.dialog_input)
                .setPositiveButton(R.string.alert_ok) { dialog: DialogInterface, _: Int ->
                    val dlg = dialog as AlertDialog
                    val txt = dlg.findViewById<TextInputEditText>(R.id.text1)
                    addCoupon(txt!!.text.toString())
                }.show()
        return super.onOptionsItemSelected(item)
    }

    fun refreshCoupons() {
        GetCoupons().executeArray<Coupon> {
            when(it) {
                is RemoteResponse.Success -> {
                    if (it.body.isEmpty()) {
                        binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_EMPTY)
                        return@executeArray
                    }
                    binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_OK)
                    val couponsRecyclerViewAdapter = CouponsRecyclerViewAdapter(it.body, isEditMode, object : OnCouponItemInteractionListener {
                        override fun onSelect(coupon: Coupon) {
                            this@CouponActivity.coupon = coupon
                            applyCoupon(coupon.code!!)
                        }
                    })
                    val llm = LinearLayoutManager(this@CouponActivity)
                    llm.orientation = LinearLayoutManager.VERTICAL
                    binding.recyclerView.setHasFixedSize(true)
                    binding.recyclerView.layoutManager = llm
                    binding.recyclerView.adapter = couponsRecyclerViewAdapter
                }

                is RemoteResponse.Error -> {
                    it.error.showAlert(this)
                    binding.recyclerView.invokeState(EmptyStateRecyclerView.STATE_ERROR)

                }
            }

        }
    }

    fun applyCoupon(code: String) {
        ApplyCoupon(code).execute<ApplyCouponResponse> {
            when(it) {
                is RemoteResponse.Success -> {
                    val intent = Intent()
                    intent.putExtra("coupon", coupon)
                    intent.putExtra("costAfterCoupon", it.body.costAfterCoupon)
                    setResult(Activity.RESULT_OK, intent)
                    finish()
                }

                is RemoteResponse.Error -> {
                    it.error.showAlert(this)
                }
            }
        }
    }

    fun addCoupon(code: String) {
        AddCoupon(code).execute<EmptyClass> {
            when(it) {
                is RemoteResponse.Success -> {
                    refreshCoupons()
                }

                is RemoteResponse.Error -> {
                    it.error.showAlert(this)
                }
            }

        }
    }
}
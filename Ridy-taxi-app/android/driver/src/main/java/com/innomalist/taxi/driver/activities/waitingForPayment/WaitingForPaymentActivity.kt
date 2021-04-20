package com.innomalist.taxi.driver.activities.waitingForPayment

import android.app.Activity
import android.os.Bundle
import android.view.View
import androidx.databinding.DataBindingUtil
import com.innomalist.taxi.common.models.Request
import com.innomalist.taxi.common.models.Service
import com.innomalist.taxi.common.networking.socket.CurrentRequestResult
import com.innomalist.taxi.common.networking.socket.GetCurrentRequestInfo
import com.innomalist.taxi.common.networking.socket.interfaces.EmptyClass
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import com.innomalist.taxi.common.networking.socket.interfaces.SocketNetworkDispatcher
import com.innomalist.taxi.common.utils.AlertDialogBuilder
import com.innomalist.taxi.common.utils.AlertDialogBuilder.show
import com.innomalist.taxi.common.utils.AlerterHelper
import com.innomalist.taxi.common.utils.LoadingDialog
import com.innomalist.taxi.driver.R
import com.innomalist.taxi.driver.databinding.ActivityWaitingForPaymentBinding
import com.innomalist.taxi.driver.networking.socket.PaidInCash
import com.innomalist.taxi.driver.ui.DriverBaseActivity

class WaitingForPaymentActivity : DriverBaseActivity() {
    lateinit var binding: ActivityWaitingForPaymentBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        immersiveScreen = true
        super.onCreate(savedInstanceState)
        binding = DataBindingUtil.setContentView(this@WaitingForPaymentActivity, R.layout.activity_waiting_for_payment)
        SocketNetworkDispatcher.instance.onPaid = {
            this.setResult(Activity.RESULT_OK)
            finish()
        }
        binding.buttonPaidInCash.setOnClickListener { this.onPaidCash() }
    }

    private fun onPaidCash() {
        LoadingDialog.display(this@WaitingForPaymentActivity)
        PaidInCash().execute<EmptyClass> {
            LoadingDialog.hide()
            when(it) {
                is RemoteResponse.Success -> {
                    this.setResult(Activity.RESULT_OK)
                    finish()
                }

                is RemoteResponse.Error -> {
                    AlerterHelper.showError(this, it.error.message)
                }
            }
        }
    }

    private fun requestRefresh() {
        GetCurrentRequestInfo().execute<CurrentRequestResult> {
            when(it) {
                is RemoteResponse.Success -> {
                    travel = it.body.request
                    if(it.body.request.service!!.paymentMethod == Service.PaymentMethod.CashCredit || it.body.request.service!!.paymentMethod == Service.PaymentMethod.OnlyCash) {
                        binding.buttonPaidInCash.visibility = View.VISIBLE
                    }
                    refreshPage()
                }

                is RemoteResponse.Error -> {
                    this.setResult(Activity.RESULT_OK)
                    finish()
                }
            }
        }
    }

    override fun onReconnected() {
        super.onReconnected()
        requestRefresh()
    }

    private fun refreshPage() {
        val request = travel
        when (request!!.status) {
            Request.Status.Finished, Request.Status.WaitingForReview -> {
                this.setResult(Activity.RESULT_OK)
                finish()
            }

            Request.Status.WaitingForPostPay -> {

            }
            
            else -> {
                show(this, "Unhandled service status: ${request.status}", AlertDialogBuilder.DialogButton.OK, null)
            }
        }
    }
}
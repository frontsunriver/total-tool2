package com.innomalist.taxi.common.activities.chargeAccount

import android.app.Activity
import android.content.DialogInterface
import android.content.Intent
import android.os.Bundle
import android.view.View
import android.widget.ArrayAdapter
import android.widget.LinearLayout
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.view.ContextThemeWrapper
import androidx.databinding.DataBindingUtil
import com.braintreepayments.api.dropin.DropInActivity
import com.braintreepayments.api.dropin.DropInRequest
import com.braintreepayments.api.dropin.DropInResult
import com.google.android.material.button.MaterialButton
import com.google.android.material.button.MaterialButtonToggleGroup
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import com.google.android.material.textfield.TextInputEditText
import com.innomalist.taxi.common.Config
import com.innomalist.taxi.common.R
import com.innomalist.taxi.common.activities.payment.PaymentActivity
import com.innomalist.taxi.common.components.BaseActivity
import com.innomalist.taxi.common.databinding.ActivityChargeAccountBinding
import com.innomalist.taxi.common.models.PaymentGateway
import com.innomalist.taxi.common.models.PaymentGatewayType
import com.innomalist.taxi.common.models.WalletItem
import com.innomalist.taxi.common.networking.http.GetStripeClientSecret
import com.innomalist.taxi.common.networking.http.StripeClientSecretEndpointResult
import com.innomalist.taxi.common.networking.socket.*
import com.innomalist.taxi.common.networking.socket.interfaces.EmptyClass
import com.innomalist.taxi.common.networking.socket.interfaces.ErrorStatus
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import com.innomalist.taxi.common.networking.socket.interfaces.SocketNetworkDispatcher
import com.innomalist.taxi.common.utils.AlertDialogBuilder
import com.innomalist.taxi.common.utils.AlerterHelper
import com.stripe.android.ApiResultCallback
import com.stripe.android.PaymentConfiguration
import com.stripe.android.PaymentIntentResult
import com.stripe.android.Stripe
import com.stripe.android.model.ConfirmPaymentIntentParams
import com.stripe.android.model.StripeIntent
import java.text.NumberFormat
import java.util.*
import kotlin.collections.ArrayList

class ChargeAccountActivity : BaseActivity() {
    lateinit var binding: ActivityChargeAccountBinding
    private var selectedPayment: PaymentGateway? = null
    private var paymentGateways: List<PaymentGateway> = ArrayList()
    var currency: String? = null
    private var stripe: Stripe? = null
    private var tripFee: Double = 0.0
    private var walletItems: List<WalletItem> = ArrayList()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = DataBindingUtil.setContentView(this, R.layout.activity_charge_account)
        initializeToolbar(getString(R.string.drawer_wallet))
        binding.loadingMode = true
        binding.waitingForPayment = false
        binding.checkoutButton.setOnClickListener { this.onCheckoutClicked() }
        WalletInfo().execute<WalletInfoResult> {
            when (it) {
                is RemoteResponse.Success -> {
                    binding.loadingMode = false
                    if (it.body.gateways.count() < 1) {
                        AlertDialogBuilder.show(this@ChargeAccountActivity, "No Gateway available.", AlertDialogBuilder.DialogButton.OK) {
                            finish()
                        }
                        return@execute
                    }
                    if (it.body.wallet.count() < 1) {
                        binding.emptyBalance = true
                        if (intent.getStringExtra("currency") == null) {
                            AlertDialogBuilder.show(this@ChargeAccountActivity, getString(R.string.message_wallet_trip_record_empty), AlertDialogBuilder.DialogButton.OK) {
                                finish()
                            }
                            return@execute
                        }
                    }
                    this.paymentGateways = it.body.gateways
                    this.walletItems = it.body.wallet.sortedByDescending { x -> x.amount }
                    val items = this.walletItems.map { walletItem ->
                        val nf = NumberFormat.getCurrencyInstance()
                        nf.currency = Currency.getInstance(walletItem.currency!!)
                        nf.format(walletItem.amount)
                    }
                    val adapter = ArrayAdapter(this, android.R.layout.simple_dropdown_item_1line, items)
                    binding.balanceAutocomplete.setAdapter(adapter)
                    binding.balanceAutocomplete.setOnItemClickListener { _, _, position, _ ->
                        currency = walletItems[position].currency
                    }
                    if (it.body.wallet.isNotEmpty()) {
                        binding.balanceAutocomplete.setText(items[0], false)
                        currency = this.walletItems[0].currency
                    } else {
                        binding.balanceAutocomplete.visibility = View.GONE
                    }
                    when {
                        it.body.gateways.size > 1 -> {
                            for (gw in it.body.gateways) {
                                val btn = MaterialButton(ContextThemeWrapper(this, R.style.Widget_MaterialComponents_Button_TextButton))
                                btn.id = gw.id
                                btn.text = gw.title
                                val params = LinearLayout.LayoutParams(
                                        LinearLayout.LayoutParams.WRAP_CONTENT,
                                        LinearLayout.LayoutParams.WRAP_CONTENT
                                )
                                params.setMargins(0, 0, 8, 0)
                                params.weight = 1f
                                btn.layoutParams = params
                                btn.setOnClickListener {
                                    selectedPayment = gw
                                    validateCardInput(this.selectedPayment!!)
                                    binding.checkoutButton.isEnabled = true
                                    binding.checkoutButton.text = getString(R.string.checkout_filled, gw.title)
                                }
                                binding.layoutMethods.addView(btn)
                            }
                        }
                        it.body.gateways.size == 1 -> {
                            selectedPayment = it.body.gateways[0]
                            validateCardInput(this.selectedPayment!!)
                            binding.checkoutButton.isEnabled = true
                            binding.checkoutButton.text = getString(R.string.checkout_filled, selectedPayment!!.title)
                            binding.paymentToggleLayout.visibility = View.GONE
                            binding.titleMethod.visibility = View.GONE
                        }
                    }
                    if (intent.getStringExtra("currency") != null) {
                        this.currency = intent.getStringExtra("currency")
                        binding.balanceAutocomplete.isEnabled = false
                        val index = this.walletItems.indexOfFirst { walletItem -> walletItem.currency == this.currency }
                        tripFee = intent.getDoubleExtra("defaultAmount", 0.0) - if (index < 0) 0.0 else this.walletItems[index].amount
                        binding.textAmount.setText("$tripFee")
                        if (index > 0) {
                            binding.balanceAutocomplete.setSelection(index)
                        }
                        binding.emptyBalance = index < 0

                    }
                }

                is RemoteResponse.Error -> {
                    it.error.showAlert(this)
                    finish()
                }
            }
        }
        binding.paymentToggleLayout.addOnButtonCheckedListener { _: MaterialButtonToggleGroup?, checkedId: Int, isChecked: Boolean ->
            if (!isChecked) {
                selectedPayment = null
                binding.cardInput.visibility = View.GONE
                binding.checkoutButton.isEnabled = false
                binding.checkoutButton.text = getString(R.string.checkout_empty)
                return@addOnButtonCheckedListener
            } else {
                selectedPayment = paymentGateways.first { it.id == checkedId }
                validateCardInput(this.selectedPayment!!)

            }
        }
        binding.chargeAddFirst.text = resources.getInteger(R.integer.charge_first).toString()
        binding.chargeAddSecond.text = resources.getInteger(R.integer.charge_second).toString()
        binding.chargeAddThird.text = resources.getInteger(R.integer.charge_third).toString()
        binding.chargeAddFirst.setOnClickListener { addCharge(R.integer.charge_first) }
        binding.chargeAddSecond.setOnClickListener { addCharge(R.integer.charge_second) }
        binding.chargeAddThird.setOnClickListener { addCharge(R.integer.charge_third) }
    }

    private fun validateCardInput(gateway: PaymentGateway) {
        when (gateway.type) {
            PaymentGatewayType.Flutterwave, PaymentGatewayType.PayGate, PaymentGatewayType.Stripe -> {
                binding.cardInput.visibility = View.VISIBLE
            }

            else -> {
                binding.cardInput.visibility = View.GONE
            }
        }
    }

    private fun onCheckoutClicked() {
        if (binding.textAmount.text.toString().isEmpty()) {
            binding.textAmount.error = getString(R.string.error_charge_field_empty)
            return
        }
        val amountDouble = binding.textAmount.text.toString().replace(",", "").toDouble()
        if (tripFee > 0 && tripFee < amountDouble) {
            binding.textAmount.error = getString(R.string.error_charge_field_less_than_trip_fee)
            return
        }
        when (selectedPayment!!.type) {
            PaymentGatewayType.Stripe -> {
                var amount = amountDouble.toInt()
                if (currency == "USD") {
                    amount = (amountDouble * 100).toInt()
                }
                binding.waitingForPayment = true
                GetStripeClientSecret(selectedPayment!!.id, amount = amount, currency = currency!!).execute<StripeClientSecretEndpointResult> {
                    when (it) {
                        is RemoteResponse.Success -> {
                            PaymentConfiguration.init(applicationContext, selectedPayment!!.publicKey!!)
                            val params = binding.cardInput.paymentMethodCreateParams
                            if (params != null) {
                                val confirmParams = ConfirmPaymentIntentParams.createWithPaymentMethodCreateParams(params, it.body.clientSecret)
                                stripe = Stripe(applicationContext, PaymentConfiguration.getInstance(applicationContext).publishableKey)
                                stripe!!.confirmPayment(this, confirmParams)
                            }
                        }

                        is RemoteResponse.Error -> {
                            AlerterHelper.showError(this, it.error.localizedDescription)
                        }
                    }

                }
            }

            PaymentGatewayType.Braintree -> {
                startBrainTree()
            }

            PaymentGatewayType.Flutterwave, PaymentGatewayType.PayGate -> {
                if (binding.cardInput.card == null) {
                    AlerterHelper.showWarning(this, getString(R.string.error_charge_card_is_empty))
                    return
                }
                val cardNumber = binding.cardInput.card!!.number
                val expiryYear = binding.cardInput.card!!.expYear
                val expiryMonth = binding.cardInput.card!!.expMonth
                val cvc = binding.cardInput.card!!.cvc
                val token = "{\"cardNumber\":${cardNumber},\"cvv\":${cvc},\"expiryMonth\":${expiryMonth},\"expiryYear\":${expiryYear}}"
                chargeAccount(amountDouble, token)
            }

            else -> {
                binding.waitingForPayment = true
                GetPaymentLink(selectedPayment!!.id, amount = amountDouble, currency = currency!!, serverUrl = Config.Backend).execute<GetPaymentLinkResult> {
                    binding.waitingForPayment = false
                    when (it) {
                        is RemoteResponse.Success -> {
                            val intent = Intent(this@ChargeAccountActivity, PaymentActivity::class.java)
                            intent.putExtra("redirectionUrl", it.body.url)
                            startActivityForResult(intent, WEB_PAYMENT_CALLBACK)
                        }

                        is RemoteResponse.Error -> {
                            it.error.showAlert(this)
                        }
                    }
                }
            }
        }
    }

    private fun startBrainTree() {
        val dropInRequest = DropInRequest().clientToken(selectedPayment!!.publicKey)
        startActivityForResult(dropInRequest.getIntent(this), REQUEST_CODE)
    }

    private fun addCharge(resId: Int) {
        try {
            binding.textAmount.setText(getString(resId))
        } catch (e: Exception) {
            e.printStackTrace()
        }
    }

    override fun onResume() {
        super.onResume()
        SocketNetworkDispatcher.instance.onFinished = {
            finish()
        }
    }

    fun chargeAccount(amount: Double, paymentToken: String, pin: Int? = null, otp: Int? = null, transactionId: String? = null) {
        binding.waitingForPayment = true
        WalletTopUp(selectedPayment!!.id, currency!!, paymentToken, amount, pin, otp, transactionId).execute<EmptyClass> {
            binding.waitingForPayment = false
            when (it) {
                is RemoteResponse.Success -> {
                    setResult(Activity.RESULT_OK)
                    finish()
                }

                is RemoteResponse.Error -> {
                    when (it.error.status) {
                        ErrorStatus.PINCodeRequired -> {
                            val builder = MaterialAlertDialogBuilder(this)
                                    .setTitle(getString(R.string.message_pin_required_title))
                                    .setMessage(getString(R.string.message_pin_required_message))
                                    .setView(R.layout.dialog_input)
                                    .setPositiveButton(R.string.alert_ok) { dialog: DialogInterface, _: Int ->
                                        val dlg = dialog as AlertDialog
                                        val txt = dlg.findViewById<TextInputEditText>(R.id.text1)
                                        chargeAccount(amount, paymentToken, pin = txt!!.text.toString().toInt())
                                    }
                            builder.show()
                            return@execute
                        }
                        ErrorStatus.OTPCodeRequired -> {
                            val builder = MaterialAlertDialogBuilder(this)
                                    .setTitle(getString(R.string.message_otp_required_title))
                                    .setMessage(getString(R.string.message_otp_required_message))
                                    .setView(R.layout.dialog_input)
                                    .setPositiveButton(R.string.alert_ok) { dialog: DialogInterface, _: Int ->
                                        val dlg = dialog as AlertDialog
                                        val txt = dlg.findViewById<TextInputEditText>(R.id.text1)
                                        chargeAccount(amount, paymentToken, otp = txt!!.text.toString().toInt(), transactionId = it.error.message)
                                    }
                            builder.show()
                            return@execute
                        }
                        else -> {
                            it.error.showAlert(this)
                        }
                    }
                }
            }
        }
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        val amount = binding.textAmount.text.toString().replace(",", "").toDouble()
        when (requestCode) {
            REQUEST_CODE -> {
                when (resultCode) {
                    Activity.RESULT_OK -> {
                        val result: DropInResult = data!!.getParcelableExtra(DropInResult.EXTRA_DROP_IN_RESULT)!!
                        chargeAccount(amount, result.paymentMethodNonce!!.nonce)
                    }
                    Activity.RESULT_CANCELED -> {
                        AlerterHelper.showWarning(this, getString(R.string.warning_user_canceled))
                    }
                    else -> { // handle errors here, an exception may be available in
                        val error = data!!.getSerializableExtra(DropInActivity.EXTRA_ERROR) as Exception
                        AlerterHelper.showError(this@ChargeAccountActivity, error.message)
                    }
                }
            }

            WEB_PAYMENT_CALLBACK -> {
                when (resultCode) {
                    RESULT_OK -> {
                        AlerterHelper.showInfo(this, getString(R.string.message_payment_done))
                    }

                    RESULT_CANCELED -> {
                        AlerterHelper.showError(this, getString(R.string.message_payment_failed))
                    }
                }
            }

            else -> {
                stripe!!.onPaymentResult(requestCode, data, object : ApiResultCallback<PaymentIntentResult> {
                    override fun onSuccess(result: PaymentIntentResult) {
                        val paymentIntent = result.intent
                        val status = paymentIntent.status
                        if (status == StripeIntent.Status.Succeeded) {
                            chargeAccount(amount, paymentIntent.id ?: "ID")
                        } else {
                            AlerterHelper.showError(this@ChargeAccountActivity, paymentIntent.lastPaymentError?.message
                                    ?: "Unknown Payment Error")
                        }
                    }

                    override fun onError(e: Exception) {
                        AlerterHelper.showError(this@ChargeAccountActivity, e.toString())
                    }
                })
            }
        }
    }

    companion object {
        private const val REQUEST_CODE = 243
        private const val WEB_PAYMENT_CALLBACK = 244
    }
}
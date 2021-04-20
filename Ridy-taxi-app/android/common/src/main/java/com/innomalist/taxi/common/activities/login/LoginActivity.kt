package com.innomalist.taxi.common.activities.login/*
package com.innomalist.taxi.common.activities.login

import android.app.Activity
import android.content.Context
import android.content.Intent
import android.graphics.Color
import android.os.Build
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.WindowManager
import android.widget.EditText
import androidx.appcompat.widget.AppCompatButton
import androidx.databinding.DataBindingUtil
import androidx.viewpager.widget.PagerAdapter
import com.google.android.material.textfield.TextInputEditText
import com.innomalist.taxi.common.R
import com.innomalist.taxi.common.components.BaseActivity
import com.innomalist.taxi.common.databinding.ActivityLoginBinding
import com.innomalist.taxi.common.interfaces.AlertDialogEvent
import com.innomalist.taxi.common.utils.AlertDialogBuilder.DialogResult
import com.innomalist.taxi.common.utils.ServerResponse
import java.io.BufferedReader
import java.io.DataOutputStream
import java.io.InputStreamReader
import java.net.HttpURLConnection
import java.net.URL
import java.net.URLEncoder
import java.util.*

class LoginActivity : BaseActivity() {
    private var myViewPagerAdapter: MyViewPagerAdapter? = null
    private var layouts: IntArray = IntArray(2)
    lateinit var binding: ActivityLoginBinding
    var txtMobile: TextInputEditText? = null
    var txtCode: EditText? = null
    var btnSend: AppCompatButton? = null
    var btnVerify: AppCompatButton? = null
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        // Making notification bar transparent
        if (Build.VERSION.SDK_INT >= 21) window.decorView.systemUiVisibility = View.SYSTEM_UI_FLAG_LAYOUT_STABLE or View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
        binding = DataBindingUtil.setContentView(this@LoginActivity, R.layout.activity_login)
        // layouts of all welcome sliders
// add few more layouts if you want
        layouts = intArrayOf(
                R.layout.fragment_login_step_first,
                R.layout.fragment_login_step_second)
        // making notification bar transparent
        changeStatusBarColor()
        myViewPagerAdapter = MyViewPagerAdapter()
        binding.viewpager.adapter = myViewPagerAdapter
    }

    private fun getItem(i: Int): Int {
        return binding.viewpager.currentItem + i
    }

    */
/**
     * Making notification bar transparent
     *//*

    private fun changeStatusBarColor() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            val window = window
            window.addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS)
            window.statusBarColor = Color.TRANSPARENT
        }
    }

    */
/**
     * View pager adapter
     *//*

    private inner class MyViewPagerAdapter internal constructor() : PagerAdapter() {
        private var layoutInflater: LayoutInflater? = null
        override fun instantiateItem(container: ViewGroup, position: Int): Any {
            layoutInflater = getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater
            val view = layoutInflater!!.inflate(layouts[position], container, false)
            if (position == 0) {
                btnSend = findViewById(R.id.button_send)
                txtMobile = findViewById(R.id.edit_mobile_number)
                btnSend!!.setOnClickListener { eventBus.post(RequestSMSEvent(txtMobile!!.getText().toString())) }
            }
            if (position == 1) {
                btnVerify = findViewById(R.id.button_verify)
                txtMobile = findViewById(R.id.edit_mobile_number)
                txtCode = findViewById(R.id.text_code)
                btnVerify!!.setOnClickListener { eventBus.post(VerifyCodeEvent(txtMobile!!.text.toString(), txtCode!!.getText().toString())) }
            }
            container.addView(view)
            return view
        }

        override fun getCount(): Int {
            return layouts.size
        }

        override fun isViewFromObject(view: View, obj: Any): Boolean {
            return view === obj
        }

        override fun destroyItem(container: ViewGroup, position: Int, `object`: Any) {
            val view = `object` as View
            container.removeView(view)
        }
    }

    inner class RequestSMSEvent internal constructor(var mobileNumber: String) : BaseRequestEvent(RequestSMSResultEvent(ServerResponse.REQUEST_TIMEOUT.value))

    inner class RequestSMSResultEvent : BaseResultEvent {
        internal constructor(code: Int) : super(code) {}
        internal constructor(code: Int, message: String?) : super(code, message) {}
    }

    inner class VerifyCodeEvent internal constructor(var mobileNumber: String, var code: String) : BaseRequestEvent(VerifyCodeResultEvent(ServerResponse.REQUEST_TIMEOUT.value))

    inner class VerifyCodeResultEvent : BaseResultEvent {
        internal constructor(code: Int) : super(code) {}
        internal constructor(code: Int, message: String?) : super(code, message) {}
    }

    @Subscribe(threadMode = ThreadMode.BACKGROUND)
    fun onRequestValidation(event: RequestSMSEvent) {
        try {
            val url = URL(getString(R.string.verification_address))
            val client = url.openConnection() as HttpURLConnection
            client.requestMethod = "POST"
            client.doOutput = true
            client.doInput = true
            val wr = DataOutputStream(client.outputStream)
            val postDataParams = HashMap<String, String>()
            postDataParams["mobile"] = event.mobileNumber
            val result = StringBuilder()
            var first = true
            for ((key, value) in postDataParams) {
                if (first) first = false else result.append("&")
                result.append(URLEncoder.encode(key, "UTF-8"))
                result.append("=")
                result.append(URLEncoder.encode(value, "UTF-8"))
            }
            wr.write(result.toString().toByteArray())
            val reader = BufferedReader(InputStreamReader(client.inputStream))
            val sb = StringBuilder()
            var line: String?
            while (reader.readLine().also { line = it } != null) sb.append(line)
            val res = sb.toString()
            if (res == "200") eventBus.post(RequestSMSResultEvent(200)) else eventBus.post(RequestSMSResultEvent(666, res))
        } catch (exception: Exception) {
            eventBus.post(RequestSMSResultEvent(666, exception.message))
        }
    }

    @Subscribe(threadMode = ThreadMode.MAIN)
    fun onRequestSMSResult(event: RequestSMSResultEvent) {
        if (event.hasError()) {
            event.showError(this@LoginActivity, AlertDialogEvent { result: DialogResult -> if (result == DialogResult.RETRY) btnSend!!.callOnClick() })
            return
        }
        binding.viewpager.currentItem = getItem(+1)
    }

    @Subscribe(threadMode = ThreadMode.BACKGROUND)
    fun onVerifyCode(event: VerifyCodeEvent) {
        try {
            val url = URL(getString(R.string.verify_address))
            val client = url.openConnection() as HttpURLConnection
            client.requestMethod = "POST"
            client.doOutput = true
            client.doInput = true
            val wr = DataOutputStream(client.outputStream)
            val postDataParams = HashMap<String, String>()
            postDataParams["mobile"] = event.mobileNumber
            postDataParams["code"] = event.code
            val result = StringBuilder()
            var first = true
            for ((key, value) in postDataParams) {
                if (first) first = false else result.append("&")
                result.append(URLEncoder.encode(key, "UTF-8"))
                result.append("=")
                result.append(URLEncoder.encode(value, "UTF-8"))
            }
            wr.write(result.toString().toByteArray())
            val reader = BufferedReader(InputStreamReader(client.inputStream))
            val sb = StringBuilder()
            var line: String?
            while (reader.readLine().also { line = it } != null) sb.append(line)
            val res = sb.toString()
            if (res == "200") eventBus.post(VerifyCodeResultEvent(200)) else eventBus.post(VerifyCodeResultEvent(666, res))
        } catch (exception: Exception) {
            eventBus.post(VerifyCodeResultEvent(666, exception.message))
        }
    }

    @Subscribe(threadMode = ThreadMode.MAIN)
    fun onVerifyResult(event: VerifyCodeResultEvent) {
        if (event.hasError()) {
            event.showError(this@LoginActivity, AlertDialogEvent { result: DialogResult -> if (result == DialogResult.RETRY) btnVerify!!.callOnClick() })
            return
        }
        val data = Intent()
        data.putExtra("mobile", getString(R.string.default_country_code) + txtMobile!!.text.toString())
        setResult(Activity.RESULT_OK, data)
        finish()
    }
}*/

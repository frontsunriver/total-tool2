package com.innomalist.taxi.common.activities.payment

import android.annotation.SuppressLint
import android.annotation.TargetApi
import android.net.http.SslError
import android.os.Build
import android.os.Bundle
import android.util.Log
import android.view.Gravity
import android.view.View
import android.webkit.*
import android.widget.FrameLayout
import android.widget.ProgressBar
import com.innomalist.taxi.common.components.BaseActivity

class PaymentActivity: BaseActivity() {
    private lateinit var progressBar: ProgressBar
    private var callbackUrl: String = "payment.callback"

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
         Log.d("PayAction", intent.action ?: "Null")
         Log.d("PayData", intent.dataString ?: "Null")
         if((intent.dataString ?: "").contains(".payment")) {
             returnResult(RESULT_OK)
         }
        // Retrieve parameters
        val redirectionUrl = intent.getStringExtra("redirectionUrl")

        val contentView = FrameLayout(this)
        setContentView(contentView)

         if(redirectionUrl != null) {
             val webView = initWebView(redirectionUrl)
             contentView.addView(webView)
         }

        progressBar = initProgressBar()
        contentView.addView(progressBar)
    }

    override fun onBackPressed() {
        returnResult(RESULT_CANCELED)
    }

    private fun initProgressBar(): ProgressBar {
        val progressBar = ProgressBar(this)
        val progressBarLyt = FrameLayout.LayoutParams(FrameLayout.LayoutParams.WRAP_CONTENT, FrameLayout.LayoutParams.WRAP_CONTENT)
        progressBarLyt.gravity = Gravity.CENTER_HORIZONTAL or Gravity.CENTER_VERTICAL
        progressBar.isIndeterminate = true
        progressBar.layoutParams = progressBarLyt

        return progressBar
    }

    @SuppressLint("SetJavaScriptEnabled")
    private fun initWebView(url: String): WebView {
        val webView = WebView(this)
        webView.settings.javaScriptEnabled = true
        webView.settings.domStorageEnabled = true
        webView.loadUrl(url)

        webView.webViewClient = object: WebViewClient() {
            override fun onPageFinished(view: WebView, url: String) {
                progressBar.visibility = View.GONE
                super.onPageFinished(view, url)
            }

            @Suppress("OverridingDeprecatedMember")
            override fun shouldOverrideUrlLoading(view: WebView, url: String): Boolean {
                return checkUrl(webView, url)
            }

            @TargetApi(Build.VERSION_CODES.LOLLIPOP)
            override fun shouldOverrideUrlLoading(view: WebView, webResourceRequest: WebResourceRequest): Boolean {
                return checkUrl(webView, webResourceRequest.url.toString())
            }

            override fun onReceivedError(view: WebView?, errorCode: Int, description: String?, failingUrl: String?) {
                super.onReceivedError(view, errorCode, description, failingUrl)
                Log.e("Error in gateway", description!!)
            }

            override fun onReceivedHttpError(view: WebView?, request: WebResourceRequest?, errorResponse: WebResourceResponse?) {
                super.onReceivedHttpError(view, request, errorResponse)
                Log.e("Error in gateway", errorResponse?.reasonPhrase ?: "no reason")
            }

            override fun onReceivedError(view: WebView?, request: WebResourceRequest?, error: WebResourceError?) {
                super.onReceivedError(view, request, error)
                Log.e("Error in gateway", error.toString())
            }

            override fun onReceivedSslError(view: WebView?, handler: SslErrorHandler?, error: SslError?) {
                super.onReceivedSslError(view, handler, error)
                Log.e("Error in Gateway", error.toString())
            }
        }
        webView.canGoForward()
        return webView
    }

    private fun returnResult(result: Int) {
        setResult(result)
        finish()
    }

    private fun isCallbackUrl(url: String): Boolean {
        return url.startsWith(callbackUrl) || url.contains("ridyverifiedpayment")
    }

    private fun isCancelUrl(url: String): Boolean {
        return url.contains("ridycancelpayment")
    }

    private fun checkUrl(view: WebView, url: String): Boolean {
        val isCallBack = isCallbackUrl(url)
        val isCancel = isCancelUrl(url)
        when {
            isCallBack -> {
                view.stopLoading()
                returnResult(RESULT_OK)
            }
            isCancel -> {
                view.stopLoading()
                returnResult(RESULT_CANCELED)
            }
            else -> view.loadUrl(url)
        }
        return (!isCallBack)
    }
}
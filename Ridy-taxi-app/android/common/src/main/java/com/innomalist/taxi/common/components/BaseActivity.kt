package com.innomalist.taxi.common.components

import android.app.Activity
import android.content.Context
import android.graphics.Color
import android.os.Build
import android.os.Bundle
import android.util.TypedValue
import android.view.View
import android.view.WindowManager
import androidx.annotation.StringRes
import androidx.appcompat.app.ActionBar
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.widget.Toolbar
import com.innomalist.taxi.common.MyTaxiApplication
import com.innomalist.taxi.common.R
import com.innomalist.taxi.common.utils.LocaleHelper
import com.innomalist.taxi.common.utils.MyPreferenceManager


open class BaseActivity : AppCompatActivity() {
    var toolbar: ActionBar? = null
    private var screenDensity = 0f
    private var isInForeground = false
    var showConnectionDialog = true
    var shouldReconnect = true
    lateinit var app: MyTaxiApplication
    open var immersiveScreen = false

    val preferences: MyPreferenceManager
        get() {
            return MyPreferenceManager.getInstance(applicationContext)
        }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        app = application as MyTaxiApplication
        screenDensity = applicationContext.resources.displayMetrics.density
        setActivityTheme(this@BaseActivity)
        if(immersiveScreen) {
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
                window.apply {
                    clearFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS)
                    addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS)
                    if (Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.R) {
                        setDecorFitsSystemWindows(false)
                    } else if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
                        decorView.systemUiVisibility = View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN or View.SYSTEM_UI_FLAG_LIGHT_STATUS_BAR
                    } else {
                        decorView.systemUiVisibility = View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
                    }
                    statusBarColor = Color.TRANSPARENT
                }
            }
        }
    }

    override fun attachBaseContext(base: Context) {
        super.attachBaseContext(LocaleHelper.onAttach(base))
    }

    fun initializeToolbar(title: String?) {
        val toolbarView = findViewById<Toolbar>(R.id.toolbar)
        setSupportActionBar(toolbarView)
        toolbar = supportActionBar
        if (toolbar != null) {
            toolbar!!.setDisplayHomeAsUpEnabled(true)
            toolbar!!.title = title
            toolbarView.setNavigationOnClickListener { onBackPressed() }
        }
    }

    fun initializeToolbar(@StringRes title: Int) {
        initializeToolbar(getString(title))
    }

    val primaryColor: Int
        get() {
            val typedValue = TypedValue()
            val a = this.obtainStyledAttributes(typedValue.data, intArrayOf(R.attr.colorPrimary))
            val color = a.getColor(0, 0)
            a.recycle()
            return color
        }

    open fun onReconnected() {

    }

    /*override fun onWindowFocusChanged(hasFocus: Boolean) {
        super.onWindowFocusChanged(hasFocus)
        if (hasFocus and isFullscreen) {
            window.decorView.systemUiVisibility = (View.SYSTEM_UI_FLAG_LAYOUT_STABLE
                    or View.SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION
                    or View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
                    or View.SYSTEM_UI_FLAG_HIDE_NAVIGATION
                    or View.SYSTEM_UI_FLAG_FULLSCREEN
                    or View.SYSTEM_UI_FLAG_IMMERSIVE_STICKY)
        }
    }*/

    val currentTheme: Int
        get() = R.style.Theme_Default

    private fun setActivityTheme(activity: AppCompatActivity) {
        activity.setTheme(currentTheme)
    }

    private fun clearReferences() {
        val currActivity: Activity = app.getCurrentActivity()
        if (this == currActivity) app.setCurrentActivity(null)
    }

    override fun onResume() {
        super.onResume()
        app.setCurrentActivity(this)
        isInForeground = true

    }

    override fun onPause() {
        super.onPause()
        //clearReferences()
        isInForeground = false
    }

    override fun onDestroy() {
        super.onDestroy()
        clearReferences()
    }

    fun convertDPToPixel(dp: Int): Int {
        return (dp * screenDensity).toInt()
    }
}
package com.innomalist.taxi.common

import android.app.Activity
import android.app.Application
import androidx.appcompat.app.AppCompatDelegate
import androidx.lifecycle.Lifecycle
import androidx.lifecycle.LifecycleObserver
import androidx.lifecycle.OnLifecycleEvent
import androidx.lifecycle.ProcessLifecycleOwner
import com.google.firebase.FirebaseApp
import com.google.firebase.iid.FirebaseInstanceId
import com.innomalist.taxi.common.components.BaseActivity
import com.innomalist.taxi.common.networking.socket.interfaces.ConnectionError
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import com.innomalist.taxi.common.networking.socket.interfaces.SocketNetworkDispatcher
import com.innomalist.taxi.common.utils.AlertDialogBuilder
import com.innomalist.taxi.common.utils.LoadingDialog
import com.innomalist.taxi.common.utils.MyPreferenceManager
import kotlinx.coroutines.Dispatchers.Main
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch

class MyTaxiApplication: Application(), LifecycleObserver {
    private var currentActivity: BaseActivity? = null

    override fun onCreate() {
        FirebaseApp.initializeApp(applicationContext)
        val nightMode = AppCompatDelegate.MODE_NIGHT_NO
        AppCompatDelegate.setDefaultNightMode(nightMode)
        ProcessLifecycleOwner.get().lifecycle.addObserver(this)
        super.onCreate()
    }


    fun getCurrentActivity(): Activity {
        return currentActivity!!
    }

    fun setCurrentActivity(mCurrentActivity: BaseActivity?) {
        currentActivity = mCurrentActivity
    }

    @OnLifecycleEvent(Lifecycle.Event.ON_START)
    fun onMoveToForeground() {
        if(currentActivity is BaseActivity && !(currentActivity as BaseActivity).shouldReconnect) return
        val token = MyPreferenceManager.getInstance(this).token ?: return
        if(SocketNetworkDispatcher.currentNamespace == null)  return
        if(currentActivity != null) LoadingDialog.display(currentActivity!!)
        FirebaseInstanceId.getInstance().instanceId.addOnCompleteListener { fb ->
            SocketNetworkDispatcher.instance.connect(SocketNetworkDispatcher.currentNamespace!!, token, fb.result!!.token) {
                when(it) {
                    is RemoteResponse.Success -> {
                        LoadingDialog.hide()
                        currentActivity?.onReconnected()
                    }

                    is RemoteResponse.Error -> {
                        GlobalScope.launch(Main) {
                            if(it.error == ConnectionError.TokenVerificationError)
                                return@launch
                            AlertDialogBuilder.show(currentActivity!!, getString(R.string.error_message_reconnection_failed, it.error.rawValue),AlertDialogBuilder.DialogButton.OK) {
                                currentActivity!!.finishAffinity()
                            }
                        }
                    }
                }
            }
        }
    }

    @OnLifecycleEvent(Lifecycle.Event.ON_STOP)
    fun onMoveToBackground() {
        SocketNetworkDispatcher.instance.disconnect()
    }
}
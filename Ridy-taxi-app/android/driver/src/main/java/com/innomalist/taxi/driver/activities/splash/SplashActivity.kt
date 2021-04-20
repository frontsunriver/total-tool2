package com.innomalist.taxi.driver.activities.splash

import android.app.Activity
import android.content.Intent
import android.os.Bundle
import android.view.View
import androidx.databinding.DataBindingUtil
import com.firebase.ui.auth.AuthUI
import com.firebase.ui.auth.AuthUI.IdpConfig.PhoneBuilder
import com.google.firebase.auth.FirebaseAuth
import com.google.firebase.iid.FirebaseInstanceId
import com.innomalist.taxi.common.components.BaseActivity
import com.innomalist.taxi.common.interfaces.AlertDialogEvent
import com.innomalist.taxi.common.networking.socket.interfaces.ConnectionError
import com.innomalist.taxi.common.networking.socket.interfaces.Namespace
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import com.innomalist.taxi.common.networking.socket.interfaces.SocketNetworkDispatcher
import com.innomalist.taxi.common.utils.AlertDialogBuilder
import com.innomalist.taxi.common.utils.AlertDialogBuilder.DialogResult
import com.innomalist.taxi.common.utils.AlertDialogBuilder.show
import com.innomalist.taxi.common.utils.AlerterHelper.showError
import com.innomalist.taxi.common.utils.CommonUtils.isInternetDisabled
import com.innomalist.taxi.common.utils.MyPreferenceManager.Companion.getInstance
import com.innomalist.taxi.driver.R
import com.innomalist.taxi.driver.activities.main.MainActivity
import com.innomalist.taxi.driver.activities.profile.ProfileActivity
import com.innomalist.taxi.driver.databinding.ActivitySplashBinding
import com.innomalist.taxi.driver.networking.http.GetRegisterInfo
import com.innomalist.taxi.driver.networking.http.Login
import com.innomalist.taxi.driver.networking.http.LoginResult
import com.innomalist.taxi.driver.networking.http.RegistrationInfo

class SplashActivity : BaseActivity() {
    private lateinit var binding: ActivitySplashBinding
    private var SIGN_IN_ACTIVITY = 123
    private var startRequested = false
    private val onLoginClicked = View.OnClickListener {
        startActivityForResult(
                AuthUI.getInstance()
                        .createSignInIntentBuilder()
                        .setAvailableProviders(listOf(PhoneBuilder().build()))
                        .setLogo(R.drawable.logo)
                        .setTheme(currentTheme)
                        .build(),
                SIGN_IN_ACTIVITY)
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        shouldReconnect = false
        immersiveScreen = true
        super.onCreate(savedInstanceState)
        binding = DataBindingUtil.setContentView(this, R.layout.activity_splash)
        binding.loginButton.setOnClickListener(onLoginClicked)
    }

    private fun checkPermissions() {
        if (isInternetDisabled(this)) {
            show(this, getString(R.string.message_internet_connection), AlertDialogBuilder.DialogButton.CANCEL_RETRY, AlertDialogEvent { result: DialogResult ->
                if (result === DialogResult.RETRY) {
                    checkPermissions()
                } else {
                    finishAffinity()
                }
            })
            return
        }
        if(getInstance(applicationContext).token != null) {
            tryConnect(getInstance(applicationContext).token!!)
        } else {
            goToLoginMode()
        }
    }

    private fun tryConnect(jwtToken: String) {
        FirebaseInstanceId.getInstance().instanceId.addOnCompleteListener {fb ->
            SocketNetworkDispatcher.instance.connect(Namespace.Driver, jwtToken, fb.result?.token ?: "") {
                when (it) {
                    is RemoteResponse.Success -> {
                        startMainActivity()
                    }

                    is RemoteResponse.Error -> {
                        when (it.error) {
                            ConnectionError.RegistrationIncomplete -> {
                                runOnUiThread {
                                    showRegisterForm(jwtToken)
                                }
                            }

                            else -> {
                                runOnUiThread {
                                    goToLoginMode()
                                    try {
                                        it.error.showAlert(this)
                                    } catch (exception: Exception) {

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private fun showRegisterForm(jwtToken: String) {
        GetRegisterInfo(jwtToken).execute<RegistrationInfo> {
            when(it) {
                is RemoteResponse.Success -> {
                    runOnUiThread {
                        preferences.driver = it.body.driver
                        preferences.services = ArrayList(it.body.services)
                        val intent = Intent(this@SplashActivity, ProfileActivity::class.java)
                        intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                        startActivity(intent)
                    }
                }

                is RemoteResponse.Error -> {
                }
            }
        }
    }

    override fun onResume() {
        super.onResume()
        checkPermissions()
    }

    private fun startMainActivity() {
        if (startRequested) return
        startRequested = true
        val intent = Intent(this@SplashActivity, MainActivity::class.java)
        intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
        startActivity(intent)
    }

    private fun tryLogin(firebaseToken: String) {
        goToLoadingMode()
        Login(firebaseToken).execute<LoginResult> {
            when(it) {
                is RemoteResponse.Success -> {
                    getInstance(applicationContext).driver = it.body.user
                    getInstance(applicationContext).token = it.body.token
                    tryConnect(it.body.token)
                }
                is RemoteResponse.Error -> {
                    showError(this, it.error.localizedDescription)
                }
            }
        }
    }

    private fun goToLoadingMode() {
        binding.loginButton.visibility = View.GONE
        binding.progressBar.visibility = View.VISIBLE
    }

    private fun goToLoginMode() {
        binding.loginButton.visibility = View.VISIBLE
        binding.progressBar.visibility = View.GONE
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (requestCode == SIGN_IN_ACTIVITY) {
            if (resultCode == Activity.RESULT_OK) {
                FirebaseAuth.getInstance().currentUser!!.getIdToken(false).addOnCompleteListener {
                    tryLogin(it.result!!.token!!)
                }
                return
            }
            goToLoginMode()
        }
    }
}
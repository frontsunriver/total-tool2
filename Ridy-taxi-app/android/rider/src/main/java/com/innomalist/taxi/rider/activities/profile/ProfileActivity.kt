package com.innomalist.taxi.rider.activities.profile

import android.Manifest
import android.app.Activity
import android.content.Intent
import android.content.pm.PackageManager
import android.graphics.Color
import android.net.Uri
import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import android.widget.ArrayAdapter
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.databinding.DataBindingUtil
import com.esafirm.imagepicker.features.ImagePicker
import com.esafirm.imagepicker.features.ReturnMode
import com.google.android.material.dialog.MaterialAlertDialogBuilder
import com.innomalist.taxi.common.components.BaseActivity
import com.innomalist.taxi.common.models.Media
import com.innomalist.taxi.common.models.Rider
import com.innomalist.taxi.common.networking.socket.interfaces.EmptyClass
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import com.innomalist.taxi.common.utils.DataBinder
import com.innomalist.taxi.rider.R
import com.innomalist.taxi.rider.databinding.ActivityEditProfileBinding
import com.innomalist.taxi.rider.networking.socket.UpdateProfile
import com.innomalist.taxi.rider.networking.socket.UpdateProfileImage
import com.yalantis.ucrop.UCrop
import java.io.File
import java.io.FileInputStream
import java.util.*

class ProfileActivity : BaseActivity() {
    lateinit var binding: ActivityEditProfileBinding
    var rider: Rider? = null
    private val requestWriteStoragePermission = 402

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = DataBindingUtil.setContentView(this, R.layout.activity_edit_profile)
        val adapter = ArrayAdapter(this, android.R.layout.simple_dropdown_item_1line, resources.getStringArray(R.array.genders))
        binding.genderAutocomplete.setAdapter(adapter)
        rider = preferences.rider
        binding.user = rider
        DataBinder.setMedia(binding.profileImage, rider!!.media)
        binding.fabAddPhoto.setOnClickListener{ profileImageClicked() }
        initializeToolbar("")
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        menuInflater.inflate(R.menu.actionbar_save, menu)
        return super.onCreateOptionsMenu(menu)
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        UpdateProfile(rider!!).execute<EmptyClass> {
            when(it) {
                is RemoteResponse.Success -> {
                    setResult(Activity.RESULT_OK)
                    saveUserInfo()
                    finish()
                }

                is RemoteResponse.Error -> {
                    it.error.showAlert(this)
                }
            }

        }
        return super.onOptionsItemSelected(item)
    }

    private fun saveUserInfo() {
        preferences.rider = rider
    }

    private fun profileImageClicked() {
        if(ContextCompat.checkSelfPermission(this, Manifest.permission.WRITE_EXTERNAL_STORAGE) == PackageManager.PERMISSION_GRANTED) {
            ImagePicker.create(this@ProfileActivity)
                    .returnMode(ReturnMode.ALL) // set whether pick and / or camera action should return immediate result or not.
                    .folderMode(true) // folder mode (false by default)
                    .toolbarFolderTitle(getString(R.string.picker_folder)) // folder selection title
                    .toolbarImageTitle(getString(R.string.picker_tap_select)) // image selection title
                    .toolbarArrowColor(Color.WHITE) // Toolbar 'up' arrow color
                    .single() // single mode
                    .limit(10) // max images can be selected (99 by default)
                    .showCamera(true) // show camera or not (true by default)
                    .imageDirectory("Camera") // directory name for captured image  ("Camera" folder by default)
                    .theme(R.style.ImagePickerTheme) // must inherit ef_BaseTheme. please refer to sample
                    .start()
        } else {
            MaterialAlertDialogBuilder(this)
                    .setTitle(R.string.message_default_title)
                    .setMessage(R.string.message_write_storage_profile)
                    .setPositiveButton(R.string.alert_ok) { _, _ ->
                        ActivityCompat.requestPermissions(this, arrayOf(Manifest.permission.WRITE_EXTERNAL_STORAGE), requestWriteStoragePermission)
                    }
                    .setNegativeButton(R.string.alert_cancel, null)
                    .show()
        }
    }

    override fun onRequestPermissionsResult(requestCode: Int, permissions: Array<out String>, grantResults: IntArray) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        if(requestCode == requestWriteStoragePermission) {
            if(!grantResults.contains(-1)) {
                profileImageClicked()
            }
        }
    }

    public override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (ImagePicker.shouldHandle(requestCode, resultCode, data)) {
            val image = ImagePicker.getFirstImageOrNull(data)
            val destinationUri = Uri.fromFile(File(cacheDir, "p.jpg"))
            val options = UCrop.Options()
            options.setStatusBarColor(ContextCompat.getColor(this, R.color.black))
            options.setToolbarColor(ContextCompat.getColor(this, R.color.black))
            options.setToolbarWidgetColor(ContextCompat.getColor(this, R.color.white))
            UCrop.of(Uri.fromFile(File(image.path)), destinationUri)
                    .withAspectRatio(1f, 1f)
                    .withMaxResultSize(200, 200)
                    .withOptions(options)
                    .start(this@ProfileActivity)
        }
        if (resultCode == Activity.RESULT_OK && requestCode == UCrop.REQUEST_CROP) {
            val resultUri = UCrop.getOutput(data!!) ?: return
            val file = File(resultUri.path!!)
            val dt = ByteArray(file.length().toInt())
            FileInputStream(file).read(dt)
            UpdateProfileImage(data = dt).execute<Media> {
                when(it) {
                    is RemoteResponse.Success -> {
                        rider!!.media = it.body
                        saveUserInfo()
                        binding.user!!.media = it.body
                    }

                    is RemoteResponse.Error -> {
                        it.error.showAlert(this)
                    }
                }

            }
        } else if (resultCode == UCrop.RESULT_ERROR) try {
            throw Objects.requireNonNull(UCrop.getError(data!!)!!)
        } catch (throwable: Throwable) {
            throwable.printStackTrace()
        }
    }
}
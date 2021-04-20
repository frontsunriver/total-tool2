package com.innomalist.taxi.driver.activities.profile

import android.Manifest
import android.app.Activity
import android.content.Intent
import android.content.pm.PackageManager
import android.graphics.Color
import android.net.Uri
import android.os.AsyncTask
import android.os.Build
import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import android.view.View
import android.widget.ArrayAdapter
import androidx.core.content.ContextCompat
import androidx.databinding.DataBindingUtil
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.bumptech.glide.Glide
import com.esafirm.imagepicker.features.ImagePicker
import com.esafirm.imagepicker.features.ReturnMode
import com.innomalist.taxi.common.Config
import com.innomalist.taxi.common.components.BaseActivity
import com.innomalist.taxi.common.interfaces.AlertDialogEvent
import com.innomalist.taxi.common.interfaces.IDocumentEvent
import com.innomalist.taxi.common.models.Driver
import com.innomalist.taxi.common.models.Media
import com.innomalist.taxi.common.models.Service
import com.innomalist.taxi.common.networking.socket.interfaces.EmptyClass
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import com.innomalist.taxi.common.utils.AlertDialogBuilder
import com.innomalist.taxi.common.utils.AlertDialogBuilder.show
import com.innomalist.taxi.common.utils.AlerterHelper
import com.innomalist.taxi.common.utils.CommonUtils
import com.innomalist.taxi.common.utils.Validators.validateEmailAddress
import com.innomalist.taxi.driver.R
import com.innomalist.taxi.driver.activities.profile.adapters.DocumentsRecyclerViewAdapter
import com.innomalist.taxi.driver.activities.profile.adapters.ServicesRecyclerViewAdapter
import com.innomalist.taxi.driver.activities.profile.adapters.ServicesRecyclerViewAdapter.OnServiceItemInteractionListener
import com.innomalist.taxi.driver.databinding.ActivityEditProfileBinding
import com.innomalist.taxi.driver.networking.http.Register
import com.yalantis.ucrop.UCrop
import java.io.*
import java.net.HttpURLConnection
import java.net.URL

class ProfileActivity : BaseActivity(), OnServiceItemInteractionListener {
    lateinit var binding: ActivityEditProfileBinding
    lateinit var driver: Driver
    var imageToUpload = ImageToUpload.Profile
    var documents: ArrayList<Media>? = null
    var documentsRecyclerViewAdapter: DocumentsRecyclerViewAdapter? = null
    
    override fun onChecked(service: Service) {
        if(driver.services == null) {
            driver.services = listOf(service)
            return
        }
        if (driver.services!!.find { x -> x.id == service.id } == null) {
            val m = driver.services!!.toMutableList()
            m.add(service)
            driver.services = m
        }
    }

    override fun onUnchecked(service: Service) {
        if (driver.services!!.find { x -> x.id == service.id } != null) {
            val m = driver.services!!.toMutableList()
            m.remove(service)
            driver.services = m
        }
    }

    enum class ImageToUpload {
        Profile, Document
    }
    override fun onCreate(savedInstanceState: Bundle?) {
        shouldReconnect = false
        super.onCreate(savedInstanceState)
        binding = DataBindingUtil.setContentView(this, R.layout.activity_edit_profile)
        val adapter = ArrayAdapter(this, android.R.layout.simple_dropdown_item_1line, resources.getStringArray(R.array.genders))
        binding.genderAutocomplete.setAdapter(adapter)
        binding.profileImage.setOnClickListener(onProfileImageClicked)
        initializeToolbar("")
        toolbar!!.setHomeAsUpIndicator(R.drawable.ic_close)
        driver = preferences.driver!!

        if (driver.status == Driver.Status.SoftReject) binding.textNote.text = getString(R.string.soft_reject_notice, preferences.driver!!.documentsNote) else binding.layoutNote.visibility = View.GONE
        binding.user = driver
        documents = ArrayList(driver.documents!!)
        if (documents == null) {
            documents = ArrayList()
        }
        val linearLayoutManager = LinearLayoutManager(this@ProfileActivity, RecyclerView.HORIZONTAL, false)
        binding.documents.layoutManager = linearLayoutManager
        documentsRecyclerViewAdapter = DocumentsRecyclerViewAdapter(documents!!) { }
        binding.documents.adapter = documentsRecyclerViewAdapter
        val services: List<Service> = preferences.services ?: ArrayList()
        val servicesLayoutManager = LinearLayoutManager(this@ProfileActivity, RecyclerView.VERTICAL, false)
        binding.services.layoutManager = servicesLayoutManager
        val servicesRecyclerViewAdapter = ServicesRecyclerViewAdapter(services, this@ProfileActivity)
        binding.services.adapter = servicesRecyclerViewAdapter
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        menuInflater.inflate(R.menu.actionbar_save, menu)
        return super.onCreateOptionsMenu(menu)
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        var failed = false
        if (binding.firstNameTextLayout.editText!!.text.toString().isEmpty()) {
            binding.firstNameTextLayout.error = "Field Can't be empty"
            failed = true
        } else {
            binding.firstNameTextLayout.isErrorEnabled = false
        }
        if (binding.lastNameTextLayout.editText!!.text.toString().isEmpty()) {
            binding.lastNameTextLayout.error = "Field Can't be empty"
            failed = true
        } else {
            binding.lastNameTextLayout.isErrorEnabled = false
        }
        if (binding.carColorTextLayout.editText!!.text.toString().isEmpty()) {
            binding.carColorTextLayout.error = "Field Can't be empty"
            failed = true
        } else {
            binding.carColorTextLayout.isErrorEnabled = false
        }
        if (binding.plateNumTextLayout.editText!!.text.toString().isEmpty()) {
            binding.plateNumTextLayout.error = "Field Can't be empty"
            failed = true
        } else {
            binding.plateNumTextLayout.isErrorEnabled = false
        }
        if (binding.carYearTextLayout.editText!!.text.toString().isEmpty()) {
            binding.carYearTextLayout.error = "Field Can't be empty"
            failed = true
        } else {
            binding.carYearTextLayout.isErrorEnabled = false
        }
        if (binding.certificateTextLayout.editText!!.text.toString().isEmpty()) {
            binding.certificateTextLayout.error = "Field Can't be empty"
            failed = true
        } else {
            binding.certificateTextLayout.isErrorEnabled = false
        }
        if (binding.accountNumberTextLayout.editText!!.text.toString().isEmpty()) {
            binding.accountNumberTextLayout.error = "Field Can't be empty"
            failed = true
        } else {
            binding.accountNumberTextLayout.isErrorEnabled = false
        }
        if (binding.addressTextLayout.editText!!.text.toString().isEmpty()) {
            binding.addressTextLayout.error = "Field Can't be empty"
            failed = true
        } else {
            binding.addressTextLayout.isErrorEnabled = false
        }
        if (binding.emailTextLayout.editText!!.text.toString().isEmpty() || !validateEmailAddress(binding.emailTextLayout.editText!!.text.toString())) {
            binding.emailTextLayout.error = "E-mail invalid"
            failed = true
        } else {
            binding.emailTextLayout.isErrorEnabled = false
        }
        if (failed) return false
        if (documents!!.isEmpty()) {
            show(this, "You need to upload your documentations.", event = null)
            return false
        }
        Register(preferences.token!!, driver).execute<EmptyClass> {
            when(it) {
                is RemoteResponse.Success -> {
                    show(this, "Your registration info was saved. App will close now. You can open app later to see Administrator approval status.", AlertDialogBuilder.DialogButton.OK, AlertDialogEvent {
                        preferences.driver = driver
                        setResult(Activity.RESULT_OK)
                        finish()
                    })
                }

                is RemoteResponse.Error -> {
                    AlerterHelper.showError(this, it.error.localizedDescription)
                }
            }
        }
        return super.onOptionsItemSelected(item)
    }

    private var onProfileImageClicked: View.OnClickListener = View.OnClickListener {
        imageToUpload = ImageToUpload.Profile
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            requestPermissions(arrayOf(Manifest.permission.WRITE_EXTERNAL_STORAGE), CommonUtils.MY_PERMISSIONS_REQUEST_STORAGE)
        } else {
            openPicker()
        }
    }

    private fun openPicker() {
        ImagePicker.create(this@ProfileActivity)
                .returnMode(ReturnMode.ALL) // set whether pick and / or camera action should return immediate result or not.
                .folderMode(true) // folder mode (false by default)
                .toolbarFolderTitle(getString(R.string.picker_folder)) // folder selection title
                .toolbarImageTitle(getString(R.string.picker_tap_select)) // image selection title
                .toolbarArrowColor(Color.WHITE) // Toolbar 'up' arrow color
                .single() // single mode
                .theme(R.style.ImagePickerTheme) // must inherit ef_BaseTheme. please refer to sample
                .start()
    }


    override fun onRequestPermissionsResult(requestCode: Int, permissions: Array<String>, grantResults: IntArray) {
        when (requestCode) {
            CommonUtils.MY_PERMISSIONS_REQUEST_STORAGE -> {
                // If request is cancelled, the result arrays are empty.
                if (grantResults.isNotEmpty() && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                    openPicker()
                } else {
                    show(this@ProfileActivity, getString(R.string.prompt_storage_rw), event = null)
                }
            }
        }
    }

    public override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (ImagePicker.shouldHandle(requestCode, resultCode, data)) { // or get a single image only
            val image = ImagePicker.getFirstImageOrNull(data)
            val destinationUri = Uri.fromFile(File(cacheDir, "p.jpg"))
            val options = UCrop.Options()
            options.setStatusBarColor(ContextCompat.getColor(this, R.color.black))
            options.setToolbarColor(ContextCompat.getColor(this, R.color.black))
            options.setToolbarWidgetColor(ContextCompat.getColor(this, R.color.white))
            val uCrop = UCrop.of(Uri.fromFile(File(image.path)), destinationUri).withOptions(options)
            if(imageToUpload == ImageToUpload.Profile) {
                uCrop.withAspectRatio(1f, 1f)
                        .withMaxResultSize(200, 200)
            }
            uCrop.start(this@ProfileActivity)
            return
        }
        if (resultCode == Activity.RESULT_OK && requestCode == UCrop.REQUEST_CROP) {
            val resultUri = UCrop.getOutput(data!!) ?: return
            when (imageToUpload) {
                ImageToUpload.Profile -> UploadDocument().execute("driver image", resultUri.path)
                ImageToUpload.Document -> UploadDocument().execute("document", resultUri.path)
            }
        } else if (resultCode == UCrop.RESULT_ERROR) try {
            throw UCrop.getError(data!!)!!
        } catch (throwable: Throwable) {
            throwable.printStackTrace()
        }
    }

    fun onUploadDocument(view: View?) {
        imageToUpload = ImageToUpload.Document
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            requestPermissions(arrayOf(Manifest.permission.WRITE_EXTERNAL_STORAGE), CommonUtils.MY_PERMISSIONS_REQUEST_STORAGE)
        } else {
            openPicker()
        }
    }

    private inner class UploadDocument : AsyncTask<String, Void, String>() {
        override fun doInBackground(vararg params: String): String {
            return try {
                val conn: HttpURLConnection?
                val dos: DataOutputStream?
                val lineEnd = "\r\n"
                val twoHyphens = "--"
                val boundary = "*****"
                var bytesRead: Int
                var bytesAvailable: Int
                var bufferSize: Int
                val buffer: ByteArray
                val maxBufferSize = 1024 * 1024
                val sourceFile = File(params[1])
                if (sourceFile.isFile) {
                    try {
                        val upLoadServerUri = "${Config.Backend}driver/upload"
                        val fileName = "doc.jpg"
                        val fileInputStream = FileInputStream(sourceFile)
                        val url = URL(upLoadServerUri)
                        conn = url.openConnection() as HttpURLConnection
                        conn.doInput = true
                        conn.doOutput = true
                        conn.useCaches = false
                        conn.requestMethod = "POST"
                        conn.setRequestProperty("Connection", "Keep-Alive")
                        conn.setRequestProperty("ENCTYPE", "multipart/form-data")
                        conn.setRequestProperty("Content-Type", "multipart/form-data;boundary=$boundary")
                        conn.setRequestProperty("number", this@ProfileActivity.driver.mobileNumber.toString())
                        conn.setRequestProperty("type", params[0])
                        dos = DataOutputStream(conn.outputStream)
                        dos.writeBytes(twoHyphens + boundary + lineEnd)
                        dos.writeBytes("Content-Disposition: form-data; name=\"file\";filename=\"$fileName\"$lineEnd")
                        dos.writeBytes(lineEnd)
                        bytesAvailable = fileInputStream.available()
                        bufferSize = Math.min(bytesAvailable, maxBufferSize)
                        buffer = ByteArray(bufferSize)
                        bytesRead = fileInputStream.read(buffer, 0, bufferSize)
                        while (bytesRead > 0) {
                            dos.write(buffer, 0, bufferSize)
                            bytesAvailable = fileInputStream.available()
                            bufferSize = Math.min(bytesAvailable, maxBufferSize)
                            bytesRead = fileInputStream.read(buffer, 0, bufferSize)
                        }
                        dos.writeBytes(lineEnd)
                        dos.writeBytes(twoHyphens + boundary + twoHyphens + lineEnd)
                        val serverResponseCode = conn.responseCode
                        fileInputStream.close()
                        dos.flush()
                        dos.close()
                        if (serverResponseCode == 200) {
                            val br = BufferedReader(InputStreamReader(conn.inputStream))
                            val sb = StringBuilder()
                            var output: String?
                            while (br.readLine().also { output = it } != null) {
                                sb.append(output)
                                return sb.toString()
                            }
                            "OK"
                        } else {
                            "FAILED"
                        }
                    } catch (e: Exception) {
                        e.printStackTrace()
                        "FAILED"
                    }
                } else {
                    "FAILED"
                }
            } catch (ex: Exception) {
                ex.printStackTrace()
                "FAILED"
            }
        }

        override fun onPostExecute(result: String) {
            if (result == "FAILED") {
                show(this@ProfileActivity, result, event = null)
                return
            }
            when (imageToUpload) {
                ImageToUpload.Profile -> Glide.with(this@ProfileActivity).load(Config.Backend + result.replace("\"", "")).into(binding.profileImage)
                ImageToUpload.Document -> {
                    documents!!.add(Media(result.replace("\"", ""), Media.PathType.relative))
                    documentsRecyclerViewAdapter = DocumentsRecyclerViewAdapter(documents!!.toList(), IDocumentEvent { })
                    binding.documents.adapter = documentsRecyclerViewAdapter
                }
            }
        }

        override fun onPreExecute() {}
        override fun onProgressUpdate(vararg values: Void) {}
    }
}
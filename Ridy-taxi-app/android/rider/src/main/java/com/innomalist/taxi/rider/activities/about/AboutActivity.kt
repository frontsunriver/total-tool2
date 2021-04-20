package com.innomalist.taxi.rider.activities.about

import android.os.Bundle
import android.view.View
import com.innomalist.taxi.common.components.BaseActivity
import com.innomalist.taxi.rider.BuildConfig
import com.innomalist.taxi.rider.R
import de.psdev.licensesdialog.LicensesDialog
import de.psdev.licensesdialog.licenses.ApacheSoftwareLicense20
import de.psdev.licensesdialog.licenses.BSD3ClauseLicense
import de.psdev.licensesdialog.licenses.MITLicense
import de.psdev.licensesdialog.model.Notice
import de.psdev.licensesdialog.model.Notices
import mehdi.sakout.aboutpage.AboutPage
import mehdi.sakout.aboutpage.Element

class AboutActivity : BaseActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val versionElement = Element().setTitle(getString(R.string.version_name, BuildConfig.VERSION_NAME))
        val licenseElement = Element().setTitle(getString(R.string.legal_notices)).setIconDrawable(R.drawable.copyright).setOnClickListener {
            val notices = Notices()
            notices.addNotice(Notice("AndroidX Library", "https://developer.android.com/topic/libraries/support-library/index.html", "Google Inc.", ApacheSoftwareLicense20()))
            notices.addNotice(Notice("uCrop", "https://github.com/yalantis/ucrop", "Yalantis", ApacheSoftwareLicense20()))
            notices.addNotice(Notice("Lottie", "https://github.com/airbnb/lottie-android", "AirBnb", ApacheSoftwareLicense20()))
            notices.addNotice(Notice("Glide", "https://github.com/bumptech/glide", "Google Inc.", BSD3ClauseLicense()))
            notices.addNotice(Notice("Google Play Services", "https://developer.android.com/index.html", "Google Inc.", ApacheSoftwareLicense20()))
            notices.addNotice(Notice("android-maps-utils", "https://github.com/googlemaps/android-maps-utils", "Google Inc.", ApacheSoftwareLicense20()))
            notices.addNotice(Notice("Alerter", "https://github.com/Tapadoo/Alerter", "Tapadoo, Dublin", MITLicense()))
            notices.addNotice(Notice("Android About Page", "https://github.com/medyo/android-about-page", "Mehdi Sakout", MITLicense()))
            LicensesDialog.Builder(this@AboutActivity)
                    .setNotices(notices)
                    .setIncludeOwnLicense(true)
                    .build()
                    .show()
        }
        val aboutPage = AboutPage(this@AboutActivity)
                .setImage(R.drawable.logo)
                .addItem(versionElement)
                .addGroup(getString(R.string.about_contacts))
        if (getString(R.string.email) != "") aboutPage.addEmail(getString(R.string.email))
        if (getString(R.string.website) != "") aboutPage.addWebsite(getString(R.string.website))
        if (getString(R.string.privacy_policy_url) != "") aboutPage.addWebsite(getString(R.string.privacy_policy_url), getString(R.string.privacy_policy))
        if (getString(R.string.twitter) != "") aboutPage.addTwitter(getString(R.string.twitter))
        if (getString(R.string.instagram) != "") aboutPage.addInstagram(getString(R.string.instagram))
        if (getString(R.string.facebook) != "") aboutPage.addFacebook(getString(R.string.facebook))
        if (getString(R.string.play_store_rider) != "") aboutPage.addPlayStore(getString(R.string.play_store_rider))
        aboutPage.addItem(licenseElement)
        aboutPage.setDescription(getString(R.string.about_rider_description))
        val view = aboutPage.create()
        setContentView(view)
    }
}
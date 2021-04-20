using System;
using Android.App;
using Android.Content;
using Android.OS;

using Android.Views;
using AndroidX.Preference;
using WoWonder.Helpers.Utils;
using WoWonderClient;



namespace WoWonder.Activities.SettingsPreferences.Support
{
    public class SettingsSupportPrefsFragment : PreferenceFragmentCompat, ISharedPreferencesOnSharedPreferenceChangeListener
    {
        #region Variables Basic

        private Preference HelpPref,ReportProblemPref, AboutAppPref, RateAppPref, PrivacyPolicyPref, TermsOfUsePref; 
        private readonly Activity ActivityContext;

        #endregion 

        #region General

        public SettingsSupportPrefsFragment(Activity context)
        {
            try
            {
                ActivityContext = context;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                // create ContextThemeWrapper from the original Activity Context with the custom theme
                Context contextThemeWrapper = AppSettings.SetTabDarkTheme ? new ContextThemeWrapper(ActivityContext, Resource.Style.SettingsThemeDark) : new ContextThemeWrapper(ActivityContext, Resource.Style.SettingsTheme);

                // clone the inflater using the ContextThemeWrapper
                LayoutInflater localInflater = inflater.CloneInContext(contextThemeWrapper);

                View view = base.OnCreateView(localInflater, container, savedInstanceState);

                return view;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }

        public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
        {
            try
            {
                // Load the preferences from an XML resource
                AddPreferencesFromResource(Resource.Xml.SettingsPrefs_Help_Support);

                MainSettings.SharedData = PreferenceManager.SharedPreferences;
                InitComponent();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override void OnResume()
        {
            try
            {
                base.OnResume();
                PreferenceManager.SharedPreferences.RegisterOnSharedPreferenceChangeListener(this);
                AddOrRemoveEvent(true);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override void OnPause()
        {
            try
            {
                base.OnPause();
                PreferenceScreen.SharedPreferences.UnregisterOnSharedPreferenceChangeListener(this);
                AddOrRemoveEvent(false);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override void OnLowMemory()
        {
            try
            {
                GC.Collect(GC.MaxGeneration);
                base.OnLowMemory();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }


        #endregion

        #region Functions

        private void InitComponent()
        {
            try
            {
                MainSettings.SharedData = PreferenceManager.SharedPreferences;
                PreferenceManager.SharedPreferences.RegisterOnSharedPreferenceChangeListener(this);

                HelpPref = FindPreference("help_key");
                ReportProblemPref = FindPreference("Report_key");
                AboutAppPref = FindPreference("About_key");
                RateAppPref = FindPreference("RateApp_key");
                PrivacyPolicyPref = FindPreference("PrivacyPolicy_key");
                TermsOfUsePref = FindPreference("TermsOfUse_key");

                //Delete Preference
                var mCategorySupport = (PreferenceCategory)FindPreference("SectionSupport_key");
                switch (AppSettings.ShowSettingsHelp)
                {
                    case false:
                        mCategorySupport.RemovePreference(HelpPref);
                        break;
                }

                switch (AppSettings.ShowSettingsReportProblem)
                {
                    case false:
                        mCategorySupport.RemovePreference(ReportProblemPref);
                        break;
                }

                var mCategoryAbout = (PreferenceCategory)FindPreference("SectionAbout_key");
                switch (AppSettings.ShowSettingsAbout)
                {
                    case false:
                        mCategoryAbout.RemovePreference(AboutAppPref);
                        break;
                }

                switch (AppSettings.ShowSettingsRateApp)
                {
                    case false:
                        mCategoryAbout.RemovePreference(RateAppPref);
                        break;
                }

                switch (AppSettings.ShowSettingsPrivacyPolicy)
                {
                    case false:
                        mCategoryAbout.RemovePreference(PrivacyPolicyPref);
                        break;
                }

                switch (AppSettings.ShowSettingsTermsOfUse)
                {
                    case false:
                        mCategoryAbout.RemovePreference(TermsOfUsePref);
                        break;
                }
                  
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void AddOrRemoveEvent(bool addEvent)
        {
            try
            {
                switch (addEvent)
                {
                    // true +=  // false -=
                    case true:
                        HelpPref.PreferenceClick += HelpPref_OnPreferenceClick;
                        ReportProblemPref.PreferenceClick += ReportProblemPref_OnPreferenceClick;
                        AboutAppPref.PreferenceClick += AboutAppPref_OnPreferenceClick;
                        PrivacyPolicyPref.PreferenceClick += PrivacyPolicyPref_OnPreferenceClick;
                        TermsOfUsePref.PreferenceClick += TermsOfUsePref_OnPreferenceClick;
                        RateAppPref.PreferenceClick += RateAppPrefOnPreferenceClick;
                        break;
                    default:
                        HelpPref.PreferenceClick -= HelpPref_OnPreferenceClick;
                        ReportProblemPref.PreferenceClick -= ReportProblemPref_OnPreferenceClick;
                        AboutAppPref.PreferenceClick -= AboutAppPref_OnPreferenceClick;
                        PrivacyPolicyPref.PreferenceClick -= PrivacyPolicyPref_OnPreferenceClick;
                        TermsOfUsePref.PreferenceClick -= TermsOfUsePref_OnPreferenceClick;
                        RateAppPref.PreferenceClick -= RateAppPrefOnPreferenceClick;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void RateAppPrefOnPreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            try
            {
                StoreReviewApp store = new StoreReviewApp();
                store.OpenStoreReviewPage(Activity.PackageName);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Events

        //Terms Of Use
        private void TermsOfUsePref_OnPreferenceClick(object sender, Preference.PreferenceClickEventArgs preferenceClickEventArgs)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(LocalWebViewActivity));
                intent.PutExtra("URL", Client.WebsiteUrl + "/terms/terms");
                intent.PutExtra("Type", ActivityContext.GetString(Resource.String.Lbl_TermsOfUse));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Privacy Policy
        private void PrivacyPolicyPref_OnPreferenceClick(object sender, Preference.PreferenceClickEventArgs preferenceClickEventArgs)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(LocalWebViewActivity));
                intent.PutExtra("URL", Client.WebsiteUrl + "/terms/privacy-policy");
                intent.PutExtra("Type", ActivityContext.GetString(Resource.String.Privacy_Policy));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //About Us
        private void AboutAppPref_OnPreferenceClick(object sender, Preference.PreferenceClickEventArgs preferenceClickEventArgs)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(LocalWebViewActivity));
                intent.PutExtra("URL", Client.WebsiteUrl + "/terms/about-us");
                intent.PutExtra("Type", ActivityContext.GetString(Resource.String.Lbl_About_App));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Report a Problem
        private void ReportProblemPref_OnPreferenceClick(object sender, Preference.PreferenceClickEventArgs preferenceClickEventArgs)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(LocalWebViewActivity));
                intent.PutExtra("URL", Client.WebsiteUrl + "/contact-us");
                intent.PutExtra("Type", ActivityContext.GetString(Resource.String.Lbl_Report_Problem));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Help
        private void HelpPref_OnPreferenceClick(object sender, Preference.PreferenceClickEventArgs preferenceClickEventArgs)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(LocalWebViewActivity));
                intent.PutExtra("URL", Client.WebsiteUrl + "/contact-us");
                intent.PutExtra("Type", ActivityContext.GetString(Resource.String.Lbl_Help));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
             
        }
    }
} 
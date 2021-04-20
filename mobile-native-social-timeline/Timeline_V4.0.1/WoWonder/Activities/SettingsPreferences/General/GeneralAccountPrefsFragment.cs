using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Preference;
using Java.Lang;
using WoWonder.Activities.BlockedUsers;
using WoWonder.Activities.MyProfile;
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using WoWonderClient.Requests;
using Exception = System.Exception;

namespace WoWonder.Activities.SettingsPreferences.General
{
    public class GeneralAccountPrefsFragment : PreferenceFragmentCompat, ISharedPreferencesOnSharedPreferenceChangeListener, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback, MaterialDialog.IInputCallback
    { 
        #region  Variables Basic

        private Preference EditProfilePref,EditAccountPref, EditSocialLinksPref, EditPasswordPref, BlockedUsersPref, DeleteAccountPref, AboutMePref,TwoFactorPref, ManageSessionsPref , VerificationPref; 
        //private ListPreference LangPref;
        private string SAbout = "", TypeDialog; 
        private readonly Activity ActivityContext;
        private Preference NightMode;

        #endregion

        #region General

        public GeneralAccountPrefsFragment(Activity context)
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
                AddPreferencesFromResource(Resource.Xml.SettingsPrefs_GeneralAccount);

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

                EditProfilePref = FindPreference("editprofile_key");
                AboutMePref = FindPreference("about_me_key");
                EditAccountPref = FindPreference("editAccount_key");
                EditSocialLinksPref = FindPreference("editSocialLinks_key");
                EditPasswordPref = FindPreference("editpassword_key");
                BlockedUsersPref = FindPreference("blocked_key");
                DeleteAccountPref = FindPreference("deleteaccount_key");
                TwoFactorPref = FindPreference("Twofactor_key");
                ManageSessionsPref = FindPreference("ManageSessions_key"); 
                NightMode = FindPreference("Night_Mode_key");
                VerificationPref = FindPreference("verification_key");
                 
                //LangPref = (ListPreference) FindPreference("Lang_key");

                //Update Preferences data on Load
                OnSharedPreferenceChanged(MainSettings.SharedData, "about_me_key");
                OnSharedPreferenceChanged(MainSettings.SharedData, "Night_Mode_key");
                //OnSharedPreferenceChanged(MainSettings.SharedData, "Lang_key");
                 
                NightMode.IconSpaceReserved = false;

                //Delete Preference
                var mCategoryAccount = (PreferenceCategory)FindPreference("SectionAccount_key");
                switch (AppSettings.ShowSettingsAccount)
                {
                    case false:
                        mCategoryAccount.RemovePreference(EditAccountPref);
                        break;
                }

                switch (AppSettings.ShowSettingsSocialLinks)
                {
                    case false:
                        mCategoryAccount.RemovePreference(EditSocialLinksPref);
                        break;
                }
            
                switch (AppSettings.ShowSettingsBlockedUsers)
                {
                    case false:
                        mCategoryAccount.RemovePreference(BlockedUsersPref);
                        break;
                }

                switch (AppSettings.ShowSettingsVerification)
                {
                    case false:
                        mCategoryAccount.RemovePreference(VerificationPref);
                        break;
                }

                var mCategorySecurity = (PreferenceCategory)FindPreference("SecurityAccount_key");
                switch (AppSettings.ShowSettingsPassword)
                {
                    case false:
                        mCategorySecurity.RemovePreference(EditPasswordPref);
                        break;
                }
 
                switch (AppSettings.ShowSettingsDeleteAccount)
                {
                    case false:
                        mCategorySecurity.RemovePreference(DeleteAccountPref);
                        break;
                }

                switch (AppSettings.ShowSettingsTwoFactor)
                {
                    case false:
                        mCategorySecurity.RemovePreference(TwoFactorPref);
                        break;
                }
              
                switch (AppSettings.ShowSettingsManageSessions)
                {
                    case false:
                        mCategorySecurity.RemovePreference(ManageSessionsPref);
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
                        //LangPref.PreferenceChange += LangPref_OnPreferenceChange; 
                        EditProfilePref.PreferenceClick += EditProfilePref_OnPreferenceClick;
                        EditAccountPref.PreferenceClick += EditAccountPrefOnPreferenceClick;
                        EditSocialLinksPref.PreferenceClick += EditSocialLinksPref_OnPreferenceClick;
                        EditPasswordPref.PreferenceClick += EditPasswordPref_OnPreferenceClick;
                        BlockedUsersPref.PreferenceClick += BlockedUsersPref_OnPreferenceClick;
                        DeleteAccountPref.PreferenceClick += DeleteAccountPref_OnPreferenceClick;
                        TwoFactorPref.PreferenceClick += TwoFactorPrefOnPreferenceClick;
                        ManageSessionsPref.PreferenceClick += ManageSessionsPrefOnPreferenceClick;
                        VerificationPref.PreferenceClick += VerificationPrefOnPreferenceClick;
                        break;
                    default:
                        //LangPref.PreferenceChange -= LangPref_OnPreferenceChange; 
                        EditProfilePref.PreferenceClick -= EditProfilePref_OnPreferenceClick;
                        EditAccountPref.PreferenceClick -= EditAccountPrefOnPreferenceClick;
                        EditSocialLinksPref.PreferenceClick -= EditSocialLinksPref_OnPreferenceClick;
                        EditPasswordPref.PreferenceClick -= EditPasswordPref_OnPreferenceClick;
                        BlockedUsersPref.PreferenceClick -= BlockedUsersPref_OnPreferenceClick;
                        DeleteAccountPref.PreferenceClick -= DeleteAccountPref_OnPreferenceClick;
                        TwoFactorPref.PreferenceClick -= TwoFactorPrefOnPreferenceClick;
                        ManageSessionsPref.PreferenceClick -= ManageSessionsPrefOnPreferenceClick;
                        VerificationPref.PreferenceClick -= VerificationPrefOnPreferenceClick;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events
         
        //Edit Profile
        private void EditProfilePref_OnPreferenceClick(object sender, Preference.PreferenceClickEventArgs preferenceClickEventArgs)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(EditMyProfileActivity));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Edit Account
        private void EditAccountPrefOnPreferenceClick(object sender, Preference.PreferenceClickEventArgs preferenceClickEventArgs)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(MyAccountActivity));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Edit Social Links
        private void EditSocialLinksPref_OnPreferenceClick(object sender, Preference.PreferenceClickEventArgs preferenceClickEventArgs)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(EditSocialLinksActivity));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Edit Password
        private void EditPasswordPref_OnPreferenceClick(object sender, Preference.PreferenceClickEventArgs preferenceClickEventArgs)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(PasswordActivity));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Block users
        private void BlockedUsersPref_OnPreferenceClick(object sender, Preference.PreferenceClickEventArgs preferenceClickEventArgs)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(BlockedUsersActivity));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Verification
        private void VerificationPrefOnPreferenceClick(object sender, Preference.PreferenceClickEventArgs preferenceClickEventArgs)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(VerificationActivity));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        //Delete Account  
        private void DeleteAccountPref_OnPreferenceClick(object sender, Preference.PreferenceClickEventArgs preferenceClickEventArgs)
        {
            try
            {
                TypeDialog = "DeleteAccount";

                var dialog = new MaterialDialog.Builder(ActivityContext).Theme(AppSettings.SetTabDarkTheme ? Theme.Dark : Theme.Light);

                dialog.Title(Resource.String.Lbl_Warning).TitleColorRes(Resource.Color.primary);
                dialog.Content(ActivityContext.GetText(Resource.String.Lbl_Are_you_DeleteAccount) + " " + AppSettings.ApplicationName);
                dialog.PositiveText(ActivityContext.GetText(Resource.String.Lbl_Ok)).OnPositive(this);
                dialog.NegativeText(ActivityContext.GetText(Resource.String.Lbl_Cancel)).OnNegative(this);
                dialog.Build().Show();
                dialog.AlwaysCallSingleChoiceCallback();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
          
        //TwoFactor
        private void TwoFactorPrefOnPreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(TwoFactorAuthActivity));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //ManageSessions
        private void ManageSessionsPrefOnPreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(ManageSessionsActivity));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }


        //Lang
        //private void LangPref_OnPreferenceChange(object sender, Preference.PreferenceChangeEventArgs eventArgs)
        //{
        //    try
        //    {
        //        if (eventArgs.Handled)
        //        {
        //            var etp = (ListPreference) sender;
        //            var value = eventArgs.NewValue;

        //            AppSettings.Lang = value.ToString();

        //            MainSettings.SetApplicationLang(Activity, AppSettings.Lang);

        //            Toast.MakeText(ActivityContext, GetText(Resource.String.Lbl_Application_Restart), ToastLength.Long)?.Show();

        //            var intent = new Intent(Activity, typeof(SplashScreenActivity));
        //            intent.AddCategory(Intent?.CategoryHome);
        //            intent.SetAction(Intent?.ActionMain);
        //            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask | ActivityFlags.ClearTask);
        //            Activity.StartActivity(intent);
        //            Activity.FinishAffinity();

        //            AppSettings.Lang = value.ToString();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Methods.DisplayReportResultTrack(e);
        //    }
        //}

        #endregion

        //On Change 
        public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
            try
            {
                var dataUser = ListUtils.MyProfileList?.FirstOrDefault();

                switch (key)
                {
                    case "about_me_key":
                    {
                        // Set summary to be the user-description for the selected value
                        Preference etp = FindPreference("about_me_key"); 
                        if (dataUser != null)
                        {
                            SAbout = WoWonderTools.GetAboutFinal(dataUser);

                            MainSettings.SharedData?.Edit()?.PutString("about_me_key", SAbout)?.Commit();
                            etp.Summary = SAbout;
                        }

                        string getvalue = MainSettings.SharedData?.GetString("about_me_key", SAbout);
                        etp.Summary = getvalue;
                        break;
                    }
                    case "Night_Mode_key":
                    {
                        // Set summary to be the user-description for the selected value
                        Preference etp = FindPreference("Night_Mode_key");

                        string getValue = MainSettings.SharedData?.GetString("Night_Mode_key", string.Empty);
                        if (getValue == MainSettings.LightMode)
                        {
                            etp.Summary = ActivityContext.GetString(Resource.String.Lbl_Light);
                        }
                        else if (getValue == MainSettings.DarkMode)
                        {
                            etp.Summary = ActivityContext.GetString(Resource.String.Lbl_Dark);
                        }
                        else if (getValue == MainSettings.DefaultMode)
                        {
                            etp.Summary = ActivityContext.GetString(Resource.String.Lbl_SetByBattery);
                        }
                        else
                        {
                            etp.Summary = getValue;
                        }

                        break;
                    }
                }

                //else if (key.Equals("Lang_key"))
                //{
                //    var valueAsText = LangPref.Entry;
                //    if (!string.IsNullOrEmpty(valueAsText))
                //    {
                //        AppSettings.FlowDirectionRightToLeft = false;
                //        if (valueAsText.ToLower().Contains("english"))
                //        {
                //           // MainSettings.SharedData?.Edit()?.PutString("Lang_key", "en")?.Commit();
                //            LangPref.SetValueIndex(1);
                //        }
                //        else if (valueAsText.ToLower().Contains("arabic"))
                //        {
                //            //MainSettings.SharedData?.Edit()?.PutString("Lang_key", "ar")?.Commit();
                //            LangPref.SetValueIndex(2);
                //            AppSettings.FlowDirectionRightToLeft = true;
                //        }
                //        else if (valueAsText.ToLower().Contains("german"))
                //        {
                //            //MainSettings.SharedData?.Edit()?.PutString("Lang_key", "de")?.Commit();
                //            LangPref.SetValueIndex(3);
                //        }
                //        else if (valueAsText.ToLower().Contains("greek"))
                //        {
                //            //MainSettings.SharedData?.Edit()?.PutString("Lang_key", "el")?.Commit();
                //            LangPref.SetValueIndex(4);
                //        }
                //        else if (valueAsText.ToLower().Contains("spanish"))
                //        {
                //           // MainSettings.SharedData?.Edit()?.PutString("Lang_key", "es")?.Commit();
                //            LangPref.SetValueIndex(5);
                //        }
                //        else if (valueAsText.ToLower().Contains("french"))
                //        {
                //           // MainSettings.SharedData?.Edit()?.PutString("Lang_key", "fr")?.Commit();
                //            LangPref.SetValueIndex(6);
                //        }
                //        else if (valueAsText.ToLower().Contains("italian"))
                //        {
                //          //  MainSettings.SharedData?.Edit()?.PutString("Lang_key", "it")?.Commit();
                //            LangPref.SetValueIndex(7);
                //        }
                //        else if (valueAsText.ToLower().Contains("japanese"))
                //        {
                //           // MainSettings.SharedData?.Edit()?.PutString("Lang_key", "ja")?.Commit();
                //            LangPref.SetValueIndex(8);
                //        }
                //        else if (valueAsText.ToLower().Contains("dutch"))
                //        {
                //           // MainSettings.SharedData?.Edit()?.PutString("Lang_key", "nl")?.Commit();
                //            LangPref.SetValueIndex(9);
                //        }
                //        else if (valueAsText.ToLower().Contains("portuguese"))
                //        {
                //          //  MainSettings.SharedData?.Edit()?.PutString("Lang_key", "pt")?.Commit();
                //            LangPref.SetValueIndex(10);
                //        }
                //        else if (valueAsText.ToLower().Contains("romanian"))
                //        {
                //           // MainSettings.SharedData?.Edit()?.PutString("Lang_key", "ro")?.Commit();
                //            LangPref.SetValueIndex(11);
                //        }
                //        else if (valueAsText.ToLower().Contains("russian"))
                //        {
                //          //  MainSettings.SharedData?.Edit()?.PutString("Lang_key", "ru")?.Commit();
                //            LangPref.SetValueIndex(12);
                //        }
                //        else if (valueAsText.ToLower().Contains("russian"))
                //        {
                //           // MainSettings.SharedData?.Edit()?.PutString("Lang_key", "ru")?.Commit();
                //            LangPref.SetValueIndex(13);
                //        }
                //        else if (valueAsText.ToLower().Contains("albanian"))
                //        {
                //            //MainSettings.SharedData?.Edit()?.PutString("Lang_key", "sq")?.Commit();
                //            LangPref.SetValueIndex(14);
                //        }
                //        else if (valueAsText.ToLower().Contains("serbian"))
                //        {
                //            //MainSettings.SharedData?.Edit()?.PutString("Lang_key", "sr")?.Commit();
                //            LangPref.SetValueIndex(15);
                //        }
                //        else if (valueAsText.ToLower().Contains("turkish"))
                //        {
                //            //MainSettings.SharedData?.Edit()?.PutString("Lang_key", "tr")?.Commit();
                //            LangPref.SetValueIndex(16);
                //        }
                //        else
                //        {
                //           // MainSettings.SharedData?.Edit()?.PutString("Lang_key", "Auto")?.Commit();
                //            LangPref.SetValueIndex(0);
                //        }
                //    }
                //    else
                //    {
                //        //MainSettings.SharedData?.Edit()?.PutString("Lang_key", "Auto")?.Commit();
                //        LangPref.SetValueIndex(0);
                //    }
                //}
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public override bool OnPreferenceTreeClick(Preference preference)
        {
            try
            {
                switch (preference.Key)
                {
                    case "about_me_key":
                    {
                        TypeDialog = "About";
                        var dialog = new MaterialDialog.Builder(ActivityContext).Theme(AppSettings.SetTabDarkTheme ? Theme.Dark : Theme.Light);
                        dialog.Title(GetString(Resource.String.Lbl_About)).TitleColorRes(Resource.Color.primary);
                        dialog.Input(GetString(Resource.String.Lbl_About), preference.Summary, false, this);
                        dialog.InputType(InputTypes.TextFlagImeMultiLine);
                        dialog.PositiveText(GetText(Resource.String.Lbl_Save)).OnPositive(this);
                        dialog.NegativeText(GetText(Resource.String.Lbl_Cancel)).OnNegative(this);
                        dialog.AlwaysCallSingleChoiceCallback();
                        dialog.Build().Show();
                      
                        return true;
                    }
                    case "Night_Mode_key":
                    {
                        TypeDialog = "NightMode";

                        var arrayAdapter = new List<string>();
                        var dialogList = new MaterialDialog.Builder(ActivityContext).Theme(AppSettings.SetTabDarkTheme ? Theme.Dark : Theme.Light);

                        dialogList.Title(Resource.String.Lbl_Theme).TitleColorRes(Resource.Color.primary);

                        arrayAdapter.Add(GetText(Resource.String.Lbl_Light));
                        arrayAdapter.Add(GetText(Resource.String.Lbl_Dark));

                        switch ((int)Build.VERSION.SdkInt)
                        {
                            case >= 29:
                                arrayAdapter.Add(GetText(Resource.String.Lbl_SetByBattery));
                                break;
                        }

                        dialogList.Items(arrayAdapter);
                        dialogList.PositiveText(GetText(Resource.String.Lbl_Close)).OnPositive(this);
                        dialogList.AlwaysCallSingleChoiceCallback();
                        dialogList.ItemsCallback(this).Build().Show();
                 
                        return true;
                    }
                    default:
                        return base.OnPreferenceTreeClick(preference);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return base.OnPreferenceTreeClick(preference);
            }
        }

        #region MaterialDialog

        public void OnClick(MaterialDialog p0, DialogAction p1)
        {
            try
            {
                switch (TypeDialog)
                {
                    case "DeleteAccount" when p1 == DialogAction.Positive:
                    {
                        var intent = new Intent(ActivityContext, typeof(DeleteAccountActivity));
                        ActivityContext.StartActivity(intent);
                        break;
                    }
                    case "DeleteAccount":
                    {
                        if (p1 == DialogAction.Negative)
                        {
                            p0.Dismiss();
                        }

                        break;
                    }
                    default:
                    {
                        if (p1 == DialogAction.Positive)
                        {
                        
                        }
                        else if (p1 == DialogAction.Negative)
                        {
                            p0.Dismiss();
                        }

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                string text = itemString.ToString();
                switch (TypeDialog)
                {
                    case "NightMode":
                    {
                        string getValue = MainSettings.SharedData?.GetString("Night_Mode_key", string.Empty);

                        if (text == GetString(Resource.String.Lbl_Light) && getValue != MainSettings.LightMode)
                        {
                            //Set Light Mode   
                            NightMode.Summary = ActivityContext.GetString(Resource.String.Lbl_Light);

                            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
                            AppSettings.SetTabDarkTheme = false;
                            MainSettings.SharedData?.Edit()?.PutString("Night_Mode_key", MainSettings.LightMode)?.Commit();

                            switch (Build.VERSION.SdkInt)
                            {
                                case >= BuildVersionCodes.Lollipop:
                                    ActivityContext.Window?.ClearFlags(WindowManagerFlags.TranslucentStatus);
                                    ActivityContext.Window?.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                                    break;
                            }

                            Intent intent = new Intent(ActivityContext, typeof(TabbedMainActivity));
                            intent.AddCategory(Intent.CategoryHome);
                            intent.SetAction(Intent.ActionMain);
                            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask | ActivityFlags.ClearTask);
                            intent.AddFlags(ActivityFlags.NoAnimation);
                            ActivityContext.FinishAffinity();
                            ActivityContext.OverridePendingTransition(0, 0);
                            ActivityContext.StartActivity(intent);
                        }
                        else if (text == GetString(Resource.String.Lbl_Dark) && getValue != MainSettings.DarkMode)
                        {
                            NightMode.Summary = ActivityContext.GetString(Resource.String.Lbl_Dark);

                            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightYes;
                            AppSettings.SetTabDarkTheme = true;
                            MainSettings.SharedData?.Edit()?.PutString("Night_Mode_key", MainSettings.DarkMode)?.Commit();

                            switch (Build.VERSION.SdkInt)
                            {
                                case >= BuildVersionCodes.Lollipop:
                                    ActivityContext.Window?.ClearFlags(WindowManagerFlags.TranslucentStatus);
                                    ActivityContext.Window?.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                                    break;
                            }

                            Intent intent = new Intent(ActivityContext, typeof(TabbedMainActivity));
                            intent.AddCategory(Intent.CategoryHome);
                            intent.SetAction(Intent.ActionMain);
                            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask | ActivityFlags.ClearTask);
                            intent.AddFlags(ActivityFlags.NoAnimation);
                            ActivityContext.FinishAffinity();
                            ActivityContext.OverridePendingTransition(0, 0);
                            ActivityContext.StartActivity(intent);
                        }
                        else if (text == GetString(Resource.String.Lbl_SetByBattery) && getValue != MainSettings.DefaultMode)
                        {
                            NightMode.Summary = ActivityContext.GetString(Resource.String.Lbl_SetByBattery);
                            MainSettings.SharedData?.Edit()?.PutString("Night_Mode_key", MainSettings.DefaultMode)?.Commit();

                            switch ((int)Build.VERSION.SdkInt)
                            {
                                case >= 29:
                                {
                                    AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightFollowSystem;

                                    var currentNightMode = Resources?.Configuration?.UiMode & UiMode.NightMask;
                                    AppSettings.SetTabDarkTheme = currentNightMode switch
                                    {
                                        UiMode.NightNo =>
                                            // Night mode is not active, we're using the light theme
                                            false,
                                        UiMode.NightYes =>
                                            // Night mode is active, we're using dark theme
                                            true,
                                        _ => AppSettings.SetTabDarkTheme
                                    };

                                    break;
                                }
                                default:
                                {
                                    AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightAutoBattery;

                                    var currentNightMode = Resources?.Configuration?.UiMode & UiMode.NightMask;
                                    AppSettings.SetTabDarkTheme = currentNightMode switch
                                    {
                                        UiMode.NightNo =>
                                            // Night mode is not active, we're using the light theme
                                            false,
                                        UiMode.NightYes =>
                                            // Night mode is active, we're using dark theme
                                            true,
                                        _ => AppSettings.SetTabDarkTheme
                                    };

                                    switch (Build.VERSION.SdkInt)
                                    {
                                        case >= BuildVersionCodes.Lollipop:
                                            ActivityContext.Window?.ClearFlags(WindowManagerFlags.TranslucentStatus);
                                            ActivityContext.Window?.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                                            break;
                                    }

                                    Intent intent = new Intent(ActivityContext, typeof(TabbedMainActivity));
                                    intent.AddCategory(Intent.CategoryHome);
                                    intent.SetAction(Intent.ActionMain);
                                    intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask | ActivityFlags.ClearTask);
                                    intent.AddFlags(ActivityFlags.NoAnimation);
                                    ActivityContext.FinishAffinity();
                                    ActivityContext.OverridePendingTransition(0, 0);
                                    ActivityContext.StartActivity(intent);
                                    break;
                                }
                            }
                        }

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnInput(MaterialDialog p0, ICharSequence p1)
        {
            try
            {
                if (p1.Length() <= 0) return;

                var strName = p1.ToString();
                if (!string.IsNullOrEmpty(strName) || !string.IsNullOrWhiteSpace(strName))
                {
                    switch (TypeDialog)
                    {
                        case "About":
                        {
                            MainSettings.SharedData?.Edit()?.PutString("about_me_key", strName)?.Commit();
                            AboutMePref.Summary = strName;

                            var dataUser = ListUtils.MyProfileList?.FirstOrDefault();
                            if (dataUser != null)
                            {
                                dataUser.About = strName;
                                SAbout = strName;

                                var sqLiteDatabase = new SqLiteDatabase();
                                sqLiteDatabase.Insert_Or_Update_To_MyProfileTable(dataUser);
                             
                            }

                            if (Methods.CheckConnectivity())
                            {
                                var dataPrivacy = new Dictionary<string, string>
                                {
                                    {"about", strName}
                                };

                                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Global.UpdateUserDataAsync(dataPrivacy) });
                            }
                            else
                            {
                                Toast.MakeText(ActivityContext, ActivityContext.GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion
    }
}
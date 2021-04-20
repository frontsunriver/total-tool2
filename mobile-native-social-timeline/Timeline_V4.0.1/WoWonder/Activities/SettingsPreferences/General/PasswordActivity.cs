using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.Content.Res;
using WoWonder.Activities.Base;
using WoWonder.Activities.Default;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.SettingsPreferences.General
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class PasswordActivity : BaseActivity 
    {
        #region Variables Basic

        private EditText TxtCurrentPassword, TxtNewPassword, TxtRepeatPassword;
        private TextView TxtSave, IconCurrentPassword, IconNewPassword, IconRepeatPassword, TxtLinkForget;

        #endregion

        #region General

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                SetTheme(AppSettings.SetTabDarkTheme ? Resource.Style.MyTheme_Dark_Base : Resource.Style.MyTheme_Base);

                Methods.App.FullScreenApp(this);

                // Create your application here
                SetContentView(Resource.Layout.Settings_Password_Layout);

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar(); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        protected override void OnResume()
        {
            try
            {
                base.OnResume();
                AddOrRemoveEvent(true);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        protected override void OnPause()
        {
            try
            {
                base.OnPause();
                AddOrRemoveEvent(false);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override void OnTrimMemory(TrimMemory level)
        {
            try
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                base.OnTrimMemory(level);
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
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        protected override void OnDestroy()
        {
            try
            {
                DestroyBasic();
                base.OnDestroy();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
        #endregion

        #region Menu

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        #endregion

        #region Functions

        private void InitComponent()
        {
            try
            {
                TxtSave = FindViewById<TextView>(Resource.Id.toolbar_title);

                IconCurrentPassword = FindViewById<TextView>(Resource.Id.IconCurrentPassword);
                TxtCurrentPassword = FindViewById<EditText>(Resource.Id.CurrentPasswordEditText);

                IconNewPassword = FindViewById<TextView>(Resource.Id.IconNewPassword);
                TxtNewPassword = FindViewById<EditText>(Resource.Id.NewPasswordEditText);

                IconRepeatPassword = (TextView)FindViewById(Resource.Id.IconRepeatPassword);
                TxtRepeatPassword = (EditText)FindViewById(Resource.Id.RepeatPasswordEditText);

                TxtLinkForget = FindViewById<TextView>(Resource.Id.linkText);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconCurrentPassword, FontAwesomeIcon.Key);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconNewPassword, FontAwesomeIcon.UnlockAlt);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconRepeatPassword, FontAwesomeIcon.LockAlt);

                Methods.SetColorEditText(TxtCurrentPassword, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtNewPassword, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtRepeatPassword, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                AdsGoogle.Ad_AdMobNative(this); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void InitToolbar()
        {
            try
            {
                var toolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
                if (toolBar != null)
                {
                    toolBar.Title = GetText(Resource.String.Lbl_Change_Password);
                    toolBar.SetTitleTextColor(Color.ParseColor(AppSettings.MainColor));
                    SetSupportActionBar(toolBar);
                    SupportActionBar.SetDisplayShowCustomEnabled(true);
                    SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                    SupportActionBar.SetHomeButtonEnabled(true);
                    SupportActionBar.SetDisplayShowHomeEnabled(true);
                    SupportActionBar.SetHomeAsUpIndicator(AppCompatResources.GetDrawable(this, AppSettings.FlowDirectionRightToLeft ? Resource.Drawable.ic_action_right_arrow_color : Resource.Drawable.ic_action_left_arrow_color));

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
                        TxtLinkForget.Click += TxtLinkForget_OnClick;
                        TxtSave.Click += SaveData_OnClick;
                        break;
                    default:
                        TxtLinkForget.Click -= TxtLinkForget_OnClick;
                        TxtSave.Click -= SaveData_OnClick;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        private void DestroyBasic()
        {
            try
            { 
                TxtSave = null!;
                IconCurrentPassword = null!;
                TxtCurrentPassword = null!;
                IconNewPassword = null!;
                TxtNewPassword = null!;
                IconRepeatPassword = null!;
                TxtRepeatPassword = null!;
                TxtLinkForget = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        private void TxtLinkForget_OnClick(object sender, EventArgs e)
        {
            try
            {
                StartActivity(typeof(ForgetPasswordActivity));
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Save data 
        private async void SaveData_OnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    return;
                }

                if (string.IsNullOrEmpty(TxtCurrentPassword.Text) || string.IsNullOrEmpty(TxtNewPassword.Text) || string.IsNullOrEmpty(TxtRepeatPassword.Text))
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_Please_check_your_details), ToastLength.Long)?.Show();
                    return;
                }

                if (TxtNewPassword.Text != TxtRepeatPassword.Text)
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_Your_password_dont_match), ToastLength.Long)?.Show();
                    return;
                }

                //Show a progress
                AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));

                var dataPrivacy = new Dictionary<string, string>
                {
                    {"new_password", TxtNewPassword.Text},
                    {"current_password", TxtCurrentPassword.Text}
                };

                var (apiStatus, respond) = await RequestsAsync.Global.UpdateUserDataAsync(dataPrivacy);
                switch (apiStatus)
                {
                    case 200:
                    {
                        switch (respond)
                        {
                            case MessageObject result when result.Message.Contains("updated"):
                                UserDetails.Password = TxtNewPassword.Text;

                                Toast.MakeText(this, result.Message, ToastLength.Short)?.Show();
                                AndHUD.Shared.Dismiss(this);
                                break;
                            case MessageObject result:
                                //Show a Error image with a message
                                AndHUD.Shared.ShowError(this, result.Message, MaskType.Clear, TimeSpan.FromSeconds(1));
                                break;
                        }

                        break;
                    }
                    default:
                        Methods.DisplayAndHudErrorResult(this, respond);
                        break;
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                //Show a Error image with a message
                AndHUD.Shared.ShowError(this, e.Message, MaskType.Clear, TimeSpan.FromSeconds(1));
            }
        }

        #endregion

    }
}
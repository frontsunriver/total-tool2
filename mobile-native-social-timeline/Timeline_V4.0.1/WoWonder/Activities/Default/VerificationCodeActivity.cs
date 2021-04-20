using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
using WoWonder.Activities.Tabbes;
using WoWonder.Activities.WalkTroutPage;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using WoWonderClient;
using WoWonderClient.Classes.Global;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Default
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class VerificationCodeActivity : BaseActivity
    {
        #region Variables Basic

        private EditText TxtNumber1;
        private Button BtnVerify;
        private string TypeCode;

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
                SetContentView(Resource.Layout.VerificationCodeLayout);

                TypeCode = Intent?.GetStringExtra("TypeCode") ?? "";

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
                TxtNumber1 = FindViewById<EditText>(Resource.Id.TextNumber1);
                BtnVerify = FindViewById<Button>(Resource.Id.verifyButton);

                Methods.SetColorEditText(TxtNumber1, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
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
                    toolBar.Title = " ";
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
                        BtnVerify.Click += BtnVerifyOnClick;
                        break;
                    default:
                        BtnVerify.Click -= BtnVerifyOnClick;
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
                TxtNumber1 = null!;
                BtnVerify = null!;
                TypeCode = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        private async void BtnVerifyOnClick(object sender, EventArgs e)
        {
            try
            {
                switch (string.IsNullOrEmpty(TxtNumber1.Text))
                {
                    case false when !string.IsNullOrWhiteSpace(TxtNumber1.Text):
                    {
                        if (Methods.CheckConnectivity())
                        {
                            //Show a progress
                            AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));

                            switch (TypeCode)
                            {
                                case "TwoFactor":
                                {
                                    var (apiStatus, respond) = await RequestsAsync.Auth.TwoFactorAsync(UserDetails.UserId, TxtNumber1.Text, UserDetails.DeviceId);
                                    switch (apiStatus)
                                    {
                                        case 200:
                                        {
                                            switch (respond)
                                            {
                                                case AuthObject auth:
                                                {
                                                    SetDataLogin(auth);

                                                    switch (AppSettings.ShowWalkTroutPage)
                                                    {
                                                        case true:
                                                        {
                                                            Intent newIntent = new Intent(this, typeof(AppIntroWalkTroutPage));
                                                            newIntent?.PutExtra("class", "login");
                                                            StartActivity(newIntent);
                                                            break;
                                                        }
                                                        default:
                                                            StartActivity(new Intent(this, typeof(TabbedMainActivity)));
                                                            break;
                                                    }

                                                    AndHUD.Shared.Dismiss(this);
                                                    FinishAffinity();
                                                    break;
                                                }
                                            }

                                            break;
                                        }
                                        default:
                                        {
                                            switch (respond)
                                            {
                                                case ErrorObject errorMessage:
                                                {
                                                    var errorId = errorMessage.Error.ErrorId;
                                                    switch (errorId)
                                                    {
                                                        case "3":
                                                            Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), GetText(Resource.String.Lbl_CodeNotCorrect), GetText(Resource.String.Lbl_Ok));
                                                            break;
                                                    }
                                                    break;
                                                }
                                            }
                                            Methods.DisplayReportResult(this, respond);
                                            break;
                                        }
                                    }

                                    break;
                                }
                                case "AccountSms":
                                {
                                    var (apiStatus, respond) = await RequestsAsync.Auth.ActiveAccountSmsAsync(UserDetails.UserId, TxtNumber1.Text, UserDetails.DeviceId);
                                    switch (apiStatus)
                                    {
                                        case 200:
                                        {
                                            switch (respond)
                                            {
                                                case AuthObject auth:
                                                {
                                                    SetDataLogin(auth);

                                                    switch (AppSettings.ShowWalkTroutPage)
                                                    {
                                                        case true:
                                                        {
                                                            Intent newIntent = new Intent(this, typeof(AppIntroWalkTroutPage));
                                                            newIntent?.PutExtra("class", "login");
                                                            StartActivity(newIntent);
                                                            break;
                                                        }
                                                        default:
                                                            StartActivity(new Intent(this, typeof(TabbedMainActivity)));
                                                            break;
                                                    }

                                                    AndHUD.Shared.Dismiss(this);
                                                    FinishAffinity();
                                                    break;
                                                }
                                            }

                                            break;
                                        }
                                        default:
                                        {
                                            switch (respond)
                                            {
                                                case ErrorObject errorMessage:
                                                {
                                                    var errorId = errorMessage.Error.ErrorId;
                                                    switch (errorId)
                                                    {
                                                        case "3":
                                                            Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), GetText(Resource.String.Lbl_CodeNotCorrect), GetText(Resource.String.Lbl_Ok));
                                                            break;
                                                    }
                                                    break;
                                                }
                                            }
                                            Methods.DisplayReportResult(this, respond);
                                            break;
                                        }
                                    }

                                    break;
                                }
                            }
                         
                            AndHUD.Shared.Dismiss(this);
                        }
                        else
                        {
                            Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                        }

                        break;
                    }
                    default:
                        Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_Security), GetText(Resource.String.Lbl_Please_enter_your_data), GetText(Resource.String.Lbl_Ok));
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                AndHUD.Shared.Dismiss(this);
            }
        }

        #endregion

        private void SetDataLogin(AuthObject auth)
        {
            try
            {
                Current.AccessToken = auth.AccessToken;

                UserDetails.AccessToken = auth.AccessToken;
                UserDetails.UserId = auth.UserId;
                UserDetails.Status = "Pending";
                UserDetails.Cookie = auth.AccessToken;

                //Insert user data to database
                var user = new DataTables.LoginTb
                {
                    UserId = UserDetails.UserId,
                    AccessToken = UserDetails.AccessToken,
                    Cookie = UserDetails.Cookie,
                    Username = UserDetails.Username,
                    Password = UserDetails.Password,
                    Status = "Pending",
                    Lang = "",
                    DeviceId = UserDetails.DeviceId,
                    Email = UserDetails.Email,
                };
                ListUtils.DataUserLoginList.Clear();
                ListUtils.DataUserLoginList.Add(user);

                var dbDatabase = new SqLiteDatabase();
                dbDatabase.InsertOrUpdateLogin_Credentials(user);
                

                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => ApiRequest.Get_MyProfileData_Api(this) });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }
}
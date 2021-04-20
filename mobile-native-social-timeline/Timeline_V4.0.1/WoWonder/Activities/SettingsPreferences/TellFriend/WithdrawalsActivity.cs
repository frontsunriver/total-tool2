using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.Content.Res;
using Java.Lang;
using WoWonder.Activities.Base;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
  
namespace WoWonder.Activities.SettingsPreferences.TellFriend
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class WithdrawalsActivity : BaseActivity, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private TextView TxtMyBalance , IconWithdrawMethod , IconAmount , IconPayPalEmail, IconAccountNumber, IconCountry, IconAccountName, IconSwiftCode, IconAddress;
        private EditText TxtWithdrawMethod , TxtAmount , TxtPayPalEmail, TxtAccountNumber, TxtCountry, TxtAccountName, TxtSwiftCode, TxtAddress;
        private LinearLayout LayoutPayPalEmail , LayoutBank;
        private Button BtnRequestWithdrawal;
        private double CountBalance;
        private string TypeDialog ,TypeWithdrawMethod = "paypal";

        #endregion

        #region General

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                Methods.App.FullScreenApp(this);

                SetTheme(AppSettings.SetTabDarkTheme ? Resource.Style.MyTheme_Dark_Base : Resource.Style.MyTheme_Base);

                // Create your application here
                SetContentView(Resource.Layout.WithdrawalsLayout);
                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                Get_Data_User(); 
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
                TxtMyBalance = FindViewById<TextView>(Resource.Id.myBalance);

                IconWithdrawMethod = FindViewById<TextView>(Resource.Id.IconWithdrawMethod);
                TxtWithdrawMethod = FindViewById<EditText>(Resource.Id.WithdrawMethodEditText);

                IconAmount = FindViewById<TextView>(Resource.Id.IconAmount);
                TxtAmount = FindViewById<EditText>(Resource.Id.AmountEditText);

                LayoutPayPalEmail = FindViewById<LinearLayout>(Resource.Id.LayoutPayPalEmail);
                IconPayPalEmail = FindViewById<TextView>(Resource.Id.IconPayPalEmail);
                TxtPayPalEmail = FindViewById<EditText>(Resource.Id.PayPalEmailEditText);

                LayoutBank = FindViewById<LinearLayout>(Resource.Id.LayoutBank);
                IconAccountNumber = FindViewById<TextView>(Resource.Id.IconAccountNumber);
                TxtAccountNumber = FindViewById<EditText>(Resource.Id.AccountNumberEditText);

                IconCountry = FindViewById<TextView>(Resource.Id.IconCountry);
                TxtCountry = FindViewById<EditText>(Resource.Id.CountryEditText);

                IconAccountName = FindViewById<TextView>(Resource.Id.IconAccountName);
                TxtAccountName = FindViewById<EditText>(Resource.Id.AccountNameEditText);

                IconSwiftCode = FindViewById<TextView>(Resource.Id.IconSwiftCode);
                TxtSwiftCode = FindViewById<EditText>(Resource.Id.SwiftCodeEditText);

                IconAddress = FindViewById<TextView>(Resource.Id.IconAddress);
                TxtAddress = FindViewById<EditText>(Resource.Id.AddressEditText);

                BtnRequestWithdrawal = FindViewById<Button>(Resource.Id.RequestWithdrawalButton);
                 
                Methods.SetColorEditText(TxtWithdrawMethod, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtAmount, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtPayPalEmail, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtAccountNumber, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtCountry, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtAccountName, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtSwiftCode, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtAddress, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconWithdrawMethod, FontAwesomeIcon.MoneyCheckAlt);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconAmount, FontAwesomeIcon.MoneyBillWave);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeBrands, IconPayPalEmail, FontAwesomeIcon.CcPaypal);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconAccountNumber, FontAwesomeIcon.MoneyCheck);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconCountry, FontAwesomeIcon.MapMarkedAlt); 
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconAccountName, FontAwesomeIcon.User);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconSwiftCode, FontAwesomeIcon.Barcode); 
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconAddress, FontAwesomeIcon.Paragraph);

                Methods.SetFocusable(TxtWithdrawMethod); 
                Methods.SetFocusable(TxtCountry);

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
                    toolBar.Title = GetText(Resource.String.Lbl_Withdrawals);
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
                        TxtWithdrawMethod.Touch += TxtWithdrawMethodOnTouch;
                        TxtCountry.Touch += TxtCountryOnTouch;
                        BtnRequestWithdrawal.Click += BtnRequestWithdrawalOnClick;
                        break;
                    default:
                        TxtWithdrawMethod.Touch -= TxtWithdrawMethodOnTouch;
                        TxtCountry.Touch -= TxtCountryOnTouch;
                        BtnRequestWithdrawal.Click -= BtnRequestWithdrawalOnClick;
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
                TxtMyBalance = null!;
                IconWithdrawMethod = null!;
                TxtWithdrawMethod = null!;
                IconAmount= null!;
                TxtAmount = null!;
                LayoutPayPalEmail= null!;
                IconPayPalEmail = null!;
                TxtPayPalEmail = null!;
                LayoutBank = null!;
                IconAccountNumber = null!;
                TxtAccountNumber = null!;
                IconCountry= null!;
                TxtCountry = null!;
                IconAccountName = null!;
                TxtAccountName = null!;
                IconSwiftCode = null!;
                TxtSwiftCode= null!;
                IconAddress = null!;
                TxtAddress = null!;  
                BtnRequestWithdrawal = null!;
                TypeDialog = null!; 
                TypeWithdrawMethod = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events

        private void TxtCountryOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                TypeDialog = "Country";

                var countriesArray = WoWonderTools.GetCountryList(this);

                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                var arrayAdapter = countriesArray.Select(item => item.Value).ToList();

                dialogList.Title(GetText(Resource.String.Lbl_Country)).TitleColorRes(Resource.Color.primary);
                dialogList.Items(arrayAdapter);
                dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                dialogList.AlwaysCallSingleChoiceCallback();
                dialogList.ItemsCallback(this).Build().Show();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void TxtWithdrawMethodOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                TypeDialog = "WithdrawMethod";

                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                var arrayAdapter = new List<string>
                {
                    GetText(Resource.String.Btn_Paypal), GetText(Resource.String.Lbl_Bank)
                };

                dialogList.Title(GetText(Resource.String.Lbl_WithdrawMethod)).TitleColorRes(Resource.Color.primary);
                dialogList.Items(arrayAdapter);
                dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                dialogList.AlwaysCallSingleChoiceCallback();
                dialogList.ItemsCallback(this).Build().Show(); 
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        private async void BtnRequestWithdrawalOnClick(object sender, EventArgs e)
        {
            try
            {
                if (CountBalance < Convert.ToDouble(TxtAmount.Text))
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_ThereIsNoBalance), ToastLength.Long)?.Show();
                    return;
                }

                if (Convert.ToDouble(TxtAmount.Text) < Convert.ToDouble(ListUtils.SettingsSiteList?.MWithdrawal))
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_CantRequestWithdrawals), ToastLength.Long)?.Show();
                    return;
                }

                switch (TypeWithdrawMethod)
                {
                    case "paypal" when string.IsNullOrEmpty(TxtPayPalEmail.Text.Replace(" ", "")) || string.IsNullOrEmpty(TxtAmount.Text.Replace(" ", "")):
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_check_your_details), ToastLength.Long)?.Show();
                        return;
                    case "bank" when string.IsNullOrEmpty(TxtAmount.Text.Replace(" ", "")) || string.IsNullOrEmpty(TxtAccountNumber.Text.Replace(" ", "")) || string.IsNullOrEmpty(TxtCountry.Text.Replace(" ", ""))
                                     || string.IsNullOrEmpty(TxtAccountName.Text.Replace(" ", "")) || string.IsNullOrEmpty(TxtSwiftCode.Text.Replace(" ", "")) || string.IsNullOrEmpty(TxtAddress.Text.Replace(" ", "")):
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_check_your_details), ToastLength.Long)?.Show();
                        return;
                }

                if (Methods.CheckConnectivity())
                {
                    //Show a progress
                    AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));

                    var dictionary = new Dictionary<string, string>
                    {
                        {"type", TypeWithdrawMethod},
                        {"amount", TxtAmount.Text},
                    };

                    switch (TypeWithdrawMethod)
                    {
                        case "paypal":
                            dictionary.Add("paypal_email", TxtPayPalEmail.Text);
                            break;
                        case "bank":
                            dictionary.Add("iban", TxtAccountNumber.Text);
                            dictionary.Add("country", TxtCountry.Text);
                            dictionary.Add("full_name", TxtAccountName.Text);
                            dictionary.Add("swift_code", TxtSwiftCode.Text);
                            dictionary.Add("address", TxtAddress.Text);
                            break;
                    }

                    var (apiStatus, respond) = await RequestsAsync.Global.WithdrawAsync(dictionary);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case MessageObject result:
                                    Console.WriteLine(result.Message);
                                    AndHUD.Shared.Dismiss(this);
                                    Toast.MakeText(this, GetText(Resource.String.Lbl_RequestSentWithdrawals), ToastLength.Long)?.Show();

                                    Finish();
                                    break;
                            }

                            break;
                        }
                        default:
                            Methods.DisplayAndHudErrorResult(this, respond);
                            break;
                    }
                }
                else
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
                }
            }
            catch (Exception exception)
            {
                AndHUD.Shared.Dismiss(this);
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region MaterialDialog

        public void OnClick(MaterialDialog p0, DialogAction p1)
        {
            try
            {
                if (p1 == DialogAction.Positive)
                {

                }
                else if (p1 == DialogAction.Negative)
                {
                    p0.Dismiss();
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
                switch (TypeDialog)
                {
                    case "WithdrawMethod":
                    {
                        if (itemString.ToString() == GetText(Resource.String.Btn_Paypal))
                        {
                            TypeWithdrawMethod = "paypal";
                            LayoutPayPalEmail.Visibility = ViewStates.Visible;
                            LayoutBank.Visibility = ViewStates.Gone;
                        }
                        else
                        {
                            TypeWithdrawMethod = "bank";

                            LayoutPayPalEmail.Visibility = ViewStates.Gone;
                            LayoutBank.Visibility = ViewStates.Visible;
                        }

                        TxtWithdrawMethod.Text = itemString.ToString();
                        break;
                    }
                    case "Country":
                        TxtCountry.Text = itemString.ToString();
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        private async void Get_Data_User()
        {
            try
            {
                switch (ListUtils.MyProfileList?.Count)
                {
                    case 0:
                        await ApiRequest.Get_MyProfileData_Api(this);
                        break;
                }

                var local = ListUtils.MyProfileList?.FirstOrDefault();
                if (local != null)
                {
                    CountBalance = Convert.ToDouble(local.Balance);
                    TxtMyBalance.Text = GetText(Resource.String.Lbl_Withdrawals_SubText1) + " $" + CountBalance.ToString(CultureInfo.InvariantCulture) + ", " + GetText(Resource.String.Lbl_Withdrawals_SubText2) + " $" + ListUtils.SettingsSiteList?.MWithdrawal;

                    TxtPayPalEmail.Text = local.PaypalEmail;

                    TxtWithdrawMethod.Text = GetText(Resource.String.Btn_Paypal);
                    TypeWithdrawMethod = "paypal";
                    LayoutPayPalEmail.Visibility = ViewStates.Visible;
                    LayoutBank.Visibility = ViewStates.Gone;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

    }
}
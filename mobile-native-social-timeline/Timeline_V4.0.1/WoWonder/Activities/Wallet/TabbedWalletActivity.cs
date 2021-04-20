using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using Com.Razorpay;
using IAmMert.ReadableBottomBarLib;
using Newtonsoft.Json;
using WoWonder.Activities.Base;
using WoWonder.Activities.Wallet.Fragment;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.Payment;
using WoWonderClient.Classes.Global;
using WoWonderClient.Requests;
using Xamarin.PayPal.Android;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Wallet
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class TabbedWalletActivity : BaseActivity, ReadableBottomBar.IItemSelectListener, IPaymentResultListener
    { 
        #region Variables Basic

        private ReadableBottomBar BottomBar;
        private FragmentBottomNavigationView FragmentBottomNavigator;
        public SendMoneyFragment SendMoneyFragment;
        public AddFundsFragment AddFundsFragment;
        public string TypeOpenPayment = "";
        private static TabbedWalletActivity Instance;

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
                SetContentView(Resource.Layout.TabbedWalletLayout);

                Instance = this;

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();

                SetupBottomNavigationView();
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
                switch (AppSettings.ShowPaypal)
                {
                    case true:
                        AddFundsFragment?.InitPayPalPayment?.StopPayPalService();
                        break;
                }

                switch (AppSettings.ShowRazorPay)
                {
                    case true:
                        AddFundsFragment?.InitRazorPay?.StopRazorPay();
                        break;
                }

                switch (AppSettings.ShowPayStack)
                {
                    case true:
                        AddFundsFragment?.PayStackPayment?.StopPayStack();
                        break;
                }
                
                switch (AppSettings.ShowCashFree)
                {
                    case true:
                        AddFundsFragment?.CashFreePayment?.StopCashFree();
                        break;
                }

                switch (AppSettings.ShowPaySera)
                {
                    case true:
                        AddFundsFragment?.PaySeraPayment?.StopPaySera();
                        break;
                }

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
                BottomBar = FindViewById<ReadableBottomBar>(Resource.Id.readableBottomBar);
                BottomBar.SetOnItemSelectListener(this); 
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
                    toolBar.Title = GetString(Resource.String.Lbl_WalletCredits);
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

        public static TabbedWalletActivity GetInstance()
        {
            try
            {
                return Instance;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }
        private void DestroyBasic()
        {
            try
            {
                BottomBar = null!;
                FragmentBottomNavigator = null!;
                SendMoneyFragment = null!;
                AddFundsFragment = null!;
                TypeOpenPayment = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Set Navigation And Show Fragment

        private void SetupBottomNavigationView()
        {
            try
            {
                FragmentBottomNavigator = new FragmentBottomNavigationView(this);

                SendMoneyFragment = new SendMoneyFragment();
                AddFundsFragment = new AddFundsFragment();

                FragmentBottomNavigator.FragmentListTab0.Add(SendMoneyFragment);
                FragmentBottomNavigator.FragmentListTab1.Add(AddFundsFragment);

                FragmentBottomNavigator.NavigationTabBarOnStartTabSelected(0);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion
         
        #region Permissions && Result
       
        //Result 
        protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
                switch (TypeOpenPayment)
                {
                    case "AddFundsFragment" when requestCode == InitPayPalPayment.PayPalDataRequestCode:
                        switch (resultCode)
                        {
                            case Result.Ok:
                                var confirmObj = data.GetParcelableExtra(PaymentActivity.ExtraResultConfirmation);
                                PaymentConfirmation configuration = Android.Runtime.Extensions.JavaCast<PaymentConfirmation>(confirmObj);
                                if (configuration != null)
                                {
                                    //string createTime = configuration.ProofOfPayment.CreateTime;
                                    //string intent = configuration.ProofOfPayment.Intent;
                                    //string paymentId = configuration.ProofOfPayment.PaymentId;
                                    //string state = configuration.ProofOfPayment.State;
                                    //string transactionId = configuration.ProofOfPayment.TransactionId;
                                    if (Methods.CheckConnectivity())
                                    {
                                        var (apiStatus, respond) = await RequestsAsync.Global.TopUpWalletAsync(UserDetails.UserId, AddFundsFragment?.TxtAmount.Text).ConfigureAwait(false);
                                        switch (apiStatus)
                                        {
                                            case 200:
                                                RunOnUiThread(() =>
                                                {
                                                    try
                                                    {
                                                        AddFundsFragment.TxtAmount.Text = string.Empty;

                                                        Toast.MakeText(this, GetText(Resource.String.Lbl_PaymentSuccessfully), ToastLength.Long)?.Show();
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        Methods.DisplayReportResultTrack(e);
                                                    }
                                                });
                                                break;
                                            default:
                                                Methods.DisplayReportResult(this, respond);
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
                                    }
                                }

                                break;
                            case (int)Result.Canceled:
                                Toast.MakeText(this, GetText(Resource.String.Lbl_Canceled), ToastLength.Long)?.Show();
                                break;
                        }

                        break;
                    case "AddFundsFragment":
                    {
                        switch (requestCode)
                        {
                            case PaymentActivity.ResultExtrasInvalid:
                                Toast.MakeText(this, GetText(Resource.String.Lbl_Invalid), ToastLength.Long)?.Show();
                                break;
                        }

                        break;
                    }
                    default:
                    {
                        switch (requestCode)
                        {
                            case 1202 when resultCode ==  Result.Ok:
                            {
                                var userObject = data.GetStringExtra("DataUser");
                                switch (string.IsNullOrEmpty(userObject))
                                {
                                    case false:
                                    {
                                        var userData = JsonConvert.DeserializeObject<UserDataObject>(userObject);
                                        if (userData != null)
                                        {
                                            SendMoneyFragment.TxtEmail.Text = WoWonderTools.GetNameFinal(userData);
                                            SendMoneyFragment.UserId = userData.UserId;
                                        }

                                        break;
                                    }
                                }

                                break;
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
         
        #endregion 

        public void OnItemSelected(int index)
        {
            try
            {
                FragmentBottomNavigator.NavigationTabBarOnStartTabSelected(index);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
 
        public void OnPaymentError(int code, string response)
        {
            try
            {
                Console.WriteLine("razorpay : Payment failed: " + code + " " + response);
                Toast.MakeText(this, "Payment failed: " + response, ToastLength.Long)?.Show();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public async void OnPaymentSuccess(string razorpayPaymentId)
        {
            try
            {
                Console.WriteLine("razorpay : Payment Successful:" + razorpayPaymentId);

                switch (string.IsNullOrEmpty(razorpayPaymentId))
                {
                    case false when Methods.CheckConnectivity():
                    {
                        var priceInt = Convert.ToInt32(AddFundsFragment?.Price) * 100;
                        var keyValues = new Dictionary<string, string>
                        {
                            {"merchant_amount", priceInt.ToString()},
                        };

                        var (apiStatus, respond) = await RequestsAsync.Global.RazorPayAsync(razorpayPaymentId, "wallet", keyValues).ConfigureAwait(false);
                        switch (apiStatus)
                        {
                            case 200:
                                RunOnUiThread(() =>
                                {
                                    try
                                    {
                                        AddFundsFragment.TxtAmount.Text = string.Empty;
                                        Toast.MakeText(this, GetText(Resource.String.Lbl_PaymentSuccessfully), ToastLength.Long)?.Show();
                                    }
                                    catch (Exception e)
                                    {
                                        Methods.DisplayReportResultTrack(e);
                                    }
                                });
                                break;
                            default:
                                Methods.DisplayReportResult(this, respond);
                                break;
                        }

                        break;
                    }
                    case false:
                        Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
    }
}
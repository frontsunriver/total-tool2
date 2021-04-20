using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;

using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using AndroidX.RecyclerView.Widget;
using Com.Razorpay;
using Java.Lang;
using WoWonder.Activities.Base;
using WoWonder.Activities.General.Adapters;
using WoWonder.Activities.Suggested.User;
using WoWonder.Activities.Tabbes;
using WoWonder.Activities.WalkTroutPage;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonder.Payment;
using WoWonder.PaymentGoogle;
using WoWonder.SQLite;
using WoWonderClient;
using WoWonderClient.Classes.Global;
using WoWonderClient.InAppBilling;
using WoWonderClient.Requests;
using Xamarin.PayPal.Android;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.General
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize | ConfigChanges.SmallestScreenSize | ConfigChanges.UiMode | ConfigChanges.Locale)]
    public class GoProActivity : BaseActivity, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback , IPaymentResultListener
    {
        #region Variables Basic

        private RecyclerView MainRecyclerView, MainPlansRecyclerView;
        private GridLayoutManager LayoutManagerView;
        private LinearLayoutManager PlansLayoutManagerView;
        private GoProFeaturesAdapter FeaturesAdapter;
        private UpgradeGoProAdapter PlansAdapter;
        private InitPayPalPayment InitPayPalPayment;
        private InitInAppBillingPayment BillingPayment;
        private InitRazorPayPayment InitRazorPay;
        private InitPayStackPayment PayStackPayment;
        private InitCashFreePayment CashFreePayment;
        private InitPaySeraPayment PaySeraPayment;
        private ImageView IconClose;
        private string Caller, PayId, Price, PayType;
        private UpgradeGoProClass ItemUpgrade;

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
                SetContentView(Resource.Layout.Go_Pro_Layout);

                Caller = Intent?.GetStringExtra("class") ?? "";

                //Get Value And Set Toolbar
                InitBuy();
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();
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
                switch (AppSettings.ShowInAppBilling)
                {
                    case true when Client.IsExtended:
                        BillingPayment?.DisconnectInAppBilling();
                        break;
                }
               
                switch (AppSettings.ShowPaypal)
                {
                    case true:
                        InitPayPalPayment?.StopPayPalService();
                        break;
                }

                switch (AppSettings.ShowRazorPay)
                {
                    case true:
                        InitRazorPay?.StopRazorPay();
                        break;
                }

                switch (AppSettings.ShowPayStack)
                {
                    case true:
                        PayStackPayment?.StopPayStack();
                        break;
                }

                switch (AppSettings.ShowCashFree)
                {
                    case true:
                        CashFreePayment?.StopCashFree();
                        break;
                }

                switch (AppSettings.ShowPaySera)
                {
                    case true:
                        PaySeraPayment?.StopPaySera();
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
                    FinishPage();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        #endregion

        #region Functions
       
        private void InitBuy()
        {
            try
            {
                BillingPayment = AppSettings.ShowInAppBilling switch
                {
                    true when Client.IsExtended => new InitInAppBillingPayment(this),
                    _ => BillingPayment
                };

                InitPayPalPayment = AppSettings.ShowPaypal switch
                {
                    true => new InitPayPalPayment(this),
                    _ => InitPayPalPayment
                };

                InitRazorPay = AppSettings.ShowRazorPay switch
                {
                    true => new InitRazorPayPayment(this),
                    _ => InitRazorPay
                };

                PayStackPayment = AppSettings.ShowPayStack switch
                {
                    true => new InitPayStackPayment(this),
                    _ => PayStackPayment
                };

                CashFreePayment = AppSettings.ShowCashFree switch
                {
                    true => new InitCashFreePayment(this),
                    _ => CashFreePayment
                };

                PaySeraPayment = AppSettings.ShowPaySera switch
                {
                    true => new InitPaySeraPayment(this),
                    _ => PaySeraPayment
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void InitComponent()
        {
            try
            {
                MainRecyclerView = FindViewById<RecyclerView>(Resource.Id.recycler);
                MainPlansRecyclerView = FindViewById<RecyclerView>(Resource.Id.recycler2);
                IconClose = FindViewById<ImageView>(Resource.Id.iv1);
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
                    toolBar.Title = GetText(Resource.String.Lbl_Go_Pro);
                    toolBar.SetTitleTextColor(Color.ParseColor(AppSettings.MainColor));
                    SetSupportActionBar(toolBar);
                    SupportActionBar.SetDisplayShowCustomEnabled(true);
                    SupportActionBar.SetDisplayHomeAsUpEnabled(false);
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

        private void SetRecyclerViewAdapters()
        {
            try
            {
                FeaturesAdapter = new GoProFeaturesAdapter(this);
                LayoutManagerView = new GridLayoutManager(this, 3);
                MainRecyclerView.SetLayoutManager(LayoutManagerView);
                MainRecyclerView.HasFixedSize = true;
                MainRecyclerView.SetAdapter(FeaturesAdapter);

                PlansAdapter = new UpgradeGoProAdapter(this);
                PlansLayoutManagerView = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
                MainPlansRecyclerView.SetLayoutManager(PlansLayoutManagerView);
                MainPlansRecyclerView.HasFixedSize = true;
                MainPlansRecyclerView.SetAdapter(PlansAdapter);
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
                        PlansAdapter.UpgradeButtonItemClick += PlansAdapterOnItemClick;
                        IconClose.Click += IconCloseOnClick;
                        break;
                    default:
                        PlansAdapter.UpgradeButtonItemClick -= PlansAdapterOnItemClick;
                        IconClose.Click -= IconCloseOnClick;
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
                MainRecyclerView = null!;
                MainPlansRecyclerView = null!;
                LayoutManagerView = null!;
                PlansLayoutManagerView = null!;
                FeaturesAdapter = null!;
                PlansAdapter = null!;
                InitPayPalPayment = null!;
                InitRazorPay = null!;
                PayStackPayment = null!;
                PayStackPayment = null!;
                BillingPayment = null!;
                IconClose = null!;
                PayId = null!;
                Price = null!;
                PayType = null!;
                ItemUpgrade = null!; 
                PaySeraPayment = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events

        private void PlansAdapterOnItemClick(object sender, UpgradeGoProAdapterClickEventArgs e)
        {
            try
            {
                switch (e.Position)
                {
                    case > -1:
                    {
                        ItemUpgrade = PlansAdapter.GetItem(e.Position);
                        if (ItemUpgrade != null)
                        {
                            var arrayAdapter = new List<string>();
                            var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                            switch (AppSettings.ShowInAppBilling)
                            {
                                case true when Client.IsExtended:
                                    arrayAdapter.Add(GetString(Resource.String.Btn_GooglePlay));
                                    break;
                            }

                            switch (AppSettings.ShowPaypal)
                            {
                                case true:
                                    arrayAdapter.Add(GetString(Resource.String.Btn_Paypal));
                                    break;
                            }

                            switch (AppSettings.ShowCreditCard)
                            {
                                case true:
                                    arrayAdapter.Add(GetString(Resource.String.Lbl_CreditCard));
                                    break;
                            }

                            switch (AppSettings.ShowBankTransfer)
                            {
                                case true:
                                    arrayAdapter.Add(GetString(Resource.String.Lbl_BankTransfer));
                                    break;
                            }

                            switch (AppSettings.ShowRazorPay)
                            {
                                case true:
                                    arrayAdapter.Add(GetString(Resource.String.Lbl_RazorPay));
                                    break;
                            }

                            switch (AppSettings.ShowPayStack)
                            {
                                case true:
                                    arrayAdapter.Add(GetString(Resource.String.Lbl_PayStack));
                                    break;
                            }

                            switch (AppSettings.ShowCashFree)
                            {
                                case true:
                                    arrayAdapter.Add(GetString(Resource.String.Lbl_CashFree));
                                    break;
                            }

                            switch (AppSettings.ShowPaySera)
                            {
                                case true:
                                    arrayAdapter.Add(GetString(Resource.String.Lbl_PaySera));
                                    break;
                            }

                            dialogList.Items(arrayAdapter);
                            dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                            dialogList.AlwaysCallSingleChoiceCallback();
                            dialogList.ItemsCallback(this).Build().Show(); 
                        }

                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Close
        private void IconCloseOnClick(object sender, EventArgs e)
        {
            try
            {
                FinishPage();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Result

        //Result
        protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);

                switch (AppSettings.ShowInAppBilling)
                {
                    case true when Client.IsExtended:
                        BillingPayment?.Handler?.HandleActivityResult(requestCode, resultCode, data);
                        break;
                }

                switch (requestCode)
                {
                    case InitPayPalPayment.PayPalDataRequestCode:
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

                                    switch (PayType)
                                    {
                                        case "membership":
                                            await SetProAsync();
                                            break;
                                    }
                                }

                                break;
                            case Result.Canceled:
                                Toast.MakeText(this, GetText(Resource.String.Lbl_Canceled), ToastLength.Long)?.Show();
                                break;
                        }

                        break;
                    case PaymentActivity.ResultExtrasInvalid:
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Invalid), ToastLength.Long)?.Show();
                        break;
                    case BillingProcessor.PurchaseFlowRequestCode when resultCode == Result.Ok && AppSettings.ShowInAppBilling && Client.IsExtended:
                        await SetProAsync();
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }



        #endregion

        #region MaterialDialog

        public async void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                string text = itemString.ToString();
                if (text == GetString(Resource.String.Btn_Paypal))
                {
                    Price = ItemUpgrade.PlanPrice;
                    PayType = "membership";
                    PayId = ItemUpgrade.Id.ToString();
                    InitPayPalPayment.BtnPaypalOnClick(Price, "membership");
                }
                else if (text == GetString(Resource.String.Btn_GooglePlay))
                {
                    Price = ItemUpgrade.PlanPrice;
                    PayId = ItemUpgrade.Id.ToString();

                    BillingPayment.SetConnInAppBilling();
                    BillingPayment.InitInAppBilling(Price, "membership", PayId);
                }
                else if (text == GetString(Resource.String.Lbl_CreditCard))
                {
                    OpenIntentCreditCard();
                }
                else if (text == GetString(Resource.String.Lbl_BankTransfer))
                {
                    OpenIntentBankTransfer();
                } 
                else if (text == GetString(Resource.String.Lbl_RazorPay))
                {
                    Price = ItemUpgrade.PlanPrice;
                    PayId = ItemUpgrade.Id.ToString();

                    InitRazorPay?.BtnRazorPayOnClick(Price, "membership", PayId);
                }
                else if (text == GetString(Resource.String.Lbl_PayStack))
                {
                    Price = ItemUpgrade.PlanPrice;
                    PayId = ItemUpgrade.Id.ToString();

                    var dialog = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);
                    dialog.Title(Resource.String.Lbl_PayStack).TitleColorRes(Resource.Color.primary);
                    dialog.Input(Resource.String.Lbl_Email, 0, false, async (materialDialog, s) =>
                    {
                        try
                        {
                            switch (s.Length)
                            {
                                case <= 0:
                                    return;
                            }
                            var check = Methods.FunString.IsEmailValid(s.ToString().Replace(" ", ""));
                            switch (check)
                            {
                                case false:
                                    Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_VerificationFailed), GetText(Resource.String.Lbl_IsEmailValid), GetText(Resource.String.Lbl_Ok));
                                    return;
                                default:
                                    Toast.MakeText(this, GetText(Resource.String.Lbl_Please_wait), ToastLength.Short)?.Show();

                                    await PayStack(s.ToString());
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            Methods.DisplayReportResultTrack(e);
                        }
                    });
                    dialog.InputType(InputTypes.TextVariationEmailAddress);
                    dialog.PositiveText(GetText(Resource.String.Lbl_PayNow)).OnPositive(this);
                    dialog.NegativeText(GetText(Resource.String.Lbl_Cancel)).OnNegative(this);
                    dialog.AlwaysCallSingleChoiceCallback();
                    dialog.Build().Show();
                }
                else if (text == GetString(Resource.String.Lbl_CashFree))
                {
                    OpenCashFreeDialog();
                }
                else if (text == GetString(Resource.String.Lbl_PaySera))
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_Please_wait), ToastLength.Short)?.Show();

                    await PaySera();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

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
  
        private void OpenIntentCreditCard()
        {
            try
            {
                Intent intent = new Intent(this, typeof(PaymentCardDetailsActivity));
                intent.PutExtra("Id", ItemUpgrade.Id.ToString());
                intent.PutExtra("Price", ItemUpgrade.PlanPrice);
                intent.PutExtra("payType", "membership");
                StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void OpenIntentBankTransfer()
        {
            try
            {
                Intent intent = new Intent(this, typeof(PaymentLocalActivity));
                intent.PutExtra("Id", ItemUpgrade.Id.ToString());
                intent.PutExtra("Price", ItemUpgrade.PlanPrice);
                intent.PutExtra("payType", "membership");
                StartActivity(intent);
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
                        var type = PayId switch
                        {
                            "1" => "week",
                            "2" => "month",
                            "3" => "year",
                            "4" => "life-time",
                            _ => ""
                        };
                        var keyValues = new Dictionary<string, string>
                        {
                            {"type", type}, //week,year,month,life-time 
                        };

                        var (apiStatus, respond) = await RequestsAsync.Global.RazorPayAsync(razorpayPaymentId, "upgrade", keyValues).ConfigureAwait(false);
                        switch (apiStatus)
                        {
                            case 200:
                                RunOnUiThread(() =>
                                {
                                    try
                                    {
                                        var dataUser = ListUtils.MyProfileList?.FirstOrDefault();
                                        if (dataUser != null)
                                        {
                                            dataUser.IsPro = "1";

                                            var sqlEntity = new SqLiteDatabase();
                                            sqlEntity.Insert_Or_Update_To_MyProfileTable(dataUser);
                                        
                                        }

                                        Toast.MakeText(this, GetText(Resource.String.Lbl_Upgraded), ToastLength.Long)?.Show();
                                        FinishPage();
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
         
        private async Task PayStack(string email)
        {
            try
            {
                if (Methods.CheckConnectivity())
                {  
                    var keyValues = new Dictionary<string, string>
                    {
                        {"email", email},
                    };

                    var type = PayId switch
                    {
                        "1" => "week",
                        "2" => "month",
                        "3" => "year",
                        "4" => "life-time",
                        _ => ""
                    };
                     
                    var (apiStatus, respond) = await RequestsAsync.Global.InitializePayStackAsync(type, keyValues);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case InitializePaymentObject result:
                                {
                                    var priceInt = Convert.ToInt32(Price) * 100;

                                    PayStackPayment ??= new InitPayStackPayment(this);
                                    PayStackPayment.DisplayPayStackPayment(result.Url, "membership", priceInt.ToString(), PayId);
                                    break;
                                }
                            }

                            break;
                        }
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
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private EditText TxtName, TxtEmail, TxtPhone;
        private void OpenCashFreeDialog()
        {
            try
            {
                var dialog = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light)
                    .Title(GetText(Resource.String.Lbl_CashFree)).TitleColorRes(Resource.Color.primary)
                    .CustomView(Resource.Layout.CashFreePaymentLayout, true)
                    .PositiveText(GetText(Resource.String.Lbl_PayNow)).OnPositive(async (materialDialog, action) =>
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(TxtName.Text) || string.IsNullOrWhiteSpace(TxtName.Text))
                            {
                                Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_name), ToastLength.Short)?.Show();
                                return;
                            }

                            var check = Methods.FunString.IsEmailValid(TxtEmail.Text.Replace(" ", ""));
                            switch (check)
                            {
                                case false:
                                    Methods.DialogPopup.InvokeAndShowDialog(this, GetText(Resource.String.Lbl_VerificationFailed), GetText(Resource.String.Lbl_IsEmailValid), GetText(Resource.String.Lbl_Ok));
                                    return;
                            }

                            if (string.IsNullOrEmpty(TxtPhone.Text) || string.IsNullOrWhiteSpace(TxtPhone.Text))
                            {
                                Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_your_data), ToastLength.Short)?.Show();
                                return;
                            }

                            Toast.MakeText(this, GetText(Resource.String.Lbl_Please_wait), ToastLength.Short)?.Show();

                            await CashFree(TxtName.Text, TxtEmail.Text, TxtPhone.Text);
                        }
                        catch (Exception e)
                        {
                            Methods.DisplayReportResultTrack(e);
                        }
                    })
                    .NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(new WoWonderTools.MyMaterialDialog())
                    .Build();

                var iconName = dialog.CustomView.FindViewById<TextView>(Resource.Id.IconName);
                TxtName = dialog.CustomView.FindViewById<EditText>(Resource.Id.NameEditText);

                var iconEmail = dialog.CustomView.FindViewById<TextView>(Resource.Id.IconEmail);
                TxtEmail = dialog.CustomView.FindViewById<EditText>(Resource.Id.EmailEditText);

                var iconPhone = dialog.CustomView.FindViewById<TextView>(Resource.Id.IconPhone);
                TxtPhone = dialog.CustomView.FindViewById<EditText>(Resource.Id.PhoneEditText);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, iconName, FontAwesomeIcon.User);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, iconEmail, FontAwesomeIcon.PaperPlane);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, iconPhone, FontAwesomeIcon.Mobile);

                Methods.SetColorEditText(TxtName, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtEmail, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtPhone, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                var local = ListUtils.MyProfileList?.FirstOrDefault();
                if (local != null)
                {
                    TxtName.Text = WoWonderTools.GetNameFinal(local);
                    TxtEmail.Text = local.Email;
                    TxtPhone.Text = local.PhoneNumber;
                }

                dialog.Show();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private async Task CashFree(string name, string email, string phone)
        {
            try
            {
                if (Methods.CheckConnectivity())
                {
                    var type = PayId switch
                    {
                        "1" => "week",
                        "2" => "month",
                        "3" => "year",
                        "4" => "life-time",
                        _ => ""
                    };
                      
                    var keyValues = new Dictionary<string, string>
                    {
                        {"name", name},
                        {"phone", phone},
                        {"email", email}, 
                    };

                    var (apiStatus, respond) = await RequestsAsync.Global.InitializeCashFreeAsync(type, AppSettings.CashFreeCurrency, ListUtils.SettingsSiteList?.CashfreeSecretKey ?? "", ListUtils.SettingsSiteList?.CashfreeMode, keyValues);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case CashFreeObject result:
                                    CashFreePayment ??= new InitCashFreePayment(this);
                                    CashFreePayment.DisplayCashFreePayment(result, "membership", Price, PayId);
                                    break;
                            }

                            break;
                        }
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
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private async Task PaySera()
        {
            try
            {
                if (Methods.CheckConnectivity())
                {
                    var type = PayId switch
                    {
                        "1" => "week",
                        "2" => "month",
                        "3" => "year",
                        "4" => "life-time",
                        _ => ""
                    };

                    var (apiStatus, respond) = await RequestsAsync.Global.InitializePaySeraAsync(type, new Dictionary<string, string>());
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case InitializePaymentObject result:
                                    PaySeraPayment ??= new InitPaySeraPayment(this);
                                    PaySeraPayment.DisplayPaySeraPayment(result.Url, "membership", Price, PayId);
                                    break;
                            }

                            break;
                        }
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
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        private void FinishPage()
        {
            try
            {
                switch (Caller)
                {
                    case "register" when AppSettings.ShowSuggestedUsersOnRegister:
                    {
                        Intent newIntent = new Intent(this, typeof(SuggestionsUsersActivity));
                        newIntent?.PutExtra("class", "register");
                        StartActivity(newIntent);
                        break;
                    }
                    case "register":
                        StartActivity(new Intent(this, typeof(TabbedMainActivity)));
                        break;
                    case "login" when AppSettings.ShowWalkTroutPage:
                    {
                        Intent newIntent = new Intent(this, typeof(AppIntroWalkTroutPage));
                        newIntent?.PutExtra("class", "login");
                        StartActivity(newIntent);
                        break;
                    }
                    case "login":
                        StartActivity(new Intent(this, typeof(TabbedMainActivity)));
                        break;
                }

                Finish();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }


        private async Task SetProAsync()
        {
            try
            {
                if (Methods.CheckConnectivity())
                {
                    var (apiStatus, respond) = await RequestsAsync.Global.SetProAsync(PayId).ConfigureAwait(false);
                    switch (apiStatus)
                    {
                        case 200:
                            RunOnUiThread(() =>
                            {
                                try
                                {
                                    var dataUser = ListUtils.MyProfileList?.FirstOrDefault();
                                    if (dataUser != null)
                                    {
                                        dataUser.IsPro = "1";

                                        var sqlEntity = new SqLiteDatabase();
                                        sqlEntity.Insert_Or_Update_To_MyProfileTable(dataUser);
                                    
                                    }

                                    Toast.MakeText(this, GetText(Resource.String.Lbl_Upgraded), ToastLength.Long)?.Show();
                                    FinishPage();
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
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

    }
}
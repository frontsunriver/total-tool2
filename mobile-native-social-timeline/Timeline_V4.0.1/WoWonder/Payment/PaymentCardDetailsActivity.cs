using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.Content.Res;
using Com.Stripe.Android;
using Com.Stripe.Android.Model;
using Com.Stripe.Android.View;
using WoWonder.Activities.Base;
using WoWonder.Activities.Fundings;
using WoWonder.Activities.Wallet;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Math = System.Math;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Payment
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class PaymentCardDetailsActivity : BaseActivity, ITokenCallback ,  PaymentSession.IPaymentSessionListener, IEphemeralKeyProvider, IPaymentCompletionProvider , ISourceCallback
    {
        #region Variables Basic

        private TextView CardNumber, CardExpire, CardCvv, CardName;
        private EditText EtName;
        private Button BtnApply;
        private CardMultilineWidget MultilineWidget;

        private Stripe Stripe;
        private PaymentSession PaymentSession;

        private string Price, PayType, Id, TokenId;
      
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
                SetContentView(Resource.Layout.PaymentCardDetailsLayout);

                Id = Intent?.GetStringExtra("Id") ?? "";
                Price = Intent?.GetStringExtra("Price");
                PayType = Intent?.GetStringExtra("payType");

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                InitWalletStripe();
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
                CardNumber = (TextView)FindViewById(Resource.Id.card_number);
                CardExpire = (TextView)FindViewById(Resource.Id.card_expire);
                CardCvv = (TextView)FindViewById(Resource.Id.card_cvv);
                CardName = (TextView)FindViewById(Resource.Id.card_name);

                MultilineWidget = (CardMultilineWidget)FindViewById(Resource.Id.card_multiline_widget);

                EtName = (EditText)FindViewById(Resource.Id.et_name);
                BtnApply = (Button)FindViewById(Resource.Id.ApplyButton);

                Methods.SetColorEditText(EtName, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
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
                    toolBar.Title = GetString(Resource.String.Lbl_CreditCard);
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
                        MultilineWidget.CvcComplete += MultilineWidgetOnCvcComplete;
                        EtName.TextChanged += EtNameOnTextChanged;
                        BtnApply.Click += BtnApplyOnClick;
                        break;
                    default:
                        MultilineWidget.CvcComplete -= MultilineWidgetOnCvcComplete;
                        EtName.TextChanged -= EtNameOnTextChanged;
                        BtnApply.Click -= BtnApplyOnClick;
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

        private void MultilineWidgetOnCvcComplete(object sender, EventArgs e)
        {
            try
            {
                if (MultilineWidget.Card != null && MultilineWidget.Card.ValidateCard() && MultilineWidget.ValidateAllFields())
                {
                    switch (MultilineWidget.Card.Number.Trim().Length)
                    {
                        case 0:
                            CardNumber.Text = "**** **** **** ****";
                            break;
                        default:
                        {
                            string number = InsertPeriodically(MultilineWidget.Card.Number.Trim(), " ", 4);
                            CardNumber.Text = number;
                            break;
                        }
                    }

                    CardExpire.Text = MultilineWidget.Card.ExpMonth.ToString().Trim().Length switch
                    {
                        0 when MultilineWidget.Card.ExpYear.ToString().Trim().Length == 0 => "MM/YY",
                        _ => MultilineWidget.Card.ExpMonth + "/" + MultilineWidget.Card.ExpYear
                    };

                    CardCvv.Text = MultilineWidget.Card.CVC.Trim().Length == 0 ? "***" : MultilineWidget.Card.CVC.Trim();
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        private void EtNameOnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CardName.Text = e.Text.ToString().Trim().Length == 0 ? GetString(Resource.String.Lbl_YourName) : e.Text.ToString().Trim();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Stripe
        private void BtnApplyOnClick(object sender, EventArgs e)
        {
            try
            {
                //Show a progress
                if (MultilineWidget.Card.ValidateCard() && !string.IsNullOrEmpty(EtName.Text))
                {
                    AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));
                    
                    Card card = MultilineWidget.Card;
                    Stripe.CreateToken(card, PaymentConfiguration.Instance.PublishableKey, this);
                }
                else
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_PleaseVerifyDataCard), ToastLength.Long)?.Show();
                }
            }
            catch (Exception exception)
            {
                AndHUD.Shared.Dismiss(this);
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private string InsertPeriodically(string text, string insert, int period)
        {
            try
            {
                var parts = SplitInParts(text, period);
                string formatted = string.Join(insert, parts);
                return formatted;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return text;
            }
        }

        public static IEnumerable<string> SplitInParts(string s, int partLength)
        {
            switch (s)
            {
                case null:
                    throw new ArgumentNullException("s");
            }
            switch (partLength)
            {
                case <= 0:
                    throw new ArgumentException("Part length has to be positive.", "partLength");
            }

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

        #endregion

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
                if (data != null)
                    PaymentSession?.HandlePaymentData(requestCode, (int)resultCode, data);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }


        #region Stripe

        private void InitWalletStripe()
        {
            try
            {
                var stripePublishableKey = ListUtils.SettingsSiteList?.StripeId ?? "";
                switch (string.IsNullOrEmpty(stripePublishableKey))
                {
                    case false:
                        PaymentConfiguration.Init(stripePublishableKey);
                        Stripe = new Stripe(this, stripePublishableKey);
                        break;
                    default:
                        Toast.MakeText(this, GetText(Resource.String.Lbl_ErrorConnectionSystemStripe), ToastLength.Long)?.Show();
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public void OnError(Java.Lang.Exception error)
        {
            try
            {
                AndHUD.Shared.Dismiss(this);
                Toast.MakeText(this, error.Message, ToastLength.Long)?.Show();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public async void OnSuccess(Source p0)
        {
            try
            {
                Console.WriteLine(p0);
                Console.WriteLine(p0.Status);
                
                if (p0.Status.Contains("succeeded"))
                {
                    switch (PayType)
                    {
                        //send api  
                        case "Funding":
                        {
                            var (apiStatus, respond) = await RequestsAsync.Funding.FundingPayAsync(Id, Price);
                            switch (apiStatus)
                            {
                                case 200:
                                    Toast.MakeText(this, GetText(Resource.String.Lbl_Donated), ToastLength.Long)?.Show();
                                    FundingViewActivity.GetInstance()?.StartApiService();
                                    break;
                                default:
                                    Methods.DisplayReportResult(this, respond);
                                    break;
                            }

                            break;
                        }
                        case "membership" when Methods.CheckConnectivity():
                        {
                            var (apiStatus, respond) = await RequestsAsync.Global.SetProAsync(Id);
                            switch (apiStatus)
                            {
                                case 200:
                                {
                                    var dataUser = ListUtils.MyProfileList?.FirstOrDefault();
                                    if (dataUser != null)
                                    {
                                        dataUser.IsPro = "1";

                                        var sqlEntity = new SqLiteDatabase();
                                        sqlEntity.Insert_Or_Update_To_MyProfileTable(dataUser);
                                    
                                    }

                                    Toast.MakeText(this, GetText(Resource.String.Lbl_Upgraded), ToastLength.Long)?.Show();
                                    break;
                                }
                                default:
                                    Methods.DisplayReportResult(this, respond);
                                    break;
                            }

                            break;
                        }
                        case "membership":
                            Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
                            break;
                        case "AddFunds":
                        {
                            var tabbedWallet = TabbedWalletActivity.GetInstance();
                            if (Methods.CheckConnectivity() && tabbedWallet != null)
                            {
                                var (apiStatus, respond) = await RequestsAsync.Global.SendMoneyWalletAsync(tabbedWallet.SendMoneyFragment?.UserId, tabbedWallet.SendMoneyFragment?.TxtAmount.Text);
                                switch (apiStatus)
                                {
                                    case 200:
                                        tabbedWallet.SendMoneyFragment.TxtAmount.Text = string.Empty;
                                        tabbedWallet.SendMoneyFragment.TxtEmail.Text = string.Empty;

                                        Toast.MakeText(this, GetText(Resource.String.Lbl_Sent_successfully), ToastLength.Long)?.Show();
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

                            break;
                        }
                    }

                    AndHUD.Shared.Dismiss(this);
                    Finish();
                }
                 
                //await Task.Run(() =>
                //{
                //    try
                //    {
                //      var sourceIdParams =  PaymentIntentParams.CreateConfirmPaymentIntentWithSourceIdParams(p0.Id, p0.ClientSecret, "stripe://payment_intent_return");

                //        PaymentIntent paymentIntent = Stripe.ConfirmPaymentIntentSynchronous(sourceIdParams, "pk_test_1ujWeV5SjafkpuEK7NMpURNz");
                //        Methods.DisplayReportResultTrack(paymentIntent);
                //    }
                //    catch (Exception e)
                //    {
                //        Methods.DisplayReportResultTrack(e); 
                //    } 
                //}); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public  void OnSuccess(Token token)
        {
            try
            {
                // Send token to your own web service
                //var stripeBankAccount = token.BankAccount;
                //var stripeCard = token.Card;
                //var stripeCreated = token.Created;
                TokenId = token.Id;
                //var stripeLiveMode = token.Livemode;
                //var stripeType = token.Type;
                //var stripeUsed = token.Used;
                var currencyCode = ListUtils.SettingsSiteList?.StripeCurrency ?? "USD";
                 
                CustomerSession.InitCustomerSession(this);
                CustomerSession.Instance.SetCustomerShippingInformation(this, new ShippingInformation());
                CustomerSession.Instance.AddProductUsageTokenIfValid(TokenId);

                // Create the PaymentSession
                PaymentSession = new PaymentSession(this);
                PaymentSession.Init(this, GetPaymentSessionConfig());
                 
                var priceInt = Convert.ToInt32(Price) * 100;
                Stripe.CreateSource(SourceParams.CreateAlipaySingleUseParams(priceInt, currencyCode.ToLower(), EtName.Text,UserDetails.Email, "stripe://payment_intent_return"), this); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                AndHUD.Shared.Dismiss(this);
            }
        }

        #endregion

        public PaymentSessionConfig GetPaymentSessionConfig()
        {
            try
            {
                PaymentSessionConfig config = new PaymentSessionConfig.Builder()
                    .SetShippingMethodsRequired(true)
                    .SetShippingInfoRequired(true)
                    .SetPrepopulatedShippingInfo(new ShippingInformation(
                        new Address.Builder()
                            .SetLine1("123 Market St")
                            .SetCity("San Francisco")
                            .SetState("CA")
                            .SetPostalCode("94107")
                            .SetCountry("US")
                            .Build(),
                        "Jenny Rosen",
                        "4158675309"
                    ))
                    .Build();
                return config;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null;
            }
        }

        public void OnCommunicatingStateChanged(bool isCommunicating)
        {
             
        }

        public void OnError(int errorCode, string errorMessage)
        {
            try
            {
                AndHUD.Shared.Dismiss(this);
                Toast.MakeText(this, errorMessage, ToastLength.Long)?.Show();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnPaymentSessionDataChanged(PaymentSessionData data)
        {
            try
            {
                switch (data.PaymentReadyToCharge)
                {
                    case true:
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void CreateEphemeralKey(string p0, IEphemeralKeyUpdateListener p1)
        {
            try
            {

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void CompletePayment(PaymentSessionData p0, IPaymentResultListener p1)
        {
            try
            {

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }
}
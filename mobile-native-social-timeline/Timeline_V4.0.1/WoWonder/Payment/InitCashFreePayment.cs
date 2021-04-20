using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Util;
using Android.Webkit;
using Android.Widget;
using AndroidHUD;
using WoWonder.Activities.Fundings;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using WoWonderClient.Classes.Global;
using WoWonderClient.Requests;
using Exception = System.Exception;

namespace WoWonder.Payment
{
    public class InitCashFreePayment 
    { 
        private readonly Activity ActivityContext;
        private Dialog CashFreeWindow;
        private WebView HybridView;
        private string  Price, PayType, Id;
        private CashFreeObject CashFreeObject;

        public InitCashFreePayment(Activity context)
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

        public void DisplayCashFreePayment(CashFreeObject cashFreeObject, string payType, string price, string id)
        {
            try
            {
                CashFreeObject = cashFreeObject;
                Price = price;
                PayType = payType;
                Id = id;

                CashFreeWindow = new Dialog(ActivityContext, AppSettings.SetTabDarkTheme ? Resource.Style.MyDialogThemeDark : Resource.Style.MyDialogTheme);
                CashFreeWindow.SetContentView(Resource.Layout.PaymentWebViewLayout);

                var title = (TextView)CashFreeWindow.FindViewById(Resource.Id.toolbar_title);
                if (title != null)
                    title.Text = ActivityContext.GetText(Resource.String.Lbl_PayWith) + " " + ActivityContext.GetText(Resource.String.Lbl_CashFree);

                var closeButton = (TextView)CashFreeWindow.FindViewById(Resource.Id.toolbar_close);
                if (closeButton != null)
                {
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, closeButton, IonIconsFonts.Close);

                    closeButton.SetTextSize(ComplexUnitType.Sp, 20f);
                    closeButton.Click += CloseButtonOnClick;
                }

                HybridView = CashFreeWindow.FindViewById<WebView>(Resource.Id.LocalWebView);

                //Set WebView
                HybridView.SetWebViewClient(new MyWebViewClient(this));
                if (HybridView.Settings != null)
                {
                    HybridView.Settings.LoadsImagesAutomatically = true;
                    HybridView.Settings.JavaScriptEnabled = true;
                    HybridView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
                    HybridView.Settings.SetLayoutAlgorithm(WebSettings.LayoutAlgorithm.TextAutosizing);
                    HybridView.Settings.DomStorageEnabled = true;
                    HybridView.Settings.AllowFileAccess = true;
                    HybridView.Settings.DefaultTextEncodingName = "utf-8";

                    HybridView.Settings.UseWideViewPort = true;
                    HybridView.Settings.LoadWithOverviewMode = true;

                    HybridView.Settings.SetSupportZoom(false);
                    HybridView.Settings.BuiltInZoomControls = false;
                    HybridView.Settings.DisplayZoomControls = false;

                    switch (string.IsNullOrEmpty(CashFreeObject.JsonForm))
                    {
                        case false:
                            //Load url to be rendered on WebView
                            HybridView.LoadUrl(CashFreeObject.JsonForm);

                            CashFreeWindow.Show();
                            break;
                    }
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
          
        private void CloseButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                CashFreeWindow.Hide();
                CashFreeWindow.Dismiss();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public void StopCashFree()
        {
            try
            {
                if (CashFreeWindow != null)
                {
                    CashFreeWindow.Hide();
                    CashFreeWindow.Dismiss();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private async Task CashFree(CashFreeGetStatusObject statusObject, string request)
        {
            try
            {
                if (Methods.CheckConnectivity())
                {
                    var keyValues = new Dictionary<string, string>
                    {
                        {"txStatus", statusObject.TxStatus}, 
                        {"orderAmount", statusObject.OrderAmount},
                        {"referenceId", statusObject.ReferenceId},
                        {"paymentMode", statusObject.PaymentMode},
                        {"txMsg", statusObject.TxMsg},
                        {"txTime", statusObject.TxTime},

                        {"signature", CashFreeObject.Signature}, 
                        {"orderId", CashFreeObject.OrderId},
                    }; 

                    switch (request)
                    {
                        case "fund":
                            keyValues.Add("amount", Price);
                            keyValues.Add("fund_id", Id);
                            break;
                        case "upgrade":
                            keyValues.Add("pro_type", Id);
                            break;
                        case "wallet":
                            keyValues.Add("amount", Price);
                            break;
                    }

                    var (apiStatus, respond) = await RequestsAsync.Global.CashFreeAsync(request, keyValues);
                    switch (apiStatus)
                    {
                        case 200:
                            AndHUD.Shared.Dismiss(ActivityContext);

                            switch (request)
                            {
                                case "fund":
                                    Toast.MakeText(ActivityContext, ActivityContext.GetText(Resource.String.Lbl_Donated), ToastLength.Long)?.Show();
                                    FundingViewActivity.GetInstance()?.StartApiService();
                                    break;
                                case "upgrade":
                                    var dataUser = ListUtils.MyProfileList?.FirstOrDefault();
                                    if (dataUser != null)
                                    {
                                        dataUser.IsPro = "1";

                                        var sqlEntity = new SqLiteDatabase();
                                        sqlEntity.Insert_Or_Update_To_MyProfileTable(dataUser);
                                    
                                    }

                                    Toast.MakeText(ActivityContext, ActivityContext.GetText(Resource.String.Lbl_Upgraded), ToastLength.Long)?.Show();
                                    break;
                                case "wallet":
                                    Toast.MakeText(ActivityContext, ActivityContext.GetText(Resource.String.Lbl_PaymentSuccessfully), ToastLength.Long)?.Show();
                                    break;
                            }
                         
                            StopCashFree();
                            break;
                        default:
                            Methods.DisplayAndHudErrorResult(ActivityContext, respond);
                            break;
                    }
                }
                else
                {
                    Toast.MakeText(ActivityContext, ActivityContext.GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
                }
            }
            catch (Exception e)
            {
                AndHUD.Shared.Dismiss(ActivityContext);
                Methods.DisplayReportResultTrack(e);
            }
        }

        private class MyWebViewClient : WebViewClient 
        {
            private readonly InitCashFreePayment MActivity;
            public MyWebViewClient(InitCashFreePayment mActivity)
            {
                MActivity = mActivity;
            }

            public override async void OnPageStarted(WebView view, string url, Bitmap favicon)
            {
                try
                { 
                    if (url.Contains("requests.php?f=cashfree"))
                    {
                        //Show a progress
                        AndHUD.Shared.Show(MActivity.ActivityContext, MActivity.ActivityContext.GetText(Resource.String.Lbl_Processing));

                        var (apiStatus, respond) = await RequestsAsync.Global.CashFreeGetStatusAsync(MActivity.CashFreeObject.AppId, ListUtils.SettingsSiteList?.CashfreeSecretKey ?? "", MActivity.CashFreeObject.OrderId, ListUtils.SettingsSiteList?.CashfreeMode);
                        switch (apiStatus)
                        {
                            case 200:
                            {
                                switch (respond)
                                {
                                    case CashFreeGetStatusObject result:
                                        switch (MActivity.PayType)
                                        {
                                            case "Funding":
                                                await MActivity.CashFree(result, "fund");
                                                break;
                                            case "membership":
                                                await MActivity.CashFree(result, "upgrade");
                                                break;
                                            case "AddFunds":
                                                await MActivity.CashFree(result, "wallet");
                                                break;
                                        }

                                        break;
                                }

                                break;
                            }
                            default:
                                Methods.DisplayReportResult(MActivity.ActivityContext, respond);
                                break;
                        } 
                    }
                    else
                    {
                        base.OnPageStarted(view, url, favicon);
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                } 
            } 
        }
    }
}
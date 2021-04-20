using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Util;
using Android.Webkit;
using Android.Widget;
using WoWonder.Activities.Fundings;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using WoWonderClient.Requests;
using Exception = System.Exception;

namespace WoWonder.Payment
{
    public class InitPaySeraPayment
    {
        private readonly Activity ActivityContext;
        private Dialog PaySeraWindow;
        private WebView HybridView;
        private string Url, Price, PayType, Id;
        public InitPaySeraPayment(Activity context)
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

        public void DisplayPaySeraPayment(string url, string payType, string price, string id)
        {
            try
            {
                Url = url;
                Price = price;
                PayType = payType;
                Id = id;

                PaySeraWindow = new Dialog(ActivityContext, AppSettings.SetTabDarkTheme ? Resource.Style.MyDialogThemeDark : Resource.Style.MyDialogTheme);
                PaySeraWindow.SetContentView(Resource.Layout.PaymentWebViewLayout);

                var title = (TextView)PaySeraWindow.FindViewById(Resource.Id.toolbar_title);
                if (title != null)
                    title.Text = ActivityContext.GetText(Resource.String.Lbl_PayWith) + " " + ActivityContext.GetText(Resource.String.Lbl_PaySera);

                var closeButton = (TextView)PaySeraWindow.FindViewById(Resource.Id.toolbar_close);
                if (closeButton != null)
                {
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, closeButton, IonIconsFonts.Close);

                    closeButton.SetTextSize(ComplexUnitType.Sp, 20f);
                    closeButton.Click += CloseButtonOnClick;
                }

                HybridView = PaySeraWindow.FindViewById<WebView>(Resource.Id.LocalWebView);

                //Set WebView
                if (HybridView != null)
                {
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
                    }

                    //Load url to be rendered on WebView
                    HybridView.LoadUrl(Url);
                }

                PaySeraWindow.Show();
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
                PaySeraWindow.Hide();
                PaySeraWindow.Dismiss();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public void StopPaySera()
        {
            try
            {
                if (PaySeraWindow != null)
                {
                    PaySeraWindow.Hide();
                    PaySeraWindow.Dismiss();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private async Task PaySera(string request)
        {
            try
            {
                if (Methods.CheckConnectivity())
                {
                    var keyValues = new Dictionary<string, string>();

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

                    var (apiStatus, respond) = await RequestsAsync.Global.PaySeraAsync(request, keyValues);
                    switch (apiStatus)
                    {
                        case 200:
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

                            StopPaySera();
                            break;
                        default:
                            Methods.DisplayReportResult(ActivityContext, respond);
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
                Methods.DisplayReportResultTrack(e);
            }
        }

        private class MyWebViewClient : WebViewClient 
        {
            private readonly InitPaySeraPayment MActivity;
            public MyWebViewClient(InitPaySeraPayment mActivity)
            {
                MActivity = mActivity;
            }

            public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
            {
                try
                {
                    if (string.IsNullOrEmpty(request?.Url?.ToString()))
                        return false;

                    if (request.Url.ToString() == MActivity.Url)
                    {
                        view.LoadUrl(request.Url.ToString());
                    }
                    else if (request.Url.ToString().Contains("reference"))
                    {
                        //https://demo.wowonder.com/requests.php?f=PaySera&s=fund&amount=12&fund_id=403&trxref=5f3f88e2a43c7&reference=5f3f88e2a43c7

                        var reference = request.Url.ToString()?.Split("&reference=")?.Last();
                        if (string.IsNullOrEmpty(reference))
                            return false;

                        switch (MActivity.PayType)
                        {
                            case "Funding":
                                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => MActivity.PaySera( "fund") });
                                break;
                            case "membership":
                                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => MActivity.PaySera("upgrade") });
                                break;
                            case "AddFunds":
                                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => MActivity.PaySera("wallet") });
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
                return false;
            }

            public override void OnPageStarted(WebView view, string url, Bitmap favicon)
            {
                try
                {
                    base.OnPageStarted(view, url, favicon);

                    if (view.Settings != null)
                    {
                        view.Settings.JavaScriptEnabled = true;
                        view.Settings.DomStorageEnabled = true;
                        view.Settings.AllowFileAccess = true;
                        view.Settings.JavaScriptCanOpenWindowsAutomatically = true;
                        view.Settings.UseWideViewPort = true;
                        view.Settings.LoadWithOverviewMode = true;
                        view.Settings.SetSupportZoom(false);
                        view.Settings.BuiltInZoomControls = false;
                        view.Settings.DisplayZoomControls = false;
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
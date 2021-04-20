using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS;

using Android.Views;
using Android.Webkit;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using AndroidX.SwipeRefreshLayout.Widget;
using WoWonder.Activities.Base;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Utils;
using WoWonderClient;
using Exception = System.Exception;
using Object = Java.Lang.Object;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class LocalWebViewActivity : BaseActivity
    {
        #region Variables Basic

        private SwipeRefreshLayout SwipeRefreshLayout;
        private WebView HybridView;
        private string Url = "", TypeUrl = "";
        private AdView MAdView;

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
                SetContentView(Resource.Layout.LocalWebViewLayout);

                var data = Intent?.GetStringExtra("URL") ?? "Data not available";
                if (data != "Data not available" && !string.IsNullOrEmpty(data))
                {
                    Url = data;
                }
                var type = Intent?.GetStringExtra("Type") ?? "Data not available";
                if (type != "Data not available" && !string.IsNullOrEmpty(type))
                {
                    TypeUrl = type;
                }

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetWebView(); 
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

                MAdView?.Resume();
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

                MAdView?.Pause();
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
                HybridView = FindViewById<WebView>(Resource.Id.LocalWebView);
                SwipeRefreshLayout = (SwipeRefreshLayout)FindViewById(Resource.Id.swipeRefreshLayout);
                SwipeRefreshLayout.SetColorSchemeResources(Android.Resource.Color.HoloBlueLight, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
                SwipeRefreshLayout.Refreshing = true;
                SwipeRefreshLayout.Enabled = true;
                SwipeRefreshLayout.SetProgressBackgroundColorSchemeColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#424242") : Color.ParseColor("#f7f7f7"));
 

                MAdView = FindViewById<AdView>(Resource.Id.adView);
                AdsGoogle.InitAdView(MAdView, null); 
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
                Toolbar toolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
                if (toolBar != null)
                {
                    toolBar.Title = TypeUrl;
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

        private void SetWebView()
        {
            try
            {
                //Set WebView and Load url to be rendered on WebView
                if (!Methods.CheckConnectivity())
                { 
                    SwipeRefreshLayout.Refreshing = false;

                    Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
                }
                else
                {
                    if (Url.Contains(Client.WebsiteUrl))
                    { 
                        //Set WebView
                        HybridView.SetWebViewClient(new MyWebViewClient(this));

                        switch (AppSettings.Lang)
                        {
                            case "en":
                                HybridView.LoadUrl(Url + "?lang=english");
                                break;
                            case "ar":
                                HybridView.LoadUrl(Url + "?lang=arabic");
                                AppSettings.FlowDirectionRightToLeft = true;
                                break;
                            case "de":
                                HybridView.LoadUrl(Url + "?lang=german");
                                break;
                            case "el":
                                HybridView.LoadUrl(Url + "?lang=greek");
                                break;
                            case "es":
                                HybridView.LoadUrl(Url + "?lang=spanish");
                                break;
                            case "fr":
                                HybridView.LoadUrl(Url + "?lang=french");
                                break;
                            case "it":
                                HybridView.LoadUrl(Url + "?lang=italian");
                                break;
                            case "ja":
                                HybridView.LoadUrl(Url + "?lang=japanese");
                                break;
                            case "nl":
                                HybridView.LoadUrl(Url + "?lang=dutch");
                                break;
                            case "pt":
                                HybridView.LoadUrl(Url + "?lang=portuguese");
                                break;
                            case "ro":
                                HybridView.LoadUrl(Url + "?lang=romanian");
                                break;
                            case "ru":
                                HybridView.LoadUrl(Url + "?lang=russian");
                                break;
                            case "sq":
                                HybridView.LoadUrl(Url + "?lang=albanian");
                                break;
                            case "sr":
                                HybridView.LoadUrl(Url + "?lang=serbian");
                                break;
                            case "tr":
                                HybridView.LoadUrl(Url + "?lang=turkish");
                                break;
                            default:
                                HybridView.LoadUrl(Url);
                                break;
                        } 
                    }
                    else
                    {
                        //Set WebView
                        HybridView.SetWebViewClient(new MyWebViewClient(this));
                        //Load url to be rendered on WebView
                        HybridView.LoadUrl(Url);
                    } 
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
                        SwipeRefreshLayout.Refresh += SwipeRefreshLayoutOnRefresh;
                        break;
                    default:
                        SwipeRefreshLayout.Refresh -= SwipeRefreshLayoutOnRefresh;
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
                MAdView?.Destroy();

                HybridView = null!;
                SwipeRefreshLayout = null!;
                MAdView = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        //Event Refresh Data Page
        private void SwipeRefreshLayoutOnRefresh(object sender, EventArgs e)
        {
            try
            {
                HybridView.Reload();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        private class MyWebViewClient : WebViewClient, IValueCallback
        {
            private readonly LocalWebViewActivity ViewActivity;
            public MyWebViewClient(LocalWebViewActivity mActivity)
            {
                ViewActivity = mActivity;
            }

            public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
            {
                view.LoadUrl(request.Url.ToString());
                return true;
            }

            public override void OnPageStarted(WebView view, string url, Bitmap favicon)
            {
                try
                {
                    base.OnPageStarted(view, url, favicon);
                    ViewActivity.SwipeRefreshLayout.Refreshing = true;
                    ViewActivity.SwipeRefreshLayout.Enabled = true;

                    view.Settings.JavaScriptEnabled = true;
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public override void OnPageFinished(WebView view, string url)
            {
                try
                {
                    base.OnPageFinished(view, url);
                    ViewActivity.SwipeRefreshLayout.Refreshing = false;
                    ViewActivity.SwipeRefreshLayout.Enabled = false;

                    const string js = "javascript:" +
                                      "$('.header-container').hide();" +
                                      "$('.footer-wrapper').hide();" +
                                      "$('.content-container').css('margin-top', '0');" +
                                      "$('.wo_about_wrapper_parent').css('top', '0');";

                    switch (Build.VERSION.SdkInt)
                    {
                        case >= (BuildVersionCodes)19:
                            view.EvaluateJavascript(js, this);
                            break;
                        default:
                            view.LoadUrl(js);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public override void OnReceivedError(WebView view, IWebResourceRequest request, WebResourceError error)
            {
                try
                {
                    base.OnReceivedError(view, request, error);
                    ViewActivity.SwipeRefreshLayout.Refreshing = false;
                    ViewActivity.SwipeRefreshLayout.Enabled = false;
                    const string js = "javascript:" +
                                      "$('.header-container').hide();" +
                                      "$('.footer-wrapper').hide();" +
                                      "$('.content-container').css('margin-top', '0');" +
                                      "$('.wo_about_wrapper_parent').css('top', '0');";

                    switch (Build.VERSION.SdkInt)
                    {
                        case >= (BuildVersionCodes)19:
                            view.EvaluateJavascript(js, this);
                            break;
                        default:
                            view.LoadUrl(js);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public void OnReceiveValue(Object value)
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
}
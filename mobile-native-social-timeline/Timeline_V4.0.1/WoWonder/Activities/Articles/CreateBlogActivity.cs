using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS;
using Android.Provider;

using Android.Views;
using Android.Webkit;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using AndroidX.SwipeRefreshLayout.Widget;
using WoWonder.Activities.Base;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient;
using Console = System.Console;
using Object = Java.Lang.Object;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Articles
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class CreateBlogActivity : BaseActivity
    {
        #region Variables Basic

        private SwipeRefreshLayout SwipeRefreshLayout;
        private WebView HybridView;
        private string Url = "";
        private AdView MAdView;
        private static IValueCallback MUm;
        private static IValueCallback MUma;
        private static readonly int Fcr = 1;

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
                var toolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
                if (toolBar != null)
                {
                    toolBar.Title = GetText(Resource.String.Lbl_CreateArticle);
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
                    switch ((int)Build.VERSION.SdkInt)
                    {
                        case < 23:
                            LoadWebView();
                            break;
                        default:
                        {
                            if (CheckSelfPermission(Manifest.Permission.ReadExternalStorage) == Permission.Granted && CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == Permission.Granted
                                && CheckSelfPermission(Manifest.Permission.Camera) == Permission.Granted && CheckSelfPermission(Manifest.Permission.AccessMediaLocation) == Permission.Granted)
                            {
                                LoadWebView();
                            }
                            else
                            {
                                new PermissionsController(this).RequestPermission(108);
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

        private void LoadWebView()
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
                    //Set WebView   
                    WebSettings webSettings = HybridView.Settings;
                    HybridView.SetWebViewClient(new MyWebViewClient(this));
                    HybridView.SetWebChromeClient(new MyWebChromeClient(this));

                    webSettings.JavaScriptEnabled = true;
                    webSettings.AllowFileAccess = true;
                    webSettings.LoadsImagesAutomatically = true;
                    webSettings.JavaScriptCanOpenWindowsAutomatically = true;
                    webSettings.SetLayoutAlgorithm(WebSettings.LayoutAlgorithm.TextAutosizing);
                    webSettings.DomStorageEnabled = true;
                    HybridView.ClearCache(true);
                    webSettings.UseWideViewPort = true;
                    webSettings.LoadWithOverviewMode = true;

                    webSettings.SetSupportZoom(false);
                    webSettings.BuiltInZoomControls = false;
                    webSettings.DisplayZoomControls = false;

                    HybridView.CopyBackForwardList();
                    HybridView.CanGoBackOrForward(0);

                    Url = AppSettings.SetTabDarkTheme switch
                    {
                        //Load url to be rendered on WebView
                        true => Client.WebsiteUrl + "/create-blog?c_id=" + UserDetails.AccessToken + "&user_id=" +
                                UserDetails.UserId + "&application=phone&mode=night",
                        _ => Client.WebsiteUrl + "/create-blog?c_id=" + UserDetails.AccessToken + "&user_id=" +
                             UserDetails.UserId + "&application=phone"
                    };

                    switch (AppSettings.Lang)
                    {
                        case "en":
                            HybridView.LoadUrl(Url + "&lang=english");
                            break;
                        case "ar":
                            HybridView.LoadUrl(Url + "&lang=arabic");
                            AppSettings.FlowDirectionRightToLeft = true;
                            break;
                        case "de":
                            HybridView.LoadUrl(Url + "&lang=german");
                            break;
                        case "el":
                            HybridView.LoadUrl(Url + "&lang=greek");
                            break;
                        case "es":
                            HybridView.LoadUrl(Url + "&lang=spanish");
                            break;
                        case "fr":
                            HybridView.LoadUrl(Url + "&lang=french");
                            break;
                        case "it":
                            HybridView.LoadUrl(Url + "&lang=italian");
                            break;
                        case "ja":
                            HybridView.LoadUrl(Url + "&lang=japanese");
                            break;
                        case "nl":
                            HybridView.LoadUrl(Url + "&lang=dutch");
                            break;
                        case "pt":
                            HybridView.LoadUrl(Url + "&lang=portuguese");
                            break;
                        case "ro":
                            HybridView.LoadUrl(Url + "&lang=romanian");
                            break;
                        case "ru":
                            HybridView.LoadUrl(Url + "&lang=russian");
                            break;
                        case "sq":
                            HybridView.LoadUrl(Url + "&lang=albanian");
                            break;
                        case "sr":
                            HybridView.LoadUrl(Url + "&lang=serbian");
                            break;
                        case "tr":
                            HybridView.LoadUrl(Url + "&lang=turkish");
                            break;
                        default:
                            HybridView.LoadUrl(Url);
                            break;
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

        #region Result & Permissions

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);

                switch ((int)Build.VERSION.SdkInt)
                {
                    case >= 21:
                    {
                        Android.Net.Uri[] results = null;
                        switch (resultCode)
                        {
                            //Check if response is positive
                            case Result.Ok:
                            {
                                if (requestCode == Fcr)
                                {
                                    switch (MUma)
                                    {
                                        case null:
                                            return;
                                    }

                                    var dataString = data?.Data?.ToString();
                                    if (dataString != null)
                                    {
                                        results = new[]
                                        {
                                            Android.Net.Uri.Parse(dataString)
                                        };
                                    }
                                }

                                break;
                            }
                        }
                        MUma.OnReceiveValue(results);
                        MUma = null;
                        break;
                    }
                    default:
                    {
                        if (requestCode == Fcr)
                        {
                            switch (MUm)
                            {
                                case null:
                                    return;
                            }

                            Android.Net.Uri result = data == null || resultCode != Result.Ok ? null : data.Data;
                            MUm.OnReceiveValue(result);
                            MUm = null;
                        }

                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            try
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

                switch (requestCode)
                {
                    case 108 when grantResults.Length > 0 && grantResults[0] == Permission.Granted:
                        LoadWebView();
                        break;
                    case 108:
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Permission_is_denied), ToastLength.Long)?.Show();
                        Finish();
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        private class MyWebViewClient : WebViewClient, IValueCallback
        {
            private readonly CreateBlogActivity MActivity;
            public MyWebViewClient(CreateBlogActivity mActivity)
            {
                MActivity = mActivity;
            }

            public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
            {
                try
                {
                    if (request.Url.ToString() == MActivity.Url)
                    {
                        view.LoadUrl(request.Url.ToString());
                    }
                    else if (request.Url.ToString().Contains("read-blog"))
                    {
                        var con = ArticlesActivity.GetInstance();
                        if (con != null)
                        {
                            con.SwipeRefreshLayout.Refreshing = true;

                            con.MAdapter.ArticlesList.Clear();
                            con.MAdapter.NotifyDataSetChanged();

                            con.MainScrollEvent.IsLoading = false;

                            con.StartApiService();
                        }
                        MActivity.Finish();
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

                    view.Settings.JavaScriptEnabled = true;
                    view.Settings.DomStorageEnabled = true;
                    view.Settings.AllowFileAccess = true;
                    view.Settings.JavaScriptCanOpenWindowsAutomatically = true;
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
                    MActivity.SwipeRefreshLayout.Refreshing = false;
                    MActivity.SwipeRefreshLayout.Enabled = false;

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
                    MActivity.SwipeRefreshLayout.Refreshing = false;
                    MActivity.SwipeRefreshLayout.Enabled = false;

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

        private class MyWebChromeClient : WebChromeClient
        {
            private readonly Activity MActivity;

            public MyWebChromeClient(Activity mActivity)
            {
                MActivity = mActivity;
            }
             
            [Android.Runtime.Register("onShowFileChooser", "(Landroid/webkit/WebView;Landroid/webkit/ValueCallback;Landroid/webkit/WebChromeClient$FileChooserParams;)Z", "GetOnShowFileChooser_Landroid_webkit_WebView_Landroid_webkit_ValueCallback_Landroid_webkit_WebChromeClient_FileChooserParams_Handler", ApiSince = 21)]
            public override bool OnShowFileChooser(WebView webView, IValueCallback filePathCallback, FileChooserParams fileChooserParams)
            {
                try
                {
                    MUma?.OnReceiveValue(null);

                    MUma = filePathCallback;
                     
                    Intent contentSelectionIntent = Android.OS.Environment.GetExternalStorageState(null)!.Equals(Android.OS.Environment.MediaMounted) ? new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri) : new Intent(Intent.ActionPick, MediaStore.Images.Media.InternalContentUri);
                    contentSelectionIntent.SetType("image/*");
                    contentSelectionIntent.PutExtra("return-data", true); //added snippet
                    contentSelectionIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    MActivity.StartActivityForResult(Intent.CreateChooser(contentSelectionIntent, "image Chooser"), Fcr);

                    return true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    return false;
                }
            }
        }
         
    }
}
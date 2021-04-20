using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Gms.Ads;
using Android.Gms.Ads.AppOpen;
using Android.Gms.Ads.DoubleClick;
using Android.Gms.Ads.Formats;
using Android.Gms.Ads.Initialization;
using Android.Gms.Ads.Rewarded;
using Android.Gms.Ads.RewardedInterstitial;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using Exception = System.Exception;
using Object = Java.Lang.Object;

namespace WoWonder.Helpers.Ads
{
    public static class AdsGoogle
    {
        private static int CountInterstitial;
        private static int CountRewarded;
        private static int CountAppOpen;
        private static int CountRewardedInterstitial;

        #region Interstitial

        private class AdMobInterstitial
        {
            private InterstitialAd Ad;

            public void ShowAd(Context context)
            {
                try
                {
                    Ad = new InterstitialAd(context) { AdUnitId = AppSettings.AdInterstitialKey };

                    var listener = new InterstitialAdListener(Ad);
                    listener.OnAdLoaded();
                    Ad.AdListener = listener;

                    var requestBuilder = new AdRequest.Builder();
#pragma warning disable 618
                    requestBuilder.AddTestDevice(UserDetails.AndroidId);
#pragma warning restore 618
                    Ad.LoadAd(requestBuilder.Build());
                }
                catch (Exception exception)
                {
                    Methods.DisplayReportResultTrack(exception);
                }
            }
        }

        private class InterstitialAdListener : AdListener
        {
            private readonly InterstitialAd Ad;

            public InterstitialAdListener(InterstitialAd ad)
            {
                Ad = ad;
            }

            public override void OnAdLoaded()
            {
                try
                {
                    base.OnAdLoaded();

                    if (Ad != null && Ad.IsLoaded)
                        Ad?.Show();
                }
                catch (Exception exception)
                {
                    Methods.DisplayReportResultTrack(exception);
                }
            }
        }


        public static void Ad_Interstitial(Activity context)
        {
            try
            {
                switch (AppSettings.ShowAdMobInterstitial)
                {
                    case true:
                    {
                        if (CountInterstitial == AppSettings.ShowAdMobInterstitialCount)
                        {
                            CountInterstitial = 0;
                            AdMobInterstitial ads = new AdMobInterstitial();
                            ads.ShowAd(context);
                        }
                        else
                        {
                            Ad_AppOpenManager(context);
                        }

                        CountInterstitial++;
                        break;
                    }
                    default:
                        Ad_AppOpenManager(context);
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Native

        private class AdMobNative : Object, UnifiedNativeAd.IOnUnifiedNativeAdLoadedListener
        {
            private TemplateView Template;
            private Activity Context;
            public void ShowAd(Activity context, TemplateView template = null)
            {
                try
                {
                    Context = context;

                    Template = template ?? Context.FindViewById<TemplateView>(Resource.Id.my_template);
                    if (Template != null)
                    {
                        Template.Visibility = ViewStates.Gone;

                        switch (AppSettings.ShowAdMobNative)
                        {
                            case true:
                            {
                                AdLoader.Builder builder = new AdLoader.Builder(Context, AppSettings.AdAdMobNativeKey);
                                builder.ForUnifiedNativeAd(this);
                                VideoOptions videoOptions = new VideoOptions.Builder()
                                    .SetStartMuted(true)
                                    .Build();
                                NativeAdOptions adOptions = new NativeAdOptions.Builder()
                                    .SetVideoOptions(videoOptions)
                                    .Build();

                                builder.WithNativeAdOptions(adOptions);

                                AdLoader adLoader = builder.WithAdListener(new AdListener()).Build();
                                adLoader.LoadAd(new AdRequest.Builder().Build());
                                break;
                            }
                            default:
                                Template.Visibility = ViewStates.Gone;
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public void OnUnifiedNativeAdLoaded(UnifiedNativeAd ad)
            {
                try
                {
                    NativeTemplateStyle styles = new NativeTemplateStyle.Builder().Build();

                    if (Template.GetTemplateTypeName() == TemplateView.NativeContentAd)
                    {
                        Template.NativeContentAdView(ad);
                    }
                    else
                    {
                        Template.SetStyles(styles);
                        Template.SetNativeAd(ad);
                    }

                    Template.Visibility = ViewStates.Visible;
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }
        }

        public static void Ad_AdMobNative(Activity context, TemplateView template = null)
        {
            try
            {
                switch (AppSettings.ShowAdMobNative)
                {
                    case true:
                    {
                        AdMobNative ads = new AdMobNative();
                        ads.ShowAd(context, template);
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Rewarded

        public class AdMobRewardedVideo : RewardedAdLoadCallback
        {
            private RewardedAd Rad;
            private Activity Context;
            public void ShowAd(Activity context)
            {
                try
                {
                    Context = context;

                    // Use an activity context to get the rewarded video instance. 
                    Rad = new RewardedAd(context, AppSettings.AdRewardVideoKey);

                    AdRequest adRequest = new AdRequest.Builder().Build();
                    Rad.LoadAd(adRequest, this);
                }
                catch (Exception exception)
                {
                    Methods.DisplayReportResultTrack(exception);
                }
            }

            public override void OnRewardedAdLoaded()
            {
                try
                {
                    base.OnRewardedAdLoaded();

                    if (Rad != null && Rad.IsLoaded)
                        Rad.Show(Context, new MyRewardedAdCallback(Rad));
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public override void OnRewardedAdFailedToLoad(LoadAdError p0)
            {
                try
                {
                    base.OnRewardedAdFailedToLoad(p0);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            private class MyRewardedAdCallback : RewardedAdCallback
            {
                private RewardedAd Rad;
                public MyRewardedAdCallback(RewardedAd rad)
                {
                    Rad = rad;
                }

                public override void OnRewardedAdOpened()
                {
                    try
                    {
                        base.OnRewardedAdOpened();
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }

                public override void OnRewardedAdClosed()
                {
                    try
                    {
                        base.OnRewardedAdClosed();
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }

                public override void OnRewardedAdFailedToShow(AdError p0)
                {
                    try
                    {
                        base.OnRewardedAdFailedToShow(p0);
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }

                public override void OnUserEarnedReward(IRewardItem p0)
                {
                    try
                    {
                        //finish time ad    
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }
            }
        }

        public static AdMobRewardedVideo Ad_RewardedVideo(Activity context)
        {
            try
            {
                switch (AppSettings.ShowAdMobRewardVideo)
                {
                    case true when CountRewarded == AppSettings.ShowAdMobRewardedVideoCount:
                    {
                        CountRewarded = 0;
                        AdMobRewardedVideo ads = new AdMobRewardedVideo();
                        ads.ShowAd(context);
                        return ads;
                    }
                    case true:
                        Ad_RewardedInterstitial(context);
                        break;
                    default:
                        Ad_RewardedInterstitial(context);
                        break;
                }

                return null!;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }

        #endregion

        #region Banner

        public static void InitAdView(AdView mAdView, RecyclerView mRecycler)
        {
            try
            {
                switch (mAdView)
                {
                    case null:
                        return;
                }

                switch (AppSettings.ShowAdMobBanner)
                {
                    case true:
                    {
                        mAdView.Visibility = ViewStates.Visible;
                        var adRequest = new AdRequest.Builder();
#pragma warning disable 618
                        adRequest.AddTestDevice(UserDetails.AndroidId);
#pragma warning restore 618
                        mAdView.LoadAd(adRequest.Build());
                        mAdView.AdListener = new MyAdListener(mAdView, mRecycler);
                        break;
                    }
                    default:
                    {
                        mAdView.Pause();
                        mAdView.Visibility = ViewStates.Gone;
                        if (mRecycler != null) Methods.SetMargin(mRecycler, 0, 0, 0, 0);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private class MyAdListener : AdListener
        {
            private readonly AdView MAdView;
            private readonly RecyclerView MRecycler;
            public MyAdListener(AdView mAdView, RecyclerView mRecycler)
            {
                MAdView = mAdView;
                MRecycler = mRecycler;
            }

            public override void OnAdFailedToLoad(LoadAdError p0)
            {
                try
                {
                    MAdView.Visibility = ViewStates.Gone;
                    if (MRecycler != null) Methods.SetMargin(MRecycler, 0, 0, 0, 0);
                    base.OnAdFailedToLoad(p0);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }


            public override void OnAdLoaded()
            {
                try
                {
                    MAdView.Visibility = ViewStates.Visible;

                    Resources r = Application.Context.Resources;
                    int px = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, MAdView.AdSize.Height, r.DisplayMetrics);
                    if (MRecycler != null) Methods.SetMargin(MRecycler, 0, 0, 0, px);

                    base.OnAdLoaded();
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }
        }

        #endregion

        #region Publisher

        public static void InitPublisherAdView(PublisherAdView mAdView)
        {
            try
            {
                switch (mAdView)
                {
                    case null:
                        return;
                }

                switch (AppSettings.ShowAdMobBanner)
                {
                    case true:
                    {
                        mAdView.Visibility = ViewStates.Visible;
                        var adRequest = new PublisherAdRequest.Builder();
#pragma warning disable 618
                        adRequest.AddTestDevice(UserDetails.AndroidId);
#pragma warning restore 618
                        mAdView.AdListener = new MyPublisherAdViewListener(mAdView);
                        mAdView.LoadAd(adRequest.Build());
                        break;
                    }
                    default:
                        mAdView.Pause();
                        mAdView.Visibility = ViewStates.Gone;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private class MyPublisherAdViewListener : AdListener
        {
            private readonly PublisherAdView MAdView;
            public MyPublisherAdViewListener(PublisherAdView mAdView)
            {
                MAdView = mAdView;
            }

            public override void OnAdFailedToLoad(LoadAdError p0)
            {
                try
                {
                    MAdView.Visibility = ViewStates.Gone;
                    base.OnAdFailedToLoad(p0);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public override void OnAdLoaded()
            {
                try
                {
                    MAdView.Visibility = ViewStates.Visible;
                    base.OnAdLoaded();
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }
        }

        #endregion

        #region AppOpen

        public static void Ad_AppOpenManager(Activity context)
        {
            try
            {
                switch (AppSettings.ShowAdMobAppOpen)
                {
                    case true:
                    {
                        if (CountAppOpen == AppSettings.ShowAdMobAppOpenCount)
                        {
                            CountAppOpen = 0;

                            AppOpenManager appOpenManager = new AppOpenManager(context);
                            appOpenManager.ShowAdIfAvailable();
                        }
                        else
                        {
                            AdsFacebook.InitInterstitial(context);
                        }

                        CountAppOpen++;
                        break;
                    }
                    default:
                        AdsFacebook.InitInterstitial(context);
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private class AppOpenManager : AppOpenAd.AppOpenAdLoadCallback
        {
            private static readonly long AdExpiryDuration = 3600000 * 4;
            private readonly Activity MostCurrentActivity;
            private static AppOpenAd Ad;
            private static bool IsShowingAd;
            private static long LastAdFetchTime;

            public AppOpenManager(Activity context)
            {
                try
                {
                    MostCurrentActivity = context;
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public bool IsAdExpired()
            {
                try
                {
                    return Methods.Time.CurrentTimeMillis() - LastAdFetchTime > AdExpiryDuration;
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                    return false;
                }
            }

            public bool IsAdAvailable()
            {
                try
                {
                    return Ad != null && !IsAdExpired();
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                    return false;
                }
            }

            public void FetchAd()
            {
                try
                {
                    if (IsAdAvailable())
                    {
                        return;
                    }

                    AdRequest request = new AdRequest.Builder().Build();
                    AppOpenAd.Load(MostCurrentActivity, AppSettings.AdAdMobAppOpenKey, request, AppOpenAd.AppOpenAdOrientationPortrait, this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public void ShowAdIfAvailable(FullScreenContentCallback listener)
            {
                try
                {
                    switch (IsShowingAd)
                    {
                        case true:
                            //Can't show the ad: Already showing the ad
                            return;
                    }

                    if (!IsAdAvailable())
                    {
                        //Can't show the ad: Ad not available
                        FetchAd();
                        return;
                    }

                    Ad.Show(MostCurrentActivity, new MyFullScreenContentCallback(this, listener));
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public void ShowAdIfAvailable()
            {
                try
                {
                    ShowAdIfAvailable(new FullScreenContentCallback());
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public override void OnAppOpenAdLoaded(AppOpenAd ad)
            {
                try
                {
                    base.OnAppOpenAdLoaded(ad);

                    LastAdFetchTime = Methods.Time.CurrentTimeMillis();
                    Ad = ad;
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public override void OnAppOpenAdFailedToLoad(LoadAdError error)
            {
                try
                {
                    base.OnAppOpenAdFailedToLoad(error);
                    Console.WriteLine("Failed to load an ad: " + error.Message);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            private class MyFullScreenContentCallback : FullScreenContentCallback
            {
                private readonly FullScreenContentCallback Listener;
                private readonly AppOpenManager AppOpenAdManager;
                public MyFullScreenContentCallback(AppOpenManager appOpenManager, FullScreenContentCallback listener)
                {
                    try
                    {
                        Listener = listener;
                        AppOpenAdManager = appOpenManager;
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }

                public override void OnAdFailedToShowFullScreenContent(AdError p0)
                {
                    try
                    {
                        base.OnAdFailedToShowFullScreenContent(p0);
                        Listener?.OnAdFailedToShowFullScreenContent(p0);
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }

                public override void OnAdShowedFullScreenContent()
                {
                    try
                    {
                        base.OnAdShowedFullScreenContent();
                        Listener?.OnAdShowedFullScreenContent();
                        IsShowingAd = true;
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }

                public override void OnAdDismissedFullScreenContent()
                {
                    try
                    {
                        base.OnAdDismissedFullScreenContent();
                        Listener?.OnAdDismissedFullScreenContent();
                        IsShowingAd = false;
                        Ad = null;
                        AppOpenAdManager.FetchAd();
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }
            }
        }

        #endregion

        #region RewardedInterstitial

        public class AdMobRewardedInterstitial : RewardedInterstitialAdLoadCallback
        {
            private Activity Context;
            private RewardedInterstitialAd Rad;
            public void ShowAd(Activity context)
            {
                try
                {
                    Context = context;

                    AdRequest adRequest = new AdRequest.Builder().Build();

                    // Use an activity context to get the rewarded video instance. 
                    RewardedInterstitialAd.Load(context, AppSettings.AdRewardedInterstitialKey, adRequest, this);
                }
                catch (Exception exception)
                {
                    Methods.DisplayReportResultTrack(exception);
                }
            }

            public override void OnRewardedInterstitialAdFailedToLoad(LoadAdError p0)
            {
                try
                {
                    base.OnRewardedInterstitialAdFailedToLoad(p0);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public override void OnRewardedInterstitialAdLoaded(RewardedInterstitialAd ad)
            {
                try
                {
                    base.OnRewardedInterstitialAdLoaded(ad);
                    Rad = ad;
                    Rad?.Show(Context, new MyUserEarnedRewardListener(Rad));

                    Rad.SetFullScreenContentCallback(new MyFullScreenContentCallback(Rad));
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            private class MyUserEarnedRewardListener : Object, IOnUserEarnedRewardListener
            {
                private RewardedInterstitialAd Rad;
                public MyUserEarnedRewardListener(RewardedInterstitialAd rad)
                {
                    Rad = rad;
                }

                public void OnUserEarnedReward(IRewardItem p0)
                {

                }
            }

            private class MyFullScreenContentCallback : FullScreenContentCallback
            {
                private RewardedInterstitialAd Rad;
                public MyFullScreenContentCallback(RewardedInterstitialAd rad)
                {
                    Rad = rad;
                }

                public override void OnAdFailedToShowFullScreenContent(AdError p0)
                {
                    try
                    {
                        base.OnAdFailedToShowFullScreenContent(p0);
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }

                public override void OnAdShowedFullScreenContent()
                {
                    try
                    {
                        base.OnAdShowedFullScreenContent();
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }

                public override void OnAdDismissedFullScreenContent()
                {
                    try
                    {
                        base.OnAdDismissedFullScreenContent();
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }
            }
        }

        public static AdMobRewardedInterstitial Ad_RewardedInterstitial(Activity context)
        {
            try
            {
                switch (AppSettings.ShowAdMobRewardedInterstitial)
                {
                    case true when CountRewardedInterstitial == AppSettings.ShowAdMobRewardedInterstitialCount:
                    {
                        CountRewardedInterstitial = 0;
                        AdMobRewardedInterstitial ads = new AdMobRewardedInterstitial();
                        ads.ShowAd(context);
                        return ads;
                    }
                    case true:
                        AdsFacebook.InitRewardVideo(context);
                        break;
                    default:
                        AdsFacebook.InitRewardVideo(context);
                        break;
                }

                return null!;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }

        #endregion

        public static class InitializeAdsGoogle
        {
            public static void Initialize(Context context)
            {
                try
                {
                    if (AppSettings.ShowAdMobBanner || AppSettings.ShowAdMobInterstitial || AppSettings.ShowAdMobRewardVideo || AppSettings.ShowAdMobNative || AppSettings.ShowAdMobAppOpen || AppSettings.ShowAdMobRewardedInterstitial)
                        MobileAds.Initialize(context, new MyInitializationCompleteListener());
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            private class MyInitializationCompleteListener : Object, IOnInitializationCompleteListener
            {
                public void OnInitializationComplete(IInitializationStatus p0)
                {

                }
            }
        }
    }
}
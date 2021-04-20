using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.Utils;
using Xamarin.Facebook.Ads;

namespace WoWonder.Helpers.Ads
{
    public static class AdsFacebook
    {
        private static int CountInterstitial;
        private static int CountRewarded;

        #region Banner

        public static AdView InitAdView(Activity activity, LinearLayout adContainer)
        {
            try
            {
                switch (AppSettings.ShowFbBannerAds)
                {
                    case true:
                    {
                        InitializeFacebook.Initialize(activity);

                        AdView adView = new AdView(activity, AppSettings.AdsFbBannerKey, AdSize.BannerHeight50);
                        // Add the ad view to your activity layout
                        adContainer.AddView(adView);

#pragma warning disable 618
                        adView.SetAdListener(new BannerAdListener());
#pragma warning restore 618
                        // Request an ad
                        adView.LoadAd();

                        return adView;
                    }
                    default:
                        return null!;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }

        private class BannerAdListener : Java.Lang.Object, IAdListener
        {

            /// <summary>
            /// Ad clicked callback
            /// </summary>
            /// <param name="ad"></param>
            public void OnAdClicked(IAd ad)
            {

            }

            /// <summary>
            /// Ad loaded callback
            /// </summary>
            /// <param name="ad"></param>
            public void OnAdLoaded(IAd ad)
            {

            }

            /// <summary>
            /// Ad error callback
            /// </summary>
            /// <param name="ad"></param>
            /// <param name="adError"></param>
            public void OnError(IAd ad, AdError adError)
            {
                try
                {
                    var error = adError.ErrorMessage;
                    Console.WriteLine(error);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            /// <summary>
            /// Ad impression logged callback
            /// </summary>
            /// <param name="ad"></param>
            public void OnLoggingImpression(IAd ad)
            {

            }
        }

        #endregion

        #region Interstitial

        public static InterstitialAd InitInterstitial(Activity activity)
        {
            try
            {
                switch (AppSettings.ShowFbInterstitialAds)
                {
                    case true when CountInterstitial == AppSettings.ShowAdMobInterstitialCount:
                    {
                        InitializeFacebook.Initialize(activity);

                        CountInterstitial = 0;
                        var interstitialAd = new InterstitialAd(activity, AppSettings.AdsFbInterstitialKey);

#pragma warning disable 618
                        interstitialAd.SetAdListener(new MyInterstitialAdListener(activity, interstitialAd));
#pragma warning restore 618
                        // Request an ad
                        interstitialAd.LoadAd();

                        return interstitialAd;
                    }
                    case true:
                        CountInterstitial++;
                        break;
                }

                return null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }

        private class MyInterstitialAdListener : Java.Lang.Object, IInterstitialAdListener
        {
            private readonly InterstitialAd InterstitialAd;
            private readonly Activity Activity;
            public MyInterstitialAdListener(Activity activity, InterstitialAd interstitialAd)
            {
                Activity = activity;
                InterstitialAd = interstitialAd;
            }

            /// <summary>
            /// Ad clicked callback
            /// </summary>
            /// <param name="ad"></param>
            public void OnAdClicked(IAd ad)
            {

            }

            /// <summary>
            /// Ad loaded callback
            /// </summary>
            /// <param name="ad"></param>
            public void OnAdLoaded(IAd ad)
            {
                try
                {
                    // Show the ad
                    InterstitialAd?.Show();
                    Console.WriteLine(Activity);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            /// <summary>
            /// Ad error callback
            /// </summary>
            /// <param name="ad"></param>
            /// <param name="adError"></param>
            public void OnError(IAd ad, AdError adError)
            {
                try
                {
                    var error = adError.ErrorMessage;
                    Console.WriteLine(error);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            /// <summary>
            /// Ad impression logged callback
            /// </summary>
            /// <param name="ad"></param>
            public void OnLoggingImpression(IAd ad)
            {

            }

            /// <summary>
            /// Interstitial dismissed callback
            /// </summary>
            /// <param name="ad"></param>
            public void OnInterstitialDismissed(IAd ad)
            {

            }

            /// <summary>
            /// Interstitial ad displayed callback
            /// </summary>
            /// <param name="ad"></param>
            public void OnInterstitialDisplayed(IAd ad)
            {

            }
        }

        #endregion

        #region RewardVideo

        public static RewardedVideoAd InitRewardVideo(Activity activity)
        {
            try
            {
                switch (AppSettings.ShowFbRewardVideoAds)
                {
                    case true when CountRewarded == AppSettings.ShowAdMobRewardedVideoCount:
                    {
                        InitializeFacebook.Initialize(activity);

                        CountRewarded = 0;

                        var rewardVideoAd = new RewardedVideoAd(activity, AppSettings.AdsFbRewardVideoKey);

#pragma warning disable 618
                        rewardVideoAd.SetAdListener(new MyRRewardVideoAdListener(activity, rewardVideoAd));
#pragma warning restore 618
                        rewardVideoAd.LoadAd();
                        //RewardVideoAd.SetRewardData(new RewardData("YOUR_USER_ID", "YOUR_REWARD"));

                        return rewardVideoAd;
                    }
                    case true:
                        CountRewarded++;
                        break;
                }

                return null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }

        private class MyRRewardVideoAdListener : Java.Lang.Object, IS2SRewardedVideoAdListener
        {
            private readonly RewardedVideoAd RewardVideoAd;
            private readonly Activity Activity;
            public MyRRewardVideoAdListener(Activity activity, RewardedVideoAd rewardVideoAd)
            {
                Activity = activity;
                RewardVideoAd = rewardVideoAd;
                Console.WriteLine(Activity);
            }

            /// <summary>
            /// Ad clicked callback
            /// </summary>
            /// <param name="ad"></param>
            public void OnAdClicked(IAd ad)
            {

            }

            /// <summary>
            /// Ad loaded callback
            /// </summary>
            /// <param name="ad"></param>
            public void OnAdLoaded(IAd ad)
            {
                try
                {
                    // Show the ad
                    RewardVideoAd?.Show();
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            /// <summary>
            /// Ad error callback
            /// </summary>
            /// <param name="ad"></param>
            /// <param name="adError"></param>
            public void OnError(IAd ad, AdError adError)
            {
                try
                {
                    var error = adError.ErrorMessage;
                    Console.WriteLine(error);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            /// <summary>
            /// Ad impression logged callback
            /// </summary>
            /// <param name="ad"></param>
            public void OnLoggingImpression(IAd ad)
            {

            }

            /// <summary>
            /// Rewarded Video Closed
            /// </summary>
            public void OnRewardedVideoClosed()
            {

            }

            /// <summary>
            /// Rewarded Video View Complete
            /// </summary>
            public void OnRewardedVideoCompleted()
            {

            }

            /// <summary>
            /// Reward Video Server Failed
            /// </summary>
            public void OnRewardServerFailed()
            {

            }

            /// <summary>
            /// Reward Video Server Succeeded
            /// </summary>
            public void OnRewardServerSuccess()
            {

            }
        }

        #endregion

        #region Native

        public static void InitNative(Activity activity, LinearLayout nativeAdLayout, NativeAd ad)
        {
            try
            {
                switch (AppSettings.ShowFbNativeAds)
                {
                    case true:
                    {
                        InitializeFacebook.Initialize(activity);

                        switch (ad)
                        {
                            case null:
                            {
                                var nativeAd = new NativeAd(activity, AppSettings.AdsFbNativeKey);
#pragma warning disable 618
                                nativeAd.SetAdListener(new NativeAdListener(activity, nativeAd, nativeAdLayout));
#pragma warning restore 618

                                // Initiate a request to load an ad.
                                nativeAd.LoadAd();
                                break;
                            }
                            default:
                            {
                                var holder = new AdHolder(nativeAdLayout);
                                LoadAd(activity, holder, ad, nativeAdLayout);
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

        private class NativeAdListener : Java.Lang.Object, INativeAdListener
        {
            private readonly NativeAd NativeAd;
            private readonly LinearLayout NativeAdLayout;
            private readonly Activity Activity;
            public NativeAdListener(Activity activity, NativeAd nativeAd, LinearLayout nativeAdLayout)
            {
                Activity = activity;
                NativeAd = nativeAd;
                NativeAdLayout = nativeAdLayout;
            }

            /// <summary>
            /// Ad clicked callback
            /// </summary>
            /// <param name="ad"></param>
            public void OnAdClicked(IAd ad)
            {

            }

            /// <summary>
            /// Ad loaded callback
            /// </summary>
            /// <param name="ad"></param>
            public void OnAdLoaded(IAd ad)
            {
                try
                {
                    if (NativeAd == null || NativeAd != ad)
                    {
                        // Race condition, load() called again before last ad was displayed
                        return;
                    }

                    switch (NativeAdLayout)
                    {
                        case null:
                            return;
                        default:
                            Activity?.RunOnUiThread(() =>
                            {
                                try
                                {
                                    NativeAdLayout.Visibility = ViewStates.Visible;

                                    // Unregister last ad
                                    NativeAd.UnregisterView();

                                    //if (NativeAdChoicesContainer != null)
                                    //{
                                    //    //var adOptionsView = new NativeAdView(Activity, NativeAd, NativeAdLayout);
                                    //    NativeAdChoicesContainer.RemoveAllViews();
                                    //    NativeAdChoicesContainer.AddView(NativeAdLayout, 0);
                                    //}

                                    InflateAd(NativeAd, NativeAdLayout);
                                }
                                catch (Exception e)
                                {
                                    Methods.DisplayReportResultTrack(e);
                                }
                            });
                            break;
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            /// <summary>
            /// Ad error callback
            /// </summary>
            /// <param name="ad"></param>
            /// <param name="adError"></param>
            public void OnError(IAd ad, AdError adError)
            {
                try
                {
                    var error = adError.ErrorMessage;
                    Console.WriteLine(error);

                    if (NativeAdLayout != null)
                        NativeAdLayout.Visibility = ViewStates.Gone;
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            /// <summary>
            /// Ad impression logged callback
            /// </summary>
            /// <param name="ad"></param>
            public void OnLoggingImpression(IAd ad)
            {

            }

            /// <summary>
            /// on Media Downloaded
            /// </summary>
            /// <param name="p0"></param>
            public void OnMediaDownloaded(IAd p0)
            {

            }

            //private LinearLayout NativeAdChoicesContainer;

            private void InflateAd(NativeAd nativeAd, View adView)
            {
                try
                {
                    // Create native UI using the ad metadata.
                    var holder = new AdHolder(adView);
                    //NativeAdChoicesContainer = holder.NativeAdChoicesContainer;

                    // Setting the Text
                    holder.NativeAdSocialContext.Text = nativeAd.AdSocialContext;
                    holder.NativeAdCallToAction.Text = nativeAd.AdCallToAction;
                    holder.NativeAdCallToAction.Visibility = nativeAd.HasCallToAction ? ViewStates.Visible : ViewStates.Invisible;
                    holder.NativeAdTitle.Text = nativeAd.AdvertiserName;
                    holder.NativeAdBody.Text = nativeAd.AdBodyText;
                    holder.SponsoredLabel.Text = Activity.GetText(Resource.String.sponsored);

                    // You can use the following to specify the clickable areas.
                    List<View> clickableViews = new List<View> { holder.NativeAdIcon, holder.NativeAdMedia, holder.NativeAdCallToAction };

                    nativeAd.RegisterViewForInteraction(NativeAdLayout, holder.NativeAdMedia, holder.NativeAdIcon, clickableViews);

                    // Optional: tag views
                    NativeAdBase.NativeComponentTag.TagView(holder.NativeAdIcon, NativeAdBase.NativeComponentTag.AdIcon);
                    NativeAdBase.NativeComponentTag.TagView(holder.NativeAdTitle, NativeAdBase.NativeComponentTag.AdTitle);
                    NativeAdBase.NativeComponentTag.TagView(holder.NativeAdBody, NativeAdBase.NativeComponentTag.AdBody);
                    NativeAdBase.NativeComponentTag.TagView(holder.NativeAdSocialContext, NativeAdBase.NativeComponentTag.AdSocialContext);
                    NativeAdBase.NativeComponentTag.TagView(holder.NativeAdCallToAction, NativeAdBase.NativeComponentTag.AdCallToAction);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public class MediaViewListener : Java.Lang.Object, IMediaViewListener
            {
                public void OnComplete(MediaView p0)
                {

                }

                public void OnEnterFullscreen(MediaView p0)
                {

                }

                public void OnExitFullscreen(MediaView p0)
                {

                }

                public void OnFullscreenBackground(MediaView p0)
                {

                }

                public void OnFullscreenForeground(MediaView p0)
                {

                }

                public void OnPause(MediaView p0)
                {

                }

                public void OnPlay(MediaView p0)
                {

                }

                public void OnVolumeChange(MediaView p0, float p1)
                {

                }
            }
        }

        private static void LoadAd(Activity activity, AdHolder holder, NativeAd nativeAd, LinearLayout adView)
        {
            try
            {
                adView.Visibility = ViewStates.Visible;

                //if (holder.NativeAdChoicesContainer != null)
                //{
                //    //var adOptionsView = new AdOptionsView(activity, nativeAd, adView);
                //    //holder.NativeAdChoicesContainer.RemoveAllViews();
                //    //holder.NativeAdChoicesContainer.AddView(adView, 0);
                //}

                // Setting the Text
                holder.NativeAdSocialContext.Text = nativeAd.AdSocialContext;
                holder.NativeAdCallToAction.Text = nativeAd.AdCallToAction;
                holder.NativeAdCallToAction.Visibility = nativeAd.HasCallToAction ? ViewStates.Visible : ViewStates.Invisible;
                holder.NativeAdTitle.Text = nativeAd.AdvertiserName;
                holder.NativeAdBody.Text = nativeAd.AdBodyText;
                holder.SponsoredLabel.Text = activity.GetText(Resource.String.sponsored);

                // You can use the following to specify the clickable areas.
                List<View> clickableViews = new List<View> { holder.NativeAdIcon, holder.NativeAdMedia, holder.NativeAdCallToAction };

                nativeAd.RegisterViewForInteraction(adView, holder.NativeAdMedia, holder.NativeAdIcon, clickableViews);

                // Optional: tag views
                NativeAdBase.NativeComponentTag.TagView(holder.NativeAdIcon, NativeAdBase.NativeComponentTag.AdIcon);
                NativeAdBase.NativeComponentTag.TagView(holder.NativeAdTitle, NativeAdBase.NativeComponentTag.AdTitle);
                NativeAdBase.NativeComponentTag.TagView(holder.NativeAdBody, NativeAdBase.NativeComponentTag.AdBody);
                NativeAdBase.NativeComponentTag.TagView(holder.NativeAdSocialContext, NativeAdBase.NativeComponentTag.AdSocialContext);
                NativeAdBase.NativeComponentTag.TagView(holder.NativeAdCallToAction, NativeAdBase.NativeComponentTag.AdCallToAction);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private class AdHolder : RecyclerView.ViewHolder
        {
            public View NativeAdLayout { get; private set; }
            public MediaView NativeAdMedia { get; private set; }
            public MediaView NativeAdIcon { get; private set; }
            public TextView NativeAdTitle { get; private set; }
            public TextView NativeAdBody { get; private set; }
            public TextView NativeAdSocialContext { get; private set; }
            public TextView SponsoredLabel { get; private set; }
            public Button NativeAdCallToAction { get; private set; }
            public LinearLayout NativeAdChoicesContainer { get; private set; }

            public AdHolder(View adLayout) : base(adLayout)
            {
                try
                {
                    NativeAdLayout = adLayout;

                    NativeAdMedia = adLayout.FindViewById<MediaView>(Resource.Id.native_ad_media);
                    NativeAdTitle = adLayout.FindViewById<TextView>(Resource.Id.native_ad_title);
                    NativeAdBody = adLayout.FindViewById<TextView>(Resource.Id.native_ad_body);
                    NativeAdSocialContext = adLayout.FindViewById<TextView>(Resource.Id.native_ad_social_context);
                    SponsoredLabel = adLayout.FindViewById<TextView>(Resource.Id.native_ad_sponsored_label);
                    NativeAdCallToAction = adLayout.FindViewById<Button>(Resource.Id.native_ad_call_to_action);
                    NativeAdIcon = adLayout.FindViewById<MediaView>(Resource.Id.native_ad_icon);
                    NativeAdChoicesContainer = adLayout.FindViewById<LinearLayout>(Resource.Id.ad_choices_container);

                    NativeAdMedia?.SetListener(new NativeAdListener.MediaViewListener());
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }
        }

        #endregion
    }

    public class MyNativeAdsManagerListener : Java.Lang.Object, NativeAdsManager.IListener
    {
        private readonly NativePostAdapter NativePostAdapter2;
        public MyNativeAdsManagerListener(NativePostAdapter nativePostAdapter)
        {
            try
            {
                NativePostAdapter2 = nativePostAdapter;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnAdError(AdError p0)
        {

        }

        public void OnAdsLoaded()
        {
            try
            {
                NativeAd ad = NativePostAdapter2?.MNativeAdsManager?.NextNativeAd();
                if (ad != null)
                {
                    NativePostAdapter2.MAdItems.Add(ad);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }

    public static class InitializeFacebook
    {
        public static void Initialize(Context context)
        {
            try
            {
                MapsInitializer.Initialize(context);
                AudienceNetworkAds.Initialize(context);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }

}
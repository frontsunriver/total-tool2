using System;
using System.Collections.Generic;
using System.Linq;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Com.Google.Ads.Interactivemedia.V3.Api;
using Com.Google.Android.Exoplayer2;
using Com.Google.Android.Exoplayer2.Drm;
using Com.Google.Android.Exoplayer2.Ext.Ima;
using Com.Google.Android.Exoplayer2.Extractor.TS;
using Com.Google.Android.Exoplayer2.Source;
using Com.Google.Android.Exoplayer2.Source.Ads;
using Com.Google.Android.Exoplayer2.Source.Dash;
using Com.Google.Android.Exoplayer2.Source.Hls;
using Com.Google.Android.Exoplayer2.Source.Smoothstreaming;
using Com.Google.Android.Exoplayer2.Trackselection;
using Com.Google.Android.Exoplayer2.UI;
using Com.Google.Android.Exoplayer2.Upstream;
using Com.Google.Android.Exoplayer2.Util;
using Java.Lang;
using WoWonder.Library.Anjo.Share;
using WoWonder.Library.Anjo.Share.Abstractions;
using WoWonder.Activities.Tabbes;
using WoWonder.Activities.Videos;
using WoWonder.Helpers.Utils;
using WoWonder.MediaPlayers;
using WoWonderClient.Classes.Movies;
using Exception = System.Exception;
using Uri = Android.Net.Uri;
 
namespace WoWonder.Helpers.Controller
{
    public class VideoController : Java.Lang.Object, View.IOnClickListener, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private Activity ActivityContext { get; set; }
        private string ActivityName { get; set; }

        public IPlayer Factory;
        private static SimpleExoPlayer Player { get; set; }

        private ImaAdsLoader ImaAdsLoader;
        private PlayerEvents PlayerListener;
        private static PlayerView FullscreenPlayerView;

        public PlayerView SimpleExoPlayerView;
        private FrameLayout MainVideoFrameLayout , MFullScreenButton;
        private PlayerControlView ControlView;

        private ImageView MFullScreenIcon;

        private static IMediaSource VideoSource;


        private readonly int ResumeWindow = 0;
        private readonly long ResumePosition = 0;


        public GetMoviesObject.Movie VideoData;


        #endregion

        public VideoController(Activity activity, string activtyName)
        {
            try
            {
                ActivityName = activtyName;
                ActivityContext = activity;

                Initialize();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void Initialize()
        {
            try
            {
                PlayerListener = new PlayerEvents(ActivityContext, ControlView);

                if (ActivityName != "FullScreen")
                {
                    SimpleExoPlayerView = ActivityContext.FindViewById<PlayerView>(Resource.Id.player_view);
                    SimpleExoPlayerView.SetControllerVisibilityListener(PlayerListener);
                    SimpleExoPlayerView.RequestFocus();

                    //Player initialize
                    ControlView = SimpleExoPlayerView.FindViewById<PlayerControlView>(Resource.Id.exo_controller);
                    PlayerListener = new PlayerEvents(ActivityContext, ControlView);

                    MFullScreenIcon = ControlView.FindViewById<ImageView>(Resource.Id.exo_fullscreen_icon);
                    MFullScreenButton = ControlView.FindViewById<FrameLayout>(Resource.Id.exo_fullscreen_button);

                    MainVideoFrameLayout = ActivityContext.FindViewById<FrameLayout>(Resource.Id.root);
                    MainVideoFrameLayout.SetOnClickListener(this);
                      
                    switch (MFullScreenButton.HasOnClickListeners)
                    {
                        case false:
                            MFullScreenButton.SetOnClickListener(this);
                            break;
                    } 
                }
                else
                {
                    FullscreenPlayerView = ActivityContext.FindViewById<PlayerView>(Resource.Id.player_view2);
                    ControlView = FullscreenPlayerView.FindViewById<PlayerControlView>(Resource.Id.exo_controller);
                    PlayerListener = new PlayerEvents(ActivityContext, ControlView);
                   
                    MFullScreenIcon = ControlView.FindViewById<ImageView>(Resource.Id.exo_fullscreen_icon);
                    MFullScreenButton = ControlView.FindViewById<FrameLayout>(Resource.Id.exo_fullscreen_button);

                    switch (MFullScreenButton.HasOnClickListeners)
                    {
                        case false:
                            MFullScreenButton.SetOnClickListener(this);
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        public void PlayVideo(string videoUrL, GetMoviesObject.Movie videoObject)
        {
            try
            {
                if (videoObject != null)
                {
                    VideoData = videoObject;

                    VideoViewerActivity.GetInstance()?.LoadVideo_Data(videoObject);

                    TabbedMainActivity.GetInstance()?.SetOnWakeLock();

                    ReleaseVideo();

                    MFullScreenIcon.SetImageDrawable(ActivityContext.GetDrawable(Resource.Drawable.ic_action_ic_fullscreen_expand));

                    Uri videoUrl = Uri.Parse(!string.IsNullOrEmpty(videoUrL) ? videoUrL : VideoData.Source);

                    AdaptiveTrackSelection.Factory trackSelectionFactory = new AdaptiveTrackSelection.Factory();
                    var trackSelector = new DefaultTrackSelector(DefaultTrackSelector.Parameters.GetDefaults(ActivityContext), trackSelectionFactory);
                    trackSelector.SetParameters(new DefaultTrackSelector.ParametersBuilder(ActivityContext));

                    Player = new SimpleExoPlayer.Builder(ActivityContext).Build();

                    // Produces DataSource instances through which media data is loaded.
                    var defaultSource = GetMediaSourceFromUrl(videoUrl, videoUrl?.Path?.Split('.').Last(), "normal");

                    VideoSource = PlayerSettings.ShowInteractiveMediaAds switch
                    {
                        //Set Interactive Media Ads 
                        true => CreateMediaSourceWithAds(defaultSource, PlayerSettings.ImAdsUri),
                        _ => null!
                    };

                    switch (SimpleExoPlayerView)
                    {
                        case null:
                            Initialize();
                            break;
                    }

                    switch (PlayerSettings.EnableOfflineMode)
                    {
                        //Set Cache Media Load
                        case true:
                        {
                            VideoSource = VideoSource == null? CreateCacheMediaSource(defaultSource, videoUrl): CreateCacheMediaSource(VideoSource, videoUrl);
                            if (VideoSource != null)
                            {
                                SimpleExoPlayerView.Player = Player;
                                Player.Prepare(VideoSource);
                                    //Player.AddListener(PlayerListener);

                                    Player.PlayWhenReady = true;

                                bool haveResumePosition = ResumeWindow != C.IndexUnset;
                                switch (haveResumePosition)
                                {
                                    case true:
                                        Player.SeekTo(ResumeWindow, ResumePosition);
                                        break;
                                }

                                return;
                            }

                            break;
                        }
                    }

                    switch (VideoSource)
                    {
                        case null:
                        {
                            switch (string.IsNullOrEmpty(videoUrL))
                            {
                                case false when videoUrL.Contains("youtube") || videoUrL.Contains("Youtube") || videoUrL.Contains("youtu"):
                                    //Task.Factory.StartNew(async () =>
                                    //{
                                    //    var newurl = await VideoInfoRetriever.GetEmbededVideo(VideoData.source);
                                    //    videoSource = CreateDefaultMediaSource(Android.Net.Uri.Parse(newurl));
                                    //});
                                    break;
                                case false:
                                {
                                    VideoSource = GetMediaSourceFromUrl(Uri.Parse(videoUrL), videoUrL?.Split('.').Last(), "normal");

                                    SimpleExoPlayerView.Player = Player;
                                    Player.Prepare(VideoSource);

                                            //Player.AddListener(PlayerListener);
                                            Player.PlayWhenReady = true;

                                    bool haveResumePosition = ResumeWindow != C.IndexUnset;
                                    switch (haveResumePosition)
                                    {
                                        case true:
                                            Player.SeekTo(ResumeWindow, ResumePosition);
                                            break;
                                    }
                                    break;
                                }
                            }

                            break;
                        }
                        default:
                        {
                            SimpleExoPlayerView.Player = Player;
                            Player.Prepare(VideoSource);

                                //Player.AddListener(PlayerListener);
                                Player.PlayWhenReady = true;

                            bool haveResumePosition = ResumeWindow != C.IndexUnset;
                            switch (haveResumePosition)
                            {
                                case true:
                                    Player.SeekTo(ResumeWindow, ResumePosition);
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void ReleaseVideo()
        {
            try
            {
                if (Player != null)
                {
                    SetStopVideo();

                    Player?.Release();
                    Player = null!;

                    //GC Collector
                    GC.Collect();
                }

                TabbedMainActivity.GetInstance()?.SetOffWakeLock();

                //var tab = VideoViewerActivity.GetInstance()?.TabVideosAbout;
                //if (tab != null)
                //{
                //    tab.VideoDescriptionLayout.Visibility = ViewStates.Visible;
                //    tab.VideoTittle.Text = VideoData?.Name;
                //}

                SimpleExoPlayerView.Player = null!;
                ReleaseAdsLoader();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void SetStopVideo()
        {
            try
            {
                if (SimpleExoPlayerView.Player != null)
                {
                    SimpleExoPlayerView.Player.PlayWhenReady = SimpleExoPlayerView.Player.PlaybackState switch
                    {
                        IPlayer.StateReady => false,
                        _ => SimpleExoPlayerView.Player.PlayWhenReady
                    };

                    //GC Collector
                    GC.Collect();
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        #region Video player

        private IMediaSource CreateCacheMediaSource(IMediaSource videoSource, Uri videoUrL)
        {
            try
            {
                switch (PlayerSettings.EnableOfflineMode)
                {
                    case true:
                        videoSource = GetMediaSourceFromUrl(videoUrL, videoUrL?.Path?.Split('.').Last(), "normal");
                        return videoSource;
                    default:
                        return null!;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }

        private IMediaSource CreateMediaSourceWithAds(IMediaSource videoSource, Uri imAdsUri)
        {
            try
            {
                // Player = ExoPlayerFactory.NewSimpleInstance(ActivityContext);
                SimpleExoPlayerView.Player = Player;

                switch (ImaAdsLoader)
                {
                    case null:
                    {
                        Player ??= new SimpleExoPlayer.Builder(ActivityContext).Build();
                        SimpleExoPlayerView.Player = Player;

                        switch (ImaAdsLoader)
                        {
                            case null:
                            {
                                var imaSdkSettings = ImaSdkFactory.Instance.CreateImaSdkSettings();
                                imaSdkSettings.AutoPlayAdBreaks = true;
                                imaSdkSettings.DebugMode = true;

                                ImaAdsLoader = new ImaAdsLoader.Builder(ActivityContext)
                                    .SetImaSdkSettings(imaSdkSettings)
                                    .SetMediaLoadTimeoutMs(30 * 1000)
                                    .SetVastLoadTimeoutMs(30 * 1000)
                                    .BuildForAdTag(imAdsUri); // here is url for vast xml file

                                IMediaSourceFactory adMediaSourceFactory = new MediaSourceFactoryAnonymousInnerClass(this);

                                IMediaSource mediaSourceWithAds = new AdsMediaSource(videoSource, adMediaSourceFactory, ImaAdsLoader, SimpleExoPlayerView);
                                return mediaSourceWithAds;
                            }
                        }

                        break;
                    }
                }
            }
            catch (ClassNotFoundException e)
            {
                Console.WriteLine(e.Message);
                // IMA extension not loaded.
                return null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
            return null!;
        }

        private class MediaSourceFactoryAnonymousInnerClass : Java.Lang.Object, IMediaSourceFactory
        {
            private readonly VideoController OuterInstance;
            private IDrmSessionManager DrmSessionManager = null;

            public MediaSourceFactoryAnonymousInnerClass(VideoController outerInstance)
            {
                OuterInstance = outerInstance;
                DrmSessionManager = IDrmSessionManager.DummyDrmSessionManager;
            }

            public IMediaSource CreateMediaSource(Uri uri)
            {
                return OuterInstance.GetMediaSourceFromUrl(uri, uri?.Path?.Split('.').Last(), "ads");
            }

            public int[] GetSupportedTypes()
            {
                return new[] { C.TypeDash, C.TypeSs, C.TypeHls, C.TypeOther };
            }

            public IMediaSourceFactory SetDrmSessionManager(IDrmSessionManager drmSessionManager)
            {
                DrmSessionManager = drmSessionManager ?? IDrmSessionManager.DummyDrmSessionManager;
                return this;
            }
        }

        private IMediaSource GetMediaSourceFromUrl(Uri uri, string extension, string tag)
        {
            var BandwidthMeter = DefaultBandwidthMeter.GetSingletonInstance(ActivityContext);
            //DefaultDataSourceFactory dataSourceFactory = new DefaultDataSourceFactory(ActivityContext, Util.GetUserAgent(ActivityContext, AppSettings.ApplicationName), mBandwidthMeter);
            var buildHttpDataSourceFactory = new DefaultDataSourceFactory(ActivityContext, BandwidthMeter, new DefaultHttpDataSourceFactory(Util.GetUserAgent(ActivityContext, AppSettings.ApplicationName)));
            var buildHttpDataSourceFactoryNull = new DefaultDataSourceFactory(ActivityContext, BandwidthMeter, new DefaultHttpDataSourceFactory(Util.GetUserAgent(ActivityContext, AppSettings.ApplicationName)));
            int type = Util.InferContentType(uri, extension);
            try
            {

                IMediaSource src;
                switch (type)
                {
                    case C.TypeSs:
                        src = new SsMediaSource.Factory(new DefaultSsChunkSource.Factory(buildHttpDataSourceFactory), buildHttpDataSourceFactoryNull).SetTag(tag).SetDrmSessionManager(IDrmSessionManager.DummyDrmSessionManager).CreateMediaSource(uri);
                        break;
                    case C.TypeDash:
                        src = new DashMediaSource.Factory(new DefaultDashChunkSource.Factory(buildHttpDataSourceFactory), buildHttpDataSourceFactoryNull).SetTag(tag).SetDrmSessionManager(IDrmSessionManager.DummyDrmSessionManager).CreateMediaSource(uri);
                        break;
                    case C.TypeHls:
                        DefaultHlsExtractorFactory defaultHlsExtractorFactory = new DefaultHlsExtractorFactory(DefaultTsPayloadReaderFactory.FlagAllowNonIdrKeyframes, true);
                        src = new HlsMediaSource.Factory(buildHttpDataSourceFactory).SetTag(tag).SetExtractorFactory(defaultHlsExtractorFactory).CreateMediaSource(uri);
                        break;
                    case C.TypeOther:
                    default:
                        src = new ProgressiveMediaSource.Factory(buildHttpDataSourceFactory).SetTag(tag).CreateMediaSource(uri);
                        break;
                }

                return src;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                try
                {
                    return new ProgressiveMediaSource.Factory(buildHttpDataSourceFactory).SetTag(tag).CreateMediaSource(uri);
                }
                catch (Exception exception)
                {
                    Methods.DisplayReportResultTrack(exception);
                    return null!;
                }
            }
        }

        public void OnClick(View v)
        {
            try
            {
                if (v.Id == MFullScreenIcon.Id || v.Id == MFullScreenButton.Id)
                {
                    InitFullscreenDialog();
                }

            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void ReleaseAdsLoader()
        {
            try
            {
                if (ImaAdsLoader != null)
                {
                    ImaAdsLoader.Release();
                    ImaAdsLoader = null!;
                    SimpleExoPlayerView.OverlayFrameLayout.RemoveAllViews();
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        public void RestartPlayAfterShrinkScreen()
        {
            try
            {
                SimpleExoPlayerView.Player = null!;
                SimpleExoPlayerView.Player = Player;
                SimpleExoPlayerView.Player.PlayWhenReady = true;
                MFullScreenIcon.SetImageDrawable(ActivityContext.GetDrawable(Resource.Drawable.ic_action_ic_fullscreen_expand));
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void PlayFullScreen()
        {
            try
            {
                if (FullscreenPlayerView != null)
                {
                    //Player.AddListener(PlayerListener);
                    FullscreenPlayerView.Player = Player;
                    if (FullscreenPlayerView.Player != null) FullscreenPlayerView.Player.PlayWhenReady = true;
                    MFullScreenIcon.SetImageDrawable(ActivityContext.GetDrawable(Resource.Drawable.ic_action_ic_fullscreen_skrink));
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Event 
         
        //Full Screen
        public void InitFullscreenDialog(string action = "Open")
        {
            try
            {
                if (ActivityName != "FullScreen" && action == "Open")
                {
                    Intent intent = new Intent(ActivityContext, typeof(FullScreenVideoActivity));
                    //intent.PutExtra("Downloaded", DownloadIcon?.Tag?.ToString());
                    intent.PutExtra("Type", "Movies");
                    ActivityContext.StartActivityForResult(intent, 2000);
                }
                else
                {
                    Intent intent = new Intent();
                    ActivityContext.SetResult(Result.Ok, intent);
                    ActivityContext.Finish();
                }

            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Menu More >>  
        public void MoreButton_OnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(ActivityContext).Theme(AppSettings.SetTabDarkTheme ? Theme.Dark : Theme.Light);

                arrayAdapter.Add(ActivityContext.GetString(Resource.String.Lbl_CopeLink));
                arrayAdapter.Add(ActivityContext.GetString(Resource.String.Lbl_Share));

                dialogList.Title(ActivityContext.GetString(Resource.String.Lbl_More)).TitleColorRes(Resource.Color.primary);
                dialogList.Items(arrayAdapter);
                dialogList.NegativeText(ActivityContext.GetText(Resource.String.Lbl_Close)).OnNegative(this);
                dialogList.AlwaysCallSingleChoiceCallback();
                dialogList.ItemsCallback(this).Build().Show();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Event Menu >> Share
        public async void OnMenu_ShareIcon_Click(GetMoviesObject.Movie video)
        {
            try
            {
                switch (CrossShare.IsSupported)
                {
                    //Share Plugin same as flame
                    case false:
                        return;
                    default:
                        await CrossShare.Current.Share(new ShareMessage
                        {
                            Title = video.Name,
                            Text = video.Description,
                            Url = video.Url
                        });
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Event Menu >> Cope Link
        private void OnMenu_CopeLink_Click(GetMoviesObject.Movie video)
        {
            try
            { 
                Methods.CopyToClipboard(ActivityContext, video.Url); 
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Share
        public void ShareIcon_Click(object sender, EventArgs e)
        {
            try
            {
                OnMenu_ShareIcon_Click(VideoData);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        #endregion

        #region MaterialDialog

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                string text = itemString.ToString();
                if (text == ActivityContext.GetString(Resource.String.Lbl_CopeLink))
                {
                    OnMenu_CopeLink_Click(VideoData);
                }
                else if (text == ActivityContext.GetString(Resource.String.Lbl_Share))
                {
                    OnMenu_ShareIcon_Click(VideoData);
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

        #endregion
    }
}
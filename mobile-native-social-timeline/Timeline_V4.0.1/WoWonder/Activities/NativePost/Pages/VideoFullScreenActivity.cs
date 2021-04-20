using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Google.Android.Exoplayer2;
using Com.Google.Android.Exoplayer2.Drm;
using Com.Google.Android.Exoplayer2.Extractor.TS;
using Com.Google.Android.Exoplayer2.Source;
using Com.Google.Android.Exoplayer2.Source.Dash;
using Com.Google.Android.Exoplayer2.Source.Hls;
using Com.Google.Android.Exoplayer2.Source.Smoothstreaming;
using Com.Google.Android.Exoplayer2.Trackselection;
using Com.Google.Android.Exoplayer2.UI;
using Com.Google.Android.Exoplayer2.Upstream;
using Com.Google.Android.Exoplayer2.Util;
using WoWonder.Activities.Base;
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.Utils;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.NativePost.Pages
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", LaunchMode = LaunchMode.SingleInstance, ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.Locale | ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenLayout | ConfigChanges.ScreenSize | ConfigChanges.SmallestScreenSize | ConfigChanges.UiMode)]
    public class VideoFullScreenActivity : BaseActivity, IPlayerEventListener
    {
        #region Variables Basic

        private ProgressBar ProgressBar; 
        private string VideoUrl;
         
        private PlayerView VideoSurfaceView;
        private ImageView BtFullScreen;
        private SimpleExoPlayer VideoPlayer;
        private DefaultDataSourceFactory DefaultDataSourceFac;

        #endregion

        #region General

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetTheme(AppSettings.SetTabDarkTheme ? Resource.Style.MyTheme_Dark_Base : Resource.Style.MyTheme_Base);

                // Create your application here
                //Set Full screen 
                Methods.App.FullScreenApp(this, true);

                //newUiOptions |= (int)SystemUiFlags.LowProfile;
                //newUiOptions |= (int)SystemUiFlags.Immersive;

                //ScreenOrientation.Portrait >>  Make to run your application only in portrait mode
                //ScreenOrientation.Landscape >> Make to run your application only in LANDSCAPE mode 
                //RequestedOrientation = ScreenOrientation.Landscape;

                SetContentView(Resource.Layout.VideoFullScreenLayout);

                VideoUrl = Intent?.GetStringExtra("videoUrl") ?? "";
                //var VideoDuration = Intent?.GetStringExtra("videoDuration") ?? "";

                //Get Value And Set Toolbar
                InitComponent(); 
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
         
        #region Functions

        private void InitComponent()
        {
            try
            { 
                VideoSurfaceView = FindViewById<PlayerView>(Resource.Id.pv_fullscreen);
                BtFullScreen = FindViewById<ImageView>(Resource.Id.exo_fullscreen_icon);

                SetPlayer();
                 
                BtFullScreen.Click += BtFullScreen_Click;

                ///////////////////////////

                ProgressBar = FindViewById<ProgressBar>(Resource.Id.progress_bar);
                ProgressBar.Visibility = ViewStates.Visible;
 
                //===================== Exo Player ======================== 

                // Uri
                Uri uri = Uri.Parse(VideoUrl);
                var videoSource = GetMediaSourceFromUrl(uri, uri?.Path?.Split('.').Last(), "normal");

                VideoPlayer.Prepare(videoSource);
                VideoPlayer.PlayWhenReady = true;

                TabbedMainActivity.GetInstance()?.SetOnWakeLock();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }


        private void SetPlayer()
        {
            try
            {
                var BandwidthMeter = DefaultBandwidthMeter.GetSingletonInstance(this);

                DefaultTrackSelector trackSelector = new DefaultTrackSelector(this);
                trackSelector.SetParameters(new DefaultTrackSelector.ParametersBuilder(this));

                VideoPlayer = new SimpleExoPlayer.Builder(this).SetTrackSelector(trackSelector).Build();

                DefaultDataSourceFac = new DefaultDataSourceFactory(this, Util.GetUserAgent(this, AppSettings.ApplicationName), BandwidthMeter);
                VideoSurfaceView.UseController = true;
                VideoSurfaceView.Player = VideoPlayer;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private IMediaSource GetMediaSourceFromUrl(Uri uri, string extension, string tag)
        {
            var BandwidthMeter = DefaultBandwidthMeter.GetSingletonInstance(this);
            //DefaultDataSourceFactory dataSourceFactory = new DefaultDataSourceFactory(ActivityContext, Util.GetUserAgent(this, AppSettings.ApplicationName), mBandwidthMeter);
            var buildHttpDataSourceFactory = new DefaultDataSourceFactory(this, BandwidthMeter, new DefaultHttpDataSourceFactory(Util.GetUserAgent(this, AppSettings.ApplicationName)));
            var buildHttpDataSourceFactoryNull = new DefaultDataSourceFactory(this, BandwidthMeter, new DefaultHttpDataSourceFactory(Util.GetUserAgent(this, AppSettings.ApplicationName)));
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
         

        private void BtFullScreen_Click(object sender, EventArgs e)
        {
            Finish();
        }

        public void OnLoadingChanged(bool p0)
        {
        }

        public void OnPlaybackParametersChanged(PlaybackParameters p0)
        {
        }

        public void OnPlayerError(ExoPlaybackException p0)
        {
        }

        public void OnPlayerStateChanged(bool playWhenReady, int playbackState)
        {
            try
            {
                switch (playbackState)
                {
                    case IPlayer.StateBuffering:
                        ProgressBar.Visibility = ViewStates.Visible;
                        break;
                    case IPlayer.StateReady:
                        ProgressBar.Visibility = ViewStates.Gone;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnIsPlayingChanged(bool p0)
        {

        }

        public void OnPositionDiscontinuity(int p0)
        {
        }

        public void OnRepeatModeChanged(int p0)
        {
        }

        public void OnSeekProcessed()
        {
        }

        public void OnShuffleModeEnabledChanged(bool p0)
        {
        }

        public void OnTimelineChanged(Timeline p0, int p2)
        {
        }

        public void OnTracksChanged(TrackGroupArray p0, TrackSelectionArray p1)
        {
        }

        //private void PostVideoViewOnPrepared(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        PostVideoView.Start();
        //        ProgressBar.Visibility = ViewStates.Invisible;
        //    }
        //    catch (Exception exception)
        //    {
        //        Methods.DisplayReportResultTrack(exception);
        //    }
        //}

        //private void PostVideoViewOnCompletion(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        PostVideoView.Pause();
        //        OnBackPressed();
        //    }
        //    catch (Exception exception)
        //    {
        //        Methods.DisplayReportResultTrack(exception);
        //    }
        //}

        #endregion
         
        public override void OnBackPressed()
        {
            try
            {
               // PostVideoView?.StopPlayback();
                //PostVideoView = null!;

                TabbedMainActivity.GetInstance()?.SetOffWakeLock();

                base.OnBackPressed();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                base.OnBackPressed();
            }
        }

        protected override void OnPause()
        {
            try
            {
                base.OnPause();

                VideoPlayer.PlayWhenReady = false;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        protected override void OnRestart()
        {
            try
            {
                base.OnRestart();

                VideoPlayer.PlayWhenReady = true;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }
    }
}
using System;
using Android.App;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using DT.Xamarin.Agora;
using DT.Xamarin.Agora.Video;
using WoWonder.Activities.Live.Rtc;
using WoWonder.Activities.Live.Stats;
using WoWonder.Helpers.Utils;

namespace WoWonder.Activities.Live.Page
{
    [Activity]
    public class RtcBaseActivity : AppCompatActivity, IEventHandler 
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                Window?.SetSoftInputMode(SoftInput.AdjustResize);

                Methods.App.FullScreenApp(this , true);

                RegisterRtcEventHandler(this);
                ConfigVideo();
                //JoinChannel(); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void ConfigVideo()
        {
            try
            {
                VideoEncoderConfiguration configuration = new VideoEncoderConfiguration(Constants.VideoDimensions[Config().GetVideoDimenIndex()], VideoEncoderConfiguration.FRAME_RATE.FrameRateFps15,
                    VideoEncoderConfiguration.StandardBitrate, VideoEncoderConfiguration.ORIENTATION_MODE.OrientationModeFixedPortrait)
                {
                    MirrorMode = Constants.VideoMirrorModes[Config().GetMirrorEncodeIndex()]
                };
                RtcEngine().SetVideoEncoderConfiguration(configuration); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        protected void JoinChannel()
        {
            try
            {
                RtcEngine().JoinChannel(null, Config().GetChannelName(), "", 0);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //protected TextureView PrepareRtcVideo(int uid, bool local)
        //{
        //    try
        //    {
        //        // Render local/remote video on a SurfaceView

        //        var surface = DT.Xamarin.Agora.RtcEngine.CreateTextureView(ApplicationContext);
        //        if (local)
        //        {
        //            RtcEngine().SetupLocalVideo(new VideoCanvas(surface, VideoCanvas.RenderModeHidden, 0, Constants.VideoMirrorModes[Config().GetMirrorLocalIndex()]));
        //        }
        //        else
        //        {
        //            RtcEngine().SetupRemoteVideo(new VideoCanvas(surface, VideoCanvas.RenderModeHidden, uid, Constants.VideoMirrorModes[Config().GetMirrorRemoteIndex()]));
        //        }
        //        return surface;
        //    }
        //    catch (Exception e)
        //    {
        //        Methods.DisplayReportResultTrack(e);
        //        return null;
        //    }
        //}
         
        protected SurfaceView PrepareRtcVideo(int uid, bool local)
        {
            try
            {
                SurfaceView surface = DT.Xamarin.Agora.RtcEngine.CreateRendererView(ApplicationContext);
                switch (local)
                {
                    case true:
                        RtcEngine().SetupLocalVideo(new VideoCanvas(surface, VideoCanvas.RenderModeHidden, Config().GetChannelName(), 0, Constants.VideoMirrorModes[Config().GetMirrorLocalIndex()]));
                        break;
                    default:
                        RtcEngine().SetupRemoteVideo(new VideoCanvas(surface, VideoCanvas.RenderModeHidden, Config().GetChannelName(), uid, Constants.VideoMirrorModes[Config().GetMirrorRemoteIndex()]));
                        break;
                }
                return surface;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null;
            }
        }
         
        protected void RemoveRtcVideo(int uid, bool local)
        {
            try
            {
                switch (local)
                {
                    case true:
                        RtcEngine().SetupLocalVideo(null);
                        break;
                    default:
                        RtcEngine().SetupRemoteVideo(new VideoCanvas(null, VideoCanvas.RenderModeHidden, uid));
                        break;
                }
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
                RemoveRtcEventHandler(this);
                RtcEngine().LeaveChannel(); 
                base.OnDestroy();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void RegisterRtcEventHandler(IEventHandler handler)
        {
            try
            {
                Application().RegisterEventHandler(handler);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private new MainApplication Application()
        {
            try
            {
                return (MainApplication)((Activity)this).Application;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null;
            }
        }

        protected RtcEngine RtcEngine()
        {
            try
            {
                return Application()?.RtcEngine();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null;
            }
        }

        protected EngineConfig Config()
        {
            try
            {
                return Application().EngineConfig();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null;
            }
        }

        protected StatsManager StatsManager() { return Application().StatsManager(); }

        private void RemoveRtcEventHandler(IEventHandler handler)
        {
            try
            {
                Application().RemoveEventHandler(handler);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }


        public void OnFirstLocalVideoFrame(int width, int height, int elapsed)
        {
             
        }

        public void OnFirstRemoteVideoDecoded(int uid, int width, int height, int elapsed)
        {
           
        }

        public void OnLeaveChannel(IRtcEngineEventHandler.RtcStats stats)
        {
           
        }

        public void OnJoinChannelSuccess(string channel, int uid, int elapsed)
        {
           
        }

        public void OnUserOffline(int uid, int reason)
        {
           
        }

        public void OnUserJoined(int uid, int elapsed)
        {
            
        }

        public void OnLastmileQuality(int quality)
        {
           
        }

        public void OnLastmileProbeResult(IRtcEngineEventHandler.LastmileProbeResult result)
        {
           
        }

        public void OnLocalVideoStats(IRtcEngineEventHandler.LocalVideoStats stats)
        {
           
        }

        public void OnRtcStats(IRtcEngineEventHandler.RtcStats stats)
        {
           
        }

        public void OnNetworkQuality(int uid, int txQuality, int rxQuality)
        {
           
        }

        public void OnRemoteVideoStats(IRtcEngineEventHandler.RemoteVideoStats stats)
        {
           
        }

        public void OnRemoteAudioStats(IRtcEngineEventHandler.RemoteAudioStats stats)
        {
           
        } 
    }
}
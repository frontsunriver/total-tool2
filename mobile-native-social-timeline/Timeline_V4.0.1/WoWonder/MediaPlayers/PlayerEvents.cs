using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Com.Google.Android.Exoplayer2;
using Com.Google.Android.Exoplayer2.Source;
using Com.Google.Android.Exoplayer2.Trackselection;
using Com.Google.Android.Exoplayer2.UI;
using WoWonder.Helpers.Utils;
using Object = Java.Lang.Object;

namespace WoWonder.MediaPlayers
{
    public class PlayerEvents : Object, IPlayerEventListener, PlayerControlView.IVisibilityListener, PlayerControlView.IProgressUpdateListener
    {
        //private readonly Activity ActContext;
        private readonly ProgressBar LoadingprogressBar;
        private readonly ImageButton VideoPlayButton, VideoResumeButton;

        public PlayerEvents(Activity act, PlayerControlView controlView)
        {
            try
            {
                //ActContext = act;

                if (controlView != null)
                {
                    VideoPlayButton = controlView.FindViewById<ImageButton>(Resource.Id.exo_play);
                    VideoResumeButton = controlView.FindViewById<ImageButton>(Resource.Id.exo_pause);
                    LoadingprogressBar = act.FindViewById<ProgressBar>(Resource.Id.progress_bar);
                }  
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            } 
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
                if (VideoResumeButton == null || VideoPlayButton == null || LoadingprogressBar== null)
                    return;

                switch (playbackState)
                {
                    case IPlayer.StateEnded:
                    {
                        switch (playWhenReady)
                        {
                            case false:
                                VideoResumeButton.Visibility = ViewStates.Visible;
                                break;
                            default:
                                VideoResumeButton.Visibility = ViewStates.Gone;
                                VideoPlayButton.Visibility = ViewStates.Visible;
                                break;
                        }

                        LoadingprogressBar.Visibility = ViewStates.Invisible;
                        break;
                    }
                    case IPlayer.StateReady:
                    {
                        switch (playWhenReady)
                        {
                            case false:
                                VideoResumeButton.Visibility = ViewStates.Gone;
                                VideoPlayButton.Visibility = ViewStates.Visible;
                                break;
                            default:
                                VideoResumeButton.Visibility = ViewStates.Visible;
                                break;
                        }

                        LoadingprogressBar.Visibility = ViewStates.Invisible;
                        break;
                    }
                    case IPlayer.StateBuffering:
                        LoadingprogressBar.Visibility = ViewStates.Visible;
                        VideoResumeButton.Visibility = ViewStates.Invisible;
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
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

        public void OnTimelineChanged(Timeline timeline, int reason)
        {
             
        }

        public void OnTracksChanged(TrackGroupArray p0, TrackSelectionArray p1)
        {

        }

        public void OnVisibilityChange(int p0)
        {

        }
         
        public void OnProgressUpdate(long position, long bufferedPosition)
        {
             
        }
    }
}
using System;
using System.Timers;
using Android.Graphics;
using Android.OS;

using Android.Views;
using Android.Widget;
using AndroidX.Interpolator.View.Animation;
using AT.Markushi.UI;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using Exception = System.Exception;

namespace WoWonder.Activities.Comment.Fragment
{
    public class RecordSoundFragment : AndroidX.Fragment.App.Fragment
    { 
        private CircleButton RecordPlayButton, RecordCloseButton;
        private SeekBar VoiceSeekBar;
        public string RecordFilePath;
        private Methods.AudioRecorderAndPlayer AudioPlayerClass;
        private CommentActivity MainActivityView;
        private Timer TimerSound;
         
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                View view = inflater.Inflate(Resource.Layout.RecourdSoundFragment, container, false); 
                return view;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            try
            {
                base.OnViewCreated(view, savedInstanceState);
                RecordFilePath = Arguments.GetString("FilePath");

                MainActivityView = (CommentActivity)Activity;
                MainActivityView.BtnVoice.SetImageResource(Resource.Drawable.microphone);
                MainActivityView.BtnVoice.Tag = "Audio";

                RecordPlayButton = view.FindViewById<CircleButton>(Resource.Id.playButton);
                RecordCloseButton = view.FindViewById<CircleButton>(Resource.Id.closeRecordButton);

                VoiceSeekBar = view.FindViewById<SeekBar>(Resource.Id.voiceseekbar);

                VoiceSeekBar.Progress = 0;
                RecordCloseButton.Click += RecordCloseButtonClick;
                RecordPlayButton.Click += RecordPlayButton_Click;
                RecordPlayButton.Tag = "Stop";

                AudioPlayerClass = new Methods.AudioRecorderAndPlayer(MainActivityView.PostId);
                TimerSound = new Timer();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void RecordCloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                switch (string.IsNullOrEmpty(RecordFilePath))
                {
                    case false:
                        AudioPlayerClass.StopAudioPlay();
                        break;
                }

                switch (UserDetails.SoundControl)
                {
                    case true:
                        Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("Error.mp3");
                        break;
                }

                var fragmentHolder = Activity.FindViewById<FrameLayout>(Resource.Id.TopFragmentHolder);

                AudioPlayerClass.Delete_Sound_Path(RecordFilePath);
                var interpolator = new FastOutSlowInInterpolator();
                fragmentHolder.Animate().SetInterpolator(interpolator).TranslationY(1200).SetDuration(300);
                Activity.SupportFragmentManager.BeginTransaction().Remove(this)?.Commit();

                MainActivityView.BtnVoice.SetImageResource(Resource.Drawable.microphone);
                MainActivityView.BtnVoice.Tag = "Free";
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void RecordPlayButton_Click(object sender, EventArgs e)
        {
            try
            {
                switch (string.IsNullOrEmpty(RecordFilePath))
                {
                    case false when RecordPlayButton?.Tag?.ToString() == "Stop":
                        RecordPlayButton.Tag = "Playing";
                        RecordPlayButton.SetColor(Color.ParseColor("#efefef"));
                        RecordPlayButton.SetImageResource(Resource.Drawable.ic_stop_dark_arrow);

                        AudioPlayerClass.PlayAudioFromPath(RecordFilePath);
                        VoiceSeekBar.Max = AudioPlayerClass.Player.Duration;
                        TimerSound.Interval = 1000;
                        TimerSound.Elapsed += TimerSound_Elapsed;
                        TimerSound.Start();
                        break;
                    case false:
                        RestPlayButton();
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void TimerSound_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (AudioPlayerClass.Player.CurrentPosition + 50 >= AudioPlayerClass.Player.Duration &&
                    AudioPlayerClass.Player.CurrentPosition + 50 <= AudioPlayerClass.Player.Duration + 20)
                {
                    VoiceSeekBar.Progress = AudioPlayerClass.Player.Duration;
                    RestPlayButton();
                    TimerSound.Stop();
                }
                else if (VoiceSeekBar.Max != AudioPlayerClass.Player.Duration && AudioPlayerClass.Player.Duration == 0)
                {
                    RestPlayButton();
                    VoiceSeekBar.Max = AudioPlayerClass.Player.Duration;
                }
                else
                {
                    VoiceSeekBar.Progress = AudioPlayerClass.Player.CurrentPosition;
                }
            }
            catch (Exception ex)
            {
                Methods.DisplayReportResultTrack(ex);
            }
        }

        private void RestPlayButton()
        {
            try
            {
                MainActivityView?.RunOnUiThread(() =>
                {
                    RecordPlayButton.Tag = "Stop";
                    RecordPlayButton.SetColor(Color.White);
                    RecordPlayButton.SetImageResource(Resource.Drawable.ic_play_dark_arrow);
                    AudioPlayerClass.Player.Stop();
                    VoiceSeekBar.Progress = 0;
                });

                TimerSound.Stop();
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
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

    }
}
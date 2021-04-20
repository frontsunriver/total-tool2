using System;
using System.Threading.Tasks;
using System.Timers;
using Android;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;

using Android.Views;
using Android.Widget;
using AT.Markushi.UI;
using Com.Sothree.Slidinguppanel;
using Google.Android.Material.BottomSheet;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Posts;

namespace WoWonder.Activities.AddPost
{
    public class VoiceRecorder : BottomSheetDialogFragment
    {
        #region Variables Basic

        private AddPostActivity MainActivityContext;

        private TextView IconClose, IconMicrophone;
        private CircleButton RecordPlayButton, RecordCloseButton, SendRecordButton, BtnVoice;
        private LinearLayout RecordLayout;
        private SeekBar VoiceSeekBar;
        private string RecordFilePath, TextRecorder;
        private Methods.AudioRecorderAndPlayer AudioPlayerClass;
        private Timer TimerSound;
        private bool IsRecording;

        #endregion

        #region General

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            MainActivityContext = (AddPostActivity)Activity;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                var contextThemeWrapper = AppSettings.SetTabDarkTheme ? new ContextThemeWrapper(Activity, Resource.Style.MyTheme_Dark_Base) : new ContextThemeWrapper(Activity, Resource.Style.MyTheme_Base);

                // clone the inflater using the ContextThemeWrapper 
                LayoutInflater localInflater = inflater.CloneInContext(contextThemeWrapper);

                View view = localInflater.Inflate(Resource.Layout.DialogVoiceRecorder, container, false);

                InitComponent(view);
                 
                return view;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
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

        public override void OnDestroy()
        { 
            try
            {
                AudioPlayerClass?.StopAudioPlay();

                base.OnDestroy();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            } 
        }

        #endregion

        #region Functions

        private void InitComponent(View view)
        {
            try
            {
                IconClose = view.FindViewById<TextView>(Resource.Id.IconBack);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconClose, IonIconsFonts.Close);
                IconClose.Click += IconClose_Click;

                IconMicrophone = view.FindViewById<TextView>(Resource.Id.iconMicrophone);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconMicrophone, IonIconsFonts.Microphone);

                RecordLayout = view.FindViewById<LinearLayout>(Resource.Id.recordLayout);
                
                RecordPlayButton = view.FindViewById<CircleButton>(Resource.Id.playButton);
                RecordPlayButton.Tag = "Stop";
                RecordPlayButton.Click += RecordPlayButton_Click;
                 
                RecordCloseButton = view.FindViewById<CircleButton>(Resource.Id.closeRecordButton);
                RecordCloseButton.Click += RecordCloseButtonClick;

                SendRecordButton = view.FindViewById<CircleButton>(Resource.Id.sendRecordButton);
                SendRecordButton.Visibility = ViewStates.Visible;
                SendRecordButton.Click += SendRecordButton_Click;

                VoiceSeekBar = view.FindViewById<SeekBar>(Resource.Id.voiceseekbar);
                VoiceSeekBar.Progress = 0;
                 
                BtnVoice = view.FindViewById<CircleButton>(Resource.Id.startRecordButton);
                BtnVoice.LongClickable = true;
                BtnVoice.Tag = "Free";
                BtnVoice.LongClick += BtnVoiceOnLongClick;
                BtnVoice.Touch += BtnVoiceOnTouch;
                 
                AudioPlayerClass = new Methods.AudioRecorderAndPlayer("");
                TimerSound = new Timer();
                TextRecorder = "";
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Event
         
        private void SendRecordButton_Click(object sender, EventArgs e)
        {
            try
            { 
                MainActivityContext.NameAlbumButton.Visibility = ViewStates.Gone;

                //remove file the type
                MainActivityContext.AttachmentsAdapter.RemoveAll();

                var attach = new Attachments
                {
                    Id = MainActivityContext.AttachmentsAdapter.AttachmentList.Count + 1,
                    TypeAttachment = "postMusic",
                    FileSimple = "Audio_File",
                    FileUrl = RecordFilePath
                };

                MainActivityContext.AttachmentsAdapter.Add(attach);

                MainActivityContext.SlidingUpPanel.SetPanelState(SlidingUpPanelLayout.PanelState.Collapsed);

                Dismiss();
            }
            catch (Exception ez)
            {
                Console.WriteLine(ez);
            }
        }

        //Back
        private void IconClose_Click(object sender, EventArgs e)
        {
            try
            {
                AudioPlayerClass?.StopAudioPlay();
                Dismiss();
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
                 
                AudioPlayerClass.Delete_Sound_Path(RecordFilePath);

                RecordLayout.Visibility = ViewStates.Gone;

                BtnVoice.SetImageResource(0);
                BtnVoice.Tag = "Free";

                RecordFilePath = ""; 
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
                Activity?.RunOnUiThread(() =>
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

        //record voices ( Permissions is 102 )
        private void BtnVoiceOnLongClick(object sender, View.LongClickEventArgs e)
        {
            try
            {
                switch ((int)Build.VERSION.SdkInt)
                {
                    case < 23:
                        StartRecording();
                        break;
                    default:
                    {
                        //Check to see if any permission in our group is available, if one, then all are
                        if (Activity.CheckSelfPermission(Manifest.Permission.RecordAudio) == Permission.Granted)
                        {
                            StartRecording();
                        }
                        else
                        {
                            new PermissionsController(Activity).RequestPermission(102);
                        }

                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void BtnVoiceOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                var handled = false;

                switch (e?.Event?.Action)
                {
                    case MotionEventActions.Up:
                        try
                        {
                            switch (IsRecording)
                            {
                                case true:
                                {
                                    AudioPlayerClass.StopRecording();
                                    RecordFilePath = AudioPlayerClass.GetRecorded_Sound_Path();

                                    BtnVoice.SetColorFilter(Color.ParseColor(AppSettings.MainColor));
                                    BtnVoice.SetImageResource(0);
                                    BtnVoice.Tag = "tick";

                                    switch (TextRecorder)
                                    {
                                        case "Recording":
                                        {
                                            switch (string.IsNullOrEmpty(RecordFilePath))
                                            {
                                                case false:
                                                    Console.WriteLine("FilePath" + RecordFilePath);

                                                    RecordLayout.Visibility = ViewStates.Visible;
                                                    break;
                                            }

                                            TextRecorder = "";
                                            break;
                                        }
                                    }

                                    IsRecording = false;
                                    break;
                                }
                                default:
                                {
                                    switch (UserDetails.SoundControl)
                                    {
                                        case true:
                                            Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("Error.mp3");
                                            break;
                                    }

                                    Toast.MakeText(Activity, Activity.GetText(Resource.String.Lbl_HoldToRecord), ToastLength.Short)?.Show();
                                    break;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            Methods.DisplayReportResultTrack(exception);
                        }

                        BtnVoice.Pressed = false;
                        handled = true;
                        break;
                }

                e.Handled = handled;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private async void StartRecording()
        {
            try
            {
                switch (BtnVoice.Tag?.ToString())
                {
                    case "Free":
                    {
                        //Set Record Style
                        IsRecording = true;

                        switch (UserDetails.SoundControl)
                        {
                            case true:
                                Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("RecourdVoiceButton.mp3");
                                break;
                        }

                        if (TextRecorder != null && TextRecorder != "Recording")
                            TextRecorder = "Recording";

                        BtnVoice.SetColorFilter(Color.White);
                        BtnVoice.SetImageResource(Resource.Drawable.ic_stop_white_24dp);

                        AudioPlayerClass = new Methods.AudioRecorderAndPlayer("");

                        //Start Audio record
                        await Task.Delay(600);
                        AudioPlayerClass.StartRecording();
                        break;
                    }
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
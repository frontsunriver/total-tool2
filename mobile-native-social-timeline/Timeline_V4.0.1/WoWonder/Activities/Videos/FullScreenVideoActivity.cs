using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Com.Google.Android.Exoplayer2.UI;
using WoWonder.Activities.Base;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;

namespace WoWonder.Activities.Videos
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", LaunchMode = LaunchMode.SingleInstance,ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.Locale |ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenLayout |ConfigChanges.ScreenSize | ConfigChanges.SmallestScreenSize | ConfigChanges.UiMode)]
    public class FullScreenVideoActivity : BaseActivity
    {
        public VideoController VideoActionsController;
        private PlayerView FullscreenplayerView;

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

                SetContentView(Resource.Layout.FullScreenDialog_Layout);

                string type = Intent?.GetStringExtra("Type"); 
                switch (type)
                {
                    case "Movies":
                        VideoActionsController = new VideoController(this, "FullScreen");
                        VideoActionsController.PlayFullScreen();
                        //if (Intent?.GetStringExtra("Downloaded") == "Downloaded")
                        //    VideoActionsController.DownloadIcon.SetImageDrawable(GetDrawable(Resource.Drawable.ic_checked_red));
                        break;
                    case "Post":
                        FullscreenplayerView = FindViewById<PlayerView>(Resource.Id.player_view2);
                        break;
                } 
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        protected override void OnResume()
        {
            try
            {
                GC.Collect(0);
                base.OnResume();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }


        public override void OnBackPressed()
        {
            VideoActionsController?.InitFullscreenDialog("Close");
            base.OnBackPressed();
        }

        public override void OnTrimMemory(TrimMemory level)
        {
            try
            {
                
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                base.OnTrimMemory(level);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
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

         
        public void PlayFullScreen()
        {
            try
            {
                if (FullscreenplayerView != null)
                {
                    //WRecyclerView.GetInstance().VideoSurfaceView.Player?.AddListener(PlayerLitsener);
                    //FullscreenplayerView.Player = Player;
                   // FullscreenplayerView.Player.PlayWhenReady = true;
                    //MFullScreenIcon.SetImageDrawable(ActivityContext.GetDrawable(Resource.Drawable.ic_action_ic_fullscreen_skrink));
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }


    }
}
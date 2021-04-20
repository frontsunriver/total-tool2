using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using AppIntro;
using WoWonder.Activities.General;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Activities.Suggested.User;
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;

namespace WoWonder.Activities.WalkTroutPage
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class AppIntroWalkTroutPage : AppIntro2
    {
        private int Count = 1;
        private string Caller = "";

        #region General

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                AddSlide(new AnimFragment1());
                AddSlide(new AnimFragment2());
                AddSlide(new AnimFragment4());
                AddSlide(new AnimFragment3());

                if (AppSettings.WalkThroughSetFlowAnimation)
                    SetFlowAnimation();

                if (AppSettings.WalkThroughSetZoomAnimation)
                    SetZoomAnimation();

                else if (AppSettings.WalkThroughSetSlideOverAnimation)
                    SetSlideOverAnimation();

                else if (AppSettings.WalkThroughSetFadeAnimation)
                    SetFadeAnimation();

                if (AppSettings.WalkThroughSetDepthAnimation)
                    SetDepthAnimation();

                ShowStatusBar(false);

                //SetNavBarColor(Color.ParseColor(AppSettings.MainColor));
                SetIndicatorColor(Color.ParseColor(AppSettings.MainColor), Color.ParseColor("#888888"));
                //SetBarColor(Color.ParseColor("#3F51B5"));
                // SetSeparatorColor(Color.ParseColor("#2196f3"));

                Caller = Intent?.GetStringExtra("class") ?? "";

                switch ((int)Build.VERSION.SdkInt)
                {
                    // Check if we're running on Android 5.0 or higher
                    case < 23:
                        break;
                    default:
                        RequestPermissions(new[]
                        {
                            Manifest.Permission.AccessFineLocation,
                            Manifest.Permission.AccessCoarseLocation,
                            Manifest.Permission.Camera
                        }, 208);
                        break;
                }

                if (Methods.CheckConnectivity())
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { ApiPostAsync.FetchFirstNewsFeedApiPosts, ApiRequest.LoadSuggestedUser, ApiRequest.LoadSuggestedGroup, ApiRequest.GetMyGroups, ApiRequest.GetMyPages });
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

        #region Permissions && Result

        //Permissions
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            try
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }


        #endregion

        #region Functions

        public override void OnSlideChanged()
        {
            try
            {
                base.OnSlideChanged();
                Pressed();
                Count++;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void Pressed()
        {
            try
            {
                switch (Count)
                {
                    case 1:
                        {
                            switch ((int)Build.VERSION.SdkInt)
                            {
                                case >= 23 when AppSettings.InvitationSystem:
                                    RequestPermissions(new[]
                                    {
                                    Manifest.Permission.ReadContacts,
                                    Manifest.Permission.ReadPhoneNumbers,
                                    Manifest.Permission.GetAccounts,
                                }, 2);
                                    break;
                                case >= 23:
                                    RequestPermissions(new[]
                                    {
                                    Manifest.Permission.Camera
                                }, 2);
                                    break;
                            }

                            break;
                        }
                    case 2:
                        {
                            switch ((int)Build.VERSION.SdkInt)
                            {
                                case >= 23:
                                    RequestPermissions(new[]
                                    {
                                    Manifest.Permission.RecordAudio,
                                    Manifest.Permission.ModifyAudioSettings
                                }, 4);
                                    break;
                            }

                            break;
                        }
                    case 3:
                        break;
                    case 4:
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        // Do something when users tap on Done button.
        public override void OnDonePressed()
        {
            try
            {
                if (Caller.Contains("register"))
                {
                    switch (ListUtils.SettingsSiteList?.MembershipSystem)
                    {
                        case "1":
                            {
                                var intent = new Intent(this, typeof(GoProActivity));
                                intent.PutExtra("class", "register");
                                StartActivity(intent);
                                break;
                            }
                        default:
                            {
                                switch (AppSettings.ShowSuggestedUsersOnRegister)
                                {
                                    case true:
                                        {
                                            Intent newIntent = new Intent(this, typeof(SuggestionsUsersActivity));
                                            newIntent?.PutExtra("class", "register");
                                            StartActivity(newIntent);
                                            break;
                                        }
                                    default:
                                        StartActivity(new Intent(this, typeof(TabbedMainActivity)));
                                        break;
                                }

                                break;
                            }
                    }
                }
                else
                {
                    StartActivity(new Intent(this, typeof(TabbedMainActivity)));
                }

                Finish();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        // Do something when users tap on Skip button.
        public override void OnSkipPressed()
        {
            try
            {
                if (Caller.Contains("register"))
                {
                    switch (ListUtils.SettingsSiteList?.MembershipSystem)
                    {
                        case "1":
                            {
                                var intent = new Intent(this, typeof(GoProActivity));
                                intent.PutExtra("class", "register");
                                StartActivity(intent);
                                break;
                            }
                        default:
                            {
                                switch (AppSettings.ShowSuggestedUsersOnRegister)
                                {
                                    case true:
                                        {
                                            Intent newIntent = new Intent(this, typeof(SuggestionsUsersActivity));
                                            newIntent?.PutExtra("class", "register");
                                            StartActivity(newIntent);
                                            break;
                                        }
                                    default:
                                        StartActivity(new Intent(this, typeof(TabbedMainActivity)));
                                        break;
                                }

                                break;
                            }
                    }
                }
                else
                {
                    StartActivity(new Intent(this, typeof(TabbedMainActivity)));
                }

                Finish();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }


        #endregion

    }
}
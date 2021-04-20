using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Views;
using AndroidX.AppCompat.App;
using WoWonder.Helpers.Utils;

namespace WoWonder.Activities.Base
{
    [Activity]
    public class BaseActivity : AppCompatActivity
    {
        #region General

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

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            try
            {
                base.OnConfigurationChanged(newConfig);

                var currentNightMode = newConfig.UiMode & UiMode.NightMask;
                AppSettings.SetTabDarkTheme = currentNightMode switch
                {
                    UiMode.NightNo =>
                        // Night mode is not active, we're using the light theme
                        false,
                    UiMode.NightYes =>
                        // Night mode is active, we're using dark theme
                        true,
                    _ => AppSettings.SetTabDarkTheme
                };

                SetTheme(AppSettings.SetTabDarkTheme ? Resource.Style.MyTheme_Dark_Base : Resource.Style.MyTheme_Base);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        #endregion

        #region Menu

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        #endregion

    }
}
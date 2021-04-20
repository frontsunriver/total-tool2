using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using WoWonder.Library.Anjo.Share;
using WoWonder.Library.Anjo.Share.Abstractions;
using WoWonder.Activities.Base;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.SettingsPreferences.TellFriend
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MyAffiliatesActivity : BaseActivity 
    {
        #region Variables Basic

        private ImageView ImageUser;
        private TextView TxtLink , TxtMyAffiliates;
        private Button BtnShare; 

        #endregion
         
        #region General

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetTheme(AppSettings.SetTabDarkTheme ? Resource.Style.MyTheme_Dark_Base : Resource.Style.MyTheme_Base);
                Methods.App.FullScreenApp(this);

                // Create your application here
                SetContentView(Resource.Layout.Settings_MyAffiliates_Layout);

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();

                AdsGoogle.Ad_AdMobNative(this);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        protected override void OnResume()
        {
            try
            {
                base.OnResume();
                AddOrRemoveEvent(true);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        protected override void OnPause()
        {
            try
            {
                base.OnPause();
                AddOrRemoveEvent(false);
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
        protected override void OnDestroy()
        {
            try
            {
                DestroyBasic();
                base.OnDestroy();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
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

        #region Functions

        private void InitComponent()
        {
            try
            {
                ImageUser = FindViewById<ImageView>(Resource.Id.ImageUser);
                TxtLink = FindViewById<TextView>(Resource.Id.linkText);
                TxtMyAffiliates = FindViewById<TextView>(Resource.Id.myAffiliatesText);
                BtnShare = FindViewById<Button>(Resource.Id.cont);

                GlideImageLoader.LoadImage(this, UserDetails.Avatar, ImageUser, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);

                //https://demo.wowonder.com/register?ref=waelanjo
                TxtLink.Text = Client.WebsiteUrl + "/register?ref=" + UserDetails.Username;
                 
                switch (Convert.ToInt32(ListUtils.SettingsSiteList?.AmountPercentRef ?? "0"))
                {
                    case > 0:
                        TxtMyAffiliates.Text = GetString(Resource.String.Lbl_EarnUpTo) + "%" + ListUtils.SettingsSiteList?.AmountPercentRef + " " + GetString(Resource.String.Lbl_forEachUserYourReferToUs)  + " !";
                        break;
                    default:
                        var (currency, currencyIcon) = WoWonderTools.GetCurrency(ListUtils.SettingsSiteList?.AdsCurrency);
                        Console.WriteLine(currency);

                        TxtMyAffiliates.Text = GetString(Resource.String.Lbl_EarnUpTo) + " " + currencyIcon + ListUtils.SettingsSiteList?.AmountRef + " " + GetString(Resource.String.Lbl_forEachUserYourReferToUs) + " !";
                        break;
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void InitToolbar()
        {
            try
            {
                var toolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
                if (toolBar != null)
                {
                    toolBar.Title = GetText(Resource.String.Lbl_MyAffiliates);
                    toolBar.SetTitleTextColor(Color.ParseColor(AppSettings.MainColor));
                    SetSupportActionBar(toolBar);
                    SupportActionBar.SetDisplayShowCustomEnabled(true);
                    SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                    SupportActionBar.SetHomeButtonEnabled(true);
                    SupportActionBar.SetDisplayShowHomeEnabled(true);
                    SupportActionBar.SetHomeAsUpIndicator(AppCompatResources.GetDrawable(this, AppSettings.FlowDirectionRightToLeft ? Resource.Drawable.ic_action_right_arrow_color : Resource.Drawable.ic_action_left_arrow_color));

                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        private void AddOrRemoveEvent(bool addEvent)
        {
            try
            {
                switch (addEvent)
                {
                    // true +=  // false -=
                    case true:
                        BtnShare.Click += BtnShareOnClick;
                        TxtLink.LongClick += TxtLinkOnLongClick;
                        break;
                    default:
                        BtnShare.Click -= BtnShareOnClick;
                        TxtLink.LongClick -= TxtLinkOnLongClick;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        private void DestroyBasic()
        {
            try
            {
                ImageUser= null!;
                TxtLink = null!;
                TxtMyAffiliates = null!;
                BtnShare = null!; 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        //Copy
        private void TxtLinkOnLongClick(object sender, View.LongClickEventArgs e)
        {
            try
            {
                Methods.CopyToClipboard(this, TxtLink.Text);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Share
        private async void BtnShareOnClick(object sender, EventArgs e)
        {
            try
            {
                switch (CrossShare.IsSupported)
                {
                    //Share Plugin same as video
                    case false:
                        return;
                    default:
                        await CrossShare.Current.Share(new ShareMessage
                        {
                            Title = UserDetails.Username,
                            Text = "",
                            Url = TxtLink.Text
                        });
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion 
         
    }
}
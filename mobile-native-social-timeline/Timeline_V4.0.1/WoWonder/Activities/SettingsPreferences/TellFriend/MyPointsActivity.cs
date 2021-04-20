using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using WoWonder.Activities.Base;
using WoWonder.Activities.Wallet;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.SettingsPreferences.TellFriend
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MyPointsActivity : BaseActivity
    {
        #region Variables Basic

        private ImageView ImageUser;
        private TextView NameUser , TxtSubTitle;
        private TextView CircleCommentsPoint, IconCommentsPoint, TextCommentsPoint;
        private TextView CircleCreatePostPoint, IconCreatePostPoint, TextCreatePostPoint;
        private TextView CircleReactingPoint, IconReactingPoint, TextReactingPoint;
        private TextView CircleCreateBlogPoint, IconCreateBlogPoint, TextCreateBlogPoint;
        private TextView TextAddWallet;
        private RelativeLayout ReactingPointLayouts , AddWalletLayouts;
        private AdView MAdView;

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
                SetContentView(Resource.Layout.MyPointsLayout);

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar(); 
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

                MAdView?.Resume();
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

                MAdView?.Pause();
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
                ImageUser = FindViewById<ImageView>(Resource.Id.imageUser);
                NameUser = FindViewById<TextView>(Resource.Id.nameUser);
                TxtSubTitle = FindViewById<TextView>(Resource.Id.subTitle);

                CircleCommentsPoint = FindViewById<TextView>(Resource.Id.circleCommentsPoint);
                IconCommentsPoint = FindViewById<TextView>(Resource.Id.IconCommentsPoint);
                TextCommentsPoint = FindViewById<TextView>(Resource.Id.TextCommentsPoint);

                CircleCreatePostPoint = FindViewById<TextView>(Resource.Id.circleCreatePostPoint);
                IconCreatePostPoint = FindViewById<TextView>(Resource.Id.IconCreatePostPoint);
                TextCreatePostPoint = FindViewById<TextView>(Resource.Id.TextCreatePostPoint);

                ReactingPointLayouts = FindViewById<RelativeLayout>(Resource.Id.ReactingPointLayouts);
                CircleReactingPoint = FindViewById<TextView>(Resource.Id.circleReactingPoint);
                IconReactingPoint = FindViewById<TextView>(Resource.Id.IconReactingPoint);
                TextReactingPoint = FindViewById<TextView>(Resource.Id.TextReactingPoint);

                CircleCreateBlogPoint = FindViewById<TextView>(Resource.Id.circleCreateBlogPoint);
                IconCreateBlogPoint = FindViewById<TextView>(Resource.Id.IconCreateBlogPoint);
                TextCreateBlogPoint = FindViewById<TextView>(Resource.Id.TextCreateBlogPoint);

                AddWalletLayouts = FindViewById<RelativeLayout>(Resource.Id.AddWalletLayouts); 
                TextAddWallet = FindViewById<TextView>(Resource.Id.TextAddWallet);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeSolid, CircleCommentsPoint, FontAwesomeIcon.Circle);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeSolid, CircleCreatePostPoint, FontAwesomeIcon.Circle);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeSolid, CircleReactingPoint, FontAwesomeIcon.Circle);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeSolid, CircleCreateBlogPoint, FontAwesomeIcon.Circle);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeSolid, IconCommentsPoint, FontAwesomeIcon.CommentAlt);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeRegular, IconCreatePostPoint, FontAwesomeIcon.Newspaper);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeRegular, IconCreateBlogPoint, FontAwesomeIcon.Blog);
                 
                CircleCommentsPoint.SetTextColor(Color.ParseColor("#4caf50"));
                CircleCreatePostPoint.SetTextColor(Color.ParseColor("#2196F3"));
                CircleCreateBlogPoint.SetTextColor(Color.ParseColor("#7a7a7a"));

                MAdView = FindViewById<AdView>(Resource.Id.adView);
                AdsGoogle.InitAdView(MAdView, null);

                var myProfile = ListUtils.MyProfileList?.FirstOrDefault();
                if (myProfile != null)
                {
                    GlideImageLoader.LoadImage(this, myProfile.Avatar, ImageUser, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                    NameUser.Text = WoWonderTools.GetNameFinal(myProfile);

                    TxtSubTitle.Text = GetString(Resource.String.Btn_Points) + " : " + myProfile.Points;
                }
              
                var setting = ListUtils.SettingsSiteList;
                if (setting != null)
                {
                    TextCommentsPoint.Text = GetString(Resource.String.Lbl_Earn) + " " + setting.CommentsPoint + " " + GetString(Resource.String.Lbl_ByCommentingAnyPost);
                    TextCreatePostPoint.Text = GetString(Resource.String.Lbl_Earn) + " " + setting.CreatepostPoint + " " + GetString(Resource.String.Lbl_ByCreatingNewPost);
                    TextCreateBlogPoint.Text = GetString(Resource.String.Lbl_Earn) + " " + setting.CreateblogPoint + " " + GetString(Resource.String.Lbl_ByCreatingNewBlog);

                    switch (AppSettings.PostButton)
                    {
                        case PostButtonSystem.ReactionDefault:
                        case PostButtonSystem.ReactionSubShine:
                            FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeRegular, IconReactingPoint, FontAwesomeIcon.Smile);
                            CircleReactingPoint.SetTextColor(Color.ParseColor("#FF9800"));
                            TextReactingPoint.Text = GetString(Resource.String.Lbl_Earn) + " " + setting.ReactionPoint + " " + GetString(Resource.String.Lbl_ByReactingAnyPost);
                            break;
                        case PostButtonSystem.Wonder:
                            FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeSolid, IconReactingPoint, FontAwesomeIcon.Exclamation);
                            CircleReactingPoint.SetTextColor(Color.ParseColor("#b71c1c"));

                            TextReactingPoint.Text = GetString(Resource.String.Lbl_Earn) + " " + setting.WondersPoint + " " + GetString(Resource.String.Lbl_ByWonderingAnyPost);
                            break;
                        case PostButtonSystem.DisLike:
                            FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeSolid, IconReactingPoint, FontAwesomeIcon.ThumbsDown);
                            CircleReactingPoint.SetTextColor(Color.ParseColor("#0D47A1"));

                            TextReactingPoint.Text = GetString(Resource.String.Lbl_Earn) + " " + setting.DislikesPoint + " " + GetString(Resource.String.Lbl_ByDislikingAnyPost);
                            break;
                        case PostButtonSystem.Like:
                            ReactingPointLayouts.Visibility = ViewStates.Gone;
                            break;
                    } 
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
                    toolBar.Title = GetString(Resource.String.Lbl_MyPoints);
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
                        AddWalletLayouts.Click += AddWalletLayoutsOnClick;
                        break;
                    default:
                        AddWalletLayouts.Click -= AddWalletLayoutsOnClick;
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
                MAdView?.Destroy();

                ImageUser = null!;
                NameUser = null!;
                TxtSubTitle = null!;
                CircleCommentsPoint = null!;
                IconCommentsPoint = null!;
                TextCommentsPoint = null!;
                CircleCreatePostPoint = null!;
                IconCreatePostPoint = null!;
                TextCreatePostPoint = null!;
                ReactingPointLayouts = null!;
                CircleReactingPoint = null!;
                IconReactingPoint = null!;
                TextReactingPoint = null!;
                CircleCreateBlogPoint = null!;
                IconCreateBlogPoint = null!;
                TextCreateBlogPoint = null!;
                AddWalletLayouts = null!;
                TextAddWallet = null!;
                MAdView = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        private void AddWalletLayoutsOnClick(object sender, EventArgs e)
        {
            try
            {
                Intent intent = new Intent(this,typeof(TabbedWalletActivity));
                StartActivity(intent);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        #endregion

    }
}
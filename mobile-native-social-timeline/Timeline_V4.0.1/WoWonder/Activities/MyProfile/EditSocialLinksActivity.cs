using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS; 
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using Java.Lang;
using WoWonder.Activities.Base;
using WoWonder.Activities.MyProfile.Adapters;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.MyProfile
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class EditSocialLinksActivity : BaseActivity, MaterialDialog.ISingleButtonCallback,MaterialDialog.IInputCallback
    {
        #region Variables Basic

        private SocialLinksAdapter MAdapter;
        private SwipeRefreshLayout SwipeRefreshLayout;
        private RecyclerView MRecycler;
        private LinearLayoutManager LayoutManager;
        private ViewStub EmptyStateLayout;
        private SocialItem SocialItem;
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
                SetContentView(Resource.Layout.RecyclerDefaultLayout);

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();

                Get_Data_User();
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
                MRecycler = (RecyclerView)FindViewById(Resource.Id.recyler);
                EmptyStateLayout = FindViewById<ViewStub>(Resource.Id.viewStub);
                EmptyStateLayout.Visibility = ViewStates.Gone;

                SwipeRefreshLayout = (SwipeRefreshLayout)FindViewById(Resource.Id.swipeRefreshLayout);
                SwipeRefreshLayout.SetColorSchemeResources(Android.Resource.Color.HoloBlueLight, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
                SwipeRefreshLayout.Refreshing = false;
                SwipeRefreshLayout.Enabled = false;
                SwipeRefreshLayout.SetProgressBackgroundColorSchemeColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#424242") : Color.ParseColor("#f7f7f7"));


                MAdView = FindViewById<AdView>(Resource.Id.adView);
                AdsGoogle.InitAdView(MAdView, MRecycler);

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
                    toolBar.Title = GetText(Resource.String.Lbl_SocialLinks);
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

        private void SetRecyclerViewAdapters()
        {
            try
            {
                MAdapter = new SocialLinksAdapter(this);
                LayoutManager = new LinearLayoutManager(this);
                MRecycler.SetLayoutManager(LayoutManager);
                MRecycler.SetAdapter(MAdapter);
                MRecycler.HasFixedSize = true;
                MRecycler.SetItemViewCacheSize(10);
                MRecycler.GetLayoutManager().ItemPrefetchEnabled = true;
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
                        MAdapter.ItemClick += MAdapterOnItemClick;
                        break;
                    default:
                        MAdapter.ItemClick -= MAdapterOnItemClick;
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

                MAdapter = null!;
                SwipeRefreshLayout = null!;
                MRecycler = null!;
                EmptyStateLayout = null!;
                SocialItem = null!;
                MAdView = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        private void MAdapterOnItemClick(object sender, SocialLinksAdapterClickEventArgs e)
        {
            try
            {
                var item = MAdapter.GetItem(e.Position);
                if (item != null)
                {
                    SocialItem = item;

                    var dialog = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                    dialog.Title(item.SocialName).TitleColorRes(Resource.Color.primary);
                    dialog.Input(Resource.String.Lbl_Enter_your_link, 0, false, this);

                    dialog.InputType(InputTypes.TextFlagImeMultiLine);
                    dialog.PositiveText(GetText(Resource.String.Lbl_Save)).OnPositive(this);
                    dialog.NegativeText(GetText(Resource.String.Lbl_Cancel)).OnNegative(this);
                    dialog.Build().Show();
                    dialog.AlwaysCallSingleChoiceCallback();
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region MaterialDialog
         
        public void OnInput(MaterialDialog p0, ICharSequence p1)
        {
            try
            {
                if (p1.Length() > 0)
                {
                    var strName = p1.ToString();

                    if (Methods.CheckConnectivity())
                    {
                        var dataUser = ListUtils.MyProfileList?.FirstOrDefault();
                        if (SocialItem != null)
                        {
                            MAdapter.Update(SocialItem, strName);

                            var dataPrivacy = new Dictionary<string, string>();

                            switch (SocialItem.Id)
                            {
                                case 1:
                                {
                                    dataPrivacy.Add("facebook", strName);
                                    if (dataUser != null) dataUser.Facebook = strName;
                                    break;
                                }
                                case 2:
                                {
                                    dataPrivacy.Add("twitter", strName);
                                    if (dataUser != null) dataUser.Twitter = strName;
                                    break;
                                }
                                case 3:
                                {
                                    dataPrivacy.Add("google", strName);
                                    if (dataUser != null) dataUser.Google = strName;
                                    break;
                                }
                                case 4:
                                {
                                    dataPrivacy.Add("vk", strName);
                                    if (dataUser != null) dataUser.Vk = strName;
                                    break;
                                }
                                case 5:
                                {
                                    dataPrivacy.Add("linkedin", strName);
                                    if (dataUser != null) dataUser.Linkedin = strName;
                                    break;
                                }
                                case 6:
                                {
                                    dataPrivacy.Add("instagram", strName);
                                    if (dataUser != null) dataUser.Instagram = strName;
                                    break;
                                }
                                case 7:
                                {
                                    dataPrivacy.Add("youtube", strName);
                                    if (dataUser != null) dataUser.Youtube = strName;
                                    break;
                                }
                            }

                            if (dataUser != null)
                            {
                                var sqLiteDatabase = new SqLiteDatabase();
                                sqLiteDatabase.Insert_Or_Update_To_MyProfileTable(dataUser);
                                
                            }

                            PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Global.UpdateUserDataAsync(dataPrivacy) }); 
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_The_name_can_not_be_blank), ToastLength.Short)?.Show();
                }
            }
            catch (Exception ex)
            {
                Methods.DisplayReportResultTrack(ex);
            }
        }

        public void OnClick(MaterialDialog p0, DialogAction p1)
        {
        }
      
        #endregion

        private void Get_Data_User()
        {
            try
            {
                var local = ListUtils.MyProfileList?.FirstOrDefault();
                if (local != null)
                {
                    foreach (var item in MAdapter.SocialList)
                    {
                        switch (item.Id)
                        {
                            //Facebook
                            case 1 when !string.IsNullOrEmpty(local.Facebook):
                                item.SocialLinkName = local.Facebook;
                                item.Checkvisibilty = true;
                                break;
                            case 1:
                                item.Checkvisibilty = false;
                                break;
                            // Twitter
                            case 2 when !string.IsNullOrEmpty(local.Twitter):
                                item.SocialLinkName = local.Twitter;
                                item.Checkvisibilty = true;
                                break;
                            case 2:
                                item.Checkvisibilty = false;
                                break;
                            // Google
                            case 3 when !string.IsNullOrEmpty(local.Google):
                                item.SocialLinkName = local.Google;
                                item.Checkvisibilty = true;
                                break;
                            case 3:
                                item.Checkvisibilty = false;
                                break;
                            // Vkontakte
                            case 4 when !string.IsNullOrEmpty(local.Vk):
                                item.SocialLinkName = local.Vk;
                                item.Checkvisibilty = true;
                                break;
                            case 4:
                                item.Checkvisibilty = false;
                                break;
                            // Linkedin
                            case 5 when !string.IsNullOrEmpty(local.Linkedin):
                                item.SocialLinkName = local.Linkedin;
                                item.Checkvisibilty = true;
                                break;
                            case 5:
                                item.Checkvisibilty = false;
                                break;
                            // Instagram
                            case 6 when !string.IsNullOrEmpty(local.Instagram):
                                item.SocialLinkName = local.Instagram;
                                item.Checkvisibilty = true;
                                break;
                            case 6:
                                item.Checkvisibilty = false;
                                break;
                            // YouTube
                            case 7 when !string.IsNullOrEmpty(local.Youtube):
                                item.SocialLinkName = local.Youtube;
                                item.Checkvisibilty = true;
                                break;
                            case 7:
                                item.Checkvisibilty = false;
                                break;
                        }}}
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        } 
    }
}
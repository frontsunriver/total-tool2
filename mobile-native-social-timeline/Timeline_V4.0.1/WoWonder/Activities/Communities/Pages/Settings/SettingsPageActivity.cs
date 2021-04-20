using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS;


using Android.Views;
using AndroidX.AppCompat.Content.Res;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using Newtonsoft.Json;
using WoWonder.Activities.Base;
using WoWonder.Activities.Communities.Adapters;
using WoWonder.Activities.Jobs;
using WoWonder.Activities.Offers;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Communities.Pages.Settings
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class SettingsPageActivity : BaseActivity 
    {
        #region Variables Basic

        private SettingsAdapter MAdapter;
        private SwipeRefreshLayout SwipeRefreshLayout;
        private RecyclerView MRecycler;
        private LinearLayoutManager LayoutManager;
        private AdView MAdView;
        private string PageId = "";
        private PageClass PageData;

        #endregion

        #region General

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                SetTheme(AppSettings.SetTabDarkTheme ? Resource.Style.MyTheme_Dark_Base : Resource.Style.MyTheme_Base);

                // Create your application here
                SetContentView(Resource.Layout.RecyclerDefaultLayout);

                PageId = Intent?.GetStringExtra("PagesId");

                if (!string.IsNullOrEmpty(Intent?.GetStringExtra("PageData")))
                    PageData = JsonConvert.DeserializeObject<PageClass>(Intent?.GetStringExtra("PageData"));

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();
                AdsGoogle.Ad_Interstitial(this);
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
                MAdView?.Resume();
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
                MAdView?.Pause();
                base.OnPause();
                AddOrRemoveEvent(false);
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

        private void SetRecyclerViewAdapters()
        {
            try
            {
                MAdapter = new SettingsAdapter(this, "Page" , PageData);
                LayoutManager = new LinearLayoutManager(this);
                MRecycler.SetLayoutManager(LayoutManager);
                MRecycler.HasFixedSize = true;
                MRecycler.SetItemViewCacheSize(10);
                MRecycler.GetLayoutManager().ItemPrefetchEnabled = true;
                MRecycler.SetAdapter(MAdapter);
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
                    toolBar.Title = GetText(Resource.String.Lbl_Settings);
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
                PageId = null!;
                PageData = null!;
                MAdView = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        private void MAdapterOnItemClick(object sender, SettingsAdapterClickEventArgs adapterClickEvents)
        {
            try
            {
                var position = adapterClickEvents.Position;
                switch (position)
                {
                    case >= 0:
                    {
                        var item = MAdapter.GetItem(position);
                        if (item != null)
                        {
                            switch (item.Id)
                            {
                                // General
                                case 1:
                                {
                                    var intent = new Intent(this, typeof(PageGeneralActivity));
                                    intent.PutExtra("PageData", JsonConvert.SerializeObject(PageData));
                                    intent.PutExtra("PageId", PageId);
                                    StartActivityForResult(intent , 1250);
                                    break;
                                }
                                // PageInformation
                                case 2:
                                {
                                    var intent = new Intent(this, typeof(PageInfoActivity));
                                    intent.PutExtra("PageData", JsonConvert.SerializeObject(PageData));
                                    intent.PutExtra("PageId", PageId);
                                    StartActivityForResult(intent, 1250);
                                    break;
                                }
                                //ActionButtons
                                case 3:
                                {
                                    var intent = new Intent(this, typeof(PageActionButtonsActivity));
                                    intent.PutExtra("PageData", JsonConvert.SerializeObject(PageData));
                                    intent.PutExtra("PageId", PageId);
                                    StartActivityForResult(intent, 1250);
                                    break;
                                }
                                //SocialLinks
                                case 4:
                                {
                                    var intent = new Intent(this, typeof(PageSocialLinksActivity));
                                    intent.PutExtra("PageData", JsonConvert.SerializeObject(PageData));
                                    intent.PutExtra("PageId", PageId);
                                    StartActivityForResult(intent, 1250);
                                    break;
                                }
                                //OfferAJob
                                case 5:
                                {
                                    var intent = new Intent(this, typeof(OfferAJobActivity));
                                    intent.PutExtra("PageId", PageId);
                                    StartActivity(intent);
                                    break;
                                }
                                //Offer
                                case 6:
                                {
                                    var intent = new Intent(this, typeof(CreateOffersActivity));
                                    intent.PutExtra("PageId", PageId);
                                    StartActivity(intent);
                                    break;
                                }
                                //Admin
                                case 7:
                                {
                                    var intent = new Intent(this, typeof(PagesAdminActivity));
                                    intent.PutExtra("PageData", JsonConvert.SerializeObject(PageData));
                                    intent.PutExtra("PageId", PageId);
                                    StartActivity(intent);
                                    break;
                                }
                                //DeletePage
                                case 8:
                                {
                                    var intent = new Intent(this, typeof(DeleteCommunitiesActivity));
                                    intent.PutExtra("Id", PageId);
                                    intent.PutExtra("Type", "Page");
                                    StartActivityForResult(intent, 2019);
                                    break;
                                }
                            }
                        }

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

        #region Result

        //Result
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);

                switch (requestCode)
                {
                    case 2019 when resultCode == Result.Ok:
                    {
                        var manged = PagesActivity.GetInstance().MAdapter.SocialList.FirstOrDefault(a => a.TypeView == SocialModelType.MangedPages);
                        var dataListGroup = manged?.PagesModelClass.PagesList?.FirstOrDefault(a => a.PageId == PageId);
                        if (dataListGroup != null)
                        {
                            manged.PagesModelClass.PagesList.Remove(dataListGroup);
                            PagesActivity.GetInstance().MAdapter.NotifyDataSetChanged();

                            ListUtils.MyPageList.Remove(dataListGroup);

                            Finish();
                        } 

                        Intent returnIntent = new Intent();
                        SetResult(Result.Ok, returnIntent);
                        Finish();
                        break;
                    }
                    case 1250 when resultCode == Result.Ok:
                    {
                        var pageItem = data.GetStringExtra("pageItem") ?? "";
                        if (string.IsNullOrEmpty(pageItem))
                        {
                            PageData = JsonConvert.DeserializeObject<PageClass>(Intent?.GetStringExtra("pageItem"));
                            PageProfileActivity.PageData = PageData;
                        }

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
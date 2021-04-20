using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS;


using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using WoWonder.Library.Anjo.IntegrationRecyclerView;
using Bumptech.Glide.Util; 
using Newtonsoft.Json;
using WoWonder.Activities.Base;
using WoWonder.Activities.Communities.Adapters;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Activities.Search;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Page;
using WoWonderClient.Requests;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Communities.Pages
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class PagesActivity : BaseActivity
    {
        #region Variables Basic

        public SocialAdapter MAdapter;
        private SwipeRefreshLayout SwipeRefreshLayout;
        private RecyclerView MRecycler;
        private LinearLayoutManager LayoutManager;
        private ViewStub EmptyStateLayout;
        private View Inflated;
        private TextView TxtCreate;  
        private AdView MAdView;
        private static PagesActivity Instance; 
        private RecyclerViewOnScrollListener MainScrollEvent;

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
                 
                Instance = this;

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();

                StartApiService();
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

                SwipeRefreshLayout = (SwipeRefreshLayout)FindViewById(Resource.Id.swipeRefreshLayout);
                if (SwipeRefreshLayout != null)
                {
                    SwipeRefreshLayout.SetColorSchemeResources(Android.Resource.Color.HoloBlueLight, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
                    SwipeRefreshLayout.Refreshing = true;
                    SwipeRefreshLayout.Enabled = true;
                    SwipeRefreshLayout.SetProgressBackgroundColorSchemeColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#424242") : Color.ParseColor("#f7f7f7"));
                    SwipeRefreshLayout.SetBackgroundColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#282828") : Color.White);
                }

                MRecycler.SetBackgroundColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#282828") : Color.White);

                TxtCreate = FindViewById<TextView>(Resource.Id.toolbar_title);
                if (TxtCreate != null)
                {
                    TxtCreate.Text = GetString(Resource.String.Lbl_Create);
                    TxtCreate.Visibility = ViewStates.Visible;
                }
                 
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
                    toolBar.Title = GetText(Resource.String.Lbl_ExplorePage);
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
                MAdapter = new SocialAdapter(this, SocialModelType.Pages);
                LayoutManager = new LinearLayoutManager(this);
                MRecycler.SetLayoutManager(LayoutManager);
                MRecycler.SetAdapter(MAdapter);
                MRecycler.HasFixedSize = true;
                MRecycler.SetItemViewCacheSize(10);
                MRecycler.GetLayoutManager().ItemPrefetchEnabled = true;
                var sizeProvider = new FixedPreloadSizeProvider(10, 10);
                var preLoader = new RecyclerViewPreloader<SocialModelsClass>(this, MAdapter, sizeProvider, 8);
                MRecycler.AddOnScrollListener(preLoader);

                RecyclerViewOnScrollListener xamarinRecyclerViewOnScrollListener = new RecyclerViewOnScrollListener(LayoutManager);
                MainScrollEvent = xamarinRecyclerViewOnScrollListener;
                MainScrollEvent.LoadMoreEvent += MainScrollEventOnLoadMoreEvent;
                MRecycler.AddOnScrollListener(xamarinRecyclerViewOnScrollListener);
                MainScrollEvent.IsLoading = false;
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
                        TxtCreate.Click += TxtCreateOnClick;
                        MAdapter.PageItemClick += MAdapterOnItemClick;
                        SwipeRefreshLayout.Refresh += SwipeRefreshLayoutOnRefresh;
                        break;
                    default:
                        TxtCreate.Click -= TxtCreateOnClick;
                        MAdapter.PageItemClick -= MAdapterOnItemClick;
                        SwipeRefreshLayout.Refresh -= SwipeRefreshLayoutOnRefresh;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static PagesActivity GetInstance()
        {
            try
            {
                return Instance;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
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
                Inflated = null!;
                TxtCreate = null!; 
                Instance = null!;
                MAdView = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events
         
        //Scroll
        private void MainScrollEventOnLoadMoreEvent(object sender, EventArgs e)
        {
            try
            {
                //Code get last id where LoadMore >>
                var item = MAdapter.SocialList.LastOrDefault();
                if (item != null && !string.IsNullOrEmpty(item?.PageData?.PageId) && !MainScrollEvent.IsLoading)
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => GetLikedPages(item?.PageData?.PageId) });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Refresh
        private void SwipeRefreshLayoutOnRefresh(object sender, EventArgs e)
        {
            try
            {
                MAdapter.SocialList.Clear();
                MAdapter.NotifyDataSetChanged();

                MainScrollEvent.IsLoading = false;

                StartApiService();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void MAdapterOnItemClick(object sender, PageAdapterClickEventArgs e)
        {
            try
            {
                var item = MAdapter.GetItem(e.Position);
                if (item != null)
                { 
                    MainApplication.GetInstance()?.NavigateTo(this, typeof(PageProfileActivity), item.PageData);
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Event Create New Page
        private void TxtCreateOnClick(object sender, EventArgs e)
        {
            try
            {
                var intent = new Intent(this, typeof(CreatePageActivity));
                StartActivityForResult(intent, 200);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Permissions && Result

        //Result
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
                switch (requestCode)
                {
                    case 200 when resultCode == Result.Ok:
                    {
                        string result = data?.GetStringExtra("pageItem") ?? "";
                        switch (string.IsNullOrEmpty(result))
                        {
                            case false:
                            {
                                var item = JsonConvert.DeserializeObject<PageClass>(result);
                                switch (item)
                                {
                                    case null:
                                        return;
                                }

                                switch (MAdapter.SocialList.Count)
                                {
                                    case > 0:
                                    {
                                        var check = MAdapter.SocialList.FirstOrDefault(a => a.TypeView == SocialModelType.MangedPages);
                                        if (check != null)
                                        {
                                            MAdapter.PagesAdapter?.PageList?.Insert(0, item);
                                            MAdapter?.PagesAdapter?.NotifyDataSetChanged();
                                            MAdapter.NotifyDataSetChanged();
                                        }
                                        else
                                        {
                                            var socialSection = new SocialModelsClass
                                            {
                                                PagesModelClass = new PagesModelClass
                                                {
                                                    PagesList = new List<PageClass> { item },
                                                    More = "",
                                                    TitleHead = GetString(Resource.String.Lbl_Manage_Pages)
                                                },
                                                Id = 11111111,
                                                TypeView = SocialModelType.MangedPages
                                            };
                                            MAdapter.SocialList.Insert(0, socialSection);
                                            MAdapter.NotifyDataSetChanged();
                                        }

                                        break;
                                    }
                                    default:
                                    {
                                        var socialSection = new SocialModelsClass
                                        {
                                            PagesModelClass = new PagesModelClass
                                            {
                                                PagesList = new List<PageClass> { item },
                                                More = "",
                                                TitleHead = GetString(Resource.String.Lbl_Manage_Pages)
                                            },
                                            Id = 11111111,
                                            TypeView = SocialModelType.MangedPages
                                        };
                                        MAdapter.SocialList.Insert(0, socialSection);
                                        MAdapter.NotifyDataSetChanged();
                                        break;
                                    }
                                }

                                break;
                            }
                        }

                        ShowEmptyPage();
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

        #region Get Page

        private void StartApiService()
        {
            if (!Methods.CheckConnectivity())
                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
            else
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { GetMyPages   });
        }

        private async Task GetMyPages()
        {
            if (Methods.CheckConnectivity())
            {
                var (apiStatus, respond) = await RequestsAsync.Page.GetMyPagesAsync("0", "7");
                if (apiStatus != 200 || respond is not ListPagesObject result || result.Data == null)
                {
                    Methods.DisplayReportResult(this, respond);
                }
                else
                {
                    var respondList = result.Data.Count;
                    switch (respondList)
                    {
                        case > 0:
                        {
                            result.Data.Reverse();

                            var checkList = MAdapter.SocialList.FirstOrDefault(q => q.TypeView == SocialModelType.MangedPages);
                            switch (checkList)
                            {
                                case null:
                                {
                                    var socialSection = new SocialModelsClass
                                    {
                                        Id = 0001111111,
                                        PagesModelClass = new PagesModelClass
                                        {
                                            PagesList = new List<PageClass>(),
                                            More = "",
                                            TitleHead = GetString(Resource.String.Lbl_Manage_Pages)
                                        },
                                        TypeView = SocialModelType.MangedPages
                                    };

                                    foreach (var item in from item in result.Data let check = socialSection.PagesModelClass.PagesList.FirstOrDefault(a => a.PageId == item.PageId) where check == null select item)
                                    {
                                        socialSection.PagesModelClass.PagesList.Add(item);

                                        switch (ListUtils.MyPageList.FirstOrDefault(a => a.PageId == item.PageId))
                                        {
                                            case null:
                                                ListUtils.MyPageList.Add(item);
                                                break;
                                        }
                                    }

                                    MAdapter.SocialList.Insert(0, socialSection);
                                    break;
                                }
                                default:
                                {
                                    foreach (var item in from item in result.Data let check = checkList.PagesModelClass.PagesList.FirstOrDefault(a => a.PageId == item.PageId) where check == null select item)
                                    {
                                        checkList.PagesModelClass.PagesList.Add(item);

                                        switch (ListUtils.MyPageList.FirstOrDefault(a => a.PageId == item.PageId))
                                        {
                                            case null:
                                                ListUtils.MyPageList.Add(item);
                                                break;
                                        }
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                    }
                }

                await GetLikedPages();

                RunOnUiThread(() => { MAdapter.NotifyDataSetChanged(); });

                RunOnUiThread(ShowEmptyPage);
            }
            else
            {
                Inflated ??= EmptyStateLayout.Inflate();
                EmptyStateInflater x = new EmptyStateInflater();
                x.InflateLayout(Inflated, EmptyStateInflater.Type.NoConnection);
                switch (x.EmptyStateButton.HasOnClickListeners)
                {
                    case false:
                        x.EmptyStateButton.Click += null!;
                        x.EmptyStateButton.Click += EmptyStateButtonOnClick;
                        break;
                }

                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
            }
        }
         
        private async Task GetLikedPages(string offset = "0")
        {
            switch (MainScrollEvent.IsLoading)
            {
                case true:
                    return;
            }

            if (Methods.CheckConnectivity())
            {
                MainScrollEvent.IsLoading = true; 

                var (apiStatus, respond) = await RequestsAsync.Page.GetLikedPagesAsync(UserDetails.UserId, offset, "10");
                if (apiStatus != 200 || respond is not ListPagesObject result || result.Data == null)
                {
                    MainScrollEvent.IsLoading = false;
                    Methods.DisplayReportResult(this, respond);
                }
                else
                {
                    var respondList = result.Data.Count;
                    switch (respondList)
                    {
                        case > 0:
                        {
                            result.Data.RemoveAll(a => a.PageId == null);

                            var checkList = MAdapter.SocialList.FirstOrDefault(q => q.TypeView == SocialModelType.LikedPages);
                            switch (checkList)
                            {
                                case null:
                                {
                                    var section = new SocialModelsClass
                                    {
                                        Id = 000001010101,
                                        TitleHead = GetString(Resource.String.Lbl_Liked_Pages),
                                        TypeView = SocialModelType.Section
                                    };
                                    MAdapter.SocialList.Add(section);

                                    foreach (var item in from item in result.Data let check = MAdapter.SocialList.FirstOrDefault(a => a.Id == Convert.ToInt32(item.PageId)) where check == null select item)
                                    {
                                        item.IsLiked = new IsLiked
                                        {
                                            Bool = true,
                                            String = "yes"
                                        };

                                        MAdapter.SocialList.Add(new SocialModelsClass
                                        {
                                            PageData = item,
                                            Id = Convert.ToInt32(item.PageId),
                                            TypeView = SocialModelType.LikedPages
                                        });
                                    }

                                    break;
                                }
                                default:
                                {
                                    foreach (var item in from item in result.Data let check = MAdapter.SocialList.FirstOrDefault(a => a.Id == Convert.ToInt32(item.PageId)) where check == null select item)
                                    {
                                        item.IsLiked = new IsLiked
                                        {
                                            Bool = true,
                                            String = "yes"
                                        };

                                        MAdapter.SocialList.Add(new SocialModelsClass
                                        {
                                            PageData = item,
                                            Id = Convert.ToInt32(item.PageId),
                                            TypeView = SocialModelType.LikedPages
                                        });
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                        default:
                        {
                            switch (MAdapter.SocialList.Count)
                            {
                                case > 10 when !MRecycler.CanScrollVertically(1):
                                    Toast.MakeText(this, GetText(Resource.String.Lbl_NoMorePages), ToastLength.Short)?.Show();
                                    break;
                            }

                            break;
                        }
                    }
                }

                MainScrollEvent.IsLoading = false;
                RunOnUiThread(ShowEmptyPage);
            }
            else
            {
                Inflated ??= EmptyStateLayout.Inflate();
                EmptyStateInflater x = new EmptyStateInflater();
                x.InflateLayout(Inflated, EmptyStateInflater.Type.NoConnection);
                switch (x.EmptyStateButton.HasOnClickListeners)
                {
                    case false:
                        x.EmptyStateButton.Click += null!;
                        x.EmptyStateButton.Click += EmptyStateButtonOnClick;
                        break;
                }

                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                MainScrollEvent.IsLoading = false;
            }
        }
          
        private void ShowEmptyPage()
        {
            try
            {
                MainScrollEvent.IsLoading = false;
                SwipeRefreshLayout.Refreshing = false;

                switch (MAdapter.SocialList.Count)
                {
                    case > 0:
                        MAdapter.NotifyDataSetChanged();

                        MRecycler.Visibility = ViewStates.Visible;
                        EmptyStateLayout.Visibility = ViewStates.Gone;
                        break;
                    default:
                    {
                        MRecycler.Visibility = ViewStates.Gone;

                        Inflated ??= EmptyStateLayout.Inflate();

                        EmptyStateInflater x = new EmptyStateInflater();
                        x.InflateLayout(Inflated, EmptyStateInflater.Type.NoPage);
                        switch (x.EmptyStateButton.HasOnClickListeners)
                        {
                            case false:
                                x.EmptyStateButton.Click += null!;
                                x.EmptyStateButton.Click += SearchButtonOnClick;
                                break;
                        }
                        EmptyStateLayout.Visibility = ViewStates.Visible;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                MainScrollEvent.IsLoading = false;
                SwipeRefreshLayout.Refreshing = false;
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Event Open Search And Get Page Random
        private void SearchButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                var intent = new Intent(this, typeof(SearchTabbedActivity));
                intent.PutExtra("Key", "Random_Pages");
                StartActivity(intent);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //No Internet Connection 
        private void EmptyStateButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                StartApiService();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }


        #endregion
    }
}
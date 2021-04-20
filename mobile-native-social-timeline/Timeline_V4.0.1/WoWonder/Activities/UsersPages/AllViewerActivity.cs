using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WoWonder.Activities.Communities.Groups;
using WoWonder.Activities.Communities.Pages;
using WoWonder.Activities.Contacts.Adapters;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Activities.Search.Adapters;
using WoWonder.Activities.Story;
using WoWonder.Activities.Story.Adapters;
using WoWonder.Activities.UserProfile.Adapters;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Group;
using WoWonderClient.Classes.Page;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Classes.Story;
using WoWonderClient.Classes.User;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.UsersPages
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class AllViewerActivity : BaseActivity
    {
        #region Variables Basic

        private dynamic MAdapter;
        private SwipeRefreshLayout SwipeRefreshLayout;
        private RecyclerView MRecycler;
        private LinearLayoutManager LayoutManager;
        private GridLayoutManager GridLayoutManager;
        private ViewStub EmptyStateLayout;
        private View Inflated;
        //Type = MangedGroupsModel , MangedPagesModel >> itemObject =  SocialModelsClass
        //Type = StoryModel , FollowersModel , GroupsModel , PagesModel >> itemObject = AdapterModelsClass
        //private SocialModelsClass ModelsClass;
        private AdapterModelsClass AdapterModelsClass;
        private FollowersModelClass FollowersModel;
        private GroupsModelClass GroupsModel;
        private PagesModelClass PagesModel;
        private ImagesModelClass ImagesModel; 
        private string Type, PassedId;
        private RecyclerViewOnScrollListener MainScrollEvent;
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

                Type = Intent?.GetStringExtra("Type") ?? "";
                PassedId = Intent?.GetStringExtra("PassedId") ?? "";

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();

                SetData();
                AdsGoogle.Ad_RewardedVideo(this);
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
                SwipeRefreshLayout.SetColorSchemeResources(Android.Resource.Color.HoloBlueLight, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
                SwipeRefreshLayout.Refreshing = true;
                SwipeRefreshLayout.Enabled = true;
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
                    toolBar.Title = Type switch
                    {
                        "MangedGroupsModel" => GetString(Resource.String.Lbl_Manage_Groups),
                        "MangedPagesModel" => GetString(Resource.String.Lbl_Manage_Pages),
                        "StoryModel" => GetString(Resource.String.Lbl_Story),
                        "FollowersModel" => GetString(Resource.String.Lbl_Following),
                        "GroupsModel" => GetString(Resource.String.Lbl_Groups),
                        "PagesModel" => GetString(Resource.String.Lbl_Pages),
                        "ImagesModel" => GetString(Resource.String.Lbl_Photos),
                        _ => toolBar.Title
                    };

                    toolBar.SetTitleTextColor(Color.ParseColor(AppSettings.MainColor));
                    SetSupportActionBar(toolBar);
                    SupportActionBar.SetDisplayShowCustomEnabled(true);
                    SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                    SupportActionBar.SetHomeButtonEnabled(true);
                    SupportActionBar.SetDisplayShowHomeEnabled(true);
                    SupportActionBar.SetHomeAsUpIndicator(AppCompatResources.GetDrawable(this, AppSettings.FlowDirectionRightToLeft ? Resource.Drawable.ic_action_right_arrow_color : AppSettings.FlowDirectionRightToLeft ? Resource.Drawable.ic_action_right_arrow_color : Resource.Drawable.ic_action_left_arrow_color));

                    
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
                dynamic preLoader = null!;
                var sizeProvider = new FixedPreloadSizeProvider(10, 10);

                switch (Type)
                {
                    case "GroupsModel":
                    case "MangedGroupsModel":
                        LayoutManager = new LinearLayoutManager(this);
                        MAdapter = new SearchGroupAdapter(this) {GroupList = new ObservableCollection<GroupClass>()};
                        switch (MAdapter)
                        {
                            case SearchGroupAdapter adapter1:
                                adapter1.ItemClick += GroupsModelOnItemClick;
                                adapter1.JoinButtonItemClick += MAdapterOnJoinButtonItemClick;
                                break;
                        }
                        preLoader = new RecyclerViewPreloader<GroupClass>(this, MAdapter, sizeProvider, 10);
                        break;
                    case "PagesModel":
                    case "MangedPagesModel":
                        LayoutManager = new LinearLayoutManager(this);
                        MAdapter = new SearchPageAdapter(this) {PageList = new ObservableCollection<PageClass>()};
                        switch (MAdapter)
                        {
                            case SearchPageAdapter adapter2:
                                adapter2.ItemClick += PagesModelOnItemClick;
                                adapter2.LikeButtonItemClick += MAdapterOnLikeButtonItemClick;
                                break;
                        }
                        preLoader = new RecyclerViewPreloader<PageClass>(this, MAdapter, sizeProvider, 10);
                        break;
                    case "StoryModel":
                        LayoutManager = new LinearLayoutManager(this);
                        MAdapter = new RowStoryAdapter(this) { StoryList = new ObservableCollection<StoryDataObject>() };
                        switch (MAdapter)
                        {
                            case RowStoryAdapter adapter3:
                                adapter3.ItemClick += StoryModelOnItemClick;
                                break;
                        }
                        preLoader = new RecyclerViewPreloader<StoryDataObject>(this, MAdapter, sizeProvider, 10);
                        break;
                    case "FollowersModel":
                        LayoutManager = new LinearLayoutManager(this);
                        MAdapter = new ContactsAdapter(this, true, ContactsAdapter.TypeTextSecondary.LastSeen){UserList = new ObservableCollection<UserDataObject>()};
                        switch (MAdapter)
                        {
                            case ContactsAdapter adapter4:
                                adapter4.ItemClick += AdapterFollowersOnItemClick;
                                adapter4.FollowButtonItemClick += adapter4.OnFollowButtonItemClick;
                                break;
                        }
                        preLoader = new RecyclerViewPreloader<UserDataObject>(this, MAdapter, sizeProvider, 10);
                        break;
                    case "ImagesModel":
                        GridLayoutManager = new GridLayoutManager(this, 3);
                        GridLayoutManager.SetSpanSizeLookup(new MySpanSizeLookup(4, 1, 1)); //5, 1, 2 
                        MAdapter = new UserPhotosAdapter(this){UserPhotosList =new ObservableCollection<PostDataObject>()};
                        switch (MAdapter)
                        {
                            case UserPhotosAdapter adapter5:
                                adapter5.ItemClick += AdapterPhotosOnItemClick;
                                break;
                        }
                        preLoader = new RecyclerViewPreloader<PostDataObject>(this, MAdapter, sizeProvider, 10);
                        break; 
                }

                MRecycler.SetLayoutManager(LayoutManager ?? GridLayoutManager);
                MRecycler.HasFixedSize = true;
                MRecycler.SetItemViewCacheSize(10);
                MRecycler.GetLayoutManager().ItemPrefetchEnabled = true; 
                MRecycler.AddOnScrollListener(preLoader);
                MRecycler.SetAdapter(MAdapter);
                 
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

        private void MAdapterOnLikeButtonItemClick(object sender, SearchPageAdapterClickEventArgs e)
        {
            try
            {
                var item = MAdapter.GetItem(e.Position);
                if (item != null)
                {
                    WoWonderTools.SetLikePage(this, item.PageId, e.Button);
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
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
                
                MAdView = null!; 
                AdapterModelsClass = null!;
                FollowersModel = null!;
                GroupsModel = null!;
                PagesModel = null!;
                ImagesModel = null!;

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events
          
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

        private void MainScrollEventOnLoadMoreEvent(object sender, EventArgs e)
        {
            try
            {
                switch (MainScrollEvent.IsLoading)
                {
                    case false:
                        switch (Type)
                        {
                            case "StoryModel":

                                break;
                            case "FollowersModel":
                                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { LoadContactsAsync });
                                break;
                            case "GroupsModel":

                                break;
                            case "PagesModel":

                                break;
                            case "ImagesModel":
                                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { LoadMyImage });
                                break;
                            case "MangedGroupsModel":
                                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { GetMyGroups });
                                break;
                            case "MangedPagesModel":
                                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { GetMyPages });
                                break;
                        }

                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        private void PagesModelOnItemClick(object sender, SearchPageAdapterClickEventArgs e)
        {
            try
            {
                switch (MAdapter)
                {
                    case SearchPageAdapter adapter:
                    {
                        var item = adapter.GetItem(e.Position);
                        if (item != null)
                        {
                            MainApplication.GetInstance()?.NavigateTo(this, typeof(PageProfileActivity), item);
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

        private void MAdapterOnJoinButtonItemClick(object sender, SearchGroupAdapterClickEventArgs e)
        {
            try
            {
                var item = MAdapter.GetItem(e.Position);
                if (item != null)
                {
                    WoWonderTools.SetJoinGroup(this, item.GroupId, e.Button);
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        private void GroupsModelOnItemClick(object sender, SearchGroupAdapterClickEventArgs e)
        {
            try
            {
                switch (MAdapter)
                {
                    case SearchGroupAdapter adapter:
                    {
                        var item = adapter.GetItem(e.Position);
                        if (item != null)
                        {
                            MainApplication.GetInstance()?.NavigateTo(this, typeof(GroupProfileActivity), item);
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

        private void AdapterFollowersOnItemClick(object sender, ContactsAdapterClickEventArgs e)
        {
            try
            {
                switch (MAdapter)
                {
                    case ContactsAdapter adapter:
                    {
                        var item = adapter.GetItem(e.Position);
                        if (item != null)
                        {
                            WoWonderTools.OpenProfile(this, item.UserId, item);
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

        private void StoryModelOnItemClick(object sender, RowStoryAdapterClickEventArgs e)
        {
            try
            {
                switch (MAdapter)
                {
                    case RowStoryAdapter adapter:
                        try
                        {
                            //Open View Story Or Create New Story
                            var item = adapter.GetItem(e.Position);
                            if (item != null)
                            { 
                                if (item.Type != "Your")
                                {
                                    Intent intent = new Intent(this, typeof(ViewStoryActivity));
                                    intent.PutExtra("UserId", item.UserId);
                                    intent.PutExtra("DataItem", JsonConvert.SerializeObject(item));
                                    StartActivity(intent);
                                } 
                            }
                        }
                        catch (Exception exception)
                        {
                            Methods.DisplayReportResultTrack(exception);
                        }

                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void AdapterPhotosOnItemClick(object sender, UserPhotosAdapterClickEventArgs e)
        {
            try
            {
                switch (MAdapter)
                {
                    case UserPhotosAdapter adapter:
                        try
                        {
                            //Open View Story Or Create New Story
                            var item = adapter.GetItem(e.Position);
                            if (item != null)
                            {
                                var intent = new Intent(this, typeof(ImagePostViewerActivity));
                                intent.PutExtra("itemIndex", "00"); 
                                intent.PutExtra("AlbumObject", JsonConvert.SerializeObject(item)); // PostDataObject 
                                OverridePendingTransition(Resource.Animation.abc_popup_enter, Resource.Animation.popup_exit); 
                                StartActivity(intent); 
                            }
                        }
                        catch (Exception exception)
                        {
                            Methods.DisplayReportResultTrack(exception);
                        }

                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        private void SetData()
        {
            try
            {
                var item = Intent?.GetStringExtra("itemObject");
                if (string.IsNullOrEmpty(item))
                    return;
                  
                switch (Type)
                {
                    case "MangedGroupsModel":
                    case "MangedPagesModel":
                    {
                        switch (MAdapter)
                        {
                            case SearchGroupAdapter adapter1:
                            {
                                GroupsModel = JsonConvert.DeserializeObject<GroupsModelClass>(item);
                                if (GroupsModel != null)
                                {
                                    adapter1.GroupList = new ObservableCollection<GroupClass>(GroupsModel.GroupsList);
                                    adapter1.NotifyDataSetChanged();
                                }

                                break;
                            }
                            case SearchPageAdapter adapter2:
                            {
                                PagesModel = JsonConvert.DeserializeObject<PagesModelClass>(item);
                                if (PagesModel != null)
                                {
                                    adapter2.PageList = new ObservableCollection<PageClass>(PagesModel.PagesList);
                                    adapter2.NotifyDataSetChanged();
                                }

                                break;
                            }
                        }

                        break;
                    } 
                    case "StoryModel":
                    case "FollowersModel":
                    case "GroupsModel":
                    case "PagesModel":
                    case "ImagesModel":
                    {
                        switch (MAdapter)
                        {
                            case SearchGroupAdapter adapter1:
                            {
                                GroupsModel = JsonConvert.DeserializeObject<GroupsModelClass>(item);
                                if (GroupsModel != null)
                                {
                                    adapter1.GroupList = new ObservableCollection<GroupClass>(GroupsModel.GroupsList);
                                    adapter1.NotifyDataSetChanged();
                                }

                                break;
                            }
                            case SearchPageAdapter adapter2:
                            {
                                PagesModel = JsonConvert.DeserializeObject<PagesModelClass>(item);
                                if (PagesModel != null)
                                {
                                    adapter2.PageList = new ObservableCollection<PageClass>(PagesModel.PagesList);
                                    adapter2.NotifyDataSetChanged();
                                }

                                break;
                            }
                            case ContactsAdapter adapter3:
                            {
                                FollowersModel = JsonConvert.DeserializeObject<FollowersModelClass>(item);
                                if (FollowersModel != null)
                                {
                                    adapter3.UserList = new ObservableCollection<UserDataObject>(FollowersModel.FollowersList);
                                    adapter3.NotifyDataSetChanged();
                                }

                                break;
                            }
                            case RowStoryAdapter adapter4:
                            {
                                AdapterModelsClass = JsonConvert.DeserializeObject<AdapterModelsClass>(item);
                                if (AdapterModelsClass != null)
                                {
                                    adapter4.StoryList = new ObservableCollection<StoryDataObject>(AdapterModelsClass.StoryList);
                                    adapter4.StoryList.Remove(adapter4.StoryList.FirstOrDefault(a => a.Type == "Your"));
                                    adapter4.NotifyDataSetChanged();
                                }

                                break;
                            }
                            case UserPhotosAdapter adapter5:
                            {
                                ImagesModel = JsonConvert.DeserializeObject<ImagesModelClass>(item);
                                if (ImagesModel != null)
                                {
                                    adapter5.UserPhotosList = new ObservableCollection<PostDataObject>(ImagesModel.ImagesList);
                                    adapter5.NotifyDataSetChanged();
                                }

                                break;
                            }
                        }

                        break;
                    } 
                }

                if (MAdapter?.ItemCount > 0)
                {
                    MRecycler.Visibility = ViewStates.Visible;
                    EmptyStateLayout.Visibility = ViewStates.Gone;
                }
                else
                    ShowEmptyPage();

                SwipeRefreshLayout.Refreshing = false;
                SwipeRefreshLayout.Enabled = false;

                StartApiService(); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                SwipeRefreshLayout.Refreshing = false;
                SwipeRefreshLayout.Enabled = false;
            }
        }

        private void StartApiService()
        {
            if (!Methods.CheckConnectivity())
                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
            else
            {
                switch (Type)
                {
                    case "StoryModel":
                        
                        break;
                    case "FollowersModel":
                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { LoadContactsAsync });
                        break;
                    case "GroupsModel":
                         
                        break;
                    case "PagesModel":
                         
                        break;
                    case "ImagesModel":
                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> {LoadMyImage });
                        break;
                    case "MangedGroupsModel":
                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { GetMyGroups });
                        break;
                    case "MangedPagesModel":
                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { GetMyPages });
                        break;
                } 
            }
        }

        private void ShowEmptyPage()
        {
            try
            {
                MainScrollEvent.IsLoading = false;
                SwipeRefreshLayout.Refreshing = false;

                if (MAdapter?.ItemCount > 0)
                {
                    MRecycler.Visibility = ViewStates.Visible;
                    EmptyStateLayout.Visibility = ViewStates.Gone;
                }
                else
                {
                    MRecycler.Visibility = ViewStates.Gone;

                    Inflated ??= EmptyStateLayout.Inflate();

                    EmptyStateInflater x = new EmptyStateInflater();

                    switch (Type)
                    {
                        case "GroupsModel":
                        case "MangedGroupsModel":
                            x.InflateLayout(Inflated, EmptyStateInflater.Type.NoGroup);
                            break;
                        case "PagesModel":
                        case "MangedPagesModel":
                            x.InflateLayout(Inflated, EmptyStateInflater.Type.NoPage);
                            break;
                        case "StoryModel":
                            x.InflateLayout(Inflated, EmptyStateInflater.Type.NoGroup);
                            break;
                        case "FollowersModel":
                            x.InflateLayout(Inflated, EmptyStateInflater.Type.NoUsers);
                            break;
                        case "ImagesModel":
                            x.InflateLayout(Inflated, EmptyStateInflater.Type.NoAlbum);
                            break;
                    }

                    x.EmptyStateButton.Visibility = ViewStates.Gone;
                    EmptyStateLayout.Visibility = ViewStates.Visible;
                }
            }
            catch (Exception e)
            {
                MainScrollEvent.IsLoading = false;
                SwipeRefreshLayout.Refreshing = false;
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Load Contacts 

        private async Task LoadContactsAsync()
        {
            switch (MAdapter)
            {
                case ContactsAdapter adapter when MainScrollEvent.IsLoading:
                    return;
                case ContactsAdapter adapter:
                {
                    var lastIdUser = adapter.UserList.LastOrDefault()?.UserId ?? "0";
                    if (Methods.CheckConnectivity())
                    {
                        MainScrollEvent.IsLoading = true;

                        var countList = adapter.UserList.Count;
                        var (apiStatus, respond) = await RequestsAsync.Global.GetFriendsAsync(PassedId, "following", "10", lastIdUser);
                        if (apiStatus != 200 || respond is not GetFriendsObject result || result.DataFriends == null)
                        {
                            MainScrollEvent.IsLoading = false;
                            Methods.DisplayReportResult(this, respond);
                        }
                        else
                        {
                            var respondList = result.DataFriends.Following.Count;
                            switch (respondList)
                            {
                                case > 0 when countList > 0:
                                {
                                    foreach (var item in from item in result.DataFriends.Following let check = adapter.UserList.FirstOrDefault(a => a.UserId == item.UserId) where check == null select item)
                                    {
                                        adapter.UserList.Add(item);
                                    }

                                    RunOnUiThread(() => { adapter.NotifyItemRangeInserted(countList, adapter.UserList.Count - countList); });
                                    break;
                                }
                                case > 0:
                                    adapter.UserList = new ObservableCollection<UserDataObject>(result.DataFriends.Following);
                                    RunOnUiThread(() => { adapter.NotifyDataSetChanged(); });
                                    break;
                                default:
                                {
                                    switch (adapter.UserList.Count)
                                    {
                                        case > 10 when !MRecycler.CanScrollVertically(1):
                                            Toast.MakeText(this, GetText(Resource.String.Lbl_No_more_users), ToastLength.Short)?.Show();
                                            break;
                                    }

                                    break;
                                }
                            }
                        }

                        RunOnUiThread(ShowEmptyPage);
                    }
                    else
                    {
                        Inflated = EmptyStateLayout.Inflate();
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
                    MainScrollEvent.IsLoading = false;
                    break;
                }
            }
        }
         
        #endregion

        #region Load Image 
          
        private async Task LoadMyImage()
        {
            switch (MAdapter)
            {
                case UserPhotosAdapter adapter when MainScrollEvent.IsLoading:
                    return;
                case UserPhotosAdapter adapter:
                {
                    if (Methods.CheckConnectivity())
                    {
                        MainScrollEvent.IsLoading = true;

                        var offset = adapter.UserPhotosList.LastOrDefault()?.Id ?? "0";
                        var countList = adapter.UserPhotosList.Count;
                        var (apiStatus, respond) = await RequestsAsync.Album.GetPostByTypeAsync(PassedId , "photos", "10", offset);
                        if (apiStatus != 200 || respond is not PostObject result || result.Data == null)
                        {
                            Methods.DisplayReportResult(this, respond);
                        }
                        else
                        {
                            var respondList = result.Data?.Count;
                            switch (respondList)
                            {
                                case > 0:
                                {
                                    result.Data.RemoveAll(w => string.IsNullOrEmpty(w.PostFileFull));

                                    switch (countList)
                                    {
                                        case > 0:
                                        {
                                            foreach (var item in from item in result.Data let check = adapter.UserPhotosList.FirstOrDefault(a => a.Id == item.Id) where check == null select item)
                                            {
                                                adapter.UserPhotosList.Add(item);
                                            }

                                            RunOnUiThread(() => { adapter.NotifyItemRangeInserted(countList, adapter.UserPhotosList.Count - countList); });
                                            break;
                                        }
                                        default:
                                            adapter.UserPhotosList = new ObservableCollection<PostDataObject>(result.Data);
                                            RunOnUiThread(() => { adapter.NotifyDataSetChanged(); });
                                            break;
                                    }

                                    break;
                                }
                                default:
                                {
                                    switch (adapter.UserPhotosList.Count)
                                    {
                                        case > 10 when !MRecycler.CanScrollVertically(1):
                                            Toast.MakeText(this, GetText(Resource.String.Lbl_NoMorePhoto), ToastLength.Short)?.Show();
                                            break;
                                    }

                                    break;
                                }
                            }
                        }

                        RunOnUiThread(ShowEmptyPage);
                    }
                    else
                    {
                        Inflated = EmptyStateLayout.Inflate();
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
                    MainScrollEvent.IsLoading = false;
                    break;
                }
            }
        }

        #endregion

        #region Load Manged Group

        private async Task GetMyGroups()
        {
            switch (MAdapter)
            {
                case SearchGroupAdapter adapter1 when MainScrollEvent.IsLoading:
                    return;
                case SearchGroupAdapter adapter1:
                {
                    var lastIdGroup = adapter1.GroupList.LastOrDefault()?.GroupId ?? "0";
                    if (Methods.CheckConnectivity())
                    {
                        MainScrollEvent.IsLoading = true;
                        var countList = adapter1.GroupList.Count;

                        var (apiStatus, respond) = await RequestsAsync.Group.GetMyGroupsAsync(lastIdGroup, "10");
                        if (apiStatus != 200 || respond is not ListGroupsObject result || result.Data == null)
                        {
                            Methods.DisplayReportResult(this, respond);
                        }
                        else
                        {
                            var respondList = result.Data.Count;
                            switch (respondList)
                            {
                                case > 0 when countList > 0:
                                {
                                    foreach (var item in from item in result.Data let check = adapter1.GroupList.FirstOrDefault(a => a.GroupId == item.GroupId) where check == null select item)
                                    {
                                        adapter1.GroupList.Add(item);

                                        switch (ListUtils.MyGroupList.FirstOrDefault(a => a.GroupId == item.GroupId))
                                        {
                                            case null:
                                                ListUtils.MyGroupList.Add(item);
                                                break;
                                        }
                                    }

                                    RunOnUiThread(() => { adapter1.NotifyItemRangeInserted(countList, adapter1.GroupList.Count - countList); });
                                    break;
                                }
                                case > 0:
                                    adapter1.GroupList = new ObservableCollection<GroupClass>(result.Data);
                                    RunOnUiThread(() => { adapter1.NotifyDataSetChanged(); });
                                    break;
                                default:
                                {
                                    switch (adapter1.GroupList.Count)
                                    {
                                        case > 10 when !MRecycler.CanScrollVertically(1):
                                            Toast.MakeText(this, GetText(Resource.String.Lbl_NoMoreGroup), ToastLength.Short)?.Show();
                                            break;
                                    }

                                    break;
                                }
                            }
                        }

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

                    break;
                }
            }
        }

        #endregion

        #region Load Manged Page
         
        private async Task GetMyPages()
        {
            switch (MAdapter)
            {
                case SearchPageAdapter adapter1 when MainScrollEvent.IsLoading:
                    return;
                case SearchPageAdapter adapter1:
                {
                    var lastIdPage = adapter1.PageList.LastOrDefault()?.PageId ?? "0";

                    if (Methods.CheckConnectivity())
                    {
                        MainScrollEvent.IsLoading = true;
                        var countList = adapter1.PageList.Count;

                        var (apiStatus, respond) = await RequestsAsync.Page.GetMyPagesAsync(lastIdPage, "10");
                        if (apiStatus != 200 || respond is not ListPagesObject result || result.Data == null)
                        {
                            Methods.DisplayReportResult(this, respond);
                        }
                        else
                        {
                            var respondList = result.Data.Count;
                            switch (respondList)
                            {
                                case > 0 when countList > 0:
                                {
                                    foreach (var item in from item in result.Data let check = adapter1.PageList.FirstOrDefault(a => a.PageId == item.PageId) where check == null select item)
                                    {
                                        adapter1.PageList.Add(item);

                                        switch (ListUtils.MyPageList.FirstOrDefault(a => a.PageId == item.PageId))
                                        {
                                            case null:
                                                ListUtils.MyPageList.Add(item);
                                                break;
                                        }
                                    }

                                    RunOnUiThread(() => { adapter1.NotifyItemRangeInserted(countList, adapter1.PageList.Count - countList); });
                                    break;
                                }
                                case > 0:
                                    adapter1.PageList = new ObservableCollection<PageClass>(result.Data);
                                    RunOnUiThread(() => { adapter1.NotifyDataSetChanged(); });
                                    break;
                                default:
                                {
                                    switch (adapter1.PageList.Count)
                                    {
                                        case > 10 when !MRecycler.CanScrollVertically(1):
                                            Toast.MakeText(this, GetText(Resource.String.Lbl_NoMorePages), ToastLength.Short)?.Show();
                                            break;
                                    }

                                    break;
                                }
                            }
                        }

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

                    break;
                }
            }
        }
     
        #endregion
    } 
} 
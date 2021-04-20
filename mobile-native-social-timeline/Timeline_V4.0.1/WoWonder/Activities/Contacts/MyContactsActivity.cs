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
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using WoWonder.Library.Anjo.IntegrationRecyclerView;
using Bumptech.Glide.Util;
using WoWonder.Activities.Base;
using WoWonder.Activities.Contacts.Adapters;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.User;
using WoWonderClient.Requests;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using AndroidX.Core.Content;
using AndroidX.AppCompat.Content.Res;

namespace WoWonder.Activities.Contacts
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MyContactsActivity : BaseActivity
    {
        #region Variables Basic

        private ContactsAdapter MAdapter;
        private SwipeRefreshLayout SwipeRefreshLayout;
        private RecyclerView MRecycler;
        private LinearLayoutManager LayoutManager;
        private ViewStub EmptyStateLayout;
        private View Inflated;
        private RecyclerViewOnScrollListener MainScrollEvent;
        private string TypeContacts = "" , UserId = "";
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

                var contactsType = Intent?.GetStringExtra("ContactsType") ?? "Data not available";
                if (contactsType != "Data not available" && !string.IsNullOrEmpty(contactsType))
                    TypeContacts = contactsType;

                UserId = Intent?.GetStringExtra("UserId");
                 
                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();

                LoadContacts();
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
                base.OnTrimMemory(level);
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced); 
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
                base.OnLowMemory();
                GC.Collect(GC.MaxGeneration);
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
                if (UserId == UserDetails.UserId && MAdapter.UserList.Count > 0)
                {
                    try
                    {
                        switch (TypeContacts)
                        {
                            case "Following":
                            {
                                var sqlEntity = new SqLiteDatabase();
                                sqlEntity.Insert_Or_Replace_MyContactTable(MAdapter.UserList);
                                break;
                            }
                            default:
                                ListUtils.MyFollowersList = new ObservableCollection<UserDataObject>(MAdapter.UserList);
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }

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
                    toolBar.Title = AppSettings.ConnectivitySystem == 1 ? GetText(TypeContacts == "Following" ? Resource.String.Lbl_Following : Resource.String.Lbl_Followers) : GetText(Resource.String.Lbl_Friends);

                    toolBar.SetTitleTextColor(ContextCompat.GetColor(this, Resource.Color.primary));
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
                MAdapter = new ContactsAdapter(this,true,ContactsAdapter.TypeTextSecondary.About)
                {
                    UserList = new ObservableCollection<UserDataObject>(),
                };
                LayoutManager = new LinearLayoutManager(this);
                MRecycler.SetLayoutManager(LayoutManager); 
                MRecycler.HasFixedSize = true;
                MRecycler.SetItemViewCacheSize(50);
                MRecycler.GetLayoutManager().ItemPrefetchEnabled = true;
                var sizeProvider = new FixedPreloadSizeProvider(10, 10);
                var preLoader = new RecyclerViewPreloader<UserDataObject>(this, MAdapter, sizeProvider, 10);
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
         
        private void AddOrRemoveEvent(bool addEvent)
        {
            try
            {
                switch (addEvent)
                {
                    // true +=  // false -=
                    case true:
                        MAdapter.ItemClick += MAdapterOnItemClick;
                        MAdapter.FollowButtonItemClick += MAdapter.OnFollowButtonItemClick;
                        SwipeRefreshLayout.Refresh += SwipeRefreshLayoutOnRefresh;
                        break;
                    default:
                        MAdapter.ItemClick -= MAdapterOnItemClick;
                        MAdapter.FollowButtonItemClick -= MAdapter.OnFollowButtonItemClick;
                        SwipeRefreshLayout.Refresh -= SwipeRefreshLayoutOnRefresh;
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
                Inflated = null!;
                MainScrollEvent = null!;
                TypeContacts = null!;
                UserId = null!;
                MAdView = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        //Refresh
        private void SwipeRefreshLayoutOnRefresh(object sender, EventArgs e)
        {
            try
            {
                MAdapter.UserList.Clear();
                MAdapter.NotifyDataSetChanged();

                MainScrollEvent.IsLoading = false;

                StartApiService();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Scroll
        private void MainScrollEventOnLoadMoreEvent(object sender, EventArgs e)
        {
            try
            {
                //Code get last id where LoadMore >>
                var item = MAdapter.UserList.LastOrDefault();
                if (item != null && !string.IsNullOrEmpty(item.UserId) && !MainScrollEvent.IsLoading)
                    StartApiService(item.UserId);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void MAdapterOnItemClick(object sender, ContactsAdapterClickEventArgs e)
        {
            try
            {
                var item = MAdapter.GetItem(e.Position);
                if (item != null)
                {
                    WoWonderTools.OpenProfile(this, item.UserId, item); 
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Load Contacts 

        private void LoadContacts()
        {
            try
            {
                if (UserId == UserDetails.UserId && TypeContacts == "Following")
                {
                    var sqlEntity = new SqLiteDatabase();
                    ObservableCollection<UserDataObject> userList = sqlEntity.Get_MyContact();
                    

                    MAdapter.UserList = new ObservableCollection<UserDataObject>(userList);
                    MAdapter.NotifyDataSetChanged();
                     
                    SwipeRefreshLayout.Refreshing = false;
                }

                var lastIdUser = MAdapter.UserList.LastOrDefault()?.UserId ?? "0";
                StartApiService(lastIdUser);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        private void StartApiService(string offset = "0")
        {
            if (!Methods.CheckConnectivity())
                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
            else
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => LoadContactsAsync(offset) });
        }

        private async Task LoadContactsAsync(string offset = "0")
        {
            switch (MainScrollEvent.IsLoading)
            {
                case true:
                    return;
            }

            if (Methods.CheckConnectivity())
            {
                MainScrollEvent.IsLoading = true;
                switch (TypeContacts)
                {
                    case "Following":
                    {
                        var countList = MAdapter.UserList.Count;
                        var (apiStatus, respond) = await RequestsAsync.Global.GetFriendsAsync(UserId, "following", "10", offset);
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
                                    foreach (var item in result.DataFriends.Following)
                                    {
                                        var check = MAdapter.UserList.FirstOrDefault(a => a.UserId == item.UserId);
                                        switch (check)
                                        {
                                            case null:
                                                MAdapter.UserList.Add(item);
                                                break;
                                            default:
                                                check = item;
                                                check.Name = item.Name;
                                                break;
                                        }
                                    }
                                 
                                    RunOnUiThread(() => { MAdapter.NotifyItemRangeInserted(countList, MAdapter.UserList.Count - countList); });
                                    break;
                                }
                                case > 0:
                                    MAdapter.UserList = new ObservableCollection<UserDataObject>(result.DataFriends.Following);
                                    RunOnUiThread(() => { MAdapter.NotifyDataSetChanged(); });
                                    break;
                                default:
                                {
                                    switch (MAdapter.UserList.Count)
                                    {
                                        case > 10 when !MRecycler.CanScrollVertically(1):
                                            Toast.MakeText(this, GetText(Resource.String.Lbl_No_more_users), ToastLength.Short)?.Show();
                                            break;
                                    }

                                    break;
                                }
                            }
                        }

                        break;
                    }
                    default:
                    {
                        var countList = MAdapter.UserList.Count;
                        var (apiStatus, respond) = await RequestsAsync.Global.GetFriendsAsync(UserId, "followers", "10", "", offset);
                        if (apiStatus != 200 || respond is not GetFriendsObject result || result.DataFriends == null)
                        {
                            MainScrollEvent.IsLoading = false;
                            Methods.DisplayReportResult(this, respond);
                        }
                        else
                        {
                            var respondList = result.DataFriends.Followers.Count;
                            switch (respondList)
                            {
                                case > 0 when countList > 0:
                                {
                                    foreach (var item in result.DataFriends.Followers)
                                    {
                                        var check = MAdapter.UserList.FirstOrDefault(a => a.UserId == item.UserId);
                                        switch (check)
                                        {
                                            case null:
                                                MAdapter.UserList.Add(item);
                                                break;
                                            default:
                                                check = item;
                                                check.Name = item.Name;
                                                break;
                                        }
                                    }
                                 
                                    RunOnUiThread(() => { MAdapter.NotifyItemRangeInserted(countList, MAdapter.UserList.Count - countList); });
                                    break;
                                }
                                case > 0:
                                    MAdapter.UserList = new ObservableCollection<UserDataObject>(result.DataFriends.Followers);
                                    RunOnUiThread(() => { MAdapter.NotifyDataSetChanged(); });
                                    break;
                                default:
                                {
                                    switch (MAdapter.UserList.Count)
                                    {
                                        case > 10 when !MRecycler.CanScrollVertically(1):
                                            Toast.MakeText(this, GetText(Resource.String.Lbl_No_more_users), ToastLength.Short)?.Show();
                                            break;
                                    }

                                    break;
                                }
                            }
                        }

                        break;
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
        }

        private void ShowEmptyPage()
        {
            try
            {
                MainScrollEvent.IsLoading = false; 
                SwipeRefreshLayout.Refreshing = false;

                switch (MAdapter.UserList.Count)
                {
                    case > 0:
                        MRecycler.Visibility = ViewStates.Visible;
                        EmptyStateLayout.Visibility = ViewStates.Gone;
                        break;
                    default:
                    {
                        MRecycler.Visibility = ViewStates.Gone;

                        Inflated ??= EmptyStateLayout.Inflate();

                        EmptyStateInflater x = new EmptyStateInflater();
                        x.InflateLayout(Inflated, EmptyStateInflater.Type.NoUsers);
                        switch (x.EmptyStateButton.HasOnClickListeners)
                        {
                            case false:
                                x.EmptyStateButton.Click += null!;
                                break;
                        }
                        EmptyStateLayout.Visibility = ViewStates.Visible;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                SwipeRefreshLayout.Refreshing = false;
                Methods.DisplayReportResultTrack(e);
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
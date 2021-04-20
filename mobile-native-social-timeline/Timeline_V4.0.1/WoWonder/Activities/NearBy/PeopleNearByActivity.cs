using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;


using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.Gms.Ads;
using Android.Locations;
using AndroidX.AppCompat.Content.Res;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using Google.Android.Material.FloatingActionButton;
using Plugin.Geolocator;
using WoWonder.Activities.Base;
using WoWonder.Activities.NearBy.Adapters;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.User;
using WoWonderClient.Requests;
using AlertDialog = Android.App.AlertDialog;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.NearBy
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class PeopleNearByActivity : BaseActivity
    {
        #region Variables Basic

        public NearByAdapter MAdapter;
        public SwipeRefreshLayout SwipeRefreshLayout;
        public RecyclerView MRecycler;
        private GridLayoutManager LayoutManager;
        public ViewStub EmptyStateLayout;
        private View Inflated;
        public RecyclerViewOnScrollListener MainScrollEvent;
        private FloatingActionButton FloatingActionButtonView;
        private LocationManager LocationManager;
        private bool ShowAlertDialogGps = true; 
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

                InitializeLocationManager();

                LoadNearBy();
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
                ListUtils.ListCachedDataNearby = MAdapter.UserList.Count switch
                {
                    > 0 => MAdapter.UserList,
                    _ => ListUtils.ListCachedDataNearby
                };

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

                FloatingActionButtonView = FindViewById<FloatingActionButton>(Resource.Id.floatingActionButtonView);
                FloatingActionButtonView.Visibility = ViewStates.Visible;

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
                    toolBar.Title = GetText(Resource.String.Lbl_FindFriends);

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
                MAdapter = new NearByAdapter(this)
                {
                    UserList = new ObservableCollection<UserDataObject>()
                };
                LayoutManager = new GridLayoutManager(this, 2);
                MRecycler.SetLayoutManager(LayoutManager);
                MRecycler.AddItemDecoration(new GridSpacingItemDecoration(2, 3, true));
                //var sizeProvider = new FixedPreloadSizeProvider(10, 10);
                //var preLoader = new RecyclerViewPreloader<UserDataObject>(this, MAdapter, sizeProvider, 8);
                //MRecycler.AddOnScrollListener(preLoader);
                //MRecycler.HasFixedSize = true;
                //MRecycler.SetItemViewCacheSize(10);
                //MRecycler.GetLayoutManager().ItemPrefetchEnabled = true;
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
                        MAdapter.FollowButtonItemClick += OnFollowButtonItemClick;
                        SwipeRefreshLayout.Refresh += SwipeRefreshLayoutOnRefresh;
                        FloatingActionButtonView.Click += FloatingActionButtonViewOnClick;
                        break;
                    default:
                        MAdapter.ItemClick -= MAdapterOnItemClick;
                        MAdapter.FollowButtonItemClick -= OnFollowButtonItemClick;
                        SwipeRefreshLayout.Refresh -= SwipeRefreshLayoutOnRefresh;
                        FloatingActionButtonView.Click -= FloatingActionButtonViewOnClick;
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
                FloatingActionButtonView = null!;
                Inflated = null!;
                MainScrollEvent = null!;
                MAdView = null!; 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        private void FloatingActionButtonViewOnClick(object sender, EventArgs e)
        {
            try
            {
                FilterNearByDialogFragment mFragment = new FilterNearByDialogFragment();
                mFragment.Show(SupportFragmentManager, mFragment.Tag);
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

        private void MAdapterOnItemClick(object sender, NearByAdapterClickEventArgs e)
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

        private void OnFollowButtonItemClick(object sender, NearByAdapterClickEventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
                else
                {
                    switch (e.Position)
                    {
                        case > -1:
                        {
                            UserDataObject item = MAdapter.GetItem(e.Position);
                            if (item != null)
                            {
                                WoWonderTools.SetAddFriend(this, item, e.BtnAddUser);
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }


        #endregion

        #region Load NearBy 

        private void LoadNearBy()
        {
            try
            {
                if (MAdapter != null)
                {
                    switch (ListUtils.ListCachedDataNearby.Count)
                    {
                        case > 0:
                            MAdapter.UserList = ListUtils.ListCachedDataNearby;
                            MAdapter.NotifyDataSetChanged();
                            break;
                    }
                }

                GetFilter();

                switch ((int)Build.VERSION.SdkInt)
                {
                    // Check if we're running on Android 5.0 or higher
                    case < 23:
                        StartApiService();
                        break;
                    default:
                    {
                        if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) == Permission.Granted && CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) == Permission.Granted)
                            StartApiService();
                        else
                            new PermissionsController(this).RequestPermission(105);
                        break;
                    }
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void StartApiService(string offset = "0")
        {
            if (!Methods.CheckConnectivity())
                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
            else
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => LoadNearByAsync(offset) });
        }

        private async Task LoadNearByAsync(string offset = "0")
        {
            switch (MainScrollEvent.IsLoading)
            {
                case true:
                    return;
            }

            if (Methods.CheckConnectivity())
            {
                MainScrollEvent.IsLoading = true;

                switch (offset)
                {
                    case "0":
                        await GetPosition();
                        break;
                }

                var dictionary = new Dictionary<string, string>
                {
                    {"limit", "10"},
                    {"offset", offset},
                    {"gender", UserDetails.NearByGender},
                    //{"keyword", ""},
                    {"status", UserDetails.NearByStatus},
                    {"distance", UserDetails.NearByDistanceCount},
                    {"lat", UserDetails.Lat},
                    {"lng", UserDetails.Lng},
                    {"relship", UserDetails.NearByRelationship},
                };

                var countList = MAdapter.UserList.Count;
                var (apiStatus, respond) = await RequestsAsync.Nearby.GetNearbyUsersAsync(dictionary);
                switch (apiStatus)
                {
                    case 200:
                    {
                        switch (respond)
                        {
                            case GetNearbyUsersObject result:
                            {
                                var respondList = result.NearbyUsers.Count;
                                switch (respondList)
                                {
                                    case > 0:
                                    {
                                        foreach (var item in from item in result.NearbyUsers let check = MAdapter.UserList.FirstOrDefault(a => a.UserId == item.UserId) where check == null select item)
                                        {
                                            MAdapter.UserList.Add(item);
                                        }

                                        RunOnUiThread(() => { MAdapter.NotifyItemRangeInserted(countList, MAdapter.UserList.Count - countList); });
                                        break;
                                    }
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

                                break;
                            }
                        }

                        break;
                    }
                    default:
                        Methods.DisplayReportResult(this, respond);
                        break;
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
                        x.InflateLayout(Inflated, EmptyStateInflater.Type.NoNearBy);
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
                MainScrollEvent.IsLoading = false;
                SwipeRefreshLayout.Refreshing = false;
                Methods.DisplayReportResultTrack(e);
            }
        }

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

        #region Location

        private void InitializeLocationManager()
        {
            try
            {
                LocationManager = (LocationManager)GetSystemService(LocationService);
                var criteriaForLocationService = new Criteria
                {
                    Accuracy = Accuracy.Fine
                };
                var acceptableLocationProviders = LocationManager.GetProviders(criteriaForLocationService, true);
                string locationProvider = acceptableLocationProviders.Any() ? acceptableLocationProviders.First() : string.Empty;
                Console.WriteLine(locationProvider);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Get Position GPS Current Location
        private async Task GetPosition()
        {
            try
            {
                await Task.Delay(0);

                switch ((int)Build.VERSION.SdkInt)
                {
                    // Check if we're running on Android 5.0 or higher
                    case < 23:
                        CheckAndGetLocation();
                        break;
                    default:
                    {
                        if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) == Permission.Granted && CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) == Permission.Granted)
                        {
                            CheckAndGetLocation();
                        }
                        else
                        {
                            new PermissionsController(this).RequestPermission(105);
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

        private async void CheckAndGetLocation()
        {
            try
            {
                if (!LocationManager.IsProviderEnabled(LocationManager.GpsProvider))
                {
                    switch (ShowAlertDialogGps)
                    {
                        case true:
                            ShowAlertDialogGps = false;

                            RunOnUiThread(() =>
                            {
                                try
                                {
                                    // Call your Alert message
                                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                                    alert.SetTitle(GetString(Resource.String.Lbl3_Use_Location) + "?");
                                    alert.SetMessage(GetString(Resource.String.Lbl3_GPS_is_disabled) + "?");

                                    alert.SetPositiveButton(GetString(Resource.String.Lbl_Ok), (senderAlert, args) =>
                                    {
                                        //Open intent Gps
                                        new IntentController(this).OpenIntentGps(LocationManager);
                                    });

                                    alert.SetNegativeButton(GetString(Resource.String.Lbl_Cancel), (senderAlert, args) => { });

                                    Dialog gpsDialog = alert.Create();
                                    gpsDialog.Show();
                                }
                                catch (Exception e)
                                {
                                    Methods.DisplayReportResultTrack(e);
                                }
                            });
                            break;
                    }
                }
                else
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;
                    var position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(10000));
                    Console.WriteLine("Position Status: {0}", position.Timestamp);
                    Console.WriteLine("Position Latitude: {0}", position.Latitude);
                    Console.WriteLine("Position Longitude: {0}", position.Longitude);

                    UserDetails.Lat = position.Latitude.ToString(CultureInfo.InvariantCulture);
                    UserDetails.Lng = position.Longitude.ToString(CultureInfo.InvariantCulture);

                    var dd = locator.StopListeningAsync();
                    Console.WriteLine(dd);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Permissions 
         
        //Permissions
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            try
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

                switch (requestCode)
                {
                    case 105 when grantResults.Length > 0 && grantResults[0] == Permission.Granted:
                        StartApiService();
                        break;
                    case 105:
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Permission_is_denied), ToastLength.Long)?.Show();
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        #endregion

        private void GetFilter()
        {
            try
            {
                var dbDatabase = new SqLiteDatabase();

                var data = dbDatabase.GetNearByFilterById();
                if (data != null)
                {
                    UserDetails.NearByGender = data.Gender;
                    UserDetails.NearByDistanceCount = data.DistanceValue.ToString();
                    UserDetails.NearByStatus = data.Status.ToString();
                    UserDetails.NearByRelationship = data.Relationship;
                }
                else
                {
                    UserDetails.NearByDistanceCount = "0";
                    UserDetails.NearByGender = "all"; 
                    UserDetails.NearByStatus = "2";
                    UserDetails.NearByRelationship = "5";
                }
                
                
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

    }
}
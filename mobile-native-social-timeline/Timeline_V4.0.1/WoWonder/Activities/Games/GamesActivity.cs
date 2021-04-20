using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;


using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Tabs;
using WoWonder.Activities.Base;
using WoWonder.Activities.Games.Fragment;
using WoWonder.Adapters;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Games;
using WoWonderClient.Requests;
using SearchView = AndroidX.AppCompat.Widget.SearchView;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Games
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class GamesActivity : BaseActivity, TabLayoutMediator.ITabConfigurationStrategy
    {
        #region Variables Basic

        private MainTabAdapter Adapter;
        private ViewPager2 ViewPager;
        private GamesFragment GamesTab;
        private MyGamesFragment MyGamesTab;
        private TabLayout TabLayout;
        private FloatingActionButton FloatingActionButtonView;
        private SearchView SearchView;
        private Toolbar ToolBar;
        public string SearchKey;

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
                SetContentView(Resource.Layout.EventMain_Layout);

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();

                LoadDataApi();
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
                AddOrRemoveGames(true);
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
                AddOrRemoveGames(false);
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
                ListUtils.ListCachedDataGames = GamesTab.MAdapter.GamesList.Count switch
                {
                    > 0 => GamesTab.MAdapter.GamesList,
                    _ => ListUtils.ListCachedDataGames
                };

                ListUtils.ListCachedDataMyGames = MyGamesTab.MAdapter.GamesList.Count switch
                {
                    > 0 => MyGamesTab.MAdapter.GamesList,
                    _ => ListUtils.ListCachedDataMyGames
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.SearchGif_Menu, menu);
            WoWonderTools.ChangeMenuIconColor(menu, Color.ParseColor("#888888"));
            try
            {
                var item = menu.FindItem(Resource.Id.searchUserBar);
                SearchView searchItem = (SearchView)item.ActionView;

                SearchView = searchItem.JavaCast<SearchView>();
                SearchView.SetQuery("", false);
                SearchView.SetIconifiedByDefault(false);
                SearchView.OnActionViewExpanded();
                SearchView.Iconified = false;
                SearchView.QueryTextChange += SearchViewOnQueryTextChange;
                SearchView.QueryTextSubmit += SearchViewOnQueryTextSubmit;
                SearchView.ClearFocus();

                //Change text colors
                var editText = (EditText)SearchView.FindViewById(Resource.Id.search_src_text);
                editText.SetHintTextColor(Color.Black);
                editText.SetTextColor(Color.ParseColor("#888888")); 

                //Change Color Icon Search
                ImageView searchViewIcon = (ImageView)SearchView.FindViewById(Resource.Id.search_mag_icon);
                searchViewIcon.SetColorFilter(Color.ParseColor(AppSettings.MainColor));

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
             
            return base.OnCreateOptionsMenu(menu);
        }

        private void SearchViewOnQueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            try
            {
                SearchKey = e.NewText;

                GamesTab.MAdapter.GamesList.Clear();
                GamesTab.MAdapter.NotifyDataSetChanged();

                GamesTab.SwipeRefreshLayout.Refreshing = true;

                if (!Methods.CheckConnectivity())
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                else
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => SearchGames() });

                //Hide keyboard programmatically in MonoDroid
                e.Handled = true;

                SearchView.ClearFocus();

                var inputManager = (InputMethodManager)GetSystemService(InputMethodService);
                inputManager?.HideSoftInputFromWindow(ToolBar.WindowToken, 0);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void SearchViewOnQueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            try
            {
                SearchKey = e.NewText;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Functions

        private void InitComponent()
        {
            try
            {
                ViewPager = FindViewById<ViewPager2>(Resource.Id.viewpager);
                TabLayout = FindViewById<TabLayout>(Resource.Id.tabs);

                FloatingActionButtonView = FindViewById<FloatingActionButton>(Resource.Id.floatingActionButtonView);
                FloatingActionButtonView.Visibility = ViewStates.Gone;

                ViewPager.OffscreenPageLimit = 2;
                SetUpViewPager(ViewPager);
                new TabLayoutMediator(TabLayout, ViewPager, this).Attach();
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
                ToolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
                if (ToolBar != null)
                {
                    ToolBar.Title = GetText(Resource.String.Lbl_Games);
                    ToolBar.SetTitleTextColor(Color.ParseColor(AppSettings.MainColor));
                    SetSupportActionBar(ToolBar);
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

        private void AddOrRemoveGames(bool addGames)
        {
            try
            {
                switch (addGames)
                {
                    // true +=  // false -=
                    case true:
                        break;
                    default:
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
                ViewPager = null!;
                TabLayout = null!;
                FloatingActionButtonView = null!;
                ToolBar = null!;
                SearchKey = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Set Tap

        private void SetUpViewPager(ViewPager2 viewPager)
        {
            try
            {
                GamesTab = new GamesFragment();
                MyGamesTab = new MyGamesFragment();

                Adapter = new MainTabAdapter(this);
                Adapter.AddFragment(GamesTab, GetText(Resource.String.Lbl_Games));
                Adapter.AddFragment(MyGamesTab, GetText(Resource.String.Lbl_MyGames));

                viewPager.CurrentItem = Adapter.ItemCount;
                viewPager.OffscreenPageLimit = Adapter.ItemCount;

                viewPager.Orientation = ViewPager2.OrientationHorizontal;
                viewPager.Adapter = Adapter;
                viewPager.Adapter.NotifyDataSetChanged();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void OnConfigureTab(TabLayout.Tab tab, int position)
        {
            try
            {
                tab.SetText(Adapter.GetFragment(position));
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Get Games Api 

        private void LoadDataApi()
        {
            try
            {
                string offsetGames = "0", offsetMyGames = "0";

                if (GamesTab.MAdapter != null && ListUtils.ListCachedDataGames.Count > 0)
                {
                    GamesTab.MAdapter.GamesList = ListUtils.ListCachedDataGames;
                    GamesTab.MAdapter.NotifyDataSetChanged();

                    var item = GamesTab.MAdapter.GamesList.LastOrDefault();
                    if (item != null && !string.IsNullOrEmpty(item.Id))
                        offsetGames = item.Id;
                }

                if (MyGamesTab.MAdapter != null && ListUtils.ListCachedDataMyGames.Count > 0)
                {
                    MyGamesTab.MAdapter.GamesList = ListUtils.ListCachedDataMyGames;
                    MyGamesTab.MAdapter.NotifyDataSetChanged();

                    var item = MyGamesTab.MAdapter.GamesList.LastOrDefault();
                    if (item != null && !string.IsNullOrEmpty(item.Id))
                        offsetMyGames = item.Id;
                }

                StartApiService(offsetGames, offsetMyGames);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void StartApiService(string offsetGames = "0", string offsetMyGames = "0")
        {
            if (Methods.CheckConnectivity())
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => GetGames(offsetGames), () => GetMyGames(offsetMyGames) });
            else
                Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
        }

        public async Task GetGames(string offset = "0")
        {
            switch (GamesTab.MainScrollEvent.IsLoading)
            {
                case true:
                    return;
            }

            if (Methods.CheckConnectivity())
            {
                GamesTab.MainScrollEvent.IsLoading = true;
                var countList = GamesTab.MAdapter.GamesList.Count;

                var (respondCode, respondString) = await RequestsAsync.Games.FetchGamesAsync("6", offset);
                switch (respondCode)
                {
                    case 200:
                    {
                        switch (respondString)
                        {
                            case FetchGamesObject result:
                            {
                                var respondList = result.Data.Count;
                                switch (respondList)
                                {
                                    case > 0 when countList > 0:
                                    {
                                        foreach (var item in from item in result.Data let check = GamesTab.MAdapter.GamesList.FirstOrDefault(a => a.Id == item.Id) where check == null select item)
                                        {
                                            GamesTab.MAdapter.GamesList.Add(item);
                                        }

                                        RunOnUiThread(() => { GamesTab.MAdapter.NotifyItemRangeInserted(countList, GamesTab.MAdapter.GamesList.Count - countList); });
                                        break;
                                    }
                                    case > 0:
                                        GamesTab.MAdapter.GamesList = new ObservableCollection<GamesDataObject>(result.Data);
                                        RunOnUiThread(() => { GamesTab.MAdapter.NotifyDataSetChanged(); });
                                        break;
                                    default:
                                    {
                                        switch (GamesTab.MAdapter.GamesList.Count)
                                        {
                                            case > 10 when !GamesTab.MRecycler.CanScrollVertically(1):
                                                Toast.MakeText(this, GetText(Resource.String.Lbl_NoMoreGames), ToastLength.Short)?.Show();
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
                        Methods.DisplayReportResult(this, respondString);
                        break;
                }

                RunOnUiThread(() => ShowEmptyPage("GetGames"));
            }
            else
            {
                GamesTab.Inflated = GamesTab.EmptyStateLayout.Inflate();
                EmptyStateInflater x = new EmptyStateInflater();
                x.InflateLayout(GamesTab.Inflated, EmptyStateInflater.Type.NoConnection);
                switch (x.EmptyStateButton.HasOnClickListeners)
                {
                    case false:
                        x.EmptyStateButton.Click += null!;
                        x.EmptyStateButton.Click += EmptyStateButtonOnClick;
                        break;
                }

                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                GamesTab.MainScrollEvent.IsLoading = false;
            }
        }

        public async Task GetMyGames(string offset = "0")
        {
            switch (MyGamesTab.MainScrollEvent.IsLoading)
            {
                case true:
                    return;
            }

            if (Methods.CheckConnectivity())
            {
                MyGamesTab.MainScrollEvent.IsLoading = true;
                var countList = MyGamesTab.MAdapter.GamesList.Count;

                var (respondCode, respondString) = await RequestsAsync.Games.FetchMyhGamesAsync("6", offset);
                switch (respondCode)
                {
                    case 200:
                    {
                        switch (respondString)
                        {
                            case FetchGamesObject result:
                            {
                                var respondList = result.Data.Count;
                                switch (respondList)
                                {
                                    case > 0 when countList > 0:
                                    {
                                        foreach (var item in from item in result.Data let check = MyGamesTab.MAdapter.GamesList.FirstOrDefault(a => a.Id == item.Id) where check == null select item)
                                        {
                                            MyGamesTab.MAdapter.GamesList.Add(item);
                                        }

                                        RunOnUiThread(() => { MyGamesTab.MAdapter.NotifyItemRangeInserted(countList, MyGamesTab.MAdapter.GamesList.Count - countList); });
                                        break;
                                    }
                                    case > 0:
                                        MyGamesTab.MAdapter.GamesList = new ObservableCollection<GamesDataObject>(result.Data);
                                        RunOnUiThread(() => { MyGamesTab.MAdapter.NotifyDataSetChanged(); });
                                        break;
                                    default:
                                    {
                                        switch (MyGamesTab.MAdapter.GamesList.Count)
                                        {
                                            case > 10 when !MyGamesTab.MRecycler.CanScrollVertically(1):
                                                Toast.MakeText(this, GetText(Resource.String.Lbl_NoMoreGames), ToastLength.Short)?.Show();
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
                        Methods.DisplayReportResult(this, respondString);
                        break;
                }

                RunOnUiThread(()=> ShowEmptyPage("GetMyGames"));
            }
            else
            {
                MyGamesTab.Inflated = MyGamesTab.EmptyStateLayout.Inflate();
                EmptyStateInflater x = new EmptyStateInflater();
                x.InflateLayout(MyGamesTab.Inflated, EmptyStateInflater.Type.NoConnection);
                switch (x.EmptyStateButton.HasOnClickListeners)
                {
                    case false:
                        x.EmptyStateButton.Click += null!;
                        x.EmptyStateButton.Click += EmptyStateButtonOnClick;
                        break;
                }

                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                MyGamesTab.MainScrollEvent.IsLoading = false;
            }
        }

        public async Task SearchGames(string offset = "0")
        {
            switch (GamesTab.MainScrollEvent.IsLoading)
            {
                case true:
                    return;
            }

            if (Methods.CheckConnectivity())
            {
                GamesTab.MainScrollEvent.IsLoading = true;
                var countList = GamesTab.MAdapter.GamesList.Count;

                var (respondCode, respondString) = await RequestsAsync.Games.SearchGamesAsync(SearchKey,"15", offset);
                switch (respondCode)
                {
                    case 200:
                    {
                        switch (respondString)
                        {
                            case FetchGamesObject result:
                            {
                                var respondList = result.Data.Count;
                                switch (respondList)
                                {
                                    case > 0 when countList > 0:
                                    {
                                        foreach (var item in from item in result.Data let check = GamesTab.MAdapter.GamesList.FirstOrDefault(a => a.Id == item.Id) where check == null select item)
                                        {
                                            GamesTab.MAdapter.GamesList.Add(item);
                                        }

                                        RunOnUiThread(() => { GamesTab.MAdapter.NotifyItemRangeInserted(countList, GamesTab.MAdapter.GamesList.Count - countList); });
                                        break;
                                    }
                                    case > 0:
                                        GamesTab.MAdapter.GamesList = new ObservableCollection<GamesDataObject>(result.Data);
                                        RunOnUiThread(() => { GamesTab.MAdapter.NotifyDataSetChanged(); });
                                        break;
                                    default:
                                    {
                                        switch (GamesTab.MAdapter.GamesList.Count)
                                        {
                                            case > 10 when !GamesTab.MRecycler.CanScrollVertically(1):
                                                Toast.MakeText(this, GetText(Resource.String.Lbl_NoMoreGames), ToastLength.Short)?.Show();
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
                        Methods.DisplayReportResult(this, respondString);
                        break;
                }

                RunOnUiThread(() => ShowEmptyPage("GetGames"));
            }
            else
            {
                GamesTab.Inflated = GamesTab.EmptyStateLayout.Inflate();
                EmptyStateInflater x = new EmptyStateInflater();
                x.InflateLayout(GamesTab.Inflated, EmptyStateInflater.Type.NoConnection);
                switch (x.EmptyStateButton.HasOnClickListeners)
                {
                    case false:
                        x.EmptyStateButton.Click += null!;
                        x.EmptyStateButton.Click += EmptyStateButtonOnClick;
                        break;
                }

                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                GamesTab.MainScrollEvent.IsLoading = false;
            }
        }

        private void ShowEmptyPage(string type)
        {
            try
            {
                switch (type)
                {
                    case "GetGames":
                    {
                        GamesTab.MainScrollEvent.IsLoading = false;
                        GamesTab.SwipeRefreshLayout.Refreshing = false;

                        switch (GamesTab.MAdapter.GamesList.Count)
                        {
                            case > 0:
                                GamesTab.MRecycler.Visibility = ViewStates.Visible;
                                GamesTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                                break;
                            default:
                            {
                                GamesTab.MRecycler.Visibility = ViewStates.Gone;

                                GamesTab.Inflated = GamesTab.Inflated switch
                                {
                                    null => GamesTab.EmptyStateLayout.Inflate(),
                                    _ => GamesTab.Inflated
                                };

                                EmptyStateInflater x = new EmptyStateInflater();
                                x.InflateLayout(GamesTab.Inflated, EmptyStateInflater.Type.NoGames);
                                switch (x.EmptyStateButton.HasOnClickListeners)
                                {
                                    case false:
                                        x.EmptyStateButton.Click += null!;
                                        break;
                                }
                                GamesTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                                break;
                            }
                        }

                        break;
                    }
                    case "GetMyGames":
                    {
                        MyGamesTab.MainScrollEvent.IsLoading = false;
                        MyGamesTab.SwipeRefreshLayout.Refreshing = false;

                        switch (MyGamesTab.MAdapter.GamesList.Count)
                        {
                            case > 0:
                                MyGamesTab.MRecycler.Visibility = ViewStates.Visible;
                                MyGamesTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                                break;
                            default:
                            {
                                MyGamesTab.MRecycler.Visibility = ViewStates.Gone;

                                MyGamesTab.Inflated = MyGamesTab.Inflated switch
                                {
                                    null => MyGamesTab.EmptyStateLayout.Inflate(),
                                    _ => MyGamesTab.Inflated
                                };

                                EmptyStateInflater x = new EmptyStateInflater();
                                x.InflateLayout(MyGamesTab.Inflated, EmptyStateInflater.Type.NoGames);
                                switch (x.EmptyStateButton.HasOnClickListeners)
                                {
                                    case false:
                                        x.EmptyStateButton.Click += null!;
                                        break;
                                }
                                MyGamesTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                                break;
                            }
                        }

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                GamesTab.MainScrollEvent.IsLoading = false;
                GamesTab.SwipeRefreshLayout.Refreshing = false;
                MyGamesTab.MainScrollEvent.IsLoading = false;
                MyGamesTab.SwipeRefreshLayout.Refreshing = false;
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
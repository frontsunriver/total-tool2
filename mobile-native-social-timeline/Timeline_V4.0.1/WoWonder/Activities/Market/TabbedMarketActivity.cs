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
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Tabs;
using Newtonsoft.Json;
using WoWonder.Activities.Base;
using WoWonder.Activities.Market.Fragment;
using WoWonder.Activities.NearbyShops;
using WoWonder.Adapters;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Product;
using WoWonderClient.Requests;
using SearchView = AndroidX.AppCompat.Widget.SearchView;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Market
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class TabbedMarketActivity : BaseActivity, TabLayoutMediator.ITabConfigurationStrategy
    {
        #region Variables Basic

        private MainTabAdapter Adapter;
        private ViewPager2 ViewPager;
        public MarketFragment MarketTab;
        public MyProductsFragment MyProductsTab;
        private TabLayout TabLayout;
        private FloatingActionButton FloatingActionButtonView;
        private RecyclerView CatRecyclerView;
        private CategoriesAdapter CategoriesAdapter;
        private SearchView SearchBox;
        private TextView FilterButton;
        private ImageView DiscoverButton;
        private string KeySearch = "";
        private static TabbedMarketActivity Instance;

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
                SetContentView(Resource.Layout.MarketMain_Layout);

                Instance = this;

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();

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
                ListUtils.ListCachedDataProduct = MarketTab.MAdapter.MarketList.Count switch
                {
                    > 0 => MarketTab.MAdapter.MarketList,
                    _ => ListUtils.ListCachedDataProduct
                };

                ListUtils.ListCachedDataMyProduct = MyProductsTab.MAdapter.MarketList.Count switch
                {
                    > 0 => MyProductsTab.MAdapter.MarketList,
                    _ => ListUtils.ListCachedDataMyProduct
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
         
        private void SearchViewOnQueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            try
            {
                KeySearch = e.NewText;

                MarketTab.MAdapter.MarketList.Clear();
                MarketTab.MAdapter.NotifyDataSetChanged();

                MarketTab.SwipeRefreshLayout.Refreshing = true;
                MarketTab.SwipeRefreshLayout.Enabled = true;
                 
                if (Methods.CheckConnectivity())
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => GetMarketByKey(KeySearch) });
                else
                    Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
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
                KeySearch = e.NewText;
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

                ViewPager.OffscreenPageLimit = 2;
                SetUpViewPager(ViewPager);
                new TabLayoutMediator(TabLayout, ViewPager, this).Attach();

                CatRecyclerView = FindViewById<RecyclerView>(Resource.Id.catRecyler);

                DiscoverButton = (ImageView)FindViewById(Resource.Id.discoverButton);
                DiscoverButton.Visibility = AppSettings.ShowNearbyShops switch
                {
                    false => ViewStates.Gone,
                    _ => DiscoverButton.Visibility
                };

                FilterButton = (TextView)FindViewById(Resource.Id.filter_icon);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, FilterButton, IonIconsFonts.Options);
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
                    toolBar.Title = " ";
                    toolBar.SetTitleTextColor(Color.ParseColor(AppSettings.MainColor));
                    SetSupportActionBar(toolBar);
                    SupportActionBar.SetDisplayShowCustomEnabled(true);
                    SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                    SupportActionBar.SetHomeButtonEnabled(true);
                    SupportActionBar.SetDisplayShowHomeEnabled(true);

                    
                }
                
                SearchBox = FindViewById<SearchView>(Resource.Id.searchBox);
                SearchBox.SetQuery("", false);
                SearchBox.SetIconifiedByDefault(false);
                SearchBox.OnActionViewExpanded();
                SearchBox.Iconified = false;
                SearchBox.QueryTextChange += SearchViewOnQueryTextChange;
                SearchBox.QueryTextSubmit += SearchViewOnQueryTextSubmit;
                SearchBox.ClearFocus();

                //Change text colors
                var editText = (EditText)SearchBox.FindViewById(Resource.Id.search_src_text);
                editText.SetHintTextColor(Color.White);
                editText.SetTextColor(Color.White);

                //Remove Icon Search
                ImageView searchViewIcon = (ImageView)SearchBox.FindViewById(Resource.Id.search_mag_icon);
                ViewGroup linearLayoutSearchView = (ViewGroup)searchViewIcon.Parent;
                linearLayoutSearchView.RemoveView(searchViewIcon);
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
                switch (CategoriesController.ListCategoriesProducts.Count)
                {
                    case > 0:
                    {
                        var check = CategoriesController.ListCategoriesProducts.Where(a => a.CategoriesColor == AppSettings.MainColor).ToList();
                        switch (check.Count)
                        {
                            case > 0:
                            {
                                foreach (var all in check)
                                    all.CategoriesColor = "#ffffff";
                                break;
                            }
                        }
                     
                        CatRecyclerView.HasFixedSize = true;
                        CatRecyclerView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false));
                        CategoriesAdapter = new CategoriesAdapter(this)
                        {
                            MCategoriesList = CategoriesController.ListCategoriesProducts,
                        };
                        CatRecyclerView.SetAdapter(CategoriesAdapter);
                        CatRecyclerView.NestedScrollingEnabled = false;
                        CategoriesAdapter.NotifyDataSetChanged();
                        CatRecyclerView.Visibility = ViewStates.Visible;
                        CategoriesAdapter.ItemClick += CategoriesAdapterOnItemClick;
                        break;
                    }
                    default:
                        CatRecyclerView.Visibility = ViewStates.Gone;
                        break;
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
                        FloatingActionButtonView.Click += CreateProductOnClick;
                        FilterButton.Click += FilterButtonOnClick;
                        DiscoverButton.Click += DiscoverButtonOnClick;
                        break;
                    default:
                        FloatingActionButtonView.Click -= CreateProductOnClick;
                        FilterButton.Click -= FilterButtonOnClick;
                        DiscoverButton.Click -= DiscoverButtonOnClick;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static TabbedMarketActivity GetInstance()
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
                ViewPager= null!;
                TabLayout = null!;
                MarketTab = null!;
                MyProductsTab = null!;
                FloatingActionButtonView = null!;
                CatRecyclerView = null!;
                CategoriesAdapter = null!;
                DiscoverButton = null!;
                FilterButton = null!;
                KeySearch = null!;
                Instance = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events

        private void FilterButtonOnClick(object sender, EventArgs e)
        {
            try
            { 
                FilterMarketDialogFragment mFragment = new FilterMarketDialogFragment();

                Bundle bundle = new Bundle();
                bundle.PutString("TypeFilter", "Market");

                mFragment.Arguments = bundle;

                mFragment.Show(SupportFragmentManager, mFragment.Tag);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void CreateProductOnClick(object sender, EventArgs e)
        {
            try
            {
                StartActivityForResult(new Intent(this, typeof(CreateProductActivity)), 200);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void CategoriesAdapterOnItemClick(object sender, CategoriesAdapterClickEventArgs e)
        {
            try
            {
                KeySearch = "";

                MarketTab.MAdapter.MarketList.Clear();
                MarketTab.MAdapter.NotifyDataSetChanged();
                 
                var item = CategoriesAdapter.GetItem(e.Position);
                if (item != null)
                {
                    var check = CategoriesAdapter.MCategoriesList.Where(a => a.CategoriesColor == AppSettings.MainColor).ToList();
                    switch (check.Count)
                    {
                        case > 0:
                        {
                            foreach (var all in check)
                                all.CategoriesColor = "#ffffff";
                            break;
                        }
                    }

                    var click = CategoriesAdapter.MCategoriesList.FirstOrDefault(a => a.CategoriesId == item.CategoriesId);
                    if (click != null) click.CategoriesColor = AppSettings.MainColor;

                    CategoriesAdapter.NotifyDataSetChanged();

                    MarketTab.SwipeRefreshLayout.Refreshing = true;
                    MarketTab.SwipeRefreshLayout.Enabled = true;
                     
                    if (Methods.CheckConnectivity())
                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => GetMarketByKey(KeySearch, item.CategoriesId) });
                    else
                        Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
                } 
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Nearby Shops
        private void DiscoverButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                StartActivity(new Intent(this, typeof(NearbyShopsActivity)));
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
                    case 200:
                    {
                        switch (resultCode)
                        {
                            case Result.Ok:
                            {
                                if (MarketTab != null)
                                {
                                    var result = data.GetStringExtra("product");

                                    var item = JsonConvert.DeserializeObject<ProductDataObject>(result);

                                    MarketTab.MAdapter.MarketList.Insert(0, item);
                                    MarketTab.MAdapter.NotifyItemInserted(0);
                                }

                                break;
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
         
        #region Set Tab

        private void SetUpViewPager(ViewPager2 viewPager)
        {
            try
            {
                MyProductsTab = new MyProductsFragment();
                MarketTab = new MarketFragment();

                Adapter = new MainTabAdapter(this);
                Adapter.AddFragment(MarketTab, GetText(Resource.String.Lbl_Market));
                Adapter.AddFragment(MyProductsTab, GetText(Resource.String.Lbl_MyProducts));

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

        #endregion Set Tab

        #region Get Market Api 

        private void LoadDataApi()
        {
            try
            {
                string offsetMarket = "0", offsetMyProducts = "0";

                if (MarketTab.MAdapter != null && ListUtils.ListCachedDataProduct.Count > 0)
                {
                    MarketTab.MAdapter.MarketList = ListUtils.ListCachedDataProduct;
                    MarketTab.MAdapter.NotifyDataSetChanged();

                    var item = MarketTab.MAdapter.MarketList.LastOrDefault();
                    if (item != null && !string.IsNullOrEmpty(item.Id))
                        offsetMarket = item.Id;
                }

                if (MyProductsTab.MAdapter != null && ListUtils.ListCachedDataMyProduct.Count > 0)
                {
                    MyProductsTab.MAdapter.MarketList = ListUtils.ListCachedDataMyProduct;
                    MyProductsTab.MAdapter.NotifyDataSetChanged();

                    var item = MyProductsTab.MAdapter.MarketList.LastOrDefault();
                    if (item != null && !string.IsNullOrEmpty(item.Id))
                        offsetMyProducts = item.Id;
                }
                  
                StartApiService(offsetMarket, offsetMyProducts);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        private void StartApiService(string offsetMarket = "0" , string offsetMyProducts = "0")
        {
            if (Methods.CheckConnectivity())
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => GetMarket(offsetMarket), () => GetMyProducts(offsetMyProducts) });
            else
                Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
        }

        public async Task GetMarket(string offset = "0")
        {
            switch (MarketTab.MainScrollEvent.IsLoading)
            {
                case true:
                    return;
            }

            if (Methods.CheckConnectivity())
            {
                MarketTab.MainScrollEvent.IsLoading = true;

                var countList = MarketTab.MAdapter.MarketList.Count;
                var (apiStatus, respond) = await RequestsAsync.Market.GetProductsAsync("", "10", offset,"","", UserDetails.MarketDistanceCount);
                switch (apiStatus)
                {
                    case 200:
                    {
                        switch (respond)
                        {
                            case GetProductsObject result:
                            {
                                var respondList = result.Products.Count;
                                switch (respondList)
                                {
                                    case > 0 when countList > 0:
                                    {
                                        foreach (var item in from item in result.Products let check = MarketTab.MAdapter.MarketList.FirstOrDefault(a => a.Id == item.Id) where check == null select item)
                                        {
                                            MarketTab.MAdapter.MarketList.Add(item);
                                        }

                                        RunOnUiThread(() => { MarketTab.MAdapter.NotifyItemRangeInserted(countList , MarketTab.MAdapter.MarketList.Count - countList); });
                                        break;
                                    }
                                    case > 0:
                                        MarketTab.MAdapter.MarketList = new ObservableCollection<ProductDataObject>(result.Products);
                                        RunOnUiThread(() => { MarketTab.MAdapter.NotifyDataSetChanged(); });
                                        break;
                                    default:
                                    {
                                        switch (MarketTab.MAdapter.MarketList.Count)
                                        {
                                            case > 10 when !MarketTab.MRecycler.CanScrollVertically(1):
                                                Toast.MakeText(this, GetText(Resource.String.Lbl_NoMoreProducts), ToastLength.Short)?.Show();
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

                RunOnUiThread(() => { ShowEmptyPage("GetMarket");}); 
            }
            else
            {
                MarketTab.Inflated = MarketTab.EmptyStateLayout.Inflate();
                EmptyStateInflater x = new EmptyStateInflater();
                x.InflateLayout(MarketTab.Inflated, EmptyStateInflater.Type.NoConnection);
                switch (x.EmptyStateButton.HasOnClickListeners)
                {
                    case false:
                        x.EmptyStateButton.Click += null!;
                        x.EmptyStateButton.Click += EmptyStateButtonOnClick;
                        break;
                }

                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                MarketTab.MainScrollEvent.IsLoading = false;
            }
        }

        public async Task GetMyProducts(string offset = "0")
        {
            switch (MyProductsTab.MainScrollEvent.IsLoading)
            {
                case true:
                    return;
            }

            MyProductsTab.MainScrollEvent.IsLoading = true;
            var countList = MyProductsTab.MAdapter.MarketList.Count;
            var (apiStatus, respond) = await RequestsAsync.Market.GetProductsAsync(UserDetails.UserId, "10", offset);
            switch (apiStatus)
            {
                case 200:
                {
                    switch (respond)
                    {
                        case GetProductsObject result:
                        {
                            var respondList = result.Products.Count;
                            switch (respondList)
                            {
                                case > 0 when countList > 0:
                                {
                                    foreach (var item in from item in result.Products let check = MyProductsTab.MAdapter.MarketList.FirstOrDefault(a => a.Id == item.Id) where check == null select item)
                                    {
                                        MyProductsTab.MAdapter.MarketList.Add(item);
                                    }

                                    RunOnUiThread(() => { MyProductsTab.MAdapter.NotifyItemRangeInserted(countList, MyProductsTab.MAdapter.MarketList.Count - countList); });
                                    break;
                                }
                                case > 0:
                                    MyProductsTab.MAdapter.MarketList = new ObservableCollection<ProductDataObject>(result.Products);
                                    RunOnUiThread(() => { MyProductsTab.MAdapter.NotifyDataSetChanged(); });
                                    break;
                                default:
                                {
                                    switch (MyProductsTab.MAdapter.MarketList.Count)
                                    {
                                        case > 10 when !MyProductsTab.MRecycler.CanScrollVertically(1):
                                            Toast.MakeText(this, GetText(Resource.String.Lbl_NoMoreProducts), ToastLength.Short)?.Show();
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

            RunOnUiThread(() => { ShowEmptyPage("GetMyProducts"); }); 
        }

        private void ShowEmptyPage(string type)
        {
            try
            {
                switch (type)
                {
                    case "GetMarket":
                    {
                        MarketTab.MainScrollEvent.IsLoading = false;
                        MarketTab.SwipeRefreshLayout.Refreshing = false;

                        switch (MarketTab.MAdapter.MarketList.Count)
                        {
                            case > 0:
                                MarketTab.MRecycler.Visibility = ViewStates.Visible;
                                MarketTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                                break;
                            default:
                            {
                                MarketTab.MRecycler.Visibility = ViewStates.Gone;

                                MarketTab.Inflated = MarketTab.Inflated switch
                                {
                                    null => MarketTab.EmptyStateLayout.Inflate(),
                                    _ => MarketTab.Inflated
                                };

                                EmptyStateInflater x = new EmptyStateInflater();
                                x.InflateLayout(MarketTab.Inflated, EmptyStateInflater.Type.NoProduct);
                                switch (x.EmptyStateButton.HasOnClickListeners)
                                {
                                    case false:
                                        x.EmptyStateButton.Click += null!;
                                        x.EmptyStateButton.Click += BtnCreateProductsOnClick;
                                        break;
                                }
                                MarketTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                                break;
                            }
                        }

                        break;
                    }
                    case "GetMyProducts":
                    {
                        MyProductsTab.MainScrollEvent.IsLoading = false;
                        MyProductsTab.SwipeRefreshLayout.Refreshing = false;

                        switch (MyProductsTab.MAdapter.MarketList.Count)
                        {
                            case > 0:
                                MyProductsTab.MRecycler.Visibility = ViewStates.Visible;
                                MyProductsTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                                break;
                            default:
                            {
                                MyProductsTab.MRecycler.Visibility = ViewStates.Gone;

                                MyProductsTab.Inflated = MyProductsTab.Inflated switch
                                {
                                    null => MyProductsTab.EmptyStateLayout.Inflate(),
                                    _ => MyProductsTab.Inflated
                                };

                                EmptyStateInflater x = new EmptyStateInflater();
                                x.InflateLayout(MyProductsTab.Inflated, EmptyStateInflater.Type.NoProduct);
                                switch (x.EmptyStateButton.HasOnClickListeners)
                                {
                                    case false:
                                        x.EmptyStateButton.Click += null!;
                                        x.EmptyStateButton.Click += BtnCreateProductsOnClick;
                                        break;
                                }
                                MyProductsTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                                break;
                            }
                        }

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                MarketTab.MainScrollEvent.IsLoading = false;
                MarketTab.SwipeRefreshLayout.Refreshing = false;
                MyProductsTab.MainScrollEvent.IsLoading = false;
                MyProductsTab.SwipeRefreshLayout.Refreshing = false;
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Event Add New Product  >> CreateProduct_Activity
        private void BtnCreateProductsOnClick(object sender, EventArgs e)
        {
            try
            {
                var intent = new Intent(this, typeof(CreateProductActivity));
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

        private async Task GetMarketByKey(string key = "", string categoriesId = "")
        {
            switch (MarketTab.MainScrollEvent.IsLoading)
            {
                case true:
                    return;
            }

            if (Methods.CheckConnectivity())
            {
                MarketTab.MainScrollEvent.IsLoading = true;
                var countList = MarketTab.MAdapter.MarketList.Count;
                var (apiStatus, respond) = await RequestsAsync.Market.GetProductsAsync("", "10", "0", categoriesId, key, UserDetails.MarketDistanceCount);
                switch (apiStatus)
                {
                    case 200:
                    {
                        switch (respond)
                        {
                            case GetProductsObject result:
                            {
                                var respondList = result.Products.Count;
                                switch (respondList)
                                {
                                    case > 0 when countList > 0:
                                    {
                                        foreach (var item in from item in result.Products let check = MarketTab.MAdapter.MarketList.FirstOrDefault(a => a.Id == item.Id) where check == null select item)
                                        {
                                            MarketTab.MAdapter.MarketList.Add(item);
                                        }

                                        RunOnUiThread(() => { MarketTab.MAdapter.NotifyItemRangeInserted(countList, MarketTab.MAdapter.MarketList.Count - countList); });
                                        break;
                                    }
                                    case > 0:
                                        MarketTab.MAdapter.MarketList = new ObservableCollection<ProductDataObject>(result.Products);
                                        RunOnUiThread(() => { MarketTab.MAdapter.NotifyDataSetChanged(); });
                                        break;
                                    default:
                                    {
                                        switch (MarketTab.MAdapter.MarketList.Count)
                                        {
                                            case > 10 when !MarketTab.MRecycler.CanScrollVertically(1):
                                                Toast.MakeText(this, GetText(Resource.String.Lbl_NoMoreProducts), ToastLength.Short)?.Show();
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

                RunOnUiThread(() => { ShowEmptyPage("GetMarket"); });
            }
            else
            {
                MarketTab.Inflated = MarketTab.EmptyStateLayout.Inflate();
                EmptyStateInflater x = new EmptyStateInflater();
                x.InflateLayout(MarketTab.Inflated, EmptyStateInflater.Type.NoConnection);
                switch (x.EmptyStateButton.HasOnClickListeners)
                {
                    case false:
                        x.EmptyStateButton.Click += null!;
                        x.EmptyStateButton.Click += EmptyStateButtonOnClick;
                        break;
                }

                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                MarketTab.MainScrollEvent.IsLoading = false;
            }
        }

        #endregion
         
    }
}
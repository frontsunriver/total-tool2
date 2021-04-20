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
using Android.Runtime;


using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using WoWonder.Activities.AddPost.Adapters;
using WoWonder.Activities.Base;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.User;
using WoWonderClient.Requests;
using SearchView = AndroidX.AppCompat.Widget.SearchView;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.AddPost
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MentionActivity : BaseActivity
    {
        #region Variables Basic

        public static MentionAdapter MAdapter;
        private SwipeRefreshLayout SwipeRefreshLayout;
        private RecyclerView MRecycler;
        private LinearLayoutManager LayoutManager;
        private ViewStub EmptyStateLayout;
        private View Inflated;
        private RecyclerViewOnScrollListener MainScrollEvent;
        private TextView BtnAction;
        private SearchView SearchView;
        private Toolbar ToolBar;
        private string SearchText = ""; 
        private AdView MAdView;
        private Xamarin.Facebook.Ads.InterstitialAd InterstitialAd;

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

                LoadContacts();

                InterstitialAd = AdsFacebook.InitInterstitial(this);
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.SearchGif_Menu, menu);
            WoWonderTools.ChangeMenuIconColor(menu, Color.ParseColor("#888888"));

            var item = menu.FindItem(Resource.Id.searchUserBar);
            SearchView searchItem = (SearchView)item.ActionView;

            SearchView = searchItem.JavaCast<SearchView>();
            SearchView.SetIconifiedByDefault(true);
            SearchView.QueryTextChange += SearchView_OnTextChange;
            SearchView.QueryTextSubmit += SearchView_OnTextSubmit;

            //Change text colors
            var editText = (EditText)SearchView.FindViewById(Resource.Id.search_src_text);
            editText.SetHintTextColor(Color.Black);
            editText.SetTextColor(Color.ParseColor("#888888")); 

            //Change Color Icon Search
            ImageView searchViewIcon = (ImageView)SearchView.FindViewById(Resource.Id.search_mag_icon);
            searchViewIcon.SetColorFilter(Color.ParseColor(AppSettings.MainColor));

            return base.OnCreateOptionsMenu(menu);
        }

        private void SearchView_OnTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            try
            {
                SearchText = e.NewText;

                SearchView.ClearFocus();

                SwipeRefreshLayout.Refreshing = true;
                SwipeRefreshLayout.Enabled = true;

                MAdapter.MentionList.Clear();
                MAdapter.NotifyDataSetChanged();

                StartSearchRequest();

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

        private void SearchView_OnTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            try
            {
                SearchText = e.NewText;
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
                MRecycler = (RecyclerView)FindViewById(Resource.Id.recyler);
                EmptyStateLayout = FindViewById<ViewStub>(Resource.Id.viewStub);

                SwipeRefreshLayout = (SwipeRefreshLayout)FindViewById(Resource.Id.swipeRefreshLayout);
                SwipeRefreshLayout.SetColorSchemeResources(Android.Resource.Color.HoloBlueLight, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
                SwipeRefreshLayout.Refreshing = true;
                SwipeRefreshLayout.Enabled = true;
                SwipeRefreshLayout.SetProgressBackgroundColorSchemeColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#424242") : Color.ParseColor("#f7f7f7"));

                BtnAction = FindViewById<TextView>(Resource.Id.toolbar_title);
                BtnAction.Visibility = ViewStates.Visible;
                       
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
                ToolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
                if (ToolBar != null)
                {
                    ToolBar.Title = GetText(Resource.String.Lbl_MentionsFriend);

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

        private void SetRecyclerViewAdapters()
        {
            try
            {
                MAdapter = new MentionAdapter(this)
                {
                    MentionList = new ObservableCollection<UserDataObject>(),
                };
                LayoutManager = new LinearLayoutManager(this);
                MRecycler.SetLayoutManager(LayoutManager); 
                MRecycler.HasFixedSize = true;
                MRecycler.SetItemViewCacheSize(10);
                MRecycler.GetLayoutManager().ItemPrefetchEnabled = true; 
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
                        BtnAction.Click += BtnActionOnClick;
                        SwipeRefreshLayout.Refresh += SwipeRefreshLayoutOnRefresh;
                        MAdapter.ItemClick += MAdapterOnItemClick;
                        break;
                    default:
                        BtnAction.Click -= BtnActionOnClick;
                        SwipeRefreshLayout.Refresh -= SwipeRefreshLayoutOnRefresh;
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
                InterstitialAd?.Destroy();

                MAdapter = null!;
                SwipeRefreshLayout = null!;
                MRecycler = null!;
                EmptyStateLayout = null!;
                Inflated = null!;
                MainScrollEvent = null!;
                BtnAction = null!;
                ToolBar = null!;
                SearchText = null!;
                MAdView = null!;
                InterstitialAd = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events

        private void MAdapterOnItemClick(object sender, MentionAdapterClickEventArgs e)
        {
            try
            {
                var item = MAdapter.GetItem(e.Position);
                switch (item)
                {
                    case null:
                        return;
                    default:
                        item.Selected = !item.Selected;
                        MAdapter.NotifyItemChanged(e.Position);
                        break;
                }
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
                MAdapter.MentionList.Clear();
                MAdapter.NotifyDataSetChanged();

               if (MainScrollEvent != null) MainScrollEvent.IsLoading = false;

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
                var item = MAdapter.MentionList.LastOrDefault();
                if (item != null && !string.IsNullOrEmpty(item.UserId) && !MainScrollEvent.IsLoading)
                    StartApiService();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void BtnActionOnClick(object sender, EventArgs e)
        {
            try
            {
                var resultIntent = new Intent();
                resultIntent?.PutExtra("Mentions", MAdapter.MentionList.Where(a => a.Selected).ToString());
                SetResult(Result.Ok, resultIntent);
                Finish();
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
                var sqlEntity = new SqLiteDatabase();
                var userList = sqlEntity.Get_MyContact();

                MAdapter.MentionList = new ObservableCollection<UserDataObject>(userList);
                MAdapter.NotifyDataSetChanged();

                

                SwipeRefreshLayout.Refreshing = false;

                var lastIdUser = MAdapter.MentionList.LastOrDefault()?.UserId ?? "0";
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
                var countList = MAdapter.MentionList.Count;
                var (apiStatus, respond) = await RequestsAsync.Global.GetFriendsAsync(UserDetails.UserId, "following", "10", offset);
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
                            foreach (var item in from item in result.DataFriends.Following let check = MAdapter.MentionList.FirstOrDefault(a => a.UserId == item.UserId) where check == null select item)
                            {
                                MAdapter.MentionList.Add(item);
                            }

                            RunOnUiThread(() => { MAdapter.NotifyItemRangeInserted(countList, MAdapter.MentionList.Count - countList); });
                            break;
                        }
                        case > 0:
                            MAdapter.MentionList = new ObservableCollection<UserDataObject>(result.DataFriends.Following);
                            RunOnUiThread(() => { MAdapter.NotifyDataSetChanged(); });
                            break;
                        default:
                        {
                            switch (MAdapter.MentionList.Count)
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
            }
            MainScrollEvent.IsLoading = false;
        }

        private void ShowEmptyPage()
        {
            try
            {              
                MainScrollEvent.IsLoading = false;
                SwipeRefreshLayout.Refreshing = false;

                switch (MAdapter.MentionList.Count)
                {
                    case > 0:
                    {
                        MRecycler.Visibility = ViewStates.Visible;
                        EmptyStateLayout.Visibility = ViewStates.Gone;

                        var sqlEntity = new SqLiteDatabase();
                        sqlEntity.Insert_Or_Replace_MyContactTable(MAdapter.MentionList);
                        break;
                    }
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
                MainScrollEvent.IsLoading = false;
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
         
        #region Load Data Search 

        private void Search()
        {
            try
            {
                switch (string.IsNullOrEmpty(SearchText))
                {
                    case false:
                    {
                        if (Methods.CheckConnectivity())
                        {
                            MAdapter?.MentionList?.Clear();
                            MAdapter?.NotifyDataSetChanged();

                            SwipeRefreshLayout.Refreshing = SwipeRefreshLayout.Refreshing switch
                            {
                                false => true,
                                _ => SwipeRefreshLayout.Refreshing
                            };

                            if (EmptyStateLayout != null)
                                EmptyStateLayout.Visibility = ViewStates.Gone;

                            StartSearchRequest();
                        }

                        break;
                    }
                    default:
                    {
                        Inflated = Inflated switch
                        {
                            null => EmptyStateLayout?.Inflate(),
                            _ => Inflated
                        };

                        EmptyStateInflater x = new EmptyStateInflater();
                        x.InflateLayout(Inflated, EmptyStateInflater.Type.NoSearchResult);
                        switch (x.EmptyStateButton.HasOnClickListeners)
                        {
                            case false:
                                x.EmptyStateButton.Click -= EmptyStateButtonOnClick;
                                x.EmptyStateButton.Click -= TryAgainButton_Click;
                                break;
                        }

                        x.EmptyStateButton.Click += TryAgainButton_Click;
                        if (EmptyStateLayout != null)
                        {
                            EmptyStateLayout.Visibility = ViewStates.Visible;
                        }

                        SwipeRefreshLayout.Refreshing = SwipeRefreshLayout.Refreshing switch
                        {
                            true => false,
                            _ => SwipeRefreshLayout.Refreshing
                        };
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private async void StartSearchRequest()
        {
            try
            {
                int countUserList = MAdapter.MentionList.Count;

                var dictionary = new Dictionary<string, string>
                {
                    {"user_id", UserDetails.UserId},
                    {"limit", "10"},
                    {"user_offset", "0"},
                    {"gender", UserDetails.SearchGender},
                    {"search_key", SearchText},
                };

                var (apiStatus, respond) = await RequestsAsync.Global.SearchAsync(dictionary);
                switch (apiStatus)
                {
                    case 200:
                    {
                        switch (respond)
                        {
                            case GetSearchObject result:
                            {
                                var respondUserList = result.Users?.Count;
                                switch (respondUserList)
                                {
                                    case > 0 when countUserList > 0:
                                    {
                                        foreach (var item in from item in result.Users let check = MAdapter.MentionList.FirstOrDefault(a => a.UserId == item.UserId) where check == null select item)
                                        {
                                            MAdapter.MentionList.Add(item);
                                        }

                                        RunOnUiThread(() => { MAdapter.NotifyItemRangeInserted(countUserList - 1, MAdapter.MentionList.Count - countUserList); });
                                        break;
                                    }
                                    case > 0:
                                        MAdapter.MentionList = new ObservableCollection<UserDataObject>(result.Users);
                                        RunOnUiThread(() => { MAdapter.NotifyDataSetChanged(); });
                                        break;
                                    default:
                                    {
                                        switch (MAdapter.MentionList.Count)
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

                MainScrollEvent.IsLoading = false;

                RunOnUiThread(ShowEmptyPage);
            }
            catch (Exception e)
            {
                StartSearchRequest();
                Methods.DisplayReportResultTrack(e);
            }
        }

        //No Internet Connection 
        private void TryAgainButton_Click(object sender, EventArgs e)
        {
            try
            {
                SearchText = "a";

                Search();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

    }
}
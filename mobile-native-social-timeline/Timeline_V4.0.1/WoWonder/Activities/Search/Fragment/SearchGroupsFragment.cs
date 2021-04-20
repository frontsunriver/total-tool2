using System;
using System.Collections.ObjectModel;
using System.Linq;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS;


using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using WoWonder.Library.Anjo.IntegrationRecyclerView;
using Bumptech.Glide.Util;
using WoWonder.Activities.Communities.Groups;
using WoWonder.Activities.Search.Adapters;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;

namespace WoWonder.Activities.Search.Fragment
{
    public class SearchGroupsFragment : AndroidX.Fragment.App.Fragment
    {
        #region Variables Basic

        public SearchGroupAdapter MAdapter;
        private SearchTabbedActivity ContextSearch;
        private SwipeRefreshLayout SwipeRefreshLayout;
        public RecyclerView MRecycler;
        public ProgressBar ProgressBarLoader;
        private LinearLayoutManager LayoutManager;
        public ViewStub EmptyStateLayout;
        public View Inflated;
        public RecyclerViewOnScrollListener MainScrollEvent;
        private AdView MAdView;

        #endregion

        #region General

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            { 
                View view = inflater.Inflate(Resource.Layout.SearchUsers_Layout, container, false); 
                ContextSearch = (SearchTabbedActivity)Activity; 
                return view;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            try
            {
                base.OnViewCreated(view, savedInstanceState);

                InitComponent(view);
                SetRecyclerViewAdapters();

                switch (string.IsNullOrEmpty(ContextSearch.DataKey))
                {
                    case false:
                        ContextSearch.Search("a");

                        switch (ContextSearch.DataKey)
                        {
                            case "Random_Groups":
                            {
                                if (ProgressBarLoader != null)
                                    ProgressBarLoader.Visibility = ViewStates.Visible;

                                if (EmptyStateLayout != null)
                                    EmptyStateLayout.Visibility = ViewStates.Gone;

                                if (MRecycler != null)
                                    MRecycler.Visibility = ViewStates.Visible;

                                ContextSearch.ViewPager.SetCurrentItem(2, true);
                                break;
                            }
                            case "Random_Pages":
                            {
                                if (ContextSearch.PagesTab.ProgressBarLoader != null)
                                    ContextSearch.PagesTab.ProgressBarLoader.Visibility = ViewStates.Visible;


                                if (ContextSearch.PagesTab.EmptyStateLayout != null)
                                    ContextSearch.PagesTab.EmptyStateLayout.Visibility = ViewStates.Gone;

                                if (ContextSearch.PagesTab.MRecycler != null)
                                    ContextSearch.PagesTab.MRecycler.Visibility = ViewStates.Visible;

                                ContextSearch.ViewPager.SetCurrentItem(1, true);
                                break;
                            }
                        }

                        break;
                    default:
                        ContextSearch.Search("");
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
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

        #endregion

        #region Functions

        private void InitComponent(View view)
        {
            try
            {
                MRecycler = (RecyclerView)view.FindViewById(Resource.Id.recyler);
                EmptyStateLayout = view.FindViewById<ViewStub>(Resource.Id.viewStub);

                ProgressBarLoader = (ProgressBar)view.FindViewById(Resource.Id.sectionProgress);
                ProgressBarLoader.Visibility = ViewStates.Gone;

                SwipeRefreshLayout = (SwipeRefreshLayout)view.FindViewById(Resource.Id.swipeRefreshLayout);
                SwipeRefreshLayout.SetColorSchemeResources(Android.Resource.Color.HoloBlueLight, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
                SwipeRefreshLayout.Refreshing = false;
                SwipeRefreshLayout.Enabled = false;
                SwipeRefreshLayout.SetProgressBackgroundColorSchemeColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#424242") : Color.ParseColor("#f7f7f7"));


                MAdView = view.FindViewById<AdView>(Resource.Id.adView);
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
                MAdapter = new SearchGroupAdapter(Activity) { GroupList = new ObservableCollection<GroupClass>() };
                MAdapter.ItemClick += MAdapterOnItemClick;
                MAdapter.JoinButtonItemClick += MAdapterOnJoinButtonItemClick;
                LayoutManager = new LinearLayoutManager(Activity);
                MRecycler.SetLayoutManager(LayoutManager); 
                MRecycler.HasFixedSize = true;
                MRecycler.SetItemViewCacheSize(10);
                MRecycler.GetLayoutManager().ItemPrefetchEnabled = true;
                var sizeProvider = new FixedPreloadSizeProvider(10, 10);
                var preLoader = new RecyclerViewPreloader<GroupClass>(Activity, MAdapter, sizeProvider, 10);
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

        #endregion

        #region Event

        //Scroll
        private void MainScrollEventOnLoadMoreEvent(object sender, EventArgs e)
        {
            try
            {
                //Code get last id where LoadMore >>
                var item = MAdapter.GroupList.LastOrDefault();
                if (item != null && !string.IsNullOrEmpty(item.GroupId) && !MainScrollEvent.IsLoading)
                {
                    ContextSearch.OffsetGroup = item.GroupId;
                    ContextSearch.StartApiService();
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void MAdapterOnItemClick(object sender, SearchGroupAdapterClickEventArgs e)
        {
            try
            {
                var item = MAdapter.GetItem(e.Position);
                if (item != null)
                {
                    MainApplication.GetInstance()?.NavigateTo(Activity, typeof(GroupProfileActivity), item);
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
                    WoWonderTools.SetJoinGroup(Activity, item.GroupId, e.Button);
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        } 

        #endregion

    }
}
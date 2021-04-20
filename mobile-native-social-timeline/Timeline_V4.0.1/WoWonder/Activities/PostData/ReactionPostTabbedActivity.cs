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
using AndroidX.AppCompat.Content.Res;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.Tabs;
using Newtonsoft.Json;
using WoWonder.Activities.Base;
using WoWonder.Activities.PostData.Fragment;
using WoWonder.Adapters;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Requests;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.PostData
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class ReactionPostTabbedActivity : BaseActivity, TabLayoutMediator.ITabConfigurationStrategy
    {
        #region Variables Basic

        private MainTabAdapter Adapter;
        private ViewPager2 ViewPager;
        private TabLayout TabLayout;

        private AngryReactionFragment AngryTab;
        private HahaReactionFragment HahaTab;
        private LikeReactionFragment LikeTab;
        private LoveReactionFragment LoveTab;
        private SadReactionFragment SadTab;
        private WowReactionFragment WowTab;

        private string PostId = "", TypeReaction = "Like";
        private PostDataObject PostData;
        

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
                SetContentView(Resource.Layout.PostReactionsLayout);

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();

                TypeReaction = "Like";

                LoadDataPost();
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
                ViewPager = FindViewById<ViewPager2>(Resource.Id.viewpager);
                TabLayout = FindViewById<TabLayout>(Resource.Id.tabs);

                ViewPager.OffscreenPageLimit = 6;
                SetUpViewPager(ViewPager);
                new TabLayoutMediator(TabLayout, ViewPager, this).Attach();
                TabLayout.SetTabTextColors(AppSettings.SetTabDarkTheme ? Color.White : Color.Black, Color.ParseColor(AppSettings.MainColor));
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
                    toolBar.Title = GetString(Resource.String.Lbl_PostReaction);
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
        private void DestroyBasic()
        {
            try
            {
                

                Adapter = null!;
                ViewPager = null!;
                TabLayout = null!;
                AngryTab = null!;
                HahaTab = null!;
                LikeTab = null!;
                LoveTab = null!;
                SadTab = null!;
                WowTab = null!;
                PostId = null!;
                TypeReaction = null!;
                PostData = null!;
                 
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
                PostData = JsonConvert.DeserializeObject<PostDataObject>(Intent?.GetStringExtra("PostObject") ?? "");
                if (PostData != null)
                {
                    Adapter = new MainTabAdapter(this);
                     
                    switch (PostData.Reaction.Count)
                    {
                        case > 0:
                        {
                            Bundle args = new Bundle();
                            args.PutString("NamePage", "Post");
                         
                            LikeTab = new LikeReactionFragment();
                            LoveTab = new LoveReactionFragment();
                            HahaTab = new HahaReactionFragment();
                            WowTab = new WowReactionFragment();
                            SadTab = new SadReactionFragment();
                            AngryTab = new AngryReactionFragment();

                            LikeTab.Arguments = args;
                            LoveTab.Arguments = args;
                            HahaTab.Arguments = args;
                            WowTab.Arguments = args;
                            SadTab.Arguments = args;
                            AngryTab.Arguments = args;
                         
                            Adapter.AddFragment(LikeTab, GetText(Resource.String.Btn_Likes));
                            Adapter.AddFragment(LoveTab, GetText(Resource.String.Btn_Love));
                            Adapter.AddFragment(HahaTab, GetText(Resource.String.Btn_Haha));
                            Adapter.AddFragment(WowTab, GetText(Resource.String.Btn_Wow));
                            Adapter.AddFragment(SadTab, GetText(Resource.String.Btn_Sad));
                            Adapter.AddFragment(AngryTab, GetText(Resource.String.Btn_Angry)); //wael
                            break;
                        }
                    }
                    //else
                    //{
                    //    if (PostData.Reaction.Like > 0 || PostData.Reaction.Like1 > 0)
                    //    {
                    //        LikeTab = new LikeReactionFragment();
                    //        Adapter.AddFragment(LikeTab, GetText(Resource.String.Btn_Likes));
                    //    }

                    //    if (PostData.Reaction.Love > 0 || PostData.Reaction.Love2 > 0)
                    //    {
                    //        LoveTab = new LoveReactionFragment();
                    //        Adapter.AddFragment(LoveTab, GetText(Resource.String.Btn_Love));
                    //    }

                    //    if (PostData.Reaction.HaHa > 0 || PostData.Reaction.HaHa3 > 0)
                    //    {
                    //        HahaTab = new HahaReactionFragment();
                    //        Adapter.AddFragment(HahaTab, GetText(Resource.String.Btn_Haha));
                    //    }

                    //    if (PostData.Reaction.Wow > 0 || PostData.Reaction.Wow4 > 0)
                    //    {
                    //        WowTab = new WowReactionFragment();
                    //        Adapter.AddFragment(WowTab, GetText(Resource.String.Btn_Wow));
                    //    }

                    //    if (PostData.Reaction.Sad > 0 || PostData.Reaction.Sad5 > 0)
                    //    {
                    //        SadTab = new SadReactionFragment();
                    //        Adapter.AddFragment(SadTab, GetText(Resource.String.Btn_Sad));
                    //    }

                    //    if (PostData.Reaction.Angry > 0 || PostData.Reaction.Angry6 > 0)
                    //    {
                    //        AngryTab = new AngryReactionFragment();
                    //        Adapter.AddFragment(AngryTab, GetText(Resource.String.Btn_Angry));
                    //    }
                    //}

                    viewPager.CurrentItem = Adapter.ItemCount;
                    viewPager.OffscreenPageLimit = Adapter.ItemCount;

                    viewPager.Orientation = ViewPager2.OrientationHorizontal;
                    viewPager.RegisterOnPageChangeCallback(new MyOnPageChangeCallback(this));
                    viewPager.Adapter = Adapter;
                    viewPager.Adapter.NotifyDataSetChanged();
                } 
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

        private class MyOnPageChangeCallback : ViewPager2.OnPageChangeCallback
        {
            private readonly ReactionPostTabbedActivity Activity;

            public MyOnPageChangeCallback(ReactionPostTabbedActivity activity)
            {
                try
                {
                    Activity = activity;
                }
                catch (Exception exception)
                {
                    Methods.DisplayReportResultTrack(exception);
                }
            }

            public override void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
            {
                try
                {
                    base.OnPageScrolled(position, positionOffset, positionOffsetPixels);
                    Activity.TypeReaction = position switch
                    {
                        0 => "Like",
                        1 => "Love",
                        2 => "Haha",
                        3 => "Wow",
                        4 => "Sad",
                        5 => "Angry",
                        _ => "Like"
                    };
                }
                catch (Exception exception)
                {
                    Methods.DisplayReportResultTrack(exception);
                }
            }

            public override void OnPageSelected(int position)
            {
                try
                {
                    base.OnPageSelected(position);
                    Activity.TypeReaction = position switch
                    {
                        0 => "Like",
                        1 => "Love",
                        2 => "Haha",
                        3 => "Wow",
                        4 => "Sad",
                        5 => "Angry",
                        _ => "Like"
                    };
                }
                catch (Exception exception)
                {
                    Methods.DisplayReportResultTrack(exception);
                }
            }
        }

        #endregion Set Tab

        #region Load data post 

        private void LoadDataPost()
        {
            try
            { 
                if (PostData != null)
                    PostId = PostData.PostId;

                StartApiService();
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
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => LoadDataPostAsync(offset) });
        }

        private async Task LoadDataPostAsync(string offset = "0")
        {
            if (LikeTab != null && LikeTab.MainScrollEvent.IsLoading)
                return;

            if (LoveTab != null && LoveTab.MainScrollEvent.IsLoading)
                return;

            if (WowTab != null && WowTab.MainScrollEvent.IsLoading)
                return;

            if (HahaTab != null && HahaTab.MainScrollEvent.IsLoading)
                return;

            if (SadTab != null && SadTab.MainScrollEvent.IsLoading)
                return;

            if (AngryTab != null && AngryTab.MainScrollEvent.IsLoading)
                return;

            if (LikeTab != null)
                LikeTab.MainScrollEvent.IsLoading = true;
            if (LoveTab != null)
                LoveTab.MainScrollEvent.IsLoading = true;
            if (WowTab != null)
                WowTab.MainScrollEvent.IsLoading = true;
            if (HahaTab != null)
                HahaTab.MainScrollEvent.IsLoading = true;
            if (SadTab != null)
                SadTab.MainScrollEvent.IsLoading = true;
            if (AngryTab != null)
                AngryTab.MainScrollEvent.IsLoading = true;


            var (apiStatus, respond) = await RequestsAsync.Posts.GetPostReactionsAsync(PostId, "10", TypeReaction, offset);
            switch (apiStatus)
            {
                case 200:
                {
                    switch (respond)
                    {
                        case PostReactionsObject result:
                        {
                            if (LikeTab != null)
                            {
                                int countLikeUserList = LikeTab?.MAdapter?.UserList?.Count ?? 0;

                                //Like
                                var respondListLike = result.Data.Like.Count;
                                switch (respondListLike)
                                {
                                    case > 0:
                                    {
                                        var dataTab = Adapter.FragmentNames.FirstOrDefault(a => a.Contains(GetText(Resource.String.Btn_Likes)));
                                        if (string.IsNullOrEmpty(dataTab))
                                            Adapter.AddFragment(LikeTab, GetText(Resource.String.Btn_Likes));

                                        switch (countLikeUserList)
                                        {
                                            case > 0:
                                            {
                                                foreach (var item in from item in result.Data.Like let check = LikeTab.MAdapter.UserList.FirstOrDefault(a => a.UserId == item.UserId) where check == null select item)
                                                {
                                                    LikeTab.MAdapter.UserList.Add(item);
                                                }

                                                RunOnUiThread(() => { LikeTab.MAdapter.NotifyItemRangeInserted(countLikeUserList - 1, LikeTab.MAdapter.UserList.Count - countLikeUserList); });
                                                break;
                                            }
                                            default:
                                                LikeTab.MAdapter.UserList = new ObservableCollection<UserDataObject>(result.Data.Like);
                                                RunOnUiThread(() => { LikeTab.MAdapter.NotifyDataSetChanged(); });
                                                break;
                                        }

                                        break;
                                    }
                                    default:
                                    {
                                        switch (LikeTab.MAdapter.UserList.Count)
                                        {
                                            case > 10 when !LikeTab.MRecycler.CanScrollVertically(1):
                                                Toast.MakeText(this, GetText(Resource.String.Lbl_No_more_users), ToastLength.Short)?.Show();
                                                break;
                                        }

                                        break;
                                    }
                                }
                            }

                            if (LoveTab != null)
                            {
                                int countLoveUserList = LoveTab?.MAdapter?.UserList?.Count ?? 0;

                                //Love
                                var respondListLove = result.Data.Love.Count;
                                switch (respondListLove)
                                {
                                    case > 0:
                                    {
                                        var dataTab = Adapter.FragmentNames.FirstOrDefault(a => a.Contains(GetText(Resource.String.Btn_Love)));
                                        if (string.IsNullOrEmpty(dataTab))
                                            Adapter.AddFragment(LoveTab, GetText(Resource.String.Btn_Love));

                                        switch (countLoveUserList)
                                        {
                                            case > 0:
                                            {
                                                foreach (var item in from item in result.Data.Love let check = LoveTab.MAdapter.UserList.FirstOrDefault(a => a.UserId == item.UserId) where check == null select item)
                                                {
                                                    LoveTab.MAdapter.UserList.Add(item);
                                                }

                                                RunOnUiThread(() => { LoveTab.MAdapter.NotifyItemRangeInserted(countLoveUserList - 1, LoveTab.MAdapter.UserList.Count - countLoveUserList); });
                                                break;
                                            }
                                            default:
                                                LoveTab.MAdapter.UserList = new ObservableCollection<UserDataObject>(result.Data.Love);
                                                RunOnUiThread(() => { LoveTab.MAdapter.NotifyDataSetChanged(); });
                                                break;
                                        }

                                        break;
                                    }
                                    default:
                                    {
                                        switch (LoveTab.MAdapter.UserList.Count)
                                        {
                                            case > 10 when !LoveTab.MRecycler.CanScrollVertically(1):
                                                Toast.MakeText(this, GetText(Resource.String.Lbl_No_more_users), ToastLength.Short)?.Show();
                                                break;
                                        }

                                        break;
                                    }
                                }
                            }

                            if (WowTab != null)
                            {
                                int countWowUserList = WowTab?.MAdapter?.UserList?.Count ?? 0;

                                //Wow
                                var respondListWow = result.Data.Wow.Count;
                                switch (respondListWow)
                                {
                                    case > 0:
                                    {
                                        var dataTab = Adapter.FragmentNames.FirstOrDefault(a => a.Contains(GetText(Resource.String.Btn_Wow)));
                                        if (string.IsNullOrEmpty(dataTab))
                                            Adapter.AddFragment(WowTab, GetText(Resource.String.Btn_Wow));

                                        switch (countWowUserList)
                                        {
                                            case > 0:
                                            {
                                                foreach (var item in from item in result.Data.Wow let check = WowTab.MAdapter.UserList.FirstOrDefault(a => a.UserId == item.UserId) where check == null select item)
                                                {
                                                    WowTab.MAdapter.UserList.Add(item);
                                                }

                                                RunOnUiThread(() => { WowTab.MAdapter.NotifyItemRangeInserted(countWowUserList - 1, WowTab.MAdapter.UserList.Count - countWowUserList); });
                                                break;
                                            }
                                            default:
                                                WowTab.MAdapter.UserList = new ObservableCollection<UserDataObject>(result.Data.Wow);
                                                RunOnUiThread(() => { WowTab.MAdapter.NotifyDataSetChanged(); });
                                                break;
                                        }

                                        break;
                                    }
                                    default:
                                    {
                                        switch (WowTab.MAdapter.UserList.Count)
                                        {
                                            case > 10 when !WowTab.MRecycler.CanScrollVertically(1):
                                                Toast.MakeText(this, GetText(Resource.String.Lbl_No_more_users), ToastLength.Short)?.Show();
                                                break;
                                        }

                                        break;
                                    }
                                }
                            }

                            if (HahaTab != null)
                            {
                                int countHahaUserList = HahaTab?.MAdapter?.UserList?.Count ?? 0;

                                //Haha
                                var respondListHaha = result.Data.Haha.Count;
                                switch (respondListHaha)
                                {
                                    case > 0:
                                    {
                                        var dataTab = Adapter.FragmentNames.FirstOrDefault(a => a.Contains(GetText(Resource.String.Btn_Haha)));
                                        if (string.IsNullOrEmpty(dataTab))
                                            Adapter.AddFragment(HahaTab, GetText(Resource.String.Btn_Haha));

                                        switch (countHahaUserList)
                                        {
                                            case > 0:
                                            {
                                                foreach (var item in from item in result.Data.Haha let check = HahaTab.MAdapter.UserList.FirstOrDefault(a => a.UserId == item.UserId) where check == null select item)
                                                {
                                                    HahaTab.MAdapter.UserList.Add(item);
                                                }

                                                RunOnUiThread(() => { HahaTab.MAdapter.NotifyItemRangeInserted(countHahaUserList - 1, HahaTab.MAdapter.UserList.Count - countHahaUserList); });
                                                break;
                                            }
                                            default:
                                                HahaTab.MAdapter.UserList = new ObservableCollection<UserDataObject>(result.Data.Haha);
                                                RunOnUiThread(() => { HahaTab.MAdapter.NotifyDataSetChanged(); });
                                                break;
                                        }

                                        break;
                                    }
                                    default:
                                    {
                                        switch (HahaTab.MAdapter.UserList.Count)
                                        {
                                            case > 10 when !HahaTab.MRecycler.CanScrollVertically(1):
                                                Toast.MakeText(this, GetText(Resource.String.Lbl_No_more_users), ToastLength.Short)?.Show();
                                                break;
                                        }

                                        break;
                                    }
                                }
                            }

                            if (SadTab != null)
                            {
                                int countSadUserList = SadTab?.MAdapter?.UserList?.Count ?? 0;

                                //Sad
                                var respondListSad = result.Data.Sad.Count;
                                switch (respondListSad)
                                {
                                    case > 0:
                                    {
                                        var dataTab = Adapter.FragmentNames.FirstOrDefault(a => a.Contains(GetText(Resource.String.Btn_Sad)));
                                        if (string.IsNullOrEmpty(dataTab))
                                            Adapter.AddFragment(SadTab, GetText(Resource.String.Btn_Sad));

                                        switch (countSadUserList)
                                        {
                                            case > 0:
                                            {
                                                foreach (var item in from item in result.Data.Sad let check = SadTab.MAdapter.UserList.FirstOrDefault(a => a.UserId == item.UserId) where check == null select item)
                                                {
                                                    SadTab.MAdapter.UserList.Add(item);
                                                }

                                                RunOnUiThread(() => { SadTab.MAdapter.NotifyItemRangeInserted(countSadUserList - 1, SadTab.MAdapter.UserList.Count - countSadUserList); });
                                                break;
                                            }
                                            default:
                                                SadTab.MAdapter.UserList = new ObservableCollection<UserDataObject>(result.Data.Sad);
                                                RunOnUiThread(() => { SadTab.MAdapter.NotifyDataSetChanged(); });
                                                break;
                                        }

                                        break;
                                    }
                                    default:
                                    {
                                        switch (SadTab.MAdapter.UserList.Count)
                                        {
                                            case > 10 when !SadTab.MRecycler.CanScrollVertically(1):
                                                Toast.MakeText(this, GetText(Resource.String.Lbl_No_more_users), ToastLength.Short)?.Show();
                                                break;
                                        }

                                        break;
                                    }
                                }
                            }

                            if (AngryTab != null)
                            {
                                int countAngryUserList = AngryTab?.MAdapter?.UserList?.Count ?? 0;

                                //Angry
                                var respondListAngry = result.Data.Angry.Count;
                                switch (respondListAngry)
                                {
                                    case > 0:
                                    {
                                        string dataTab = Adapter.FragmentNames.FirstOrDefault(a => a.Contains(GetText(Resource.String.Btn_Angry)));
                                        if (string.IsNullOrEmpty(dataTab))
                                            Adapter.AddFragment(AngryTab, GetText(Resource.String.Btn_Angry));

                                        switch (countAngryUserList)
                                        {
                                            case > 0:
                                            {
                                                foreach (var item in from item in result.Data.Angry let check = AngryTab.MAdapter.UserList.FirstOrDefault(a => a.UserId == item.UserId) where check == null select item)
                                                {
                                                    AngryTab.MAdapter.UserList.Add(item);
                                                }

                                                RunOnUiThread(() => { AngryTab.MAdapter.NotifyItemRangeInserted(countAngryUserList - 1, AngryTab.MAdapter.UserList.Count - countAngryUserList); });
                                                break;
                                            }
                                            default:
                                                AngryTab.MAdapter.UserList = new ObservableCollection<UserDataObject>(result.Data.Angry);
                                                RunOnUiThread(() => { AngryTab.MAdapter.NotifyDataSetChanged(); });
                                                break;
                                        }

                                        break;
                                    }
                                    default:
                                    {
                                        switch (AngryTab.MAdapter.UserList.Count)
                                        {
                                            case > 10 when !AngryTab.MRecycler.CanScrollVertically(1):
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

                    break;
                }
                default:
                    Methods.DisplayReportResult(this, respond);
                    break;
            }

            RunOnUiThread(ShowEmptyPage);
             
            if (LikeTab != null)
                LikeTab.MainScrollEvent.IsLoading = false;
            if (LoveTab != null)
                LoveTab.MainScrollEvent.IsLoading = false;
            if (WowTab != null)
                WowTab.MainScrollEvent.IsLoading = false;
            if (HahaTab != null)
                HahaTab.MainScrollEvent.IsLoading = false;
            if (SadTab != null)
                SadTab.MainScrollEvent.IsLoading = false;
            if (AngryTab != null)
                AngryTab.MainScrollEvent.IsLoading = false; 
        }

        private void ShowEmptyPage()
        {
            try
            {
                if (LikeTab != null)
                    LikeTab.MainScrollEvent.IsLoading = false;
                if (LoveTab != null)
                    LoveTab.MainScrollEvent.IsLoading = false;
                if (WowTab != null)
                    WowTab.MainScrollEvent.IsLoading = false;
                if (HahaTab != null)
                    HahaTab.MainScrollEvent.IsLoading = false;
                if (SadTab != null)
                    SadTab.MainScrollEvent.IsLoading = false;
                if (AngryTab != null)
                    AngryTab.MainScrollEvent.IsLoading = false;

                if (Adapter.ItemCount != ViewPager.Adapter.ItemCount)
                {
                    ViewPager.CurrentItem = Adapter.ItemCount;
                    ViewPager.Adapter = Adapter;
                    ViewPager.Adapter.NotifyDataSetChanged();
                }

                if (LikeTab != null)
                {
                    LikeTab.SwipeRefreshLayout.Refreshing = LikeTab.SwipeRefreshLayout.Refreshing switch
                    {
                        true => false,
                        _ => LikeTab.SwipeRefreshLayout.Refreshing
                    };

                    switch (LikeTab.MAdapter.UserList.Count)
                    {
                        case > 0:
                            LikeTab.MRecycler.Visibility = ViewStates.Visible;
                            LikeTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                            break;
                        default:
                        {
                            LikeTab.MRecycler.Visibility = ViewStates.Gone;

                            LikeTab.Inflated = LikeTab.Inflated switch
                            {
                                null => LikeTab.EmptyStateLayout.Inflate(),
                                _ => LikeTab.Inflated
                            };

                            EmptyStateInflater x = new EmptyStateInflater();
                            x.InflateLayout(LikeTab.Inflated, EmptyStateInflater.Type.NoUsersReaction);
                            switch (x.EmptyStateButton.HasOnClickListeners)
                            {
                                case false:
                                    x.EmptyStateButton.Click += null!;
                                    break;
                            }
                            LikeTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                            break;
                        }
                    }
                }

                if (LoveTab != null)
                {
                    LoveTab.SwipeRefreshLayout.Refreshing = LoveTab.SwipeRefreshLayout.Refreshing switch
                    {
                        true => false,
                        _ => LoveTab.SwipeRefreshLayout.Refreshing
                    };

                    switch (LoveTab.MAdapter.UserList.Count)
                    {
                        case > 0:
                            LoveTab.MRecycler.Visibility = ViewStates.Visible;
                            LoveTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                            break;
                        default:
                        {
                            LoveTab.MRecycler.Visibility = ViewStates.Gone;

                            LoveTab.Inflated = LoveTab.Inflated switch
                            {
                                null => LoveTab.EmptyStateLayout.Inflate(),
                                _ => LoveTab.Inflated
                            };

                            EmptyStateInflater x = new EmptyStateInflater();
                            x.InflateLayout(LoveTab.Inflated, EmptyStateInflater.Type.NoUsersReaction);
                            switch (x.EmptyStateButton.HasOnClickListeners)
                            {
                                case false:
                                    x.EmptyStateButton.Click += null!;
                                    break;
                            }
                            LoveTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                            break;
                        }
                    }
                }

                if (WowTab != null)
                {
                    WowTab.SwipeRefreshLayout.Refreshing = WowTab.SwipeRefreshLayout.Refreshing switch
                    {
                        true => false,
                        _ => WowTab.SwipeRefreshLayout.Refreshing
                    };

                    switch (WowTab.MAdapter.UserList.Count)
                    {
                        case > 0:
                            WowTab.MRecycler.Visibility = ViewStates.Visible;
                            WowTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                            break;
                        default:
                        {
                            WowTab.MRecycler.Visibility = ViewStates.Gone;

                            WowTab.Inflated = WowTab.Inflated switch
                            {
                                null => WowTab.EmptyStateLayout.Inflate(),
                                _ => WowTab.Inflated
                            };

                            EmptyStateInflater x = new EmptyStateInflater();
                            x.InflateLayout(WowTab.Inflated, EmptyStateInflater.Type.NoUsersReaction);
                            switch (x.EmptyStateButton.HasOnClickListeners)
                            {
                                case false:
                                    x.EmptyStateButton.Click += null!;
                                    break;
                            }
                            WowTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                            break;
                        }
                    }
                }

                if (HahaTab != null)
                {
                    HahaTab.SwipeRefreshLayout.Refreshing = HahaTab.SwipeRefreshLayout.Refreshing switch
                    {
                        true => false,
                        _ => HahaTab.SwipeRefreshLayout.Refreshing
                    };

                    switch (HahaTab.MAdapter.UserList.Count)
                    {
                        case > 0:
                            HahaTab.MRecycler.Visibility = ViewStates.Visible;
                            HahaTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                            break;
                        default:
                        {
                            HahaTab.MRecycler.Visibility = ViewStates.Gone;

                            HahaTab.Inflated = HahaTab.Inflated switch
                            {
                                null => HahaTab.EmptyStateLayout.Inflate(),
                                _ => HahaTab.Inflated
                            };

                            EmptyStateInflater x = new EmptyStateInflater();
                            x.InflateLayout(HahaTab.Inflated, EmptyStateInflater.Type.NoUsersReaction);
                            switch (x.EmptyStateButton.HasOnClickListeners)
                            {
                                case false:
                                    x.EmptyStateButton.Click += null!;
                                    break;
                            }
                            HahaTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                            break;
                        }
                    }
                }

                if (SadTab != null)
                {
                    SadTab.SwipeRefreshLayout.Refreshing = SadTab.SwipeRefreshLayout.Refreshing switch
                    {
                        true => false,
                        _ => SadTab.SwipeRefreshLayout.Refreshing
                    };

                    switch (SadTab.MAdapter.UserList.Count)
                    {
                        case > 0:
                            SadTab.MRecycler.Visibility = ViewStates.Visible;
                            SadTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                            break;
                        default:
                        {
                            SadTab.MRecycler.Visibility = ViewStates.Gone;

                            SadTab.Inflated = SadTab.Inflated switch
                            {
                                null => SadTab.EmptyStateLayout.Inflate(),
                                _ => SadTab.Inflated
                            };

                            EmptyStateInflater x = new EmptyStateInflater();
                            x.InflateLayout(SadTab.Inflated, EmptyStateInflater.Type.NoUsersReaction);
                            switch (x.EmptyStateButton.HasOnClickListeners)
                            {
                                case false:
                                    x.EmptyStateButton.Click += null!;
                                    break;
                            }
                            SadTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                            break;
                        }
                    }
                }

                if (AngryTab != null)
                {
                    AngryTab.SwipeRefreshLayout.Refreshing = AngryTab.SwipeRefreshLayout.Refreshing switch
                    {
                        true => false,
                        _ => AngryTab.SwipeRefreshLayout.Refreshing
                    };

                    switch (AngryTab.MAdapter.UserList.Count)
                    {
                        case > 0:
                            AngryTab.MRecycler.Visibility = ViewStates.Visible;
                            AngryTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                            break;
                        default:
                        {
                            AngryTab.MRecycler.Visibility = ViewStates.Gone;

                            AngryTab.Inflated = AngryTab.Inflated switch
                            {
                                null => AngryTab.EmptyStateLayout.Inflate(),
                                _ => AngryTab.Inflated
                            };

                            EmptyStateInflater x = new EmptyStateInflater();
                            x.InflateLayout(AngryTab.Inflated, EmptyStateInflater.Type.NoUsersReaction);
                            switch (x.EmptyStateButton.HasOnClickListeners)
                            {
                                case false:
                                    x.EmptyStateButton.Click += null!;
                                    break;
                            }
                            AngryTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                            break;
                        }
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
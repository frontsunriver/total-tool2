using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS; 
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using WoWonder.Library.Anjo.IntegrationRecyclerView;
using Bumptech.Glide.Util;
using Newtonsoft.Json;
using WoWonder.Activities.Communities.Groups;
using WoWonder.Activities.Communities.Pages;
using WoWonder.Activities.Events;
using WoWonder.Activities.Memories;
using WoWonder.Activities.NativePost.Pages;
using WoWonder.Activities.Story;
using WoWonder.Activities.Tabbes.Adapters;
using WoWonder.Activities.UserProfile;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Story;
using WoWonderClient.Requests;

namespace WoWonder.Activities.Tabbes.Fragment
{
    public class NotificationsFragment : AndroidX.Fragment.App.Fragment
    {
        #region Variables Basic

        private NotificationsAdapter MAdapter;
        private TabbedMainActivity GlobalContext;
        private SwipeRefreshLayout SwipeRefreshLayout;
        private RecyclerView MRecycler;
        private LinearLayoutManager LayoutManager;
        private ViewStub EmptyStateLayout;
        private View Inflated;
        private RecyclerViewOnScrollListener MainScrollEvent;
       
        #endregion

        #region General

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
            GlobalContext = (TabbedMainActivity)Activity ?? TabbedMainActivity.GetInstance();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            { 
                View view = inflater.Inflate(Resource.Layout.Tab_Notifications_Layout, container, false); 
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

                switch (AppSettings.SetTabOnButton)
                {
                    case false:
                    {
                        var parasms = (RelativeLayout.LayoutParams)SwipeRefreshLayout.LayoutParameters;
                        // Check if we're running on Android 5.0 or higher
                        parasms.TopMargin = (int)Build.VERSION.SdkInt < 23 ? 80 : 120;

                        MRecycler.LayoutParameters = parasms;
                        MRecycler.SetPadding(0, 0, 0, 0);
                        break;
                    }
                }

                if (!Methods.CheckConnectivity())
                    Toast.MakeText(Context, Context.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                else
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => LoadGeneralData(true) });

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

                SwipeRefreshLayout = (SwipeRefreshLayout)view.FindViewById(Resource.Id.swipeRefreshLayout);
                SwipeRefreshLayout.SetColorSchemeResources(Android.Resource.Color.HoloBlueLight, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
                SwipeRefreshLayout.Refreshing = true;
                SwipeRefreshLayout.Enabled = true;
                SwipeRefreshLayout.SetProgressBackgroundColorSchemeColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#424242") : Color.ParseColor("#f7f7f7"));

                SwipeRefreshLayout.Refresh += SwipeRefreshLayoutOnRefresh;

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
         
        private void SetRecyclerViewAdapters()
        {
            try
            {
                MAdapter = new NotificationsAdapter(Activity) { NotificationsList = new ObservableCollection<NotificationObject>() };
                MAdapter.ItemClick += MAdapterOnItemClick;
                LayoutManager = new LinearLayoutManager(Activity);
                MRecycler.SetLayoutManager(LayoutManager); 
                MRecycler.HasFixedSize = true;
                MRecycler.SetItemViewCacheSize(10);
                MRecycler.GetLayoutManager().ItemPrefetchEnabled = true;
                var sizeProvider = new FixedPreloadSizeProvider(10, 10);
                var preLoader = new RecyclerViewPreloader<Notification>(Activity, MAdapter, sizeProvider, 10);
                MRecycler.AddOnScrollListener(preLoader);
                MRecycler.SetAdapter(MAdapter);

                MRecycler.NestedScrollingEnabled = true;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion
         
        #region Event

        //Open user profile
        private void MAdapterOnItemClick(object sender, NotificationsAdapterClickEventArgs e)
        {
            try
            {
                switch (e.Position)
                {
                    case > -1:
                    {
                        var item = MAdapter.GetItem(e.Position);
                        if (item != null)
                        {
                            EventClickNotification(Activity, item);
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

        //Refresh
        private void SwipeRefreshLayoutOnRefresh(object sender, EventArgs e)
        {
            try
            {
                MAdapter.NotificationsList.Clear();
                MAdapter.NotifyDataSetChanged();

                if (MainScrollEvent != null) MainScrollEvent.IsLoading = false;

                MRecycler.Visibility = ViewStates.Visible;
                EmptyStateLayout.Visibility = ViewStates.Gone;

                if (!Methods.CheckConnectivity())
                    Toast.MakeText(Context, Context.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                else
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => LoadGeneralData(true) });
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
                //Code get last id where LoadMore >>
                var item = MAdapter.NotificationsList.LastOrDefault();
                if (item != null && !string.IsNullOrEmpty(item.NotifierId) && !MainScrollEvent.IsLoading)
                {
                    if (!Methods.CheckConnectivity())
                        Toast.MakeText(Context, Context.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    else
                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => LoadGeneralData(false, item.NotifierId) });
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        #endregion

        #region Load Notification 

        //Get General Data Using Api >> notifications , pro_users , promoted_pages , trending_hashTag
        public async Task<(string, string, string, string)> LoadGeneralData(bool seenNotifications, string offset = "")
        {
            try
            {
                switch (MainScrollEvent.IsLoading)
                {
                    case true:
                        return ("", "", "", "");
                }

                if (Methods.CheckConnectivity())
                {
                    MainScrollEvent.IsLoading = true;
                     
                    var (apiStatus, respond) = await RequestsAsync.Global.GetGeneralDataAsync(seenNotifications, UserDetails.OnlineUsers, UserDetails.DeviceId, offset);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case GetGeneralDataObject result:
                                    Activity?.RunOnUiThread(() =>
                                    {
                                        try
                                        {
                                            // Notifications
                                            var countNotificationsList = MAdapter.NotificationsList.Count;
                                            var respondList = result.Notifications.Count;
                                            switch (respondList)
                                            {
                                                case > 0 when countNotificationsList > 0:
                                                {
                                                    var listNew = result.Notifications?.Where(c => !MAdapter.NotificationsList.Select(fc => fc.Id).Contains(c.Id)).ToList();
                                                    switch (listNew.Count)
                                                    {
                                                        case > 0:
                                                        {
                                                            foreach (var notification in listNew)
                                                            {
                                                                MAdapter.NotificationsList.Insert(0 , notification);
                                                            }

                                                            MAdapter.NotifyItemRangeInserted(countNotificationsList - 1, MAdapter.NotificationsList.Count - countNotificationsList);
                                                            break;
                                                        }
                                                    }

                                                    break;
                                                }
                                                case > 0:
                                                    MAdapter.NotificationsList = new ObservableCollection<NotificationObject>(result.Notifications);
                                                    MAdapter.NotifyDataSetChanged();
                                                    break;
                                                default:
                                                {
                                                    switch (MAdapter.NotificationsList.Count)
                                                    {
                                                        case > 10 when !MRecycler.CanScrollVertically(1):
                                                            Toast.MakeText(Context, Context.GetText(Resource.String.Lbl_NoMoreNotifications), ToastLength.Short)?.Show();
                                                            break;
                                                    }

                                                    break;
                                                }
                                            }
                                    
                                            switch (AppSettings.ShowTrendingPage)
                                            {
                                                case true when GlobalContext.TrendingTab != null:
                                                {
                                                    // FriendRequests
                                                    var respondListFriendRequests = result?.FriendRequests?.Count;
                                                    switch (respondListFriendRequests)
                                                    {
                                                        case > 0:
                                                        {
                                                            ListUtils.FriendRequestsList = new ObservableCollection<UserDataObject>(result.FriendRequests);

                                                            var checkList = GlobalContext.TrendingTab.MAdapter.TrendingList.FirstOrDefault(q => q.Type == Classes.ItemType.FriendRequest);
                                                            switch (checkList)
                                                            {
                                                                case null:
                                                                {
                                                                    var friendRequests = new Classes.TrendingClass
                                                                    {
                                                                        Id = 400,
                                                                        UserList = new List<UserDataObject>(),
                                                                        Type = Classes.ItemType.FriendRequest
                                                                    };

                                                                    var list = result.FriendRequests.TakeLast(4).ToList();
                                                                    switch (list.Count)
                                                                    {
                                                                        case > 0:
                                                                            friendRequests.UserList.AddRange(list);
                                                                            break;
                                                                    }

                                                                    GlobalContext.TrendingTab.MAdapter.TrendingList.Add(friendRequests);
                                                
                                                                    GlobalContext.TrendingTab.MAdapter.TrendingList.Add(new Classes.TrendingClass
                                                                    { 
                                                                        Type = Classes.ItemType.Divider
                                                                    });
                                                                    break;
                                                                }
                                                                default:
                                                                {
                                                                    switch (checkList.UserList.Count)
                                                                    {
                                                                        case < 3:
                                                                        {
                                                                            var list = result.FriendRequests.TakeLast(4).ToList();
                                                                            switch (list.Count)
                                                                            {
                                                                                case > 0:
                                                                                    checkList.UserList.AddRange(list);
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
                                                    }

                                                    // TrendingHashtag
                                                    var respondListHashTag = result?.TrendingHashtag?.Count;
                                                    switch (respondListHashTag)
                                                    {
                                                        case > 0 when AppSettings.ShowTrendingHashTags:
                                                        {
                                                            ListUtils.HashTagList = new ObservableCollection<TrendingHashtag>(result.TrendingHashtag);

                                                            var checkList = GlobalContext.TrendingTab.MAdapter.TrendingList.FirstOrDefault(q => q.Type == Classes.ItemType.HashTag);
                                                            switch (checkList)
                                                            {
                                                                case null:
                                                                {
                                                                    GlobalContext.TrendingTab.MAdapter.TrendingList.Add(new Classes.TrendingClass
                                                                    {
                                                                        Id = 900,
                                                                        Title = Activity.GetText(Resource.String.Lbl_TrendingHashTags),
                                                                        SectionType = Classes.ItemType.HashTag,
                                                                        Type = Classes.ItemType.Section,
                                                                    });

                                                                    var list = result.TrendingHashtag.Take(5).ToList();

                                                                    foreach (var item in from item in list let check = GlobalContext.TrendingTab.MAdapter.TrendingList.FirstOrDefault(a => a.HashTags?.Id == item.Id && a.Type == Classes.ItemType.HashTag) where check == null select item)
                                                                    {
                                                                        GlobalContext.TrendingTab.MAdapter.TrendingList.Add(new Classes.TrendingClass
                                                                        {
                                                                            Id = long.Parse(item.Id),
                                                                            HashTags = item,
                                                                            Type = Classes.ItemType.HashTag
                                                                        });
                                                                    }

                                                                    GlobalContext.TrendingTab.MAdapter.TrendingList.Add(new Classes.TrendingClass
                                                                    {
                                                                        Type = Classes.ItemType.Divider
                                                                    });
                                                                    break;
                                                                }
                                                            }

                                                            break;
                                                        }
                                                    }

                                                    // PromotedPages
                                                    var respondListPromotedPages = result.PromotedPages?.Count;
                                                    switch (respondListPromotedPages)
                                                    {
                                                        case > 0 when AppSettings.ShowPromotedPages:
                                                        {
                                                            var checkList = GlobalContext.TrendingTab.MAdapter.TrendingList.FirstOrDefault(q => q.Type == Classes.ItemType.ProPage);
                                                            switch (checkList)
                                                            {
                                                                case null:
                                                                {
                                                                    var proPage = new Classes.TrendingClass
                                                                    {
                                                                        Id = 200,
                                                                        PageList = new List<PageClass>(),
                                                                        Type = Classes.ItemType.ProPage
                                                                    };

                                                                    foreach (var item in from item in result.PromotedPages let check = proPage.PageList.FirstOrDefault(a => a.PageId == item.PageId) where check == null select item)
                                                                    {
                                                                        proPage.PageList.Add(item);
                                                                    }

                                                                    GlobalContext.TrendingTab.MAdapter.TrendingList.Insert(0, proPage);
                                                                    GlobalContext.TrendingTab.MAdapter.TrendingList.Insert(1, new Classes.TrendingClass
                                                                    {
                                                                        Type = Classes.ItemType.Divider
                                                                    });
                                                                    break;
                                                                }
                                                                default:
                                                                {
                                                                    foreach (var item in from item in result.PromotedPages let check = checkList.PageList.FirstOrDefault(a => a.PageId == item.PageId) where check == null select item)
                                                                    {
                                                                        checkList.PageList.Add(item);
                                                                    }

                                                                    break;
                                                                }
                                                            }

                                                            break;
                                                        }
                                                    }

                                                    // ProUsers
                                                    var respondListProUsers = result?.ProUsers?.Count;
                                                    switch (respondListProUsers)
                                                    {
                                                        case > 0 when AppSettings.ShowProUsersMembers:
                                                        {
                                                            var checkList = GlobalContext.TrendingTab.MAdapter.TrendingList.FirstOrDefault(q => q.Type == Classes.ItemType.ProUser);
                                                            switch (checkList)
                                                            {
                                                                case null:
                                                                {
                                                                    var proUser = new Classes.TrendingClass
                                                                    {
                                                                        Id = 100,
                                                                        UserList = new List<UserDataObject>(),
                                                                        Type = Classes.ItemType.ProUser
                                                                    };

                                                                    foreach (var item in from item in result.ProUsers let check = proUser.UserList.FirstOrDefault(a => a.UserId == item.UserId) where check == null select item)
                                                                    {
                                                                        proUser.UserList.Add(item);
                                                                    }

                                                                    GlobalContext.TrendingTab.MAdapter.TrendingList.Insert(0, proUser);
                                                                    GlobalContext.TrendingTab.MAdapter.TrendingList.Insert(1, new Classes.TrendingClass
                                                                    {
                                                                        Type = Classes.ItemType.Divider
                                                                    });
                                                                    break;
                                                                }
                                                                default:
                                                                {
                                                                    foreach (var item in from item in result.ProUsers let check = checkList.UserList.FirstOrDefault(a => a.UserId == item.UserId) where check == null select item)
                                                                    {
                                                                        checkList.UserList.Add(item);
                                                                    }

                                                                    break;
                                                                }
                                                            }

                                                            break;
                                                        }
                                                    }

                                                    switch (AppSettings.ShowInfoCoronaVirus)
                                                    {
                                                        case true:
                                                        {
                                                            var check = GlobalContext.TrendingTab.MAdapter.TrendingList.FirstOrDefault(q => q.Type == Classes.ItemType.CoronaVirus);
                                                            switch (check)
                                                            {
                                                                case null:
                                                                {
                                                                    var coronaVirus = new Classes.TrendingClass
                                                                    {
                                                                        Id = 20316,
                                                                        Type = Classes.ItemType.CoronaVirus
                                                                    };

                                                                    GlobalContext.TrendingTab.MAdapter.TrendingList.Insert(0, coronaVirus);
                                                                    GlobalContext.TrendingTab.MAdapter.TrendingList.Insert(1, new Classes.TrendingClass
                                                                    {
                                                                        Type = Classes.ItemType.Divider
                                                                    });
                                                                    break;
                                                                }
                                                            }

                                                            break;
                                                        }
                                                    }
                                        
                                                    GlobalContext.TrendingTab.MAdapter.NotifyDataSetChanged();
                                                    break;
                                                }
                                            }
                                     
                                            MainScrollEvent.IsLoading = false;
                                            ShowEmptyPage();
                                        }
                                        catch (Exception e)
                                        {
                                            Methods.DisplayReportResultTrack(e);
                                        }
                                    }); 
                                    return (result.NewNotificationsCount, result.NewFriendRequestsCount, result.CountNewMessages, result.Announcement?.AnnouncementClass?.TextDecode);
                            }

                            break;
                        }
                        default:
                            Methods.DisplayReportResult(Activity, respond);
                            break;
                    }

                    Activity?.RunOnUiThread(ShowEmptyPage);
                    MainScrollEvent.IsLoading = false;

                    return ("", "", "", "");
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

                    Toast.MakeText(Context, Context.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
            }
            catch (Exception e)
            {
                MainScrollEvent.IsLoading = false;
                Methods.DisplayReportResultTrack(e); 
            }
            MainScrollEvent.IsLoading = false;
            return ("", "", "", "");
        }

        private void ShowEmptyPage()
        {
            try
            {
                MainScrollEvent.IsLoading = false;
                SwipeRefreshLayout.Refreshing = false;

                switch (MAdapter.NotificationsList.Count)
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
                        x.InflateLayout(Inflated, EmptyStateInflater.Type.NoNotifications);
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
                if (!Methods.CheckConnectivity())
                    Toast.MakeText(Context, Context.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                else
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => LoadGeneralData(true) });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion
         
        public void EventClickNotification(Activity activity , NotificationObject item)
        {
            try
            {
                switch (item.Type)
                {
                    case "following":
                    case "visited_profile":
                    case "accepted_request":
                        WoWonderTools.OpenProfile(Activity, item.Notifier.UserId, item.Notifier);
                        break;
                    case "liked_page":
                    case "invited_page":
                    case "accepted_invite":
                    {
                        var intent = new Intent(Activity, typeof(PageProfileActivity));
                        //intent.PutExtra("PageObject", JsonConvert.SerializeObject(item));
                        intent.PutExtra("PageId", item.PageId);
                        activity.StartActivity(intent);
                        break;
                    }
                    case "joined_group":
                    case "accepted_join_request":
                    case "added_you_to_group":
                    {
                        var intent = new Intent(activity, typeof(GroupProfileActivity));
                        //intent.PutExtra("GroupObject", JsonConvert.SerializeObject(item));
                        intent.PutExtra("GroupId", item.GroupId);
                        activity.StartActivity(intent);
                        break;
                    }
                    case "comment":
                    case "wondered_post":
                    case "wondered_comment":
                    case "reaction":
                    case "wondered_reply_comment":
                    case "comment_mention":
                    case "comment_reply_mention":
                    case "liked_post":
                    case "liked_comment":
                    case "liked_reply_comment":
                    case "post_mention":
                    case "share_post":
                    case "shared_your_post":
                    case "comment_reply":
                    case "also_replied":
                    case "profile_wall_post":
                    {
                        var intent = new Intent(activity, typeof(ViewFullPostActivity));
                        intent.PutExtra("Id", item.PostId);
                        // intent.PutExtra("DataItem", JsonConvert.SerializeObject(item));
                        activity.StartActivity(intent);
                        break;
                    }
                    case "going_event":
                    {
                        var intent = new Intent(activity, typeof(EventViewActivity));
                        intent.PutExtra("EventId", item.EventId);
                        if (item.Event != null)
                            intent.PutExtra("EventView", JsonConvert.SerializeObject(item.Event));
                        activity.StartActivity(intent);
                        break;
                    }
                    case "viewed_story":
                    {
                        //"url": "https:\/\/demo.wowonder.com\/timeline&u=Matan&story=true&story_id=1946",
                        //var id = item.Url.Split("/").Last().Split("&story_id=").Last();

                        StoryDataObject dataMyStory = GlobalContext?.NewsFeedTab?.PostFeedAdapter?.HolderStory?.StoryAdapter?.StoryList?.FirstOrDefault(o => o.UserId == UserDetails.UserId);
                        if (dataMyStory != null)
                        {
                            Intent intent = new Intent(activity, typeof(ViewStoryActivity));
                            intent.PutExtra("UserId", dataMyStory.UserId);
                            intent.PutExtra("DataItem", JsonConvert.SerializeObject(dataMyStory));
                            activity.StartActivity(intent);
                        }

                        break;
                    }
                    case "requested_to_join_group":
                    {
                        var intent = new Intent(activity, typeof(JoinRequestActivity));
                        intent.PutExtra("GroupId", item.GroupId);
                        activity.StartActivity(intent);
                        break;
                    }
                    case "memory":
                    {
                        var intent = new Intent(activity, typeof(MemoriesActivity));
                        activity.StartActivity(intent);
                        break;
                    }
                    case "gift":
                    {
                        var ajaxUrl = item.AjaxUrl.Split(new[] { "&", "gift_img=" }, StringSplitOptions.None);
                        var urlImage = WoWonderTools.GetTheFinalLink(ajaxUrl?[3]?.Replace("%2F", "/"));

                        var intent = new Intent(Activity, typeof(UserProfileActivity));
                        intent.PutExtra("UserObject", JsonConvert.SerializeObject(item.Notifier));
                        intent.PutExtra("UserId", item.Notifier.UserId);
                        intent.PutExtra("GifLink", urlImage);
                        Activity.StartActivity(intent);
                        break;
                    }
                    default:
                        WoWonderTools.OpenProfile(Activity, item.Notifier.UserId, item.Notifier);
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
    }
}
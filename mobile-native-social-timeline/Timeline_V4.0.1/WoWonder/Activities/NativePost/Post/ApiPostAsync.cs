using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Views;
using Java.Lang;
using Newtonsoft.Json;
using WoWonder.Activities.NativePost.Extra;
using WoWonder.Activities.Tabbes;
using WoWonder.Activities.UserProfile;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using WoWonderClient;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Classes.Story;
using WoWonderClient.Requests;
using Exception = System.Exception;

namespace WoWonder.Activities.NativePost.Post
{
    public class ApiPostAsync
    {
        private readonly Activity ActivityContext;
        private readonly NativePostAdapter NativeFeedAdapter;
        private readonly WRecyclerView WRecyclerView;
        private static bool ShowFindMoreAlert;
        private static PostModelType LastAdsType = PostModelType.AdMob3;
        public static List<PostDataObject> PostCacheList { private set; get; } 
         
        public ApiPostAsync(WRecyclerView recyclerView, NativePostAdapter adapter)
        {
            try
            {
                ActivityContext = adapter.ActivityContext;
                NativeFeedAdapter = adapter;
                WRecyclerView = recyclerView;
                PostCacheList = new List<PostDataObject>();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Api

        public async Task FetchNewsFeedApiPosts(string offset = "0", string typeRun = "Add", string hash = "")
        {
            switch (WRecyclerView.MainScrollEvent.IsLoading)
            {
                case true:
                    return;
            }

            if (!Methods.CheckConnectivity())
                return;

            WRecyclerView.Hash = hash;
            int apiStatus;
            dynamic respond;

            WRecyclerView.MainScrollEvent.IsLoading = true; 

            switch (NativeFeedAdapter.NativePostType)
            {
                case NativeFeedType.Global:
                    (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_news_feed", NativeFeedAdapter.IdParameter, "", WRecyclerView.Filter);
                    break;
                case NativeFeedType.User:
                    (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_user_posts", NativeFeedAdapter.IdParameter);
                    break;
                case NativeFeedType.Group:
                    (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_group_posts", NativeFeedAdapter.IdParameter);
                    break;
                case NativeFeedType.Page:
                    (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_page_posts", NativeFeedAdapter.IdParameter);
                    break;
                case NativeFeedType.Event:
                    (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_event_posts", NativeFeedAdapter.IdParameter);
                    break;
                case NativeFeedType.Saved:
                    (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "saved");
                    break;
                case NativeFeedType.HashTag:
                    (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "hashtag", "", hash);
                    break;
                case NativeFeedType.Popular:
                    (apiStatus, respond) = await RequestsAsync.Posts.GetPopularPost(AppSettings.PostApiLimitOnScroll, offset);
                    break;
                default:
                    return;
            }

            if (apiStatus != 200 || respond is not PostObject result || result.Data == null)
            {
                WRecyclerView.MainScrollEvent.IsLoading = false; 
                Methods.DisplayReportResult(ActivityContext, respond);
            }
            else
                LoadDataApi(apiStatus, respond, offset, typeRun);
        }

        public async Task FetchSearchForPosts(string offset, string id, string searchQuery, string type)
        {
            if (!Methods.CheckConnectivity())
                return;

            var (apiStatus, respond) = await RequestsAsync.Posts.SearchForPosts(AppSettings.PostApiLimitOnScroll, offset, id, searchQuery, type);
            if (apiStatus != 200 || respond is not PostObject result || result.Data == null)
            {
                WRecyclerView.MainScrollEvent.IsLoading = false;
                Methods.DisplayReportResult(ActivityContext, respond);
            }
            else LoadDataApi(apiStatus, respond, offset);
        }

        public void LoadDataApi(int apiStatus, dynamic respond, string offset, string typeRun = "Add")
        {
            try
            {
                if (apiStatus != 200 || respond is not PostObject result || result.Data == null)
                {
                    WRecyclerView.MainScrollEvent.IsLoading = false;
                    Methods.DisplayReportResult(ActivityContext, respond);
                }
                else
                { 
                    if (WRecyclerView.SwipeRefreshLayoutView != null && WRecyclerView.SwipeRefreshLayoutView.Refreshing)
                        WRecyclerView.SwipeRefreshLayoutView.Refreshing = false;

                    var countList = NativeFeedAdapter.ItemCount;
                    switch (result.Data.Count)
                    {
                        case > 0:
                        {
                            result.Data.RemoveAll(a => a.Publisher == null && a.UserData == null); 
                            GetAllPostLive(result.Data);

                            switch (offset)
                            {
                                case "0" when countList > 10 && typeRun == "Insert" && NativeFeedAdapter.NativePostType == NativeFeedType.Global:
                                {
                                    result.Data.Reverse();
                                    bool add = false;

                                    foreach (var post in from post in result.Data let check = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a?.PostData?.PostId == post.PostId && a.TypeView == PostFunctions.GetAdapterType(post)) where check == null select post)
                                    {
                                        add = true;
                                        NativeFeedAdapter.NewPostList.Add(post);
                                    }

                                    ActivityContext?.RunOnUiThread(() =>
                                    {
                                        try
                                        {
                                            WRecyclerView.PopupBubbleView.Visibility = add switch
                                            {
                                                true when WRecyclerView.PopupBubbleView != null &&
                                                          WRecyclerView.PopupBubbleView.Visibility !=
                                                          ViewStates.Visible &&
                                                          AppSettings.ShowNewPostOnNewsFeed => ViewStates.Visible,
                                                _ => WRecyclerView.PopupBubbleView.Visibility
                                            };
                                        }
                                        catch (Exception e)
                                        {
                                            Methods.DisplayReportResultTrack(e);
                                        }
                                    });
                                    break;
                                }
                                default:
                                {
                                    bool add = false;
                                    foreach (var post in from post in result.Data let check = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a?.PostData?.PostId == post.PostId && a?.TypeView == PostFunctions.GetAdapterType(post)) where check == null select post)
                                    {
                                        add = true;
                                        var combiner = new FeedCombiner(null, NativeFeedAdapter.ListDiffer, ActivityContext);

                                        switch (NativeFeedAdapter.NativePostType)
                                        {
                                            case NativeFeedType.Global:
                                            {
                                                switch (result.Data.Count)
                                                {
                                                    case < 6 when NativeFeedAdapter.ListDiffer.Count < 6:
                                                    {
                                                        switch (ShowFindMoreAlert)
                                                        {
                                                            case false:
                                                                ShowFindMoreAlert = true;

                                                                combiner.AddFindMoreAlertPostView("Pages");
                                                                combiner.AddFindMoreAlertPostView("Groups");
                                                                break;
                                                        }

                                                        break;
                                                    }
                                                }

                                                var check1 = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SuggestedGroupsBox);
                                                switch (check1)
                                                {
                                                    case null when AppSettings.ShowSuggestedGroup && NativeFeedAdapter.ListDiffer.Count > 0 && NativeFeedAdapter.ListDiffer.Count % AppSettings.ShowSuggestedGroupCount == 0 && ListUtils.SuggestedGroupList.Count > 0:
                                                        combiner.AddSuggestedBoxPostView(PostModelType.SuggestedGroupsBox);
                                                        break;
                                                }

                                                var check2 = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SuggestedUsersBox);
                                                switch (check2)
                                                {
                                                    case null when AppSettings.ShowSuggestedUser && NativeFeedAdapter.ListDiffer.Count > 0 && NativeFeedAdapter.ListDiffer.Count % AppSettings.ShowSuggestedUserCount == 0 && ListUtils.SuggestedUserList.Count > 0:
                                                        combiner.AddSuggestedBoxPostView(PostModelType.SuggestedUsersBox);
                                                        break;
                                                }

                                                break;
                                            }
                                        }

                                        switch (NativeFeedAdapter.ListDiffer.Count % AppSettings.ShowAdMobNativeCount)
                                        {
                                            case 0 when NativeFeedAdapter.ListDiffer.Count > 0 && AppSettings.ShowAdMobNativePost:
                                                switch (LastAdsType)
                                                {
                                                    case PostModelType.AdMob1:
                                                        LastAdsType = PostModelType.AdMob2;
                                                        combiner.AddAdsPostView(PostModelType.AdMob1);
                                                        break;
                                                    case PostModelType.AdMob2:
                                                        LastAdsType = PostModelType.AdMob3;
                                                        combiner.AddAdsPostView(PostModelType.AdMob2);
                                                        break;
                                                    case PostModelType.AdMob3:
                                                        LastAdsType = PostModelType.AdMob1;
                                                        combiner.AddAdsPostView(PostModelType.AdMob3);
                                                        break;
                                                }

                                                break;
                                        }

                                        var combine = new FeedCombiner(RegexFilterText(post), NativeFeedAdapter.ListDiffer, ActivityContext);
                                        switch (post.PostType)
                                        {
                                            case "ad":
                                                combine.AddAdsPost();
                                                break;
                                            default:
                                            {
                                                bool isPromoted = post.IsPostBoosted == "1" || post.SharedInfo.SharedInfoClass != null && post.SharedInfo.SharedInfoClass?.IsPostBoosted == "1";
                                                switch (isPromoted)
                                                {
                                                    //Promoted
                                                    case true:
                                                        combine.CombineDefaultPostSections("Top");
                                                        break;
                                                    default:
                                                        combine.CombineDefaultPostSections();
                                                        break;
                                                }

                                                break;
                                            }
                                        }

                                        switch (NativeFeedAdapter.ListDiffer.Count % AppSettings.ShowFbNativeAdsCount)
                                        {
                                            case 0 when NativeFeedAdapter.ListDiffer.Count > 0 && AppSettings.ShowFbNativeAds:
                                                combiner.AddAdsPostView(PostModelType.FbAdNative);
                                                break;
                                        }
                                    }

                                    switch (add)
                                    {
                                        case true:
                                            ActivityContext?.RunOnUiThread(() =>
                                            {
                                                try
                                                {
                                                    var d = new Runnable(() => { NativeFeedAdapter.NotifyItemRangeInserted(countList, NativeFeedAdapter.ListDiffer.Count - countList); }); d.Run();
                                                    GC.Collect();
                                                }
                                                catch (Exception e)
                                                {
                                                    Methods.DisplayReportResultTrack(e);
                                                }
                                            });
                                            break;
                                    }

                                    //else
                                    //{
                                    //    Toast.MakeText(ActivityContext, ActivityContext.GetText(Resource.String.Lbl_NoMorePost), ToastLength.Short)?.Show(); 
                                    //}
                                    break;
                                }
                            }

                            break;
                        }
                    }

                    ActivityContext?.RunOnUiThread(ShowEmptyPage);

                    WRecyclerView.DataPostJson = NativeFeedAdapter.NativePostType switch
                    {
                        NativeFeedType.Global => JsonConvert.SerializeObject(result),
                        _ => WRecyclerView.DataPostJson
                    };
                }

                WRecyclerView.MainScrollEvent.IsLoading = false;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                WRecyclerView.MainScrollEvent.IsLoading = false;
            }
        }

        public void LoadTopDataApi(List<PostDataObject> list)
        {
            try
            {
                NativeFeedAdapter.ListDiffer.Clear();
                NativeFeedAdapter.NotifyDataSetChanged();

                var combiner = new FeedCombiner(null, NativeFeedAdapter.ListDiffer, ActivityContext);
                combiner.AddPostBoxPostView("feed", -1);

                switch (AppSettings.ShowStory)
                {
                    case true:
                        combiner.AddStoryPostView("feed", -1);
                        break;
                }

                switch (list.Count)
                {
                    case > 0:
                    {
                        bool add = false;
                        foreach (var post in from post in list let check = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a?.PostData?.PostId == post.PostId && a?.TypeView == PostFunctions.GetAdapterType(post)) where check == null select post)
                        {
                            add = true;
                            var combine = new FeedCombiner(RegexFilterText(post), NativeFeedAdapter.ListDiffer, ActivityContext);
                            switch (post.PostType)
                            {
                                case "ad":
                                    combine.AddAdsPost();
                                    break;
                                default:
                                    combine.CombineDefaultPostSections();
                                    break;
                            }
                        }
                     
                        switch (PostCacheList?.Count)
                        {
                            case > 0:
                                LoadBottomDataApi(PostCacheList.Take(30).ToList());
                                break;
                        }
                     
                        switch (add)
                        {
                            case true:
                                ActivityContext?.RunOnUiThread(() =>
                                {
                                    try
                                    {
                                        NativeFeedAdapter.NotifyDataSetChanged();
                                        NativeFeedAdapter.NewPostList.Clear();
                                    }
                                    catch (Exception e)
                                    {
                                        Methods.DisplayReportResultTrack(e);
                                    }
                                });
                                break;
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

        public void LoadMemoriesDataApi(int apiStatus, dynamic respond, List<AdapterModelsClass> diffList)
        {
            try
            {
                switch (WRecyclerView.MainScrollEvent.IsLoading)
                {
                    case true:
                        return;
                }

                WRecyclerView.MainScrollEvent.IsLoading = true;

                if (apiStatus != 200 || respond is not FetchMemoriesObject result || result.Data == null)
                {
                    WRecyclerView.MainScrollEvent.IsLoading = false;
                    Methods.DisplayReportResult(ActivityContext, respond);
                }
                else
                {
                    if (WRecyclerView.SwipeRefreshLayoutView != null && WRecyclerView.SwipeRefreshLayoutView.Refreshing)
                        WRecyclerView.SwipeRefreshLayoutView.Refreshing = false;

                    var countList = NativeFeedAdapter.ItemCount;
                    switch (result.Data.Posts.Count)
                    {
                        case > 0:
                        {
                            result.Data.Posts.RemoveAll(a => a.Publisher == null && a.UserData == null);
                            result.Data.Posts.Reverse();

                            foreach (var post in from post in result.Data.Posts let check = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a?.PostData?.PostId == post.PostId && a?.TypeView == PostFunctions.GetAdapterType(post)) where check == null select post)
                            {
                                switch (post.Publisher)
                                {
                                    case null when post.UserData == null:
                                        continue;
                                    default:
                                    {
                                        var combine = new FeedCombiner(RegexFilterText(post), NativeFeedAdapter.ListDiffer, ActivityContext);
                                        combine.CombineDefaultPostSections();
                                        break;
                                    }
                                }
                            }

                            ActivityContext?.RunOnUiThread(() =>
                            {
                                try
                                {
                                    var d = new Runnable(() => { NativeFeedAdapter.NotifyItemRangeInserted(countList, NativeFeedAdapter.ListDiffer.Count - countList); }); d.Run(); 
                                    GC.Collect();
                                }
                                catch (Exception e)
                                {
                                    Methods.DisplayReportResultTrack(e);
                                }
                            });
                            break;
                        }
                    } 
                }

                WRecyclerView.MainScrollEvent.IsLoading = false;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                WRecyclerView.MainScrollEvent.IsLoading = false;
            }
        }
         
        public async Task FetchLoadMoreNewsFeedApiPosts()
        {
            try
            {
                if (!Methods.CheckConnectivity())
                    return;

                if (NativeFeedAdapter.NativePostType != NativeFeedType.Global)
                    return;
                 
                switch (PostCacheList?.Count)
                {
                    case > 40:
                        return;
                }
                  
                var diff = NativeFeedAdapter.ListDiffer;
                var list = new List<AdapterModelsClass>(diff);
                switch (list.Count)
                {
                    case <= 20:
                        return;
                }

                var item = list.LastOrDefault();
                 
                var lastItem = list.IndexOf(item);

                item = list[lastItem];

                string offset;

                switch (item.TypeView)
                {
                    case PostModelType.Divider:
                    case PostModelType.ViewProgress:
                    case PostModelType.AdMob1:
                    case PostModelType.AdMob2:
                    case PostModelType.AdMob3:
                    case PostModelType.FbAdNative:
                    case PostModelType.AdsPost:
                    case PostModelType.SuggestedGroupsBox:
                    case PostModelType.SuggestedUsersBox:
                    case PostModelType.CommentSection:
                    case PostModelType.AddCommentSection:
                        item = list.LastOrDefault(a => a.TypeView != PostModelType.Divider && a.TypeView != PostModelType.ViewProgress && a.TypeView != PostModelType.AdMob1 && a.TypeView != PostModelType.AdMob2 && a.TypeView != PostModelType.AdMob3 && a.TypeView != PostModelType.FbAdNative && a.TypeView != PostModelType.AdsPost && a.TypeView != PostModelType.SuggestedGroupsBox && a.TypeView != PostModelType.SuggestedUsersBox && a.TypeView != PostModelType.CommentSection && a.TypeView != PostModelType.AddCommentSection);
                        offset = item?.PostData?.PostId ?? "0";
                        Console.WriteLine(offset);
                        break;
                    default:
                        offset = item.PostData?.PostId ?? "0";
                        break;
                }

                Console.WriteLine(offset);
              
                int apiStatus;
                dynamic respond;

                switch (NativeFeedAdapter.NativePostType)
                {
                    case NativeFeedType.Global:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_news_feed", NativeFeedAdapter.IdParameter, "", WRecyclerView.Filter);
                        break; 
                    default:
                        return;
                }

                if (apiStatus != 200 || respond is not PostObject result || result.Data == null)
                { 
                    Methods.DisplayReportResult(ActivityContext, respond);
                }
                else
                {
                    PostCacheList ??= new List<PostDataObject>();

                    var countList = PostCacheList?.Count ?? 0;
                    switch (result?.Data?.Count)
                    {
                        case > 0:
                        {
                            result.Data.RemoveAll(a => a.Publisher == null && a.UserData == null);
                         
                            switch (countList)
                            {
                                case > 0:
                                {
                                    foreach (var post in from post in result.Data let check = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a?.PostData?.PostId == post.PostId && a?.TypeView == PostFunctions.GetAdapterType(post)) where check == null select post)
                                    {
                                        PostCacheList.Add(post);
                                    }

                                    break;
                                }
                                default:
                                    PostCacheList = new List<PostDataObject>(result.Data);
                                    break;
                            }

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
          
        public bool LoadBottomDataApi(List<PostDataObject> list)
        {
            try
            {
                var countList = NativeFeedAdapter.ItemCount;
                switch (list?.Count)
                {
                    case > 0:
                    {
                        bool add = false;
                        foreach (var post in from post in list let check = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a?.PostData?.PostId == post.PostId && a?.TypeView == PostFunctions.GetAdapterType(post)) where check == null select post)
                        {
                            add = true;
                            var combiner = new FeedCombiner(null, NativeFeedAdapter.ListDiffer, ActivityContext);

                            switch (NativeFeedAdapter.NativePostType)
                            {
                                case NativeFeedType.Global:
                                {
                                    var check1 = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SuggestedGroupsBox);
                                    switch (check1)
                                    {
                                        case null when AppSettings.ShowSuggestedGroup && NativeFeedAdapter.ListDiffer.Count > 0 && NativeFeedAdapter.ListDiffer.Count % AppSettings.ShowSuggestedGroupCount == 0 && ListUtils.SuggestedGroupList.Count > 0:
                                            combiner.AddSuggestedBoxPostView(PostModelType.SuggestedGroupsBox);
                                            break;
                                    }

                                    var check2 = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SuggestedUsersBox);
                                    switch (check2)
                                    {
                                        case null when AppSettings.ShowSuggestedUser && NativeFeedAdapter.ListDiffer.Count > 0 && NativeFeedAdapter.ListDiffer.Count % AppSettings.ShowSuggestedUserCount == 0 && ListUtils.SuggestedUserList.Count > 0:
                                            combiner.AddSuggestedBoxPostView(PostModelType.SuggestedUsersBox);
                                            break;
                                    }

                                    break;
                                }
                            }

                            switch (NativeFeedAdapter.ListDiffer.Count % AppSettings.ShowAdMobNativeCount)
                            {
                                case 0 when NativeFeedAdapter.ListDiffer.Count > 0 && AppSettings.ShowAdMobNativePost:
                                    switch (LastAdsType)
                                    {
                                        case PostModelType.AdMob1:
                                            LastAdsType = PostModelType.AdMob2;
                                            combiner.AddAdsPostView(PostModelType.AdMob1);
                                            break;
                                        case PostModelType.AdMob2:
                                            LastAdsType = PostModelType.AdMob3;
                                            combiner.AddAdsPostView(PostModelType.AdMob2);
                                            break;
                                        case PostModelType.AdMob3:
                                            LastAdsType = PostModelType.AdMob1;
                                            combiner.AddAdsPostView(PostModelType.AdMob3);
                                            break;
                                    }

                                    break;
                            }

                            var combine = new FeedCombiner(RegexFilterText(post), NativeFeedAdapter.ListDiffer, ActivityContext);
                            switch (post.PostType)
                            {
                                case "ad":
                                    combine.AddAdsPost();
                                    break;
                                default:
                                {
                                    bool isPromoted = post.IsPostBoosted == "1" || post.SharedInfo.SharedInfoClass != null && post.SharedInfo.SharedInfoClass?.IsPostBoosted == "1";
                                    switch (isPromoted)
                                    {
                                        //Promoted
                                        case true:
                                            combine.CombineDefaultPostSections("Top");
                                            break;
                                        default:
                                            combine.CombineDefaultPostSections();
                                            break;
                                    }

                                    break;
                                }
                            }

                            switch (NativeFeedAdapter.ListDiffer.Count % AppSettings.ShowFbNativeAdsCount)
                            {
                                case 0 when NativeFeedAdapter.ListDiffer.Count > 0 && AppSettings.ShowFbNativeAds:
                                    combiner.AddAdsPostView(PostModelType.FbAdNative);
                                    break;
                            }
                        }

                        switch (add)
                        {
                            case true:
                                ActivityContext?.RunOnUiThread(() =>
                                {
                                    try
                                    {
                                        var d = new Runnable(() => { NativeFeedAdapter.NotifyItemRangeInserted(countList, NativeFeedAdapter.ListDiffer.Count - countList); }); d.Run();
                                        GC.Collect(); 
                                    }
                                    catch (Exception e)
                                    {
                                        Methods.DisplayReportResultTrack(e);
                                    }
                                });
                                break;
                        }

                        PostCacheList.RemoveRange(0, list.Count - 1); 
                        ActivityContext?.RunOnUiThread(ShowEmptyPage);

                        return add;
                    }
                    default:
                        return false;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return false;
            }
        }

        private void ShowEmptyPage()
        {
            try
            {
                NativeFeedAdapter.SetLoaded();
                var viewProgress = NativeFeedAdapter.ListDiffer.FirstOrDefault(anjo => anjo.TypeView == PostModelType.ViewProgress);
                if (viewProgress != null)
                    WRecyclerView.RemoveByRowIndex(viewProgress);

                var emptyStateCheck = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a.PostData != null && a.TypeView != PostModelType.AddPostBox && a.TypeView != PostModelType.FilterSection && a.TypeView != PostModelType.SearchForPosts);
                if (emptyStateCheck != null)
                {
                    var emptyStateChecker = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);
                    if (emptyStateChecker != null && NativeFeedAdapter.ListDiffer.Count > 1)
                        WRecyclerView.RemoveByRowIndex(emptyStateChecker);
                }
                else
                {
                    var emptyStateChecker = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);
                    switch (emptyStateChecker)
                    {
                        case null:
                        {
                            var data = new AdapterModelsClass
                            {
                                TypeView = PostModelType.EmptyState,
                                Id = 744747447,
                            };
                            NativeFeedAdapter.ListDiffer.Add(data);
                            NativeFeedAdapter.NotifyItemInserted(NativeFeedAdapter.ListDiffer.IndexOf(data));
                            break;
                        }
                    }
                }

                WRecyclerView.MainScrollEvent.IsLoading = false;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        private void GetAllPostLive(List<PostDataObject> list)
        {
            try
            {
                var listLivePost = list?.Where(a => a.LiveTime != null && a.LiveTime.Value > 0 && a.IsStillLive != null && a.IsStillLive.Value && string.IsNullOrEmpty(a.AgoraResourceId) && string.IsNullOrEmpty(a.PostFile))?.ToList();
                switch (NativeFeedAdapter.NativePostType)
                {
                    case NativeFeedType.Global:
                        var mainActivity = TabbedMainActivity.GetInstance();
                        var checkSection = mainActivity?.NewsFeedTab?.PostFeedAdapter?.ListDiffer?.FirstOrDefault(a => a.TypeView == PostModelType.Story);
                        if (checkSection != null)
                        {
                            if (listLivePost?.Count > 0)
                            {
                                foreach (var post in from post in listLivePost let check = checkSection.StoryList.FirstOrDefault(a => a?.DataLivePost?.PostId == post.PostId) where check == null select post)
                                { 
                                    if (checkSection.StoryList.Count > 1)
                                    {
                                        checkSection.StoryList.Insert(1, new StoryDataObject
                                        {
                                            Avatar = post.Publisher.Avatar,
                                            Type = "Live",
                                            Username = ActivityContext.GetText(Resource.String.Lbl_Live),
                                            DataLivePost = post
                                        });
                                    }
                                    else
                                    {
                                        checkSection.StoryList.Add(new StoryDataObject
                                        { 
                                            Avatar = post.Publisher.Avatar,
                                            Type = "Live",
                                            Username = WoWonderTools.GetNameFinal(post.Publisher),
                                            DataLivePost = post,
                                        });
                                    } 
                                }

                                ActivityContext?.RunOnUiThread(() =>
                                {
                                    try
                                    {
                                        var d = new Runnable(() => { mainActivity?.NewsFeedTab?.PostFeedAdapter.NotifyItemChanged(mainActivity.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(checkSection)); });
                                        d.Run();
                                    }
                                    catch (Exception e)
                                    {
                                        Methods.DisplayReportResultTrack(e);
                                    }
                                }); 
                            } 
                        } 
                        break;
                    case NativeFeedType.User when NativeFeedAdapter.IdParameter != UserDetails.UserId:
                        var userProfileActivity = UserProfileActivity.GetInstance();
                        if (userProfileActivity != null)
                        {
                            if (listLivePost?.Count > 0)
                            {
                                userProfileActivity.DataLivePost = listLivePost.FirstOrDefault();
                                userProfileActivity.LiveLayout.Visibility = ViewStates.Visible;
                            }
                            else
                            {
                                userProfileActivity.DataLivePost = null;
                                userProfileActivity.LiveLayout.Visibility = ViewStates.Gone;
                            }
                        }

                        break;
                    default:
                        return;
                }
                 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }


        public void InsertTheLatestPosts()
        {
            try
            {
                switch (string.IsNullOrEmpty(WRecyclerView.DataPostJson))
                {
                    case false:
                    {
                        SqLiteDatabase dbDatabase = new SqLiteDatabase();
                        dbDatabase.InsertOrUpdatePost(WRecyclerView.DataPostJson);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static PostDataObject RegexFilterText(PostDataObject item)
        {
            try
            {
                Dictionary<string, string> dataUser = new Dictionary<string, string>();

                if (string.IsNullOrEmpty(item.PostText))
                    return item;

                if (item.PostText.Contains("data-id="))
                {
                    try
                    {
                        //string pattern = @"(data-id=[""'](.*?)[""']|href=[""'](.*?)[""']|'>(.*?)a>)";

                        string pattern = @"(data-id=[""'](.*?)[""']|href=[""'](.*?)[""'])";
                        var aa = Regex.Matches(item.PostText, pattern);
                        switch (aa?.Count)
                        {
                            case > 0:
                            {
                                for (int i = 0; i < aa.Count; i++)
                                { 
                                    string userid = ""; 
                                    if (aa.Count > i)
                                        userid = aa[i]?.Value?.Replace("data-id=", "").Replace('"', ' ').Replace(" ", "");
                                
                                    string username = ""; 
                                    if (aa.Count > i + 1)
                                        username = aa[i + 1]?.Value?.Replace("href=", "").Replace('"', ' ').Replace(" ", "").Replace(Client.WebsiteUrl, "").Replace("\n", "");

                                    if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(username))
                                        continue;

                                    var data = dataUser.FirstOrDefault(a => a.Key?.ToString() == userid && a.Value?.ToString() == username);
                                    if (data.Key != null) 
                                        continue;

                                    i++;

                                    switch (string.IsNullOrWhiteSpace(userid))
                                    {
                                        case false when !string.IsNullOrWhiteSpace(username) && !dataUser.ContainsKey(userid):
                                            dataUser.Add(userid, username);
                                            break;
                                    }
                                }

                                item.RegexFilterList = new Dictionary<string, string>(dataUser);
                                return item;
                            }
                            default:
                                return item;
                        }
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }

                return item;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return item;
            }
        }
         
        public static async Task FetchFirstNewsFeedApiPosts()
        {
            try
            {
                if (!Methods.CheckConnectivity())
                    return;
                 
                var (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnBackground); 
                if (apiStatus != 200 || respond is not PostObject result)
                {
                    //Methods.DisplayReportResult(ActivityContext, respond);
                }
                else
                {
                    switch (result?.Data?.Count)
                    {
                        case > 0:
                        {
                            result.Data.RemoveAll(a => a.Publisher == null && a.UserData == null);
                            SqLiteDatabase dbDatabase = new SqLiteDatabase();
                            //Insert All data to database
                            dbDatabase.InsertOrUpdatePost(JsonConvert.SerializeObject(result));
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
    }
}
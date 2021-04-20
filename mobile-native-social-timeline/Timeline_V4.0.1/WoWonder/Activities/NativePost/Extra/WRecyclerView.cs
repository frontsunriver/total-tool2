using Android.Content;
using Android.Runtime; 
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Google.Android.Exoplayer2;
using Com.Google.Android.Exoplayer2.Source;
using Com.Google.Android.Exoplayer2.Source.Dash;
using Com.Google.Android.Exoplayer2.Source.Hls;
using Com.Google.Android.Exoplayer2.Source.Smoothstreaming;
using Com.Google.Android.Exoplayer2.Trackselection;
using Com.Google.Android.Exoplayer2.UI;
using Com.Google.Android.Exoplayer2.Upstream;
using Com.Google.Android.Exoplayer2.Upstream.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using Com.Google.Android.Exoplayer2.Database;
using Com.Google.Android.Exoplayer2.Drm;
using Com.Google.Android.Exoplayer2.Extractor.TS;
using Com.Google.Android.Exoplayer2.Offline;
using Google.Android.Material.FloatingActionButton;
using Java.Util;
using WoWonder.Activities.NativePost.Pages;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Requests;
using Console = System.Console;
using DownloadManager = Com.Google.Android.Exoplayer2.Offline.DownloadManager;
using Exception = System.Exception;
using LayoutDirection = Android.Views.LayoutDirection;
using Object = Java.Lang.Object;
using Uri = Android.Net.Uri;
using Util = Com.Google.Android.Exoplayer2.Util.Util;

namespace WoWonder.Activities.NativePost.Extra
{
    public class WRecyclerView : RecyclerView/*, IOnLoadMoreListener*/
    {
        private static WRecyclerView Instance;
        private enum VolumeState { On, Off }

        private FrameLayout MediaContainerLayout;
        private ImageView Thumbnail, PlayControl;
        private PlayerView VideoSurfaceView;
        private SimpleExoPlayer VideoPlayer;
        private View ViewHolderParent;
        public View ViewHolderVoicePlayer;
        private int VideoSurfaceDefaultHeight;
        private int ScreenDefaultHeight;
        private Context MainContext;
        private bool IsVideoViewAdded;
        public bool IsVoicePlayed;
        private Uri VideoUrl;
        public string Hash;
        public RecyclerScrollListener MainScrollEvent;
        public NativePostAdapter NativeFeedAdapter;
        public SwipeRefreshLayout SwipeRefreshLayoutView;
        public FloatingActionButton PopupBubbleView;
        private VolumeState VolumeStateProvider;
         
        private CacheDataSourceFactory CacheDataSourceFactory;
        private static SimpleCache Cache;
        private static DefaultBandwidthMeter BandwidthMeter;
        private DefaultDataSourceFactory DefaultDataSourceFac;
        public static string Filter { set; get; }
        public static string DataPostJson = "";
        public ApiPostAsync ApiPostAsync;

        protected WRecyclerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public WRecyclerView(Context context) : base(context)
        {
            Init(context);
        }

        public WRecyclerView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context);
        }

        public WRecyclerView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Init(context);
        }

        private void Init(Context context)
        {
            try
            {
                MainContext = context;

                Instance = this;

                LayoutDirection = AppSettings.FlowDirectionRightToLeft switch
                {
                    true => LayoutDirection.Rtl,
                    _ => LayoutDirection
                };

                HasFixedSize = true;
                SetItemViewCacheSize(50);
                //SetItemAnimator(new DefaultItemAnimator());
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.ColorPost, 15);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.ImagePost, 30);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.TextSectionPostPart, 30);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.BottomPostPart, 30);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.PrevBottomPostPart, 40);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.HeaderPost, 30);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.PromotePost, 10);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.SharedHeaderPost, 10);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.Story, 1);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.SuggestedGroupsBox, 1);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.SuggestedUsersBox, 1);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.MultiImage2, 10);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.MultiImage3, 10);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.MultiImage4, 10);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.PollPost, 20);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.AlertBox, 2);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.YoutubePost, 15);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.VideoPost, 15);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.AdMob1, 10);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.AdMob2, 10);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.AdMob3, 10);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.AddPostBox, 1);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.AdsPost, 6);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.DeepSoundPost, 5);
                GetRecycledViewPool().SetMaxRecycledViews((int)PostModelType.Divider, 20);

                ClearAnimation();
                var f = GetItemAnimator();
                ((SimpleItemAnimator)f).SupportsChangeAnimations = false;
                f.ChangeDuration = 0;

                //DividerItemDecoration itemDecorator = new DividerItemDecoration(MainContext, DividerItemDecoration.Vertical);
                //itemDecorator.SetDrawable(ContextCompat.GetDrawable(MainContext, Resource.Drawable.Post_Devider_Shape));
                //AddItemDecoration(itemDecorator);

                var point = Methods.App.OverrideGetSize(MainContext);
                if (point != null)
                {
                    VideoSurfaceDefaultHeight = point.X;
                    ScreenDefaultHeight = point.Y;
                }

                VideoSurfaceView = new PlayerView(MainContext)
                {
                    ResizeMode = AspectRatioFrameLayout.ResizeModeFixedWidth
                };

                //===================== Exo Player ========================
                SetPlayer();
                //=============================================

                MainScrollEvent = new RecyclerScrollListener(this);
                AddOnScrollListener(MainScrollEvent);
                AddOnChildAttachStateChangeListener(new ChildAttachStateChangeListener(this));
                MainScrollEvent.LoadMoreEvent += MainScrollEvent_LoadMoreEvent;
                MainScrollEvent.IsLoading = false; 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static WRecyclerView GetInstance()
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

        public void SetXAdapter(NativePostAdapter adapter, SwipeRefreshLayout swipeRefreshLayout)
        {
            try
            {
                NativeFeedAdapter = adapter;
                //NativeFeedAdapter.SetOnLoadMoreListener(this);
                SwipeRefreshLayoutView = swipeRefreshLayout;
                ApiPostAsync = new ApiPostAsync(this, adapter);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        /// <summary>
        /// All
        /// People I Follow 
        /// </summary>
        /// <param name="filter">0,1</param>
        public void SetFilter(string filter)
        {
            try
            {
                Filter = filter;

                var tab = TabbedMainActivity.GetInstance()?.NewsFeedTab;
                if (tab != null)
                {
                    tab.SwipeRefreshLayout.Refreshing = true;
                    tab.SwipeRefreshLayoutOnRefresh(this, EventArgs.Empty);
                    tab.RemoveHandler();

                    tab.PostFeedAdapter.ListDiffer.Clear();
                    tab.PostFeedAdapter.NotifyDataSetChanged();

                    tab.LoadPost(false);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public string GetFilter()
        {
            return Filter ?? "";
        }

        //PopupBubble 
        public void SetXPopupBubble(FloatingActionButton popupBubble)
        {
            try
            {
                if (popupBubble != null)
                {
                    PopupBubbleView = popupBubble;
                    PopupBubbleView.Visibility = ViewStates.Gone;
                    PopupBubbleView.Click += PopupBubbleViewOnClick;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void PopupBubbleViewOnClick(object sender, EventArgs e)
        {
            try
            {
                ApiPostAsync.LoadTopDataApi(NativeFeedAdapter.NewPostList);

                PopupBubbleView.Visibility = ViewStates.Gone;
                ScrollToPosition(0); 
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //play the video in the row
        private void PlayVideo(bool isEndOfList)
        {
            try
            {
                Console.WriteLine(isEndOfList);
                switch (VideoSurfaceView)
                {
                    case null:
                        return;
                }
                 
                switch (IsVideoViewAdded)
                {
                    case true when ViewHolderParent != null:
                        return;
                }

                int currentPosition = ((LinearLayoutManager)Objects.RequireNonNull(GetLayoutManager())).FindFirstCompletelyVisibleItemPosition() + 3;
                var typePost = NativeFeedAdapter.GetItem(currentPosition);
                switch (typePost)
                {
                    case null:
                        return;
                }

                var postFeedType = PostFunctions.GetAdapterType(typePost.PostData);
                if (postFeedType != PostModelType.VideoPost) 
                    return;
                 
                var item = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.VideoPost && a.PostData.PostId == typePost.PostData.PostId);
                int indexPost = NativeFeedAdapter.ListDiffer.IndexOf(item);

                var child = ((LinearLayoutManager)GetLayoutManager()).FindViewByPosition(indexPost); 
                var holder = (AdapterHolders.PostVideoSectionViewHolder) child?.Tag;
                switch (holder)
                {
                    case null:
                        return;
                }

                ResetMediaPlayer();

                // remove any old surface views from previously playing videos
                VideoSurfaceView.Visibility = ViewStates.Invisible;
                RemoveVideoView(VideoSurfaceView);

                switch (VideoPlayer)
                {
                    case null:
                        SetPlayer();
                        break;
                }

                MediaContainerLayout = holder.MediaContainer;
                Thumbnail = holder.VideoImage;
                ViewHolderParent = holder.ItemView;
                PlayControl = holder.PlayControl;

                switch (IsVideoViewAdded)
                {
                    case false:
                        AddVideoView();
                        break;
                }

                VideoSurfaceView.Player = VideoPlayer;

                var controlView = VideoSurfaceView.FindViewById<PlayerControlView>(Resource.Id.exo_controller);

                VideoSurfaceView.SetMinimumHeight(holder.VideoImage.Height);
                controlView.SetMinimumHeight(holder.VideoImage.Height);
                         
                VideoUrl = Uri.Parse(item.PostData.PostFileFull);

                holder.VideoUrl = VideoUrl.ToString();

                if (item.PostData.Blur != "0")
                    return;

                TabbedMainActivity.GetInstance()?.SetOnWakeLock();

                //>> New Code 
                //===================== Exo Player ======================== 
                var lis = new ExoPlayerRecyclerEvent(controlView, this, holder);
                lis.MFullScreenButton.SetOnClickListener(new NewClicker(lis.MFullScreenButton, holder.VideoUrl, this));

                var dataSpec = new DataSpec(VideoUrl); //0, 1000 * 1024, null

                switch (Cache)
                {
                    case null:
                        CacheVideosFiles(VideoUrl);
                        break;
                }

                CacheDataSourceFactory ??= new CacheDataSourceFactory(Cache, DefaultDataSourceFac);

                CacheUtil.GetCached(dataSpec, Cache, new MyCacheKeyFactory());
                var videoSource = GetMediaSourceFromUrl(VideoUrl, VideoUrl?.Path?.Split('.').Last(), "normal");

                VideoPlayer.Prepare(videoSource);
                VideoPlayer.PlayWhenReady = true;

                //VideoPlayer.AddListener(null);
                 
                if (Methods.CheckConnectivity())
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.GetPostDataAsync(item.PostData.PostId, "post_data", "1") });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void PlayVideo(bool isEndOfList, AdapterHolders.PostVideoSectionViewHolder holder, PostDataObject item)
        {
            try
            {
                Console.WriteLine(isEndOfList);
                switch (VideoPlayer)
                {
                    case null:
                        SetPlayer();
                        break;
                }

                switch (VideoSurfaceView)
                {
                    case null:
                        return;
                }

                ResetMediaPlayer();

                VideoSurfaceView.Visibility = ViewStates.Invisible;
                RemoveVideoView(VideoSurfaceView);

                MediaContainerLayout = holder.MediaContainer;
                Thumbnail = holder.VideoImage;
                ViewHolderParent = holder.ItemView;
                PlayControl = holder.PlayControl;

                switch (IsVideoViewAdded)
                {
                    case false:
                        AddVideoView();
                        break;
                }

                VideoSurfaceView.Player = VideoPlayer;

                var controlView = VideoSurfaceView.FindViewById<PlayerControlView>(Resource.Id.exo_controller);
                VideoUrl = Uri.Parse(item.PostFileFull);

                VideoSurfaceView.SetMinimumHeight(holder.VideoImage.Height);
                controlView.SetMinimumHeight(holder.VideoImage.Height);

                holder.VideoUrl = VideoUrl.ToString();

                if (item.Blur != "0")
                    return;

                TabbedMainActivity.GetInstance()?.SetOnWakeLock();

                //>> New Code 
                //===================== Exo Player ======================== 
                var lis = new ExoPlayerRecyclerEvent(controlView, this, holder);
                lis.MFullScreenButton.SetOnClickListener(new NewClicker(lis.MFullScreenButton, holder.VideoUrl, this));

                var dataSpec = new DataSpec(VideoUrl); //0, 1000 * 1024, null

                switch (Cache)
                {
                    case null:
                        CacheVideosFiles(VideoUrl);
                        break;
                }

                CacheDataSourceFactory ??= new CacheDataSourceFactory(Cache, DefaultDataSourceFac);

                CacheUtil.GetCached(dataSpec, Cache, new MyCacheKeyFactory());
                var videoSource = GetMediaSourceFromUrl(VideoUrl, VideoUrl?.Path?.Split('.').Last(), "normal");

                VideoPlayer.Prepare(videoSource);
                VideoPlayer.PlayWhenReady = true;

                //VideoPlayer.AddListener(null);

                if (Methods.CheckConnectivity())
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.GetPostDataAsync(item.PostId, "post_data", "1") });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private class NewClicker : Object, IOnClickListener
        {
            private string VideoUrl { get; set; }
            private FrameLayout FullScreenButton { get; set; }
            private WRecyclerView WRecyclerViewController { get; set; }
            public NewClicker(FrameLayout fullScreenButton, string videoUrl, WRecyclerView wRecyclerViewController)
            {
                WRecyclerViewController = wRecyclerViewController;
                FullScreenButton = fullScreenButton;
                VideoUrl = videoUrl;
            }
            public void OnClick(View v)
            {
                if (v.Id == FullScreenButton.Id)
                {
                    try
                    {

                        WRecyclerViewController.VideoPlayer.PlayWhenReady = false;

                        Intent intent = new Intent(WRecyclerViewController.MainContext, typeof(VideoFullScreenActivity));
                        intent.PutExtra("videoUrl", VideoUrl);
                        //  intent.PutExtra("videoDuration", videoPlayer.Duration.ToString());
                        MainApplication.GetInstance().Activity.StartActivity(intent);
                    }
                    catch (Exception exception)
                    {
                        Methods.DisplayReportResultTrack(exception);
                    }
                }
            }
        }

        private void MainScrollEvent_LoadMoreEvent(object sender, EventArgs e)
        {
            try
            {
                switch (NativeFeedAdapter.NativePostType)
                {
                    case NativeFeedType.Memories:
                    case NativeFeedType.Share:
                        return;
                }

                var diff = NativeFeedAdapter.ListDiffer;
                var list = new List<AdapterModelsClass>(diff);
                switch (list.Count)
                {
                    case <= 20 when !MainScrollEvent.IsLoading:
                        return;
                }

                NativeFeedAdapter.SetLoading();

                //if (ApiPostAsync.PostCacheList?.Count > 0 && NativeFeedAdapter.NativePostType == NativeFeedType.Global)
                //{
                //  var addedData = ApiPostAsync.LoadBottomDataApi(ApiPostAsync.PostCacheList.Take(20).ToList());
                //  if (addedData)
                //      return;
                //}
                 
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

                if (!Methods.CheckConnectivity())
                    Toast.MakeText(MainContext, MainContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                else
                {
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => NativeFeedAdapter.NativePostType != NativeFeedType.HashTag ? ApiPostAsync.FetchNewsFeedApiPosts(offset) : ApiPostAsync.FetchNewsFeedApiPosts(offset, "Add", Hash) });
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void InsertByRowIndex(AdapterModelsClass item, string index = "")
        {
            try
            {
                var diff = NativeFeedAdapter.ListDiffer;
                var diffList = new List<AdapterModelsClass>(diff);

                int countIndex = 1;
                var model1 = diffList.FirstOrDefault(a => a.TypeView == PostModelType.Story);
                var model2 = diffList.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                var model3 = diffList.FirstOrDefault(a => a.TypeView == PostModelType.AlertBox);
                var model4 = diffList.FirstOrDefault(a => a.TypeView == PostModelType.SearchForPosts);

                if (model4 != null)
                    countIndex += diffList.IndexOf(model4);
                else if (model3 != null)
                    countIndex += diffList.IndexOf(model3);
                else if (model2 != null)
                    countIndex += diffList.IndexOf(model2);
                else if (model1 != null)
                    countIndex += diffList.IndexOf(model1);
                else
                    countIndex = 0;

                countIndex = string.IsNullOrEmpty(index) switch
                {
                    false => Convert.ToInt32(index),
                    _ => countIndex
                };

                diffList.Insert(countIndex, item);

                var emptyStateChecker = diffList.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);
                if (emptyStateChecker != null && diffList.Count > 1)
                {
                    diffList.Remove(emptyStateChecker);

                }

                NativeFeedAdapter.NotifyItemRangeInserted(diff.Count - 1, diffList.Count);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void RemoveByRowIndex(AdapterModelsClass item)
        {
            try
            {
                var diff = NativeFeedAdapter.ListDiffer;
                var index = diff.IndexOf(item);
                switch (index)
                {
                    case <= 0:
                        return;
                    default:
                        diff.RemoveAt(index);
                        NativeFeedAdapter.NotifyItemRemoved(index);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        protected override void OnDetachedFromWindow()
        {
            try
            {
                base.OnDetachedFromWindow();
                if (GetAdapter() != null)
                {
                    SetAdapter(null);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public class RecyclerScrollListener : OnScrollListener
        {
            public delegate void LoadMoreEventHandler(object sender, EventArgs e);

            public event LoadMoreEventHandler LoadMoreEvent;

            public bool IsLoading { get; set; }
            public bool IsScrolling { get; set; }

            private PreCachingLayoutManager LayoutManager { get; set; }
            private readonly WRecyclerView XRecyclerView;

            public RecyclerScrollListener(WRecyclerView recyclerView)
            { 
                try
                {
                    XRecyclerView = recyclerView;
                    LayoutManager ??= (PreCachingLayoutManager)recyclerView.GetLayoutManager();
                    IsLoading = false;
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
            {
                try
                {
                    base.OnScrollStateChanged(recyclerView, newState);

                    switch (newState)
                    {
                        case (int)Android.Widget.ScrollState.TouchScroll:
                        //if (Glide.With(XRecyclerView.Context).IsPaused)
                        //    Glide.With(XRecyclerView.Context).ResumeRequests();
                        case (int)Android.Widget.ScrollState.Fling:
                            IsScrolling = true;
                            //Glide.With(XRecyclerView.Context).PauseRequests();
                            break;
                        case (int) Android.Widget.ScrollState.Idle:
                        {
                            switch (AppSettings.AutoPlayVideo)
                            {
                                // There's a special case when the end of the list has been reached.
                                // Need to handle that with this bit of logic
                                case true:
                                    XRecyclerView.PlayVideo(!recyclerView.CanScrollVertically(1));
                                    break;
                            }
                              
                            XRecyclerView?.ApiPostAsync?.FetchLoadMoreNewsFeedApiPosts().ConfigureAwait(false);

                            //if (Glide.With(XRecyclerView.Context).IsPaused)
                            //    Glide.With(XRecyclerView.Context).ResumeRequests();

                            break;

                        }
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
            {
                try
                {
                    base.OnScrolled(recyclerView, dx, dy);

                    var visibleItemCount = recyclerView.ChildCount;
                    var totalItemCount = recyclerView.GetAdapter().ItemCount;

                    LayoutManager ??= (PreCachingLayoutManager)recyclerView.GetLayoutManager();

                    int pastVisibleItems = LayoutManager.FindFirstVisibleItemPosition();
                      
                    if (visibleItemCount + pastVisibleItems + 30 < totalItemCount)
                        return;

                    switch (IsLoading)
                    {
                        //&& !recyclerView.CanScrollVertically(1)
                        case true:
                            return;
                        default:
                            //IsLoading = true;
                            LoadMoreEvent?.Invoke(this, null);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }
        }

        #region Listeners

        private class ChildAttachStateChangeListener : Object, IOnChildAttachStateChangeListener
        {
            private readonly WRecyclerView XRecyclerView;

            public ChildAttachStateChangeListener(WRecyclerView recyclerView)
            {
                XRecyclerView = recyclerView;
            }

            public void OnChildViewAttachedToWindow(View view)
            {
                try
                { 
                    if (XRecyclerView.ViewHolderParent != null && XRecyclerView.ViewHolderParent.Equals(view))
                    {
                        switch (XRecyclerView.IsVideoViewAdded)
                        {
                            case false:
                                return;
                        }

                        XRecyclerView.RemoveVideoView(XRecyclerView.VideoSurfaceView);
                        XRecyclerView.VideoSurfaceView.Visibility = ViewStates.Invisible;

                        XRecyclerView.ReleasePlayer();

                        if (XRecyclerView.Thumbnail != null)
                        {
                            XRecyclerView.Thumbnail.Visibility = ViewStates.Visible;
                            switch (string.IsNullOrEmpty(XRecyclerView.VideoUrl.Path))
                            {
                                case false:
                                    XRecyclerView.NativeFeedAdapter.FullGlideRequestBuilder.Load(XRecyclerView.VideoUrl).Into(XRecyclerView.Thumbnail);
                                    break;
                            }
                        }

                        if (XRecyclerView.PlayControl != null) XRecyclerView.PlayControl.Visibility = ViewStates.Visible;

                        var mainHolder = XRecyclerView.GetChildViewHolder(view);
                        if (!(mainHolder is AdapterHolders.PostVideoSectionViewHolder holder))
                            return;

                        holder.VideoImage.Visibility = ViewStates.Visible;
                        holder.PlayControl.Visibility = ViewStates.Visible;
                        holder.VideoProgressBar.Visibility = ViewStates.Gone;
                    }
                    else if (XRecyclerView.ViewHolderVoicePlayer != null && XRecyclerView.ViewHolderVoicePlayer.Equals(view))
                    {
                        switch (XRecyclerView.IsVoicePlayed)
                        {
                            case false:
                                return;
                            default:
                                XRecyclerView.ResetMediaPlayer();
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public void OnChildViewDetachedFromWindow(View view)
            {
                try
                {
                    if (XRecyclerView.ViewHolderParent != null && XRecyclerView.ViewHolderParent.Equals(view))
                    {
                        switch (XRecyclerView.IsVideoViewAdded)
                        {
                            case false:
                                return;
                        }

                        XRecyclerView.RemoveVideoView(XRecyclerView.VideoSurfaceView);
                        XRecyclerView.VideoSurfaceView.Visibility = ViewStates.Invisible;

                        XRecyclerView.ReleasePlayer();

                        if (XRecyclerView.Thumbnail != null)
                        {
                            XRecyclerView.Thumbnail.Visibility = ViewStates.Visible;
                            switch (string.IsNullOrEmpty(XRecyclerView.VideoUrl.Path))
                            {
                                case false:
                                    XRecyclerView.NativeFeedAdapter.FullGlideRequestBuilder.Load(XRecyclerView.VideoUrl).Into(XRecyclerView.Thumbnail);
                                    break;
                            }
                        }

                        if (XRecyclerView.PlayControl != null) XRecyclerView.PlayControl.Visibility = ViewStates.Visible;

                        var mainHolder = XRecyclerView.GetChildViewHolder(view);
                        if (!(mainHolder is AdapterHolders.PostVideoSectionViewHolder holder))
                            return;

                        holder.VideoImage.Visibility = ViewStates.Visible;
                        holder.PlayControl.Visibility = ViewStates.Visible;
                        holder.VideoProgressBar.Visibility = ViewStates.Gone;
                    }
                    else if (XRecyclerView.ViewHolderVoicePlayer != null && XRecyclerView.ViewHolderVoicePlayer.Equals(view))
                    {
                        switch (XRecyclerView.IsVoicePlayed)
                        {
                            case false:
                                return;
                        }

                        XRecyclerView.ResetMediaPlayer();

                        var holder = new AdapterHolders.SoundPostViewHolder(XRecyclerView.ViewHolderVoicePlayer, XRecyclerView.NativeFeedAdapter, null);
                        if (holder != null)
                        {
                            holder.LoadingProgressView.Visibility = ViewStates.Gone;
                            holder.PlayButton.Visibility = ViewStates.Visible;
                            holder.PlayButton.SetImageResource(Resource.Drawable.icon_player_play);
                            holder.PlayButton.Tag = "Play";

                            switch (Build.VERSION.SdkInt)
                            {
                                case >= BuildVersionCodes.N:
                                    holder.SeekBar.SetProgress(0, true);
                                    break;
                                // For API < 24 
                                default:
                                    holder.SeekBar.Progress = 0;
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

        private class ExoPlayerRecyclerEvent : Object, IPlayerEventListener, PlayerControlView.IVisibilityListener, PlayerControlView.IProgressUpdateListener
        {
            private readonly ProgressBar LoadingProgressBar;
            private readonly ImageButton VideoPlayButton, VideoResumeButton;
            private readonly ImageView VolumeControl;
            public readonly FrameLayout MFullScreenButton;
            private readonly WRecyclerView XRecyclerView;
            private readonly SimpleExoPlayer Player;
            public ExoPlayerRecyclerEvent(PlayerControlView controlView, WRecyclerView recyclerView, AdapterHolders.PostVideoSectionViewHolder holder)
            {
                try
                {
                    XRecyclerView = recyclerView;
                    switch (controlView)
                    {
                        case null:
                            return;
                    }
                    Player = XRecyclerView.VideoPlayer;
                    //player.AddListener(this);

                    var mFullScreenIcon = controlView.FindViewById<ImageView>(Resource.Id.exo_fullscreen_icon);
                    MFullScreenButton = controlView.FindViewById<FrameLayout>(Resource.Id.exo_fullscreen_button);

                    VideoPlayButton = controlView.FindViewById<ImageButton>(Resource.Id.exo_play);
                    VideoResumeButton = controlView.FindViewById<ImageButton>(Resource.Id.exo_pause);
                    VolumeControl = controlView.FindViewById<ImageView>(Resource.Id.exo_volume_icon);

                    switch (AppSettings.ShowFullScreenVideoPost)
                    {
                        case false:
                            mFullScreenIcon.Visibility = ViewStates.Gone;
                            MFullScreenButton.Visibility = ViewStates.Gone;
                            break;
                    }

                    if (holder != null) LoadingProgressBar = holder.VideoProgressBar;

                    SetVolumeControl(XRecyclerView.VolumeStateProvider == VolumeState.On ? VolumeState.On : VolumeState.Off);

                    switch (VolumeControl.HasOnClickListeners)
                    {
                        case false:
                            VolumeControl.Click += (sender, args) =>
                            {
                                ToggleVolume();
                            };
                            break;
                    }

                    switch (MFullScreenButton.HasOnClickListeners)
                    {
                        case false:
                            MFullScreenButton.Click += (sender, args) =>
                            {
                                new PostClickListener((Activity)recyclerView.Context, recyclerView.NativeFeedAdapter.NativePostType).InitFullscreenDialog(Uri.Parse(holder?.VideoUrl), null);
                            };
                            break;
                    }

                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            private void ToggleVolume()
            {
                try
                {
                    switch (XRecyclerView.VideoPlayer)
                    {
                        case null:
                            return;
                        default:
                            switch (XRecyclerView.VolumeStateProvider)
                            {
                                case VolumeState.Off:
                                    SetVolumeControl(VolumeState.On);
                                    break;
                                case VolumeState.On:
                                    SetVolumeControl(VolumeState.Off);
                                    break;
                                default:
                                    SetVolumeControl(VolumeState.Off);
                                    break;
                            }

                            break;
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            private void SetVolumeControl(VolumeState state)
            {
                try
                {
                    XRecyclerView.VolumeStateProvider = state;
                    switch (state)
                    {
                        case VolumeState.Off:
                            XRecyclerView.VolumeStateProvider = VolumeState.Off;
                            XRecyclerView.VideoPlayer.Volume = 0f;
                            AnimateVolumeControl();
                            break;
                        case VolumeState.On:
                            XRecyclerView.VolumeStateProvider = VolumeState.On;
                            XRecyclerView.VideoPlayer.Volume = 1f;
                            AnimateVolumeControl();
                            break;
                        default:
                            XRecyclerView.VideoPlayer.Volume = 1f;
                            AnimateVolumeControl();
                            break;
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            private void AnimateVolumeControl()
            {
                try
                {
                    switch (VolumeControl)
                    {
                        case null:
                            return;
                        default:
                            VolumeControl.BringToFront();
                            switch (XRecyclerView.VolumeStateProvider)
                            {
                                case VolumeState.Off:
                                    VolumeControl.SetImageResource(Resource.Drawable.ic_volume_off_grey_24dp);

                                    break;
                                case VolumeState.On:
                                    VolumeControl.SetImageResource(Resource.Drawable.ic_volume_up_grey_24dp);
                                    break;
                                default:
                                    VolumeControl.SetImageResource(Resource.Drawable.ic_volume_off_grey_24dp);
                                    break;
                            }
                            //VolumeControl.Animate().Cancel();

                            //VolumeControl.Alpha = (1f);

                            //VolumeControl.Animate().Alpha(0f).SetDuration(600).SetStartDelay(1000);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            private void UpdateVideoPosition()
            {
                try
                {
                    var curPosition = Player.CurrentPosition;
                    var negativePosition = Player.Duration;
                    //var kkk = 0;
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public void OnLoadingChanged(bool p0)
            {
            }

            public void OnPlaybackParametersChanged(PlaybackParameters p0)
            {
            }

            public void OnPlayerError(ExoPlaybackException p0)
            {
            }

            public void OnPlayerStateChanged(bool playWhenReady, int playbackState)
            {
                try
                {
                    //if (VideoResumeButton == null || VideoPlayButton == null || LoadingProgressBar == null)
                    //    return;
                    UpdateVideoPosition();

                    switch (playbackState)
                    {
                        case IPlayer.StateEnded:
                        {
                            switch (playWhenReady)
                            {
                                case false:
                                    VideoResumeButton.Visibility = ViewStates.Visible;
                                    break;
                                default:
                                    VideoResumeButton.Visibility = ViewStates.Gone;
                                    VideoPlayButton.Visibility = ViewStates.Visible;
                                    break;
                            }

                            LoadingProgressBar.Visibility = ViewStates.Invisible;
                            XRecyclerView.VideoPlayer.SeekTo(0);

                            TabbedMainActivity.GetInstance()?.SetOffWakeLock();
                            break;
                        }
                        case IPlayer.StateReady:
                        {
                            switch (playWhenReady)
                            {
                                case false:
                                    VideoResumeButton.Visibility = ViewStates.Gone;
                                    VideoPlayButton.Visibility = ViewStates.Visible;
                                    break;
                                default:
                                    VideoResumeButton.Visibility = ViewStates.Visible;
                                    break;
                            }

                            LoadingProgressBar.Visibility = ViewStates.Invisible;

                            switch (XRecyclerView.IsVideoViewAdded)
                            {
                                case false:
                                    XRecyclerView.AddVideoView();
                                    break;
                            }

                            switch (XRecyclerView.IsVoicePlayed)
                            {
                                case true:
                                    XRecyclerView.ResetMediaPlayer();
                                    break;
                            }

                            TabbedMainActivity.GetInstance()?.SetOnWakeLock();
                            break;
                        }
                        case IPlayer.StateBuffering:
                            VideoPlayButton.Visibility = ViewStates.Invisible;
                            LoadingProgressBar.Visibility = ViewStates.Visible;
                            VideoResumeButton.Visibility = ViewStates.Invisible;
                            break;
                    }
                }
                catch (Exception exception)
                {
                    Methods.DisplayReportResultTrack(exception);
                }
            }

            public void OnIsPlayingChanged(bool p0)
            {

            }

            public void OnPositionDiscontinuity(int p0)
            {
            }

            public void OnRepeatModeChanged(int p0)
            {
            }

            public void OnSeekProcessed()
            {
            }

            public void OnShuffleModeEnabledChanged(bool p0)
            {
            }

            public void OnTimelineChanged(Timeline p0,  int p2)
            {
            }

            public void OnTracksChanged(TrackGroupArray p0, TrackSelectionArray p1)
            {
            }

            public void OnVisibilityChange(int p0)
            {
            }

            public void OnProgressUpdate(long position, long bufferedPosition)
            {
            }
        }

        #endregion

        #region VideoObject player

        private void SetPlayer()
        {
            try
            {
                BandwidthMeter = DefaultBandwidthMeter.GetSingletonInstance(MainContext);

                DefaultTrackSelector trackSelector = new DefaultTrackSelector(MainContext);
                trackSelector.SetParameters(new DefaultTrackSelector.ParametersBuilder(MainContext));

                VideoPlayer = new SimpleExoPlayer.Builder(MainContext).SetTrackSelector(trackSelector).Build();

                DefaultDataSourceFac = new DefaultDataSourceFactory(MainContext, Util.GetUserAgent(MainContext, AppSettings.ApplicationName), BandwidthMeter);
                VideoSurfaceView.UseController = true;
                VideoSurfaceView.Player = VideoPlayer; 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private IMediaSource GetMediaSourceFromUrl(Uri uri, string extension, string tag)
        {
            var BandwidthMeter = DefaultBandwidthMeter.GetSingletonInstance(MainContext);
            //DefaultDataSourceFactory dataSourceFactory = new DefaultDataSourceFactory(ActivityContext, Util.GetUserAgent(MainContext, AppSettings.ApplicationName), mBandwidthMeter);
            var buildHttpDataSourceFactory = new DefaultDataSourceFactory(MainContext, BandwidthMeter, new DefaultHttpDataSourceFactory(Util.GetUserAgent(MainContext, AppSettings.ApplicationName)));
            var buildHttpDataSourceFactoryNull = new DefaultDataSourceFactory(MainContext, BandwidthMeter, new DefaultHttpDataSourceFactory(Util.GetUserAgent(MainContext, AppSettings.ApplicationName)));
            int type = Util.InferContentType(uri, extension);
            try
            {

                IMediaSource src;
                switch (type)
                {
                    case C.TypeSs:
                        src = new SsMediaSource.Factory(new DefaultSsChunkSource.Factory(buildHttpDataSourceFactory), buildHttpDataSourceFactoryNull).SetTag(tag).SetDrmSessionManager(IDrmSessionManager.DummyDrmSessionManager).CreateMediaSource(uri);
                        break;
                    case C.TypeDash:
                        src = new DashMediaSource.Factory(new DefaultDashChunkSource.Factory(buildHttpDataSourceFactory), buildHttpDataSourceFactoryNull).SetTag(tag).SetDrmSessionManager(IDrmSessionManager.DummyDrmSessionManager).CreateMediaSource(uri);
                        break;
                    case C.TypeHls:
                        DefaultHlsExtractorFactory defaultHlsExtractorFactory = new DefaultHlsExtractorFactory(DefaultTsPayloadReaderFactory.FlagAllowNonIdrKeyframes, true);
                        src = new HlsMediaSource.Factory(buildHttpDataSourceFactory).SetTag(tag).SetExtractorFactory(defaultHlsExtractorFactory).CreateMediaSource(uri);
                        break;
                    case C.TypeOther:
                    default:
                        src = new ProgressiveMediaSource.Factory(buildHttpDataSourceFactory).SetTag(tag).CreateMediaSource(uri);
                        break;
                }

                return src;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                try
                {
                    return new ProgressiveMediaSource.Factory(buildHttpDataSourceFactory).SetTag(tag).CreateMediaSource(uri);
                }
                catch (Exception exception)
                {
                    Methods.DisplayReportResultTrack(exception);
                    return null!;
                }
            }
        }


        public void CacheVideosFiles(Uri videoUrl)
        {
            try
            {
                Cache ??= new SimpleCache(MainContext.CacheDir, new NoOpCacheEvictor(), new ExoDatabaseProvider(MainContext));

                CacheDataSourceFactory ??= new CacheDataSourceFactory(Cache, DefaultDataSourceFac);

                //var dataSpec = new DataSpec(videoUrl, 0, 3000 * 1024, null); //0, 1000 * 1024, null
                //CacheUtil.GetCached(dataSpec, Cache, new MyCacheKeyFactory());
                //string userAgent = Util.GetUserAgent(MainContext, AppSettings.ApplicationName);
                //var bandwidthMeter = new DefaultBandwidthMeter.Builder(MainContext).Build();
                //var defaultDataSourceFactory = new DefaultDataSourceFactory(MainContext, bandwidthMeter, new DefaultHttpDataSourceFactory(userAgent, bandwidthMeter));

                if (videoUrl.Path != null && videoUrl.Path.Contains(".mp4"))
                {
                    new Thread(() =>
                    {
                        try
                        {
                            //DownloaderConstructorHelper constructorHelper = new DownloaderConstructorHelper(Cache, defaultDataSourceFactory);
                            //IDownloaderFactory factory = new DefaultDownloaderFactory(constructorHelper);

                            int type = Util.InferContentType(videoUrl, videoUrl.Path.Split('.').Last());
                            string typeVideo = type switch
                            {
                                C.TypeSs => DownloadRequest.TypeSs,
                                C.TypeDash => DownloadRequest.TypeDash,
                                C.TypeHls => DownloadRequest.TypeHls,
                                C.TypeOther => DownloadRequest.TypeProgressive,
                                _ => DownloadRequest.TypeProgressive
                            };

                            DownloadManager downloadManager = new DownloadManager(MainContext, new DefaultDatabaseProvider(new ExoDatabaseProvider(MainContext)), Cache, CacheDataSourceFactory);
                            downloadManager.AddDownload(new DownloadRequest("id", typeVideo, videoUrl, /* streamKeys= */new List<StreamKey>(), null!, /* data= */ null!));
                        }
                        catch (Exception e)
                        {
                            Methods.DisplayReportResultTrack(e);
                        }
                    }).Start();
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }


        private class MyCacheKeyFactory : Object, ICacheKeyFactory
        {
            public string Key { set; get; }

            public string BuildCacheKey(DataSpec dataSpec)
            {
                Key = dataSpec.Key;
                return dataSpec.Key;
            }
        }

        private int GetVisibleVideoSurfaceHeight(int playPosition)
        {
            try
            {
                var at = playPosition - ((LinearLayoutManager)Objects.RequireNonNull(GetLayoutManager())).FindFirstVisibleItemPosition();

                var child = GetChildAt(at);
                switch (child)
                {
                    case null:
                        return 0;
                }
                int[] location = new int[2];
                child.GetLocationInWindow(location);
                return location[1] switch
                {
                    < 0 => location[1] + VideoSurfaceDefaultHeight,
                    _ => ScreenDefaultHeight - location[1]
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return 0;
            }
        }

        private void AddVideoView()
        {
            try
            {
                //var d = MediaContainerLayout.FindViewById<PlayerView>(VideoSurfaceView.Id);
                //if (d == null)
                //{
                MediaContainerLayout.AddView(VideoSurfaceView);
                IsVideoViewAdded = true;
                VideoSurfaceView.RequestFocus();
                VideoSurfaceView.Visibility = ViewStates.Visible;

                //}

                Thumbnail.Visibility = ViewStates.Gone;
                PlayControl.Visibility = ViewStates.Gone;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void RemoveVideoView(PlayerView videoView)
        {
            try
            {
                var parent = (ViewGroup)videoView.Parent;
                switch (parent)
                {
                    case null:
                        return;
                }

                var index = parent.IndexOfChild(videoView);
                switch (index)
                {
                    case < 0:
                        return;
                    default:
                        parent.RemoveViewAt(index);
                        IsVideoViewAdded = false;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void StopVideo()
        {
            try
            {
                ResetMediaPlayer();

                switch (VideoSurfaceView.Player)
                {
                    case null:
                        return;
                }
                switch (VideoSurfaceView.Player.PlaybackState)
                {
                    case IPlayer.StateReady:
                        VideoSurfaceView.Player.PlayWhenReady = false;

                        TabbedMainActivity.GetInstance()?.SetOffWakeLock();
                        break;
                }

                //GC Collect
                GC.Collect();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void ReleasePlayer()
        {
            try
            {
                StopVideo();
                VideoSurfaceView?.Player?.Stop();

                if (VideoPlayer != null)
                {
                    VideoPlayer.Release();
                    VideoPlayer = null!;
                }

                ViewHolderParent = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        private void ResetMediaPlayer()
        {
            try
            {
                var list = NativeFeedAdapter.ListDiffer.Where(a => a.TypeView == PostModelType.VoicePost && a.VoicePlayer != null).ToList();
                switch (list.Count)
                {
                    case > 0:
                    {
                        ViewHolderVoicePlayer = null;
                        IsVoicePlayed = false;

                        foreach (var item in list)
                        {
                            if (item.VoicePlayer != null)
                            {
                                item.VoicePlayer.Completion += null!;
                                item.VoicePlayer.Prepared += null!;

                                item.VoicePlayer.Stop();
                            }

                            item.VoicePlayer?.Release();
                            item.VoicePlayer = null;

                            if (item.Timer != null)
                            {
                                item.Timer.Enabled = false;
                                item.Timer.Stop();
                            }
                            item.Timer = null;

                            try
                            {
                                NativeFeedAdapter.NotifyItemChanged(NativeFeedAdapter.ListDiffer.IndexOf(item), "WithoutBlobAudio");
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);
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

        //public void OnLoadMore(int currentPage)
        //{
        //    try
        //    {
        //        if (NativeFeedAdapter.NativePostType == NativeFeedType.Memories || NativeFeedAdapter.NativePostType == NativeFeedType.Share)
        //            return;

        //        MainScrollEvent.IsLoading = true;
        //        var diff = NativeFeedAdapter.ListDiffer;
        //        var list = new List<AdapterModelsClass>(diff);
        //        if (list.Count <= 20)
        //            return;

        //        var item = list.LastOrDefault();

        //        NativeFeedAdapter.SetLoading();

        //        var lastItem = list.IndexOf(item);

        //        item = list[lastItem];

        //        string offset;

        //        if (item.TypeView == PostModelType.Divider || item.TypeView == PostModelType.ViewProgress || item.TypeView == PostModelType.AdMob1 || item.TypeView == PostModelType.AdMob2|| item.TypeView == PostModelType.AdMob3 || item.TypeView == PostModelType.FbAdNative || item.TypeView == PostModelType.AdsPost || item.TypeView == PostModelType.SuggestedGroupsBox || item.TypeView == PostModelType.SuggestedUsersBox || item.TypeView == PostModelType.CommentSection || item.TypeView == PostModelType.AddCommentSection)
        //        {
        //            item = list.LastOrDefault(a => a.TypeView != PostModelType.Divider && a.TypeView != PostModelType.ViewProgress && a.TypeView != PostModelType.AdMob1 && a.TypeView != PostModelType.AdMob2 && a.TypeView != PostModelType.AdMob3 && a.TypeView != PostModelType.FbAdNative && a.TypeView != PostModelType.AdsPost && a.TypeView != PostModelType.SuggestedGroupsBox && a.TypeView != PostModelType.SuggestedUsersBox && a.TypeView != PostModelType.CommentSection && a.TypeView != PostModelType.AddCommentSection);
        //            offset = item?.PostData?.PostId ?? "0";
        //            Console.WriteLine(offset);
        //        }
        //        else
        //        {
        //            offset = item.PostData?.PostId ?? "0";
        //        }

        //        Console.WriteLine(offset);

        //        if (!Methods.CheckConnectivity())
        //            Toast.MakeText(MainContext, MainContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
        //        else
        //        {
        //            PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => NativeFeedAdapter.NativePostType != NativeFeedType.HashTag ? FetchNewsFeedApiPosts(offset) : FetchNewsFeedApiPosts(offset, "Add", Hash) });
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        Methods.DisplayReportResultTrack(e);
        //    }
        //}


        //wael function fix emojis 
        public static Dictionary<string, string> GetAddDiscountList()
        {
            try
            {
                var arrayAdapter = new Dictionary<string, string>
                {
                    {":)", "smile"},
                    {"(&lt;", "joy"},
                    {"**)", "relaxed"},
                    {":p", "stuck-out-tongue-winking-eye"},
                    {":_p", "stuck-out-tongue"},
                    {"B)", "sunglasses"},
                    {";)", "wink"},
                    {":D", "grin"},
                    {"/_)", "smirk"},
                    {"0)", "innocent"},
                    {":_(", "cry"},
                    {":__(", "sob"},
                    {":(", "disappointed"},
                    {":*", "kissing-heart"},
                    {"&lt;3", "heart"},
                    {"&lt;/3", "broken-heart"},
                    {"*_*", "heart-eyes"},
                    {"&lt;5", "star"},
                    {":o", "open-mouth"},
                    {":0", "scream"},
                    {"o(", "anguished"},
                    {"-_(", "unamused"},
                    {"x(", "angry"},
                    {"X(", "rage"},
                    {"-_-", "expressionless"},
                    {":-/", "confused"},
                    {":|", "neutral-face"},
                    {"!_", "exclamation"},
                    {":|", "neutral-face"},
                    {":|", "neutral-face"},
                    {":yum:", "yum"},
                    {":triumph:", "triumph"},
                    {":imp:", "imp"},
                    {":hear_no_evil:", "hear-no-evil"},
                    {":alien:", "alien"},
                    {":yellow_heart:", "yellow-heart"},
                    {":sleeping:", "sleeping"},
                    {":mask:", "mask"},
                    {":no_mouth:", "no-mouth"},
                    {":weary:", "weary"},
                    {":dizzy_face:", "dizzy-face"},
                    {":man:", "man"},
                    {":woman:", "woman"},
                    {":boy:", "boy"},
                    {":girl:", "girl"},
                    {":?lder_man:", "older-man"},
                    {":?lder_woman:", "older-woman"},
                    {":cop:", "cop"},
                    {":dancers:", "dancers"},
                    {":speak_no_evil:", "speak-no-evil"},
                    {":lips:", "lips"},
                    {":see_no_evil:", "see-no-evil"},
                    {":dog:", "dog"},
                    {":bear:", "bear"},
                    {":rose:", "rose"},
                    {":gift_heart:", "gift-heart"},
                    {":ghost:", "ghost"},
                    {":bell:", "bell"},
                    {":video_game:", "video-game"},
                    {":soccer:", "soccer"},
                    {":books:", "books"},
                    {":moneybag:", "moneybag"},
                    {":mortar_board:", "mortar-board"},
                    {":hand:", "hand"},
                    {":tiger:", "tiger"},
                    {":elephant:", "elephant"},
                    {":scream_cat:", "scream-cat"},
                    {":monkey:", "monkey"},
                    {":bird:", "bird"},
                    {":snowflake:", "snowflake"},
                    {":sunny:", "sunny"},
                    {":?cean:", "ocean"},
                    {":umbrella:", "umbrella"},
                    {":hibiscus:", "hibiscus"},
                    {":tulip:", "tulip"},
                    {":computer:", "computer"},
                    {":bomb:", "bomb"},
                    {":gem:", "gem"},
                    {":ring:", "ring"},
                    {":)", "??"},
                    {"(&lt;", "??"},
                    {"**)", "??"},
                    {":p", "??"},
                    {":_p", "??"},
                    {"B)", "??"},
                    {";)", "??"},
                    {":D", "??"},
                    {"/_)", "smirk"},
                    {"0)", "innocent"},
                    {":_(", "cry"},
                    {":__(", "sob"},
                    {":(", "disappointed"},
                    {":*", "kissing-heart"},
                    {"&lt;3", "heart"},
                    {"&lt;/3", "broken-heart"},
                    {"*_*", "heart-eyes"},
                    {"&lt;5", "star"},
                    {":o", "open-mouth"},
                    {":0", "scream"},
                    {"o(", "anguished"},
                    {"-_(", "unamused"},
                    {"x(", "angry"},
                    {"X(", "rage"},
                    {"-_-", "expressionless"},
                    {":-/", "confused"},
                    {":|", "neutral-face"},
                    {"!_", "exclamation"},
                    {":|", "neutral-face"},
                };

                return arrayAdapter;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return new Dictionary<string, string>();
            }
        }
          
    }
}
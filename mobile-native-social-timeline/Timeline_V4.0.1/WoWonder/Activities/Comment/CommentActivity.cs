using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS; 
using AndroidX.Interpolator.View.Animation; 
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using AT.Markushi.UI;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using TheArtOfDev.Edmodo.Cropper;
using Java.IO;
using Newtonsoft.Json;
using WoWonder.Activities.Comment.Adapters;
using WoWonder.Activities.Comment.Fragment;
using WoWonder.Activities.NativePost.Extra;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Comments;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Requests;
using Uri = Android.Net.Uri;
using SupportFragment = AndroidX.Fragment.App.Fragment;
using WoWonder.Activities.Tabbes;
using Bumptech.Glide.Util;
using WoWonder.Library.Anjo.IntegrationRecyclerView;
using Developer.SEmojis.Actions;
using Developer.SEmojis.Helper;
using WoWonder.Activities.Base;
using WoWonder.Activities.PostData; 

namespace WoWonder.Activities.Comment
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class CommentActivity : BaseActivity
    {
        #region Variables Basic

        private static CommentActivity Instance;
        public CommentAdapter MAdapter;
        private SwipeRefreshLayout SwipeRefreshLayout;
        private RecyclerView MRecycler;
        private LinearLayoutManager LayoutManager;
        private RecyclerViewOnScrollListener MainScrollEvent;
        private EmojiconEditText TxtComment;
        private TextView LikeCountBox;
        private ImageView ImgSent, ImgGallery, ImgBack;
        public CircleButton BtnVoice;
        private PostDataObject PostObject;
        public string PostId;
        private string Type, PathImage, PathVoice, TextRecorder = "";
        private FrameLayout TopFragment;
        private RecordSoundFragment RecordSoundFragment;
        private bool IsRecording;
        private Methods.AudioRecorderAndPlayer RecorderService;
        private LinearLayout CommentLayout;
        private ImageView EmojisView;
        private LinearLayout RootView;

        #endregion

        #region General

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                Window?.SetSoftInputMode(SoftInput.AdjustResize);

                SetTheme(AppSettings.SetTabDarkTheme ? Resource.Style.MyTheme_Dark_Base : Resource.Style.MyTheme_Base);

                Methods.App.FullScreenApp(this);

                // Create your application here
                SetContentView(Resource.Layout.Native_Comment_Layout);

                Instance = this;

                Type = Intent?.GetStringExtra("Type") ?? string.Empty;
                PostId = Intent?.GetStringExtra("PostId") ?? string.Empty;
                PostObject = JsonConvert.DeserializeObject<PostDataObject>(Intent?.GetStringExtra("PostObject") ?? string.Empty);
                  
                //Get Value And Set Toolbar
                InitComponent();
                SetRecyclerViewAdapters();

                LoadDataPost(); 

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
                ResetMediaPlayer();
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
                RootView = FindViewById<LinearLayout>(Resource.Id.main_content);

                MRecycler = (RecyclerView)FindViewById(Resource.Id.recycler_view);

                SwipeRefreshLayout = (SwipeRefreshLayout)FindViewById(Resource.Id.swipeRefreshLayout);
                SwipeRefreshLayout.SetColorSchemeResources(Android.Resource.Color.HoloBlueLight, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
                SwipeRefreshLayout.Refreshing = true;
                SwipeRefreshLayout.Enabled = true;
                SwipeRefreshLayout.SetProgressBackgroundColorSchemeColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#424242") : Color.ParseColor("#f7f7f7"));

                LikeCountBox = FindViewById<TextView>(Resource.Id.like_box);
                EmojisView = FindViewById<ImageView>(Resource.Id.emojiicon);
                TxtComment = FindViewById<EmojiconEditText>(Resource.Id.commenttext);
                ImgSent = FindViewById<ImageView>(Resource.Id.send);
                ImgGallery = FindViewById<ImageView>(Resource.Id.image);
                ImgBack = FindViewById<ImageView>(Resource.Id.back);
                CommentLayout = FindViewById<LinearLayout>(Resource.Id.commentLayout);

                BtnVoice = FindViewById<CircleButton>(Resource.Id.voiceButton);
                BtnVoice.LongClickable = true;
                BtnVoice.Tag = "Free";
                BtnVoice.SetImageResource(Resource.Drawable.microphone);

                TopFragment = FindViewById<FrameLayout>(Resource.Id.TopFragmentHolder);

                TxtComment.Text = "";
                PathImage = "";
                TextRecorder = "";

                Methods.SetColorEditText(TxtComment, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                RecordSoundFragment = new RecordSoundFragment();
                SupportFragmentManager.BeginTransaction().Add(TopFragment.Id, RecordSoundFragment, RecordSoundFragment.Tag);
                 
                switch (AppSettings.FlowDirectionRightToLeft)
                {
                    case true:
                        ImgBack.SetImageResource(Resource.Drawable.ic_action_ic_back_rtl);
                        break;
                }

                ImgGallery.SetImageDrawable(AppSettings.SetTabDarkTheme ? GetDrawable(Resource.Drawable.ic_action_addpost_Ligth) : GetDrawable(Resource.Drawable.ic_action_AddPost));

                var emojisIcon = new EmojIconActions(this, RootView, TxtComment, EmojisView);
                emojisIcon.ShowEmojIcon();
                emojisIcon.SetIconsIds(Resource.Drawable.ic_action_keyboard, Resource.Drawable.ic_action_sentiment_satisfied_alt); 
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
                MAdapter = new CommentAdapter(this)
                {
                    CommentList = new ObservableCollection<CommentObjectExtra>()
                };
                LayoutManager = new LinearLayoutManager(this);
                MRecycler.SetLayoutManager(LayoutManager);
                MRecycler.HasFixedSize = true;
                MRecycler.SetItemViewCacheSize(10);
                MRecycler.GetLayoutManager().ItemPrefetchEnabled = true;
                var sizeProvider = new FixedPreloadSizeProvider(10, 10);
                var preLoader = new RecyclerViewPreloader<CommentObjectExtra>(this, MAdapter, sizeProvider, 10);
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
                        ImgSent.Click += ImgSentOnClick;
                        ImgGallery.Click += ImgGalleryOnClick;
                        ImgBack.Click += ImgBackOnClick;
                        BtnVoice.LongClick += BtnVoiceOnLongClick;
                        BtnVoice.Touch += BtnVoiceOnTouch; 
                        SwipeRefreshLayout.Refresh += SwipeRefreshLayoutOnRefresh;
                        LikeCountBox.Click += LikeCountBoxOnClick;
                        break;
                    default:
                        ImgSent.Click -= ImgSentOnClick;
                        ImgGallery.Click -= ImgGalleryOnClick;
                        ImgBack.Click -= ImgBackOnClick;
                        BtnVoice.LongClick -= BtnVoiceOnLongClick;
                        BtnVoice.Touch -= BtnVoiceOnTouch;
                        SwipeRefreshLayout.Refresh -= SwipeRefreshLayoutOnRefresh;
                        LikeCountBox.Click -= LikeCountBoxOnClick;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static CommentActivity GetInstance()
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
                Instance = null!;
                MAdapter = null!;
                SwipeRefreshLayout = null!;
                MRecycler = null!;
                TxtComment = null!;
                LikeCountBox = null!;
                ImgSent = null!; ImgGallery = null!; ImgBack = null!;
                BtnVoice = null!;
                PostObject = null!;
                PostId = null!;
                PathImage = null!; PathVoice = null!; TextRecorder = null!;
                TopFragment = null!;
                RecordSoundFragment = null!;
                RecorderService = null!;
                CommentLayout = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events

        private void LikeCountBoxOnClick(object sender, EventArgs e)
        {
            try
            {
                switch (AppSettings.PostButton)
                {
                    case PostButtonSystem.ReactionDefault:
                    case PostButtonSystem.ReactionSubShine:
                    {
                        switch (PostObject.Reaction.Count)
                        {
                            case > 0:
                            {
                                var intent = new Intent(this, typeof(ReactionPostTabbedActivity));
                                intent.PutExtra("PostObject", JsonConvert.SerializeObject(PostObject));
                                StartActivity(intent);
                                break;
                            }
                        }

                        break;
                    }
                    default:
                    {
                        var intent = new Intent(this, typeof(PostDataActivity));
                        intent.PutExtra("PostId", PostObject.PostId);
                        intent.PutExtra("PostType", "post_likes");
                        StartActivity(intent);
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void BtnVoiceOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                var handled = false;

                switch (e.Event?.Action)
                {
                    case MotionEventActions.Up:
                        try
                        {
                            switch (IsRecording)
                            {
                                case true:
                                {
                                    RecorderService.StopRecording();
                                    PathVoice = RecorderService.GetRecorded_Sound_Path();

                                    BtnVoice.SetColorFilter(Color.ParseColor(AppSettings.MainColor));
                                    BtnVoice.SetImageResource(Resource.Drawable.microphone);

                                    switch (TextRecorder)
                                    {
                                        case "Recording":
                                        {
                                            switch (string.IsNullOrEmpty(PathVoice))
                                            {
                                                case false:
                                                {
                                                    Bundle bundle = new Bundle();
                                                    bundle.PutString("FilePath", PathVoice);
                                                    RecordSoundFragment.Arguments = bundle;
                                                    ReplaceTopFragment(RecordSoundFragment);
                                                    break;
                                                }
                                            }

                                            TextRecorder = "";
                                            break;
                                        }
                                    }

                                    IsRecording = false;
                                    break;
                                }
                                default:
                                {
                                    switch (UserDetails.SoundControl)
                                    {
                                        case true:
                                            Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("Error.mp3");
                                            break;
                                    }

                                    Toast.MakeText(this, GetText(Resource.String.Lbl_HoldToRecord), ToastLength.Short)?.Show();
                                    break;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            Methods.DisplayReportResultTrack(exception);
                        }

                        BtnVoice.Pressed = false;
                        handled = true;
                        break;
                }

                e.Handled = handled;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //record voices ( Permissions is 102 )
        private void BtnVoiceOnLongClick(object sender, View.LongClickEventArgs e)
        {
            try
            {
                switch ((int)Build.VERSION.SdkInt)
                {
                    case < 23:
                        StartRecording();
                        break;
                    default:
                    {
                        //Check to see if any permission in our group is available, if one, then all are
                        if (CheckSelfPermission(Manifest.Permission.RecordAudio) == Permission.Granted)
                        {
                            StartRecording();
                        }
                        else
                        {
                            new PermissionsController(this).RequestPermission(102);
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

        private async void StartRecording()
        {
            try
            {
                switch (BtnVoice.Tag?.ToString())
                {
                    case "Free":
                    {
                        //Set Record Style
                        IsRecording = true;

                        switch (UserDetails.SoundControl)
                        {
                            case true:
                                Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("RecourdVoiceButton.mp3");
                                break;
                        }

                        if (TextRecorder != null && TextRecorder != "Recording")
                            TextRecorder = "Recording";

                        BtnVoice.SetColorFilter(Color.ParseColor("#FA3C4C"));
                        BtnVoice.SetImageResource(Resource.Drawable.ic_stop_white_24dp);

                        RecorderService = new Methods.AudioRecorderAndPlayer(PostId);
                        //Start Audio record
                        await Task.Delay(600);
                        RecorderService.StartRecording();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Back
        private void ImgBackOnClick(object sender, EventArgs e)
        {
            try
            {
                Finish();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Open Gallery
        private void ImgGalleryOnClick(object sender, EventArgs e)
        {
            try
            {
                OpenDialogGallery(); //requestCode >> 500 => Image Gallery
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Api sent Comment
        private async void ImgSentOnClick(object sender, EventArgs e)
        {
            try
            {
                IsRecording = false;

                switch (BtnVoice.Tag?.ToString())
                {
                    case "Audio":
                    {
                        var interTortola = new FastOutSlowInInterpolator();
                        TopFragment.Animate()?.SetInterpolator(interTortola)?.TranslationY(1200)?.SetDuration(300);
                        SupportFragmentManager.BeginTransaction().Remove(RecordSoundFragment)?.Commit();

                        PathVoice = RecorderService.GetRecorded_Sound_Path();
                        break;
                    }
                }

                if (string.IsNullOrEmpty(TxtComment.Text) && string.IsNullOrWhiteSpace(TxtComment.Text) && string.IsNullOrEmpty(PathImage) && string.IsNullOrEmpty(PathVoice))
                    return;

                if (Methods.CheckConnectivity())
                {
                    var dataUser = ListUtils.MyProfileList?.FirstOrDefault();
                    //Comment Code 

                    var unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    string time2 = unixTimestamp.ToString(CultureInfo.InvariantCulture);

                    CommentObjectExtra comment = new CommentObjectExtra
                    {
                        Id = unixTimestamp.ToString(),
                        PostId = PostObject.Id,
                        UserId = UserDetails.UserId,
                        Text = TxtComment.Text,
                        Time = time2,
                        CFile = PathImage,
                        Record = PathVoice,
                        Publisher = dataUser,
                        Url = dataUser?.Url,
                        Fullurl = PostObject?.PostUrl,
                        Orginaltext = TxtComment.Text,
                        Owner = true,
                        CommentLikes = "0",
                        CommentWonders = "0",
                        IsCommentLiked = false,
                        Replies = "0",
                        RepliesCount = "0"
                    };

                    MAdapter.CommentList.Add(comment);

                    var index = MAdapter.CommentList.IndexOf(comment);
                    switch (index)
                    {
                        case > -1:
                            MAdapter.NotifyItemInserted(index);
                            break;
                    }

                    MRecycler.Visibility = ViewStates.Visible;

                    var dd = MAdapter.CommentList.FirstOrDefault();
                    if (dd?.Text == MAdapter.EmptyState)
                    {
                        MAdapter.CommentList.Remove(dd);
                        MAdapter.NotifyItemRemoved(MAdapter.CommentList.IndexOf(dd));
                    }

                    ImgGallery.SetImageDrawable(AppSettings.SetTabDarkTheme ? GetDrawable(Resource.Drawable.ic_action_addpost_Ligth) : GetDrawable(Resource.Drawable.ic_action_AddPost));
                    var text = TxtComment.Text;

                    //Hide keyboard
                    TxtComment.Text = "";

                    var (apiStatus, respond) = await RequestsAsync.Comment.CreatePostCommentsAsync(PostObject.PostId, text, PathImage, PathVoice);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case CreateComments result:
                                {
                                    var date = MAdapter.CommentList.FirstOrDefault(a => a.Id == comment.Id) ?? MAdapter.CommentList.FirstOrDefault(x => x.Id == result.Data.Id);
                                    if (date != null)
                                    {
                                        var db = ClassMapper.Mapper?.Map<CommentObjectExtra>(result.Data);

                                        date = db;
                                        date.Id = result.Data.Id;

                                        index = MAdapter.CommentList.IndexOf(MAdapter.CommentList.FirstOrDefault(a => a.Id == unixTimestamp.ToString()));
                                        MAdapter.CommentList[index] = index switch
                                        {
                                            > -1 => db,
                                            _ => MAdapter.CommentList[index]
                                        };

                                        var postFeedAdapter = TabbedMainActivity.GetInstance()?.NewsFeedTab?.PostFeedAdapter;
                                        var dataGlobal = postFeedAdapter?.ListDiffer?.Where(a => a.PostData?.Id == PostObject?.PostId).ToList();
                                        switch (dataGlobal?.Count)
                                        {
                                            case > 0:
                                            {
                                                foreach (var dataClass in from dataClass in dataGlobal let indexCom = postFeedAdapter.ListDiffer.IndexOf(dataClass) where indexCom > -1 select dataClass)
                                                {
                                                    dataClass.PostData.PostComments = MAdapter.CommentList.Count.ToString();

                                                    switch (dataClass.PostData.GetPostComments?.Count)
                                                    {
                                                        case > 0:
                                                        {
                                                            var dataComment = dataClass.PostData.GetPostComments.FirstOrDefault(a => a.Id == date.Id);
                                                            switch (dataComment)
                                                            {
                                                                case null:
                                                                    dataClass.PostData.GetPostComments.Add(date);
                                                                    break;
                                                            }

                                                            break;
                                                        }
                                                        default:
                                                            dataClass.PostData.GetPostComments = new List<GetCommentObject> { date };
                                                            break;
                                                    }

                                                    postFeedAdapter.NotifyItemChanged(postFeedAdapter.ListDiffer.IndexOf(dataClass), "commentReplies");
                                                }

                                                break;
                                            }
                                        }

                                        var postFeedAdapter2 = WRecyclerView.GetInstance()?.NativeFeedAdapter;
                                        var dataGlobal2 = postFeedAdapter2?.ListDiffer?.Where(a => a.PostData?.Id == PostObject?.PostId).ToList();
                                        switch (dataGlobal2?.Count)
                                        {
                                            case > 0:
                                            {
                                                foreach (var dataClass in from dataClass in dataGlobal2 let indexCom = postFeedAdapter2.ListDiffer.IndexOf(dataClass) where indexCom > -1 select dataClass)
                                                {
                                                    dataClass.PostData.PostComments = MAdapter.CommentList.Count.ToString();

                                                    switch (dataClass.PostData.GetPostComments?.Count)
                                                    {
                                                        case > 0:
                                                        {
                                                            var dataComment = dataClass.PostData.GetPostComments.FirstOrDefault(a => a.Id == date.Id);
                                                            switch (dataComment)
                                                            {
                                                                case null:
                                                                    dataClass.PostData.GetPostComments.Add(date);
                                                                    break;
                                                            }

                                                            break;
                                                        }
                                                        default:
                                                            dataClass.PostData.GetPostComments = new List<GetCommentObject> { date };
                                                            break;
                                                    }

                                                    postFeedAdapter2.NotifyItemChanged(postFeedAdapter2.ListDiffer.IndexOf(dataClass), "commentReplies");
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
                    }
                    //else Methods.DisplayReportResult(this, respond);

                    //Hide keyboard
                    TxtComment.Text = "";
                    PathImage = "";
                    PathVoice = "";

                    BtnVoice.Tag = "Free";
                    BtnVoice.SetImageResource(Resource.Drawable.microphone);
                    BtnVoice.ClearColorFilter();
                }
                else
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
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
                MAdapter.CommentList.Clear();
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
                var item = MAdapter.CommentList.LastOrDefault();
                if (item != null && !string.IsNullOrEmpty(item.Id) && !MainScrollEvent.IsLoading)
                    StartApiService(item.Id);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
          
        #endregion

        #region Load Comment 

        private void LoadDataPost()
        {
            try
            {
                if (PostObject != null)
                {
                    switch (AppSettings.PostButton)
                    {
                        case PostButtonSystem.ReactionDefault:
                        case PostButtonSystem.ReactionSubShine:
                        {
                            PostObject.Reaction ??= new Reaction();

                            if (PostObject.Reaction != null)
                                LikeCountBox.Text = PostObject.Reaction.Count + " " + GetString(Resource.String.Lbl_Reactions);
                            else
                                LikeCountBox.Text = "0 " + GetString(Resource.String.Lbl_Reactions);
                            break;
                        }
                        default:
                        {
                            if (PostObject.PostLikes != null)
                                LikeCountBox.Text = PostObject.PostLikes + " " + GetString(Resource.String.Btn_Likes);
                            else
                                LikeCountBox.Text = "0 " + GetString(Resource.String.Btn_Likes);
                            break;
                        }
                    }

                    switch (PostObject.CommentsStatus)
                    {
                        case "0":
                            MAdapter.CommentList.Clear();

                            MAdapter.CommentList.Add(new CommentObjectExtra
                            {
                                Id = MAdapter.EmptyState,
                                Text = MAdapter.EmptyState,
                                Orginaltext = GetText(Resource.String.Lbl_CommentsAreDisabledBy) + " " + WoWonderTools.GetNameFinal(PostObject.Publisher),
                            });

                            MAdapter.NotifyDataSetChanged();

                            CommentLayout.Visibility = ViewStates.Gone;
                            break;
                        default:
                            //if (PostObject?.GetPostComments?.Count > 0)
                            //{
                            //    var db = ClassMapper.Mapper?.Map<List<CommentObjectExtra>>(PostObject.GetPostComments);
                            //    MAdapter.CommentList = new ObservableCollection<CommentObjectExtra>(db);
                            //}
                            //else
                            //{
                            //    MAdapter.CommentList = new ObservableCollection<CommentObjectExtra>();
                            //}
                         
                            //if (MAdapter.CommentList.Count > 0)
                            //    MAdapter?.NotifyDataSetChanged();

                            StartApiService();
                            break;
                    }
                }

                switch (Type)
                {
                    case "Normal_Gallery":
                        OpenDialogGallery(); //requestCode >> 500 => Image Gallery
                        break;
                    case "Normal_EmojiIcon":
                        EmojisView.PerformClick();
                        break;
                }
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
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => LoadDataComment(offset) });
        }

        private async Task LoadDataComment(string offset)
        {
            switch (MainScrollEvent.IsLoading)
            {
                case true:
                    return;
            }

            if (Methods.CheckConnectivity())
            {
                MainScrollEvent.IsLoading = true;
                var countList = MAdapter.CommentList.Count; 
                var (apiStatus, respond) = await RequestsAsync.Comment.GetPostCommentsAsync(PostId, "10", offset);
                if (apiStatus != 200 || respond is not CommentObject result || result.CommentList == null)
                {
                    MainScrollEvent.IsLoading = false;
                    Methods.DisplayReportResult(this, respond);
                }
                else
                {
                    var respondList = result.CommentList?.Count;
                    switch (respondList)
                    {
                        case > 0:
                        {
                            foreach (var item in result.CommentList)
                            {
                                CommentObjectExtra check = MAdapter.CommentList.FirstOrDefault(a => a.Id == item.Id);
                                switch (check)
                                {
                                    case null:
                                    {
                                        var db = ClassMapper.Mapper?.Map<CommentObjectExtra>(item);
                                        if (db != null) MAdapter.CommentList.Add(db);
                                        break;
                                    }
                                    default:
                                        check = ClassMapper.Mapper?.Map<CommentObjectExtra>(item);
                                        check.Replies = item.Replies;
                                        check.RepliesCount = item.RepliesCount;
                                        break;
                                }
                            }

                            RunOnUiThread(() => { MAdapter.NotifyDataSetChanged(); });
                            break;
                        }
                    }
                }

                RunOnUiThread(ShowEmptyPage);
            }
        }

        private void ShowEmptyPage()
        {
            try
            {
                MainScrollEvent.IsLoading = false;
                SwipeRefreshLayout.Refreshing = false;
                 
                switch (MAdapter.CommentList.Count)
                {
                    case > 0:
                    {
                        var emptyStateChecker = MAdapter.CommentList.FirstOrDefault(a => a.Text == MAdapter.EmptyState);
                        if (emptyStateChecker != null && MAdapter.CommentList.Count > 1)
                        {
                            MAdapter.CommentList.Remove(emptyStateChecker);
                            MAdapter.NotifyDataSetChanged();
                        }

                        break;
                    }
                    default:
                    {
                        MAdapter.CommentList.Clear();
                        var d = new CommentObjectExtra { Text = MAdapter.EmptyState };
                        MAdapter.CommentList.Add(d);
                        MAdapter.NotifyDataSetChanged();
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
                    //If its from Camera or Gallery  
                    case 500:
                    {
                        Uri uri = data.Data;
                        var filepath = Methods.AttachmentFiles.GetActualPathFromFile(this, uri);
                        PickiTonCompleteListener(filepath);
                        break;
                    }
                    case CropImage.CropImageActivityRequestCode:
                    {
                        var result = CropImage.GetActivityResult(data);

                        switch (resultCode)
                        {
                            case Result.Ok when result.IsSuccessful:
                            {
                                var resultUri = result.Uri;

                                switch (string.IsNullOrEmpty(resultUri.Path))
                                {
                                    case false:
                                    {
                                        PathImage = resultUri.Path;

                                        File file2 = new File(resultUri.Path);
                                        var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);
                                        Glide.With(this).Load(photoUri).Apply(new RequestOptions()).Into(ImgGallery);

                                        //GlideImageLoader.LoadImage(this, PathImage, ImgGallery, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);
                                        break;
                                    }
                                    default:
                                        Toast.MakeText(this, GetText(Resource.String.Lbl_something_went_wrong), ToastLength.Long)?.Show();
                                        break;
                                }

                                break;
                            }
                            case Result.Ok:
                                Toast.MakeText(this, GetText(Resource.String.Lbl_something_went_wrong), ToastLength.Long)
                                    .Show();
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

        //Permissions
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            try
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                switch (requestCode)
                {
                    case 108 when grantResults.Length > 0 && grantResults[0] == Permission.Granted:
                        OpenDialogGallery(); //requestCode >> 500 => Image Gallery
                        break;
                    case 108:
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Permission_is_denied), ToastLength.Long)?.Show();
                        break;
                    case 102 when grantResults.Length > 0 && grantResults[0] == Permission.Granted:
                        StartRecording();
                        break;
                    case 102:
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

        #region PickiT >> Gert path file

        private void PickiTonCompleteListener(string path)
        {
            //Dismiss dialog and return the path
            try
            {
                //  Check if it was a Drive/local/unknown provider file and display a Toast
                //if (wasDriveFile) => "Drive file was selected" 
                //else if (wasUnknownProvider)  => "File was selected from unknown provider" 
                //else => "Local file was selected"

                //  Chick if it was successful
                var check = WoWonderTools.CheckMimeTypesWithServer(path);
                switch (check)
                {
                    case false:
                        //this file not supported on the server , please select another file 
                        Toast.MakeText(this, GetString(Resource.String.Lbl_ErrorFileNotSupported), ToastLength.Short)?.Show();
                        return;
                }

                var type = Methods.AttachmentFiles.Check_FileExtension(path);
                switch (type)
                {
                    case "Image":
                    {
                        File file2 = new File(PathImage);
                        var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);
                        Glide.With(this).Load(photoUri).Apply(new RequestOptions()).Into(ImgGallery);

                        //GlideImageLoader.LoadImage(this, PathImage, ImgGallery, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);
                        break;
                    }
                    default:
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Failed_to_load), ToastLength.Short)?.Show();
                        break;
                }
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
                var list = MAdapter.CommentList.Where(a => !string.IsNullOrEmpty(a.Record) && a.MediaPlayer != null).ToList();
                switch (list.Count)
                {
                    case > 0:
                    {
                        foreach (var item in list)
                        {
                            if (item.MediaPlayer != null)
                            {
                                item.MediaPlayer.Stop();
                                item.MediaPlayer.Reset();
                            }
                            item.MediaPlayer = null!;
                            item.MediaTimer = null!;

                            item.MediaPlayer?.Release();
                            item.MediaPlayer = null!;
                        }
                        MAdapter.NotifyDataSetChanged();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        private void OpenDialogGallery()
        {
            try
            {
                switch ((int)Build.VERSION.SdkInt)
                {
                    // Check if we're running on Android 5.0 or higher
                    case < 23:
                    {
                        Methods.Path.Chack_MyFolder();

                        //Open Image 
                        var myUri = Uri.FromFile(new File(Methods.Path.FolderDiskImage, Methods.GetTimestamp(DateTime.Now) + ".jpeg"));
                        CropImage.Activity()
                            .SetInitialCropWindowPaddingRatio(0)
                            .SetAutoZoomEnabled(true)
                            .SetMaxZoom(4)
                            .SetGuidelines(CropImageView.Guidelines.On)
                            .SetCropMenuCropButtonTitle(GetText(Resource.String.Lbl_Crop))
                            .SetOutputUri(myUri).Start(this);
                        break;
                    }
                    default:
                    {
                        if (!CropImage.IsExplicitCameraPermissionRequired(this) && CheckSelfPermission(Manifest.Permission.ReadExternalStorage) == Permission.Granted &&
                            CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == Permission.Granted && CheckSelfPermission(Manifest.Permission.Camera) == Permission.Granted)
                        {
                            Methods.Path.Chack_MyFolder();

                            //Open Image 
                            var myUri = Uri.FromFile(new File(Methods.Path.FolderDiskImage, Methods.GetTimestamp(DateTime.Now) + ".jpeg"));
                            CropImage.Activity()
                                .SetInitialCropWindowPaddingRatio(0)
                                .SetAutoZoomEnabled(true)
                                .SetMaxZoom(4)
                                .SetGuidelines(CropImageView.Guidelines.On)
                                .SetCropMenuCropButtonTitle(GetText(Resource.String.Lbl_Crop))
                                .SetOutputUri(myUri).Start(this);
                        }
                        else
                        {
                            new PermissionsController(this).RequestPermission(108);
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

        private void ReplaceTopFragment(SupportFragment fragmentView)
        {
            try
            {
                switch (fragmentView.IsVisible)
                {
                    case true:
                        return;
                }

                var trans = SupportFragmentManager.BeginTransaction();
                trans.Replace(TopFragment.Id, fragmentView);

                switch (SupportFragmentManager.BackStackEntryCount)
                {
                    case 0:
                        trans.AddToBackStack(null);
                        break;
                }

                trans.Commit();

                TopFragment.TranslationY = 1200;
                TopFragment.Animate().SetInterpolator(new FastOutSlowInInterpolator()).TranslationYBy(-1200)
                    .SetDuration(500);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
          
    }
}  
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS; 
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using WoWonder.Library.Anjo.IntegrationRecyclerView;
using Bumptech.Glide.Request;
using Bumptech.Glide.Util;
using Developer.SEmojis.Actions;
using Developer.SEmojis.Helper;
using TheArtOfDev.Edmodo.Cropper;
using Java.IO;
using Newtonsoft.Json;
using WoWonder.Activities.Base;
using WoWonder.Activities.Comment.Adapters;
using WoWonder.Activities.NativePost.Extra;
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Comments;
using WoWonderClient.Requests;
using Console = System.Console;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.Comment
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class ReplyCommentActivity : BaseActivity
    {
        #region Variables Basic

        private static ReplyCommentActivity Instance;
        public ReplyCommentAdapter MAdapter;
        private ViewStub CommentLayout;
        private RecyclerView MRecycler;
        private LinearLayoutManager LayoutManager;
        private RecyclerViewOnScrollListener MainScrollEvent;
        private View CommentLayoutView;
        private TextView ReplyCountTextView;
        public EmojiconEditText TxtComment;
        private ImageView ImgBack, ImgSent, ImgGallery;
        private CommentObjectExtra CommentObject;
        private string CommentId, PathImage;
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
                SetContentView(Resource.Layout.Native_Reply_Comment_Layout);

                Instance = this;

                CommentId = Intent?.GetStringExtra("CommentId") ?? string.Empty;
                CommentObject = JsonConvert.DeserializeObject<CommentObjectExtra>(Intent?.GetStringExtra("CommentObject") ?? "");

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

                EmojisView = FindViewById<ImageView>(Resource.Id.emojiicon);
                TxtComment = FindViewById<EmojiconEditText>(Resource.Id.commenttext);
                ImgSent = FindViewById<ImageView>(Resource.Id.send);
                ImgGallery = FindViewById<ImageView>(Resource.Id.image);
                ImgBack = FindViewById<ImageView>(Resource.Id.back);
                CommentLayout = FindViewById<ViewStub>(Resource.Id.comment_layout);

                ReplyCountTextView = FindViewById<TextView>(Resource.Id.replyCountTextview);
                 
                switch (AppSettings.FlowDirectionRightToLeft)
                {
                    case true:
                        ImgBack.SetImageResource(Resource.Drawable.ic_action_ic_back_rtl);
                        break;
                }

                ImgGallery.SetImageDrawable(AppSettings.SetTabDarkTheme ? GetDrawable(Resource.Drawable.ic_action_addpost_Ligth) : GetDrawable(Resource.Drawable.ic_action_AddPost));

                var repliesCount = !string.IsNullOrEmpty(CommentObject.RepliesCount) ? CommentObject.RepliesCount : CommentObject.Replies ?? ""; 
                ReplyCountTextView.Text = repliesCount + " " + GetString(Resource.String.Lbl_Replies);

                Methods.SetColorEditText(TxtComment, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                TxtComment.Text = "@" + CommentObject?.Publisher?.Username + " " ?? "";
                PathImage = "";

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
                MAdapter = new ReplyCommentAdapter(this)
                {
                    ReplyCommentList = new ObservableCollection<CommentObjectExtra>()
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
                        break;
                    default:
                        ImgSent.Click -= ImgSentOnClick;
                        ImgGallery.Click -= ImgGalleryOnClick;
                        ImgBack.Click -= ImgBackOnClick;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static ReplyCommentActivity GetInstance()
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
                MRecycler = null!;
                CommentLayout = null!;
                CommentLayoutView = null!;
                ReplyCountTextView = null!;
                ImgBack = null!;
                ImgSent = null!;
                ImgGallery = null!;
                CommentObject = null!;
                CommentId = null!;
                PathImage = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events
         
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
                if (string.IsNullOrEmpty(TxtComment.Text) && string.IsNullOrWhiteSpace(TxtComment.Text) && string.IsNullOrEmpty(PathImage))
                    return;

                if (Methods.CheckConnectivity())
                { 
                    CommentObject.Replies ??= "0";
                    CommentObject.RepliesCount ??= "0";

                    //Comment Code 
                    var dataUser = ListUtils.MyProfileList?.FirstOrDefault();

                    var unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    CommentObjectExtra comment = new CommentObjectExtra
                    {
                        Id = unixTimestamp.ToString(),
                        PostId = CommentObject.PostId,
                        UserId = UserDetails.UserId,
                        Text = TxtComment.Text,
                        Time = unixTimestamp.ToString(),
                        CFile = PathImage,
                        Record = "",
                        Publisher = dataUser,
                        Url = dataUser?.Url,
                        Fullurl = CommentObject?.Fullurl,
                        Orginaltext = TxtComment.Text,
                        Owner = true,
                        CommentLikes = "0",
                        CommentWonders = "0",
                        IsCommentLiked = false,
                        Replies = "0",
                        RepliesCount = "0",
                    };

                    MAdapter.ReplyCommentList.Add(comment);

                    var index = MAdapter.ReplyCommentList.IndexOf(comment);
                    switch (index)
                    {
                        case > -1:
                            MAdapter.NotifyItemInserted(index);
                            break;
                    }

                    MRecycler.Visibility = ViewStates.Visible;

                    var dd = MAdapter.ReplyCommentList.FirstOrDefault();
                    if (dd?.Text == MAdapter.EmptyState)
                    {
                        MAdapter.ReplyCommentList.Remove(dd);
                        MAdapter.NotifyItemRemoved(MAdapter.ReplyCommentList.IndexOf(dd));
                    }

                    var repliesCount = !string.IsNullOrEmpty(CommentObject.RepliesCount) ? CommentObject.RepliesCount : CommentObject.Replies ?? "";

                    //CommentLayout.Visibility = ViewStates.Gone;
                    bool success = int.TryParse(repliesCount, out var number);
                    switch (success)
                    {
                        case true:
                        {
                            Console.WriteLine("Converted '{0}' to {1}.", repliesCount, number);
                            var x = number + 1;
                            ReplyCountTextView.Text = x + " " + GetString(Resource.String.Lbl_Replies);
                            break;
                        }
                        default:
                            Console.WriteLine("Attempted conversion of '{0}' failed.", repliesCount ?? "<null>");
                            ReplyCountTextView.Text = 1 + " " + GetString(Resource.String.Lbl_Replies);
                            break;
                    }

                    ImgGallery.SetImageDrawable(AppSettings.SetTabDarkTheme ? GetDrawable(Resource.Drawable.ic_action_addpost_Ligth) : GetDrawable(Resource.Drawable.ic_action_AddPost));

                    var text = TxtComment.Text;

                    //Hide keyboard
                    TxtComment.Text = "";

                    var (apiStatus, respond) = await RequestsAsync.Comment.CreatePostCommentsAsync(CommentId, text, PathImage, "", "create_reply");
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case CreateComments result:
                                {
                                    var date = MAdapter.ReplyCommentList.FirstOrDefault(a => a.Id == comment.Id) ?? MAdapter.ReplyCommentList.FirstOrDefault(x => x.Id == result.Data.Id);
                                    if (date != null)
                                    {
                                        var db = ClassMapper.Mapper?.Map<CommentObjectExtra>(result.Data);

                                        date = db;
                                        date.Id = result.Data.Id;

                                        index = MAdapter.ReplyCommentList.IndexOf(MAdapter.ReplyCommentList.FirstOrDefault(a => a.Id == unixTimestamp.ToString()));
                                        MAdapter.ReplyCommentList[index] = index switch
                                        {
                                            > -1 => db,
                                            _ => MAdapter.ReplyCommentList[index]
                                        };

                                        var commentAdapter = CommentActivity.GetInstance()?.MAdapter;
                                        var commentObject = commentAdapter?.CommentList?.FirstOrDefault(a => a.Id == CommentId);
                                        if (commentObject != null)
                                        {
                                            commentObject.Replies = commentAdapter.CommentList.Count.ToString();
                                            commentObject.RepliesCount = commentAdapter.CommentList.Count.ToString();
                                            commentAdapter.NotifyDataSetChanged();
                                        }

                                        var postFeedAdapter = TabbedMainActivity.GetInstance()?.NewsFeedTab?.PostFeedAdapter;
                                        var dataGlobal = postFeedAdapter?.ListDiffer?.Where(a => a.PostData?.Id == CommentObject?.PostId).ToList();
                                        switch (dataGlobal?.Count)
                                        {
                                            case > 0:
                                            {
                                                foreach (var dataClass in from dataClass in dataGlobal let indexCom = postFeedAdapter.ListDiffer.IndexOf(dataClass) where indexCom > -1 select dataClass)
                                                {
                                                    switch (dataClass.PostData.GetPostComments?.Count)
                                                    {
                                                        case > 0:
                                                        {
                                                            var dataComment = dataClass.PostData.GetPostComments.FirstOrDefault(a => a.Id == date.Id);
                                                            if (dataComment != null)
                                                            {
                                                                dataComment.Replies = MAdapter.ReplyCommentList.Count.ToString();
                                                                dataComment.RepliesCount = MAdapter.ReplyCommentList.Count.ToString();
                                                            }

                                                            break;
                                                        }
                                                    }

                                                    postFeedAdapter.NotifyItemChanged(postFeedAdapter.ListDiffer.IndexOf(dataClass), "commentReplies");
                                                }

                                                break;
                                            }
                                        }

                                        var postFeedAdapter2 = WRecyclerView.GetInstance()?.NativeFeedAdapter;
                                        var dataGlobal1 = postFeedAdapter2?.ListDiffer?.Where(a => a.PostData?.Id == CommentObject?.PostId).ToList();
                                        switch (dataGlobal1?.Count)
                                        {
                                            case > 0:
                                            {
                                                foreach (var dataClass in from dataClass in dataGlobal1 let indexCom = postFeedAdapter2.ListDiffer.IndexOf(dataClass) where indexCom > -1 select dataClass)
                                                {
                                                    switch (dataClass.PostData.GetPostComments?.Count)
                                                    {
                                                        case > 0:
                                                        {
                                                            var dataComment = dataClass.PostData.GetPostComments.FirstOrDefault(a => a.Id == date.Id);
                                                            if (dataComment != null)
                                                            {
                                                                dataComment.Replies = MAdapter.ReplyCommentList.Count.ToString();
                                                                dataComment.RepliesCount = MAdapter.ReplyCommentList.Count.ToString();
                                                            }

                                                            break;
                                                        }
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

        //Scroll
        private void MainScrollEventOnLoadMoreEvent(object sender, EventArgs e)
        {
            try
            {
                //Code get last id where LoadMore >>
                var item = MAdapter.ReplyCommentList.LastOrDefault();
                if (item != null && !string.IsNullOrEmpty(item.Id) && !MainScrollEvent.IsLoading)
                    StartApiService(item.Id);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Load Comment Reply

        private void LoadDataPost()
        {
            try
            { 
                if (CommentObject != null)
                {
                    var repliesCount = !string.IsNullOrEmpty(CommentObject.RepliesCount) ? CommentObject.RepliesCount : CommentObject.Replies ?? "";
                    ReplyCountTextView.Text = repliesCount + " " + GetString(Resource.String.Lbl_Replies);
                    CommentLayout.LayoutResource = string.IsNullOrEmpty(CommentObject.CFile) ? Resource.Layout.Style_Comment : Resource.Layout.Style_Comment_Image;
                }

                CommentLayoutView = CommentLayout.Inflate();

                var holder = new CommentAdapterViewHolder(CommentLayoutView, MAdapter, new CommentClickListener(this, "Reply"))
                {
                    ReplyTextView = { Visibility = ViewStates.Gone }
                };

                //Load data same as comment adapter
                var commentAdapter = new CommentAdapter(this);
                commentAdapter.LoadCommentData(CommentObject, holder);
                 
                StartApiService(); 
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
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => LoadDataCommentReply(offset) });
        }

        private async Task LoadDataCommentReply(string offset)
        {
            switch (MainScrollEvent.IsLoading)
            {
                case true:
                    return;
            }

            if (Methods.CheckConnectivity())
            {
                MainScrollEvent.IsLoading = true;
                var countList = MAdapter.ReplyCommentList.Count;
                var (apiStatus, respond) = await RequestsAsync.Comment.GetPostCommentsAsync(CommentId, "10", offset, "fetch_comments_reply");
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
                                CommentObjectExtra check = MAdapter.ReplyCommentList.FirstOrDefault(a => a.Id == item.Id);
                                switch (check)
                                {
                                    case null:
                                    {
                                        var db = ClassMapper.Mapper?.Map<CommentObjectExtra>(item);
                                        if (db != null) MAdapter.ReplyCommentList.Add(db);
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

                switch (MAdapter.ReplyCommentList.Count)
                {
                    case > 0:
                    {
                        var emptyStateChecker = MAdapter.ReplyCommentList.FirstOrDefault(a => a.Text == MAdapter.EmptyState);
                        if (emptyStateChecker != null && MAdapter.ReplyCommentList.Count > 1)
                        {
                            MAdapter.ReplyCommentList.Remove(emptyStateChecker);
                            MAdapter.NotifyDataSetChanged();
                        }

                        break;
                    }
                    default:
                    {
                        MAdapter.ReplyCommentList.Clear();
                        var d = new CommentObjectExtra { Text = MAdapter.EmptyState };
                        MAdapter.ReplyCommentList.Add(d);
                        MAdapter.NotifyDataSetChanged();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                MainScrollEvent.IsLoading = false;
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
                                Toast.MakeText(this, GetText(Resource.String.Lbl_something_went_wrong), ToastLength.Long)?.Show();
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
                var list = MAdapter.ReplyCommentList.Where(a => !string.IsNullOrEmpty(a.Record) && a.MediaPlayer != null).ToList();
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
         
    }
} 
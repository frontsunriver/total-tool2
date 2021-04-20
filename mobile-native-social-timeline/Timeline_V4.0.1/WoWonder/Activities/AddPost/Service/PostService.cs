using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Iceteck.SiliCompressorrLib;
using Java.Lang;
using Java.Net;
using Newtonsoft.Json; 
using WoWonder.Activities.Communities.Groups;
using WoWonder.Activities.Communities.Pages;
using WoWonder.Activities.Events;
using WoWonder.Activities.MyProfile;
using WoWonder.Activities.NativePost.Extra;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Classes.Story;
using WoWonderClient.Requests;
using Exception = System.Exception;
using File = Java.IO.File;

namespace WoWonder.Activities.AddPost.Service
{
    public class FileUpload
    {
        public string IdPost { set; get; }
        public string PagePost { set; get; }
        public string Content { set; get; }
        public string PostPrivacy { set; get; }
        public string PostFeelingType { set; get; }
        public string PostFeelingText { set; get; }
        public string PlaceText { set; get; }
        public ObservableCollection<Attachments> AttachmentList { set; get; }
        public ObservableCollection<PollAnswers> AnswersList { set; get; }
        public string IdColor { set; get; }
        public string AlbumName { set; get; }

        //Story
        public string StoryTitle { set; get; }
        public string StoryDescription { set; get; }
        public string StoryFilePath { set; get; }
        public string StoryFileType { set; get; }
        public string StoryThumbnail { set; get; }
    }

    [Service(Exported = false)]
    public class PostService : Android.App.Service
    {
        #region Variables Basic

        public string ActionPostService;
        public static string ActionPost;
        public static string ActionStory;
        private static string PagePost;
        private static PostService Service;
        private AddPostActivity GlobalContextPost;
        private TabbedMainActivity GlobalContextTabbed;
        public FileUpload DataPost;

        #endregion

        #region General

        public static PostService GetPostService()
        {
            return Service;
        }
         
        public override IBinder OnBind(Intent intent)
        {
            return null!;
        }
         
        public override void OnCreate()
        {
            try
            {
                base.OnCreate();
                Service = this;

                GlobalContextPost = AddPostActivity.GetInstance();
                GlobalContextTabbed = TabbedMainActivity.GetInstance();
                MNotificationManager = (NotificationManager)GetSystemService(NotificationService);

                Create_Progress_Notification();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            try
            {
                base.OnStartCommand(intent, flags, startId);

                ActionPostService = intent.Action;
                var data = intent.GetStringExtra("DataPost");
                PagePost = intent.GetStringExtra("PagePost") ?? "";
                switch (string.IsNullOrEmpty(data))
                {
                    case false:
                    {
                        DataPost = JsonConvert.DeserializeObject<FileUpload>(data);
                        if (ActionPostService == ActionPost)
                        {
                            if (DataPost != null)
                            {
                                var attach = DataPost.AttachmentList.FirstOrDefault();
                                switch (AppSettings.EnableVideoCompress)
                                {
                                    case true when attach?.TypeAttachment == "postVideo":
                                    {
                                        File destinationPath = new File(Methods.Path.FolderDcimVideo + "/Compressor");

                                        if (!Directory.Exists(destinationPath.Path))
                                            Directory.CreateDirectory(destinationPath.Path);

                                        new VideoCompressAsyncTask(this).Execute("false", attach.FileUrl, destinationPath.Path);
                                        break;
                                    }
                                    default:
                                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { AddPost });
                                        break;
                                }
                            }
                        }
                        else if (ActionPostService == ActionStory)
                        { 
                            if (DataPost != null)
                            {
                                switch (AppSettings.EnableVideoCompress)
                                {
                                    case true when DataPost.StoryFileType != "image":
                                    {
                                        File destinationPath = new File(Methods.Path.FolderDcimVideo + "/Compressor");

                                        if (!Directory.Exists(destinationPath.Path))
                                            Directory.CreateDirectory(destinationPath.Path);

                                        Task.Factory.StartNew(() => new VideoCompressAsyncTask(this).Execute("false", DataPost.StoryFilePath, destinationPath.Path));
                                        break;
                                    }
                                    default:
                                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { AddStory });
                                        break;
                                }

                                UpdateNotification("Story");
                            }
                        }

                        break;
                    }
                }

                return StartCommandResult.Sticky;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return StartCommandResult.NotSticky;
            }
        }

        public async Task AddPost()
        {
            try
            {
                var (apiStatus, respond) = await RequestsAsync.Posts.AddNewPostAsync(UserDetails.UserId , DataPost.IdPost, DataPost.PagePost, DataPost.Content, DataPost.PostPrivacy, DataPost.PostFeelingType, DataPost.PostFeelingText, DataPost.PlaceText, DataPost.AttachmentList, DataPost.AnswersList, DataPost.IdColor, DataPost.AlbumName);
                switch (apiStatus)
                {
                    case 200:
                    {
                        switch (respond)
                        {
                            case AddPostObject postObject:
                                Toast.MakeText(Application.Context, Application.Context.GetText(Resource.String.Lbl_Post_Added), ToastLength.Short)?.Show();

                                GlobalContextTabbed?.RunOnUiThread(() =>
                                {
                                    try
                                    {
                                        switch (UserDetails.SoundControl)
                                        {
                                            case true:
                                                Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("PopNotificationPost.mp3");
                                                break;
                                        }

                                        if (postObject.PostData != null)
                                        {
                                            switch (PagePost)
                                            {
                                                case "Normal":
                                                case "Normal_More":
                                                case "Normal_Gallery":
                                                {
                                                    var countList = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ItemCount;

                                                    var combine = new FeedCombiner(ApiPostAsync.RegexFilterText(postObject.PostData), GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer, this);

                                                    var check = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.PostData != null && a.TypeView != PostModelType.AddPostBox && a.TypeView != PostModelType.FilterSection && a.TypeView != PostModelType.SearchForPosts);
                                                    if (check != null)
                                                        combine.CombineDefaultPostSections("Top");
                                                    else
                                                        combine.CombineDefaultPostSections();

                                                    int countIndex = 1;
                                                    var model1 = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.Story);
                                                    var model2 = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                                                    var model3 = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.FilterSection);
                                                    var model4 = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AlertBox);
                                                    var model5 = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SearchForPosts);

                                                    if (model5 != null)
                                                        countIndex += GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(model5) + 1;
                                                    else if (model4 != null)
                                                        countIndex += GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(model4) + 1;
                                                    else if (model3 != null)
                                                        countIndex += GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(model3) + 1;
                                                    else if (model2 != null)
                                                        countIndex += GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(model2) + 1;
                                                    else if (model1 != null)
                                                        countIndex += GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(model1) + 1;
                                                    else
                                                        countIndex = 0;

                                                    var emptyStateChecker = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);
                                                    if (emptyStateChecker != null && GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.Count > 1)
                                                        GlobalContextTabbed.NewsFeedTab.MainRecyclerView.RemoveByRowIndex(emptyStateChecker);

                                                    GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.NotifyItemRangeInserted(countIndex, GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.Count - countList);

                                                    if (DataPost.IdPost == UserDetails.UserId)
                                                    {
                                                        MyProfileActivity myProfileActivity = MyProfileActivity.GetInstance();
                                                        if (myProfileActivity == null)
                                                            return;
                                                        
                                                        var countList1 = myProfileActivity.PostFeedAdapter.ItemCount;

                                                        var combine1 = new FeedCombiner(ApiPostAsync.RegexFilterText(postObject.PostData), myProfileActivity.PostFeedAdapter.ListDiffer, this);

                                                        var check1 = myProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.PostData != null && a.TypeView != PostModelType.AddPostBox && a.TypeView != PostModelType.FilterSection && a.TypeView != PostModelType.SearchForPosts);
                                                        if (check1 != null)
                                                            combine1.CombineDefaultPostSections("Top");
                                                        else
                                                            combine1.CombineDefaultPostSections();

                                                        int countIndex1 = 1;
                                                        var model11 = myProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.Story);
                                                        var model21 = myProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                                                        var model31 = myProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.FilterSection);
                                                        var model41 = myProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AlertBox);
                                                        var model51 = myProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SearchForPosts);

                                                        if (model51 != null)
                                                            countIndex1 += myProfileActivity.PostFeedAdapter.ListDiffer.IndexOf(model51) + 1;
                                                        else if (model41 != null)
                                                            countIndex1 += myProfileActivity.PostFeedAdapter.ListDiffer.IndexOf(model41) + 1;
                                                        else if (model31 != null)
                                                            countIndex1 += myProfileActivity.PostFeedAdapter.ListDiffer.IndexOf(model31) + 1;
                                                        else if (model21 != null)
                                                            countIndex1 += myProfileActivity.PostFeedAdapter.ListDiffer.IndexOf(model21) + 1;
                                                        else if (model11 != null)
                                                            countIndex1 += myProfileActivity.PostFeedAdapter.ListDiffer.IndexOf(model11) + 1;
                                                        else
                                                            countIndex1 = 0;

                                                        var emptyStateChecker1 = myProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);
                                                        if (emptyStateChecker1 != null && myProfileActivity.PostFeedAdapter.ListDiffer.Count > 1)
                                                            myProfileActivity.MainRecyclerView.RemoveByRowIndex(emptyStateChecker1);

                                                        myProfileActivity.PostFeedAdapter.NotifyItemRangeInserted(countIndex1, myProfileActivity.PostFeedAdapter.ListDiffer.Count - countList1);
                                                    }
                                                    break;
                                                }
                                                case "SocialGroup":
                                                case "SocialGroup_More":
                                                case "SocialGroup_Gallery":
                                                {
                                                    GroupProfileActivity groupProfileActivity = GroupProfileActivity.GetInstance(); 
                                                    if (groupProfileActivity == null)
                                                        return;

                                                    var countList = groupProfileActivity.PostFeedAdapter.ItemCount;

                                                    var combine = new FeedCombiner(ApiPostAsync.RegexFilterText(postObject.PostData), groupProfileActivity.PostFeedAdapter.ListDiffer, this);

                                                    var check = groupProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.PostData != null && a.TypeView != PostModelType.AddPostBox && a.TypeView != PostModelType.FilterSection && a.TypeView != PostModelType.SearchForPosts);
                                                    if (check != null)
                                                        combine.CombineDefaultPostSections("Top");
                                                    else
                                                        combine.CombineDefaultPostSections();

                                                    int countIndex = 1;
                                                    var model1 = groupProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.Story);
                                                    var model2 = groupProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                                                    var model3 = groupProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.FilterSection);
                                                    var model4 = groupProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AlertBox);
                                                    var model5 = groupProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SearchForPosts);

                                                    if (model5 != null)
                                                        countIndex += groupProfileActivity.PostFeedAdapter.ListDiffer.IndexOf(model5) + 1;
                                                    else if (model4 != null)
                                                        countIndex += groupProfileActivity.PostFeedAdapter.ListDiffer.IndexOf(model4) + 1;
                                                    else if (model3 != null)
                                                        countIndex += groupProfileActivity.PostFeedAdapter.ListDiffer.IndexOf(model3) + 1;
                                                    else if (model2 != null)
                                                        countIndex += groupProfileActivity.PostFeedAdapter.ListDiffer.IndexOf(model2) + 1;
                                                    else if (model1 != null)
                                                        countIndex += groupProfileActivity.PostFeedAdapter.ListDiffer.IndexOf(model1) + 1;
                                                    else
                                                        countIndex = 0;

                                                    var emptyStateChecker = groupProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);
                                                    if (emptyStateChecker != null && groupProfileActivity.PostFeedAdapter.ListDiffer.Count > 1)
                                                        groupProfileActivity.MainRecyclerView.RemoveByRowIndex(emptyStateChecker);

                                                    groupProfileActivity.PostFeedAdapter.NotifyItemRangeInserted(countIndex, groupProfileActivity.PostFeedAdapter.ListDiffer.Count - countList);
                                                    break;
                                                }
                                                case "SocialPage":
                                                case "SocialPage_More":
                                                case "SocialPage_Gallery":
                                                {
                                                    PageProfileActivity pageProfileActivity = PageProfileActivity.GetInstance();
                                                    if (pageProfileActivity == null)
                                                        return;

                                                    var countList = pageProfileActivity.PostFeedAdapter.ItemCount;

                                                    var combine = new FeedCombiner(ApiPostAsync.RegexFilterText(postObject.PostData), pageProfileActivity.PostFeedAdapter.ListDiffer, this);

                                                    var check = pageProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.PostData != null && a.TypeView != PostModelType.AddPostBox && a.TypeView != PostModelType.FilterSection && a.TypeView != PostModelType.SearchForPosts);
                                                    if (check != null)
                                                        combine.CombineDefaultPostSections("Top");
                                                    else
                                                        combine.CombineDefaultPostSections();

                                                    int countIndex = 1;
                                                    var model1 = pageProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.Story);
                                                    var model2 = pageProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                                                    var model3 = pageProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.FilterSection);
                                                    var model4 = pageProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AlertBox);
                                                    var model5 = pageProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SearchForPosts);

                                                    if (model5 != null)
                                                        countIndex += pageProfileActivity.PostFeedAdapter.ListDiffer.IndexOf(model5) + 1;
                                                    else if (model4 != null)
                                                        countIndex += pageProfileActivity.PostFeedAdapter.ListDiffer.IndexOf(model4) + 1;
                                                    else if (model3 != null)
                                                        countIndex += pageProfileActivity.PostFeedAdapter.ListDiffer.IndexOf(model3) + 1;
                                                    else if (model2 != null)
                                                        countIndex += pageProfileActivity.PostFeedAdapter.ListDiffer.IndexOf(model2) + 1;
                                                    else if (model1 != null)
                                                        countIndex += pageProfileActivity.PostFeedAdapter.ListDiffer.IndexOf(model1) + 1;
                                                    else
                                                        countIndex = 0;

                                                    var emptyStateChecker = pageProfileActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);
                                                    if (emptyStateChecker != null && pageProfileActivity.PostFeedAdapter.ListDiffer.Count > 1)
                                                        pageProfileActivity.MainRecyclerView.RemoveByRowIndex(emptyStateChecker);

                                                    pageProfileActivity.PostFeedAdapter.NotifyItemRangeInserted(countIndex, pageProfileActivity.PostFeedAdapter.ListDiffer.Count - countList);
                                                    break;
                                                }
                                                case "SocialEvent":
                                                case "SocialEvent_More":
                                                case "SocialEvent_Gallery":
                                                {
                                                    EventViewActivity eventViewActivity = EventViewActivity.GetInstance();
                                                    if (eventViewActivity == null)
                                                        return;

                                                    var countList = eventViewActivity.PostFeedAdapter.ItemCount;

                                                    var combine = new FeedCombiner(ApiPostAsync.RegexFilterText(postObject.PostData), eventViewActivity.PostFeedAdapter.ListDiffer, this);

                                                    var check = eventViewActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.PostData != null && a.TypeView != PostModelType.AddPostBox && a.TypeView != PostModelType.FilterSection && a.TypeView != PostModelType.SearchForPosts);
                                                    if (check != null)
                                                        combine.CombineDefaultPostSections("Top");
                                                    else
                                                        combine.CombineDefaultPostSections();

                                                    int countIndex = 1;
                                                    var model1 = eventViewActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.Story);
                                                    var model2 = eventViewActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                                                    var model3 = eventViewActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.FilterSection);
                                                    var model4 = eventViewActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AlertBox);
                                                    var model5 = eventViewActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SearchForPosts);

                                                    if (model5 != null)
                                                        countIndex += eventViewActivity.PostFeedAdapter.ListDiffer.IndexOf(model5) + 1;
                                                    else if (model4 != null)
                                                        countIndex += eventViewActivity.PostFeedAdapter.ListDiffer.IndexOf(model4) + 1;
                                                    else if (model3 != null)
                                                        countIndex += eventViewActivity.PostFeedAdapter.ListDiffer.IndexOf(model3) + 1;
                                                    else if (model2 != null)
                                                        countIndex += eventViewActivity.PostFeedAdapter.ListDiffer.IndexOf(model2) + 1;
                                                    else if (model1 != null)
                                                        countIndex += eventViewActivity.PostFeedAdapter.ListDiffer.IndexOf(model1) + 1;
                                                    else
                                                        countIndex = 0;

                                                    var emptyStateChecker = eventViewActivity.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);
                                                    if (emptyStateChecker != null && eventViewActivity.PostFeedAdapter.ListDiffer.Count > 1)
                                                        eventViewActivity.MainRecyclerView.RemoveByRowIndex(emptyStateChecker);

                                                    eventViewActivity.PostFeedAdapter.NotifyItemRangeInserted(countIndex, eventViewActivity.PostFeedAdapter.ListDiffer.Count - countList);
                                                    break;
                                                }
                                            }
                                        }
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
                    default:
                        Methods.DisplayReportResult(GlobalContextPost, respond);
                        break;
                }
                RemoveNotification();
            }
            catch (Exception e)
            {
                RemoveNotification();
                Methods.DisplayReportResultTrack(e);
            }
        }

        public async Task AddStory()
        {
            try
            {
                var postFeedAdapter = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter;
                var checkSection = postFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.Story);
                if (checkSection != null)
                {
                    var modelStory = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.HolderStory.StoryAdapter;

                    string time = Methods.Time.TimeAgo(DateTime.Now, false);
                    var unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    string time2 = unixTimestamp.ToString();

                    var userData = ListUtils.MyProfileList?.FirstOrDefault();

                    //just pass file_path and type video or image
                    var (apiStatus, respond) = await RequestsAsync.Story.CreateStoryAsync(DataPost.StoryTitle, DataPost.StoryDescription, DataPost.StoryFilePath, DataPost.StoryFileType, DataPost.StoryThumbnail);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case CreateStoryObject result:
                                {
                                    Toast.MakeText(GlobalContextTabbed, GlobalContextTabbed.GetText(Resource.String.Lbl_Story_Added), ToastLength.Short)?.Show();

                                    var check = modelStory.StoryList?.FirstOrDefault(a => a.UserId == UserDetails.UserId);
                                    if (check != null)
                                    {
                                        switch (DataPost.StoryFileType)
                                        {
                                            case "image":
                                            {
                                                var item = new StoryDataObject.Story
                                                {
                                                    UserId = UserDetails.UserId,
                                                    Id = result.StoryId,
                                                    Description = DataPost.StoryDescription,
                                                    Title = DataPost.StoryTitle,
                                                    TimeText = time,
                                                    IsOwner = true,
                                                    Expire = "",
                                                    Posted = time2,
                                                    Thumbnail = DataPost.StoryFilePath,
                                                    UserData = userData,
                                                    Images = new List<StoryDataObject.Image>(),
                                                    Videos = new List<StoryDataObject.Video>()
                                                };

                                                check.DurationsList ??= new List<long> { AppSettings.StoryDuration }; 
                                                check.DurationsList.Add(AppSettings.StoryDuration);

                                                check.Stories.Add(item);
                                                            break;
                                            }
                                            default:
                                            {
                                                var item = new StoryDataObject.Story
                                                {
                                                    UserId = UserDetails.UserId,
                                                    Id = result.StoryId,
                                                    Description = DataPost.StoryDescription,
                                                    Title = DataPost.StoryTitle,
                                                    TimeText = time,
                                                    IsOwner = true,
                                                    Expire = "",
                                                    Posted = time2,
                                                    Thumbnail = DataPost.StoryThumbnail,
                                                    UserData = userData,
                                                    Images = new List<StoryDataObject.Image>(),
                                                    Videos = new List<StoryDataObject.Video>
                                                    {
                                                        new StoryDataObject.Video
                                                        {
                                                            StoryId = result.StoryId,
                                                            Filename = DataPost.StoryFilePath,
                                                            Id = time2,
                                                            Expire = time2,
                                                            Type = "video",
                                                        }
                                                    }
                                                };

                                                var duration = WoWonderTools.GetDuration(DataPost.StoryFilePath);

                                                check.DurationsList ??= new List<long> { Long.ParseLong(duration) };
                                                check.DurationsList.Add(Long.ParseLong(duration)); 
                                                         
                                                check.Stories.Add(item);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        switch (DataPost.StoryFileType)
                                        {
                                            case "image":
                                            {
                                                var item = new StoryDataObject
                                                {
                                                    Type = "image",
                                                    Stories = new List<StoryDataObject.Story>
                                                    {
                                                        new StoryDataObject.Story
                                                        {
                                                            UserId = UserDetails.UserId,
                                                            Id = result.StoryId,
                                                            Description = DataPost.StoryDescription,
                                                            Title = DataPost.StoryTitle,
                                                            TimeText = time,
                                                            IsOwner = true,
                                                            Expire = "",
                                                            Posted = time2,
                                                            Thumbnail = DataPost.StoryFilePath,
                                                            UserData = userData,
                                                            Images = new List<StoryDataObject.Image>(),
                                                            Videos = new List<StoryDataObject.Video>(),
                                                        }

                                                    },
                                                    UserId = userData?.UserId,
                                                    Username = userData?.Username,
                                                    Email = userData?.Email,
                                                    FirstName = userData?.FirstName,
                                                    LastName = userData?.LastName,
                                                    Avatar = userData?.Avatar,
                                                    Cover = userData?.Cover,
                                                    BackgroundImage = userData?.BackgroundImage,
                                                    RelationshipId = userData?.RelationshipId,
                                                    Address = userData?.Address,
                                                    Working = userData?.Working,
                                                    Gender = userData?.Gender,
                                                    Facebook = userData?.Facebook,
                                                    Google = userData?.Google,
                                                    Twitter = userData?.Twitter,
                                                    Linkedin = userData?.Linkedin,
                                                    Website = userData?.Website,
                                                    Instagram = userData?.Instagram,
                                                    WebDeviceId = userData?.WebDeviceId,
                                                    Language = userData?.Language,
                                                    IpAddress = userData?.IpAddress,
                                                    PhoneNumber = userData?.PhoneNumber,
                                                    Timezone = userData?.Timezone,
                                                    Lat = userData?.Lat,
                                                    Lng = userData?.Lng,
                                                    About = userData?.About,
                                                    Birthday = userData?.Birthday,
                                                    Registered = userData?.Registered,
                                                    Lastseen = userData?.Lastseen,
                                                    LastLocationUpdate = userData?.LastLocationUpdate,
                                                    Balance = userData?.Balance,
                                                    Verified = userData?.Verified,
                                                    Status = userData?.Status,
                                                    Active = userData?.Active,
                                                    Admin = userData?.Admin,
                                                    IsPro = userData?.IsPro,
                                                    ProType = userData?.ProType,
                                                    School = userData?.School,
                                                    Name = userData?.Name,
                                                    AndroidMDeviceId = userData?.AndroidMDeviceId,
                                                    ECommented = userData?.ECommented,
                                                    AndroidNDeviceId = userData?.AndroidMDeviceId,
                                                    AvatarFull = userData?.AvatarFull,
                                                    BirthPrivacy = userData?.BirthPrivacy,
                                                    CanFollow = userData?.CanFollow,
                                                    ConfirmFollowers = userData?.ConfirmFollowers,
                                                    CountryId = userData?.CountryId,
                                                    EAccepted = userData?.EAccepted,
                                                    EFollowed = userData?.EFollowed,
                                                    EJoinedGroup = userData?.EJoinedGroup,
                                                    ELastNotif = userData?.ELastNotif,
                                                    ELiked = userData?.ELiked,
                                                    ELikedPage = userData?.ELikedPage,
                                                    EMentioned = userData?.EMentioned,
                                                    EProfileWallPost = userData?.EProfileWallPost,
                                                    ESentmeMsg = userData?.ESentmeMsg,
                                                    EShared = userData?.EShared,
                                                    EVisited = userData?.EVisited,
                                                    EWondered = userData?.EWondered,
                                                    EmailNotification = userData?.EmailNotification,
                                                    FollowPrivacy = userData?.FollowPrivacy,
                                                    FriendPrivacy = userData?.FriendPrivacy,
                                                    GenderText = userData?.GenderText,
                                                    InfoFile = userData?.InfoFile,
                                                    IosMDeviceId = userData?.IosMDeviceId,
                                                    IosNDeviceId = userData?.IosNDeviceId,
                                                    IsFollowing = userData?.IsFollowing,
                                                    IsFollowingMe = userData?.IsFollowingMe,
                                                    LastAvatarMod = userData?.LastAvatarMod,
                                                    LastCoverMod = userData?.LastCoverMod,
                                                    LastDataUpdate = userData?.LastDataUpdate,
                                                    LastFollowId = userData?.LastFollowId,
                                                    LastLoginData = userData?.LastLoginData,
                                                    LastseenStatus = userData?.LastseenStatus,
                                                    LastseenTimeText = userData?.LastseenTimeText,
                                                    LastseenUnixTime = userData?.LastseenUnixTime,
                                                    MessagePrivacy = userData?.MessagePrivacy,
                                                    NewEmail = userData?.NewEmail,
                                                    NewPhone = userData?.NewPhone,
                                                    NotificationSettings = userData?.NotificationSettings,
                                                    NotificationsSound = userData?.NotificationsSound,
                                                    OrderPostsBy = userData?.OrderPostsBy,
                                                    PaypalEmail = userData?.PaypalEmail,
                                                    PostPrivacy = userData?.PostPrivacy,
                                                    Referrer = userData?.Referrer,
                                                    ShareMyData = userData?.ShareMyData,
                                                    ShareMyLocation = userData?.ShareMyLocation,
                                                    ShowActivitiesPrivacy = userData?.ShowActivitiesPrivacy,
                                                    TwoFactor = userData?.TwoFactor,
                                                    TwoFactorVerified = userData?.TwoFactorVerified,
                                                    Url = userData?.Url,
                                                    VisitPrivacy = userData?.VisitPrivacy,
                                                    Vk = userData?.Vk,
                                                    Wallet = userData?.Wallet,
                                                    WorkingLink = userData?.WorkingLink,
                                                    Youtube = userData?.Youtube,
                                                    City = userData?.City,
                                                    Points = userData?.Points,
                                                    DailyPoints = userData?.DailyPoints,
                                                    PointDayExpire = userData?.PointDayExpire,
                                                    State = userData?.State,
                                                    Zip = userData?.Zip,
                                                    Details = new DetailsUnion
                                                    {
                                                        DetailsClass = new Details(),
                                                    },
                                                };

                                                item.DurationsList ??= new List<long> { AppSettings.StoryDuration };
                                                item.DurationsList.Add(AppSettings.StoryDuration);

                                                modelStory.StoryList?.Add(item);
                                                break;
                                            }
                                            default:
                                            {
                                                var item = new StoryDataObject
                                                {
                                                    Type = "video",
                                                    Stories = new List<StoryDataObject.Story>
                                                    {
                                                        new StoryDataObject.Story
                                                        {
                                                            UserId = UserDetails.UserId,
                                                            Id = result.StoryId,
                                                            Description = DataPost.StoryDescription,
                                                            Title = DataPost.StoryTitle,
                                                            TimeText = time,
                                                            IsOwner = true,
                                                            Expire = "",
                                                            Posted = time2,
                                                            Thumbnail = DataPost.StoryThumbnail,
                                                            UserData = userData,
                                                            Images = new List<StoryDataObject.Image>(),
                                                            Videos = new List<StoryDataObject.Video>
                                                            {
                                                                new StoryDataObject.Video
                                                                {
                                                                    StoryId = result.StoryId,
                                                                    Filename = DataPost.StoryFilePath,
                                                                    Id = time2,
                                                                    Expire = time2,
                                                                    Type = "video",
                                                                }
                                                            }
                                                        },
                                                    },
                                                    UserId = userData?.UserId,
                                                    Username = userData?.Username,
                                                    Email = userData?.Email,
                                                    FirstName = userData?.FirstName,
                                                    LastName = userData?.LastName,
                                                    Avatar = userData?.Avatar,
                                                    Cover = userData?.Cover,
                                                    BackgroundImage = userData?.BackgroundImage,
                                                    RelationshipId = userData?.RelationshipId,
                                                    Address = userData?.Address,
                                                    Working = userData?.Working,
                                                    Gender = userData?.Gender,
                                                    Facebook = userData?.Facebook,
                                                    Google = userData?.Google,
                                                    Twitter = userData?.Twitter,
                                                    Linkedin = userData?.Linkedin,
                                                    Website = userData?.Website,
                                                    Instagram = userData?.Instagram,
                                                    WebDeviceId = userData?.WebDeviceId,
                                                    Language = userData?.Language,
                                                    IpAddress = userData?.IpAddress,
                                                    PhoneNumber = userData?.PhoneNumber,
                                                    Timezone = userData?.Timezone,
                                                    Lat = userData?.Lat,
                                                    Lng = userData?.Lng,
                                                    About = userData?.About,
                                                    Birthday = userData?.Birthday,
                                                    Registered = userData?.Registered,
                                                    Lastseen = userData?.Lastseen,
                                                    LastLocationUpdate = userData?.LastLocationUpdate,
                                                    Balance = userData?.Balance,
                                                    Verified = userData?.Verified,
                                                    Status = userData?.Status,
                                                    Active = userData?.Active,
                                                    Admin = userData?.Admin,
                                                    IsPro = userData?.IsPro,
                                                    ProType = userData?.ProType,
                                                    School = userData?.School,
                                                    Name = userData?.Name,
                                                    AndroidMDeviceId = userData?.AndroidMDeviceId,
                                                    ECommented = userData?.ECommented,
                                                    AndroidNDeviceId = userData?.AndroidMDeviceId,
                                                    AvatarFull = userData?.AvatarFull,
                                                    BirthPrivacy = userData?.BirthPrivacy,
                                                    CanFollow = userData?.CanFollow,
                                                    ConfirmFollowers = userData?.ConfirmFollowers,
                                                    CountryId = userData?.CountryId,
                                                    EAccepted = userData?.EAccepted,
                                                    EFollowed = userData?.EFollowed,
                                                    EJoinedGroup = userData?.EJoinedGroup,
                                                    ELastNotif = userData?.ELastNotif,
                                                    ELiked = userData?.ELiked,
                                                    ELikedPage = userData?.ELikedPage,
                                                    EMentioned = userData?.EMentioned,
                                                    EProfileWallPost = userData?.EProfileWallPost,
                                                    ESentmeMsg = userData?.ESentmeMsg,
                                                    EShared = userData?.EShared,
                                                    EVisited = userData?.EVisited,
                                                    EWondered = userData?.EWondered,
                                                    EmailNotification = userData?.EmailNotification,
                                                    FollowPrivacy = userData?.FollowPrivacy,
                                                    FriendPrivacy = userData?.FriendPrivacy,
                                                    GenderText = userData?.GenderText,
                                                    InfoFile = userData?.InfoFile,
                                                    IosMDeviceId = userData?.IosMDeviceId,
                                                    IosNDeviceId = userData?.IosNDeviceId,
                                                    IsFollowing = userData?.IsFollowing,
                                                    IsFollowingMe = userData?.IsFollowingMe,
                                                    LastAvatarMod = userData?.LastAvatarMod,
                                                    LastCoverMod = userData?.LastCoverMod,
                                                    LastDataUpdate = userData?.LastDataUpdate,
                                                    LastFollowId = userData?.LastFollowId,
                                                    LastLoginData = userData?.LastLoginData,
                                                    LastseenStatus = userData?.LastseenStatus,
                                                    LastseenTimeText = userData?.LastseenTimeText,
                                                    LastseenUnixTime = userData?.LastseenUnixTime,
                                                    MessagePrivacy = userData?.MessagePrivacy,
                                                    NewEmail = userData?.NewEmail,
                                                    NewPhone = userData?.NewPhone,
                                                    NotificationSettings = userData?.NotificationSettings,
                                                    NotificationsSound = userData?.NotificationsSound,
                                                    OrderPostsBy = userData?.OrderPostsBy,
                                                    PaypalEmail = userData?.PaypalEmail,
                                                    PostPrivacy = userData?.PostPrivacy,
                                                    Referrer = userData?.Referrer,
                                                    ShareMyData = userData?.ShareMyData,
                                                    ShareMyLocation = userData?.ShareMyLocation,
                                                    ShowActivitiesPrivacy = userData?.ShowActivitiesPrivacy,
                                                    TwoFactor = userData?.TwoFactor,
                                                    TwoFactorVerified = userData?.TwoFactorVerified,
                                                    Url = userData?.Url,
                                                    VisitPrivacy = userData?.VisitPrivacy,
                                                    Vk = userData?.Vk,
                                                    Wallet = userData?.Wallet,
                                                    WorkingLink = userData?.WorkingLink,
                                                    Youtube = userData?.Youtube,
                                                    City = userData?.City,
                                                    Points = userData?.Points,
                                                    DailyPoints = userData?.DailyPoints,
                                                    State = userData?.State,
                                                    Zip = userData?.Zip,
                                                    Details = new DetailsUnion
                                                    {
                                                        DetailsClass = new Details(),
                                                    },
                                                };

                                                var duration = WoWonderTools.GetDuration(DataPost.StoryFilePath);

                                                item.DurationsList ??= new List<long> { Long.ParseLong(duration) };
                                                item.DurationsList.Add(Long.ParseLong(duration));

                                                modelStory.StoryList?.Add(item);
                                                break;
                                            }
                                        }
                                    }

                                    modelStory.NotifyDataSetChanged();

                                    switch (UserDetails.SoundControl)
                                    {
                                        case true:
                                            Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("PopNotificationPost.mp3");
                                            break;
                                    }
                                    break;
                                }
                            }

                            break;
                        }
                        default:
                            Methods.DisplayReportResult(GlobalContextTabbed, respond);
                            break;
                    }
                }

                RemoveNotification();
            }
            catch (Exception e)
            {
                RemoveNotification();
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        #region Notification

        private readonly string NotificationChannelId = "wowonder_ch_1";

        private NotificationManager MNotificationManager;
        private NotificationCompat.Builder NotificationBuilder;
        private RemoteViews NotificationView;
        private void Create_Progress_Notification()
        {
            try
            {
                MNotificationManager = (NotificationManager)GetSystemService(NotificationService);

                NotificationView = new RemoteViews(PackageName, Resource.Layout.ViewProgressNotification);

                Intent resultIntent = new Intent();
                PendingIntent resultPendingIntent = PendingIntent.GetActivity(this, 0, resultIntent, PendingIntentFlags.UpdateCurrent);
                NotificationBuilder = new NotificationCompat.Builder(this, NotificationChannelId);
                NotificationBuilder.SetSmallIcon(Resource.Mipmap.icon);
                NotificationBuilder.SetColor(ContextCompat.GetColor(this, Resource.Color.accent));
                NotificationBuilder.SetCustomContentView(NotificationView)
                    .SetOngoing(true)
                    .SetContentIntent(resultPendingIntent)
                    .SetDefaults(NotificationCompat.DefaultAll)
                    .SetPriority((int)NotificationPriority.High);

                NotificationBuilder.SetVibrate(new[] { 0L });
                NotificationBuilder.SetVisibility(NotificationCompat.VisibilityPublic);

                switch (Build.VERSION.SdkInt)
                {
                    case >= BuildVersionCodes.O:
                    {
                        var importance = NotificationImportance.High;
                        NotificationChannel notificationChannel = new NotificationChannel(NotificationChannelId, AppSettings.ApplicationName, importance);
                        notificationChannel.EnableLights(false);
                        notificationChannel.EnableVibration(false);
                        NotificationBuilder.SetChannelId(NotificationChannelId);

                        MNotificationManager?.CreateNotificationChannel(notificationChannel);
                        break;
                    }
                }

                MNotificationManager?.Notify(2020, NotificationBuilder.Build());
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void RemoveNotification()
        {
            try
            {
                NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager?.Cancel(2020);

                MNotificationManager?.CancelAll(); 
                StopSelf();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void UpdateNotification(string type)
        {
            try
            {
                switch (type)
                {
                    case "Post":
                        NotificationView.SetTextViewText(Resource.Id.title, GetString(Resource.String.Lbl_UploadingPost));
                        break;
                    case "Story":
                        NotificationView.SetTextViewText(Resource.Id.title, GetString(Resource.String.Lbl_UploadingStory));
                        break;
                }

                MNotificationManager?.Notify(2020, NotificationBuilder.Build());
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion
         
    }
     
    public class VideoCompressAsyncTask : AsyncTask<string, string, string>
    {
        private readonly Context MContext;
        private readonly PostService Service;

        public VideoCompressAsyncTask(Context context )
        {
            try
            {
                MContext = context;
                Service = PostService.GetPostService();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         

        protected override string RunInBackground(params string[] paths)
        {
            string filePath = null!;
            try
            {
                //This bellow is just a temporary solution to test that method call works
                var b = bool.Parse(paths[0]);
                switch (b)
                {
                    case true:
                        filePath = SiliCompressor.With(MContext).CompressVideo(paths[1], paths[2]);
                        break;
                    default:
                    {
                        Android.Net.Uri videoContentUri = Android.Net.Uri.Parse(paths[1]);

                        // Example using the bitrate and video size parameters = >> filePath = SiliCompressor.with(mContext).compressVideo(videoContentUri, paths[2], 1280,720,1500000);*/
                        filePath = SiliCompressor.With(MContext).CompressVideo(videoContentUri?.ToString(), paths[2]);
                        break;
                    }
                }
            }
            catch (URISyntaxException e)
            {
                Methods.DisplayReportResultTrack(e);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
            return filePath;
        }

        protected override void OnPostExecute(string compressedFilePath)
        {
            try
            {
                base.OnPostExecute(compressedFilePath);

                File imageFile = new File(compressedFilePath);
                //float length = imageFile.Length() / 1024f; // Size in KB
                //string value;
                //if (length >= 1024)
                //    value = length / 1024f + " MB";
                //else
                //    value = length + " KB";

                //Console.WriteLine("Name: " + imageFile.Name + " Size: " + value);

                //Console.WriteLine("Silicompressor Path: " + compressedFilePath);

                if (Service.ActionPostService == PostService.ActionPost)
                {
                    var attach = Service.DataPost.AttachmentList.FirstOrDefault();
                    switch (attach?.TypeAttachment)
                    {
                        case "postVideo":
                            attach.FileUrl = imageFile.Path;

                            PollyController.RunRetryPolicyFunction(new List<Func<Task>> { Service.AddPost });
                            break;
                    }
                }
                else if (Service.ActionPostService == PostService.ActionStory)
                {
                    var attach = Service.DataPost;
                    if (attach != null)
                    {
                        attach.StoryFilePath = imageFile.Path;

                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { Service.AddStory });
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
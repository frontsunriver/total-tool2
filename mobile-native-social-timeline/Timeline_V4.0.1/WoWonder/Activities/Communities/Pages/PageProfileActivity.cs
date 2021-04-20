using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AFollestad.MaterialDialogs;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS; 
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using AndroidX.SwipeRefreshLayout.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Google.Android.Material.FloatingActionButton;
using TheArtOfDev.Edmodo.Cropper;
using Java.Lang;
using Newtonsoft.Json;
using WoWonder.Library.Anjo.Share;
using WoWonder.Library.Anjo.Share.Abstractions;
using WoWonder.Activities.AddPost;
using WoWonder.Activities.Base;
using WoWonder.Activities.Communities.Pages.Settings;
using WoWonder.Activities.General;
using WoWonder.Activities.Live.Utils;
using WoWonder.Activities.NativePost.Extra;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Message;
using WoWonderClient.Classes.Page;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Classes.Product;
using WoWonderClient.Requests;
using Exception = System.Exception;
using File = Java.IO.File;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.Communities.Pages
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class PageProfileActivity : BaseActivity, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback, MaterialDialog.IInputCallback
    {
        #region Variables Basic

        private SwipeRefreshLayout SwipeRefreshLayout;
        private ImageView ProfileImage, CoverImage, IconBack;
        private TextView TxtPageName, TxtPageUsername, IconEdit;
        private Button BtnLike, ActionButton;
        private ImageButton BtnMore, MessageButton;
        private FloatingActionButton FloatingActionButtonView;
        private LinearLayout EditAvatarImagePage;
        private TextView TxtEditPageInfo;
        public WRecyclerView MainRecyclerView;
        public NativePostAdapter PostFeedAdapter;
        private string PageId = "", UserId = "", ImageType = "";
        public static PageClass PageData;
        private FeedCombiner Combiner;
        private static PageProfileActivity Instance;
        private AdView MAdView;

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
                SetContentView(Resource.Layout.Page_Profile_Layout);

                Instance = this; 
                PageId = Intent?.GetStringExtra("PageId") ?? string.Empty;

                //Get Value And Set Toolbar
                InitComponent(); 
                SetRecyclerViewAdapters();

                GetDataPage();
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
                MainRecyclerView?.StopVideo();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        protected override void OnStop()
        {
            try
            {
                base.OnStop();
                MainRecyclerView?.StopVideo();
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
                MainRecyclerView.ReleasePlayer();
                MAdView?.Destroy();
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
                SwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);
                SwipeRefreshLayout.SetColorSchemeResources(Android.Resource.Color.HoloBlueLight, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
                SwipeRefreshLayout.Refreshing = false;
                SwipeRefreshLayout.Enabled = true;
                SwipeRefreshLayout.SetProgressBackgroundColorSchemeColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#424242") : Color.ParseColor("#f7f7f7"));

                ProfileImage = (ImageView)FindViewById(Resource.Id.image_profile);
                CoverImage = (ImageView)FindViewById(Resource.Id.iv1);
                IconBack = (ImageView)FindViewById(Resource.Id.image_back);
                EditAvatarImagePage = (LinearLayout)FindViewById(Resource.Id.LinearEdit);
                TxtEditPageInfo = (TextView)FindViewById(Resource.Id.tv_EditPageinfo);
                TxtPageName = (TextView)FindViewById(Resource.Id.Page_name);
                TxtPageUsername = (TextView)FindViewById(Resource.Id.Page_Username);
                BtnLike = (Button)FindViewById(Resource.Id.likeButton);
                BtnLike.Tag = "UserPage";

                BtnMore = (ImageButton)FindViewById(Resource.Id.morebutton);
                IconEdit = (TextView)FindViewById(Resource.Id.IconEdit);
                
                FloatingActionButtonView = FindViewById<FloatingActionButton>(Resource.Id.floatingActionButtonView);
                MessageButton = (ImageButton)FindViewById(Resource.Id.messagebutton);
                ActionButton = (Button)FindViewById(Resource.Id.actionButton);
                FloatingActionButtonView.Visibility = ViewStates.Gone;

                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconEdit, IonIconsFonts.Create); 
                 
                switch (AppSettings.FlowDirectionRightToLeft)
                {
                    case true:
                        IconBack.SetImageResource(Resource.Drawable.ic_action_ic_back_rtl);
                        break;
                }
               
                MAdView = FindViewById<AdView>(Resource.Id.adView);
                AdsGoogle.InitAdView(MAdView, MainRecyclerView);
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
                MainRecyclerView = FindViewById<WRecyclerView>(Resource.Id.newsfeedRecyler);
                PostFeedAdapter = new NativePostAdapter(this, PageId, MainRecyclerView, NativeFeedType.Page);
                MainRecyclerView?.SetXAdapter(PostFeedAdapter, SwipeRefreshLayout);
                Combiner = new FeedCombiner(null, PostFeedAdapter.ListDiffer, this); 
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
                        SwipeRefreshLayout.Refresh += SwipeRefreshLayoutOnRefresh;
                        BtnLike.Click += BtnLikeOnClick;
                        BtnMore.Click += BtnMoreOnClick;
                        TxtEditPageInfo.Click += TxtEditPageInfoOnClick;
                        IconBack.Click += IconBackOnClick;
                        EditAvatarImagePage.Click += EditAvatarImagePageOnClick;
                        FloatingActionButtonView.Click += AddPostOnClick;
                        MessageButton.Click += MessageButtonOnClick;
                        ProfileImage.Click += UserProfileImageOnClick;
                        CoverImage.Click += CoverImageOnClick;
                        break;
                    default:
                        SwipeRefreshLayout.Refresh -= SwipeRefreshLayoutOnRefresh;
                        BtnLike.Click -= BtnLikeOnClick;
                        BtnMore.Click -= BtnMoreOnClick;
                        TxtEditPageInfo.Click -= TxtEditPageInfoOnClick;
                        IconBack.Click -= IconBackOnClick;
                        EditAvatarImagePage.Click -= EditAvatarImagePageOnClick;
                        FloatingActionButtonView.Click -= AddPostOnClick;
                        MessageButton.Click -= MessageButtonOnClick;
                        ProfileImage.Click -= UserProfileImageOnClick;
                        CoverImage.Click -= CoverImageOnClick;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static PageProfileActivity GetInstance()
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
                SwipeRefreshLayout = null!;
                ProfileImage = null!;
                CoverImage = null!;
                IconBack = null!;
                TxtPageName = null!;
                TxtPageUsername = null!; 
                IconEdit = null!; 
                BtnLike = null!;
                ActionButton = null!;
                BtnMore = null!;
                MessageButton = null!;
                FloatingActionButtonView = null!;
                TxtEditPageInfo = null!;
                MainRecyclerView = null!;
                PostFeedAdapter = null!;
                PageId = null!;
                UserId = null!;
                ImageType = null!;
                PageData = null!; 
                EditAvatarImagePage = null!; 
                MAdView = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events
          
        //Refresh
        private void SwipeRefreshLayoutOnRefresh(object sender, EventArgs e)
        {
            try
            {
                PostFeedAdapter.ListDiffer.Clear();
                PostFeedAdapter.NotifyDataSetChanged();

                GetDataPage();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
        
        //Open image Cover
        private void CoverImageOnClick(object sender, EventArgs e)
        {
            try
            {
                var media = WoWonderTools.GetFile("", Methods.Path.FolderDiskImage, PageData.Cover.Split('/').Last(), PageData.Cover);
                if (media.Contains("http"))
                {
                    var intent = new Intent(Intent.ActionView, Uri.Parse(media));
                    StartActivity(intent);
                }
                else
                {
                    var file2 = new File(media);
                    var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);

                    var intent = new Intent(Intent.ActionPick);
                    intent.SetAction(Intent.ActionView);
                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    intent.SetDataAndType(photoUri, "image/*");
                    StartActivity(intent);
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Open image Avatar
        private void UserProfileImageOnClick(object sender, EventArgs e)
        {
            try
            {
                var media = WoWonderTools.GetFile("", Methods.Path.FolderDiskImage, PageData.Avatar.Split('/').Last(), PageData.Avatar);
                if (media.Contains("http"))
                {
                    var intent = new Intent(Intent.ActionView, Uri.Parse(media));
                    StartActivity(intent);
                }
                else
                {
                    var file2 = new File(media);
                    var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);

                    var intent = new Intent(Intent.ActionPick);
                    intent.SetAction(Intent.ActionView);
                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    intent.SetDataAndType(photoUri, "image/*");
                    StartActivity(intent);
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        // Rating Page 
        public void RatingLinerOnClick()
        {
            try
            {
                if (PageData.IsRated != null && PageData.IsRated.Value)
                {
                    // You have already rated this page!
                    Toast.MakeText(this, GetString(Resource.String.Lbl_YouHaveAlReadyRatedThisPage), ToastLength.Short)?.Show(); 
                }
                else
                { 
                    var dialog = new DialogRatingBarFragment(this,PageId,PageData);
                    dialog.Show(SupportFragmentManager, dialog.Tag);
                    dialog.OnUpComplete += DialogOnOnUpComplete;
                } 
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void DialogOnOnUpComplete(object sender, DialogRatingBarFragment.RatingBarUpEventArgs e)
        {
            try
            {
                var th = new Thread(ActLikeARequest);
                th.Start();
            }
            catch (Exception ex)
            {
                Methods.DisplayReportResultTrack(ex);
            }
        }

        private void ActLikeARequest()
        {
            var x = Resource.Animation.slide_right;
            Console.WriteLine(x);
        }

        //Event Add New post  
        private void AddPostOnClick(object sender, EventArgs e)
        {
            try
            {
                var intent = new Intent(this, typeof(AddPostActivity));
                intent.PutExtra("Type", "SocialPage");
                intent.PutExtra("PostId", PageId);
                intent.PutExtra("itemObject", JsonConvert.SerializeObject(PageData));
                StartActivityForResult(intent, 2500);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Event Update Image Cover Page
        private void EditAvatarImagePageOnClick(object sender, EventArgs e)
        {
            try
            {
                OpenDialogGallery("Avatar");
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void IconBackOnClick(object sender, EventArgs e)
        {
            Finish();
        }

        private void TxtEditPageInfoOnClick(object sender, EventArgs e)
        {
            try
            {
                OpenDialogGallery("Cover");
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Event Show More : Copy Link , Share , Edit (If user isOwner_Pages)
        private void BtnMoreOnClick(object sender, EventArgs e)
        {
            try
            {
                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                arrayAdapter.Add(GetString(Resource.String.Lbl_CopeLink));
                arrayAdapter.Add(GetString(Resource.String.Lbl_Share));
                arrayAdapter.Add(GetString(Resource.String.Lbl_Reviews));
                if (PageData.IsPageOnwer != null && PageData.IsPageOnwer.Value)
                {
                    switch (PageData?.Boosted)
                    {
                        case "0":
                            arrayAdapter.Add(GetString(Resource.String.Lbl_BoostPage));
                            break;
                        case "1":
                            arrayAdapter.Add(GetString(Resource.String.Lbl_UnBoostPage));
                            break;
                    }
                    arrayAdapter.Add(GetString(Resource.String.Lbl_Settings)); 
                }

                dialogList.Title(GetString(Resource.String.Lbl_More)).TitleColorRes(Resource.Color.primary);
                dialogList.Items(arrayAdapter);
                dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                dialogList.AlwaysCallSingleChoiceCallback();
                dialogList.ItemsCallback(this).Build().Show();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Event Like => like , dislike 
        private async void BtnLikeOnClick(object sender, EventArgs e)
        {
            try
            {
                switch (BtnLike?.Tag?.ToString())
                {
                    case "MyPage":
                        SettingsPage_OnClick();
                        break;
                    default:
                    {
                        if (!Methods.CheckConnectivity())
                        {
                            Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                        }
                        else
                        {
                            var isLiked = BtnLike?.Text == GetText(Resource.String.Btn_Liked) ? "false" : "true";
                            BtnLike.BackgroundTintList = isLiked == "yes" || isLiked == "true" ? ColorStateList.ValueOf(Color.ParseColor("#efefef")) : ColorStateList.ValueOf(Color.ParseColor(AppSettings.MainColor));
                            BtnLike.Text = GetText(isLiked == "yes" || isLiked == "true" ? Resource.String.Btn_Liked : Resource.String.Btn_Like);
                            BtnLike.SetTextColor(isLiked == "yes" || isLiked == "true" ? Color.Black : Color.White);

                            var (apiStatus, respond) = await RequestsAsync.Page.LikePageAsync(PageId);
                            switch (apiStatus)
                            {
                                case 200:
                                {
                                    switch (respond)
                                    {
                                        case LikePageObject result:
                                            isLiked = result.LikeStatus == "unliked" ? "false" : "true";
                                            BtnLike.BackgroundTintList = isLiked == "yes" || isLiked == "true" ? ColorStateList.ValueOf(Color.ParseColor("#efefef")) : ColorStateList.ValueOf(Color.ParseColor(AppSettings.MainColor));
                                            BtnLike.Text = GetText(isLiked == "yes" || isLiked == "true" ? Resource.String.Btn_Liked : Resource.String.Btn_Like);
                                            BtnLike.SetTextColor(isLiked == "yes" || isLiked == "true" ? Color.Black : Color.White);
                                            break;
                                    }

                                    break;
                                }
                                default:
                                    Methods.DisplayReportResult(this, respond);
                                    break;
                            }
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
         
        //Event Send Message to page  
        private void MessageButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                switch (AppSettings.MessengerIntegration)
                {
                    case true when AppSettings.ShowDialogAskOpenMessenger:
                    {
                        var dialog = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                        dialog.Title(Resource.String.Lbl_Warning).TitleColorRes(Resource.Color.primary);
                        dialog.Content(GetText(Resource.String.Lbl_ContentAskOPenAppMessenger));
                        dialog.PositiveText(GetText(Resource.String.Lbl_Yes)).OnPositive((materialDialog, action) =>
                        {
                            try
                            {
                                Methods.App.OpenAppByPackageName(this, AppSettings.MessengerPackageName, "OpenChatPage", new ChatObject { UserId = UserId, PageId = PageId, PageName = PageData.PageName, Avatar = PageData.Avatar });
                            }
                            catch (Exception exception)
                            {
                                Methods.DisplayReportResultTrack(exception);
                            }
                        });
                        dialog.NegativeText(GetText(Resource.String.Lbl_No)).OnNegative(this);
                        dialog.AlwaysCallSingleChoiceCallback();
                        dialog.Build().Show();
                        break;
                    }
                    case true:
                        Methods.App.OpenAppByPackageName(this, AppSettings.MessengerPackageName, "OpenChatPage", new ChatObject { UserId = UserId, PageId = PageId, PageName = PageData.PageName, Avatar = PageData.Avatar });
                        break;
                    default:
                    {
                        if (!Methods.CheckConnectivity())
                        {
                            Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                            return;
                        }

                        var dialog = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                        dialog.Title(GetString(Resource.String.Lbl_SendMessageTo) + " " + Methods.FunString.DecodeString(PageData.Name)).TitleColorRes(Resource.Color.primary);
                        dialog.Input(Resource.String.Lbl_WriteMessage, 0, false, this);
                        dialog.InputType(InputTypes.TextFlagImeMultiLine);
                        dialog.PositiveText(GetText(Resource.String.Btn_Send)).OnPositive(this);
                        dialog.NegativeText(GetText(Resource.String.Lbl_Cancel)).OnNegative(this);
                        dialog.Build().Show();
                        dialog.AlwaysCallSingleChoiceCallback();
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Event Invite friends to like this Page
        private void MembersLinerOnClick(object sender, EventArgs e)
        {
            try
            {
                var intent = new Intent(this, typeof(InviteMembersPageActivity));
                intent.PutExtra("PageId", PageId);
                StartActivity(intent);
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
                    //If its from Camera or Gallery
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
                                        string pathImg;
                                        switch (ImageType)
                                        {
                                            case "Cover":
                                            {
                                                pathImg = resultUri.Path;

                                                //Set image
                                                File file2 = new File(pathImg);
                                                var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);
                                                Glide.With(this).Load(photoUri).Apply(new RequestOptions()).Into(CoverImage);

                                                UpdateImagePage_Api(ImageType, pathImg);
                                                break;
                                            }
                                            case "Avatar":
                                            {
                                                pathImg = resultUri.Path;

                                                //Set image
                                                File file2 = new File(pathImg);
                                                var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);
                                                Glide.With(this).Load(photoUri).Apply(new RequestOptions()).Into(ProfileImage);

                                                var dataPage = PagesActivity.GetInstance()?.MAdapter?.PagesAdapter?.PageList?.FirstOrDefault(a => a.PageId == PageId);
                                                if (dataPage != null)
                                                {
                                                    dataPage.Avatar = pathImg;
                                                    PagesActivity.GetInstance()?.MAdapter?.PagesAdapter?.NotifyDataSetChanged();

                                                    var dataPage2 = ListUtils.MyPageList.FirstOrDefault(a => a.PageId == PageId);
                                                    if (dataPage2 != null)
                                                    {
                                                        dataPage2.Avatar = pathImg;
                                                    }
                                                }

                                                UpdateImagePage_Api(ImageType, pathImg);
                                                break;
                                            }
                                        }

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
                    //Edit post
                    case 2500 when resultCode == Result.Ok:
                    {
                        if (!string.IsNullOrEmpty(data.GetStringExtra("itemObject")))
                        {
                            var postData = JsonConvert.DeserializeObject<PostDataObject>(data.GetStringExtra("itemObject")!);
                            if (postData != null)
                            {
                                var countList = PostFeedAdapter.ItemCount;

                                var combine = new FeedCombiner(postData, PostFeedAdapter.ListDiffer, this);
                                combine.CombineDefaultPostSections("Top");

                                var countIndex = 1;
                                var model1 = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.Story);
                                var model2 = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                                var model3 = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AlertBox);
                                var model4 = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SearchForPosts);

                                if (model4 != null)
                                    countIndex += PostFeedAdapter.ListDiffer.IndexOf(model4) + 1;
                                else if (model3 != null)
                                    countIndex += PostFeedAdapter.ListDiffer.IndexOf(model3) + 1;
                                else if (model2 != null)
                                    countIndex += PostFeedAdapter.ListDiffer.IndexOf(model2) + 1;
                                else if (model1 != null)
                                    countIndex += PostFeedAdapter.ListDiffer.IndexOf(model1) + 1;
                                else
                                    countIndex = 0;

                                PostFeedAdapter.NotifyItemRangeInserted(countIndex, PostFeedAdapter.ListDiffer.Count - countList);
                            }
                        }
                        else
                        {
                            PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => MainRecyclerView.ApiPostAsync.FetchNewsFeedApiPosts() });
                        }

                        break;
                    }
                    //Edit post
                    case 3950 when resultCode == Result.Ok:
                    {
                        var postId = data.GetStringExtra("PostId") ?? "";
                        var postText = data.GetStringExtra("PostText") ?? "";
                        var diff = PostFeedAdapter.ListDiffer;
                        var dataGlobal = diff.Where(a => a.PostData?.Id == postId).ToList();
                        switch (dataGlobal.Count)
                        {
                            case > 0:
                            {
                                foreach (var postData in dataGlobal)
                                {
                                    postData.PostData.Orginaltext = postText;
                                    var index = diff.IndexOf(postData);
                                    switch (index)
                                    {
                                        case > -1:
                                            PostFeedAdapter.NotifyItemChanged(index);
                                            break;
                                    }
                                }

                                var checkTextSection = dataGlobal.FirstOrDefault(w => w.TypeView == PostModelType.TextSectionPostPart);
                                switch (checkTextSection)
                                {
                                    case null:
                                    {
                                        var collection = dataGlobal.FirstOrDefault()?.PostData;
                                        var item = new AdapterModelsClass
                                        {
                                            TypeView = PostModelType.TextSectionPostPart,
                                            Id = Convert.ToInt32((int)PostModelType.TextSectionPostPart + collection?.Id),
                                            PostData = collection,
                                            IsDefaultFeedPost = true
                                        };

                                        var headerPostIndex = diff.IndexOf(dataGlobal.FirstOrDefault(w => w.TypeView == PostModelType.HeaderPost));
                                        switch (headerPostIndex)
                                        {
                                            case > -1:
                                                diff.Insert(headerPostIndex + 1, item);
                                                PostFeedAdapter.NotifyItemInserted(headerPostIndex + 1);
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
                    //Edit post product 
                    case 3500 when resultCode == Result.Ok:
                    {
                        if (string.IsNullOrEmpty(data.GetStringExtra("itemData"))) return;
                        var item = JsonConvert.DeserializeObject<ProductDataObject>(data.GetStringExtra("itemData") ?? "");
                        if (item != null)
                        {
                            var diff = PostFeedAdapter.ListDiffer;
                            var dataGlobal = diff.Where(a => a.PostData?.Id == item.PostId).ToList();
                            switch (dataGlobal.Count)
                            {
                                case > 0:
                                {
                                    foreach (var postData in dataGlobal)
                                    {
                                        var index = diff.IndexOf(postData);
                                        switch (index)
                                        {
                                            case > -1:
                                            {
                                                var productUnion = postData.PostData.Product?.ProductClass;
                                                if (productUnion != null) productUnion.Id = item.Id;
                                                productUnion = item;
                                                Console.WriteLine(productUnion);

                                                PostFeedAdapter.NotifyItemChanged(PostFeedAdapter.ListDiffer.IndexOf(postData));
                                                break;
                                            }
                                        }
                                    }

                                    break;
                                }
                            }
                        }

                        break;
                    }
                    case 2005 when resultCode == Result.Ok:
                    {
                        var result = data.GetStringExtra("pageItem"); 
                        var item = JsonConvert.DeserializeObject<PageClass>(result ?? string.Empty);
                        if (item != null)
                            LoadPassedData(item);
                        break;
                    }
                    case 2019 when resultCode == Result.Ok:
                    {
                        var dataPage = PagesActivity.GetInstance()?.MAdapter?.PagesAdapter?.PageList?.FirstOrDefault(a => a.PageId == PageId);
                        if (dataPage != null)
                        {
                            PagesActivity.GetInstance()?.MAdapter?.PagesAdapter?.PageList.Remove(dataPage);
                            PagesActivity.GetInstance()?.MAdapter?.PagesAdapter?.NotifyDataSetChanged();

                            ListUtils.MyPageList.Remove(dataPage);

                            Finish();
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
                        OpenDialogGallery(ImageType);
                        break;
                    case 108:
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Permission_is_denied), ToastLength.Long)?.Show();
                        break;
                    case 235 when grantResults.Length > 0 && grantResults[0] == Permission.Granted:
                        new LiveUtil(this).OpenDialogLive();
                        break;
                    case 235:
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
   
        #region Update Image Avatar && Cover

        private void OpenDialogGallery(string typeImage)
        {
            try
            {
                ImageType = typeImage;
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

        // Function Update Image Page : Avatar && Cover
        private async void UpdateImagePage_Api(string type, string path)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
                else
                {
                    switch (type)
                    {
                        case "Avatar":
                        {
                            var (apiStatus, respond) = await RequestsAsync.Page.UpdatePageAvatarAsync(PageId, path).ConfigureAwait(false);
                            switch (apiStatus)
                            {
                                case 200:
                                {
                                    switch (respond)
                                    {
                                        case MessageObject result:
                                            Toast.MakeText(this, result.Message, ToastLength.Short)?.Show();
                                 
                                            //GlideImageLoader.LoadImage(this, file.Path, ProfileImage, ImageStyle.RoundedCrop, ImagePlaceholders.Color);
                                            break;
                                    }

                                    break;
                                }
                                default:
                                    Methods.DisplayReportResult(this, respond);
                                    break;
                            }

                            break;
                        }
                        case "Cover":
                        {
                            var (apiStatus, respond) = await RequestsAsync.Page.UpdatePageCoverAsync(PageId, path).ConfigureAwait(false);
                            switch (apiStatus)
                            {
                                case 200:
                                {
                                    if (respond is not MessageObject result)
                                        return;

                                    Toast.MakeText(this, result.Message, ToastLength.Short)?.Show();
                             
                                    //GlideImageLoader.LoadImage(this, file.Path, CoverImage, ImageStyle.CenterCrop, ImagePlaceholders.Color);
                                    break;
                                }
                                default:
                                    Methods.DisplayReportResult(this, respond);
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

        #endregion

        #region Get Data Page
         
        private void GetDataPage()
        {
            try
            { 
                PageData = JsonConvert.DeserializeObject<PageClass>(Intent?.GetStringExtra("PageObject"));
                if (PageData != null)
                {
                    LoadPassedData(PageData); 
                }

                PostFeedAdapter.SetLoading();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }

            StartApiService();
        }

        private void LoadPassedData(PageClass pageData)
        {
            try
            {
                PageData = pageData; 
                UserId = pageData.IsPageOnwer != null && pageData.IsPageOnwer.Value ? UserDetails.UserId : pageData.UserId;
                 
                GlideImageLoader.LoadImage(this, pageData.Avatar, ProfileImage, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);
                Glide.With(this).Load(pageData.Cover.Replace(" ", "")).Apply(new RequestOptions().Placeholder(Resource.Drawable.Cover_image).Error(Resource.Drawable.Cover_image)).Into(CoverImage);

                if (pageData.IsPageOnwer != null && pageData.IsPageOnwer.Value)
                {
                    BtnLike.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(AppSettings.MainColor));
                    BtnLike.Text = GetText(Resource.String.Lbl_Edit);
                    BtnLike.SetTextColor(Color.White);
                    BtnLike.Tag = "MyPage";
                    BtnMore.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(AppSettings.MainColor));
                    BtnMore.ImageTintList = ColorStateList.ValueOf(Color.White); 
                }
                else
                {
                    BtnLike.BackgroundTintList = WoWonderTools.IsLikedPage(pageData) ? ColorStateList.ValueOf(Color.ParseColor("#efefef")) : ColorStateList.ValueOf(Color.ParseColor(AppSettings.MainColor));
                    BtnLike.Text = GetText(WoWonderTools.IsLikedPage(pageData) ? Resource.String.Btn_Liked : Resource.String.Btn_Like);
                    BtnLike.SetTextColor(WoWonderTools.IsLikedPage(pageData) ? Color.Black : Color.White);
                    BtnMore.BackgroundTintList = WoWonderTools.IsLikedPage(pageData) ? ColorStateList.ValueOf(Color.ParseColor("#efefef")) : ColorStateList.ValueOf(Color.ParseColor(AppSettings.MainColor));
                    BtnMore.ImageTintList = WoWonderTools.IsLikedPage(pageData) ? ColorStateList.ValueOf(Color.Black) : ColorStateList.ValueOf(Color.White);
                    BtnLike.Tag = "UserPage"; 
                }

                if (pageData.IsPageOnwer != null && pageData.IsPageOnwer.Value)
                {
                    EditAvatarImagePage.Visibility = ViewStates.Visible;
                    TxtEditPageInfo.Visibility = ViewStates.Visible;
                    FloatingActionButtonView.Visibility = ViewStates.Visible;
                    MessageButton.Visibility = ViewStates.Gone;
                }
                else
                {
                    EditAvatarImagePage.Visibility = ViewStates.Gone;
                    TxtEditPageInfo.Visibility = ViewStates.Gone;
                    FloatingActionButtonView.Visibility = ViewStates.Gone;
                    MessageButton.Visibility = ViewStates.Visible;
                }

                var modelsClass = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.InfoPageBox);
                switch (modelsClass)
                {
                    case null:
                        Combiner.InfoPageBox(new PageInfoModelClass { PageClass = pageData, PageId = pageData.PageId }, 0);
                        break;
                    default:
                        modelsClass.PageInfoModelClass = new PageInfoModelClass
                        {
                            PageClass = pageData,
                            PageId = pageData.PageId
                        };
                        PostFeedAdapter.NotifyItemChanged(PostFeedAdapter.ListDiffer.IndexOf(modelsClass));
                        break;
                }
                 
                var checkAboutBox = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AboutBox);
                switch (checkAboutBox)
                {
                    case null:
                        Combiner.AboutBoxPostView(Methods.FunString.DecodeString(pageData.About), 0);
                        break;
                    default:
                        checkAboutBox.AboutModel.Description = Methods.FunString.DecodeString(pageData.About);
                        PostFeedAdapter.NotifyItemChanged(PostFeedAdapter.ListDiffer.IndexOf(checkAboutBox));
                        break;
                }

                if (pageData.IsPageOnwer != null && pageData.IsPageOnwer.Value || pageData.UsersPost == "1")
                {
                    var checkSection = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                    switch (checkSection)
                    {
                        case null:
                        {
                            Combiner.AddPostBoxPostView("Page", -1, new PostDataObject { PageId = pageData.PageId, Publisher = new PublisherPost { PageName = pageData.PageName, Avatar = pageData.Avatar } });

                            switch (AppSettings.ShowSearchForPosts)
                            {
                                case true:
                                    Combiner.SearchForPostsView("Page", new PostDataObject { PageId = pageData.PageId, Publisher = new PublisherPost { PageName = pageData.PageName, Avatar = pageData.Avatar } });
                                    break;
                            }

                            PostFeedAdapter.NotifyItemInserted(PostFeedAdapter.ListDiffer.Count -1 );
                            break;
                        }
                    }

                    FloatingActionButtonView.Visibility = ViewStates.Visible;
                }
                 
                TxtPageUsername.Text = "@" + pageData.Username;
                TxtPageName.Text = Methods.FunString.DecodeString(pageData.Name);
                 
                SetCallActionsButtons(pageData);
                SetAdminInfo(pageData);
                
                WoWonderTools.GetFile("", Methods.Path.FolderDiskImage, pageData.Cover.Split('/').Last(), pageData.Cover);
                WoWonderTools.GetFile("", Methods.Path.FolderDiskImage, pageData.Avatar.Split('/').Last(), pageData.Avatar); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void SetCallActionsButtons(PageClass pageData)
        {
            try
            {

                if (pageData.CallActionType != "0" && !string.IsNullOrEmpty(pageData.CallActionTypeUrl))
                {
                    var name = "Lbl_call_action_" + pageData.CallActionType;
                    var resourceId = Resources?.GetIdentifier(name, "string", ApplicationInfo?.PackageName) ?? 0 ;
                    ActionButton.Visibility = ViewStates.Visible;
                    ActionButton.Text = Resources?.GetString(resourceId);
                    switch (ActionButton.HasOnClickListeners)
                    {
                        case false:
                            ActionButton.Click += (sender, args) =>
                            {
                                try
                                {
                                    switch (string.IsNullOrEmpty(pageData.CallActionTypeUrl))
                                    {
                                        case false:
                                            new IntentController(this).OpenBrowserFromApp(pageData.CallActionTypeUrl);
                                            break;
                                        default:
                                            Toast.MakeText(this,GetString(Resource.String.Lbl_call_action_sorry),ToastLength.Short)?.Show();
                                            break;
                                    }
                                }
                                catch (Exception e)
                                {
                                    Methods.DisplayReportResultTrack(e);
                                }
                            };
                            break;
                    } 
                }
                else
                {
                    ReplaceView(ActionButton, BtnLike);
                }
                
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        private void SetAdminInfo(PageClass pageData)
        {
            try
            {  
                if (pageData.AdminInfo?.AdminInfoClass != null && pageData.AdminInfo?.AdminInfoClass?.UserId == UserDetails.UserId)
                {
                    switch (pageData.AdminInfo?.AdminInfoClass.Avatar)
                    {
                        case "0":
                            TxtEditPageInfo.Visibility = ViewStates.Gone;
                            EditAvatarImagePage.Visibility = ViewStates.Gone;
                            break;
                    }
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void ReplaceView(View currentView, View newView)
        {
            var parent = GetParent(currentView);
            switch (parent)
            {
                case null:
                    return;
            }
            var index = parent.IndexOfChild(currentView);
            RemoveView(currentView);
            RemoveView(newView);
            parent.AddView(newView, index);
        }

        private ViewGroup GetParent(View view)
        {
            return (ViewGroup)view.Parent;
        }

        private void RemoveView(View view)
        {
            var parent = GetParent(view);
            parent?.RemoveView(view);
        }

        private void StartApiService()
        {
            if (!Methods.CheckConnectivity())
                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
            else
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { GetPageDataApi, () => MainRecyclerView.ApiPostAsync.FetchNewsFeedApiPosts() });
        }

        private async Task GetPageDataApi()
        {
            var (apiStatus, respond) = await RequestsAsync.Page.GetPageDataAsync(PageId);

            if (apiStatus != 200 || respond is not GetPageDataObject result || result.PageData == null)
                Methods.DisplayReportResult(this, respond);
            else
            {
                PageData = result.PageData;
                LoadPassedData(PageData);
            } 
        }

        #endregion

        #region MaterialDialog

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                var text = itemString.ToString();
                if (text == GetString(Resource.String.Lbl_CopeLink))
                {
                    CopyLinkEvent();
                }
                else if (text == GetString(Resource.String.Lbl_Share))
                {
                    ShareEvent();
                }
                else if (text == GetString(Resource.String.Lbl_Settings))
                {
                    SettingsPage_OnClick();
                }
                else if (text == GetString(Resource.String.Lbl_BoostPage) || text == GetString(Resource.String.Lbl_UnBoostPage))
                {
                    BoostPageEvent();
                }
                else if (text == GetString(Resource.String.Lbl_Reviews))
                {
                    ReviewsEvent();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnClick(MaterialDialog p0, DialogAction p1)
        {
            try
            {
               if (p1 == DialogAction.Positive) 
               {

               }
               else if (p1 == DialogAction.Negative)
               {
                   p0.Dismiss();
               }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public void OnInput(MaterialDialog p0, ICharSequence p1)
        {
            //Send Message to page 
            try
            {
                if (p1.Length() > 0)
                {
                    var strName = p1.ToString();

                    if (!Methods.CheckConnectivity())
                    {
                        Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection),ToastLength.Short)?.Show();
                        return;
                    }

                    if (!string.IsNullOrEmpty(strName) || !string.IsNullOrWhiteSpace(strName))
                    {
                        var unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        var time = unixTimestamp.ToString();

                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.PageChat.SendMessageToPageChatAsync(PageId, UserId, time, strName) });
                        Toast.MakeText(this, GetString(Resource.String.Lbl_MessageSentSuccessfully), ToastLength.Short)?.Show();
                    }
                    else
                    {
                        Toast.MakeText(this, GetString(Resource.String.Lbl_something_went_wrong), ToastLength.Short)?.Show();
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        //Event Menu >> Copy Link
        private void CopyLinkEvent()
        {
            try
            {
                Methods.CopyToClipboard(this, PageData.Url);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Event Menu >> Share
        private async void ShareEvent()
        {
            try
            {
                switch (CrossShare.IsSupported)
                {
                    //Share Plugin same as video
                    case false:
                        return;
                    default:
                        await CrossShare.Current.Share(new ShareMessage
                        {
                            Title = PageData.PageTitle,
                            Text = PageData.About,
                            Url = PageData.Url
                        });
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Event Menu >> Settings
        private void SettingsPage_OnClick()
        {
            try
            {
                var intent = new Intent(this, typeof(SettingsPageActivity));
                intent.PutExtra("PageData", JsonConvert.SerializeObject(PageData));
                intent.PutExtra("PagesId", PageId);
                StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        private async void BoostPageEvent()
        {
            try
            {  
                var dataUser = ListUtils.MyProfileList?.FirstOrDefault();
                if (dataUser?.IsPro != "1" && ListUtils.SettingsSiteList?.Pro == "1" && AppSettings.ShowGoPro)
                {
                    var intent = new Intent(this, typeof(GoProActivity));
                    StartActivity(intent);
                    return;
                }

                if (Methods.CheckConnectivity())
                {
                    PageData.Boosted = "1";
                    //Sent Api 
                    var (apiStatus, respond) = await RequestsAsync.Page.BoostPageAsync(PageId);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case MessageObject actionsObject when actionsObject.Message == "boosted":
                                    PageData.Boosted = "1";
                                    Toast.MakeText(this, GetText(Resource.String.Lbl_PageSuccessfullyBoosted), ToastLength.Short)?.Show();
                                    break;
                                case MessageObject actionsObject:
                                    PageData.Boosted = "0";
                                    break;
                            }

                            break;
                        }
                        default:
                            Methods.DisplayReportResult(this, respond);
                            break;
                    } 
                }
                else
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        private void ReviewsEvent()
        {
            try
            {
                var intent = new Intent(this, typeof(ReviewsPageActivity));
                intent.PutExtra("PageId", PageId);
                StartActivity(intent); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion
         
    }
}
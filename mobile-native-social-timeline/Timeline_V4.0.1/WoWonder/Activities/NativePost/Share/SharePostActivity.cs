using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.Content.Res;
using Newtonsoft.Json;
using WoWonder.Activities.Base;
using WoWonder.Activities.MyProfile;
using WoWonder.Activities.NativePost.Extra;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Requests;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.NativePost.Share
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class SharePostActivity : BaseActivity
    { 
        #region Variables Basic

        private Toolbar TopToolBar;
        private ImageView PostSectionImage;
        private TextView TxtSharePost, TxtUserName;
        private EditText TxtContentPost; 
        private WRecyclerView MainRecyclerView;
        private NativePostAdapter PostFeedAdapter;
        private GroupClass GroupData;
        private PageClass PageData;
        private PostDataObject PostData;
        private string TypePost = "";
        
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
                SetContentView(Resource.Layout.SharePostLayout);
                 
                var postdate = Intent?.GetStringExtra("ShareToType") ?? "Data not available";
                if (postdate != "Data not available" && !string.IsNullOrEmpty(postdate)) TypePost = postdate; //Group , Page , MyTimeline
                 
                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();

                GetDataPost();
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
                TxtSharePost = FindViewById<TextView>(Resource.Id.toolbar_title);
                TxtContentPost = FindViewById<EditText>(Resource.Id.editTxtEmail); 
                PostSectionImage = FindViewById<ImageView>(Resource.Id.postsectionimage); 
                TxtUserName = FindViewById<TextView>(Resource.Id.card_name); 

                Methods.SetColorEditText(TxtContentPost, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                TxtContentPost.ClearFocus(); 
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
                TopToolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
                if (TopToolBar != null)
                {
                    TopToolBar.Title = GetText(Resource.String.Lbl_SharePost);
                    TopToolBar.SetTitleTextColor(Color.ParseColor(AppSettings.MainColor));
                    SetSupportActionBar(TopToolBar);
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
                MainRecyclerView = FindViewById<WRecyclerView>(Resource.Id.Recyler);
                PostFeedAdapter = new NativePostAdapter(this,"", MainRecyclerView, NativeFeedType.Share);
                MainRecyclerView.SetXAdapter(PostFeedAdapter, null);
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
                        TxtSharePost.Click += TxtSharePostOnClick;
                        break;
                    default:
                        TxtSharePost.Click -= TxtSharePostOnClick;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Event

        //Share Post 
        private async void TxtSharePostOnClick(object sender, EventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
                    return;
                }

                //Show a progress
                AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));

                switch (TypePost)
                {
                    case "Group":
                    {
                        (int apiStatus, dynamic respond) = await RequestsAsync.Posts.SharePostToAsync(PostData.PostId, GroupData.GroupId, "share_post_on_group", TxtContentPost.Text);
                        switch (apiStatus)
                        {
                            case 200:
                                ResultApi(apiStatus, respond);
                                break;
                            default:
                                Methods.DisplayAndHudErrorResult(this, respond);
                                break;
                        }
                        break;
                    }
                    case "Page":
                    {
                        (int apiStatus, dynamic respond) = await RequestsAsync.Posts.SharePostToAsync(PostData.PostId, PageData.PageId, "share_post_on_page",TxtContentPost.Text);
                        switch (apiStatus)
                        {
                            case 200:
                                ResultApi(apiStatus, respond);
                                break;
                            default:
                                Methods.DisplayAndHudErrorResult(this, respond);
                                break;
                        }
                        break;
                    }
                    case "MyTimeline":
                    {
                        (int apiStatus, dynamic respond) = await RequestsAsync.Posts.SharePostToAsync(PostData.PostId, UserDetails.UserId, "share_post_on_timeline", TxtContentPost.Text);
                        switch (apiStatus)
                        {
                            case 200:
                                ResultApi(apiStatus, respond);
                                break;
                            default:
                                Methods.DisplayAndHudErrorResult(this, respond);
                                break;
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

        private void ResultApi(int apiStatus, dynamic respond)
        {
            try
            {
                switch (apiStatus)
                {
                    case 200:
                    {
                        switch (respond)
                        {
                            case SharePostToObject result:
                            {
                                AndHUD.Shared.Dismiss(this);

                                //if (result.Data.SharedInfo.SharedInfoClass == null)
                                //{
                                //    result.Data.ParentId = PostData.PostId;

                                //    result.Data.SharedInfo = new SharedInfoUnion
                                //    {
                                //        SharedInfoClass = PostData
                                //    };
                                //}

                                //var globalContextTabbed = TabbedMainActivity.GetInstance();

                                //var countList = globalContextTabbed.NewsFeedTab.PostFeedAdapter.ItemCount;

                                //var combine = new FeedCombiner(result.Data, globalContextTabbed?.NewsFeedTab?.PostFeedAdapter?.ListDiffer, this);
                                //combine.CombineDefaultPostSections("Top");

                                //int countIndex = 1;
                                //var model1 = globalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.Story);
                                //var model2 = globalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                                //var model3 = globalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.FilterSection);
                                //var model4 = globalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AlertBox);
                                //var model5 = globalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SearchForPosts);

                                //if (model5 != null)
                                //    countIndex += globalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(model5) + 1;
                                //else if (model4 != null)
                                //    countIndex += globalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(model4) + 1;
                                //else if (model3 != null)
                                //    countIndex += globalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(model3) + 1;
                                //else if (model2 != null)
                                //    countIndex += globalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(model2) + 1;
                                //else if (model1 != null)
                                //    countIndex += globalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(model1) + 1;
                                //else
                                //    countIndex = 0;

                                //var emptyStateChecker = globalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);
                                //if (emptyStateChecker != null && globalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.Count > 1)
                                //    globalContextTabbed.NewsFeedTab.MainRecyclerView.RemoveByRowIndex(emptyStateChecker);

                                //globalContextTabbed.NewsFeedTab.PostFeedAdapter.NotifyItemRangeInserted(countIndex, globalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.Count - countList);

                                switch (TypePost)
                                {
                                    case "MyTimeline":
                                    {
                                        MyProfileActivity myProfileActivity = MyProfileActivity.GetInstance();
                                        if (myProfileActivity != null)
                                        {
                                            var countList1 = myProfileActivity.PostFeedAdapter.ItemCount;

                                            var combine1 = new FeedCombiner(result.Data, myProfileActivity.PostFeedAdapter.ListDiffer, this);

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
                                }

                                Toast.MakeText(this, GetText(Resource.String.Lbl_PostSuccessfullyShared), ToastLength.Short)?.Show();

                                switch (UserDetails.SoundControl)
                                {
                                    case true:
                                        Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("PopNotificationPost.mp3");
                                        break;
                                }

                                Finish();
                                break;
                            }
                        }

                        break;
                    }
                    default:
                        Methods.DisplayAndHudErrorResult(this, respond);
                        break;
                }
            }
            catch (Exception e)
            {
                AndHUD.Shared.Dismiss(this);
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        #endregion
         
        private void GetDataPost()
        {
            try
            {
                switch (TypePost)
                {
                    case "Group":
                    {
                        GroupData = JsonConvert.DeserializeObject<GroupClass>(Intent?.GetStringExtra("ShareToGroup") ?? "");
                        if (GroupData != null)
                        {
                            GlideImageLoader.LoadImage(this, GroupData.Avatar, PostSectionImage, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                            TxtUserName.Text = GroupData.GroupName;
                        }

                        break;
                    }
                    case "Page":
                    {
                        PageData = JsonConvert.DeserializeObject<PageClass>(Intent?.GetStringExtra("ShareToPage") ?? "");
                        if (PageData != null)
                        {
                            GlideImageLoader.LoadImage(this, PageData.Avatar, PostSectionImage, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                            TxtUserName.Text = PageData.PageName;
                        }

                        break;
                    }
                    case "MyTimeline":
                        GlideImageLoader.LoadImage(this, UserDetails.Avatar, PostSectionImage, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                        TxtUserName.Text = UserDetails.FullName;
                        break;
                }

                PostData = JsonConvert.DeserializeObject<PostDataObject>(Intent?.GetStringExtra("PostObject") ?? "");
                if (PostData != null)
                {
                    switch (TypePost)
                    {
                        case "Group" when PostData.GroupRecipient == null:
                        {
                            if (GroupData != null)
                            {
                                PostData.GroupId = GroupData.GroupId;
                                PostData.GroupRecipient = GroupData;
                            } 
                            break;
                        }
                        case "Page" when PostData.Publisher == null:
                        {
                            if (PageData != null)
                            {
                                PostData.PageId = PageData.PageId;
                                PostData.Publisher = new PublisherPost
                                {
                                    Avatar = PageData.Avatar,
                                    About = PageData.About,
                                    Active = PageData.Active,
                                    Address = PageData.Address,
                                    BackgroundImage = PageData.BackgroundImage,
                                    Boosted = Convert.ToInt32(PageData.Boosted),
                                    CallActionType = Convert.ToInt32(PageData.CallActionType),
                                    Category = PageData.Category,
                                    Company = PageData.Company,
                                    Cover = PageData.Cover,
                                    Google = PageData.Google,
                                    Instgram = PageData.Instgram,
                                    IsPageOnwer = PageData.IsPageOnwer,
                                    Linkedin = PageData.Linkedin,
                                    Name = PageData.Name,
                                    PageCategory = Convert.ToInt32(PageData.PageCategory),
                                    PageDescription = PageData.PageDescription,
                                    PageId = Convert.ToInt32(PageData.PageId),
                                    PageName = PageData.PageName,
                                    PageTitle = PageData.PageTitle,
                                    Phone = PageData.Phone,
                                    Rating = Convert.ToInt32(PageData.Rating),
                                    Registered = PageData.Registered,
                                    Twitter = PageData.Twitter,
                                    Url = PageData.Url,
                                };
                            } 
                            break;
                        }
                    }
                     
                    var combine = new FeedCombiner(PostData, PostFeedAdapter.ListDiffer, this);
                    combine.CombineDefaultPostSections();

                    PostFeedAdapter.NotifyDataSetChanged();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        } 
    }
}
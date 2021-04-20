using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

using Android.Views;
using Android.Widget;
using TheArtOfDev.Edmodo.Cropper;
using Newtonsoft.Json;
using WoWonder.Library.Anjo.Share;
using WoWonder.Library.Anjo.Share.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AFollestad.MaterialDialogs;
using Android.Content.Res;
using Android.Gms.Ads;
using Android.Graphics;
using AndroidX.Core.Content;
using AndroidX.SwipeRefreshLayout.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Google.Android.Material.FloatingActionButton;
using Java.Lang;
using WoWonder.Activities.AddPost;
using WoWonder.Activities.Base;
using WoWonder.Activities.Communities.Groups.Settings;
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
using WoWonderClient.Classes.Group;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Classes.Product;
using WoWonderClient.Requests;
using Exception = System.Exception;
using File = Java.IO.File;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.Communities.Groups
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class GroupProfileActivity : BaseActivity, MaterialDialog.IListCallback,MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private SwipeRefreshLayout SwipeRefreshLayout;
        private Button BtnJoin;
        private ImageButton BtnMore;
        private ImageView UserProfileImage, CoverImage, IconBack;
        private TextView IconEdit, TxtGroupName, TxtGroupUsername, TxtEditGroupInfo;
        private FloatingActionButton FloatingActionButtonView;
        private LinearLayout EditAvatarImageGroup;
        public WRecyclerView MainRecyclerView;
        public NativePostAdapter PostFeedAdapter;
        private string GroupId, ImageType;
        public static GroupClass GroupDataClass;
        private ImageView JoinRequestImage1, JoinRequestImage2, JoinRequestImage3;
        private RelativeLayout LayoutJoinRequest;
        private FeedCombiner Combiner;
        private static GroupProfileActivity Instance;
        private AdView MAdView;

        #endregion

        #region General

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            { 
                SetTheme(AppSettings.SetTabDarkTheme ? Resource.Style.MyTheme_Dark_Base : Resource.Style.MyTheme_Base);                 
                
                base.OnCreate(savedInstanceState);

                Methods.App.FullScreenApp(this);

                // Create your application here
                SetContentView(Resource.Layout.Group_Profile_Layout);

                Instance = this;

                GroupId = Intent?.GetStringExtra("GroupId") ?? string.Empty;

                //Get Value And Set Toolbar
                InitComponent();
                SetRecyclerViewAdapters();

                GetDataGroup();
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
                SwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);
                SwipeRefreshLayout.SetColorSchemeResources(Android.Resource.Color.HoloBlueLight, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
                SwipeRefreshLayout.Refreshing = false;
                SwipeRefreshLayout.Enabled = true;
                SwipeRefreshLayout.SetProgressBackgroundColorSchemeColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#424242") : Color.ParseColor("#f7f7f7"));

                UserProfileImage = (ImageView)FindViewById(Resource.Id.image_profile);
                CoverImage = (ImageView)FindViewById(Resource.Id.iv1);

                IconBack = (ImageView)FindViewById(Resource.Id.image_back);
                EditAvatarImageGroup = (LinearLayout)FindViewById(Resource.Id.LinearEdit);
                TxtEditGroupInfo = (TextView)FindViewById(Resource.Id.tv_EditGroupinfo);
                TxtGroupName = (TextView)FindViewById(Resource.Id.Group_name);
                TxtGroupUsername = (TextView)FindViewById(Resource.Id.Group_Username);
                BtnJoin = (Button)FindViewById(Resource.Id.joinButton);
                BtnMore = (ImageButton)FindViewById(Resource.Id.morebutton);
                IconEdit = (TextView)FindViewById(Resource.Id.IconEdit);
                FloatingActionButtonView = FindViewById<FloatingActionButton>(Resource.Id.floatingActionButtonView);
                FloatingActionButtonView.Visibility = ViewStates.Gone;
                JoinRequestImage1 = (ImageView)FindViewById(Resource.Id.image_page_1);
                JoinRequestImage2 = (ImageView)FindViewById(Resource.Id.image_page_2);
                JoinRequestImage3 = (ImageView)FindViewById(Resource.Id.image_page_3);

                LayoutJoinRequest = (RelativeLayout)FindViewById(Resource.Id.layout_join_Request);

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
                PostFeedAdapter = new NativePostAdapter(this, GroupId, MainRecyclerView, NativeFeedType.Group);
                MainRecyclerView.SetXAdapter(PostFeedAdapter, SwipeRefreshLayout);
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
                        IconBack.Click += IconBackOnClick;
                        EditAvatarImageGroup.Click += EditAvatarImageGroupOnClick;
                        TxtEditGroupInfo.Click += TxtEditGroupInfoOnClick;
                        BtnJoin.Click += BtnJoinOnClick;
                        BtnMore.Click += BtnMoreOnClick;
                        FloatingActionButtonView.Click += AddPostOnClick;
                        LayoutJoinRequest.Click += LayoutJoinRequestOnClick;
                        UserProfileImage.Click += UserProfileImageOnClick;
                        CoverImage.Click += CoverImageOnClick;
                        break;
                    default:
                        SwipeRefreshLayout.Refresh -= SwipeRefreshLayoutOnRefresh;
                        IconBack.Click -= IconBackOnClick;
                        EditAvatarImageGroup.Click -= EditAvatarImageGroupOnClick;
                        TxtEditGroupInfo.Click -= TxtEditGroupInfoOnClick;
                        BtnJoin.Click -= BtnJoinOnClick;
                        BtnMore.Click -= BtnMoreOnClick;
                        FloatingActionButtonView.Click -= AddPostOnClick;
                        LayoutJoinRequest.Click -= LayoutJoinRequestOnClick;
                        UserProfileImage.Click -= UserProfileImageOnClick;
                        CoverImage.Click -= CoverImageOnClick;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public static GroupProfileActivity GetInstance()
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
                BtnJoin = null!;
                BtnMore = null!;
                UserProfileImage = null!;
                CoverImage = null!;
                IconBack = null!;
                IconEdit = null!;
                TxtGroupName = null!;
                TxtGroupUsername = null!;
                TxtEditGroupInfo = null!;
                FloatingActionButtonView = null!;
                EditAvatarImageGroup = null!;
                MainRecyclerView = null!;
                PostFeedAdapter = null!;
                GroupId = null!;
                ImageType = null!;
                GroupDataClass = null!;
                JoinRequestImage1 = null!;
                JoinRequestImage2 = null!;
                JoinRequestImage3 = null!;
                LayoutJoinRequest = null!;
                Combiner = null!;
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

                GetDataGroup();
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
                var media = WoWonderTools.GetFile("", Methods.Path.FolderDiskImage, GroupDataClass.Cover.Split('/').Last(), GroupDataClass.Cover);
                if (media.Contains("http"))
                {
                    Intent intent = new Intent(Intent.ActionView, Uri.Parse(media));
                    StartActivity(intent);
                }
                else
                {
                    File file2 = new File(media);
                    var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);

                    Intent intent = new Intent(Intent.ActionPick);
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
                var media = WoWonderTools.GetFile("", Methods.Path.FolderDiskImage, GroupDataClass.Avatar.Split('/').Last(), GroupDataClass.Avatar);
                if (media.Contains("http"))
                {
                    Intent intent = new Intent(Intent.ActionView, Uri.Parse(media));
                    StartActivity(intent);
                }
                else
                {
                    File file2 = new File(media);
                    var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);

                    Intent intent = new Intent(Intent.ActionPick);
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

        //Event Show More : Copy Link , Share , Edit (If user isOwner_Groups)
        private void BtnMoreOnClick(object sender, EventArgs e)
        {
            try
            { 
                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                arrayAdapter.Add(GetString(Resource.String.Lbl_CopeLink));
                arrayAdapter.Add(GetString(Resource.String.Lbl_Share));
                if (GroupDataClass.IsOwner != null && GroupDataClass.IsOwner.Value)
                {
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

        //Event Add New post
        private void AddPostOnClick(object sender, EventArgs e)
        {
            try
            {
                var intent = new Intent(this, typeof(AddPostActivity));
                intent.PutExtra("Type", "SocialGroup");
                intent.PutExtra("PostId", GroupId);
                intent.PutExtra("itemObject", JsonConvert.SerializeObject(GroupDataClass));
                StartActivityForResult(intent, 2500);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Event Join_Group => Joined , Join Group
        private async void BtnJoinOnClick(object sender, EventArgs e)
        {
            try
            {
                switch (BtnJoin?.Tag?.ToString())
                {
                    case "MyGroup":
                        SettingGroup_OnClick();
                        break;
                    default:
                    {
                        if (!Methods.CheckConnectivity())
                        {
                            Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                        }
                        else
                        {
                            string isJoined = BtnJoin.Text == GetText(Resource.String.Btn_Joined) ? "false" : "true";
                            BtnJoin.BackgroundTintList = isJoined == "yes" || isJoined == "true" ? ColorStateList.ValueOf(Color.ParseColor("#efefef")) : ColorStateList.ValueOf(Color.ParseColor(AppSettings.MainColor));
                            BtnJoin.Text = GetText(isJoined == "yes" || isJoined == "true" ? Resource.String.Btn_Joined : Resource.String.Btn_Join_Group);
                            BtnJoin.SetTextColor(isJoined == "yes" || isJoined == "true" ? Color.Black : Color.White);

                            var (apiStatus, respond) = await RequestsAsync.Group.JoinGroupAsync(GroupId);
                            switch (apiStatus)
                            {
                                case 200:
                                {
                                    switch (respond)
                                    {
                                        case JoinGroupObject result when result.JoinStatus == "requested":
                                            BtnJoin.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(AppSettings.MainColor));
                                            BtnJoin.Text = GetText(Resource.String.Lbl_Request);
                                            BtnJoin.SetTextColor(Color.White);
                                            BtnMore.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(AppSettings.MainColor));
                                            BtnMore.ImageTintList = ColorStateList.ValueOf(Color.White);
                                            break;
                                        case JoinGroupObject result:
                                            isJoined = result.JoinStatus == "left" ? "false" : "true";
                                            BtnJoin.BackgroundTintList = isJoined == "yes" || isJoined == "true" ? ColorStateList.ValueOf(Color.ParseColor("#efefef")) : ColorStateList.ValueOf(Color.ParseColor(AppSettings.MainColor));
                                            BtnJoin.Text = GetText(isJoined == "yes" || isJoined == "true" ? Resource.String.Btn_Joined : Resource.String.Btn_Join_Group);
                                            BtnJoin.SetTextColor(isJoined == "yes" || isJoined == "true" ? Color.Black : Color.White);
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

        //Event Update Image Cover Group
        private void TxtEditGroupInfoOnClick(object sender, EventArgs e)
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

        //Event Update Image avatar Group
        private void EditAvatarImageGroupOnClick(object sender, EventArgs e)
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

        //Back
        private void IconBackOnClick(object sender, EventArgs e)
        {
           Finish();
        }

        //Join Request
        private void LayoutJoinRequestOnClick(object sender, EventArgs e)
        {
            try
            {
                var intent = new Intent(this, typeof(JoinRequestActivity));
                intent.PutExtra("GroupId", GroupId);
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
                            case Result.Ok:
                            {
                                switch (result.IsSuccessful)
                                {
                                    case true:
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

                                                        UpdateImageGroup_Api(ImageType, pathImg);
                                                        break;
                                                    }
                                                    case "Avatar":
                                                    {
                                                        pathImg = resultUri.Path;
                                     
                                                        //Set image
                                                        File file2 = new File(pathImg);
                                                        var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);
                                                        Glide.With(this).Load(photoUri).Apply(new RequestOptions()).Into(UserProfileImage);

                                                        var dataGroup = GroupsActivity.GetInstance()?.MAdapter?.GroupsAdapter?.GroupList?.FirstOrDefault(a => a.GroupId == GroupId);
                                                        if (dataGroup != null)
                                                        { 
                                                            dataGroup.Avatar = pathImg;
                                                            GroupsActivity.GetInstance()?.MAdapter?.GroupsAdapter?.NotifyDataSetChanged();

                                                            var dataGroup2 = ListUtils.MyGroupList.FirstOrDefault(a => a.GroupId == GroupId);
                                                            if (dataGroup2 != null)
                                                            {
                                                                dataGroup2.Avatar = pathImg;
                                                            }
                                                        }
                                     
                                                        UpdateImageGroup_Api(ImageType, pathImg);
                                                        break;
                                                    }
                                                }

                                                break;
                                            }
                                            default:
                                                Toast.MakeText(this, GetText(Resource.String.Lbl_something_went_wrong),ToastLength.Long)?.Show();
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
                    //add post
                    case 2500 when resultCode == Result.Ok:
                    {
                        if (!string.IsNullOrEmpty(data.GetStringExtra("itemObject")))
                        {
                            var postData = JsonConvert.DeserializeObject<PostDataObject>(data.GetStringExtra("itemObject") ?? "");
                            if (postData != null)
                            {
                                var countList = PostFeedAdapter.ItemCount;

                                var combine = new FeedCombiner(postData, PostFeedAdapter.ListDiffer, this);
                                combine.CombineDefaultPostSections("Top");

                                int countIndex = 1;
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
                        List<AdapterModelsClass> dataGlobal = diff.Where(a => a.PostData?.Id == postId).ToList();
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
                        string result = data.GetStringExtra("groupItem") ?? "";
                        var item = JsonConvert.DeserializeObject<GroupClass>(result);
                        if (item != null)
                            LoadPassedData(item);
                        break;
                    }
                    case 2019 when resultCode == Result.Ok:
                    {
                        var dataGroup = GroupsActivity.GetInstance()?.MAdapter?.GroupsAdapter?.GroupList?.FirstOrDefault(a => a.GroupId == GroupId);
                        if (dataGroup != null)
                        {
                            GroupsActivity.GetInstance()?.MAdapter?.GroupsAdapter?.GroupList.Remove(dataGroup);
                            GroupsActivity.GetInstance()?.MAdapter?.GroupsAdapter?.NotifyDataSetChanged();

                            ListUtils.MyGroupList.Remove(dataGroup);
                        }
                        Finish();
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
         
        #region MaterialDialog

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                string text = itemString.ToString();
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
                    SettingGroup_OnClick();
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

        //Event Menu >> Copy Link
        private void CopyLinkEvent()
        {
            try
            {
                Methods.CopyToClipboard(this, GroupDataClass.Url);
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
                            Title = GroupDataClass.GroupName,
                            Text = GroupDataClass.About,
                            Url = GroupDataClass.Url
                        });
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Event Menu >> Setting
        private void SettingGroup_OnClick()
        {
            try
            {
                var intent = new Intent(this, typeof(SettingsGroupActivity));
                intent.PutExtra("itemObject", JsonConvert.SerializeObject(GroupDataClass));
                intent.PutExtra("GroupId", GroupId);
                StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        } 
         
        #endregion

        #region Get Data Group

        private void GetDataGroup()
        {
            try
            {
                GroupDataClass = JsonConvert.DeserializeObject<GroupClass>(Intent?.GetStringExtra("GroupObject") ?? "");
                if (GroupDataClass != null)
                {
                    LoadPassedData(GroupDataClass); 
                }
                 
                PostFeedAdapter.SetLoading();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }

            StartApiService();
        }

        private void LoadPassedData(GroupClass result)
        {
            try
            {
                GlideImageLoader.LoadImage(this, result.Avatar, UserProfileImage, ImageStyle.CenterCrop, ImagePlaceholders.Color);
                Glide.With(this).Load(result.Cover.Replace(" ", "")).Apply(new RequestOptions().Placeholder(Resource.Drawable.Cover_image).Error(Resource.Drawable.Cover_image)).Into(CoverImage);

                TxtGroupUsername.Text = "@" + Methods.FunString.DecodeString(result.Username); 
                TxtGroupName.Text = Methods.FunString.DecodeString(result.Name);
                 
                if (result.UserId == UserDetails.UserId)
                    result.IsOwner = true;
                
                if (result.IsOwner != null && result.IsOwner.Value)
                {
                    BtnJoin.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(AppSettings.MainColor));
                    BtnJoin.Text = GetText(Resource.String.Lbl_Edit);
                    BtnJoin.SetTextColor( Color.White);
                    BtnJoin.Tag = "MyGroup";
                    BtnMore.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(AppSettings.MainColor));
                    BtnMore.ImageTintList = ColorStateList.ValueOf(Color.White); 
                }
                else
                {
                    BtnJoin.BackgroundTintList = WoWonderTools.IsJoinedGroup(result) ? ColorStateList.ValueOf(Color.ParseColor("#efefef")) : ColorStateList.ValueOf(Color.ParseColor(AppSettings.MainColor));
                    BtnJoin.Text = GetText(WoWonderTools.IsJoinedGroup(result) ? Resource.String.Btn_Joined : Resource.String.Btn_Join_Group);
                    BtnJoin.SetTextColor(WoWonderTools.IsJoinedGroup(result) ? Color.Black : Color.White);
                    BtnMore.BackgroundTintList = WoWonderTools.IsJoinedGroup(result) ? ColorStateList.ValueOf(Color.ParseColor("#efefef")) : ColorStateList.ValueOf(Color.ParseColor(AppSettings.MainColor));
                    BtnMore.ImageTintList = WoWonderTools.IsJoinedGroup(result) ? ColorStateList.ValueOf(Color.Black) : ColorStateList.ValueOf(Color.White);
                    BtnJoin.Tag = "UserGroup";
                }

                var modelsClass = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.InfoGroupBox);
                switch (modelsClass)
                {
                    case null:
                        Combiner.InfoGroupBox(new GroupPrivacyModelClass {GroupClass = result, GroupId = result.GroupId}, 0);
                        break;
                    default:
                        modelsClass.PrivacyModelClass = new GroupPrivacyModelClass
                        {
                            GroupClass = result,
                            GroupId = result.GroupId
                        };
                        PostFeedAdapter.NotifyItemChanged(PostFeedAdapter.ListDiffer.IndexOf(modelsClass));
                        break;
                }
                 
                var checkAboutBox = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AboutBox);
                switch (checkAboutBox)
                {
                    case null:
                        Combiner.AboutBoxPostView(Methods.FunString.DecodeString(result.About), 0);
                        break;
                    default:
                        checkAboutBox.AboutModel.Description = Methods.FunString.DecodeString(result.About);
                        PostFeedAdapter.NotifyItemChanged(PostFeedAdapter.ListDiffer.IndexOf(checkAboutBox));
                        break;
                }

                if (result.IsOwner != null && result.IsOwner.Value || WoWonderTools.IsJoinedGroup(result))
                {
                    var checkSection = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                    switch (checkSection)
                    {
                        case null:
                        {
                            Combiner.AddPostBoxPostView("Group", -1, new PostDataObject { GroupRecipient = result });

                            switch (AppSettings.ShowSearchForPosts)
                            {
                                case true:
                                    Combiner.SearchForPostsView("Group", new PostDataObject { GroupRecipient = result });
                                    break;
                            } 

                            PostFeedAdapter.NotifyItemInserted(PostFeedAdapter.ListDiffer.Count -1 );
                            break;
                        }
                    }
                    FloatingActionButtonView.Visibility = ViewStates.Visible;
                }
                 
                if (result.IsOwner != null && result.IsOwner.Value)
                {
                    EditAvatarImageGroup.Visibility = ViewStates.Visible;
                    TxtEditGroupInfo.Visibility = ViewStates.Visible;
                }
                else
                {
                    EditAvatarImageGroup.Visibility = ViewStates.Gone;
                    TxtEditGroupInfo.Visibility = ViewStates.Gone;
                }
                  
                if (WoWonderTools.IsJoinedGroup(result) || result.Privacy == "1" || result.IsOwner != null && result.IsOwner.Value)
                {
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => MainRecyclerView.ApiPostAsync.FetchNewsFeedApiPosts() });
                }
                else
                {
                    PostFeedAdapter.SetLoaded();

                    var viewProgress = PostFeedAdapter.ListDiffer.FirstOrDefault(anjo => anjo.TypeView == PostModelType.ViewProgress);
                    if (viewProgress != null)
                        MainRecyclerView.RemoveByRowIndex(viewProgress);

                    var emptyStateCheck = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.PostData != null && a.TypeView != PostModelType.AddPostBox && a.TypeView != PostModelType.InfoGroupBox && a.TypeView != PostModelType.SearchForPosts);
                    if (emptyStateCheck != null)
                    {
                        var emptyStateChecker = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);
                        if (emptyStateChecker != null && PostFeedAdapter.ListDiffer.Count > 1)
                            PostFeedAdapter.ListDiffer.Remove(emptyStateChecker);
                    }
                    else
                    {
                        var emptyStateChecker = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);
                        switch (emptyStateChecker)
                        {
                            case null:
                                PostFeedAdapter.ListDiffer.Add(new AdapterModelsClass { TypeView = PostModelType.EmptyState, Id = 744747447 });
                                break;
                        }
                    }
                    PostFeedAdapter.NotifyDataSetChanged();
                }

                WoWonderTools.GetFile("", Methods.Path.FolderDiskImage, result.Cover.Split('/').Last(), result.Cover);
                WoWonderTools.GetFile("", Methods.Path.FolderDiskImage, result.Avatar.Split('/').Last(), result.Avatar);
                SwipeRefreshLayout.Refreshing = false;
            }
            catch (Exception e)
            {
                SwipeRefreshLayout.Refreshing = false;
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void StartApiService()
        {
            if (!Methods.CheckConnectivity())
                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
            else
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { GetGroupDataApi, GetJoin });
        }

        private async Task GetGroupDataApi()
        {
            var (apiStatus, respond) = await RequestsAsync.Group.GetGroupDataAsync(GroupId);

            if (apiStatus != 200 || respond is not GetGroupDataObject result || result.GroupData == null)
                Methods.DisplayReportResult(this, respond);
            else
            {
                GroupDataClass = result.GroupData;
                RunOnUiThread(() => { LoadPassedData(GroupDataClass); }); 
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

        // Function Update Image Group : Avatar && Cover
        private async void UpdateImageGroup_Api(string type, string path)
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
                            var (apiStatus, respond) = await RequestsAsync.Group.UpdateGroupAvatarAsync(GroupId, path).ConfigureAwait(false);
                            switch (apiStatus)
                            {
                                case 200:
                                {
                                    switch (respond)
                                    {
                                        case MessageObject result:
                                            Toast.MakeText(this, result.Message, ToastLength.Short)?.Show(); 
                                            //GlideImageLoader.LoadImage(this, file.Path, UserProfileImage, ImageStyle.RoundedCrop, ImagePlaceholders.Color);
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
                            var (apiStatus, respond) = await RequestsAsync.Group.UpdateGroupCoverAsync(GroupId, path).ConfigureAwait(false);
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

        private async Task GetJoin()
        {
            if (GroupDataClass.UserId == UserDetails.UserId)
            {
                var (apiStatus, respond) = await RequestsAsync.Group.GetGroupJoinRequestsAsync(GroupId, "5");
                switch (apiStatus)
                {
                    case 200:
                    {
                        switch (respond)
                        {
                            case GetGroupJoinRequestsObject result:
                                RunOnUiThread(() =>
                                {
                                    var respondList = result.Data.Count;
                                    switch (respondList)
                                    {
                                        case > 0:
                                            LayoutJoinRequest.Visibility = ViewStates.Visible;
                                            try
                                            {
                                                var list = result.Data.TakeLast(4).ToList();

                                                for (var i = 0; i < list.Count; i++)
                                                {
                                                    var item = list[i];
                                                    switch (item)
                                                    {
                                                        case null:
                                                            continue;
                                                        default:
                                                            switch (i)
                                                            {
                                                                case 0:
                                                                    GlideImageLoader.LoadImage(this, item?.UserData?.Avatar, JoinRequestImage1, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                                                                    break;
                                                                case 1:
                                                                    GlideImageLoader.LoadImage(this, item?.UserData?.Avatar, JoinRequestImage2, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                                                                    break;
                                                                case 2:
                                                                    GlideImageLoader.LoadImage(this, item?.UserData?.Avatar, JoinRequestImage3, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
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

                                            break;
                                        default:
                                            LayoutJoinRequest.Visibility = ViewStates.Gone;
                                            break;
                                    }
                                });
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
                RunOnUiThread(() => { LayoutJoinRequest.Visibility = ViewStates.Gone; });
            } 
        }
    }
}
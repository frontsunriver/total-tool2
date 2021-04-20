using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AFollestad.MaterialDialogs;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using AndroidX.SwipeRefreshLayout.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Java.IO;
using Java.Lang;
using Newtonsoft.Json;
using TheArtOfDev.Edmodo.Cropper;
using WoWonder.Activities.Base;
using WoWonder.Activities.General;
using WoWonder.Activities.Live.Utils;
using WoWonder.Activities.NativePost.Extra;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Activities.SettingsPreferences.General;
using WoWonder.Activities.SettingsPreferences.Privacy;
using WoWonder.Activities.Wallet;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.Library.Anjo.Share;
using WoWonder.Library.Anjo.Share.Abstractions;
using WoWonder.SQLite;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Classes.Product;
using WoWonderClient.Classes.User;
using WoWonderClient.Requests;
using Console = System.Console;
using Exception = System.Exception;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.MyProfile
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MyProfileActivity : BaseActivity, MaterialDialog.IListCallback
    {
        #region Variables Basic
         
        private ImageView ImageCover, ImageAvatar, ImageBack; 
        private RelativeLayout BtnMore; 
        private TextView TxtName, TxtUsername;
        private Button BtnEdit, BtnWallet;

        private SwipeRefreshLayout SwipeRefreshLayout;
        public WRecyclerView MainRecyclerView;

        private AdView MAdView;

        private FeedCombiner Combiner; 
        private static MyProfileActivity Instance;
        private UserDataObject UserData;

        public NativePostAdapter PostFeedAdapter;
        private string ImageType;

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
                SetContentView(Resource.Layout.MyProfile_Layout);

                Instance = this;

                //Get Value And Set Toolbar
                InitComponent(); 
                SetRecyclerViewAdapters();

                GetMyInfoData();
                PostClickListener.OpenMyProfile = true;

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
         
        protected override void OnDestroy()
        {
            try
            {
                MainRecyclerView.ReleasePlayer();
                PostClickListener.OpenMyProfile = false;
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
         
        #region Functions

        private void InitComponent()
        {
            try
            { 
                ImageCover = FindViewById<ImageView>(Resource.Id.cover_image);
                ImageBack = FindViewById<ImageView>(Resource.Id.back); 
                BtnMore = FindViewById<RelativeLayout>(Resource.Id.BtnMore); 
                ImageAvatar = FindViewById<ImageView>(Resource.Id.profileimage_head);
                
                TxtName = FindViewById<TextView>(Resource.Id.name_profile);
                TxtUsername = FindViewById<TextView>(Resource.Id.username_profile);

                BtnEdit = FindViewById<Button>(Resource.Id.btnEdit);
                BtnWallet = FindViewById<Button>(Resource.Id.btnWallet);

                MainRecyclerView = FindViewById<WRecyclerView>(Resource.Id.newsfeedRecyler);
                
                SwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);
                SwipeRefreshLayout.SetColorSchemeResources(Android.Resource.Color.HoloBlueLight, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
                SwipeRefreshLayout.Refreshing = false;
                SwipeRefreshLayout.Enabled = true;
                SwipeRefreshLayout.SetProgressBackgroundColorSchemeColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#424242") : Color.ParseColor("#f7f7f7"));
                 
                MAdView = FindViewById<AdView>(Resource.Id.adView);
                AdsGoogle.InitAdView(MAdView, MainRecyclerView);
                 
                if (AppSettings.FlowDirectionRightToLeft)
                    ImageBack.SetImageResource(Resource.Drawable.ic_action_ic_back_rtl);

                BtnWallet.Text = GetText(AppSettings.ShowWallet ? Resource.String.Lbl_Wallet : Resource.String.Lbl_Share);
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
                PostFeedAdapter = new NativePostAdapter(this, UserDetails.UserId, MainRecyclerView, NativeFeedType.User);
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
                        ImageCover.Click += ImageCoverOnClick;
                        ImageBack.Click += ImageBackOnClick;
                        BtnMore.Click += BtnMoreOnClick; 
                        ImageAvatar.Click += ImageAvatarOnClick;
                        BtnEdit.Click += BtnEditOnClick;
                        BtnWallet.Click += BtnWalletOnClick;
                        SwipeRefreshLayout.Refresh += SwipeRefreshLayoutOnRefresh;
                        break;
                    default:
                        ImageCover.Click -= ImageCoverOnClick;
                        ImageBack.Click -= ImageBackOnClick;
                        BtnMore.Click -= BtnMoreOnClick;
                        ImageAvatar.Click -= ImageAvatarOnClick;
                        BtnEdit.Click -= BtnEditOnClick;
                        BtnWallet.Click -= BtnWalletOnClick;
                        SwipeRefreshLayout.Refresh -= SwipeRefreshLayoutOnRefresh;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static MyProfileActivity GetInstance()
        {
            try
            {
                return Instance;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null;
            }
        }

        private void DestroyBasic()
        {
            try
            {
                ImageCover = null!;
                Instance = null!;
                ImageAvatar = null!;
                ImageBack = null!;
                BtnMore = null!;
                TxtName = null!;
                TxtUsername = null!;
                BtnEdit = null!; 
                SwipeRefreshLayout = null!;
                MainRecyclerView = null!;
                MAdView = null!;
                Combiner = null!;
                UserData = null!;
                PostFeedAdapter = null!; 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion
         
        #region Event
         
        private void SwipeRefreshLayoutOnRefresh(object sender, EventArgs e)
        {
            try
            {
                PostFeedAdapter.ListDiffer.Clear();
                PostFeedAdapter.NotifyDataSetChanged();

                StartApiService();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void BtnWalletOnClick(object sender, EventArgs e)
        {
            try
            {
                if (AppSettings.ShowWallet)
                { 
                    var intent = new Intent(this, typeof(TabbedWalletActivity));
                    StartActivity(intent);
                }
                else
                {
                    OnShare_Button_Click();
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        private void BtnEditOnClick(object sender, EventArgs e)
        {
            try
            {
                var intent = new Intent(this, typeof(EditMyProfileActivity));
                StartActivityForResult(intent, 5124);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void ImageAvatarOnClick(object sender, EventArgs e)
        {
            try
            {
                var media = WoWonderTools.GetFile("", Methods.Path.FolderDiskImage, UserData.Avatar.Split('/').Last(), UserData.Avatar);
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

                //var intent = new Intent(this, typeof(ViewFullPostActivity));
                //intent.PutExtra("Id", UserData.AvatarPostId);
                ////intent.PutExtra("DataItem", JsonConvert.SerializeObject(e.NewsFeedClass));
                //StartActivity(intent);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void BtnMoreOnClick(object sender, EventArgs e)
        {
            try
            {
                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);
                arrayAdapter.Add(GetText(Resource.String.Lbl_EditAvatar));
                arrayAdapter.Add(GetText(Resource.String.Lbl_EditCover));
                
                arrayAdapter.Add(GetText(Resource.String.Lbl_CopeLink));
                arrayAdapter.Add(GetText(Resource.String.Lbl_Share));
                arrayAdapter.Add(GetText(Resource.String.Lbl_Activities));
                arrayAdapter.Add(GetText(Resource.String.Lbl_ViewPrivacy));
                arrayAdapter.Add(GetText(Resource.String.Lbl_SettingsAccount));

                switch (ListUtils.SettingsSiteList?.Pro)
                {
                    case "1" when AppSettings.ShowGoPro && UserData.ProType != "4":
                        arrayAdapter.Add(GetText(Resource.String.Lbl_upgrade_now));
                        break;
                }

                dialogList.Title(Resource.String.Lbl_More).TitleColorRes(Resource.Color.primary);
                dialogList.Items(arrayAdapter);
                dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(new WoWonderTools.MyMaterialDialog());
                dialogList.AlwaysCallSingleChoiceCallback();
                dialogList.ItemsCallback(this).Build().Show();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void ImageBackOnClick(object sender, EventArgs e)
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

        private void ImageCoverOnClick(object sender, EventArgs e)
        {
            try
            {
                var media = WoWonderTools.GetFile("", Methods.Path.FolderDiskImage, UserData.Cover.Split('/').Last(), UserData.Cover);
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

                //var intent = new Intent(this, typeof(ViewFullPostActivity));
                //intent.PutExtra("Id", UserData.CoverPostId);
                ////intent.PutExtra("DataItem", JsonConvert.SerializeObject(e.NewsFeedClass));
                //StartActivity(intent);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion
         
        #region Get Profile

        private void GetMyInfoData()
        {
            try
            {
                var dataUser = ListUtils.MyProfileList?.FirstOrDefault();
                if (dataUser != null)
                {
                    LoadPassedDate(dataUser);

                    switch (ListUtils.MyFollowersList.Count)
                    {
                        case > 0 when dataUser.Details.DetailsClass != null:
                            LoadFriendsLayout(new List<UserDataObject>(ListUtils.MyFollowersList), Methods.FunString.FormatPriceValue(Convert.ToInt32(dataUser.Details.DetailsClass.FollowingCount)));
                            break;
                    }

                    PostFeedAdapter.NotifyDataSetChanged();
                }

                PostFeedAdapter.SetLoading();

                StartApiService();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void StartApiService()
        {
            if (!Methods.CheckConnectivity())
                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
            else
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { GetProfileApi, GetAlbumUserApi });
        }

        private async Task GetProfileApi()
        {
            var (apiStatus, respond) = await RequestsAsync.Global.GetUserDataAsync(UserDetails.UserId);

            if (apiStatus != 200 || respond is not GetUserDataObject result || result.UserData == null)
            {
                Methods.DisplayReportResult(this, respond);
            }
            else
            {
                LoadPassedDate(result.UserData);

                switch (result.JoinedGroups.Count)
                {
                    case > 0 when result.UserData.Details.DetailsClass != null:
                        RunOnUiThread(() => { LoadGroupsLayout(result.JoinedGroups, Methods.FunString.FormatPriceValue(Convert.ToInt32(result.UserData.Details.DetailsClass.GroupsCount))); });
                        break;
                    case > 0:
                        RunOnUiThread(() => { LoadGroupsLayout(result.JoinedGroups, Methods.FunString.FormatPriceValue(result.JoinedGroups.Count)); });
                        break;
                }

                switch (result.LikedPages.Count)
                {
                    case > 0:
                        RunOnUiThread(() => { LoadPagesLayout(result.LikedPages); });
                        break;
                }
                 
                switch (result.Following.Count)
                {
                    //if (SPrivacyFriend == "0" || result.UserProfileObject?.IsFollowing == "1" && SPrivacyFriend == "1" || SPrivacyFriend == "2")
                    case > 0 when result.UserData.Details.DetailsClass != null:
                        RunOnUiThread(() => { LoadFriendsLayout(result.Following, Methods.FunString.FormatPriceValue(Convert.ToInt32(result.UserData.Details.DetailsClass.FollowingCount))); });
                        break;
                }
                 
                //##Set the AddBox place on Main RecyclerView
                //------------------------------------------------------------------------
                var check = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.FollowersBox);
                var check2 = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.GroupsBox);
                var check3 = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.PagesBox);
                var check4 = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.ImagesBox);
                var check5 = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AboutBox);
                var check6 = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.InfoUserBox);
                var check8 = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);

                if (check != null)
                {
                    Combiner.AddPostBoxPostView("user", PostFeedAdapter.ListDiffer.IndexOf(check) + 1);
                }
                else if (check2 != null)
                {
                    Combiner.AddPostBoxPostView("user", PostFeedAdapter.ListDiffer.IndexOf(check2) + 1);
                }
                else if (check3 != null)
                {
                    Combiner.AddPostBoxPostView("user", PostFeedAdapter.ListDiffer.IndexOf(check3) + 1);
                }
                else if (check4 != null)
                {
                    Combiner.AddPostBoxPostView("user", PostFeedAdapter.ListDiffer.IndexOf(check4) + 1);
                }
                else if (check5 != null)
                {
                    Combiner.AddPostBoxPostView("user", PostFeedAdapter.ListDiffer.IndexOf(check5) + 1);
                }
                else if (check6 != null)
                {
                    Combiner.AddPostBoxPostView("user", PostFeedAdapter.ListDiffer.IndexOf(check6) + 1);
                }
                else if (check8 != null)
                {
                    Combiner.AddPostBoxPostView("user", PostFeedAdapter.ListDiffer.IndexOf(check8) + 1);
                }

                switch (AppSettings.ShowSearchForPosts)
                {
                    case true:
                        Combiner.SearchForPostsView("user");
                        break;
                }

                //------------------------------------------------------------------------ 
                RunOnUiThread(() =>
                {
                    try
                    {
                        PostFeedAdapter.NotifyDataSetChanged();
                        var sqlEntity = new SqLiteDatabase();
                        sqlEntity.Insert_Or_Update_To_MyProfileTable(result.UserData);

                        switch (result.Following?.Count)
                        {
                            case > 0:
                                sqlEntity.Insert_Or_Replace_MyContactTable(new ObservableCollection<UserDataObject>(result.Following));
                                break;
                        }

                        ListUtils.MyFollowersList = result.Followers?.Count switch
                        {
                            > 0 => new ObservableCollection<UserDataObject>(result.Followers),
                            _ => ListUtils.MyFollowersList
                        };
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                });

                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => MainRecyclerView.ApiPostAsync.FetchNewsFeedApiPosts() });
            }
        }

        private async Task GetAlbumUserApi()
        {
            var (apiStatus, respond) = await RequestsAsync.Album.GetPostByTypeAsync(UserDetails.UserId, "photos");
            if (apiStatus != 200 || respond is not PostObject result)
            {
                Methods.DisplayReportResult(this, respond);
            }
            else
            {
                switch (result.Data.Count)
                {
                    case > 0:
                        {
                            result.Data.RemoveAll(w => string.IsNullOrEmpty(w.PostFileFull));

                            var count = result.Data.Count;
                            switch (count)
                            {
                                case > 10:
                                    RunOnUiThread(() => { LoadImagesLayout(result.Data.Take(9).ToList(), Methods.FunString.FormatPriceValue(Convert.ToInt32(count.ToString()))); });
                                    break;
                                case > 5:
                                    RunOnUiThread(() => { LoadImagesLayout(result.Data.Take(6).ToList(), Methods.FunString.FormatPriceValue(Convert.ToInt32(count.ToString()))); });
                                    break;
                                case > 0:
                                    RunOnUiThread(() => { LoadImagesLayout(result.Data.ToList(), Methods.FunString.FormatPriceValue(Convert.ToInt32(count.ToString()))); });
                                    break;
                            }

                            break;
                        }
                }
            }
        }

        private void LoadPassedDate(UserDataObject result)
        {
            try
            {
                UserData = result;

                TxtUsername.Text = "@" +result.Username; 
                  
                TxtName.Text = WoWonderTools.GetNameFinal(result);

                switch (result.Verified)
                {
                    case "1":
                        TxtName.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.icon_checkmark_small_vector, 0);
                        break;
                }

                var checkHeaderSection = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.ProfileHeaderSection);
                if (checkHeaderSection == null)
                {
                    Combiner.ProfileHeaderView(result, 0);
                } 

                var checkAboutBox = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AboutBox);
                switch (checkAboutBox)
                {
                    case null:
                        Combiner.AboutBoxPostView(WoWonderTools.GetAboutFinal(result), 2); 
                        break;
                    default:
                        checkAboutBox.AboutModel.Description = WoWonderTools.GetAboutFinal(result); 
                        break;
                }

                var checkInfoUserBox = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.InfoUserBox);
                switch (checkInfoUserBox)
                {
                    case null:
                        Combiner.InfoUserBoxPostView(result, 4);
                        break;
                    default:
                        checkInfoUserBox.InfoUserModel.UserData = result; 
                        break;
                }

                GlideImageLoader.LoadImage(this, result.Avatar, ImageAvatar, ImageStyle.CircleCrop, ImagePlaceholders.Drawable); 
                Glide.With(this).Load(result.Cover).Apply(new RequestOptions().Placeholder(Resource.Drawable.Cover_image).Error(Resource.Drawable.Cover_image)).Into(ImageCover);
                 
                WoWonderTools.GetFile("", Methods.Path.FolderDiskImage, result.Cover.Split('/').Last(), result.Cover);
                WoWonderTools.GetFile("", Methods.Path.FolderDiskImage, result.Avatar.Split('/').Last(), result.Avatar);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void LoadFriendsLayout(List<UserDataObject> followers, string friendsCounter)
        {
            try
            {
                if (followers?.Count > 0)
                {
                    BtnMore.Visibility = ViewStates.Visible;

                    var followersClass = new FollowersModelClass
                    {
                        TitleHead = GetText(AppSettings.ConnectivitySystem == 1 ? Resource.String.Lbl_Following : Resource.String.Lbl_Friends),
                        FollowersList = new List<UserDataObject>(followers.Take(12)),
                        More = friendsCounter
                    };

                    var check = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.FollowersBox);
                    if (check != null)
                    {
                        check.FollowersModel = followersClass;
                    }
                    else
                    {
                        Combiner.FollowersBoxPostView(followersClass, 6); 
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void LoadImagesLayout(List<PostDataObject> images, string imagesCounter)
        {
            try
            {
                if (images?.Count > 0)
                {
                    var imagesClass = new ImagesModelClass
                    {
                        //TitleHead = GetText(Resource.String.Lbl_Profile_Picture),
                        TitleHead = GetText(Resource.String.Lbl_FeaturedPosts),
                        ImagesList = images,
                        More = images.Count.ToString()
                    };

                    var check = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.ImagesBox);
                    if (check != null)
                    {
                        check.ImagesModel = imagesClass; 
                    }
                    else
                    {
                        Combiner.ImagesBoxPostView(imagesClass, 6); 
                    }

                    RunOnUiThread(() => { PostFeedAdapter.NotifyDataSetChanged(); });
                    //------------------------------------------------------------------------ 
                    Console.WriteLine(imagesCounter);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void LoadPagesLayout(List<PageClass> pages)
        {
            try
            {
                if (pages?.Count > 0)
                {
                    var checkNull = pages.Where(a => string.IsNullOrEmpty(a.PageId)).ToList();
                    if (checkNull.Count is > 0)
                        foreach (var item in checkNull)
                            pages.Remove(item);

                    var pagesClass = new PagesModelClass
                    {
                        TitleHead = GetText(Resource.String.Lbl_PagesLiked),
                        PagesList = new List<PageClass>(pages)
                    };

                    var check = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.PagesBox);
                    if (check != null)
                    {
                        check.PagesModel = pagesClass; 
                    }
                    else
                    {
                        Combiner.PagesBoxPostView(pagesClass, 6); 
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void LoadGroupsLayout(List<GroupClass> groups, string groupsCounter)
        {
            try
            {
                if (groups?.Count > 0)
                {
                    var groupsClass = new GroupsModelClass
                    {
                        TitleHead = GetText(Resource.String.Lbl_Groups),
                        GroupsList = new List<GroupClass>(groups.Take(12)),
                        More = groupsCounter,
                        UserProfileId = UserDetails.UserId
                    };

                    var check = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.GroupsBox);
                    if (check != null)
                    {
                        check.GroupsModel = groupsClass;
                        //PostFeedAdapter.NotifyItemInserted(PostFeedAdapter.ListDiffer.IndexOf(check) + 1);
                    }
                    else
                    {
                        Combiner.GroupsBoxPostView(groupsClass, 6);
                        //PostFeedAdapter.NotifyItemInserted(1);
                    }
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
                var text = itemString.ToString();
                if (text == GetText(Resource.String.Lbl_EditAvatar))
                {
                    OpenDialogGallery("Avatar");
                }
                else if (text == GetText(Resource.String.Lbl_EditCover))
                {
                    OpenDialogGallery("Cover");
                }
                else if (text == GetText(Resource.String.Lbl_CopeLink))
                {
                    OnCopeLinkToProfile_Button_Click();
                }
                else if (text == GetText(Resource.String.Lbl_Share))
                {
                    OnShare_Button_Click();
                }
                else if (text == GetText(Resource.String.Lbl_ViewPrivacy))
                {
                    OnViewPrivacy_Button_Click();
                }
                else if (text == GetText(Resource.String.Lbl_Activities))
                {
                    OnMyActivities_Button_Click();
                }
                else if (text == GetText(Resource.String.Lbl_SettingsAccount))
                {
                    OnSettingsAccount_Button_Click();
                }
                else if (text == GetText(Resource.String.Lbl_upgrade_now))
                {
                    UpgradeNow_Click();
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        //Event Menu >> Cope Link To Profile
        private void OnCopeLinkToProfile_Button_Click()
        {
            try
            {
                Methods.CopyToClipboard(this, UserData.Url);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Event Menu >> Share
        private async void OnShare_Button_Click()
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
                            Title = UserData.Name,
                            Text = "",
                            Url = UserData.Url
                        });
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Event Menu >> View Privacy Shortcuts
        private void OnViewPrivacy_Button_Click()
        {
            try
            {
                var intent = new Intent(this, typeof(PrivacyActivity));
                StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Event Menu >> View Privacy Shortcuts
        private void OnMyActivities_Button_Click()
        {
            try
            {
                var intent = new Intent(this, typeof(MyActivitiesActivity));
                StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Event Menu >> General Account 
        private void OnSettingsAccount_Button_Click()
        {
            try
            {
                var intent = new Intent(this, typeof(GeneralAccountActivity));
                StartActivity(intent);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void UpgradeNow_Click()
        {
            try
            {
                var intent = new Intent(this, typeof(GoProActivity));
                StartActivity(intent);
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

        private async Task Update_Image_Api(string type, string path)
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
                                var (apiStatus, respond) = await RequestsAsync.Global.UpdateUserAvatarAsync(path);
                                switch (apiStatus)
                                {
                                    case 200:
                                        {
                                            switch (respond)
                                            {
                                                case MessageObject result:
                                                    {
                                                        Console.WriteLine(result.Message);
                                                        Toast.MakeText(this, GetText(Resource.String.Lbl_Image_changed_successfully), ToastLength.Short)?.Show();

                                                        var file2 = new File(path);
                                                        var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);
                                                        Glide.With(this).Load(photoUri).Apply(new RequestOptions().CircleCrop()).Into(ImageAvatar);


                                                        //Set image  
                                                        //GlideImageLoader.LoadImage(this, path, UserProfileImage, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                                                        break;
                                                    }
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
                                var (apiStatus, respond) = await RequestsAsync.Global.UpdateUserCoverAsync(path);
                                switch (apiStatus)
                                {
                                    case 200:
                                        {
                                            switch (respond)
                                            {
                                                case MessageObject result:
                                                    {
                                                        Console.WriteLine(result.Message);
                                                        Toast.MakeText(this, GetText(Resource.String.Lbl_Image_changed_successfully), ToastLength.Short)?.Show();

                                                        //Set image 
                                                        var file2 = new File(path);
                                                        var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);
                                                        Glide.With(this).Load(photoUri).Apply(new RequestOptions()).Into(ImageCover);

                                                        //GlideImageLoader.LoadImage(this, path, CoverImage, ImageStyle.FitCenter, ImagePlaceholders.Drawable);
                                                        break;
                                                    }
                                            }

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
                                                                        pathImg = resultUri.Path;
                                                                        UserDetails.Cover = pathImg;
                                                                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => Update_Image_Api(ImageType, pathImg) });
                                                                        break;
                                                                    case "Avatar":
                                                                        pathImg = resultUri.Path;
                                                                        UserDetails.Avatar = pathImg;
                                                                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => Update_Image_Api(ImageType, pathImg) });
                                                                        break;
                                                                }

                                                                break;
                                                            }
                                                        default:
                                                            Toast.MakeText(this, GetText(Resource.String.Lbl_something_went_wrong), ToastLength.Long)?.Show();
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
                            var item = JsonConvert.DeserializeObject<ProductDataObject>(data?.GetStringExtra("itemData") ?? "");
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
                    //Edit profile 
                    case 5124 when resultCode == Result.Ok:
                        {
                            var dataUser = ListUtils.MyProfileList?.FirstOrDefault();
                            if (dataUser != null)
                                LoadPassedDate(dataUser);
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
          
    }
}
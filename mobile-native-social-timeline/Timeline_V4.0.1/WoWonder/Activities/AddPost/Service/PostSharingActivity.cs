using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AFollestad.MaterialDialogs;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS; 
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using AndroidX.Core.Content;
using AndroidX.Core.Widget;
using AndroidX.RecyclerView.Widget;
using AndroidX.SlidingPaneLayout.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Com.Luseen.Autolinklibrary; 
using Com.Sothree.Slidinguppanel;
using TheArtOfDev.Edmodo.Cropper;
using Java.Lang;
using Newtonsoft.Json;
using WoWonder.Activities.AddPost.Adapters;
using WoWonder.Activities.Base;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using WoWonderClient;
using WoWonderClient.Classes.Event;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Posts;
using Console = System.Console;
using Exception = System.Exception;
using File = Java.IO.File;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.AddPost.Service
{
    [Activity(Icon = "@mipmap/icon", Label = "Post Sharing", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    [IntentFilter(new[] { Intent.ActionSend , Intent.ActionSendMultiple }, Categories = new[] { Intent.CategoryDefault } , DataMimeTypes = new []{ "application/*"  , "image/*" , "video/*"  , "audio/*" , "text/plain" })]
    public class PostSharingActivity : BaseActivity, SlidingPaneLayout.IPanelSlideListener, SlidingUpPanelLayout.IPanelSlideListener, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback, MaterialDialog.IInputCallback
    {
        #region Variables Basic

        private Toolbar TopToolBar;
        public SlidingUpPanelLayout SlidingUpPanel;
        private ImageView PostSectionImage;
        private TextView TxtAddPost, TxtUserName;
        private EditText TxtContentPost;
        private RecyclerView PostTypeRecyclerView, AttachmentRecyclerView, PollRecyclerView, ColorBoxRecyclerView;
        private MainPostAdapter MainPostAdapter;
        public AttachmentsAdapter AttachmentsAdapter;
        private ImageView IconHappy, IconTag, IconImage, ColoredImage;
        private AddPollAdapter AddPollAnswerAdapter;
        private ColorBoxAdapter ColorBoxAdapter;
        private NestedScrollView ScrollView;
        private View ImportPanel;
        private Button AddAnswerButton, PostPrivacyButton;
        public Button NameAlbumButton;
        private AutoLinkTextView MentionTextView;
        private string MentionText = "", PlaceText = "", FeelingText = "";
        private readonly string ActivityText = "";
        private string ListeningText = "", PlayingText = "", WatchingText = "", TravelingText = "", GifFile = "", AlbumName = "";
        private string PagePost = "", IdPost = "", PostPrivacy = "", IdColor = "";
        private string PostFeelingType = "", PostFeelingText = "";
        private readonly string PostActivityType = "";
        private string TypeDialog = "", PermissionsType = "";
        private TextSanitizer TextSanitizer;
        private EventDataObject DataEvent;
        private GroupClass DataGroup;
        private PageClass DataPage; 
        private UserDataObject DataUser;
        private VoiceRecorder VoiceRecorder;

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
                SetContentView(Resource.Layout.AddPost_Layout);

                if (CheckAccess())
                {
                    //Get Value And Set Toolbar
                    InitComponent();
                    InitToolbar();
                    SetRecyclerViewAdapters();

                    GetPrivacyPost();

                    TextSanitizer = new TextSanitizer(MentionTextView, this, "AddPost");

                    Methods.Path.Chack_MyFolder();

                    InitTransferMessageContacts();
                }
                else
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_ErrorFileSharing), ToastLength.Long)?.Show();
                    Finish();
                }
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
                TxtAddPost = FindViewById<TextView>(Resource.Id.toolbar_title);
                TxtContentPost = FindViewById<EditText>(Resource.Id.editTxtEmail);
                SlidingUpPanel = FindViewById<SlidingUpPanelLayout>(Resource.Id.sliding_layout);
                PostSectionImage = FindViewById<ImageView>(Resource.Id.postsectionimage);
                PostTypeRecyclerView = FindViewById<RecyclerView>(Resource.Id.Recyler);
                AttachmentRecyclerView = FindViewById<RecyclerView>(Resource.Id.AttachementRecyler);
                TxtUserName = FindViewById<TextView>(Resource.Id.card_name);
                IconImage = FindViewById<ImageView>(Resource.Id.ImageIcon);
                IconHappy = FindViewById<ImageView>(Resource.Id.Activtyicon);
                IconTag = FindViewById<ImageView>(Resource.Id.TagIcon);
                ScrollView = FindViewById<NestedScrollView>(Resource.Id.scroll_View);
                ColorBoxRecyclerView = FindViewById<RecyclerView>(Resource.Id.ColorboxRecyler);
                ColoredImage = FindViewById<ImageView>(Resource.Id.ColorImage);
                NameAlbumButton = FindViewById<Button>(Resource.Id.nameAlbumButton);

                IconTag.Tag = "Close";

                Methods.SetColorEditText(TxtContentPost, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                MentionTextView = FindViewById<AutoLinkTextView>(Resource.Id.MentionTextview);
                PostPrivacyButton = FindViewById<Button>(Resource.Id.cont);

                TxtContentPost.ClearFocus();
                SlidingUpPanel.SetPanelState(SlidingUpPanelLayout.PanelState.Collapsed);
                SlidingUpPanel.AddPanelSlideListener(this);
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
                    TopToolBar.Title = GetText(Resource.String.Lbl_AddPost);
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
                PostTypeRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
                MainPostAdapter = new MainPostAdapter(this);
                PostTypeRecyclerView.SetAdapter(MainPostAdapter);

                AttachmentsAdapter = new AttachmentsAdapter(this);
                AttachmentRecyclerView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false));
                AttachmentRecyclerView.SetAdapter(AttachmentsAdapter);
                AttachmentRecyclerView.NestedScrollingEnabled = false;

                switch (AppSettings.ShowColor)
                {
                    case true:
                        ColorBoxAdapter = new ColorBoxAdapter(this, ColorBoxRecyclerView);
                        ColorBoxRecyclerView.NestedScrollingEnabled = false;

                        ColorBoxRecyclerView.Visibility = ViewStates.Visible;
                        break;
                    default:
                        ColorBoxRecyclerView.Visibility = ViewStates.Invisible;
                        break;
                }

                ColorBoxRecyclerView.Visibility = ColorBoxAdapter.ColorsList.Count switch
                {
                    0 => ViewStates.Invisible,
                    _ => ColorBoxRecyclerView.Visibility
                };
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
                    {
                        AttachmentsAdapter.DeleteItemClick += AttachmentsAdapterOnDeleteItemClick;
                        PostPrivacyButton.Click += PostPrivacyButton_Click;
                        MainPostAdapter.ItemClick += MainPostAdapterOnItemClick;
                        NameAlbumButton.Click += NameAlbumButtonOnClick;
                        TxtAddPost.Click += TxtAddPostOnClick;
                        switch (AppSettings.ShowColor)
                        {
                            case true:
                                ColorBoxAdapter.ItemClick += ColorBoxAdapter_ItemClick;
                                break;
                        }
                        break;
                    }
                    default:
                    {
                        AttachmentsAdapter.DeleteItemClick -= AttachmentsAdapterOnDeleteItemClick;
                        PostPrivacyButton.Click -= PostPrivacyButton_Click;
                        MainPostAdapter.ItemClick -= MainPostAdapterOnItemClick;
                        TxtAddPost.Click -= TxtAddPostOnClick;
                        NameAlbumButton.Click -= NameAlbumButtonOnClick;
                        switch (AppSettings.ShowColor)
                        {
                            case true:
                                ColorBoxAdapter.ItemClick -= ColorBoxAdapter_ItemClick;
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

        #endregion

        #region Events

        private void AttachmentsAdapterOnDeleteItemClick(object sender, AttachmentsAdapterClickEventArgs e)
        {
            try
            {
                var position = e.Position;
                switch (position)
                {
                    case >= 0:
                    {
                        var item = AttachmentsAdapter.GetItem(position);
                        if (item != null)
                        {
                            AttachmentsAdapter.Remove(item);

                            //remove file the type
                            var listAttach = AttachmentsAdapter.AttachmentList.Where(a => a.TypeAttachment.Contains("postPhotos")).ToList();
                            switch (listAttach.Count)
                            {
                                case > 1:
                                {
                                    NameAlbumButton.Visibility = ViewStates.Visible;

                                    foreach (var attachments in listAttach)
                                        attachments.TypeAttachment = "postPhotos[]";
                                    break;
                                }
                                default:
                                {
                                    NameAlbumButton.Visibility = ViewStates.Gone;

                                    foreach (var attachments in listAttach.Where(attachments => attachments.TypeAttachment.Contains("postPhotos")).ToList())
                                    {
                                        attachments.TypeAttachment = "postPhotos";
                                    }

                                    break;
                                }
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

        private void NameAlbumButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                TypeDialog = "AddPicturesToAlbumName";

                var dialog = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                dialog.Title(GetText(Resource.String.Lbl_AddPicturesToAlbum)).TitleColorRes(Resource.Color.primary);
                dialog.Input(Resource.String.Lbl_AlbumName, 0, false, this);
                dialog.InputType(InputTypes.TextFlagImeMultiLine);
                dialog.PositiveText(GetText(Resource.String.Lbl_Submit)).OnPositive(this);
                dialog.NegativeText(GetText(Resource.String.Lbl_Cancel)).OnNegative(this);
                dialog.AlwaysCallSingleChoiceCallback();
                dialog.ItemsCallback(this).Build().Show();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Event Add post 
        private void TxtAddPostOnClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtContentPost.Text) && string.IsNullOrEmpty(MentionTextView.Text) && AttachmentsAdapter.AttachmentList.Count == 0)
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_YouCannot_PostanEmptyPost), ToastLength.Long)?.Show();
                }
                else
                {
                    if (!Methods.CheckConnectivity())
                    {
                        Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                        return;
                    }

                    string content = !string.IsNullOrEmpty(MentionText) ? TxtContentPost.Text + " " + GetText(Resource.String.Lbl_With) + " " + MentionText.Remove(MentionText.Length - 1, 1) : TxtContentPost.Text;

                    if (ListUtils.SettingsSiteList?.MaxCharacters != null)
                    {
                        int max = Convert.ToInt32(ListUtils.SettingsSiteList?.MaxCharacters);
                        if (max < content?.Length)
                        {
                            //You have exceeded the text limit, must be less than ListUtils.SettingsSiteList?.MaxCharacters
                            Toast.MakeText(this, GetString(Resource.String.Lbl_Error_MaxCharacters) + " " + ListUtils.SettingsSiteList?.MaxCharacters, ToastLength.Short)?.Show();
                            return;
                        }
                    }

                    //Show a progress
                    //AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));

                    var item = new FileUpload
                    {
                        IdPost = IdPost,
                        PagePost = PagePost,
                        Content = content,
                        PostPrivacy = PostPrivacy,
                        PostFeelingType = PostFeelingType,
                        PostFeelingText = PostFeelingText,
                        PlaceText = PlaceText,
                        AttachmentList = AttachmentsAdapter.AttachmentList,
                        AnswersList = AddPollAnswerAdapter?.AnswersList,
                        IdColor = IdColor,
                        AlbumName = AlbumName,
                    };

                    Intent intent = new Intent(this, typeof(PostService));
                    intent.SetAction(PostService.ActionPost);
                    intent.PutExtra("DataPost", JsonConvert.SerializeObject(item));
                    intent.PutExtra("PagePost", PagePost);
                    StartService(intent);

                    Finish();

                    //var (apiStatus, respond) = await ApiRequest.AddNewPost_Async(IdPost, PagePost, content, PostPrivacy, PostFeelingType, PostFeelingText, PlaceText, AttachmentsAdapter.AttachmentList, AddPollAnswerAdapter?.AnswersList, IdColor);
                    //if (apiStatus == 200)
                    //{
                    //    if (respond is AddPostObject postObject)
                    //    {
                    //        //AndHUD.Shared.Dismiss(this);
                    //        Toast.MakeText(this, GetText(Resource.String.Lbl_Post_Added), ToastLength.Short)?.Show();

                    //        // put the String to pass back into an Intent and close this activity
                    //        var resultIntent = new Intent();
                    //        if (postObject.PostData != null)
                    //        {
                    //            resultIntent?.PutExtra("itemObject", JsonConvert.SerializeObject(postObject.PostData));
                    //        }
                    //        SetResult(Result.Ok, resultIntent);

                    //        if (UserDetails.SoundControl)
                    //            Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("PopNotificationPost.mp3");

                    //        RemoveNotification();

                    //        Finish();
                    //    }
                    //}
                    //else
                    //{
                    //    Methods.DisplayReportResult(this, respond);
                    //    //Show a Error image with a message
                    //    //AndHUD.Shared.ShowError(this, GetText(Resource.String.Lbl_Post_Failed), MaskType.Clear, TimeSpan.FromSeconds(1));
                    //}

                    //AndHUD.Shared.Dismiss(this);
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                //AndHUD.Shared.ShowError(this, GetText(Resource.String.Lbl_Post_Failed), MaskType.Clear, TimeSpan.FromSeconds(1));
            }
        }

        private void MainPostAdapterOnItemClick(object sender, MainPostAdapterClickEventArgs e)
        {
            try
            {
                if (ImportPanel != null)
                    ImportPanel.Visibility = ViewStates.Gone;

                if (MainPostAdapter.PostTypeList[e.Position] != null)
                {
                    switch (MainPostAdapter.PostTypeList[e.Position].Id)
                    {
                        //Image Gallery
                        case 1:
                        {
                            PermissionsType = "Image";

                            switch ((int)Build.VERSION.SdkInt)
                            {
                                // Check if we're running on Android 5.0 or higher 
                                case < 23:
                                    OpenDialogGallery();
                                    break;
                                default:
                                {
                                    if (CheckSelfPermission(Manifest.Permission.Camera) == Permission.Granted && CheckSelfPermission(Manifest.Permission.ReadExternalStorage) == Permission.Granted
                                        && CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == Permission.Granted)
                                    {
                                        OpenDialogGallery();
                                    }
                                    else
                                    {
                                        new PermissionsController(this).RequestPermission(108);
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                        //video Gallery
                        case 2:
                            OpenDialogVideo();
                            break;
                        // Mention
                        case 3:
                            StartActivityForResult(new Intent(this, typeof(MentionActivity)), 3);
                            break;
                        // Location
                        // Check if we're running on Android 5.0 or higher
                        case 4 when (int)Build.VERSION.SdkInt < 23:
                        //Open intent Location when the request code of result is 502
                        case 4 when CheckSelfPermission(Manifest.Permission.AccessFineLocation) == Permission.Granted && CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) == Permission.Granted:
                            //Open intent Location when the request code of result is 502
                            new IntentController(this).OpenIntentLocation();
                            break;
                        case 4:
                            new PermissionsController(this).RequestPermission(105);
                            break;
                        // Feeling
                        case 5:
                            //StartActivityForResult(new Intent(this, typeof(Feelings_Activity)), 5);
                            try
                            {
                                TypeDialog = "Feelings";

                                var arrayAdapter = new List<string>();
                                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                                switch (AppSettings.ShowFeeling)
                                {
                                    case true:
                                        arrayAdapter.Add(GetText(Resource.String.Lbl_Feeling));
                                        break;
                                }
                                switch (AppSettings.ShowListening)
                                {
                                    case true:
                                        arrayAdapter.Add(GetText(Resource.String.Lbl_Listening));
                                        break;
                                }
                                switch (AppSettings.ShowPlaying)
                                {
                                    case true:
                                        arrayAdapter.Add(GetText(Resource.String.Lbl_Playing));
                                        break;
                                }
                                switch (AppSettings.ShowWatching)
                                {
                                    case true:
                                        arrayAdapter.Add(GetText(Resource.String.Lbl_Watching));
                                        break;
                                }
                                switch (AppSettings.ShowTraveling)
                                {
                                    case true:
                                        arrayAdapter.Add(GetText(Resource.String.Lbl_Traveling));
                                        break;
                                }

                                dialogList.Title(GetString(Resource.String.Lbl_What_Are_You_Doing)).TitleColorRes(Resource.Color.primary);
                                dialogList.Items(arrayAdapter);
                                dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                                dialogList.AlwaysCallSingleChoiceCallback();
                                dialogList.ItemsCallback(this).Build().Show();
                            }
                            catch (Exception exception)
                            {
                                Methods.DisplayReportResultTrack(exception);
                            }

                            break;
                        // Camera
                        case 6:
                        {
                            PermissionsType = "Camera";

                            switch ((int)Build.VERSION.SdkInt)
                            {
                                // Check if we're running on Android 5.0 or higher
                                case < 23:
                                    //requestCode >> 503 => Camera
                                    new IntentController(this).OpenIntentCamera();
                                    break;
                                default:
                                {
                                    if (CheckSelfPermission(Manifest.Permission.Camera) == Permission.Granted && CheckSelfPermission(Manifest.Permission.ReadExternalStorage) == Permission.Granted
                                        && CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == Permission.Granted)
                                    {
                                        //requestCode >> 503 => Camera
                                        new IntentController(this).OpenIntentCamera();
                                    }
                                    else
                                    {
                                        new PermissionsController(this).RequestPermission(108);
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                        // Gif
                        case 7:
                            StartActivityForResult(new Intent(this, typeof(GifActivity)), 7);
                            break;
                        // File
                        case 8:
                        {
                            PermissionsType = "File";

                            switch ((int)Build.VERSION.SdkInt)
                            {
                                // Check if we're running on Android 5.0 or higher
                                case < 23:
                                    //requestCode >> 504 => File
                                    new IntentController(this).OpenIntentFile(GetText(Resource.String.Lbl_SelectFile));
                                    break;
                                default:
                                {
                                    if (CheckSelfPermission(Manifest.Permission.ReadExternalStorage) == Permission.Granted &&
                                        CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == Permission.Granted)
                                    {
                                        //requestCode >> 504 => File
                                        new IntentController(this).OpenIntentFile(GetText(Resource.String.Lbl_SelectFile));
                                    }
                                    else
                                    {
                                        new PermissionsController(this).RequestPermission(108);
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                        // Music
                        case 9:
                        {
                            PermissionsType = "Music";

                            switch ((int)Build.VERSION.SdkInt)
                            {
                                // Check if we're running on Android 5.0 or higher
                                case < 23:
                                    new IntentController(this).OpenIntentAudio(); //505
                                    break;
                                default:
                                {
                                    if (CheckSelfPermission(Manifest.Permission.ReadExternalStorage) == Permission.Granted && CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == Permission.Granted)
                                        new IntentController(this).OpenIntentAudio(); //505
                                    else
                                        new PermissionsController(this).RequestPermission(100);
                                    break;
                                }
                            }

                            break;
                        }
                        // VoiceRecorder
                        case 10:
                        {
                            PermissionsType = "Music";

                            switch ((int)Build.VERSION.SdkInt)
                            {
                                // Check if we're running on Android 5.0 or higher
                                case < 23:
                                    VoiceRecorder = new VoiceRecorder();
                                    VoiceRecorder.Show(SupportFragmentManager, VoiceRecorder.Tag);
                                    break;
                                default:
                                {
                                    if (CheckSelfPermission(Manifest.Permission.ReadExternalStorage) == Permission.Granted && CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == Permission.Granted)
                                    {
                                        VoiceRecorder = new VoiceRecorder();
                                        VoiceRecorder.Show(SupportFragmentManager, VoiceRecorder.Tag);
                                    }
                                    else
                                        new PermissionsController(this).RequestPermission(102);

                                    break;
                                }
                            }

                            break;
                        }
                        // Polls
                        case 11:
                        {
                            if (ColoredImage.Visibility != ViewStates.Gone)
                            {
                                ColoredImage.Visibility = ViewStates.Gone;

                                TxtContentPost.SetTextColor(new Color(ContextCompat.GetColor(TxtContentPost.Context, Resource.Color.textDark_color)));
                                TxtContentPost.SetHintTextColor(new Color(ContextCompat.GetColor(TxtContentPost.Context, Resource.Color.textDark_color)));
                            }

                            TxtContentPost.ClearFocus();
                            SlidingUpPanel.SetPanelState(SlidingUpPanelLayout.PanelState.Collapsed);

                            ImportPanel ??= FindViewById<ViewStub>(Resource.Id.stub_import)?.Inflate();
                            if (ImportPanel != null)
                            {
                                ImportPanel.Visibility = ViewStates.Visible;

                                PollRecyclerView ??= (RecyclerView)ImportPanel.FindViewById(Resource.Id.Recyler);
                                AddAnswerButton = (Button)ImportPanel.FindViewById(Resource.Id.addanswer);

                                AttachmentsAdapter?.AttachmentList.Clear();
                                AddPollAnswerAdapter = new AddPollAdapter(this);
                                PollRecyclerView?.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Vertical, false));
                                PollRecyclerView?.SetAdapter(AddPollAnswerAdapter);
                                AddPollAnswerAdapter.AnswersList.Add(new PollAnswers { Answer = GetText(Resource.String.Lbl2_Polls) + " 1", Id = 1 });
                                AddPollAnswerAdapter.AnswersList.Add(new PollAnswers { Answer = GetText(Resource.String.Lbl2_Polls) + " 2", Id = 2 });
                                AddPollAnswerAdapter.NotifyDataSetChanged();


                                switch (AddAnswerButton.HasOnClickListeners)
                                {
                                    case false:
                                        AddAnswerButton.Click += AddAnswerButtonOnClick;
                                        break;
                                }

                                PollRecyclerView.NestedScrollingEnabled = false;
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void ColorBoxAdapter_ItemClick(object sender, ColorBoxAdapterClickEventArgs e)
        {
            try
            {
                var item = ColorBoxAdapter.ColorsList[e.Position];
                switch (item)
                {
                    case null:
                        return;
                }

                switch (AttachmentsAdapter.AttachmentList.Count)
                {
                    case > 0:
                        AttachmentsAdapter.AttachmentList.Clear();
                        AttachmentsAdapter.NotifyDataSetChanged();
                        break;
                }

                IdColor = item.Id.ToString();
                switch (item.Color1)
                {
                    case "#ffffff" when item.Color2 == "#efefef":
                        ColoredImage.Visibility = ViewStates.Gone;

                        TxtContentPost.SetTextColor(new Color(ContextCompat.GetColor(TxtContentPost.Context, Resource.Color.textDark_color)));
                        TxtContentPost.SetHintTextColor(new Color(ContextCompat.GetColor(TxtContentPost.Context, Resource.Color.textDark_color)));

                        return;
                }

                ColoredImage.Visibility = ViewStates.Visible;
                switch (string.IsNullOrEmpty(item.Image))
                {
                    case false:
                        Glide.With(this).Load(item.Image).Apply(new RequestOptions()).Into(ColoredImage);
                        //GlideImageLoader.LoadImage(this, item.Image, ColoredImage, ImageStyle.FitCenter, ImagePlaceholders.Color, false);
                        break;
                    default:
                    {
                        var colorsList = new List<int>();

                        switch (string.IsNullOrEmpty(item.Color1))
                        {
                            case false:
                                colorsList.Add(Color.ParseColor(item.Color1));
                                break;
                        }

                        switch (string.IsNullOrEmpty(item.Color2))
                        {
                            case false:
                                colorsList.Add(Color.ParseColor(item.Color2));
                                break;
                        }

                        GradientDrawable gd = new GradientDrawable(GradientDrawable.Orientation.TopBottom, colorsList.ToArray());
                        gd.SetCornerRadius(0f);
                        ColoredImage.Background = gd;
                        break;
                    }
                }

                switch (string.IsNullOrEmpty(item.TextColor))
                {
                    case false:
                        TxtContentPost.SetTextColor(Color.ParseColor(item.TextColor));
                        TxtContentPost.SetHintTextColor(Color.ParseColor(item.TextColor));
                        break;
                }

                var layoutparams = (RelativeLayout.LayoutParams)TxtContentPost.LayoutParameters;
                layoutparams.AddRule(LayoutRules.CenterInParent);
                TxtContentPost.LayoutParameters = layoutparams;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Permissions && Result

        private Uri UriData;

        //Result
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);

                SlidingUpPanel.SetPanelState(SlidingUpPanelLayout.PanelState.Collapsed);

                if (ColoredImage.Visibility != ViewStates.Gone)
                {
                    ColoredImage.Visibility = ViewStates.Gone;

                    TxtContentPost.SetTextColor(new Color(ContextCompat.GetColor(TxtContentPost.Context, Resource.Color.textDark_color)));
                    TxtContentPost.SetHintTextColor(new Color(ContextCompat.GetColor(TxtContentPost.Context, Resource.Color.textDark_color)));
                }

                switch (requestCode)
                {
                    // Add image 
                    case 500 when resultCode == Result.Ok:
                    {
                        if (data.ClipData != null)
                        {
                            var mClipData = data.ClipData;
                            for (var i = 0; i < mClipData.ItemCount; i++)
                            {
                                var item = mClipData.GetItemAt(i);
                                Uri uri = item.Uri;
                                var filepath = Methods.AttachmentFiles.GetActualPathFromFile(this, uri);
                                PickiTonCompleteListener(filepath);
                            }
                        }
                        else
                        {
                            Uri uri = data.Data;
                            var filepath = Methods.AttachmentFiles.GetActualPathFromFile(this, uri);
                            PickiTonCompleteListener(filepath);
                        }

                        break;
                    }
                    // Add video 
                    case 501 when resultCode == Result.Ok:
                    {
                        NameAlbumButton.Visibility = ViewStates.Gone;

                        AttachmentsAdapter.RemoveAll();

                        UriData = data.Data;
                        //PickiT.GetPath(uriData, (int)Build.VERSION.SdkInt);

                        var filepath = Methods.AttachmentFiles.GetActualPathFromFile(this, data.Data);
                        if (filepath != null)
                        {
                            var type = Methods.AttachmentFiles.Check_FileExtension(filepath);
                            switch (type)
                            {
                                case "Video":
                                {
                                    var fileName = filepath.Split('/').Last();
                                    var fileNameWithoutExtension = fileName.Split('.').First();
                                    var pathWithoutFilename = Methods.Path.FolderDcimImage;
                                    var fullPathFile = new File(Methods.Path.FolderDcimImage, fileNameWithoutExtension + ".png");

                                    var videoPlaceHolderImage = Methods.MultiMedia.GetMediaFrom_Gallery(pathWithoutFilename, fileNameWithoutExtension + ".png");
                                    switch (videoPlaceHolderImage)
                                    {
                                        case "File Dont Exists":
                                        {
                                            var bitmapImage = Methods.MultiMedia.Retrieve_VideoFrame_AsBitmap(this, data.Data.ToString());
                                            Methods.MultiMedia.Export_Bitmap_As_Image(bitmapImage, fileNameWithoutExtension, pathWithoutFilename);
                                            break;
                                        }
                                    }

                                    //remove file the type
                                    var imageAttach = AttachmentsAdapter.AttachmentList.Where(a => a.TypeAttachment != "postVideo").ToList();
                                    switch (imageAttach.Count)
                                    {
                                        case > 0:
                                        {
                                            foreach (var image in imageAttach)
                                                AttachmentsAdapter.Remove(image);
                                            break;
                                        }
                                    }
                             
                                    var attach = new Attachments
                                    {
                                        Id = AttachmentsAdapter.AttachmentList.Count + 1,
                                        TypeAttachment = "postVideo",
                                        FileSimple = fullPathFile.AbsolutePath,
                                        Thumb = new Attachments.VideoThumb
                                        {
                                            FileUrl = fullPathFile.AbsolutePath
                                        },
                                        FileUrl = filepath
                                    };

                                    AttachmentsAdapter.Add(attach);
                                    break;
                                }
                            }
                        }

                        break;
                    }
                    // Add video Camera 
                    case 513 when resultCode == Result.Ok:
                    {
                        NameAlbumButton.Visibility = ViewStates.Gone;

                        AttachmentsAdapter.RemoveAll();

                        var filepath = Methods.AttachmentFiles.GetActualPathFromFile(this, data.Data);
                        if (filepath != null)
                        {
                            var type = Methods.AttachmentFiles.Check_FileExtension(filepath);
                            switch (type)
                            {
                                case "Video":
                                {
                                    var fileName = filepath.Split('/').Last();
                                    var fileNameWithoutExtension = fileName.Split('.').First();
                                    var pathWithoutFilename = Methods.Path.FolderDcimImage;
                                    var fullPathFile = new File(Methods.Path.FolderDcimImage, fileNameWithoutExtension + ".png");

                                    var videoPlaceHolderImage = Methods.MultiMedia.GetMediaFrom_Gallery(pathWithoutFilename, fileNameWithoutExtension + ".png");
                                    switch (videoPlaceHolderImage)
                                    {
                                        case "File Dont Exists":
                                        {
                                            var bitmapImage = Methods.MultiMedia.Retrieve_VideoFrame_AsBitmap(this, data.Data.ToString());
                                            Methods.MultiMedia.Export_Bitmap_As_Image(bitmapImage, fileNameWithoutExtension, pathWithoutFilename);
                                            break;
                                        }
                                    }

                                    //remove file the type
                                    var imageAttach = AttachmentsAdapter.AttachmentList.Where(a => a.TypeAttachment != "postVideo").ToList();
                                    switch (imageAttach.Count)
                                    {
                                        case > 0:
                                        {
                                            foreach (var image in imageAttach)
                                                AttachmentsAdapter.Remove(image);
                                            break;
                                        }
                                    }

                                    var attach = new Attachments
                                    {
                                        Id = AttachmentsAdapter.AttachmentList.Count + 1,
                                        TypeAttachment = "postVideo",
                                        FileSimple = fullPathFile.AbsolutePath,
                                        Thumb = new Attachments.VideoThumb
                                        {
                                            FileUrl = fullPathFile.AbsolutePath
                                        },
                                        FileUrl = filepath
                                    };

                                    AttachmentsAdapter.Add(attach);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            UriData = data.Data;
                            var filepath2 = Methods.AttachmentFiles.GetActualPathFromFile(this, UriData);
                            PickiTonCompleteListener(filepath2);
                        }

                        break;
                    }
                    // Mention
                    case 3 when resultCode == Result.Ok:
                        try
                        {
                            var dataUser = MentionActivity.MAdapter.MentionList.Where(a => a.Selected).ToList();
                            switch (dataUser.Count)
                            {
                                case > 0:
                                {
                                    foreach (var item in dataUser) MentionText += " @" + item.Username + " ,";

                                    TextSanitizer.Load(LoadPostStrings());
                                    break;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Methods.DisplayReportResultTrack(e);
                        }

                        break;
                    // Location
                    case 502 when resultCode == Result.Ok:
                    {
                        var placeAddress = data.GetStringExtra("Address") ?? "";
                        switch (string.IsNullOrEmpty(placeAddress))
                        {
                            //var placeLatLng = data.GetStringExtra("latLng") ?? "";
                            case false:
                            {
                                PlaceText = string.IsNullOrEmpty(PlaceText) switch
                                {
                                    false => string.Empty,
                                    _ => PlaceText
                                };

                                PlaceText = " /" + placeAddress;
                                TextSanitizer.Load(LoadPostStrings());
                                break;
                            }
                        }

                        break;
                    }
                    // Feeling
                    case 5 when resultCode == Result.Ok:
                    {
                        var feelings = data.GetStringExtra("FeelingName") ?? "Data not available";
                        var feelingsDisplayText = data.GetStringExtra("Feelings") ?? "Data not available";
                        if (feelings != "Data not available" && !string.IsNullOrEmpty(feelings))
                        {
                            FeelingText = feelingsDisplayText; //This Will be displayed And translated
                            PostFeelingType = "feelings"; //Type Of feeling
                            PostFeelingText = feelings.ToLower(); //This will be send via API
                            TextSanitizer.Load(LoadPostStrings());
                        }

                        break;
                    }
                    // Add image using camera
                    case 503 when resultCode == Result.Ok:
                    {
                        //remove file the type
                        var videoAttach = AttachmentsAdapter.AttachmentList.Where(a => !a.TypeAttachment.Contains("postPhotos")).ToList();
                        switch (videoAttach.Count)
                        {
                            case > 0:
                            {
                                foreach (var video in videoAttach)
                                    AttachmentsAdapter.Remove(video);
                                break;
                            }
                        }

                        if (string.IsNullOrEmpty(IntentController.CurrentPhotoPath))
                        {
                            Uri uri = data.Data;
                            var filepath = Methods.AttachmentFiles.GetActualPathFromFile(this, uri);
                            PickiTonCompleteListener(filepath);
                        }
                        else
                        {
                            if (Methods.MultiMedia.CheckFileIfExits(IntentController.CurrentPhotoPath) != "File Dont Exists")
                            {
                                var attach = new Attachments
                                {
                                    Id = AttachmentsAdapter.AttachmentList.Count + 1,
                                    TypeAttachment = "postPhotos",
                                    FileSimple = IntentController.CurrentPhotoPath,
                                    FileUrl = IntentController.CurrentPhotoPath
                                };

                                AttachmentsAdapter.Add(attach);

                                switch (AttachmentsAdapter.AttachmentList.Count)
                                {
                                    case > 1:
                                    {
                                        NameAlbumButton.Visibility = ViewStates.Visible;

                                        foreach (var item in AttachmentsAdapter.AttachmentList)
                                            item.TypeAttachment = "postPhotos[]";
                                        break;
                                    }
                                    default:
                                    {
                                        NameAlbumButton.Visibility = ViewStates.Gone;

                                        foreach (var item in AttachmentsAdapter.AttachmentList)
                                            item.TypeAttachment = "postPhotos";
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                //Toast.MakeText(this, GetText(Resource.String.Lbl_Failed_to_load), ToastLength.Short)?.Show();
                            }
                        }

                        break;
                    }
                    // Gif
                    case 7 when resultCode == Result.Ok:
                    {
                        var giflink = data.GetStringExtra("gif") ?? "Data not available";
                        if (giflink != "Data not available" && !string.IsNullOrEmpty(giflink))
                        {
                            GifFile = giflink;

                            //remove file the type
                            AttachmentsAdapter.RemoveAll();

                            var attach = new Attachments
                            {
                                Id = AttachmentsAdapter.AttachmentList.Count + 1,
                                TypeAttachment = "postPhotos",
                                FileSimple = GifFile,
                                FileUrl = GifFile
                            };

                            AttachmentsAdapter.Add(attach);
                        }

                        break;
                    }
                    // File
                    case 504 when resultCode == Result.Ok:
                    {
                        Uri uri = data.Data;
                        var filepath = Methods.AttachmentFiles.GetActualPathFromFile(this, uri);
                        PickiTonCompleteListener(filepath);
                        break;
                    }
                    // Music
                    case 505 when resultCode == Result.Ok:
                    {
                        Uri uri = data.Data;
                        var filepath = Methods.AttachmentFiles.GetActualPathFromFile(this, uri);
                        PickiTonCompleteListener(filepath);
                        break;
                    }
                    // Add image 
                    case CropImage.CropImageActivityRequestCode when resultCode == Result.Ok:
                    {
                        var videoAttach = AttachmentsAdapter.AttachmentList.Where(a => !a.TypeAttachment.Contains("postPhotos")).ToList();

                        switch (videoAttach.Count)
                        {
                            case > 0:
                            {
                                foreach (var video in videoAttach)
                                    AttachmentsAdapter.Remove(video);
                                break;
                            }
                        }

                        var result = CropImage.GetActivityResult(data);

                        switch (result.IsSuccessful)
                        {
                            case true:
                            {
                                var resultUri = result.Uri;

                                switch (string.IsNullOrEmpty(resultUri.Path))
                                {
                                    case false:
                                    {
                                        var attach = new Attachments
                                        {
                                            Id = AttachmentsAdapter.AttachmentList.Count + 1,
                                            TypeAttachment = "postPhotos",
                                            FileSimple = resultUri.Path,
                                            FileUrl = resultUri.Path
                                        };

                                        AttachmentsAdapter.Add(attach);

                                        switch (AttachmentsAdapter.AttachmentList.Count)
                                        {
                                            case > 1:
                                            {
                                                NameAlbumButton.Visibility = ViewStates.Visible;

                                                foreach (var item in AttachmentsAdapter.AttachmentList)
                                                {
                                                    item.TypeAttachment = "postPhotos[]";
                                                }

                                                break;
                                            }
                                            default:
                                            {
                                                NameAlbumButton.Visibility = ViewStates.Gone;

                                                foreach (var item in AttachmentsAdapter.AttachmentList)
                                                    item.TypeAttachment = "postPhotos";
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
                        }

                        break;
                    }
                }

                TxtContentPost.ClearFocus();
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
                        switch (PermissionsType)
                        {
                            //requestCode >> 500 => Image Gallery
                            case "Image" when AppSettings.ImageCropping:
                                OpenDialogGallery();
                                break;
                            case "VideoGallery":
                                //requestCode >> 501 => video Gallery
                                new IntentController(this).OpenIntentVideoGallery();
                                break;
                            case "VideoCamera":
                                //requestCode >> 513 => video Camera
                                new IntentController(this).OpenIntentVideoCamera();
                                break;
                            case "Camera":
                                //requestCode >> 503 => Camera
                                new IntentController(this).OpenIntentCamera();
                                break;
                            case "File":
                                //requestCode >> 504 => File
                                new IntentController(this).OpenIntentFile(GetText(Resource.String.Lbl_SelectFile));
                                break;
                            case "Music":
                                //requestCode >> 505 => Music
                                new IntentController(this).OpenIntentAudio();
                                break;
                        }

                        break;
                    case 108:
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Permission_is_denied), ToastLength.Long)?.Show();
                        break;
                    case 105 when grantResults.Length > 0 && grantResults[0] == Permission.Granted:
                        //Open intent Location when the request code of result is 502
                        new IntentController(this).OpenIntentLocation();
                        break;
                    case 105:
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Permission_is_denied), ToastLength.Long)?.Show();
                        break;
                    case 102 when grantResults.Length > 0 && grantResults[0] == Permission.Granted:
                        VoiceRecorder = new VoiceRecorder();
                        VoiceRecorder.Show(SupportFragmentManager, VoiceRecorder.Tag);
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

        #region Panel Item Post

        public void OnPanelClosed(View panel)
        {
            try
            {
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnPanelOpened(View panel)
        {
            try
            {
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        void SlidingPaneLayout.IPanelSlideListener.OnPanelSlide(View panel, float slideOffset)
        {
            try
            {
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnPanelStateChanged(View p0, SlidingUpPanelLayout.PanelState p1, SlidingUpPanelLayout.PanelState p2)
        {
            try
            {
                if (p1 == SlidingUpPanelLayout.PanelState.Expanded && p2 == SlidingUpPanelLayout.PanelState.Dragging)
                {
                    switch (IconTag?.Tag?.ToString())
                    {
                        case "Open":
                            IconTag.SetImageResource(Resource.Drawable.icon_mention_contact_vector);
                            IconTag.Tag = "Close";
                            IconImage.Visibility = ViewStates.Visible;
                            IconHappy.Visibility = ViewStates.Visible;
                            break;
                    }
                }
                else if (p1 == SlidingUpPanelLayout.PanelState.Collapsed && p2 == SlidingUpPanelLayout.PanelState.Dragging)
                {
                    switch (IconTag?.Tag?.ToString())
                    {
                        case "Close":
                            IconTag.SetImageResource(Resource.Drawable.ic_action_arrow_down_sign);
                            IconTag.Tag = "Open";
                            IconImage.Visibility = ViewStates.Invisible;
                            IconHappy.Visibility = ViewStates.Invisible;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        void SlidingUpPanelLayout.IPanelSlideListener.OnPanelSlide(View p0, float p1)
        {
            try
            {
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Privacy

        private void LoadDataUser()
        {
            try
            {
                if (DataUser != null)
                {
                    GlideImageLoader.LoadImage(this, DataUser.Avatar, PostSectionImage, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);

                    TxtUserName.Text = WoWonderTools.GetNameFinal(DataUser);

                    PostPrivacyButton.Text = GetString(Resource.String.Lbl_Everyone);

                    //if (dataUser.post_privacy.Contains("0"))
                    //    PostPrivacyButton.Text = GetString(Resource.String.Lbl_Everyone);
                    //else if (dataUser.post_privacy.Contains("ifollow"))
                    //    PostPrivacyButton.Text = GetString(Resource.String.Lbl_People_i_Follow);
                    //else if (dataUser.post_privacy.Contains("me"))
                    //    PostPrivacyButton.Text = GetString(Resource.String.Lbl_People_Follow_Me);
                    //else
                    //    PostPrivacyButton.Text = GetString(Resource.String.Lbl_No_body);

                    PostPrivacy = "0";
                }
                else
                {
                    TxtUserName.Text = UserDetails.Username;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void GetPrivacyPost()
        {
            try
            {
                DataUser = ListUtils.MyProfileList?.FirstOrDefault();
                switch (PagePost)
                {
                    case "Normal":
                    case "Normal_More":
                    case "Normal_Gallery":
                        LoadDataUser();

                        switch (PagePost)
                        {
                            case "Normal_More":
                                SlidingUpPanel.SetPanelState(SlidingUpPanelLayout.PanelState.Expanded);
                                break;
                            case "Normal_Gallery":
                            {
                                PermissionsType = "Image";

                                OpenDialogGallery(); //requestCode >> 500 => Image Gallery
                                break;
                            }
                        }

                        break;
                    case "SocialGroup":
                    case "SocialGroup_More":
                    case "SocialGroup_Gallery":
                    {
                        DataGroup = JsonConvert.DeserializeObject<GroupClass>(Intent?.GetStringExtra("itemObject") ?? "");
                        if (DataGroup != null)
                        {
                            PostPrivacyButton.SetBackgroundResource(0);
                            PostPrivacyButton.Enabled = false;
                            PostPrivacyButton.Text = GetText(Resource.String.Lbl_PostingAs) + " " + WoWonderTools.GetNameFinal(DataUser);

                            GlideImageLoader.LoadImage(this, DataGroup.Avatar, PostSectionImage, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                            TxtUserName.Text = DataGroup.GroupName;
                        }
                        else
                        {
                            LoadDataUser();
                        }

                        switch (PagePost)
                        {
                            case "SocialGroup_More":
                                SlidingUpPanel.SetPanelState(SlidingUpPanelLayout.PanelState.Expanded);
                                break;
                            case "SocialGroup_Gallery":
                            {
                                PermissionsType = "Image";

                                OpenDialogGallery(); //requestCode >> 500 => Image Gallery
                                break;
                            }
                        }

                        break;
                    }
                    case "SocialPage":
                    case "SocialPage_More":
                    case "SocialPage_Gallery":
                    {
                        DataPage = JsonConvert.DeserializeObject<PageClass>(Intent?.GetStringExtra("itemObject") ?? "");
                        if (DataPage != null)
                        {
                            PostPrivacyButton.SetBackgroundResource(0);
                            PostPrivacyButton.Enabled = false;
                            PostPrivacyButton.Text = GetText(Resource.String.Lbl_PostingAs) + " " + WoWonderTools.GetNameFinal(DataUser);

                            GlideImageLoader.LoadImage(this, DataPage.Avatar, PostSectionImage, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                            TxtUserName.Text = DataPage.PageName;
                        }
                        else
                        {
                            LoadDataUser();
                        }

                        switch (PagePost)
                        {
                            case "SocialPage_More":
                                SlidingUpPanel.SetPanelState(SlidingUpPanelLayout.PanelState.Expanded);
                                break;
                            case "SocialPage_Gallery":
                            {
                                PermissionsType = "Image";

                                OpenDialogGallery(); //requestCode >> 500 => Image Gallery
                                break;
                            }
                        }

                        break;
                    }
                    case "SocialEvent":
                    case "SocialEvent_More":
                    case "SocialEvent_Gallery":
                    {
                        DataEvent = JsonConvert.DeserializeObject<EventDataObject>(Intent?.GetStringExtra("itemObject") ?? "");
                        if (DataEvent != null)
                        {
                            PostPrivacyButton.SetBackgroundResource(0);
                            PostPrivacyButton.Enabled = false;
                            PostPrivacyButton.Text = GetText(Resource.String.Lbl_PostingAs) + " " + WoWonderTools.GetNameFinal(DataUser);

                            GlideImageLoader.LoadImage(this, DataEvent.Cover, PostSectionImage, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                            TxtUserName.Text = DataEvent.Name;
                        }
                        else
                        {
                            LoadDataUser();
                        }

                        switch (PagePost)
                        {
                            case "SocialEvent_More":
                                SlidingUpPanel.SetPanelState(SlidingUpPanelLayout.PanelState.Expanded);
                                break;
                            case "SocialEvent_Gallery":
                            {
                                PermissionsType = "Image";

                                OpenDialogGallery(); //requestCode >> 500 => Image Gallery
                                break;
                            }
                        }

                        break;
                    }
                    default:
                        LoadDataUser();
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void PostPrivacyButton_Click(object sender, EventArgs e)
        {
            try
            {
                TypeDialog = "PostPrivacy";

                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                arrayAdapter.Add(GetString(Resource.String.Lbl_Everyone));// > 0

                switch (AppSettings.ConnectivitySystem)
                {
                    case 1:
                        arrayAdapter.Add(GetString(Resource.String.Lbl_People_i_Follow));// > 1
                        arrayAdapter.Add(GetText(Resource.String.Lbl_People_Follow_Me));// > 2 
                        break;
                    default:
                        arrayAdapter.Add(GetString(Resource.String.Lbl_MyFriends)); // > 1 
                        break;
                }
                arrayAdapter.Add(GetString(Resource.String.Lbl_No_body)); // > 3
                arrayAdapter.Add(GetText(Resource.String.Lbl_Anonymous)); // > 4

                dialogList.Title(GetText(Resource.String.Lbl_PostPrivacy)).TitleColorRes(Resource.Color.primary);
                dialogList.Items(arrayAdapter);
                dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                dialogList.ItemsCallback(this).Build().Show();
                dialogList.AlwaysCallSingleChoiceCallback();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void AddAnswerButtonOnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                switch (AddPollAnswerAdapter.AnswersList.Count)
                {
                    case < 8:
                        AddPollAnswerAdapter.AnswersList.Add(new PollAnswers { Answer = "", Id = AddPollAnswerAdapter.AnswersList.Count });
                        AddPollAnswerAdapter.NotifyItemInserted(AddPollAnswerAdapter.AnswersList.Count);
                        PollRecyclerView.ScrollToPosition(AddPollAnswerAdapter.AnswersList.Count);
                        ScrollView.ScrollTo(0, ScrollView.Bottom + 500);
                        ScrollView.SmoothScrollTo(0, ScrollView.Bottom + 200);
                        break;
                    default:
                        Toast.MakeText(this, GetText(Resource.String.Lbl2_PollsLimitError), ToastLength.Long)?.Show();
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

        public void OnSelection(MaterialDialog p0, View p1, int itemid, ICharSequence itemString)
        {
            try
            {
                switch (TypeDialog)
                {
                    case "PostPrivacy":
                    {
                        PostPrivacyButton.Text = itemString.ToString();

                        if (itemString.ToString() == GetString(Resource.String.Lbl_Everyone))
                            PostPrivacy = "0";
                        else if (itemString.ToString() == GetString(Resource.String.Lbl_People_i_Follow) || itemString.ToString() == GetString(Resource.String.Lbl_MyFriends))
                            PostPrivacy = "1";
                        else if (itemString.ToString() == GetString(Resource.String.Lbl_People_Follow_Me))
                            PostPrivacy = "2";
                        else if (itemString.ToString() == GetString(Resource.String.Lbl_No_body))
                            PostPrivacy = "3";
                        else if (itemString.ToString() == GetString(Resource.String.Lbl_Anonymous))
                            PostPrivacy = "4";
                        else
                            PostPrivacy = "0";
                        break;
                    }
                    case "PostVideos" when itemString.ToString() == GetText(Resource.String.Lbl_VideoGallery):
                    {
                        PermissionsType = "VideoGallery";
                        switch ((int)Build.VERSION.SdkInt)
                        {
                            // Check if we're running on Android 5.0 or higher
                            case < 23:
                                //requestCode >> 501 => video Gallery
                                new IntentController(this).OpenIntentVideoGallery();
                                break;
                            default:
                            {
                                if (CheckSelfPermission(Manifest.Permission.Camera) == Permission.Granted
                                    && CheckSelfPermission(Manifest.Permission.ReadExternalStorage) == Permission.Granted
                                    && CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == Permission.Granted)
                                {
                                    //requestCode >> 501 => video Gallery
                                    new IntentController(this).OpenIntentVideoGallery();
                                }
                                else
                                {
                                    new PermissionsController(this).RequestPermission(108);
                                }

                                break;
                            }
                        }

                        break;
                    }
                    case "PostVideos":
                    {
                        if (itemString.ToString() == GetText(Resource.String.Lbl_RecordVideoFromCamera))
                        {
                            PermissionsType = "VideoCamera";

                            switch ((int)Build.VERSION.SdkInt)
                            {
                                // Check if we're running on Android 5.0 or higher
                                case < 23:
                                    //requestCode >> 513 => video Camera
                                    new IntentController(this).OpenIntentVideoCamera();
                                    break;
                                default:
                                {
                                    if (CheckSelfPermission(Manifest.Permission.Camera) == Permission.Granted
                                        && CheckSelfPermission(Manifest.Permission.ReadExternalStorage) == Permission.Granted
                                        && CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == Permission.Granted)
                                    {
                                        //requestCode >> 513 => video Camera
                                        new IntentController(this).OpenIntentVideoCamera();
                                    }
                                    else
                                    {
                                        new PermissionsController(this).RequestPermission(108);
                                    }

                                    break;
                                }
                            }
                        }

                        break;
                    }
                    // Feelings
                    case "Feelings" when itemid == 0:
                        StartActivityForResult(new Intent(this, typeof(FeelingsActivity)), 5);
                        break;
                    //Listening
                    case "Feelings" when itemid == 1:
                    {
                        TypeDialog = "Listening";

                        var dialog = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                        dialog.Title(Resource.String.Lbl_Listening).TitleColorRes(Resource.Color.primary);
                        dialog.Input(Resource.String.Lbl_Comment_Hint_Listening, 0, false, this);
                        dialog.InputType(InputTypes.TextFlagImeMultiLine);
                        dialog.PositiveText(GetText(Resource.String.Lbl_Submit)).OnPositive(this);
                        dialog.NegativeText(GetText(Resource.String.Lbl_Cancel)).OnNegative(this);
                        dialog.AlwaysCallSingleChoiceCallback();
                        dialog.Build().Show();
                        break;
                    }
                    //Playing
                    case "Feelings" when itemid == 2:
                    {
                        TypeDialog = "Playing";

                        var dialog = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                        dialog.Title(Resource.String.Lbl_Playing).TitleColorRes(Resource.Color.primary);
                        dialog.Input(Resource.String.Lbl_Comment_Hint_Playing, 0, false, this);
                        dialog.InputType(InputTypes.TextFlagImeMultiLine);
                        dialog.PositiveText(GetText(Resource.String.Lbl_Submit)).OnPositive(this);
                        dialog.NegativeText(GetText(Resource.String.Lbl_Cancel)).OnNegative(this);
                        dialog.AlwaysCallSingleChoiceCallback();
                        dialog.Build().Show();
                        break;
                    }
                    //Watching
                    case "Feelings" when itemid == 3:
                    {
                        TypeDialog = "Watching";

                        var dialog = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                        dialog.Title(Resource.String.Lbl_Watching).TitleColorRes(Resource.Color.primary);
                        dialog.Input(Resource.String.Lbl_Comment_Hint_Watching, 0, false, this);
                        dialog.InputType(InputTypes.TextFlagImeMultiLine);
                        dialog.PositiveText(GetText(Resource.String.Lbl_Submit)).OnPositive(this);
                        dialog.NegativeText(GetText(Resource.String.Lbl_Cancel)).OnNegative(this);
                        dialog.AlwaysCallSingleChoiceCallback();
                        dialog.Build().Show();
                        break;
                    }
                    case "Feelings":
                    {
                        switch (itemid)
                        {
                            //Traveling
                            case 4:
                            {
                                TypeDialog = "Traveling";

                                var dialog = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                                dialog.Title(Resource.String.Lbl_Traveling).TitleColorRes(Resource.Color.primary);
                                dialog.Input(Resource.String.Lbl_Comment_Hint_Traveling, 0, false, this);
                                dialog.InputType(InputTypes.TextFlagImeMultiLine);
                                dialog.PositiveText(GetText(Resource.String.Lbl_Submit)).OnPositive(this);
                                dialog.NegativeText(GetText(Resource.String.Lbl_Cancel)).OnNegative(this);
                                dialog.AlwaysCallSingleChoiceCallback();
                                dialog.Build().Show();
                                break;
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

        public void OnClick(MaterialDialog p0, DialogAction p1)
        {
            try
            {
                switch (TypeDialog)
                {
                    case "PostPrivacy":
                    {
                        if (p1 == DialogAction.Positive) p0.Dismiss();
                        break;
                    }
                    case "PostBack" when p1 == DialogAction.Positive:
                        p0.Dismiss();


                        Finish();
                        break;
                    case "PostBack":
                    {
                        if (p1 == DialogAction.Negative)
                        {
                            p0.Dismiss();
                        }

                        break;
                    }
                    default:
                    {
                        if (p1 == DialogAction.Positive)
                        {
                        }
                        else if (p1 == DialogAction.Negative)
                        {
                            p0.Dismiss();
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

        public void OnInput(MaterialDialog p0, ICharSequence p1)
        {
            try
            {
                switch (TypeDialog)
                {
                    case "AddPicturesToAlbumName":
                    {
                        if (p1.Length() > 0)
                        {
                            var strName = p1.ToString();
                            AlbumName = strName;
                            NameAlbumButton.Text = Methods.FunString.SubStringCutOf(strName, 30);
                        }

                        break;
                    }
                    case "Listening":
                    {
                        if (p1.Length() > 0)
                        {
                            var strName = p1.ToString();
                            ListeningText = strName;
                            PostFeelingText = strName;
                            PostFeelingType = "listening"; //Type Of listening
                        }

                        break;
                    }
                    case "Playing":
                    {
                        if (p1.Length() > 0)
                        {
                            var strName = p1.ToString();
                            PlayingText = strName;
                            PostFeelingText = strName;
                            PostFeelingType = "playing"; //Type Of playing
                        }

                        break;
                    }
                    case "Watching":
                    {
                        if (p1.Length() > 0)
                        {
                            var strName = p1.ToString();
                            WatchingText = strName;
                            PostFeelingText = strName;
                            PostFeelingType = "watching"; //Type Of watching
                        }

                        break;
                    }
                    case "Traveling":
                    {
                        if (p1.Length() > 0)
                        {
                            var strName = p1.ToString();
                            TravelingText = strName;
                            PostFeelingText = strName;
                            PostFeelingType = "traveling"; //Type Of traveling
                        }

                        break;
                    }
                }

                TextSanitizer.Load(LoadPostStrings());

                var inputManager = (InputMethodManager)GetSystemService(InputMethodService);
                inputManager?.HideSoftInputFromWindow(TopToolBar.WindowToken, 0);

                TopToolBar.ClearFocus();

                SlidingUpPanel.SetPanelState(SlidingUpPanelLayout.PanelState.Collapsed);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Path

        private void PickiTonCompleteListener(string path)
        {
            //Dismiss dialog and return the path
            try
            {
                //  Check if it was a Drive/local/unknown provider file and display a Toast
                //if (wasDriveFile)
                //{
                //    // "Drive file was selected"
                //}
                //else if (wasUnknownProvider)
                //{
                //    // "File was selected from unknown provider"
                //}
                //else
                //{
                //    // "Local file was selected"
                //}

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
                    case "File":
                    {
                        NameAlbumButton.Visibility = ViewStates.Gone;

                        //remove file the type
                        AttachmentsAdapter.RemoveAll();

                        var attach = new Attachments
                        {
                            Id = AttachmentsAdapter.AttachmentList.Count + 1,
                            TypeAttachment = "postFile",
                            FileSimple = "Image_File",
                            FileUrl = path
                        };

                        AttachmentsAdapter.Add(attach);
                        break;
                    }
                    case "Video":
                    {
                        NameAlbumButton.Visibility = ViewStates.Gone;

                        AttachmentsAdapter.RemoveAll();

                        var fileName = path.Split('/').Last();
                        var fileNameWithoutExtenion = fileName.Split('.').First();

                        var pathImage = Methods.Path.FolderDcimImage + "/" + fileNameWithoutExtenion + ".png";

                        var vidoPlaceHolderImage = Methods.MultiMedia.GetMediaFrom_Gallery(Methods.Path.FolderDcimImage, fileNameWithoutExtenion + ".png");
                        switch (vidoPlaceHolderImage)
                        {
                            case "File Dont Exists":
                            {
                                var bitmapImage = Methods.MultiMedia.Retrieve_VideoFrame_AsBitmap(this, path);
                                Methods.MultiMedia.Export_Bitmap_As_Image(bitmapImage, fileNameWithoutExtenion, Methods.Path.FolderDcimImage);
                                break;
                            }
                        }

                        var attach = new Attachments
                        {
                            Id = AttachmentsAdapter.AttachmentList.Count + 1,
                            TypeAttachment = "postVideo",
                            FileSimple = pathImage,
                            Thumb = new Attachments.VideoThumb
                            {
                                FileUrl = pathImage
                            },

                            FileUrl = path
                        };

                        AttachmentsAdapter.Add(attach);
                        break;
                    }
                    case "Audio":
                    {
                        NameAlbumButton.Visibility = ViewStates.Gone;
                        //var fileName = filepath.Split('/').Last();
                        //var fileNameWithoutExtension = fileName.Split('.').First();

                        //remove file the type
                        AttachmentsAdapter.RemoveAll();

                        var attach = new Attachments
                        {
                            Id = AttachmentsAdapter.AttachmentList.Count + 1,
                            TypeAttachment = "postMusic",
                            FileSimple = "Audio_File",
                            FileUrl = path
                        };

                        AttachmentsAdapter.Add(attach);
                        break;
                    }
                    case "Image":
                    {
                        //remove file the type
                        var videoAttach = AttachmentsAdapter.AttachmentList.Where(a => !a.TypeAttachment.Contains("postPhotos")).ToList();
                        switch (videoAttach.Count)
                        {
                            case > 0:
                            {
                                foreach (var video in videoAttach)
                                    AttachmentsAdapter.Remove(video);
                                break;
                            }
                        }

                        var attach = new Attachments
                        {
                            Id = AttachmentsAdapter.AttachmentList.Count + 1,
                            TypeAttachment = "postPhotos",
                            FileSimple = path,
                            FileUrl = path
                        };

                        AttachmentsAdapter.Add(attach);

                        switch (AttachmentsAdapter.AttachmentList.Count)
                        {
                            case > 1:
                            {
                                NameAlbumButton.Visibility = ViewStates.Visible;

                                foreach (var item in AttachmentsAdapter.AttachmentList)
                                    item.TypeAttachment = "postPhotos[]";
                                break;
                            }
                            default:
                            {
                                NameAlbumButton.Visibility = ViewStates.Gone;

                                foreach (var item in AttachmentsAdapter.AttachmentList)
                                    item.TypeAttachment = "postPhotos";
                                break;
                            }
                        }

                        break;
                    }
                    default:
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Failed_to_load), ToastLength.Short)?.Show();
                        break;
                }

                SlidingUpPanel.SetPanelState(SlidingUpPanelLayout.PanelState.Collapsed);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion
      
        // Event Back
        public override void OnBackPressed()
        {
            try
            {
                if (!string.IsNullOrEmpty(TxtContentPost.Text) || !string.IsNullOrEmpty(MentionText) || AttachmentsAdapter.AttachmentList.Count > 0)
                {
                    TypeDialog = "PostBack";

                    var dialog = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                    dialog.Title(GetText(Resource.String.Lbl_Title_Back)).TitleColorRes(Resource.Color.primary);
                    dialog.Content(GetText(Resource.String.Lbl_Content_Back));
                    dialog.PositiveText(GetText(Resource.String.Lbl_PositiveText_Back)).OnPositive(this);
                    dialog.NegativeText(GetText(Resource.String.Lbl_NegativeText_Back)).OnNegative(this);
                    dialog.AlwaysCallSingleChoiceCallback();
                    dialog.ItemsCallback(this).Build().Show();
                }
                else
                {

                    base.OnBackPressed();
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

        private string LoadPostStrings()
        {
            try
            {
                var newActivityText = string.Empty;
                var newFeelingText = string.Empty;
                var newMentionText = string.Empty;
                var newPlaceText = string.Empty;

                newActivityText = string.IsNullOrEmpty(ActivityText) switch
                {
                    false => PostActivityType + " " + ActivityText,
                    _ => newActivityText
                };

                newFeelingText = string.IsNullOrEmpty(ListeningText) switch
                {
                    false => GetText(Resource.String.Lbl_ListeningTo) + " " + ListeningText,
                    _ => newFeelingText
                };

                switch (string.IsNullOrEmpty(PlayingText))
                {
                    case false:
                        newFeelingText = GetText(Resource.String.Lbl_Playing) + " " + PlayingText;
                        break;
                }

                newFeelingText = string.IsNullOrEmpty(WatchingText) switch
                {
                    false => GetText(Resource.String.Lbl_Watching) + " " + WatchingText,
                    _ => newFeelingText
                };

                switch (string.IsNullOrEmpty(TravelingText))
                {
                    case false:
                        newFeelingText = GetText(Resource.String.Lbl_Traveling) + " " + TravelingText;
                        break;
                }

                newFeelingText = string.IsNullOrEmpty(FeelingText) switch
                {
                    false => GetText(Resource.String.Lbl_Feeling) + " " + FeelingText,
                    _ => newFeelingText
                };

                switch (string.IsNullOrEmpty(MentionText))
                {
                    case false:
                        newMentionText += " " + GetText(Resource.String.Lbl_With) + " " + MentionText.Remove(MentionText.Length - 1, 1);
                        break;
                }

                switch (string.IsNullOrEmpty(PlaceText))
                {
                    case false:
                        newPlaceText += " " + GetText(Resource.String.Lbl_At) + " " + PlaceText;
                        break;
                }

                var mainString = newActivityText + newFeelingText + newMentionText + newPlaceText;
                return mainString;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return "";
            }
        }

        private void OpenDialogVideo()
        {
            try
            {
                TypeDialog = "PostVideos";

                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                arrayAdapter.Add(GetText(Resource.String.Lbl_VideoGallery));
                arrayAdapter.Add(GetText(Resource.String.Lbl_RecordVideoFromCamera));

                dialogList.Title(GetText(Resource.String.Lbl_SelectVideoFrom)).TitleColorRes(Resource.Color.primary);
                dialogList.Items(arrayAdapter);
                dialogList.PositiveText(GetText(Resource.String.Lbl_Close)).OnPositive(this);
                dialogList.AlwaysCallSingleChoiceCallback();
                dialogList.ItemsCallback(this).Build().Show();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void RemoveLocation()
        {
            try
            {
                RunOnUiThread(() =>
                {
                    MentionTextView.Text = "";

                    PlaceText = string.Empty;
                    TextSanitizer.Load(LoadPostStrings());
                });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Transfer Message Contacts

        private bool CheckAccess()
        {
            try
            {
                var a = new Client(AppSettings.TripleDesAppServiceProvider);
                Console.WriteLine(a);

                var dbDatabase = new SqLiteDatabase(); 
                dbDatabase.CheckTablesStatus();
               
                var login = dbDatabase.Get_data_Login_Credentials();
                if (login != null && !string.IsNullOrEmpty(UserDetails.AccessToken) && !string.IsNullOrEmpty(UserDetails.UserId))
                {
                    dbDatabase.GetSettings();
                    DataUser = dbDatabase.Get_MyProfile();

                    PagePost = "Normal_More";
                    IdPost = UserDetails.UserId;

                    
                    return true;
                }

                
                return false;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return false;
            }
        }
         
        private void InitTransferMessageContacts()
        {
            try
            { 
                // Get intent, action and MIME type
                var intent = Intent;
                var action = intent.Action;
                var type = intent.Type;

                switch (action)
                {
                    case Intent.ActionSend when type != null && Intent?.Extras != null:
                    {
                        switch (type)
                        {
                            //if ("text/x-vcard".Equals(type))
                            //{
                            //    //HandleSendTextCard(intent); // Handle text being sent
                            //}
                            case "text/plain":
                                HandleSendText(intent); // Handle text being sent
                                break;
                            default:
                            {
                                if (type.StartsWith("image/") || type.StartsWith("video/") || type.StartsWith("application/")|| type.StartsWith("audio/"))
                                {
                                    HandleSendFile(intent); // Handle single file being sent
                                }

                                break;
                            }
                        }

                        break;
                    }
                    case Intent.ActionSendMultiple when type != null:
                        HandleSendMultipleFiles(intent); // Handle multiple images being sent
                        break;
                    default:
                        // Handle other intents, such as being started from the home screen
                        break;
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        /// <summary>
        /// Update UI to reflect text being shared 
        /// </summary>
        /// <param name="intent"></param>
        private void HandleSendText(Intent intent)
        {
            try
            {
                var sharedText = intent.GetStringExtra(Intent.ExtraText);
                if (sharedText != null)
                {
                    var replacement = Regex.Replace(sharedText, @"\t|\n|\r", "");

                    TxtContentPost.Text = replacement;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //private void HandleSendTextCard(Intent intent)
        //{
        //    try
        //    {
        //        var sharedText = intent.GetStringExtra(Intent?.ExtraText);
        //        if (sharedText != null)
        //        {
        //              AllItem = new List<Attachments>();

        //            string timeNow = DateTime.Now.ToShortTimeString();
        //            var unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        //            string time2 = Convert.ToString(unixTimestamp);
                     
        //            var contact = Methods.PhoneContactManager.Get_ContactInfoBy_Id(intent.Data.LastPathSegment);
        //            if (contact != null)
        //            {
        //                var name = contact.UserDisplayName;
        //                var phone = contact.PhoneNumber;

        //                Attachments m1 = new Attachments
        //                {
        //                    Id = time2,
        //                    FromId = UserDetails.UserId,
        //                    ContactName = name,
        //                    ContactNumber = phone,
        //                    TimeText = timeNow,
        //                    Position = "right",
        //                    ModelType = MessageModelType.RightContact
        //                };
        //                AllItem.Add(m1);
        //            } 
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Methods.DisplayReportResultTrack(e);
        //    }
        //}

        private void HandleSendFile(Intent intent)
        {
            try
            {
                var uri = intent.GetParcelableExtra(Intent.ExtraStream);
                if (uri != null && uri is Uri fileUri)
                {
                    // Update UI to reflect image being shared
                    var filePath = Methods.AttachmentFiles.GetActualPathFromFile(this, fileUri);
                    var check = WoWonderTools.CheckMimeTypesWithServer(filePath);
                    switch (check)
                    {
                        case false:
                            //this file not supported on the server , please select another file 
                            Toast.MakeText(this, GetString(Resource.String.Lbl_ErrorFileNotSupported), ToastLength.Short)?.Show();
                            return;
                        default:
                            PickiTonCompleteListener(filePath);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        /// <summary>
        ///  Update UI to reflect multiple file being shared
        /// </summary>
        /// <param name="intent"></param>
        private void HandleSendMultipleFiles(Intent intent)
        {
            try
            { 
                var uris = intent.GetParcelableArrayListExtra(Intent.ExtraStream); 
                if (uris != null)
                {
                    foreach (var uri in uris)
                    {
                        if (uri != null && uri is Uri fileUri)
                        { 
                            // Update UI to reflect image being shared
                            var filePath = Methods.AttachmentFiles.GetActualPathFromFile(this, fileUri);
                            var check = WoWonderTools.CheckMimeTypesWithServer(filePath);
                            switch (check)
                            {
                                case false:
                                    //this file not supported on the server , please select another file 
                                    Toast.MakeText(this, GetString(Resource.String.Lbl_ErrorFileNotSupported), ToastLength.Short)?.Show();
                                    return;
                                default:
                                    PickiTonCompleteListener(filePath);
                                    break;
                            }
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
         
    }
}
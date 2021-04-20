using System;
using System.Collections.ObjectModel;
using System.Linq;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads.DoubleClick;
using Android.Graphics;
using Android.OS;

using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.Content.Res;
using AndroidX.RecyclerView.Widget;
using TheArtOfDev.Edmodo.Cropper;
using Java.IO;
using Newtonsoft.Json;
using WoWonder.Activities.AddPost.Adapters;
using WoWonder.Activities.Base;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Album;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.Album
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class CreateAlbumActivity : BaseActivity
    {
        #region Variables Basic

        private AttachmentsAdapter MAdapter;
        private RecyclerView MRecycler;
        private LinearLayoutManager LayoutManager;
        private EditText TxtNameAlbum;
        private TextView TxtAdd, IconName;
        private PublisherAdView PublisherAdView;

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
                SetContentView(Resource.Layout.CreateAlbumLayout);

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();  
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
                PublisherAdView?.Resume();
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
                PublisherAdView?.Pause();
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
                MRecycler = (RecyclerView)FindViewById(Resource.Id.recycler);
                TxtNameAlbum = (EditText)FindViewById(Resource.Id.NameEditText);

                IconName = FindViewById<TextView>(Resource.Id.IconName);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconName, FontAwesomeIcon.User);
                 
                TxtAdd = FindViewById<TextView>(Resource.Id.toolbar_title);
                TxtAdd.Text = GetText(Resource.String.Lbl_Create);

                Methods.SetColorEditText(TxtNameAlbum, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                
                PublisherAdView = FindViewById<PublisherAdView>(Resource.Id.multiple_ad_sizes_view); 
                AdsGoogle.InitPublisherAdView(PublisherAdView);
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
                var toolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
                if (toolBar != null)
                {
                    toolBar.Title = GetString(Resource.String.Lbl_CreateAlbum);
                    toolBar.SetTitleTextColor(Color.ParseColor(AppSettings.MainColor));
                    SetSupportActionBar(toolBar);
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
                MAdapter = new AttachmentsAdapter(this) {AttachmentList = new ObservableCollection<Attachments>()};
                LayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
                MRecycler.SetLayoutManager(LayoutManager);
                MRecycler.SetAdapter(MAdapter);

                MRecycler.Visibility = ViewStates.Visible;

                // Add first image Default 
                var attach = new Attachments
                {
                    Id = MAdapter.AttachmentList.Count + 1,
                    TypeAttachment = "Default",
                    FileSimple = "addImage",
                    FileUrl = "addImage"
                };

                MAdapter.Add(attach);
                MAdapter.NotifyDataSetChanged();
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
                        MAdapter.DeleteItemClick += MAdapterOnDeleteItemClick;
                        MAdapter.ItemClick += MAdapterOnItemClick;
                        TxtAdd.Click += TxtAddOnClick;
                        break;
                    default:
                        MAdapter.DeleteItemClick -= MAdapterOnDeleteItemClick;
                        MAdapter.ItemClick -= MAdapterOnItemClick;
                        TxtAdd.Click -= TxtAddOnClick;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        private void DestroyBasic()
        {
            try
            {
                PublisherAdView?.Destroy();
                MAdapter = null!;
                MRecycler = null!;
                TxtNameAlbum = null!;
                TxtAdd = null!;
                IconName = null!;

                PublisherAdView = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events

        private void MAdapterOnDeleteItemClick(object sender, AttachmentsAdapterClickEventArgs e)
        {
            try
            {
                var position = e.Position;
                switch (position)
                {
                    case >= 0:
                    {
                        var item = MAdapter.GetItem(position);
                        if (item != null)
                        {
                            MAdapter.Remove(item);
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

        private async void TxtAddOnClick(object sender, EventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
                else
                {
                    if (string.IsNullOrEmpty(TxtNameAlbum.Text.Replace(" ","")))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_name), ToastLength.Short)?.Show();
                        return;
                    }

                    switch (MAdapter.AttachmentList.Count)
                    {
                        case <= 1:
                            Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_Image), ToastLength.Short)?.Show();
                            break;
                        default:
                        {
                            //Show a progress
                            AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading) + "...");
                         
                            var list = MAdapter.AttachmentList.Where(a => a.TypeAttachment != "Default").ToList(); 
                            var (apiStatus, respond) = await RequestsAsync.Album.CreateAlbumAsync(TxtNameAlbum.Text.Replace(" ", ""), new ObservableCollection<Attachments>(list));
                            switch (apiStatus)
                            {
                                case 200:
                                {
                                    switch (respond)
                                    {
                                        case CreateAlbumObject result:
                                        {
                                            switch (result.Data.PhotoAlbum.Count)
                                            {
                                                case > 0:
                                                {
                                                    AndHUD.Shared.Dismiss(this);
                                                    Toast.MakeText(this, GetText(Resource.String.Lbl_CreatedSuccessfully), ToastLength.Short)?.Show();

                                                    //AlbumItem >> PostDataObject  
                                                    Intent returnIntent = new Intent();
                                                    returnIntent?.PutExtra("AlbumItem", JsonConvert.SerializeObject(result.Data));
                                                    SetResult(Result.Ok, returnIntent);
                                                    Finish();
                                                    break;
                                                }
                                            }

                                            break;
                                        }
                                    }

                                    break;
                                }
                                default:
                                    Methods.DisplayAndHudErrorResult(this, respond);
                                    break;
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                AndHUD.Shared.Dismiss(this);
            }
        }
         
        private void MAdapterOnItemClick(object sender, AttachmentsAdapterClickEventArgs adapterClickEvents)
        {
            try
            {
                var position = adapterClickEvents.Position;
                switch (position)
                {
                    case >= 0:
                    {
                        var item = MAdapter.GetItem(position);
                        switch (item)
                        {
                            case null:
                                return;
                        }
                        if (item.TypeAttachment != "Default") return;
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
                }
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
                    // Add image 
                    case CropImage.CropImageActivityRequestCode when resultCode == Result.Ok:
                    {
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
                                        //  Chick if it was successful
                                        var check = WoWonderTools.CheckMimeTypesWithServer(resultUri.Path);
                                        switch (check)
                                        {
                                            case false:
                                                //this file not supported on the server , please select another file 
                                                Toast.MakeText(this, GetString(Resource.String.Lbl_ErrorFileNotSupported), ToastLength.Short)?.Show();
                                                return;
                                        } 

                                        var attach = new Attachments
                                        {
                                            Id = MAdapter.AttachmentList.Count + 1,
                                            TypeAttachment = "postPhotos[]",
                                            FileSimple = resultUri.Path,
                                            FileUrl = resultUri.Path
                                        };

                                        MAdapter.Add(attach);
                                        break;
                                    }
                                    default:
                                        Toast.MakeText(this, GetText(Resource.String.Lbl_something_went_wrong), ToastLength.Long)?.Show();
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
                        OpenDialogGallery();
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
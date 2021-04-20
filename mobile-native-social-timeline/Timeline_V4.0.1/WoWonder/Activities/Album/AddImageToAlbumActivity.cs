using System;
using System.Collections.ObjectModel;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;


using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.RecyclerView.Widget;
using WoWonder.Library.Anjo.IntegrationRecyclerView;
using Bumptech.Glide.Util;
using Google.Android.Material.AppBar;
using Java.IO;
using Newtonsoft.Json;
using TheArtOfDev.Edmodo.Cropper;
using WoWonder.Activities.Album.Adapters;
using WoWonder.Activities.Base;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils; 
using WoWonderClient.Classes.Album;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Requests;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.Album
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class AddImageToAlbumActivity : BaseActivity
    {
        #region Variables Basic

        private CollapsingToolbarLayout CollapsingToolbar;
        private TextView ToolbarTitle , AddImage;

        private EditText TxtAlbumName;
        private PhotosAdapter MAdapter;
        private RecyclerView MRecycler;
        private GridLayoutManager LayoutManager;

        private Button PublishButton;
        private PostDataObject ImageData;
        private ObservableCollection<Attachments> PathImage = new ObservableCollection<Attachments>();
        
        #endregion

        #region General

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                Methods.App.FullScreenApp(this);

                // Create your application here
                SetContentView(Resource.Layout.AddImageToAlbumLayout);

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();

                Get_DataImage();
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
                CollapsingToolbar = FindViewById<CollapsingToolbarLayout>(Resource.Id.collapsing_toolbar);
                CollapsingToolbar.Title = "";

                ToolbarTitle = FindViewById<TextView>(Resource.Id.toolbar_title);
                AddImage = FindViewById<TextView>(Resource.Id.addImage);
                TxtAlbumName = FindViewById<EditText>(Resource.Id.albumName);
                MRecycler = FindViewById<RecyclerView>(Resource.Id.recycle);
                PublishButton = FindViewById<Button>(Resource.Id.publishButton);
                
                Methods.SetColorEditText(TxtAlbumName, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                PathImage = new ObservableCollection<Attachments>();
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
                    toolBar.Title = " ";
                    toolBar.SetTitleTextColor(Color.ParseColor(AppSettings.MainColor));
                    SetSupportActionBar(toolBar);
                    SupportActionBar.SetDisplayShowCustomEnabled(true);
                    SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                    SupportActionBar.SetHomeButtonEnabled(true);
                    SupportActionBar.SetDisplayShowHomeEnabled(true); 

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
                MAdapter = new PhotosAdapter(this)
                {
                    PhotosList = new ObservableCollection<PhotoAlbumObject>()
                };
                LayoutManager = new GridLayoutManager(this, 2);
                LayoutManager.SetSpanSizeLookup(new MySpanSizeLookup(4, 1, 1)); //5, 1, 2
                MRecycler.SetLayoutManager(LayoutManager);
                MRecycler.HasFixedSize = true;
                MRecycler.SetItemViewCacheSize(10);
                MRecycler.GetLayoutManager().ItemPrefetchEnabled = true;
                var sizeProvider = new FixedPreloadSizeProvider(10, 10);
                var preLoader = new RecyclerViewPreloader<PhotoAlbumObject>(this, MAdapter, sizeProvider, 10);
                MRecycler.AddOnScrollListener(preLoader);
                MRecycler.SetAdapter(MAdapter); 
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
                        AddImage.Click += AddImageOnClick;
                        PublishButton.Click += PublishButtonOnClick;
                        break;
                    default:
                        AddImage.Click -= AddImageOnClick;
                        PublishButton.Click -= PublishButtonOnClick;
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
                CollapsingToolbar = null!;
                MAdapter = null!;
                ToolbarTitle = null!;
                AddImage = null!;
                TxtAlbumName = null!;
                MAdapter = null!;
                MRecycler = null!;
                LayoutManager = null!;
                PublishButton = null!;
                ImageData = null!;
                PathImage = null!; 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events

        //Add image
        private void AddImageOnClick(object sender, EventArgs e)
        {
            try
            {
                OpenDialogGallery();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Publish New image => send api 
        private async void PublishButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    return;
                }

                switch (PathImage?.Count)
                {
                    case 0:
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_Image), ToastLength.Short)?.Show();
                        return;
                }

                //Show a progress
                AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));

                var (apiStatus, respond) = await RequestsAsync.Album.AddImageToAlbumAsync(ImageData.PostId, PathImage);
                switch (apiStatus)
                {
                    case 200:
                    {
                        switch (respond)
                        {
                            case CreateAlbumObject result:
                            {
                                switch (result.Data?.PhotoAlbum?.Count)
                                {
                                    //Add new item to list
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
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                AndHUD.Shared.Dismiss(this);
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
                                        var check = WoWonderTools.CheckMimeTypesWithServer(resultUri.Path);
                                        switch (check)
                                        {
                                            case false:
                                                //this file not supported on the server , please select another file 
                                                Toast.MakeText(this, GetString(Resource.String.Lbl_ErrorFileNotSupported), ToastLength.Short)?.Show();
                                                return;
                                        }
                             
                                        MAdapter.PhotosList.Add(new PhotoAlbumObject
                                        {
                                            Image = resultUri.Path
                                        });
                                        MAdapter.NotifyDataSetChanged();

                                        PathImage.Add(new Attachments
                                        {
                                            Id = MAdapter.PhotosList.Count + 1,
                                            TypeAttachment = "postPhotos[]",
                                            FileSimple = resultUri.Path,
                                            FileUrl = resultUri.Path
                                        });
                                        break;
                                    }
                                }

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

        #region Path

        private void PickiTonCompleteListener(string path)
        {
            try
            {
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
                        MAdapter.PhotosList.Add(new PhotoAlbumObject
                        {
                            Image = path
                        });
                        MAdapter.NotifyDataSetChanged();

                        PathImage.Add(new Attachments
                        {
                            Id = MAdapter.PhotosList.Count + 1,
                            TypeAttachment = "postPhotos[]",
                            FileSimple = path,
                            FileUrl = path
                        });
                        break;
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

        //Get Data 
        private void Get_DataImage()
        {
            try
            {
                ImageData = JsonConvert.DeserializeObject<PostDataObject>(Intent?.GetStringExtra("AlbumObject"));
                if (ImageData != null)
                {
                    ToolbarTitle.Text = Methods.FunString.DecodeString(ImageData.AlbumName);

                    MAdapter.PhotosList = new ObservableCollection<PhotoAlbumObject>(ImageData.PhotoAlbum);
                    MAdapter.NotifyDataSetChanged(); 
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
using System;
using System.Linq;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;


using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.Core.Content;
using AndroidX.Core.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using TheArtOfDev.Edmodo.Cropper;
using WoWonder.Activities.Base;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Requests;
using Console = System.Console;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using File = Java.IO.File;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.SettingsPreferences.General
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class VerificationActivity : BaseActivity
    {
        #region Variables Basic

        private ImageView YourImage, PassportImage;
        private Button BtnAddImage, BtnAddImagePassport, BtnSubmit;
        private EditText NameEdit, MessagesEdit;
        private string PathYourImage = "" , PathPassportImage = "", TypeImage;
        private NestedScrollView ScrollView;
        private LinearLayout VerifiedLinear, NotVerifiedLinear;
        private TextView VerifiedIcon, TextTitleVerified;
      
        #endregion

        #region General

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                Methods.App.FullScreenApp(this);

                SetTheme(AppSettings.SetTabDarkTheme ? Resource.Style.MyTheme_Dark_Base : Resource.Style.MyTheme_Base);

                // Create your application here
                SetContentView(Resource.Layout.VerificationLayout);
                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                Get_Data_User();
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
                YourImage = FindViewById<ImageView>(Resource.Id.Image);
                BtnAddImage = FindViewById<Button>(Resource.Id.btn_AddPhoto);

                PassportImage = FindViewById<ImageView>(Resource.Id.ImagePassport);
                BtnAddImagePassport = FindViewById<Button>(Resource.Id.btn_Passport);
                
                NameEdit = FindViewById<EditText>(Resource.Id.Name_text);
                MessagesEdit = FindViewById<EditText>(Resource.Id.Messages_Edit);
                BtnSubmit = FindViewById<Button>(Resource.Id.submitButton);

                TextTitleVerified = FindViewById<TextView>(Resource.Id.textTitileVerified);
                VerifiedIcon = FindViewById<TextView>(Resource.Id.verifiedIcon);
                ScrollView = FindViewById<NestedScrollView>(Resource.Id.ScrollView);
                VerifiedLinear = FindViewById<LinearLayout>(Resource.Id.verified);
                NotVerifiedLinear = FindViewById<LinearLayout>(Resource.Id.notVerified);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, VerifiedIcon, IonIconsFonts.CheckmarkCircle);
                VerifiedIcon.SetTextColor(Color.ParseColor(AppSettings.MainColor));
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

        private void AddOrRemoveEvent(bool addEvent)
        {
            try
            {
                switch (addEvent)
                {
                    // true +=  // false -=
                    case true:
                        BtnAddImage.Click += BtnAddImageOnClick;
                        BtnSubmit.Click += BtnSubmitOnClick;
                        BtnAddImagePassport.Click += BtnAddImagePassportOnClick;
                        break;
                    default:
                        BtnAddImage.Click -= BtnAddImageOnClick;
                        BtnSubmit.Click -= BtnSubmitOnClick;
                        BtnAddImagePassport.Click -= BtnAddImagePassportOnClick;
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
                YourImage = null!;
                BtnAddImage = null!;
                PassportImage = null!;
                BtnAddImagePassport = null!;
                NameEdit = null!;
                MessagesEdit = null!;
                BtnSubmit = null!;
                TextTitleVerified = null!;
                VerifiedIcon = null!;
                ScrollView = null!;
                VerifiedLinear = null!;
                NotVerifiedLinear = null!;
                PathYourImage = "";
                PathPassportImage = "";
                TypeImage = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        private async void BtnSubmitOnClick(object sender, EventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
                else
                {
                    if (string.IsNullOrEmpty(NameEdit.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_name), ToastLength.Short)?.Show();
                        return;
                    }

                    if (string.IsNullOrEmpty(MessagesEdit.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_Messages), ToastLength.Short)?.Show();
                        return;
                    }

                    if (string.IsNullOrEmpty(PathYourImage) || string.IsNullOrEmpty(PathPassportImage))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_Image), ToastLength.Short)?.Show();
                    }
                    else
                    {
                        //Show a progress
                        AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading) + "...");

                        var (apiStatus, respond) = await RequestsAsync.Global.VerificationAsync(NameEdit.Text, MessagesEdit.Text, PathYourImage , PathPassportImage);
                        switch (apiStatus)
                        {
                            case 200:
                            {
                                switch (respond)
                                {
                                    case MessageObject result:
                                        Console.WriteLine(result.Message);
                                        AndHUD.Shared.Dismiss(this);
                                        Toast.MakeText(this, GetText(Resource.String.Lbl_Successfully_Verification), ToastLength.Short)?.Show();

                                        Finish();
                                        break;
                                }

                                break;
                            }
                            default:
                                Methods.DisplayAndHudErrorResult(this, respond);
                                break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                AndHUD.Shared.Dismiss(this);
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        private void BtnAddImagePassportOnClick(object sender, EventArgs e)
        {
            try
            {
                OpenGallery("Passport");
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        private void BtnAddImageOnClick(object sender, EventArgs e)
        {
            try
            {
                OpenGallery("YourImage");
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Permissions && Result

        protected override void OnActivityResult(int requestCode,Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);

                switch (requestCode)
                {
                    //If its from Camera or Gallery  
                    case CropImage.CropImageActivityRequestCode:
                    {
                        CropImage.ActivityResult result = CropImage.GetActivityResult(data);

                        switch (resultCode)
                        {
                            case Result.Ok when result.IsSuccessful:
                            {
                                Uri resultUri = result.Uri;

                                switch (string.IsNullOrEmpty(resultUri.Path))
                                {
                                    case false:
                                        switch (TypeImage)
                                        {
                                            case "YourImage":
                                            {
                                                PathYourImage = resultUri.Path;
                                                File file2 = new File(resultUri.Path);
                                                var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);
                                                Glide.With(this).Load(photoUri).Apply(new RequestOptions()).Into(YourImage);
                                                break;
                                            }
                                            case "Passport":
                                            {
                                                PathPassportImage = resultUri.Path;
                                                File file2 = new File(resultUri.Path);
                                                var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);
                                                Glide.With(this).Load(photoUri).Apply(new RequestOptions()).Into(PassportImage);
                                                break;
                                            }
                                        }

                                        break;
                                    default:
                                        Toast.MakeText(this, GetText(Resource.String.Lbl_something_went_wrong), ToastLength.Short)?.Show();
                                        break;
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            try
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

                switch (requestCode)
                {
                    //Image Picker
                    case 108 when grantResults.Length > 0 && grantResults[0] == Permission.Granted:
                        //Open Image 
                        OpenGallery(TypeImage);
                        break;
                    case 108:
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Permission_is_denied), ToastLength.Long)?.Show();
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void OpenGallery(string type)
        {
            try
            {
                TypeImage = type;

                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
                else
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
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        #endregion

        private async void Get_Data_User()
        {
            try
            {
                switch (ListUtils.MyProfileList?.Count)
                {
                    case 0:
                        await ApiRequest.Get_MyProfileData_Api(this);
                        break;
                }

                var local = ListUtils.MyProfileList?.FirstOrDefault();
                if (local != null)
                {
                    switch (local.Verified)
                    {
                        case "1":
                            VerifiedLinear.Visibility = ViewStates.Visible;
                            NotVerifiedLinear.Visibility = ViewStates.Gone;
                            ScrollView.Visibility = ViewStates.Gone;
                            TextTitleVerified.Text = GetText(Resource.String.Lbl_WelcomeTo) + " " + AppSettings.ApplicationName;
                            break;
                        default:
                            VerifiedLinear.Visibility = ViewStates.Gone;
                            NotVerifiedLinear.Visibility = ViewStates.Visible;
                            ScrollView.Visibility = ViewStates.Visible;
                            TextTitleVerified.Text = GetText(Resource.String.Lbl_Please_select_Image_passport);
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

    }
}
using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;

using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.Content.Res;
using AndroidX.Core.Content;
using AT.Markushi.UI;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using TheArtOfDev.Edmodo.Cropper;
using Java.IO;
using WoWonder.Activities.Base;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonderClient.Requests;
using Console = System.Console;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Uri = Android.Net.Uri;

namespace WoWonder.Payment
{
    [Activity(Icon ="@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class PaymentLocalActivity : BaseActivity
    {
        #region Variables Basic

        private TextView BankName, CardNumber, CardCode, CardCountry, CardName;
        private ImageView Image;
        private CircleButton ImageClose;
        private Button BtnAddImage, BtnApply;
        private string Id, Price, PayType, PathImage = "";

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
                SetContentView(Resource.Layout.PaymentLocalLayout);

                Id = Intent?.GetStringExtra("Id") ?? "";
                Price = Intent?.GetStringExtra("Price") ?? "";
                PayType = Intent?.GetStringExtra("payType") ?? ""; // membership , Funding , AddFunds ,SendMoney 

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar(); 
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
                BankName = (TextView)FindViewById(Resource.Id.bank_name);
                CardNumber = (TextView)FindViewById(Resource.Id.card_number);
                CardCode = (TextView)FindViewById(Resource.Id.card_code);
                CardCountry = (TextView)FindViewById(Resource.Id.card_country);
                CardName = (TextView)FindViewById(Resource.Id.card_name); 
                Image = (ImageView)FindViewById(Resource.Id.Image);
                 
                ImageClose = (CircleButton)FindViewById(Resource.Id.ImageCircle);
                BtnAddImage = (Button)FindViewById(Resource.Id.btn_AddPhoto);
                BtnApply = (Button)FindViewById(Resource.Id.ApplyButton);

                string bankDescription = ListUtils.SettingsSiteList?.BankDescription;
                bankDescription = bankDescription?.Replace("&lt;", "<").Replace("&gt;", ">");
                var splitText = bankDescription?.Split(new[] { "<p>", "</p>" }, StringSplitOptions.None);
                Console.WriteLine(splitText);
                  
                switch (splitText?.Length)
                {
                    case > 0:
                    {
                        CardNumber.Text = splitText[1];
                        CardName.Text = splitText[3];
                        CardCode.Text = splitText[5];
                        CardCountry.Text = splitText[7];

                        var bankName = splitText[0].Split(new[] { ">", "</h4>" }, StringSplitOptions.None);
                        switch (bankName.Length)
                        {
                            case > 0:
                                BankName.Text = bankName[12];
                                BankName.Visibility = ViewStates.Visible;
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

        private void InitToolbar()
        {
            try
            {
                var toolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
                if (toolBar != null)
                {
                    toolBar.Title = GetString(Resource.String.Lbl_BankTransfer);
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
         
        private void AddOrRemoveEvent(bool addEvent)
        {
            try
            {
                switch (addEvent)
                {
                    // true +=  // false -=
                    case true:
                        ImageClose.Click += ImageCloseOnClick;
                        BtnAddImage.Click += BtnAddImageOnClick;
                        BtnApply.Click += BtnApplyOnClick;
                        break;
                    default:
                        ImageClose.Click -= ImageCloseOnClick;
                        BtnAddImage.Click -= BtnAddImageOnClick;
                        BtnApply.Click -= BtnApplyOnClick;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        #endregion

        #region Events

        private void BtnAddImageOnClick(object sender, EventArgs e)
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

        private async void BtnApplyOnClick(object sender, EventArgs e)
        {
            try
            {
                if (Methods.CheckConnectivity())
                {
                    if (string.IsNullOrEmpty(PathImage) || string.IsNullOrWhiteSpace(PathImage))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_ErrorPleaseSelectImage), ToastLength.Long)?.Show();
                        return;
                    }

                    //Show a progress
                    AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));

                    int apiStatus = 0; dynamic respond = null!;
                    string description;
                    switch (PayType)
                    {
                        case "membership":
                            description = "Account upgrade request for type" + Id + " = " + Price;
                            (apiStatus, respond) = await RequestsAsync.Global.UploadBankRecipeAsync(Id, Price, description, PathImage);
                            break;
                        case "AddFunds":
                            description = "Add to balance = " + Price; 
                            //wallet
                            (apiStatus, respond) = await RequestsAsync.Global.UploadBankRecipeAsync("wallet", Price, description, PathImage);
                            break;
                        case "Funding":
                            description = "Doante = " + Price;
                            //funding
                            (apiStatus, respond) = await RequestsAsync.Global.UploadBankRecipeAsync("funding", Price, description, PathImage, Id);
                            break;
                    }

                    switch (apiStatus)
                    {
                        case 200:
                            RunOnUiThread(() =>
                            {
                                try
                                {
                                    AndHUD.Shared.Dismiss(this);
                                    Toast.MakeText(this, GetText(Resource.String.Lbl_YourWasReceiptSuccessfullyUploaded), ToastLength.Short)?.Show();
                                    Finish();
                                }
                                catch (Exception exception)
                                {
                                    Methods.DisplayReportResultTrack(exception);
                                }
                            });
                            break;
                        default:
                            Methods.DisplayAndHudErrorResult(this, respond);
                            break;
                    }
                }
                else
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
                } 
            }
            catch (Exception exception)
            {
                AndHUD.Shared.Dismiss(this);
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void ImageCloseOnClick(object sender, EventArgs e)
        {
            try
            {
                PathImage = "";
                GlideImageLoader.LoadImage(this, "Grey_Offline", Image, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);
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
                    case 108:
                    case CropImage.CropImageActivityRequestCode:
                    {
                        if (Methods.CheckConnectivity())
                        {
                            var result = CropImage.GetActivityResult(data);
                            switch (result.IsSuccessful)
                            {
                                case true:
                                {
                                    var resultPathImage = result.Uri.Path;
                                    switch (string.IsNullOrEmpty(resultPathImage))
                                    {
                                        case false:
                                        {
                                            PathImage = resultPathImage;

                                            File file2 = new File(PathImage);
                                            var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);
                                            Glide.With(this).Load(photoUri).Apply(new RequestOptions().CenterCrop()).Into(Image);
                                            break;
                                        }
                                    }

                                    break;
                                }
                                default:
                                    Toast.MakeText(this, GetText(Resource.String.Lbl_something_went_wrong), ToastLength.Long)?.Show();
                                    break;
                            }
                        }
                        else
                        {
                            Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
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
                    case 106 when grantResults.Length > 0 && grantResults[0] == Permission.Granted:
                        OpenDialogGallery();
                        break;
                    case 106:
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
                        var myUri = Uri.FromFile(new File(Methods.Path.FolderDcimImage, Methods.GetTimestamp(DateTime.Now) + ".jpeg"));
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
                            var myUri = Uri.FromFile(new File(Methods.Path.FolderDcimImage, Methods.GetTimestamp(DateTime.Now) + ".jpeg"));
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
                            //request Code 108
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
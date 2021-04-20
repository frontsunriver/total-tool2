using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AFollestad.MaterialDialogs;
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
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using TheArtOfDev.Edmodo.Cropper;
using Java.IO;
using Java.Lang;
using WoWonder.Activities.Base;
using WoWonder.Activities.Offers.Adapters;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Offers;
using WoWonderClient.Requests;
using Console = System.Console;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.Offers
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class CreateOffersActivity : BaseActivity, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback, View.IOnClickListener
    {
        #region Variables Basic

        private TextView TxtSave;
        private TextView IconDiscountType, IconDiscountItems, IconCurrency, IconDescription, IconDate, IconTime, TxtAddImg;
        private EditText TxtDiscountType, TxtDiscountItems,TxtCurrency, TxtDate, TxtTime, TxtDescription;
        private ImageView Image;
        private RecyclerView MRecycler;
        private LinearLayoutManager LayoutManager;
        private DiscountTypeAdapter MAdapter;
        private string TypeDialog, CurrencyId, ImagePath, PageId, AddDiscountId;

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
                SetContentView(Resource.Layout.CreateOffersLayout);

                PageId = Intent?.GetStringExtra("PageId") ?? "";

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
                TxtSave = FindViewById<TextView>(Resource.Id.toolbar_title);

                Image = FindViewById<ImageView>(Resource.Id.image);
                TxtAddImg = FindViewById<TextView>(Resource.Id.addImg);

                IconDiscountType = FindViewById<TextView>(Resource.Id.IconDiscountType);
                TxtDiscountType = FindViewById<EditText>(Resource.Id.DiscountTypeEditText);
              
                MRecycler = FindViewById<RecyclerView>(Resource.Id.Recyler);
                MRecycler.Visibility = ViewStates.Gone;

                IconDiscountItems = FindViewById<TextView>(Resource.Id.IconDiscountItems);
                TxtDiscountItems = FindViewById<EditText>(Resource.Id.DiscountItemsEditText);
                  
                IconCurrency = FindViewById<TextView>(Resource.Id.IconCurrency);
                TxtCurrency = FindViewById<EditText>(Resource.Id.CurrencyEditText);

                IconDescription = FindViewById<TextView>(Resource.Id.IconDescription);
                TxtDescription = FindViewById<EditText>(Resource.Id.DescriptionEditText);
                 
                IconDate = FindViewById<TextView>(Resource.Id.IconDate);
                TxtDate = FindViewById<EditText>(Resource.Id.DateEditText);

                IconTime = FindViewById<TextView>(Resource.Id.IconTime);
                TxtTime = FindViewById<EditText>(Resource.Id.TimeEditText);
               
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconDiscountType, FontAwesomeIcon.User);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconDiscountItems, FontAwesomeIcon.MapMarkedAlt);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconCurrency, FontAwesomeIcon.DollarSign);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconDescription, FontAwesomeIcon.Paragraph);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconDate, FontAwesomeIcon.CalendarAlt); 
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconTime, FontAwesomeIcon.Clock);  

                Methods.SetColorEditText(TxtDiscountType, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtDiscountItems, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtCurrency, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtDescription, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtDate, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtTime, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                Methods.SetFocusable(TxtDiscountType);
                Methods.SetFocusable(TxtCurrency);
                Methods.SetFocusable(TxtDate);
                Methods.SetFocusable(TxtTime);

                TxtDate.SetOnClickListener(this);
                TxtTime.SetOnClickListener(this); 
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
                    toolBar.Title = GetText(Resource.String.Lbl_CreateOffers);
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
                LayoutManager = new LinearLayoutManager(this);
                MAdapter = new DiscountTypeAdapter(this) { DiscountList = new ObservableCollection<DiscountOffers>() };
                MRecycler.SetLayoutManager(LayoutManager);
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
                        TxtSave.Click += TxtSaveOnClick;
                        TxtCurrency.Touch += TxtCurrencyOnTouch;
                        TxtAddImg.Click += TxtAddImgOnClick;
                        TxtDiscountType.Touch += TxtDiscountTypeOnTouch;
                        break;
                    default:
                        TxtSave.Click -= TxtSaveOnClick;
                        TxtCurrency.Touch -= TxtCurrencyOnTouch;
                        TxtAddImg.Click -= TxtAddImgOnClick;
                        TxtDiscountType.Touch -= TxtDiscountTypeOnTouch;
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
                MAdapter = null!;
                TxtSave = null!;
                Image = null!;
                TxtAddImg = null!;
                IconDiscountType = null!;
                TxtDiscountType = null!;
                MRecycler = null!;
                IconDiscountItems = null!;
                TxtDiscountItems = null!;
                IconCurrency = null!;
                TxtCurrency = null!;
                IconDescription = null!;
                TxtDescription = null!;
                IconDate = null!;
                TxtDate  = null!;
                IconTime = null!;
                TxtTime = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        private void TxtAddImgOnClick(object sender, EventArgs e)
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

        private void TxtCurrencyOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                if (ListUtils.SettingsSiteList?.CurrencySymbolArray.CurrencyList != null)
                {
                    TypeDialog = "Currency";

                    var arrayAdapter = WoWonderTools.GetCurrencySymbolList();
                    switch (arrayAdapter?.Count)
                    {
                        case > 0:
                        {
                            var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                            dialogList.Title(GetText(Resource.String.Lbl_SelectCurrency)).TitleColorRes(Resource.Color.primary);
                            dialogList.Items(arrayAdapter);
                            dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                            dialogList.AlwaysCallSingleChoiceCallback();
                            dialogList.ItemsCallback(this).Build().Show();
                            break;
                        }
                    }
                }
                else
                {
                    Methods.DisplayReportResult(this, "Not have List Currency");
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
        
        private void TxtDiscountTypeOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                TypeDialog = "DiscountOffersAdapter";

                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);
                var arrayAdapter = WoWonderTools.GetAddDiscountList(this).Select(pair => pair.Value).ToList();
                dialogList.Title(GetText(Resource.String.Lbl_DiscountType)).TitleColorRes(Resource.Color.primary);
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
         
        private async void TxtSaveOnClick(object sender, EventArgs e)
        {
            try
            {
                if (Methods.CheckConnectivity())
                {
                    if (string.IsNullOrEmpty(TxtDiscountType.Text) || string.IsNullOrEmpty(TxtDiscountItems.Text) || string.IsNullOrEmpty(TxtCurrency.Text)
                        || string.IsNullOrEmpty(TxtDescription.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_your_data), ToastLength.Short)?.Show();
                        return;
                    }

                    if (string.IsNullOrEmpty(ImagePath))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_Image), ToastLength.Short)?.Show();
                        return;
                    }

                    //Show a progress
                    AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));

                    var dictionary = new Dictionary<string, string>
                    {
                        {"discount_type", AddDiscountId},
                        {"currency", CurrencyId},
                        {"page_id", PageId},
                        {"expire_date", TxtDate.Text},
                        {"expire_time", TxtTime.Text},
                        {"description", TxtDescription.Text},
                        {"discounted_items", TxtDiscountItems.Text},
                    };

                    switch (MAdapter.DiscountList.Count)
                    {
                        case > 0:
                        {
                            foreach (var discount in MAdapter.DiscountList)
                            {
                                switch (discount)
                                {
                                    case null:
                                        continue;
                                    default:
                                        switch (discount.DiscountType)
                                        {
                                            case "discount_percent":
                                                dictionary.Add("discount_percent", discount.DiscountFirst);
                                                break;
                                            case "discount_amount":
                                                dictionary.Add("discount_amount", discount.DiscountFirst);
                                                break;
                                            case "buy_get_discount":
                                                dictionary.Add("discount_percent", discount.DiscountFirst);
                                                dictionary.Add("buy", discount.DiscountSec);
                                                dictionary.Add("get", discount.DiscountThr);
                                                break;
                                            case "spend_get_off":
                                                dictionary.Add("spend", discount.DiscountSec);
                                                dictionary.Add("amount_off", discount.DiscountThr);
                                                break;
                                            case "free_shipping": //Not have tag
                                                break;
                                        }

                                        break;
                                }
                            }

                            break;
                        }
                    }

                    var (apiStatus, respond) = await RequestsAsync.Offers.CreateOfferAsync(dictionary, ImagePath);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case CreateOfferObject result:
                                    Console.WriteLine(result.Data);
                                    Toast.MakeText(this, GetString(Resource.String.Lbl_OfferSuccessfullyAdded), ToastLength.Short)?.Show();

                                    AndHUD.Shared.Dismiss(this);
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
                else
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
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
                                        ImagePath = resultUri.Path;
                                        File file2 = new File(resultUri.Path);
                                        var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);
                                        Glide.With(this).Load(photoUri).Apply(new RequestOptions()).Into(Image);

                                        //GlideImageLoader.LoadImage(this, resultUri.Path, Image, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);
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

        #region MaterialDialog

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

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                switch (TypeDialog)
                {
                    case "Currency":
                        TxtCurrency.Text = itemString.ToString(); 

                        var (currency, currencyIcon) = WoWonderTools.GetCurrency(itemId.ToString());
                        CurrencyId = currency;
                        Console.WriteLine(currencyIcon);
                        break;
                    case "DiscountOffersAdapter":
                    {
                        AddDiscountId = WoWonderTools.GetAddDiscountList(this)?.FirstOrDefault(a => a.Value == itemString.ToString()).Key.ToString();

                        TxtDiscountType.Text = itemString.ToString();

                        switch (AddDiscountId)
                        {
                            case "free_shipping":
                                MRecycler.Visibility = ViewStates.Gone;
                                MAdapter.DiscountList.Clear();
                                MAdapter.NotifyDataSetChanged();
                                break;
                            default:
                                MRecycler.Visibility = ViewStates.Visible;
                                MAdapter.DiscountList.Clear();

                                MAdapter.DiscountList.Add(new DiscountOffers
                                {
                                    DiscountType = AddDiscountId,
                                    DiscountFirst = "",
                                    DiscountSec = "",
                                    DiscountThr = "",
                                });
                                MAdapter.NotifyDataSetChanged();
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
          
        public void OnClick(View v)
        {
            try
            {
                if (v.Id == TxtTime.Id)
                {
                    var frag = PopupDialogController.TimePickerFragment.NewInstance(delegate (DateTime time)
                    {
                        TxtTime.Text = time.ToShortTimeString();
                    });

                    frag.Show(SupportFragmentManager, PopupDialogController.TimePickerFragment.Tag);
                } 
                else if (v.Id == TxtDate.Id)
                {
                    var frag = PopupDialogController.DatePickerFragment.NewInstance(delegate (DateTime time)
                    {
                        TxtDate.Text = time.Date.ToString("yyyy-MM-dd");
                    });

                    frag.Show(SupportFragmentManager, PopupDialogController.DatePickerFragment.Tag);
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

    }
}
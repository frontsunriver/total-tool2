using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AFollestad.MaterialDialogs;
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
using Java.Lang;
using Newtonsoft.Json;
using WoWonder.Activities.AddPost.Adapters;
using WoWonder.Activities.Base;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Classes.Product;
using WoWonderClient.Requests;
using Exception = System.Exception;
using File = Java.IO.File;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.Market
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class CreateProductActivity : BaseActivity, View.IOnFocusChangeListener, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private TextView TxtAdd, IconTitle, IconPrice, IconLocation, IconCategories, IconAbout, IconType;
        private EditText TxtTitle, TxtPrice, TxtCurrency, TxtLocation, TxtAbout, TxtCategory;
        private RadioButton RbNew,RbUsed;
        private string CategoryId = "", CurrencyId = "", ProductType = "" , PlaceText = "" , TypeDialog = ""; 
        private PublisherAdView PublisherAdView;
        private AttachmentsAdapter MAdapter;
        private RecyclerView MRecycler;
        private LinearLayoutManager LayoutManager;

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
                SetContentView(Resource.Layout.CreateProduct_Layout);

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
                TxtAdd = FindViewById<TextView>(Resource.Id.toolbar_title);

                IconTitle = FindViewById<TextView>(Resource.Id.IconTitle); 
                TxtTitle = FindViewById<EditText>(Resource.Id.TitleEditText); 
                IconPrice = FindViewById<TextView>(Resource.Id.IconPrice);
                TxtPrice = FindViewById<EditText>(Resource.Id.PriceEditText);
                TxtCurrency = FindViewById<EditText>(Resource.Id.CurrencyEditText);  
                IconLocation = FindViewById<TextView>(Resource.Id.IconLocation);
                TxtLocation = FindViewById<EditText>(Resource.Id.LocationEditText); 
                IconCategories = FindViewById<TextView>(Resource.Id.IconCategories);
                TxtCategory = FindViewById<EditText>(Resource.Id.CategoriesEditText); 
                IconAbout = FindViewById<TextView>(Resource.Id.IconAbout);
                TxtAbout = FindViewById<EditText>(Resource.Id.AboutEditText);
                IconType = FindViewById<TextView>(Resource.Id.IconType);
                RbNew = FindViewById<RadioButton>(Resource.Id.radioNew);
                RbUsed = FindViewById<RadioButton>(Resource.Id.radioUsed);

                MRecycler = (RecyclerView)FindViewById(Resource.Id.imageRecyler);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconTitle, FontAwesomeIcon.User);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconLocation, FontAwesomeIcon.MapMarkedAlt);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconPrice, FontAwesomeIcon.MoneyBillAlt);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconAbout, FontAwesomeIcon.Paragraph);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeBrands, IconCategories, FontAwesomeIcon.Buromobelexperte);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconType, FontAwesomeIcon.LayerPlus);

                Methods.SetColorEditText(TxtTitle, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtPrice, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtCurrency, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtLocation, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtCategory, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtAbout, AppSettings.SetTabDarkTheme ? Color.White : Color.Black); 

                Methods.SetFocusable(TxtCategory);
                Methods.SetFocusable(TxtCurrency);

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
                    toolBar.Title = GetText(Resource.String.Lbl_CreateNewProduct);
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
                MAdapter = new AttachmentsAdapter(this) { AttachmentList = new ObservableCollection<Attachments>() };
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
                        RbNew.CheckedChange += RbNewOnCheckedChange;
                        RbUsed.CheckedChange += RbUsedOnCheckedChange;
                        TxtAdd.Click += TxtAddOnClick;
                        TxtLocation.OnFocusChangeListener = this; 
                        TxtCategory.Touch += TxtCategoryOnClick;
                        TxtCurrency.Touch += TxtCurrencyOnTouch;
                        break;
                    default:
                        MAdapter.DeleteItemClick -= MAdapterOnDeleteItemClick;
                        MAdapter.ItemClick -= MAdapterOnItemClick;
                        RbNew.CheckedChange -= RbNewOnCheckedChange;
                        RbUsed.CheckedChange -= RbUsedOnCheckedChange;
                        TxtAdd.Click -= TxtAddOnClick;
                        TxtLocation.OnFocusChangeListener = null!; 
                        TxtCategory.Touch -= TxtCategoryOnClick;
                        TxtCurrency.Touch -= TxtCurrencyOnTouch;
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

                TxtAdd = null!;
                IconTitle = null!;
                TxtTitle = null!;
                IconPrice = null!;
                TxtPrice = null!;
                TxtCurrency = null!;
                IconLocation = null!;
                TxtLocation = null!;
                IconCategories = null!;
                TxtCategory = null!;
                IconAbout = null!;
                TxtAbout = null!;
                IconType = null!;
                RbNew = null!;
                RbUsed = null!;
                MAdapter = null!;
                MRecycler = null!;
                PublisherAdView = null!;
                CategoryId = "";
                CurrencyId = "";
                ProductType = "";
                PlaceText = "";
                TypeDialog = "";
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


        private void MAdapterOnItemClick(object sender, AttachmentsAdapterClickEventArgs e)
        {
            try
            {
                var position = e.Position;
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
                        OpenDialogGallery(); //requestCode >> 500 => Image Gallery
                        break;
                    }
                }
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

                var arrayAdapter = WoWonderTools.GetCurrencySymbolList();
                switch (arrayAdapter?.Count)
                {
                    case > 0:
                    {
                        TypeDialog = "Currency";
                     
                        var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);
                     
                        dialogList.Title(GetText(Resource.String.Lbl_SelectCurrency)).TitleColorRes(Resource.Color.primary);
                        dialogList.Items(arrayAdapter);
                        dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                        dialogList.AlwaysCallSingleChoiceCallback();
                        dialogList.ItemsCallback(this).Build().Show();
                        break;
                    }
                    default:
                        Methods.DisplayReportResult(this, "Not have List Currency Products");
                        break;
                } 
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        private void TxtCategoryOnClick(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                switch (CategoriesController.ListCategoriesProducts.Count)
                {
                    case > 0:
                    {
                        TypeDialog = "Categories";

                        var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                        var arrayAdapter = CategoriesController.ListCategoriesProducts.Select(item => item.CategoriesName).ToList();

                        dialogList.Title(GetText(Resource.String.Lbl_SelectCategories)).TitleColorRes(Resource.Color.primary);
                        dialogList.Items(arrayAdapter);
                        dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                        dialogList.AlwaysCallSingleChoiceCallback();
                        dialogList.ItemsCallback(this).Build().Show();
                        break;
                    }
                    default:
                        Methods.DisplayReportResult(this, "Not have List Categories Products");
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void TxtLocationOnFocusChange()
        {
            try
            {
                switch ((int)Build.VERSION.SdkInt)
                {
                    // Check if we're running on Android 5.0 or higher
                    case < 23:
                        //Open intent Location when the request code of result is 502
                        new IntentController(this).OpenIntentLocation();
                        break;
                    default:
                    {
                        if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) == Permission.Granted && CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) == Permission.Granted)
                        {
                            //Open intent Location when the request code of result is 502
                            new IntentController(this).OpenIntentLocation();
                        }
                        else
                        {
                            new PermissionsController(this).RequestPermission(105);
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
                    if (string.IsNullOrEmpty(TxtTitle.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_name), ToastLength.Short)?.Show();
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtPrice.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_price), ToastLength.Short)?.Show();
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtLocation.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_Location), ToastLength.Short)?.Show();
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtAbout.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_about), ToastLength.Short)?.Show();
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtCurrency.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_Currency), ToastLength.Short)?.Show();
                        return;
                    }

                    var list = MAdapter.AttachmentList.Where(a => a.TypeAttachment != "Default").ToList();
                    switch (list.Count)
                    {
                        case 0:
                            Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_Image), ToastLength.Short)?.Show();
                            break;
                        default:
                        {
                            //Show a progress
                            AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading) + "...");
                         
                            var (apiStatus, respond) = await RequestsAsync.Market.CreateProductAsync(TxtTitle.Text, TxtAbout.Text, TxtLocation.Text, TxtPrice.Text, CurrencyId, CategoryId, ProductType, list);
                            switch (apiStatus)
                            {
                                case 200:
                                {
                                    switch (respond)
                                    {
                                        case CreateProductObject result:
                                        {
                                            AndHUD.Shared.Dismiss(this); 
                                            Toast.MakeText(this, GetText(Resource.String.Lbl_CreatedSuccessfully), ToastLength.Short)?.Show();
                                
                                            var listImage = list.Select(productPathImage => new Images {Id = "", ProductId = result.ProductId.ToString(), Image = productPathImage.FileSimple, ImageOrg = productPathImage.FileSimple}).ToList();

                                            //Add new item to my Event list
                                            var user = ListUtils.MyProfileList?.FirstOrDefault();
                                            ProductDataObject data = new ProductDataObject
                                            {
                                                Name = TxtTitle.Text,
                                                UserId = UserDetails.UserId,
                                                Id = result.ProductId.ToString(),
                                                Location = TxtLocation.Text,
                                                Description = TxtAbout.Text,
                                                Category = CategoryId,
                                                Images = new List<Images>(listImage),
                                                Price = TxtPrice.Text,
                                                Type = ProductType,
                                                Seller = user,
                                                Currency = CurrencyId,
                                                PostId = result.ProductPostId.ToString(),
                                            };

                                            if (TabbedMarketActivity.GetInstance()?.MyProductsTab.MAdapter.MarketList != null)
                                            {
                                                TabbedMarketActivity.GetInstance()?.MyProductsTab.MAdapter?.MarketList?.Insert(0, data);
                                                TabbedMarketActivity.GetInstance()?.MyProductsTab.MAdapter?.NotifyItemInserted(TabbedMarketActivity.GetInstance().MyProductsTab.MAdapter.MarketList.IndexOf(data));
                                            }

                                            Intent returnIntent = new Intent();
                                            returnIntent?.PutExtra("product", JsonConvert.SerializeObject(data));
                                            SetResult(Result.Ok, returnIntent);

                                
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

        private void RbUsedOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            try
            {
                var isChecked = RbUsed.Checked;
                switch (isChecked)
                {
                    case true:
                        RbNew.Checked = false;
                        ProductType = "1";
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void RbNewOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            try
            {
                var isChecked = RbNew.Checked;
                switch (isChecked)
                {
                    case true:
                        RbUsed.Checked = false;
                        ProductType = "0";
                        break;
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
                    case 502 when resultCode == Result.Ok:
                        GetPlaceFromPicker(data);
                        break;
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
                                                var productPathImage = resultUri.Path; 
                                                var attach = new Attachments
                                                {
                                                    Id = MAdapter.AttachmentList.Count + 1,
                                                    TypeAttachment = "images[]",
                                                    FileSimple = productPathImage,
                                                    FileUrl = productPathImage
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
                    case 105 when grantResults.Length > 0 && grantResults[0] == Permission.Granted:
                        //Open intent Location when the request code of result is 502
                        new IntentController(this).OpenIntentLocation();
                        break;
                    case 105:
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
                switch (TypeDialog)
                {
                    case "Categories":
                        CategoryId = CategoriesController.ListCategoriesProducts.FirstOrDefault(categories => categories.CategoriesName == itemString.ToString())?.CategoriesId;
                        TxtCategory.Text = itemString.ToString();
                        break;
                    case "Currency":
                        TxtCurrency.Text = itemString.ToString();
                        CurrencyId = WoWonderTools.GetIdCurrency(itemString.ToString());
                        break;
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

        #endregion

        private void GetPlaceFromPicker(Intent data)
        {
            try
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

                        PlaceText = placeAddress;
                        TxtLocation.Text = PlaceText;
                        break;
                    }
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

        public void OnFocusChange(View v, bool hasFocus)
        {
            if (v?.Id == TxtLocation.Id && hasFocus)
            {
                TxtLocationOnFocusChange();
            }
        }
    }
} 
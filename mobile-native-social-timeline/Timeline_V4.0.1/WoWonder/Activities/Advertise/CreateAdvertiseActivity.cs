using System;
using System.Collections.Generic;
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
using AndroidX.Core.Content;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using TheArtOfDev.Edmodo.Cropper;
using Java.IO;
using Java.Lang;
using WoWonder.Activities.Base;
using WoWonder.Activities.MyProfile;
using WoWonder.Activities.NativePost.Extra;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Advertise;
using WoWonderClient.Requests;
using static WoWonder.Helpers.Controller.PopupDialogController;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.Advertise
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class CreateAdvertiseActivity : BaseActivity, MaterialDialog.IListCallback , MaterialDialog.ISingleButtonCallback 
    {
        #region Variables Basic

        private TextView TxtAdd , BtnSelectImage;
        private ImageView ImageAd;
        private EditText TxtName ,TxtTitle, TxtDescription, TxtStartDate, TxtEndDate, TxtWebsite, TxtMyPages, TxtLocation, TxtAudience, TxtGender, TxtPlacement, TxtBudget, TxtBidding;
        private string PathImage, GenderStatus, TotalIdAudienceChecked, PlacementStatus, BiddingStatus, TypeDialog;
        private PublisherAdView PublisherAdView;
        private TabbedMainActivity GlobalContextTabbed;

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
                SetContentView(Resource.Layout.CreateAdvertiseLayout);

                GlobalContextTabbed = TabbedMainActivity.GetInstance();

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

                TxtName = FindViewById<EditText>(Resource.Id.NameEditText);

                ImageAd = FindViewById<ImageView>(Resource.Id.image);
                BtnSelectImage = FindViewById<TextView>(Resource.Id.ChooseImageText);

                TxtTitle = FindViewById<EditText>(Resource.Id.TitleEditText); 
                TxtDescription = FindViewById<EditText>(Resource.Id.DescriptionEditText);
                TxtStartDate = FindViewById<EditText>(Resource.Id.StartDateEditText);
                TxtEndDate = FindViewById<EditText>(Resource.Id.EndDateEditText);
                TxtWebsite = FindViewById<EditText>(Resource.Id.WebsiteEditText);
                TxtMyPages = FindViewById<EditText>(Resource.Id.MyPagesEditText);
                TxtLocation = FindViewById<EditText>(Resource.Id.LocationEditText);
                TxtAudience = FindViewById<EditText>(Resource.Id.AudienceEditText);
                TxtGender = FindViewById<EditText>(Resource.Id.GenderEditText);
                TxtPlacement = FindViewById<EditText>(Resource.Id.PlacementEditText);
                TxtBudget = FindViewById<EditText>(Resource.Id.BudgetEditText);
                TxtBidding = FindViewById<EditText>(Resource.Id.BiddingEditText);

                PublisherAdView = FindViewById<PublisherAdView>(Resource.Id.multiple_ad_sizes_view);
                AdsGoogle.InitPublisherAdView(PublisherAdView);
                    
                Methods.SetColorEditText(TxtName, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtTitle, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtDescription, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtStartDate, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtEndDate, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtWebsite, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtMyPages, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtLocation, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtAudience, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtGender, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtPlacement, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtBudget, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtBidding, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                Methods.SetFocusable(TxtStartDate);
                Methods.SetFocusable(TxtEndDate);
                Methods.SetFocusable(TxtMyPages);
                Methods.SetFocusable(TxtLocation);
                Methods.SetFocusable(TxtAudience);
                Methods.SetFocusable(TxtGender);
                Methods.SetFocusable(TxtPlacement);
                Methods.SetFocusable(TxtBidding); 
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
                    toolBar.Title = GetString(Resource.String.Lbl_Create_Ad);
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
                        TxtAdd.Click += TxtAddOnClick;
                        BtnSelectImage.Click += BtnSelectImageOnClick;
                        TxtStartDate.Touch += TxtStartDateOnTouch;
                        TxtEndDate.Touch += TxtEndDateOnTouch;
                        TxtMyPages.Touch += TxtMyPagesOnTouch;
                        TxtLocation.Touch += TxtLocationOnTouch;
                        TxtAudience.Touch += TxtAudienceOnTouch;
                        TxtGender.Touch += TxtGenderOnTouch;
                        TxtPlacement.Touch += TxtPlacementOnTouch;
                        TxtBidding.Touch += TxtBiddingOnTouch;
                        break;
                    default:
                        TxtAdd.Click -= TxtAddOnClick;
                        BtnSelectImage.Click -= BtnSelectImageOnClick;
                        TxtStartDate.Touch -= TxtStartDateOnTouch;
                        TxtEndDate.Touch -= TxtEndDateOnTouch;
                        TxtMyPages.Touch -= TxtMyPagesOnTouch;
                        TxtLocation.Touch -= TxtLocationOnTouch;
                        TxtAudience.Touch -= TxtAudienceOnTouch;
                        TxtGender.Touch -= TxtGenderOnTouch;
                        TxtPlacement.Touch -= TxtPlacementOnTouch;
                        TxtBidding.Touch -= TxtBiddingOnTouch;
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
                TxtName = null!;
                ImageAd = null!;
                BtnSelectImage = null!;
                TxtTitle = null!;
                TxtDescription = null!;
                TxtStartDate = null!;
                TxtEndDate = null!;
                TxtWebsite = null!;
                TxtMyPages = null!;
                TxtLocation = null!;
                TxtAudience = null!;
                TxtGender = null!;
                TxtPlacement = null!;
                TxtBudget = null!;
                TxtBidding = null!;

                PublisherAdView = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
      
        #endregion

        #region Events

        //Add Image
        private void BtnSelectImageOnClick(object sender, EventArgs e)
        {
            try
            {
                OpenDialogGallery(); //requestCode >> 500 => Image Gallery 
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        private void TxtBiddingOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                TypeDialog = "Bidding";

                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                arrayAdapter.Add(GetText(Resource.String.Lbl_BiddingClick)); //clicks
                arrayAdapter.Add(GetText(Resource.String.Lbl_BiddingViews)); //views

                dialogList.Title(GetText(Resource.String.Lbl_Bidding)).TitleColorRes(Resource.Color.primary);
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

        private void TxtPlacementOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                TypeDialog = "Placement";

                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                arrayAdapter.Add(GetText(Resource.String.Lbl_PlacementPost)); //post
                arrayAdapter.Add(GetText(Resource.String.Lbl_PlacementSidebar)); //sidebar

                dialogList.Title(GetText(Resource.String.Lbl_Placement)).TitleColorRes(Resource.Color.primary);
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
          
        private void TxtAudienceOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                TypeDialog = "Audience";

                var arrayIndexAdapter = new int[] { };
                var countriesArray = WoWonderTools.GetCountryList(this);
                
                var arrayAdapter = countriesArray.Select(item => item.Value).ToList();
               
                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);
                dialogList.Title(GetText(Resource.String.Lbl_Audience)).TitleColorRes(Resource.Color.primary)
                    .Items(arrayAdapter)
                    .ItemsCallbackMultiChoice(arrayIndexAdapter, OnSelection)
                    .AlwaysCallMultiChoiceCallback()
                    .AutoDismiss(false)
                    .PositiveText(GetText(Resource.String.Lbl_Close)).OnPositive(this)
                    .NeutralText(Resource.String.Lbl_SelectAll).OnNeutral((dialog, action) =>
                    {
                        try
                        { 
                            dialog.SelectAllIndices();

                            TotalIdAudienceChecked = "";
                            foreach (var item in countriesArray)
                            {
                                TotalIdAudienceChecked += item.Key + ",";
                            } 
                        }
                        catch (Exception ex)
                        {
                            Methods.DisplayReportResultTrack(ex);
                        }
                    })
                    .Build().Show();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void TxtMyPagesOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                TypeDialog = "MyPages";

                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                switch (ListUtils.MyPageList?.Count)
                {
                    case > 0:
                        arrayAdapter.AddRange(ListUtils.MyPageList.Select(item => item.PageName));
                        break;
                }

                dialogList.Title(GetString(Resource.String.Lbl_MyPages)).TitleColorRes(Resource.Color.primary);
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
         
        private void TxtGenderOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                TypeDialog = "Genders";

                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                arrayAdapter.Add(GetText(Resource.String.Lbl_All));

                switch (ListUtils.SettingsSiteList?.Genders?.Count)
                {
                    case > 0:
                        arrayAdapter.AddRange(from item in ListUtils.SettingsSiteList?.Genders select item.Value);
                        break;
                    default:
                        arrayAdapter.Add(GetText(Resource.String.Radio_Male));
                        arrayAdapter.Add(GetText(Resource.String.Radio_Female));
                        break;
                }

                dialogList.Title(GetText(Resource.String.Lbl_Gender)).TitleColorRes(Resource.Color.primary);
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

        private void TxtLocationOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                TypeDialog = "Country";

                var countriesArray = WoWonderTools.GetCountryList(this);

                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                var arrayAdapter = countriesArray.Select(item => item.Value).ToList();

                dialogList.Title(GetText(Resource.String.Lbl_Location)).TitleColorRes(Resource.Color.primary);
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

        private void TxtEndDateOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                var frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    TxtEndDate.Text = time.Date.ToString("yyyy-MM-dd");
                });

                frag.Show(SupportFragmentManager, DatePickerFragment.Tag);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void TxtStartDateOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                var frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    TxtStartDate.Text = time.Date.ToString("yyyy-MM-dd");
                });

                frag.Show(SupportFragmentManager, DatePickerFragment.Tag);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        //Save 
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
                    if (string.IsNullOrEmpty(TxtName.Text) || string.IsNullOrWhiteSpace(TxtName.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_name), ToastLength.Short)?.Show();
                        return;
                    }
                     
                    if (string.IsNullOrEmpty(TxtTitle.Text) || string.IsNullOrWhiteSpace(TxtTitle.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_title), ToastLength.Short)?.Show();
                        return;
                    }
                     
                    if (string.IsNullOrEmpty(TxtDescription.Text) || string.IsNullOrWhiteSpace(TxtDescription.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_Description), ToastLength.Short)?.Show();
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtStartDate.Text) || string.IsNullOrWhiteSpace(TxtStartDate.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_start_date), ToastLength.Short)?.Show();
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(TxtEndDate.Text) || string.IsNullOrWhiteSpace(TxtEndDate.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_end_date), ToastLength.Short)?.Show();
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(TxtWebsite.Text) || string.IsNullOrWhiteSpace(TxtWebsite.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_Website), ToastLength.Short)?.Show();
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(TxtMyPages.Text) || string.IsNullOrWhiteSpace(TxtMyPages.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_page), ToastLength.Short)?.Show();
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(TxtLocation.Text) || string.IsNullOrWhiteSpace(TxtLocation.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_Location), ToastLength.Short)?.Show();
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(TxtAudience.Text) || string.IsNullOrWhiteSpace(TxtAudience.Text) || string.IsNullOrEmpty(TotalIdAudienceChecked))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_Audience), ToastLength.Short)?.Show();
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(TxtGender.Text) || string.IsNullOrWhiteSpace(TxtGender.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_Gender), ToastLength.Short)?.Show();
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(TxtPlacement.Text) || string.IsNullOrWhiteSpace(TxtPlacement.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_Placement), ToastLength.Short)?.Show();
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(TxtBudget.Text) || string.IsNullOrWhiteSpace(TxtBudget.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_Budget), ToastLength.Short)?.Show();
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(TxtBidding.Text) || string.IsNullOrWhiteSpace(TxtBidding.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_Bidding), ToastLength.Short)?.Show();
                        return;
                    }

                    if (string.IsNullOrEmpty(PathImage))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_Image), ToastLength.Short)?.Show();
                        return;
                    }

                    //Show a progress
                    AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));

                    TotalIdAudienceChecked = TotalIdAudienceChecked.Length switch
                    {
                        > 0 => TotalIdAudienceChecked.Remove(TotalIdAudienceChecked.Length - 1, 1),
                        _ => TotalIdAudienceChecked
                    };

                    var dictionary = new Dictionary<string, string>
                    {
                        {"name", TxtName.Text},
                        {"website",TxtWebsite.Text},
                        {"headline",TxtTitle.Text}, 
                        {"description", TxtDescription.Text},
                        {"bidding", BiddingStatus},
                        {"appears", PlacementStatus},
                        {"audience-list", TotalIdAudienceChecked},
                        {"gender", GenderStatus},
                        {"location", TxtLocation.Text}, 
                        {"start", TxtStartDate.Text},
                        {"end", TxtEndDate.Text},
                    };

                    var (apiStatus, respond) = await RequestsAsync.Advertise.CreateAdvertiseAsync(dictionary, PathImage);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case CreateAdvertiseObject result:
                                {
                                    AndHUD.Shared.Dismiss(this);
                                    Toast.MakeText(this, GetText(Resource.String.Lbl_CreatedSuccessfully), ToastLength.Short)?.Show();

                                    //Add new item to list
                                    if (result.Data?.PostClass != null)
                                    {
                                        result.Data.Value.PostClass.PostType = "ad";

                                        var countList = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ItemCount;
                                 
                                        var combine = new FeedCombiner(ApiPostAsync.RegexFilterText(result.Data.Value.PostClass), GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer, this);
                                        combine.AddAdsPost();

                                        int countIndex = 1;
                                        var model1 = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.Story);
                                        var model2 = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                                        var model3 = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.FilterSection);
                                        var model4 = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AlertBox);
                                        var model5 = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SearchForPosts);

                                        if (model5 != null)
                                            countIndex += GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(model5) + 1;
                                        else if (model4 != null)
                                            countIndex += GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(model4) + 1;
                                        else if (model3 != null)
                                            countIndex += GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(model3) + 1;
                                        else if (model2 != null)
                                            countIndex += GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(model2) + 1;
                                        else if (model1 != null)
                                            countIndex += GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.IndexOf(model1) + 1;
                                        else
                                            countIndex = 0;

                                        var emptyStateChecker = GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);
                                        if (emptyStateChecker != null && GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.Count > 1)
                                            GlobalContextTabbed.NewsFeedTab.MainRecyclerView.RemoveByRowIndex(emptyStateChecker);

                                        GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.NotifyItemRangeInserted(countIndex, GlobalContextTabbed.NewsFeedTab.PostFeedAdapter.ListDiffer.Count - countList);

                                        // My Profile
                                        MyProfileActivity myProfileActivity = MyProfileActivity.GetInstance();
                                        if (myProfileActivity != null)
                                        { 
                                            var countList1 = myProfileActivity.PostFeedAdapter.ItemCount;

                                            var combine1 = new FeedCombiner(ApiPostAsync.RegexFilterText(result.Data.Value.PostClass), myProfileActivity.PostFeedAdapter.ListDiffer, this);

                                            combine1.AddAdsPost();

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
                                                PathImage = resultUri.Path;
                                                File file2 = new File(resultUri.Path);
                                                var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);
                                                Glide.With(this).Load(photoUri).Apply(new RequestOptions()).Into(ImageAd);
                                 
                                                //GlideImageLoader.LoadImage(this, resultUri.Path, ImgCover, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);
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

        #region MaterialDialog

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                switch (TypeDialog)
                {
                    case "Genders" when itemString.ToString() == GetText(Resource.String.Lbl_All):
                        TxtGender.Text = GetText(Resource.String.Lbl_All);
                        GenderStatus = "all";
                        break;
                    case "Genders" when ListUtils.SettingsSiteList?.Genders?.Count > 0:
                    {
                        var key = ListUtils.SettingsSiteList?.Genders?.FirstOrDefault(a => a.Value == itemString.ToString()).Key;
                        if (key != null)
                        {
                            TxtGender.Text = itemString.ToString();
                            GenderStatus = key;
                        }
                        else
                        {
                            TxtGender.Text = itemString.ToString();
                            GenderStatus = "male";
                        }

                        break;
                    }
                    case "Genders" when itemString.ToString() == GetText(Resource.String.Radio_Male):
                        TxtGender.Text = GetText(Resource.String.Radio_Male);
                        GenderStatus = "male";
                        break;
                    case "Genders" when itemString.ToString() == GetText(Resource.String.Radio_Female):
                        TxtGender.Text = GetText(Resource.String.Radio_Female);
                        GenderStatus = "female";
                        break;
                    case "Genders":
                        TxtGender.Text = GetText(Resource.String.Radio_Male);
                        GenderStatus = "male";
                        break;
                    case "Country":
                        TxtLocation.Text = itemString.ToString();
                        break;
                    case "MyPages":
                    {
                        var dataPage = ListUtils.MyPageList[itemId];
                        if (dataPage != null)
                        {
                            TxtWebsite.Text = dataPage.Url;
                        }

                        TxtMyPages.Text = itemString.ToString();
                        break;
                    }
                    case "Placement":
                    {
                        if (itemString.ToString() == GetText(Resource.String.Lbl_PlacementPost))
                        {
                            PlacementStatus = "post";
                        }
                        else if (itemString.ToString() == GetText(Resource.String.Lbl_PlacementSidebar))
                        {
                            PlacementStatus = "sidebar";
                        } 

                        TxtPlacement.Text = itemString.ToString();
                        break;
                    }
                    case "Bidding":
                    {
                        if (itemString.ToString() == GetText(Resource.String.Lbl_BiddingClick))
                        {
                            BiddingStatus = "clicks";
                        }
                        else if (itemString.ToString() == GetText(Resource.String.Lbl_BiddingViews))
                        {
                            BiddingStatus = "views";
                        }

                        TxtBidding.Text = itemString.ToString();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private bool OnSelection(MaterialDialog dialog, int[] which, string[] text)
        {
            try
            {
                switch (TypeDialog)
                {
                    case "Audience":
                    {
                        TotalIdAudienceChecked = "";
                        var countriesArray = WoWonderTools.GetCountryList(this);
                        for (int i = 0; i < which.Length; i++)
                        {
                            var itemString = text[i];
                            var check = countriesArray.FirstOrDefault(a => a.Value == itemString).Key;
                            if (check != null)
                            {
                                TotalIdAudienceChecked += check + ",";
                            } 
                        }

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return true;
            }
            return true;
        }

        public void OnClick(MaterialDialog p0, DialogAction p1)
        {
            try
            {
                if (p1 == DialogAction.Positive)
                {
                    TxtAudience.Text = TypeDialog switch
                    {
                        "Audience" when !string.IsNullOrEmpty(TotalIdAudienceChecked) => GetText(Resource.String
                            .Lbl_Selected),
                        _ => TxtAudience.Text
                    };

                    p0.Dismiss();
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
         
    }
}
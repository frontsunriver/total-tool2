using System;
using System.Linq;
using AFollestad.MaterialDialogs;
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
using Java.Lang;
using Newtonsoft.Json;
using WoWonder.Activities.Base;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Page;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Communities.Pages
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class CreatePageActivity : BaseActivity, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private TextView TxtSave, IconTitle, IconUrl, IconCategories, IconAbout;
        private EditText TxtTitle, TxtUrl, TxtCategories, TxtAbout;
        private string CategoryId = "";
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
                SetContentView(Resource.Layout.CreatePage_Layout);

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
                TxtSave = FindViewById<TextView>(Resource.Id.toolbar_title);

                IconTitle = FindViewById<TextView>(Resource.Id.IconTitle);
                TxtTitle = FindViewById<EditText>(Resource.Id.TitleEditText);

                IconUrl = FindViewById<TextView>(Resource.Id.IconUrl);
                TxtUrl = FindViewById<EditText>(Resource.Id.UrlEditText);

                IconCategories = FindViewById<TextView>(Resource.Id.IconCategories);
                TxtCategories = FindViewById<EditText>(Resource.Id.CategoriesEditText);

                IconAbout = FindViewById<TextView>(Resource.Id.IconAbout);
                TxtAbout = FindViewById<EditText>(Resource.Id.AboutEditText);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconTitle, FontAwesomeIcon.UserFriends);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconUrl, FontAwesomeIcon.Link);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeBrands, IconCategories, FontAwesomeIcon.Buromobelexperte);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconAbout, FontAwesomeIcon.Paragraph);

                Methods.SetColorEditText(TxtTitle, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtUrl, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtCategories, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtAbout, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                
                Methods.SetFocusable(TxtCategories);

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
                    toolBar.Title = GetText(Resource.String.Lbl_Create_New_Page);
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
                        TxtSave.Click += TxtCreateOnClick;
                        TxtCategories.Touch += TxtCategoryOnClick;
                        break;
                    default:
                        TxtSave.Click -= TxtCreateOnClick;
                        TxtCategories.Touch -= TxtCategoryOnClick;
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

                TxtSave = null!;
                IconTitle = null!;
                IconUrl = null!;
                IconCategories = null!;
                IconAbout = null!;
                TxtTitle = null!;
                TxtUrl = null!;
                TxtCategories = null!;
                TxtAbout = null!;
                CategoryId = null!;
                PublisherAdView = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events

        private void TxtCategoryOnClick(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                switch (CategoriesController.ListCategoriesPage.Count)
                {
                    case > 0:
                    {
                        var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                        var arrayAdapter = CategoriesController.ListCategoriesPage.Select(item => item.CategoriesName).ToList();

                        dialogList.Title(GetText(Resource.String.Lbl_SelectCategories)).TitleColorRes(Resource.Color.primary);
                        dialogList.Items(arrayAdapter);
                        dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                        dialogList.AlwaysCallSingleChoiceCallback();
                        dialogList.ItemsCallback(this).Build().Show();
                        break;
                    }
                    default:
                        Methods.DisplayReportResult(this, "Not have List Categories Page");
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private async void TxtCreateOnClick(object sender, EventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    return;
                } 
                if (string.IsNullOrEmpty(TxtUrl.Text))
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_name), ToastLength.Short)?.Show();
                    return;
                } 
                if (string.IsNullOrEmpty(TxtTitle.Text))
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_title), ToastLength.Short)?.Show();
                    return;
                }
                if (string.IsNullOrEmpty(TxtAbout.Text))
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_about), ToastLength.Short)?.Show();
                    return;
                }
                if (string.IsNullOrEmpty(CategoryId))
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_category), ToastLength.Short)?.Show();
                    return;
                }

                //Show a progress
                AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading) + "...");

                var (apiStatus, respond) = await RequestsAsync.Page.CreatePageAsync(TxtUrl.Text.Replace(" ", ""), TxtTitle.Text, CategoryId, TxtAbout.Text);
                switch (apiStatus)
                {
                    case 200:
                    {
                        switch (respond)
                        {
                            case CreatePageObject result:
                            {
                                AndHUD.Shared.Dismiss(this);
                                Toast.MakeText(this, GetText(Resource.String.Lbl_CreatedSuccessfully), ToastLength.Short)?.Show();

                                Intent returnIntent = new Intent();
                                if (result.PageData != null)
                                    returnIntent?.PutExtra("pageItem", JsonConvert.SerializeObject(result.PageData));
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
            }
            catch (Exception exception)
            {
                AndHUD.Shared.Dismiss(this); 
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        #endregion

        #region MaterialDialog

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                CategoryId = CategoriesController.ListCategoriesPage.FirstOrDefault(categories => categories.CategoriesName == itemString.ToString())?.CategoriesId;
                TxtCategories.Text = itemString.ToString();
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
    }
}
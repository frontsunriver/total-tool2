using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using AndroidX.RecyclerView.Widget;
using Java.Lang;
using Newtonsoft.Json;
using WoWonder.Activities.Base;
using WoWonder.Adapters;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Communities.Pages.Settings
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class PageGeneralActivity : BaseActivity, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private TextView TxtSave, IconTitle, IconUrl, IconCategories, IconSubCategories, IconUsersPost;
        private EditText TxtTitle, TxtUrl, TxtCategories, TxtSubCategories;
        private LinearLayout SubCategoriesLayout;
        private RadioButton RadioEnable, RadioDisable;
        private RecyclerView MRecycler;
        private CustomFieldsAdapter MAdapter;
        private LinearLayoutManager LayoutManager;
        private string CategoryId = "", SubCategoryId = "", PagesId = "", DialogType , UsersPost;
        private PageClass PageData;
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
                SetContentView(Resource.Layout.PageGeneralLayout);

                var id = Intent?.GetStringExtra("PageId") ?? "Data not available";
                if (id != "Data not available" && !string.IsNullOrEmpty(id)) PagesId = id;

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();

                Get_Data_Page();
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

                SubCategoriesLayout = FindViewById<LinearLayout>(Resource.Id.LayoutSubCategories);
                IconSubCategories = FindViewById<TextView>(Resource.Id.IconSubCategories);
                TxtSubCategories = FindViewById<EditText>(Resource.Id.SubCategoriesEditText);
                SubCategoriesLayout.Visibility = ViewStates.Gone;

                MRecycler = FindViewById<RecyclerView>(Resource.Id.Recycler);
                MRecycler.Visibility = ViewStates.Gone;

                IconUsersPost = FindViewById<TextView>(Resource.Id.IconUsersPost);
                RadioEnable = FindViewById<RadioButton>(Resource.Id.radioEnable);
                RadioDisable = FindViewById<RadioButton>(Resource.Id.radioDisable);
                 
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconTitle, FontAwesomeIcon.UserFriends);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconUrl, FontAwesomeIcon.Link);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeBrands, IconCategories, FontAwesomeIcon.Buromobelexperte);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeBrands, IconSubCategories, FontAwesomeIcon.Buromobelexperte);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconUsersPost, FontAwesomeIcon.LayerPlus);

                Methods.SetColorEditText(TxtTitle, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtUrl, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtCategories, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtSubCategories, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                Methods.SetFocusable(TxtCategories);
                Methods.SetFocusable(TxtSubCategories);

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
                    toolBar.Title = GetText(Resource.String.Lbl_General);
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
                MAdapter = new CustomFieldsAdapter(this) { FieldList = new ObservableCollection<CustomField>() };
                LayoutManager = new LinearLayoutManager(this);
                MRecycler.SetLayoutManager(LayoutManager);
                MRecycler.HasFixedSize = true;
                MRecycler.SetItemViewCacheSize(10);
                MRecycler.GetLayoutManager().ItemPrefetchEnabled = true;
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
                        TxtCategories.Touch += TxtCategoryOnClick;
                        TxtSubCategories.Touch += TxtSubCategoriesOnTouch;
                        RadioEnable.CheckedChange += RadioEnableOnCheckedChange;
                        RadioDisable.CheckedChange += RadioDisableOnCheckedChange;
                        break;
                    default:
                        TxtSave.Click -= TxtSaveOnClick;
                        TxtCategories.Touch -= TxtCategoryOnClick;
                        TxtSubCategories.Touch -= TxtSubCategoriesOnTouch;
                        RadioEnable.CheckedChange -= RadioEnableOnCheckedChange;
                        RadioDisable.CheckedChange -= RadioDisableOnCheckedChange;
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
                IconSubCategories = null!;
                IconUsersPost = null!;
                TxtTitle = null!;
                TxtUrl = null!;
                TxtCategories = null!;
                TxtSubCategories = null!;
                SubCategoriesLayout = null!;
                MAdapter = null!;
                MRecycler = null!;
                RadioEnable = null!;
                RadioDisable = null!;
                LayoutManager = null!;
                CategoryId = null!;
                SubCategoryId = null!;
                PagesId = null!;
                DialogType = null!;
                UsersPost = null!;
                PageData = null!;
                PublisherAdView = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events

        private void RadioDisableOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            try
            {
                var isChecked = RadioDisable.Checked;
                switch (isChecked)
                {
                    case true:
                        RadioEnable.Checked = false;
                        UsersPost = "0";
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void RadioEnableOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            try
            {
                var isChecked = RadioEnable.Checked;
                switch (isChecked)
                {
                    case true:
                        RadioDisable.Checked = false;
                        UsersPost = "1";
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }


        private void TxtSubCategoriesOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                switch (CategoriesController.ListCategoriesPage.Count)
                {
                    case > 0:
                    {
                        DialogType = "SubCategories";

                        var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                        var arrayAdapter = new List<string>();

                        var subCat = CategoriesController.ListCategoriesPage.FirstOrDefault(a => a.CategoriesId == CategoryId)?.SubList;
                        arrayAdapter = subCat?.Count switch
                        {
                            > 0 => subCat.Select(item => item.Lang).ToList(),
                            _ => arrayAdapter
                        };

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

        private void TxtCategoryOnClick(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                switch (CategoriesController.ListCategoriesPage.Count)
                {
                    case > 0:
                    {
                        DialogType = "Categories";

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

        private async void TxtSaveOnClick(object sender, EventArgs e)
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
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_title), ToastLength.Short)?.Show();
                        return;
                    }
                    if (string.IsNullOrEmpty(TxtUrl.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_name), ToastLength.Short)?.Show();
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(TxtCategories.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_category), ToastLength.Short)?.Show();
                        return;
                    }

                    //Show a progress
                    AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading) + "...");

                    var dictionary = new Dictionary<string, string>
                    {
                        {"page_title", TxtTitle.Text},
                        {"page_name", TxtUrl.Text.Replace(" " , "")},
                        {"page_category", CategoryId},
                        {"users_post", UsersPost},
                        {"sub_category", SubCategoryId},
                    };

                    switch (MAdapter.FieldList.Count)
                    {
                        case > 0:
                        {
                            foreach (var field in MAdapter.FieldList)
                            {
                                dictionary.Add(field.Fid, field.FieldAnswer);
                            }

                            break;
                        }
                    }

                    var (apiStatus, respond) = await RequestsAsync.Page.UpdatePageDataAsync(PagesId, dictionary);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case MessageObject result:
                                {
                                    AndHUD.Shared.Dismiss(this);
                                    Console.WriteLine(result.Message);

                                    PageData.PageTitle = TxtTitle.Text;
                                    PageData.Username = TxtUrl.Text;
                                    PageData.Category = TxtCategories.Text;

                                    PageData.PageCategory = CategoryId;

                                    PageProfileActivity.PageData = PageData;
                          
                                    Toast.MakeText(this, GetText(Resource.String.Lbl_YourPageWasUpdated), ToastLength.Short)?.Show();
                              
                                    Intent returnIntent = new Intent();
                                    returnIntent?.PutExtra("pageItem", JsonConvert.SerializeObject(PageData));
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
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                AndHUD.Shared.Dismiss(this);
            }
        }

        #endregion

        #region MaterialDialog

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                switch (DialogType)
                {
                    case "Categories":
                    {
                        var category = CategoriesController.ListCategoriesPage.FirstOrDefault(categories => categories.CategoriesName == itemString.ToString());
                        if (category != null)
                        {
                            CategoryId = category.CategoriesId;

                            switch (category.SubList.Count)
                            {
                                case > 0:
                                    SubCategoriesLayout.Visibility = ViewStates.Visible;
                                    TxtSubCategories.Text = "";
                                    SubCategoryId = "";
                                    break;
                                default:
                                    SubCategoriesLayout.Visibility = ViewStates.Gone;
                                    SubCategoryId = "";
                                    break;
                            }
                        }
                        TxtCategories.Text = itemString.ToString();
                        break;
                    }
                    case "SubCategories":
                    {
                        var category = CategoriesController.ListCategoriesPage.FirstOrDefault(categories => categories.CategoriesId == CategoryId)?.SubList.FirstOrDefault(sub => sub.LangKey == itemString.ToString());
                        if (category != null)
                        {
                            SubCategoryId = category.CategoryId;
                        }
                        TxtSubCategories.Text = itemString.ToString();
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

        //Get Data Page and set Categories
        private void Get_Data_Page()
        {
            try
            {
                PageData = JsonConvert.DeserializeObject<PageClass>(Intent?.GetStringExtra("PageData"));
                if (PageData != null)
                {
                    TxtTitle.Text = PageData.PageTitle;
                    TxtUrl.Text = PageData.Username;

                    TxtCategories.Text = Methods.FunString.DecodeString(PageData.Category);
                    CategoryId = PageData.PageCategory;

                    switch (string.IsNullOrEmpty(PageData.PageSubCategory))
                    {
                        case false:
                        {
                            var category = CategoriesController.ListCategoriesPage.FirstOrDefault(categories => categories.CategoriesId == CategoryId)?.SubList.FirstOrDefault(sub => sub.CategoryId == PageData.PageSubCategory);
                            if (category != null)
                            {
                                TxtSubCategories.Text = category.Lang;
                                SubCategoryId = category.CategoryId;
                                SubCategoriesLayout.Visibility = ViewStates.Visible;
                            }

                            break;
                        }
                    }

                    UsersPost = PageData.UsersPost;

                    switch (PageData.UsersPost)
                    {
                        //Enable
                        case "1":
                            RadioDisable.Checked = false;
                            RadioEnable.Checked = true;
                            break;
                        //Disable
                        default:
                            RadioDisable.Checked = true;
                            RadioEnable.Checked = false;
                            break;
                    }

                    switch (ListUtils.SettingsSiteList?.PageCustomFields?.Count)
                    {
                        case > 0:
                            MAdapter.FieldList = new ObservableCollection<CustomField>(ListUtils.SettingsSiteList?.PageCustomFields);
                            MAdapter.NotifyDataSetChanged();

                            MRecycler.Visibility = ViewStates.Visible;
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
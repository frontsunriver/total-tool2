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
 
namespace WoWonder.Activities.Communities.Groups.Settings
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class GroupGeneralActivity : BaseActivity, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private TextView TxtCreate, IconTitle, IconUrl, IconAbout, IconCategories, IconSubCategories;
        private EditText TxtTitle, TxtUrl, TxtAbout, TxtCategories, TxtSubCategories;
        private LinearLayout SubCategoriesLayout; 
        private RecyclerView MRecycler;
        private CustomFieldsAdapter MAdapter;
        private LinearLayoutManager LayoutManager;
        private string GroupsId, CategoryId = "", SubCategoryId = "", TypeDialog = "";
        private GroupClass GroupData;
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
                SetContentView(Resource.Layout.GroupGeneralLayout);

                var id = Intent?.GetStringExtra("GroupId") ?? "Data not available";
                if (id != "Data not available" && !string.IsNullOrEmpty(id)) GroupsId = id;

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();
                Get_Data_Group();
                AdsGoogle.Ad_Interstitial(this);
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
                TxtCreate = FindViewById<TextView>(Resource.Id.toolbar_title);

                IconTitle = FindViewById<TextView>(Resource.Id.IconTitle);
                TxtTitle = FindViewById<EditText>(Resource.Id.TitleEditText);

                IconUrl = FindViewById<TextView>(Resource.Id.IconUrl);
                TxtUrl = FindViewById<EditText>(Resource.Id.UrlEditText);

                IconAbout = FindViewById<TextView>(Resource.Id.IconAbout);
                TxtAbout = FindViewById<EditText>(Resource.Id.AboutEditText);

                IconCategories = FindViewById<TextView>(Resource.Id.IconCategories);
                TxtCategories = FindViewById<EditText>(Resource.Id.CategoriesEditText);

                SubCategoriesLayout = FindViewById<LinearLayout>(Resource.Id.LayoutSubCategories);
                IconSubCategories = FindViewById<TextView>(Resource.Id.IconSubCategories);
                TxtSubCategories = FindViewById<EditText>(Resource.Id.SubCategoriesEditText);
                SubCategoriesLayout.Visibility = ViewStates.Gone;

                MRecycler = FindViewById<RecyclerView>(Resource.Id.Recycler);
                MRecycler.Visibility = ViewStates.Gone;

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconTitle, FontAwesomeIcon.UserFriends);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconUrl, FontAwesomeIcon.Link);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconAbout, FontAwesomeIcon.Paragraph);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeBrands, IconCategories, FontAwesomeIcon.Buromobelexperte);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeBrands, IconSubCategories, FontAwesomeIcon.Buromobelexperte);
                 
                Methods.SetColorEditText(TxtTitle, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtUrl, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtCategories, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtAbout, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
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
                MAdapter = new CustomFieldsAdapter(this) { FieldList = new ObservableCollection<CustomField>()};
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
                        TxtCreate.Click += TxtCreateOnClick;
                        TxtCategories.Touch += TxtCategoryOnClick;
                        TxtSubCategories.Touch += TxtSubCategoriesOnTouch;
                        break;
                    default:
                        TxtCreate.Click -= TxtCreateOnClick;
                        TxtCategories.Touch -= TxtCategoryOnClick;
                        TxtSubCategories.Touch -= TxtSubCategoriesOnTouch;
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

                TxtCreate = null!;
                IconTitle = null!;
                IconUrl = null!;
                IconAbout = null!;
                IconCategories = null!;
                IconSubCategories = null!;
                TxtTitle = null!;
                TxtUrl = null!;
                TxtAbout = null!;
                TxtCategories = null!;
                TxtSubCategories = null!;
                SubCategoriesLayout = null!;
                MRecycler = null!;
                MAdapter = null!;
                LayoutManager = null!;
                GroupsId = null!;
                CategoryId = "";
                SubCategoryId = "";
                TypeDialog = "";
                GroupData = null!;

                PublisherAdView = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events

        private void TxtSubCategoriesOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                switch (CategoriesController.ListCategoriesGroup.Count)
                {
                    case > 0:
                    {
                        TypeDialog = "SubCategories";

                        var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                        var arrayAdapter = new List<string>();

                        var subCat = CategoriesController.ListCategoriesGroup.FirstOrDefault(a => a.CategoriesId == CategoryId)?.SubList;
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
                        Methods.DisplayReportResult(this, "Not have List Categories Group");
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

                switch (CategoriesController.ListCategoriesGroup.Count)
                {
                    case > 0:
                    {
                        TypeDialog = "Categories";

                        var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                        var arrayAdapter = CategoriesController.ListCategoriesGroup.Select(item => item.CategoriesName).ToList();

                        dialogList.Title(GetText(Resource.String.Lbl_SelectCategories)).TitleColorRes(Resource.Color.primary);
                        dialogList.Items(arrayAdapter);
                        dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                        dialogList.AlwaysCallSingleChoiceCallback();
                        dialogList.ItemsCallback(this).Build().Show();
                        break;
                    }
                    default:
                        Methods.DisplayReportResult(this, "Not have List Categories Group");
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
                if (string.IsNullOrEmpty(TxtAbout.Text))
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_about), ToastLength.Short)?.Show();
                    return;
                }
                if (string.IsNullOrEmpty(TxtCategories.Text))
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_Please_select_category), ToastLength.Short)?.Show();
                    return;
                }

                //Show a progress
                AndHUD.Shared.Show(this, GetString(Resource.String.Lbl_Loading) + "...");

                var dictionary = new Dictionary<string, string>
                {
                    {"group_title", TxtTitle.Text},
                    {"group_name", TxtUrl.Text.Replace(" " , "")},
                    {"about", TxtAbout.Text},
                    {"category", CategoryId},
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

                var (apiStatus, respond) = await RequestsAsync.Group.UpdateGroupDataAsync(GroupsId, dictionary);
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
                                GroupData.GroupName = TxtUrl.Text;
                                GroupData.GroupTitle = TxtTitle.Text;
                                GroupData.Username = TxtUrl.Text;
                                GroupData.About = TxtAbout.Text;
                                GroupData.CategoryId = CategoryId;
                                GroupData.Category = TxtCategories.Text;

                                GroupProfileActivity.GroupDataClass = GroupData;

                                Toast.MakeText(this, GetText(Resource.String.Lbl_YourGroupWasUpdated), ToastLength.Short)?.Show();

                                Intent returnIntent = new Intent();
                                returnIntent?.PutExtra("groupItem", JsonConvert.SerializeObject(GroupData));
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
                switch (TypeDialog)
                {
                    case "Categories":
                    {
                        var category = CategoriesController.ListCategoriesGroup.FirstOrDefault(categories => categories.CategoriesName == itemString.ToString());
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
                        var category = CategoriesController.ListCategoriesGroup.FirstOrDefault(categories => categories.CategoriesId == CategoryId)?.SubList.FirstOrDefault(sub => sub.LangKey == itemString.ToString());
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

        //Get Data Group and set Categories
        private void Get_Data_Group()
        {
            try
            {
                GroupData = JsonConvert.DeserializeObject<GroupClass>(Intent?.GetStringExtra("GroupData"));
                if (GroupData != null)
                {
                    TxtTitle.Text = GroupData.GroupTitle;
                    TxtUrl.Text = GroupData.Username;
                    TxtAbout.Text = Methods.FunString.DecodeString(GroupData.About);
                    TxtCategories.Text = CategoriesController.ListCategoriesGroup.FirstOrDefault(categories => categories.CategoriesId == GroupData.CategoryId)?.CategoriesName ?? GroupData.Category;

                    CategoryId = GroupData.CategoryId;

                    switch (string.IsNullOrEmpty(GroupData.SubCategory))
                    {
                        case false:
                        {
                            var category = CategoriesController.ListCategoriesGroup.FirstOrDefault(categories => categories.CategoriesId == CategoryId)?.SubList.FirstOrDefault(sub => sub.CategoryId == GroupData.SubCategory);
                            if (category != null)
                            {
                                SubCategoriesLayout.Visibility = ViewStates.Visible;
                                TxtSubCategories.Text = category.Lang;
                                SubCategoryId = category.CategoryId;
                            }

                            break;
                        }
                    }
                    
                    switch (ListUtils.SettingsSiteList?.GroupCustomFields?.Count)
                    {
                        case > 0:
                            MAdapter.FieldList = new ObservableCollection<CustomField>(ListUtils.SettingsSiteList?.GroupCustomFields);
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

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
using Newtonsoft.Json;
using WoWonder.Activities.Base;
using WoWonder.Activities.Jobs.Adapters;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Jobs;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.Jobs
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class CreateJobActivity : BaseActivity, View.IOnFocusChangeListener, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    { 
        #region Variables Basic

        private TextView TxtSave;
        private TextView IconTitle, IconLocation, IconSalary, IconCurrency, IconJobType, IconDescription, IconCategory, TxtAddImg;
        private EditText TxtTitle, TxtLocation, TxtMinimum, TxtMaximum, TxtCurrency, TxtSalaryDate, TxtJobType, TxtDescription, TxtCategory;
        public EditText TxtAddQuestion;
        private ImageView Image;
        private RecyclerView MRecycler;
        private LinearLayoutManager LayoutManager;
        private QuestionAdapter MAdapter;
        private string TypeDialog, CategoryId, JobTypeId, SalaryDateId, CurrencyId, ImagePath, PageId, Lat, Lng;

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
                SetContentView(Resource.Layout.CreateJobLayout);

                PageId = Intent?.GetStringExtra("PageId") ?? "";

                //Get Value And Set Toolbar
                InitComponent();
                SetRecyclerViewAdapters();
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

                IconTitle = FindViewById<TextView>(Resource.Id.IconTitle);
                TxtTitle = FindViewById<EditText>(Resource.Id.TitleEditText);

                IconLocation = FindViewById<TextView>(Resource.Id.IconLocation);
                TxtLocation = FindViewById<EditText>(Resource.Id.LocationEditText);

                IconSalary = FindViewById<TextView>(Resource.Id.IconSalary);
                TxtMinimum = FindViewById<EditText>(Resource.Id.MinimumEditText);
                TxtMaximum = FindViewById<EditText>(Resource.Id.MaximumEditText);

                IconCurrency = FindViewById<TextView>(Resource.Id.IconCurrency);
                TxtCurrency = FindViewById<EditText>(Resource.Id.CurrencyEditText);
                TxtSalaryDate = FindViewById<EditText>(Resource.Id.SalaryDateEditText);

                IconJobType = FindViewById<TextView>(Resource.Id.IconJobType);
                TxtJobType = FindViewById<EditText>(Resource.Id.JobTypeEditText);

                IconCategory = FindViewById<TextView>(Resource.Id.IconCategory);
                TxtCategory = FindViewById<EditText>(Resource.Id.CategoryEditText);

                IconDescription = FindViewById<TextView>(Resource.Id.IconDescription);
                TxtDescription = FindViewById<EditText>(Resource.Id.DescriptionEditText);

                TxtAddQuestion  = FindViewById<EditText>(Resource.Id.AddQuestionEditText);
                TxtAddQuestion.Text = GetText(Resource.String.Lbl_AddQuestion) + "(0)";

                MRecycler = FindViewById<RecyclerView>(Resource.Id.Recyler);
                  
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconTitle, FontAwesomeIcon.User);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconLocation, FontAwesomeIcon.MapMarkedAlt);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconSalary, FontAwesomeIcon.MoneyBillAlt);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconCurrency, FontAwesomeIcon.DollarSign);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconJobType, FontAwesomeIcon.Briefcase);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconDescription, FontAwesomeIcon.Paragraph);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeBrands, IconCategory, FontAwesomeIcon.Buromobelexperte);

                Methods.SetColorEditText(TxtTitle, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtLocation, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtMinimum, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtMaximum, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtCurrency, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtSalaryDate, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtJobType, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtDescription, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtAddQuestion, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtCategory, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                Methods.SetFocusable(TxtCurrency);
                Methods.SetFocusable(TxtSalaryDate);
                Methods.SetFocusable(TxtJobType);
                Methods.SetFocusable(TxtCategory);
                Methods.SetFocusable(TxtAddQuestion);
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
                    toolBar.Title =  GetText(Resource.String.Lbl_CreateJob);
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
                MAdapter = new QuestionAdapter(this) { QuestionList = new ObservableCollection<QuestionJob>() }; 
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
                        TxtLocation.OnFocusChangeListener = this; 
                        TxtSalaryDate.Touch += TxtSalaryDateOnTouch;
                        TxtJobType.Touch += TxtJobTypeOnTouch;
                        TxtCategory.Touch += TxtCategoryOnTouch;
                        TxtCurrency.Touch += TxtCurrencyOnTouch;
                        TxtAddImg.Click += TxtAddImgOnClick;
                        TxtAddQuestion.Touch += TxtAddQuestionOnTouch;
                        break;
                    default:
                        TxtSave.Click -= TxtSaveOnClick;
                        TxtLocation.OnFocusChangeListener = null!; 
                        TxtSalaryDate.Touch -= TxtSalaryDateOnTouch;
                        TxtJobType.Touch -= TxtJobTypeOnTouch;
                        TxtCategory.Touch -= TxtCategoryOnTouch;
                        TxtCurrency.Touch -= TxtCurrencyOnTouch;
                        TxtAddImg.Click -= TxtAddImgOnClick;
                        TxtAddQuestion.Touch -= TxtAddQuestionOnTouch;
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
                TxtSave = null!;
                Image = null!;
                TxtAddImg = null!;
                IconTitle = null!;
                TxtTitle = null!;
                IconLocation = null!;
                TxtLocation = null!;
                IconSalary = null!;
                TxtMinimum = null!;
                TxtMaximum = null!;
                IconCurrency = null!;
                TxtCurrency = null!;
                TxtSalaryDate = null!;
                IconJobType = null!;
                TxtJobType = null!;
                IconCategory = null!;
                TxtCategory = null!;
                IconDescription = null!;
                TxtDescription = null!;
                TxtAddQuestion = null!;
                MRecycler = null!;
                MAdapter = null!;
                LayoutManager = null!;
                TypeDialog = null!;
                CategoryId = null!;
                JobTypeId = null!;
                SalaryDateId = null!;
                CurrencyId = null!;
                ImagePath = null!;
                PageId = null!;
                Lat = null!;
                Lng = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        private void TxtAddQuestionOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                switch (MAdapter.ItemCount)
                {
                    case < 4:
                    {
                        TypeDialog = "AddQuestion";

                        var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                        var arrayAdapter = WoWonderTools.GetAddQuestionList(this).Select(item => item.Value).ToList();

                        dialogList.Title(GetText(Resource.String.Lbl_TypeQuestion)).TitleColorRes(Resource.Color.primary);
                        dialogList.Items(arrayAdapter);
                        dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                        dialogList.AlwaysCallSingleChoiceCallback();
                        dialogList.ItemsCallback(this).Build().Show();
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

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
         
        private void TxtCategoryOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                switch (CategoriesController.ListCategoriesJob.Count)
                {
                    case > 0:
                    {
                        TypeDialog = "Categories";

                        var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                        var arrayAdapter = CategoriesController.ListCategoriesJob.Select(item => item.CategoriesName).ToList();

                        dialogList.Title(GetText(Resource.String.Lbl_SelectCategories)).TitleColorRes(Resource.Color.primary);
                        dialogList.Items(arrayAdapter);
                        dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                        dialogList.AlwaysCallSingleChoiceCallback();
                        dialogList.ItemsCallback(this).Build().Show();
                        break;
                    }
                    default:
                        Methods.DisplayReportResult(this, "Not have List Categories Job");
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void TxtJobTypeOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                TypeDialog = "JobType";
                var arrayAdapter = WoWonderTools.GetJobTypeList(this).Select(item => item.Value).ToList();

                dialogList.Title(GetText(Resource.String.Lbl_JobType)).TitleColorRes(Resource.Color.primary);
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

        private void TxtSalaryDateOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                TypeDialog = "SalaryDate";

                var arrayAdapter = WoWonderTools.GetSalaryDateList(this).Select(item => item.Value).ToList();

                dialogList.Title(GetText(Resource.String.Lbl_SalaryDate)).TitleColorRes(Resource.Color.primary);
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
         
        private void TxtLocationOnClick()
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

        private async void TxtSaveOnClick(object sender, EventArgs e)
        {
            try
            {
                if (Methods.CheckConnectivity())
                {

                    if (string.IsNullOrEmpty(TxtTitle.Text) || string.IsNullOrEmpty(TxtLocation.Text) || string.IsNullOrEmpty(TxtMinimum.Text)
                        || string.IsNullOrEmpty(TxtMaximum.Text) || string.IsNullOrEmpty(TxtSalaryDate.Text) || string.IsNullOrEmpty(TxtJobType.Text)
                        || string.IsNullOrEmpty(TxtDescription.Text) || string.IsNullOrEmpty(TxtCategory.Text))
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
                        {"job_title", TxtTitle.Text},
                        {"description", TxtDescription.Text},
                        {"location", TxtLocation.Text},
                        {"job_type", JobTypeId},
                        {"category", CategoryId},
                        {"page_id", PageId},
                        {"lat", Lat},
                        {"lng", Lng},
                        {"minimum", TxtMinimum.Text},
                        {"maximum", TxtMaximum.Text},
                        {"salary_date", SalaryDateId},
                        {"currency", CurrencyId}, 
                        {"image_type", "upload"}, 
                    };

                    switch (MAdapter.QuestionList.Count)
                    {
                        case > 0:
                        {
                            for (int i = 0; i < MAdapter.QuestionList.Count; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                    {
                                        var question = MAdapter.QuestionList[i];
                                        switch (question)
                                        {
                                            case null:
                                                continue;
                                        }
                                        dictionary.Add("question_one", question.Question);
                                        dictionary.Add("question_one_type", question.QuestionType);
                                        dictionary.Add("question_one_answers", question.QuestionAnswer);
                                        break;
                                    }
                                    case 1:
                                    {
                                        var question = MAdapter.QuestionList[i];
                                        switch (question)
                                        {
                                            case null:
                                                continue;
                                        }
                                        dictionary.Add("question_two", question.Question);
                                        dictionary.Add("question_two_type", question.QuestionType);
                                        dictionary.Add("question_two_answers", question.QuestionAnswer);
                                        break;
                                    }
                                    case 2:
                                    {
                                        var question = MAdapter.QuestionList[i];
                                        switch (question)
                                        {
                                            case null:
                                                continue;
                                        }
                                        dictionary.Add("question_three", question.Question);
                                        dictionary.Add("question_three_type", question.QuestionType);
                                        dictionary.Add("question_three_answers", question.QuestionAnswer);
                                        break;
                                    }
                                }
                            }

                            break;
                        }
                    }
                     
                    var (apiStatus, respond) = await RequestsAsync.Jobs.CreateJobAsync(dictionary, ImagePath);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case CreateJobObject result:
                                {
                                    Toast.MakeText(this, GetString(Resource.String.Lbl_jobSuccessfullyAdded), ToastLength.Short)?.Show();

                                    AndHUD.Shared.Dismiss(this);
                             
                                    Intent intent = new Intent();
                                    intent.PutExtra("JobsItem", JsonConvert.SerializeObject(result.Data));
                                    SetResult(Result.Ok, intent);
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
                    case 502 when resultCode == Result.Ok:
                        GetPlaceFromPicker(data);
                        break;
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
                    case 105 when grantResults.Length > 0 && grantResults[0] == Permission.Granted:
                        //Open intent Camera when the request code of result is 503
                        new IntentController(this).OpenIntentLocation();
                        break;
                    case 105:
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Permission_is_denied), ToastLength.Long)?.Show();
                        break;
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
                    case "Categories":
                        CategoryId = CategoriesController.ListCategoriesJob.FirstOrDefault(categories => categories.CategoriesName == itemString.ToString())?.CategoriesId;
                        TxtCategory.Text = itemString.ToString();
                        break;
                    case "JobType":
                        JobTypeId = WoWonderTools.GetJobTypeList(this)?.FirstOrDefault(a => a.Value == itemString.ToString()).Key.ToString();
                        TxtJobType.Text = itemString.ToString();
                        break;
                    case "SalaryDate":
                        SalaryDateId = WoWonderTools.GetSalaryDateList(this)?.FirstOrDefault(a => a.Value == itemString.ToString()).Key.ToString();
                        TxtSalaryDate.Text = itemString.ToString();
                        break;
                    case "Currency":
                        TxtCurrency.Text = itemString.ToString();
                        CurrencyId = WoWonderTools.GetIdCurrency(itemString.ToString());
                        break;
                    case "AddQuestion":
                    {
                        TxtAddQuestion.Text = GetText(Resource.String.Lbl_AddQuestion) + "(" + MAdapter.ItemCount + ")";
                     
                        var addQuestionId = WoWonderTools.GetAddQuestionList(this)?.FirstOrDefault(a => a.Value == itemString.ToString()).Key.ToString();
                        //SetQuestionOne(addQuestionId);
                        MAdapter.QuestionList.Add(new QuestionJob
                        {
                            Id = MAdapter.ItemCount,
                            QuestionType = addQuestionId
                        });
                        MAdapter.NotifyItemInserted(MAdapter.QuestionList.IndexOf(MAdapter.QuestionList.Last()));
                        break;
                    }
                    case "AddQuestionAdapter":
                    {
                        TxtAddQuestion.Text = GetText(Resource.String.Lbl_AddQuestion) + "(" + MAdapter.ItemCount + ")";

                        var addQuestionId = WoWonderTools.GetAddQuestionList(this)?.FirstOrDefault(a => a.Value == itemString.ToString()).Key.ToString();

                        var data = MAdapter.QuestionList.FirstOrDefault(a => a.Id == ItemQuestionJob.Id && a.QuestionType == ItemQuestionJob.QuestionType);
                        if (data != null)
                        {
                            data.QuestionType = addQuestionId;
                            MAdapter.NotifyItemChanged(MAdapter.QuestionList.IndexOf(data));
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

        #region Question

        private QuestionJob ItemQuestionJob;

        public void OpenDialogSetQuestion(QuestionJob item)
        {
            try
            {
                ItemQuestionJob = item;

                TypeDialog = "AddQuestionAdapter";

                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light); 
                var arrayAdapter = WoWonderTools.GetAddQuestionList(this).Select(pair => pair.Value).ToList(); 
                dialogList.Title(GetText(Resource.String.Lbl_TypeQuestion)).TitleColorRes(Resource.Color.primary);
                dialogList.Items(arrayAdapter);
                dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                dialogList.AlwaysCallSingleChoiceCallback();
                dialogList.ItemsCallback(this).Build().Show();
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
                var placeLatLng = data.GetStringExtra("latLng") ?? "";
                TxtLocation.Text = string.IsNullOrEmpty(placeAddress) switch
                {
                    false => placeAddress,
                    _ => TxtLocation.Text
                };
                switch (string.IsNullOrEmpty(placeLatLng))
                {
                    case false:
                    {
                        var latLng = placeLatLng.Split(",");
                        Lat = latLng.First();
                        Lng = latLng.Last();
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
                TxtLocationOnClick();
            }
        }
    }
}
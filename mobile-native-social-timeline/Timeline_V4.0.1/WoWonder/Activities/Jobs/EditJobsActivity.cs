using System;
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
using Java.Lang;
using Newtonsoft.Json;
using WoWonder.Activities.Base;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Jobs;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Jobs
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class EditJobsActivity : BaseActivity, View.IOnFocusChangeListener, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic
      
        private TextView TxtSave;
        private TextView IconTitle, IconLocation, IconSalary, IconSalaryDate, IconJobType, IconDescription, IconCategory;
        private EditText TxtTitle, TxtLocation, TxtMinimum, TxtMaximum, TxtSalaryDate, TxtJobType, TxtDescription, TxtCategory;
        private string TypeDialog, CategoryId, JobTypeId, SalaryDateId;
        private JobInfoObject DataInfoObject;
         
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
                SetContentView(Resource.Layout.EditJobsLayout);

                var dataObject = Intent?.GetStringExtra("JobsObject");
                DataInfoObject = string.IsNullOrEmpty(dataObject) switch
                {
                    false => JsonConvert.DeserializeObject<JobInfoObject>(dataObject),
                    _ => DataInfoObject
                };

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                BindJobPost();
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

                IconTitle = FindViewById<TextView>(Resource.Id.IconTitle);
                TxtTitle = FindViewById<EditText>(Resource.Id.TitleEditText);

                IconLocation = FindViewById<TextView>(Resource.Id.IconLocation);
                TxtLocation = FindViewById<EditText>(Resource.Id.LocationEditText);
                
                IconSalary = FindViewById<TextView>(Resource.Id.IconSalary);
                TxtMinimum = FindViewById<EditText>(Resource.Id.MinimumEditText);
                TxtMaximum = FindViewById<EditText>(Resource.Id.MaximumEditText);

                IconSalaryDate = FindViewById<TextView>(Resource.Id.IconSalaryDate);
                TxtSalaryDate = FindViewById<EditText>(Resource.Id.SalaryDateEditText);

                IconJobType = FindViewById<TextView>(Resource.Id.IconJobType);
                TxtJobType = FindViewById<EditText>(Resource.Id.JobTypeEditText);

                IconCategory = FindViewById<TextView>(Resource.Id.IconCategory);
                TxtCategory = FindViewById<EditText>(Resource.Id.CategoryEditText);

                IconDescription = FindViewById<TextView>(Resource.Id.IconDescription);
                TxtDescription = FindViewById<EditText>(Resource.Id.DescriptionEditText);
                 
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconTitle, FontAwesomeIcon.User);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconLocation, FontAwesomeIcon.MapMarkedAlt);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconSalary, FontAwesomeIcon.MoneyBillAlt);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconSalaryDate, FontAwesomeIcon.Times);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconJobType, FontAwesomeIcon.Briefcase); 
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconDescription, FontAwesomeIcon.Paragraph);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeBrands, IconCategory, FontAwesomeIcon.Buromobelexperte);

                Methods.SetColorEditText(TxtTitle, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtLocation, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtMinimum, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtMaximum, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtSalaryDate, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtJobType, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtDescription, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtCategory, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                Methods.SetFocusable(TxtSalaryDate);
                Methods.SetFocusable(TxtJobType);
                Methods.SetFocusable(TxtCategory);
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
                        TxtSave.Click += TxtSaveOnClick;
                        TxtLocation.OnFocusChangeListener = this; 
                        TxtSalaryDate.Touch += TxtSalaryDateOnTouch;
                        TxtJobType.Touch += TxtJobTypeOnTouch;
                        TxtCategory.Touch += TxtCategoryOnTouch;
                        break;
                    default:
                        TxtSave.Click -= TxtSaveOnClick;
                        TxtLocation.OnFocusChangeListener = null!; 
                        TxtSalaryDate.Touch -= TxtSalaryDateOnTouch;
                        TxtJobType.Touch -= TxtJobTypeOnTouch;
                        TxtCategory.Touch -= TxtCategoryOnTouch;
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
                IconTitle = null!;
                TxtTitle = null!;
                IconLocation = null!;
                TxtLocation = null!;
                IconSalary = null!;
                TxtMinimum = null!;
                TxtMaximum = null!;
                IconSalaryDate = null!;
                TxtSalaryDate = null!;
                IconJobType = null!;
                TxtJobType = null!;
                IconCategory = null!;
                TxtCategory = null!;
                IconDescription = null!;
                TxtDescription = null!;
                TypeDialog = null!;
                CategoryId = null!;
                JobTypeId = null!;
                SalaryDateId = null!;
                DataInfoObject = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

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
                      
                    //Show a progress
                    AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));
                     
                    var (apiStatus, respond) = await RequestsAsync.Jobs.EditJobAsync(DataInfoObject.Id, TxtTitle.Text, TxtDescription.Text, TxtLocation.Text,
                        TxtMinimum.Text, TxtMaximum.Text, SalaryDateId, JobTypeId, CategoryId);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case MessageJobObject result:
                                {
                                    Console.WriteLine(result.MessageData);
                                    Toast.MakeText(this, GetString(Resource.String.Lbl_jobSuccessfullyEdited), ToastLength.Short)?.Show();
                                    AndHUD.Shared.Dismiss(this);

                                    JobInfoObject newInfoObject = new JobInfoObject
                                    {
                                        Title = TxtTitle.Text,
                                        Location = TxtLocation.Text,
                                        Minimum = TxtMinimum.Text,
                                        Maximum = TxtMaximum.Text,
                                        SalaryDate = TxtSalaryDate.Text,
                                        JobType = TxtJobType.Text,
                                        Description = TxtDescription.Text,
                                        Category = CategoryId,
                                    };
                             
                                    JobInfoObject data = JobsActivity.GetInstance()?.MAdapter?.JobList?.FirstOrDefault(a => a.Id == DataInfoObject.Id);
                                    if (data != null)
                                    {
                                        data.Title = TxtTitle.Text;
                                        data.Location = TxtLocation.Text;
                                        data.Minimum = TxtMinimum.Text;
                                        data.Maximum = TxtMaximum.Text; 
                                        data.SalaryDate = TxtSalaryDate.Text; 
                                        data.JobType = TxtJobType.Text; 
                                        data.Description = TxtDescription.Text; 
                                        data.Category = CategoryId; 
                                        JobsActivity.GetInstance().MAdapter.NotifyItemChanged(JobsActivity.GetInstance().MAdapter.JobList.IndexOf(data));
                                    }

                                    Intent intent = new Intent();
                                    intent.PutExtra("JobsItem", JsonConvert.SerializeObject(newInfoObject));
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
                TxtLocation.Text = string.IsNullOrEmpty(placeAddress) switch
                {
                    //var placeLatLng = data.GetStringExtra("latLng") ?? "";
                    false => placeAddress,
                    _ => TxtLocation.Text
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void BindJobPost()
        {
            try
            {
                if (DataInfoObject != null)
                {
                    DataInfoObject = WoWonderTools.ListFilterJobs(DataInfoObject);

                    TxtTitle.Text = Methods.FunString.DecodeString(DataInfoObject.Title);
                    TxtLocation.Text = DataInfoObject.Location;

                    //Set Description
                    TxtDescription.Text = Methods.FunString.DecodeString(DataInfoObject.Description);

                    TxtMinimum.Text = DataInfoObject.Minimum;
                    TxtMaximum.Text = DataInfoObject.Maximum;

                    //Set Salary Date
                    SalaryDateId = DataInfoObject.SalaryDate;
                    TxtSalaryDate.Text = DataInfoObject.SalaryDate switch
                    {
                        "per_hour" => GetString(Resource.String.Lbl_per_hour),
                        "per_day" => GetString(Resource.String.Lbl_per_day),
                        "per_week" => GetString(Resource.String.Lbl_per_week),
                        "per_month" => GetString(Resource.String.Lbl_per_month),
                        "per_year" => GetString(Resource.String.Lbl_per_year),
                        _ => TxtSalaryDate.Text
                    };

                    //Set job type
                    JobTypeId = DataInfoObject.JobType;
                    TxtJobType.Text = DataInfoObject.JobType switch
                    {
                        "full_time" => GetString(Resource.String.Lbl_full_time),
                        "part_time" => GetString(Resource.String.Lbl_part_time),
                        "internship" => GetString(Resource.String.Lbl_internship),
                        "volunteer" => GetString(Resource.String.Lbl_volunteer),
                        "contract" => GetString(Resource.String.Lbl_contract),
                        _ => TxtJobType.Text
                    };

                    CategoryId = DataInfoObject.Category;
                    TxtCategory.Text = CategoriesController.ListCategoriesJob.FirstOrDefault(categories => categories.CategoriesId == DataInfoObject.Category)?.CategoriesName;
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
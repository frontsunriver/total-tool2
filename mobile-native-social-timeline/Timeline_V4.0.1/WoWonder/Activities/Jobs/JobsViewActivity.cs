using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using AndroidX.AppCompat.Widget;
using Java.Lang;
using Newtonsoft.Json;
using WoWonder.Activities.NativePost.Extra;
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonder.Library.Anjo.SuperTextLibrary;
using WoWonderClient.Classes.Jobs;
using WoWonderClient.Requests; 
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using String = Java.Lang.String;

using WoWonder.Activities.Base;
using WoWonderClient;

namespace WoWonder.Activities.Jobs
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class JobsViewActivity : BaseActivity, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private ImageView JobCoverImage, IconBack;
        private ImageView JobAvatar;
        private TextView TxtMore, JobTitle, PageName, MaximumNumber, MinimumNumber;
        private AppCompatTextView JobInfo;
        private Button JobButton; 
        private SuperTextView Description;
        private JobInfoObject DataInfoObject;
        private string DialogType;
        private StReadMoreOption ReadMoreOption;

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
                SetContentView(Resource.Layout.JobsViewLayout);
              
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
                IconBack = FindViewById<ImageView>(Resource.Id.iv_back);

                JobCoverImage = FindViewById<ImageView>(Resource.Id.JobCoverImage);
                JobAvatar = FindViewById<ImageView>(Resource.Id.JobAvatar);
                JobTitle = FindViewById<TextView>(Resource.Id.Jobtitle);
                PageName = FindViewById<TextView>(Resource.Id.pageName);
                JobInfo = FindViewById<AppCompatTextView>(Resource.Id.JobInfo);
                JobButton = FindViewById<Button>(Resource.Id.JobButton);
                JobButton.Tag = "Apply";

                //MinimumTextView = FindViewById<TextView>(Resource.Id.minimum);
                //MaximumTextView = FindViewById<TextView>(Resource.Id.maximum);
                MaximumNumber = FindViewById<TextView>(Resource.Id.maximumNumber);
                MinimumNumber = FindViewById<TextView>(Resource.Id.minimumNumber);
                Description = FindViewById<SuperTextView>(Resource.Id.description);

                var font = Typeface.CreateFromAsset(Resources?.Assets, "ionicons.ttf");
                JobInfo.SetTypeface(font, TypefaceStyle.Normal);

                TxtMore = FindViewById<TextView>(Resource.Id.toolbar_title);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, TxtMore, IonIconsFonts.More);
                TxtMore.SetTextSize(ComplexUnitType.Sp, 20f);
                TxtMore.Visibility = ViewStates.Gone;

                ReadMoreOption = new StReadMoreOption.Builder()
                    .TextLength(400, StReadMoreOption.TypeCharacter)
                    .MoreLabel(GetText(Resource.String.Lbl_ReadMore))
                    .LessLabel(GetText(Resource.String.Lbl_ReadLess))
                    .MoreLabelColor(Color.ParseColor(AppSettings.MainColor))
                    .LessLabelColor(Color.ParseColor(AppSettings.MainColor))
                    .LabelUnderLine(true)
                    .Build();

                switch (AppSettings.FlowDirectionRightToLeft)
                {
                    case true:
                        IconBack.SetImageResource(Resource.Drawable.ic_action_ic_back_rtl);
                        break;
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
                    toolBar.Title = Methods.FunString.DecodeString(DataInfoObject.Title);
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
                        TxtMore.Click += TxtMoreOnClick;
                        JobButton.Click += JobButtonOnClick;
                        IconBack.Click += IconBackOnClick;
                        Description.LongClick += DescriptionOnLongClick;
                        break;
                    default:
                        TxtMore.Click -= TxtMoreOnClick;
                        JobButton.Click -= JobButtonOnClick;
                        IconBack.Click -= IconBackOnClick;
                        Description.LongClick -= DescriptionOnLongClick;
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
                IconBack = null!;
                JobCoverImage = null!;
                JobAvatar= null!;
                JobTitle = null!;
                PageName = null!;
                JobInfo = null!;
                JobButton = null!;
                MaximumNumber = null!;
                MinimumNumber = null!;
                Description = null!;
                TxtMore = null!;
                DialogType = null!;
                ReadMoreOption = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        private void DescriptionOnLongClick(object sender, View.LongClickEventArgs e)
        {
            try
            {
                if (Methods.FunString.StringNullRemover(DataInfoObject.Description) != "Empty")
                {
                    Methods.CopyToClipboard(this, DataInfoObject.Description); 
                } 
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void IconBackOnClick(object sender, EventArgs e)
        {
            try
            {
                Finish();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
          
        private void TxtMoreOnClick(object sender, EventArgs e)
        {
            try
            {
                DialogType = "More";
                  
                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                arrayAdapter.Add(GetText(Resource.String.Lbl_Edit));
                arrayAdapter.Add(GetText(Resource.String.Lbl_Delete)); 

                dialogList.Title(GetText(Resource.String.Lbl_More)).TitleColorRes(Resource.Color.primary);
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

        private void JobButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                switch (DataInfoObject)
                {
                    case null:
                        return;
                    default:
                        switch (JobButton?.Tag?.ToString())
                        {
                            // Open Apply Job Activity 
                            case "ShowApply":
                            {
                                switch (DataInfoObject.ApplyCount)
                                {
                                    case "0":
                                        Toast.MakeText(this, GetString(Resource.String.Lbl_ThereAreNoRequests), ToastLength.Short)?.Show();
                                        return;
                                }
                         
                                var intent = new Intent(this, typeof(ShowApplyJobActivity)); 
                                intent.PutExtra("JobsObject", JsonConvert.SerializeObject(DataInfoObject));
                                StartActivity(intent);
                                break;
                            }
                            case "Apply":
                            {
                                var intent = new Intent(this, typeof(ApplyJobActivity));
                                intent.PutExtra("JobsObject", JsonConvert.SerializeObject(DataInfoObject));
                                StartActivityForResult(intent,367);
                                break;
                            }
                        }

                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region MaterialDialog

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                string text = itemString.ToString();
                if (text == GetText(Resource.String.Lbl_Edit))
                {
                    //Open Edit Job
                    var intent = new Intent(this, typeof(EditJobsActivity));
                    intent.PutExtra("JobsObject", JsonConvert.SerializeObject(DataInfoObject));
                    StartActivityForResult(intent,246); 
                }
                else if (text == GetText(Resource.String.Lbl_Delete))
                {
                    DialogType = "Delete";

                    var dialog = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light); 
                    dialog.Title(Resource.String.Lbl_Warning).TitleColorRes(Resource.Color.primary);
                    dialog.Content(GetText(Resource.String.Lbl_DeleteJobs));
                    dialog.PositiveText(GetText(Resource.String.Lbl_Yes)).OnPositive(this);
                    dialog.NegativeText(GetText(Resource.String.Lbl_No)).OnNegative(this);
                    dialog.AlwaysCallSingleChoiceCallback();
                    dialog.ItemsCallback(this).Build().Show();
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
                switch (DialogType)
                {
                    case "Delete" when p1 == DialogAction.Positive:
                    {
                        // Send Api delete 

                        if (Methods.CheckConnectivity())
                        {
                            var adapterGlobal = WRecyclerView.GetInstance()?.NativeFeedAdapter;
                            var diff = adapterGlobal?.ListDiffer;
                            var dataGlobal = diff?.Where(a => a.PostData?.PostId == DataInfoObject?.PostId).ToList();
                            if (dataGlobal != null)
                            {
                                foreach (var postData in dataGlobal)
                                {
                                    WRecyclerView.GetInstance()?.RemoveByRowIndex(postData);
                                }
                            }

                            var recycler = TabbedMainActivity.GetInstance()?.NewsFeedTab?.MainRecyclerView;
                            var dataGlobal2 = recycler?.NativeFeedAdapter.ListDiffer?.Where(a => a.PostData?.PostId == DataInfoObject?.PostId).ToList();
                            if (dataGlobal2 != null)
                            {
                                foreach (var postData in dataGlobal2)
                                {
                                    recycler.RemoveByRowIndex(postData);
                                }
                            }
                              
                            var dataJob = JobsActivity.GetInstance()?.MAdapter?.JobList?.FirstOrDefault(a => a.Id == DataInfoObject.Id);
                            if (dataJob != null)
                            {
                                JobsActivity.GetInstance()?.MAdapter?.JobList.Remove(dataJob);
                                JobsActivity.GetInstance().MAdapter.NotifyItemRemoved(JobsActivity.GetInstance().MAdapter.JobList.IndexOf(dataJob));
                            }

                            Toast.MakeText(this, GetText(Resource.String.Lbl_postSuccessfullyDeleted), ToastLength.Short)?.Show();
                            PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.PostActionsAsync(DataInfoObject.PostId, "delete") });
                        }
                        else
                        {
                            Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                        }

                        break;
                    }
                    case "Delete":
                    {
                        if (p1 == DialogAction.Negative)
                        {
                            p0.Dismiss();
                        }

                        break;
                    }
                    default:
                    {
                        if (p1 == DialogAction.Positive)
                        {
                        
                        }
                        else if (p1 == DialogAction.Negative)
                        {
                            p0.Dismiss();
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
         
        #region  Result

        //Result
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
                switch (requestCode)
                {
                    case 367 when resultCode == Result.Ok:
                        //Already applied 
                        DataInfoObject.Apply = "true";
                        JobButton.Text = GetString(Resource.String.Lbl_already_applied);
                        JobButton.Enabled = false;
                        break;
                    case 246 when resultCode == Result.Ok:
                    {
                        var jobsItem = data.GetStringExtra("JobsItem") ?? "";
                        if (string.IsNullOrEmpty(jobsItem)) return;
                        var dataObject = JsonConvert.DeserializeObject<JobInfoObject>(jobsItem);
                        if (dataObject != null)
                        {
                            DataInfoObject.Title =  dataObject.Title;
                            DataInfoObject.Location =  dataObject.Location;
                            DataInfoObject.Minimum =  dataObject.Minimum;
                            DataInfoObject.Maximum =  dataObject.Maximum;
                            DataInfoObject.SalaryDate =  dataObject.SalaryDate;
                            DataInfoObject.JobType =  dataObject.JobType;
                            DataInfoObject.Description =  dataObject.Description;
                            DataInfoObject.Category = dataObject.Category;

                            BindJobPost();
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

        private void BindJobPost()
        {
            try
            {
                if (DataInfoObject != null)
                {
                    DataInfoObject = WoWonderTools.ListFilterJobs(DataInfoObject);
                     
                    GlideImageLoader.LoadImage(this, DataInfoObject.Page.Avatar, JobAvatar, ImageStyle.RoundedCrop, ImagePlaceholders.Drawable);

                    var image = DataInfoObject.Image.Replace(Client.WebsiteUrl + "/", "");
                    DataInfoObject.Image = image.Contains("http") switch
                    {
                        false => Client.WebsiteUrl + "/" + image,
                        _ => image
                    };

                    GlideImageLoader.LoadImage(this, DataInfoObject.Image, JobCoverImage, ImageStyle.FitCenter, ImagePlaceholders.Drawable);
                     
                    if (DataInfoObject.IsOwner != null && DataInfoObject.IsOwner.Value)
                    {
                        TxtMore.Visibility = ViewStates.Visible;
                        JobButton.Text = GetString(Resource.String.Lbl_show_applies) + " (" + DataInfoObject.ApplyCount + ")";
                        JobButton.Tag = "ShowApply";
                    }

                    switch (DataInfoObject.Apply)
                    {
                        //Set Button if its applied
                        case "true":
                            JobButton.Text = GetString(Resource.String.Lbl_already_applied);
                            JobButton.Enabled = false;
                            break;
                    }
                     
                    JobTitle.Text = Methods.FunString.DecodeString(DataInfoObject.Title);

                    if (DataInfoObject.Page != null)
                    {
                        PageName.Text = "@" + Methods.FunString.DecodeString(DataInfoObject.Page.PageName.Replace("@" , ""));
                        if (DataInfoObject.Page.IsPageOnwer != null && DataInfoObject.Page.IsPageOnwer.Value)
                        {
                            JobButton.Text = GetString(Resource.String.Lbl_show_applies) + " (" + DataInfoObject.ApplyCount + ")";
                        }
                    }
                     
                    //Set Description
                    var description = Methods.FunString.DecodeString(DataInfoObject.Description);
                    Description.Text = description; 
                    ReadMoreOption.AddReadMoreTo(Description, new String(description));

                    //Set Salary Date
                    string salaryDate = DataInfoObject.SalaryDate switch
                    {
                        "per_hour" => GetString(Resource.String.Lbl_per_hour),
                        "per_day" => GetString(Resource.String.Lbl_per_day),
                        "per_week" => GetString(Resource.String.Lbl_per_week),
                        "per_month" => GetString(Resource.String.Lbl_per_month),
                        "per_year" => GetString(Resource.String.Lbl_per_year),
                        _ => GetString(Resource.String.Lbl_Unknown)
                    };

                    MinimumNumber.Text = DataInfoObject.Minimum + " " + salaryDate;
                    MaximumNumber.Text = DataInfoObject.Maximum + " " + salaryDate;

                    //Set job Time
                    var jobInfo = IonIconsFonts.Pin + " " + DataInfoObject.Location + "  " + " ";
                    jobInfo += IonIconsFonts.Time + " " + Methods.Time.TimeAgo(Convert.ToInt32(DataInfoObject.Time), false) + " " + " ";

                    jobInfo += DataInfoObject.JobType switch
                    {
                        //Set job type
                        "full_time" => IonIconsFonts.IosBriefcase + " " + GetString(Resource.String.Lbl_full_time),
                        "part_time" => IonIconsFonts.IosBriefcase + " " + GetString(Resource.String.Lbl_part_time),
                        "internship" => IonIconsFonts.IosBriefcase + " " + GetString(Resource.String.Lbl_internship),
                        "volunteer" => IonIconsFonts.IosBriefcase + " " + GetString(Resource.String.Lbl_volunteer),
                        "contract" => IonIconsFonts.IosBriefcase + " " + GetString(Resource.String.Lbl_contract),
                        _ => IonIconsFonts.IosBriefcase + " " + GetString(Resource.String.Lbl_Unknown)
                    };

                    var categoryName = CategoriesController.ListCategoriesJob.FirstOrDefault(categories => categories.CategoriesId == DataInfoObject.Category)?.CategoriesName;
                    jobInfo += " " + " " + IonIconsFonts.Pricetag + " " + categoryName;

                    var woTextDecorator = new WoTextDecorator
                    {
                        DecoratedContent = new SpannableString(jobInfo),
                        Content = jobInfo
                    };
                    woTextDecorator.SetTextColor(IonIconsFonts.Pin, "#ff5722");
                    woTextDecorator.SetTextColor(IonIconsFonts.Time, "#4caf50");
                    woTextDecorator.SetTextColor(IonIconsFonts.IosBriefcase, "#2196f3");
                    woTextDecorator.SetTextColor(IonIconsFonts.Pricetag, "#795548");
                    woTextDecorator.Build(JobInfo, woTextDecorator.DecoratedContent);
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using AFollestad.MaterialDialogs;
using Android.Content;
using Android.OS;

using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomSheet;
using Java.Lang;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using Exception = System.Exception;

namespace WoWonder.Activities.Jobs
{
    public class FilterJobDialogFragment : BottomSheetDialogFragment, SeekBar.IOnSeekBarChangeListener, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private JobsActivity ContextJobs;
        private Button BtnApply;
        private TextView IconBack, IconDistance, IconJobType, TxtJobType, JobTypeMoreIcon, IconCategories, TxtCategories, CategoriesMoreIcon, TxtDistanceCount;
        private SeekBar DistanceBar;                                                     
        private RelativeLayout LayoutJobType, LayoutCategories;                          
        private int DistanceCount;
        private string JobType, CategoryId, TypeDialog = "";

        #endregion

        #region General

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
            ContextJobs = (JobsActivity)Activity;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                //    View view = inflater.Inflate(Resource.Layout.ButtomSheetJobhFilter, container, false);
                Context contextThemeWrapper = AppSettings.SetTabDarkTheme ? new ContextThemeWrapper(Activity, Resource.Style.MyTheme_Dark_Base) : new ContextThemeWrapper(Activity, Resource.Style.MyTheme_Base);
                // clone the inflater using the ContextThemeWrapper
                LayoutInflater localInflater = inflater.CloneInContext(contextThemeWrapper);

                View view = localInflater?.Inflate(Resource.Layout.ButtomSheetJobhFilter, container, false); 
                return view;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            try
            {
                base.OnViewCreated(view, savedInstanceState);

                InitComponent(view);

                LayoutJobType.Click += LayoutJobTypeOnClick;
                LayoutCategories.Click += LayoutCategoriesOnClick;
                IconBack.Click += IconBackOnClick;
                BtnApply.Click += BtnApplyOnClick;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
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

        #region Functions

        private void InitComponent(View view)
        {
            try
            {
                IconBack = view.FindViewById<TextView>(Resource.Id.IconBack);

                IconDistance = view.FindViewById<TextView>(Resource.Id.IconDistance);
                TxtDistanceCount = view.FindViewById<TextView>(Resource.Id.Distancenumber);

                LayoutJobType = view.FindViewById<RelativeLayout>(Resource.Id.LayoutJobType);
                LayoutCategories = view.FindViewById<RelativeLayout>(Resource.Id.LayoutCategories);

                DistanceBar = view.FindViewById<SeekBar>(Resource.Id.distanceSeeker);
                DistanceBar.Max = 300;
                DistanceBar.SetOnSeekBarChangeListener(this);
                 
                IconJobType = view.FindViewById<TextView>(Resource.Id.IconJobType);
                TxtJobType = view.FindViewById<TextView>(Resource.Id.textJobType);
                JobTypeMoreIcon = view.FindViewById<TextView>(Resource.Id.JobTypeMoreIcon);
               
                IconCategories = view.FindViewById<TextView>(Resource.Id.IconCategories);
                TxtCategories = view.FindViewById<TextView>(Resource.Id.textCategories);
                CategoriesMoreIcon = view.FindViewById<TextView>(Resource.Id.CategoriesMoreIcon);
                 
                BtnApply = view.FindViewById<Button>(Resource.Id.ApplyButton);
                 
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconDistance, FontAwesomeIcon.StreetView);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconBack, AppSettings.FlowDirectionRightToLeft ? IonIconsFonts.IosArrowDropright : IonIconsFonts.IosArrowDropleft);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeBrands, IconCategories, FontAwesomeIcon.Buromobelexperte);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconJobType, FontAwesomeIcon.Briefcase);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, JobTypeMoreIcon, AppSettings.FlowDirectionRightToLeft ? IonIconsFonts.IosArrowDropleft : IonIconsFonts.IosArrowDropright);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, CategoriesMoreIcon, AppSettings.FlowDirectionRightToLeft ? IonIconsFonts.IosArrowDropleft : IonIconsFonts.IosArrowDropright);

                switch (Build.VERSION.SdkInt)
                {
                    case >= BuildVersionCodes.N:
                        DistanceBar.SetProgress(string.IsNullOrEmpty(UserDetails.FilterJobLocation) ? 300 : Convert.ToInt32(UserDetails.FilterJobLocation), true);
                        break;
                    // For API < 24 
                    default:
                        DistanceBar.Progress = string.IsNullOrEmpty(UserDetails.FilterJobLocation) ? 300 : Convert.ToInt32(UserDetails.FilterJobLocation);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Event

        //Back
        private void IconBackOnClick(object sender, EventArgs e)
        {
            try
            {
                Dismiss();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Save data 
        private void BtnApplyOnClick(object sender, EventArgs e)
        {
            try
            {
                UserDetails.FilterJobType = JobType; 
                UserDetails.FilterJobLocation = DistanceCount.ToString();
                UserDetails.FilterJobCategories = CategoryId;

                ContextJobs.MAdapter.JobList.Clear();
                ContextJobs.MAdapter.NotifyDataSetChanged();
                ContextJobs.SwipeRefreshLayout.Refreshing = true;

                ContextJobs.StartApiService();

                Dismiss();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
           
        //Categories
        private void LayoutCategoriesOnClick(object sender, EventArgs e)
        {
            try
            {
                switch (CategoriesController.ListCategoriesJob.Count)
                {
                    case > 0:
                    {
                        TypeDialog = "Categories";

                        var dialogList = new MaterialDialog.Builder(Context).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                        var arrayAdapter = CategoriesController.ListCategoriesJob.Select(item => item.CategoriesName).ToList();

                        dialogList.Title(GetText(Resource.String.Lbl_SelectCategories)).TitleColorRes(Resource.Color.primary);
                        dialogList.Items(arrayAdapter);
                        dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                        dialogList.AlwaysCallSingleChoiceCallback();
                        dialogList.ItemsCallback(this).Build().Show();
                        break;
                    }
                    default:
                        Methods.DisplayReportResult(Activity, "Not have List Categories Job");
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //JobType
        private void LayoutJobTypeOnClick(object sender, EventArgs e)
        {
            try
            {
                TypeDialog = "JobType";

                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(Context).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                arrayAdapter.Add(GetText(Resource.String.Lbl_full_time));
                arrayAdapter.Add(GetText(Resource.String.Lbl_part_time));
                arrayAdapter.Add(GetText(Resource.String.Lbl_internship));
                arrayAdapter.Add(GetText(Resource.String.Lbl_volunteer));
                arrayAdapter.Add(GetText(Resource.String.Lbl_contract)); 

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

        #endregion
         
        #region MaterialDialog

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                string text = itemString.ToString();

                switch (TypeDialog)
                {
                    case "Categories":
                        CategoryId = CategoriesController.ListCategoriesJob.FirstOrDefault(categories => categories.CategoriesName == itemString.ToString())?.CategoriesId;
                        TxtCategories.Text = itemString.ToString();
                        break;
                    case "JobType":
                    {
                        TxtJobType.Text = text;

                        if (text == GetText(Resource.String.Lbl_full_time))
                            JobType = "full_time";
                        else if (text == GetText(Resource.String.Lbl_part_time))
                            JobType = "part_time";
                        else if (text == GetText(Resource.String.Lbl_internship))
                            JobType = "internship";
                        else if (text == GetText(Resource.String.Lbl_volunteer))
                            JobType = "volunteer";
                        else if (text == GetText(Resource.String.Lbl_contract))
                            JobType = "contract";
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

        #region SeekBar

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            try
            {
                TxtDistanceCount.Text = progress + " " + GetText(Resource.String.Lbl_km);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {

        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            try
            {
                DistanceCount = seekBar.Progress;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

    }
}
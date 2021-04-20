using System;
using System.Collections.Generic;
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
using Console = System.Console;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Jobs
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class ApplyJobActivity : BaseActivity, View.IOnFocusChangeListener, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private TextView TxtSave;
        private TextView IconName, IconLocation, IconPhone, IconEmail, IconWork, IconPosition, IconDescription, IconDate;
        private EditText TxtName, TxtLocation, TxtPhone, TxtEmail, TxtWork, TxtPosition, TxtDescription, TxtFromDate, TxtToDate;
        private CheckBox ChkCurrentlyWork;
        private ViewStub QuestionOneLayout, QuestionTwoLayout, QuestionThreeLayout;
        private View InflatedQuestionOne, InflatedQuestionTwo, InflatedQuestionThree;
        private TextView TxtQuestion;
        private EditText EdtQuestion;
        private RadioButton RdoYes, RdoNo; 
        private string DialogType, CurrentlyWork;
        private JobInfoObject DataInfoObject;
        private readonly string[] ExperienceDate = Application.Context.Resources?.GetStringArray(Resource.Array.experience_date);
        private readonly string[] JobCategories = Application.Context.Resources?.GetStringArray(Resource.Array.job_categories);
        private string QuestionOneAnswer, QuestionTwoAnswer, QuestionThreeAnswer;
         
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
                SetContentView(Resource.Layout.ApplyJoblayout);
              
                var dataObject = Intent?.GetStringExtra("JobsObject");
                DataInfoObject = string.IsNullOrEmpty(dataObject) switch
                {
                    false => JsonConvert.DeserializeObject<JobInfoObject>(dataObject),
                    _ => DataInfoObject
                };

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                LoadMyDate();
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

                IconName = FindViewById<TextView>(Resource.Id.IconName);
                TxtName = FindViewById<EditText>(Resource.Id.NameEditText);
                IconLocation = FindViewById<TextView>(Resource.Id.IconLocation);
                TxtLocation = FindViewById<EditText>(Resource.Id.LocationEditText);
                IconPhone = FindViewById<TextView>(Resource.Id.IconPhone);
                TxtPhone = FindViewById<EditText>(Resource.Id.PhoneEditText);
                IconEmail = FindViewById<TextView>(Resource.Id.IconEmail);
                TxtEmail = FindViewById<EditText>(Resource.Id.EmailEditText);

                IconPosition = FindViewById<TextView>(Resource.Id.IconPosition);
                TxtPosition = FindViewById<EditText>(Resource.Id.PositionEditText);

                IconWork = FindViewById<TextView>(Resource.Id.IconWorkStatus);
                TxtWork = FindViewById<EditText>(Resource.Id.WorkStatusEditText);

                IconDescription = FindViewById<TextView>(Resource.Id.IconDescription);
                TxtDescription = FindViewById<EditText>(Resource.Id.DescriptionEditText);

                IconDate = FindViewById<TextView>(Resource.Id.IconDate);
                TxtFromDate = FindViewById<EditText>(Resource.Id.FromDateEditText);
                TxtToDate = FindViewById<EditText>(Resource.Id.ToDateEditText);
                 
                ChkCurrentlyWork = FindViewById<CheckBox>(Resource.Id.iCurrentlyWorkCheckBox);
            
                QuestionOneLayout = FindViewById<ViewStub>(Resource.Id.viewStubQuestionOne);
                QuestionTwoLayout = FindViewById<ViewStub>(Resource.Id.viewStubQuestionTwo);
                QuestionThreeLayout = FindViewById<ViewStub>(Resource.Id.viewStubQuestionThree);

                //free_text_question,yes_no_question,multiple_choice_question
                SetQuestion();
                 
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconName, FontAwesomeIcon.User);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconLocation, FontAwesomeIcon.MapMarkedAlt);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconPhone, FontAwesomeIcon.Phone);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconWork, FontAwesomeIcon.Briefcase);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconEmail, FontAwesomeIcon.PaperPlane);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconPosition, FontAwesomeIcon.MapPin);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconDescription, FontAwesomeIcon.Paragraph);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconDate, FontAwesomeIcon.Calendar);

                Methods.SetColorEditText(TxtName, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtLocation, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtPhone, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtEmail, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtPosition, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtWork, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtDescription, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtFromDate, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtToDate, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                Methods.SetFocusable(TxtFromDate); 
                Methods.SetFocusable(TxtToDate); 
                Methods.SetFocusable(TxtPosition); 
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
                        TxtSave.Click += TxtSaveOnClick;
                        TxtLocation.OnFocusChangeListener = this; 
                        TxtFromDate.Touch += TxtFromDateOnTouch;
                        TxtToDate.Touch += TxtToDateOnTouch;
                        TxtPosition.Touch += TxtPositionOnClick;
                        ChkCurrentlyWork.CheckedChange += ChkCurrentlyWorkOnCheckedChange;
                        break;
                    default:
                        TxtSave.Click -= TxtSaveOnClick;
                        TxtLocation.OnFocusChangeListener = null!; 
                        TxtFromDate.Touch -= TxtFromDateOnTouch;
                        TxtToDate.Touch -= TxtToDateOnTouch;
                        TxtPosition.Touch -= TxtPositionOnClick;
                        ChkCurrentlyWork.CheckedChange -= ChkCurrentlyWorkOnCheckedChange;
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
                IconName = null!;
                TxtName = null!;
                IconLocation = null!;
                TxtLocation = null!;
                IconPhone = null!;
                TxtPhone = null!;
                IconEmail = null!;
                TxtEmail = null!;
                IconPosition = null!;
                TxtPosition = null!;
                IconWork = null!;
                TxtWork = null!;
                IconDescription = null!;
                TxtDescription = null!;
                IconDate = null!;
                TxtFromDate = null!;
                TxtToDate = null!;
                ChkCurrentlyWork = null!;
                DataInfoObject = null!;
                QuestionOneLayout = null!;
                DialogType = null!;
                TxtQuestion = null!;
                CurrentlyWork = null!;
                EdtQuestion = null!;
                RdoYes = null!;
                RdoNo = null!;
                InflatedQuestionOne = null!;
                InflatedQuestionTwo = null!; 
                InflatedQuestionThree = null!;
                QuestionTwoLayout = null!;
                QuestionThreeLayout = null!;
                QuestionOneAnswer = null!; 
                QuestionTwoAnswer = null!; 
                QuestionThreeAnswer = null!;

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
      
        #endregion

        #region Events

        private async void TxtSaveOnClick(object sender, EventArgs e)
        {
            try
            {
                if (Methods.CheckConnectivity())
                {

                    if (string.IsNullOrEmpty(TxtName.Text) || string.IsNullOrEmpty(TxtPhone.Text) || string.IsNullOrEmpty(TxtLocation.Text)
                        || string.IsNullOrEmpty(TxtWork.Text) || string.IsNullOrEmpty(TxtDescription.Text)|| string.IsNullOrEmpty(TxtFromDate.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_your_data), ToastLength.Short)?.Show();
                        return;
                    }

                    switch (CurrentlyWork)
                    {
                        case "off" when string.IsNullOrEmpty(TxtToDate.Text):
                            Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_your_data), ToastLength.Short)?.Show();
                            return;
                    }

                    var check = Methods.FunString.IsEmailValid(TxtEmail.Text.Replace(" ", ""));
                    switch (check)
                    {
                        case false:
                            Toast.MakeText(this, GetText(Resource.String.Lbl_IsEmailValid), ToastLength.Short)?.Show();
                            return;
                    }
                     
                    //Show a progress
                    AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));
                     
                    var dictionary = new Dictionary<string, string>
                    {
                        {"job_id", DataInfoObject.Id},
                        {"user_name", TxtName.Text},
                        {"phone_number", TxtPhone.Text},
                        {"location", TxtLocation.Text},
                        {"email", TxtEmail.Text},
                        {"where_did_you_work", TxtWork.Text},
                        {"Position", TxtPosition.Text},
                        {"experience_description", TxtDescription.Text},
                        {"experience_start_date", TxtFromDate.Text},
                        {"experience_end_date", TxtToDate.Text},
                        {"i_currently_work", CurrentlyWork},
                        {"question_one_answer", QuestionOneAnswer},
                        {"question_two_answer", QuestionTwoAnswer},
                        {"question_three_answer", QuestionThreeAnswer},
                    };
                    
                    var (apiStatus, respond) = await RequestsAsync.Jobs.ApplyJobAsync(dictionary);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case MessageJobObject result:
                                {
                                    Console.WriteLine(result.MessageData);
                                    Toast.MakeText(this, "You have successfully applied to this job", ToastLength.Short)?.Show();
                                    AndHUD.Shared.Dismiss(this);

                                    var data =  JobsActivity.GetInstance()?.MAdapter?.JobList?.FirstOrDefault(a => a.Id == DataInfoObject.Id);
                                    if (data != null)
                                    {
                                        data.Apply = "true"; 
                                        JobsActivity.GetInstance().MAdapter.NotifyItemChanged(JobsActivity.GetInstance().MAdapter.JobList.IndexOf(data));
                                    }

                                    SetResult(Result.Ok);
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
         
        private void TxtPositionOnClick(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                DialogType = "Position";
                var arrayAdapter = JobCategories.ToList();

                dialogList.Title(GetText(Resource.String.Lbl_Position)).TitleColorRes(Resource.Color.primary);
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
         
        private void ChkCurrentlyWorkOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            try
            {
                TxtToDate.Visibility = e.IsChecked ? ViewStates.Invisible : ViewStates.Visible;
                CurrentlyWork = e.IsChecked ? "on" : "off";
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void TxtToDateOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                DialogType = "ToDate";
                var arrayAdapter = ExperienceDate.ToList();

                dialogList.Title(GetText(Resource.String.Lbl_ToDate)).TitleColorRes(Resource.Color.primary);
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

        private void TxtFromDateOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                DialogType = "FromDate";
                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                var arrayAdapter = ExperienceDate.ToList();

                dialogList.Title(GetText(Resource.String.Lbl_FromDate)).TitleColorRes(Resource.Color.primary);
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
                switch (DialogType)
                {
                    case "Position":
                        TxtPosition.Text = itemString.ToString();
                        break;
                    case "FromDate":
                        TxtFromDate.Text = itemString.ToString();
                        break;
                    case "ToDate":
                        TxtToDate.Text = itemString.ToString();
                        break;
                    case "QuestionOne":
                        EdtQuestion.Text = itemString.ToString();
                        QuestionOneAnswer = itemId.ToString();
                        break;
                    case "QuestionTwo":
                        EdtQuestion.Text = itemString.ToString();
                        QuestionTwoAnswer = itemId.ToString();
                        break;
                    case "QuestionThree":
                        EdtQuestion.Text = itemString.ToString();
                        QuestionThreeAnswer = itemId.ToString();
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Question

        private void SetQuestion()
        {
            try
            {
                #region Question One

                switch (DataInfoObject.QuestionOneType)
                {
                    case "free_text_question":
                    {
                        QuestionOneLayout.LayoutResource = Resource.Layout.ViewSub_Question_EditText;

                        InflatedQuestionOne = InflatedQuestionOne switch
                        {
                            null => QuestionOneLayout.Inflate(),
                            _ => InflatedQuestionOne
                        };

                        QuestionOneLayout.Visibility = ViewStates.Visible;

                        TxtQuestion = InflatedQuestionOne.FindViewById<TextView>(Resource.Id.QuestionTextView);
                        TxtQuestion.Text = Methods.FunString.DecodeString(DataInfoObject.QuestionOne);

                        EdtQuestion = InflatedQuestionOne.FindViewById<EditText>(Resource.Id.QuestionEditText);
                        Methods.SetColorEditText(EdtQuestion, AppSettings.SetTabDarkTheme ? Color.White : Color.Black); 
                          
                        EdtQuestion.TextChanged += (sender, args) =>
                        {
                            try
                            {
                                QuestionOneAnswer = args.Text.ToString();
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        };
                        break;
                    }
                    case "yes_no_question":
                    {
                        QuestionOneLayout.LayoutResource = Resource.Layout.ViewSub_Question_CheckBox;

                        InflatedQuestionOne = InflatedQuestionOne switch
                        {
                            null => QuestionOneLayout.Inflate(),
                            _ => InflatedQuestionOne
                        };

                        QuestionOneLayout.Visibility = ViewStates.Visible;

                        TxtQuestion = InflatedQuestionOne.FindViewById<TextView>(Resource.Id.QuestionTextView);
                        TxtQuestion.Text = Methods.FunString.DecodeString(DataInfoObject.QuestionOne);
                             
                        RdoYes = InflatedQuestionOne.FindViewById<RadioButton>(Resource.Id.radioYes);
                        RdoNo = InflatedQuestionOne.FindViewById<RadioButton>(Resource.Id.radioNo);
                        RdoYes.CheckedChange += (sender, args) =>
                        {
                            try
                            {
                                var isChecked = RdoYes.Checked;
                                switch (isChecked)
                                {
                                    case false:
                                        return;
                                    default:
                                        RdoNo.Checked = false;
                                        QuestionOneAnswer = "yes";
                                        break;
                                }
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        };
                        RdoNo.CheckedChange += (sender, args) =>
                        {
                            try
                            {
                                var isChecked = RdoNo.Checked;
                                switch (isChecked)
                                {
                                    case false:
                                        return;
                                    default:
                                        RdoNo.Checked = false;
                                        QuestionOneAnswer = "no";
                                        break;
                                }
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        }; 
                        break;
                    }
                    case "multiple_choice_question":
                    {
                        QuestionOneLayout.LayoutResource = Resource.Layout.ViewSub_Question_List;

                        InflatedQuestionOne = InflatedQuestionOne switch
                        {
                            null => QuestionOneLayout.Inflate(),
                            _ => InflatedQuestionOne
                        };

                        QuestionOneLayout.Visibility = ViewStates.Visible;

                        TxtQuestion = InflatedQuestionOne.FindViewById<TextView>(Resource.Id.QuestionTextView);  
                        TxtQuestion.Text = Methods.FunString.DecodeString(DataInfoObject.QuestionOne);
                             
                        EdtQuestion = InflatedQuestionOne.FindViewById<EditText>(Resource.Id.QuestionEditText);
                        Methods.SetColorEditText(EdtQuestion, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                        Methods.SetFocusable(EdtQuestion);
                        EdtQuestion.Touch += (sender, args) =>
                        {
                            if (args.Event.Action != MotionEventActions.Down) return;

                            DialogType = "QuestionOne";
                            var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                            var arrayAdapter = new List<string>();
                            switch (DataInfoObject.QuestionOneAnswers?.Count)
                            {
                                case > 0:
                                    arrayAdapter = DataInfoObject.QuestionOneAnswers;
                                    break;
                            }
                             
                            dialogList.Title(Methods.FunString.DecodeString(DataInfoObject.QuestionOne)).TitleColorRes(Resource.Color.primary);
                            dialogList.Items(arrayAdapter);
                            dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                            dialogList.AlwaysCallSingleChoiceCallback();
                            dialogList.ItemsCallback(this).Build().Show();
                        };
                        break;
                    }
                    default:
                        QuestionOneLayout.Visibility = ViewStates.Gone;
                        break;
                }

                #endregion

                #region Question Two

                switch (DataInfoObject.QuestionTwoType)
                {
                    case "free_text_question":
                    {
                        QuestionTwoLayout.LayoutResource = Resource.Layout.ViewSub_Question_EditText;

                        InflatedQuestionTwo = InflatedQuestionTwo switch
                        {
                            null => QuestionTwoLayout.Inflate(),
                            _ => InflatedQuestionTwo
                        };

                        QuestionTwoLayout.Visibility = ViewStates.Visible;

                        TxtQuestion = InflatedQuestionTwo.FindViewById<TextView>(Resource.Id.QuestionTextView);
                        TxtQuestion.Text = Methods.FunString.DecodeString(DataInfoObject.QuestionTwo);
                             
                        EdtQuestion = InflatedQuestionTwo.FindViewById<EditText>(Resource.Id.QuestionEditText);
                        Methods.SetColorEditText(EdtQuestion, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                         
                        EdtQuestion.TextChanged += (sender, args) =>
                        {
                            try
                            {
                                QuestionTwoAnswer = args.Text.ToString();
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        };
                        break;
                    }
                    case "yes_no_question":
                    {
                        QuestionTwoLayout.LayoutResource = Resource.Layout.ViewSub_Question_CheckBox;

                        InflatedQuestionTwo = InflatedQuestionTwo switch
                        {
                            null => QuestionTwoLayout.Inflate(),
                            _ => InflatedQuestionTwo
                        };

                        QuestionTwoLayout.Visibility = ViewStates.Visible;

                        TxtQuestion = InflatedQuestionTwo.FindViewById<TextView>(Resource.Id.QuestionTextView); 
                        TxtQuestion.Text = Methods.FunString.DecodeString(DataInfoObject.QuestionTwo);
                             
                        RdoYes = InflatedQuestionTwo.FindViewById<RadioButton>(Resource.Id.radioYes);
                        RdoNo = InflatedQuestionTwo.FindViewById<RadioButton>(Resource.Id.radioNo);
                        RdoYes.CheckedChange += (sender, args) =>
                        {
                            try
                            {
                                var isChecked = RdoYes.Checked;
                                switch (isChecked)
                                {
                                    case false:
                                        return;
                                    default:
                                        RdoNo.Checked = false;
                                        QuestionTwoAnswer = "yes";
                                        break;
                                }
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        };
                        RdoNo.CheckedChange += (sender, args) =>
                        {
                            try
                            {
                                var isChecked = RdoNo.Checked;
                                switch (isChecked)
                                {
                                    case false:
                                        return;
                                    default:
                                        RdoNo.Checked = false;
                                        QuestionTwoAnswer = "no";
                                        break;
                                }
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        };
                        break;
                    }
                    case "multiple_choice_question":
                    {
                        QuestionTwoLayout.LayoutResource = Resource.Layout.ViewSub_Question_List;

                        InflatedQuestionTwo = InflatedQuestionTwo switch
                        {
                            null => QuestionTwoLayout.Inflate(),
                            _ => InflatedQuestionTwo
                        };

                        QuestionTwoLayout.Visibility = ViewStates.Visible;

                        TxtQuestion = InflatedQuestionTwo.FindViewById<TextView>(Resource.Id.QuestionTextView);
                        TxtQuestion.Text = Methods.FunString.DecodeString(DataInfoObject.QuestionTwo);
                             
                        EdtQuestion = InflatedQuestionTwo.FindViewById<EditText>(Resource.Id.QuestionEditText);
                        Methods.SetColorEditText(EdtQuestion, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                        Methods.SetFocusable(EdtQuestion);
                        EdtQuestion.Touch += (sender, args) =>
                        {
                            if (args.Event.Action != MotionEventActions.Down) return;

                            DialogType = "QuestionTwo";
                            var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                            var arrayAdapter = new List<string>();
                            switch (DataInfoObject.QuestionTwoAnswers?.Count)
                            {
                                case > 0:
                                    arrayAdapter = DataInfoObject.QuestionTwoAnswers;
                                    break;
                            }

                            dialogList.Title(Methods.FunString.DecodeString(DataInfoObject.QuestionTwo)).TitleColorRes(Resource.Color.primary);
                            dialogList.Items(arrayAdapter);
                            dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                            dialogList.AlwaysCallSingleChoiceCallback();
                            dialogList.ItemsCallback(this).Build().Show();
                        };
                        break;
                    }
                    default:
                        QuestionTwoLayout.Visibility = ViewStates.Gone;
                        break;
                }

                #endregion

                #region Question Three

                switch (DataInfoObject.QuestionThreeType)
                {
                    case "free_text_question":
                    {
                        QuestionThreeLayout.LayoutResource = Resource.Layout.ViewSub_Question_EditText;

                        InflatedQuestionThree = InflatedQuestionThree switch
                        {
                            null => QuestionThreeLayout.Inflate(),
                            _ => InflatedQuestionThree
                        };

                        QuestionThreeLayout.Visibility = ViewStates.Visible;

                        TxtQuestion = InflatedQuestionThree.FindViewById<TextView>(Resource.Id.QuestionTextView);
                        TxtQuestion.Text = Methods.FunString.DecodeString(DataInfoObject.QuestionThree);
                             
                        EdtQuestion = InflatedQuestionThree.FindViewById<EditText>(Resource.Id.QuestionEditText);
                        Methods.SetColorEditText(EdtQuestion, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                          
                        EdtQuestion.TextChanged += (sender, args) =>
                        {
                            try
                            {
                                QuestionThreeAnswer = args.Text.ToString();
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        };
                        break;
                    }
                    case "yes_no_question":
                    {
                        QuestionThreeLayout.LayoutResource = Resource.Layout.ViewSub_Question_CheckBox;

                        InflatedQuestionThree = InflatedQuestionThree switch
                        {
                            null => QuestionThreeLayout.Inflate(),
                            _ => InflatedQuestionThree
                        };

                        QuestionThreeLayout.Visibility = ViewStates.Visible;

                        TxtQuestion = InflatedQuestionThree.FindViewById<TextView>(Resource.Id.QuestionTextView);
                        TxtQuestion.Text = Methods.FunString.DecodeString(DataInfoObject.QuestionThree);
                        
                        RdoYes = InflatedQuestionThree.FindViewById<RadioButton>(Resource.Id.radioYes);
                        RdoNo = InflatedQuestionThree.FindViewById<RadioButton>(Resource.Id.radioNo);
                        RdoYes.CheckedChange += (sender, args) =>
                        {
                            try
                            {
                                var isChecked = RdoYes.Checked;
                                switch (isChecked)
                                {
                                    case false:
                                        return;
                                    default:
                                        RdoNo.Checked = false;
                                        QuestionThreeAnswer = "yes";
                                        break;
                                }
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        };
                        RdoNo.CheckedChange += (sender, args) =>
                        {
                            try
                            {
                                var isChecked = RdoNo.Checked;
                                switch (isChecked)
                                {
                                    case false:
                                        return;
                                    default:
                                        RdoNo.Checked = false;
                                        QuestionThreeAnswer = "no";
                                        break;
                                }
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        };
                            break;
                    }
                    case "multiple_choice_question":
                    {
                        QuestionThreeLayout.LayoutResource = Resource.Layout.ViewSub_Question_List;

                        InflatedQuestionThree = InflatedQuestionThree switch
                        {
                            null => QuestionThreeLayout.Inflate(),
                            _ => InflatedQuestionThree
                        };

                        QuestionThreeLayout.Visibility = ViewStates.Visible;

                        TxtQuestion = InflatedQuestionThree.FindViewById<TextView>(Resource.Id.QuestionTextView);
                        TxtQuestion.Text = Methods.FunString.DecodeString(DataInfoObject.QuestionThree);
                        
                        EdtQuestion = InflatedQuestionThree.FindViewById<EditText>(Resource.Id.QuestionEditText);
                        Methods.SetColorEditText(EdtQuestion, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
 
                        Methods.SetFocusable(EdtQuestion);
                        EdtQuestion.Touch += (sender, args) =>
                        {
                            if (args.Event.Action != MotionEventActions.Down) return;

                            DialogType = "QuestionThree";
                            var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                            var arrayAdapter = new List<string>();
                            switch (DataInfoObject.QuestionThreeAnswers?.Count)
                            {
                                case > 0:
                                    arrayAdapter = DataInfoObject.QuestionThreeAnswers;
                                    break;
                            }

                            dialogList.Title(Methods.FunString.DecodeString(DataInfoObject.QuestionThree)).TitleColorRes(Resource.Color.primary);
                            dialogList.Items(arrayAdapter);
                            dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                            dialogList.AlwaysCallSingleChoiceCallback();
                            dialogList.ItemsCallback(this).Build().Show();
                        };
                        break;
                    }
                    default:
                        QuestionThreeLayout.Visibility = ViewStates.Gone;
                        break;
                }

                #endregion
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

        private void LoadMyDate()
        {
            try
            {
                var dataUser = ListUtils.MyProfileList?.FirstOrDefault();
                switch (dataUser)
                {
                    case null:
                        return;
                }
                TxtName.Text = WoWonderTools.GetNameFinal(dataUser); 
                TxtPhone.Text = dataUser.PhoneNumber;
                TxtEmail.Text = dataUser.Email;
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
using System;
using System.Collections.Generic;
using System.Linq;
using AFollestad.MaterialDialogs;
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
using WoWonder.Activities.Base;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using WoWonderClient.Classes.Global;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.SettingsPreferences.General
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MyAccountActivity : BaseActivity, View.IOnClickListener, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private EditText TxtUsername,TxtEmail, TxtBirthday, TxtGender, TxtCountry;
        private TextView TxtSave, IconName, IconEmail, IconBirthday, IconGender, IconCountry;
        private string GenderStatus = "", CountryId , TypeDialog = "";

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
                SetContentView(Resource.Layout.Settings_MyAccount_Layout);

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                Get_Data_User();
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
                TxtUsername = FindViewById<EditText>(Resource.Id.NameEditText);

                IconEmail = FindViewById<TextView>(Resource.Id.IconEmail);
                TxtEmail = FindViewById<EditText>(Resource.Id.EmailEditText);
                 
                IconBirthday = FindViewById<TextView>(Resource.Id.IconBirthday);
                TxtBirthday = FindViewById<EditText>(Resource.Id.BirthdayEditText);
                TxtBirthday.SetOnClickListener(this);

                IconGender = (TextView)FindViewById(Resource.Id.IconGender);
                TxtGender = (EditText)FindViewById(Resource.Id.GenderEditText);
               
                IconCountry = FindViewById<TextView>(Resource.Id.IconCountry);
                TxtCountry = FindViewById<EditText>(Resource.Id.CountryEditText);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconName, FontAwesomeIcon.User);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconEmail, FontAwesomeIcon.PaperPlane);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconBirthday, FontAwesomeIcon.BirthdayCake);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconGender, FontAwesomeIcon.VenusMars);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconCountry, FontAwesomeIcon.MapMarkedAlt);  

                Methods.SetColorEditText(TxtCountry, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtUsername, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtEmail, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtBirthday, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtGender, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                Methods.SetFocusable(TxtGender);

                AdsGoogle.Ad_AdMobNative(this);
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
                    toolBar.Title = GetText(Resource.String.Lbl_My_Account);
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
                        TxtGender.Touch += TxtGenderOnTouch;
                        TxtSave.Click += SaveData_OnClick;
                        TxtCountry.Touch += TxtCountryOnTouch;
                        break;
                    default:
                        TxtGender.Touch -= TxtGenderOnTouch;
                        TxtSave.Click -= SaveData_OnClick;
                        TxtCountry.Touch -= TxtCountryOnTouch;
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
                TxtUsername = null!;
                IconEmail = null!;
                TxtEmail = null!;
                IconBirthday = null!;
                TxtBirthday = null!;
                IconGender = null!;
                TxtGender = null!;
                IconCountry = null!;
                TxtCountry = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        private void TxtCountryOnTouch(object sender, View.TouchEventArgs e)
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
                dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
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
                dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
                dialogList.AlwaysCallSingleChoiceCallback();
                dialogList.ItemsCallback(this).Build().Show();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Save data 
        private async void SaveData_OnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                if (Methods.CheckConnectivity())
                {
                    //Show a progress
                    AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));
                     
                    var dictionary = new Dictionary<string, string>
                    {
                        {"username", TxtUsername.Text.Replace(" ","")},
                        {"email", TxtEmail.Text},
                        {"gender", GenderStatus},
                        {"country_id", CountryId},
                    };

                    string newFormat = "";
                    switch (string.IsNullOrEmpty(TxtBirthday.Text))
                    {
                        case false:
                        {
                            var date = TxtBirthday.Text.Split(new char[] {'-' , '/' });
                            newFormat = date.Length switch
                            {
                                > 0 => date[0] + "-" + date[1] + "-" + date[2],
                                _ => newFormat
                            };

                            dictionary.Add("birthday", newFormat);
                            break;
                        }
                    }
                    
                    var (apiStatus, respond) = await WoWonderClient.Requests.RequestsAsync.Global.UpdateUserDataAsync(dictionary);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case MessageObject result when result.Message.Contains("updated"):
                                {
                                    Toast.MakeText(this, result.Message, ToastLength.Short)?.Show();

                                    var local = ListUtils.MyProfileList?.FirstOrDefault();
                                    if (local != null)
                                    {
                                        local.Username = TxtUsername.Text.Replace(" ", "");

                                        local.Birthday = string.IsNullOrEmpty(newFormat) switch
                                        {
                                            false => newFormat,
                                            _ => local.Birthday
                                        };

                                        local.Gender = GenderStatus;
                                        local.GenderText = TxtGender.Text;
                                        local.CountryId = CountryId;

                                        switch (ListUtils.SettingsSiteList?.EmailValidation)
                                        {
                                            case "1" when local.Email != TxtEmail.Text:
                                                //wael send code Email Validation
                                                break;
                                            default:
                                                local.Email = TxtEmail.Text;
                                                break;
                                        }
                                     
                                        var sqLiteDatabase = new SqLiteDatabase(); 
                                        sqLiteDatabase.Insert_Or_Update_To_MyProfileTable(local);
                                    
                                    }
                                
                                    AndHUD.Shared.Dismiss(this);
                                    break;
                                }
                                case MessageObject result:
                                    //Show a Error image with a message
                                    AndHUD.Shared.ShowError(this, result.Message, MaskType.Clear, TimeSpan.FromSeconds(1));
                                    break;
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
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                //Show a Error image with a message
                AndHUD.Shared.ShowError(this, e.Message, MaskType.Clear, TimeSpan.FromSeconds(1));
                //AndHUD.Shared.Dismiss(this);
            }
        }
         
        #endregion
         
        private void Get_Data_User()
        {
            try
            {
                var local = ListUtils.MyProfileList?.FirstOrDefault();
                if (local != null)
                {
                    TxtUsername.Text = local.Username;
                    TxtEmail.Text = local.Email;

                    try
                    {
                        DateTime date = DateTime.Parse(local.Birthday);
                        string newFormat = date.Day + "/" + date.Month + "/" + date.Year;
                        TxtBirthday.Text = newFormat;
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                        TxtBirthday.Text = local.Birthday;
                    }


                    switch (string.IsNullOrEmpty(local.CountryId))
                    {
                        case false when local.CountryId != "0":
                        {
                            var countryName = WoWonderTools.GetCountryList(this).FirstOrDefault(a => a.Key == local.CountryId).Value;

                            TxtCountry.Text = countryName;
                            break;
                        }
                    }

                    switch (ListUtils.SettingsSiteList?.Genders?.Count)
                    {
                        case > 0:
                        {
                            var value = ListUtils.SettingsSiteList?.Genders?.FirstOrDefault(a => a.Key == local.Gender).Value;
                            if (value != null)
                            {
                                TxtGender.Text = value;
                                GenderStatus = local.Gender;
                            }
                            else
                            {
                                TxtGender.Text = GetText(Resource.String.Radio_Male);
                                GenderStatus = "male";
                            }

                            break;
                        }
                        default:
                        {
                            if (local.Gender == GetText(Resource.String.Radio_Male))
                            {
                                TxtGender.Text = GetText(Resource.String.Radio_Male);
                                GenderStatus = "male";
                            }
                            else if (local.Gender == GetText(Resource.String.Radio_Female))
                            {
                                TxtGender.Text = GetText(Resource.String.Radio_Female);
                                GenderStatus = "female";
                            }
                            else
                            {
                                TxtGender.Text = GetText(Resource.String.Radio_Male);
                                GenderStatus = "male";
                            }

                            break;
                        }
                    } 
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        #region MaterialDialog

        public void OnClick(MaterialDialog p0, DialogAction p1)
        {
            try
            {
                if (p1 == DialogAction.Positive)
                {
                    Finish();
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
                    {
                        var countriesArray = WoWonderTools.GetCountryList(this);
                        var check = countriesArray.FirstOrDefault(a => a.Value == itemString.ToString()).Key;
                        if (check != null)
                        {
                            CountryId = check;
                        }

                        TxtCountry.Text = itemString.ToString();
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
         
        public void OnClick(View v)
        {
            try
            {
                if (v.Id == TxtBirthday.Id)
                {
                    var frag = PopupDialogController.DatePickerFragment.NewInstance(delegate (DateTime time)
                    {
                        TxtBirthday.Text = time.Date.ToString("yy-MM-dd");
                    });
                    frag.Show(SupportFragmentManager, PopupDialogController.DatePickerFragment.Tag);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }
}
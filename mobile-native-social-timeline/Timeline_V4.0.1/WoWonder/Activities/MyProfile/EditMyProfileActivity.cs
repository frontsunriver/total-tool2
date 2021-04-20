using System;
using System.Collections.Generic;
using System.Linq;
using AFollestad.MaterialDialogs;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads.DoubleClick;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.Content.Res;
using Java.Lang;
using WoWonder.Activities.Base;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using WoWonderClient.Classes.Global;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.MyProfile
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class EditMyProfileActivity : BaseActivity, View.IOnFocusChangeListener, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private TextView TxtSave;
        //private TextView IconName, IconLocation, IconMobile, IconWebsite, IconWork, IconSchool, IconRelationship;
        private EditText TxtFirstName, TxtLastName, TxtLocation, TxtMobile,TxtWebsite,TxtWork,TxtSchool, TxtRelationship;
        private PublisherAdView PublisherAdView;
        private string TypeDialog,IdRelationShip;

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
                SetContentView(Resource.Layout.EditMyProfile_layout);

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();  
                GetMyInfoData();
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

                //IconName = FindViewById<TextView>(Resource.Id.IconName);
                TxtFirstName = FindViewById<EditText>(Resource.Id.FirstNameEditText);
                TxtLastName = FindViewById<EditText>(Resource.Id.LastNameEditText);
                //IconLocation = FindViewById<TextView>(Resource.Id.IconLocation);
                TxtLocation = FindViewById<EditText>(Resource.Id.LocationEditText);
                //IconMobile = FindViewById<TextView>(Resource.Id.IconPhone);
                TxtMobile = FindViewById<EditText>(Resource.Id.PhoneEditText); 
                //IconWebsite = FindViewById<TextView>(Resource.Id.IconWebsite);
                TxtWebsite = FindViewById<EditText>(Resource.Id.WebsiteEditText); 
                //IconWork = FindViewById<TextView>(Resource.Id.IconWorkStatus);
                TxtWork = FindViewById<EditText>(Resource.Id.WorkStatusEditText); 
                //IconSchool = FindViewById<TextView>(Resource.Id.IconSchool);
                TxtSchool = FindViewById<EditText>(Resource.Id.SchoolEditText);
                //IconRelationship = FindViewById<TextView>(Resource.Id.IconRelationship);
                TxtRelationship = FindViewById<EditText>(Resource.Id.RelationshipEditText);

                /*Methods.SetColorEditText(TxtFirstName, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtLastName, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtLocation, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtMobile, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtWebsite, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtWork, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtSchool, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtRelationship, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);*/
 
                /*FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconName, FontAwesomeIcon.User);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconLocation, FontAwesomeIcon.MapMarkedAlt);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconMobile, FontAwesomeIcon.Mobile);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconWork, FontAwesomeIcon.Briefcase);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconWebsite, FontAwesomeIcon.Globe);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconSchool, FontAwesomeIcon.School);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconRelationship, FontAwesomeIcon.Heart);*/
 
                Methods.SetFocusable(TxtRelationship);

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
                    toolBar.Title = GetString(Resource.String.Lbl_Update_DataProfile);
                    //toolBar.SetTitleTextColor(Color.ParseColor(AppSettings.MainColor));
                    SetSupportActionBar(toolBar);
                    SupportActionBar.SetDisplayShowCustomEnabled(true);
                    SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                    SupportActionBar.SetHomeButtonEnabled(true);
                    SupportActionBar.SetDisplayShowHomeEnabled(true);
                    SupportActionBar.SetHomeAsUpIndicator(AppCompatResources.GetDrawable(this, AppSettings.FlowDirectionRightToLeft ? Resource.Drawable.ic_action_right_arrow_color : Resource.Drawable.ic_action_left_arrow_color));

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
                        TxtSave.Click +=  TxtSaveOnClick;
                        TxtLocation.OnFocusChangeListener = this; 
                        TxtRelationship.Touch += TxtRelationshipOnTouch;
                        break;
                    default:
                        TxtSave.Click -= TxtSaveOnClick;
                        TxtLocation.OnFocusChangeListener = null!;
                        TxtRelationship.Touch -= TxtRelationshipOnTouch;
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
                //IconName = null!;
                TxtFirstName = null!;
                TxtLastName  = null!;
                //IconLocation = null!;
                TxtLocation  = null!;
                //IconMobile = null!;
                TxtMobile = null!;
                //IconWebsite  = null!;
                TxtWebsite = null!;
                //IconWork = null!;
                TxtWork = null!;
                //IconSchool = null!;
                TxtSchool = null!;
                //IconRelationship = null!;
                TxtRelationship = null!;
                PublisherAdView = null!;
                IdRelationShip = null!;
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
                    //Show a progress
                    AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));

                    var dictionary = new Dictionary<string, string>
                    {
                        {"first_name", TxtFirstName.Text},
                        {"last_name", TxtLastName.Text},
                        {"address", TxtLocation.Text},
                        {"phone_number", TxtMobile.Text},
                        {"website", TxtWebsite.Text},
                        {"working", TxtWork.Text},
                        {"school", TxtSchool.Text},
                        {"relationship", IdRelationShip}
                    };

                    var (apiStatus, respond) = await RequestsAsync.Global.UpdateUserDataAsync(dictionary);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case MessageObject result:
                                {
                                    var dataUser = ListUtils.MyProfileList?.FirstOrDefault();
                                    if (dataUser != null)
                                    {
                                        dataUser.FirstName = TxtFirstName.Text;
                                        dataUser.LastName = TxtLastName.Text;
                                        dataUser.Address = TxtLocation.Text;
                                        dataUser.PhoneNumber = TxtMobile.Text;
                                        dataUser.Website = TxtWebsite.Text;
                                        dataUser.Working = TxtWork.Text;
                                        dataUser.School = TxtSchool.Text;
                                        dataUser.RelationshipId = IdRelationShip;

                                        var sqLiteDatabase = new SqLiteDatabase();
                                        sqLiteDatabase.Insert_Or_Update_To_MyProfileTable(dataUser);
                                
                                    }

                                    Toast.MakeText(this, result.Message, ToastLength.Short)?.Show();
                                    AndHUD.Shared.Dismiss(this);

                                    Intent  intent = new Intent();
                                    SetResult(Result.Ok , intent);
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
                        //Open intent Camera when the request code of result is 502
                        new IntentController(this).OpenIntentLocation();
                        break;
                    default:
                    {
                        if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) == Permission.Granted && CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) == Permission.Granted)
                        {
                            //Open intent Camera when the request code of result is 502
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

        private void TxtRelationshipOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                TypeDialog = "RelationShip";

                string[] relationshipArray = Application.Context.Resources?.GetStringArray(Resource.Array.RelationShipArray);

                var dialogList = new MaterialDialog.Builder(this);

                var arrayAdapter = relationshipArray?.ToList();

                dialogList.Title(GetText(Resource.String.Lbl_ChooseRelationshipStatus)).TitleColorRes(Resource.Color.primary);
                dialogList.Items(arrayAdapter);
                dialogList.PositiveText(GetText(Resource.String.Lbl_Close)).OnPositive(this);
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
                        //Open intent Camera when the request code of result is 502
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

        private void GetMyInfoData()
        {
            try
            { 
                var local = ListUtils.MyProfileList?.FirstOrDefault();
                if (local != null)
                {
                    TxtFirstName.Text = Methods.FunString.DecodeString(local.FirstName);
                    TxtLastName.Text = Methods.FunString.DecodeString(local.LastName);
                    TxtLocation.Text = local.Address;
                    TxtMobile.Text = local.PhoneNumber;
                    TxtWebsite.Text = local.Website;
                    TxtWork.Text = local.Working;
                    TxtSchool.Text = local.School;
                    IdRelationShip = local.RelationshipId;

                    string relationship = WoWonderTools.GetRelationship(Convert.ToInt32(local.RelationshipId));
                    if (Methods.FunString.StringNullRemover(relationship) != "Empty")
                    {
                        TxtRelationship.Text = relationship;
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
                    case "RelationShip":
                        IdRelationShip = itemId.ToString();
                        TxtRelationship.Text = itemString.ToString();
                        break;
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

        public void OnFocusChange(View v, bool hasFocus)
        {
            if (v?.Id == TxtLocation.Id && hasFocus)
            {
                TxtLocationOnClick();
            } 
        }
         
    }
}
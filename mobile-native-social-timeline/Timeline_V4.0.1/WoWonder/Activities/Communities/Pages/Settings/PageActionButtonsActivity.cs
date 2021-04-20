using System;
using System.Collections.Generic;
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
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Communities.Pages.Settings
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class PageActionButtonsActivity : BaseActivity, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private TextView TxtSave, IconCallToAction, IconCallToTargetUrl;
        private EditText TxtCallToAction, TxtCallToTargetUrl;
        private string  PagesId = "", CallToActionId = "", DialogType;
        private PageClass PageData;
        private readonly string[] CallToAction = Application.Context.Resources?.GetStringArray(Resource.Array.call_action_type);
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
                SetContentView(Resource.Layout.PageActionButtonsLayout);

                var id = Intent?.GetStringExtra("PageId") ?? "Data not available";
                if (id != "Data not available" && !string.IsNullOrEmpty(id)) PagesId = id;

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();

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

                IconCallToAction = FindViewById<TextView>(Resource.Id.IconCallToAction);
                TxtCallToAction = FindViewById<EditText>(Resource.Id.CallToActionEditText);

                IconCallToTargetUrl = FindViewById<TextView>(Resource.Id.IconCallToTargetUrl);
                TxtCallToTargetUrl = FindViewById<EditText>(Resource.Id.CallToTargetUrlEditText);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconCallToAction, FontAwesomeIcon.BezierCurve);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconCallToTargetUrl, FontAwesomeIcon.Bullseye);

                Methods.SetColorEditText(TxtCallToAction, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtCallToTargetUrl, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                Methods.SetFocusable(TxtCallToAction);

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
                    toolBar.Title = GetText(Resource.String.Lbl_ActionButtons);
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
                        TxtCallToAction.Touch += TxtCallToActionOnTouch;
                        break;
                    default:
                        TxtSave.Click -= TxtSaveOnClick;
                        TxtCallToAction.Touch -= TxtCallToActionOnTouch;
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
                IconCallToAction = null!;
                IconCallToTargetUrl = null!;
                TxtCallToAction = null!;
                TxtCallToTargetUrl = null!;
                PagesId = "";
                CallToActionId = "";
                DialogType = null!;
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

        //CallToAction
        private void TxtCallToActionOnTouch(object sender, View.TouchEventArgs e)
        {
            try
            {
                if (e?.Event?.Action != MotionEventActions.Down) return;

                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                DialogType = "CallToAction";
                var arrayAdapter = CallToAction.ToList();

                dialogList.Title(GetText(Resource.String.Lbl_CallToAction)).TitleColorRes(Resource.Color.primary);
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
                    //Show a progress
                    AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading) + "...");

                    var dictionary = new Dictionary<string, string>
                    {
                        {"call_action_type", CallToActionId},
                        {"call_action_type_text", TxtCallToAction.Text},
                        {"call_action_type_url", TxtCallToTargetUrl.Text},
                    };

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

                                    PageData.CallActionTypeText = TxtCallToAction.Text;
                                    PageData.CallActionTypeUrl = TxtCallToTargetUrl.Text;
                                    PageData.CallActionType = CallToActionId;

                                    PageProfileActivity.PageData = PageData;

                                    Toast.MakeText(this, GetText(Resource.String.Lbl_YourPageWasUpdated), ToastLength.Short)?.Show();
                             
                                    Intent returnIntent = new Intent();
                                    returnIntent.PutExtra("pageItem", JsonConvert.SerializeObject(PageData));
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
                    case "CallToAction":
                        TxtCallToAction.Text = itemString.ToString();
                        CallToActionId = (itemId + 1).ToString();
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

        //Get Data Page and set Categories
        private void Get_Data_Page()
        {
            try
            {
                PageData = JsonConvert.DeserializeObject<PageClass>(Intent?.GetStringExtra("PageData") ?? "");
                if (PageData != null)
                {
                    TxtCallToAction.Text = PageData.CallActionTypeText;
                    TxtCallToTargetUrl.Text = PageData.CallActionTypeUrl;

                    CallToActionId = PageData.CallActionType;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

    }
}
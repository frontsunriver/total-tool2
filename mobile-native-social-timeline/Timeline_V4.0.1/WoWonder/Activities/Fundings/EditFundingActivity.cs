using System;
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
using WoWonderClient.Classes.Funding;
using WoWonderClient.Classes.Global;
using WoWonderClient.Requests;
using Console = System.Console;
using Exception = System.Exception;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Fundings
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class EditFundingActivity : BaseActivity, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        #region Variables Basic

        private TextView TxtAdd,IconTitle, IconAmount, IconDescription;
        private EditText TxtTitle, TxtAmount, TxtDescription;
        private FundingDataObject DataObject;
        private RelativeLayout ImageLayout;
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
                SetContentView(Resource.Layout.CreateFundingLayout);

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();

                GetDataFunding();
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
                TxtAdd = FindViewById<TextView>(Resource.Id.toolbar_title);
                
                IconTitle = FindViewById<TextView>(Resource.Id.IconTitle);
                TxtTitle = FindViewById<EditText>(Resource.Id.TitleEditText);

                IconAmount = FindViewById<TextView>(Resource.Id.IconAmount);
                TxtAmount = FindViewById<EditText>(Resource.Id.AmountEditText);

                IconDescription = FindViewById<TextView>(Resource.Id.IconDescription);
                TxtDescription = FindViewById<EditText>(Resource.Id.DescriptionEditText);

                TxtAdd.Text = GetText(Resource.String.Lbl_Save);
                TxtAdd.SetTextColor(Color.White);

                ImageLayout = FindViewById<RelativeLayout>(Resource.Id.imageLayout);
                ImageLayout.Visibility = ViewStates.Gone;

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconTitle, FontAwesomeIcon.User);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconAmount, FontAwesomeIcon.MoneyBillWave);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconDescription, FontAwesomeIcon.AudioDescription);

                Methods.SetColorEditText(TxtTitle, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtAmount, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                Methods.SetColorEditText(TxtDescription, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                //Methods.SetFocusable(TxtAmount);

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
                    toolBar.Title = GetString(Resource.String.Lbl_FundingRequests);
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
                        TxtAdd.Click += TxtAddOnClick;
                        //TxtAmount.Touch += TxtAmountOnTouch;
                        break;
                    default:
                        TxtAdd.Click -= TxtAddOnClick;
                        //TxtAmount.Touch -= TxtAmountOnTouch;
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
                TxtAdd = null!;
                IconTitle  = null!;
                TxtTitle = null!;
                IconAmount = null!;
                TxtAmount = null!;
                IconDescription = null!;
                TxtDescription = null!; 
                ImageLayout = null!;

                PublisherAdView = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        //Amount
        //private void TxtAmountOnTouch(object sender, View.TouchEventArgs e)
        //{
        //    try
        //    {
        //        if (e?.Event?.Action != MotionEventActions.Down) return;

        //        var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

        //        var arrayAdapter = new List<string> { "5", "10", "15", "20", "25", "30", "35", "40", "45", "50", "55", "60", "65", "70", "75", "80", "85", "90", "95", "100" };

        //        dialogList.Title(GetText(Resource.String.Lbl_Amount));
        //        dialogList.Items(arrayAdapter);
        //        dialogList.NegativeText(GetText(Resource.String.Lbl_Close)).OnNegative(this);
        //        dialogList.AlwaysCallSingleChoiceCallback();
        //        dialogList.ItemsCallback(this).Build().Show();
        //    }
        //    catch (Exception exception)
        //    {
        //        Methods.DisplayReportResultTrack(exception);
        //    }
        //}

        //Save 
        private async void TxtAddOnClick(object sender, EventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
                else
                {
                    if (string.IsNullOrEmpty(TxtTitle.Text) || string.IsNullOrWhiteSpace(TxtTitle.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_name), ToastLength.Short)?.Show();
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtAmount.Text) || string.IsNullOrWhiteSpace(TxtAmount.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_amount), ToastLength.Short)?.Show();
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtDescription.Text))
                    {
                        Toast.MakeText(this, GetText(Resource.String.Lbl_Please_enter_Description), ToastLength.Short)?.Show();
                        return;
                    }

                    //Show a progress
                    AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));

                    var (apiStatus, respond) = await RequestsAsync.Funding.EditFundingAsync(DataObject.Id ,TxtTitle.Text, TxtDescription.Text, TxtAmount.Text);
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

                                    var instance = FundingActivity.GetInstance();
                                    var dataFunding = instance?.FundingTab?.MAdapter?.FundingList?.FirstOrDefault(a => a.Id == DataObject.Id);
                                    if (dataFunding != null)
                                    {
                                        dataFunding.Id = DataObject.Id;
                                        dataFunding.Title = TxtTitle.Text;
                                        dataFunding.Description = TxtDescription.Text;
                                        dataFunding.Amount = TxtAmount.Text;

                                        var index = instance.FundingTab.MAdapter.FundingList.IndexOf(dataFunding);
                                        switch (index)
                                        {
                                            case > -1:
                                                instance.FundingTab.MAdapter.FundingList[index] = dataFunding;
                                                instance.FundingTab.MAdapter.NotifyItemChanged(index);
                                                break;
                                        } 
                                    }

                                    var dataMyFunding = instance?.MyFundingTab?.MAdapter?.FundingList?.FirstOrDefault(a => a.Id == DataObject.Id);
                                    if (dataMyFunding != null)
                                    {
                                        dataMyFunding.Id = DataObject.Id;
                                        dataMyFunding.Title = TxtTitle.Text;
                                        dataMyFunding.Description = TxtDescription.Text;
                                        dataMyFunding.Amount = TxtAmount.Text;

                                        var index = instance.MyFundingTab.MAdapter.FundingList.IndexOf(dataMyFunding);
                                        switch (index)
                                        {
                                            case > -1:
                                                instance.MyFundingTab.MAdapter.FundingList[index] = dataMyFunding;
                                                instance.MyFundingTab.MAdapter.NotifyItemChanged(index);
                                                break;
                                        }

                                        Intent intent = new Intent();
                                        intent.PutExtra("itemData", JsonConvert.SerializeObject(dataMyFunding));
                                        SetResult(Result.Ok, intent); 
                                    }

                                    break;
                                }
                            }
 
                            Toast.MakeText(this, GetString(Resource.String.Lbl_FundingSuccessfullyEdited), ToastLength.Short)?.Show();
                            Finish();
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
                TxtAmount.Text = itemString.ToString();
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

        private void GetDataFunding()
        {
            try
            {
                DataObject = JsonConvert.DeserializeObject<FundingDataObject>(Intent?.GetStringExtra("FundingObject"));
                if (DataObject != null)
                { 
                    TxtTitle.Text = Methods.FunString.DecodeString(DataObject.Title);
                    TxtDescription.Text = Methods.FunString.DecodeString(DataObject.Description);

                    TxtAmount.Text = DataObject.Amount; 
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

    }
}
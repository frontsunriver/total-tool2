using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS; 
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using WoWonder.Activities.Base;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Covid19
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class Covid19Activity : BaseActivity 
    {
        #region Variables Basic

        private TextView TxtCountry, TxtNewCases, TxtNewDeaths, TxtActiveCases, TxtDeaths, TxtTotalCases, TxtRecovered, TxtTime;
        private TextView IconVirus1, IconVirus2, IconVirus3, IconVirus4,IconVirus5, IconVirus6, IconTime;
        
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
                SetContentView(Resource.Layout.CoronaVirusLayout);

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();

                StartApiService();
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
                TxtCountry = FindViewById<TextView>(Resource.Id.country);

                TxtNewCases = FindViewById<TextView>(Resource.Id.newCases);
                TxtNewDeaths = FindViewById<TextView>(Resource.Id.newDeaths);
                TxtActiveCases = FindViewById<TextView>(Resource.Id.activeCases);
                TxtDeaths = FindViewById<TextView>(Resource.Id.deaths);
                TxtTotalCases = FindViewById<TextView>(Resource.Id.totalcases);
                TxtRecovered = FindViewById<TextView>(Resource.Id.recovered);
                TxtTime = FindViewById<TextView>(Resource.Id.time);

                IconVirus1 = FindViewById<TextView>(Resource.Id.iconVirus1);
                IconVirus2 = FindViewById<TextView>(Resource.Id.iconVirus2);
                IconVirus3 = FindViewById<TextView>(Resource.Id.iconVirus3);
                IconVirus4 = FindViewById<TextView>(Resource.Id.iconVirus4);
                IconVirus5 = FindViewById<TextView>(Resource.Id.iconVirus5);
                IconVirus6 = FindViewById<TextView>(Resource.Id.iconVirus6);

                IconTime = FindViewById<TextView>(Resource.Id.iconTime);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconVirus1, IonIconsFonts.IosBug);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconVirus2, IonIconsFonts.IosBug);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconVirus3, IonIconsFonts.IosBug);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconVirus4, IonIconsFonts.IosBug);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconVirus5, IonIconsFonts.IosBug);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconVirus6, IonIconsFonts.IosBug);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconTime, FontAwesomeIcon.Clock);
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
                    toolBar.Title = GetText(Resource.String.Lbl_CoronaVirus);
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
         
        private void DestroyBasic()
        {
            try
            {
                TxtCountry = null!; 
                TxtNewCases = null!;
                TxtNewDeaths = null!;
                TxtActiveCases = null!;
                TxtDeaths = null!;
                TxtTotalCases = null!;
                TxtRecovered = null!;
                TxtTime = null!; 
                IconVirus1 = null!;
                IconVirus2 = null!;
                IconVirus3 = null!;
                IconVirus4 = null!;
                IconVirus5 = null!;
                IconVirus6 = null!; 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion
         
        #region Load Data 
         
        private void StartApiService()
        {
            if (!Methods.CheckConnectivity())
                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
            else
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { LoadDataAsync });
        }
         
        private async Task LoadDataAsync()
        {
            var (apiStatus, respond) = await ApiRequest.GetInfoCovid19Async(UserDetails.Country);
            if (apiStatus != 200 || respond is not Covid19Object result || result.Response == null)
            {
                switch (AppSettings.SetApisReportMode)
                {
                    case true when apiStatus != 400 && respond is ErrorCovid19Object error:
                        Methods.DialogPopup.InvokeAndShowDialog(this, "ReportMode", error.Message, "Close");
                        break;
                }
            }
            else
            {
                 RunOnUiThread(() =>
                 {
                     try
                     {
                         var data = result.Response.FirstOrDefault(a => a.Country == UserDetails.Country);
                         if (data != null)
                         {
                             TxtCountry.Text = data.Country;

                             TxtNewCases.Text = data.Cases.New?.Replace("+", "") ?? "0";
                             TxtNewDeaths.Text = data.Deaths.New?.Replace("+", "") ?? "0";
                             TxtActiveCases.Text = data.Cases.Active;
                             TxtDeaths.Text = data.Deaths.Total;
                             TxtTotalCases.Text = data.Cases.Total;
                             TxtRecovered.Text = data.Cases.Recovered;
                              
                             TxtTime.Text = data.Day; 
                         }
                         else
                         {
                             TxtCountry.Text = "0";
                             TxtNewCases.Text = "0";
                             TxtNewDeaths.Text = "0";
                             TxtActiveCases.Text = "0";
                             TxtDeaths.Text = "0";
                             TxtTotalCases.Text = "0";
                             TxtRecovered.Text = "0";
                             TxtTime.Text = ""; 
                         } 
                     }
                     catch (Exception exception)
                     {
                         Methods.DisplayReportResultTrack(exception);
                     }
                 });
            } 
        }
          
        #endregion


    }
}
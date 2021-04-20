using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;

using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.Content.Res;
using AndroidX.RecyclerView.Widget;
using WoWonder.Activities.Base;
using WoWonder.Activities.SettingsPreferences.Adapters;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Requests;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.SettingsPreferences.InvitationLinks
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class InvitationLinksActivity : BaseActivity
    {
        #region Variables Basic

        private TextView AvailableTextView, GeneratedCount, UsedCount;
        private Button GenerateButton;

        private RecyclerView MRecycler;
        private InvitationLinksAdapter MAdapter;
        private LinearLayoutManager LayoutManager;

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
                SetContentView(Resource.Layout.InvitationLinksLayout);

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();

                StartApiService();

                AdsGoogle.Ad_Interstitial(this);
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
                AvailableTextView = FindViewById<TextView>(Resource.Id.availableTextView);
                GeneratedCount = FindViewById<TextView>(Resource.Id.generatedCount);
                UsedCount = FindViewById<TextView>(Resource.Id.usedCount);
                GenerateButton = FindViewById<Button>(Resource.Id.GenerateButton);
                MRecycler = FindViewById<RecyclerView>(Resource.Id.recyler);
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
                    toolBar.Title = GetText(Resource.String.Lbl_InvitationLinks);
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
                MAdapter = new InvitationLinksAdapter(this)
                {
                    LinksList = new ObservableCollection<InvitationDataObject>()
                };
                LayoutManager = new LinearLayoutManager(this);
                MRecycler.SetLayoutManager(LayoutManager);
                MRecycler.HasFixedSize = true;
                MRecycler.SetItemViewCacheSize(10);
                MRecycler.GetLayoutManager().ItemPrefetchEnabled = true;
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
                        MAdapter.CopyItemClick += MAdapterOnCopyItemClick;
                        MAdapter.ItemClick += MAdapterOnItemClick;
                        GenerateButton.Click += GenerateButtonOnClick;
                        break;
                    default:
                        MAdapter.CopyItemClick -= MAdapterOnCopyItemClick;
                        MAdapter.ItemClick -= MAdapterOnItemClick;
                        GenerateButton.Click -= GenerateButtonOnClick;
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
                MAdapter = null!;
                MRecycler = null!;
                AvailableTextView = null!;
                GeneratedCount = null!;
                UsedCount = null!;
                GenerateButton = null!;
                MRecycler = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        //Generate new link
        private async void GenerateButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
                    return;
                }

                //Show a progress
                AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));

                var (apiStatus, respond) = await RequestsAsync.Global.CreateInvitationCodeAsync();
                switch (apiStatus)
                {
                    case 200:
                    {
                        switch (respond)
                        {
                            case CreateInvitationCodeObject result:
                            {
                                var unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                         

                                MAdapter.LinksList.Add(new InvitationDataObject
                                {
                                    Link = result.Link,
                                    Time = unixTimestamp.ToString()
                                });

                                MAdapter.NotifyDataSetChanged();

                                AndHUD.Shared.Dismiss(this);
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
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                AndHUD.Shared.Dismiss(this);
            }
        }

        //Open profile 
        private void MAdapterOnItemClick(object sender, InvitationLinksAdapterClickEventArgs e)
        {
            try
            {
                var position = e.Position;
                switch (position)
                {
                    case > -1:
                    {
                        var item = MAdapter.GetItem(position);
                        if (item?.UserData?.UserDataClass != null)
                        {
                            WoWonderTools.OpenProfile(this, item.UserData.Value.UserDataClass.UserId, item.UserData.Value.UserDataClass);
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

        //Copy link
        private void MAdapterOnCopyItemClick(object sender, InvitationLinksAdapterClickEventArgs e)
        {
            try
            {
                var position = e.Position;
                switch (position)
                {
                    case > -1:
                    {
                        var item = MAdapter.GetItem(position);
                        switch (string.IsNullOrEmpty(item?.Link))
                        {
                            case false:
                                Methods.CopyToClipboard(this, item.Link);
                                break;
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

        #endregion

        #region Load Data

        private void StartApiService()
        {
            if (!Methods.CheckConnectivity())
                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
            else
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { LoadData });
        }

        private async Task LoadData()
        {
            if (Methods.CheckConnectivity())
            {
                var countList = MAdapter.LinksList.Count;
                var (respondCode, respondString) = await RequestsAsync.Global.InvitationAsync();
                switch (respondCode)
                {
                    case 200:
                    {
                        switch (respondString)
                        {
                            case InvitationObject result:
                            {
                                RunOnUiThread(() => 
                                {
                                    try
                                    {
                                        AvailableTextView.Text = result.AvailableLinks;

                                        if (result.GeneratedLinks != null)
                                            GeneratedCount.Text = result.GeneratedLinks.Value.ToString();

                                        if (result.UsedLinks != null)
                                            UsedCount.Text = result.UsedLinks.Value.ToString();
                                    }
                                    catch (Exception e)
                                    {
                                        Methods.DisplayReportResultTrack(e);
                                    }
                                });
                      
                                var respondList = result.Data.Count;
                                switch (respondList)
                                {
                                    case > 0 when countList > 0:
                                    {
                                        foreach (var item in from item in result.Data let check = MAdapter.LinksList.FirstOrDefault(a => a.Id == item.Id) where check == null select item)
                                        {
                                            MAdapter.LinksList.Add(item);
                                        }

                                        RunOnUiThread(() => { MAdapter.NotifyItemRangeInserted(countList, MAdapter.LinksList.Count - countList); });
                                        break;
                                    }
                                    case > 0:
                                        MAdapter.LinksList = new ObservableCollection<InvitationDataObject>(result.Data);
                                        RunOnUiThread(() => { MAdapter.NotifyDataSetChanged(); });
                                        break;
                                    default:
                                    {
                                        switch (MAdapter.LinksList.Count)
                                        {
                                            case > 10 when !MRecycler.CanScrollVertically(1):
                                                Toast.MakeText(this, GetText(Resource.String.Lbl_NoMoreFunding), ToastLength.Short)?.Show();
                                                break;
                                        }

                                        break;
                                    }
                                }

                                break;
                            }
                        }

                        break;
                    }
                    default:
                        Methods.DisplayReportResult(this, respondString);
                        break;
                }

                RunOnUiThread(ShowEmptyPage);
            }
            else
            {
                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
            }
        }

        private void ShowEmptyPage()
        {
            try
            {
                MRecycler.Visibility = MAdapter.LinksList.Count switch
                {
                    > 0 => ViewStates.Visible,
                    _ => ViewStates.Gone
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        #endregion


    }
}
using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;


using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.Content.Res;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using Java.IO;
using WoWonder.Activities.Base;
using WoWonder.Activities.SettingsPreferences.Adapters;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Requests;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.SettingsPreferences.MyInformation
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MyInformationActivity : BaseActivity
    {
        #region Variables Basic

        private ImageView ImageUser;

        private MyInformationAdapter MAdapter; 
        private RecyclerView MRecycler;
        private GridLayoutManager LayoutManager;
        private TextView TxtName;
        
        private Button BtnDownload;
        private string Link;
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
                SetContentView(Resource.Layout.MyInformationLayout);

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();

                AdsGoogle.Ad_RewardedVideo(this);
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
                MRecycler = (RecyclerView)FindViewById(Resource.Id.recyler);

                ImageUser = FindViewById<ImageView>(Resource.Id.imageUser);
                TxtName = FindViewById<TextView>(Resource.Id.nameUser);

                BtnDownload = FindViewById<Button>(Resource.Id.downloadButton);
                BtnDownload.Visibility = ViewStates.Gone;
                 
                var myProfile = ListUtils.MyProfileList?.FirstOrDefault();
                if (myProfile != null)
                {
                    GlideImageLoader.LoadImage(this, myProfile.Avatar, ImageUser, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                    TxtName.Text = WoWonderTools.GetNameFinal(myProfile); 
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
                    toolBar.Title = GetText(Resource.String.Lbl_DownloadMyInformation);
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
                MAdapter = new MyInformationAdapter(this);
                LayoutManager = new GridLayoutManager(this, 2);
                LayoutManager.SetSpanSizeLookup(new MySpanSizeLookup(4, 1, 1)); //5, 1, 2 
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
                        MAdapter.ItemClick += MAdapterOnItemClick;
                        BtnDownload.Click += BtnDownloadOnClick;
                        break;
                    default:
                        MAdapter.ItemClick -= MAdapterOnItemClick;
                        BtnDownload.Click -= BtnDownloadOnClick;
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
                ImageUser= null!;
                TxtName = null!; 
                BtnDownload = null!;
                
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events

        private void BtnDownloadOnClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Link)) return;

                 var fileName = Link.Split('/').Last();
                 Link = WoWonderTools.GetFile("", Methods.Path.FolderDcimFile, fileName, Link);

                 var fileSplit = Link.Split('/').Last();
                 string getFile = Methods.MultiMedia.GetMediaFrom_Disk(Methods.Path.FolderDcimFile, fileSplit);
                 if (getFile != "File Dont Exists")
                 {
                     File file2 = new File(getFile);
                     var photoUri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", file2);

                     Intent openFile = new Intent(Intent.ActionView, photoUri);
                     openFile.SetFlags(ActivityFlags.NewTask);
                     openFile.SetFlags(ActivityFlags.GrantReadUriPermission);
                     StartActivity(openFile);
                 }
                 else
                 {
                     Intent intent = new Intent(Intent.ActionView, Uri.Parse(Link));
                     StartActivity(intent);
                 }
                  
                Toast.MakeText(this, GetText(Resource.String.Lbl_YourFileIsDownloaded), ToastLength.Long)?.Show();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception); 
            }
        }


        //Download Info
        private async void MAdapterOnItemClick(object sender, MyInformationAdapterClickEventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
                    return;
                }
                 
                var position = e.Position;
                switch (position)
                {
                    case > -1:
                    {
                        var item = MAdapter.GetItem(position);
                        if (item != null)
                        {
                            //Show a progress
                            AndHUD.Shared.Show(this, GetText(Resource.String.Lbl_Loading));

                            var (apiStatus, respond) = await RequestsAsync.Global.DownloadInfoAsync(item.Type);
                            switch (apiStatus)
                            {
                                case 200:
                                {
                                    switch (respond)
                                    {
                                        case DownloadInfoObject result:
                                        {
                                            Link = result.Link;
                                            var fileName = Link.Split('/').Last();
                                            WoWonderTools.GetFile("", Methods.Path.FolderDcimFile, fileName, Link);

                                            BtnDownload.Visibility = ViewStates.Visible;
                                 
                                            Toast.MakeText(this, GetText(Resource.String.Lbl_YourFileIsReady), ToastLength.Long)?.Show(); 
                                
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
    }
}
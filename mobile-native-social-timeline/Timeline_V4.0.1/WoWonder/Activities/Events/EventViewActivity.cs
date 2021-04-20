using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using AndroidX.SwipeRefreshLayout.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Google.Android.Material.AppBar;
using Google.Android.Material.FloatingActionButton;
using Java.Lang;
using Newtonsoft.Json;
using WoWonder.Library.Anjo.Share;
using WoWonder.Library.Anjo.Share.Abstractions;
using WoWonder.Activities.AddPost;
using WoWonder.Activities.Base;
using WoWonder.Activities.Live.Utils;
using WoWonder.Activities.NativePost.Extra;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonder.Library.Anjo.SuperTextLibrary;
using WoWonderClient.Classes.Event;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Classes.Product;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Uri = Android.Net.Uri;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using String = Java.Lang.String;

namespace WoWonder.Activities.Events
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class EventViewActivity : BaseActivity, IOnMapReadyCallback, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback, AppBarLayout.IOnOffsetChangedListener
    {
        #region Variables Basic

        private CollapsingToolbarLayout ToolbarLayout;
        private GoogleMap Map;
        private double CurrentLongitude, CurrentLatitude;
        private TextView TxtName, TxtGoing, TxtInterested, TxtStartDate, TxtEndDate, TxtLocation, TxtDescription;
        private SuperTextView TxtDescriptionText;
        private FloatingActionButton FloatingActionButtonView;
        private ImageView ImageEventCover;
        private Button BtnGo, BtnInterested;
        public WRecyclerView MainRecyclerView;
        public NativePostAdapter PostFeedAdapter;
        private ImageButton BtnMore;
        private EventDataObject EventData;
        private static EventViewActivity Instance;
        private SwipeRefreshLayout SwipeRefreshLayout;
        private AppBarLayout AppBarLayout;
        private string Name, EventType, EventId;
        private bool IsShow = true;
        private int ScrollRange = -1;
        private FeedCombiner Combiner;
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
                SetContentView(Resource.Layout.EventView_Layout);

                Instance = this;

                EventId = Intent?.GetStringExtra("EventId") ?? "";
                EventType = Intent?.GetStringExtra("EventType") ?? "";
                  
                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();
                SetRecyclerViewAdapters();

                LoadData();
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
        protected override void OnStop()
        {
            try
            {
                base.OnStop();
                MainRecyclerView?.StopVideo();
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
                MainRecyclerView.ReleasePlayer();
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
                ToolbarLayout = FindViewById<CollapsingToolbarLayout>(Resource.Id.collapsingToolbar);
                ToolbarLayout.Title = "";
                AppBarLayout = FindViewById<AppBarLayout>(Resource.Id.appbar_ptwo);
                TxtName = FindViewById<TextView>(Resource.Id.tvName_ptwo);

                TxtGoing = FindViewById<TextView>(Resource.Id.GoingTextview);

                TxtInterested = FindViewById<TextView>(Resource.Id.InterestedTextview);
                TxtStartDate = FindViewById<TextView>(Resource.Id.txtStartDate);
                TxtEndDate = FindViewById<TextView>(Resource.Id.txtEndDate);


                TxtLocation = FindViewById<TextView>(Resource.Id.LocationTextview);
                TxtDescription = FindViewById<TextView>(Resource.Id.tv_about);
                TxtDescriptionText = FindViewById<SuperTextView>(Resource.Id.tv_aboutdescUser);
                 
                ImageEventCover = FindViewById<ImageView>(Resource.Id.EventCover);

                BtnGo = FindViewById<Button>(Resource.Id.ButtonGoing);
                BtnInterested = FindViewById<Button>(Resource.Id.ButtonIntersted);

                FloatingActionButtonView = FindViewById<FloatingActionButton>(Resource.Id.floatingActionButtonView);
                FloatingActionButtonView.Visibility = ViewStates.Visible;

                MainRecyclerView = FindViewById<WRecyclerView>(Resource.Id.newsfeedRecyler);
                BtnMore = (ImageButton)FindViewById(Resource.Id.morebutton);

                SwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);
                SwipeRefreshLayout.SetColorSchemeResources(Android.Resource.Color.HoloBlueLight, Android.Resource.Color.HoloGreenLight, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
                SwipeRefreshLayout.Refreshing = false;
                SwipeRefreshLayout.Enabled = false;
                SwipeRefreshLayout.SetProgressBackgroundColorSchemeColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#424242") : Color.ParseColor("#f7f7f7"));

                AppBarLayout.AddOnOffsetChangedListener(this);

                var mapFrag = SupportMapFragment.NewInstance();
                SupportFragmentManager.BeginTransaction().Add(Resource.Id.map, mapFrag, mapFrag.Tag)?.Commit();
                mapFrag.GetMapAsync(this);
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
                PostFeedAdapter = new NativePostAdapter(this, EventId, MainRecyclerView, NativeFeedType.Event);
                MainRecyclerView.SetXAdapter(PostFeedAdapter, null);
                Combiner = new FeedCombiner(null, PostFeedAdapter.ListDiffer, this);
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
                        FloatingActionButtonView.Click += AddPostOnClick;
                        BtnGo.Click += BtnGoOnClick;
                        BtnInterested.Click += BtnInterestedOnClick;
                        BtnMore.Click += BtnMoreOnClick;
                        break;
                    default:
                        FloatingActionButtonView.Click -= AddPostOnClick;
                        BtnGo.Click -= BtnGoOnClick;
                        BtnInterested.Click -= BtnInterestedOnClick;
                        BtnMore.Click -= BtnMoreOnClick;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public static EventViewActivity GetInstance()
        {
            try
            {
                return Instance;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }


        private void DestroyBasic()
        {
            try
            {
                ToolbarLayout = null!;
                AppBarLayout = null!;
                TxtName = null!;
                TxtGoing = null!;
                TxtInterested = null!;
                TxtStartDate  = null!;
                TxtEndDate = null!;
                TxtLocation = null!;
                TxtDescription = null!;
                TxtDescriptionText = null!;
                ImageEventCover = null!;
                BtnGo = null!;
                BtnInterested = null!;
                FloatingActionButtonView = null!;
                MainRecyclerView = null!;
                BtnMore = null!;
                SwipeRefreshLayout = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        //Event Show More : Copy Link , Share , Edit (If user isOwner_Event)
        private void BtnMoreOnClick(object sender, EventArgs e)
        {
            try
            {
                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(this).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                arrayAdapter.Add(GetString(Resource.String.Lbl_CopeLink));
                arrayAdapter.Add(GetString(Resource.String.Lbl_Share));
                switch (EventData.IsOwner)
                {
                    case true:
                        arrayAdapter.Add(GetString(Resource.String.Lbl_Edit));
                        break;
                }

                dialogList.Title(GetString(Resource.String.Lbl_More)).TitleColorRes(Resource.Color.primary);
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

        private void BtnInterestedOnClick(object sender, EventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    return;
                }

                switch (BtnInterested?.Tag?.ToString())
                {
                    case "false":
                        BtnInterested.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                        BtnInterested.SetTextColor(Color.ParseColor("#ffffff"));
                        BtnInterested.Text = GetText(Resource.String.Lbl_Interested);
                        BtnInterested.Tag = "true";
                        break;
                    default:
                        BtnInterested.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                        BtnInterested.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                        BtnInterested.Text = GetText(Resource.String.Lbl_Interested);
                        BtnInterested.Tag = "false";
                        break;
                }

                var dataEvent = EventMainActivity.GetInstance()?.EventTab.MAdapter.EventList?.FirstOrDefault(a => a.Id == EventData.Id);
                if (dataEvent != null)
                {
                    dataEvent.IsInterested = Convert.ToBoolean(BtnInterested?.Tag?.ToString());
                }

                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Event.InterestEventAsync(EventData.Id) });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void BtnGoOnClick(object sender, EventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    return;
                }

                switch (BtnGo?.Tag?.ToString())
                {
                    case "false":
                        BtnGo.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                        BtnGo.SetTextColor(Color.ParseColor("#ffffff"));
                        BtnGo.Text = GetText(Resource.String.Lbl_Going);
                        BtnGo.Tag = "true";
                        break;
                    default:
                        BtnGo.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                        BtnGo.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                        BtnGo.Text = GetText(Resource.String.Lbl_Go);
                        BtnGo.Tag = "false";
                        break;
                }

                var list = EventMainActivity.GetInstance()?.EventTab.MAdapter?.EventList;
                var dataEvent = list?.FirstOrDefault(a => a.Id == EventData.Id);
                if (dataEvent != null)
                {
                    dataEvent.IsGoing = Convert.ToBoolean(BtnGo?.Tag?.ToString());
                    EventMainActivity.GetInstance()?.EventTab.MAdapter.NotifyItemChanged(list.IndexOf(dataEvent));
                }

                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Event.GoToEventAsync(EventData.Id) });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void AddPostOnClick(object sender, EventArgs e)
        {
            try
            {
                var intent = new Intent(this, typeof(AddPostActivity));
                intent.PutExtra("Type", "SocialEvent");
                intent.PutExtra("PostId", EventData.Id);
                intent.PutExtra("itemObject", JsonConvert.SerializeObject(EventData));
                StartActivityForResult(intent, 2500);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Location

        public async void OnMapReady(GoogleMap googleMap)
        {
            try
            {
                var latLng = await GetLocationFromAddress(EventData.Location);
                if (latLng != null)
                {
                    CurrentLatitude = latLng.Latitude;
                    CurrentLongitude = latLng.Longitude;
                }

                Map = googleMap;

                //Optional
                googleMap.UiSettings.ZoomControlsEnabled = false;
                googleMap.UiSettings.CompassEnabled = false;

                googleMap.MoveCamera(CameraUpdateFactory.ZoomIn());

                var makerOptions = new MarkerOptions();
                makerOptions.SetPosition(new LatLng(CurrentLatitude, CurrentLongitude));
                makerOptions.SetTitle(GetText(Resource.String.Lbl_EventPlace));

                Map.AddMarker(makerOptions);
                Map.MapType = GoogleMap.MapTypeNormal;

                switch (AppSettings.SetTabDarkTheme)
                {
                    case true:
                    {
                        MapStyleOptions style = MapStyleOptions.LoadRawResourceStyle(this, Resource.Raw.map_dark);
                        Map.SetMapStyle(style);
                        break;
                    }
                }

                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(new LatLng(CurrentLatitude, CurrentLongitude));
                builder.Zoom(10);
                builder.Bearing(155);
                builder.Tilt(65);

                CameraPosition cameraPosition = builder.Build();

                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                googleMap.MoveCamera(cameraUpdate);

                Map.MapClick += MapOnMapClick;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void MapOnMapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            try
            {
                // Create a Uri from an intent string. Use the result to create an Intent?. 
                var uri = Uri.Parse("geo:" + CurrentLatitude + "," + CurrentLongitude);
                var intent = new Intent(Intent.ActionView, uri);
                intent.SetPackage("com.google.android.apps.maps");
                intent.AddFlags(ActivityFlags.NewTask);
                StartActivity(intent);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private async Task<LatLng> GetLocationFromAddress(string strAddress)
        {
            #pragma warning disable 618
            var locale = (int)Build.VERSION.SdkInt < 25 ? Resources?.Configuration?.Locale : Resources?.Configuration?.Locales.Get(0) ?? Resources?.Configuration?.Locale;
            #pragma warning restore 618
            Geocoder coder = new Geocoder(this, locale);

            try
            {
                var address = await coder.GetFromLocationNameAsync(strAddress, 2);
                switch (address)
                {
                    case null:
                        return null!;
                }

                Address location = address[0];
                var lat = location.Latitude;
                var lng = location.Longitude;

                LatLng p1 = new LatLng(lat, lng);
                return p1;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
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
                    //add post
                    case 2500 when resultCode == Result.Ok:
                    {
                        if (!string.IsNullOrEmpty(data.GetStringExtra("itemObject")))
                        {
                            var postData = JsonConvert.DeserializeObject<PostDataObject>(data.GetStringExtra("itemObject"));
                            if (postData != null)
                            {
                                var countList = PostFeedAdapter.ItemCount;

                                var combine = new FeedCombiner(postData, PostFeedAdapter.ListDiffer, this);
                                combine.CombineDefaultPostSections("Top");

                                int countIndex = 1;
                                var model1 = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.Story);
                                var model2 = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                                var model3 = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AlertBox);
                                var model4 = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SearchForPosts);

                                if (model4 != null)
                                    countIndex += PostFeedAdapter.ListDiffer.IndexOf(model4) + 1;
                                else if (model3 != null)
                                    countIndex += PostFeedAdapter.ListDiffer.IndexOf(model3) + 1;
                                else if (model2 != null)
                                    countIndex += PostFeedAdapter.ListDiffer.IndexOf(model2) + 1;
                                else if (model1 != null)
                                    countIndex += PostFeedAdapter.ListDiffer.IndexOf(model1) + 1;
                                else
                                    countIndex = 0;

                                PostFeedAdapter.NotifyItemRangeInserted(countIndex, PostFeedAdapter.ListDiffer.Count - countList);
                            }
                        }
                        else
                        {
                            PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => MainRecyclerView.ApiPostAsync.FetchNewsFeedApiPosts() });
                        }

                        break;
                    }
                    //Edit post
                    case 3950 when resultCode == Result.Ok:
                    {
                        var postId = data.GetStringExtra("PostId") ?? "";
                        var postText = data.GetStringExtra("PostText") ?? "";
                        var diff = PostFeedAdapter.ListDiffer;
                        List<AdapterModelsClass> dataGlobal = diff.Where(a => a.PostData?.Id == postId).ToList();
                        switch (dataGlobal.Count)
                        {
                            case > 0:
                            {
                                foreach (var postData in dataGlobal)
                                {
                                    postData.PostData.Orginaltext = postText;
                                    var index = diff.IndexOf(postData);
                                    switch (index)
                                    {
                                        case > -1:
                                            PostFeedAdapter.NotifyItemChanged(index);
                                            break;
                                    }
                                }

                                var checkTextSection = dataGlobal.FirstOrDefault(w => w.TypeView == PostModelType.TextSectionPostPart);
                                switch (checkTextSection)
                                {
                                    case null:
                                    {
                                        var collection = dataGlobal.FirstOrDefault()?.PostData;
                                        var item = new AdapterModelsClass
                                        {
                                            TypeView = PostModelType.TextSectionPostPart,
                                            Id = Convert.ToInt32((int)PostModelType.TextSectionPostPart + collection?.Id),
                                            PostData = collection,
                                            IsDefaultFeedPost = true
                                        };

                                        var headerPostIndex = diff.IndexOf(dataGlobal.FirstOrDefault(w => w.TypeView == PostModelType.HeaderPost));
                                        switch (headerPostIndex)
                                        {
                                            case > -1:
                                                diff.Insert(headerPostIndex + 1, item);
                                                PostFeedAdapter.NotifyItemInserted(headerPostIndex + 1);
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
                    //Edit post product 
                    case 3500 when resultCode == Result.Ok:
                    {
                        if (string.IsNullOrEmpty(data.GetStringExtra("itemData"))) return;
                        var item = JsonConvert.DeserializeObject<ProductDataObject>(data.GetStringExtra("itemData"));
                        if (item != null)
                        {
                            var diff = PostFeedAdapter.ListDiffer;
                            var dataGlobal = diff.Where(a => a.PostData?.Id == item.PostId).ToList();
                            switch (dataGlobal.Count)
                            {
                                case > 0:
                                {
                                    foreach (var postData in dataGlobal)
                                    {
                                        var index = diff.IndexOf(postData);
                                        switch (index)
                                        {
                                            case > -1:
                                            {
                                                var productUnion = postData.PostData.Product?.ProductClass;
                                                if (productUnion != null) productUnion.Id = item.Id;
                                                productUnion = item;
                                                Console.WriteLine(productUnion);

                                                PostFeedAdapter.NotifyItemChanged(PostFeedAdapter.ListDiffer.IndexOf(postData));
                                                break;
                                            }
                                        }
                                    }

                                    break;
                                }
                            }
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

        //Permissions
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            try
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                switch (requestCode)
                { 
                    case 235 when grantResults.Length > 0 && grantResults[0] == Permission.Granted:
                        new LiveUtil(this).OpenDialogLive();
                        break;
                    case 235:
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

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                string text = itemString.ToString();
                if (text == GetString(Resource.String.Lbl_CopeLink))
                {
                    CopyLinkEvent();
                }
                else if (text == GetString(Resource.String.Lbl_Share))
                {
                    ShareEvent();
                }
                else if (text == GetString(Resource.String.Lbl_Edit))
                {
                    EditInfoEvent_OnClick();
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

        //Event Menu >> Copy Link
        private void CopyLinkEvent()
        {
            try
            {
                Methods.CopyToClipboard(this, EventData.Url); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Event Menu >> Share
        private async void ShareEvent()
        {
            try
            {
                switch (CrossShare.IsSupported)
                {
                    //Share Plugin  
                    case false:
                        return;
                    default:
                        await CrossShare.Current.Share(new ShareMessage
                        {
                            Title = EventData.Name,
                            Text = EventData.Description,
                            Url = EventData.Url
                        });
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        //Event Menu >> Edit Info Event if user == is_owner
        private void EditInfoEvent_OnClick()
        {
            try
            {
                switch (EventData.IsOwner)
                {
                    case true:
                    {
                        var intent = new Intent(this, typeof(EditEventActivity));
                        intent.PutExtra("EventData", JsonConvert.SerializeObject(EventData));
                        intent.PutExtra("EventId", EventData.Id);
                        StartActivity(intent);
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

        private void LoadData()
        {
            try
            {
                EventData = JsonConvert.DeserializeObject<EventDataObject>(Intent?.GetStringExtra("EventView") ?? "");
                if (EventData != null) 
                {
                    GetDataEvent(EventData);
                }

                if (!Methods.CheckConnectivity())
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                else
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { GetEventById }); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
          
        private void GetDataEvent(EventDataObject eventData)
        {
            try
            { 
                if (eventData != null)
                {
                    Glide.With(this).Load(eventData.Cover).Apply(new RequestOptions()).Into(ImageEventCover);

                    Name = Methods.FunString.DecodeString(eventData.Name);

                    TxtName.Text = Name;
                    ToolbarLayout.Title = Name;
                    SupportActionBar.Title = Name;

                    if (string.IsNullOrEmpty(eventData.GoingCount))
                        eventData.GoingCount = "0";

                    if (string.IsNullOrEmpty(eventData.InterestedCount))
                        eventData.InterestedCount = "0";

                    TxtGoing.Text = eventData.GoingCount + " " + GetText(Resource.String.Lbl_GoingPeople);
                    TxtInterested.Text = eventData.InterestedCount + " " + GetText(Resource.String.Lbl_InterestedPeople);
                    TxtLocation.Text = eventData.Location;

                    TxtStartDate.Text = eventData.StartDate;
                    TxtEndDate.Text = eventData.EndDate;


                    switch (string.IsNullOrEmpty(eventData.Description))
                    {
                        case false:
                        {
                            var description = Methods.FunString.DecodeString(eventData.Description);
                            var readMoreOption = new StReadMoreOption.Builder()
                                .TextLength(250, StReadMoreOption.TypeCharacter)
                                .MoreLabel(GetText(Resource.String.Lbl_ReadMore))
                                .LessLabel(GetText(Resource.String.Lbl_ReadLess))
                                .MoreLabelColor(Color.ParseColor(AppSettings.MainColor))
                                .LessLabelColor(Color.ParseColor(AppSettings.MainColor))
                                .LabelUnderLine(true)
                                .Build();
                            readMoreOption.AddReadMoreTo(TxtDescriptionText, new String(description));
                            break;
                        }
                        default:
                            TxtDescription.Visibility = ViewStates.Gone;
                            TxtDescriptionText.Visibility = ViewStates.Gone;
                            break;
                    }
                     
                    switch (EventType)
                    {
                        case "events":

                            break;
                        case "going":
                            eventData.IsGoing = true;
                            break;
                        case "past":

                            break;
                        case "myEvent":
                            BtnGo.Visibility = ViewStates.Invisible;
                            BtnInterested.Visibility = ViewStates.Invisible; 
                            break;
                        case "interested":
                            eventData.IsInterested = true;
                            break;
                        case "invited":

                            break;
                    }
                     
                    if (eventData.IsGoing != null && eventData.IsGoing.Value)
                    {
                        BtnGo.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                        BtnGo.SetTextColor(Color.ParseColor("#ffffff"));
                        BtnGo.Text = GetText(Resource.String.Lbl_Going);
                        BtnGo.Tag = "true";
                    }
                    else
                    {
                        BtnGo.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                        BtnGo.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                        BtnGo.Text = GetText(Resource.String.Lbl_Go);
                        BtnGo.Tag = "false";
                    }

                    if (eventData.IsInterested != null && eventData.IsInterested.Value)
                    {
                        BtnInterested.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                        BtnInterested.SetTextColor(Color.ParseColor("#ffffff"));
                        BtnInterested.Text = GetText(Resource.String.Lbl_Interested);
                        BtnInterested.Tag = "true";
                    }
                    else
                    {
                        BtnInterested.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                        BtnInterested.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                        BtnInterested.Text = GetText(Resource.String.Lbl_Interested);
                        BtnInterested.Tag = "false";
                    }
                     
                    //add post  
                    var checkSection = PostFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                    switch (checkSection)
                    {
                        case null:
                            Combiner.AddPostDivider();
                            Combiner.AddPostBoxPostView("Event", -1, new PostDataObject { Event = new EventUnion { EventClass = eventData } });
                         
                            PostFeedAdapter.NotifyItemInserted(PostFeedAdapter.ListDiffer.Count -1);
                            break;
                    }

                    StartApiService();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void StartApiService()
        {
            if (!Methods.CheckConnectivity())
                Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
            else
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => MainRecyclerView.ApiPostAsync.FetchNewsFeedApiPosts() });
        }
         
        private async Task GetEventById()
        {
            try
            {
                if (Methods.CheckConnectivity())
                {
                    var (apiStatus, respond) = await RequestsAsync.Event.GetEventById(EventId);
                    switch (apiStatus)
                    {
                        case 200:
                        {
                            switch (respond)
                            {
                                case GetEventByIdObject result:
                                    GetDataEvent(result.Data);
                                    break;
                            }

                            break;
                        }
                        default:
                            Methods.DisplayReportResult(this, respond);
                            break;
                    }
                }
                else
                {
                    Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public void OnOffsetChanged(AppBarLayout appBarLayout, int verticalOffset)
        {
            ScrollRange = ScrollRange switch
            {
                -1 => appBarLayout.TotalScrollRange,
                _ => ScrollRange
            };

            switch (ScrollRange + verticalOffset)
            {
                case 0:
                    ToolbarLayout.Title = Name;
                    IsShow = true;
                    break;
                default:
                {
                    switch (IsShow)
                    {
                        case true:
                            ToolbarLayout.Title = " ";
                            IsShow = false;
                            break;
                    }

                    break;
                }
            }
        }
    }
}
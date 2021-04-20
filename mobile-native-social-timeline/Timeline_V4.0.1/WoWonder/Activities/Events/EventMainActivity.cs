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
using AndroidX.AppCompat.Content.Res;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Tabs;
using WoWonder.Activities.Base;
using WoWonder.Activities.Events.Fragment;
using WoWonder.Adapters;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Event;
using WoWonderClient.Requests;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace WoWonder.Activities.Events
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class EventMainActivity : BaseActivity, TabLayoutMediator.ITabConfigurationStrategy
    {
        #region Variables Basic

        private static EventMainActivity Instance;
        private MainTabAdapter Adapter;
        private ViewPager2 ViewPager;
        public EventFragment EventTab;
        public MyEventFragment MyEventTab;
        private GoingFragment GoingTab;
        private InterestedFragment InterestedTab;
        private InvitedFragment InvitedTab;
        private PastFragment PastTab;
        private TabLayout TabLayout;
        private FloatingActionButton FloatingActionButtonView;
         
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
                SetContentView(Resource.Layout.EventMain_Layout);

                Instance = this;

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();

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
                ViewPager = FindViewById<ViewPager2>(Resource.Id.viewpager);
                TabLayout = FindViewById<TabLayout>(Resource.Id.tabs);

                FloatingActionButtonView = FindViewById<FloatingActionButton>(Resource.Id.floatingActionButtonView);

                SetUpViewPager(ViewPager);
                new TabLayoutMediator(TabLayout, ViewPager, this).Attach();
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
                    toolBar.Title = GetText(Resource.String.Lbl_Events);
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
                        FloatingActionButtonView.Click += BtnCreateEventsOnClick;
                        break;
                    default:
                        FloatingActionButtonView.Click -= BtnCreateEventsOnClick;
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
                ViewPager = null!;
                EventTab = null!;
                MyEventTab = null!;
                GoingTab = null!;
                InterestedTab = null!;
                InvitedTab = null!;
                PastTab = null!;
                TabLayout = null!;
                FloatingActionButtonView = null!;
                Instance = null!;
                
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        public static EventMainActivity GetInstance()
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
         
        #endregion

        #region Events

        private void BtnCreateEventsOnClick(object sender, EventArgs e)
        {
            try
            {
                StartActivity(new Intent(this, typeof(CreateEventActivity)));
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }


        #endregion

        #region Set Tap

        private void SetUpViewPager(ViewPager2 viewPager)
        {
            try
            {
                EventTab = new EventFragment();
                GoingTab = new GoingFragment();
                InvitedTab = new InvitedFragment();
                InterestedTab = new InterestedFragment();
                PastTab = new PastFragment();
                MyEventTab = new MyEventFragment();

                Adapter = new MainTabAdapter(this);

                Adapter.AddFragment(EventTab, GetText(Resource.String.Lbl_All_Events));

                switch (AppSettings.ShowEventGoing)
                {
                    case true:
                        Adapter.AddFragment(GoingTab, GetText(Resource.String.Lbl_Going));
                        break;
                }

                switch (AppSettings.ShowEventInvited)
                {
                    case true:
                        Adapter.AddFragment(InvitedTab, GetText(Resource.String.Lbl_Invited));
                        break;
                }

                switch (AppSettings.ShowEventInterested)
                {
                    case true:
                        Adapter.AddFragment(InterestedTab, GetText(Resource.String.Lbl_Interested));
                        break;
                }

                switch (AppSettings.ShowEventPast)
                {
                    case true:
                        Adapter.AddFragment(PastTab, GetText(Resource.String.Lbl_Past));
                        break;
                }

                Adapter.AddFragment(MyEventTab, GetText(Resource.String.Lbl_My_Events));

                viewPager.CurrentItem = Adapter.ItemCount;
                viewPager.OffscreenPageLimit = Adapter.ItemCount;

                viewPager.Orientation = ViewPager2.OrientationHorizontal;
                viewPager.Adapter = Adapter;
                viewPager.Adapter.NotifyDataSetChanged();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void OnConfigureTab(TabLayout.Tab tab, int position)
        {
            try
            {
                tab.SetText(Adapter.GetFragment(position));
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        #region Get Event Api 

        public void StartApiService(string offset = "0", string typeEvent = "events")
        {
            if (Methods.CheckConnectivity())
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => GetEvent(offset, typeEvent) });
            else
                Toast.MakeText(this, GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Long)?.Show();
        }

        private async Task GetEvent(string offset = "0", string typeEvent = "events")
        {
            switch (typeEvent)
            {
                case "events" when EventTab.MainScrollEvent != null && EventTab.MainScrollEvent.IsLoading:
                    return;
            }

            if (Methods.CheckConnectivity())
            { 
                var dictionary = new Dictionary<string, string>();
                switch (typeEvent)
                {
                    case "events":
                        dictionary.Add("offset", offset);
                        dictionary.Add("fetch", "events");
                        if (EventTab?.MainScrollEvent != null)
                            EventTab.MainScrollEvent.IsLoading = true;
                        break;
                    case "going":
                        dictionary.Add("going_offset", offset);
                        dictionary.Add("fetch", "going");
                        GoingTab.MainScrollEvent.IsLoading = AppSettings.ShowEventGoing switch
                        {
                            true when GoingTab?.MainScrollEvent != null => true,
                            _ => GoingTab.MainScrollEvent.IsLoading
                        };
                        break;
                    case "past":
                        dictionary.Add("past_offset", offset);
                        dictionary.Add("fetch", "past");
                        PastTab.MainScrollEvent.IsLoading = AppSettings.ShowEventPast switch
                        {
                            true when PastTab?.MainScrollEvent != null => true,
                            _ => PastTab.MainScrollEvent.IsLoading
                        };
                        break;
                    case "myEvent":
                        dictionary.Add("my_offset", offset);
                        dictionary.Add("fetch", "my_events");
                        if (MyEventTab?.MainScrollEvent != null)
                            MyEventTab.MainScrollEvent.IsLoading = true;
                        break;
                    case "interested":
                        dictionary.Add("interested_offset", offset);
                        dictionary.Add("fetch", "interested");
                        InterestedTab.MainScrollEvent.IsLoading = AppSettings.ShowEventInterested switch
                        {
                            true when InterestedTab?.MainScrollEvent != null => true,
                            _ => InterestedTab.MainScrollEvent.IsLoading
                        };
                        break;
                    case "invited":
                        dictionary.Add("invited_offset", offset);
                        dictionary.Add("fetch", "invited");
                        InvitedTab.MainScrollEvent.IsLoading = AppSettings.ShowEventInvited switch
                        {
                            true when InvitedTab?.MainScrollEvent != null => true,
                            _ => InvitedTab.MainScrollEvent.IsLoading
                        };
                        break;
                }
                 
                var (apiStatus, respond) = await RequestsAsync.Event.GetEventsAsync(dictionary);
                switch (apiStatus)
                {
                    case 200:
                    {
                        switch (respond)
                        {
                            case GetEventsObject result:
                            {
                                switch (typeEvent)
                                {
                                    //Events
                                    //==============================================================
                                    case "events" when EventTab != null:
                                    {
                                        var countList = EventTab.MAdapter.EventList.Count;
                                        var respondList = result.Events.Count;
                                        switch (respondList)
                                        {
                                            case > 0 when countList > 0:
                                            {
                                                foreach (var item in from item in result.Events let check = EventTab.MAdapter.EventList.FirstOrDefault(a => a.Id == item.Id) where check == null select item)
                                                {
                                                    EventTab.MAdapter.EventList.Add(item);
                                                }

                                                RunOnUiThread(() => { EventTab.MAdapter.NotifyItemRangeInserted(countList, EventTab.MAdapter.EventList.Count - countList); });
                                                break;
                                            }
                                            case > 0:
                                                EventTab.MAdapter.EventList = new ObservableCollection<EventDataObject>(result.Events);
                                                RunOnUiThread(() => { EventTab.MAdapter.NotifyDataSetChanged(); });
                                                break;
                                            default:
                                            {
                                                switch (EventTab.MAdapter.EventList.Count)
                                                {
                                                    case > 10 when !EventTab.MRecycler.CanScrollVertically(1):
                                                        Toast.MakeText(this, GetText(Resource.String.Lbl_NoMoreEvent), ToastLength.Short)?.Show();
                                                        break;
                                                }

                                                break;
                                            }
                                        }

                                        RunOnUiThread(() => { ShowEmptyPage("Event"); });
                                        break;
                                    }
                                }

                                switch (AppSettings.ShowEventGoing)
                                {
                                    //Going 
                                    //==============================================================
                                    case true when typeEvent == "going" && GoingTab != null:
                                    {
                                        int countGoingList = GoingTab.MAdapter.EventList.Count;

                                        var respondGoingList = result.Going.Count;
                                        switch (respondGoingList)
                                        {
                                            case > 0 when countGoingList > 0:
                                            {
                                                foreach (var item in from item in result.Going let check = GoingTab.MAdapter.EventList.FirstOrDefault(a => a.Id == item.Id) where check == null select item)
                                                {
                                                    GoingTab.MAdapter.EventList.Add(item);
                                                }

                                                RunOnUiThread(() => { GoingTab.MAdapter.NotifyItemRangeInserted(countGoingList - 1, GoingTab.MAdapter.EventList.Count - countGoingList); });
                                                break;
                                            }
                                            case > 0:
                                                GoingTab.MAdapter.EventList = new ObservableCollection<EventDataObject>(result.Going);
                                                RunOnUiThread(() => { GoingTab.MAdapter.NotifyDataSetChanged(); });
                                                break;
                                            default:
                                            {
                                                switch (GoingTab.MAdapter.EventList.Count)
                                                {
                                                    case > 10 when !GoingTab.MRecycler.CanScrollVertically(1):
                                                        Toast.MakeText(this, GetText(Resource.String.Lbl_NoMoreEvent), ToastLength.Short)?.Show();
                                                        break;
                                                }

                                                break;
                                            }
                                        }

                                        RunOnUiThread(() => { ShowEmptyPage("Going"); });
                                        break;
                                    }
                                }

                                switch (AppSettings.ShowEventInvited)
                                {
                                    //Invited 
                                    //==============================================================
                                    case true when typeEvent == "invited" && InvitedTab != null:
                                    {
                                        int countInvitedList = InvitedTab.MAdapter.EventList.Count;

                                        var respondInvitedList = result.Invited.Count;
                                        switch (respondInvitedList)
                                        {
                                            case > 0 when countInvitedList > 0:
                                            {
                                                foreach (var item in from item in result.Invited let check = InvitedTab.MAdapter.EventList.FirstOrDefault(a => a.Id == item.Id) where check == null select item)
                                                {
                                                    InvitedTab.MAdapter.EventList.Add(item);
                                                }

                                                RunOnUiThread(() => { InvitedTab.MAdapter.NotifyItemRangeInserted(countInvitedList - 1, InvitedTab.MAdapter.EventList.Count - countInvitedList); });
                                                break;
                                            }
                                            case > 0:
                                                InvitedTab.MAdapter.EventList = new ObservableCollection<EventDataObject>(result.Invited);
                                                RunOnUiThread(() => { InvitedTab.MAdapter.NotifyDataSetChanged(); });
                                                break;
                                            default:
                                            {
                                                switch (InvitedTab.MAdapter.EventList.Count)
                                                {
                                                    case > 10 when !InvitedTab.MRecycler.CanScrollVertically(1):
                                                        Toast.MakeText(this, GetText(Resource.String.Lbl_NoMoreEvent), ToastLength.Short)?.Show();
                                                        break;
                                                }

                                                break;
                                            }
                                        }

                                        RunOnUiThread(() => { ShowEmptyPage("Invited"); });
                                        break;
                                    }
                                }

                                switch (AppSettings.ShowEventInterested)
                                {
                                    //Interested 
                                    //==============================================================
                                    case true when typeEvent == "interested" && InterestedTab != null:
                                    {
                                        int countInterestedList = InterestedTab.MAdapter.EventList.Count;

                                        var respondInterestedList = result.Interested.Count;
                                        switch (respondInterestedList)
                                        {
                                            case > 0 when countInterestedList > 0:
                                            {
                                                foreach (var item in from item in result.Interested let check = InterestedTab.MAdapter.EventList.FirstOrDefault(a => a.Id == item.Id) where check == null select item)
                                                {
                                                    InterestedTab.MAdapter.EventList.Add(item);
                                                }

                                                RunOnUiThread(() => { InterestedTab.MAdapter.NotifyItemRangeInserted(countInterestedList - 1, InterestedTab.MAdapter.EventList.Count - countInterestedList); });
                                                break;
                                            }
                                            case > 0:
                                                InterestedTab.MAdapter.EventList = new ObservableCollection<EventDataObject>(result.Interested);
                                                RunOnUiThread(() => { InterestedTab.MAdapter.NotifyDataSetChanged(); });
                                                break;
                                            default:
                                            {
                                                switch (InterestedTab.MAdapter.EventList.Count)
                                                {
                                                    case > 10 when !InterestedTab.MRecycler.CanScrollVertically(1):
                                                        Toast.MakeText(this, GetText(Resource.String.Lbl_NoMoreEvent), ToastLength.Short)?.Show();
                                                        break;
                                                }

                                                break;
                                            }
                                        }

                                        RunOnUiThread(() => { ShowEmptyPage("Interested"); });
                                        break;
                                    }
                                }

                                switch (AppSettings.ShowEventPast)
                                {
                                    //Past 
                                    //==============================================================
                                    case true when typeEvent == "past" && PastTab != null:
                                    {
                                        int countPastList = PastTab.MAdapter.EventList.Count;

                                        var respondPastList = result.Past.Count;
                                        switch (respondPastList)
                                        {
                                            case > 0 when countPastList > 0:
                                            {
                                                foreach (var item in from item in result.Past let check = PastTab.MAdapter.EventList.FirstOrDefault(a => a.Id == item.Id) where check == null select item)
                                                {
                                                    PastTab.MAdapter.EventList.Add(item);
                                                }

                                                RunOnUiThread(() => { PastTab.MAdapter.NotifyItemRangeInserted(countPastList - 1, PastTab.MAdapter.EventList.Count - countPastList); });
                                                break;
                                            }
                                            case > 0:
                                                PastTab.MAdapter.EventList = new ObservableCollection<EventDataObject>(result.Past);
                                                RunOnUiThread(() => { PastTab.MAdapter.NotifyDataSetChanged(); });
                                                break;
                                            default:
                                            {
                                                switch (PastTab.MAdapter.EventList.Count)
                                                {
                                                    case > 10 when !PastTab.MRecycler.CanScrollVertically(1):
                                                        Toast.MakeText(this, GetText(Resource.String.Lbl_NoMoreEvent), ToastLength.Short)?.Show();
                                                        break;
                                                }

                                                break;
                                            }
                                        }

                                        RunOnUiThread(() => { ShowEmptyPage("Past"); });
                                        break;
                                    }
                                }

                                switch (typeEvent)
                                {
                                    //My Event 
                                    //==============================================================
                                    case "myEvent" when MyEventTab != null:
                                    {
                                        int myEventsCountList = MyEventTab.MAdapter.EventList.Count;
                                        var myEventsList = result.MyEvents.Count;
                                        switch (myEventsList)
                                        {
                                            case > 0 when myEventsCountList > 0:
                                            {
                                                foreach (var item in from item in result.MyEvents let check = MyEventTab.MAdapter.EventList.FirstOrDefault(a => a.Id == item.Id) where check == null select item)
                                                {
                                                    MyEventTab.MAdapter.EventList.Add(item);
                                                }

                                                RunOnUiThread(() => { MyEventTab.MAdapter.NotifyItemRangeInserted(myEventsCountList - 1, MyEventTab.MAdapter.EventList.Count - myEventsCountList); });
                                                break;
                                            }
                                            case > 0:
                                                MyEventTab.MAdapter.EventList = new ObservableCollection<EventDataObject>(result.MyEvents);
                                                RunOnUiThread(() => { MyEventTab.MAdapter.NotifyDataSetChanged(); });
                                                break;
                                            default:
                                            {
                                                switch (MyEventTab.MAdapter.EventList.Count)
                                                {
                                                    case > 10 when !MyEventTab.MRecycler.CanScrollVertically(1):
                                                        Toast.MakeText(this, GetText(Resource.String.Lbl_NoMoreEvent), ToastLength.Short)?.Show();
                                                        break;
                                                }

                                                break;
                                            }
                                        }

                                        RunOnUiThread(() => { ShowEmptyPage("MyEvent"); });
                                        break;
                                    }
                                }

                                break;
                            }
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
                if (MyEventTab != null)
                {
                    EventTab.Inflated = EventTab.Inflated switch
                    {
                        null => EventTab.EmptyStateLayout.Inflate(),
                        _ => EventTab.Inflated
                    };

                    EmptyStateInflater x = new EmptyStateInflater();
                    x.InflateLayout(EventTab.Inflated, EmptyStateInflater.Type.NoConnection);
                    switch (x.EmptyStateButton.HasOnClickListeners)
                    {
                        case false:
                            x.EmptyStateButton.Click += null!;
                            x.EmptyStateButton.Click += EmptyStateButtonOnClick;
                            break;
                    }

                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    if (EventTab?.MainScrollEvent != null) EventTab.MainScrollEvent.IsLoading = false;
                }
               
            }
        }
         

        private void ShowEmptyPage(string type)
        {
            try
            {
                switch (type)
                {
                    case "Event":
                    {
                        EventTab.MainScrollEvent.IsLoading = false;
                        EventTab.SwipeRefreshLayout.Refreshing = false;

                        switch (EventTab.MAdapter.EventList.Count)
                        {
                            case > 0:
                                EventTab.MRecycler.Visibility = ViewStates.Visible;
                                EventTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                                break;
                            default:
                            {
                                EventTab.MRecycler.Visibility = ViewStates.Gone;

                                EventTab.Inflated = EventTab.Inflated switch
                                {
                                    null => EventTab.EmptyStateLayout.Inflate(),
                                    _ => EventTab.Inflated
                                };

                                EmptyStateInflater x = new EmptyStateInflater();
                                x.InflateLayout(EventTab.Inflated, EmptyStateInflater.Type.NoEvent);
                                switch (x.EmptyStateButton.HasOnClickListeners)
                                {
                                    case false:
                                        x.EmptyStateButton.Click += null!;
                                        x.EmptyStateButton.Click += BtnCreateEventsOnClick;
                                        break;
                                }
                                EventTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                                break;
                            }
                        }

                        break;
                    }
                    case "Going":
                    {
                        GoingTab.MainScrollEvent.IsLoading = false;
                        GoingTab.SwipeRefreshLayout.Refreshing = false;

                        switch (GoingTab.MAdapter.EventList.Count)
                        {
                            case > 0:
                                GoingTab.MRecycler.Visibility = ViewStates.Visible;
                                GoingTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                                break;
                            default:
                            {
                                GoingTab.MRecycler.Visibility = ViewStates.Gone;

                                GoingTab.Inflated = GoingTab.Inflated switch
                                {
                                    null => GoingTab.EmptyStateLayout.Inflate(),
                                    _ => GoingTab.Inflated
                                };

                                EmptyStateInflater x = new EmptyStateInflater();
                                x.InflateLayout(GoingTab.Inflated, EmptyStateInflater.Type.NoEvent);
                                switch (x.EmptyStateButton.HasOnClickListeners)
                                {
                                    case false:
                                        x.EmptyStateButton.Click += null!;
                                        x.EmptyStateButton.Click += BtnCreateEventsOnClick;
                                        break;
                                }
                                GoingTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                                break;
                            }
                        }

                        break;
                    }
                    case "Invited":
                    {
                        InvitedTab.MainScrollEvent.IsLoading = false;
                        InvitedTab.SwipeRefreshLayout.Refreshing = false;

                        switch (InvitedTab.MAdapter.EventList.Count)
                        {
                            case > 0:
                                InvitedTab.MRecycler.Visibility = ViewStates.Visible;
                                InvitedTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                                break;
                            default:
                            {
                                InvitedTab.MRecycler.Visibility = ViewStates.Gone;

                                InvitedTab.Inflated = InvitedTab.Inflated switch
                                {
                                    null => InvitedTab.EmptyStateLayout.Inflate(),
                                    _ => InvitedTab.Inflated
                                };

                                EmptyStateInflater x = new EmptyStateInflater();
                                x.InflateLayout(InvitedTab.Inflated, EmptyStateInflater.Type.NoEvent);
                                switch (x.EmptyStateButton.HasOnClickListeners)
                                {
                                    case false:
                                        x.EmptyStateButton.Click += null!;
                                        x.EmptyStateButton.Click += BtnCreateEventsOnClick;
                                        break;
                                }
                                InvitedTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                                break;
                            }
                        }

                        break;
                    }
                    case "Interested":
                    {
                        InterestedTab.MainScrollEvent.IsLoading = false;
                        InterestedTab.SwipeRefreshLayout.Refreshing = false;

                        switch (InterestedTab.MAdapter.EventList.Count)
                        {
                            case > 0:
                                InterestedTab.MRecycler.Visibility = ViewStates.Visible;
                                InterestedTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                                break;
                            default:
                            {
                                InterestedTab.MRecycler.Visibility = ViewStates.Gone;

                                InterestedTab.Inflated = InterestedTab.Inflated switch
                                {
                                    null => InterestedTab.EmptyStateLayout.Inflate(),
                                    _ => InterestedTab.Inflated
                                };

                                EmptyStateInflater x = new EmptyStateInflater();
                                x.InflateLayout(InterestedTab.Inflated, EmptyStateInflater.Type.NoEvent);
                                switch (x.EmptyStateButton.HasOnClickListeners)
                                {
                                    case false:
                                        x.EmptyStateButton.Click += null!;
                                        x.EmptyStateButton.Click += BtnCreateEventsOnClick;
                                        break;
                                }
                                InterestedTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                                break;
                            }
                        }

                        break;
                    }
                    case "Past":
                    {
                        PastTab.MainScrollEvent.IsLoading = false;
                        PastTab.SwipeRefreshLayout.Refreshing = false;

                        switch (PastTab.MAdapter.EventList.Count)
                        {
                            case > 0:
                                PastTab.MRecycler.Visibility = ViewStates.Visible;
                                PastTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                                break;
                            default:
                            {
                                PastTab.MRecycler.Visibility = ViewStates.Gone;

                                PastTab.Inflated = PastTab.Inflated switch
                                {
                                    null => PastTab.EmptyStateLayout.Inflate(),
                                    _ => PastTab.Inflated
                                };

                                EmptyStateInflater x = new EmptyStateInflater();
                                x.InflateLayout(PastTab.Inflated, EmptyStateInflater.Type.NoEvent);
                                switch (x.EmptyStateButton.HasOnClickListeners)
                                {
                                    case false:
                                        x.EmptyStateButton.Click += null!;
                                        x.EmptyStateButton.Click += BtnCreateEventsOnClick;
                                        break;
                                }
                                PastTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                                break;
                            }
                        }

                        break;
                    }
                    case "MyEvent":
                    {
                        MyEventTab.MainScrollEvent.IsLoading = false;
                        MyEventTab.SwipeRefreshLayout.Refreshing = false;

                        switch (MyEventTab.MAdapter.EventList.Count)
                        {
                            case > 0:
                                MyEventTab.MRecycler.Visibility = ViewStates.Visible;
                                MyEventTab.EmptyStateLayout.Visibility = ViewStates.Gone;
                                break;
                            default:
                            {
                                MyEventTab.MRecycler.Visibility = ViewStates.Gone;

                                MyEventTab.Inflated = MyEventTab.Inflated switch
                                {
                                    null => MyEventTab.EmptyStateLayout.Inflate(),
                                    _ => MyEventTab.Inflated
                                };

                                EmptyStateInflater x = new EmptyStateInflater();
                                x.InflateLayout(MyEventTab.Inflated, EmptyStateInflater.Type.NoEvent);
                                switch (x.EmptyStateButton.HasOnClickListeners)
                                {
                                    case false:
                                        x.EmptyStateButton.Click += null!;
                                        x.EmptyStateButton.Click += BtnCreateEventsOnClick;
                                        break;
                                }
                                MyEventTab.EmptyStateLayout.Visibility = ViewStates.Visible;
                                break;
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

        //No Internet Connection 
        private void EmptyStateButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                StartApiService();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion
    }
}
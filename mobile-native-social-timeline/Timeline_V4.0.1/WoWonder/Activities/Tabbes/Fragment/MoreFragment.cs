using System;
using System.Linq;
using AFollestad.MaterialDialogs;
using Android.Content;
using Android.Gms.Ads;
using Android.OS; 
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using WoWonder.Activities.Album;
using WoWonder.Activities.Articles;
using WoWonder.Activities.CommonThings;
using WoWonder.Activities.Communities.Groups;
using WoWonder.Activities.Communities.Pages;
using WoWonder.Activities.Contacts;
using WoWonder.Activities.Events;
using WoWonder.Activities.Fundings;
using WoWonder.Activities.Games;
using WoWonder.Activities.Jobs;
using WoWonder.Activities.Market;
using WoWonder.Activities.Memories;
using WoWonder.Activities.Movies;
using WoWonder.Activities.MyPhoto;
using WoWonder.Activities.MyProfile;
using WoWonder.Activities.MyVideo;
using WoWonder.Activities.NativePost.Pages;
using WoWonder.Activities.NearBy;
using WoWonder.Activities.Offers;
using WoWonder.Activities.Pokes;
using WoWonder.Activities.PopularPosts;
using WoWonder.Activities.SettingsPreferences.General;
using WoWonder.Activities.SettingsPreferences.InvitationLinks;
using WoWonder.Activities.SettingsPreferences.MyInformation;
using WoWonder.Activities.SettingsPreferences.Notification;
using WoWonder.Activities.SettingsPreferences.Privacy;
using WoWonder.Activities.SettingsPreferences.Support;
using WoWonder.Activities.SettingsPreferences.TellFriend;
using WoWonder.Activities.Tabbes.Adapters;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using Exception = System.Exception;

namespace WoWonder.Activities.Tabbes.Fragment
{
    public class MoreFragment : AndroidX.Fragment.App.Fragment,  MaterialDialog.ISingleButtonCallback
    {
        #region  Variables Basic
         
        public MoreSectionAdapter MoreSectionAdapter;
        private RecyclerView MoreRecylerView;
        public AdView MAdView;

        #endregion

        #region General

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            { 
                View view = inflater.Inflate(Resource.Layout.Tab_More_Layout, container, false); 
                return view;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            try
            {
                base.OnViewCreated(view, savedInstanceState);
                SetRecyclerViewAdapters(view);

                AddOrRemoveEvent(true);

                switch (AppSettings.SetTabOnButton)
                {
                    case false:
                    {
                        var parasms = (LinearLayout.LayoutParams)MoreRecylerView.LayoutParameters;
                        // Check if we're running on Android 5.0 or higher
                        parasms.TopMargin = (int)Build.VERSION.SdkInt < 23 ? 130 : 270;

                        MoreRecylerView.LayoutParameters = parasms;
                        MoreRecylerView.SetPadding(0, 0, 0, 0);
                        break;
                    }
                }

            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public override void OnLowMemory()
        {
            try
            {
                GC.Collect(GC.MaxGeneration);
                base.OnLowMemory();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion
          
        #region Functions

        private void SetRecyclerViewAdapters(View view)
        {
            try
            { 
                MoreRecylerView = (RecyclerView)view.FindViewById(Resource.Id.Recyler);
                MoreRecylerView.NestedScrollingEnabled = true;

                MoreSectionAdapter = new MoreSectionAdapter(Activity);

                switch (AppSettings.MoreTheme)
                {
                    case MoreTheme.BeautyTheme:
                    {
                        var layoutManager = new GridLayoutManager(Activity, 4);
                     
                        var countListFirstRow = MoreSectionAdapter.SectionList.Where(q => q.StyleRow == 0).ToList().Count;

                        layoutManager.SetSpanSizeLookup(new MySpanSizeLookup2(countListFirstRow, 1, 4));//20, 1, 4
                        MoreRecylerView.SetLayoutManager(layoutManager);
                        MoreRecylerView.SetAdapter(MoreSectionAdapter);
                        break;
                    }
                    default:
                        MoreRecylerView.SetLayoutManager(new LinearLayoutManager(Activity)); 
                        MoreRecylerView.SetAdapter(MoreSectionAdapter);
                        break;
                }
                //MoreRecylerView.HasFixedSize = true;
                MoreRecylerView.SetItemViewCacheSize(50);
                MoreRecylerView.GetLayoutManager().ItemPrefetchEnabled = true;

                MAdView = view.FindViewById<AdView>(Resource.Id.adView);
                AdsGoogle.InitAdView(MAdView, MoreRecylerView);
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
                        MoreSectionAdapter.ItemClick += MoreSection_OnItemClick;
                        break;
                    default:
                        MoreSectionAdapter.ItemClick -= MoreSection_OnItemClick;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Events

        //Event Open Intent Activity
        private void MoreSection_OnItemClick(object sender, MoreSectionAdapterClickEventArgs adapterClickEvents)
        {
            try
            {
                var position = adapterClickEvents.Position;
                switch (position)
                {
                    case >= 0:
                    {
                        var item = MoreSectionAdapter?.GetItem(position); 
                        if (item != null)
                        {
                            switch (item.Id)
                            {
                                // My Profile
                                case 1:
                                {
                                    var intent = new Intent(Context, typeof(MyProfileActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // Messages
                                case 2: 
                                    Methods.App.OpenAppByPackageName(Context, AppSettings.MessengerPackageName, "OpenChatApp");
                                    break;
                                // Contacts
                                case 3:
                                {
                                    var intent = new Intent(Context, typeof(MyContactsActivity));
                                    intent.PutExtra("ContactsType", "Following");
                                    intent.PutExtra("UserId", UserDetails.UserId);
                                    StartActivity(intent);
                                    break;
                                }
                                // Pokes
                                case 4:
                                {
                                    var intent = new Intent(Context, typeof(PokesActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // Album
                                case 5:
                                {
                                    var intent = new Intent(Context, typeof(MyAlbumActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // MyImages
                                case 6:
                                {
                                    var intent = new Intent(Context, typeof(MyPhotosActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // MyVideos
                                case 7:
                                {
                                    var intent = new Intent(Context, typeof(MyVideoActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // Saved Posts
                                case 8:
                                {
                                    var intent = new Intent(Context, typeof(SavedPostsActivity)); 
                                    StartActivity(intent);
                                    break;
                                }
                                // Groups
                                case 9:
                                {
                                    var intent = new Intent(Context, typeof(GroupsActivity)); 
                                    StartActivity(intent);
                                    break;
                                }
                                // Pages
                                case 10:
                                {
                                    var intent = new Intent(Context, typeof(PagesActivity)); 
                                    StartActivity(intent);
                                    break;
                                }
                                // Blogs
                                case 11:
                                    StartActivity(new Intent(Context, typeof(ArticlesActivity)));
                                    break;
                                // Market
                                case 12:
                                    StartActivity(new Intent(Context, typeof(TabbedMarketActivity)));
                                    break;
                                // Popular Posts
                                case 13:
                                {
                                    var intent = new Intent(Context, typeof(PopularPostsActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // Events
                                case 14:
                                {
                                    var intent = new Intent(Context, typeof(EventMainActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // Find Friends
                                case 15:
                                {
                                    var intent = new Intent(Context, typeof(PeopleNearByActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // Movies
                                case 16:
                                {
                                    var intent = new Intent(Context, typeof(MoviesActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // jobs
                                case 17:
                                {
                                    var intent = new Intent(Context, typeof(JobsActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // common things
                                case 18:
                                {
                                    var intent = new Intent(Context, typeof(CommonThingsActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // Fundings
                                case 19:
                                {
                                    var intent = new Intent(Context, typeof(FundingActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // Games
                                case 20:
                                {
                                    var intent = new Intent(Context, typeof(GamesActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // Help & Support
                                case 80:
                                {
                                    var intent = new Intent(Context, typeof(MemoriesActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // Help & Support
                                //Settings Page
                                case 82:
                                {
                                    var intent = new Intent(Context, typeof(OffersActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // General Account
                                case 21:
                                {
                                    var intent = new Intent(Context, typeof(GeneralAccountActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // Privacy
                                case 22:
                                {
                                    var intent = new Intent(Context, typeof(PrivacyActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // Notification
                                case 23:
                                {
                                    var intent = new Intent(Context, typeof(MessegeNotificationActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // InvitationLinks
                                case 24:
                                {
                                    var intent = new Intent(Context, typeof(InvitationLinksActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // MyInformation
                                case 25:
                                {
                                    var intent = new Intent(Context, typeof(MyInformationActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // Tell Friends
                                case 26:
                                {
                                    var intent = new Intent(Context, typeof(TellFriendActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // Help & Support
                                case 27:
                                {
                                    var intent = new Intent(Context, typeof(SupportActivity));
                                    StartActivity(intent);
                                    break;
                                }
                                // Logout
                                case 28:
                                {
                                    var dialog = new MaterialDialog.Builder(Context).Theme(AppSettings.SetTabDarkTheme ? Theme.Dark : Theme.Light);

                                    dialog.Title(Resource.String.Lbl_Warning).TitleColorRes(Resource.Color.primary);
                                    dialog.Content(Context.GetText(Resource.String.Lbl_Are_you_logout));
                                    dialog.PositiveText(Context.GetText(Resource.String.Lbl_Ok)).OnPositive(this);
                                    dialog.NegativeText(Context.GetText(Resource.String.Lbl_Cancel)).OnNegative(this);
                                    dialog.AlwaysCallSingleChoiceCallback();
                                    dialog.Build().Show();
                                    break;
                                } 
                            }
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
         
        #region MaterialDialog
         
        public void OnClick(MaterialDialog p0, DialogAction p1)
        {
            try
            {
                if (p1 == DialogAction.Positive)
                {
                    Toast.MakeText(Activity, Activity.GetText(Resource.String.Lbl_You_will_be_logged), ToastLength.Long)?.Show();
                    ApiRequest.Logout(Activity);
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
        
    }
}
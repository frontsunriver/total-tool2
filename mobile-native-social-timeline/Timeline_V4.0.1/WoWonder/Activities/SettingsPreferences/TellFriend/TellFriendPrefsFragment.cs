using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

using Android.Views;
using AndroidX.Preference;
using WoWonder.Library.Anjo.Share;
using WoWonder.Library.Anjo.Share.Abstractions;
using WoWonder.Activities.SettingsPreferences.InviteFriends;
using WoWonder.Activities.Wallet;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;

 

namespace WoWonder.Activities.SettingsPreferences.TellFriend
{
    public class TellFriendPrefsFragment : PreferenceFragmentCompat, ISharedPreferencesOnSharedPreferenceChangeListener
    {
        #region  Variables Basic

        private Preference SharePref, MyAffiliatesPref, InviteFriendsPref, MyPointsPref, MyBalancePref, WithdrawalsPref;
        private string InviteSmsText = ""; 
        private readonly Activity ActivityContext;

        #endregion

        #region General

        public TellFriendPrefsFragment(Activity context)
        {
            try
            {
                ActivityContext = context;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                // create ContextThemeWrapper from the original Activity Context with the custom theme
                Context contextThemeWrapper = AppSettings.SetTabDarkTheme ? new ContextThemeWrapper(ActivityContext, Resource.Style.SettingsThemeDark) : new ContextThemeWrapper(ActivityContext, Resource.Style.SettingsTheme);

                // clone the inflater using the ContextThemeWrapper
                LayoutInflater localInflater = inflater.CloneInContext(contextThemeWrapper);

                View view = base.OnCreateView(localInflater, container, savedInstanceState);

                return view;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }

        public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
        {
            try
            {
                // Load the preferences from an XML resource
                AddPreferencesFromResource(Resource.Xml.SettingsPrefs_TellFriend);

                MainSettings.SharedData = PreferenceManager.SharedPreferences;
                InitComponent();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override void OnResume()
        {
            try
            {
                base.OnResume();
                PreferenceManager.SharedPreferences.RegisterOnSharedPreferenceChangeListener(this);
                AddOrRemoveEvent(true);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override void OnPause()
        {
            try
            {
                base.OnPause();
                PreferenceScreen.SharedPreferences.UnregisterOnSharedPreferenceChangeListener(this);
                AddOrRemoveEvent(false);
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
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        #endregion

        #region Functions

        private void InitComponent()
        {
            try
            {
                MainSettings.SharedData = PreferenceManager.SharedPreferences;
                PreferenceManager.SharedPreferences.RegisterOnSharedPreferenceChangeListener(this);

                SharePref = FindPreference("Share_key");
                MyAffiliatesPref = FindPreference("MyAffiliates_key");
                InviteFriendsPref = FindPreference("InviteFriends_key");
                MyPointsPref = FindPreference("MyPoints_key");
                MyBalancePref = FindPreference("MyBalance_key");
                WithdrawalsPref = FindPreference("Withdrawals_key");

                //Delete Preference
                var mCategoryEarnings = (PreferenceCategory)FindPreference("SectionEarnings_key");
                 
                switch (AppSettings.ShowSettingsMyAffiliates)
                {
                    case false:
                        mCategoryEarnings.RemovePreference(MyAffiliatesPref);
                        break;
                }
                 
                switch (AppSettings.ShowUserPoint)
                {
                    case false:
                        mCategoryEarnings.RemovePreference(MyPointsPref);
                        break;
                }
                 
                switch (AppSettings.ShowWallet)
                {
                    case false:
                        mCategoryEarnings.RemovePreference(MyBalancePref);
                        break;
                }

                switch (AppSettings.ShowWithdrawals)
                {
                    case false:
                        mCategoryEarnings.RemovePreference(WithdrawalsPref);
                        break;
                }


                var mCategoryInvite = (PreferenceCategory)FindPreference("SectionInvite_key"); 
                switch (AppSettings.ShowSettingsShare)
                {
                    case false:
                        mCategoryInvite.RemovePreference(SharePref);
                        break;
                }

                switch (AppSettings.InvitationSystem)
                {
                    case false:
                        mCategoryInvite.RemovePreference(InviteFriendsPref);
                        break;
                }

                InviteSmsText = GetText(Resource.String.Lbl_InviteSMSText_1) + " " + AppSettings.ApplicationName + " " +
                                GetText(Resource.String.Lbl_InviteSMSText_2); 
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
                        SharePref.PreferenceClick += SharePref_OnPreferenceClick;
                        MyAffiliatesPref.PreferenceClick += MyAffiliatesPref_OnPreferenceClick;
                        InviteFriendsPref.PreferenceClick += InviteFriendsPrefOnPreferenceClick;
                        MyPointsPref.PreferenceClick += MyPointsPrefOnPreferenceClick;
                        MyBalancePref.PreferenceClick += MyBalancePrefOnPreferenceClick;
                        WithdrawalsPref.PreferenceClick += WithdrawalsPrefOnPreferenceClick;
                        break;
                    default:
                        SharePref.PreferenceClick -= SharePref_OnPreferenceClick;
                        MyAffiliatesPref.PreferenceClick -= MyAffiliatesPref_OnPreferenceClick;
                        InviteFriendsPref.PreferenceClick -= InviteFriendsPrefOnPreferenceClick;
                        MyPointsPref.PreferenceClick -= MyPointsPrefOnPreferenceClick;
                        MyBalancePref.PreferenceClick -= MyBalancePrefOnPreferenceClick;
                        WithdrawalsPref.PreferenceClick -= WithdrawalsPrefOnPreferenceClick;
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

        //Withdrawals
        private void WithdrawalsPrefOnPreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            try
            {
                ActivityContext.StartActivity(new Intent(ActivityContext, typeof(WithdrawalsActivity)));
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }


        //MyBalance
        private void MyBalancePrefOnPreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(TabbedWalletActivity));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //MyPoints
        private void MyPointsPrefOnPreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(MyPointsActivity));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }


        //Share App with your friends using Url This App in Market Google play 
        private async void SharePref_OnPreferenceClick(object sender, Preference.PreferenceClickEventArgs preferenceClickEventArgs)
        {
            try
            {
                switch (CrossShare.IsSupported)
                {
                    //Share Plugin same as flame
                    case false:
                        return;
                    default:
                        await CrossShare.Current.Share(new ShareMessage
                        {
                            Title = AppSettings.ApplicationName,
                            Text = InviteSmsText,
                            Url = "http://play.google.com/store/apps/details?id=" + ActivityContext.PackageName
                        });
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        //My Affiliates
        private void MyAffiliatesPref_OnPreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(MyAffiliatesActivity));
                ActivityContext.StartActivity(intent);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void InviteFriendsPrefOnPreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            try
            {
                switch ((int)Build.VERSION.SdkInt)
                {
                    case < 23:
                    {
                        var intent = new Intent(ActivityContext, typeof(InviteFriendsActivity));
                        ActivityContext.StartActivity(intent);
                        break;
                    }
                    default:
                    {
                        //Check to see if any permission in our group is available, if one, then all are
                        if (ActivityContext.CheckSelfPermission(Manifest.Permission.ReadContacts) == Permission.Granted)
                        {
                            var intent = new Intent(ActivityContext, typeof(InviteFriendsActivity));
                            ActivityContext.StartActivity(intent);
                        }
                        else
                        {
                            new PermissionsController(ActivityContext).RequestPermission(101);
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
         
        //On Change 
        public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
            try
            {

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }
}
   
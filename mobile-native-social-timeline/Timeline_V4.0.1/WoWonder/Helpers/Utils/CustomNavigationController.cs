using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;  
using Android.Views;
using AndroidX.Core.Content;
using MeoNavLib.Com;
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;

namespace WoWonder.Helpers.Utils
{
    public class CustomNavigationController : Java.Lang.Object , MeowBottomNavigation.IClickListener, MeowBottomNavigation.IReselectListener
    {
        private readonly Activity MainContext;
        public int PageNumber;
        private static int OpenNewsFeedTab = 1;

        private readonly TabbedMainActivity Context;
        private readonly MeowBottomNavigation NavigationTabBar;
        private List<MeowBottomNavigation.Model> Models;

        public CustomNavigationController(Activity activity , MeowBottomNavigation bottomNavigation)
        {
            try
            {
                MainContext = activity;
                NavigationTabBar = bottomNavigation;

                Context = activity switch
                {
                    TabbedMainActivity cont => cont,
                    _ => Context
                };

                Initialize();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void Initialize()
        {
            try
            {
                Models = new List<MeowBottomNavigation.Model>
                {
                    new MeowBottomNavigation.Model(0, ContextCompat.GetDrawable(MainContext, Resource.Drawable.icon_home_vector)),
                    new MeowBottomNavigation.Model(1, ContextCompat.GetDrawable(MainContext, Resource.Drawable.icon_notification_vector)),
                };

                switch (AppSettings.ShowTrendingPage)
                {
                    case true:
                        Models.Add(new MeowBottomNavigation.Model(2, ContextCompat.GetDrawable(MainContext, Resource.Drawable.icon_fire_vector)));
                        break;
                }

                Models.Add(new MeowBottomNavigation.Model(3, ContextCompat.GetDrawable(MainContext, Resource.Drawable.ic_menu)));
                 
                NavigationTabBar.AddModel(Models);

                NavigationTabBar.SetDefaultIconColor(Color.ParseColor("#bfbfbf"));
                NavigationTabBar.SetSelectedIconColor(Color.ParseColor(AppSettings.MainColor));

                NavigationTabBar.SetBackgroundBottomColor(AppSettings.SetTabDarkTheme ? Color.Black : Color.White);
                NavigationTabBar.SetCircleColor(AppSettings.SetTabDarkTheme ? Color.Black : Color.White);

                NavigationTabBar.SetCountTextColor(Color.White);
                NavigationTabBar.SetCountBackgroundColor(Color.ParseColor(AppSettings.MainColor));

                NavigationTabBar.SetOnClickMenuListener(this);
                NavigationTabBar.SetOnReselectListener(this); 
            } 
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnClickItem(MeowBottomNavigation.Model item)
        {
            try
            {
                if (!item.GetCount().Equals("0") || !item.GetCount().Equals("empty"))
                {
                    NavigationTabBar.SetCount(item.GetId(), "empty"); 
                }

                PageNumber = item.GetId();
                
                switch (PageNumber)
                {
                    case >= 0:
                        switch (PageNumber)
                        {
                            // News_Feed_Tab
                            case 0:
                            {
                                Context.FloatingActionButton.Visibility = AppSettings.ShowAddPostOnNewsFeed switch
                                {
                                    true when Context.FloatingActionButton.Visibility == ViewStates.Invisible =>
                                        ViewStates.Visible,
                                    _ => Context.FloatingActionButton.Visibility
                                };

                                AdsGoogle.Ad_AppOpenManager(MainContext);
                                break;
                            }
                            // Notifications_Tab
                            case 1:
                            {
                                Context.FloatingActionButton.Visibility = Context.FloatingActionButton.Visibility switch
                                {
                                    ViewStates.Visible => ViewStates.Gone,
                                    _ => Context.FloatingActionButton.Visibility
                                };

                                AdsGoogle.Ad_RewardedVideo(MainContext);
                                break;
                            }
                            // Trending_Tab
                            case 2 when AppSettings.ShowTrendingPage:
                            {
                                Context.FloatingActionButton.Visibility = Context.FloatingActionButton.Visibility switch
                                {
                                    ViewStates.Visible => ViewStates.Gone,
                                    _ => Context.FloatingActionButton.Visibility
                                };

                                AdsGoogle.Ad_Interstitial(MainContext);

                                switch (AppSettings.ShowLastActivities)
                                {
                                    case true:
                                        Task.Factory.StartNew(() => { Context.TrendingTab.StartApiService(); });
                                        break;
                                }

                                Context.InAppReview();
                                break;
                            }
                            // More_Tab
                            case 3:
                            {
                                Context.FloatingActionButton.Visibility = Context.FloatingActionButton.Visibility switch
                                {
                                    ViewStates.Visible => ViewStates.Gone,
                                    _ => Context.FloatingActionButton.Visibility
                                };

                                AdsGoogle.Ad_RewardedInterstitial(MainContext);
                                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => ApiRequest.Get_MyProfileData_Api(MainContext) });
                                break;
                            }
                        }

                        break;
                }

                if (Context.ViewPager.CurrentItem != PageNumber)
                    Context.ViewPager.SetCurrentItem(PageNumber, false);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnReselectItem(MeowBottomNavigation.Model item)
        {
            try
            {
                var p = item.GetId();

                switch (p)
                {
                    case < 0:
                        return;
                    // News_Feed_Tab
                    case 0 when OpenNewsFeedTab == 2:
                        OpenNewsFeedTab = 1;
                        Context.NewsFeedTab.MainRecyclerView.ScrollToPosition(0);
                        break;
                    case 0:
                        OpenNewsFeedTab++;
                        break;
                    // Notifications_Tab
                    case 1:
                        Context.NewsFeedTab?.MainRecyclerView?.StopVideo();
                        OpenNewsFeedTab = 1;
                        break;
                    // Trending_Tab
                    case 2 when AppSettings.ShowTrendingPage:
                        Context.NewsFeedTab?.MainRecyclerView?.StopVideo();
                        OpenNewsFeedTab = 1;
                        break;
                    // More_Tab
                    case 3:
                        Context.NewsFeedTab?.MainRecyclerView?.StopVideo();
                        OpenNewsFeedTab = 1;
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void ShowBadge(int id , string count, bool showBadge)
        {
            try
            {
                switch (id)
                {
                    case < 0:
                        return;
                }

                switch (showBadge)
                {
                    case true:
                        switch (id)
                        {
                            // News_Feed_Tab
                            case 0:
                                NavigationTabBar.SetCount(0, count);
                                break;
                            // Notifications_Tab
                            case 1:
                                NavigationTabBar.SetCount(1, count);
                                break;
                            // Trending_Tab
                            case 2:
                                NavigationTabBar.SetCount(2, count);
                                break;
                            // More_Tab
                            case 3:
                                NavigationTabBar.SetCount(3, count);
                                break;
                        }

                        break;
                    default:
                        switch (id)
                        {
                            // News_Feed_Tab
                            case 0:
                                NavigationTabBar.SetCount(0, "empty");
                                break;
                            // Notifications_Tab
                            case 1:
                                NavigationTabBar.SetCount(1, "empty");
                                break;
                            // Trending_Tab
                            case 2:
                                NavigationTabBar.SetCount(2, "empty");
                                break;
                            // More_Tab
                            case 3:
                                NavigationTabBar.SetCount(3, "empty");
                                break;
                        }

                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        } 
    }
}
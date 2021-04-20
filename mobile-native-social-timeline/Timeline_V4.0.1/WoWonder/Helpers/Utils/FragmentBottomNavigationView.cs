using System;
using System.Collections.Generic;
using Android.App;
using WoWonder.Activities.Wallet;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;

namespace WoWonder.Helpers.Utils
{
    public class FragmentBottomNavigationView
    {
        private readonly TabbedWalletActivity Context; 
        public readonly List<Fragment> FragmentListTab0 = new List<Fragment>();
        public readonly List<Fragment> FragmentListTab1 = new List<Fragment>();
        private int PageNumber;

        public FragmentBottomNavigationView(Activity context)
        {
            Context = (TabbedWalletActivity)context;
        }
          
        public void NavigationTabBarOnStartTabSelected(int index)
        {
            try
            {
                switch (index)
                {
                    case 0:
                        PageNumber = 0;
                        ShowFragment0();
                        break;

                    case 1:
                        PageNumber = 1;
                        ShowFragment1();
                        break;

                    default:
                        PageNumber = 0;
                        ShowFragment0();
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public int GetCountFragment()
        {
            try
            {
                return PageNumber switch
                {
                    0 => FragmentListTab0.Count > 1 ? FragmentListTab0.Count : 0,
                    1 => FragmentListTab1.Count > 1 ? FragmentListTab1.Count : 0,
                    _ => FragmentListTab0.Count > 1 ? FragmentListTab0.Count : 0
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return 0;
            }
        }

        public static void HideFragmentFromList(List<Fragment> fragmentList, FragmentTransaction ft)
        {
            try
            {
                switch (fragmentList.Count)
                {
                    case > 0:
                    {
                        foreach (var fra in fragmentList)
                        {
                            switch (fra.IsAdded)
                            {
                                case true when fra.IsVisible:
                                    ft.Hide(fra);
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

        public Fragment GetSelectedTabBackStackFragment()
        {
            switch (PageNumber)
            {
                case 0:
                    {
                        var currentFragment = FragmentListTab0[FragmentListTab0.Count - 2];
                        if (currentFragment != null)
                            return currentFragment;
                        break;
                    }
                case 1:
                    {
                        var currentFragment = FragmentListTab1[FragmentListTab1.Count - 2];
                        if (currentFragment != null)
                            return currentFragment;
                        break;
                    }

                default:
                    return null!;

            }

            return null!;
        }

        public Fragment GetSelectedTabLastStackFragment()
        {
            switch (PageNumber)
            {
                case 0:
                    {
                        var currentFragment = FragmentListTab0[FragmentListTab0.Count - 1];
                        if (currentFragment != null)
                            return currentFragment;
                        break;
                    }
                case 1:
                    {
                        var currentFragment = FragmentListTab1[FragmentListTab1.Count - 1];
                        if (currentFragment != null)
                            return currentFragment;
                        break;
                    }

                default:
                    return null!;

            }

            return null!;
        }
         
        public void DisplayFragment(Fragment newFragment)
        {
            try
            {
                FragmentTransaction ft = Context.SupportFragmentManager.BeginTransaction();

                HideFragmentFromList(FragmentListTab0, ft);
                HideFragmentFromList(FragmentListTab1, ft);

                switch (PageNumber)
                {
                    case 0:
                    {
                        switch (FragmentListTab0.Contains(newFragment))
                        {
                            case false:
                                FragmentListTab0.Add(newFragment);
                                break;
                        }

                        break;
                    }
                    case 1:
                    {
                        switch (FragmentListTab1.Contains(newFragment))
                        {
                            case false:
                                FragmentListTab1.Add(newFragment);
                                break;
                        }

                        break;
                    }
                }

                switch (newFragment.IsAdded)
                {
                    case false:
                        ft.Add(Resource.Id.mainFragment, newFragment, PageNumber + newFragment.Id.ToString());
                        break;
                    default:
                        ft.Show(newFragment);
                        break;
                }

                ft.CommitNow();
                Context.SupportFragmentManager.ExecutePendingTransactions();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            } 
        }
         
        public void RemoveFragment(Fragment oldFragment)
        {
            FragmentTransaction ft = Context.SupportFragmentManager.BeginTransaction();

            switch (PageNumber)
            {
                case 0:
                {
                    if (FragmentListTab0.Contains(oldFragment))
                        FragmentListTab0.Remove(oldFragment);
                    break;
                }
                case 1:
                {
                    if (FragmentListTab1.Contains(oldFragment))
                        FragmentListTab1.Remove(oldFragment);
                    break;
                }
            }

            HideFragmentFromList(FragmentListTab0, ft);
            HideFragmentFromList(FragmentListTab1, ft);

            switch (oldFragment.IsAdded)
            {
                case true:
                    ft.Remove(oldFragment);
                    break;
            }

            switch (PageNumber)
            {
                case 0:
                {
                    var currentFragment = FragmentListTab0[FragmentListTab0.Count - 1];
                    ft.Show(currentFragment)?.Commit();
                    break;
                }
                case 1:
                {
                    var currentFragment = FragmentListTab1[FragmentListTab1.Count - 1];
                    ft.Show(currentFragment)?.Commit();
                    break;
                }
            }
        }

        public void OnBackStackClickFragment()
        {
            switch (PageNumber)
            {
                case 0 when FragmentListTab0.Count > 1:
                {
                    var currentFragment = FragmentListTab0[FragmentListTab0.Count - 1];
                    if (currentFragment != null)
                        RemoveFragment(currentFragment);
                    break;
                }
                case 0:
                    Context.Finish();
                    break;
                case 1 when FragmentListTab1.Count > 1:
                {
                    var currentFragment = FragmentListTab1[FragmentListTab1.Count - 1];
                    if (currentFragment != null)
                        RemoveFragment(currentFragment);
                    break;
                }
                case 1:
                    Context.Finish();
                    break;
            }
        }

        public void ShowFragment0()
        {
            try
            {
                switch (FragmentListTab0.Count)
                {
                    case < 0:
                        return;
                    // If user presses it while still on that tab it removes all fragments from the list
                    case > 1:
                    {
                        FragmentTransaction ft = Context.SupportFragmentManager.BeginTransaction();

                        for (var index = FragmentListTab0.Count - 1; index > 0; index--)
                        {
                            var oldFragment = FragmentListTab0[index];
                            if (FragmentListTab0.Contains(oldFragment))
                                FragmentListTab0.Remove(oldFragment);

                            switch (oldFragment.IsAdded)
                            {
                                case true:
                                    ft.Remove(oldFragment);
                                    break;
                            }

                            HideFragmentFromList(FragmentListTab0, ft);
                        }

                        var currentFragment = FragmentListTab0[FragmentListTab0.Count - 1];
                        ft.Show(currentFragment)?.Commit();
                        break;
                    }
                    default:
                    {
                        var currentFragment = FragmentListTab0[FragmentListTab0.Count - 1];
                        if (currentFragment != null)
                            DisplayFragment(currentFragment);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void ShowFragment1()
        {
            switch (FragmentListTab1.Count)
            {
                case < 0:
                    return;
            }
            var currentFragment = FragmentListTab1[FragmentListTab1.Count - 1];
            if (currentFragment != null)
                DisplayFragment(currentFragment);
        }

    }
}
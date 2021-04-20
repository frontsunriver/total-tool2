using System;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using AndroidX.ViewPager2.Widget;

namespace WoWonder.Helpers.Utils
{
    public class RecyclerViewOnItemTouch : Java.Lang.Object, RecyclerView.IOnItemTouchListener
    {
        private readonly RecyclerView RecyclerView;
        private readonly ViewPager2 ViewPager;
        int lastX;

        public RecyclerViewOnItemTouch(RecyclerView recyclerView, ViewPager2 viewPager)
        {
            try
            {
                RecyclerView = recyclerView;
                ViewPager = viewPager;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }


        public bool OnInterceptTouchEvent(RecyclerView recyclerView, MotionEvent @event)
        {
            try
            {
                switch (@event.Action)
                {
                    case MotionEventActions.Down:
                        lastX = (int)@event.GetX();
                        break;
                    case MotionEventActions.Move:
                        bool isScrollingRight = @event.GetX() < lastX;
                        if (isScrollingRight && ((LinearLayoutManager)RecyclerView.GetLayoutManager()).FindLastCompletelyVisibleItemPosition() == RecyclerView.GetAdapter().ItemCount - 1 ||
                            !isScrollingRight && ((LinearLayoutManager)RecyclerView.GetLayoutManager()).FindFirstCompletelyVisibleItemPosition() == 0)
                        {
                            ViewPager.UserInputEnabled = true;
                        }
                        else
                        {
                            ViewPager.UserInputEnabled = false;
                        }
                        break;
                    case MotionEventActions.Up:
                        lastX = 0;
                        ViewPager.UserInputEnabled = true;
                        break;
                }
                return false;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return false;
            }
        }

        public void OnRequestDisallowInterceptTouchEvent(bool disallow)
        {
            
        }

        public void OnTouchEvent(RecyclerView recyclerView, MotionEvent @event)
        {
            
        }
    }
}
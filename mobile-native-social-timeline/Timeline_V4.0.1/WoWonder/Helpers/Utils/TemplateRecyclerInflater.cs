using System;
using Android.App;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using WoWonder.Library.Anjo.IntegrationRecyclerView;
using Bumptech.Glide.Util;

namespace WoWonder.Helpers.Utils
{
    public class TemplateRecyclerInflater
    {
        public LinearLayout MainLinear;
        public TextView TitleText, IconTitle, DescriptionText, MoreText;
        public RecyclerView Recyler;
        public dynamic LayoutManager;

        public enum TypeLayoutManager
        {
            LinearLayoutManagerVertical,
            LinearLayoutManagerHorizontal,
            GridLayoutManagerVertical,
            GridLayoutManagerHorizontal,
            StaggeredGridLayoutManagerVertical,
            StaggeredGridLayoutManagerHorizontal
        }

        private void InitComponent(View inflated)
        {
            try
            {
                MainLinear = (LinearLayout)inflated.FindViewById(Resource.Id.mainLinear);
                TitleText = (TextView)inflated.FindViewById(Resource.Id.textTitle);
                IconTitle = (TextView)inflated.FindViewById(Resource.Id.iconTitle);
                DescriptionText = (TextView)inflated.FindViewById(Resource.Id.textSecondery);
                MoreText = (TextView)inflated.FindViewById(Resource.Id.textMore);
                Recyler = (RecyclerView)inflated.FindViewById(Resource.Id.recyler);
                IconTitle.Visibility = ViewStates.Gone;

                //FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconTitle, FontAwesomeIcon.AngleLeft);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void InflateLayout<T>(Activity activity, View inflated, dynamic mAdapter, TypeLayoutManager manager, int spanCount = 0, bool showTitle = true, string titleText = "", string descriptionText = "", bool showMore = false) where T : class
        {
            try
            {
                InitComponent(inflated);

                switch (showTitle)
                {
                    case true:
                    {
                        MainLinear.Visibility = ViewStates.Visible;

                        if (string.IsNullOrEmpty(titleText))
                        {
                            TitleText.Visibility = ViewStates.Gone;
                            IconTitle.Visibility = ViewStates.Gone;
                        }
                        else
                        {
                            TitleText.Text = titleText; 
                        }

                        if (string.IsNullOrEmpty(descriptionText))
                            DescriptionText.Visibility = ViewStates.Gone;
                        else
                            DescriptionText.Text = descriptionText;

                        MoreText.Visibility = showMore switch
                        {
                            true => ViewStates.Visible,
                            _ => MoreText.Visibility
                        };
                        break;
                    }
                    default:
                        MainLinear.Visibility = ViewStates.Gone;
                        break;
                }

                switch (manager)
                {
                    case TypeLayoutManager.LinearLayoutManagerHorizontal:
                        LayoutManager = new LinearLayoutManager(activity, LinearLayoutManager.Horizontal, false);
                        Recyler.NestedScrollingEnabled = false;
                        break;
                    case TypeLayoutManager.LinearLayoutManagerVertical:
                        LayoutManager = new LinearLayoutManager(activity);
                        break;
                    case TypeLayoutManager.GridLayoutManagerVertical:
                        LayoutManager = new GridLayoutManager(activity, spanCount);
                        break;
                    case TypeLayoutManager.GridLayoutManagerHorizontal:
                        LayoutManager = new GridLayoutManager(activity, spanCount, LinearLayoutManager.Horizontal, false);
                        Recyler.NestedScrollingEnabled = false;
                        break;
                    case TypeLayoutManager.StaggeredGridLayoutManagerVertical:
                        LayoutManager = new StaggeredGridLayoutManager(spanCount, LinearLayoutManager.Vertical);
                        break;
                    case TypeLayoutManager.StaggeredGridLayoutManagerHorizontal:
                        LayoutManager = new StaggeredGridLayoutManager(spanCount, LinearLayoutManager.Horizontal);
                        Recyler.NestedScrollingEnabled = false;
                        break;
                    default:
                        LayoutManager = new LinearLayoutManager(activity);
                        break;
                }

                Recyler.SetLayoutManager(LayoutManager);
                Recyler.SetItemViewCacheSize(20);
                Recyler.HasFixedSize = true;
                Recyler.SetItemViewCacheSize(10);
                Recyler.GetLayoutManager().ItemPrefetchEnabled = true;

                var sizeProvider = new FixedPreloadSizeProvider(10, 10);
                var preLoader = new RecyclerViewPreloader<T>(activity, mAdapter, sizeProvider, 10);
                Recyler.AddOnScrollListener(preLoader);

                Recyler.SetAdapter(mAdapter);

                if (Recyler.Visibility != ViewStates.Visible)
                    Recyler.Visibility = ViewStates.Visible;

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }
}
using System;
using AndroidX.RecyclerView.Widget;


namespace WoWonder.Helpers.Utils
{
    public class RecyclerViewOnScrollListener : RecyclerView.OnScrollListener
    {
        public delegate void LoadMoreEventHandler(object sender, EventArgs e);

        public event LoadMoreEventHandler LoadMoreEvent;

        public bool IsLoading { get; set; }
        public dynamic LayoutManager;
        public int SpanCount;

        public RecyclerViewOnScrollListener(dynamic layoutManager, int spanCount = 0)
        {
            try
            {
                IsLoading = false;
                LayoutManager = layoutManager;
                SpanCount = spanCount;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            try
            {
                base.OnScrolled(recyclerView, dx, dy);

                var visibleItemCount = recyclerView.ChildCount;
                var totalItemCount = recyclerView.GetAdapter().ItemCount;

                int pastVisibleItems;
                switch (LayoutManager)
                {
                    case LinearLayoutManager managerLinear:
                        pastVisibleItems = managerLinear.FindFirstVisibleItemPosition();
                        break;
                    default:
                        switch (LayoutManager)
                        {
                            case GridLayoutManager managerGrid:
                                pastVisibleItems = managerGrid.FindFirstVisibleItemPosition();
                                break;
                            case StaggeredGridLayoutManager managerStaggeredGrid:
                            {
                                int[] firstVisibleItemPositions = new int[SpanCount];
                                pastVisibleItems = managerStaggeredGrid.FindFirstVisibleItemPositions(firstVisibleItemPositions)[0];
                                break;
                            }
                            default:
                                pastVisibleItems = LayoutManager?.FindFirstVisibleItemPosition() ?? 0;
                                break;
                        }

                        break;
                }

                if (visibleItemCount + pastVisibleItems + 4 < totalItemCount)
                    return;

                switch (IsLoading)
                {
                    //&& !recyclerView.CanScrollVertically(1)
                    case true:
                        return;
                    default:
                        //IsLoading = true;
                        LoadMoreEvent?.Invoke(this, null);

                        //if (visibleItemCount + pastVisibleItems + 8 >= totalItemCount)
                        //{
                        //    //Load More  from API Request
                        //    if (IsLoading == false)
                        //    {
                        //        LoadMoreEvent?.Invoke(this, null);
                        //        IsLoading = true;
                        //    }
                        //}
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
    }
}
using System;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using JetBrains.Annotations;

namespace WoWonder.Library.Anjo.IntegrationRecyclerView
{
    public class RecyclerToListViewScrollListener : RecyclerView.OnScrollListener
    {
        public static readonly int UNKNOWN_SCROLL_STATE = int.MinValue;
        private readonly AbsListView.IOnScrollListener scrollListener;
        private int lastFirstVisible = -1;
        private int lastVisibleCount = -1;
        private int lastItemCount = -1;

        public RecyclerToListViewScrollListener([NotNull] AbsListView.IOnScrollListener scrollListener)
        {
            this.scrollListener = scrollListener;
        }

        public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
        {
            base.OnScrollStateChanged(recyclerView, newState);

            ScrollState listViewState = newState switch
            {
                RecyclerView.ScrollStateDragging => ScrollState.TouchScroll,
                RecyclerView.ScrollStateIdle => ScrollState.Idle,
                RecyclerView.ScrollStateSettling => ScrollState.Fling,
                _ => ScrollState.TouchScroll
            };

            scrollListener.OnScrollStateChanged(null /*view*/, listViewState);

        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            LinearLayoutManager layoutManager = (LinearLayoutManager)recyclerView.GetLayoutManager();

            int firstVisible = layoutManager.FindFirstVisibleItemPosition();
            int visibleCount = Math.Abs(firstVisible - layoutManager.FindLastVisibleItemPosition());
            int itemCount = recyclerView.GetAdapter().ItemCount;

            if (firstVisible != lastFirstVisible
                || visibleCount != lastVisibleCount
                || itemCount != lastItemCount)
            {
                scrollListener.OnScroll(null, firstVisible, visibleCount, itemCount);
                lastFirstVisible = firstVisible;
                lastVisibleCount = visibleCount;
                lastItemCount = itemCount;
            }
        }

    }

}
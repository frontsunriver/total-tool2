using System;
using System.Collections.ObjectModel;
using Android.App;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;

namespace WoWonder.Activities.Tabbes.Adapters
{
    public class HashtagUserAdapter : RecyclerView.Adapter 
    {
        private readonly Activity ActivityContext;

        public ObservableCollection<TrendingHashtag> MHashtagList = new ObservableCollection<TrendingHashtag>();

        public HashtagUserAdapter(Activity context)
        {
            try
            {
                HasStableIds = true;
                ActivityContext = context;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => MHashtagList?.Count ?? 0;
 
        public event EventHandler<HashtagUserAdapterClickEventArgs> ItemClick;
        public event EventHandler<HashtagUserAdapterClickEventArgs> ItemLongClick;


        // Create new views (invoked by the layout manager) 
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_HLastSearch_View
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_HLastSearch_View, parent, false);
                var vh = new HashtagUserAdapterViewHolder(itemView, Click, LongClick);
                return vh;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            try
            {
                switch (viewHolder)
                {
                    case HashtagUserAdapterViewHolder holder:
                    {
                        var item = MHashtagList[position];
                        if (item != null)
                        {
                            holder.Button.Text = "#" + item.Tag;
                        }

                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                Console.WriteLine(ActivityContext);
            }
        }

        public TrendingHashtag GetItem(int position)
        {
            return MHashtagList[position];
        }

        public override long GetItemId(int position)
        {
            try
            {
                return position;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return 0;
            }
        }

        public override int GetItemViewType(int position)
        {
            try
            {
                return position;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return 0;
            }
        }

        private void Click(HashtagUserAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(HashtagUserAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }
    }

    public class HashtagUserAdapterViewHolder : RecyclerView.ViewHolder
    {
        public HashtagUserAdapterViewHolder(View itemView, Action<HashtagUserAdapterClickEventArgs> clickListener,
            Action<HashtagUserAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                Button = MainView.FindViewById<Button>(Resource.Id.cont);

                //Create an Event
                itemView.Click += (sender, e) => clickListener(new HashtagUserAdapterClickEventArgs
                    {View = itemView, Position = AdapterPosition});
                itemView.LongClick += (sender, e) => longClickListener(new HashtagUserAdapterClickEventArgs
                    {View = itemView, Position = AdapterPosition});
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Variables Basic

        public View MainView { get; }

        

        public Button Button { get; private set; }

        #endregion
    }

    public class HashtagUserAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
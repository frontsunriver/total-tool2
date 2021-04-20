using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.App;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Java.Util;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Page;
using IList = System.Collections.IList;
using Object = Java.Lang.Object;

namespace WoWonder.Activities.Communities.Adapters
{
    public class ReviewsAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<ReviewsAdapterClickEventArgs> ItemClick;
        public event EventHandler<ReviewsAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;
        public ObservableCollection<ReviewsPageObject.Datum> UserList = new ObservableCollection<ReviewsPageObject.Datum>();

        public ReviewsAdapter(Activity activity)
        {
            try
            {
                HasStableIds = true;
                ActivityContext = activity;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => UserList?.Count ?? 0;
 
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_Reviews_View
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_Reviews_View, parent, false);
                var vh = new ReviewsAdapterViewHolder(itemView, Click, LongClick);
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
                    case ReviewsAdapterViewHolder holder:
                    {
                        var item = UserList[position];
                        if (item != null)
                        {
                            GlideImageLoader.LoadImage(ActivityContext, item.UserData.Avatar, holder.Image, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);

                            holder.Name.Text = WoWonderTools.GetNameFinal(item.UserData);
                     
                            switch (string.IsNullOrEmpty(item.Valuation))
                            {
                                case false:
                                    holder.Review.Text = Methods.FunString.DecodeString(item.Review);
                                    holder.Review.Visibility = ViewStates.Visible;
                                    break;
                                default:
                                    holder.Review.Visibility = ViewStates.Gone;
                                    break;
                            }

                            holder.Val.Text = item.Valuation;
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
        public override void OnViewRecycled(Object holder)
        {
            try
            {
                 if (ActivityContext?.IsDestroyed != false)
                        return;

                 switch (holder)
                 {
                     case ReviewsAdapterViewHolder viewHolder:
                         Glide.With(ActivityContext).Clear(viewHolder.Image);
                         break;
                 }
                base.OnViewRecycled(holder);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        public ReviewsPageObject.Datum GetItem(int position)
        {
            return UserList[position];
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

        private void Click(ReviewsAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(ReviewsAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }

        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = UserList[p0];
                switch (item)
                {
                    case null:
                        return Collections.SingletonList(p0);
                }

                if (item.UserData.Avatar != "")
                {
                    d.Add(item.UserData.Avatar);
                    return d;
                }

                return d;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return Collections.SingletonList(p0);
            }
        }

        public RequestBuilder GetPreloadRequestBuilder(Object p0)
        {
            return GlideImageLoader.GetPreLoadRequestBuilder(ActivityContext, p0.ToString(), ImageStyle.CircleCrop);
        }
    }

    public class ReviewsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ReviewsAdapterViewHolder(View itemView, Action<ReviewsAdapterClickEventArgs> clickListener,
            Action<ReviewsAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                Image = MainView.FindViewById<ImageView>(Resource.Id.image);
                Name = MainView.FindViewById<TextView>(Resource.Id.name);
                Val = MainView.FindViewById<TextView>(Resource.Id.count);
                Review = MainView.FindViewById<TextView>(Resource.Id.review);
               
                //Event
                itemView.Click += (sender, e) => clickListener(new ReviewsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new ReviewsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });

              
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Variables Basic

        public View MainView { get; }

        public ImageView Image { get; private set; }
        public TextView Name { get; private set; }
        public TextView Val { get; private set; }
        public TextView Review { get; private set; }
        

        #endregion
    }

    public class ReviewsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
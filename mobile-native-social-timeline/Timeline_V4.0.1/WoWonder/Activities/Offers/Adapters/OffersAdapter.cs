using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.App;
using Android.Views;

using Android.Widget;
using Bumptech.Glide;
using Java.Util;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Offers;
using IList = System.Collections.IList;
using Android.Content.Res;

using Android.Graphics;
using AndroidX.Core.View;
using AndroidX.RecyclerView.Widget;

namespace WoWonder.Activities.Offers.Adapters
{
    public class OffersAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<OffersAdapterViewHolderClickEventArgs> ItemClick;
        public event EventHandler<OffersAdapterViewHolderClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;

        public ObservableCollection<OffersDataObject> OffersList = new ObservableCollection<OffersDataObject>();

        public OffersAdapter(Activity context)
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

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_OffersView
                View itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_OffersView, parent, false);
                var vh = new OffersAdapterViewHolder(itemView, OnClick, OnLongClick);
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
                    case OffersAdapterViewHolder holder:
                    {
                        var item = OffersList[position];
                        if (item != null)
                        {
                            GlideImageLoader.LoadImage(ActivityContext, item.Image, holder.Image, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);

                            holder.Type.Text = Methods.FunString.DecodeString(item.OfferText) + " " + Methods.FunString.DecodeString(item.DiscountedItems);
                            holder.Title.Text = Methods.FunString.SubStringCutOf(Methods.FunString.DecodeString(item.Description),200);
                            holder.Date.Text = ActivityContext.GetText(Resource.String.Lbl_End_Date) + " : " + item.ExpireDate;

                            ViewCompat.SetBackgroundTintList(holder.Date, ColorStateList.ValueOf(Color.ParseColor(Methods.FunString.RandomColor())));

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
        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            try
            {
                if (ActivityContext?.IsDestroyed != false)
                        return;

                switch (holder)
                {
                    case OffersAdapterViewHolder viewHolder:
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
        public override int ItemCount => OffersList?.Count ?? 0;

        public OffersDataObject GetItem(int position)
        {
            return OffersList[position];
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

        void OnClick(OffersAdapterViewHolderClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(OffersAdapterViewHolderClickEventArgs args) => ItemLongClick?.Invoke(this, args);

        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = OffersList[p0];
                switch (item)
                {
                    case null:
                        return d;
                    default:
                    {
                        switch (string.IsNullOrEmpty(item.Image))
                        {
                            case false:
                                d.Add(item.Image);
                                break;
                        }

                        return d;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return Collections.SingletonList(p0);
            }
        }

        public RequestBuilder GetPreloadRequestBuilder(Java.Lang.Object p0)
        {
            return GlideImageLoader.GetPreLoadRequestBuilder(ActivityContext, p0.ToString(), ImageStyle.CenterCrop);
        }

    }

    public class OffersAdapterViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic

        public View MainView { get; private set; }

        public ImageView Image { get; private set; }
        public TextView Title { get; private set; }
        public TextView Type { get; private set; }
        public TextView Date { get; private set; }


        #endregion

        public OffersAdapterViewHolder(View itemView, Action<OffersAdapterViewHolderClickEventArgs> clickListener, Action<OffersAdapterViewHolderClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                Image = MainView.FindViewById<ImageView>(Resource.Id.image);
                Type = MainView.FindViewById<TextView>(Resource.Id.subtitle);
                Title = MainView.FindViewById<TextView>(Resource.Id.title);
                Date = MainView.FindViewById<TextView>(Resource.Id.date);
                 
                //Create an Event
                itemView.Click += (sender, e) => clickListener(new OffersAdapterViewHolderClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new OffersAdapterViewHolderClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }

    public class OffersAdapterViewHolderClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
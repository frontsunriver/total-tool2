using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide.Request;
using Java.IO;
using Java.Util;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using Console = System.Console;
using IList = System.Collections.IList;
using Object = Java.Lang.Object;

namespace WoWonder.Activities.NearbyShops.Adapters
{
    public class NearbyShopsAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<NearbyShopsAdapterClickEventArgs> ItemClick;
        public event EventHandler<NearbyShopsAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;
        public ObservableCollection<NearbyShopsDataObject> NearbyShopsList = new ObservableCollection<NearbyShopsDataObject>();

        public NearbyShopsAdapter(Activity context)
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

        public override int ItemCount => NearbyShopsList?.Count ?? 0;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_Market_view
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_Market_view, parent, false);
                var vh = new NearbyShopsAdapterViewHolder(itemView, Click, LongClick); 
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
                    case NearbyShopsAdapterViewHolder holder:
                    {
                        var item = NearbyShopsList[position];
                        if (item != null) Initialize(holder, item);
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void Initialize(NearbyShopsAdapterViewHolder holder, NearbyShopsDataObject item)
        {
            try
            {
                switch (item.Product?.ProductClass?.Images?.Count)
                {
                    case > 0 when item.Product?.ProductClass != null && item.Product.Value.ProductClass.Images[0].Image.Contains("http"):
                        GlideImageLoader.LoadImage(ActivityContext, item.Product?.ProductClass?.Images?[0]?.Image, holder.Thumbnail, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);
                        break;
                    case > 0:
                        Glide.With(ActivityContext).Load(new File(item.Product?.ProductClass?.Images?[0]?.Image)).Apply(new RequestOptions().CenterCrop().Placeholder(Resource.Drawable.ImagePlacholder).Error(Resource.Drawable.ImagePlacholder)).Into(holder.Thumbnail);
                        break;
                }

                GlideImageLoader.LoadImage(ActivityContext, item.Product?.ProductClass?.Seller?.Avatar, holder.Userprofilepic, ImageStyle.CircleCrop, ImagePlaceholders.Color);

            

                holder.Title.Text = Methods.FunString.DecodeString(item.Product?.ProductClass?.Name);

                holder.UserName.Text = WoWonderTools.GetNameFinal(item.Product?.ProductClass?.Seller);
                holder.Time.Text = item.Product?.ProductClass?.TimeText;

                var (currency, currencyIcon) = WoWonderTools.GetCurrency(item.Product?.ProductClass?.Currency);
                Console.WriteLine(currency);

                holder.TxtPrice.Text =  item.Product?.ProductClass?.Price + " " + currencyIcon;
                holder.LocationText.Text = !string.IsNullOrEmpty(item.Product?.ProductClass?.Location) ? item.Product?.ProductClass?.Location : ActivityContext.GetText(Resource.String.Lbl_Unknown);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
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
                     case NearbyShopsAdapterViewHolder viewHolder:
                         Glide.With(ActivityContext).Clear(viewHolder.Thumbnail);
                         break;
                 }
                base.OnViewRecycled(holder);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public NearbyShopsDataObject GetItem(int position)
        {
            return NearbyShopsList[position];
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

        private void Click(NearbyShopsAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(NearbyShopsAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }

        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = NearbyShopsList[p0];
                switch (item)
                {
                    case null:
                        return Collections.SingletonList(p0);
                }

                switch (item.Product?.ProductClass?.Images?.Count)
                {
                    case > 0:
                        d.Add(item.Product?.ProductClass?.Images[0].Image);
                        d.Add(item.Product?.ProductClass?.Seller.Avatar);
                        return d;
                    default:
                        d.Add(item.Product?.ProductClass?.Seller.Avatar);

                        return d;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return Collections.SingletonList(p0);
            }
        }

        public RequestBuilder GetPreloadRequestBuilder(Object p0)
        {

            return Glide.With(ActivityContext).Load(p0.ToString()).Apply(new RequestOptions().CenterCrop().SetDiskCacheStrategy(DiskCacheStrategy.All));
        }
    }

    public class NearbyShopsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public NearbyShopsAdapterViewHolder(View itemView, Action<NearbyShopsAdapterClickEventArgs> clickListener, Action<NearbyShopsAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                Thumbnail = MainView.FindViewById<ImageView>(Resource.Id.thumbnail);
                Title = MainView.FindViewById<TextView>(Resource.Id.titleTextView);
              
                LocationText = MainView.FindViewById<TextView>(Resource.Id.LocationText);
                Userprofilepic = MainView.FindViewById<ImageView>(Resource.Id.userprofile_pic);
                UserName = MainView.FindViewById<TextView>(Resource.Id.User_name);
                Time = MainView.FindViewById<TextView>(Resource.Id.card_dist);
                TxtPrice = MainView.FindViewById<TextView>(Resource.Id.pricetext);
                DeviderView = MainView.FindViewById<View>(Resource.Id.view);
                DeviderView.SetBackgroundColor(!AppSettings.SetTabDarkTheme ? Color.ParseColor("#e7e7e7") : Color.ParseColor("#BDBDBD"));

                //Event
                itemView.Click += (sender, e) => clickListener(new NearbyShopsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new NearbyShopsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #region Variables Basic

        public View MainView { get; }

        public ImageView Thumbnail { get; private set; }
        public TextView Title { get; private set; }
     
        public ImageView Userprofilepic { get; private set; }
        public TextView UserName { get; private set; }
        public TextView Time { get; private set; }
        public TextView LocationText { get; private set; }
        public TextView TxtPrice { get; private set; }
        public View DeviderView { get; private set; }

        #endregion
    }


    public class NearbyShopsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}

using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.App;
using Android.Graphics;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide.Request;
using Java.IO;
using Java.Util;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Product;
using Console = System.Console;
using IList = System.Collections.IList;
using Object = Java.Lang.Object;

namespace WoWonder.Activities.Market.Adapters
{
    public class MarketAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<MarketAdapterClickEventArgs> ItemClick;
        public event EventHandler<MarketAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;
        public ObservableCollection<ProductDataObject> MarketList =new ObservableCollection<ProductDataObject>();

        public MarketAdapter(Activity context)
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

        public override int ItemCount => MarketList?.Count ?? 0;
 
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_Market_view
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_Market_view, parent, false);
                var vh = new MarketAdapterViewHolder(itemView, Click, LongClick);
               
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
                    case MarketAdapterViewHolder holder:
                    {
                        var item = MarketList[position];
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

        private void Initialize(MarketAdapterViewHolder holder, ProductDataObject item)
        {
            try
            {
                switch (item.Images?.Count)
                {
                    case > 0 when item.Images[0].Image.Contains("http"):
                        GlideImageLoader.LoadImage(ActivityContext, item.Images?[0]?.Image, holder.Thumbnail, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);
                        break;
                    case > 0:
                        Glide.With(ActivityContext).Load(new File(item.Images?[0]?.Image)).Apply(new RequestOptions().CenterCrop().Placeholder(Resource.Drawable.ImagePlacholder).Error(Resource.Drawable.ImagePlacholder)).Into(holder.Thumbnail);
                        break;
                }

                GlideImageLoader.LoadImage(ActivityContext, item.Seller?.Avatar, holder.Userprofilepic,ImageStyle.CircleCrop, ImagePlaceholders.Color);
                  
                holder.Title.Text = Methods.FunString.DecodeString(item.Name);
                holder.UserName.Text = WoWonderTools.GetNameFinal(item.Seller);
                holder.Time.Text = item.TimeText;

                var (currency, currencyIcon) = WoWonderTools.GetCurrency(item.Currency);
                Console.WriteLine(currency);

                holder.TxtPrice.Text = item.Price + " " + currencyIcon;
                holder.LocationText.Text = !string.IsNullOrEmpty(item.Location) ? Methods.FunString.DecodeString(item.Location)  : ActivityContext.GetText(Resource.String.Lbl_Unknown);
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
                    case MarketAdapterViewHolder viewHolder:
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
        public ProductDataObject GetItem(int position)
        {
            return MarketList[position];
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

        private void Click(MarketAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(MarketAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }

        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = MarketList[p0];
                switch (item)
                {
                    case null:
                        return Collections.SingletonList(p0);
                }

                switch (item.Images?.Count)
                {
                    case > 0:
                        d.Add(item.Images[0].Image);
                        d.Add(item.Seller.Avatar);
                        return d;
                    default:
                        d.Add(item.Seller.Avatar);
                
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
            
            return Glide.With(ActivityContext).Load(p0.ToString())
                .Apply(new RequestOptions().CenterCrop().SetDiskCacheStrategy(DiskCacheStrategy.All));
        }
    }

    public class MarketAdapterViewHolder : RecyclerView.ViewHolder
    {
        public MarketAdapterViewHolder(View itemView, Action<MarketAdapterClickEventArgs> clickListener,Action<MarketAdapterClickEventArgs> longClickListener) : base(itemView)
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
                itemView.Click += (sender, e) => clickListener(new MarketAdapterClickEventArgs{View = itemView, Position = AdapterPosition});
                itemView.LongClick += (sender, e) => longClickListener(new MarketAdapterClickEventArgs{View = itemView, Position = AdapterPosition});

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

    public class MarketAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    } 
}
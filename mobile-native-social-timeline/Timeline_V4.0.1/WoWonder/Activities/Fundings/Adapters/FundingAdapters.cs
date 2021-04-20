using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Android.App;
using Android.Graphics;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Funding;
using Java.Util;
using IList = System.Collections.IList;

namespace WoWonder.Activities.Fundings.Adapters
{
    public class FundingAdapters : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<FundingAdaptersViewHolderClickEventArgs> ItemClick;
        public event EventHandler<FundingAdaptersViewHolderClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;

        public ObservableCollection<FundingDataObject> FundingList = new ObservableCollection<FundingDataObject>();

        public FundingAdapters(Activity context)
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
                //Setup your layout here >> Style_LastActivities_View
                View itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_FundingView, parent, false);
                var vh = new FundingAdaptersViewHolder(itemView, OnClick, OnLongClick);
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
                    case FundingAdaptersViewHolder holder:
                    {
                        var item = FundingList[position];
                        if (item != null)
                        {  
                            GlideImageLoader.LoadImage(ActivityContext, item.Image, holder.Image, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);

                            holder.Title.Text = Methods.FunString.DecodeString(item.Title);
                         
                            if (item.UserData != null)
                                holder.Username.Text = WoWonderTools.GetNameFinal(item.UserData);
                            else
                                holder.Username.Visibility = ViewStates.Gone;

                            try
                            {
                                item.Raised = item.Raised.Replace(AppSettings.CurrencyFundingPriceStatic, "");
                                item.Amount = item.Amount.Replace(AppSettings.CurrencyFundingPriceStatic, "");
                             
                                decimal d = decimal.Parse(item.Raised, CultureInfo.InvariantCulture);
                                holder.Raised.Text = AppSettings.CurrencyFundingPriceStatic + d.ToString("0.00");

                                decimal amount = decimal.Parse(item.Amount, CultureInfo.InvariantCulture);
                                holder.TottalAmount.Text = AppSettings.CurrencyFundingPriceStatic + amount.ToString("0.00");

                                holder.Progress.Progress = Convert.ToInt32(item.Bar?.ToString("0") ?? "0");
                            }
                            catch (Exception exception)
                            {
                                holder.Raised.Text = AppSettings.CurrencyFundingPriceStatic + item.Raised;
                                holder.TottalAmount.Text = AppSettings.CurrencyFundingPriceStatic + item.Amount;
                                holder.Progress.Progress = Convert.ToInt32(item.Bar);
                                Methods.DisplayReportResultTrack(exception);
                            }
                        
                            holder.DonationTime.Text = IonIconsFonts.Time + "  " + Methods.Time.TimeAgo(Convert.ToInt32(item.Time), false);
                         
                            switch (string.IsNullOrEmpty(item.Description))
                            {
                                case false:
                                    holder.Description.Text = Methods.FunString.DecodeString(item.Description);
                                    break;
                                default:
                                    ActivityContext.GetText(Resource.String.Lbl_NoAnyDescription);
                                    break;
                            }
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
                    case FundingAdaptersViewHolder viewHolder:
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
        public override int ItemCount => FundingList?.Count ?? 0;
 
        public FundingDataObject GetItem(int position)
        {
            return FundingList[position];
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

        void OnClick(FundingAdaptersViewHolderClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(FundingAdaptersViewHolderClickEventArgs args) => ItemLongClick?.Invoke(this, args);
         
        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = FundingList[p0];
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

    public class FundingAdaptersViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic

        public View MainView { get; private set; }

        public ImageView Image { get; private set; }
        public TextView Title { get; private set; }
        public TextView TottalAmount { get; private set; }

        public TextView Raised { get; private set; }

        public TextView Username { get; private set; }
        public TextView DonationTime { get; private set; }
        

        public TextView Description { get; private set; }
        public ProgressBar Progress { get; private set; }

        #endregion

        public FundingAdaptersViewHolder(View itemView, Action<FundingAdaptersViewHolderClickEventArgs> clickListener, Action<FundingAdaptersViewHolderClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView; 

                Image = (ImageView)MainView.FindViewById(Resource.Id.fundImage);
                Title = (TextView)MainView.FindViewById(Resource.Id.fundTitle);
                Description = (TextView)MainView.FindViewById(Resource.Id.fundDescription);
                Progress = (ProgressBar)MainView.FindViewById(Resource.Id.progressBar);
                TottalAmount = (TextView)MainView.FindViewById(Resource.Id.TottalAmount);
                Raised = (TextView)MainView.FindViewById(Resource.Id.raised);
                Username = (TextView)MainView.FindViewById(Resource.Id.fundUsername);
                DonationTime = (TextView)MainView.FindViewById(Resource.Id.time);
                 
                var font = Typeface.CreateFromAsset(Application.Context.Resources?.Assets, "ionicons.ttf");
                DonationTime?.SetTypeface(font, TypefaceStyle.Normal);

                //Create an Event
                itemView.Click += (sender, e) => clickListener(new FundingAdaptersViewHolderClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new FundingAdaptersViewHolderClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }

    public class FundingAdaptersViewHolderClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
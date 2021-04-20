using System;
using System.Collections.ObjectModel;
using Android.App;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Refractored.Controls;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Funding;
using Exception = System.Exception;
using Object = Java.Lang.Object;

namespace WoWonder.Activities.Fundings.Adapters
{
    public class RecentDonationAdapter : RecyclerView.Adapter 
    { 
        public event EventHandler<RecentDonationAdapterClickEventArgs> FollowButtonItemClick;
        public event EventHandler<RecentDonationAdapterClickEventArgs> ItemClick;
        public event EventHandler<RecentDonationAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;
        public ObservableCollection<RecentDonation> UserList = new ObservableCollection<RecentDonation>();
        
        public RecentDonationAdapter(Activity activity)
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
                //Setup your layout here >> Style_HContact_view
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_HContact_view, parent, false);
                var vh = new RecentDonationAdapterViewHolder(itemView, FollowButtonClick, Click, LongClick);
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
                    case RecentDonationAdapterViewHolder holder:
                    {
                        var item = UserList[position];
                        if (item != null)
                        {
                            GlideImageLoader.LoadImage(ActivityContext, item.UserData.Avatar, holder.Image, ImageStyle.CircleCrop, ImagePlaceholders.Drawable, true);

                            holder.Name.Text = Methods.FunString.SubStringCutOf(WoWonderTools.GetNameFinal(item.UserData), 20);

                            switch (item.UserData.Verified)
                            {
                                case "1":
                                    holder.Name.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.icon_checkmark_small_vector, 0);
                                    break;
                            }

                            holder.About.Text = AppSettings.CurrencyFundingPriceStatic + item.Amount + " " + Methods.Time.TimeAgo(Convert.ToInt32(item.Time), true);

                            //Online Or offline
                            var online = WoWonderTools.GetStatusOnline(Convert.ToInt32(item.UserData.LastseenUnixTime), item.UserData.LastseenStatus);
                            holder.ImageLastSeen.SetImageResource(online ? Resource.Drawable.Green_Color : Resource.Drawable.Grey_Offline);
                         
                            WoWonderTools.SetAddFriendCondition(item.UserData.IsFollowing, holder.Button);
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
                    case RecentDonationAdapterViewHolder viewHolder:
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

        public RecentDonation GetItem(int position)
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

        private void FollowButtonClick(RecentDonationAdapterClickEventArgs args) => FollowButtonItemClick?.Invoke(this, args);
        private void Click(RecentDonationAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        private void LongClick(RecentDonationAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
           
    }

    public class RecentDonationAdapterViewHolder : RecyclerView.ViewHolder
    {
        public RecentDonationAdapterViewHolder(View itemView, Action<RecentDonationAdapterClickEventArgs> followButtonClickListener, Action<RecentDonationAdapterClickEventArgs> clickListener, Action<RecentDonationAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                Image = MainView.FindViewById<ImageView>(Resource.Id.card_pro_pic);
                Name = MainView.FindViewById<TextView>(Resource.Id.card_name);
                About = MainView.FindViewById<TextView>(Resource.Id.card_dist);
                Button = MainView.FindViewById<Button>(Resource.Id.cont);
                ImageLastSeen = (CircleImageView)MainView.FindViewById(Resource.Id.ImageLastseen);

                //Event
                Button.Click += (sender, e) => followButtonClickListener(new RecentDonationAdapterClickEventArgs { View = itemView, Position = AdapterPosition, BtnAddUser = Button });
                itemView.Click += (sender, e) => clickListener(new RecentDonationAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new RecentDonationAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
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
        public TextView About { get; private set; }
        public Button Button { get; private set; }
        public CircleImageView ImageLastSeen { get; private set; }

        #endregion
    }

    public class RecentDonationAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
        public Button BtnAddUser { get; set; }
    }
}
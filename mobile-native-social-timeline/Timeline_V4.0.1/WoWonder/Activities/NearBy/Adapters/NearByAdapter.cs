
using Android.Views;
using Android.Widget;
using Refractored.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.App;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Java.Util;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using IList = System.Collections.IList;
using Object = Java.Lang.Object;

namespace WoWonder.Activities.NearBy.Adapters
{
    public class NearByAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        private readonly Activity ActivityContext;

        public ObservableCollection<UserDataObject> UserList =new ObservableCollection<UserDataObject>();

        public NearByAdapter(Activity context)
        {
            try
            {
                //HasStableIds = true;
                ActivityContext = context; 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => UserList?.Count ?? 0;

        public event EventHandler<NearByAdapterClickEventArgs> FollowButtonItemClick;
        public event EventHandler<NearByAdapterClickEventArgs> ItemClick;

        public event EventHandler<NearByAdapterClickEventArgs> ItemLongClick;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_NearBy_view
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_NearBy_view, parent, false);
                var vh = new NearByAdapterViewHolder(itemView, FollowButtonClick, Click, LongClick);
                return vh;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }
         
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            try
            {
                switch (viewHolder)
                {
                    case NearByAdapterViewHolder holder:
                    {
                        var users = UserList[position];
                        switch (users)
                        {
                            case null:
                                return;
                        }

                        GlideImageLoader.LoadImage(ActivityContext, users.Avatar, holder.Image, ImageStyle.CircleCrop, ImagePlaceholders.Color);

                        var online = WoWonderTools.GetStatusOnline(Convert.ToInt32(users.LastseenUnixTime), users.LastseenStatus);

                        switch (online)
                        {
                            //Online Or offline
                            case true:
                                //Online
                                holder.ImageOnline.SetImageResource(Resource.Drawable.Green_Color);
                                holder.LastTimeOnline.Text = ActivityContext.GetString(Resource.String.Lbl_Online);
                                break;
                            default:
                                holder.ImageOnline.SetImageResource(Resource.Drawable.Grey_Offline);
                                holder.LastTimeOnline.Text = Methods.Time.TimeAgo(Convert.ToInt32(users.LastseenUnixTime), false);
                                break;
                        }

                        holder.Name.Text = Methods.FunString.SubStringCutOf(WoWonderTools.GetNameFinal(users), 14);

                        switch (users.Verified)
                        {
                            case "1":
                                holder.Name.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.icon_checkmark_small_vector, 0);
                                break;
                        }

                        WoWonderTools.SetAddFriendCondition(users.IsFollowing, holder.Button);
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
                     case NearByAdapterViewHolder viewHolder:
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
        public UserDataObject GetItem(int position)
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

        private void FollowButtonClick(NearByAdapterClickEventArgs args) => FollowButtonItemClick?.Invoke(this, args);
        private void Click(NearByAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        private void LongClick(NearByAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
     
        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = UserList[p0];
                switch (item)
                {
                    case null:
                        return d;
                    default:
                    {
                        switch (string.IsNullOrEmpty(item.Avatar))
                        {
                            case false:
                                d.Add(item.Avatar);
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

        public RequestBuilder GetPreloadRequestBuilder(Object p0)
        {
            return GlideImageLoader.GetPreLoadRequestBuilder(ActivityContext, p0.ToString(), ImageStyle.CenterCrop);
        } 
    }
     
    public class NearByAdapterViewHolder : RecyclerView.ViewHolder
    {
        public NearByAdapterViewHolder(View itemView, Action<NearByAdapterClickEventArgs> followButtonClickListener, Action<NearByAdapterClickEventArgs> clickListener, Action<NearByAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                Image = MainView.FindViewById<CircleImageView>(Resource.Id.people_profile_sos);
                ImageOnline = MainView.FindViewById<CircleImageView>(Resource.Id.ImageLastseen);
                Name = MainView.FindViewById<TextView>(Resource.Id.people_profile_name);
                LastTimeOnline = MainView.FindViewById<TextView>(Resource.Id.people_profile_time);
                Button = MainView.FindViewById<Button>(Resource.Id.btn_follow_people);



                //Event
                Button.Click += (sender, e) => followButtonClickListener(new NearByAdapterClickEventArgs { View = itemView, Position = AdapterPosition, BtnAddUser = Button });
                itemView.Click += (sender, e) => clickListener(new NearByAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new NearByAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #region Variables Basic

        public View MainView { get; }
        public CircleImageView Image { get; set; }
        public CircleImageView ImageOnline { get; set; }
        public TextView Name { get; set; }
        public TextView LastTimeOnline { get; set; }
        public Button Button { get; set; }

        #endregion Variables Basic
    }

    public class NearByAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
        public Button BtnAddUser { get; set; }
    }
}
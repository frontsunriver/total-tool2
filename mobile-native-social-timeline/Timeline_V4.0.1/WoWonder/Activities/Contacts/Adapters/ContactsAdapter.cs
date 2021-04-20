using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.App;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Java.Util;
using Refractored.Controls;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using Exception = System.Exception;
using IList = System.Collections.IList;
using Object = Java.Lang.Object;

namespace WoWonder.Activities.Contacts.Adapters
{
    public class ContactsAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public enum TypeTextSecondary
        {
            About,
            LastSeen,
            None
        }
        public event EventHandler<ContactsAdapterClickEventArgs> FollowButtonItemClick;
        public event EventHandler<ContactsAdapterClickEventArgs> ItemClick;
        public event EventHandler<ContactsAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;
        public ObservableCollection<UserDataObject> UserList = new ObservableCollection<UserDataObject>();
        private readonly bool ShowButton;
        private readonly TypeTextSecondary Type;

        public ContactsAdapter(Activity activity, bool showButton, TypeTextSecondary type)
        {
            try
            {
                HasStableIds = true;
                ActivityContext = activity;
                ShowButton = showButton;
                Type = type; 
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
                var vh = new ContactsAdapterViewHolder(itemView, FollowButtonClick, Click, LongClick);
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
                    case ContactsAdapterViewHolder holder:
                    {
                        var item = UserList[position];
                        if (item != null)
                        {
                            Initialize(holder, item);

                            holder.Button.Visibility = ShowButton switch
                            {
                                false => ViewStates.Gone,
                                _ => holder.Button.Visibility
                            };
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

        private void Initialize(ContactsAdapterViewHolder holder, UserDataObject users)
        {
            try
            {
                GlideImageLoader.LoadImage(ActivityContext, users.Avatar, holder.Image, ImageStyle.CircleCrop,ImagePlaceholders.Drawable,true);

                holder.Name.Text = Methods.FunString.SubStringCutOf(WoWonderTools.GetNameFinal(users), 20);

                switch (users.Verified)
                {
                    case "1":
                        holder.Name.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.icon_checkmark_small_vector, 0);
                        break;
                }

                switch (Type)
                {
                    case TypeTextSecondary.None:
                        holder.About.Visibility = ViewStates.Gone;
                        break;
                    default:
                        holder.About.Text = Type == TypeTextSecondary.About ? Methods.FunString.SubStringCutOf(WoWonderTools.GetAboutFinal(users), 25) : ActivityContext.GetString(Resource.String.Lbl_Last_seen) + " " + Methods.Time.TimeAgo(Convert.ToInt32(users.LastseenUnixTime), true);
                        break;
                }

                //Online Or offline
                var online = WoWonderTools.GetStatusOnline(Convert.ToInt32(users.LastseenUnixTime), users.LastseenStatus);
                holder.ImageLastSeen.SetImageResource(online ? Resource.Drawable.Green_Color : Resource.Drawable.Grey_Offline);

                switch (ShowButton)
                {
                    case false:
                        return;
                    default:
                        WoWonderTools.SetAddFriendCondition(users.IsFollowing, holder.Button);
                        break;
                }
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
                    case ContactsAdapterViewHolder viewHolder:
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

        private void FollowButtonClick(ContactsAdapterClickEventArgs args) => FollowButtonItemClick?.Invoke(this, args);
        private void Click(ContactsAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        private void LongClick(ContactsAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
          
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

                if (item.Avatar != "")
                {
                    d.Add(item.Avatar);
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

        #region Event Add Friend

        public void OnFollowButtonItemClick(object sender, ContactsAdapterClickEventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(ActivityContext, ActivityContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
                else
                {
                    switch (e.Position)
                    {
                        case > -1:
                        {
                            UserDataObject item = GetItem(e.Position);
                            if (item != null)
                            {
                                WoWonderTools.SetAddFriend(ActivityContext , item , e.BtnAddUser);
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        #endregion

    }

    public class ContactsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ContactsAdapterViewHolder(View itemView, Action<ContactsAdapterClickEventArgs> followButtonClickListener, Action<ContactsAdapterClickEventArgs> clickListener, Action<ContactsAdapterClickEventArgs> longClickListener) : base(itemView)
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
                Button.Click += (sender, e) => followButtonClickListener(new ContactsAdapterClickEventArgs { View = itemView, Position = AdapterPosition , BtnAddUser = Button });
                itemView.Click += (sender, e) => clickListener(new ContactsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new ContactsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
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

    public class ContactsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
        public Button BtnAddUser { get; set; }
    } 
}
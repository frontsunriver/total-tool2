using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.App;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Java.IO;
using Java.Util; 
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using IList = System.Collections.IList;

namespace WoWonder.Activities.UserProfile.Adapters
{
    public class UserGroupsAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<UserGroupsAdapterClickEventArgs> ItemClick;
        public event EventHandler<UserGroupsAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;
        public ObservableCollection<GroupClass> GroupList = new ObservableCollection<GroupClass>();

        public UserGroupsAdapter(Activity context)
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

        public override int ItemCount => GroupList?.Count ?? 0;
 
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_Group_Simple
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_Group_Simple, parent, false);
                var vh = new UserGroupsAdapterViewHolder(itemView, Click, LongClick);
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
                if (viewHolder is not UserGroupsAdapterViewHolder holder)
                    return;

                var item = GroupList[position];
                switch (item)
                {
                    case null:
                        return;
                }

                if (item.Avatar.Contains("http"))
                    GlideImageLoader.LoadImage(ActivityContext, item.Avatar, holder.Image, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);
                else
                    Glide.With(ActivityContext).Load(new File(item.Avatar)).Apply(new RequestOptions().Placeholder(Resource.Drawable.ImagePlacholder).Error(Resource.Drawable.ImagePlacholder)).Into(holder.Image);


                holder.Name.Text = Methods.FunString.SubStringCutOf(Methods.FunString.DecodeString(item.Name), 20);

                item.IsOwner = item.UserId == UserDetails.UserId; 
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
                    case UserGroupsAdapterViewHolder viewHolder:
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
        public GroupClass GetItem(int position)
        {
            return GroupList[position];
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

        private void Click(UserGroupsAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(UserGroupsAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }


        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = GroupList[p0];
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

        public RequestBuilder GetPreloadRequestBuilder(Java.Lang.Object p0)
        {
            return Glide.With(ActivityContext).Load(p0.ToString())
                .Apply(new RequestOptions().CircleCrop());
        } 
    }

    public class UserGroupsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public UserGroupsAdapterViewHolder(View itemView, Action<UserGroupsAdapterClickEventArgs> clickListener,Action<UserGroupsAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;
                Image = MainView.FindViewById<ImageView>(Resource.Id.ImageGroup);
                Name = MainView.FindViewById<TextView>(Resource.Id.Group_titile);

                itemView.Click += (sender, e) => clickListener(new UserGroupsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new UserGroupsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #region Variables Basic

        public View MainView { get; }
        public ImageView Image { get; set;}
        public TextView Name { get; set;}

        #endregion
    }

    public class UserGroupsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
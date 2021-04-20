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
using IList = System.Collections.IList;
using Object = Java.Lang.Object;

namespace WoWonder.Activities.Suggested.Adapters
{ 
    public class SuggestionsUserAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<SuggestionsUserAdapterClickEventArgs> FollowButtonItemClick;
        public event EventHandler<SuggestionsUserAdapterClickEventArgs> ItemClick;
        public event EventHandler<SuggestionsUserAdapterClickEventArgs> ItemLongClick;

        public readonly Activity ActivityContext;
        public ObservableCollection<UserDataObject> UserList = new ObservableCollection<UserDataObject>();
        
        public SuggestionsUserAdapter(Activity context)
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
         
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_PageCircle_view
                View itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_Suggestions_view, parent, false);
                var vh = new SuggestionsUserAdapterViewHolder(itemView, FollowButtonClick, Click, LongClick);
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
                    case SuggestionsUserAdapterViewHolder holder:
                    {
                        var item = UserList[position];
                        if (item != null)
                        {
                            holder.Username.Text = Methods.FunString.SubStringCutOf("@" + item.Username, 15) ;
                            holder.Name.Text = Methods.FunString.SubStringCutOf(WoWonderTools.GetNameFinal(item),15) ;

                            GlideImageLoader.LoadImage(ActivityContext, item.Avatar, holder.Image, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);

                            WoWonderTools.SetAddFriendCondition(item.IsFollowing, holder.Button); 
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
                    case SuggestionsUserAdapterViewHolder viewHolder:
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

        public override int ItemCount => UserList?.Count ?? 0;
 
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

        private void FollowButtonClick(SuggestionsUserAdapterClickEventArgs args) => FollowButtonItemClick?.Invoke(this, args);
        void Click(SuggestionsUserAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void LongClick(SuggestionsUserAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);


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
            return GlideImageLoader.GetPreLoadRequestBuilder(ActivityContext, p0.ToString(), ImageStyle.CircleCrop);
        } 
    }

    public class SuggestionsUserAdapterViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic


        public View MainView { get;  set; }

        
        public ImageView Image { get; set; }
        public CircleImageView ImageOnline { get; set; }

        public TextView Name { get; set; }
        public TextView Username { get; set; }
        public Button Button { get; set; }

        #endregion

        public SuggestionsUserAdapterViewHolder(View itemView, Action<SuggestionsUserAdapterClickEventArgs> followButtonClickListener, Action<SuggestionsUserAdapterClickEventArgs> clickListener,Action<SuggestionsUserAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                Image = MainView.FindViewById<ImageView>(Resource.Id.people_profile_sos);
                Name = MainView.FindViewById<TextView>(Resource.Id.name);
                Username = MainView.FindViewById<TextView>(Resource.Id.username);
                Button = MainView.FindViewById<Button>(Resource.Id.FollowButton);

                //Event
                Button.Click += (sender, e) => followButtonClickListener(new SuggestionsUserAdapterClickEventArgs { View = itemView, Position = AdapterPosition, BtnAddUser = Button });
                itemView.Click += (sender, e) => clickListener(new SuggestionsUserAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new SuggestionsUserAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }

    public class SuggestionsUserAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
        public Button BtnAddUser { get; set; }
    } 
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.App;
using Android.Graphics;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Java.Util;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using IList = System.Collections.IList;
using Object = Java.Lang.Object;

namespace WoWonder.Activities.Suggested.Adapters
{
    public class SuggestedGroupAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<SuggestedGroupAdapterClickEventArgs> JoinButtonItemClick;
        public event EventHandler<SuggestedGroupAdapterClickEventArgs> ItemClick;
        public event EventHandler<SuggestedGroupAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;
        public ObservableCollection<GroupClass> GroupList = new ObservableCollection<GroupClass>();

        public SuggestedGroupAdapter(Activity context)
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
                //Setup your layout here >> Style_SuggestedGroupView
                View itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_SuggestedGroupView, parent, false);
                var vh = new SuggestedGroupAdapterViewHolder(itemView, JoinButtonClick, Click, LongClick);
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
                    case SuggestedGroupAdapterViewHolder holder:
                    {
                        var item = GroupList[position];
                        if (item != null)
                        {
                            GlideImageLoader.LoadImage(ActivityContext, item.Cover, holder.Image, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);

                            holder.Name.Text = Methods.FunString.DecodeString(item.GroupName);
                            holder.CountMembers.Text = Methods.FunString.FormatPriceValue(item.Members) +  " " +ActivityContext.GetString(Resource.String.Lbl_Members);

                            if (item.IsOwner != null && item.IsOwner.Value || item.UserId == UserDetails.UserId)
                            {
                                holder.JoinButton.Visibility = ViewStates.Gone;
                            }
                            else
                            {
                                if (WoWonderTools.IsJoinedGroup(item))
                                {
                                    holder.JoinButton.SetBackgroundResource(Resource.Drawable.buttonFlatGray);
                                    holder.JoinButton.SetTextColor(Color.White);
                                    holder.JoinButton.Text = ActivityContext.GetString(Resource.String.Btn_Joined);
                                    holder.JoinButton.Tag = "true";
                                }
                                else
                                {
                                    holder.JoinButton.SetBackgroundResource(Resource.Drawable.buttonFlat);
                                    holder.JoinButton.SetTextColor(Color.White);
                                    holder.JoinButton.Text = ActivityContext.GetString(Resource.String.Btn_Join_Group);
                                    holder.JoinButton.Tag = "false";
                                }
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

        public override void OnViewRecycled(Object holder)
        {
            try
            {
                if (ActivityContext?.IsDestroyed != false)
                    return;

                switch (holder)
                {
                    case SuggestedGroupAdapterViewHolder viewHolder:
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
        public override int ItemCount => GroupList?.Count ?? 0;

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

        void JoinButtonClick(SuggestedGroupAdapterClickEventArgs args) => JoinButtonItemClick?.Invoke(this, args);
        void Click(SuggestedGroupAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void LongClick(SuggestedGroupAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);


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

        public RequestBuilder GetPreloadRequestBuilder(Object p0)
        {
            return GlideImageLoader.GetPreLoadRequestBuilder(ActivityContext, p0.ToString(), ImageStyle.CircleCrop);
        }

    }

    public class SuggestedGroupAdapterViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic
         
        public View MainView { get; private set; }
         
        public ImageView Image { get; private set; }

        public TextView Name { get; private set; }
        public TextView CountMembers { get; private set; }
        public Button JoinButton { get; private set; }

        #endregion

        public SuggestedGroupAdapterViewHolder(View itemView, Action<SuggestedGroupAdapterClickEventArgs> joinButtonClickListener, Action<SuggestedGroupAdapterClickEventArgs> clickListener, Action<SuggestedGroupAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                Image = MainView.FindViewById<ImageView>(Resource.Id.coverGroup);
                Name = MainView.FindViewById<TextView>(Resource.Id.name);
                CountMembers = MainView.FindViewById<TextView>(Resource.Id.countMembers);
                JoinButton = MainView.FindViewById<Button>(Resource.Id.JoinButton);

                //Event
                JoinButton.Click += (sender, e) => joinButtonClickListener(new SuggestedGroupAdapterClickEventArgs { View = itemView, Position = AdapterPosition , JoinButton = JoinButton });
                itemView.Click += (sender, e) => clickListener(new SuggestedGroupAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                Console.WriteLine(longClickListener);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }

    public class SuggestedGroupAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
        public Button JoinButton { get; set; }
    } 
}
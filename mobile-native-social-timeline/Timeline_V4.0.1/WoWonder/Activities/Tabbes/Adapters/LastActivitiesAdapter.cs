using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Android.App; 
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Java.Util;
using Refractored.Controls;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using IList = System.Collections.IList;

namespace WoWonder.Activities.Tabbes.Adapters
{
    public class LastActivitiesAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<LastActivitiesAdapterClickEventArgs> ItemClick;
        public event EventHandler<LastActivitiesAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext; 
        public ObservableCollection<ActivityDataObject> LastActivitiesList = new ObservableCollection<ActivityDataObject>();

        public LastActivitiesAdapter(Activity context)
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

        public override int ItemCount => LastActivitiesList?.Count ?? 0;
 

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_LastActivities_View
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_LastActivities_View, parent, false);
                var vh = new LastActivitiesAdapterViewHolder(itemView, Click, LongClick);
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
                    case LastActivitiesAdapterViewHolder holder:
                    {
                        var item = LastActivitiesList[position];
                        if (item != null)
                        {
                            InitializeLast(holder, item);
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

        private void InitializeLast(LastActivitiesAdapterViewHolder holder, ActivityDataObject item)
        {
            try
            {
                GlideImageLoader.LoadImage(ActivityContext, item.Activator.Avatar, holder.ActivitiesImage, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);

                string replace = "";
                if (item.ActivityType.Contains("reaction"))
                {  
                    if (item.ActivityType.Contains("Like"))
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.emoji_like);
                    }
                    else if (item.ActivityType.Contains("Love"))
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.emoji_love);
                    }
                    else if (item.ActivityType.Contains("HaHa"))
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.emoji_haha);
                    }
                    else if (item.ActivityType.Contains("Wow"))
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.emoji_wow);
                    }
                    else if (item.ActivityType.Contains("Sad"))
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.emoji_sad);
                    }
                    else if (item.ActivityType.Contains("Angry"))
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.emoji_angry);
                    }

                    if (UserDetails.LangName.Contains("fr"))
                    {
                        var split = item.ActivityText.Split("reacted to").Last().Replace("post", "");
                        replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_ReactedTo) + " " + ActivityContext.GetString(Resource.String.Lbl_Post) + " " + split;
                    }
                    else
                        replace = item.ActivityText.Replace("reacted to", ActivityContext.GetString(Resource.String.Lbl_ReactedTo)).Replace("post", ActivityContext.GetString(Resource.String.Lbl_Post));

                }
                else switch (item.ActivityType)
                {
                    case "friend":
                    case "following":
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.ic_add);
                        //holder.Icon.SetColorFilter(Color.ParseColor("#333333"), PorterDuff.Mode.Multiply);

                        if (item.ActivityText.Contains("started following"))
                        {
                            if (UserDetails.LangName.Contains("fr"))
                            {
                                var split = item.ActivityText.Split("started following").Last().Replace("post", "");
                                replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_StartedFollowing) + " " + split;
                            }
                            else
                                replace = item.ActivityText.Replace("started following", ActivityContext.GetString(Resource.String.Lbl_StartedFollowing));
                        }
                        else if (item.ActivityText.Contains("become friends with"))
                        {
                            if (UserDetails.LangName.Contains("fr"))
                            {
                                var split = item.ActivityText.Split("become friends with").Last().Replace("post", "");
                                replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_BecomeFriendsWith) + " " + ActivityContext.GetString(Resource.String.Lbl_Post) + " " + split;
                            }
                            else
                                replace = item.ActivityText.Replace("become friends with", ActivityContext.GetString(Resource.String.Lbl_BecomeFriendsWith));
                        }
                        else if (item.ActivityText.Contains("is following"))
                        {
                            if (UserDetails.LangName.Contains("fr"))
                            {
                                var split = item.ActivityText.Split("is following").Last().Replace("post", "");
                                replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_IsFollowing) + " " + split;
                            }
                            else
                                replace = item.ActivityText.Replace("is following", ActivityContext.GetString(Resource.String.Lbl_IsFollowing));
                        }

                        break;
                    }
                    case "liked_post":
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.emoji_like);
                  
                        if (UserDetails.LangName.Contains("fr"))
                        {
                            var split = item.ActivityText.Split("liked").Last().Replace("post", "");
                            replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Btn_Liked) + " " + ActivityContext.GetString(Resource.String.Lbl_Post) + " " + split;
                        }
                        else
                            replace = item.ActivityText.Replace("liked", ActivityContext.GetString(Resource.String.Btn_Liked)).Replace("post", ActivityContext.GetString(Resource.String.Lbl_Post));

                        break;
                    }
                    case "wondered_post":
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.ic_action_wowonder);
                        //holder.Icon.SetColorFilter(Color.ParseColor("#b71c1c"), PorterDuff.Mode.Multiply);

                        if (item.ActivityText.Contains("wondered"))
                        {
                            if (UserDetails.LangName.Contains("fr"))
                            {
                                var split = item.ActivityText.Split("wondered").Last().Replace("post", "");
                                replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_wondered) + " " + ActivityContext.GetString(Resource.String.Lbl_Post) + " " + split;
                            }
                            else
                                replace = item.ActivityText.Replace("wondered", ActivityContext.GetString(Resource.String.Lbl_wondered)).Replace("post", ActivityContext.GetString(Resource.String.Lbl_Post));
                        }
                        else if (item.ActivityText.Contains("disliked"))
                        {
                            if (UserDetails.LangName.Contains("fr"))
                            {
                                var split = item.ActivityText.Split("disliked").Last().Replace("post", "");
                                replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_disliked) + " " + ActivityContext.GetString(Resource.String.Lbl_Post) + " " + split;
                            }
                            else
                                replace = item.ActivityText.Replace("disliked", ActivityContext.GetString(Resource.String.Lbl_disliked)).Replace("post", ActivityContext.GetString(Resource.String.Lbl_Post));
                        }

                        break;
                    }
                    case "shared_post":
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.ic_action_share);
                        // holder.Icon.SetColorFilter(Color.ParseColor("#333333"), PorterDuff.Mode.Multiply);

                        if (UserDetails.LangName.Contains("fr"))
                        {
                            var split = item.ActivityText.Split("shared").Last().Replace("post", "");
                            replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_shared) + " " + ActivityContext.GetString(Resource.String.Lbl_Post) + " " + split;
                        }
                        else
                            replace = item.ActivityText.Replace("shared", ActivityContext.GetString(Resource.String.Lbl_shared)).Replace("post", ActivityContext.GetString(Resource.String.Lbl_Post));

                        break;
                    }
                    case "commented_post":
                    {
                        holder.Icon.SetImageResource(Resource.Drawable.ic_action_comment);
                        holder.Icon.SetImageResource(Resource.Drawable.ic_action_comments);
                        // holder.Icon.SetColorFilter(Color.ParseColor("#333333"), PorterDuff.Mode.Multiply);

                                if (UserDetails.LangName.Contains("fr"))
                        {
                            var split = item.ActivityText.Split("commented on").Last().Replace("post", "");
                            replace = item.Activator.Name + " " + ActivityContext.GetString(Resource.String.Lbl_CommentedOn) + " " + ActivityContext.GetString(Resource.String.Lbl_Post) + " " + split;
                        }
                        else
                        {
                            replace = item.ActivityText.Replace("commented on", ActivityContext.GetString(Resource.String.Lbl_CommentedOn)).Replace("post", ActivityContext.GetString(Resource.String.Lbl_Post));
                        }

                        break;
                    }
                }
                  
                holder.ActivitiesEvent.Text = !string.IsNullOrEmpty(replace) ? replace : item.ActivityText;
                
                holder.Time.Text = Methods.Time.TimeAgo(Convert.ToInt32(item.Time), false);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
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
                    case LastActivitiesAdapterViewHolder viewHolder:
                        Glide.With(ActivityContext).Clear(viewHolder.ActivitiesImage);
                        break;
                }
                base.OnViewRecycled(holder);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        public ActivityDataObject GetItem(int position)
        {
            return LastActivitiesList[position];
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


        private void Click(LastActivitiesAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(LastActivitiesAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }


        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = LastActivitiesList[p0];
                switch (item)
                {
                    case null:
                        return d;
                    default:
                    {
                        switch (string.IsNullOrEmpty(item.Activator.Avatar))
                        {
                            case false:
                                d.Add(item.Activator.Avatar);
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
            return GlideImageLoader.GetPreLoadRequestBuilder(ActivityContext, p0.ToString(), ImageStyle.CircleCrop);
        }


    }

    public class LastActivitiesAdapterViewHolder : RecyclerView.ViewHolder
    {
        public LastActivitiesAdapterViewHolder(View itemView,Action<LastActivitiesAdapterClickEventArgs> clickListener,Action<LastActivitiesAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                ActivitiesImage = MainView.FindViewById<CircleImageView>(Resource.Id.Image);
                ActivitiesEvent = MainView.FindViewById<TextView>(Resource.Id.LastActivitiesText);
                Icon = MainView.FindViewById<ImageView>(Resource.Id.ImageIcon);
                Time = MainView.FindViewById<TextView>(Resource.Id.Time);
                 
                //Create an Event
                itemView.Click += (sender, e) => clickListener(new LastActivitiesAdapterClickEventArgs{View = itemView, Position = AdapterPosition});
                itemView.LongClick += (sender, e) => longClickListener(new LastActivitiesAdapterClickEventArgs{View = itemView, Position = AdapterPosition});
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Variables Basic

        public View MainView { get; set; }

        public CircleImageView ActivitiesImage { get; private set; }
        public TextView ActivitiesEvent { get; private set; }
        public ImageView Icon { get; private set; }
        public TextView Time { get; private set; }

        #endregion
    }

    public class LastActivitiesAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
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
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using IList = System.Collections.IList;

namespace WoWonder.Activities.Search.Adapters
{
    public class SearchGroupAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<SearchGroupAdapterClickEventArgs> JoinButtonItemClick;
        public event EventHandler<SearchGroupAdapterClickEventArgs> ItemClick;
        public event EventHandler<SearchGroupAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;

        public ObservableCollection<GroupClass> GroupList = new ObservableCollection<GroupClass>();
        public SearchGroupAdapter(Activity context)
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

        public override int ItemCount => GroupList?.Count ?? 0;
 
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_HPage_view
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_HPage_view, parent, false);
                var vh = new SearchGroupAdapterViewHolder(itemView, JoinButtonClick, Click, LongClick);
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
                    case SearchGroupAdapterViewHolder holder:
                    {
                        var item = GroupList[position];
                        if (item != null)
                        {
                            Initialize(holder, item);
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

        private void Initialize(SearchGroupAdapterViewHolder holder, GroupClass item)
        {
            try
            {
                GlideImageLoader.LoadImage(ActivityContext, item.Avatar, holder.Image, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                   
                if (!string.IsNullOrEmpty(item.GroupTitle) || !string.IsNullOrWhiteSpace(item.GroupTitle))
                {
                    holder.Name.Text = Methods.FunString.SubStringCutOf(Methods.FunString.DecodeString(item.GroupTitle), 20);
                }
                else
                {
                    holder.Name.Text = Methods.FunString.SubStringCutOf(Methods.FunString.DecodeString(item.GroupName), 20);
                } 

                CategoriesController cat = new CategoriesController();
                holder.About.Text = cat.Get_Translate_Categories_Communities(item.CategoryId, item.Category , "Group");

                //var drawable = TextDrawable.InvokeBuilder().BeginConfig().FontSize(30).EndConfig().BuildRound("", Color.ParseColor("#1A237E"));

                if (item.IsOwner != null && item.IsOwner.Value || item.UserId == UserDetails.UserId)
                {
                    holder.Button.Visibility = ViewStates.Gone;
                }
                else
                {
                    //Set style Btn Joined Group 
                    if (WoWonderTools.IsJoinedGroup(item))
                    {
                        holder.Button.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                        holder.Button.SetTextColor(Color.ParseColor("#ffffff"));
                        holder.Button.Text = ActivityContext.GetText(Resource.String.Btn_Joined);
                        holder.Button.Tag = "true";
                    }
                    else
                    {
                        holder.Button.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                        holder.Button.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                        holder.Button.Text = ActivityContext.GetText(Resource.String.Btn_Join_Group);
                        holder.Button.Tag = "false";
                    }
                }
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
                    case SearchGroupAdapterViewHolder viewHolder:
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

        private void JoinButtonClick(SearchGroupAdapterClickEventArgs args)
        {
            JoinButtonItemClick?.Invoke(this, args);
        }

        private void Click(SearchGroupAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(SearchGroupAdapterClickEventArgs args)
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
            return GlideImageLoader.GetPreLoadRequestBuilder(ActivityContext, p0.ToString(), ImageStyle.CircleCrop);
        }


    }

    public class SearchGroupAdapterViewHolder : RecyclerView.ViewHolder
    {
        public SearchGroupAdapterViewHolder(View itemView, Action<SearchGroupAdapterClickEventArgs> joinButtonClickListener, Action<SearchGroupAdapterClickEventArgs> clickListener,Action<SearchGroupAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                Image = MainView.FindViewById<ImageView>(Resource.Id.Image);
                Name = MainView.FindViewById<TextView>(Resource.Id.card_name);
                About = MainView.FindViewById<TextView>(Resource.Id.card_dist);
                Button = MainView.FindViewById<Button>(Resource.Id.cont);
                IconGroup = MainView.FindViewById<ImageView>(Resource.Id.Icon);
                CircleView = MainView.FindViewById<View>(Resource.Id.image_view);

                CircleView.SetBackgroundResource(Resource.Drawable.circlegradient2);
                IconGroup.SetImageResource(Resource.Drawable.icon_social_group_vector);
                //Event 

                Button.Click += (sender, e) => joinButtonClickListener(new SearchGroupAdapterClickEventArgs{View = itemView, Position = AdapterPosition , Button = Button });
                itemView.Click += (sender, e) => clickListener(new SearchGroupAdapterClickEventArgs{View = itemView, Position = AdapterPosition});
                itemView.LongClick += (sender, e) => longClickListener(new SearchGroupAdapterClickEventArgs{View = itemView, Position = AdapterPosition});

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Variables Basic

        public View MainView { get; }
         
        public ImageView Image { get; private set; }
        public View CircleView { get; private set; }

        public TextView Name { get; private set; }
        public TextView About { get; private set; }
        public Button Button { get; private set; }
        public ImageView IconGroup { get; private set; }

        #endregion
    }

    public class SearchGroupAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
        public Button Button { get; set; }
    }
}
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
    public class SearchPageAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<SearchPageAdapterClickEventArgs> LikeButtonItemClick;
        public event EventHandler<SearchPageAdapterClickEventArgs> ItemClick;
        public event EventHandler<SearchPageAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;

        public ObservableCollection<PageClass> PageList = new ObservableCollection<PageClass>();

        public SearchPageAdapter(Activity context)
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

        public override int ItemCount => PageList?.Count ?? 0;
 
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_HPage_view
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_HPage_view, parent, false);
                var vh = new SearchPageAdapterViewHolder(itemView, LikeButtonClick, Click, LongClick);
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
                    case SearchPageAdapterViewHolder holder:
                    {
                        var item = PageList[position];
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

        private void Initialize(SearchPageAdapterViewHolder holder, PageClass item)
        {
            try
            {
                GlideImageLoader.LoadImage(ActivityContext, item.Avatar, holder.Image, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);

                if (!string.IsNullOrEmpty(item.PageTitle) || !string.IsNullOrWhiteSpace(item.PageTitle))
                    holder.Name.Text = Methods.FunString.SubStringCutOf(Methods.FunString.DecodeString(item.PageTitle), 20);
                else
                    holder.Name.Text = Methods.FunString.SubStringCutOf(Methods.FunString.DecodeString(item.PageName), 20);

                CategoriesController cat = new CategoriesController();
                holder.About.Text = cat.Get_Translate_Categories_Communities(item.PageCategory, item.Category, "Page");

                //var drawable = TextDrawable.InvokeBuilder().BeginConfig().FontSize(30).EndConfig().BuildRound("", Color.ParseColor("#BF360C"));
                //holder.ImageView.SetImageDrawable(drawable);

                if (item.IsPageOnwer != null && item.IsPageOnwer.Value || item.UserId == UserDetails.UserId)
                {
                    holder.Button.Visibility = ViewStates.Gone;
                }
                else
                {
                    //Set style Btn Like page 
                    if (WoWonderTools.IsLikedPage(item))
                    {
                        holder.Button.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                        holder.Button.SetTextColor(Color.ParseColor("#ffffff"));
                        holder.Button.Text = ActivityContext.GetText(Resource.String.Btn_Unlike);
                        holder.Button.Tag = "true";
                    }
                    else
                    {
                        holder.Button.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                        holder.Button.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                        holder.Button.Text = ActivityContext.GetText(Resource.String.Btn_Like);
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
                    case SearchPageAdapterViewHolder viewHolder:
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
        public PageClass GetItem(int position)
        {
            return PageList[position];
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

        private void LikeButtonClick(SearchPageAdapterClickEventArgs args)
        {
            LikeButtonItemClick?.Invoke(this, args);
        }

        private void Click(SearchPageAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(SearchPageAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }
         
        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = PageList[p0];
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

    public class SearchPageAdapterViewHolder : RecyclerView.ViewHolder
    {
        public SearchPageAdapterViewHolder(View itemView, Action<SearchPageAdapterClickEventArgs> likeButtonClickListener, Action<SearchPageAdapterClickEventArgs> clickListener,Action<SearchPageAdapterClickEventArgs> longClickListener) : base(itemView)
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

               //CircleView.SetBackgroundResource(Resource.Drawable.circlegradient3);
                IconGroup.SetImageResource(Resource.Drawable.icon_social_flag_vector);
              
                //Event      
                Button.Click += (sender, e) => likeButtonClickListener(new SearchPageAdapterClickEventArgs{View = itemView, Position = AdapterPosition , Button  = Button });
                itemView.Click += (sender, e) => clickListener(new SearchPageAdapterClickEventArgs{View = itemView, Position = AdapterPosition});
                itemView.LongClick += (sender, e) => longClickListener(new SearchPageAdapterClickEventArgs{View = itemView, Position = AdapterPosition}); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Variables Basic

        public View MainView { get; }

        
        public ImageView Image { get; set; }

        public TextView Name { get; set; }
        public TextView About { get; set; }
        public Button Button { get; set; }
        public ImageView IconGroup { get; set; }
        public View CircleView { get; set; }

        #endregion
    }

    public class SearchPageAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
        public Button Button { get; set; }
    }
}
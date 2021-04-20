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
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using IList = System.Collections.IList;

namespace WoWonder.Activities.Tabbes.Adapters
{
    public class ShortcutsAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        private readonly Activity ActivityContext;
        public ObservableCollection<Classes.ShortCuts> ShortcutsList = new ObservableCollection<Classes.ShortCuts>();

        public ShortcutsAdapter(Activity context)
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

        public override int ItemCount => ShortcutsList?.Count ?? 0;

        public event EventHandler<ShortcutsAdapterClickEventArgs> ItemClick;
        public event EventHandler<ShortcutsAdapterClickEventArgs> ItemLongClick;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_Pro_Users_view
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_PageCircle_view, parent, false);
                var vh = new ShortcutsAdapterViewHolder(itemView, Click, LongClick);
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
                    case ShortcutsAdapterViewHolder holder:
                    {
                        var item = ShortcutsList[position];
                        if (item != null)
                        {
                            switch (item.Type)
                            {
                                case "Page":
                                    GlideImageLoader.LoadImage(ActivityContext, item.PageClass.Avatar, holder.Image, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);
                                    holder.Name.Text = item.PageClass.Name;

                                    holder.ImageCircle.SetImageResource(Resource.Drawable.Orange_Color);
                                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, holder.IconPage, IonIconsFonts.IosFlag);
                                    break;
                                case "Group":
                                    GlideImageLoader.LoadImage(ActivityContext, item.GroupClass.Avatar, holder.Image, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);
                                    holder.Name.Text = item.GroupClass.Name;

                                    holder.ImageCircle.SetImageResource(Resource.Drawable.Blue_Color);
                                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, holder.IconPage, IonIconsFonts.IosPeople);
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
                    case ShortcutsAdapterViewHolder viewHolder:
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
        public Classes.ShortCuts GetItem(int position)
        {
            return ShortcutsList[position];
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

        private void Click(ShortcutsAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(ShortcutsAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }


        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = ShortcutsList[p0];
                switch (item)
                {
                    case null:
                        return d;
                    default:
                        switch (item.Type)
                        {
                            case "Page":
                                switch (string.IsNullOrEmpty(item.PageClass.Avatar))
                                {
                                    case false:
                                        d.Add(item.PageClass.Avatar);
                                        break;
                                }
                                break;
                            case "Group":
                                switch (string.IsNullOrEmpty(item.GroupClass.Avatar))
                                {
                                    case false:
                                        d.Add(item.GroupClass.Avatar);
                                        break;
                                }
                                break;
                        }
                     
                        return d;
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

    public class ShortcutsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ShortcutsAdapterViewHolder(View itemView, Action<ShortcutsAdapterClickEventArgs> clickListener, Action<ShortcutsAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                ImageCircle = MainView.FindViewById<CircleImageView>(Resource.Id.ImageCircle);
                Image = MainView.FindViewById<ImageView>(Resource.Id.Image);
                Name = MainView.FindViewById<TextView>(Resource.Id.Name);
                IconPage = MainView.FindViewById<TextView>(Resource.Id.Icon);
                 
                //Create an Event
                itemView.Click += (sender, e) => clickListener(new ShortcutsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new ShortcutsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Variables Basic

        public View MainView { get; }

        public CircleImageView ImageCircle { get; private set; } 
        public ImageView Image { get; private set; } 
        public TextView Name { get; private set; }
        public TextView IconPage { get; private set; }

        #endregion
    }

    public class ShortcutsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
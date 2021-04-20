using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.App;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Java.Util;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using IList = System.Collections.IList;

namespace WoWonder.Activities.Tabbes.Adapters
{
    public class ProPagesAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        
        public Activity ActivityContext;

        public ObservableCollection<PageClass> MProPagesList =
            new ObservableCollection<PageClass>();

        public ProPagesAdapter(Activity context)
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

        public override int ItemCount
        {
            get
            {
                if (MProPagesList != null)
                    return MProPagesList.Count;
                return 0;
            }
        }

        public event EventHandler<ProPagesAdapterClickEventArgs> ItemClick;
        public event EventHandler<ProPagesAdapterClickEventArgs> ItemLongClick;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_PageCircle_view
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_PageCircle_view, parent, false);
                var vh = new ProPagesAdapterViewHolder(itemView, Click, LongClick);
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
                    case ProPagesAdapterViewHolder holder:
                    {
                        var item = MProPagesList[position];
                        if (item != null)
                        {
                            //Dont Remove this code #####
                        
                            FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, holder.IconPage, IonIconsFonts.IosFlag);

                            GlideImageLoader.LoadImage(ActivityContext, item.Avatar, holder.Image, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                         
                            string name = Methods.FunString.DecodeString(item.PageName);
                            holder.Name.Text = name;
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
                    case ProPagesAdapterViewHolder viewHolder:
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
            return MProPagesList[position];
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

        private void Click(ProPagesAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(ProPagesAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }

        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = MProPagesList[p0];
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

    public class ProPagesAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ProPagesAdapterViewHolder(View itemView, Action<ProPagesAdapterClickEventArgs> clickListener,
            Action<ProPagesAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                MainView = itemView;

                Image = MainView.FindViewById<ImageView>(Resource.Id.Image);
                Name = MainView.FindViewById<TextView>(Resource.Id.Name);
                IconPage = MainView.FindViewById<TextView>(Resource.Id.Icon);


                //Create an Event
                itemView.Click += (sender, e) => clickListener(new ProPagesAdapterClickEventArgs
                    {View = itemView, Position = AdapterPosition});
                itemView.LongClick += (sender, e) => longClickListener(new ProPagesAdapterClickEventArgs
                    {View = itemView, Position = AdapterPosition});
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
        public TextView IconPage { get; set; }

        #endregion
    }

    public class ProPagesAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
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

namespace WoWonder.Activities.CommonThings.Adapters
{
    public class CommonThingsAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<CommonThingsAdapterClickEventArgs> ItemClick;
        public event EventHandler<CommonThingsAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;

        public ObservableCollection<CommonThingsObject> CommonThingsList = new ObservableCollection<CommonThingsObject>();

        public CommonThingsAdapter(Activity context)
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

        public override int ItemCount => CommonThingsList?.Count ?? 0;


        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_HPage_view
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_CommonThingsView, parent, false);
                var vh = new CommonThingsAdapterViewHolder(itemView, Click, LongClick);
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
                    case CommonThingsAdapterViewHolder holder:
                    {
                        var item = CommonThingsList[position];
                        if (item != null)
                        {
                            GlideImageLoader.LoadImage(ActivityContext, item.UserData.Avatar, holder.Image, ImageStyle.FitCenter, ImagePlaceholders.Drawable);

                            holder.Name.Text = WoWonderTools.GetNameFinal(item.UserData);
                            holder.CountCommon.Text = item.CommonThings + " " + ActivityContext.GetText(Resource.String.Lbl_ThingInCommon);
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
                    case CommonThingsAdapterViewHolder viewHolder:
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
        public CommonThingsObject GetItem(int position)
        {
            return CommonThingsList[position];
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

        private void Click(CommonThingsAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(CommonThingsAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }


        public RequestBuilder GetPreloadRequestBuilder(Java.Lang.Object p0)
        {
            return GlideImageLoader.GetPreLoadRequestBuilder(ActivityContext, p0.ToString(), ImageStyle.CircleCrop);
        }

        System.Collections.IList ListPreloader.IPreloadModelProvider.GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = CommonThingsList[p0];
                switch (item)
                {
                    case null:
                        return d;
                    default:
                    {
                        switch (string.IsNullOrEmpty(item.UserData.Avatar))
                        {
                            case false:
                                d.Add(item.UserData.Avatar);
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
    }

    public class CommonThingsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public CommonThingsAdapterViewHolder(View itemView, Action<CommonThingsAdapterClickEventArgs> clickListener, Action<CommonThingsAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                Image = MainView.FindViewById<ImageView>(Resource.Id.image);
                Name = MainView.FindViewById<TextView>(Resource.Id.name);
                Icon = MainView.FindViewById<TextView>(Resource.Id.icon);
                CountCommon = MainView.FindViewById<TextView>(Resource.Id.countCommon);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, Icon, FontAwesomeIcon.AmericanSignLanguageInterpreting);

                //Event  
                itemView.Click += (sender, e) => clickListener(new CommonThingsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new CommonThingsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });

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
        public TextView Icon { get; private set; } 
        public TextView CountCommon { get; private set; }

        #endregion
    }

    public class CommonThingsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
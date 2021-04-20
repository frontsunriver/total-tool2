using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.App;


using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide.Request;
using Java.IO;
using Java.Util;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Album;
using IList = System.Collections.IList;
using Object = Java.Lang.Object;

namespace WoWonder.Activities.Album.Adapters
{
    public class PhotosAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<PhotosAdapterClickEventArgs> ItemClick;
        public event EventHandler<PhotosAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext; 
        public ObservableCollection<PhotoAlbumObject> PhotosList = new ObservableCollection<PhotoAlbumObject>();

        public PhotosAdapter(Activity context)
        {
            try
            {
                ActivityContext = context;
                HasStableIds = true;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => PhotosList?.Count ?? 0;
         
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_ImageAlbum_view
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_ImageAlbum_view, parent, false);
                var vh = new PhotosAdapterViewHolder(itemView, Click, LongClick);
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
                    case PhotosAdapterViewHolder holder:
                    {
                        var item = PhotosList[position];
                        if (item != null)
                        {
                            switch (string.IsNullOrEmpty(item.Image))
                            {
                                case false when item.Image.Contains("file://") || item.Image.Contains("content://") || item.Image.Contains("storage") || item.Image.Contains("/data/user/0/"):
                                {
                                    File file2 = new File(item.Image);
                                    var photoUri = FileProvider.GetUriForFile(ActivityContext, ActivityContext.PackageName + ".fileprovider", file2);
                                    Glide.With(ActivityContext).Load(photoUri).Apply(new RequestOptions()).Into(holder.Image);
                                    break;
                                }
                                default:
                                    GlideImageLoader.LoadImage(ActivityContext, item.Image, holder.Image, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);
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
        public override void OnViewRecycled(Object holder)
        {
            try
            {
                switch (holder)
                {
                    case PhotosAdapterViewHolder viewHolder:
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
        public PhotoAlbumObject GetItem(int position)
        {
            return PhotosList[position];
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

        private void Click(PhotosAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(PhotosAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }


        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = PhotosList[p0];

                switch (item)
                {
                    case null:
                        return Collections.SingletonList(p0);
                }

                if (item.Image != "")
                {
                    d.Add(item.Image);
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
            return Glide.With(ActivityContext).Load(p0.ToString()).Apply(new RequestOptions().CenterCrop().SetDiskCacheStrategy(DiskCacheStrategy.All));
        }
    }

    public class PhotosAdapterViewHolder : RecyclerView.ViewHolder
    {
        public PhotosAdapterViewHolder(View itemView, Action<PhotosAdapterClickEventArgs> clickListener,Action<PhotosAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;
                Image = MainView.FindViewById<ImageView>(Resource.Id.image);

                //Event
                itemView.Click += (sender, e) => clickListener(new PhotosAdapterClickEventArgs{View = itemView, Position = AdapterPosition});
                itemView.LongClick += (sender, e) => longClickListener(new PhotosAdapterClickEventArgs{View = itemView, Position = AdapterPosition});
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #region Variables Basic

        public View MainView { get; }

        public ImageView Image { get; private set; }

        #endregion
    }

    public class PhotosAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
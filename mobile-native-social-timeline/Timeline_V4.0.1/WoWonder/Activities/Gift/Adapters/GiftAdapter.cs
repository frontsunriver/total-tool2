using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using IList = System.Collections.IList;
using Object = Java.Lang.Object;

namespace WoWonder.Activities.Gift.Adapters
{
    public class GiftAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    { 
        #region Variables Basic

        private readonly Activity ActivityContext;
        public ObservableCollection<GiftObject.DataGiftObject> GiftsList = new ObservableCollection<GiftObject.DataGiftObject>();
        public event EventHandler<GiftAdapterClickEventArgs> OnItemClick;
        public event EventHandler<GiftAdapterClickEventArgs> OnItemLongClick;
        #endregion

        public GiftAdapter(Activity context)
        {
            try
            {
                ActivityContext = context;
                HasStableIds = true;
                GiftsList = ListUtils.GiftsList;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => GiftsList?.Count ?? 0;
 
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_GiftView
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_Gif_View, parent, false);
                var vh = new GiftAdapterViewHolder(itemView, Click, LongClick);
                return vh;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
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
                    case GiftAdapterViewHolder holder:
                    {
                        var item = GiftsList[position];
                        if (item != null)
                        {
                            var imageSplit = item.MediaFile.Split('/').Last(); 
                            string getImage = Methods.MultiMedia.GetMediaFrom_Disk(Methods.Path.FolderDiskGif, imageSplit);
                            switch (getImage)
                            {
                                case "File Dont Exists":
                                    Glide.With(ActivityContext).Load(item.MediaFile).Apply(new RequestOptions().Placeholder(Resource.Drawable.ImagePlacholder)).Into(holder.ImgGift);
                                    Methods.MultiMedia.DownloadMediaTo_DiskAsync(Methods.Path.FolderDiskGif, item.MediaFile);
                                    break;
                                default:
                                {
                                    File file2 = new File(getImage);
                                    var photoUri = FileProvider.GetUriForFile(ActivityContext, ActivityContext.PackageName + ".fileprovider", file2);
                                    Glide.With(ActivityContext).Load(photoUri).Apply(new RequestOptions().Placeholder(Resource.Drawable.ImagePlacholder)).Into(holder.ImgGift);
                                    break;
                                }
                            } 
                        }

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
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
                     case GiftAdapterViewHolder viewHolder:
                         Glide.With(ActivityContext).Clear(viewHolder.ImgGift);
                         break;
                 }
                base.OnViewRecycled(holder);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        public GiftObject.DataGiftObject GetItem(int position)
        {
            return GiftsList[position];
        }

        public override long GetItemId(int position)
        {
            try
            {
                return position;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return 0;
            }
        }

        public override int GetItemViewType(int position)
        {
            try
            {
                return position;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return 0;
            }
        }

        public void Click(GiftAdapterClickEventArgs args)
        {
            OnItemClick?.Invoke(this, args);
        }

        public void LongClick(GiftAdapterClickEventArgs args)
        {
            OnItemLongClick?.Invoke(this, args);
        }

        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = GiftsList[p0];

                switch (item)
                {
                    case null:
                        return Collections.SingletonList(p0);
                }

                if (item.MediaFile != "")
                {
                    d.Add(item.MediaFile);
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
            return Glide.With(ActivityContext).Load(p0.ToString())
                .Apply(new RequestOptions().CenterCrop().SetDiskCacheStrategy(DiskCacheStrategy.All));
        }
    }

    public class GiftAdapterViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic

        public ImageView ImgGift { get; set; }
        public View MainView { get; }

        #endregion

        public GiftAdapterViewHolder(View itemView, Action<GiftAdapterClickEventArgs> clickListener,Action<GiftAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                ImgGift = MainView.FindViewById<ImageView>(Resource.Id.Image);

                itemView.Click += (sender, e) => clickListener(new GiftAdapterClickEventArgs{ View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new GiftAdapterClickEventArgs{ View = itemView, Position = AdapterPosition });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        } 
    }

    public class GiftAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
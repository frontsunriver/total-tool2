using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Android.App;
using Android.Content.Res;
using Android.Graphics; 
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using ImageViews.Rounded;
using Java.IO;
using Java.Util;
using Refractored.Controls;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Story;
using Console = System.Console;
using IList = System.Collections.IList;

namespace WoWonder.Activities.Story.Adapters
{
    public class StoryAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<StoryAdapterClickEventArgs> ItemClick;
        public event EventHandler<StoryAdapterClickEventArgs> ItemLongClick;
        private string YourImageUri;

        private readonly Activity ActivityContext;

        public ObservableCollection<StoryDataObject> StoryList = new ObservableCollection<StoryDataObject>();

        public StoryAdapter(Activity context)
        {
            try
            {
                HasStableIds = true;
                ActivityContext = context;

                var dataOwner = StoryList.FirstOrDefault(a => a.Type == "Your");
                switch (dataOwner)
                {
                    case null:
                        StoryList.Add(new StoryDataObject
                        {
                            Avatar = UserDetails.Avatar,
                            Type = "Your",
                            Username = context.GetText(Resource.String.Lbl_YourStory),
                            Stories = new List<StoryDataObject.Story>
                            {
                                new StoryDataObject.Story
                                {
                                    Thumbnail = UserDetails.Avatar,
                                }
                            }
                        });
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => StoryList?.Count ?? 0;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_Story_view
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_HStoryView, parent, false);
                var vh = new StoryAdapterViewHolder(itemView, Click, LongClick);
                return vh;
            }
            catch (Exception exception)
            {
                Console.WriteLine("EX:ALLEN >> " + exception);
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
                    case StoryAdapterViewHolder holder:
                        {
                            var item = StoryList[position];
                            if (item != null)
                            {
                                if (item.Stories?.Count == 1 && item.Stories[0]?.Posted == null)
                                    holder.StoryAvatar.Visibility = ViewStates.Gone;
                                else
                                    holder.SelfAvartar.Visibility = ViewStates.Gone;

                                switch (item.Stories?.Count)
                                {
                                    case > 0 when item.Stories[0].Thumbnail.Contains("http"):
                                        GlideImageLoader.LoadImage(ActivityContext, item.Stories[0]?.Thumbnail, holder.RoundImage, ImageStyle.RoundedCrop, ImagePlaceholders.Drawable);
                                        break;
                                    case > 0:
                                        Glide.With(ActivityContext).Load(new File(item.Stories[0].Thumbnail)).Apply(new RequestOptions().Placeholder(Resource.Drawable.ImagePlacholder).Error(Resource.Drawable.ImagePlacholder)).Into(holder.RoundImage);
                                        break;
                                }

                                switch (item.Type)
                                {
                                    case "Your":
                                        {
                                            holder.Circleindicator.Visibility = ViewStates.Gone;
                                            YourImageUri = item.Stories[0]?.Thumbnail;
                                            GlideImageLoader.LoadImage(ActivityContext, YourImageUri, holder.RoundImage, ImageStyle.RoundedCrop, ImagePlaceholders.Drawable);
                                            GlideImageLoader.LoadImage(ActivityContext, YourImageUri, holder.Image, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);

                                            break;
                                        } 
                                    case "Live":
                                        { 
                                            holder.Circleindicator.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(AppSettings.MainColor)); 
                                             
                                            GlideImageLoader.LoadImage(ActivityContext, item.Avatar , holder.RoundImage, ImageStyle.RoundedCrop, ImagePlaceholders.Drawable);
                                            GlideImageLoader.LoadImage(ActivityContext, item.Avatar, holder.Image, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);

                                            break;
                                        } 
                                    default:
                                        item.ProfileIndicator ??= AppSettings.MainColor;

                                        holder.Circleindicator.BackgroundTintList = ColorStateList.ValueOf(Color.ParseColor(item.ProfileIndicator)); // Default_Color 

                                        GlideImageLoader.LoadImage(ActivityContext, item.Avatar, holder.Image, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                                        break;
                                }
                                                               
                                holder.Name.Text = Methods.FunString.SubStringCutOf(WoWonderTools.GetNameFinal(item), 12);
                                 
                                if (item.DataLivePost != null && item.Type == "Live")
                                    holder.VideoStory.Visibility = ViewStates.Visible;
                                else  
                                    holder.VideoStory.Visibility = ViewStates.Gone;

                                switch (holder.Circleindicator.HasOnClickListeners)
                                {
                                    case false:
                                        holder.Circleindicator.Click += (sender, e) => Click(new StoryAdapterClickEventArgs { View = holder.MainView, Position = position });
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
                    case StoryAdapterViewHolder viewHolder:
                        //Glide.With(ActivityContext).Clear(viewHolder.Image);
                        break;
                }
                base.OnViewRecycled(holder);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        public StoryDataObject GetItem(int position)
        {
            return StoryList[position];
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

        private void Click(StoryAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(StoryAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }


        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = StoryList[p0];
                switch (item)
                {
                    case null:
                        return d;
                    default:
                        {
                            switch (string.IsNullOrEmpty(item.Stories[0].Thumbnail))
                            {
                                case false:
                                    d.Add(item.Stories[0].Thumbnail);
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

    public class StoryAdapterViewHolder : RecyclerView.ViewHolder
    {
        public StoryAdapterViewHolder(View itemView, Action<StoryAdapterClickEventArgs> clickListener, Action<StoryAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                RoundImage = MainView.FindViewById<RoundedImageView>(Resource.Id.iv_round_story);
                Image = MainView.FindViewById<CircleImageView>(Resource.Id.civ_story_avatar);
                Name = MainView.FindViewById<TextView>(Resource.Id.Txt_Username);
                Circleindicator = MainView.FindViewById<CircleImageView>(Resource.Id.profile_indicator);
                StoryAvatar = MainView.FindViewById<RelativeLayout>(Resource.Id.rl_story_avatar);
                SelfAvartar = MainView.FindViewById<RelativeLayout>(Resource.Id.rl_story_self);
                VideoStory = MainView.FindViewById<LinearLayout>(Resource.Id.ll_video_story);
                //CivStoryAvatar = MainView.FindViewById<CircleImageView>(Resource.Id.civ_story_avatar);

                //Event
                itemView.Click += (sender, e) => clickListener(new StoryAdapterClickEventArgs { View = itemView, Position = AdapterPosition });

                Console.WriteLine(longClickListener);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #region Variables Basic

        public View MainView { get; private set; }

        public RoundedImageView RoundImage { get; set; }
        public CircleImageView Image { get; set; }
        public TextView Name { get; private set; }
        public CircleImageView Circleindicator { get; private set; }

        public RelativeLayout StoryAvatar { get; private set; }
        public RelativeLayout SelfAvartar { get; private set; }
        public LinearLayout VideoStory { get; private set; }

        #endregion
    }

    public class StoryAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
using System;
using System.Collections.ObjectModel;
using Android.App;
using Android.Graphics;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;

namespace WoWonder.Activities.AddPost.Adapters
{
    public class MainPostAdapter : RecyclerView.Adapter
    {
        public ObservableCollection<Classes.PostType> PostTypeList = new ObservableCollection<Classes.PostType>();

        public MainPostAdapter(Activity activityContext)
        {
            try
            {
                switch (AppSettings.ShowGalleryImage)
                {
                    case true:
                        PostTypeList.Add(new Classes.PostType
                        {
                            Id = 1,
                            TypeText = activityContext.GetText(Resource.String.Lbl_ImageGallery),
                            Image = Resource.Drawable.icon_photos_vector,
                            ImageColor = ""
                        });
                        break;
                }

                switch (AppSettings.ShowGalleryVideo)
                {
                    case true when WoWonderTools.CheckAllowedFileSharingInServer("Video"):
                        PostTypeList.Add(new Classes.PostType
                        {
                            Id = 2,
                            TypeText = activityContext.GetText(Resource.String.Lbl_VideoGallery),
                            Image = Resource.Drawable.icon_video_gallary_vector,
                            ImageColor = "#00CF91"
                        });
                        break;
                }

                switch (AppSettings.ShowMention)
                {
                    case true:
                        PostTypeList.Add(new Classes.PostType
                        {
                            Id = 3,
                            TypeText = activityContext.GetText(Resource.String.Lbl_MentionContact),
                            Image = Resource.Drawable.icon_mention_contact_vector,
                            ImageColor = "#1776CD"
                        });
                        break;
                }

                switch (AppSettings.ShowLocation)
                {
                    case true:
                    {
                        var name = activityContext.GetText(Resource.String.Lbl_Location) + "/" +
                                   activityContext.GetText(Resource.String.Lbl_Place);
                        PostTypeList.Add(new Classes.PostType
                        {
                            Id = 4,
                            TypeText = name,
                            Image = Resource.Drawable.icon_map_marker_filled_vector,
                            ImageColor = "#F85C50"
                        });
                        break;
                    }
                }

                switch (AppSettings.ShowFeelingActivity)
                {
                    case true:
                    {
                        var name = activityContext.GetText(Resource.String.Lbl_Feeling) + "/" +
                                   activityContext.GetText(Resource.String.Lbl_Activity);

                        PostTypeList.Add(new Classes.PostType
                        {
                            Id = 5,
                            TypeText = name,
                            Image = Resource.Drawable.icon_smile_emoji_vector,
                            ImageColor = ""
                        });
                        break;
                    }
                }

                switch (AppSettings.ShowGif)
                {
                    //if (AppSettings.ShowCamera)
                    //    PostTypeList.Add(new Classes.PostType
                    //    {
                    //        Id = 6,
                    //        TypeText = activityContext.GetText(Resource.String.Lbl_Camera),
                    //        Image = Resource.Drawable.ic__Attach_video,
                    //        ImageColor = ""
                    //    });
                    case true:
                        PostTypeList.Add(new Classes.PostType
                        {
                            Id = 7,
                            TypeText = activityContext.GetText(Resource.String.Lbl_Gif),
                            Image = Resource.Drawable.icon_gif_vector,
                            ImageColor = "#A854A5"
                        });
                        break;
                }
                switch (AppSettings.ShowFile)
                {
                    case true when WoWonderTools.CheckAllowedFileSharingInServer("File"):
                        PostTypeList.Add(new Classes.PostType
                        {
                            Id = 8,
                            TypeText = activityContext.GetText(Resource.String.Lbl_File),
                            Image = Resource.Drawable.ic_attach_file,
                            ImageColor = ""
                        });
                        break;
                }
                switch (AppSettings.ShowMusic)
                {
                    case true when WoWonderTools.CheckAllowedFileSharingInServer("Audio"):
                        PostTypeList.Add(new Classes.PostType
                        {
                            Id = 9,
                            TypeText = activityContext.GetText(Resource.String.Lbl_Music),
                            Image = Resource.Drawable.ic_attach_music,
                            ImageColor = ""
                        });
                        break;
                }
                switch (AppSettings.ShowMusic)
                {
                    case true when WoWonderTools.CheckAllowedFileSharingInServer("Audio"):
                        PostTypeList.Add(new Classes.PostType
                        {
                            Id = 10,
                            TypeText = activityContext.GetText(Resource.String.Lbl_VoiceRecord),
                            Image = Resource.Drawable.ic_attach_microphone,
                            ImageColor = ""
                        });
                        break;
                }
                switch (AppSettings.ShowPolls)
                {
                    case true:
                        PostTypeList.Add(new Classes.PostType
                        {
                            Id = 11,
                            TypeText = activityContext.GetText(Resource.String.Lbl2_Polls),
                            Image = Resource.Drawable.icon_bar_polls_vector,
                            ImageColor = "#8CBA51"
                        });
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => PostTypeList?.Count ?? 0;
 
        public event EventHandler<MainPostAdapterClickEventArgs> ItemClick;
        public event EventHandler<MainPostAdapterClickEventArgs> ItemLongClick;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_AddPost_View
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_AddPost_View, parent, false);
                var vh = new MainPostAdapterViewHolder(itemView, Click, LongClick);
                return vh;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            try
            {
                switch (viewHolder)
                {
                    case MainPostAdapterViewHolder holder:
                    {
                        var item = PostTypeList[position];
                        if (item != null)
                        {
                            holder.PostTypeText.Text = item.TypeText;
                            holder.PostImageIcon.SetImageResource(item.Image);

                            switch (string.IsNullOrEmpty(item.ImageColor))
                            {
                                case false:
                                    holder.PostImageIcon.SetColorFilter(Color.ParseColor(item.ImageColor));
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

        public Classes.PostType GetItem(int position)
        {
            return PostTypeList[position];
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

        private void Click(MainPostAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(MainPostAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }
    }

    public class MainPostAdapterViewHolder : RecyclerView.ViewHolder
    {
        public MainPostAdapterViewHolder(View itemView, Action<MainPostAdapterClickEventArgs> clickListener,
            Action<MainPostAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                //Get values         
                PostTypeText = (TextView) MainView.FindViewById(Resource.Id.type_name);
                PostImageIcon = (ImageView) MainView.FindViewById(Resource.Id.Iconimage);

                //Create an Event
                itemView.Click += (sender, e) => clickListener(new MainPostAdapterClickEventArgs {View = itemView, Position = AdapterPosition});
                itemView.LongClick += (sender, e) => longClickListener(new MainPostAdapterClickEventArgs {View = itemView, Position = AdapterPosition});
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Variables Basic

        public View MainView { get; }


        public TextView PostTypeText { get; }
        public ImageView PostImageIcon { get; }

        #endregion
    }

    public class MainPostAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
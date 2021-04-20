using System;
using System.Collections.ObjectModel;
using Android.App;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Utils;

namespace WoWonder.Activities.AddPost.Adapters
{
    public class Feeling
    {
        public int Id { get; set; }
        public string FeelingName { get; set; }
        public string FeelingText { get; set; }
        public string FeelingImageUrl { get; set; }
        public bool Selected { get; set; } = false;
    }

    public class FeelingsAdapter : RecyclerView.Adapter
    {
        private readonly Activity ActivityContext;
        private readonly ObservableCollection<Feeling> FeelingsList = new ObservableCollection<Feeling>();

        public FeelingsAdapter(Activity context)
        {
            try
            {
                ActivityContext = context;

                FeelingsList.Add(new Feeling
                {
                    Id = 1,
                    FeelingName = "angry", 
                    FeelingText = ActivityContext.GetText(Resource.String.Lbl_Angry),
                    FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/1f621.png"
                });
                FeelingsList.Add(new Feeling
                {
                    Id = 2,
                    FeelingName = "funny",
                    FeelingText = ActivityContext.GetText(Resource.String.Lbl_Funny), FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/1f602.png"
                });
                FeelingsList.Add(new Feeling
                {
                    Id = 3,
                    FeelingName = "loved",
                    FeelingText = ActivityContext.GetText(Resource.String.Lbl_Loved), FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/1f60d.png"
                });
                FeelingsList.Add(new Feeling
                {
                    Id = 4, FeelingName = "cool", FeelingText = ActivityContext.GetText(Resource.String.Lbl_Cool),
                    FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/1f60e.png"
                });
                FeelingsList.Add(new Feeling
                {
                    Id = 5,
                    FeelingName = "happy",
                    FeelingText = ActivityContext.GetText(Resource.String.Lbl_Happy), FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/1f603.png"
                });
                FeelingsList.Add(new Feeling
                {
                    Id = 6,
                    FeelingName = "tired",
                    FeelingText = ActivityContext.GetText(Resource.String.Lbl_Tired), FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/1f62b.png"
                });
                FeelingsList.Add(new Feeling
                {
                    Id = 7,
                    FeelingName = "sleepy",
                    FeelingText = ActivityContext.GetText(Resource.String.Lbl_Sleepy), FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/1f634.png"
                });
                FeelingsList.Add(new Feeling
                {
                    Id = 8,
                    FeelingName = "expressionless",
                    FeelingText = ActivityContext.GetText(Resource.String.Lbl_Expressionless),FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/1f611.png"
                });
                FeelingsList.Add(new Feeling
                {
                    Id = 9,
                    FeelingName = "confused",
                    FeelingText = ActivityContext.GetText(Resource.String.Lbl_Confused), FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/1f615.png"
                });
                FeelingsList.Add(new Feeling
                {
                    Id = 10,
                    FeelingName = "shocked",
                    FeelingText = ActivityContext.GetText(Resource.String.Lbl_Shocked), FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/1f631.png"
                });
                FeelingsList.Add(new Feeling
                {
                    Id = 11,
                    FeelingName = "blessed",
                    FeelingText = ActivityContext.GetText(Resource.String.Lbl_VerySad),FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/1f62d.png"
                });
                FeelingsList.Add(new Feeling
                {
                    Id = 12,
                    FeelingName = "blessed",
                    FeelingText = ActivityContext.GetText(Resource.String.Lbl_Blessed), FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/1f607.png"
                });
                FeelingsList.Add(new Feeling
                {
                    Id = 13,
                    FeelingName = "bored",
                    FeelingText = ActivityContext.GetText(Resource.String.Lbl_Bored), FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/1f610.png"
                });
                FeelingsList.Add(new Feeling
                {
                    Id = 14,
                    FeelingName = "broke",
                    FeelingText = ActivityContext.GetText(Resource.String.Lbl_Broken), FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/1f494.png"
                });
                FeelingsList.Add(new Feeling
                {
                    Id = 15,
                    FeelingName = "lovely",
                    FeelingText = ActivityContext.GetText(Resource.String.Lbl_Lovely), FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/2665.png"
                });
                FeelingsList.Add(new Feeling
                {
                    Id = 16,
                    FeelingName = "smirk",
                    FeelingText = ActivityContext.GetText(Resource.String.Lbl_Hot), FeelingImageUrl = "https://abs.twimg.com/emoji/v1/72x72/1f60f.png"
                });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => FeelingsList?.Count ?? 0;
         

        public event EventHandler<FeelingsAdapterClickEventArgs> ItemClick;
        public event EventHandler<FeelingsAdapterClickEventArgs> ItemLongClick;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_Feeling_View
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_Feeling_View, parent, false);
                var vh = new FeelingsAdapterViewHolder(itemView, Click, LongClick);
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
                    case FeelingsAdapterViewHolder holder:
                    {
                        var item = FeelingsList[position];
                        if (item != null)
                        {
                            holder.FeelingName.Text = item.FeelingText;

                            switch (string.IsNullOrEmpty(item.FeelingImageUrl))
                            {
                                case false:
                                    GlideImageLoader.LoadImage(ActivityContext, item.FeelingImageUrl, holder.Image,ImageStyle.RoundedCrop ,ImagePlaceholders.Drawable);
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
                    case FeelingsAdapterViewHolder viewHolder:
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
        public Feeling GetItem(int position)
        {
            return FeelingsList[position];
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

        private void Click(FeelingsAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(FeelingsAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }
    }

    public class FeelingsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public FeelingsAdapterViewHolder(View itemView, Action<FeelingsAdapterClickEventArgs> clickListener,Action<FeelingsAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                //Get values         
                FeelingName = (TextView) MainView.FindViewById(Resource.Id.feelingName);
                Image = (ImageView) MainView.FindViewById(Resource.Id.Image);

                //Create an Event
                itemView.Click += (sender, e) => clickListener(new FeelingsAdapterClickEventArgs {View = itemView, Position = AdapterPosition});
                itemView.LongClick += (sender, e) => longClickListener(new FeelingsAdapterClickEventArgs {View = itemView, Position = AdapterPosition});
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Variables Basic

        public View MainView { get; }
        

        public TextView FeelingName { get; }
        public ImageView Image { get; }

        #endregion
    }

    public class FeelingsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
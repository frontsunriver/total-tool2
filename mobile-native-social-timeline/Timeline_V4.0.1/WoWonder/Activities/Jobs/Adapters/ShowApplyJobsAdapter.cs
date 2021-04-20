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
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonder.Library.Anjo.SuperTextLibrary;
using WoWonderClient.Classes.Jobs;
using String = Java.Lang.String;

namespace WoWonder.Activities.Jobs.Adapters
{
    public class ShowApplyJobsAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<ShowApplyJobsAdapterClickEventArgs> MessageButtonItemClick;
        public event EventHandler<ShowApplyJobsAdapterClickEventArgs> ItemClick;
        public event EventHandler<ShowApplyJobsAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext; 
        public ObservableCollection<JobDataObject> JobList = new ObservableCollection<JobDataObject>();
        private readonly StReadMoreOption ReadMoreOption;
        public ShowApplyJobsAdapter(Activity context)
        {
            try
            {
                HasStableIds = true;
                ActivityContext = context;
                ReadMoreOption = new StReadMoreOption.Builder()
                    .TextLength(200, StReadMoreOption.TypeCharacter)
                    .MoreLabel(context.GetText(Resource.String.Lbl_ReadMore))
                    .LessLabel(context.GetText(Resource.String.Lbl_ReadLess))
                    .MoreLabelColor(Color.ParseColor(AppSettings.MainColor))
                    .LessLabelColor(Color.ParseColor(AppSettings.MainColor))
                    .LabelUnderLine(true)
                    .Build();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => JobList?.Count ?? 0;
         
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_HPage_view
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_ApplyJobView, parent, false);
                var vh = new ShowApplyJobsAdapterViewHolder(itemView, MessageButtonClick, Click, LongClick);
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
                    case ShowApplyJobsAdapterViewHolder holder:
                    {
                        var item = JobList[position];
                        if (item != null)
                        {
                            GlideImageLoader.LoadImage(ActivityContext, item.UserData.Avatar, holder.Image, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);

                            holder.Username.Text = WoWonderTools.GetNameFinal(item.UserData);
                            holder.AddressText.Text = item.Position;
                            holder.TimeText.Text = item.Location;
                            holder.PhoneText.Text = item.PhoneNumber;
                            holder.EmailText.Text = item.Email;
                            holder.PositionText.Text = item.Position;
                            holder.StartDateText.Text = item.ExperienceStartDate;
                            holder.EndDateText.Text = item.ExperienceEndDate;
                         
                            holder.Description.Text = Methods.FunString.DecodeString(item.ExperienceDescription);
                            ReadMoreOption.AddReadMoreTo(holder.Description, new String(holder.Description.Text)); 
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
                    case ShowApplyJobsAdapterViewHolder viewHolder:
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
        public JobDataObject GetItem(int position)
        {
            return JobList[position];
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

        private void MessageButtonClick(ShowApplyJobsAdapterClickEventArgs args)
        {
            MessageButtonItemClick?.Invoke(this, args);
        }
        
        private void Click(ShowApplyJobsAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(ShowApplyJobsAdapterClickEventArgs args)
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
                var item = JobList[p0];
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

    public class ShowApplyJobsAdapterViewHolder : RecyclerView.ViewHolder
    { 
        #region Variables Basic

        public View MainView { get; private set; }

        public ImageView Image { get; private set; }
        public TextView Username { get; private set; }
        public TextView AddressText { get; private set; }
        private TextView AddressIcon { get; set; }
        private TextView TimeIcon { get; set; }
        public TextView TimeText { get; private set; }
        private TextView PhoneIcon { get; set; }
        public TextView PhoneText { get; private set; }
        private TextView EmailIcon { get; set; }
        public TextView EmailText { get; private set; }
        public TextView PositionText { get; private set; }
        public TextView StartDateText { get; private set; } 
        public TextView EndDateText { get; private set; } 
        public Button Button { get; private set; }
        public SuperTextView Description { get; private set; }

        #endregion

        public ShowApplyJobsAdapterViewHolder(View itemView, Action<ShowApplyJobsAdapterClickEventArgs> messageButtonClickListener, Action<ShowApplyJobsAdapterClickEventArgs> clickListener, Action<ShowApplyJobsAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                Image = MainView.FindViewById<ImageView>(Resource.Id.image);
                Username = MainView.FindViewById<TextView>(Resource.Id.Username);
                AddressIcon = MainView.FindViewById<TextView>(Resource.Id.addressIcon);
                AddressText = MainView.FindViewById<TextView>(Resource.Id.addressText);
                TimeIcon = MainView.FindViewById<TextView>(Resource.Id.timeIcon);
                TimeText = MainView.FindViewById<TextView>(Resource.Id.timeText);
                PhoneIcon = MainView.FindViewById<TextView>(Resource.Id.phoneIcon);
                PhoneText = MainView.FindViewById<TextView>(Resource.Id.phoneText);
                EmailIcon = MainView.FindViewById<TextView>(Resource.Id.emailIcon);
                EmailText = MainView.FindViewById<TextView>(Resource.Id.emailText);
                Button = MainView.FindViewById<Button>(Resource.Id.messageButton);
                PositionText = MainView.FindViewById<TextView>(Resource.Id.PositionText);
                StartDateText = MainView.FindViewById<TextView>(Resource.Id.startDateText);
                EndDateText = MainView.FindViewById<TextView>(Resource.Id.endDateText);
                Description = MainView.FindViewById<SuperTextView>(Resource.Id.description);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, AddressIcon, IonIconsFonts.Pin);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, TimeIcon, IonIconsFonts.IosTime);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, PhoneIcon, IonIconsFonts.Images);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmailIcon, IonIconsFonts.IosMail);

                //Event  
                Button.Click += (sender, e) => messageButtonClickListener(new ShowApplyJobsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.Click += (sender, e) => clickListener(new ShowApplyJobsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new ShowApplyJobsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });

               
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

      
    }

    public class ShowApplyJobsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
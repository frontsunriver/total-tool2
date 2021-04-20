using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Android.App;
using Android.Content;

using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Java.IO;
using Java.Util;
using Newtonsoft.Json;
using WoWonder.Activities.Jobs;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient;
using WoWonderClient.Classes.Global;
using Console = System.Console;
using Exception = System.Exception;

namespace WoWonder.Activities.NearbyBusiness.Adapters
{
    public class NearbyBusinessAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider 
    {
        public event EventHandler<NearbyBusinessAdapterClickEventArgs> ItemClick;
        public event EventHandler<NearbyBusinessAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;
        public ObservableCollection<NearbyBusinessesDataObject> NearbyBusinessList = new ObservableCollection<NearbyBusinessesDataObject>();

        public NearbyBusinessAdapter(Activity context)
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

        public override int ItemCount => NearbyBusinessList?.Count ?? 0;
         
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_JobView
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_JobView, parent, false);
                var vh = new NearbyBusinessAdapterViewHolder(itemView, Click, LongClick ,this);
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
                    case NearbyBusinessAdapterViewHolder holder:
                    {
                        var item = NearbyBusinessList[position];
                        if (item.Job?.JobInfoClass != null)
                        {
                            item.Job.Value.JobInfoClass.Image =
                                item.Job.Value.JobInfoClass.Image.Contains(Client.WebsiteUrl) switch
                                {
                                    false => WoWonderTools.GetTheFinalLink(item.Job?.JobInfoClass.Image),
                                    _ => item.Job.Value.JobInfoClass.Image
                                };

                            item.Job.Value.JobInfoClass.IsOwner = item.Job?.JobInfoClass.UserId == UserDetails.UserId;

                            if (item.Job.Value.JobInfoClass.Image.Contains("http"))
                            {
                                GlideImageLoader.LoadImage(ActivityContext, item.Job.Value.JobInfoClass.Image, holder.Image, ImageStyle.FitCenter, ImagePlaceholders.Drawable);
                            }
                            else
                            {
                                File file2 = new File(item.Job.Value.JobInfoClass.Image);
                                var photoUri = FileProvider.GetUriForFile(ActivityContext, ActivityContext.PackageName + ".fileprovider", file2);
                                Glide.With(ActivityContext).Load(photoUri).Apply(new RequestOptions()).Into(holder.Image);
                            }

                            holder.Title.Text = Methods.FunString.DecodeString(item.Job.Value.JobInfoClass.Title);

                            var (currency, currencyIcon) = WoWonderTools.GetCurrency(item.Job.Value.JobInfoClass.Currency);
                            var categoryName = CategoriesController.ListCategoriesJob.FirstOrDefault(categories => categories.CategoriesId == item.Job.Value.JobInfoClass.Category)?.CategoriesName;
                            Console.WriteLine(currency);
                            if (string.IsNullOrEmpty(categoryName))
                                categoryName = Application.Context.GetText(Resource.String.Lbl_Unknown);

                            holder.Salary.Text = item.Job.Value.JobInfoClass.Minimum + " " + currencyIcon + " - " + item.Job.Value.JobInfoClass.Maximum + " " + currencyIcon + " . " + categoryName;

                            holder.Description.Text = Methods.FunString.SubStringCutOf(Methods.FunString.DecodeString(item.Job.Value.JobInfoClass.Description), 100);

                            if (item.Job.Value.JobInfoClass.IsOwner != null && item.Job.Value.JobInfoClass.IsOwner.Value)
                            {
                                holder.IconMore.Visibility = ViewStates.Visible;
                                holder.Button.Text = ActivityContext.GetString(Resource.String.Lbl_show_applies) + " (" + item.Job.Value.JobInfoClass.ApplyCount + ")";
                                holder.Button.Tag = "ShowApply";
                            }
                            else
                            {
                                holder.IconMore.Visibility = ViewStates.Gone;
                            }

                            switch (item.Job.Value.JobInfoClass.Apply)
                            {
                                //Set Button if its applied
                                case "true":
                                    holder.Button.Text = ActivityContext.GetString(Resource.String.Lbl_already_applied);
                                    holder.Button.Enabled = false;
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
                    case NearbyBusinessAdapterViewHolder viewHolder:
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

        public NearbyBusinessesDataObject GetItem(int position)
        {
            return NearbyBusinessList[position];
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

        private void Click(NearbyBusinessAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(NearbyBusinessAdapterClickEventArgs args)
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
                var item = NearbyBusinessList[p0];
                switch (item)
                {
                    case null:
                        return d;
                    default:
                    {
                        if (item.Job != null && !string.IsNullOrEmpty(item.Job.Value.JobInfoClass.Image))
                            d.Add(item.Job.Value.JobInfoClass.Image);

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

    public class NearbyBusinessAdapterViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        #region Variables Basic

        private readonly NearbyBusinessAdapter NearbyBusinessAdapter;
        public View MainView { get; }

        public ImageView Image { get; private set; }
        public TextView Title { get; private set; }
        public TextView Salary { get; private set; }
        public TextView IconMore { get; private set; }
        public Button Button { get; private set; }
        public TextView Description { get; private set; }

        #endregion

        public NearbyBusinessAdapterViewHolder(View itemView, Action<NearbyBusinessAdapterClickEventArgs> clickListener, Action<NearbyBusinessAdapterClickEventArgs> longClickListener, NearbyBusinessAdapter nearbyBusinessAdapter) : base(itemView)
        {
            try
            {
                NearbyBusinessAdapter = nearbyBusinessAdapter; 
                MainView = itemView;

                Image = MainView.FindViewById<ImageView>(Resource.Id.JobCoverImage);
                Title = MainView.FindViewById<TextView>(Resource.Id.title);
                Salary = MainView.FindViewById<TextView>(Resource.Id.salary);
                Description = MainView.FindViewById<TextView>(Resource.Id.description);
                IconMore = MainView.FindViewById<TextView>(Resource.Id.iconMore);
                Button = MainView.FindViewById<Button>(Resource.Id.applyButton);
                Button.Tag = "Apply";

                IconMore.Visibility = ViewStates.Gone; 

                //Event  
                itemView.Click += (sender, e) => clickListener(new NearbyBusinessAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new NearbyBusinessAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        public void OnClick(View v)
        {
            if (AdapterPosition != RecyclerView.NoPosition)
            {
                var item = NearbyBusinessAdapter.NearbyBusinessList[AdapterPosition];

                if (v?.Id == Button?.Id)
                { 
                    switch (Button?.Tag?.ToString())
                    {
                        // Open Apply Job Activity 
                        case "ShowApply":
                        {
                            if (item.Job != null && item.Job.Value.JobInfoClass.ApplyCount == "0")
                            {
                                Toast.MakeText(MainView.Context, MainView.Context.GetString(Resource.String.Lbl_ThereAreNoRequests), ToastLength.Short)?.Show();
                                return;
                            }

                            var intent = new Intent(MainView.Context, typeof(ShowApplyJobActivity));
                            if (item.Job != null)
                                intent.PutExtra("JobsObject", JsonConvert.SerializeObject(item.Job.Value.JobInfoClass));
                            MainView.Context.StartActivity(intent);
                            break;
                        }
                        case "Apply":
                        {
                            var intent = new Intent(MainView.Context, typeof(ApplyJobActivity));
                            if (item.Job != null)
                                intent.PutExtra("JobsObject", JsonConvert.SerializeObject(item.Job.Value.JobInfoClass));
                            MainView.Context.StartActivity(intent);
                            break;
                        }
                    }
                } 
            }
        }

    }


    public class NearbyBusinessAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Content;


using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Java.IO;
using Java.Lang;
using Java.Util;
using Newtonsoft.Json;
using WoWonder.Activities.NativePost.Extra;
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonderClient;
using WoWonderClient.Classes.Jobs;
using WoWonderClient.Requests;
using Console = System.Console;
using Exception = System.Exception;

namespace WoWonder.Activities.Jobs.Adapters
{
    public class JobsAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback
    {
        public event EventHandler<JobsAdapterClickEventArgs> ItemClick;
        public event EventHandler<JobsAdapterClickEventArgs> ItemLongClick;

        public readonly Activity ActivityContext; 
        public ObservableCollection<JobInfoObject> JobList = new ObservableCollection<JobInfoObject>();
        public JobInfoObject DataInfoObject;
        public string DialogType;

        public JobsAdapter(Activity context)
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

        public override int ItemCount => JobList?.Count ?? 0;
         
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_JobView
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_JobView, parent, false);
                var vh = new JobsAdapterViewHolder(itemView, Click, LongClick , this);
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
                    case JobsAdapterViewHolder holder:
                    {
                        var item = JobList[position];
                        if (item != null)
                        { 
                            if (item.Image.Contains("http"))
                            {
                                var image = item.Image.Replace(Client.WebsiteUrl + "/", "");
                                item.Image = image.Contains("http") switch
                                {
                                    false => Client.WebsiteUrl + "/" + image,
                                    _ => image
                                };

                                GlideImageLoader.LoadImage(ActivityContext, item.Image, holder.Image, ImageStyle.FitCenter, ImagePlaceholders.Drawable);
                            }
                            else
                            {
                                File file2 = new File(item.Image);
                                var photoUri = FileProvider.GetUriForFile(ActivityContext, ActivityContext.PackageName + ".fileprovider", file2);
                                Glide.With(ActivityContext).Load(photoUri).Apply(new RequestOptions()).Into(holder.Image);
                            }
                       
                            holder.Title.Text = Methods.FunString.DecodeString(item.Title);

                            var (currency, currencyIcon) = WoWonderTools.GetCurrency(item.Currency);
                            var categoryName = CategoriesController.ListCategoriesJob.FirstOrDefault(categories => categories.CategoriesId == item.Category)?.CategoriesName;
                            Console.WriteLine(currency);
                            if (string.IsNullOrEmpty(categoryName))
                                categoryName = Application.Context.GetText(Resource.String.Lbl_Unknown);

                            holder.Salary.Text =  item.Minimum + " " + currencyIcon + " - " + item.Maximum + " " + currencyIcon + " . " + categoryName;

                            holder.Description.Text = Methods.FunString.SubStringCutOf(Methods.FunString.DecodeString(item.Description), 100);

                            if (item.IsOwner != null && item.IsOwner.Value)
                            {
                                holder.IconMore.Visibility = ViewStates.Visible;
                                holder.Button.Text = ActivityContext.GetString(Resource.String.Lbl_show_applies) + " (" + item.ApplyCount + ")";
                                holder.Button.Tag = "ShowApply";
                            }
                            else
                            {
                                holder.IconMore.Visibility = ViewStates.Gone;
                            }

                            switch (item.Apply)
                            {
                                //Set Button if its applied
                                case "true":
                                    holder.Button.Text = ActivityContext.GetString(Resource.String.Lbl_already_applied);
                                    holder.Button.Enabled = false;
                                    break;
                                default:
                                {
                                    if (item.Apply != "true" && item.Page.IsPageOnwer != null && !item.Page.IsPageOnwer.Value)
                                    {
                                        holder.Button.Text = ActivityContext.GetString(Resource.String.Lbl_apply_now);
                                        holder.Button.Tag = "Apply";
                                    }

                                    break;
                                }
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
                    case JobsAdapterViewHolder viewHolder:
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
        public JobInfoObject GetItem(int position)
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

        private void Click(JobsAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(JobsAdapterClickEventArgs args)
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
                        if (string.IsNullOrEmpty(item.Image)) return d;
                        if (item.Image.Contains("http"))
                        {
                            var image = item.Image.Replace(Client.WebsiteUrl + "/", "");
                            item.Image = image.Contains("http") switch
                            {
                                false => Client.WebsiteUrl + "/" + image,
                                _ => image
                            };

                            d.Add(item.Image);
                        }
                        else
                        {
                            d.Add(item.Image);
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

        public void OpenDialog()
        {
            try
            {
                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(ActivityContext).Theme(AppSettings.SetTabDarkTheme ? Theme.Dark : Theme.Light);

                arrayAdapter.Add(ActivityContext.GetText(Resource.String.Lbl_Edit));
                arrayAdapter.Add(ActivityContext.GetText(Resource.String.Lbl_Delete));

                dialogList.Title(ActivityContext.GetText(Resource.String.Lbl_More)).TitleColorRes(Resource.Color.primary);
                dialogList.Items(arrayAdapter);
                dialogList.NegativeText(ActivityContext.GetText(Resource.String.Lbl_Close)).OnNegative(this);
                dialogList.AlwaysCallSingleChoiceCallback();
                dialogList.ItemsCallback(this).Build().Show();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region MaterialDialog

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                string text = itemString.ToString();
                if (text == ActivityContext.GetText(Resource.String.Lbl_Edit))
                {
                    //Open Edit Job
                    var intent = new Intent(ActivityContext, typeof(EditJobsActivity));
                    intent.PutExtra("JobsObject", JsonConvert.SerializeObject(DataInfoObject));
                    ActivityContext.StartActivityForResult(intent, 246);
                }
                else if (text == ActivityContext.GetText(Resource.String.Lbl_Delete))
                {
                    DialogType = "Delete";

                    var dialog = new MaterialDialog.Builder(ActivityContext).Theme(AppSettings.SetTabDarkTheme ? Theme.Dark : Theme.Light);
                    dialog.Title(Resource.String.Lbl_Warning).TitleColorRes(Resource.Color.primary);
                    dialog.Content(ActivityContext.GetText(Resource.String.Lbl_DeleteJobs));
                    dialog.PositiveText(ActivityContext.GetText(Resource.String.Lbl_Yes)).OnPositive(this);
                    dialog.NegativeText(ActivityContext.GetText(Resource.String.Lbl_No)).OnNegative(this);
                    dialog.AlwaysCallSingleChoiceCallback();
                    dialog.ItemsCallback(this).Build().Show();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnClick(MaterialDialog p0, DialogAction p1)
        {
            try
            {
                switch (DialogType)
                {
                    case "Delete" when p1 == DialogAction.Positive:
                    {
                        // Send Api delete 

                        if (Methods.CheckConnectivity())
                        {
                            var adapterGlobal = WRecyclerView.GetInstance()?.NativeFeedAdapter;
                            var diff = adapterGlobal?.ListDiffer;
                            var dataGlobal = diff?.Where(a => a.PostData?.PostId == DataInfoObject?.PostId).ToList();
                            if (dataGlobal != null)
                            {
                                foreach (var postData in dataGlobal)
                                {
                                    WRecyclerView.GetInstance()?.RemoveByRowIndex(postData);
                                }
                            }

                            var recycler = TabbedMainActivity.GetInstance()?.NewsFeedTab?.MainRecyclerView;
                            var dataGlobal2 = recycler?.NativeFeedAdapter.ListDiffer?.Where(a => a.PostData?.PostId == DataInfoObject?.PostId).ToList();
                            if (dataGlobal2 != null)
                            {
                                foreach (var postData in dataGlobal2)
                                {
                                    recycler.RemoveByRowIndex(postData);
                                }
                            } 

                            var dataJob = JobList?.FirstOrDefault(a => a.Id == DataInfoObject.Id);
                            if (dataJob != null)
                            {
                                JobList.Remove(dataJob);
                                NotifyItemRemoved(JobsActivity.GetInstance().MAdapter.JobList.IndexOf(dataJob));
                            }

                            Toast.MakeText(ActivityContext, ActivityContext.GetText(Resource.String.Lbl_postSuccessfullyDeleted), ToastLength.Short)?.Show();
                            PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.PostActionsAsync(DataInfoObject.PostId, "delete") });
                        }
                        else
                        {
                            Toast.MakeText(ActivityContext, ActivityContext.GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                        }

                        break;
                    }
                    case "Delete":
                    {
                        if (p1 == DialogAction.Negative)
                        {
                            p0.Dismiss();
                        }

                        break;
                    }
                    default:
                    {
                        if (p1 == DialogAction.Positive)
                        {

                        }
                        else if (p1 == DialogAction.Negative)
                        {
                            p0.Dismiss();
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

        #endregion

    }

    public class JobsAdapterViewHolder : RecyclerView.ViewHolder , View.IOnClickListener
    {
        #region Variables Basic

        private readonly JobsAdapter JobsAdapter;
        public View MainView { get; }

        public ImageView Image { get; private set; }
        public TextView Title { get; private set; }
        public TextView Salary { get; private set; }
        public TextView IconMore { get; private set; }
        public Button Button { get; private set; }
        public TextView Description { get; private set; }

        #endregion
         
        public JobsAdapterViewHolder(View itemView, Action<JobsAdapterClickEventArgs> clickListener, Action<JobsAdapterClickEventArgs> longClickListener , JobsAdapter jobsAdapter  ) : base(itemView)
        {
            try
            {
                JobsAdapter = jobsAdapter;
                MainView = itemView;

                Image = MainView.FindViewById<ImageView>(Resource.Id.JobCoverImage);
                Title = MainView.FindViewById<TextView>(Resource.Id.title);
                Salary = MainView.FindViewById<TextView>(Resource.Id.salary);
                Description = MainView.FindViewById<TextView>(Resource.Id.description);
                IconMore = MainView.FindViewById<TextView>(Resource.Id.iconMore);
                Button = MainView.FindViewById<Button>(Resource.Id.applyButton);
                if (Button != null)
                {
                    Button.Tag = "Apply";
                    Button.SetOnClickListener(this);
                }
                 
                if (IconMore != null)
                {
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconMore, IonIconsFonts.More);
                    IconMore.SetOnClickListener(this);
                }

                //Event  
                itemView.Click += (sender, e) => clickListener(new JobsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new JobsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });  
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
                var item = JobsAdapter.JobList[AdapterPosition];

                if (v?.Id == Button?.Id)
                {
                    if (!Methods.CheckConnectivity())
                    {
                        Toast.MakeText(MainView.Context, MainView.Context?.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                        return;
                    }

                    switch (Button?.Tag?.ToString())
                    {
                        // Open Apply Job Activity 
                        case "ShowApply":
                            {
                                switch (item.ApplyCount)
                                {
                                    case "0":
                                        Toast.MakeText(JobsAdapter.ActivityContext, JobsAdapter.ActivityContext.GetString(Resource.String.Lbl_ThereAreNoRequests), ToastLength.Short)?.Show();
                                        return;
                                }

                                var intent = new Intent(JobsAdapter.ActivityContext, typeof(ShowApplyJobActivity));
                                intent.PutExtra("JobsObject", JsonConvert.SerializeObject(item));
                                JobsAdapter.ActivityContext.StartActivity(intent);
                                break;
                            }
                        case "Apply":
                            {
                                var intent = new Intent(JobsAdapter.ActivityContext, typeof(ApplyJobActivity));
                                intent.PutExtra("JobsObject", JsonConvert.SerializeObject(item));
                                JobsAdapter.ActivityContext.StartActivity(intent);
                                break;
                            }
                    }
                }
                else if (v?.Id == IconMore?.Id)
                {
                    try
                    {
                        JobsAdapter.DialogType = "More";
                        JobsAdapter.DataInfoObject = item;
                        JobsAdapter.OpenDialog();
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }
            }
        }
    }

    public class JobsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
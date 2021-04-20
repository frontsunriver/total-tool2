using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Load;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide.Load.Resource.Bitmap;
using Bumptech.Glide.Request;
using Java.Util;
using WoWonder.Activities.Communities.Groups;
using WoWonder.Activities.Communities.Pages;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Activities.UserProfile.Adapters;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Group;
using WoWonderClient.Requests;
using IList = System.Collections.IList;

namespace WoWonder.Activities.Communities.Adapters
{
    public class SocialAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        public event EventHandler<GroupsAdapterClickEventArgs> GroupItemClick;
        public event EventHandler<GroupsAdapterClickEventArgs> GroupItemLongClick;

        public event EventHandler<PageAdapterClickEventArgs> PageItemClick;
        public event EventHandler<PageAdapterClickEventArgs> PageItemLongClick;

        public readonly Activity ActivityContext;
        public ObservableCollection<SocialModelsClass> SocialList { get; private set; }
        private SocialModelType SocialPageType { get; set; }
        private RecyclerView.RecycledViewPool RecycledViewPool { get; set; }
        public UserGroupsAdapter GroupsAdapter { get; private set; }
        public UserPagesAdapter PagesAdapter { get; private set; }

        public SocialAdapter(Activity context, SocialModelType socialModelType)
        {
            try
            {
                HasStableIds = true;
                ActivityContext = context;
                SocialPageType = socialModelType;
                
                SocialList = new ObservableCollection<SocialModelsClass>(); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => SocialList?.Count ?? 0;
         
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            { 
                View itemView; 
                var item = SocialList[viewType]; 
                switch (item.TypeView)
                {
                    case SocialModelType.MangedGroups:
                    {
                        itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_HRecyclerView, parent, false);
                        var vh = new AdapterHolders.GroupsSocialViewHolder(ActivityContext, itemView, this);
                        RecycledViewPool = new RecyclerView.RecycledViewPool();
                        vh.GroupsRecyclerView.SetRecycledViewPool(RecycledViewPool); 
                        return vh;
                    }
                    case SocialModelType.JoinedGroups:
                    {
                        itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_GroupCircle_view, parent, false);
                        var vh = new GroupsAdapterViewHolder(itemView, GroupsOnClick, this);
                        return vh;
                    }
                    case SocialModelType.Section:
                    {
                        itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_Section, parent, false);
                        var vh = new AdapterHolders.SectionViewHolder(itemView);
                        return vh;
                    }
                    case SocialModelType.MangedPages:
                    {
                        itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_HRecyclerView, parent, false);
                        var vh = new AdapterHolders.PagesSocialViewHolder(ActivityContext, itemView , this);
                        RecycledViewPool = new RecyclerView.RecycledViewPool();
                        vh.PagesRecyclerView.SetRecycledViewPool(RecycledViewPool); 
                        return vh;
                    }
                    case SocialModelType.LikedPages:
                    {
                        itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_HPage_view, parent, false);
                        var vh = new PageAdapterViewHolder(itemView, PageOnClick, this);
                        return vh;
                    }
                    default:
                        return null!;
                } 
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
                var item = SocialList[viewHolder.AdapterPosition];
                if (item != null)
                {
                    switch (item.TypeView)
                    {
                        case SocialModelType.MangedGroups:
                        {
                            switch (viewHolder)
                            {
                                case AdapterHolders.GroupsSocialViewHolder holder:
                                {
                                    switch (GroupsAdapter)
                                    {
                                        case null:
                                            holder.GroupsRecyclerView?.SetLayoutManager(new LinearLayoutManager(ActivityContext, LinearLayoutManager.Horizontal, false));
                                            GroupsAdapter = new UserGroupsAdapter(ActivityContext)
                                            {
                                                GroupList = new ObservableCollection<GroupClass>()
                                            };
                                            holder.GroupsRecyclerView?.SetAdapter(GroupsAdapter);
                                            GroupsAdapter.ItemClick += GroupsAdapterOnItemClick;
                                            break;
                                    }

                                    var countList = item.MangedGroupsModel.GroupsList.Count;
                                    switch (item.MangedGroupsModel.GroupsList.Count)
                                    {
                                        case > 0 when countList > 0:
                                        {
                                            foreach (var user in from user in item.MangedGroupsModel.GroupsList let check = GroupsAdapter.GroupList.FirstOrDefault(a => a.GroupId == user.GroupId) where check == null select user)
                                            {
                                                GroupsAdapter.GroupList.Add(user);
                                            }

                                            GroupsAdapter.NotifyItemRangeInserted(countList - 1, GroupsAdapter.GroupList.Count - countList);
                                            break;
                                        }
                                        case > 0:
                                            GroupsAdapter.GroupList = new ObservableCollection<GroupClass>(item.MangedGroupsModel.GroupsList);
                                            GroupsAdapter.NotifyDataSetChanged();
                                            break;
                                    }

                                    holder.AboutHead.Text = item.MangedGroupsModel?.TitleHead;
                                    holder.AboutMore.Text = item.MangedGroupsModel?.More;
                                    holder.AboutMore.Visibility = GroupsAdapter?.GroupList?.Count >= 5 ? ViewStates.Visible : ViewStates.Invisible;
                                    break;
                                }
                            }

                            break;
                        }
                        case SocialModelType.JoinedGroups:
                        {
                            switch (viewHolder)
                            {
                                case GroupsAdapterViewHolder holder:
                                {
                                    var options = new RequestOptions();
                                    options.Transform(new MultiTransformation(new CenterCrop(), new RoundedCorners(110)));
                                    options.Error(Resource.Drawable.ImagePlacholder).Placeholder(Resource.Drawable.ImagePlacholder);

                                    GlideImageLoader.LoadImage(ActivityContext, item.GroupData.Avatar, holder.Image, ImageStyle.CenterCrop, ImagePlaceholders.Drawable, false, options);

                                    holder.Name.Text = Methods.FunString.DecodeString(item.GroupData.Name);
                                     
                                    if (WoWonderTools.IsJoinedGroup(item.GroupData))
                                    {
                                        holder.JoinButton.Text = ActivityContext.GetString(Resource.String.Btn_Joined);
                                        holder.JoinButton.Tag = "true";
                                    }
                                    else
                                    {
                                        holder.JoinButton.Text = ActivityContext.GetString(Resource.String.Btn_Join_Group);
                                        holder.JoinButton.Tag = "false";
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                        case SocialModelType.Section:
                        {
                            if (viewHolder is not AdapterHolders.SectionViewHolder holder)
                                return;

                            holder.AboutHead.Text = item.TitleHead;
                            break;
                        }
                        case SocialModelType.MangedPages:
                        {
                            switch (viewHolder)
                            {
                                case AdapterHolders.PagesSocialViewHolder holder:
                                {
                                    switch (PagesAdapter)
                                    {
                                        case null:
                                            holder.PagesRecyclerView?.SetLayoutManager(new LinearLayoutManager(ActivityContext, LinearLayoutManager.Horizontal, false));
                                            PagesAdapter = new UserPagesAdapter(ActivityContext)
                                            {
                                                PageList = new ObservableCollection<PageClass>()
                                            };
                                            holder.PagesRecyclerView?.SetAdapter(PagesAdapter);
                                            PagesAdapter.ItemClick += PagesAdapterOnItemClick;
                                            break;
                                    }

                                    var countList = item.PagesModelClass.PagesList.Count;
                                    switch (item.PagesModelClass.PagesList.Count)
                                    {
                                        case > 0 when countList > 0:
                                        {
                                            foreach (var user in from user in item.PagesModelClass.PagesList let check = PagesAdapter.PageList.FirstOrDefault(a => a.PageId == user.PageId) where check == null select user)
                                            {
                                                PagesAdapter.PageList.Add(user);
                                            }

                                            PagesAdapter.NotifyItemRangeInserted(countList - 1, PagesAdapter.PageList.Count - countList);
                                            break;
                                        }
                                        case > 0:
                                            PagesAdapter.PageList = new ObservableCollection<PageClass>(item.PagesModelClass.PagesList);
                                            PagesAdapter.NotifyDataSetChanged();
                                            break;
                                    }

                                    holder.AboutHead.Text = item.PagesModelClass?.TitleHead;
                                    holder.AboutMore.Text = item.PagesModelClass?.More;
                                    holder.AboutMore.Visibility = PagesAdapter?.PageList?.Count >= 5 ? ViewStates.Visible : ViewStates.Invisible;
                                    break;
                                }
                            }

                            break;
                        }
                        case SocialModelType.LikedPages:
                        {
                            switch (viewHolder)
                            {
                                case PageAdapterViewHolder holder:
                                {
                                    GlideImageLoader.LoadImage(ActivityContext, item.PageData.Avatar, holder.Image, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                             
                                    holder.About.Text = item.PageData.Category;

                                    if (!string.IsNullOrEmpty(item.PageData.PageTitle) || !string.IsNullOrWhiteSpace(item.PageData.PageTitle))
                                        holder.Name.Text = Methods.FunString.SubStringCutOf(Methods.FunString.DecodeString(item.PageData.PageTitle), 20);
                                    else
                                        holder.Name.Text = Methods.FunString.SubStringCutOf(Methods.FunString.DecodeString(item.PageData.PageName), 20);

                                    //Set style Btn Like page 
                                    if (WoWonderTools.IsLikedPage(item.PageData))
                                    { 
                                        holder.Button.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                                        holder.Button.SetTextColor(Color.ParseColor("#ffffff"));
                                        holder.Button.Text = ActivityContext.GetText(Resource.String.Btn_Unlike);
                                        holder.Button.Tag = "true";
                                    }
                                    else
                                    {
                                        holder.Button.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                                        holder.Button.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                                        holder.Button.Text = ActivityContext.GetText(Resource.String.Btn_Like);
                                        holder.Button.Tag = "false";
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                        case SocialModelType.Pages:
                            break;
                        case SocialModelType.Groups:
                            break; 
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void PagesAdapterOnItemClick(object sender, UserPagesAdapterClickEventArgs e)
        {
            try
            {
                var position = e.Position;
                switch (position)
                {
                    case < 0:
                        return;
                }

                var item = PagesAdapter.GetItem(position);
                switch (item)
                {
                    case null:
                        return;
                    default:
                        MainApplication.GetInstance()?.NavigateTo(ActivityContext, typeof(PageProfileActivity), item);
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
          
        }

        private void GroupsAdapterOnItemClick(object sender, UserGroupsAdapterClickEventArgs e)
        {
            try
            {
                var position = e.Position;
                switch (position)
                {
                    case < 0:
                        return;
                }

                var item = GroupsAdapter.GetItem(position);
                switch (item)
                {
                    case null:
                        return;
                }

                if (UserDetails.UserId == item.UserId)
                    item.IsOwner = true;

                //if (!string.IsNullOrEmpty(item.GroupsModel.UserProfileId) && UserDetails.UserId == item.GroupsModel.UserProfileId)
                //    group.IsJoined = "true";

                MainApplication.GetInstance()?.NavigateTo(ActivityContext, typeof(GroupProfileActivity), item);
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
                 
                if (holder != null)
                {
                    switch (holder)
                    {
                        case GroupsAdapterViewHolder viewHolder:
                            Glide.With(ActivityContext).Clear(viewHolder.Image);
                            break;
                        case PageAdapterViewHolder viewHolder2:
                            Glide.With(ActivityContext).Clear(viewHolder2.Image);
                            break;
                    }
                }
                base.OnViewRecycled(holder);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        public SocialModelsClass GetItem(int position)
        {
            return SocialList[position];
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

        private void GroupsOnClick(GroupsAdapterClickEventArgs args)
        {
            GroupItemClick?.Invoke(this, args);
        }

        private void GroupsOnLongClick(GroupsAdapterClickEventArgs args)
        {
            GroupItemLongClick?.Invoke(this, args);
        }

        private void PageOnClick(PageAdapterClickEventArgs args)
        {
            PageItemClick?.Invoke(this, args);
        }

        private void PageOnLongClick(PageAdapterClickEventArgs args)
        {
            PageItemLongClick?.Invoke(this, args);
        }
         
        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = SocialList[p0];

                switch (SocialPageType)
                {
                    case SocialModelType.Groups when item.GroupData == null:
                        return d;
                    case SocialModelType.Groups:
                    {
                        switch (string.IsNullOrEmpty(item.GroupData.Avatar))
                        {
                            case false:
                                d.Add(item.GroupData.Avatar);
                                break;
                        }

                        return d;
                    }
                    case SocialModelType.Pages when item.PageData == null:
                        return d;
                    case SocialModelType.Pages:
                    {
                        switch (string.IsNullOrEmpty(item.PageData.Avatar))
                        {
                            case false:
                                d.Add(item.PageData.Avatar);
                                break;
                        }

                        return d;
                    }
                    default:
                        return Collections.SingletonList(p0);
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
            return Glide.With(ActivityContext).Load(p0.ToString())
                .Apply(new RequestOptions().CenterCrop().SetDiskCacheStrategy(DiskCacheStrategy.All)); 
        } 
    }

    public class GroupsAdapterViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        private readonly SocialAdapter SocialAdapter;
        
        public View MainView { get; }
        public ImageView Image { get; private set; }
        public TextView Name { get; private set; }
        public TextView CountJoinedUsers { get; private set; }
        public TextView JoinButton { get; private set; }

        public GroupsAdapterViewHolder(View itemView, Action<GroupsAdapterClickEventArgs> clickListener, SocialAdapter socialAdapter) : base(itemView)
        {
            try
            {
                SocialAdapter = socialAdapter;
                MainView = itemView;
                
                Image = MainView.FindViewById<ImageView>(Resource.Id.Image);
                Name = MainView.FindViewById<TextView>(Resource.Id.groupName);
                CountJoinedUsers = MainView.FindViewById<TextView>(Resource.Id.groupUsers);
                JoinButton = MainView.FindViewById<TextView>(Resource.Id.groupButtonJoin);

                JoinButton?.SetOnClickListener(this);

                //Event
                itemView.Click += (sender, e) => clickListener(new GroupsAdapterClickEventArgs {View = itemView, Position = AdapterPosition});
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public async void OnClick(View v)
        {
            try
            {
                if (AdapterPosition != RecyclerView.NoPosition)
                {
                    var item = SocialAdapter.SocialList[AdapterPosition];

                    if (v.Id == JoinButton.Id)
                    {
                        if (!Methods.CheckConnectivity())
                        {
                            Toast.MakeText(SocialAdapter.ActivityContext, SocialAdapter.ActivityContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                            return;
                        }

                        var (apiStatus, respond) = await RequestsAsync.Group.JoinGroupAsync(item.GroupData.GroupId);
                        switch (apiStatus)
                        {
                            case 200:
                            {
                                switch (respond)
                                {
                                    case JoinGroupObject result when result.JoinStatus == "requested":
                                        JoinButton.SetTextColor(Color.Gray);
                                        JoinButton.Text = Application.Context.GetText(Resource.String.Lbl_Request);
                                        break;
                                    case JoinGroupObject result:
                                    {
                                        var isJoined = result.JoinStatus == "left" ? "false" : "true";
                                        JoinButton.Text = SocialAdapter.ActivityContext.GetText(isJoined == "yes" || isJoined == "true" ? Resource.String.Btn_Joined : Resource.String.Btn_Join_Group);

                                        switch (isJoined)
                                        {
                                            case "yes":
                                            case "true":
                                                JoinButton.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                                                break;
                                            default:
                                                JoinButton.SetTextColor(Color.Black);
                                                break;
                                        }

                                        break;
                                    }
                                }

                                break;
                            }
                            default:
                                Methods.DisplayReportResult(SocialAdapter.ActivityContext, respond);
                                break;
                        } 
                    }
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        } 
    }

    public class GroupsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }


    public class PageAdapterViewHolder : RecyclerView.ViewHolder , View.IOnClickListener
    {
        #region Variables Basic
        private readonly SocialAdapter SocialAdapter;

        public View MainView { get; }

        public ImageView Image { get; private set; }

        public TextView Name { get; private set; }
        public TextView About { get; private set; }
        public Button Button { get; private set; }

        #endregion

        public PageAdapterViewHolder(View itemView, Action<PageAdapterClickEventArgs> clickListener, SocialAdapter socialAdapter) : base(itemView)
        {
            try
            {
                SocialAdapter = socialAdapter;
            
                MainView = itemView;

                Image = MainView.FindViewById<ImageView>(Resource.Id.Image);
                Name = MainView.FindViewById<TextView>(Resource.Id.card_name);
                About = MainView.FindViewById<TextView>(Resource.Id.card_dist);
                Button = MainView.FindViewById<Button>(Resource.Id.cont);

                Button?.SetOnClickListener(this);

                //Event
                itemView.Click += (sender, e) => clickListener(new PageAdapterClickEventArgs{ View = itemView, Position = AdapterPosition });
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void OnClick(View v)
        {
            try
            {
                if (AdapterPosition != RecyclerView.NoPosition)
                {
                    var item = SocialAdapter.SocialList[AdapterPosition];

                    if (v.Id == Button.Id)
                    {
                        if (!Methods.CheckConnectivity())
                        {
                            Toast.MakeText(MainView.Context, MainView.Context?.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                            return;
                        }

                        switch (Button?.Tag?.ToString())
                        {
                            case "false":
                                Button.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                                Button.SetTextColor(Color.ParseColor("#ffffff"));
                                Button.Text = MainView.Context?.GetText(Resource.String.Btn_Unlike);
                                Button.Tag = "true";
                                break;
                            default:
                                Button.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                                Button.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                                Button.Text = MainView.Context?.GetText(Resource.String.Btn_Like);
                                Button.Tag = "false";
                                break;
                        }

                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Page.LikePageAsync(item.PageData.PageId) });
                    }
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }

    public class PageAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }

}
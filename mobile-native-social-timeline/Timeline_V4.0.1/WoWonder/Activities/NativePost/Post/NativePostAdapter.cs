using System;
using Android.App; 
using Android.Views;
using Bumptech.Glide;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide.Request;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using WoWonderClient.Classes.Posts;
using Android.Content;
using Android.Gms.Ads;
using Android.Gms.Ads.Formats;
using Bumptech.Glide.Util;
using WoWonder.Library.Anjo.IntegrationRecyclerView;
using Java.Lang;
using WoWonder.Activities.Comment;
using WoWonder.Activities.Comment.Adapters;
using WoWonder.Activities.MyProfile;
using WoWonder.Activities.NativePost.Pages;
using WoWonder.Activities.UserProfile;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.Library.Anjo.SuperTextLibrary;
using WoWonder.SQLite;
using Xamarin.Facebook.Ads;
using Console = System.Console;
using Exception = System.Exception;
using NativeAd = Xamarin.Facebook.Ads.NativeAd;
using Object = Java.Lang.Object;
using Android.OS;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide.Load.Resource.Bitmap;
using Bumptech.Glide.Signature;

namespace WoWonder.Activities.NativePost.Post
{ 
    public class NativePostAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider, StTools.IXAutoLinkOnClickListener, UnifiedNativeAd.IOnUnifiedNativeAdLoadedListener
    {
        public readonly Activity ActivityContext;

        private RecyclerView MainRecyclerView { get; }
        public NativeFeedType NativePostType { get; set; }
        public string IdParameter { get; private set; }

        public readonly RequestBuilder FullGlideRequestBuilder;
        public readonly RequestBuilder CircleGlideRequestBuilder;

        public List<AdapterModelsClass> ListDiffer { get; set; }
        public readonly List<PostDataObject> NewPostList = new List<PostDataObject>(); 
        private PreCachingLayoutManager PreCachingLayout { get; set; }

        private RecyclerView.RecycledViewPool RecycledViewPool { get; set; }

        private readonly PostClickListener PostClickListener;
        public AdapterHolders.StoryViewHolder HolderStory { get; private set; }
        public StReadMoreOption ReadMoreOption { get; }
        public readonly List<NativeAd> MAdItems;
        public NativeAdsManager MNativeAdsManager;
        //private IOnLoadMoreListener OnLoadMoreListener;
        //private bool Loading;
        public int PositionSound;
        private readonly AdapterBind AdapterBind;
        private int headerCount=0;

        public NativePostAdapter(Activity context, string apiIdParameter, RecyclerView recyclerView, NativeFeedType nativePostType)
        {
            try
            {
                HasStableIds = true;
                ActivityContext = context;
                NativePostType = nativePostType;
                MainRecyclerView = recyclerView;
                IdParameter = apiIdParameter;
                PostClickListener = new PostClickListener(ActivityContext , nativePostType);

                RecycledViewPool = new RecyclerView.RecycledViewPool();

                ReadMoreOption = new StReadMoreOption.Builder()
                     .TextLength(200, StReadMoreOption.TypeCharacter)
                     .MoreLabel(context.GetText(Resource.String.Lbl_ReadMore))
                     .LessLabel(context.GetText(Resource.String.Lbl_ReadLess))
                     .MoreLabelColor(Color.ParseColor(AppSettings.MainColor))
                     .LessLabelColor(Color.ParseColor(AppSettings.MainColor))
                     .LabelUnderLine(true)
                     .Build();

                MAdItems = new List<NativeAd>();
                BindAdFb();

                Glide.Get(context).SetMemoryCategory(MemoryCategory.Low);

                var glideRequestOptions = new RequestOptions().SetDiskCacheStrategy(DiskCacheStrategy.All).Placeholder(new ColorDrawable(Color.ParseColor("#EFEFEF"))).Error(Resource.Drawable.ImagePlacholder).Format(Bumptech.Glide.Load.DecodeFormat.PreferRgb565).Apply(RequestOptions.SignatureOf(new ObjectKey(DateTime.Now.Millisecond)));
                FullGlideRequestBuilder = Glide.With(context).AsBitmap().Downsample(DownsampleStrategy.AtMost).Apply(glideRequestOptions).Thumbnail(0.5f).Override(600).Timeout(3000).CenterInside().SetUseAnimationPool(false);
              
                var glideRequestOptions2 = new RequestOptions().SetDiskCacheStrategy(DiskCacheStrategy.All).Placeholder(Resource.Drawable.no_profile_image_circle).Error(Resource.Drawable.no_profile_image_circle).Format(Bumptech.Glide.Load.DecodeFormat.PreferRgb565).CircleCrop().Apply(RequestOptions.SignatureOf(new ObjectKey(DateTime.Now.Millisecond)));
                CircleGlideRequestBuilder = Glide.With(context).AsBitmap().Downsample(DownsampleStrategy.AtMost).Apply(glideRequestOptions2).Timeout(3000).SetUseAnimationPool(false);
              
                AdapterBind = new AdapterBind(this);

                NewPostList = new List<PostDataObject>(); 
                ListDiffer = new List<AdapterModelsClass>();
                PreCachingLayout = new PreCachingLayoutManager(ActivityContext)
                {
                    Orientation = LinearLayoutManager.Vertical
                };

                PreCachingLayout.SetPreloadItemCount(35);
                PreCachingLayout.AutoMeasureEnabled = false;
                PreCachingLayout.SetExtraLayoutSpace(2000);
                MainRecyclerView.SetLayoutManager(PreCachingLayout);
                MainRecyclerView.GetLayoutManager().ItemPrefetchEnabled = true;

                // var sizeProvider = new FixedPreloadSizeProvider(600, 350);
                var sizeProvider = new ViewPreloadSizeProvider();
                var preLoader = new RecyclerViewPreloader<AdapterModelsClass>(context, this, sizeProvider, 10);
                MainRecyclerView.AddOnScrollListener(preLoader);
                MainRecyclerView.SetAdapter(this);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                View itemView;
                switch (viewType)
                {
                    case (int)PostModelType.PromotePost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_PromoteSection_Layout, parent, false);
                            var vh = new AdapterHolders.PromoteHolder(itemView);

                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = PromotePost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.HeaderPost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_TopSection_Layout, parent, false);
                            var vh = new AdapterHolders.PostTopSectionViewHolder(itemView, this, PostClickListener);

                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = HeaderPost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.TextSectionPostPart:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_TextSection_Layout, parent, false);
                            var vh = new AdapterHolders.PostTextSectionViewHolder(itemView);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = TextSectionPostPart " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.BottomPostPart:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_ButtomSection_Layout, parent, false);
                            var vh = new AdapterHolders.PostBottomSectionViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  BottomPostPart " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.PrevBottomPostPart:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_PreButtomSection_Layout, parent, false);
                            var vh = new AdapterHolders.PostPrevBottomSectionViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = PrevBottomPostPart " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.AddCommentSection:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_AddComment_Section, parent, false);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  AddCommentSection " + viewType);
                            var vh = new AdapterHolders.PostAddCommentSectionViewHolder(itemView, this, PostClickListener);
                            return vh;
                        }
                    case (int)PostModelType.CommentSection:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_Comment_Section, parent, false);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  AddCommentSection " + viewType);
                            var vh = new CommentAdapterViewHolder(itemView , new CommentAdapter(ActivityContext), new CommentClickListener(ActivityContext , "Comment") , "Post" );
                            return vh;
                        }
                    case (int)PostModelType.Divider:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Devider, parent, false);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  Divider " + viewType);
                            var vh = new AdapterHolders.PostDividerSectionViewHolder(itemView);
                            return vh;
                        }
                    case (int)PostModelType.ImagePost:
                    case (int)PostModelType.StickerPost:
                    case (int)PostModelType.MapPost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_Image_Layout, parent, false);
                            var vh = new AdapterHolders.PostImageSectionViewHolder(itemView, this, PostClickListener, viewType);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = ImagePost" + viewType);
                            return vh;
                        }
                    case (int)PostModelType.MultiImage2:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_2Images_Layout, parent, false);
                            var vh = new AdapterHolders.Post2ImageSectionViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  MultiImage2 " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.MultiImage3:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_3Images_Layout, parent, false);
                            var vh = new AdapterHolders.Post3ImageSectionViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = MultiImage3 " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.MultiImage4:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_4Images_Layout, parent, false);
                            var vh = new AdapterHolders.Post4ImageSectionViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  MultiImage4 " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.VideoPost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_video_layout, parent, false);
                            var vh = new AdapterHolders.PostVideoSectionViewHolder(itemView, this);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = VideoPost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.BlogPost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_Blog_Layout, parent, false);
                            var vh = new AdapterHolders.PostBlogSectionViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = BlogPost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.ColorPost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_ColorBox_Layout, parent, false);
                            var vh = new AdapterHolders.PostColorBoxSectionViewHolder(itemView);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = ColorPost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.EventPost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_Event_Section, parent, false);
                            var vh = new AdapterHolders.EventPostViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = EventPost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.LinkPost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_Link_Layout, parent, false);
                            var vh = new AdapterHolders.LinkPostViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = LinkPost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.MultiImages:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_MultiImages_Layout, parent, false);
                            var vh = new AdapterHolders.PostMultiImagesViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = MultiImages" + viewType);
                            return vh;
                        }
                    case (int)PostModelType.FilePost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_File_Layout, parent, false);
                            var vh = new AdapterHolders.FilePostViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = FilePost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.PurpleFundPost:
                    case (int)PostModelType.FundingPost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_Funding_Layout, parent, false);
                            var vh = new AdapterHolders.FundingPostViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = FundingPost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.ProductPost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_Product_Layout, parent, false);
                            var vh = new AdapterHolders.ProductPostViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = ProductPost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.VoicePost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_Voice_Layout, parent, false);
                            var vh = new AdapterHolders.SoundPostViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = VoicePost " + viewType);
                            return vh; 
                        }
                    case (int)PostModelType.YoutubePost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_Youtube_Section, parent, false);
                            var vh = new AdapterHolders.YoutubePostViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = YoutubePost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.OfferPost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_Offer_Section, parent, false);
                            var vh = new AdapterHolders.OfferPostViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = OfferPost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.JobPostSection1:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_Job_Layout1, parent, false);
                            var vh = new AdapterHolders.JobPostViewHolder1(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = JobPostSection1 " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.JobPostSection2:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_Job_Layout2, parent, false);
                            var vh = new AdapterHolders.JobPostViewHolder2(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  JobPostSection2 " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.PollPost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_Poll_Section, parent, false);
                            var vh = new AdapterHolders.PollsPostViewHolder(itemView, this);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = PollPost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.SharedHeaderPost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_TopSectionShare_Layout, parent, false);
                            var vh = new AdapterHolders.PostTopSharedSectionViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = SharedHeaderPost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.AgoraLivePost:
                        { 
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_AgoraLive_Layout, parent, false);
                            var vh = new AdapterHolders.PostAgoraLiveViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = SharedHeaderPost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.LivePost:
                    case (int)PostModelType.DeepSoundPost:
                    case (int)PostModelType.VimeoPost:
                    case (int)PostModelType.FacebookPost:
                    case (int)PostModelType.PlayTubePost:
                    case (int)PostModelType.TikTokPost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_WebView_Layout, parent, false);
                            var vh = new AdapterHolders.PostPlayTubeContentViewHolder(itemView);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = WebView " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.AlertBox:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_Alert, parent, false);
                            var vh = new AdapterHolders.AlertAdapterViewHolder(itemView, this, PostModelType.AlertBox);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  AlertBox " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.AlertBoxAnnouncement:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_Announcement, parent, false);
                            var vh = new AdapterHolders.AlertAdapterViewHolder(itemView, this, PostModelType.AlertBoxAnnouncement);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  AlertBoxAnnouncement " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.AlertJoinBox:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_AlertJoin, parent, false);
                            var vh = new AdapterHolders.AlertJoinAdapterViewHolder(itemView, this);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = AlertJoinBox " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.Section:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_Section, parent, false);
                            var vh = new AdapterHolders.SectionViewHolder(itemView);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = Section " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.FilterSection:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_Section, parent, false);
                            var vh = new AdapterHolders.FilterSectionViewHolder(itemView);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = Section " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.AddPostBox:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_AddPost, parent, false);
                            var vh = new AdapterHolders.AddPostViewHolder(itemView, this);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  AddPostBox " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.SearchForPosts:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_SearchForPost, parent, false);
                            var vh = new AdapterHolders.SearchForPostsViewHolder(itemView, this);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = SearchForPosts" + viewType);
                            return vh;
                        }
                    case (int)PostModelType.SocialLinks:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_SociaLink, parent, false);
                            var vh = new AdapterHolders.SocialLinksViewHolder(itemView, this);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = SocialLinks " + viewType);
                            return vh;
                        } 
                    case (int)PostModelType.AboutBox:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_About, parent, false);
                            var vh = new AdapterHolders.AboutBoxViewHolder(itemView);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = AboutBox " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.InfoUserBox:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_InfoUser, parent, false);
                            var vh = new AdapterHolders.InfoUserBoxViewHolder(itemView, ActivityContext);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = InfoUserBox " + viewType);
                            return vh;
                        } 
                    case (int)PostModelType.InfoGroupBox:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_GroupPrivacy, parent, false);
                            var vh = new AdapterHolders.InfoGroupBoxViewHolder(itemView , this);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = InfoGroupBox " + viewType);
                            return vh;
                        }  
                    case (int)PostModelType.InfoPageBox:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_InfoPage, parent, false);
                            var vh = new AdapterHolders.InfoPageBoxViewHolder(itemView , this);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = InfoPageBox " + viewType);
                            return vh;
                        } 
                    case (int)PostModelType.Story:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_HRecyclerView, parent, false);
                            var vh = new AdapterHolders.StoryViewHolder(itemView, this, PostClickListener);
                            RecycledViewPool = new RecyclerView.RecycledViewPool();
                            vh.StoryRecyclerView.SetRecycledViewPool(RecycledViewPool);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  Story " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.FollowersBox:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_HRecyclerView, parent, false);
                            var vh = new AdapterHolders.FollowersViewHolder(itemView, this, PostClickListener);
                            RecycledViewPool = new RecyclerView.RecycledViewPool();
                            vh.FollowersRecyclerView.SetRecycledViewPool(RecycledViewPool);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.GroupsBox:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_HRecyclerView, parent, false);
                            var vh = new AdapterHolders.GroupsViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = GroupsBox " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.SuggestedGroupsBox:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_HRecyclerView, parent, false);
                            var vh = new AdapterHolders.SuggestedGroupsViewHolder(itemView, this);
                            RecycledViewPool = new RecyclerView.RecycledViewPool();
                            vh.GroupsRecyclerView.SetRecycledViewPool(RecycledViewPool);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.SuggestedUsersBox:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_HRecyclerView, parent, false);
                            var vh = new AdapterHolders.SuggestedUsersViewHolder(itemView, this);
                            RecycledViewPool = new RecyclerView.RecycledViewPool();
                            vh.UsersRecyclerView.SetRecycledViewPool(RecycledViewPool);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = SuggestedUsersBox " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.ImagesBox:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_HRecyclerView, parent, false);
                            var vh = new AdapterHolders.ImagesViewHolder(itemView, this, PostClickListener);
                            RecycledViewPool = new RecyclerView.RecycledViewPool();
                            vh.ImagesRecyclerView.SetRecycledViewPool(RecycledViewPool);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = ImagesBox " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.PagesBox:
                        {
                            //itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_Pages, parent, false);
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_HRecyclerView, parent, false);
                            var vh = new AdapterHolders.PagesViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  PagesBox" + viewType);
                            return vh;
                        }
                    case (int)PostModelType.AdsPost:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.PostType_Ads, parent, false);
                            var vh = new AdapterHolders.AdsPostViewHolder(itemView, this, PostClickListener);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = AdsPost " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.EmptyState:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_EmptyState, parent, false);
                            var vh = new AdapterHolders.EmptyStateAdapterViewHolder(itemView);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  EmptyState " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.AdMob1:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.PostType_AdMob, parent, false);
                            var vh = new AdapterHolders.AdMobAdapterViewHolder(itemView, this);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  AdMob " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.AdMob2:
                        { 
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.PostType_AdMob2, parent, false);
                            var vh = new AdapterHolders.AdMobAdapterViewHolder(itemView, this);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  AdMob " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.AdMob3:
                        { 
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.PostType_AdMob3, parent, false);
                            var vh = new AdapterHolders.AdMob3AdapterViewHolder(itemView);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType =  AdMob " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.FbAdNative:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.PostType_FbNativeAd, parent, false);
                            var vh = new AdapterHolders.FbAdNativeAdapterViewHolder(ActivityContext, itemView, this);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = FbAdNative " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.ViewProgress:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ItemProgressView, parent, false);
                            var vh = new AdapterHolders.ProgressViewHolder(itemView);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = ViewProgress " + viewType);
                            return vh;
                        }
                    case (int)PostModelType.ProfileHeaderSection:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.ViewModel_ProfileHeaderSection, parent, false);
                            var vh = new AdapterHolders.ProfileHeaderSectionHolder(itemView);
                            return vh;
                        }
                    default:
                        {
                            itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Post_Content_Null_Layout, parent, false);
                            var vh = new AdapterHolders.PostDefaultSectionViewHolder(itemView);
                            Console.WriteLine("WoLog: NativePostAdapter / OnCreateViewHolder  >>  PostModelType = default " + viewType);
                            return vh;
                        }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("EX:ALLEN PostAdapter >> " + exception);
                return null!;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position, IList<Object> payloads)
        {
            try
            {
                switch (payloads.Count)
                {
                    case > 0:
                    {
                        var item = ListDiffer[position];
                        switch (payloads[0].ToString())
                        {
                            case "StoryRefresh":
                            {
                                switch (viewHolder)
                                {
                                    case AdapterHolders.StoryViewHolder holder:
                                        holder.RefreshData();
                                        break;
                                }

                                break;
                            }
                            case "reaction":
                                switch (viewHolder)
                                {
                                    case AdapterHolders.PostPrevBottomSectionViewHolder holder: 
                                        AdapterBind.PrevBottomPostPartBind(holder, item); 
                                        break;
                                    case AdapterHolders.PostBottomSectionViewHolder holder2:
                                        AdapterBind.BottomPostPartBind(holder2, item);
                                        break;
                                }

                                break;
                            case "commentReplies":
                                switch (viewHolder)
                                {
                                    case AdapterHolders.PostPrevBottomSectionViewHolder holder:
                                        AdapterBind.PrevBottomPostPartBind(holder, item);
                                        break;
                                    case AdapterHolders.PostBottomSectionViewHolder holder2:
                                        AdapterBind.BottomPostPartBind(holder2, item);
                                        break;
                                    case CommentAdapterViewHolder holder:
                                        AdapterBind.CommentSectionBind(holder, item);
                                        break;
                                }

                                break;
                            case "BoostedPost":
                                switch (viewHolder)
                                {
                                    case AdapterHolders.PromoteHolder holder:
                                        AdapterBind.PromotePostBind(holder, item);
                                        break; 
                                }

                                break;
                            case "WithoutBlobAudio":
                                switch (viewHolder)
                                {
                                    case AdapterHolders.SoundPostViewHolder holder:

                                        holder.LoadingProgressView.Visibility = ViewStates.Gone;
                                        holder.PlayButton.Visibility = ViewStates.Visible;
                                        holder.PlayButton.SetImageResource(Resource.Drawable.icon_player_play);
                                        holder.PlayButton.Tag = "Play";

                                        switch (Build.VERSION.SdkInt)
                                        {
                                            case >= BuildVersionCodes.N:
                                                holder.SeekBar.SetProgress(0, true);
                                                break;
                                            // For API < 24 
                                            default:
                                                holder.SeekBar.Progress = 0;
                                                break;
                                        }
                                     
                                        break; 
                                }

                                break;
                            default:
                                base.OnBindViewHolder(viewHolder, position, payloads);
                                break;
                        }

                        break;
                    }
                    default:
                        base.OnBindViewHolder(viewHolder, position, payloads);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                base.OnBindViewHolder(viewHolder, position, payloads);
            }
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            try
            {
                AdapterModelsClass item = ListDiffer[position];
                var itemViewType = viewHolder.ItemViewType;
                switch (itemViewType)
                {
                    case (int)PostModelType.PromotePost:
                        {
                            if (viewHolder is not AdapterHolders.PromoteHolder holder)
                                return;

                            AdapterBind.PromotePostBind(holder, item);

                            break;
                        }
                    case (int)PostModelType.HeaderPost:
                        {
                            if (viewHolder is not AdapterHolders.PostTopSectionViewHolder holder)
                                return;

                            AdapterBind.HeaderPostBind(holder, item);

                            Console.WriteLine("WoLog: NativePostAdapter / OnBindViewHolder  >>  PostModelType = HeaderPost " + position);
                            break;
                        }
                    case (int)PostModelType.SharedHeaderPost:
                        {
                            if (viewHolder is not AdapterHolders.PostTopSharedSectionViewHolder holder)
                                return;

                            AdapterBind.SharedHeaderPostBind(holder, item);

                            break;
                        }
                    case (int)PostModelType.PrevBottomPostPart:
                        {
                            if (viewHolder is not AdapterHolders.PostPrevBottomSectionViewHolder holder)
                                return;

                            AdapterBind.PrevBottomPostPartBind(holder, item);

                            Console.WriteLine("WoLog: NativePostAdapter / OnBindViewHolder  >>  PostModelType = PrevBottomPostPart " + position);
                            break;
                        }
                    case (int)PostModelType.BottomPostPart:
                        {
                            if (viewHolder is not AdapterHolders.PostBottomSectionViewHolder holder)
                                return;

                            AdapterBind.BottomPostPartBind(holder, item);

                            Console.WriteLine("WoLog: NativePostAdapter / OnBindViewHolder  >>  PostModelType =  BottomPostPart " + position);
                            break;
                        }
                    case (int)PostModelType.TextSectionPostPart:
                        {
                            if (viewHolder is not AdapterHolders.PostTextSectionViewHolder holder)
                                return;

                            AdapterBind.TextSectionPostPartBind(holder, item);
                             
                            Console.WriteLine("WoLog: NativePostAdapter / OnBindViewHolder  >>  PostModelType =  TextSectionPostPart " + position);
                            break;
                        }
                    case (int)PostModelType.CommentSection:
                        {
                            if (viewHolder is not CommentAdapterViewHolder holder)
                                return;

                            AdapterBind.CommentSectionBind(holder, item);

                            break;
                        }
                    case (int)PostModelType.AddCommentSection:
                        {
                            if (viewHolder is not AdapterHolders.PostAddCommentSectionViewHolder holder)
                                return;

                            GlideImageLoader.LoadImage(ActivityContext, UserDetails.Avatar, holder.ProfileImageView, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);

                            break;
                        }
                    case (int)PostModelType.StickerPost:
                    case (int)PostModelType.ImagePost:
                        {
                            if (viewHolder is not AdapterHolders.PostImageSectionViewHolder holder)
                                return;

                            AdapterBind.ImagePostBind(holder, item); 

                            Console.WriteLine("WoLog: NativePostAdapter / OnBindViewHolder  >>  PostModelType =  " + position);
                            break;
                        }
                    case (int)PostModelType.MapPost:
                        {
                            if (viewHolder is not AdapterHolders.PostImageSectionViewHolder holder)
                                return;

                            FullGlideRequestBuilder.Load(item.PostData.ImageUrlMap).Into(holder.Image);

                            break;
                        }
                    case (int)PostModelType.MultiImage2:
                        {
                            if (viewHolder is not AdapterHolders.Post2ImageSectionViewHolder holder)
                                return;
                              
                            AdapterBind.MultiImage2Bind(holder, item); 
                            break;
                        }
                    case (int)PostModelType.MultiImage3:
                        {
                            if (viewHolder is not AdapterHolders.Post3ImageSectionViewHolder holder)
                                return;
                             
                            AdapterBind.MultiImage3Bind(holder, item);
                            break;
                        }
                    case (int)PostModelType.MultiImage4:
                        {
                            if (viewHolder is not AdapterHolders.Post4ImageSectionViewHolder holder)
                                return;

                            AdapterBind.MultiImage4Bind(holder, item);
                            break;
                        }
                    case (int)PostModelType.MultiImages:
                        {
                            if (viewHolder is not AdapterHolders.PostMultiImagesViewHolder holder)
                                return;

                            AdapterBind.MultiImagesBind(holder, item);
                            break; 
                        }
                    case (int)PostModelType.VideoPost:
                        {
                            if (viewHolder is not AdapterHolders.PostVideoSectionViewHolder holder)
                                return;

                            AdapterBind.VideoPostBind(holder, item); 
                            break;
                        }
                    case (int)PostModelType.BlogPost:
                        {
                            if (viewHolder is not AdapterHolders.PostBlogSectionViewHolder holder)
                                return;

                            AdapterBind.BlogPostBind(holder, item); 

                            break;
                        }
                    case (int)PostModelType.ColorPost:
                        {
                            if (viewHolder is not AdapterHolders.PostColorBoxSectionViewHolder holder)
                                return;

                            AdapterBind.ColorPostBind(holder, item);
                             
                            break;
                        }
                    case (int)PostModelType.EventPost:
                        {
                            if (viewHolder is not AdapterHolders.EventPostViewHolder holder)
                                return;

                            AdapterBind.EventPostBind(holder, item);
                            break;
                        }
                    case (int)PostModelType.LinkPost:
                        {
                            if (viewHolder is not AdapterHolders.LinkPostViewHolder holder)
                                return;

                            AdapterBind.LinkPostBind(holder, item); 
                            break;
                        }
                    case (int)PostModelType.FilePost:
                        {
                            if (viewHolder is not AdapterHolders.FilePostViewHolder holder)
                                return;

                            holder.PostFileText.Text = item.PostData.PostFileName;
                            break;
                        }
                    case (int)PostModelType.FundingPost:
                        {
                            if (viewHolder is not AdapterHolders.FundingPostViewHolder holder)
                                return;

                            AdapterBind.FundingPostBind(holder, item);
                            break;
                        } 
                    case (int)PostModelType.PurpleFundPost:
                        {
                            if (viewHolder is not AdapterHolders.FundingPostViewHolder holder)
                                return;

                            AdapterBind.PurpleFundPostBind(holder, item);
                            break;
                        }
                    case (int)PostModelType.ProductPost:
                        {
                            if (viewHolder is not AdapterHolders.ProductPostViewHolder holder)
                                return;

                            AdapterBind.ProductPostBind(holder, item); 
                            break;
                        }
                    case (int)PostModelType.VoicePost:
                        {
                            if (viewHolder is not AdapterHolders.SoundPostViewHolder holder)
                                return;

                            AdapterBind.VoicePostBind(holder, item); 
                            
                            Console.WriteLine(holder);
                            break;
                        }
                    case (int)PostModelType.AgoraLivePost:
                    {
                        if (viewHolder is not AdapterHolders.PostAgoraLiveViewHolder holder)
                            return;

                        AdapterBind.AgoraLivePostBind(holder, item);

                        break;
                    }
                    case (int)PostModelType.YoutubePost:
                        {
                            if (viewHolder is not AdapterHolders.YoutubePostViewHolder holder)
                                return;

                            FullGlideRequestBuilder.Load("https://img.youtube.com/vi/" + item.PostData.PostYoutube + "/0.jpg").Into(holder.Image);
                            break;
                        }
                    case (int)PostModelType.PlayTubePost:
                        {
                            if (viewHolder is not AdapterHolders.PostPlayTubeContentViewHolder holder)
                                return;

                            AdapterBind.WebViewPostBind(holder, item, PostModelType.PlayTubePost);
                            break;
                        }
                    case (int)PostModelType.TikTokPost:
                        {
                            if (viewHolder is not AdapterHolders.PostPlayTubeContentViewHolder holder)
                                return;

                            AdapterBind.WebViewPostBind(holder, item, PostModelType.TikTokPost);
                            break;
                        }
                    case (int)PostModelType.LivePost:
                        {
                            if (viewHolder is not AdapterHolders.PostPlayTubeContentViewHolder holder)
                                return;

                            AdapterBind.WebViewPostBind(holder, item, PostModelType.LivePost);

                            break;
                        } 
                    case (int)PostModelType.DeepSoundPost:
                        {
                            if (viewHolder is not AdapterHolders.PostPlayTubeContentViewHolder holder)
                                return;

                            AdapterBind.WebViewPostBind(holder, item, PostModelType.DeepSoundPost);

                            break;
                        }
                    case (int)PostModelType.VimeoPost:
                        {
                            if (viewHolder is not AdapterHolders.PostPlayTubeContentViewHolder holder)
                                return;

                            AdapterBind.WebViewPostBind(holder, item, PostModelType.VimeoPost);

                            break;
                        }
                    case (int)PostModelType.FacebookPost:
                        {
                            if (viewHolder is not AdapterHolders.PostPlayTubeContentViewHolder holder)
                                return;

                            AdapterBind.WebViewPostBind(holder, item, PostModelType.FacebookPost);
                            break;
                        }
                    case (int)PostModelType.OfferPost:
                        {
                            if (viewHolder is not AdapterHolders.OfferPostViewHolder holder)
                                return;

                            AdapterBind.OfferPostBind(holder, item); 

                            break;
                        }
                    case (int)PostModelType.JobPostSection1:
                        {
                            if (viewHolder is not AdapterHolders.JobPostViewHolder1 holder)
                                return;

                            AdapterBind.JobPostSection1Bind(holder, item);
                            break;
                        }
                    case (int)PostModelType.JobPostSection2:
                        {
                            if (viewHolder is not AdapterHolders.JobPostViewHolder2 holder)
                                return;

                            AdapterBind.JobPostSection2Bind(holder, item);

                            break;
                        }
                    case (int)PostModelType.PollPost:
                        {
                            if (viewHolder is not AdapterHolders.PollsPostViewHolder holder)
                                return;

                            AdapterBind.PollPostBind(holder, item);

                            break;
                        }
                    case (int)PostModelType.AlertBox:
                        {
                            if (viewHolder is not AdapterHolders.AlertAdapterViewHolder holder)
                                return;

                            AdapterBind.AlertBoxBind(holder, item , PostModelType.AlertBox);
                            break;
                        }
                    case (int)PostModelType.AlertBoxAnnouncement:
                        {
                            if (viewHolder is not AdapterHolders.AlertAdapterViewHolder holder)
                                return;

                            AdapterBind.AlertBoxBind(holder, item, PostModelType.AlertBoxAnnouncement);
                             
                            break;
                        }
                    case (int)PostModelType.AlertJoinBox:
                        {
                            if (viewHolder is not AdapterHolders.AlertJoinAdapterViewHolder holder)
                                return;

                            AdapterBind.AlertJoinBoxBind(holder, item);
                            break;
                        }
                    case (int)PostModelType.Section:
                        {
                            if (viewHolder is not AdapterHolders.SectionViewHolder holder)
                                return;

                            holder.AboutHead.Text = item.AboutModel.TitleHead;

                            break;
                        }  
                    case (int)PostModelType.FilterSection: 
                        {
                            if (viewHolder is not AdapterHolders.FilterSectionViewHolder holder)
                                return;

                            holder.AboutHead.Text = item.AboutModel.TitleHead;
                            holder.AboutMore.Text = ActivityContext.GetText(Resource.String.Lbl_Sort);

                            break;
                        }
                    case (int)PostModelType.AddPostBox:
                        {
                            if (viewHolder is not AdapterHolders.AddPostViewHolder holder)
                                return;

                            GlideImageLoader.LoadImage(ActivityContext, UserDetails.Avatar, holder.ProfileImageView, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);

                            break;
                        }
                    case (int)PostModelType.SearchForPosts:
                        {
                            if (viewHolder is not AdapterHolders.SearchForPostsViewHolder holder)
                                return;
                            Console.WriteLine(holder);
                            break;
                        }
                    case (int)PostModelType.SocialLinks:
                        {
                            if (viewHolder is not AdapterHolders.SocialLinksViewHolder holder)
                                return;
                            AdapterBind.SocialLinksBind(holder, item);

                            break;
                        }
                    case (int)PostModelType.AboutBox:
                        {
                            if (viewHolder is not AdapterHolders.AboutBoxViewHolder holder)
                                return;

                            AdapterBind.AboutBoxBind(holder, item);

                            break;
                        } 
                    case (int)PostModelType.InfoUserBox:
                        {
                            if (viewHolder is not AdapterHolders.InfoUserBoxViewHolder holder)
                                return;

                            AdapterBind.InfoUserBoxBind(holder, item);

                            break;
                        }
                    case (int)PostModelType.InfoGroupBox:
                        {
                            if (viewHolder is not AdapterHolders.InfoGroupBoxViewHolder holder)
                                return;

                            AdapterBind.InfoGroupBoxBind(holder, item);

                            break;
                        }
                    case (int)PostModelType.InfoPageBox:
                        {
                            if (viewHolder is not AdapterHolders.InfoPageBoxViewHolder holder)
                                return;

                            AdapterBind.InfoPageBoxBind(holder, item);

                            break;
                        }
                    case (int)PostModelType.Story:
                        {
                            if (viewHolder is not AdapterHolders.StoryViewHolder holder)
                                return;

                            HolderStory = holder; 
                            AdapterBind.StoryBind(holder, item);
                            break;
                        }
                    case (int)PostModelType.FollowersBox:
                        {
                            if (viewHolder is not AdapterHolders.FollowersViewHolder holder)
                                return;

                            AdapterBind.FollowersBoxBind(holder, item);
                            break;
                        }
                    case (int)PostModelType.GroupsBox:
                        {
                            if (viewHolder is not AdapterHolders.GroupsViewHolder holder)
                                return;
                             
                            AdapterBind.GroupsBoxBind(holder, item);
                            break;
                        }
                    case (int)PostModelType.SuggestedGroupsBox:
                        {
                            if (viewHolder is not AdapterHolders.SuggestedGroupsViewHolder holder)
                                return;

                            AdapterBind.SuggestedGroupsBoxBind(holder, item); 
                            break;
                        }
                    case (int)PostModelType.SuggestedUsersBox:
                        {
                            if (viewHolder is not AdapterHolders.SuggestedUsersViewHolder holder)
                                return;

                            AdapterBind.SuggestedUsersBoxBind(holder, item);

                            break;
                        }
                    case (int)PostModelType.ImagesBox:
                        {
                            if (viewHolder is not AdapterHolders.ImagesViewHolder holder)
                                return;

                            AdapterBind.ImagesBoxBind(holder, item);
                            break;
                        }
                    case (int)PostModelType.PagesBox:
                        {
                            if (viewHolder is not AdapterHolders.PagesViewHolder holder)
                                return;

                            AdapterBind.PagesBoxBind(holder, item);
                            //AdapterBind.GroupsBoxBind(holder, item);

                            break;
                        }
                    case (int)PostModelType.AdsPost:
                        {
                            if (viewHolder is not AdapterHolders.AdsPostViewHolder holder)
                                return;

                            AdapterBind.AdsPostBind(holder, item);
                            break;
                        }
                    case (int)PostModelType.EmptyState:
                        {
                            if (viewHolder is not AdapterHolders.EmptyStateAdapterViewHolder holder)
                                return;

                            BindEmptyState(holder);

                            break;
                        }
                    case (int)PostModelType.AdMob1:
                    case (int)PostModelType.AdMob2:
                    case (int)PostModelType.AdMob3:
                    case (int)PostModelType.FbAdNative:
                    case (int)PostModelType.Divider:
                        break;
                    case (int)PostModelType.ViewProgress:
                        {
                            if (viewHolder is not AdapterHolders.ProgressViewHolder holder)
                                return;
                            Console.WriteLine(holder);
                            break;
                        }
                    case (int)PostModelType.ProfileHeaderSection:
                        {
                            if (viewHolder is not AdapterHolders.ProfileHeaderSectionHolder holder)
                                return;
                            if (headerCount > 0)
                                return;
                            headerCount++;
                            AdapterBind.ProfileHeaderSeltionBind(holder, item);
                            break;
                        }
                    default:
                        {
                            if (viewHolder is not AdapterHolders.PostDefaultSectionViewHolder holder)
                                return;
                            Console.WriteLine(holder);
                            break;
                        }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception); 
            }
        }
         
        #region Progress On Scroll

        //public void SetOnLoadMoreListener(IOnLoadMoreListener onLoadMoreListener)
        //{
        //    OnLoadMoreListener = onLoadMoreListener;
        //}

        //public override void OnAttachedToRecyclerView(RecyclerView recyclerView)
        //{
        //    LastItemViewDetector(recyclerView);
        //    base.OnAttachedToRecyclerView(recyclerView);
        //}

        //private void LastItemViewDetector(RecyclerView recyclerView)
        //{
        //    try
        //    {
        //        if (recyclerView.GetLayoutManager() is LinearLayoutManager layoutManager)
        //        {
        //            recyclerView.AddOnScrollListener(new MyScroll(this, layoutManager));
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Methods.DisplayReportResultTrack(e);
        //    }
        //}

        //private class MyScroll : RecyclerView.OnScrollListener
        //{
        //    private readonly LinearLayoutManager LayoutManager;
        //    private readonly NativePostAdapter PostAdapter;
        //    public MyScroll(NativePostAdapter postAdapter, LinearLayoutManager layoutManager)
        //    {
        //        PostAdapter = postAdapter;
        //        LayoutManager = layoutManager;
        //    }
        //    public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        //    {
        //        try
        //        {
        //            base.OnScrolled(recyclerView, dx, dy);

        //            if (!PostAdapter.Loading && PostAdapter.ItemCount > 10)
        //            {
        //                if (LayoutManager != null && LayoutManager.FindLastCompletelyVisibleItemPosition() == PostAdapter.ItemCount - 2)
        //                {
        //                    //bottom of list!
        //                    int currentPage = PostAdapter.ItemCount / 5;
        //                    PostAdapter.OnLoadMoreListener.OnLoadMore(currentPage);
        //                    PostAdapter.Loading = true;
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Methods.DisplayReportResultTrack(e);
        //        }
        //    }
        //}

        public void SetLoading()
        {
            try
            {
                switch (ItemCount)
                {
                    case > 0:
                    {
                        var list = ListDiffer.FirstOrDefault(anjo => anjo.TypeView == PostModelType.ViewProgress);
                        switch (list)
                        {
                            case null:
                            {
                                var data = new AdapterModelsClass
                                {
                                    TypeView = PostModelType.ViewProgress,
                                    Progress = true,
                                };
                                ListDiffer.Add(data);
                                NotifyItemInserted(ListDiffer.IndexOf(data));
                                //Loading = true;
                                break;
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

        public void SetLoaded()
        {
            try
            {
                //Loading = false;
                //var list = ListDiffer.FirstOrDefault(anjo => anjo.TypeView == PostModelType.ViewProgress);
                //if (list != null)
                //{
                //    ListDiffer.Remove(list);
                //    NotifyItemRemoved(ListDiffer.IndexOf(list));
                //}
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        public override bool OnFailedToRecycleView(Object holder)
        { 
            return true;
        }

        public void BindAdFb()
        {
            try
            {
                switch (AppSettings.ShowFbNativeAds)
                {
                    case true when MAdItems?.Count == 0:
                        MNativeAdsManager = new NativeAdsManager(ActivityContext, AppSettings.AdsFbNativeKey, 5);
                        MNativeAdsManager.LoadAds();
                        MNativeAdsManager.SetListener(new MyNativeAdsManagerListener(this));
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void BindEmptyState(AdapterHolders.EmptyStateAdapterViewHolder holder)
        {
            try
            {
                holder.EmptyText.Text = NativePostType switch
                {
                    NativeFeedType.HashTag => ActivityContext.GetText(Resource.String.Lbl_NoPost_TitleText_hashtag),
                    NativeFeedType.Saved => ActivityContext.GetText(Resource.String.Lbl_NoPost_TitleText_saved),
                    NativeFeedType.Group => ActivityContext.GetText(Resource.String.Lbl_NoPost_TitleText_Group),
                    _ => ActivityContext.GetText(Resource.String.Lbl_NoPost_TitleText)
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public AdapterModelsClass GetItem(int position)
        {
            try
            {
                if (ListDiffer.Count > position)
                {
                    var item = ListDiffer[position];
                    return item;
                }
                return null;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int ItemCount => ListDiffer?.Count ?? 0;

        public override int GetItemViewType(int position)
        {
            try
            {
                var item = ListDiffer[position];

                return item switch
                {
                    null => (int)PostModelType.NormalPost,
                    _ => item.TypeView switch
                    {
                        PostModelType.SharedHeaderPost => (int) PostModelType.SharedHeaderPost,
                        PostModelType.HeaderPost => (int) PostModelType.HeaderPost,
                        PostModelType.TextSectionPostPart => (int) PostModelType.TextSectionPostPart,
                        PostModelType.PrevBottomPostPart => (int) PostModelType.PrevBottomPostPart,
                        PostModelType.BottomPostPart => (int) PostModelType.BottomPostPart,
                        PostModelType.Divider => (int) PostModelType.Divider,
                        PostModelType.AddCommentSection => (int) PostModelType.AddCommentSection,
                        PostModelType.CommentSection => (int) PostModelType.CommentSection,
                        PostModelType.AdsPost => (int) PostModelType.AdsPost,
                        PostModelType.AlertBoxAnnouncement => (int) PostModelType.AlertBoxAnnouncement,
                        PostModelType.AlertBox => (int) PostModelType.AlertBox,
                        PostModelType.AddPostBox => (int) PostModelType.AddPostBox,
                        PostModelType.SearchForPosts => (int) PostModelType.SearchForPosts,
                        PostModelType.SocialLinks => (int) PostModelType.SocialLinks,
                        PostModelType.VideoPost => (int) PostModelType.VideoPost,
                        PostModelType.AboutBox => (int) PostModelType.AboutBox,
                        PostModelType.InfoUserBox => (int) PostModelType.InfoUserBox,
                        PostModelType.BlogPost => (int) PostModelType.BlogPost,
                        PostModelType.AgoraLivePost => (int) PostModelType.AgoraLivePost,
                        PostModelType.LivePost => (int) PostModelType.LivePost,
                        PostModelType.DeepSoundPost => (int) PostModelType.DeepSoundPost,
                        PostModelType.EmptyState => (int) PostModelType.EmptyState,
                        PostModelType.FilePost => (int) PostModelType.FilePost,
                        PostModelType.MapPost => (int) PostModelType.MapPost,
                        PostModelType.FollowersBox => (int) PostModelType.FollowersBox,
                        PostModelType.GroupsBox => (int) PostModelType.GroupsBox,
                        PostModelType.SuggestedGroupsBox => (int) PostModelType.SuggestedGroupsBox,
                        PostModelType.SuggestedUsersBox => (int) PostModelType.SuggestedUsersBox,
                        PostModelType.ImagePost => (int) PostModelType.ImagePost,
                        PostModelType.ImagesBox => (int) PostModelType.ImagesBox,
                        PostModelType.LinkPost => (int) PostModelType.LinkPost,
                        PostModelType.PagesBox => (int) PostModelType.PagesBox,
                        PostModelType.PlayTubePost => (int) PostModelType.PlayTubePost,
                        PostModelType.ProductPost => (int) PostModelType.ProductPost,
                        PostModelType.StickerPost => (int) PostModelType.StickerPost,
                        PostModelType.Story => (int) PostModelType.Story,
                        PostModelType.VoicePost => (int) PostModelType.VoicePost,
                        PostModelType.YoutubePost => (int) PostModelType.YoutubePost,
                        PostModelType.Section => (int) PostModelType.Section,
                        PostModelType.FilterSection => (int) PostModelType.FilterSection,
                        PostModelType.AlertJoinBox => (int) PostModelType.AlertJoinBox,
                        PostModelType.SharedPost => (int) PostModelType.SharedPost,
                        PostModelType.EventPost => (int) PostModelType.EventPost,
                        PostModelType.ColorPost => (int) PostModelType.ColorPost,
                        PostModelType.FacebookPost => (int) PostModelType.FacebookPost,
                        PostModelType.VimeoPost => (int) PostModelType.VimeoPost,
                        PostModelType.MultiImage2 => (int) PostModelType.MultiImage2,
                        PostModelType.MultiImage3 => (int) PostModelType.MultiImage3,
                        PostModelType.MultiImage4 => (int) PostModelType.MultiImage4,
                        PostModelType.MultiImages => (int) PostModelType.MultiImages,
                        PostModelType.JobPostSection1 => (int) PostModelType.JobPostSection1,
                        PostModelType.JobPostSection2 => (int) PostModelType.JobPostSection2,
                        PostModelType.FundingPost => (int) PostModelType.FundingPost,
                        PostModelType.PurpleFundPost => (int) PostModelType.PurpleFundPost,
                        PostModelType.PollPost => (int) PostModelType.PollPost,
                        PostModelType.AdMob1 => (int) PostModelType.AdMob1,
                        PostModelType.AdMob2 => (int) PostModelType.AdMob2,
                        PostModelType.AdMob3 => (int) PostModelType.AdMob3,
                        PostModelType.FbAdNative => (int) PostModelType.FbAdNative,
                        PostModelType.OfferPost => (int) PostModelType.OfferPost,
                        PostModelType.ViewProgress => (int) PostModelType.ViewProgress,
                        PostModelType.PromotePost => (int) PostModelType.PromotePost,
                        PostModelType.NormalPost => (int) PostModelType.NormalPost,
                        PostModelType.InfoPageBox => (int) PostModelType.InfoPageBox,
                        PostModelType.InfoGroupBox => (int) PostModelType.InfoGroupBox,
                        PostModelType.TikTokPost => (int) PostModelType.TikTokPost,
                        PostModelType.ProfileHeaderSection=> (int)PostModelType.ProfileHeaderSection,
                        _ => (int) PostModelType.NormalPost
                    }
                };
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return (int)PostModelType.NormalPost;
            }
        }

        public override void OnViewRecycled(Object holder)
        {
            try
            {
                if (ActivityContext?.IsDestroyed != false)
                    return;

                if (holder != null)
                {
                    switch (holder)
                    {
                        case AdapterHolders.PostImageSectionViewHolder viewHolder:
                            Glide.With(ActivityContext).Clear(viewHolder.Image);
                            viewHolder.Image.SetImageDrawable(null);
                            break;
                        case AdapterHolders.PostTopSectionViewHolder viewHolder2:
                            Glide.With(ActivityContext).Clear(viewHolder2.UserAvatar);
                            break;
                        case AdapterHolders.EventPostViewHolder viewHolder3:
                            Glide.With(ActivityContext).Clear(viewHolder3.Image);
                            break;
                        case AdapterHolders.ProductPostViewHolder viewHolder4:
                            Glide.With(ActivityContext).Clear(viewHolder4.Image);
                            break;
                        case AdapterHolders.OfferPostViewHolder viewHolder5:
                            Glide.With(ActivityContext).Clear(viewHolder5.ImageBlog);
                            break;
                        case AdapterHolders.PostBlogSectionViewHolder viewHolder6:
                            Glide.With(ActivityContext).Clear(viewHolder6.ImageBlog);
                            break;
                        case AdapterHolders.YoutubePostViewHolder viewHolder7:
                            Glide.With(ActivityContext).Clear(viewHolder7.Image);
                            break;
                        case AdapterHolders.PostVideoSectionViewHolder viewHolder8:
                            Glide.With(ActivityContext).Clear(viewHolder8.VideoImage);
                            break;
                        case AdapterHolders.FundingPostViewHolder viewHolder9:
                            Glide.With(ActivityContext).Clear(viewHolder9.Image);
                            break;
                        case AdapterHolders.LinkPostViewHolder viewHolder10:
                            Glide.With(ActivityContext).Clear(viewHolder10.Image);
                            break;
                        case AdapterHolders.PostColorBoxSectionViewHolder viewHolder11:
                            Glide.With(ActivityContext).Clear(viewHolder11.ColorBoxImage);
                            break;
                        case AdapterHolders.PostAddCommentSectionViewHolder viewHolder20:
                            Glide.With(ActivityContext).Clear(viewHolder20.ProfileImageView);
                            break;
                        case CommentAdapterViewHolder viewHolder21:
                        {
                            if (viewHolder21.CommentImage != null)
                                Glide.With(ActivityContext).Clear(viewHolder21.CommentImage);
                            break;
                        }
                        case AdapterHolders.PostTopSharedSectionViewHolder viewHolder22:
                            Glide.With(ActivityContext).Clear(viewHolder22.UserAvatar);
                            break;
                        case AdapterHolders.PostPrevBottomSectionViewHolder viewHolder23:
                            Glide.With(ActivityContext).Clear(viewHolder23.ImageCountLike);
                            break;
                        case AdapterHolders.JobPostViewHolder1 viewHolder24:
                            Glide.With(ActivityContext).Clear(viewHolder24.JobAvatar);
                            Glide.With(ActivityContext).Clear(viewHolder24.JobCoverImage);
                            break;
                        case AdapterHolders.PostMultiImagesViewHolder viewHolder12:
                            Glide.With(ActivityContext).Clear(viewHolder12.Image);
                            Glide.With(ActivityContext).Clear(viewHolder12.Image2);
                            Glide.With(ActivityContext).Clear(viewHolder12.Image3);
                            viewHolder12.Image.SetImageDrawable(null);
                            viewHolder12.Image2.SetImageDrawable(null);
                            viewHolder12.Image3.SetImageDrawable(null);
                            break;
                        case AdapterHolders.Post2ImageSectionViewHolder viewHolder13:
                            Glide.With(ActivityContext).Clear(viewHolder13.Image);
                            Glide.With(ActivityContext).Clear(viewHolder13.Image2);
                            viewHolder13.Image.SetImageDrawable(null);
                            viewHolder13.Image2.SetImageDrawable(null);
                            break;
                        case AdapterHolders.Post3ImageSectionViewHolder viewHolder14:
                            Glide.With(ActivityContext).Clear(viewHolder14.Image);
                            Glide.With(ActivityContext).Clear(viewHolder14.Image2);
                            Glide.With(ActivityContext).Clear(viewHolder14.Image3);
                            viewHolder14.Image.SetImageDrawable(null);
                            viewHolder14.Image2.SetImageDrawable(null);
                            viewHolder14.Image3.SetImageDrawable(null);
                            break;
                        case AdapterHolders.Post4ImageSectionViewHolder viewHolder15:
                            Glide.With(ActivityContext).Clear(viewHolder15.Image);
                            Glide.With(ActivityContext).Clear(viewHolder15.Image2);
                            Glide.With(ActivityContext).Clear(viewHolder15.Image3);
                            Glide.With(ActivityContext).Clear(viewHolder15.Image4);
                            viewHolder15.Image.SetImageDrawable(null);
                            viewHolder15.Image2.SetImageDrawable(null);
                            viewHolder15.Image3.SetImageDrawable(null);
                            viewHolder15.Image4.SetImageDrawable(null);
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

        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();

                var item = GetItem(p0);
                switch (item?.PostData)
                {
                    case null:
                        return d;
                }

                switch (item.PostData.PhotoMulti?.Count)
                {
                    case > 0:
                        d.AddRange(from photo in item.PostData.PhotoMulti where !string.IsNullOrEmpty(photo.Image) select photo.Image);
                        break;
                }

                switch (item.PostData.PhotoAlbum?.Count)
                {
                    case > 0:
                        d.AddRange(from photo in item.PostData.PhotoAlbum where !string.IsNullOrEmpty(photo.Image) select photo.Image);
                        break;
                }

                if (item.PostData.ColorId != "0")
                {
                    if (ListUtils.SettingsSiteList?.PostColors != null && ListUtils.SettingsSiteList?.PostColors.Value.PostColorsList != null)
                    {
                        var getColorObject = ListUtils.SettingsSiteList.PostColors.Value.PostColorsList.FirstOrDefault(a => a.Key == item.PostData.ColorId);
                        if (getColorObject.Value != null)
                        {
                            switch (string.IsNullOrEmpty(getColorObject.Value.Image))
                            {
                                case false:
                                    d.Add(getColorObject.Value.Image);
                                    break;
                            }
                        }
                    }
                }

                if (item.PostData.PostSticker != null && !string.IsNullOrEmpty(item.PostData.PostSticker))
                    d.Add(item.PostData.PostSticker);

                switch (string.IsNullOrEmpty(item.PostData.PostLinkImage))
                {
                    case false:
                        d.Add(item.PostData.PostLinkImage); //+ "===" + p0);
                        break;
                }

                if (PostFunctions.GetImagesExtensions(item.PostData.PostFileFull))
                    d.Add(item.PostData.PostFileFull);// + "===" + p0);

                switch (string.IsNullOrEmpty(item.PostData.PostFileThumb))
                {
                    case false:
                        d.Add(item.PostData.PostFileThumb);
                        break;
                    default:
                    {
                        if (PostFunctions.GetVideosExtensions(item.PostData.PostFileFull))
                            d.Add(item.PostData.PostFileFull);
                        break;
                    }
                }

                switch (string.IsNullOrEmpty(item.PostData.Publisher?.Avatar))
                {
                    case false:
                        d.Add(item.PostData.Publisher.Avatar);
                        break;
                }

                switch (string.IsNullOrEmpty(item.PostData.PostYoutube))
                {
                    case false:
                        d.Add("https://img.youtube.com/vi/" + item.PostData.PostYoutube + "/0.jpg");
                        break;
                }

                if (item.PostData.Product?.ProductClass?.Images != null)
                    d.AddRange(from productImage in item.PostData.Product.Value.ProductClass?.Images select productImage.Image);

                switch (string.IsNullOrEmpty(item.PostData.Blog?.Thumbnail))
                {
                    case false:
                        d.Add(item.PostData.Blog?.Thumbnail);
                        break;
                }

                switch (string.IsNullOrEmpty(item.PostData.Event?.EventClass?.Cover))
                {
                    case false:
                        d.Add(item.PostData.Event.Value.EventClass?.Cover);
                        break;
                }

                switch (string.IsNullOrEmpty(item.PostData?.PostMap))
                {
                    case false:
                    {
                        switch (item.PostData.PostMap.Contains("https://maps.googleapis.com/maps/api/staticmap?"))
                        {
                            case false:
                            {
                                string imageUrlMap = "https://maps.googleapis.com/maps/api/staticmap?";
                                //imageUrlMap += "center=" + item.CurrentLatitude + "," + item.CurrentLongitude;
                                imageUrlMap += "center=" + item.PostData.PostMap.Replace("/", "");
                                imageUrlMap += "&zoom=10";
                                imageUrlMap += "&scale=1";
                                imageUrlMap += "&size=300x300";
                                imageUrlMap += "&maptype=roadmap";
                                imageUrlMap += "&key=" + ActivityContext.GetText(Resource.String.google_maps_key);
                                imageUrlMap += "&format=png";
                                imageUrlMap += "&visual_refresh=true";
                                imageUrlMap += "&markers=size:small|color:0xff0000|label:1|" + item.PostData.PostMap.Replace("/", "");

                                item.PostData.ImageUrlMap = imageUrlMap;
                                break;
                            }
                            default:
                                item.PostData.ImageUrlMap = item.PostData.PostMap;
                                break;
                        }

                        d.Add(item.PostData.ImageUrlMap);
                        break;
                    }
                }

                return d;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                var d = new List<string>();
                return d;
            }
        }

        //private readonly List<string> ImageCachedList = new List<string>();
        //private readonly List<string> ImageCircleCachedList = new List<string>();
        //private int Count;
        public RequestBuilder GetPreloadRequestBuilder(Object p0)
        {
            try
            {
                var url = p0.ToString();
                if (url.Contains("avatar") /*&& !ImageCircleCachedList.Contains(url)*/)
                {
                    // ImageCircleCachedList.Add(url);
                    return CircleGlideRequestBuilder.Load(url);
                }
                else
                {
                    //if (!ImageCachedList.Contains(url))
                    //{
                    //    ImageCachedList.Add(url);
                    return FullGlideRequestBuilder.Load(url);
                    //}
                }

                //var f = Count++;
                //Console.WriteLine("Preloaded ++ " + f + " ++++ " + p0);
                //return null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }

        }

        private TemplateView Template;
        public void BindAdMob(AdapterHolders.AdMobAdapterViewHolder holder)
        {
            try
            {
                Template = holder.MianAlert;
               
                AdLoader.Builder builder = new AdLoader.Builder(holder.MainView.Context, AppSettings.AdAdMobNativeKey);
                builder.ForUnifiedNativeAd(this);

                VideoOptions videoOptions = new VideoOptions.Builder()
                    .SetStartMuted(true)
                    .Build();

                NativeAdOptions adOptions = new NativeAdOptions.Builder()
                    .SetVideoOptions(videoOptions)
                    .Build();

                builder.WithNativeAdOptions(adOptions);

                AdLoader adLoader = builder.WithAdListener(new AdListener()).Build();
                adLoader.LoadAd(new AdRequest.Builder().Build());
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnUnifiedNativeAdLoaded(UnifiedNativeAd ad)
        {
            try
            { 
                if (Template.GetTemplateTypeName() == TemplateView.NativeContentAd)
                {
                    Template.NativeContentAdView(ad);
                } 
                else
                {
                    NativeTemplateStyle styles = new NativeTemplateStyle.Builder().Build();

                    Template.SetStyles(styles);
                    Template.SetNativeAd(ad);
                }
                 
                ActivityContext?.RunOnUiThread(() =>
                {
                    Template.Visibility = ViewStates.Visible;
                });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void AutoLinkTextClick(StTools.XAutoLinkMode p0, string p1, Dictionary<string, string> userData)
        {
            try
            {
                p1 = p1.Replace(" ", "").Replace("\n", "");
                var typeText = Methods.FunString.Check_Regex(p1);
                switch (typeText)
                {
                    case "Email":
                        Methods.App.SendEmail(ActivityContext, p1);
                        break;
                    case "Website":
                    {
                        string url = p1.Contains("http") switch
                        {
                            false => "http://" + p1,
                            _ => p1
                        };

                        //var intent = new Intent(MainContext, typeof(LocalWebViewActivity));
                        //intent.PutExtra("URL", url);
                        //intent.PutExtra("Type", url);
                        //MainContext.StartActivity(intent);
                        new IntentController(ActivityContext).OpenBrowserFromApp(url);
                        break;
                    }
                    case "Hashtag":
                    {
                        var intent = new Intent(ActivityContext, typeof(HashTagPostsActivity));
                        intent.PutExtra("Id", p1);
                        intent.PutExtra("Tag", p1);
                        ActivityContext.StartActivity(intent);
                        break;
                    }
                    case "Mention":
                    {
                        var dataUSer = ListUtils.MyProfileList?.FirstOrDefault();
                        string name = p1.Replace("@", "");

                        var sqlEntity = new SqLiteDatabase();
                        var user = sqlEntity.Get_DataOneUser(name);
                    

                        if (user != null)
                        {
                            WoWonderTools.OpenProfile(ActivityContext, user.UserId, user);
                        }
                        else switch (userData?.Count)
                        {
                            case > 0:
                            {
                                var data = userData.FirstOrDefault(a => a.Value == name);
                                if (data.Key != null && data.Key == UserDetails.UserId)
                                {
                                    switch (PostClickListener.OpenMyProfile)
                                    {
                                        case true:
                                            return;
                                        default:
                                        {
                                            var intent = new Intent(ActivityContext, typeof(MyProfileActivity));
                                            ActivityContext.StartActivity(intent);
                                            break;
                                        }
                                    }
                                }
                                else if (data.Key != null)
                                {
                                    var intent = new Intent(ActivityContext, typeof(UserProfileActivity));
                                    //intent.PutExtra("UserObject", JsonConvert.SerializeObject(item));
                                    intent.PutExtra("UserId", data.Key);
                                    ActivityContext.StartActivity(intent);
                                }
                                else
                                {
                                    if (name == dataUSer?.Name || name == dataUSer?.Username)
                                    {
                                        switch (PostClickListener.OpenMyProfile)
                                        {
                                            case true:
                                                return;
                                            default:
                                            {
                                                var intent = new Intent(ActivityContext, typeof(MyProfileActivity));
                                                ActivityContext.StartActivity(intent);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var intent = new Intent(ActivityContext, typeof(UserProfileActivity));
                                        //intent.PutExtra("UserObject", JsonConvert.SerializeObject(item));
                                        intent.PutExtra("name", name);
                                        ActivityContext.StartActivity(intent);
                                    }
                                }

                                break;
                            }
                            default:
                            {
                                if (name == dataUSer?.Name || name == dataUSer?.Username)
                                {
                                    switch (PostClickListener.OpenMyProfile)
                                    {
                                        case true:
                                            return;
                                        default:
                                        {
                                            var intent = new Intent(ActivityContext, typeof(MyProfileActivity));
                                            ActivityContext.StartActivity(intent);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    var intent = new Intent(ActivityContext, typeof(UserProfileActivity));
                                    //intent.PutExtra("UserObject", JsonConvert.SerializeObject(item));
                                    intent.PutExtra("name", name);
                                    ActivityContext.StartActivity(intent);
                                }

                                break;
                            }
                        }

                        break;
                    }
                    case "Number":
                        Methods.App.SaveContacts(ActivityContext, p1, "", "2");
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
    }

    public sealed class PreCachingLayoutManager : LinearLayoutManager
    {
        private readonly Context Context;
        private int ExtraLayoutSpace = -1;
        private readonly int DefaultExtraLayoutSpace = 600;
        private OrientationHelper MOrientationHelper;
        private int MAdditionalAdjacentPrefetchItemCount;

        public PreCachingLayoutManager(Activity context) : base(context)
        {
            try
            {
                Context = context;
                Init();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void Init()
        {
            try
            {
                MOrientationHelper = OrientationHelper.CreateOrientationHelper(this, Orientation);
                ItemPrefetchEnabled = true;
                InitialPrefetchItemCount = 20;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void SetExtraLayoutSpace(int space)
        {
            ExtraLayoutSpace = space;
        }

        [Obsolete("deprecated")]
        protected override int GetExtraLayoutSpace(RecyclerView.State state)
        {
            return ExtraLayoutSpace switch
            {
                > 0 => ExtraLayoutSpace,
                _ => DefaultExtraLayoutSpace
            };
        }

        public void SetPreloadItemCount(int preloadItemCount)
        {
            MAdditionalAdjacentPrefetchItemCount = preloadItemCount switch
            {
                < 1 => throw new IllegalArgumentException("adjacentPrefetchItemCount must not smaller than 1!"),
                _ => preloadItemCount - 1
            };
        }

        public override void CollectAdjacentPrefetchPositions(int dx, int dy, RecyclerView.State state, ILayoutPrefetchRegistry layoutPrefetchRegistry)
        {
            try
            {
                base.CollectAdjacentPrefetchPositions(dx, dy, state, layoutPrefetchRegistry);

                var delta = Orientation == Horizontal ? dx : dy;
                if (ChildCount == 0 || delta == 0)
                    return;

                var layoutDirection = delta > 0 ? 1 : -1;
                var child = GetChildClosest(layoutDirection);
                var currentPosition = GetPosition(child) + layoutDirection;

                if (layoutDirection != 1)
                    return;

                var scrollingOffset = MOrientationHelper.GetDecoratedEnd(child) - MOrientationHelper.EndAfterPadding;
                for (var i = currentPosition + 1; i < currentPosition + MAdditionalAdjacentPrefetchItemCount + 1; i++)
                {
                    switch (i)
                    {
                        case >= 0 when i < state.ItemCount:
                            layoutPrefetchRegistry.AddPosition(i, Java.Lang.Math.Max(0, scrollingOffset));
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private View GetChildClosest(int layoutDirection)
        {
            return GetChildAt(layoutDirection == -1 ? 0 : ChildCount - 1);
        }
    }
}
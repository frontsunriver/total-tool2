using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Content;
using Android.Gms.Ads.DoubleClick;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using AT.Markushi.UI;
using Com.Luseen.Autolinklibrary;
using ImageViews.Rounded;
using Newtonsoft.Json;
using Refractored.Controls;
using WoWonder.Activities.AddPost;
using WoWonder.Activities.Communities.Adapters;
using WoWonder.Activities.Communities.Groups;
using WoWonder.Activities.Communities.Pages;
using WoWonder.Activities.Live.Page;
using WoWonder.Activities.Live.Utils;
using WoWonder.Activities.NativePost.Extra;
using WoWonder.Activities.Search;
using WoWonder.Activities.SearchForPosts;
using WoWonder.Activities.Story.Adapters;
using WoWonder.Activities.Suggested.Adapters;
using WoWonder.Activities.Suggested.Groups;
using WoWonder.Activities.Suggested.User;
using WoWonder.Activities.Tabbes;
using WoWonder.Activities.UserProfile.Adapters;
using WoWonder.Activities.UsersPages;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.Library.Anjo; 
using WoWonder.Library.UI;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Group;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Requests;
using SuperTextView = WoWonder.Library.Anjo.SuperTextLibrary.SuperTextView;

namespace WoWonder.Activities.NativePost.Post
{
    public class AdapterHolders
    { 
        public class PostTopSharedSectionViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;
             
            public AppCompatTextView Username { get; private set; }

            public AppCompatTextView TimeText { get; private set; }
            public ImageView PrivacyPostIcon { get; private set; }
            public ImageView UserAvatar { get; private set; }
            public PostTopSharedSectionViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    Username = itemView.FindViewById<AppCompatTextView>(Resource.Id.username);
                    TimeText = itemView.FindViewById<AppCompatTextView>(Resource.Id.time_text);
                    PrivacyPostIcon = itemView.FindViewById<ImageView>(Resource.Id.privacyPost);
                    UserAvatar = itemView.FindViewById<ImageView>(Resource.Id.userAvatar);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    Username.SetOnClickListener(this);
                    UserAvatar.SetOnClickListener(this); 
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == Username.Id)
                            PostClickListener.ProfilePostClick(new ProfileClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView }, "NewsFeedClass", "Username");
                        else if (v.Id == UserAvatar.Id) 
                            PostClickListener.ProfilePostClick(new ProfileClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView }, "NewsFeedClass", "UserAvatar");
                    } 
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
        }

        public class PostTopSectionViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            public readonly NativePostAdapter PostAdapter;
            public readonly PostClickListener PostClickListener;

            public TextViewWithImages Username { get;  set; }

            public AppCompatTextView TimeText { get; set; }
            public ImageView PrivacyPostIcon { get;  set; }
            public ImageView MoreIcon { get; private set; }
            public ImageView UserAvatar { get; set; }
          
            public PostTopSectionViewHolder(View itemView, NativePostAdapter postAdapter ,  PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    Username = itemView.FindViewById<TextViewWithImages>(Resource.Id.username);
                    TimeText = itemView.FindViewById<AppCompatTextView>(Resource.Id.time_text);
                    PrivacyPostIcon = itemView.FindViewById<ImageView>(Resource.Id.privacyPost);
                    UserAvatar = itemView.FindViewById<ImageView>(Resource.Id.userAvatar);
                    MoreIcon = itemView.FindViewById<ImageView>(Resource.Id.moreicon);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    UserAvatar.SetOnClickListener(this);
                    MoreIcon.SetOnClickListener(this);

                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;
                         
                        if (v.Id == UserAvatar.Id)
                            PostClickListener.ProfilePostClick(new ProfileClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView }, "NewsFeedClass", "UserAvatar");
                        else if (v.Id == MoreIcon.Id)
                            PostClickListener.MorePostIconClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            } 
        }

        public class PostTextSectionViewHolder : RecyclerView.ViewHolder
        {
            public SuperTextView Description { get; private set; }

            public PostTextSectionViewHolder(View itemView) : base(itemView)
            {
                try
                {
                    Description = itemView.FindViewById<SuperTextView>(Resource.Id.description);
                    Description?.SetTextInfo(Description); 
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }
        }

        public class PostPrevBottomSectionViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public TextView ShareCount { get; private set; }
            public TextView CommentCount { get; private set; }
            public TextView LikeCount { get; private set; }
            public TextView ViewCount { get; private set; }
            public LinearLayout CountLikeSection { get; private set; }
            public ImageView ImageCountLike { get; private set; }

            public PostPrevBottomSectionViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    ShareCount = itemView.FindViewById<TextView>(Resource.Id.Sharecount);
                    CommentCount = itemView.FindViewById<TextView>(Resource.Id.Commentcount);
                    CountLikeSection = itemView.FindViewById<LinearLayout>(Resource.Id.countLikeSection);
                    LikeCount = itemView.FindViewById<TextView>(Resource.Id.Likecount);
                    ImageCountLike = itemView.FindViewById<ImageView>(Resource.Id.ImagecountLike);
                     
                    ViewCount = itemView.FindViewById<TextView>(Resource.Id.viewcount);

                    ShareCount.Visibility = AppSettings.ShowCountSharePost switch
                    {
                        false => ViewStates.Gone,
                        _ => ShareCount.Visibility
                    };

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    LikeCount.SetOnClickListener(this);
                    CommentCount.SetOnClickListener(this);
                    ShareCount.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;
                        var postType = PostFunctions.GetAdapterType(item);

                        if (v.Id == LikeCount.Id)
                            PostClickListener.DataItemPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                        else if (v.Id == CommentCount.Id)
                            PostClickListener.CommentPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                        else if (v.Id == ShareCount.Id)
                            PostClickListener.SharePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView }, postType);
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

        }
         
        public class PostBottomSectionViewHolder : RecyclerView.ViewHolder, View.IOnClickListener, View.IOnLongClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;
            public LinearLayout MainSectionButton { get; private set; }
            public LinearLayout ShareLinearLayout { get; private set; }
            public LinearLayout CommentLinearLayout { get; private set; }
            public LinearLayout SecondReactionLinearLayout { get; set; }
            public LinearLayout ReactLinearLayout { get; set; }
            public LinearLayout ViewsLinearLayout { get; set; }
            public ReactButton LikeButton { get; private set; } 
            public TextView SecondReactionButton { get; private set; }
             
            public TextView TvCommentCount { get; private set; } 
           

            public PostBottomSectionViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    ShareLinearLayout = itemView.FindViewById<LinearLayout>(Resource.Id.ShareLinearLayout);
                    CommentLinearLayout = itemView.FindViewById<LinearLayout>(Resource.Id.CommentLinearLayout);
                    SecondReactionLinearLayout = itemView.FindViewById<LinearLayout>(Resource.Id.SecondReactionLinearLayout);
                    ViewsLinearLayout = itemView.FindViewById<LinearLayout>(Resource.Id.ViewsLinearLayout);
                    ReactLinearLayout = itemView.FindViewById<LinearLayout>(Resource.Id.ReactLinearLayout);
                    LikeButton = itemView.FindViewById<ReactButton>(Resource.Id.ReactButton); 

                    SecondReactionButton = itemView.FindViewById<TextView>(Resource.Id.SecondReactionText);

                    TvCommentCount = itemView.FindViewById<TextView>(Resource.Id.CommentText);
                    //TvCommentCount.Text = "";

                    ShareLinearLayout.Visibility = AppSettings.ShowShareButton switch
                    {
                        false => ViewStates.Gone,
                        _ => ShareLinearLayout.Visibility
                    };

                    MainSectionButton = itemView.FindViewById<LinearLayout>(Resource.Id.linerSecondReaction);
                    switch (AppSettings.PostButton)
                    {
                        case PostButtonSystem.ReactionDefault:
                        case PostButtonSystem.ReactionSubShine:
                        case PostButtonSystem.Like:
                            //MainSectionButton.WeightSum = AppSettings.ShowShareButton ? 3 : 2;

                            SecondReactionLinearLayout.Visibility = ViewStates.Gone;
                            break;
                        case PostButtonSystem.Wonder:
                            //MainSectionButton.WeightSum = AppSettings.ShowShareButton ? 4 : 3;

                            SecondReactionLinearLayout.Visibility = ViewStates.Visible;

                            SecondReactionButton.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.icon_post_wonder_vector, 0, 0, 0);
                            SecondReactionButton.Text = Application.Context.GetText(Resource.String.Btn_Wonder);
                            break;
                        case PostButtonSystem.DisLike:
                            //MainSectionButton.WeightSum = AppSettings.ShowShareButton ? 4 : 3;

                            SecondReactionLinearLayout.Visibility = ViewStates.Visible;
                            SecondReactionButton.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.ic_action_dislike, 0, 0, 0);
                            SecondReactionButton.Text = Application.Context.GetText(Resource.String.Btn_Dislike);
                            break;
                    }

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    ReactLinearLayout.SetOnClickListener(this);
                    ReactLinearLayout.SetOnLongClickListener(this);
                    CommentLinearLayout.SetOnClickListener(this);
                    ShareLinearLayout.SetOnClickListener(this);
                    SecondReactionButton.SetOnClickListener(this); 
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        var postType = PostFunctions.GetAdapterType(item);

                        if (v.Id == ReactLinearLayout.Id)
                            LikeButton.ClickLikeAndDisLike(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView }, PostAdapter);
                        else if (v.Id == CommentLinearLayout.Id)
                            PostClickListener.CommentPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                        else if (v.Id == ShareLinearLayout.Id)
                            PostClickListener.SharePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView }, postType); 
                        else if (v.Id == SecondReactionButton.Id)
                            PostClickListener.SecondReactionButtonClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public bool OnLongClick(View v)
            {
                switch (AppSettings.PostButton)
                {
                    //add event if System = ReactButton 
                    case PostButtonSystem.ReactionDefault:
                    case PostButtonSystem.ReactionSubShine:
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (ReactLinearLayout.Id == v.Id)
                            LikeButton.LongClickDialog(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView },PostAdapter);
                        break;
                    }
                }

                return true;
            } 
        }

        public class PostImageSectionViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;
            private readonly int ClickType;

            public RoundedImageView Image { get; set; }

            public PostImageSectionViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener , int clickType) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    ClickType = clickType;

                    itemView.SetLayerType(LayerType.Hardware, null);
                    Image = itemView.FindViewById<RoundedImageView>(Resource.Id.Image);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    Image.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == Image.Id)
                        {
                            switch (ClickType)
                            {
                                case (int)PostModelType.MapPost:
                                    PostClickListener.MapPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                                    break;
                                case (int)PostModelType.ImagePost:
                                case (int)PostModelType.StickerPost:
                                     PostClickListener.SingleImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
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

        public class Post2ImageSectionViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public RoundedImageView Image { get; private set; }
            public RoundedImageView Image2 { get; private set; }
          
            public Post2ImageSectionViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    itemView.SetLayerType(LayerType.Hardware, null);
                    Image = itemView.FindViewById<RoundedImageView>(Resource.Id.image);
                    Image2 = itemView.FindViewById<RoundedImageView>(Resource.Id.image2);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    Image.SetOnClickListener(this);
                    Image2.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == Image.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 0, View = MainView });
                        if (v.Id == Image2.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 1, View = MainView });
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            } 
        }

        public class Post3ImageSectionViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public RoundedImageView Image { get; private set; }
            public RoundedImageView Image2 { get; private set; }
            public RoundedImageView Image3 { get; private set; }
          
            public Post3ImageSectionViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    itemView.SetLayerType(LayerType.Hardware, null);
                    Image = itemView.FindViewById<RoundedImageView>(Resource.Id.image);
                    Image2 = itemView.FindViewById<RoundedImageView>(Resource.Id.image2);
                    Image3 = itemView.FindViewById<RoundedImageView>(Resource.Id.image3);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    Image.SetOnClickListener(this);
                    Image2.SetOnClickListener(this);
                    Image3.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == Image.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 0, View = MainView });
                        if (v.Id == Image2.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 1, View = MainView });
                        if (v.Id == Image3.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 2, View = MainView });
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
        }

        public class Post4ImageSectionViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public RoundedImageView Image { get; private set; }
            public RoundedImageView Image2 { get; private set; }
            public RoundedImageView Image3 { get; private set; }
            public RoundedImageView Image4 { get; private set; }
         
            public Post4ImageSectionViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    itemView.SetLayerType(LayerType.Hardware, null);
                    Image = itemView.FindViewById<RoundedImageView>(Resource.Id.image);
                    Image2 = itemView.FindViewById<RoundedImageView>(Resource.Id.image2);
                    Image3 = itemView.FindViewById<RoundedImageView>(Resource.Id.image3);
                    Image4 = itemView.FindViewById<RoundedImageView>(Resource.Id.image4);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    Image.SetOnClickListener(this);
                    Image2.SetOnClickListener(this);
                    Image3.SetOnClickListener(this);
                    Image4.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == Image.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 0, View = MainView });
                        if (v.Id == Image2.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 1, View = MainView });
                        if (v.Id == Image3.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 2, View = MainView });
                        if (v.Id == Image4.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 3, View = MainView });
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            } 
        }

        public class PostMultiImagesViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public RoundedImageView Image { get; private set; }
            public RoundedImageView Image2 { get; private set; }
            public RoundedImageView Image3 { get; private set; }
            public RoundedImageView Image4 { get; private set; }
            public RoundedImageView Image5 { get; private set; }
            public RoundedImageView Image6 { get; private set; }
            public RoundedImageView Image7 { get; private set; }
            public TextView CountImageLabel { get; private set; }
            public View ViewImage7 { get; private set; }

            public PostMultiImagesViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    Image = itemView.FindViewById<RoundedImageView>(Resource.Id.image);
                    Image2 = itemView.FindViewById<RoundedImageView>(Resource.Id.image2);
                    Image3 = itemView.FindViewById<RoundedImageView>(Resource.Id.image3);
                    Image4 = itemView.FindViewById<RoundedImageView>(Resource.Id.image4);
                    Image5 = itemView.FindViewById<RoundedImageView>(Resource.Id.image5);
                    Image6 = itemView.FindViewById<RoundedImageView>(Resource.Id.image6);
                    Image7 = itemView.FindViewById<RoundedImageView>(Resource.Id.image7);
                    CountImageLabel = itemView.FindViewById<TextView>(Resource.Id.counttext);
                    ViewImage7 = itemView.FindViewById<View>(Resource.Id.view_image7);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    Image.SetOnClickListener(this);
                    Image2.SetOnClickListener(this);
                    Image3.SetOnClickListener(this);
                    Image4.SetOnClickListener(this);
                    Image5.SetOnClickListener(this);
                    Image6.SetOnClickListener(this);
                    Image7.SetOnClickListener(this);
                    CountImageLabel.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == Image.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 0, View = MainView });
                        if (v.Id == Image2.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 1, View = MainView });
                        if (v.Id == Image3.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 2, View = MainView });
                        if (v.Id == Image4.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 3, View = MainView });
                        if (v.Id == Image5.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 4, View = MainView });
                        if (v.Id == Image6.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 5, View = MainView });
                        if (v.Id == Image7.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 6, View = MainView });
                        if (v.Id == CountImageLabel.Id)
                            PostClickListener.ImagePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = 8, View = MainView });
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            } 
        }
        
        public class PostVideoSectionViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;

            public ImageView PlayControl { get; private set; }
            public ImageView VideoImage { get; private set; }
            public ProgressBar VideoProgressBar { get; private set; }
            public FrameLayout MediaContainer { get; private set; }
            public string VideoUrl { get; set; }
            public PostVideoSectionViewHolder(View itemView , NativePostAdapter postAdapter) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    itemView.Tag = this;

                    //itemView.SetLayerType(LayerType.Hardware, null);
                    MediaContainer = itemView.FindViewById<FrameLayout>(Resource.Id.media_container);
                    VideoImage = itemView.FindViewById<ImageView>(Resource.Id.image);
                    PlayControl = itemView.FindViewById<ImageView>(Resource.Id.Play_control);
                    VideoProgressBar = itemView.FindViewById<ProgressBar>(Resource.Id.progressBar);

                    PostAdapter = postAdapter;

                    PlayControl.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == PlayControl.Id)
                            WRecyclerView.GetInstance()?.PlayVideo(!WRecyclerView.GetInstance().CanScrollVertically(1), this, item);
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

        }

        public class PostBlogSectionViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public RoundedImageView ImageBlog { get; private set; }
            public ImageView BlogIcon { get; private set; }
            public TextView PostBlogText { get; private set; }
            public TextView PostBlogContent { get; private set; }
            public TextView CatText { get; private set; }
            public RelativeLayout PostLinkLinearLayout { get; private set; }

            public PostBlogSectionViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    ImageBlog = itemView.FindViewById<RoundedImageView>(Resource.Id.imageblog);
                    BlogIcon = itemView.FindViewById<ImageView>(Resource.Id.blogIcon);
                    PostBlogText = itemView.FindViewById<TextView>(Resource.Id.postblogText);
                    PostBlogContent = itemView.FindViewById<TextView>(Resource.Id.postBlogContent);
                    CatText = itemView.FindViewById<TextView>(Resource.Id.catText);
                    PostLinkLinearLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.linklinearLayout);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    PostLinkLinearLayout.SetOnClickListener(this);
                    PostBlogText.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == PostLinkLinearLayout.Id || v.Id == PostBlogText.Id)
                            PostClickListener.ArticleItemPostClick(item?.Blog);
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

        }

        public class PostColorBoxSectionViewHolder : RecyclerView.ViewHolder
        {
            public SuperTextView DesTextView { get; private set; }
            public ImageView ColorBoxImage { get; private set; }
            public PostColorBoxSectionViewHolder(View itemView) : base(itemView)
            {
                try
                {
                    DesTextView = itemView.FindViewById<SuperTextView>(Resource.Id.description);
                    ColorBoxImage = itemView.FindViewById<ImageView>(Resource.Id.Image);
                    DesTextView?.SetTextInfo(DesTextView);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
        }

        public class EventPostViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public RoundedImageView Image { get; private set; }
            public AppCompatTextView TxtEventTitle { get; private set; }
            public AppCompatTextView TxtEventDescription { get; private set; }
            public AppCompatTextView TxtEventTime { get; private set; }
            public AppCompatTextView TxtEventLocation { get; private set; }
            public RelativeLayout PostLinkLinearLayout { get; private set; }
             
            public EventPostViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    Image = itemView.FindViewById<RoundedImageView>(Resource.Id.Image);
                    TxtEventTitle = itemView.FindViewById<AppCompatTextView>(Resource.Id.event_titile);
                    TxtEventDescription = itemView.FindViewById<AppCompatTextView>(Resource.Id.event_description);
                    TxtEventTime = itemView.FindViewById<AppCompatTextView>(Resource.Id.event_time);
                    TxtEventLocation = itemView.FindViewById<AppCompatTextView>(Resource.Id.event_location);
                    PostLinkLinearLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.linklinearLayout);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    PostLinkLinearLayout.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == PostLinkLinearLayout.Id)
                            PostClickListener.EventItemPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

        }

        public class LinkPostViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public RoundedImageView Image { get; private set; }
            public TextView LinkUrl { get; private set; }
            public TextView PostLinkTitle { get; private set; }
            public TextView PostLinkContent { get; private set; }
            public LinearLayout PostLinkLinearLayout { get; private set; }

            public LinkPostViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    Image = itemView.FindViewById<RoundedImageView>(Resource.Id.image);
                    LinkUrl = itemView.FindViewById<TextView>(Resource.Id.linkUrl);
                    PostLinkTitle = itemView.FindViewById<TextView>(Resource.Id.postLinkTitle);
                    PostLinkContent = itemView.FindViewById<TextView>(Resource.Id.postLinkContent);
                    PostLinkLinearLayout = itemView.FindViewById<LinearLayout>(Resource.Id.linklinearLayout);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    PostLinkLinearLayout.SetOnClickListener(this);

                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == PostLinkLinearLayout.Id)
                            PostClickListener.LinkPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView }, "LinkPost");
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

        }

        public class FilePostViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public CircleButton DownlandButton { get; private set; }
            public TextView PostFileText { get; private set; }
            public ImageView FileIcon { get; private set; }

            public FilePostViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    DownlandButton = itemView.FindViewById<CircleButton>(Resource.Id.downlaodButton);
                    PostFileText = itemView.FindViewById<TextView>(Resource.Id.postfileText);
                    FileIcon = itemView.FindViewById<ImageView>(Resource.Id.fileIcon);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    DownlandButton.SetOnClickListener(this);
                    PostFileText.SetOnClickListener(this);
                    FileIcon.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == DownlandButton.Id)
                            PostClickListener.FileDownloadPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                        else if (v.Id == PostFileText.Id || v.Id == FileIcon.Id)
                            PostClickListener.OpenFilePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

        }

        public class FundingPostViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;
             
            public ImageView Image { get; private set; }
            public TextView Title { get; private set; }
            public TextView Description { get; private set; }
            public ProgressBar Progress { get; private set; }
            public TextView TottalAmount { get; private set; }
            public TextView Raised { get; private set; }
            public TextView DonationTime { get; private set; }
            public RelativeLayout MainCardView { get; private set; }

            public FundingPostViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    MainCardView = itemView.FindViewById<RelativeLayout>(Resource.Id.info_container);
                    Image = itemView.FindViewById<ImageView>(Resource.Id.fundImage);
                    Title = itemView.FindViewById<TextView>(Resource.Id.fundTitle);
                    Description = itemView.FindViewById<TextView>(Resource.Id.fundDescription);
                    Progress = itemView.FindViewById<ProgressBar>(Resource.Id.progressBar);
                    TottalAmount = itemView.FindViewById<TextView>(Resource.Id.TottalAmount);
                    Raised = itemView.FindViewById<TextView>(Resource.Id.raised);
                    DonationTime = itemView.FindViewById<TextView>(Resource.Id.time);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    MainCardView.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == MainCardView.Id)
                            PostClickListener.OpenFundingPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            } 
        }

        public class SoundPostViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            public readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public SeekBar SeekBar { get; private set; }
            public ImageView MoreIcon { get; private set; }
            public TextView Time { get; private set; }
            public TextView Duration { get; private set; }
            public ProgressBar LoadingProgressView { get; private set; }
            public CircleButton PlayButton { get; private set; }

            public SoundPostViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    SeekBar = itemView.FindViewById<SeekBar>(Resource.Id.seekBar);
                    MoreIcon = itemView.FindViewById<ImageView>(Resource.Id.moreicon2);
                    Time = itemView.FindViewById<TextView>(Resource.Id.time);
                    Duration = itemView.FindViewById<TextView>(Resource.Id.Duration);
                    LoadingProgressView = itemView.FindViewById<ProgressBar>(Resource.Id.loadingProgressview);
                    LoadingProgressView.Visibility = ViewStates.Gone;
                    PlayButton = itemView.FindViewById<CircleButton>(Resource.Id.playButton);
                    PlayButton.Visibility = ViewStates.Visible;
                    PlayButton.SetImageResource(Resource.Drawable.icon_player_play);
                    PlayButton.Tag = "Play";

                    switch (Build.VERSION.SdkInt)
                    {
                        case >= BuildVersionCodes.N:
                            SeekBar.SetProgress(0, true);
                            break;
                        // For API < 24 
                        default:
                            SeekBar.Progress = 0;
                            break;
                    }

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    if (PostClickListener != null)
                    {
                        PlayButton.SetOnClickListener(this);
                    } 
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            } 

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition];

                        if (v.Id == PlayButton.Id)
                            PostClickListener.VoicePostClick(new GlobalClickEventArgs { AdapterModelsClass = item, NewsFeedClass = item.PostData, Position = AdapterPosition, View = MainView, HolderSound = this });
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            } 
        }

        public class YoutubePostViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public ImageView Image { get; private set; }
            public ImageView VideoIcon { get; private set; }
             
            public YoutubePostViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    Image = itemView.FindViewById<ImageView>(Resource.Id.image);
                    VideoIcon = itemView.FindViewById<ImageView>(Resource.Id.videoicon);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    VideoIcon.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == VideoIcon.Id)
                            PostClickListener.YoutubePostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            } 
        }

        public class OfferPostViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public ImageView ImageBlog { get; private set; }
            public ImageView BlogIcon { get; private set; }
            public TextView PostBlogText { get; private set; }
            public TextView PostBlogContent { get; private set; }
            public TextView CatText { get; private set; }
            public RelativeLayout MainLayout { get; private set; }

            public OfferPostViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    MainLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.mainLayout);
                    ImageBlog = itemView.FindViewById<ImageView>(Resource.Id.imageblog);
                    BlogIcon = itemView.FindViewById<ImageView>(Resource.Id.blogIcon);
                    PostBlogText = itemView.FindViewById<TextView>(Resource.Id.postblogText);
                    PostBlogContent = itemView.FindViewById<TextView>(Resource.Id.postBlogContent);
                    CatText = itemView.FindViewById<TextView>(Resource.Id.catText);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    MainLayout.SetOnClickListener(this);

                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == MainLayout.Id)
                            PostClickListener.OffersPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            } 
        }

        public class PostPlayTubeContentViewHolder : RecyclerView.ViewHolder
        {
            public WoWebView WebView { get; private set; }

            public PostPlayTubeContentViewHolder(View itemView) : base(itemView)
            {
                try
                {
                    WebView = itemView.FindViewById<WoWebView>(Resource.Id.webview);
                    //itemView.SetLayerType(LayerType.Hardware,null);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            } 
        }
      
        public class PostAgoraLiveViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public RelativeLayout MainLayout { get; private set; }
            public ImageView ImageLive { get; private set; }
            public TextView TxtName { get; private set; }
             
            public PostAgoraLiveViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    MainLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.mainLayout);
                    ImageLive = itemView.FindViewById<ImageView>(Resource.Id.ImageLive);
                    TxtName = itemView.FindViewById<TextView>(Resource.Id.Txt_Username);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    MainLayout.SetOnClickListener(this); 
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }


            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (item?.LiveTime != null && item?.LiveTime.Value > 0 && item.IsStillLive != null && item.IsStillLive.Value && string.IsNullOrEmpty(item?.AgoraResourceId) && string.IsNullOrEmpty(item?.PostFile)) //Live
                        {
                            //Owner >> ClientRoleBroadcaster , Users >> ClientRoleAudience
                            Intent intent = new Intent(PostAdapter.ActivityContext, typeof(LiveStreamingActivity));
                            intent.PutExtra(Constants.KeyClientRole, DT.Xamarin.Agora.Constants.ClientRoleAudience);
                            intent.PutExtra("PostId", item.PostId);
                            intent.PutExtra("StreamName", item.StreamName);
                            intent.PutExtra("PostLiveStream", JsonConvert.SerializeObject(item));
                            PostAdapter.ActivityContext.StartActivity(intent);
                        }
                        else if (item?.LiveTime != null && item?.LiveTime.Value > 0 && !string.IsNullOrEmpty(item?.AgoraResourceId) && !string.IsNullOrEmpty(item?.PostFile)) //Saved
                        {

                        }
                        else //End
                        {

                        }
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }
        }
      
        public class AdsPostViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;
             
            public CircleImageView UserAvatar { get; private set; }
            public AppCompatTextView Username { get; private set; }
            public AppCompatTextView TimeText { get; private set; }
            public ImageView Image { get; private set; }
            public ImageView Moreicon { get; private set; }
            public SuperTextView Description { get; private set; }
            public TextView IconLocation { get; private set; }
            public TextView TextLocation { get; private set; }
            public TextView IconLink { get; private set; }
            public AutoLinkTextView Headline { get; private set; }
            public TextView LinkUrl { get; private set; }
            public TextView PostLinkTitle { get; private set; }
            public TextView PostLinkContent { get; private set; }
            public LinearLayout PostLinkLinearLayout { get; private set; }

            public AdsPostViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    UserAvatar = itemView.FindViewById<CircleImageView>(Resource.Id.userAvatar);
                    Username = itemView.FindViewById<AppCompatTextView>(Resource.Id.username);
                    TimeText = itemView.FindViewById<AppCompatTextView>(Resource.Id.time_text);
                    Moreicon = itemView.FindViewById<ImageView>(Resource.Id.moreicon);
                    Description = itemView.FindViewById<SuperTextView>(Resource.Id.description);
                    Description?.SetTextInfo(Description);

                    IconLocation = itemView.FindViewById<TextView>(Resource.Id.iconloca);
                    TextLocation = itemView.FindViewById<TextView>(Resource.Id.textloc);
                    IconLink = itemView.FindViewById<TextView>(Resource.Id.IconLink);
                    Headline = itemView.FindViewById<AutoLinkTextView>(Resource.Id.headline);
                    Image = itemView.FindViewById<ImageView>(Resource.Id.image);
                    LinkUrl = itemView.FindViewById<TextView>(Resource.Id.linkUrl);
                    PostLinkTitle = itemView.FindViewById<TextView>(Resource.Id.postLinkTitle);
                    PostLinkContent = itemView.FindViewById<TextView>(Resource.Id.postLinkContent);
                    PostLinkLinearLayout = itemView.FindViewById<LinearLayout>(Resource.Id.linklinearLayout);

                    PostLinkTitle.Visibility = ViewStates.Gone;
                    PostLinkContent.Visibility = ViewStates.Gone;

                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconLocation, IonIconsFonts.Pin);
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconLink, IonIconsFonts.Link);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    PostLinkLinearLayout.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == PostLinkLinearLayout.Id)
                            PostClickListener.LinkPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView }, "AdsPost");
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

        }

        public class ProductPostViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public RoundedImageView Image { get; private set; }
            public ImageView LocationIcon { get; private set; }
            public TextView PostProductLocationText { get; private set; }
            public TextView PostLinkTitle { get; private set; }
            public TextView PostProductContent { get; private set; }
            public TextView TypeText { get; private set; }
            public TextView PriceText { get; private set; }
            public TextView StatusText { get; private set; }
            public RelativeLayout PostLinkLinearLayout { get; private set; }

            public ProductPostViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    Image = itemView.FindViewById<RoundedImageView>(Resource.Id.image);
                    LocationIcon = itemView.FindViewById<ImageView>(Resource.Id.locationIcon);
                    PostProductLocationText = itemView.FindViewById<TextView>(Resource.Id.postProductLocationText);
                    PostLinkTitle = itemView.FindViewById<TextView>(Resource.Id.postProductTitle);
                    PostProductContent = itemView.FindViewById<TextView>(Resource.Id.postProductContent);
                    TypeText = itemView.FindViewById<TextView>(Resource.Id.typeText);
                    PriceText = itemView.FindViewById<TextView>(Resource.Id.priceText);
                    StatusText = itemView.FindViewById<TextView>(Resource.Id.statusText);
                    PostLinkLinearLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.contentPost);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    PostLinkLinearLayout.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == PostLinkLinearLayout.Id)
                            PostClickListener.ProductPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            } 
        }

        public class PollsPostViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;

            public TextView VoteText { get; private set; }
            public RelativeLayout LinkLinearLayout { get; private set; }
            public TextView ProgressText { get; private set; }
            public ImageView CheckIcon { get; private set; }
            public ProgressBar ProgressBarView { get; private set; }
             
            public PollsPostViewHolder(View itemView, NativePostAdapter postAdapter) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    LinkLinearLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.linklinearLayout);
                    VoteText = itemView.FindViewById<TextView>(Resource.Id.voteText);
                    ProgressBarView = itemView.FindViewById<ProgressBar>(Resource.Id.progress);
                    ProgressText = itemView.FindViewById<TextView>(Resource.Id.progressTextview);
                    CheckIcon = itemView.FindViewById<ImageView>(Resource.Id.checkIcon);

                    PostAdapter = postAdapter;

                    LinkLinearLayout.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public async void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        if (!Methods.CheckConnectivity())
                        {
                            Toast.MakeText(Application.Context, Application.Context.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                            return;
                        }
                          
                        var itemObject = PostAdapter.ListDiffer[AdapterPosition];
                        if (v.Id == LinkLinearLayout.Id && itemObject != null)
                        {
                            switch (string.IsNullOrEmpty(itemObject.PostData.VotedId))
                            {
                                case false when itemObject.PostData.VotedId != "0":
                                    //You have already voted this poll.
                                    Toast.MakeText(PostAdapter.ActivityContext, PostAdapter.ActivityContext.GetText(Resource.String.Lbl_ErrorVotedPoll), ToastLength.Short)?.Show();
                                    return;
                            }
                             
                            //send api
                            var (apiStatus, respond) = await RequestsAsync.Posts.AddPollPostAsync(itemObject.PollId);
                            switch (apiStatus)
                            {
                                case 200:
                                {
                                    switch (respond)
                                    {
                                        case AddPollPostObject result:
                                        {
                                            itemObject.PostData.VotedId = itemObject.PollId;

                                            //Set The correct value after for polls after new vote
                                            var data = result.Votes.FirstOrDefault(a => a.Id == itemObject.PollId);
                                            if (data != null)
                                            {
                                                ProgressBarView.Progress = Convert.ToInt32(data.PercentageNum);
                                                ProgressText.Text = data.Percentage;

                                                switch (string.IsNullOrEmpty(itemObject.PostData.VotedId))
                                                {
                                                    case false when itemObject.PostData.VotedId != "0":
                                                    {
                                                        if (itemObject.PollsOption.Id == itemObject.PostData.VotedId)
                                                        {
                                                            CheckIcon.SetImageResource(Resource.Drawable.icon_checkmark_filled_vector);
                                                            CheckIcon.ClearColorFilter();
                                                        }
                                                        else
                                                        {
                                                            CheckIcon.SetImageResource(Resource.Drawable.icon_check_circle_vector);
                                                            CheckIcon.SetColorFilter(new PorterDuffColorFilter(Color.ParseColor("#999999"), PorterDuff.Mode.SrcAtop));
                                                        }

                                                        break;
                                                    }
                                                    default:
                                                        CheckIcon.SetImageResource(Resource.Drawable.icon_check_circle_vector);
                                                        CheckIcon.SetColorFilter(new PorterDuffColorFilter(Color.ParseColor("#999999"), PorterDuff.Mode.SrcAtop));
                                                        break;
                                                }
                                            }

                                            var dataPost = PostAdapter?.ListDiffer?.Where(a => a.PostData?.Id == itemObject.PostData?.Id && a.TypeView == PostModelType.PollPost).ToList();
                                            switch (dataPost?.Count)
                                            {
                                                case > 0:
                                                {
                                                    foreach (var post in dataPost)
                                                    {
                                                        //Set The correct value after for polls after new vote
                                                        var dataVotes = result.Votes.FirstOrDefault(a => a.Id == post.PollId);
                                                        if (dataVotes != null)
                                                        { 
                                                            post.PollsOption = dataVotes;
                                                            post.PollsOption.Id = dataVotes.Id;
                                                            post.PollsOption.PostId = dataVotes.PostId;
                                                            post.PollsOption.Text = dataVotes.Text;
                                                            post.PollsOption.Time = dataVotes.Time;
                                                            post.PollsOption.OptionVotes = dataVotes.OptionVotes;
                                                            post.PollsOption.Percentage = dataVotes.Percentage;
                                                            post.PollsOption.PercentageNum = dataVotes.PercentageNum;
                                                            post.PollsOption.All = dataVotes.All;
                                                            post.PollsOption.RelatedToPollsCount = dataVotes.RelatedToPollsCount;

                                                            var index = PostAdapter.ListDiffer.IndexOf(post);
                                                            switch (index)
                                                            {
                                                                case <= 0:
                                                                    continue;
                                                                default:
                                                                    PostAdapter.ActivityContext?.RunOnUiThread(() => PostAdapter.NotifyItemChanged(index));
                                                                    break;
                                                            }
                                                        }
                                                    }

                                                    break;
                                                }
                                            }

                                            break;
                                        }
                                    }

                                    break;
                                }
                                default:
                                {
                                    string errorText = respond is ErrorObject errorMessage ? errorMessage.Error.ErrorText : (string)respond.ToString() ?? "";
                                    switch (string.IsNullOrEmpty(errorText))
                                    {
                                        case false:
                                            Toast.MakeText(PostAdapter.ActivityContext, errorText, ToastLength.Short)?.Show();
                                            break;
                                    }

                                    Methods.DisplayReportResult(TabbedMainActivity.GetInstance(), respond);
                                    break;
                                }
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
      
        public class JobPostViewHolder1 : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public ImageView JobCoverImage { get; private set; }
            public CircleImageView JobAvatar { get; private set; }
            public TextView JobTitle { get; private set; }
            public TextView PageName { get; private set; }
            public TextView JobInfo { get; private set; }
            public RelativeLayout MainLayout { get; private set; }


            public JobPostViewHolder1(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    MainLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.mainLayout);
                    JobCoverImage = itemView.FindViewById<ImageView>(Resource.Id.JobCoverImage);
                    JobAvatar = itemView.FindViewById<CircleImageView>(Resource.Id.JobAvatar);
                    JobTitle = itemView.FindViewById<TextView>(Resource.Id.Jobtitle);
                    PageName = itemView.FindViewById<TextView>(Resource.Id.pageName);
                    JobInfo = itemView.FindViewById<TextView>(Resource.Id.JobInfo);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    MainLayout.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == MainLayout.Id)
                            PostClickListener.JobPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }


        }

        public class JobPostViewHolder2 : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public Button JobButton { get; private set; }
            public TextView MinimumTextView { get; private set; }
            public TextView MaximumTextView { get; private set; }
            public TextView MaximumNumber { get; private set; }
            public TextView MinimumNumber { get; private set; }
            public SuperTextView Description { get; private set; }
            public RelativeLayout MainLayout { get; private set; }

            public JobPostViewHolder2(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    MainLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.mainLayout);
                    JobButton = itemView.FindViewById<Button>(Resource.Id.JobButton);
                    MinimumTextView = itemView.FindViewById<TextView>(Resource.Id.minimum);
                    MaximumTextView = itemView.FindViewById<TextView>(Resource.Id.maximum);
                    MaximumNumber = itemView.FindViewById<TextView>(Resource.Id.maximumNumber);
                    MinimumNumber = itemView.FindViewById<TextView>(Resource.Id.minimumNumber);
                    Description = itemView.FindViewById<SuperTextView>(Resource.Id.description);
                    Description?.SetTextInfo(Description);
                    
                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    if (JobButton != null)
                    {
                        JobButton.Tag = "Apply";
                        JobButton.SetOnClickListener(this);
                    }

                    MainLayout.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == MainLayout.Id)
                            PostClickListener.JobPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                        else if (v.Id == JobButton.Id)
                            PostClickListener.JobButtonPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView }); 
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

        }

        public class StoryViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public RecyclerView StoryRecyclerView { get; private set; }
            public StoryAdapter StoryAdapter { get; private set; }
            public TextView AboutHead { get; private set; }
            public TextView AboutMore { get; private set; }

            public StoryViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    MainView = itemView;
                    StoryRecyclerView = MainView.FindViewById<RecyclerView>(Resource.Id.Recyler);
                    AboutHead = MainView.FindViewById<TextView>(Resource.Id.headText);
                    AboutMore = MainView.FindViewById<TextView>(Resource.Id.moreText);
 
                    AboutHead.Visibility = ViewStates.Gone;
                    AboutMore.Visibility = ViewStates.Gone;

                    if (AboutMore != null)
                    {
                        AboutMore.Visibility = ViewStates.Invisible;
                        AboutMore.SetOnClickListener(this);
                    }

                    if (StoryAdapter != null)
                        return;

                    StoryRecyclerView?.SetLayoutManager(new LinearLayoutManager(postAdapter.ActivityContext, LinearLayoutManager.Horizontal, false));
                    StoryAdapter = new StoryAdapter(postAdapter.ActivityContext);
                    StoryRecyclerView?.SetAdapter(StoryAdapter);
                    StoryRecyclerView?.AddOnItemTouchListener(new RecyclerViewOnItemTouch(StoryRecyclerView, TabbedMainActivity.GetInstance()?.ViewPager));
                    StoryAdapter.ItemClick += TabbedMainActivity.GetInstance().StoryAdapterOnItemClick;
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void RefreshData()
            {
                try
                {
                    StoryAdapter.NotifyDataSetChanged();
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition];

                        if (v.Id == AboutMore.Id)
                            PostClickListener.OpenAllViewer("StoryModel", PostAdapter.IdParameter, item);
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

        }

        public class AddPostViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;

            public CircleImageView ProfileImageView { get; private set; }
            public LinearLayout PostText { get; private set; }

            public RelativeLayout RlGallery { get; private set; }
            public RelativeLayout RlFriend { get; private set; }
            public RelativeLayout RlLive { get; private set; }

            public AddPostViewHolder(View itemView, NativePostAdapter postAdapter) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    ProfileImageView = MainView.FindViewById<CircleImageView>(Resource.Id.image);
                    PostText = MainView.FindViewById<LinearLayout>(Resource.Id.ll_post_text);

                    RlGallery = MainView.FindViewById<RelativeLayout>(Resource.Id.rlPostGallery);
                    RlFriend = MainView.FindViewById<RelativeLayout>(Resource.Id.rlPostFriend);
                    RlLive = MainView.FindViewById<RelativeLayout>(Resource.Id.rlPostLive);

                    if (!AppSettings.ShowLive)
                        RlLive.Visibility = ViewStates.Gone;

                    PostAdapter = postAdapter;

                    PostText.SetOnClickListener(this);

                    RlGallery.SetOnClickListener(this);
                    RlFriend.SetOnClickListener(this);
                    RlLive.SetOnClickListener(this); 
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition];
                        var intent = new Intent(PostAdapter.ActivityContext, typeof(AddPostActivity));
                        if (v.Id == RlGallery.Id)
                        {
                            try
                            { 
                                switch (item.TypePost)
                                {
                                    case "feed":
                                    case "user":
                                        intent.PutExtra("Type", "Normal_Gallery");
                                        intent.PutExtra("PostId", PostAdapter.IdParameter);
                                        break;
                                    case "Group":
                                        intent.PutExtra("Type", "SocialGroup_Gallery");
                                        intent.PutExtra("PostId", item.PostData.GroupRecipient.GroupId);
                                        intent.PutExtra("itemObject", JsonConvert.SerializeObject(item.PostData.GroupRecipient));
                                        break;
                                    case "Event":
                                        intent.PutExtra("Type", "SocialEvent_Gallery");
                                        if (item.PostData.Event != null)
                                        {
                                            intent.PutExtra("PostId", item.PostData.Event.Value.EventClass.Id);
                                            intent.PutExtra("itemObject", JsonConvert.SerializeObject(item.PostData.Event.Value.EventClass));
                                        }
                                        break;
                                    case "Page":
                                        intent.PutExtra("Type", "SocialPage_Gallery");
                                        intent.PutExtra("PostId", item.PostData.PageId);
                                        var page = new PageClass
                                        {
                                            PageId = item.PostData.PageId,
                                            PageName = item.PostData.Publisher.PageName,
                                            Avatar = item.PostData.Publisher.Avatar,
                                        };
                                        intent.PutExtra("itemObject", JsonConvert.SerializeObject(page));
                                        break;
                                    default:
                                        intent.PutExtra("Type", "Normal_Gallery");
                                        intent.PutExtra("PostId", PostAdapter.IdParameter);
                                        break;
                                }

                                //intent.PutExtra("itemObject", JsonConvert.SerializeObject(EventData));
                                PostAdapter.ActivityContext.StartActivityForResult(intent, 2500);
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);  
                            }
                        }
                        else if (v.Id == PostText.Id)
                        {
                            try
                            { 
                                switch (item.TypePost)
                                {
                                    case "feed":
                                    case "user":
                                        intent.PutExtra("Type", "Normal");
                                        intent.PutExtra("PostId", PostAdapter.IdParameter);
                                        break;
                                    case "Group":
                                        intent.PutExtra("Type", "SocialGroup");
                                        intent.PutExtra("PostId", item.PostData.GroupRecipient.GroupId);
                                        intent.PutExtra("itemObject", JsonConvert.SerializeObject(item.PostData.GroupRecipient));
                                        break;
                                    case "Event":
                                        intent.PutExtra("Type", "SocialEvent");
                                        if (item.PostData.Event != null)
                                        {
                                            intent.PutExtra("PostId", item.PostData.Event.Value.EventClass.Id);
                                            intent.PutExtra("itemObject", JsonConvert.SerializeObject(item.PostData.Event.Value.EventClass));
                                        }
                                        break;
                                    case "Page":
                                        intent.PutExtra("Type", "SocialPage");
                                        intent.PutExtra("PostId", item.PostData.PageId);
                                        var page = new PageClass
                                        {
                                            PageId = item.PostData.PageId,
                                            PageName = item.PostData.Publisher.PageName,
                                            Avatar = item.PostData.Publisher.Avatar,
                                        };
                                        intent.PutExtra("itemObject", JsonConvert.SerializeObject(page));
                                        break;
                                    default:
                                        intent.PutExtra("Type", "Normal");
                                        intent.PutExtra("PostId", PostAdapter.IdParameter);
                                        break;
                                }
                                PostAdapter.ActivityContext.StartActivityForResult(intent, 2500);
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);  
                            }
                        } 
                        else if (v.Id == RlFriend.Id)
                        {
                            try
                            {
                                switch (item.TypePost)
                                {
                                    case "feed":
                                    case "user":
                                        intent.PutExtra("Type", "Normal_Mention");
                                        intent.PutExtra("PostId", PostAdapter.IdParameter);
                                        break;
                                    case "Group":
                                        intent.PutExtra("Type", "SocialGroup_Mention");
                                        intent.PutExtra("PostId", item.PostData.GroupRecipient.GroupId);
                                        intent.PutExtra("itemObject", JsonConvert.SerializeObject(item.PostData.GroupRecipient));
                                        break;
                                    case "Event":
                                        intent.PutExtra("Type", "SocialEvent_Mention");
                                        if (item.PostData.Event != null)
                                        {
                                            intent.PutExtra("PostId", item.PostData.Event.Value.EventClass.Id);
                                            intent.PutExtra("itemObject", JsonConvert.SerializeObject(item.PostData.Event.Value.EventClass));
                                        }
                                        break;
                                    case "Page":
                                        intent.PutExtra("Type", "SocialPage_Mention");
                                        intent.PutExtra("PostId", item.PostData.PageId);
                                        var page = new PageClass
                                        {
                                            PageId = item.PostData.PageId,
                                            PageName = item.PostData.Publisher.PageName,
                                            Avatar = item.PostData.Publisher.Avatar,
                                        };
                                        intent.PutExtra("itemObject", JsonConvert.SerializeObject(page));
                                        break;
                                    default:
                                        intent.PutExtra("Type", "Normal_Mention");
                                        intent.PutExtra("PostId", PostAdapter.IdParameter);
                                        break;
                                }
                                PostAdapter.ActivityContext.StartActivityForResult(intent, 2500);
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);  
                            }
                        }
                        else if (v.Id == RlLive.Id)
                        {
                            try
                            { 
                                new LiveUtil(PostAdapter.ActivityContext).GoLiveOnClick();
                            }
                            catch (Exception exception)
                            {
                                Methods.DisplayReportResultTrack(exception);
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

        public class SearchForPostsViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;

            public LinearLayout SearchForPostsLayout { get; private set; }

            public SearchForPostsViewHolder(View itemView , NativePostAdapter postAdapter) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    SearchForPostsLayout = MainView.FindViewById<LinearLayout>(Resource.Id.searchForPostsLayout);

                    PostAdapter = postAdapter;

                    SearchForPostsLayout.SetOnClickListener(this); 
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition];

                        if (v.Id == SearchForPostsLayout.Id)
                        {
                            var intent = new Intent(PostAdapter.ActivityContext, typeof(SearchForPostsActivity));

                            switch (item.TypePost)
                            {
                                case "feed":
                                case "user":
                                    intent.PutExtra("TypeSearch", "user");
                                    intent.PutExtra("IdSearch", PostAdapter.IdParameter);
                                    break;
                                case "Group":
                                    intent.PutExtra("TypeSearch", "group");
                                    if (item.PostData.GroupRecipient != null)
                                        intent.PutExtra("IdSearch", item.PostData.GroupRecipient.GroupId);
                                    break;
                                case "Page":
                                    intent.PutExtra("TypeSearch", "page");
                                    if (item.PostData != null)
                                        intent.PutExtra("IdSearch", item.PostData.PageId);
                                    break;
                                default:
                                    intent.PutExtra("TypeSearch", "user");
                                    intent.PutExtra("IdSearch", PostAdapter.IdParameter);
                                    break;
                            }

                            PostAdapter.ActivityContext.StartActivity(intent);
                        }
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

        }

        public class SocialLinksViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;

            public CircleButton BtnFacebook { get; private set; }
            public CircleButton BtnInstegram { get; private set; }
            public CircleButton BtnTwitter { get; private set; }
            public CircleButton BtnGoogle { get; private set; }
            public CircleButton BtnVk { get; private set; }
            public CircleButton BtnYoutube { get; private set; }

            public SocialLinksViewHolder(View itemView , NativePostAdapter postAdapter) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    BtnFacebook = MainView.FindViewById<CircleButton>(Resource.Id.facebook_button);
                    BtnInstegram = MainView.FindViewById<CircleButton>(Resource.Id.instegram_button);
                    BtnTwitter = MainView.FindViewById<CircleButton>(Resource.Id.twitter_button);
                    BtnGoogle = MainView.FindViewById<CircleButton>(Resource.Id.google_button);
                    BtnVk = MainView.FindViewById<CircleButton>(Resource.Id.vk_button);
                    BtnYoutube = MainView.FindViewById<CircleButton>(Resource.Id.youtube_button);

                    PostAdapter = postAdapter;

                    BtnFacebook.SetOnClickListener(this);
                    BtnInstegram.SetOnClickListener(this);
                    BtnTwitter.SetOnClickListener(this);
                    BtnGoogle.SetOnClickListener(this);
                    BtnVk.SetOnClickListener(this);
                    BtnYoutube.SetOnClickListener(this); 
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition];

                        if (v.Id == BtnFacebook.Id)
                        {
                            if (Methods.CheckConnectivity())
                                new IntentController(PostAdapter.ActivityContext).OpenFacebookIntent(PostAdapter.ActivityContext, item.SocialLinksModel.Facebook);
                            else
                                Toast.MakeText(PostAdapter.ActivityContext, PostAdapter.ActivityContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                        }
                        else if (v.Id == BtnInstegram.Id)
                        {
                            if (Methods.CheckConnectivity())
                                new IntentController(PostAdapter.ActivityContext).OpenInstagramIntent(item.SocialLinksModel.Instegram);
                            else
                                Toast.MakeText(PostAdapter.ActivityContext, PostAdapter.ActivityContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                        }
                        else if (v.Id == BtnTwitter.Id)
                        {
                            if (Methods.CheckConnectivity())
                                new IntentController(PostAdapter.ActivityContext).OpenTwitterIntent(item.SocialLinksModel.Twitter);
                            else
                                Toast.MakeText(PostAdapter.ActivityContext, PostAdapter.ActivityContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                        }
                        else if (v.Id == BtnGoogle.Id)
                        {
                            if (Methods.CheckConnectivity())
                                new IntentController(PostAdapter.ActivityContext).OpenBrowserFromApp("https://plus.google.com/u/0/" + item.SocialLinksModel.Google);
                            else
                                Toast.MakeText(PostAdapter.ActivityContext, PostAdapter.ActivityContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                        }
                        else if (v.Id == BtnVk.Id)
                        {
                            if (Methods.CheckConnectivity())
                                new IntentController(PostAdapter.ActivityContext).OpenVkontakteIntent(item.SocialLinksModel.Vk);
                            else
                                Toast.MakeText(PostAdapter.ActivityContext, PostAdapter.ActivityContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                        }
                        else if (v.Id == BtnYoutube.Id)
                        {
                            if (Methods.CheckConnectivity())
                                new IntentController(PostAdapter.ActivityContext).OpenYoutubeIntent(item.SocialLinksModel.Youtube);
                            else
                                Toast.MakeText(PostAdapter.ActivityContext, PostAdapter.ActivityContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                        }
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

        }

        public class AboutBoxViewHolder : RecyclerView.ViewHolder
        {
            public View MainView { get; private set; }
            public TextView AboutHead { get; private set; }
            public SuperTextView AboutDescription { get; private set; }

            public AboutBoxViewHolder(View itemView) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    AboutHead = MainView.FindViewById<TextView>(Resource.Id.tv_about);
                    AboutDescription = MainView.FindViewById<SuperTextView>(Resource.Id.tv_aboutdescUser);
                    AboutDescription?.SetTextInfo(AboutDescription);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
        }
        
        public class InfoUserBoxViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            private readonly Activity ActivityContext;
            public View MainView { get; private set; }


            public LinearLayout LayoutTime { get; private set; }
            public TextView TimeText { get; private set; }
            public LinearLayout LayoutWebsite { get; private set; }
            public TextView WebsiteText { get; private set; }
            
            public LinearLayout LayoutGander { get; private set; }
            public TextView GanderText { get; private set; }
            
            public LinearLayout LayoutBirthday { get; private set; }
            public TextView BirthdayText { get; private set; }
            
            public LinearLayout LayoutWork { get; private set; }
            public TextView WorkText { get; private set; }
            
            public LinearLayout LayoutLive { get; private set; }
            public TextView LiveText { get; private set; }
            
            public LinearLayout LayoutStudy { get; private set; }
            public TextView StudyText { get; private set; }
            
            public LinearLayout LayoutRelationship { get; private set; }
            public TextView RelationshipText { get; private set; }

            public InfoUserBoxViewHolder(View itemView, Activity activityContext) : base(itemView)
            {
                try
                {
                    ActivityContext = activityContext;
                    MainView = itemView;
                   
                    LayoutTime = MainView.FindViewById<LinearLayout>(Resource.Id.LayoutTime);
                    //IconTime = MainView.FindViewById<TextView>(Resource.Id.IconTime);
                    TimeText = MainView.FindViewById<TextView>(Resource.Id.TimeText);

                    LayoutWebsite = MainView.FindViewById<LinearLayout>(Resource.Id.LayoutWebsite);
                    //IconWebsite = MainView.FindViewById<TextView>(Resource.Id.IconWebsite);
                    WebsiteText = MainView.FindViewById<TextView>(Resource.Id.WebsiteText);

                    LayoutGander = MainView.FindViewById<LinearLayout>(Resource.Id.LayoutGander);
                    //IconGander = MainView.FindViewById<TextView>(Resource.Id.IconGander);
                    GanderText = MainView.FindViewById<TextView>(Resource.Id.GanderText);
                 
                    LayoutBirthday = MainView.FindViewById<LinearLayout>(Resource.Id.LayoutBirthday);
                    //IconBirthday = MainView.FindViewById<TextView>(Resource.Id.IconBirthday);
                    BirthdayText = MainView.FindViewById<TextView>(Resource.Id.BirthdayText);

                    LayoutWork = MainView.FindViewById<LinearLayout>(Resource.Id.LayoutWork);
                    //IconWork = MainView.FindViewById<TextView>(Resource.Id.IconWork);
                    WorkText = MainView.FindViewById<TextView>(Resource.Id.WorkText);

                    LayoutLive = MainView.FindViewById<LinearLayout>(Resource.Id.LayoutLive);
                    //IconLive = MainView.FindViewById<TextView>(Resource.Id.IconLive);
                    LiveText = MainView.FindViewById<TextView>(Resource.Id.LiveText);

                    LayoutStudy = MainView.FindViewById<LinearLayout>(Resource.Id.LayoutStudy);
                    //IconStudy = MainView.FindViewById<TextView>(Resource.Id.IconStudy);
                    StudyText = MainView.FindViewById<TextView>(Resource.Id.StudyText);

                    LayoutRelationship = MainView.FindViewById<LinearLayout>(Resource.Id.LayoutRelationship);
                    //IconRelationship = MainView.FindViewById<TextView>(Resource.Id.IconRelationship);
                    RelationshipText = MainView.FindViewById<TextView>(Resource.Id.RelationshipText);

                    /*FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconTime, FontAwesomeIcon.Clock);
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconWebsite, FontAwesomeIcon.Globe);
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconGander, FontAwesomeIcon.VenusMars);
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconBirthday, FontAwesomeIcon.BirthdayCake); 
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconWork, FontAwesomeIcon.Briefcase);
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconStudy, FontAwesomeIcon.School); 
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconLive, FontAwesomeIcon.Home); 
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconRelationship, FontAwesomeIcon.Heart);*/

                    WebsiteText.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
             
            public void OnClick(View v)
            {
                try
                {
                    if (v.Id == WebsiteText.Id)
                    {
                        new IntentController(ActivityContext).OpenBrowserFromApp(WebsiteText.Text);
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }
        }
          
        public class InfoGroupBoxViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            private readonly NativePostAdapter PostAdapter;
            public View MainView { get; private set; }
              
            public TextView CategoryText { get; private set; }
            public ImageView IconPrivacy { get; private set; }
            public TextView PrivacyText { get; private set; }
            public TextView TxtMembers { get; private set; }
            public TextView InviteText { get; private set; }

            public InfoGroupBoxViewHolder(View itemView, NativePostAdapter postAdapter) : base(itemView)
            {
                try
                {
                    PostAdapter = postAdapter;
                    MainView = itemView;
                     
                    CategoryText = MainView.FindViewById<TextView>(Resource.Id.CategoryText);
                    IconPrivacy = (ImageView)MainView.FindViewById(Resource.Id.IconPrivacy);
                    PrivacyText = MainView.FindViewById<TextView>(Resource.Id.PrivacyText);
                    TxtMembers = (TextView)MainView.FindViewById(Resource.Id.membersText);
                    InviteText = (TextView)MainView.FindViewById(Resource.Id.InviteText);

                    TxtMembers?.SetOnClickListener(this);
                    InviteText?.SetOnClickListener(this); 
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition];

                        if (v.Id == TxtMembers.Id)
                        {
                            var intent = new Intent(PostAdapter.ActivityContext, typeof(GroupMembersActivity));
                            intent.PutExtra("itemObject", JsonConvert.SerializeObject(item.PrivacyModelClass.GroupClass));
                            intent.PutExtra("GroupId", item.PrivacyModelClass.GroupId);
                            PostAdapter.ActivityContext.StartActivity(intent);
                        }
                        else if (v.Id == InviteText.Id)
                        {
                            var intent = new Intent(PostAdapter.ActivityContext, typeof(InviteMembersGroupActivity));
                            intent.PutExtra("GroupId", item.PrivacyModelClass.GroupId);
                            PostAdapter.ActivityContext.StartActivity(intent);
                        }
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
        }
        
        public class InfoPageBoxViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            private readonly NativePostAdapter PostAdapter;
            public View MainView { get; private set; }
             
            public LinearLayout RatingLiner { get; private set; }
            public RatingBar RatingBarView { get; private set; }

            public TextView IconLike { get; private set; }
            public TextView LikeCountText { get; private set; }

            public TextView IconCategory { get; private set; }
            public TextView CategoryText { get; private set; }

            public LinearLayout MembersLiner { get; private set; }
            public TextView IconMembers { get; private set; } 
         
            public SuperTextView AboutDesc { get; private set; }
              
            public InfoPageBoxViewHolder(View itemView, NativePostAdapter postAdapter) : base(itemView)
            {
                try
                {
                    PostAdapter = postAdapter;
                    MainView = itemView;

                    RatingLiner = (LinearLayout)MainView.FindViewById(Resource.Id.ratingLiner);
                    RatingBarView = (RatingBar)MainView.FindViewById(Resource.Id.ratingBar);

                    IconLike = (TextView)MainView.FindViewById(Resource.Id.IconLike);
                    LikeCountText = (TextView)MainView.FindViewById(Resource.Id.LikeCountText);

                    IconCategory = (TextView)MainView.FindViewById(Resource.Id.IconCategory);
                    CategoryText = (TextView)MainView.FindViewById(Resource.Id.CategoryText);
                     
                    IconMembers = (TextView)MainView.FindViewById(Resource.Id.IconMembers); 
                    MembersLiner = (LinearLayout)MainView.FindViewById(Resource.Id.liner5);

                    AboutDesc = (SuperTextView)MainView.FindViewById(Resource.Id.aboutdesc);

                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconLike, IonIconsFonts.ThumbsUp);
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconMembers, IonIconsFonts.PersonAdd);
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconCategory, IonIconsFonts.Pricetag);
                     
                    RatingLiner?.SetOnClickListener(this);
                    MembersLiner?.SetOnClickListener(this); 
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition];

                        if (v.Id == RatingLiner.Id)
                        {
                            PageProfileActivity.GetInstance()?.RatingLinerOnClick();
                        }
                        else if (v.Id == MembersLiner.Id)
                        {
                            var intent = new Intent(PostAdapter.ActivityContext, typeof(InviteMembersPageActivity));
                            intent.PutExtra("PageId", item.PageInfoModelClass.PageId); 
                            PostAdapter.ActivityContext.StartActivity(intent);
                        }
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
        }
        
        public class FollowersViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public RecyclerView FollowersRecyclerView { get; private set; }
            public UserFriendsAdapter FollowersAdapter { get; private set; }
            public TextView AboutHead { get; private set; }
            public TextView AboutMore { get; private set; }

            public FollowersViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    MainView = itemView;
                    FollowersRecyclerView = MainView.FindViewById<RecyclerView>(Resource.Id.Recyler);
                    AboutHead = MainView.FindViewById<TextView>(Resource.Id.headText);
                    AboutMore = MainView.FindViewById<TextView>(Resource.Id.moreText);
                     
                    AboutMore?.SetOnClickListener(this);
                    
                    if (FollowersAdapter != null)
                        return;

                    FollowersRecyclerView?.SetLayoutManager(new LinearLayoutManager(itemView.Context, LinearLayoutManager.Horizontal, false));
                    FollowersAdapter = new UserFriendsAdapter(postAdapter.ActivityContext);
                    FollowersRecyclerView?.SetAdapter(FollowersAdapter);

                    FollowersAdapter.ItemClick += (sender, e) =>
                    {
                        try
                        {
                            var position = e.Position;
                            switch (position)
                            {
                                case < 0:
                                    return;
                            }

                            var user = FollowersAdapter.GetItem(position);
                            switch (user)
                            {
                                case null:
                                    return;
                                default:
                                    WoWonderTools.OpenProfile(postAdapter.ActivityContext, user.UserId, user);
                                    break;
                            }
                        }
                        catch (Exception exception)
                        {
                            Methods.DisplayReportResultTrack(exception);
                        }
                    };
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition];

                        if (v.Id == AboutMore.Id)
                            PostClickListener.OpenAllViewer("FollowersModel", PostAdapter.IdParameter, item);
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
        }

        public class ImagesViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public RecyclerView ImagesRecyclerView { get; private set; }
            public UserPhotosAdapter ImagesAdapter { get; private set; }
            public TextView AboutHead { get; private set; }
            public TextView AboutMore { get; private set; }

            public ImagesViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    ImagesRecyclerView = MainView.FindViewById<RecyclerView>(Resource.Id.Recyler);
                    AboutHead = MainView.FindViewById<TextView>(Resource.Id.headText);
                    AboutMore = MainView.FindViewById<TextView>(Resource.Id.moreText);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    AboutMore.SetOnClickListener(this);

                    if (ImagesAdapter != null)
                        return;
                     
                    ImagesRecyclerView.SetLayoutManager(new LinearLayoutManager(itemView.Context, LinearLayoutManager.Horizontal, false));
                    ImagesAdapter = new UserPhotosAdapter(postAdapter.ActivityContext);
                    ImagesRecyclerView.SetAdapter(ImagesAdapter);

                    ImagesAdapter.ItemClick += (sender, e) =>
                    {
                        var position = e.Position;
                        switch (position)
                        {
                            case < 0:
                                return;
                        }

                        var photo = ImagesAdapter.GetItem(position);
                        switch (photo)
                        {
                            case null:
                                return;
                            default:
                                postClickListener.OpenImageLightBox(photo);
                                break;
                        }
                    };
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition];

                        if (v.Id == AboutMore?.Id)
                            PostClickListener.OpenAllViewer("ImagesModel", PostAdapter.IdParameter, item);
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

        }

        public class PagesSocialViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            private readonly SocialAdapter SocialAdapter;
            private readonly Activity ActivityContext;

            public View MainView { get; private set; }
            public RecyclerView PagesRecyclerView { get; private set; }
            public TextView AboutHead { get; private set; }
            public TextView AboutMore { get; private set; }
            public PagesSocialViewHolder(Activity activity, View itemView, SocialAdapter socialAdapter) : base(itemView)
            {
                try
                {
                    SocialAdapter = socialAdapter;
                    ActivityContext = activity;
                     
                    MainView = itemView;
                    PagesRecyclerView = MainView.FindViewById<RecyclerView>(Resource.Id.Recyler);
                    AboutHead = MainView.FindViewById<TextView>(Resource.Id.headText);
                    AboutMore = MainView.FindViewById<TextView>(Resource.Id.moreText);

                    AboutMore?.SetOnClickListener(this); 
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
             
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = SocialAdapter.SocialList[AdapterPosition];

                        if (v.Id == AboutMore?.Id)
                        { 
                            var intent = new Intent(ActivityContext, typeof(AllViewerActivity));
                            intent.PutExtra("Type", "MangedPagesModel"); //MangedGroupsModel , MangedPagesModel 
                            intent.PutExtra("PassedId", UserDetails.UserId);
                            intent.PutExtra("itemObject", JsonConvert.SerializeObject(item.PagesModelClass));
                            ActivityContext.StartActivity(intent);
                        }
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            } 
        }

        public class GroupsSocialViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            private readonly SocialAdapter SocialAdapter;
            private readonly Activity ActivityContext;

            public View MainView { get; private set; }
            public RecyclerView GroupsRecyclerView { get; private set; }
          
            public TextView AboutHead { get; private set; }
            public TextView AboutMore { get; private set; }

            public GroupsSocialViewHolder(Activity activity, View itemView, SocialAdapter socialAdapter) : base(itemView)
            {
                try
                {
                    SocialAdapter = socialAdapter;
                    ActivityContext = activity;

                    MainView = itemView;
                    GroupsRecyclerView = MainView.FindViewById<RecyclerView>(Resource.Id.Recyler);
                    AboutHead = MainView.FindViewById<TextView>(Resource.Id.headText);
                    AboutMore = MainView.FindViewById<TextView>(Resource.Id.moreText);

                    AboutMore?.SetOnClickListener(this); 
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
             
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = SocialAdapter.SocialList[AdapterPosition];

                        if (v.Id == AboutMore?.Id)
                        {
                            var intent = new Intent(ActivityContext, typeof(AllViewerActivity));
                            intent.PutExtra("Type", "MangedGroupsModel");  
                            intent.PutExtra("PassedId", UserDetails.UserId);
                            intent.PutExtra("itemObject", JsonConvert.SerializeObject(item.MangedGroupsModel));
                            ActivityContext.StartActivity(intent);
                        }
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            } 
        }
         
        public class GroupsViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public RecyclerView GroupsRecyclerView { get; private set; }
            public UserGroupsAdapter GroupsAdapter { get; private set; }

            public TextView AboutHead { get; private set; }
            public TextView AboutMore { get; private set; }
            public GroupsViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    MainView = itemView;
                    GroupsRecyclerView = MainView.FindViewById<RecyclerView>(Resource.Id.Recyler);
                    AboutHead = MainView.FindViewById<TextView>(Resource.Id.headText);
                    AboutMore = MainView.FindViewById<TextView>(Resource.Id.moreText);

                    AboutMore?.SetOnClickListener(this);

                    if (GroupsAdapter != null)
                        return;

                    GroupsRecyclerView?.SetLayoutManager(new LinearLayoutManager(itemView.Context, LinearLayoutManager.Horizontal, false));
                    GroupsAdapter = new UserGroupsAdapter(postAdapter.ActivityContext);
                    GroupsRecyclerView?.SetAdapter(GroupsAdapter);
                    GroupsRecyclerView?.AddOnItemTouchListener(new RecyclerViewOnItemTouch(GroupsRecyclerView, TabbedMainActivity.GetInstance()?.ViewPager));
                    GroupsAdapter.ItemClick += (sender, e) =>
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

                            MainApplication.GetInstance()?.NavigateTo(postAdapter.ActivityContext, typeof(GroupProfileActivity), item);
                        }
                        catch (Exception x)
                        {
                            Methods.DisplayReportResultTrack(x);
                        }
                    };
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition];

                        if (v.Id == AboutMore?.Id)
                            PostClickListener.OpenAllViewer("GroupsModel", PostAdapter.IdParameter, item);
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

        }

        public class SuggestedUsersViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;

            public RecyclerView UsersRecyclerView { get; private set; }
            public SuggestionsUserAdapter UsersAdapter { get; private set; }

            public TextView AboutHead { get; private set; }
            public TextView AboutMore { get; private set; }

            public SuggestedUsersViewHolder(View itemView, NativePostAdapter postAdapter) : base(itemView)
            {
                try
                {
                    PostAdapter = postAdapter;

                    MainView = itemView;
                    UsersRecyclerView = MainView.FindViewById<RecyclerView>(Resource.Id.Recyler);
                    AboutHead = MainView.FindViewById<TextView>(Resource.Id.headText);
                    AboutMore = MainView.FindViewById<TextView>(Resource.Id.moreText);

                    if (AboutHead != null)
                        AboutHead.Text = postAdapter.ActivityContext.GetString(Resource.String.Lbl3_SuggestionsUsers);
                     
                    AboutMore.Text = PostAdapter.ActivityContext.GetText(Resource.String.Lbl_SeeAll);
                    AboutMore.SetTextColor(new Color(MainView.Context.GetColor(Resource.Color.primary)));
                     
                    AboutMore?.SetOnClickListener(this);

                    if (UsersAdapter != null)
                        return;
                     
                    UsersRecyclerView?.SetLayoutManager(new LinearLayoutManager(itemView.Context, LinearLayoutManager.Horizontal, false));
                    UsersAdapter = new SuggestionsUserAdapter(postAdapter.ActivityContext)
                    {
                        UserList = new ObservableCollection<UserDataObject>(ListUtils.SuggestedUserList.Take(12))
                    };
                    UsersRecyclerView?.SetAdapter(UsersAdapter);
                    UsersRecyclerView?.AddOnItemTouchListener(new RecyclerViewOnItemTouch(UsersRecyclerView, TabbedMainActivity.GetInstance()?.ViewPager));
                    UsersAdapter.ItemClick += (sender, e) =>
                    {
                        try
                        {
                            var position = e.Position;
                            switch (position)
                            {
                                case < 0:
                                    return;
                            }

                            var item = UsersAdapter.GetItem(position);
                            switch (item)
                            {
                                case null:
                                    return;
                                default:
                                    WoWonderTools.OpenProfile(postAdapter.ActivityContext, item.UserId, item);
                                    break;
                            }
                        }
                        catch (Exception x)
                        {
                            Methods.DisplayReportResultTrack(x);
                        }
                    };
                    UsersAdapter.FollowButtonItemClick += OnFollowButtonItemClick;

                    if (AboutMore != null) AboutMore.Visibility = UsersAdapter?.UserList?.Count > 4 ? ViewStates.Visible : ViewStates.Invisible;
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            private void OnFollowButtonItemClick(object sender, SuggestionsUserAdapterClickEventArgs e)
            {
                try
                {
                    if (!Methods.CheckConnectivity())
                    {
                        Toast.MakeText(UsersAdapter.ActivityContext, UsersAdapter.ActivityContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    }
                    else
                    {
                        switch (e.Position)
                        {
                            case > -1:
                            {
                                UserDataObject item = UsersAdapter.GetItem(e.Position);
                                if (item != null)
                                {
                                    WoWonderTools.SetAddFriend(UsersAdapter.ActivityContext, item, e.BtnAddUser);
                                }

                                break;
                            }
                        }
                    }
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
                        if (v.Id == AboutMore?.Id)
                        {
                            var intent = new Intent(PostAdapter.ActivityContext, typeof(SuggestionsUsersActivity));
                            intent.PutExtra("class", "newsFeed");
                            PostAdapter.ActivityContext.StartActivity(intent);
                        }
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            } 
        }

        public class SuggestedGroupsViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;

            public RecyclerView GroupsRecyclerView { get; private set; }
            public SuggestedGroupAdapter GroupsAdapter { get; private set; }

            public TextView AboutHead { get; private set; }
            public TextView AboutMore { get; private set; }

            public SuggestedGroupsViewHolder(View itemView, NativePostAdapter postAdapter) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    GroupsRecyclerView = MainView.FindViewById<RecyclerView>(Resource.Id.Recyler);
                    AboutHead = MainView.FindViewById<TextView>(Resource.Id.headText);
                    AboutMore = MainView.FindViewById<TextView>(Resource.Id.moreText);

                    AboutHead.Text = postAdapter.ActivityContext.GetString(Resource.String.Lbl_SuggestedGroups);
                    AboutMore.Text = PostAdapter.ActivityContext.GetText(Resource.String.Lbl_SeeAll);
                    AboutMore.SetTextColor(new Color(MainView.Context.GetColor(Resource.Color.primary)));
                     
                    PostAdapter = postAdapter;

                    AboutMore.SetOnClickListener(this);
                      
                    if (GroupsAdapter != null)
                        return;

                    GroupsRecyclerView.SetLayoutManager(new LinearLayoutManager(itemView.Context, LinearLayoutManager.Horizontal, false));
                    GroupsAdapter = new SuggestedGroupAdapter(postAdapter.ActivityContext)
                    {
                        GroupList = new ObservableCollection<GroupClass>(ListUtils.SuggestedGroupList.Take(12))
                    };
                    GroupsRecyclerView.SetAdapter(GroupsAdapter);
                    GroupsRecyclerView?.AddOnItemTouchListener(new RecyclerViewOnItemTouch(GroupsRecyclerView, TabbedMainActivity.GetInstance()?.ViewPager));
                    GroupsAdapter.ItemClick += (sender, e) =>
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

                            MainApplication.GetInstance()?.NavigateTo(postAdapter.ActivityContext, typeof(GroupProfileActivity), item);
                        }
                        catch (Exception x)
                        {
                            Console.WriteLine(x);
                        }
                    };
                    GroupsAdapter.JoinButtonItemClick += async (sender, e) =>
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

                            if (!Methods.CheckConnectivity())
                            {
                                Toast.MakeText(postAdapter.ActivityContext, postAdapter.ActivityContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                                return;
                            }
                             
                            var (apiStatus, respond) = await RequestsAsync.Group.JoinGroupAsync(item.GroupId);
                            switch (apiStatus)
                            {
                                case 200:
                                {
                                    switch (respond)
                                    {
                                        case JoinGroupObject result when result.JoinStatus == "requested":
                                            e.JoinButton.SetTextColor(Color.White);
                                            e.JoinButton.Text = Application.Context.GetText(Resource.String.Lbl_Request);
                                            e.JoinButton.SetBackgroundResource(Resource.Drawable.buttonFlatGray);
                                            break;
                                        case JoinGroupObject result:
                                        {
                                            var isJoined = result.JoinStatus == "left" ? "false" : "true";
                                            e.JoinButton.Text = postAdapter.ActivityContext.GetText(isJoined == "yes" || isJoined == "true" ? Resource.String.Btn_Joined : Resource.String.Btn_Join_Group);

                                            switch (isJoined)
                                            {
                                                case "yes":
                                                case "true":
                                                    e.JoinButton.SetBackgroundResource(Resource.Drawable.buttonFlatGray);
                                                    e.JoinButton.SetTextColor(Color.White);
                                                    break;
                                                default:
                                                    e.JoinButton.SetBackgroundResource(Resource.Drawable.buttonFlat);
                                                    e.JoinButton.SetTextColor(Color.White);
                                                    break;
                                            }

                                            break;
                                        }
                                    }

                                    break;
                                }
                                default:
                                    Methods.DisplayReportResult(postAdapter.ActivityContext, respond);
                                    break;
                            }
                        }
                        catch (Exception x)
                        {
                            Console.WriteLine(x);
                        }
                    };

                    AboutMore.Visibility = GroupsAdapter?.GroupList?.Count switch
                    {
                        > 4 => ViewStates.Visible,
                        _ => ViewStates.Invisible
                    };
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        if (v.Id == AboutMore.Id)
                        {
                            var intent = new Intent(PostAdapter.ActivityContext, typeof(SuggestedGroupActivity));
                            PostAdapter.ActivityContext.StartActivity(intent);
                        }
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

        }

        public class PagesViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public RecyclerView PagesRecyclerView { get; private set; }
            public UserPagesAdapter PagesAdapter { get; private set; }

            public TextView AboutHead { get; private set; }
            public TextView AboutMore { get; private set; }
            public PagesViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    MainView = itemView;
                    PagesRecyclerView = MainView.FindViewById<RecyclerView>(Resource.Id.Recyler);
                    AboutHead = MainView.FindViewById<TextView>(Resource.Id.headText);
                    AboutMore = MainView.FindViewById<TextView>(Resource.Id.moreText);

                    AboutMore.Visibility = ViewStates.Gone;
                    AboutMore?.SetOnClickListener(this);

                    if (PagesAdapter != null)
                        return;

                    PagesRecyclerView?.SetLayoutManager(new LinearLayoutManager(itemView.Context, LinearLayoutManager.Horizontal, false));
                    PagesAdapter = new UserPagesAdapter(postAdapter.ActivityContext);
                    PagesRecyclerView?.SetAdapter(PagesAdapter);
                    PagesAdapter.ItemClick += (sender, e) =>
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
                            }

                            /*if (UserDetails.UserId == item.UserId)
                                item.IsOwner = true;*/

                            MainApplication.GetInstance()?.NavigateTo(postAdapter.ActivityContext, typeof(PageProfileActivity), item);
                        }
                        catch (Exception x)
                        {
                            Methods.DisplayReportResultTrack(x);
                        }
                    };
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition];

                        if (v.Id == AboutMore?.Id)
                            PostClickListener.OpenAllViewer("PagesModel", PostAdapter.IdParameter, item);
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }
        }
        /*public class PagesViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;

            public RelativeLayout LayoutSuggestionPages { get; private set; }
            public ImageView PageImage1 { get; private set; }
            public ImageView PageImage2 { get; private set; }
            public ImageView PageImage3 { get; private set; }
            public TextView AboutHead { get; private set; }
            public TextView AboutMore { get; private set; }

            public PagesViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    LayoutSuggestionPages = MainView.FindViewById<RelativeLayout>(Resource.Id.layout_suggestion_Pages);
                    PageImage1 = MainView.FindViewById<ImageView>(Resource.Id.image_page_1);
                    PageImage2 = MainView.FindViewById<ImageView>(Resource.Id.image_page_2);
                    PageImage3 = MainView.FindViewById<ImageView>(Resource.Id.image_page_3);

                    AboutHead = MainView.FindViewById<TextView>(Resource.Id.headText);
                    AboutMore = MainView.FindViewById<TextView>(Resource.Id.moreText);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;
                     
                    LayoutSuggestionPages.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition];

                        if (v.Id == LayoutSuggestionPages.Id)
                            PostClickListener.OpenAllViewer("PagesModel", PostAdapter.IdParameter, item);
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            } 
        }*/

        public class EmptyStateAdapterViewHolder : RecyclerView.ViewHolder
        {
            public View MainView { get; private set; }
            public TextView EmptyText { get; private set; }
            public ImageView EmptyImage { get; private set; }

            public EmptyStateAdapterViewHolder(View itemView) : base(itemView)
            {
                MainView = itemView;
                EmptyText = MainView.FindViewById<TextView>(Resource.Id.textEmpty);
                EmptyImage = MainView.FindViewById<ImageView>(Resource.Id.imageEmpty);
            }
        }

        public class AlertAdapterViewHolder : RecyclerView.ViewHolder
        {
            public View MainView { get; private set; }
            public RelativeLayout MianAlert { get; private set; }
            public TextView HeadText { get; private set; }
            public TextView SubText { get; private set; }
            public View LineView { get; private set; }
            public RoundedImageView Image { get; private set; }
            public NativePostAdapter Adapter { get; private set; }
            public AlertAdapterViewHolder(View itemView, NativePostAdapter adapter , PostModelType viewType) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    Adapter = adapter;

                    MianAlert = MainView.FindViewById<RelativeLayout>(Resource.Id.main);

                    LineView = MainView.FindViewById<View>(Resource.Id.lineview);
                    HeadText = MainView.FindViewById<TextView>(Resource.Id.HeadText);
                    SubText = MainView.FindViewById<TextView>(Resource.Id.subText);
                    Image = MainView.FindViewById<RoundedImageView>(Resource.Id.Image);

                    switch (MianAlert.HasOnClickListeners)
                    {
                        case false:
                            MianAlert.Click += (sender, args) =>
                            {
                                try
                                {
                                    switch (viewType)
                                    {
                                        case PostModelType.AlertBox:
                                        {
                                            var data = Adapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AlertBox);
                                            if (data != null)
                                            {
                                                TabbedMainActivity.GetInstance()?.NewsFeedTab.MainRecyclerView.RemoveByRowIndex(data);
                                            }

                                            break;
                                        }
                                        default:
                                        {
                                            var data = Adapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.AlertBoxAnnouncement);
                                            if (data != null)
                                            {
                                                TabbedMainActivity.GetInstance()?.NewsFeedTab.MainRecyclerView.RemoveByRowIndex(data);
                                            }

                                            break;
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Methods.DisplayReportResultTrack(e); 
                                }
                            };
                            break;
                    }

                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
        }

        public class AdMobAdapterViewHolder : RecyclerView.ViewHolder
        {
            public View MainView { get; private set; }
            public TemplateView MianAlert { get; private set; }

            public AdMobAdapterViewHolder(View itemView, NativePostAdapter postAdapter) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    MianAlert = MainView.FindViewById<TemplateView>(Resource.Id.my_template);
                    MianAlert.Visibility = ViewStates.Gone;

                   postAdapter.BindAdMob(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
        }
       
        public class AdMob3AdapterViewHolder : RecyclerView.ViewHolder
        {
            public View MainView { get; private set; }
            public PublisherAdView PublisherAdView { get; private set; }

            public AdMob3AdapterViewHolder(View itemView) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    PublisherAdView = MainView.FindViewById<PublisherAdView>(Resource.Id.multiple_ad_sizes_view);
                    PublisherAdView.Visibility = ViewStates.Gone;
                    AdsGoogle.InitPublisherAdView(PublisherAdView); 
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
        }
        
        public class FbAdNativeAdapterViewHolder : RecyclerView.ViewHolder
        {
            public View MainView { get; private set; }
            public LinearLayout NativeAdLayout { get; private set; }

            public FbAdNativeAdapterViewHolder(Activity activity ,View itemView , NativePostAdapter postAdapter) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    NativeAdLayout = itemView.FindViewById<LinearLayout>(Resource.Id.native_ad_container);
                    NativeAdLayout.Visibility = ViewStates.Gone;

                    switch (postAdapter.MAdItems.Count)
                    {
                        case > 0:
                        {
                            var ad = postAdapter.MAdItems.FirstOrDefault();
                            AdsFacebook.InitNative(activity, NativeAdLayout, ad);
                            postAdapter.MAdItems.Remove(ad);
                            break;
                        }
                        default:
                            AdsFacebook.InitNative(activity, NativeAdLayout, null);
                            break;
                    }
                    postAdapter.BindAdFb();
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
        }

        public class AlertJoinAdapterViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            public NativePostAdapter PostAdapter { get; private set; }
            public RelativeLayout MainRelativeLayout { get; private set; }
            public TextView HeadText { get; private set; }
            public TextView SubText { get; private set; }
            public Button ButtonView { get; private set; }
            public ImageView IconImageView { get; private set; }
            public ImageView NormalImageView { get; private set; }

            public AlertJoinAdapterViewHolder(View itemView ,  NativePostAdapter postAdapter) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    PostAdapter = postAdapter;

                    MainRelativeLayout = MainView.FindViewById<RelativeLayout>(Resource.Id.mainview);
                    ButtonView = MainView.FindViewById<Button>(Resource.Id.buttonview);
                    HeadText = MainView.FindViewById<TextView>(Resource.Id.HeadText);
                    SubText = MainView.FindViewById<TextView>(Resource.Id.subText);
                    IconImageView = MainView.FindViewById<ImageView>(Resource.Id.IconImageview);
                    NormalImageView = MainView.FindViewById<ImageView>(Resource.Id.Imageview);

                    ButtonView.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        if (v.Id == ButtonView.Id)
                        {
                            var item = PostAdapter.ListDiffer[AdapterPosition];

                            var intent = new Intent(PostAdapter.ActivityContext, typeof(SearchTabbedActivity));

                            switch (item.AlertModel?.TypeAlert)
                            {
                                case "Pages":
                                    intent.PutExtra("Key", "Random_Pages");
                                    break;
                                case "Groups":
                                    intent.PutExtra("Key", "Random_Groups");
                                    break;
                            }

                            PostAdapter.ActivityContext.StartActivity(intent);
                        } 
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
        }

        public class SectionViewHolder : RecyclerView.ViewHolder
        {
            public View MainView { get; private set; }
            public TextView AboutHead { get; private set; }
            public TextView AboutMore { get; private set; }

            public SectionViewHolder(View itemView) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    AboutHead = MainView.FindViewById<TextView>(Resource.Id.headText);
                    AboutMore = MainView.FindViewById<TextView>(Resource.Id.moreText);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
        }
        
        public class FilterSectionViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            public TextView AboutHead { get; private set; }
            public TextView AboutMore { get; private set; }

            public FilterSectionViewHolder(View itemView) : base(itemView)
            {
                try
                {
                    MainView = itemView;

                    AboutHead = MainView.FindViewById<TextView>(Resource.Id.headText);
                    AboutMore = MainView.FindViewById<TextView>(Resource.Id.moreText);

                    AboutMore.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        if (v.Id == AboutMore.Id)
                        {
                            var arrayAdapter = new List<string>();
                            var dialogList = new MaterialDialog.Builder(MainView.Context).Theme(AppSettings.SetTabDarkTheme ? Theme.Dark : Theme.Light);

                            arrayAdapter.Add(MainView.Context.GetString(Resource.String.Lbl_All));
                            arrayAdapter.Add(MainView.Context.GetString(Resource.String.Lbl_People_i_Follow));
                             
                            dialogList.Title(MainView.Context.GetString(Resource.String.Lbl_Filter)).TitleColorRes(Resource.Color.primary);
                            dialogList.Items(arrayAdapter);
                            dialogList.PositiveText(MainView.Context.GetText(Resource.String.Lbl_Close)).OnPositive((p0, p1) =>
                            {
                                try
                                {
                                    p0.Dismiss();
                                }
                                catch (Exception e)
                                {
                                    Methods.DisplayReportResultTrack(e);
                                }
                            });
                            dialogList.AlwaysCallSingleChoiceCallback();
                            dialogList.ItemsCallback((p0, p1, itemId, itemString) =>
                            {
                                try
                                {
                                    AboutHead.Text = itemString.ToString();
                                    WRecyclerView.GetInstance()?.SetFilter(itemId.ToString());
                                }
                                catch (Exception e)
                                {
                                    Methods.DisplayReportResultTrack(e);
                                }
                            });
                            dialogList.Build().Show();
                        }
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
        }

        public class PostDividerSectionViewHolder : RecyclerView.ViewHolder
        {
          
            public PostDividerSectionViewHolder(View itemView) : base(itemView)
            {
                 
            }
        }
          
        public class PostAddCommentSectionViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
        {
            public View MainView { get; private set; }
            private readonly NativePostAdapter PostAdapter;
            private readonly PostClickListener PostClickListener;
            public CircleImageView ProfileImageView { get; private set; }
            public LinearLayout AddCommentLayout { get; private set; }
            public LinearLayout LayoutEditText { get; private set; }
            public ImageView EmojiIcon { get; private set; }
            public ImageView ImageIcon { get; private set; }
            public TextView AddCommentTextView { get; private set; }

            public PostAddCommentSectionViewHolder(View itemView, NativePostAdapter postAdapter, PostClickListener postClickListener) : base(itemView)
            {
                try
                {
                    MainView = itemView;
                    ProfileImageView = MainView.FindViewById<CircleImageView>(Resource.Id.image);

                    AddCommentLayout = MainView.FindViewById<LinearLayout>(Resource.Id.addCommentLayout);
                    LayoutEditText = MainView.FindViewById<LinearLayout>(Resource.Id.LayoutEditText);
                    AddCommentTextView = MainView.FindViewById<TextView>(Resource.Id.postText);
                    EmojiIcon = MainView.FindViewById<ImageView>(Resource.Id.Emojiicon);
                    ImageIcon = MainView.FindViewById<ImageView>(Resource.Id.Imageicon);

                    PostAdapter = postAdapter;
                    PostClickListener = postClickListener;

                    AddCommentLayout.SetOnClickListener(this);
                    LayoutEditText.SetOnClickListener(this);
                    ImageIcon.SetOnClickListener(this);
                    EmojiIcon.SetOnClickListener(this); 
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public void OnClick(View v)
            {
                try
                {
                    if (AdapterPosition != RecyclerView.NoPosition)
                    {
                        var item = PostAdapter.ListDiffer[AdapterPosition]?.PostData;

                        if (v.Id == ImageIcon.Id)
                            PostClickListener.CommentPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView } , "Normal_Gallery");
                        else if (v.Id == EmojiIcon.Id)
                            PostClickListener.CommentPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView } , "Normal_EmojiIcon");
                        else if (v.Id == AddCommentLayout.Id || v.Id == LayoutEditText.Id)
                            PostClickListener.CommentPostClick(new GlobalClickEventArgs { NewsFeedClass = item, Position = AdapterPosition, View = MainView });
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e); 
                }
            }
        }

        public class PostDefaultSectionViewHolder : RecyclerView.ViewHolder
        {
          
            public PostDefaultSectionViewHolder(View itemView) : base(itemView)
            {
                 
            }
        } 

        public class PostViewHolder : RecyclerView.ViewHolder
        {
            public View LineView { get; private set; }
            public PostViewHolder(View itemView) : base(itemView)
            {
                try
                {
                    LineView = itemView.FindViewById<TextView>(Resource.Id.simpleViewAnimator);
                    LineView.SetBackgroundColor(Color.Transparent);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }
        } 

        public class ProgressViewHolder : RecyclerView.ViewHolder
        {
            public ProgressBar ProgressBar { get; private set; }

            public ProgressViewHolder(View itemView) : base(itemView)
            {
                try
                {
                    ProgressBar = (ProgressBar)itemView.FindViewById(Resource.Id.progress_bar);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }
        }
        
        public class PromoteHolder : RecyclerView.ViewHolder
        {
            public RelativeLayout PromoteLayout { get; private set; }
            public PromoteHolder(View itemView) : base(itemView)
            {
                try
                {
                    PromoteLayout = (RelativeLayout)itemView.FindViewById(Resource.Id.promoteLayout);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }
        }

        public class ProfileHeaderSectionHolder : RecyclerView.ViewHolder
        {
            public LinearLayout MainLayout { get; private set; }
            public TextView CountFollowers { get; private set; }
            public TextView CountFollowings { get; private set; }
            public TextView CountLikes { get; private set; }
            public TextView CountPoints { get; private set; }
            public TextView TxtFollowers { get; private set; }
            public TextView TxtFollowing { get; private set; }
            public LinearLayout LlCountFollowers { get; private set; }
            public LinearLayout LlCountFollowing { get; private set; }
            public LinearLayout LlCountLike { get; private set; }
            public LinearLayout LlPoint { get; private set; }

            public ProfileHeaderSectionHolder(View itemView) : base(itemView)
            {
                try
                {
                    MainLayout = itemView.FindViewById<LinearLayout>(Resource.Id.mainLayout);

                    CountFollowers = itemView.FindViewById<TextView>(Resource.Id.CountFollowers);
                    CountFollowings = itemView.FindViewById<TextView>(Resource.Id.CountFollowing);
                    CountLikes = itemView.FindViewById<TextView>(Resource.Id.CountLikes);
                    CountPoints = itemView.FindViewById<TextView>(Resource.Id.CountPoints);

                    TxtFollowers = itemView.FindViewById<TextView>(Resource.Id.txtFollowers);
                    TxtFollowing = itemView.FindViewById<TextView>(Resource.Id.txtFollowing);

                    LlCountFollowers = itemView.FindViewById<LinearLayout>(Resource.Id.CountFollowersLayout);
                    LlCountFollowing = itemView.FindViewById<LinearLayout>(Resource.Id.CountFollowingLayout);
                    LlCountLike = itemView.FindViewById<LinearLayout>(Resource.Id.CountLikesLayout);
                    LlPoint = itemView.FindViewById<LinearLayout>(Resource.Id.CountPointsLayout);

                    switch (AppSettings.ConnectivitySystem)
                    {
                        // Following
                        case 1:
                            TxtFollowers.Text = itemView.Context.GetText(Resource.String.Lbl_Followers);
                            TxtFollowing.Text = itemView.Context.GetText(Resource.String.Lbl_Following);
                            break;
                        // Friend
                        default:
                            TxtFollowers.Text = itemView.Context.GetText(Resource.String.Lbl_Friends);
                            TxtFollowing.Text = itemView.Context.GetText(Resource.String.Lbl_Post);
                            break;
                    }

                    if (!AppSettings.ShowUserPoint)
                    {
                        LlPoint.Visibility = ViewStates.Gone;
                    }

                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

        }

    }
}
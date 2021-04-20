using System;
using System.Linq;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AT.Markushi.UI;
using Refractored.Controls; 
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.Library.Anjo.SuperTextLibrary;

namespace WoWonder.Activities.Comment.Adapters
{
    public class CommentAdapterViewHolder : RecyclerView.ViewHolder, View.IOnClickListener, View.IOnLongClickListener
    {
        #region Variables Basic

        public View MainView { get; private set; }
        public CommentAdapter CommentAdapter;
        private readonly ReplyCommentAdapter ReplyCommentAdapter;
        private readonly CommentClickListener PostClickListener;
        private readonly string TypeClass;

        public RelativeLayout MainCommentLayout { get; private set; }
        public LinearLayout BubbleLayout { get; private set; }
        public CircleImageView Image { get; private set; }
        public SuperTextView CommentText { get; private set; }
        public TextView TimeTextView { get; private set; }
        public TextView UserName { get; private set; }
        public TextView ReplyTextView { get; private set; }
        public TextView LikeTextView { get; private set; }
        public TextView DislikeTextView { get; private set; }

        public ImageView CommentImage { get; private set; }

        public LinearLayout VoiceLayout { get; private set; }
        public CircleButton PlayButton { get; private set; }
        public TextView DurationVoice { get; private set; }
        public TextView TimeVoice { get; private set; }

        public LinearLayout CountLikeSection { get; private set; }
        public TextView CountLike { get; private set; }
        public ImageView ImageCountLike { get; private set; }

        #endregion

        //Comment
        public CommentAdapterViewHolder(View itemView, CommentAdapter commentAdapter, CommentClickListener postClickListener , string typeClass = "Comment") : base(itemView)
        {
            try
            {
                MainView = itemView;

                CommentAdapter = commentAdapter;
                PostClickListener = postClickListener;
                TypeClass = typeClass;

                MainCommentLayout = MainView.FindViewById<RelativeLayout>(Resource.Id.mainComment);
                BubbleLayout = MainView.FindViewById<LinearLayout>(Resource.Id.bubble_layout);
                Image = MainView.FindViewById<CircleImageView>(Resource.Id.card_pro_pic);
                CommentText = MainView.FindViewById<SuperTextView>(Resource.Id.active);
                CommentText?.SetTextInfo(CommentText);

                UserName = MainView.FindViewById<TextView>(Resource.Id.username);
                TimeTextView = MainView.FindViewById<TextView>(Resource.Id.time);
                ReplyTextView = MainView.FindViewById<TextView>(Resource.Id.reply);
                LikeTextView = MainView.FindViewById<TextView>(Resource.Id.Like);
                DislikeTextView = MainView.FindViewById<TextView>(Resource.Id.dislike);
                CommentImage = MainView.FindViewById<ImageView>(Resource.Id.image);
                CountLikeSection = MainView.FindViewById<LinearLayout>(Resource.Id.countLikeSection);
                CountLike = MainView.FindViewById<TextView>(Resource.Id.countLike);
                ImageCountLike = MainView.FindViewById<ImageView>(Resource.Id.ImagecountLike);
                CountLikeSection.Visibility = ViewStates.Gone;
                try
                {
                    VoiceLayout = MainView.FindViewById<LinearLayout>(Resource.Id.voiceLayout);
                    PlayButton = MainView.FindViewById<CircleButton>(Resource.Id.playButton);
                    DurationVoice = MainView.FindViewById<TextView>(Resource.Id.Duration);
                    TimeVoice = MainView.FindViewById<TextView>(Resource.Id.timeVoice);

                    PlayButton?.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
                  
                var font = Typeface.CreateFromAsset(MainView.Context.Resources?.Assets, "ionicons.ttf");
                UserName.SetTypeface(font, TypefaceStyle.Normal);

                switch (AppSettings.FlowDirectionRightToLeft)
                {
                    case true:
                        BubbleLayout.SetBackgroundResource(Resource.Drawable.comment_rounded_right_layout);
                        break;
                }

                switch (AppSettings.PostButton)
                {
                    case PostButtonSystem.DisLike:
                    case PostButtonSystem.Wonder:
                        DislikeTextView.Visibility = ViewStates.Visible;
                        break;
                }

                /*ReplyTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                LikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                DislikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);*/
                ReplyTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.ParseColor("#888888"));
                LikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.ParseColor("#888888"));
                DislikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.ParseColor("#888888"));

                MainView.SetOnLongClickListener(this);
                Image.SetOnClickListener(this);
                LikeTextView.SetOnClickListener(this);
                DislikeTextView.SetOnClickListener(this);
                ReplyTextView.SetOnClickListener(this);
                CommentImage?.SetOnClickListener(this);
                CountLikeSection?.SetOnClickListener(this); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Reply
        public CommentAdapterViewHolder(View itemView, ReplyCommentAdapter commentAdapter, CommentClickListener postClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                ReplyCommentAdapter = commentAdapter;
                PostClickListener = postClickListener;
                TypeClass = "Reply";

                MainCommentLayout = MainView.FindViewById<RelativeLayout>(Resource.Id.mainComment);
                BubbleLayout = MainView.FindViewById<LinearLayout>(Resource.Id.bubble_layout);
                Image = MainView.FindViewById<CircleImageView>(Resource.Id.card_pro_pic);
                CommentText = MainView.FindViewById<SuperTextView>(Resource.Id.active);
                CommentText?.SetTextInfo(CommentText);

                UserName = MainView.FindViewById<TextView>(Resource.Id.username);
                TimeTextView = MainView.FindViewById<TextView>(Resource.Id.time);
                ReplyTextView = MainView.FindViewById<TextView>(Resource.Id.reply);
                LikeTextView = MainView.FindViewById<TextView>(Resource.Id.Like);
                DislikeTextView = MainView.FindViewById<TextView>(Resource.Id.dislike);
                CommentImage = MainView.FindViewById<ImageView>(Resource.Id.image);
                CountLikeSection = MainView.FindViewById<LinearLayout>(Resource.Id.countLikeSection);
                CountLike = MainView.FindViewById<TextView>(Resource.Id.countLike);
                ImageCountLike = MainView.FindViewById<ImageView>(Resource.Id.ImagecountLike);
                CountLikeSection.Visibility = ViewStates.Gone;
                try
                {
                    VoiceLayout = MainView.FindViewById<LinearLayout>(Resource.Id.voiceLayout);
                    PlayButton = MainView.FindViewById<CircleButton>(Resource.Id.playButton);
                    DurationVoice = MainView.FindViewById<TextView>(Resource.Id.Duration);
                    TimeVoice = MainView.FindViewById<TextView>(Resource.Id.timeVoice);

                    PlayButton?.SetOnClickListener(this);
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }

                var font = Typeface.CreateFromAsset(MainView.Context.Resources?.Assets, "ionicons.ttf");
                UserName.SetTypeface(font, TypefaceStyle.Normal);

                switch (AppSettings.FlowDirectionRightToLeft)
                {
                    case true:
                        BubbleLayout.SetBackgroundResource(Resource.Drawable.comment_rounded_right_layout);
                        break;
                }

                switch (AppSettings.PostButton)
                {
                    case PostButtonSystem.DisLike:
                    case PostButtonSystem.Wonder:
                        DislikeTextView.Visibility = ViewStates.Visible;
                        break;
                }

                ReplyTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                LikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                DislikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                MainView.SetOnLongClickListener(this);
                Image.SetOnClickListener(this);
                LikeTextView.SetOnClickListener(this);
                DislikeTextView.SetOnClickListener(this);
                ReplyTextView.SetOnClickListener(this);
                CommentImage?.SetOnClickListener(this);
                CountLikeSection?.SetOnClickListener(this);
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
                    CommentObjectExtra item = TypeClass switch
                    {
                        "Comment" => CommentAdapter.CommentList[AdapterPosition],
                        "Post" => CommentAdapter.CommentList.FirstOrDefault(danjo =>
                            string.IsNullOrEmpty(danjo.CFile) && string.IsNullOrEmpty(danjo.Record)),
                        "Reply" => ReplyCommentAdapter.ReplyCommentList[AdapterPosition],
                        _ => null!
                    };

                    if (v.Id == Image.Id)
                        PostClickListener.ProfilePostClick(new ProfileClickEventArgs { Holder = this, CommentClass = item, Position = AdapterPosition, View = MainView });
                    else if (v.Id == LikeTextView.Id)
                        PostClickListener.LikeCommentReplyPostClick(new CommentReplyClickEventArgs { Holder = this, CommentObject = item, Position = AdapterPosition, View = MainView });
                    else if (v.Id == DislikeTextView.Id)
                        PostClickListener.DislikeCommentReplyPostClick(new CommentReplyClickEventArgs { Holder = this, CommentObject = item, Position = AdapterPosition, View = MainView });
                    else if (v.Id == ReplyTextView.Id)
                        PostClickListener.CommentReplyPostClick(new CommentReplyClickEventArgs { Holder = this, CommentObject = item, Position = AdapterPosition, View = MainView });
                    else if (v.Id == CommentImage?.Id)
                        PostClickListener.OpenImageLightBox(item);
                    else if (v.Id == PlayButton?.Id)
                        PostClickListener.PlaySound(new CommentReplyClickEventArgs { Holder = this, CommentObject = item, Position = AdapterPosition, View = MainView });
                    else if (v.Id == CountLikeSection?.Id) 
                        PostClickListener.CountLikeCommentReplyPostClick(new CommentReplyClickEventArgs { Holder = this, CommentObject = item, Position = AdapterPosition, View = MainView });
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public bool OnLongClick(View v)
        {
            //add event if System = ReactButton 
            if (AdapterPosition != RecyclerView.NoPosition)
            {
                CommentObjectExtra item = TypeClass switch
                {
                    "Comment" => CommentAdapter.CommentList[AdapterPosition],
                    "Post" => CommentAdapter.CommentList.FirstOrDefault(danjo =>
                        string.IsNullOrEmpty(danjo.CFile) && string.IsNullOrEmpty(danjo.Record)),
                    "Reply" => ReplyCommentAdapter.ReplyCommentList[AdapterPosition],
                    _ => null!
                };

                if (v.Id == MainView.Id)
                    PostClickListener.MoreCommentReplyPostClick(new CommentReplyClickEventArgs { Holder = this, CommentObject = item, Position = AdapterPosition, View = MainView });
            }

            return true;
        } 

    }
}
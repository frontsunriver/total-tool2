using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Content.Res;
using AndroidX.Core.Content;
using AndroidX.Core.Graphics.Drawable;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Com.Tuyenmonkey.Textdecorator;
using Java.Lang;
using WoWonder.Activities.Comment.Adapters;
using WoWonder.Activities.Comment.Fragment;
using WoWonder.Activities.Contacts;
using WoWonder.Activities.SettingsPreferences.TellFriend;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.Library.Anjo;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Classes.Story;
using Exception = System.Exception;
using Reaction = WoWonderClient.Classes.Posts.Reaction;
using String = Java.Lang.String;

namespace WoWonder.Activities.NativePost.Post
{
    public class AdapterBind  
    {
        private readonly Activity ActivityContext;
        private readonly NativePostAdapter NativePostAdapter;
        private string UserId;

        public AdapterBind(NativePostAdapter adapter)
        {
            try
            {
                ActivityContext = adapter.ActivityContext;
                NativePostAdapter = adapter;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public void PromotePostBind(AdapterHolders.PromoteHolder holder ,AdapterModelsClass item)
        {
            try
            {
                bool isPromoted = item.PostData.IsPostBoosted == "1" || item.PostData.SharedInfo.SharedInfoClass != null && item.PostData.SharedInfo.SharedInfoClass?.IsPostBoosted == "1";
                holder.PromoteLayout.Visibility = isPromoted switch
                {
                    false => ViewStates.Gone,
                    _ => holder.PromoteLayout.Visibility
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        } 
         
        public void HeaderPostBind(AdapterHolders.PostTopSectionViewHolder holder ,AdapterModelsClass item)
        {
            try
            {
                UserDataObject publisher = item.PostData.Publisher ?? item.PostData.UserData;

                GlideImageLoader.LoadImage(ActivityContext, item.PostData.PostPrivacy == "4" ? "user_anonymous" : publisher.Avatar, holder.UserAvatar, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);

                switch (item.PostData.PostPrivacy)
                {
                    //Anonymous Post
                    case "4":
                        holder.Username.Text = ActivityContext.GetText(Resource.String.Lbl_Anonymous);
                        break;
                    default:
                        holder.Username.SetText(holder.Username.SetClick(holder.Username, item.PostData, item.PostDataDecoratedContent , holder));
                        break;
                }

                holder.TimeText.Text = item.PostData.Time;

                if (holder.PrivacyPostIcon != null && !string.IsNullOrEmpty(item.PostData.PostPrivacy) && (publisher.UserId == UserDetails.UserId || AppSettings.ShowPostPrivacyForAllUser))
                {
                    switch (item.PostData.PostPrivacy)
                    {
                        //Everyone
                        case "0":
                            holder.PrivacyPostIcon.SetImageResource(Resource.Drawable.icon_privacy_globe);
                            break;
                        default:
                        {
                            if (item.PostData.PostPrivacy.Contains("ifollow") || item.PostData.PostPrivacy == "2") //People_i_Follow
                            {
                                holder.PrivacyPostIcon.SetImageResource(Resource.Drawable.ic_friend);
                            }
                            else if (item.PostData.PostPrivacy.Contains("me") || item.PostData.PostPrivacy == "1") //People_Follow_Me
                            {
                                holder.PrivacyPostIcon.SetImageResource(Resource.Drawable.ic_users);
                            }
                            else switch (item.PostData.PostPrivacy)
                            {
                                //Anonymous
                                case "4":
                                    holder.PrivacyPostIcon.SetImageResource(Resource.Drawable.ic_detective);
                                    break;
                                //No_body) 
                                default:
                                    holder.PrivacyPostIcon.SetImageResource(Resource.Drawable.ic_lock);
                                    break;
                            }

                            break;
                        }
                    }

                    holder.PrivacyPostIcon.Visibility = ViewStates.Visible;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        } 

        public void SharedHeaderPostBind(AdapterHolders.PostTopSharedSectionViewHolder holder ,AdapterModelsClass item)
        {
            try
            { 
                var itemPost = item.PostData;

                UserDataObject publisher = itemPost.Publisher ?? itemPost.UserData;

                switch (itemPost.PostPrivacy)
                {
                    case "4":
                        GlideImageLoader.LoadImage(ActivityContext, "user_anonymous", holder.UserAvatar, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                        break;
                    default:
                        NativePostAdapter.CircleGlideRequestBuilder.Load(publisher.Avatar).Into(holder.UserAvatar);
                        break;
                }

                holder.Username.Text = itemPost.PostPrivacy switch
                {
                    //Anonymous Post
                    "4" => ActivityContext.GetText(Resource.String.Lbl_Anonymous),
                    _ => WoWonderTools.GetNameFinal(publisher)
                };

                holder.TimeText.Text = itemPost.Time;

                if (holder.PrivacyPostIcon != null && !string.IsNullOrEmpty(itemPost.PostPrivacy) && (publisher.UserId == UserDetails.UserId || AppSettings.ShowPostPrivacyForAllUser))
                {
                    switch (itemPost.PostPrivacy)
                    {
                        //Everyone
                        case "0":
                            holder.PrivacyPostIcon.SetImageResource(Resource.Drawable.icon_privacy_globe);
                            break;
                        default:
                        {
                            if (itemPost.PostPrivacy.Contains("ifollow") || itemPost.PostPrivacy == "2") //People_i_Follow
                            {
                                holder.PrivacyPostIcon.SetImageResource(Resource.Drawable.ic_friend);
                            }
                            else if (itemPost.PostPrivacy.Contains("me") || itemPost.PostPrivacy == "1") //People_Follow_Me
                            {
                                holder.PrivacyPostIcon.SetImageResource(Resource.Drawable.ic_users);
                            }
                            else switch (itemPost.PostPrivacy)
                            {
                                //Anonymous
                                case "4":
                                    holder.PrivacyPostIcon.SetImageResource(Resource.Drawable.ic_detective);
                                    break;
                                //No_body) 
                                default:
                                    holder.PrivacyPostIcon.SetImageResource(Resource.Drawable.ic_lock);
                                    break;
                            }

                            break;
                        }
                    }

                    holder.PrivacyPostIcon.Visibility = ViewStates.Visible;
                }

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        } 

        public void PrevBottomPostPartBind(AdapterHolders.PostPrevBottomSectionViewHolder holder ,AdapterModelsClass item)
        {
            try
            { 
                if (holder.CommentCount != null)
                    holder.CommentCount.Text = item.PostData.PostComments + " " + ActivityContext.GetString(Resource.String.Lbl_Comments);

                if (holder.ShareCount != null)
                    holder.ShareCount.Text = item.PostData.PostShares + " " + ActivityContext.GetString(Resource.String.Lbl_Shares);

                holder.ViewCount.Text = item.PostData.PrevButtonViewText;

                if (holder.LikeCount != null)
                {
                    switch (AppSettings.PostButton)
                    {
                        case PostButtonSystem.ReactionDefault:
                        case PostButtonSystem.ReactionSubShine:
                            holder.LikeCount.Text = item.PostData.PostLikes + " " + ActivityContext.GetString(Resource.String.Lbl_Reactions);
                            break;
                        default:
                            holder.LikeCount.Text = item.PostData.PostLikes + " " + ActivityContext.GetString(Resource.String.Btn_Likes);
                            break;
                    }
                }

                switch (AppSettings.PostButton)
                {
                    case PostButtonSystem.ReactionDefault:
                    case PostButtonSystem.ReactionSubShine:
                    {
                        item.PostData.Reaction ??= new Reaction();

                        //holder.ImageCountLike.Visibility = item.PostData.Reaction.Count > 0 ? ViewStates.Visible : ViewStates.Invisible;
                        if(item.PostData.Reaction.Count > 0)
                                holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_like);
                        else
                                holder.ImageCountLike.SetImageResource(Resource.Drawable.icon_post_like_vector);

                        if (item.PostData.Reaction.IsReacted != null && item.PostData.Reaction.IsReacted.Value)
                        {
                            switch (string.IsNullOrEmpty(item.PostData.Reaction.Type))
                            {
                                case false:
                                {
                                    var react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Id == item.PostData.Reaction.Type).Value?.Id ?? "";
                                    switch (react)
                                    {
                                        case "1":
                                            holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_like);
                                            break;
                                        case "2":
                                            holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_love);
                                            break;
                                        case "3":
                                            holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_haha);
                                            break;
                                        case "4":
                                            holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_wow);
                                            break;
                                        case "5":
                                            holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_sad);
                                            break;
                                        case "6":
                                            holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_angry);
                                            break;
                                        default: 
                                            switch (item.PostData.Reaction.Count)
                                            {
                                                case > 0:
                                                    holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_like);
                                                    break;
                                            }
                                            break;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                        {
                            switch (item.PostData.Reaction.Count)
                            {
                                case > 0:
                                    holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_like);
                                    break;
                            }
                        }

                        break;
                    }
                    default:
                        //holder.ImageCountLike.Visibility = ViewStates.Invisible;
                        holder.ImageCountLike.SetImageResource(Resource.Drawable.icon_post_like_vector);
                        break;
                }

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void BottomPostPartBind(AdapterHolders.PostBottomSectionViewHolder holder, AdapterModelsClass item)
        {
            try
            {  
                if (holder.LikeButton != null)
                    holder.LikeButton.Text = item.PostData.PostLikes;
                 
                switch (AppSettings.PostButton)
                {
                    case PostButtonSystem.ReactionDefault:
                    case PostButtonSystem.ReactionSubShine:
                    {
                        item.PostData.Reaction ??= new Reaction();

                        if (item.PostData.Reaction.IsReacted != null && item.PostData.Reaction.IsReacted.Value)
                        {
                            switch (string.IsNullOrEmpty(item.PostData.Reaction.Type))
                            {
                                case false:
                                {
                                    var react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Id == item.PostData.Reaction.Type).Value?.Id ?? "";
                                    switch (react)
                                    {
                                        case "1":
                                            holder.LikeButton.SetReactionPack(ReactConstants.Like);
                                            break;
                                        case "2":
                                            holder.LikeButton.SetReactionPack(ReactConstants.Love);
                                            break;
                                        case "3":
                                            holder.LikeButton.SetReactionPack(ReactConstants.HaHa);
                                            break;
                                        case "4":
                                            holder.LikeButton.SetReactionPack(ReactConstants.Wow);
                                            break;
                                        case "5":
                                            holder.LikeButton.SetReactionPack(ReactConstants.Sad);
                                            break;
                                        case "6":
                                            holder.LikeButton.SetReactionPack(ReactConstants.Angry);
                                            break;
                                        default:
                                            holder.LikeButton.SetReactionPack(ReactConstants.Default);
                                            break;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                            holder.LikeButton.SetReactionPack(ReactConstants.Default);

                        break;
                    }
                    default:
                    {
                        if (item.PostData.Reaction.IsReacted != null && !item.PostData.Reaction.IsReacted.Value)
                            holder.LikeButton.SetReactionPack(ReactConstants.Default);

                        if (item.PostData.IsLiked != null && item.PostData.IsLiked.Value)
                            holder.LikeButton.SetReactionPack(ReactConstants.Like);

                        if (holder.SecondReactionButton != null)
                        {
                            switch (AppSettings.PostButton)
                            {
                                case PostButtonSystem.Wonder when item.PostData.IsWondered != null && item.PostData.IsWondered.Value:
                                {
                                    Drawable unwrappedDrawable = AppCompatResources.GetDrawable(ActivityContext, Resource.Drawable.ic_action_wowonder);
                                    Drawable wrappedDrawable = DrawableCompat.Wrap(unwrappedDrawable);
                                    switch (Build.VERSION.SdkInt)
                                    {
                                        case <= BuildVersionCodes.Lollipop:
                                            DrawableCompat.SetTint(wrappedDrawable, Color.ParseColor("#f89823"));
                                            break;
                                        default:
                                            wrappedDrawable = wrappedDrawable.Mutate();
                                            wrappedDrawable.SetColorFilter(new PorterDuffColorFilter(Color.ParseColor("#f89823"), PorterDuff.Mode.SrcAtop));
                                            break;
                                    }

                                    holder.SecondReactionButton.SetCompoundDrawablesWithIntrinsicBounds(wrappedDrawable, null, null, null);

                                    holder.SecondReactionButton.Text = ActivityContext.GetString(Resource.String.Lbl_wondered);
                                    holder.SecondReactionButton.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                                    break;
                                }
                                case PostButtonSystem.Wonder:
                                {
                                    Drawable unwrappedDrawable = AppCompatResources.GetDrawable(ActivityContext, Resource.Drawable.ic_action_wowonder);
                                    Drawable wrappedDrawable = DrawableCompat.Wrap(unwrappedDrawable);
                                    switch (Build.VERSION.SdkInt)
                                    {
                                        case <= BuildVersionCodes.Lollipop:
                                            DrawableCompat.SetTint(wrappedDrawable, Color.ParseColor("#666666"));
                                            break;
                                        default:
                                            wrappedDrawable = wrappedDrawable.Mutate();
                                            wrappedDrawable.SetColorFilter(new PorterDuffColorFilter(Color.ParseColor("#666666"), PorterDuff.Mode.SrcAtop));
                                            break;
                                    }
                                    holder.SecondReactionButton.SetCompoundDrawablesWithIntrinsicBounds(wrappedDrawable, null, null, null);

                                    holder.SecondReactionButton.Text = ActivityContext.GetString(Resource.String.Btn_Wonder);
                                    holder.SecondReactionButton.SetTextColor(Color.ParseColor("#444444"));
                                    break;
                                }
                                case PostButtonSystem.DisLike when item.PostData.IsWondered != null && item.PostData.IsWondered.Value:
                                {
                                    Drawable unwrappedDrawable = AppCompatResources.GetDrawable(ActivityContext, Resource.Drawable.ic_action_dislike);
                                    Drawable wrappedDrawable = DrawableCompat.Wrap(unwrappedDrawable);

                                    switch (Build.VERSION.SdkInt)
                                    {
                                        case <= BuildVersionCodes.Lollipop:
                                            DrawableCompat.SetTint(wrappedDrawable, Color.ParseColor("#f89823"));
                                            break;
                                        default:
                                            wrappedDrawable = wrappedDrawable.Mutate();
                                            wrappedDrawable.SetColorFilter(new PorterDuffColorFilter(Color.ParseColor("#f89823"), PorterDuff.Mode.SrcAtop));
                                            break;
                                    }

                                    holder.SecondReactionButton.SetCompoundDrawablesWithIntrinsicBounds(wrappedDrawable, null, null, null);

                                    holder.SecondReactionButton.Text = ActivityContext.GetString(Resource.String.Lbl_disliked);
                                    holder.SecondReactionButton.SetTextColor(Color.ParseColor("#f89823"));
                                    break;
                                }
                                case PostButtonSystem.DisLike:
                                {
                                    Drawable unwrappedDrawable = AppCompatResources.GetDrawable(ActivityContext, Resource.Drawable.ic_action_dislike);
                                    Drawable wrappedDrawable = DrawableCompat.Wrap(unwrappedDrawable);
                                    switch (Build.VERSION.SdkInt)
                                    {
                                        case <= BuildVersionCodes.Lollipop:
                                            DrawableCompat.SetTint(wrappedDrawable, Color.ParseColor("#666666"));
                                            break;
                                        default:
                                            wrappedDrawable = wrappedDrawable.Mutate();
                                            wrappedDrawable.SetColorFilter(new PorterDuffColorFilter(Color.ParseColor("#666666"), PorterDuff.Mode.SrcAtop));
                                            break;
                                    }

                                    holder.SecondReactionButton.SetCompoundDrawablesWithIntrinsicBounds(wrappedDrawable, null, null, null);

                                    holder.SecondReactionButton.Text = ActivityContext.GetString(Resource.String.Btn_Dislike);
                                    holder.SecondReactionButton.SetTextColor(Color.ParseColor("#444444"));
                                    break;
                                }
                            }
                        }

                        break;
                    }
                }

                holder.ShareLinearLayout.Visibility = item.IsSharingPost switch
                {
                    true => ViewStates.Gone,
                    _ => AppSettings.ShowShareButton ? ViewStates.Visible : ViewStates.Gone
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        } 

        public void TextSectionPostPartBind(AdapterHolders.PostTextSectionViewHolder holder ,AdapterModelsClass item)
        {
            try
            {
                if (string.IsNullOrEmpty(item.PostData.Orginaltext) || string.IsNullOrWhiteSpace(item.PostData.Orginaltext))
                {
                    if (holder.Description.Visibility != ViewStates.Gone)
                        holder.Description.Visibility = ViewStates.Gone;
                }
                else
                {
                    if (holder.Description.Visibility != ViewStates.Visible)
                        holder.Description.Visibility = ViewStates.Visible;
                     
                    if (!holder.Description.Text.Contains(ActivityContext.GetText(Resource.String.Lbl_ReadMore)) && !holder.Description.Text.Contains(ActivityContext.GetText(Resource.String.Lbl_ReadLess)))
                    {
                        switch (item.PostData.RegexFilterList != null & item.PostData.RegexFilterList?.Count > 0)
                        {
                            case true:
                                holder.Description.SetAutoLinkOnClickListener(NativePostAdapter, item.PostData.RegexFilterList);
                                break;
                            default:
                                holder.Description.SetAutoLinkOnClickListener(NativePostAdapter, new Dictionary<string, string>());
                                break;
                        }

                        NativePostAdapter.ReadMoreOption.AddReadMoreTo(holder.Description, new String(item.PostData.Orginaltext));
                    }
                    //else if (holder.Description.Text.Contains(ActivityContext.GetText(Resource.String.Lbl_ReadLess)))
                    //{
                    //    ReadMoreOption.AddReadLess(holder.Description, new String(item.PostData.Orginaltext));
                    //}
                    else
                    {
                        switch (item.PostData.RegexFilterList != null & item.PostData.RegexFilterList?.Count > 0)
                        {
                            case true:
                                holder.Description.SetAutoLinkOnClickListener(NativePostAdapter, item.PostData.RegexFilterList);
                                break;
                            default:
                                holder.Description.SetAutoLinkOnClickListener(NativePostAdapter, new Dictionary<string, string>());
                                break;
                        }

                        NativePostAdapter.ReadMoreOption.AddReadMoreTo(holder.Description, new String(item.PostData.Orginaltext));
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        } 

        public void CommentSectionBind(CommentAdapterViewHolder holder ,AdapterModelsClass item)
        {
            try
            {
                var comment = item.PostData.GetPostComments.FirstOrDefault(danjo => string.IsNullOrEmpty(danjo.CFile) && string.IsNullOrEmpty(danjo.Record) && !string.IsNullOrEmpty(danjo.Text));
                switch (comment)
                {
                    case null:
                        return;
                }

                var db = ClassMapper.Mapper?.Map<CommentObjectExtra>(comment);
                LoadCommentData(db, holder);

                holder.CommentAdapter.CommentList = new ObservableCollection<CommentObjectExtra>(ClassMapper.Mapper?.Map<ObservableCollection<CommentObjectExtra>>(item.PostData.GetPostComments));

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void LoadCommentData(CommentObjectExtra item, RecyclerView.ViewHolder viewHolder)
        {
            try
            {
                if (viewHolder is not CommentAdapterViewHolder holder)
                    return;

                if (!string.IsNullOrEmpty(item.Orginaltext) || !string.IsNullOrWhiteSpace(item.Orginaltext))
                {
                    var text = Methods.FunString.DecodeString(item.Orginaltext);
                    NativePostAdapter.ReadMoreOption.AddReadMoreTo(holder.CommentText, new String(text)); 
                }
                else
                {
                    holder.CommentText.Visibility = ViewStates.Gone;
                }

                holder.TimeTextView.Text = Methods.Time.TimeAgo(Convert.ToInt32(item.Time), true);
                holder.UserName.Text = item.Publisher.Name;

                GlideImageLoader.LoadImage(ActivityContext, item.Publisher.Avatar, holder.Image, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);

                var textHighLighter = item.Publisher.Name;
                var textIsPro = string.Empty;

                switch (item.Publisher.Verified)
                {
                    case "1":
                        textHighLighter += " " + IonIconsFonts.CheckmarkCircle;
                        break;
                }

                switch (item.Publisher.IsPro)
                {
                    case "1":
                        textIsPro = " " + IonIconsFonts.Flash;
                        textHighLighter += textIsPro;
                        break;
                }

                var decorator = TextDecorator.Decorate(holder.UserName, textHighLighter).SetTextStyle((int)TypefaceStyle.Bold, 0, item.Publisher.Name.Length);

                switch (item.Publisher.Verified)
                {
                    case "1":
                        decorator.SetTextColor(Resource.Color.Post_IsVerified, IonIconsFonts.CheckmarkCircle);
                        break;
                }

                switch (item.Publisher.IsPro)
                {
                    case "1":
                        decorator.SetTextColor(Resource.Color.text_color_in_between, textIsPro);
                        break;
                }

                decorator.Build();

                //Image
                if (holder.ItemViewType == 1 || holder.CommentImage != null)
                {
                    //if (!string.IsNullOrEmpty(item.CFile) && (item.CFile.Contains("file://") || item.CFile.Contains("content://") || item.CFile.Contains("storage") || item.CFile.Contains("/data/user/0/")))
                    //{
                    //    File file2 = new File(item.CFile);
                    //    var photoUri = FileProvider.GetUriForFile(ActivityContext, ActivityContext.PackageName + ".fileprovider", file2);
                    //    Glide.With(ActivityContext).Load(photoUri).Apply(new RequestOptions()).Into(holder.CommentImage);

                    //    //GlideImageLoader.LoadImage(ActivityContext,item.CFile, holder.CommentImage, ImageStyle.CenterCrop, ImagePlaceholders.Color);
                    //}
                    //else
                    //{
                    //    if (!item.CFile.Contains(Client.WebsiteUrl))
                    //        item.CFile = WoWonderTools.GetTheFinalLink(item.CFile);

                    //    GlideImageLoader.LoadImage(ActivityContext, item.CFile, holder.CommentImage, ImageStyle.CenterCrop, ImagePlaceholders.Color);
                    //}
                }

                //Voice
                if (holder.VoiceLayout != null && !string.IsNullOrEmpty(item.Record))
                {
                    //LoadAudioItem(holder, position, item);
                }

                var repliesCount = !string.IsNullOrEmpty(item.RepliesCount) ? item.RepliesCount : item.Replies ?? "";
                if (repliesCount != "0" && !string.IsNullOrEmpty(repliesCount))
                    holder.ReplyTextView.Text = ActivityContext.GetText(Resource.String.Lbl_Reply) + " " + "(" + repliesCount + ")";

                switch (AppSettings.PostButton)
                {
                    case PostButtonSystem.ReactionDefault:
                    case PostButtonSystem.ReactionSubShine:
                    {
                        item.Reaction ??= new Reaction();

                        switch (item.Reaction.Count)
                        {
                            case > 0:
                                holder.CountLikeSection.Visibility = ViewStates.Visible;
                                holder.CountLike.Text = Methods.FunString.FormatPriceValue(item.Reaction.Count);
                                break;
                            default:
                                holder.CountLikeSection.Visibility = ViewStates.Gone;
                                break;
                        }

                        if (item.Reaction.IsReacted != null && item.Reaction.IsReacted.Value)
                        {
                            switch (string.IsNullOrEmpty(item.Reaction.Type))
                            {
                                case false:
                                {
                                    var react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Id == item.Reaction.Type).Value?.Id ?? "";
                                    switch (react)
                                    {
                                        case "1":
                                            ReactionComment.SetReactionPack(holder, ReactConstants.Like);
                                            holder.LikeTextView.Tag = "Liked";
                                            holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_like);
                                            break;
                                        case "2":
                                            ReactionComment.SetReactionPack(holder, ReactConstants.Love);
                                            holder.LikeTextView.Tag = "Liked";
                                            holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_love);
                                            break;
                                        case "3":
                                            ReactionComment.SetReactionPack(holder, ReactConstants.HaHa);
                                            holder.LikeTextView.Tag = "Liked";
                                            holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_haha);
                                            break;
                                        case "4":
                                            ReactionComment.SetReactionPack(holder, ReactConstants.Wow);
                                            holder.LikeTextView.Tag = "Liked";
                                            holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_wow);
                                            break;
                                        case "5":
                                            ReactionComment.SetReactionPack(holder, ReactConstants.Sad);
                                            holder.LikeTextView.Tag = "Liked";
                                            holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_sad);
                                            break;
                                        case "6":
                                            ReactionComment.SetReactionPack(holder, ReactConstants.Angry);
                                            holder.LikeTextView.Tag = "Liked";
                                            holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_angry);
                                            break;
                                        default:
                                            holder.LikeTextView.Text = ActivityContext.GetText(Resource.String.Btn_Like);
                                            //holder.LikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                                            holder.LikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.ParseColor("#888888"));
                                            holder.LikeTextView.Tag = "Like";

                                            switch (item.Reaction.Count)
                                            {
                                                case > 0:
                                                    holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_like);
                                                    break;
                                            }
                                            break;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                        {
                            holder.LikeTextView.Text = ActivityContext.GetText(Resource.String.Btn_Like);
                            //holder.LikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                            holder.LikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.ParseColor("#888888"));
                            holder.LikeTextView.Tag = "Like";

                            switch (item.Reaction.Count)
                            {
                                case > 0:
                                    holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_like);
                                    break;
                            }
                        }

                        break;
                    }
                    default:
                    {
                        switch (item.IsCommentLiked)
                        {
                            case true:
                                holder.LikeTextView.Text = ActivityContext.GetText(Resource.String.Btn_Liked);
                                holder.LikeTextView.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                                holder.LikeTextView.Tag = "Liked";
                                break;
                            default:
                                holder.LikeTextView.Text = ActivityContext.GetText(Resource.String.Btn_Like);
                                //holder.LikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                                holder.LikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.ParseColor("#888888"));
                                holder.LikeTextView.Tag = "Like";
                                break;
                        }

                        break;
                    }
                }

                holder.TimeTextView.Tag = "true";
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }


        public void ImagePostBind(AdapterHolders.PostImageSectionViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                string imageUrl;
                switch (item.PostData.PhotoAlbum?.Count)
                {
                    case > 0:
                    {
                        var imagesList = item.PostData.PhotoAlbum;
                        imageUrl = imagesList[0].Image;
                        break;
                    }
                    default:
                        imageUrl = !string.IsNullOrEmpty(item.PostData.PostSticker) ? item.PostData.PostSticker : item.PostData.PostFileFull;
                        break;
                }
                holder.Image.Layout(0, 0, 0, 0);

                if (imageUrl.Contains(".gif"))
                    Glide.With(ActivityContext).Load(imageUrl).Apply(new RequestOptions().Placeholder(Resource.Drawable.ImagePlacholder)).Into(holder.Image);
                else
                    NativePostAdapter.FullGlideRequestBuilder.Load(imageUrl).Into(holder.Image);

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void MultiImage2Bind(AdapterHolders.Post2ImageSectionViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                if (item.PostData.PhotoMulti?.Count == 2 || item.PostData.PhotoAlbum?.Count == 2)
                {
                    var imagesList = item.PostData.PhotoMulti ?? item.PostData.PhotoAlbum;

                    NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[0].Image).Into(holder.Image);
                    NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[1].Image).Into(holder.Image2);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void MultiImage3Bind(AdapterHolders.Post3ImageSectionViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                if (item.PostData.PhotoMulti?.Count == 3 || item.PostData.PhotoAlbum?.Count == 3)
                {
                    var imagesList = item.PostData.PhotoMulti ?? item.PostData.PhotoAlbum;

                    NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[0].Image).Into(holder.Image);
                    NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[1].Image).Into(holder.Image2);
                    NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[2].Image).Into(holder.Image3);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void MultiImage4Bind(AdapterHolders.Post4ImageSectionViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                if (item.PostData.PhotoMulti?.Count == 4 || item.PostData.PhotoAlbum?.Count == 4)
                {
                    var imagesList = item.PostData.PhotoMulti ?? item.PostData.PhotoAlbum;

                    NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[0].Image).Into(holder.Image);
                    NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[1].Image).Into(holder.Image2);
                    NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[2].Image).Into(holder.Image3);
                    NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[3].Image).Into(holder.Image4);
                }

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void MultiImagesBind(AdapterHolders.PostMultiImagesViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                if (item.PostData.PhotoMulti?.Count > 4 || item.PostData.PhotoAlbum?.Count > 4)
                {
                    var imagesList = item.PostData.PhotoMulti ?? item.PostData.PhotoAlbum;

                    NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[0].Image).Into(holder.Image);
                    NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[1].Image).Into(holder.Image2);
                    NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[2].Image).Into(holder.Image3);
                    NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[3].Image).Into(holder.Image4);

                    if (imagesList?.Count >= 5)
                    {
                        NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[4].Image).Into(holder.Image5);
                        if (imagesList?.Count == 5)
                        {
                            holder.CountImageLabel.Visibility = ViewStates.Gone;
                            holder.Image6.Visibility = ViewStates.Gone;
                            holder.Image7.Visibility = ViewStates.Gone;
                            holder.ViewImage7.Visibility = ViewStates.Gone;
                            return;
                        }
                        NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[5].Image).Into(holder.Image6);
                        if (imagesList?.Count == 6)
                        {
                            holder.CountImageLabel.Visibility = ViewStates.Gone;
                            holder.Image7.Visibility = ViewStates.Gone;
                            holder.ViewImage7.Visibility = ViewStates.Gone;
                            return;
                        }
                        NativePostAdapter.FullGlideRequestBuilder.Load(imagesList[6].Image).Into(holder.Image7);
                        if (imagesList?.Count == 7)
                        {
                            holder.CountImageLabel.Visibility = ViewStates.Gone;
                            holder.ViewImage7.Visibility = ViewStates.Gone;
                            return;
                        }
                        holder.CountImageLabel.Text = "+" + (imagesList?.Count - 7);
                    }
                }

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void VideoPostBind(AdapterHolders.PostVideoSectionViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                switch (string.IsNullOrEmpty(item.PostData.PostFileThumb))
                {
                    case false:
                        NativePostAdapter.FullGlideRequestBuilder.Load(item.PostData.PostFileThumb).Into(holder.VideoImage);
                        break;
                    default:
                        NativePostAdapter.FullGlideRequestBuilder.Load(item.PostData.PostFileFull).Into(holder.VideoImage);
                        break;
                }

                //Glide.With(ActivityContext)
                //        .AsBitmap()
                //        .Placeholder(Resource.Drawable.blackdefault)
                //        .Error(Resource.Drawable.blackdefault)
                //        .Load(item.PostData.PostFileFull) // or URI/path
                //        .Into(holder.VideoImage); //image view to set thumbnail to 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void BlogPostBind(AdapterHolders.PostBlogSectionViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                switch (string.IsNullOrEmpty(item.PostData?.Blog.Thumbnail))
                {
                    case false:
                        NativePostAdapter.FullGlideRequestBuilder.Load(item.PostData?.Blog.Thumbnail).Into(holder.ImageBlog);
                        break;
                }

                holder.PostBlogText.Text = item.PostData?.Blog.Title;
                holder.PostBlogContent.Text = item.PostData?.Blog.Description;
                holder.CatText.Text = item.PostData?.Blog.CategoryName; 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void ColorPostBind(AdapterHolders.PostColorBoxSectionViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                switch (string.IsNullOrEmpty(item.PostData.ColorBoxImageUrl))
                {
                    case false:
                        NativePostAdapter.FullGlideRequestBuilder.Load(item.PostData.ColorBoxImageUrl).Into(holder.ColorBoxImage);
                        break;
                }

                if (item.PostData.ColorBoxGradientDrawable != null)
                    holder.ColorBoxImage.Background = item.PostData.ColorBoxGradientDrawable;

                if (item.PostData != null)
                {
                    holder.DesTextView.SetTextColor(item.PostData.ColorBoxTextColor);
                    holder.DesTextView.Text = item.PostData.Orginaltext;
                }

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void EventPostBind(AdapterHolders.EventPostViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                if (item.PostData.Event?.EventClass != null)
                {
                    NativePostAdapter.FullGlideRequestBuilder.Load(item.PostData?.Event?.EventClass.Cover).Into(holder.Image);
                    holder.TxtEventTitle.Text = item.PostData?.Event?.EventClass?.Name;
                    holder.TxtEventDescription.Text = item.PostData?.Event?.EventClass?.Description;
                    holder.TxtEventLocation.Text = item.PostData?.Event?.EventClass?.Location;
                    holder.TxtEventTime.Text = item.PostData?.Event?.EventClass?.EndDate;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void LinkPostBind(AdapterHolders.LinkPostViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                holder.LinkUrl.Text = item.PostData?.PostLink;

                switch (string.IsNullOrEmpty(item.PostData?.PostLinkTitle))
                {
                    case false:
                        holder.PostLinkTitle.Text = item.PostData?.PostLinkTitle;
                        break;
                    default:
                        holder.PostLinkTitle.Visibility = ViewStates.Gone;
                        break;
                }

                switch (string.IsNullOrEmpty(item.PostData?.PostLinkContent))
                {
                    case false:
                        holder.PostLinkContent.Text = item.PostData?.PostLinkContent;
                        break;
                    default:
                        holder.PostLinkContent.Visibility = ViewStates.Gone;
                        break;
                }

                if (string.IsNullOrEmpty(item.PostData?.PostLinkImage) || item.PostData.PostLinkTitle.Contains("Page Not Found") || item.PostData.PostLinkContent.Contains("See posts, photos and more on Facebook."))
                    holder.Image.Visibility = ViewStates.Gone;
                else
                {
                    if (item.PostData.PostLink.Contains("facebook.com") || item.PostData.PostLinkImage.Contains("facebook.png"))
                        NativePostAdapter.FullGlideRequestBuilder.Clone().Load(item.PostData.PostLinkImage).Error(Resource.Drawable.facebook).Placeholder(Resource.Drawable.facebook).Into(holder.Image);
                    else if (item.PostData.PostLink.Contains("vimeo.com") || item.PostData.PostLinkImage.Contains("vimeo.png"))
                        NativePostAdapter.FullGlideRequestBuilder.Clone().Load(Resource.Drawable.vimeo).Error(Resource.Drawable.vimeo).Placeholder(Resource.Drawable.vimeo).Into(holder.Image);
                    else if (item.PostData.PostLinkImage.Contains("default_video_thumbnail.png"))
                        NativePostAdapter.FullGlideRequestBuilder.Clone().Load(Resource.Drawable.default_video_thumbnail).Error(Resource.Drawable.default_video_thumbnail).Placeholder(Resource.Drawable.default_video_thumbnail).Into(holder.Image);
                    else
                        NativePostAdapter.FullGlideRequestBuilder.Clone().Load(item.PostData.PostLinkImage).Placeholder(new ColorDrawable(Color.ParseColor("#efefef"))).Into(holder.Image);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void FundingPostBind(AdapterHolders.FundingPostViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                if (item.PostData.FundData != null)
                {
                    NativePostAdapter.FullGlideRequestBuilder.Load(item.PostData.FundData.Value.FundDataClass.Image).Into(holder.Image);

                    holder.Title.Text = item.PostData.FundData.Value.FundDataClass.Title;
                    holder.DonationTime.Text = item.PostData.FundData.Value.FundDataClass.Time;
                    holder.Description.Text = item.PostData.FundData.Value.FundDataClass.Description;
                    holder.Raised.Text = item.PostData.FundData.Value.FundDataClass.Raised;
                    holder.TottalAmount.Text = item.PostData.FundData.Value.FundDataClass.Amount;
                    holder.Progress.Progress = Convert.ToInt32(item.PostData.FundData.Value.FundDataClass.Bar);

                    item.PostData.FundData.Value.FundDataClass.UserData ??= item.PostData.Publisher;
                }

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void PurpleFundPostBind(AdapterHolders.FundingPostViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                if (item.PostData?.Fund?.PurpleFund?.Fund != null)
                {
                    NativePostAdapter.FullGlideRequestBuilder.Load(item.PostData?.Fund?.PurpleFund?.Fund.Image).Into(holder.Image);

                    holder.Title.Text = item.PostData?.Fund?.PurpleFund?.Fund.Title;
                    holder.DonationTime.Text = item.PostData?.Fund?.PurpleFund?.Fund.Time;
                    holder.Description.Text = item.PostData?.Fund?.PurpleFund?.Fund.Description;
                    holder.Raised.Text = item.PostData?.Fund?.PurpleFund?.Fund.Raised;
                    holder.TottalAmount.Text = item.PostData?.Fund?.PurpleFund?.Fund.Amount;
                    holder.Progress.Progress = Convert.ToInt32(item.PostData?.Fund?.PurpleFund?.Fund.Bar);

                    item.PostData.Fund.Value.PurpleFund.Fund.UserData ??= item.PostData.Publisher;
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void ProductPostBind(AdapterHolders.ProductPostViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                if (item.PostData.Product != null)
                {
                    switch (item.PostData.Product.Value.ProductClass?.Images.Count)
                    {
                        case > 0:
                            NativePostAdapter.FullGlideRequestBuilder.Load(item.PostData.Product.Value.ProductClass?.Images[0].Image)
                                .Into(holder.Image);
                            break;
                    }

                    switch (item.PostData.Product.Value.ProductClass?.Seller)
                    {
                        case null:
                        {
                            if (item.PostData.Product.Value.ProductClass != null)
                                item.PostData.Product.Value.ProductClass.Seller = item.PostData.Publisher;
                            break;
                        }
                    }

                    switch (string.IsNullOrEmpty(item.PostData.Product.Value.ProductClass?.LocationDecodedText))
                    {
                        case false:
                            holder.PostProductLocationText.Text = item.PostData.Product.Value.ProductClass?.LocationDecodedText;
                            break;
                        default:
                            holder.PostProductLocationText.Visibility = ViewStates.Gone;
                            holder.LocationIcon.Visibility = ViewStates.Gone;
                            break;
                    }

                    holder.PostLinkTitle.Text = item.PostData.Product.Value.ProductClass?.Name;
                    holder.PostProductContent.Text = item.PostData.Product.Value.ProductClass?.Description;
                    holder.PriceText.Text = item.PostData.Product.Value.ProductClass?.CurrencyText;
                    holder.TypeText.Text = item.PostData.Product.Value.ProductClass?.TypeDecodedText;
                    holder.StatusText.Text = item.PostData.Product.Value.ProductClass?.StatusDecodedText;
                }

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void VoicePostBind(AdapterHolders.SoundPostViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                switch (item.MediaIsPlaying)
                {
                    case true:
                        holder.PlayButton.SetImageResource(Resource.Drawable.icon_player_pause);
                        break;
                    default:
                    {
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
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public void AgoraLivePostBind(AdapterHolders.PostAgoraLiveViewHolder holder, AdapterModelsClass item)
        {
            try
            { 
                if (item.PostData.LiveTime != null && item.PostData.LiveTime.Value > 0)
                {
                    holder.TxtName.Text = WoWonderTools.GetNameFinal(item.PostData.Publisher) + " " + ActivityContext.GetText(Resource.String.Lbl_StartedBroadcastingLive);
                }
                else
                {
                    holder.TxtName.Text = WoWonderTools.GetNameFinal(item.PostData.Publisher) + " " + ActivityContext.GetText(Resource.String.Lbl_StreamHasEnded);
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public void WebViewPostBind(AdapterHolders.PostPlayTubeContentViewHolder holder, AdapterModelsClass item , PostModelType itemViewType)
        {
            try
            {
                switch (itemViewType)
                {
                    case PostModelType.PlayTubePost:
                    {
                        var playTubeUrl = ListUtils.SettingsSiteList?.PlaytubeUrl;

                        var fullEmbedUrl = playTubeUrl + "/embed/" + item.PostData.PostPlaytube;

                        switch (AppSettings.EmbedPlayTubePostType)
                        {
                            case true:
                            {
                                var vc = holder.WebView.LayoutParameters;
                                vc.Height = 600;
                                holder.WebView.LayoutParameters = vc;

                                holder.WebView.LoadUrl(fullEmbedUrl);
                                break;
                            }
                            default:
                                item.PostData.PostLink = fullEmbedUrl;
                                holder.WebView.Visibility = ViewStates.Gone;
                                break;
                        }

                        break;
                    }
                    case PostModelType.LivePost:
                    {
                        var liveUrl = "https://viewer.millicast.com/v2?streamId=";
                        var id = ListUtils.SettingsSiteList?.LiveAccountId;
                        string fullEmbedUrl = liveUrl + id + "/" + item.PostData.StreamName;

                        switch (AppSettings.EmbedLivePostType)
                        {
                            case true:
                            {
                                var vc = holder.WebView.LayoutParameters;
                                vc.Height = 600;
                                holder.WebView.LayoutParameters = vc;

                                holder.WebView.LoadUrl(fullEmbedUrl);
                                break;
                            }
                            default:
                                item.PostData.PostLink = fullEmbedUrl;
                                holder.WebView.Visibility = ViewStates.Gone;
                                break;
                        } 
                        break;
                    }
                    case PostModelType.DeepSoundPost:
                    {
                        var deepSoundUrl = ListUtils.SettingsSiteList?.DeepsoundUrl;

                        var fullEmbedUrl = deepSoundUrl + "/embed/" + item.PostData.PostDeepsound;

                        switch (AppSettings.EmbedDeepSoundPostType)
                        {
                            case true:
                            {
                                var vc = holder.WebView.LayoutParameters;
                                vc.Height = 480;
                                holder.WebView.LayoutParameters = vc;

                                holder.WebView.LoadUrl(fullEmbedUrl);
                                break;
                            }
                            default:
                                item.PostData.PostLink = fullEmbedUrl;
                                holder.WebView.Visibility = ViewStates.Gone;
                                break;
                        }

                        break;
                    }
                    case PostModelType.VimeoPost:
                    {
                        var fullEmbedUrl = "https://player.vimeo.com/video/" + item.PostData.PostVimeo;

                        switch (AppSettings.EmbedVimeoVideoPostType)
                        {
                            case VideoPostTypeSystem.EmbedVideo:
                            {
                                var vc = holder.WebView.LayoutParameters;
                                vc.Height = 700;
                                holder.WebView.LayoutParameters = vc;

                                holder.WebView.LoadUrl(fullEmbedUrl);
                                break;
                            }
                            default:
                                item.PostData.PostLink = fullEmbedUrl;
                                holder.WebView.Visibility = ViewStates.Gone;
                                break;
                        }

                        break;
                    }
                    case PostModelType.FacebookPost:
                    {
                        var content = "<iframe src='https://www.facebook.com/plugins/video.php?height=600&href=https://www.facebook.com/" + item.PostData.PostFacebook + "/&show_text=false'";
                        content += "width='100%' height='600' style='border:none;overflow:hidden'";
                        content += "scrolling='no' frameborder='0' allowfullscreen='true'";
                        content += "allow='autoplay; clipboard-write; encrypted-media; picture-in-picture; web-share'";
                        content += "allowFullScreen='true'>";
                        content += "</iframe>";
                            
                        var dataWebHtml = "<!DOCTYPE html>";
                        dataWebHtml += "<head><title></title>" + "</head>";
                        dataWebHtml += "<body>" + content + "</body>";
                        dataWebHtml += "</html>";
                             
                        var fullEmbedUrl = "https://www.facebook.com/video/embed?video_id=" + item.PostData.PostFacebook.Split("/videos/").Last();
                        switch (AppSettings.EmbedFacebookVideoPostType)
                        {
                            case VideoPostTypeSystem.EmbedVideo:
                            {
                                var vc = holder.WebView.LayoutParameters;
                                vc.Height = 600;
                                holder.WebView.LayoutParameters = vc;

                                //Load url to be rendered on WebView 
                                //holder.WebView.LoadUrl(fullEmbedUrl);
                                holder.WebView.LoadDataWithBaseURL(null, dataWebHtml, "text/html", "UTF-8", null);
                                holder.WebView.Visibility = ViewStates.Visible;
                                break;
                            }
                            default:
                                item.PostData.PostLink = fullEmbedUrl;
                                holder.WebView.Visibility = ViewStates.Gone;
                                break;
                        }
                         
                        break;
                    }
                    case PostModelType.TikTokPost:
                    {
                        var fullEmbedUrl = item.PostData.PostLink;
                        switch (AppSettings.EmbedTikTokVideoPostType)
                        {
                            case VideoPostTypeSystem.EmbedVideo:
                            {
                                //var videoId = item.PostData.PostTikTok.Split("/video/").Last();
                                //string content = "<blockquote class='tiktok-embed' cite='" + item.PostData.PostTikTok + "' data-video-id='" + videoId + "' style='max-width: 605px;min-width: 325px;'>" +
                                //                 "<iframe src='" + item.PostData.PostTikTok + "' style='width: 100%; height: 863px; display: block; visibility: unset; max-height: 863px;'></iframe>" +
                                //                 "</blockquote>" + "" +
                                //                 "<script async='' src='https://www.tiktok.com/embed.js'></script>";
                                 
                                //string DataWebHtml = "<!DOCTYPE html>";
                                //DataWebHtml += "<head><title></title></head>";
                                //DataWebHtml += "<body>" + content + "</body>";
                                //DataWebHtml += "</html>";

                                var vc = holder.WebView.LayoutParameters;
                                vc.Height = 1200;
                                holder.WebView.LayoutParameters = vc;

                                //holder.WebView.LoadDataWithBaseURL(null, DataWebHtml, "text/html", "UTF-8", null);
                                holder.WebView.LoadUrl(item.PostData.PostTikTok);
                                holder.WebView.Visibility = ViewStates.Visible;
                                break;
                            }
                            default:
                                item.PostData.PostLink = fullEmbedUrl;
                                holder.WebView.Visibility = ViewStates.Gone;
                                break;
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
         
        public void OfferPostBind(AdapterHolders.OfferPostViewHolder holder, AdapterModelsClass item)
        {
            try
            { 
                switch (string.IsNullOrEmpty(item.PostData.Offer?.OfferClass?.Image))
                {
                    case false:
                        NativePostAdapter.FullGlideRequestBuilder.Load(item.PostData.Offer?.OfferClass?.Image).Into(holder.ImageBlog);
                        break;
                }

                holder.PostBlogText.Text = item.PostData.Offer?.OfferClass?.OfferText;
                holder.PostBlogContent.Text = item.PostData.Offer?.OfferClass?.Description;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void JobPostSection1Bind(AdapterHolders.JobPostViewHolder1 holder, AdapterModelsClass item)
        {
            try
            {
                if (item.PostData.Job != null)
                {

                    if (item.PostData.Job.Value.JobInfoClass.Page != null)
                        NativePostAdapter.FullGlideRequestBuilder.Load(item.PostData.Job.Value.JobInfoClass.Page.Avatar).Into(holder.JobAvatar);

                    NativePostAdapter.FullGlideRequestBuilder.Load(item.PostData.Job.Value.JobInfoClass.Image).Into(holder.JobCoverImage);

                    holder.JobTitle.Text = item.PostData.Job.Value.JobInfoClass.Title;
                    holder.PageName.Text = item.PostData.Job?.JobInfoClass.Page.PageName;

                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void JobPostSection2Bind(AdapterHolders.JobPostViewHolder2 holder, AdapterModelsClass item)
        {
            try
            {
                if (item.PostData.Job != null)
                {  
                    if (item.PostData.Job.Value.JobInfoClass.ButtonText.Contains(ActivityContext.GetString(Resource.String.Lbl_show_applies)))
                    { 
                        holder.JobButton.Tag = "ShowApply";
                    }
                    else if (item.PostData.Job.Value.JobInfoClass.ButtonText.Contains(ActivityContext.GetString(Resource.String.Lbl_already_applied)))
                    {
                        holder.JobButton.Enabled = false; 
                    }
                    else if (item.PostData.Job.Value.JobInfoClass.ButtonText.Contains(ActivityContext.GetString(Resource.String.Lbl_apply_now)))
                    {
                        holder.JobButton.Tag = "Apply";
                    }

                    holder.JobButton.Text = item.PostData.Job?.JobInfoClass.ButtonText;
                    switch (string.IsNullOrEmpty(item.PostData.Job?.JobInfoClass.Description))
                    {
                        case false:
                            holder.Description.Text = item.PostData.Job?.JobInfoClass.Description;
                            break;
                        default:
                            holder.Description.Visibility = ViewStates.Gone;
                            break;
                    }
                  
                    holder.MinimumNumber.Text = item.PostData.Job?.JobInfoClass.Minimum + " " + item.PostData.Job?.JobInfoClass.SalaryDate;
                    holder.MaximumNumber.Text = item.PostData.Job?.JobInfoClass.Maximum + " " + item.PostData.Job?.JobInfoClass.SalaryDate;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void PollPostBind(AdapterHolders.PollsPostViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                holder.VoteText.Text = item.PollsOption.Text;
                holder.ProgressBarView.Progress = Convert.ToInt32(item.PollsOption.PercentageNum);
                holder.ProgressText.Text = item.PollsOption.Percentage;

                switch (string.IsNullOrEmpty(item.PostData.VotedId))
                {
                    case false when item.PostData.VotedId != "0":
                    {
                        if (item.PollsOption.Id == item.PostData.VotedId)
                        {
                            holder.CheckIcon.SetImageResource(Resource.Drawable.icon_checkmark_filled_vector);
                            holder.CheckIcon.ClearColorFilter();
                                holder.ProgressText.SetTextColor(new Color(ContextCompat.GetColor(holder.VoteText.Context, Resource.Color.primary)));
                                holder.ProgressBarView.ProgressDrawable = ContextCompat.GetDrawable(holder.ProgressBarView.Context, Resource.Drawable.primary_progress);
                        }
                        else
                        {
                            holder.CheckIcon.SetImageResource(Resource.Drawable.icon_check_circle_vector);
                            holder.CheckIcon.SetColorFilter(new PorterDuffColorFilter(Color.ParseColor("#999999"), PorterDuff.Mode.SrcAtop));
                        }

                        break;
                    }
                    default:
                        holder.CheckIcon.SetImageResource(Resource.Drawable.icon_check_circle_vector);
                        holder.CheckIcon.SetColorFilter(new PorterDuffColorFilter(Color.ParseColor("#999999"), PorterDuff.Mode.SrcAtop));
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public void AlertBoxBind(AdapterHolders.AlertAdapterViewHolder holder, AdapterModelsClass item, PostModelType itemViewType)
        {
            try
            {
                switch (itemViewType)
                {
                    case PostModelType.AlertBox:
                    {
                        holder.HeadText.Text = string.IsNullOrEmpty(item.AlertModel?.TitleHead) switch
                        {
                            false => item.AlertModel?.TitleHead,
                            _ => holder.HeadText.Text
                        };

                        holder.SubText.Text = string.IsNullOrEmpty(item.AlertModel?.SubText) switch
                        {
                            false => item.AlertModel?.SubText,
                            _ => holder.SubText.Text
                        };

                        if (item.AlertModel?.ImageDrawable != null)
                            holder.Image.SetImageResource(item.AlertModel.ImageDrawable);

                        switch (string.IsNullOrEmpty(item.AlertModel?.LinerColor))
                        {
                            case false:
                                holder.LineView.SetBackgroundColor(Color.ParseColor(item.AlertModel?.LinerColor));
                                break;
                        }
                        break;
                    }
                    case PostModelType.AlertBoxAnnouncement:
                    {
                        holder.HeadText.Text = string.IsNullOrEmpty(item.AlertModel?.TitleHead) switch
                        {
                            false => Methods.FunString.DecodeString(item.AlertModel?.TitleHead),
                            _ => holder.HeadText.Text
                        };

                        holder.SubText.Text = string.IsNullOrEmpty(item.AlertModel?.SubText) switch
                        {
                            false => Methods.FunString.DecodeString(item.AlertModel?.SubText),
                            _ => holder.SubText.Text
                        };
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void AlertJoinBoxBind(AdapterHolders.AlertJoinAdapterViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                holder.HeadText.Text = string.IsNullOrEmpty(item.AlertModel?.TitleHead) switch
                {
                    false => item.AlertModel?.TitleHead,
                    _ => holder.HeadText.Text
                };

                holder.SubText.Text = string.IsNullOrEmpty(item.AlertModel?.SubText) switch
                {
                    false => item.AlertModel?.SubText,
                    _ => holder.SubText.Text
                };

                if (item.AlertModel?.ImageDrawable != null)
                    holder.NormalImageView.SetImageResource(item.AlertModel.ImageDrawable);
                else
                    holder.NormalImageView.Visibility = ViewStates.Gone;

                if (item.AlertModel?.IconImage != null)
                    holder.IconImageView.SetImageResource(item.AlertModel.IconImage);

                switch (item.AlertModel?.TypeAlert)
                {
                    case "Groups":
                        holder.MainRelativeLayout.SetBackgroundResource(Resource.Drawable.Shape_Gradient_Linear);
                        holder.ButtonView.Text = ActivityContext.GetString(Resource.String.Lbl_FindYourGroups);
                        break;
                    case "Pages":
                        holder.MainRelativeLayout.SetBackgroundResource(Resource.Drawable.Shape_Gradient_Linear1);
                        holder.ButtonView.Text = ActivityContext.GetString(Resource.String.Lbl_FindPopularPages);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void SocialLinksBind(AdapterHolders.SocialLinksViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                if (Methods.FunString.StringNullRemover(item.SocialLinksModel.Facebook) == "Empty")
                {
                    holder.BtnFacebook.Enabled = false;
                    holder.BtnFacebook.SetColor(Color.ParseColor("#8c8a8a"));
                }
                else
                    holder.BtnFacebook.Enabled = true;

                if (Methods.FunString.StringNullRemover(item.SocialLinksModel.Google) == "Empty")
                {
                    holder.BtnGoogle.Enabled = false;
                    holder.BtnGoogle.SetColor(Color.ParseColor("#8c8a8a"));
                }
                else
                    holder.BtnGoogle.Enabled = true;

                if (Methods.FunString.StringNullRemover(item.SocialLinksModel.Twitter) == "Empty")
                {
                    holder.BtnTwitter.Enabled = false;
                    holder.BtnTwitter.SetColor(Color.ParseColor("#8c8a8a"));
                }
                else
                    holder.BtnTwitter.Enabled = true;

                if (Methods.FunString.StringNullRemover(item.SocialLinksModel.Youtube) == "Empty")
                {
                    holder.BtnYoutube.Enabled = false;
                    holder.BtnYoutube.SetColor(Color.ParseColor("#8c8a8a"));
                }
                else
                    holder.BtnYoutube.Enabled = true;

                if (Methods.FunString.StringNullRemover(item.SocialLinksModel.Vk) == "Empty")
                {
                    holder.BtnVk.Enabled = false;
                    holder.BtnVk.SetColor(Color.ParseColor("#8c8a8a"));
                }
                else
                    holder.BtnVk.Enabled = true;

                if (Methods.FunString.StringNullRemover(item.SocialLinksModel.Instegram) == "Empty")
                {
                    holder.BtnInstegram.Enabled = false;
                    holder.BtnInstegram.SetColor(Color.ParseColor("#8c8a8a"));
                }
                else
                    holder.BtnInstegram.Enabled = true;

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
          
        public void AboutBoxBind(AdapterHolders.AboutBoxViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                holder.AboutHead.Text = string.IsNullOrEmpty(item.AboutModel.TitleHead) switch
                {
                    false => item.AboutModel.TitleHead,
                    _ => holder.AboutHead.Text
                };

                holder.AboutDescription.SetAutoLinkOnClickListener(NativePostAdapter, new Dictionary<string, string>());
                holder.AboutDescription.Text = Methods.FunString.DecodeString(item.AboutModel.Description);
                NativePostAdapter.ReadMoreOption.AddReadMoreTo(holder.AboutDescription, new String(holder.AboutDescription.Text));
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
         
        public void ProfileHeaderSeltionBind(AdapterHolders.ProfileHeaderSectionHolder holder, AdapterModelsClass item)
        {
            try
            {
                if (item.headerSectionClass.UserData != null)
                {
                    string followers = Methods.FunString.FormatPriceValue(Convert.ToInt32(item.headerSectionClass.UserData.Details.DetailsClass.FollowersCount));
                    string following = Methods.FunString.FormatPriceValue(Convert.ToInt32(item.headerSectionClass.UserData.Details.DetailsClass.FollowingCount));
                    string post = Methods.FunString.FormatPriceValue(Convert.ToInt32(item.headerSectionClass.UserData.Details.DetailsClass.PostCount));
                    string likes = Methods.FunString.FormatPriceValue(Convert.ToInt32(item.headerSectionClass.UserData.Details.DetailsClass.LikesCount));
                    string points = Methods.FunString.FormatPriceValue(Convert.ToInt32(item.headerSectionClass.UserData.Points));

                    holder.CountFollowers.Text = followers;
                    holder.CountFollowings.Text = following;
                    holder.CountLikes.Text = likes;
                    holder.CountPoints.Text = points;

                    UserId = item.headerSectionClass.UserData.UserId;
                     
                    switch (AppSettings.ConnectivitySystem)
                    {
                        // Following
                        case 1:
                            holder.CountFollowers.Text = followers;
                            holder.CountFollowings.Text = following;

                            holder.LlCountFollowing.Click += LlCountFollowing_Click;

                            break;
                        // Friend
                        default: 
                            holder.CountFollowers.Text = followers;
                            holder.CountFollowings.Text = post; 
                            break;
                    }
                     
                    holder.LlCountFollowers.Click += LlCountFollowers_Click;
                    holder.LlPoint.Click +=  LlPointOnClick; 
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void LlPointOnClick(object sender, EventArgs e)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(MyPointsActivity)); 
                ActivityContext.StartActivity(intent);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void LlCountFollowing_Click(object sender, EventArgs e)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(MyContactsActivity));
                intent.PutExtra("ContactsType", "Following");
                intent.PutExtra("UserId", UserId);
                ActivityContext.StartActivity(intent);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void LlCountFollowers_Click(object sender, EventArgs e)
        {
            try
            {
                var intent = new Intent(ActivityContext, typeof(MyContactsActivity));
                intent.PutExtra("ContactsType", "Followers");
                intent.PutExtra("UserId", UserId);

                ActivityContext.StartActivity(intent);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void InfoUserBoxBind(AdapterHolders.InfoUserBoxViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                if (item.InfoUserModel.UserData != null)
                {
                    holder.TimeText.Text = Methods.Time.TimeAgo(Convert.ToInt32(item.InfoUserModel.UserData.LastseenUnixTime), false);

                    switch (string.IsNullOrEmpty(item.InfoUserModel.UserData.Website))
                    {
                        case false:
                            holder.WebsiteText.Text = item.InfoUserModel.UserData.Website;
                            holder.LayoutWebsite.Visibility = ViewStates.Visible;
                            break;
                        default:
                            holder.LayoutWebsite.Visibility = ViewStates.Gone;
                            break;
                    }

                    switch (ListUtils.SettingsSiteList?.Genders?.Count)
                    {
                        case > 0:
                        {
                            var value = ListUtils.SettingsSiteList?.Genders?.FirstOrDefault(a => a.Key == item.InfoUserModel.UserData.Gender).Value;
                            if (value != null)
                            {
                                holder.GanderText.Text = value;
                            }
                            else
                            {
                                holder.GanderText.Text = item.InfoUserModel.UserData.GenderText;
                            }

                            break;
                        }
                        default:
                        {
                            if (item.InfoUserModel.UserData.Gender == ActivityContext.GetText(Resource.String.Radio_Male))
                            {
                                holder.GanderText.Text = ActivityContext.GetText(Resource.String.Radio_Male);
                            }
                            else if (item.InfoUserModel.UserData.Gender == ActivityContext.GetText(Resource.String.Radio_Female))
                            {
                                holder.GanderText.Text = ActivityContext.GetText(Resource.String.Radio_Female);
                            }
                            else
                            {
                                holder.GanderText.Text = item.InfoUserModel.UserData.GenderText;
                            }

                            break;
                        }
                    }

                    switch (string.IsNullOrEmpty(item.InfoUserModel.UserData.Birthday))
                    {
                        case false when item.InfoUserModel.UserData.BirthPrivacy != "2" && item.InfoUserModel.UserData.Birthday != "00-00-0000" && item.InfoUserModel.UserData.Birthday != "0000-00-00":
                            try
                            {
                                DateTime date = DateTime.Parse(item.InfoUserModel.UserData.Birthday);
                                string newFormat = date.Day + "/" + date.Month + "/" + date.Year;
                                holder.BirthdayText.Text = newFormat;
                            }
                            catch
                            {
                                //Methods.DisplayReportResultTrack(e);
                                holder.BirthdayText.Text = item.InfoUserModel.UserData.Birthday;
                            }
                            holder.LayoutBirthday.Visibility = ViewStates.Visible;
                            break;
                        default:
                            holder.LayoutBirthday.Visibility = ViewStates.Gone;
                            break;
                    }

                    switch (string.IsNullOrEmpty(item.InfoUserModel.UserData.Working))
                    {
                        case false:
                            holder.WorkText.Text = ActivityContext.GetText(Resource.String.Lbl_WorkingAt) + " " + item.InfoUserModel.UserData.Working;
                            holder.LayoutWork.Visibility = ViewStates.Visible;
                            break;
                        default:
                            holder.LayoutWork.Visibility = ViewStates.Gone;
                            break;
                    }

                    switch (string.IsNullOrEmpty(item.InfoUserModel.UserData.CountryId))
                    {
                        case false when item.InfoUserModel.UserData.CountryId != "0":
                        {
                            var countryName = WoWonderTools.GetCountryList(ActivityContext).FirstOrDefault(a => a.Key  == item.InfoUserModel.UserData.CountryId).Value;
                         
                            holder.LiveText.Text = ActivityContext.GetText(Resource.String.Lbl_LivingIn) + " " + countryName;
                            holder.LayoutLive.Visibility = ViewStates.Visible;
                            break;
                        }
                        default:
                            holder.LayoutLive.Visibility = ViewStates.Gone;
                            break;
                    }

                    switch (string.IsNullOrEmpty(item.InfoUserModel.UserData.School))
                    {
                        case false:
                            holder.StudyText.Text = ActivityContext.GetText(Resource.String.Lbl_StudyingAt) + " " + item.InfoUserModel.UserData.School;
                            holder.LayoutStudy.Visibility = ViewStates.Visible;
                            break;
                        default:
                            holder.LayoutStudy.Visibility = ViewStates.Gone;
                            break;
                    }

                    switch (string.IsNullOrEmpty(item.InfoUserModel.UserData.RelationshipId))
                    {
                        case false when item.InfoUserModel.UserData.RelationshipId != "0":
                        {
                            string relationship = WoWonderTools.GetRelationship(Convert.ToInt32(item.InfoUserModel.UserData.RelationshipId));
                            if (Methods.FunString.StringNullRemover(relationship) != "Empty")
                            {
                                holder.RelationshipText.Text = relationship;
                                holder.LayoutRelationship.Visibility = ViewStates.Visible;
                            }
                            else
                                holder.LayoutRelationship.Visibility = ViewStates.Gone;

                            break;
                        }
                        default:
                            holder.LayoutRelationship.Visibility = ViewStates.Gone;
                            break;
                    }
                }

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void InfoGroupBoxBind(AdapterHolders.InfoGroupBoxViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                if (item.PrivacyModelClass?.GroupClass != null)
                {
                    holder.CategoryText.Text = Methods.FunString.DecodeString(item.PrivacyModelClass?.GroupClass.Category);
                    holder.PrivacyText.Text = ActivityContext.GetText(item.PrivacyModelClass?.GroupClass.Privacy == "1" ? Resource.String.Radio_Public : Resource.String.Radio_Private);
                    holder.IconPrivacy.SetImageResource(item.PrivacyModelClass?.GroupClass.Privacy == "1" ? Resource.Drawable.ic_global_earth : Resource.Drawable.ic_private);

                    if (item.PrivacyModelClass.GroupClass.Members != 0)
                        holder.TxtMembers.Text = Methods.FunString.FormatPriceValue(item.PrivacyModelClass.GroupClass.Members) + " " + ActivityContext.GetString(Resource.String.Lbl_Members);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void InfoPageBoxBind(AdapterHolders.InfoPageBoxViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                if (item.PageInfoModelClass?.PageClass != null)
                {
                    //Extra  
                    holder.LikeCountText.Text = item.PageInfoModelClass?.PageClass.LikesCount;
                     
                    if (item.PageInfoModelClass.PageClass.IsPageOnwer != null && item.PageInfoModelClass.PageClass.IsPageOnwer.Value)
                    {
                        holder.RatingLiner.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        holder.RatingLiner.Visibility = ViewStates.Visible;
                    }
                    holder.RatingBarView.Rating = Float.ParseFloat(item.PageInfoModelClass.PageClass.Rating);

                    holder.CategoryText.Text = new CategoriesController().Get_Translate_Categories_Communities(item.PageInfoModelClass.PageClass.PageCategory, item.PageInfoModelClass.PageClass.Category , "Page");
                     
                    if (Methods.FunString.StringNullRemover(item.PageInfoModelClass.PageClass.About) != "Empty")
                    {
                        var about = Methods.FunString.DecodeString(item.PageInfoModelClass.PageClass.About);
                        NativePostAdapter.ReadMoreOption.AddReadMoreTo(holder.AboutDesc, new String(about));
                    }
                    else
                    {
                        holder.AboutDesc.Text = ActivityContext.GetText(Resource.String.Lbl_NoAnyDescription);
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void StoryBind(AdapterHolders.StoryViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                holder.StoryAdapter.StoryList = item.StoryList.Count switch
                {
                    > 0 => new ObservableCollection<StoryDataObject>(item.StoryList),
                    _ => holder.StoryAdapter.StoryList
                };

                var dataOwner = holder.StoryAdapter.StoryList.FirstOrDefault(a => a.Type == "Your");
                switch (dataOwner)
                {
                    case null:
                        holder.StoryAdapter.StoryList.Insert(0, new StoryDataObject
                        {
                            Avatar = UserDetails.Avatar,
                            Type = "Your",
                            Username = ActivityContext.GetText(Resource.String.Lbl_YourStory),
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

                holder.StoryAdapter.NotifyDataSetChanged();

                holder.AboutMore.Visibility = holder.StoryAdapter?.StoryList?.Count > 4 ? ViewStates.Visible : ViewStates.Invisible;

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void FollowersBoxBind(AdapterHolders.FollowersViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                switch (holder.FollowersAdapter?.MUserFriendsList?.Count)
                {
                    case 0:
                        holder.FollowersAdapter.MUserFriendsList = new ObservableCollection<UserDataObject>(item.FollowersModel.FollowersList);
                        holder.FollowersAdapter.NotifyDataSetChanged();
                        break;
                }

                holder.AboutHead.Text = string.IsNullOrEmpty(item.FollowersModel.TitleHead) switch
                {
                    false => item.FollowersModel.TitleHead,
                    _ => holder.AboutHead.Text
                };

                holder.AboutMore.Text = item.FollowersModel.More;

                holder.AboutMore.Visibility = holder.FollowersAdapter?.MUserFriendsList?.Count > 4 ? ViewStates.Visible : ViewStates.Invisible;

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void GroupsBoxBind(AdapterHolders.GroupsViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                switch (holder.GroupsAdapter?.GroupList?.Count)
                {
                    case 0:
                        holder.GroupsAdapter.GroupList = new ObservableCollection<GroupClass>(item.GroupsModel.GroupsList);
                        holder.GroupsAdapter.NotifyDataSetChanged();
                        break;
                }

                holder.AboutHead.Text = string.IsNullOrEmpty(item.GroupsModel?.TitleHead) switch
                {
                    false => item.GroupsModel?.TitleHead,
                    _ => holder.AboutHead.Text
                };

                holder.AboutMore.Text = item.GroupsModel?.More;

                if (holder.GroupsAdapter != null)
                {
                    holder.AboutMore.Visibility = holder.GroupsAdapter?.GroupList?.Count > 4 ? ViewStates.Visible : ViewStates.Invisible;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void SuggestedGroupsBoxBind(AdapterHolders.SuggestedGroupsViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                switch (holder.GroupsAdapter?.GroupList?.Count)
                {
                    case 0:
                        holder.GroupsAdapter.GroupList = new ObservableCollection<GroupClass>(ListUtils.SuggestedGroupList.Take(12));
                        holder.GroupsAdapter.NotifyDataSetChanged();
                        holder.AboutMore.Text = ListUtils.SuggestedGroupList.Count.ToString();

                        holder.AboutMore.Visibility = holder.GroupsAdapter?.GroupList?.Count > 4 ? ViewStates.Visible : ViewStates.Invisible;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void SuggestedUsersBoxBind(AdapterHolders.SuggestedUsersViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                switch (holder.UsersAdapter?.UserList?.Count)
                {
                    case 0:
                        holder.UsersAdapter.UserList = new ObservableCollection<UserDataObject>(ListUtils.SuggestedUserList.Take(12));
                        holder.UsersAdapter.NotifyDataSetChanged();
                        holder.AboutMore.Text = ListUtils.SuggestedUserList.Count.ToString();

                        holder.AboutMore.Visibility = holder.UsersAdapter?.UserList?.Count > 4 ? ViewStates.Visible : ViewStates.Invisible;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        
        public void ImagesBoxBind(AdapterHolders.ImagesViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                holder.AboutHead.Text = string.IsNullOrEmpty(item.ImagesModel.TitleHead) switch
                {
                    false => item.ImagesModel.TitleHead,
                    _ => holder.AboutHead.Text
                };

                holder.AboutMore.Text = item.ImagesModel.More;

                switch (item.ImagesModel?.ImagesList)
                {
                    case null:
                        holder.MainView.Visibility = ViewStates.Gone;
                        return;
                }

                if (holder.MainView.Visibility != ViewStates.Visible)
                    holder.MainView.Visibility = ViewStates.Visible;

                switch (holder.ImagesAdapter?.UserPhotosList?.Count)
                {
                    case 0:
                        holder.ImagesAdapter.UserPhotosList = new ObservableCollection<PostDataObject>(item.ImagesModel.ImagesList);
                        holder.ImagesAdapter.NotifyDataSetChanged();
                        break;
                }

                holder.AboutMore.Visibility = holder.ImagesAdapter?.UserPhotosList?.Count > 3 ? ViewStates.Visible : ViewStates.Invisible;

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void PagesBoxBind(AdapterHolders.PagesViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                switch (holder.PagesAdapter?.PageList?.Count)
                {
                    case 0:
                        holder.PagesAdapter.PageList = new ObservableCollection<PageClass>(item.PagesModel.PagesList);
                        holder.PagesAdapter.NotifyDataSetChanged();
                        break;
                }

                holder.AboutHead.Text = string.IsNullOrEmpty(item.PagesModel?.TitleHead) switch
                {
                    false => item.PagesModel?.TitleHead,
                    _ => holder.AboutHead.Text
                };

                holder.AboutMore.Text = item.PagesModel?.More;

                if (holder.PagesAdapter != null)
                {
                    holder.AboutMore.Visibility = holder.PagesAdapter?.PageList?.Count > 4 ? ViewStates.Visible : ViewStates.Invisible;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        /*public void PagesBoxBind(AdapterHolders.PagesViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                holder.AboutHead.Text = string.IsNullOrEmpty(item.PagesModel?.TitleHead) switch
                {
                    false => item.PagesModel?.TitleHead,
                    _ => holder.AboutHead.Text
                };

                holder.AboutMore.Text = string.IsNullOrEmpty(item.PagesModel?.More) switch
                {
                    false => item.PagesModel?.More,
                    _ => holder.AboutMore.Text
                };

                var count = item.PagesModel?.PagesList.Count;
                Console.WriteLine(count);

                try
                {
                    switch (item.PagesModel?.PagesList.Count)
                    {
                        case > 0 when !string.IsNullOrEmpty(item.PagesModel?.PagesList[0]?.Avatar):
                            GlideImageLoader.LoadImage(ActivityContext, item.PagesModel?.PagesList[0]?.Avatar, holder.PageImage1, ImageStyle.CircleCrop, ImagePlaceholders.Color);
                            break;
                    }

                    switch (item.PagesModel?.PagesList.Count)
                    {
                        case > 1 when !string.IsNullOrEmpty(item.PagesModel?.PagesList[1]?.Avatar):
                            GlideImageLoader.LoadImage(ActivityContext, item.PagesModel?.PagesList[1]?.Avatar, holder.PageImage2, ImageStyle.CircleCrop, ImagePlaceholders.Color);
                            break;
                    }

                    switch (item.PagesModel?.PagesList.Count)
                    {
                        case > 2 when !string.IsNullOrEmpty(item.PagesModel?.PagesList[2]?.Avatar):
                            GlideImageLoader.LoadImage(ActivityContext, item.PagesModel?.PagesList[2]?.Avatar, holder.PageImage1, ImageStyle.CircleCrop, ImagePlaceholders.Color);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }*/

        public void AdsPostBind(AdapterHolders.AdsPostViewHolder holder, AdapterModelsClass item)
        {
            try
            {
                NativePostAdapter.FullGlideRequestBuilder.Load(item.PostData.UserData.Avatar).Into(holder.UserAvatar);

                if (string.IsNullOrEmpty(item.PostData.AdMedia))
                    holder.Image.Visibility = ViewStates.Gone;
                else
                    NativePostAdapter.FullGlideRequestBuilder.Load(item.PostData.AdMedia).Into(holder.Image);

                holder.Username.Text = item.PostData.Name;
                holder.TimeText.Text = item.PostData.Posted;
                holder.TextLocation.Text = item.PostData.Location;

                if (string.IsNullOrEmpty(item.PostData.Orginaltext))
                {
                    if (holder.Description.Visibility != ViewStates.Gone)
                        holder.Description.Visibility = ViewStates.Gone;
                }
                else
                {
                    if (holder.Description.Visibility != ViewStates.Visible)
                        holder.Description.Visibility = ViewStates.Visible;

                    if (!holder.Description.Text.Contains(ActivityContext.GetText(Resource.String.Lbl_ReadMore)) && !holder.Description.Text.Contains(ActivityContext.GetText(Resource.String.Lbl_ReadLess)))
                    {
                        switch (item.PostData.RegexFilterList != null & item.PostData.RegexFilterList?.Count > 0)
                        {
                            case true:
                                holder.Description.SetAutoLinkOnClickListener(NativePostAdapter, item.PostData.RegexFilterList);
                                break;
                            default:
                                holder.Description.SetAutoLinkOnClickListener(NativePostAdapter, new Dictionary<string, string>());
                                break;
                        }

                        NativePostAdapter.ReadMoreOption.AddReadMoreTo(holder.Description, new String(item.PostData.Orginaltext));
                    }
                    //else if (holder.Description.Text.Contains(ActivityContext.GetText(Resource.String.Lbl_ReadLess)))
                    //{
                    //    NativePostAdapter.ReadMoreOption.AddReadLess(holder.Description, new String(item.PostData.Orginaltext));
                    //}
                    else
                    {
                        switch (item.PostData.RegexFilterList != null & item.PostData.RegexFilterList?.Count > 0)
                        {
                            case true:
                                holder.Description.SetAutoLinkOnClickListener(NativePostAdapter, item.PostData.RegexFilterList);
                                break;
                            default:
                                holder.Description.SetAutoLinkOnClickListener(NativePostAdapter, new Dictionary<string, string>());
                                break;
                        }

                        NativePostAdapter.ReadMoreOption.AddReadMoreTo(holder.Description, new String(item.PostData.Orginaltext));
                    }
                }

                TextSanitizer headlineSanitizer = new TextSanitizer(holder.Headline, ActivityContext);
                headlineSanitizer.Load(item.PostData.Headline);

                holder.LinkUrl.Text = item.PostData.Url;

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

    }
}
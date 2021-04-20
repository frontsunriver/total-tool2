using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;


using Android.Views;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Com.Tuyenmonkey.Textdecorator;
using Java.IO;
using Java.Util;
using WoWonder.Activities.Comment.Fragment;
using WoWonder.Activities.MyProfile;
using WoWonder.Activities.NativePost.Pages;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Activities.UserProfile;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.Library.Anjo;
using WoWonder.Library.Anjo.SuperTextLibrary;
using WoWonder.SQLite;
using WoWonderClient;
using WoWonderClient.Classes.Comments;
using IList = System.Collections.IList;
using Object = Java.Lang.Object;
using Reaction = WoWonderClient.Classes.Posts.Reaction;
using Timer = System.Timers.Timer;

namespace WoWonder.Activities.Comment.Adapters
{
    public class CommentObjectExtra : GetCommentObject
    {
        public new MediaPlayer MediaPlayer { get; set; }
        public new Timer MediaTimer { get; set; } 
    }

    public class CommentAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider ,StTools.IXAutoLinkOnClickListener
    {
        public string EmptyState = "Wo_Empty_State";
        public readonly Activity ActivityContext;

        public ObservableCollection<CommentObjectExtra> CommentList = new ObservableCollection<CommentObjectExtra>();
        private readonly CommentClickListener PostEventListener;
        private readonly StReadMoreOption ReadMoreOption;

        public CommentAdapter(Activity context)
        {
            try
            {
                HasStableIds = true;
                ActivityContext = context;
                PostEventListener = new CommentClickListener(ActivityContext, "Comment");

                ReadMoreOption = new StReadMoreOption.Builder()
                    .TextLength(250, StReadMoreOption.TypeCharacter)
                    .MoreLabel(ActivityContext.GetText(Resource.String.Lbl_ReadMore))
                    .LessLabel(ActivityContext.GetText(Resource.String.Lbl_ReadLess))
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

        public override int ItemCount => CommentList?.Count ?? 0;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                return viewType switch
                {
                    0 => new CommentAdapterViewHolder(
                        LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_Comment, parent, false),
                        this, PostEventListener),
                    1 => new CommentAdapterViewHolder(
                        LayoutInflater.From(parent.Context)
                            ?.Inflate(Resource.Layout.Style_Comment_Image, parent, false), this, PostEventListener),
                    2 => new CommentAdapterViewHolder(
                        LayoutInflater.From(parent.Context)
                            ?.Inflate(Resource.Layout.Style_Comment_Voice, parent, false), this, PostEventListener),
                    666 => new AdapterHolders.EmptyStateAdapterViewHolder(LayoutInflater.From(parent.Context)
                        ?.Inflate(Resource.Layout.Style_EmptyState, parent, false)),
                    _ => new CommentAdapterViewHolder(
                        LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_Comment, parent, false),
                        this, PostEventListener)
                };
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }

        public void LoadCommentData(CommentObjectExtra item, CommentAdapterViewHolder holder)
        {
            try
            {
                if (!string.IsNullOrEmpty(item.Orginaltext) || !string.IsNullOrWhiteSpace(item.Orginaltext))
                {
                    var text = Methods.FunString.DecodeString(item.Orginaltext);
                    ReadMoreOption.AddReadMoreTo(holder.CommentText, new Java.Lang.String(text));
                    holder.CommentText.SetAutoLinkOnClickListener(this, new Dictionary<string, string>());
                }
                else
                {
                    holder.CommentText.Visibility = ViewStates.Gone;
                }

                holder.TimeTextView.Text = Methods.Time.TimeAgo(Convert.ToInt32(item.Time) , true);
                holder.UserName.Text = item.Publisher.Name;

                GlideImageLoader.LoadImage(ActivityContext, item.Publisher.Avatar, holder.Image, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);

                var textHighLighter = WoWonderTools.GetNameFinal(item.Publisher);
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
                    switch (string.IsNullOrEmpty(item.CFile))
                    {
                        case false when item.CFile.Contains("file://") || item.CFile.Contains("content://") || item.CFile.Contains("storage") || item.CFile.Contains("/data/user/0/"):
                        {
                            File file2 = new File(item.CFile);
                            var photoUri = FileProvider.GetUriForFile(ActivityContext, ActivityContext.PackageName + ".fileprovider", file2);
                            Glide.With(ActivityContext).Load(photoUri).Apply(new RequestOptions()).Into(holder.CommentImage);

                            //GlideImageLoader.LoadImage(ActivityContext,item.CFile, holder.CommentImage, ImageStyle.CenterCrop, ImagePlaceholders.Color);
                            break;
                        }
                        default:
                        {
                            item.CFile = item.CFile.Contains(Client.WebsiteUrl) switch
                            {
                                false => WoWonderTools.GetTheFinalLink(item.CFile),
                                _ => item.CFile
                            };

                            GlideImageLoader.LoadImage(ActivityContext, item.CFile, holder.CommentImage, ImageStyle.CenterCrop, ImagePlaceholders.Color);
                            item.CFile = WoWonderTools.GetFile("", Methods.Path.FolderDiskImage, item.CFile.Split('/').Last(), item.CFile);
                            break;
                        }
                    }
                }

                //Voice
                if (holder.VoiceLayout != null && !string.IsNullOrEmpty(item.Record))
                {
                    LoadAudioItem(holder, item);
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
                    case PostButtonSystem.Wonder:
                    case PostButtonSystem.DisLike:
                    {
                        if (item.Reaction?.IsReacted != null && !item.Reaction.IsReacted.Value)
                            ReactionComment.SetReactionPack(holder, ReactConstants.Default);

                        switch (item.IsCommentLiked)
                        {
                            case true:
                                holder.LikeTextView.Text = ActivityContext.GetText(Resource.String.Btn_Liked);
                                holder.LikeTextView.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                                holder.LikeTextView.Tag = "Liked";
                                break;
                        }

                        switch (AppSettings.PostButton)
                        {
                            case PostButtonSystem.Wonder when item.IsCommentWondered:
                            {
                                holder.DislikeTextView.Text = ActivityContext.GetString(Resource.String.Lbl_wondered);
                                holder.DislikeTextView.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                                holder.DislikeTextView.Tag = "Disliked";
                                break;
                            }
                            case PostButtonSystem.Wonder:
                            {
                                holder.DislikeTextView.Text = ActivityContext.GetString(Resource.String.Btn_Wonder);
                                holder.DislikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                                holder.DislikeTextView.Tag = "Dislike";
                                break;
                            }
                            case PostButtonSystem.DisLike when item.IsCommentWondered:
                            {
                                holder.DislikeTextView.Text = ActivityContext.GetString(Resource.String.Lbl_disliked);
                                holder.DislikeTextView.SetTextColor(Color.ParseColor("#f89823"));
                                holder.DislikeTextView.Tag = "Disliked";
                                break;
                            }
                            case PostButtonSystem.DisLike:
                            {
                                holder.DislikeTextView.Text = ActivityContext.GetString(Resource.String.Btn_Dislike);
                                holder.DislikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                                holder.DislikeTextView.Tag = "Dislike";
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

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            try
            {
                switch (viewHolder.ItemViewType)
                {
                    case 666:
                    {
                        if (viewHolder is not AdapterHolders.EmptyStateAdapterViewHolder emptyHolder)
                            return;

                        var itemEmpty = CommentList.FirstOrDefault(a => a.Id == EmptyState);
                        if (itemEmpty != null && !string.IsNullOrEmpty(itemEmpty.Orginaltext))
                        {
                            emptyHolder.EmptyText.Text = itemEmpty.Orginaltext;
                        }
                        else
                        {
                            emptyHolder.EmptyText.Text = ActivityContext.GetText(Resource.String.Lbl_NoComments);
                        }
                        return;
                    }
                }

                if (viewHolder is not CommentAdapterViewHolder holder)
                    return;

                var item = CommentList[position];
                switch (item)
                {
                    case null:
                        return;
                    default:
                        LoadCommentData(item, holder);
                        break;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public int PositionSound;

        private void LoadAudioItem(CommentAdapterViewHolder soundViewHolder, CommentObjectExtra item)
        {
            try
            { 
                soundViewHolder.VoiceLayout.Visibility = ViewStates.Visible;

                var fileName = item.Record.Split('/').Last();

                var mediaFile = WoWonderTools.GetFile(item.PostId, Methods.Path.FolderDcimSound, fileName, item.Record);

                if (string.IsNullOrEmpty(item.MediaDuration) || item.MediaDuration == "00:00")
                {
                    var duration = WoWonderTools.GetDuration(mediaFile);
                    soundViewHolder.DurationVoice.Text = Methods.AudioRecorderAndPlayer.GetTimeString(duration);
                }
                else
                    soundViewHolder.DurationVoice.Text = item.MediaDuration;

                soundViewHolder.PlayButton.Visibility = ViewStates.Visible;

                switch (item.MediaIsPlaying)
                {
                    case true:
                        soundViewHolder.PlayButton.SetImageResource(AppSettings.SetTabDarkTheme ? Resource.Drawable.ic_media_pause_light : Resource.Drawable.ic_media_pause_dark);
                        break;
                    default:
                        soundViewHolder.PlayButton.SetImageResource(Resource.Drawable.ic_play_dark_arrow);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public CommentObjectExtra GetItem(int position)
        {
            return CommentList[position];
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
                var item = CommentList[position];

                if (string.IsNullOrEmpty(item.CFile) && string.IsNullOrEmpty(item.Record) && item.Text != EmptyState)
                    return 0;

                if (!string.IsNullOrEmpty(item.CFile) && !string.IsNullOrEmpty(item.Record) || !string.IsNullOrEmpty(item.CFile))
                    return 1;

                switch (string.IsNullOrEmpty(item.Record))
                {
                    case false:
                        return 2;
                }

                if (item.Text == EmptyState)
                    return 666;

                return 0;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return 0;
            }
        }

        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = CommentList[p0];
                switch (item)
                {
                    case null:
                        return d;
                    default:
                    {
                        if (item.Text != EmptyState)
                        {
                            switch (string.IsNullOrEmpty(item.CFile))
                            {
                                case false:
                                    d.Add(item.CFile);
                                    break;
                            }

                            switch (string.IsNullOrEmpty(item.Publisher.Avatar))
                            {
                                case false:
                                    d.Add(item.Publisher.Avatar);
                                    break;
                            }

                            return d;
                        }

                        return Collections.SingletonList(p0);
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return Collections.SingletonList(p0);
            }
        }

        public RequestBuilder GetPreloadRequestBuilder(Object p0)
        {
            return GlideImageLoader.GetPreLoadRequestBuilder(ActivityContext, p0.ToString(), ImageStyle.CenterCrop);
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
}

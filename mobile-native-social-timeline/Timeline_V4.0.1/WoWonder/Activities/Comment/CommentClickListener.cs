using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using AFollestad.MaterialDialogs;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;

using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using Java.Lang;
using Newtonsoft.Json;
using WoWonder.Activities.Comment.Adapters;
using WoWonder.Activities.Comment.Fragment;
using WoWonder.Activities.NativePost.Extra;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient;
using WoWonderClient.Requests;
using Exception = System.Exception;
using Uri = Android.Net.Uri;
  
namespace WoWonder.Activities.Comment
{
    public class CommentClickListener : Java.Lang.Object , MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback, MaterialDialog.IInputCallback
    {
        private readonly Activity MainContext;
        private CommentObjectExtra CommentObject;
        private string TypeDialog;
        private readonly string TypeClass;

        public CommentClickListener(Activity context, string typeClass)
        {
            MainContext = context;
            TypeClass = typeClass;
        }

        public void ProfilePostClick(ProfileClickEventArgs e)
        {
            try
            {
                WoWonderTools.OpenProfile(MainContext, e.CommentClass.UserId, e.CommentClass.Publisher);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void MoreCommentReplyPostClick(CommentReplyClickEventArgs e)
        {
            try
            {
                if (Methods.CheckConnectivity())
                {
                    TypeDialog = "MoreComment";
                    CommentObject = e.CommentObject;

                    var arrayAdapter = new List<string>();
                    var dialogList = new MaterialDialog.Builder(MainContext).Theme(AppSettings.SetTabDarkTheme ? Theme.Dark : Theme.Light);

                    arrayAdapter.Add(MainContext.GetString(Resource.String.Lbl_CopeText));
                    arrayAdapter.Add(MainContext.GetString(Resource.String.Lbl_Report));

                    if (CommentObject?.Owner != null && CommentObject.Owner.Value || CommentObject?.Publisher?.UserId == UserDetails.UserId)
                    {
                        arrayAdapter.Add(MainContext.GetString(Resource.String.Lbl_Edit));
                        arrayAdapter.Add(MainContext.GetString(Resource.String.Lbl_Delete));
                    }

                    dialogList.Title(MainContext.GetString(Resource.String.Lbl_More)).TitleColorRes(Resource.Color.primary);
                    dialogList.Items(arrayAdapter);
                    dialogList.PositiveText(MainContext.GetText(Resource.String.Lbl_Close)).OnNegative(this);
                    dialogList.AlwaysCallSingleChoiceCallback();
                    dialogList.ItemsCallback(this).Build().Show();
                }
                else
                {
                    Toast.MakeText(MainContext, MainContext.GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //Event Menu >> Delete Comment
        private void DeleteCommentEvent(CommentObjectExtra item)
        {
            try
            {
                if (Methods.CheckConnectivity())
                {
                    TypeDialog = "DeleteComment";
                    CommentObject = item;

                    var dialog = new MaterialDialog.Builder(MainContext).Theme(AppSettings.SetTabDarkTheme ? Theme.Dark : Theme.Light);
                    dialog.Title(MainContext.GetText(Resource.String.Lbl_DeleteComment)).TitleColorRes(Resource.Color.primary);
                    dialog.Content(MainContext.GetText(Resource.String.Lbl_AreYouSureDeleteComment));
                    dialog.PositiveText(MainContext.GetText(Resource.String.Lbl_Yes)).OnPositive(this);
                    dialog.NegativeText(MainContext.GetText(Resource.String.Lbl_No)).OnNegative(this);
                    dialog.AlwaysCallSingleChoiceCallback();
                    dialog.ItemsCallback(this).Build().Show();
                }
                else
                {
                    Toast.MakeText(MainContext, MainContext.GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        //Event Menu >> Edit Comment
        private void EditCommentEvent(CommentObjectExtra item)
        {
            try
            {
                if (Methods.CheckConnectivity())
                {
                    TypeDialog = "EditComment";
                    CommentObject = item;

                    var dialog = new MaterialDialog.Builder(MainContext).Theme(AppSettings.SetTabDarkTheme ? Theme.Dark : Theme.Light);

                    dialog.Title(Resource.String.Lbl_Edit).TitleColorRes(Resource.Color.primary);
                    dialog.Input(MainContext.GetString(Resource.String.Lbl_Write_comment), Methods.FunString.DecodeString(item.Text), this);
                    
                    dialog.InputType(InputTypes.TextFlagImeMultiLine);
                    dialog.PositiveText(MainContext.GetText(Resource.String.Lbl_Update)).OnPositive(this);
                    dialog.NegativeText(MainContext.GetText(Resource.String.Lbl_Cancel)).OnNegative(this);
                    dialog.Build().Show();
                    dialog.AlwaysCallSingleChoiceCallback();
                }
                else
                {
                    Toast.MakeText(MainContext, MainContext.GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
          
        public void CommentReplyPostClick(CommentReplyClickEventArgs e)
        {
            try
            {
                switch (TypeClass)
                {
                    case "Reply":
                    {
                        var txtComment = ReplyCommentActivity.GetInstance().TxtComment;
                        if (txtComment != null)
                        {
                            txtComment.Text = "";
                            txtComment.Text = "@" + e.CommentObject.Publisher.Username + " ";
                        }

                        break;
                    }
                    default:
                    {
                        var intent = new Intent(MainContext, typeof(ReplyCommentActivity));
                        intent.PutExtra("CommentId", e.CommentObject.Id);
                        intent.PutExtra("CommentObject", JsonConvert.SerializeObject(e.CommentObject));
                        MainContext.StartActivity(intent);
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        public void LikeCommentReplyPostClick(CommentReplyClickEventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(MainContext, MainContext.GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    return;
                }

                switch (e.Holder.LikeTextView?.Tag?.ToString())
                {
                    case "Liked":
                    {
                        e.Holder.LikeTextView.Text = MainContext.GetText(Resource.String.Btn_Like);
                        //e.Holder.LikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                        e.Holder.LikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.ParseColor("#888888"));
                        e.Holder.LikeTextView.Tag = "Like";

                        switch (AppSettings.PostButton)
                        {
                            case PostButtonSystem.ReactionDefault:
                            case PostButtonSystem.ReactionSubShine:
                            {
                                var x = e.CommentObject.Reaction.Count;
                                switch (x)
                                {
                                    case > 0:
                                        e.CommentObject.Reaction.Count--;
                                        break;
                                    default:
                                        e.CommentObject.Reaction.Count = 0;
                                        break;
                                }

                                e.CommentObject.Reaction.IsReacted = false;
                                e.CommentObject.Reaction.Type = "";
                         
                                if (e.Holder.CountLike != null && e.CommentObject.Reaction.Count > 0)
                                {
                                    e.Holder.CountLikeSection.Visibility = ViewStates.Visible;
                                    e.Holder.CountLike.Text = Methods.FunString.FormatPriceValue(e.CommentObject.Reaction.Count);
                                }
                                else
                                {
                                    e.Holder.CountLikeSection.Visibility = ViewStates.Gone;
                                }
                         
                                PollyController.RunRetryPolicyFunction(TypeClass == "Reply" ? new List<Func<Task>> {() => RequestsAsync.Comment.ReactionCommentAsync(e.CommentObject.Id, "" , "reaction_reply") } : new List<Func<Task>> {() => RequestsAsync.Comment.ReactionCommentAsync(e.CommentObject.Id, "") });
                                break;
                            }
                            default:
                                e.CommentObject.IsCommentLiked = false;

                                PollyController.RunRetryPolicyFunction(TypeClass == "Reply" ? new List<Func<Task>> {() => RequestsAsync.Comment.LikeUnLikeCommentAsync(e.CommentObject.Id, "reply_like")} : new List<Func<Task>> {() => RequestsAsync.Comment.LikeUnLikeCommentAsync(e.CommentObject.Id, "comment_like")});
                                break;
                        }

                        break;
                    }
                    default:
                    {
                        switch (AppSettings.PostButton)
                        {
                            case PostButtonSystem.ReactionDefault:
                            case PostButtonSystem.ReactionSubShine:
                                new ReactionComment(MainContext, TypeClass)?.ClickDialog(e);
                                break;
                            default:
                                e.CommentObject.IsCommentLiked = true;

                                e.Holder.LikeTextView.Text = MainContext.GetText(Resource.String.Btn_Liked);
                                e.Holder.LikeTextView.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                                e.Holder.LikeTextView.Tag = "Liked";

                                PollyController.RunRetryPolicyFunction(TypeClass == "Reply" ? new List<Func<Task>> {() => RequestsAsync.Comment.LikeUnLikeCommentAsync(e.CommentObject.Id, "reply_like")} : new List<Func<Task>> {() => RequestsAsync.Comment.LikeUnLikeCommentAsync(e.CommentObject.Id, "comment_like")});
                                break;
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
         
        public void DislikeCommentReplyPostClick(CommentReplyClickEventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(MainContext, MainContext.GetText(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    return;
                }

                e.CommentObject.IsCommentWondered = e.Holder.DislikeTextView?.Tag?.ToString() != "Disliked";

                PollyController.RunRetryPolicyFunction(TypeClass == "Reply" ? new List<Func<Task>> {() => RequestsAsync.Comment.DislikeUnDislikeCommentAsync(e.CommentObject.Id, "reply_dislike")}
                                                                            : new List<Func<Task>> {() => RequestsAsync.Comment.DislikeUnDislikeCommentAsync(e.CommentObject.Id, "comment_dislike")});

                switch (AppSettings.PostButton)
                {
                    case PostButtonSystem.Wonder when e.CommentObject.IsCommentWondered:
                        {
                            e.Holder.DislikeTextView.Text = MainContext.GetString(Resource.String.Lbl_wondered);
                            e.Holder.DislikeTextView.SetTextColor(Color.ParseColor(AppSettings.MainColor));
                            e.Holder.DislikeTextView.Tag = "Disliked";
                            break;
                        }
                    case PostButtonSystem.Wonder:
                        {
                            e.Holder.DislikeTextView.Text = MainContext.GetString(Resource.String.Btn_Wonder);
                            e.Holder.DislikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                            e.Holder.DislikeTextView.Tag = "Dislike";
                            break;
                        }
                    case PostButtonSystem.DisLike when e.CommentObject.IsCommentWondered:
                        {
                            e.Holder.DislikeTextView.Text = MainContext.GetString(Resource.String.Lbl_disliked);
                            e.Holder.DislikeTextView.SetTextColor(Color.ParseColor("#f89823"));
                            e.Holder.DislikeTextView.Tag = "Disliked";
                            break;
                        }
                    case PostButtonSystem.DisLike:
                        {
                            e.Holder.DislikeTextView.Text = MainContext.GetString(Resource.String.Btn_Dislike);
                            e.Holder.DislikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                            e.Holder.DislikeTextView.Tag = "Dislike";
                            break;
                        }
                }

                switch (e.Holder.LikeTextView?.Tag?.ToString())
                {
                    case "Liked":
                        e.CommentObject.IsCommentLiked = false;

                        e.Holder.LikeTextView.Text = MainContext.GetText(Resource.String.Btn_Like);
                        //e.Holder.LikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);
                        e.Holder.LikeTextView.SetTextColor(AppSettings.SetTabDarkTheme ? Color.White : Color.ParseColor("#888888"));
                        e.Holder.LikeTextView.Tag = "Like";
                        break;
                } 
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        public void CountLikeCommentReplyPostClick(CommentReplyClickEventArgs e)
        {
            try
            { 
                var intent = new Intent(MainContext, typeof(ReactionCommentTabbedActivity));
                intent.PutExtra("TypeClass", TypeClass);
                intent.PutExtra("CommentObject", JsonConvert.SerializeObject(e.CommentObject));
                MainContext.StartActivity(intent); 
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }
         
        public void OpenImageLightBox(CommentObjectExtra item)
        {
            try
            {
                switch (item)
                {
                    case null:
                        return;
                }
                string imageUrl;

                switch (string.IsNullOrEmpty(item.CFile))
                {
                    case false when item.CFile.Contains("file://") || item.CFile.Contains("content://") || item.CFile.Contains("storage") || item.CFile.Contains("/data/user/0/"):
                        imageUrl = item.CFile;
                        break;
                    default:
                    {
                        item.CFile = item.CFile.Contains(Client.WebsiteUrl) switch
                        {
                            false => WoWonderTools.GetTheFinalLink(item.CFile),
                            _ => item.CFile
                        };

                        imageUrl = item.CFile;
                        break;
                    }
                }

                MainContext?.RunOnUiThread(() =>
                { 
                    var media = WoWonderTools.GetFile("", Methods.Path.FolderDiskImage, imageUrl.Split('/').Last(), imageUrl);
                    if (media.Contains("http"))
                    {
                        Intent intent = new Intent(Intent.ActionView, Uri.Parse(media));
                        MainContext.StartActivity(intent);
                    }
                    else
                    {
                        Java.IO.File file2 = new Java.IO.File(media);
                        var photoUri = FileProvider.GetUriForFile(MainContext, MainContext.PackageName + ".fileprovider", file2);

                        Intent intent = new Intent(Intent.ActionPick);
                        intent.SetAction(Intent.ActionView);
                        intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                        intent.SetDataAndType(photoUri, "image/*");
                        MainContext.StartActivity(intent);
                    }

                    //var getImage = Methods.MultiMedia.GetMediaFrom_Gallery(Methods.Path.FolderDcimImage, fileName);
                    //if (getImage != "File Dont Exists")
                    //{
                    //    Java.IO.File file2 = new Java.IO.File(getImage);
                    //    var photoUri = FileProvider.GetUriForFile(MainContext, MainContext.PackageName + ".fileprovider", file2);

                    //    Intent intent = new Intent(Intent.ActionPick);
                    //    intent.SetAction(Intent.ActionView);
                    //    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    //    intent.SetDataAndType(photoUri, "image/*");
                    //    MainContext.StartActivity(intent);
                    //}
                    //else
                    //{
                    //    string filename = imageUrl.Split('/').Last();
                    //    string filePath = Path.Combine(Methods.Path.FolderDcimImage);
                    //    string mediaFile = filePath + "/" + filename;

                    //    if (!Directory.Exists(filePath))
                    //        Directory.CreateDirectory(filePath);

                    //    if (!File.Exists(mediaFile))
                    //    {
                    //        WebClient webClient = new WebClient();
                    //        AndHUD.Shared.Show(MainContext, MainContext.GetText(Resource.String.Lbl_Loading));

                    //        webClient.DownloadDataAsync(new Uri(imageUrl));
                    //        webClient.DownloadProgressChanged += (sender, args) =>
                    //        {
                    //            //var progress = args.ProgressPercentage;
                    //            // holder.loadingProgressview.Progress = progress;
                    //            //Show a progress
                    //            AndHUD.Shared.Show(MainContext, MainContext.GetText(Resource.String.Lbl_Loading));
                    //        };
                    //        webClient.DownloadDataCompleted += (s, e) =>
                    //        {
                    //            try
                    //            {
                    //                File.WriteAllBytes(mediaFile, e.Result);

                    //                getImage = Methods.MultiMedia.GetMediaFrom_Gallery(Methods.Path.FolderDcimImage, fileName);
                    //                if (getImage != "File Dont Exists")
                    //                {
                    //                    Java.IO.File file2 = new Java.IO.File(getImage);

                    //                    Android.Net.Uri photoUri = FileProvider.GetUriForFile(MainContext, MainContext.PackageName + ".fileprovider", file2);

                    //                    Intent intent = new Intent(Intent.ActionPick);
                    //                    intent.SetAction(Intent.ActionView);
                    //                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    //                    intent.SetDataAndType(photoUri, "image/*");
                    //                    MainContext.StartActivity(intent);
                    //                }
                    //                else
                    //                {
                    //                    Toast.MakeText(MainContext, MainContext.GetText(Resource.String.Lbl_something_went_wrong), ToastLength.Long)?.Show();
                    //                }
                    //            }
                    //            catch (Exception exception)
                    //            {
                    //                Methods.DisplayReportResultTrack(exception);
                    //            }

                    //            //var mediaScanIntent = new Intent(Intent?.ActionMediaScannerScanFile);
                    //            //mediaScanIntent?.SetData(Uri.FromFile(new File(mediaFile)));
                    //            //Application.Context.SendBroadcast(mediaScanIntent);

                    //            // Tell the media scanner about the new file so that it is
                    //            // immediately available to the user.
                    //            MediaScannerConnection.ScanFile(Application.Context, new[] { mediaFile }, null, null);

                    //            AndHUD.Shared.Dismiss(MainContext);
                    //        };
                    //    }
                    //}
                });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void PlaySound(CommentReplyClickEventArgs args)
        {
            try
            {
                if (args.Holder.CommentAdapter.PositionSound != args.Position)
                {
                    var list = args.Holder.CommentAdapter.CommentList.Where(a => a.MediaPlayer != null).ToList();
                    switch (list.Count)
                    {
                        case > 0:
                        {
                            foreach (var item in list)
                            {
                                item.MediaIsPlaying = false;

                                if (item.MediaPlayer != null)
                                {
                                    item.MediaPlayer.Stop();
                                    item.MediaPlayer.Reset();
                                }
                                item.MediaPlayer = null!;
                                item.MediaTimer = null!;

                                item.MediaPlayer?.Release();
                                item.MediaPlayer = null!;
                            }

                            args.Holder.CommentAdapter.NotifyItemChanged(args.Holder.CommentAdapter.PositionSound, "WithoutBlobAudio");
                            break;
                        }
                    }
                }

                var fileName = args.CommentObject.Record.Split('/').Last();
                var mediaFile = WoWonderTools.GetFile(args.CommentObject.PostId, Methods.Path.FolderDcimSound, fileName, args.CommentObject.Record);
                 
                if (string.IsNullOrEmpty(args.CommentObject.MediaDuration) || args.CommentObject.MediaDuration == "00:00")
                {
                    var duration = WoWonderTools.GetDuration(mediaFile);
                    args.Holder.DurationVoice.Text = Methods.AudioRecorderAndPlayer.GetTimeString(duration);
                }
                else
                    args.Holder.DurationVoice.Text = args.CommentObject.MediaDuration;

                if (mediaFile.Contains("http"))
                    mediaFile = WoWonderTools.GetFile(args.CommentObject.PostId, Methods.Path.FolderDcimSound, fileName, args.CommentObject.Record);

                switch (args.CommentObject.MediaPlayer)
                {
                    case null:
                    {
                        args.Holder.DurationVoice.Text = "00:00";
                        args.Holder.CommentAdapter.PositionSound = args.Position;
                        args.CommentObject.MediaPlayer = new MediaPlayer();
                        args.CommentObject.MediaPlayer.SetAudioAttributes(new AudioAttributes.Builder().SetUsage(AudioUsageKind.Media).SetContentType(AudioContentType.Music).Build());

                        args.CommentObject.MediaPlayer.Completion += (sender, e) =>
                        {
                            try
                            {
                                args.Holder.PlayButton.Tag = "Play";
                                args.Holder.PlayButton.SetImageResource(Resource.Drawable.ic_play_dark_arrow);

                                args.CommentObject.MediaIsPlaying = false;

                                args.CommentObject.MediaPlayer.Stop();
                                args.CommentObject.MediaPlayer.Reset();
                                args.CommentObject.MediaPlayer = null!;

                                args.CommentObject.MediaTimer.Enabled = false;
                                args.CommentObject.MediaTimer.Stop();
                                args.CommentObject.MediaTimer = null!;
                            }
                            catch (Exception exception)
                            {
                                Methods.DisplayReportResultTrack(exception);
                            }
                        };

                        args.CommentObject.MediaPlayer.Prepared += (s, ee) =>
                        {
                            try
                            {
                                args.CommentObject.MediaIsPlaying = true;
                                args.Holder.PlayButton.Tag = "Pause";
                                args.Holder.PlayButton.SetImageResource(AppSettings.SetTabDarkTheme ? Resource.Drawable.ic_media_pause_light : Resource.Drawable.ic_media_pause_dark);
                             
                                args.CommentObject.MediaTimer ??= new Timer { Interval = 1000 };

                                args.CommentObject.MediaPlayer.Start();

                                //var durationOfSound = message.MediaPlayer.Duration;

                                args.CommentObject.MediaTimer.Elapsed += (sender, eventArgs) =>
                                {
                                    args.Holder.CommentAdapter.ActivityContext?.RunOnUiThread(() =>
                                    {
                                        try
                                        {
                                            if (args.CommentObject.MediaTimer != null && args.CommentObject.MediaTimer.Enabled)
                                            {
                                                if (args.CommentObject.MediaPlayer.CurrentPosition <= args.CommentObject.MediaPlayer.Duration)
                                                {
                                                    args.Holder.DurationVoice.Text = Methods.AudioRecorderAndPlayer.GetTimeString(args.CommentObject.MediaPlayer.CurrentPosition.ToString());
                                                }
                                                else
                                                {
                                                    args.Holder.DurationVoice.Text = Methods.AudioRecorderAndPlayer.GetTimeString(args.CommentObject.MediaPlayer.Duration.ToString());

                                                    args.Holder.PlayButton.Tag = "Play";
                                                    args.Holder.PlayButton.SetImageResource(Resource.Drawable.ic_play_dark_arrow);
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Methods.DisplayReportResultTrack(e);
                                            args.Holder.PlayButton.Tag = "Play";
                                        }
                                    });
                                };
                                args.CommentObject.MediaTimer.Start();
                            }
                            catch (Exception e)
                            {
                                Methods.DisplayReportResultTrack(e);
                            }
                        };

                        if (mediaFile.Contains("http"))
                        {
                            args.CommentObject.MediaPlayer.SetDataSource(args.Holder.CommentAdapter.ActivityContext, Uri.Parse(mediaFile));
                            args.CommentObject.MediaPlayer.PrepareAsync();
                        }
                        else
                        {
                            Java.IO.File file2 = new Java.IO.File(mediaFile);
                            var photoUri = FileProvider.GetUriForFile(args.Holder.CommentAdapter.ActivityContext, args.Holder.CommentAdapter.ActivityContext.PackageName + ".fileprovider", file2);

                            args.CommentObject.MediaPlayer.SetDataSource(args.Holder.CommentAdapter.ActivityContext, photoUri);
                            args.CommentObject.MediaPlayer.PrepareAsync();
                        }

                        //args.CommentObject.SoundViewHolder = soundViewHolder;
                        break;
                    }
                    default:
                        switch (args.Holder.PlayButton?.Tag?.ToString())
                        {
                            case "Play":
                            {
                                args.Holder.PlayButton.Tag = "Pause";
                                args.Holder.PlayButton.SetImageResource(AppSettings.SetTabDarkTheme ? Resource.Drawable.ic_media_pause_light : Resource.Drawable.ic_media_pause_dark);
                         
                                args.CommentObject.MediaIsPlaying = true;
                                args.CommentObject.MediaPlayer?.Start();

                                if (args.CommentObject.MediaTimer != null)
                                {
                                    args.CommentObject.MediaTimer.Enabled = true;
                                    args.CommentObject.MediaTimer.Start();
                                }

                                break;
                            }
                            case "Pause":
                            {
                                args.Holder.PlayButton.Tag = "Play";
                                args.Holder.PlayButton.SetImageResource(Resource.Drawable.ic_play_dark_arrow);

                                args.CommentObject.MediaIsPlaying = false;
                                args.CommentObject.MediaPlayer?.Pause();

                                if (args.CommentObject.MediaTimer != null)
                                {
                                    args.CommentObject.MediaTimer.Enabled = false;
                                    args.CommentObject.MediaTimer.Stop();
                                }

                                break;
                            }
                        }

                        break;
                }
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
                if (text == MainContext.GetString(Resource.String.Lbl_CopeText))
                {
                    Methods.CopyToClipboard(MainContext,Methods.FunString.DecodeString(CommentObject.Text));
                }
                else if (text == MainContext.GetString(Resource.String.Lbl_Report))
                {
                    if (!Methods.CheckConnectivity())
                        Toast.MakeText(MainContext, MainContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    else
                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Comment.ReportCommentAsync(CommentObject.Id) });

                    Toast.MakeText(MainContext, MainContext.GetString(Resource.String.Lbl_YourReportPost), ToastLength.Short)?.Show();
                }
                else if (text == MainContext.GetString(Resource.String.Lbl_Edit))
                {
                    EditCommentEvent(CommentObject);
                }
                else if (text == MainContext.GetString(Resource.String.Lbl_Delete))
                {
                    DeleteCommentEvent(CommentObject);
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
                if (p1 == DialogAction.Positive)
                {
                    switch (TypeDialog)
                    {
                        case "DeleteComment":
                            MainContext?.RunOnUiThread(() =>
                            {
                                try
                                {
                                    switch (TypeClass)
                                    {
                                        case "Comment":
                                        {
                                            //TypeClass
                                            var adapterGlobal = CommentActivity.GetInstance()?.MAdapter;
                                            var dataGlobal = adapterGlobal?.CommentList?.FirstOrDefault(a => a.Id == CommentObject?.Id);
                                            if (dataGlobal != null)
                                            {
                                                var index = adapterGlobal.CommentList.IndexOf(dataGlobal);
                                                switch (index)
                                                {
                                                    case > -1:
                                                        adapterGlobal.CommentList.RemoveAt(index);
                                                        adapterGlobal.NotifyItemRemoved(index);
                                                        break;
                                                }
                                            } 

                                            var dataPost = TabbedMainActivity.GetInstance()?.NewsFeedTab?.PostFeedAdapter?.ListDiffer?.Where(a => a.PostData?.PostId == CommentObject?.PostId).ToList();
                                            switch (dataPost?.Count)
                                            {
                                                case > 0:
                                                {
                                                    foreach (var post in dataPost.Where(post => post.TypeView == PostModelType.CommentSection || post.TypeView == PostModelType.AddCommentSection))
                                                    {
                                                        TabbedMainActivity.GetInstance()?.NewsFeedTab?.MainRecyclerView?.RemoveByRowIndex(post);
                                                    }

                                                    break;
                                                }
                                            }

                                            var dataPost2 = WRecyclerView.GetInstance()?.NativeFeedAdapter?.ListDiffer?.Where(a => a.PostData?.PostId == CommentObject?.PostId).ToList();
                                            switch (dataPost2?.Count)
                                            {
                                                case > 0:
                                                {
                                                    foreach (var post in dataPost2.Where(post => post.TypeView == PostModelType.CommentSection || post.TypeView == PostModelType.AddCommentSection))
                                                    { 
                                                        WRecyclerView.GetInstance()?.RemoveByRowIndex(post);
                                                    }

                                                    break;
                                                }
                                            }

                                            PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Comment.DeleteCommentAsync(CommentObject.Id) });
                                            break; 
                                        }
                                        case "Reply":
                                        {
                                            //TypeClass
                                            var adapterGlobal = ReplyCommentActivity.GetInstance()?.MAdapter;
                                            var dataGlobal = adapterGlobal?.ReplyCommentList?.FirstOrDefault(a => a.Id == CommentObject?.Id);
                                            if (dataGlobal != null)
                                            {
                                                var index = adapterGlobal.ReplyCommentList.IndexOf(dataGlobal);
                                                switch (index)
                                                {
                                                    case > -1:
                                                        adapterGlobal.ReplyCommentList.RemoveAt(index);
                                                        adapterGlobal.NotifyItemRemoved(index);
                                                        break;
                                                }
                                            }

                                            PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Comment.DeleteCommentAsync(CommentObject.Id, "delete_reply") });
                                            break;
                                        }
                                    }

                                    Toast.MakeText(MainContext, MainContext.GetText(Resource.String.Lbl_CommentSuccessfullyDeleted), ToastLength.Short)?.Show();
                                }
                                catch (Exception e)
                                {
                                    Methods.DisplayReportResultTrack(e);
                                }
                            });
                            break;
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
                else if (p1 == DialogAction.Negative)
                {
                    p0.Dismiss();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnInput(MaterialDialog p0, ICharSequence p1)
        {
            try
            {
                if (p1.Length() > 0)
                {
                    var strName = p1.ToString();

                    if (!Methods.CheckConnectivity())
                    {
                        Toast.MakeText(MainContext, MainContext.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    }
                    else
                    {
                        switch (TypeClass)
                        {
                            case "Comment":
                            {
                                //TypeClass
                                var adapterGlobal = CommentActivity.GetInstance()?.MAdapter;
                                var dataGlobal = adapterGlobal?.CommentList?.FirstOrDefault(a => a.Id == CommentObject?.Id);
                                if (dataGlobal != null)
                                {
                                    dataGlobal.Text = strName;
                                    var index = adapterGlobal.CommentList.IndexOf(dataGlobal);
                                    switch (index)
                                    {
                                        case > -1:
                                            adapterGlobal.NotifyItemChanged(index);
                                            break;
                                    }
                                }

                                var dataPost = WRecyclerView.GetInstance()?.NativeFeedAdapter?.ListDiffer?.Where(a => a.PostData?.Id == CommentObject.PostId).ToList();
                                switch (dataPost?.Count)
                                {
                                    case > 0:
                                    {
                                        foreach (var post in dataPost)
                                        {
                                            if (post.TypeView != PostModelType.CommentSection) 
                                                continue;

                                            var dataComment = post.PostData.GetPostComments?.FirstOrDefault(a => a.Id == CommentObject?.Id);
                                            if (dataComment != null)
                                            {
                                                dataComment.Text = strName;
                                                var index = post.PostData.GetPostComments.IndexOf(dataComment);
                                                switch (index)
                                                {
                                                    case > -1:
                                                        WRecyclerView.GetInstance()?.NativeFeedAdapter.NotifyItemChanged(index);
                                                        break;
                                                }
                                            }
                                        }

                                        break;
                                    }
                                }

                                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Comment.EditCommentAsync(CommentObject.Id, strName) });
                                break;
                            }
                            case "Reply":
                            {
                                //TypeClass
                                var adapterGlobal = ReplyCommentActivity.GetInstance()?.MAdapter;
                                var dataGlobal = adapterGlobal?.ReplyCommentList?.FirstOrDefault(a => a.Id == CommentObject?.Id);
                                if (dataGlobal != null)
                                {
                                    dataGlobal.Text = strName;
                                    var index = adapterGlobal.ReplyCommentList.IndexOf(dataGlobal);
                                    switch (index)
                                    {
                                        case > -1:
                                            adapterGlobal.NotifyItemChanged(index);
                                            break;
                                    }
                                }

                                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Comment.EditCommentAsync(CommentObject.Id, strName, "edit_reply") });
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

        #endregion MaterialDialog 
    }
}
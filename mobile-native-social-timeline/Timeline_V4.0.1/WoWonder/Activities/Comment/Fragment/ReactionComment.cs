using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using WoWonder.Activities.Comment.Adapters;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.Library.Anjo;
using WoWonderClient.Requests; 

namespace WoWonder.Activities.Comment.Fragment
{
    public class ReactionComment : Java.Lang.Object, PopupWindow.IOnDismissListener
    {
        private readonly Activity MainContext;
        private readonly string TypeClass;
        private PopupWindow PopupWindow;

        //ImagesButton one for every Reaction
        private ImageView MImgButtonOne;
        private ImageView MImgButtonTwo;
        private ImageView MImgButtonThree;
        private ImageView MImgButtonFour;
        private ImageView MImgButtonFive;
        private ImageView MImgButtonSix;

        private TextView ReactionLabel;

        //Integer variable to change react dialog shape Default value is react_dialog_shape
        private readonly int MReactDialogShape = Resource.Xml.react_dialog_shape;

        //Array of six Reaction one for every ImageButton Icon
        private readonly List<Reaction> MReactionPack = XReactions.GetReactions();

        private CommentReplyClickEventArgs PostData;

        public ReactionComment(Activity context , string type)
        {
            MainContext = context;
            TypeClass = type;
        }
         
        /// <summary>
        /// Show Reaction dialog when user long click on react button
        /// </summary>
        public void ClickDialog(CommentReplyClickEventArgs postData)
        {
            try
            {
                PostData = postData;

                switch (UserDetails.SoundControl)
                {
                    case true:
                        Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("appear.mp3");
                        break;
                }

                LayoutInflater layoutInflater = (LayoutInflater)MainContext?.GetSystemService(Context.LayoutInflaterService);
                View popupView = layoutInflater?.Inflate(Resource.Layout.XReactDialogLayout, null);
                popupView?.Measure((int)MeasureSpecMode.Unspecified, (int)MeasureSpecMode.Unspecified);

                PopupWindow = new PopupWindow(popupView, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, true);

                InitializingReactImages(popupView);
                ClickImageButtons();

                PopupWindow.SetBackgroundDrawable(new ColorDrawable());
                PopupWindow.AnimationStyle = Resource.Style.Animation;
                PopupWindow.Focusable = true;
                PopupWindow.ClippingEnabled = true;
                PopupWindow.OutsideTouchable = false;
                PopupWindow.SetOnDismissListener(this);

                int[] location = new int[2];
                PostData.Holder.LikeTextView.GetLocationInWindow(location);

                int OFFSET_X = 0;
                int OFFSET_Y = -400;

                PopupWindow.ShowAtLocation(PostData.Holder.LikeTextView, GravityFlags.NoGravity, location[0] + OFFSET_X, location[1] + OFFSET_Y);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view">View Object to initialize all ImagesButton</param>
        private void InitializingReactImages(View view)
        {
            try
            {
                MImgButtonOne = view.FindViewById<ImageView>(Resource.Id.imgButtonOne);
                MImgButtonTwo = view.FindViewById<ImageView>(Resource.Id.imgButtonTwo);
                MImgButtonThree = view.FindViewById<ImageView>(Resource.Id.imgButtonThree);
                MImgButtonFour = view.FindViewById<ImageView>(Resource.Id.imgButtonFour);
                MImgButtonFive = view.FindViewById<ImageView>(Resource.Id.imgButtonFive);
                MImgButtonSix = view.FindViewById<ImageView>(Resource.Id.imgButtonSix);

                ReactionLabel = view.FindViewById<TextView>(Resource.Id.reactLabel);
                ReactionLabel.Visibility = ViewStates.Invisible;

                MImgButtonOne.Visibility = ViewStates.Gone;
                MImgButtonTwo.Visibility = ViewStates.Gone;
                MImgButtonThree.Visibility = ViewStates.Gone;
                MImgButtonFour.Visibility = ViewStates.Gone;
                MImgButtonFive.Visibility = ViewStates.Gone;
                MImgButtonSix.Visibility = ViewStates.Gone;

                switch (AppSettings.PostButton)
                {
                    case PostButtonSystem.ReactionDefault:
                        Glide.With(MainContext).Load(Resource.Drawable.gif_like).Apply(new RequestOptions().CenterCrop()).Into(MImgButtonOne);
                        Glide.With(MainContext).Load(Resource.Drawable.gif_love).Apply(new RequestOptions().CenterCrop()).Into(MImgButtonTwo);
                        Glide.With(MainContext).Load(Resource.Drawable.gif_haha).Apply(new RequestOptions().CenterCrop()).Into(MImgButtonThree);
                        Glide.With(MainContext).Load(Resource.Drawable.gif_wow).Apply(new RequestOptions().CenterCrop()).Into(MImgButtonFour);
                        Glide.With(MainContext).Load(Resource.Drawable.gif_sad).Apply(new RequestOptions().CenterCrop()).Into(MImgButtonFive);
                        Glide.With(MainContext).Load(Resource.Drawable.gif_angry).Apply(new RequestOptions().CenterCrop()).Into(MImgButtonSix);
                        break;
                    case PostButtonSystem.ReactionSubShine:
                        Glide.With(MainContext).Load(Resource.Drawable.like).Apply(new RequestOptions().FitCenter()).Into(MImgButtonOne);
                        Glide.With(MainContext).Load(Resource.Drawable.love).Apply(new RequestOptions().FitCenter()).Into(MImgButtonTwo);
                        Glide.With(MainContext).Load(Resource.Drawable.haha).Apply(new RequestOptions().FitCenter()).Into(MImgButtonThree);
                        Glide.With(MainContext).Load(Resource.Drawable.wow).Apply(new RequestOptions().FitCenter()).Into(MImgButtonFour);
                        Glide.With(MainContext).Load(Resource.Drawable.sad).Apply(new RequestOptions().FitCenter()).Into(MImgButtonFive);
                        Glide.With(MainContext).Load(Resource.Drawable.angry).Apply(new RequestOptions().FitCenter()).Into(MImgButtonSix);
                        break;
                }

                SetTranslateAnimation(MImgButtonOne);
                SetTranslateAnimation(MImgButtonTwo);
                SetTranslateAnimation(MImgButtonThree);
                SetTranslateAnimation(MImgButtonFour);
                SetTranslateAnimation(MImgButtonFive);
                SetTranslateAnimation(MImgButtonSix);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private async void SetTranslateAnimation(View view)
        {
            try
            {
                var animation = new TranslateAnimation(0, 0, view.Height, 0) { Duration = 400 };
                animation.AnimationEnd += (sender, args) =>
                {
                    try
                    {
                        view.Visibility = ViewStates.Visible;
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                };
                view.StartAnimation(animation);
                await Task.Delay(300);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        /// <summary>
        /// Set onClickListener For every Image Buttons on Reaction Dialog
        /// </summary>
        private void ClickImageButtons()
        {
            try
            {
                ImgButtonSetListener(MImgButtonOne, 0, ReactConstants.Like);
                ImgButtonSetListener(MImgButtonTwo, 1, ReactConstants.Love);
                ImgButtonSetListener(MImgButtonThree, 2, ReactConstants.HaHa);
                ImgButtonSetListener(MImgButtonFour, 3, ReactConstants.Wow);
                ImgButtonSetListener(MImgButtonFive, 4, ReactConstants.Sad);
                ImgButtonSetListener(MImgButtonSix, 5, ReactConstants.Angry);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnDismiss()
        {
            try
            {
                PopupWindow?.Dismiss();
                switch (UserDetails.SoundControl)
                {
                    case true:
                        Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("leave.mp3");
                        break;
                }

            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imgButton">ImageButton view to set onClickListener</param>
        /// <param name="reactIndex">Index of Reaction to take it from ReactionPack</param>
        /// <param name="reactName"></param>
        private void ImgButtonSetListener(ImageView imgButton, int reactIndex , string reactName)
        {
            try
            {
                //if (imgButton != null && !imgButton.HasOnClickListeners)
                //    imgButton.Click += (sender, e) => ImgButtonOnClick(new ReactionsClickEventArgs { ImgButton = imgButton, Position = reactIndex, React = reactName });

                if (imgButton != null && !imgButton.HasOnClickListeners)
                    imgButton.Touch += (sender, e) => ImgButtonOnTouch(new ReactionsTouchEventArgs(e.Handled, e.Event) { ImgButton = imgButton, Position = reactIndex });

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }


        private long Then;
        private int longClickDuration = 500; //for long click to trigger after 0.5 seconds
        private int PositionSelect = -1;

        private void ImgButtonOnTouch(ReactionsTouchEventArgs e)
        {
            try
            {
                switch (e.Event.Action)
                {
                    case MotionEventActions.Down:
                        {
                            Then = Methods.Time.CurrentTimeMillis();
                            //ImgButtonOnLongClick(new ReactionsClickLongClickEventArgs(e.Handled) { ImgButton = e.ImgButton, Position = e.Position });

                            if (ReactionLabel != null)
                            {
                                PositionSelect = e.Position;
                                Reaction data = MReactionPack[e.Position];
                                ReactionLabel.Text = data.GetReactText();
                                ReactionLabel.Visibility = ViewStates.Visible;
                            }

                            switch (UserDetails.SoundControl)
                            {
                                case true:
                                    Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("down.mp3");
                                    break;
                            }
                            break;
                        }

                    case MotionEventActions.Up:
                        if (Methods.Time.CurrentTimeMillis() - Then > longClickDuration)
                        {
                            /* Implement long click behavior here */
                            //ImgButtonOnLongClick(new ReactionsClickLongClickEventArgs(e.Handled) { ImgButton = e.ImgButton, Position = e.Position });

                            if (ReactionLabel != null)
                                ReactionLabel.Visibility = ViewStates.Invisible;
                        }
                        else
                        {
                            /* Implement short click behavior here or do nothing */
                            ImgButtonOnClick(new ReactionsClickEventArgs { ImgButton = e.ImgButton, Position = e.Position });
                        }
                        break;
                    case MotionEventActions.Move when PositionSelect != e.Position:
                        {
                            Then = Methods.Time.CurrentTimeMillis();
                            //ImgButtonOnLongClick(new ReactionsClickLongClickEventArgs(e.Handled) { ImgButton = e.ImgButton, Position = e.Position });

                            if (ReactionLabel != null)
                            {
                                PositionSelect = e.Position;
                                Reaction data = MReactionPack[e.Position];
                                ReactionLabel.Text = data.GetReactText();
                                ReactionLabel.Visibility = ViewStates.Visible;
                            }

                            switch (UserDetails.SoundControl)
                            {
                                case true:
                                    Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("select.mp3");
                                    break;
                            }
                            break;
                        }
                    case MotionEventActions.Cancel:

                        PositionSelect = -1;

                        if (ReactionLabel != null)
                            ReactionLabel.Visibility = ViewStates.Invisible;

                        switch (UserDetails.SoundControl)
                        {
                            case true:
                                Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("cancel.mp3");
                                break;
                        }
                        break;
                    default:
                        return;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
         
        private void ImgButtonOnClick(ReactionsClickEventArgs e)
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(Application.Context, Application.Context.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                    return;
                }

                switch (UserDetails.SoundControl)
                {
                    case true:
                        Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("select.mp3");
                        break;
                }
                 
                PostData.CommentObject.Reaction ??= new WoWonderClient.Classes.Posts.Reaction();

                if (PostData.CommentObject.Reaction.IsReacted != null && !PostData.CommentObject.Reaction.IsReacted.Value)
                {
                    PostData.CommentObject.Reaction.IsReacted = true;
                    PostData.CommentObject.Reaction.Count++;
                }
               
                if (PostData.Holder.CountLike != null && PostData.CommentObject.Reaction.Count > 0)
                {
                    PostData.Holder.CountLikeSection.Visibility = ViewStates.Visible;
                    PostData.Holder.CountLike.Text = Methods.FunString.FormatPriceValue(PostData.CommentObject.Reaction.Count);
                }
                else
                {
                    PostData.Holder.CountLikeSection.Visibility = ViewStates.Gone;
                }
                 
                var data = MReactionPack[e.Position];
                if (data != null)
                {
                    SetReactionPack(PostData.Holder, data.GetReactText());
                    PostData.Holder.LikeTextView.Tag = "Liked";
                }

                string reactionType = TypeClass switch
                {
                    "Reply" => "reaction_reply",
                    _ => "reaction_comment"
                };

                if (e.React == ReactConstants.Like)
                {
                    PostData.Holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_like);
                  
                    PostData.CommentObject.Reaction.Type = "1"; 
                    string react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Name == "Like").Value?.Id ?? "1";
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Comment.ReactionCommentAsync(PostData.CommentObject.Id, react, reactionType) });
                }
                else if (e.React == ReactConstants.Love)
                {
                    PostData.Holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_love);

                    PostData.CommentObject.Reaction.Type = "2";
                    string react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Name == "Love").Value?.Id ?? "2";
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Comment.ReactionCommentAsync(PostData.CommentObject.Id, react, reactionType) });
                }
                else if (e.React == ReactConstants.HaHa)
                {
                    PostData.Holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_haha);

                    PostData.CommentObject.Reaction.Type = "3";
                    string react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Name == "HaHa").Value?.Id ?? "3";
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Comment.ReactionCommentAsync(PostData.CommentObject.Id, react, reactionType) });
                }
                else if (e.React == ReactConstants.Wow)
                {
                    PostData.Holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_wow);

                    PostData.CommentObject.Reaction.Type = "4";
                    string react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Name == "Wow").Value?.Id ?? "4";
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Comment.ReactionCommentAsync(PostData.CommentObject.Id, react, reactionType) });
                }
                else if (e.React == ReactConstants.Sad)
                {
                    PostData.Holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_sad);

                    PostData.CommentObject.Reaction.Type = "5";
                    string react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Name == "Sad").Value?.Id ?? "5";
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Comment.ReactionCommentAsync(PostData.CommentObject.Id, react, reactionType) });
                }
                else if (e.React == ReactConstants.Angry)
                {
                    PostData.Holder.ImageCountLike.SetImageResource(Resource.Drawable.emoji_angry);

                    PostData.CommentObject.Reaction.Type = "6";
                    string react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Name == "Angry").Value?.Id ?? "6";
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Comment.ReactionCommentAsync(PostData.CommentObject.Id, react, reactionType) });
                }

                PopupWindow?.Dismiss(); 
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        } 

        public static void SetReactionPack(CommentAdapterViewHolder holder, string react)
        {
            try
            {
               var mReactionPack = XReactions.GetReactions();

                var data = mReactionPack?.FirstOrDefault(a => a.GetReactText() == react);
                if (data != null)
                {
                    holder.LikeTextView.Text = data.GetReactText();
                    holder.LikeTextView.SetTextColor(Color.ParseColor(data.GetReactTextColor()));
                } 
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        } 
    }
}
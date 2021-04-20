using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Content.Res;
using Bumptech.Glide;
using Bumptech.Glide.Load.Resource.Drawable;
using Bumptech.Glide.Request;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils; 
using WoWonderClient.Requests; 

namespace WoWonder.Library.Anjo
{
    public class ReactButton : TextView, PopupWindow.IOnDismissListener
    {
        //ReactButton custom view object to make easy to change attribute
        private ReactButton MReactButton;

        //Reaction Alert Dialog to show Reaction layout with 6 Reactions
        private PopupWindow PopupWindow;

        //react current state as boolean variable is false in default state and true in all other states
        private bool MCurrentReactState;

        //ImagesButton one for every Reaction
        private ImageView MImgButtonOne;
        private ImageView MImgButtonTwo;
        private ImageView MImgButtonThree;
        private ImageView MImgButtonFour;
        private ImageView MImgButtonFive;
        private ImageView MImgButtonSix;

        private TextView ReactionLabel;

        //Number of Valid Reactions
        private static readonly int ReactionsNumber = 6;

        //Array of ImagesButton to set any action for all
        private readonly ImageView[] MReactImgArray = new ImageView[ReactionsNumber];

        //Reaction Object to save default Reaction
        private Reaction MDefaultReaction = XReactions.GetDefaultReact();

        //Reaction Object to save the current Reaction
        private Reaction MCurrentReaction;

        //Array of six Reaction one for every ImageButton Icon
        private List<Reaction> MReactionPack = XReactions.GetReactions();

        //Integer variable to change react dialog shape Default value is react_dialog_shape
        private int MReactDialogShape = Resource.Xml.react_dialog_shape;

        private GlobalClickEventArgs PostData;
        private string NamePage;
        private readonly Context MainContext;
        private NativePostAdapter NativeFeedAdapter;
        private static int screen_width;

        protected ReactButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ReactButton(Context context) : base(context)
        {
            MainContext = context;
            Init();
            ReactButtonDefaultSetting();
        }

        public ReactButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            MainContext = context;
            Init();
            ReactButtonDefaultSetting();
        }

        public ReactButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            MainContext = context;
            Init();
            ReactButtonDefaultSetting();
        }

        public ReactButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            MainContext = context;
            Init();
            ReactButtonDefaultSetting();
        }


        private void Init()
        {
            try
            {
                MReactButton = this;
                MDefaultReaction = XReactions.GetDefaultReact();
                MCurrentReaction = MDefaultReaction;
                AppCompatDelegate.CompatVectorFromResourcesEnabled = true;

                DisplayMetrics metrics = new DisplayMetrics();
                var display = Context?.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
                display.DefaultDisplay.GetRealMetrics(metrics);
                screen_width = metrics.WidthPixels; 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        /// <summary>
        /// Method with 2 state set first React or back to default state
        /// </summary>
        public void ClickLikeAndDisLike(GlobalClickEventArgs postData, NativePostAdapter nativeFeedAdapter, string namePage = "")
        {
            try
            {
                PostData = postData;
                NamePage = namePage;
                NativeFeedAdapter = nativeFeedAdapter;

                //Code When User Click On Button
                //If State is true , dislike The Button And Return To Default State  

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

                switch (MCurrentReactState)
                {
                    case true:
                        {
                            PostData.NewsFeedClass.Reaction ??= new WoWonderClient.Classes.Posts.Reaction();

                            UpdateReactButtonByReaction(MDefaultReaction);

                            switch (AppSettings.PostButton)
                            {
                                case PostButtonSystem.ReactionDefault:
                                case PostButtonSystem.ReactionSubShine:
                                    {
                                        if (PostData.NewsFeedClass.Reaction != null)
                                        {
                                            switch (PostData.NewsFeedClass.Reaction.Count)
                                            {
                                                case > 0:
                                                    PostData.NewsFeedClass.Reaction.Count--;
                                                    break;
                                                default:
                                                    PostData.NewsFeedClass.Reaction.Count = 0;
                                                    break;
                                            }

                                            PostData.NewsFeedClass.Reaction.Type = "";
                                            PostData.NewsFeedClass.Reaction.IsReacted = false;
                                        }

                                        break;
                                    }
                                default:
                                    {
                                        var x = Convert.ToInt32(PostData.NewsFeedClass.PostLikes);
                                        switch (x)
                                        {
                                            case > 0:
                                                x--;
                                                break;
                                            default:
                                                x = 0;
                                                break;
                                        }

                                        PostData.NewsFeedClass.IsLiked = false;
                                        PostData.NewsFeedClass.PostLikes = Convert.ToString(x, CultureInfo.InvariantCulture);
                                        break;
                                    }
                            }

                            switch (NamePage)
                            {
                                case "ImagePostViewerActivity":
                                case "MultiImagesPostViewerActivity":
                                    {
                                        //var ImgLike = PostData.View?.FindViewById<ImageView>(Resource.Id.image_like1);
                                        var likeCount = PostData.View?.FindViewById<TextView>(Resource.Id.LikeText1);
                                        if (likeCount != null && !likeCount.Text.Contains("K") && !likeCount.Text.Contains("M"))
                                        {
                                            var x = Convert.ToInt32(likeCount.Text);
                                            switch (x)
                                            {
                                                case > 0:
                                                    x--;
                                                    break;
                                                default:
                                                    x = 0;
                                                    break;
                                            }

                                            likeCount.Text = Convert.ToString(x, CultureInfo.InvariantCulture);
                                        }

                                        break;
                                    }
                                default:
                                    {
                                        var dataGlobal = nativeFeedAdapter?.ListDiffer?.Where(a => a.PostData?.Id == PostData.NewsFeedClass.PostId).ToList();
                                        switch (dataGlobal?.Count)
                                        {
                                            case > 0:
                                                {
                                                    foreach (var dataClass in from dataClass in dataGlobal let index = nativeFeedAdapter.ListDiffer.IndexOf(dataClass) where index > -1 select dataClass)
                                                    {
                                                        dataClass.PostData = postData.NewsFeedClass;
                                                        switch (AppSettings.PostButton)
                                                        {
                                                            case PostButtonSystem.ReactionDefault:
                                                            case PostButtonSystem.ReactionSubShine:
                                                                dataClass.PostData.PostLikes = PostData.NewsFeedClass.Reaction.Count.ToString();
                                                                break;
                                                            default:
                                                                dataClass.PostData.PostLikes = PostData.NewsFeedClass.PostLikes;
                                                                break;
                                                        }
                                                        nativeFeedAdapter.NotifyItemChanged(nativeFeedAdapter.ListDiffer.IndexOf(dataClass), "reaction");
                                                    }

                                                    break;
                                                }
                                        }

                                        var likeCount = PostData.View?.FindViewById<ReactButton>(Resource.Id.ReactButton);
                                        //var likeCount = PostData.View?.FindViewById<TextView>(Resource.Id.Likecount);
                                        if (likeCount != null)
                                        {
                                            switch (AppSettings.PostButton)
                                            {
                                                case PostButtonSystem.ReactionDefault:
                                                case PostButtonSystem.ReactionSubShine:
                                                    likeCount.Text = PostData.NewsFeedClass.Reaction.Count.ToString();
                                                    break;
                                                default:
                                                    likeCount.Text = PostData.NewsFeedClass.PostLikes;
                                                    break;
                                            }
                                        }

                                        break;
                                    }
                            }

                            switch (AppSettings.PostButton)
                            {
                                case PostButtonSystem.ReactionDefault:
                                case PostButtonSystem.ReactionSubShine:
                                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.PostActionsAsync(PostData.NewsFeedClass.PostId, "reaction") });
                                    break;
                                default:
                                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.PostActionsAsync(PostData.NewsFeedClass.PostId, "like") });
                                    break;
                            }
                            break;
                        }
                    default:
                        {
                            UpdateReactButtonByReaction(MReactionPack[0]);

                            switch (AppSettings.PostButton)
                            {
                                case PostButtonSystem.ReactionDefault:
                                case PostButtonSystem.ReactionSubShine:
                                    {
                                        PostData.NewsFeedClass.Reaction ??= new WoWonderClient.Classes.Posts.Reaction();

                                        if (PostData.NewsFeedClass.Reaction.IsReacted != null && !PostData.NewsFeedClass.Reaction.IsReacted.Value)
                                        {
                                            PostData.NewsFeedClass.Reaction.Count++;
                                            PostData.NewsFeedClass.Reaction.Type = "1";
                                            PostData.NewsFeedClass.Reaction.IsReacted = true;
                                        }

                                        break;
                                    }
                                default:
                                    {
                                        var x = Convert.ToInt32(PostData.NewsFeedClass.PostLikes);
                                        x++;

                                        PostData.NewsFeedClass.IsLiked = true;
                                        PostData.NewsFeedClass.PostLikes = Convert.ToString(x, CultureInfo.InvariantCulture);
                                        break;
                                    }
                            }

                            switch (NamePage)
                            {
                                case "ImagePostViewerActivity":
                                case "MultiImagesPostViewerActivity":
                                    {
                                        var likeCount = PostData.View?.FindViewById<TextView>(Resource.Id.LikeText1);

                                        if (likeCount != null && !likeCount.Text.Contains("K") && !likeCount.Text.Contains("M"))
                                        {
                                            var x = Convert.ToInt32(likeCount.Text);
                                            x++;

                                            likeCount.Text = Convert.ToString(x, CultureInfo.InvariantCulture);
                                        }

                                        break;
                                    }
                                default:
                                    {
                                        var dataGlobal = nativeFeedAdapter?.ListDiffer?.Where(a => a.PostData?.Id == PostData.NewsFeedClass.PostId).ToList();
                                        switch (dataGlobal?.Count)
                                        {
                                            case > 0:
                                                {
                                                    foreach (var dataClass in from dataClass in dataGlobal let index = nativeFeedAdapter.ListDiffer.IndexOf(dataClass) where index > -1 select dataClass)
                                                    {
                                                        dataClass.PostData = postData.NewsFeedClass;
                                                        switch (AppSettings.PostButton)
                                                        {
                                                            case PostButtonSystem.ReactionDefault:
                                                            case PostButtonSystem.ReactionSubShine:
                                                                dataClass.PostData.PostLikes = PostData.NewsFeedClass.Reaction.Count.ToString();
                                                                break;
                                                            default:
                                                                dataClass.PostData.PostLikes = PostData.NewsFeedClass.PostLikes;
                                                                break;
                                                        }
                                                        nativeFeedAdapter.NotifyItemChanged(nativeFeedAdapter.ListDiffer.IndexOf(dataClass), "reaction");
                                                    }

                                                    break;
                                                }
                                        }

                                        var likeCount = PostData.View?.FindViewById<ReactButton>(Resource.Id.ReactButton);
                                        if (likeCount != null)
                                        {
                                            switch (AppSettings.PostButton)
                                            {
                                                case PostButtonSystem.ReactionDefault:
                                                case PostButtonSystem.ReactionSubShine:
                                                    likeCount.Text = PostData.NewsFeedClass.Reaction.Count.ToString();
                                                    break;
                                                default:
                                                    likeCount.Text = PostData.NewsFeedClass.PostLikes;
                                                    break;
                                            }
                                        }

                                        break;
                                    }
                            }

                            switch (AppSettings.PostButton)
                            {
                                case PostButtonSystem.ReactionDefault:
                                case PostButtonSystem.ReactionSubShine:
                                    {
                                        string like = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Name == "Like").Value?.Id ?? "1";
                                        PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.PostActionsAsync(PostData.NewsFeedClass.PostId, "reaction", like) });
                                        break;
                                    }
                                default:
                                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.PostActionsAsync(PostData.NewsFeedClass.PostId, "like") });
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

        /// <summary>
        /// Show Reaction dialog when user long click on react button
        /// </summary>
        public void LongClickDialog(GlobalClickEventArgs postData, NativePostAdapter nativeFeedAdapter, string namePage = "")
        {
            try
            {
                PostData = postData;
                NamePage = namePage;
                NativeFeedAdapter = nativeFeedAdapter;

                switch (UserDetails.SoundControl)
                {
                    case true:
                        Methods.AudioRecorderAndPlayer.PlayAudioFromAsset("appear.mp3");
                        break;
                }
                 
                LayoutInflater layoutInflater = (LayoutInflater)Context?.GetSystemService(Context.LayoutInflaterService);
                View popupView = layoutInflater?.Inflate(Resource.Layout.XReactDialogLayout, null);
                popupView?.Measure((int)MeasureSpecMode.Unspecified, (int)MeasureSpecMode.Unspecified);

                PopupWindow = new PopupWindow(popupView, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, true);

                InitializingReactImages(popupView);
                SetReactionsArray();
                ResetReactionsIcons();
                ClickImageButtons();

                PopupWindow.SetBackgroundDrawable(new ColorDrawable());
                PopupWindow.AnimationStyle = Resource.Style.Animation;
                PopupWindow.Focusable = true;
                PopupWindow.ClippingEnabled = true;
                PopupWindow.OutsideTouchable = false;
                PopupWindow.SetOnDismissListener(this);
                  
                int[] location = new int[2]; 
                GetLocationInWindow(location);
                 
                int OFFSET_X = 0;
                int OFFSET_Y = -400;

                PopupWindow.ShowAtLocation(this, GravityFlags.NoGravity, location[0] + OFFSET_X, location[1] + OFFSET_Y); 
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
         
        public void SetReactionPack(string type)
        {
            try
            {
                if (type == ReactConstants.Like)
                {
                    UpdateReactButtonByReaction(MReactionPack[0]);
                }
                else if (type == ReactConstants.Love)
                {
                    UpdateReactButtonByReaction(MReactionPack[1]);
                }
                else if (type == ReactConstants.HaHa)
                {
                    UpdateReactButtonByReaction(MReactionPack[2]);
                }
                else if (type == ReactConstants.Wow)
                {
                    UpdateReactButtonByReaction(MReactionPack[3]);
                }
                else if (type == ReactConstants.Sad)
                {
                    UpdateReactButtonByReaction(MReactionPack[4]);
                }
                else if (type == ReactConstants.Angry)
                {
                    UpdateReactButtonByReaction(MReactionPack[5]);
                }
                else
                {
                    UpdateReactButtonByReaction(XReactions.GetDefaultReact());
                }
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
                        Glide.With(Context).Load(Resource.Drawable.gif_like).Apply(new RequestOptions().CenterCrop()).Into(MImgButtonOne);
                        Glide.With(Context).Load(Resource.Drawable.gif_love).Apply(new RequestOptions().CenterCrop()).Into(MImgButtonTwo);
                        Glide.With(Context).Load(Resource.Drawable.gif_haha).Apply(new RequestOptions().CenterCrop()).Into(MImgButtonThree);
                        Glide.With(Context).Load(Resource.Drawable.gif_wow).Apply(new RequestOptions().CenterCrop()).Into(MImgButtonFour);
                        Glide.With(Context).Load(Resource.Drawable.gif_sad).Apply(new RequestOptions().CenterCrop()).Into(MImgButtonFive);
                        Glide.With(Context).Load(Resource.Drawable.gif_angry).Apply(new RequestOptions().CenterCrop()).Into(MImgButtonSix);
                        break;
                    case PostButtonSystem.ReactionSubShine:
                        Glide.With(Context).Load(Resource.Drawable.like).Apply(new RequestOptions().FitCenter()).Into(MImgButtonOne);
                        Glide.With(Context).Load(Resource.Drawable.love).Apply(new RequestOptions().FitCenter()).Into(MImgButtonTwo);
                        Glide.With(Context).Load(Resource.Drawable.haha).Apply(new RequestOptions().FitCenter()).Into(MImgButtonThree);
                        Glide.With(Context).Load(Resource.Drawable.wow).Apply(new RequestOptions().FitCenter()).Into(MImgButtonFour);
                        Glide.With(Context).Load(Resource.Drawable.sad).Apply(new RequestOptions().FitCenter()).Into(MImgButtonFive);
                        Glide.With(Context).Load(Resource.Drawable.angry).Apply(new RequestOptions().FitCenter()).Into(MImgButtonSix);
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
        /// Put all ImagesButton on Array
        /// </summary>
        private void SetReactionsArray()
        {
            try
            {
                MReactImgArray[0] = MImgButtonOne;
                MReactImgArray[1] = MImgButtonTwo;
                MReactImgArray[2] = MImgButtonThree;
                MReactImgArray[3] = MImgButtonFour;
                MReactImgArray[4] = MImgButtonFive;
                MReactImgArray[5] = MImgButtonSix;
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
                ImgButtonSetListener(MImgButtonOne, 0);
                ImgButtonSetListener(MImgButtonTwo, 1);
                ImgButtonSetListener(MImgButtonThree, 2);
                ImgButtonSetListener(MImgButtonFour, 3);
                ImgButtonSetListener(MImgButtonFive, 4);
                ImgButtonSetListener(MImgButtonSix, 5);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imgButton">ImageButton view to set onClickListener</param>
        /// <param name="reactIndex">Index of Reaction to take it from ReactionPack</param>
        private void ImgButtonSetListener(ImageView imgButton, int reactIndex)
        {
            try
            {
                //if (imgButton != null && !imgButton.HasOnClickListeners)
                //{
                //    imgButton.Click += (sender, e) => ImgButtonOnClick(new ReactionsClickEventArgs { ImgButton = imgButton, Position = reactIndex });
                //}

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
        
        //private void ImgButtonOnLongClick(ReactionsClickLongClickEventArgs e)
        //{
        //    try
        //    {
        //        float scale = screen_width / e.ImgButton.Width;
        //        if (e.ImgButton.ScaleX == 1)
        //        {
        //            e.ImgButton.ScaleY = scale;
        //            e.ImgButton.ScaleX = scale;
        //        }
        //        else
        //        {
        //            e.ImgButton.ScaleY = 1;
        //            e.ImgButton.ScaleX = 1;
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception); 
        //    }
        //}

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

                Reaction data = MReactionPack[e.Position];
                UpdateReactButtonByReaction(data);
                PopupWindow?.Dismiss();

                PostData.NewsFeedClass.Reaction ??= new WoWonderClient.Classes.Posts.Reaction();

                if (data.GetReactText() == ReactConstants.Like)
                {
                    PostData.NewsFeedClass.Reaction.Type = "1";
                    string react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Name == "Like").Value?.Id ?? "1";
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.PostActionsAsync(PostData.NewsFeedClass.PostId, "reaction", react) });
                }
                else if (data.GetReactText() == ReactConstants.Love)
                {
                    PostData.NewsFeedClass.Reaction.Type = "2";
                    string react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Name == "Love").Value?.Id ?? "2";
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.PostActionsAsync(PostData.NewsFeedClass.PostId, "reaction", react) });
                }
                else if (data.GetReactText() == ReactConstants.HaHa)
                {
                    PostData.NewsFeedClass.Reaction.Type = "3";
                    string react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Name == "HaHa").Value?.Id ?? "3";
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.PostActionsAsync(PostData.NewsFeedClass.PostId, "reaction", react) });
                }
                else if (data.GetReactText() == ReactConstants.Wow)
                {
                    PostData.NewsFeedClass.Reaction.Type = "4";
                    string react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Name == "Wow").Value?.Id ?? "4";
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.PostActionsAsync(PostData.NewsFeedClass.PostId, "reaction", react) });
                }
                else if (data.GetReactText() == ReactConstants.Sad)
                {
                    PostData.NewsFeedClass.Reaction.Type = "5";
                    string react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Name == "Sad").Value?.Id ?? "5";
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.PostActionsAsync(PostData.NewsFeedClass.PostId, "reaction", react) });
                }
                else if (data.GetReactText() == ReactConstants.Angry)
                {
                    PostData.NewsFeedClass.Reaction.Type = "6";
                    string react = ListUtils.SettingsSiteList?.PostReactionsTypes?.FirstOrDefault(a => a.Value?.Name == "Angry").Value?.Id ?? "6";
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Posts.PostActionsAsync(PostData.NewsFeedClass.PostId, "reaction", react) });
                }

                if (PostData.NewsFeedClass.Reaction.IsReacted != null && !PostData.NewsFeedClass.Reaction.IsReacted.Value)
                {
                    PostData.NewsFeedClass.Reaction.IsReacted = true;
                    PostData.NewsFeedClass.Reaction.Count++; 
                } 

                switch (NamePage)
                {
                    case "ImagePostViewerActivity":
                    case "MultiImagesPostViewerActivity":
                    {
                        var likeCount = PostData.View?.FindViewById<TextView>(Resource.Id.LikeText1);

                        if (likeCount != null && !likeCount.Text.Contains("K") && !likeCount.Text.Contains("M"))
                        {
                            var x = Convert.ToInt32(likeCount.Text);
                            x++;

                            likeCount.Text = Convert.ToString(x, CultureInfo.InvariantCulture);
                        }

                        break;
                    }
                    default:
                    {
                        var dataGlobal = NativeFeedAdapter?.ListDiffer?.Where(a => a.PostData?.Id == PostData.NewsFeedClass.PostId).ToList();
                        switch (dataGlobal?.Count)
                        {
                            case > 0:
                            {
                                foreach (var dataClass in from dataClass in dataGlobal let index = NativeFeedAdapter.ListDiffer.IndexOf(dataClass) where index > -1 select dataClass)
                                {
                                    dataClass.PostData = PostData.NewsFeedClass;
                                    switch (AppSettings.PostButton)
                                    {
                                        case PostButtonSystem.ReactionDefault:
                                        case PostButtonSystem.ReactionSubShine:
                                            dataClass.PostData.PostLikes = PostData.NewsFeedClass.Reaction.Count.ToString();
                                            break;
                                        default:
                                            dataClass.PostData.PostLikes = PostData.NewsFeedClass.PostLikes;
                                            break;
                                    }
                                    NativeFeedAdapter.NotifyItemChanged(NativeFeedAdapter.ListDiffer.IndexOf(dataClass), "reaction");
                                }

                                break;
                            }
                        }

                        var likeCount = PostData.View?.FindViewById<ReactButton>(Resource.Id.ReactButton);
                        if (likeCount != null)
                        {
                            likeCount.Text = PostData.NewsFeedClass.Reaction.Count.ToString();
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

        /// <summary>
        /// Update All Reaction ImageButton one by one from Reactions array
        /// </summary>
        private void ResetReactionsIcons()
        {
            for (int index = 0; index < ReactionsNumber; index++)
            {
                MReactImgArray[index].SetImageResource(MReactionPack[index].GetReactIconId());
            }
        }

        /// <summary>
        /// Simple Method to set default settings for ReactButton Constructors
        /// - Default Text Is Like
        /// - set onClick And onLongClick
        /// - set Default image is Dark Like
        /// </summary>
        private void ReactButtonDefaultSetting()
        {
            try
            {
                MReactButton.Text = MDefaultReaction.GetReactText();
                //MReactButton.SetOnClickListener(this);
                //MReactButton.SetOnLongClickListener(this);

                Drawable icon =  AppCompatResources.GetDrawable(Context, MDefaultReaction.GetReactIconId());
                if (MReactButton.Text == ReactConstants.Like)
                {
                    icon = AppSettings.PostButton switch
                    {
                        PostButtonSystem.ReactionDefault => AppCompatResources.GetDrawable(Context,
                            MDefaultReaction.GetReactType() == ReactConstants.Default
                                ? Resource.Drawable.icon_post_like_vector
                                : Resource.Drawable.emoji_like) //ic_action_like
                        ,
                        PostButtonSystem.ReactionSubShine => AppCompatResources.GetDrawable(Context,
                            MDefaultReaction.GetReactType() == ReactConstants.Default
                                ? Resource.Drawable.icon_post_like_vector
                                : Resource.Drawable.like) //ic_action_like
                        ,
                        _ => icon
                    };

                    if (MDefaultReaction.GetReactType() == ReactConstants.Default)
                    {
                        //icon?.SetTint(Color.ParseColor(AppSettings.SetTabDarkTheme ? "#ffffff" : "#E02020"));
                        icon?.SetTint(Color.ParseColor(AppSettings.SetTabDarkTheme ? "#ffffff" : "#888888"));
                    }

                    switch (NamePage)
                    {
                        case "ImagePostViewerActivity":
                        case "MultiImagesPostViewerActivity":
                        {
                            var imgLike = PostData.View?.FindViewById<ImageView>(Resource.Id.image_like1);
                            imgLike?.SetImageResource(MDefaultReaction.GetReactType() == ReactConstants.Default ? Resource.Drawable.icon_post_like_vector : Resource.Drawable.emoji_like);
                            //imgLike?.SetImageResource(MDefaultReaction.GetReactType() == ReactConstants.Default ? Resource.Drawable.icon_post_like_vector : Resource.Drawable.emoji_like);
                            break;
                        }
                    }
                }
                else if (MReactButton.Text == ReactConstants.Love)
                {
                    icon = AppSettings.PostButton switch
                    {
                        PostButtonSystem.ReactionDefault => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.emoji_love),
                        PostButtonSystem.ReactionSubShine => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.love),
                        _ => icon
                    };

                    switch (NamePage)
                    {
                        case "ImagePostViewerActivity":
                        case "MultiImagesPostViewerActivity":
                        {
                            var imgLike = PostData.View?.FindViewById<ImageView>(Resource.Id.image_like1);
                            imgLike?.SetImageResource(Resource.Drawable.emoji_love);
                            break;
                        }
                    }
                }
                else if (MReactButton.Text == ReactConstants.HaHa)
                {
                    icon = AppSettings.PostButton switch
                    {
                        PostButtonSystem.ReactionDefault => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.emoji_haha),
                        PostButtonSystem.ReactionSubShine => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.haha),
                        _ => icon
                    };

                    switch (NamePage)
                    {
                        case "ImagePostViewerActivity":
                        case "MultiImagesPostViewerActivity":
                        {
                            var imgLike = PostData.View?.FindViewById<ImageView>(Resource.Id.image_like1);
                            imgLike?.SetImageResource(Resource.Drawable.emoji_haha);
                            break;
                        }
                    }
                }
                else if (MReactButton.Text == ReactConstants.Wow)
                {
                    icon = AppSettings.PostButton switch
                    {
                        PostButtonSystem.ReactionDefault => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.emoji_wow),
                        PostButtonSystem.ReactionSubShine => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.wow),
                        _ => icon
                    };

                    switch (NamePage)
                    {
                        case "ImagePostViewerActivity":
                        case "MultiImagesPostViewerActivity":
                        {
                            var imgLike = PostData.View?.FindViewById<ImageView>(Resource.Id.image_like1);
                            imgLike?.SetImageResource(Resource.Drawable.emoji_wow);
                            break;
                        }
                    }
                }
                else if (MReactButton.Text == ReactConstants.Sad)
                {
                    icon = AppSettings.PostButton switch
                    {
                        PostButtonSystem.ReactionDefault => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.emoji_sad),
                        PostButtonSystem.ReactionSubShine => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.sad),
                        _ => icon
                    };

                    switch (NamePage)
                    {
                        case "ImagePostViewerActivity":
                        case "MultiImagesPostViewerActivity":
                        {
                            var imgLike = PostData.View?.FindViewById<ImageView>(Resource.Id.image_like1);
                            imgLike?.SetImageResource(Resource.Drawable.emoji_sad);
                            break;
                        }
                    }
                }
                else if (MReactButton.Text == ReactConstants.Angry)
                {
                    icon = AppSettings.PostButton switch
                    {
                        PostButtonSystem.ReactionDefault => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.emoji_angry),
                        PostButtonSystem.ReactionSubShine => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.angry),
                        _ => icon
                    };

                    switch (NamePage)
                    {
                        case "ImagePostViewerActivity":
                        case "MultiImagesPostViewerActivity":
                        {
                            var imgLike = PostData.View?.FindViewById<ImageView>(Resource.Id.image_like1);
                            imgLike?.SetImageResource(Resource.Drawable.emoji_angry);
                            break;
                        }
                    }
                }
                else
                {
                    icon = AppCompatResources.GetDrawable(Context, MDefaultReaction.GetReactIconId());

                    switch (NamePage)
                    {
                        case "ImagePostViewerActivity":
                        case "MultiImagesPostViewerActivity":
                        {
                            var imgLike = PostData.View?.FindViewById<ImageView>(Resource.Id.image_like1);
                            //imgLike?.SetImageResource(Resource.Drawable.icon_post_like_vector);
                            imgLike?.SetImageResource(Resource.Drawable.icon_post_like_vector);
                            break;
                        }
                    }
                }

                // Drawable icon = Build.VERSION.SdkInt < BuildVersionCodes.Lollipop ? VectorDrawableCompat.Create(Context.Resources, MDefaultReaction.GetReactIconId(), Context.Theme) : AppCompatResources.GetDrawable(Context, MDefaultReaction.GetReactIconId());

                MReactButton.CompoundDrawablePadding = 20;

                switch (AppSettings.FlowDirectionRightToLeft)
                {
                    case true:
                        MReactButton.SetCompoundDrawablesWithIntrinsicBounds(null, null, icon, null);
                        break;
                    default:
                        MReactButton.SetCompoundDrawablesWithIntrinsicBounds(icon, null, null, null);
                        break;
                }

                //SetImageReaction(icon);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shapeId">Get xml Shape for react dialog layout</param>
        public void SetReactionDialogShape(int shapeId)
        {
            //Set Shape for react dialog layout
            MReactDialogShape = shapeId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="react">Reaction to update UI by take attribute from it</param>
        private void UpdateReactButtonByReaction(Reaction react)
        {
            try
            {
                MCurrentReaction = react;
                MReactButton.Text = react.GetReactText();
                MReactButton.SetTextColor(Color.ParseColor(react.GetReactTextColor()));

                //Drawable icon = Build.VERSION.SdkInt < BuildVersionCodes.Lollipop ? VectorDrawableCompat.Create(Context.Resources, react.GetReactIconId(), Context.Theme) : AppCompatResources.GetDrawable(Context,react.GetReactIconId());

                Drawable icon = AppCompatResources.GetDrawable(Context, react.GetReactIconId());
                if (MReactButton.Text == ReactConstants.Like)
                {
                    icon = AppSettings.PostButton switch
                    {
                        PostButtonSystem.ReactionDefault =>
                            //icon = AppCompatResources.GetDrawable(Context, react.GetReactType() == ReactConstants.Default ? Resource.Drawable.icon_post_like_vector : Resource.Drawable.emoji_like);  //ic_action_like
                            AppCompatResources.GetDrawable(Context,
                                react.GetReactType() == ReactConstants.Default
                                    ? Resource.Drawable.icon_post_like_vector
                                    : Resource.Drawable.emoji_like) //ic_action_like
                        ,
                        PostButtonSystem.ReactionSubShine => AppCompatResources.GetDrawable(Context,
                            react.GetReactType() == ReactConstants.Default
                                ? Resource.Drawable.icon_post_like_vector
                                : Resource.Drawable.like) //ic_action_like
                        ,
                        _ => icon
                    };

                    if (react.GetReactType() == ReactConstants.Default)
                    {
                        //icon?.SetTint(Color.ParseColor(AppSettings.SetTabDarkTheme ? "#ffffff" : "#E02020"));
                        icon?.SetTint(Color.ParseColor(AppSettings.SetTabDarkTheme ? "#ffffff" : "#888888"));
                    }

                    switch (NamePage)
                    {
                        case "ImagePostViewerActivity":
                        case "MultiImagesPostViewerActivity":
                        {
                            var imgLike = PostData.View?.FindViewById<ImageView>(Resource.Id.image_like1);
                            imgLike?.SetImageResource(Resource.Drawable.emoji_like);
                            break;
                        }
                    }
                }
                else if (MReactButton.Text == ReactConstants.Love)
                {
                    icon = AppSettings.PostButton switch
                    {
                        PostButtonSystem.ReactionDefault => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.emoji_love),
                        PostButtonSystem.ReactionSubShine => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.love),
                        _ => icon
                    };

                    switch (NamePage)
                    {
                        case "ImagePostViewerActivity":
                        case "MultiImagesPostViewerActivity":
                        {
                            var imgLike = PostData.View?.FindViewById<ImageView>(Resource.Id.image_like1);
                            imgLike?.SetImageResource(Resource.Drawable.emoji_love);
                            break;
                        }
                    }
                }
                else if (MReactButton.Text == ReactConstants.HaHa)
                {
                    icon = AppSettings.PostButton switch
                    {
                        PostButtonSystem.ReactionDefault => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.emoji_haha),
                        PostButtonSystem.ReactionSubShine => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.haha),
                        _ => icon
                    };

                    switch (NamePage)
                    {
                        case "ImagePostViewerActivity":
                        case "MultiImagesPostViewerActivity":
                        {
                            var imgLike = PostData.View?.FindViewById<ImageView>(Resource.Id.image_like1);
                            imgLike?.SetImageResource(Resource.Drawable.emoji_haha);
                            break;
                        }
                    }
                }
                else if (MReactButton.Text == ReactConstants.Wow)
                {
                    icon = AppSettings.PostButton switch
                    {
                        PostButtonSystem.ReactionDefault => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.emoji_wow),
                        PostButtonSystem.ReactionSubShine => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.wow),
                        _ => icon
                    };

                    switch (NamePage)
                    {
                        case "ImagePostViewerActivity":
                        case "MultiImagesPostViewerActivity":
                        {
                            var imgLike = PostData.View?.FindViewById<ImageView>(Resource.Id.image_like1);
                            imgLike?.SetImageResource(Resource.Drawable.emoji_wow);
                            break;
                        }
                    }
                }
                else if (MReactButton.Text == ReactConstants.Sad)
                {
                    icon = AppSettings.PostButton switch
                    {
                        PostButtonSystem.ReactionDefault => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.emoji_sad),
                        PostButtonSystem.ReactionSubShine => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.sad),
                        _ => icon
                    };

                    switch (NamePage)
                    {
                        case "ImagePostViewerActivity":
                        case "MultiImagesPostViewerActivity":
                        {
                            var imgLike = PostData.View?.FindViewById<ImageView>(Resource.Id.image_like1);
                            imgLike?.SetImageResource(Resource.Drawable.emoji_sad);
                            break;
                        }
                    }
                }
                else if (MReactButton.Text == ReactConstants.Angry)
                {
                    icon = AppSettings.PostButton switch
                    {
                        PostButtonSystem.ReactionDefault => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.emoji_angry),
                        PostButtonSystem.ReactionSubShine => AppCompatResources.GetDrawable(Context,
                            Resource.Drawable.angry),
                        _ => icon
                    };

                    switch (NamePage)
                    {
                        case "ImagePostViewerActivity":
                        case "MultiImagesPostViewerActivity":
                        {
                            var imgLike = PostData.View?.FindViewById<ImageView>(Resource.Id.image_like1);
                            imgLike?.SetImageResource(Resource.Drawable.emoji_angry);
                            break;
                        }
                    }
                }
                else
                {
                    icon = AppCompatResources.GetDrawable(Context, react.GetReactIconId());

                    switch (NamePage)
                    {
                        case "ImagePostViewerActivity":
                        case "MultiImagesPostViewerActivity":
                        {
                            var imgLike = PostData.View?.FindViewById<ImageView>(Resource.Id.image_like1);
                            imgLike?.SetImageResource(Resource.Drawable.icon_post_like_vector);
                            //imgLike?.SetImageResource(Resource.Drawable.icon_post_like_vector);
                            break;
                        }
                    }
                }

                MReactButton.CompoundDrawablePadding = 20;

                switch (AppSettings.FlowDirectionRightToLeft)
                {
                    case true:
                        MReactButton.SetCompoundDrawablesWithIntrinsicBounds(null, null, icon, null);
                        break;
                    default:
                        MReactButton.SetCompoundDrawablesWithIntrinsicBounds(icon, null, null, null);
                        break;
                }

                //SetImageReaction(icon);


                MCurrentReactState = !react.GetReactType().Equals(MDefaultReaction.GetReactType());
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reactions">Array of six Reactions to update default six Reactions</param>
        public void SetReactions(List<Reaction> reactions)
        {
            //Assert that Reactions number is six
            if (reactions.Count != ReactionsNumber)
                return;

            //Update array of library default reactions
            MReactionPack = reactions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reaction">set This Reaction as current Reaction</param>
        public void SetCurrentReaction(Reaction reaction)
        {
            UpdateReactButtonByReaction(reaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The Current reaction Object</returns>
        public Reaction GetCurrentReaction()
        {
            return MCurrentReaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reaction">Update library default Reaction by other Reaction</param>
        public void SetDefaultReaction(Reaction reaction)
        {
            MDefaultReaction = reaction;
            UpdateReactButtonByReaction(MDefaultReaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The current default Reaction object</returns>
        public Reaction GetDefaultReaction()
        {
            return MDefaultReaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if current reaction type is default</returns>
        public bool IsDefaultReaction()
        {
            return MCurrentReaction.Equals(MDefaultReaction);
        }

    }
}
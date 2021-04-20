using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Media;
using Android.OS;

using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using AT.Markushi.UI;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Developer.SEmojis.Actions;
using Developer.SEmojis.Helper;
using Java.Lang; 
using Newtonsoft.Json;
using WoWonder.Activities.AddPost.Service;
using WoWonder.Activities.Base;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonder.Library.Anjo.StoriesProgressView;
using Exception = System.Exception;
using File = Java.IO.File;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.Story
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class AddStoryActivity : BaseActivity
    {
        #region Variables Basic

        private ImageView StoryImageView;
        private VideoView StoryVideoView;
        private ImageView EmojisView;
        private CircleButton PlayIconVideo, AddStoryButton;
        private EmojiconEditText EmojisIconEditText;
        private RelativeLayout RootView;
        private string PathStory = "", Type = "", Thumbnail = UserDetails.Avatar;
        private StoriesProgressView StoriesProgress;
        private long Duration;

        #endregion

        #region General

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                Window?.SetSoftInputMode(SoftInput.AdjustResize);
                SetTheme(AppSettings.SetTabDarkTheme ? Resource.Style.MyTheme_Dark_Base : Resource.Style.MyTheme_Base);

                base.OnCreate(savedInstanceState);

                // Create your application here
                SetContentView(Resource.Layout.AddStory_layout);
                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();

                if (Intent != null)
                {
                    Thumbnail = Intent.GetStringExtra("Thumbnail") ?? UserDetails.Avatar;

                    var dataUri = Intent.GetStringExtra("Uri") ?? "Data not available";
                    if (dataUri != "Data not available" && !string.IsNullOrEmpty(dataUri))
                        PathStory = dataUri; // Uri file 
                    var dataType = Intent.GetStringExtra("Type") ?? "Data not available";
                    if (dataType != "Data not available" && !string.IsNullOrEmpty(dataType))
                        Type = dataType; // Type file  
                }

                switch (Type)
                {
                    case "image":
                        SetImageStory(PathStory);
                        break;
                    default:
                        SetVideoStory(PathStory);
                        break;
                }

            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        protected override void OnResume()
        {
            try
            {
                base.OnResume();
                AddOrRemoveEvent(true);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        protected override void OnPause()
        {
            try
            {
                base.OnPause();
                AddOrRemoveEvent(false);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override void OnTrimMemory(TrimMemory level)
        {
            try
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                base.OnTrimMemory(level);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public override void OnLowMemory()
        {
            try
            {
                GC.Collect(GC.MaxGeneration);
                base.OnLowMemory();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        protected override void OnDestroy()
        {
            try
            {
                // Very important !
                StoriesProgress.Destroy();

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                DestroyBasic();

                base.OnDestroy();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        } 
        #endregion

        #region Menu

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        #endregion

        #region Functions

        private void InitComponent()
        {
            try
            {
                StoryImageView = FindViewById<ImageView>(Resource.Id.imagstoryDisplay);
                StoryVideoView = FindViewById<VideoView>(Resource.Id.VideoView);
                PlayIconVideo = FindViewById<CircleButton>(Resource.Id.Videoicon_button);
                EmojisView = FindViewById<ImageView>(Resource.Id.emojiicon);
                EmojisIconEditText = FindViewById<EmojiconEditText>(Resource.Id.EmojiconEditText5);
                AddStoryButton = FindViewById<CircleButton>(Resource.Id.sendButton);
                RootView = FindViewById<RelativeLayout>(Resource.Id.storyDisplay);

                StoriesProgress = FindViewById<StoriesProgressView>(Resource.Id.stories);
                if (StoriesProgress != null) StoriesProgress.Visibility = ViewStates.Gone;

                Methods.SetColorEditText(EmojisIconEditText, AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

                var emojisIcon = new EmojIconActions(this, RootView, EmojisIconEditText, EmojisView);
                emojisIcon.ShowEmojIcon();
                emojisIcon.SetIconsIds(Resource.Drawable.ic_action_keyboard, Resource.Drawable.ic_action_sentiment_satisfied_alt);

                PlayIconVideo.Visibility = ViewStates.Gone;
                PlayIconVideo.Tag = "Play";
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void InitToolbar()
        {
            try
            {
                var toolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
                if (toolBar != null)
                {
                    toolBar.Title = GetString(Resource.String.Lbl_Addnewstory);
                    toolBar.SetTitleTextColor(Color.ParseColor(AppSettings.MainColor));
                    SetSupportActionBar(toolBar);
                    SupportActionBar.SetDisplayShowCustomEnabled(true);
                    SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                    SupportActionBar.SetHomeButtonEnabled(true);
                    SupportActionBar.SetDisplayShowHomeEnabled(true);
                    SupportActionBar.SetHomeAsUpIndicator(AppCompatResources.GetDrawable(this, AppSettings.FlowDirectionRightToLeft ? Resource.Drawable.ic_action_right_arrow_color : Resource.Drawable.ic_action_left_arrow_color));


                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void AddOrRemoveEvent(bool addEvent)
        {
            try
            {
                switch (addEvent)
                {
                    // true +=  // false -=
                    case true:
                        AddStoryButton.Click += AddStoryButtonOnClick;
                        StoryVideoView.Completion += StoryVideoViewOnCompletion;
                        PlayIconVideo.Click += PlayIconVideoOnClick;
                        break;
                    default:
                        AddStoryButton.Click -= AddStoryButtonOnClick;
                        StoryVideoView.Completion -= StoryVideoViewOnCompletion;
                        PlayIconVideo.Click -= PlayIconVideoOnClick;
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void SetImageStory(string url)
        {
            try
            {
                StoryImageView.Visibility = StoryImageView.Visibility switch
                {
                    ViewStates.Gone => ViewStates.Visible,
                    _ => StoryImageView.Visibility
                };

                var file = Uri.FromFile(new File(url));

                Glide.With(this).Load(file?.Path).Apply(new RequestOptions()).Into(StoryImageView);

                // GlideImageLoader.LoadImage(this, file.Path, StoryImageView, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);

                StoryVideoView.Visibility = StoryVideoView.Visibility switch
                {
                    ViewStates.Visible => ViewStates.Gone,
                    _ => StoryVideoView.Visibility
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void SetVideoStory(string url)
        {
            try
            {
                StoryImageView.Visibility = StoryImageView.Visibility switch
                {
                    ViewStates.Visible => ViewStates.Gone,
                    _ => StoryImageView.Visibility
                };

                StoryVideoView.Visibility = StoryVideoView.Visibility switch
                {
                    ViewStates.Gone => ViewStates.Visible,
                    _ => StoryVideoView.Visibility
                };

                PlayIconVideo.Visibility = ViewStates.Visible;
                PlayIconVideo.Tag = "Play";
                PlayIconVideo.SetImageResource(Resource.Drawable.ic_play_arrow);

                switch (StoryVideoView.IsPlaying)
                {
                    case true:
                        StoryVideoView.Suspend();
                        break;
                }

                if (url.Contains("http"))
                {
                    StoryVideoView.SetVideoURI(Uri.Parse(url));
                }
                else
                {
                    var file = Uri.FromFile(new File(url));
                    StoryVideoView.SetVideoPath(file?.Path);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void DestroyBasic()
        {
            try
            { 
                StoryImageView = null!;
                StoryVideoView = null!;
                PlayIconVideo = null!;
                EmojisView = null!;
                EmojisIconEditText = null!;
                AddStoryButton = null!;
                RootView = null!; 
                StoriesProgress = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
        #endregion

        #region Events

        private void PlayIconVideoOnClick(object sender, EventArgs e)
        {
            try
            {
                switch (PlayIconVideo?.Tag?.ToString())
                {
                    case "Play":
                    {
                        MediaMetadataRetriever retriever;
                        if (PathStory.Contains("http"))
                        {
                            StoryVideoView.SetVideoURI(Uri.Parse(PathStory));

                            retriever = new MediaMetadataRetriever();
                            switch ((int)Build.VERSION.SdkInt)
                            {
                                case >= 14:
                                    retriever.SetDataSource(PathStory, new Dictionary<string, string>());
                                    break;
                                default:
                                    retriever.SetDataSource(PathStory);
                                    break;
                            }
                        }
                        else
                        {
                            var file = Uri.FromFile(new File(PathStory));
                            StoryVideoView.SetVideoPath(file?.Path);

                            retriever = new MediaMetadataRetriever();
                            //if ((int)Build.VERSION.SdkInt >= 14)
                            //    retriever.SetDataSource(file.Path, new Dictionary<string, string>());
                            //else
                            //    retriever.SetDataSource(file.Path);
                            retriever.SetDataSource(file?.Path);
                        }
                        StoryVideoView.Start();

                        Duration = Long.ParseLong(retriever.ExtractMetadata(MetadataKey.Duration) ?? string.Empty);
                        retriever.Release();

                        StoriesProgress.Visibility = ViewStates.Visible;
                        StoriesProgress.SetStoriesCount(1); // <- set stories
                        StoriesProgress.SetStoryDuration(Duration); // <- set a story duration
                        StoriesProgress.StartStories(); // <- start progress

                        PlayIconVideo.Tag = "Stop";
                        PlayIconVideo.SetImageResource(Resource.Drawable.ic_stop_white_24dp);
                        break;
                    }
                    default:
                    {
                        StoriesProgress.Visibility = ViewStates.Gone;
                        StoriesProgress.Pause();

                        StoryVideoView.Pause();

                        if (PlayIconVideo != null)
                        {
                            PlayIconVideo.Tag = "Play";
                            PlayIconVideo.SetImageResource(Resource.Drawable.ic_play_arrow);
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Methods.DisplayReportResultTrack(ex);
            }
        }

        private void StoryVideoViewOnCompletion(object sender, EventArgs e)
        {
            try
            {
                StoriesProgress.Visibility = ViewStates.Gone;
                StoriesProgress.Pause();
                StoryVideoView.Pause();

                PlayIconVideo.Tag = "Play";
                PlayIconVideo.SetImageResource(Resource.Drawable.ic_play_arrow);
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        //add
        private void AddStoryButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                if (Methods.CheckConnectivity())
                { 
                    var item = new FileUpload
                    {
                        StoryFileType = Type,
                        StoryFilePath = PathStory,
                        StoryDescription = EmojisIconEditText.Text,
                        StoryTitle = EmojisIconEditText.Text, 
                        StoryThumbnail = Thumbnail, 
                    };

                    Intent intent = new Intent(this, typeof(PostService));
                    intent.SetAction(PostService.ActionStory);
                    intent.PutExtra("DataPost", JsonConvert.SerializeObject(item));
                    StartService(intent);

                    Finish();  
                }
                else
                {
                    Toast.MakeText(this, GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short)?.Show();
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception); 
            }
        }

        #endregion
    }
}
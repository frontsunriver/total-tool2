using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Media;
using Android.OS; 
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Newtonsoft.Json; 
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Utils;
using WoWonder.Library.Anjo.StoriesProgressView;
using WoWonderClient.Classes.Story;
using WoWonderClient.Requests;
using Exception = System.Exception;
using File = Java.IO.File;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.Story
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class ViewStoryActivity : AppCompatActivity, StoriesProgressView.IStoriesListener, View.IOnTouchListener, MediaPlayer.IOnCompletionListener, MediaPlayer.IOnPreparedListener
    {
        #region Variables Basic

        private ImageView StoryImageView, UserImageView;
        private VideoView StoryVideoView;
        private string UserId = "", StoryId = "";
        private int IndexItem;
        private StoriesProgressView StoriesProgress;
        private StoryDataObject DataStories;
        private View ReverseView, SkipView;
        private TextView CaptionStoryTextView, UsernameTextView, LastSeenTextView, DeleteIconView;
        private int Counter;
        private long PressTime;
        private readonly long Limit = 500L;
        private Toolbar Toolbar;
        private TabbedMainActivity GlobalContext;
        private LinearLayout StoryAboutLayout;
        private ObservableCollection<StoryDataObject> StoryList;
        private bool PlayerPaused;

        #endregion

        #region General

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetTheme(AppSettings.SetTabDarkTheme ? Resource.Style.MyTheme_Dark_Base : Resource.Style.MyTheme_Base);

                Methods.App.FullScreenApp(this, true);

                // Create your application here
                SetContentView(Resource.Layout.View_Story_Layout);

                GlobalContext = TabbedMainActivity.GetInstance();

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();

                if (Intent != null)
                {
                    UserId = Intent.GetStringExtra("UserId") ?? "";
                    IndexItem = Intent.GetIntExtra("IndexItem", 0);
                    DataStories = JsonConvert.DeserializeObject<StoryDataObject>(Intent?.GetStringExtra("DataItem") ?? "");
                }

                LoadData(DataStories);
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
                StoriesProgress?.Destroy();
                DestroyBasic();
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                base.OnDestroy();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        #region Functions

        private void InitComponent()
        {
            try
            {
                StoryAboutLayout = FindViewById<LinearLayout>(Resource.Id.storyaboutLayout);
                StoryImageView = FindViewById<ImageView>(Resource.Id.imagstoryDisplay);
                StoriesProgress = FindViewById<StoriesProgressView>(Resource.Id.stories);
                CaptionStoryTextView = FindViewById<TextView>(Resource.Id.storyaboutText);
                UserImageView = FindViewById<ImageView>(Resource.Id.imageAvatar);
                UsernameTextView = FindViewById<TextView>(Resource.Id.username);
                LastSeenTextView = FindViewById<TextView>(Resource.Id.time);
                DeleteIconView = FindViewById<TextView>(Resource.Id.DeleteIcon);
                ReverseView = FindViewById<View>(Resource.Id.reverse);
                SkipView = FindViewById<View>(Resource.Id.skip);

                StoriesProgress.Visibility = ViewStates.Visible;

                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeRegular, DeleteIconView, FontAwesomeIcon.TrashAlt);

                ReverseView.SetOnTouchListener(this);
                SkipView.SetOnTouchListener(this);

                InitVideoView();

                var checkSection = GlobalContext?.NewsFeedTab?.PostFeedAdapter?.HolderStory?.StoryAdapter?.StoryList;
                StoryList = checkSection?.Count switch
                {
                    > 0 => new ObservableCollection<StoryDataObject>(checkSection),
                    _ => StoryList
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void InitVideoView()
        {
            try
            {
                StoryVideoView = FindViewById<VideoView>(Resource.Id.VideoView);

                if (StoryVideoView != null)
                {
                    StoryVideoView.SetOnPreparedListener(this);
                    StoryVideoView.SetOnCompletionListener(this);
                    StoryVideoView.SetAudioAttributes(new AudioAttributes.Builder()?.SetUsage(AudioUsageKind.Media)?.SetContentType(AudioContentType.Movie)?.Build());
                }
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
                Toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                if (Toolbar != null)
                {
                    Toolbar.SetTitleTextColor(Color.ParseColor(AppSettings.MainColor));
                    SetSupportActionBar(Toolbar);
                    SupportActionBar.SetDisplayShowCustomEnabled(true);
                    SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                    SupportActionBar.SetHomeButtonEnabled(true);
                    SupportActionBar.SetDisplayShowHomeEnabled(true);

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
                        DeleteIconView.Click += DeleteIconViewOnClick;
                        ReverseView.Click += ReverseViewOnClick;
                        SkipView.Click += SkipViewOnClick;
                        break;
                    default:
                        DeleteIconView.Click -= DeleteIconViewOnClick;
                        ReverseView.Click -= ReverseViewOnClick;
                        SkipView.Click -= SkipViewOnClick;
                        break;
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
                StoryVideoView = null!;
                StoryAboutLayout = null!;
                StoryImageView = null!;
                StoriesProgress = null!;
                CaptionStoryTextView = null!;
                UserImageView = null!;
                UsernameTextView = null!;
                LastSeenTextView = null!;
                DeleteIconView = null!;
                ReverseView = null!;
                SkipView = null!;
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

        #region Events

        //delete story
        private async void DeleteIconViewOnClick(object sender, EventArgs e)
        {
            try
            {
                StoriesProgress.Pause();

                switch (StoryVideoView.IsPlaying)
                {
                    case true:
                        StoryVideoView.Pause();
                        break;
                }

                if (Methods.CheckConnectivity())
                {
                    (int respondCode, var respond) = await RequestsAsync.Story.DeleteStoryAsync(StoryId);
                    switch (respondCode)
                    {
                        case 200:
                        {
                            var modelStory = GlobalContext.NewsFeedTab.PostFeedAdapter.HolderStory.StoryAdapter;

                            var story = modelStory?.StoryList?.FirstOrDefault(a => a.UserId == UserId);
                            switch (story)
                            {
                                case null:
                                    return;
                            }
                            var item = story.Stories.FirstOrDefault(q => q.Id == StoryId);
                            if (item != null)
                            {
                                story.Stories.Remove(item);

                                modelStory.NotifyItemChanged(modelStory.StoryList.IndexOf(story));

                                switch (story.Stories.Count)
                                {
                                    case 0:
                                        modelStory?.StoryList.Remove(story);
                                        modelStory.NotifyDataSetChanged();
                                        break;
                                }
                            }
                            Toast.MakeText(this, GetString(Resource.String.Lbl_Deleted), ToastLength.Short)?.Show();

                            Finish();
                            break;
                        }
                        default:
                            Methods.DisplayReportResult(this, respond);
                            break;
                    }
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

        private static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1St1970).TotalMilliseconds;
        }

        private void SkipViewOnClick(object sender, EventArgs e)
        {
            try
            {
                StoriesProgress.Skip();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private void ReverseViewOnClick(object sender, EventArgs e)
        {
            try
            {
                StoriesProgress.Reverse();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        #endregion

        private async void LoadData(StoryDataObject dataStories)
        {
            try
            {
                switch (dataStories)
                {
                    case null:
                        return;
                }

                GlideImageLoader.LoadImage(this, dataStories.Avatar, UserImageView, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                //UsernameTextView.Text = WoWonderTools.GetNameFinal(dataStories); 
                DeleteIconView.Visibility = dataStories.Stories[0].IsOwner ? ViewStates.Visible : ViewStates.Invisible;

                StoriesProgress ??= FindViewById<StoriesProgressView>(Resource.Id.stories);
                if (StoriesProgress != null)
                {
                    StoriesProgress.Visibility = ViewStates.Visible;

                    int count = dataStories.Stories.Count;
                    StoriesProgress.Visibility = ViewStates.Visible;
                    StoriesProgress.SetStoriesCount(count); // <- set stories
                    StoriesProgress.SetStoriesListener(this); // <- set listener 
                    //StoriesProgress.SetStoryDuration(10000L); // <- set a story duration   

                    var fistStory = dataStories.Stories.FirstOrDefault();
                    if (fistStory != null)
                    {
                        StoriesProgress.SetStoriesCountWithDurations(dataStories.DurationsList.ToArray());

                        await SetStory(fistStory);

                        StoriesProgress.StartStories(); // <- start progress 
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private string MediaFile;

        private async Task SetStory(StoryDataObject.Story story)
        {
            try
            {
                StoryId = story.Id;
                switch (story.IsOwner)
                {
                    case true:
                        UsernameTextView.Text = WoWonderTools.GetNameFinal(DataStories) + "  " + Methods.Time.ReplaceTime(story.TimeText);
                        LastSeenTextView.Text = GetText(Resource.String.Lbl_SeenBy) + " " + story.ViewCount;
                        break;
                    default:
                        UsernameTextView.Text = WoWonderTools.GetNameFinal(DataStories);
                        LastSeenTextView.Text = story.TimeText;
                        break;
                }

                //image and video
                MediaFile = !story.Thumbnail.Contains("avatar") && story.Videos.Count == 0
                    ? story.Thumbnail
                    : story.Videos[0].Filename;

                switch (StoryVideoView)
                {
                    case null:
                        InitVideoView();
                        break;
                }

                string caption = "";
                switch (string.IsNullOrEmpty(story.Description))
                {
                    case false:
                        caption = story.Description;
                        break;
                    default:
                    {
                        caption = string.IsNullOrEmpty(story.Title) switch
                        {
                            false => story.Title,
                            _ => caption
                        };

                        break;
                    }
                }

                if (string.IsNullOrEmpty(caption) || string.IsNullOrWhiteSpace(caption))
                {
                    StoryAboutLayout.Visibility = ViewStates.Gone;
                }
                else
                {
                    StoryAboutLayout.Visibility = ViewStates.Visible;
                    CaptionStoryTextView.Text = Methods.FunString.DecodeString(caption);
                }

                switch (StoryVideoView)
                {
                    case null:
                        InitVideoView();
                        break;
                }

                var type = Methods.AttachmentFiles.Check_FileExtension(MediaFile);
                switch (type)
                {
                    case "Video":
                    {
                        var fileName = MediaFile.Split('/').Last();
                        MediaFile = WoWonderTools.GetFile(DateTime.Now.Day.ToString(), Methods.Path.FolderDiskStory, fileName, MediaFile);

                        StoryImageView.Visibility = ViewStates.Gone;
                        StoryVideoView.Visibility = ViewStates.Visible;
                        if (MediaFile.Contains("http"))
                        {
                            StoryVideoView.SetVideoURI(Uri.Parse(MediaFile));
                            StoryVideoView.Start();
                        }
                        else
                        {
                            var file = Uri.FromFile(new File(MediaFile));
                            StoryVideoView.SetVideoPath(file?.Path);
                            StoryVideoView.Start();
                        }

                        await Task.Delay(500);
                        break;
                    }
                    default:
                        StoryImageView.Visibility = ViewStates.Visible;
                        StoryVideoView.Visibility = ViewStates.Gone;

                        Glide.With(this).Load(MediaFile).Apply(new RequestOptions()).Into(StoryImageView);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnPrepared(MediaPlayer mp)
        {
            try
            {
                StoryVideoView.Start();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void OnCompletion(MediaPlayer mp)
        {
            try
            {
                mp.Stop();
                mp.Release();

                StoryVideoView = null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public async void OnNext()
        {
            try
            {
                StoryVideoView?.Pause();
                StoryVideoView = null!;

                if (Counter + 1 > DataStories.Stories.Count)
                {
                    OnComplete();
                    return;
                }

                var dataStory = DataStories.Stories[++Counter];
                if (dataStory != null)
                {
                    await SetStory(dataStory);
                }
                else
                {
                    OnComplete();
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public async void OnPrev()
        {
            try
            {
                switch (Counter - 1)
                {
                    case < 0:
                        return;
                }
                var dataStory = DataStories.Stories[--Counter];
                if (dataStory != null)
                {
                    await SetStory(dataStory);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void OnComplete()
        {
            try
            {
                IndexItem++;
                switch (StoryList.Count)
                {
                    case > 0 when StoryList.Count > IndexItem:
                        DataStories = StoryList[IndexItem];

                        StoriesProgress?.Destroy();
                        StoriesProgress = null!;

                        LoadData(DataStories);

                        DataStories.ProfileIndicator = AppSettings.StoryReadColor;
                        GlobalContext?.NewsFeedTab.PostFeedAdapter?.HolderStory?.StoryAdapter.NotifyItemChanged(IndexItem, "StoryRefresh");
                        break;
                    default:
                        AdsGoogle.Ad_Interstitial(this);
                        Finish();
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            try
            {
                switch (e.Action)
                {
                    case MotionEventActions.Down:
                        PressTime = CurrentTimeMillis();
                        StoriesProgress.Pause();

                        if (StoryVideoView != null && StoryVideoView.IsPlaying)
                        {
                            PlayerPaused = true;
                            StoryVideoView?.Pause();
                        }

                        return false;

                    case MotionEventActions.Up:
                        long now = CurrentTimeMillis();
                        StoriesProgress.Resume();

                        if (StoryVideoView != null && PlayerPaused)
                            StoryVideoView?.Resume();

                        return Limit < now - PressTime;
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
            return false;
        }

    }
}
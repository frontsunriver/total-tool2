using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using Android.Media;
using Android.Text;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Classes.Story;

namespace WoWonder.Activities.NativePost.Post
{
    public enum NativeFeedType
    {
        Global, Event, Group, Page, Popular, User, Saved, HashTag, SearchForPosts, Memories, Share
    }

    public class AdapterModelsClass
    {
        public long Id { get; set; }

        public string TypePost { get; set; }

        public PostDataObject PostData { get; set; }
        public bool IsDefaultFeedPost { get; set; }
        public bool IsSharingPost { get; set; }
        public PostModelType TypeView { get; set; }
        public SpannableString PostDataDecoratedContent { get; set; }
        public AboutModelClass AboutModel { get; set; }
        public InfoUserModelClass InfoUserModel { get; set; }
        // ardev
        public InfoUserModelClass headerSectionClass { get; set; }
        public SocialLinksModelClass SocialLinksModel { get; set; }
        public FollowersModelClass FollowersModel { get; set; }
        public GroupsModelClass GroupsModel { get; set; }
        public PagesModelClass PagesModel { get; set; }
        public ImagesModelClass ImagesModel { get; set; }
        public AlertModelClass AlertModel { get; set; }
        public GroupPrivacyModelClass PrivacyModelClass { get; set; }
        public PageInfoModelClass PageInfoModelClass { get; set; }
        public ObservableCollection<StoryDataObject> StoryList { get; set; }

        public PollsOptionObject PollsOption { get; set; }

        public string PollId { get; set; }
        public string PollOwnerUserId { get; set; }
        public bool Progress { get; set; }

        public MediaPlayer VoicePlayer { get; set; }
        public Timer Timer { get; set; }
        public bool MediaIsPlaying { get; set; }
    }

    public enum PostModelType
    {
        NormalPost = 1, AboutBox = 2, PagesBox = 3, GroupsBox = 4, FollowersBox = 5, ImagesBox = 6, Story = 7, AddPostBox = 8, EmptyState = 9, AlertBox = 10, VideoPost = 11, ImagePost = 12, VoicePost = 13,
        StickerPost = 14, YoutubePost = 15, DeepSoundPost = 16, PlayTubePost = 17, LinkPost = 18, ProductPost = 19, BlogPost = 20, FilePost = 21, AlertJoinBox = 22, SharedPost = 23, EventPost = 24, ColorPost = 25,
        FacebookPost = 26, MultiImage2 = 27, MultiImage3 = 28, MultiImage4 = 29, MultiImages = 30, PollPost = 31, AdsPost = 32, AdMob1 = 33, AdMob2 = 34,AdMob3 = 35, JobPost =36, AlertBoxAnnouncement = 37, FundingPost = 38 , PurpleFundPost = 39 ,
        SocialLinks = 40 , SuggestedGroupsBox = 41, VimeoPost = 42 , MapPost = 43, OfferPost = 44 , SearchForPosts  = 45, LivePost = 46, AgoraLivePost = 47, Section = 48 , SuggestedUsersBox = 49 , FbAdNative = 50, JobPostSection2 = 51, JobPostSection1 = 52,
        SharedHeaderPost = 53, HeaderPost = 54, TextSectionPostPart = 55, PrevBottomPostPart = 56, BottomPostPart = 57 , Divider = 58, ViewProgress = 59, PromotePost = 60,AddCommentSection = 61, CommentSection = 62, FilterSection = 63,
        InfoUserBox = 64 , InfoPageBox = 65 , InfoGroupBox = 67, TikTokPost = 68 , ProfileHeaderSection = 69
    }
      
    public class AboutModelClass
    {
        public string TitleHead { get; set; }
        public string Description { get; set; }
    }
     
    public class InfoUserModelClass
    {
        public UserDataObject UserData { get; set; }
    }

    public class SocialLinksModelClass
    {
        public string Facebook { get; set; }
        public string Instegram { get; set; }
        public string Twitter { get; set; }
        public string  Google { get; set; }
        public string Vk { get; set; }
        public string Youtube { get; set; }
    }

    public class FollowersModelClass
    {
        public List<UserDataObject> FollowersList { get; set; }
        public string TitleHead { get; set; }
        public string More { get; set; }
    }

    public class GroupPrivacyModelClass
    {
        public GroupClass GroupClass { get; set; } 
        public string GroupId { get; set; }
    }
     
    public class PageInfoModelClass
    {
        public PageClass PageClass { get; set; } 
        public string PageId { get; set; }
    }
     
    public class GroupsModelClass
    {
        public List<GroupClass> GroupsList { get; set; }
        public string TitleHead { get; set; }
        public string More { get; set; }
        public string UserProfileId { get; set; }
    }
     
    public class PagesModelClass
    {
        public List<PageClass> PagesList { get; set; }
        public string TitleHead { get; set; }
        public string More { get; set; }
    }
    public class ImagesModelClass
    {
        public List<PostDataObject> ImagesList { get; set; }
        public string TitleHead { get; set; }
        public string More { get; set; }
    }

    public class AlertModelClass
    {
        public int ImageDrawable { get; set; }
        public string TitleHead { get; set; }
        public string SubText { get; set; }
        public string LinerColor { get; set; }


        public string TypeAlert { get; set; }
        public int IconImage { get; set; }
    } 

    public class HeaderSectionClass
    {
        public string CountFollowers { get; set; }
        public string CountFollowing { get; set; }
        public string CountLikes { get; set; }
        public string CountPoints { get; set; }

    }
}
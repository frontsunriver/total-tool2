using WoWonder.Activities.NativePost.Post;
using WoWonderClient.Classes.Global;

namespace WoWonder.Activities.Communities.Adapters
{
    public enum SocialModelType
    {
        MangedGroups, JoinedGroups, MangedPages, LikedPages, Pages, Groups,Section
    }

    public class SocialModelsClass
    {
        public long Id { get; set; }
        public SocialModelType TypeView { get; set; }
        public GroupsModelClass MangedGroupsModel { get; set; }
        public PagesModelClass PagesModelClass { get; set; }
        public GroupClass GroupData { get; set; }
        public PageClass PageData { get; set; }
        public string TitleHead { get; set; }
        public string MoreIcon { get; set; }
    }

}
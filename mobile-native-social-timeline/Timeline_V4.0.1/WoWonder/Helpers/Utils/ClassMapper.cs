using System;
using AutoMapper;
using WoWonder.Activities.Comment.Adapters;
using WoWonder.SQLite;
using WoWonderClient.Classes.Comments;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Movies;

namespace WoWonder.Helpers.Utils
{
    public static class ClassMapper
    {
        public static IMapper Mapper;
        public static void SetMappers()
        {
            try
            {
                var configuration = new MapperConfiguration(cfg =>
                {
                    try
                    {
                        cfg.AllowNullCollections = true;
                     
                         
                        cfg.CreateMap<CommentObjectExtra, GetCommentObject>();
                        cfg.CreateMap<GetCommentObject, CommentObjectExtra>();

                        cfg.CreateMap<GetSiteSettingsObject.ConfigObject, DataTables.SettingsTb>().ForMember(x => x.AutoIdSettings, opt => opt.Ignore());
                        cfg.CreateMap<UserDataObject, DataTables.MyContactsTb>().ForMember(x => x.AutoIdMyFollowing, opt => opt.Ignore());
                        cfg.CreateMap<UserDataObject, DataTables.MyProfileTb>().ForMember(x => x.AutoIdMyProfile, opt => opt.Ignore());
                        cfg.CreateMap<GetMoviesObject.Movie, DataTables.WatchOfflineVideosTb>().ForMember(x => x.AutoIdWatchOfflineVideos, opt => opt.Ignore());
                        cfg.CreateMap<GiftObject.DataGiftObject, DataTables.GiftsTb>().ForMember(x => x.AutoIdGift, opt => opt.Ignore());
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                });
                // only during development, validate your mappings; remove it before release
                //configuration.AssertConfigurationIsValid();
               
                Mapper = configuration.CreateMapper(); 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }
}
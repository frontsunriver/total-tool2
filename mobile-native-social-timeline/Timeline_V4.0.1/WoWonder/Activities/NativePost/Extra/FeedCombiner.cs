using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Android.Content;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Classes.Story;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.NativePost.Extra
{
    public class FeedCombiner
    {
        private readonly Context MainContext;
        private readonly PostDataObject PostCollection;
        private readonly List<AdapterModelsClass> PostList;
        private PostModelType PostFeedType;
        private readonly PostModelResolver PostModelResolver;
        private readonly WoTextDecorator TextDecorator;

        public FeedCombiner(PostDataObject post, List<AdapterModelsClass> diffList, Context context)
        {
            try
            {
                MainContext = context;
                PostFeedType = PostFunctions.GetAdapterType(post);
                PostModelResolver = new PostModelResolver(context, PostFeedType);
                PostCollection = post;
                PostList = diffList;
                TextDecorator = new WoTextDecorator();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        // Add Post
        //=====================================

        public void AddAutoSection(bool isSharing = false)
        {
            try
            {
                switch (isSharing)
                {
                    case true:
                    {
                        var getSharedPostType = PostFunctions.GetAdapterType(PostCollection.SharedInfo.SharedInfoClass);
                        var collection = PostCollection.SharedInfo.SharedInfoClass;

                        switch (getSharedPostType)
                        {
                            case PostModelType.BlogPost:
                                PostModelResolver.PrepareBlog(collection);
                                break;
                            case PostModelType.EventPost:
                                PostModelResolver.PrepareEvent(collection);
                                switch (long.Parse(collection.EventId))
                                {
                                    case > 0:
                                        return;
                                }
                                break;
                            case PostModelType.ColorPost:
                                PostModelResolver.PrepareColorBox(collection);
                                break;
                            case PostModelType.LinkPost:
                                PostModelResolver.PrepareLink(collection);
                                break;
                            case PostModelType.ProductPost:
                                PostModelResolver.PrepareProduct(collection);
                                break;
                            case PostModelType.FundingPost:
                                PostModelResolver.PrepareFunding(collection);
                                break;
                            case PostModelType.PurpleFundPost:
                                PostModelResolver.PreparePurpleFundPost(collection);
                                break;
                            case PostModelType.OfferPost:
                                PostModelResolver.PrepareOffer(collection);
                                break;
                            case PostModelType.MapPost:
                                PostModelResolver.PrepareMapPost(collection);
                                break;
                            case PostModelType.PollPost:
                            case PostModelType.AdsPost:
                            case PostModelType.AdMob1:
                            case PostModelType.AdMob2:
                            case PostModelType.AdMob3:
                            case PostModelType.FbAdNative:
                                return;
                            case PostModelType.VideoPost:
                                WRecyclerView.GetInstance()?.CacheVideosFiles(Uri.Parse(collection.PostFileFull));
                                break;
                            case PostModelType.JobPost:
                                AddJobPost();
                                return;
                            case PostModelType.VimeoPost:
                            {
                                PostModelResolver.PreparVimeoVideo(collection); 
                                switch (AppSettings.EmbedVimeoVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        getSharedPostType = PostModelType.VimeoPost;
                                        break; 
                                    case VideoPostTypeSystem.Link:
                                        getSharedPostType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                }
                            } break;
                            case PostModelType.FacebookPost:
                            {
                                PostModelResolver.PrepareFacebookVideo(collection); 
                                switch (AppSettings.EmbedFacebookVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        getSharedPostType = PostModelType.FacebookPost;
                                        break; 
                                    case VideoPostTypeSystem.Link:
                                        getSharedPostType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                }
                            } break;
                            case PostModelType.PlayTubePost:
                            {
                                PostModelResolver.PreparePlayTubeVideo(collection);  
                                switch (AppSettings.EmbedPlayTubeVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        getSharedPostType = PostModelType.PlayTubePost;
                                        break; 
                                    case VideoPostTypeSystem.Link:
                                        getSharedPostType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                }
                            } break; 
                            case PostModelType.TikTokPost:
                            {
                                PostModelResolver.PrepareTikTokVideo(collection); 
                                switch (AppSettings.EmbedTikTokVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        getSharedPostType = PostModelType.TikTokPost;
                                        break; 
                                    case VideoPostTypeSystem.Link:
                                        getSharedPostType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                }
                            } break;
                        }

                        var item = new AdapterModelsClass
                        {
                            TypeView = getSharedPostType,
                            Id = long.Parse((int)getSharedPostType + collection.Id),
                            PostData = collection,
                            IsDefaultFeedPost = true,
                        };

                        PostList.Add(item);
                        break;
                    }
                    default:
                    {
                        switch (PostFeedType)
                        {
                            case PostModelType.BlogPost:
                                PostModelResolver.PrepareBlog(PostCollection);
                                break;
                            case PostModelType.EventPost:
                                PostModelResolver.PrepareEvent(PostCollection);
                                switch (long.Parse(PostCollection.EventId))
                                {
                                    case > 0:
                                        return;
                                }
                                break;
                            case PostModelType.ColorPost:
                                PostModelResolver.PrepareColorBox(PostCollection);
                                break;
                            case PostModelType.LinkPost:
                                PostModelResolver.PrepareLink(PostCollection);
                                break;
                            case PostModelType.ProductPost:
                                PostModelResolver.PrepareProduct(PostCollection);
                                break;
                            case PostModelType.FundingPost:
                                PostModelResolver.PrepareFunding(PostCollection);
                                break;
                            case PostModelType.PurpleFundPost:
                                PostModelResolver.PreparePurpleFundPost(PostCollection);
                                break; 
                            case PostModelType.OfferPost:
                                PostModelResolver.PrepareOffer(PostCollection);
                                break;
                            case PostModelType.MapPost:
                                PostModelResolver.PrepareMapPost(PostCollection);
                                break;
                            case PostModelType.PollPost:
                            case PostModelType.AdsPost:
                            case PostModelType.AdMob1:
                            case PostModelType.AdMob2:
                            case PostModelType.AdMob3:
                            case PostModelType.FbAdNative:
                                return;
                            case PostModelType.VideoPost:
                                WRecyclerView.GetInstance()?.CacheVideosFiles(Uri.Parse(PostCollection.PostFileFull));
                                break;
                            case PostModelType.JobPost:
                                AddJobPost();
                                return;
                            case PostModelType.SharedPost:
                                AddSharedPost();
                                return;
                            case PostModelType.VimeoPost:
                            {
                                PostModelResolver.PreparVimeoVideo(PostCollection);
                                switch (AppSettings.EmbedVimeoVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        PostFeedType = PostModelType.VimeoPost;
                                        break;
                                    case VideoPostTypeSystem.Link:
                                        PostFeedType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                    default:
                                        return;
                                }
                            }
                                break;
                            case PostModelType.FacebookPost:
                            {
                                PostModelResolver.PrepareFacebookVideo(PostCollection);
                                switch (AppSettings.EmbedFacebookVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        PostFeedType = PostModelType.FacebookPost;
                                        break;
                                    case VideoPostTypeSystem.Link:
                                        PostFeedType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                    default:
                                        return;
                                }
                            }
                                break;
                            case PostModelType.PlayTubePost:
                            {
                                PostModelResolver.PreparePlayTubeVideo(PostCollection);

                                switch (AppSettings.EmbedPlayTubeVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        PostFeedType = PostModelType.PlayTubePost;
                                        break;
                                    case VideoPostTypeSystem.Link:
                                        PostFeedType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                    default:
                                        return;
                                }
                            }
                                break;
                            case PostModelType.TikTokPost:
                            {
                                PostModelResolver.PrepareTikTokVideo(PostCollection); 
                                switch (AppSettings.EmbedTikTokVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        PostFeedType = PostModelType.TikTokPost;
                                        break;
                                    case VideoPostTypeSystem.Link:
                                        PostFeedType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                    default:
                                        return;
                                }
                            }  break; 
                        }

                        var item = new AdapterModelsClass
                        {
                            TypeView = PostFeedType,
                            Id = long.Parse((int)PostFeedType + PostCollection.Id),
                            PostData = PostCollection,
                            IsDefaultFeedPost = true,
                            PostDataDecoratedContent = TextDecorator.SetupStrings(PostCollection, MainContext),
                        };

                        PostList.Add(item);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        #region Add Post

        public void AddPostPromote(bool isSharing = false)
        {
            try
            {
                bool isPromoted = false;
                switch (isSharing)
                {
                    case true:
                    {
                        var collection = PostCollection.SharedInfo.SharedInfoClass;

                        if (collection.IsPostBoosted == "1" || collection.SharedInfo.SharedInfoClass != null && collection.SharedInfo.SharedInfoClass?.IsPostBoosted == "1")
                            isPromoted = true;

                        switch (isPromoted)
                        {
                            case true:
                            {
                                var item = new AdapterModelsClass
                                {
                                    TypeView = PostModelType.PromotePost,
                                    Id = long.Parse((int)PostModelType.PromotePost + collection.Id),
                                    PostData = collection,
                                    IsDefaultFeedPost = true,
                                };
                                PostList.Add(item);
                                break;
                            }
                        }

                        break;
                    }
                    default:
                    {
                        if (PostCollection.IsPostBoosted == "1" || PostCollection.SharedInfo.SharedInfoClass != null && PostCollection.SharedInfo.SharedInfoClass?.IsPostBoosted == "1")
                            isPromoted = true;

                        switch (isPromoted)
                        {
                            case true:
                            {
                                var item = new AdapterModelsClass
                                {
                                    TypeView = PostModelType.PromotePost,

                                    Id = long.Parse((int)PostModelType.PromotePost + PostCollection.Id),
                                    PostData = PostCollection,
                                    IsDefaultFeedPost = true,
                                };
                                PostList.Add(item);
                                break;
                            }
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

        public void AddPostHeader(bool isSharing = false)
        {
            try
            {
                switch (isSharing)
                {
                    case true:
                    {
                        var collection = PostCollection.SharedInfo.SharedInfoClass;
                        PostModelResolver.PrepareHeader(collection);
                        var item = new AdapterModelsClass
                        {
                            TypeView = PostModelType.SharedHeaderPost,
                            Id = long.Parse((int)PostModelType.SharedHeaderPost + collection.Id),
                            PostData = collection,
                            IsDefaultFeedPost = true,
                        };
                        PostList.Add(item);
                        break;
                    }
                    default:
                    {
                        PostModelResolver.PrepareHeader(PostCollection);
                        var item = new AdapterModelsClass
                        {
                            TypeView = PostModelType.HeaderPost, 
                            Id = long.Parse((int)PostModelType.HeaderPost + PostCollection.Id),
                            PostData = PostCollection,
                            IsDefaultFeedPost = true,
                            PostDataDecoratedContent = TextDecorator.SetupStrings(PostCollection, MainContext),
                        };
                        PostList.Add(item);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void AddPostTextSection(bool isSharing = false)
        {
            try
            {
                switch (isSharing)
                {
                    case true:
                    {
                        var collection = PostCollection.SharedInfo.SharedInfoClass;

                        var getSharedPostType = PostFunctions.GetAdapterType(collection);
                        switch (getSharedPostType)
                        {
                            case PostModelType.ColorPost:
                                return;
                        }

                        if (string.IsNullOrEmpty(collection.Orginaltext))
                            return;

                        PostModelResolver.PrepareTextSection(collection);

                        Console.WriteLine("TextSectionPostPart Id = " + (int)PostModelType.TextSectionPostPart + collection.Id);

                        var item = new AdapterModelsClass
                        {
                            TypeView = PostModelType.TextSectionPostPart,
                            Id = long.Parse((int)PostModelType.TextSectionPostPart + collection.Id),
                            PostData = collection,
                            IsDefaultFeedPost = true
                        };

                        PostList.Add(item);
                        break;
                    }
                    default:
                    {
                        switch (PostFeedType)
                        {
                            case PostModelType.ColorPost:
                                return;
                        }

                        if (string.IsNullOrEmpty(PostCollection.Orginaltext))
                            return;

                        PostModelResolver.PrepareTextSection(PostCollection);

                        Console.WriteLine("TextSectionPostPart Id = " + (int)PostModelType.TextSectionPostPart + PostCollection.Id);

                        var item = new AdapterModelsClass
                        {
                            TypeView = PostModelType.TextSectionPostPart,
                            Id = long.Parse((int)PostModelType.TextSectionPostPart + PostCollection.Id),
                            PostData = PostCollection,
                            IsDefaultFeedPost = true
                        };

                        PostList.Add(item);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void AddPostPrevBottom()
        {
            try
            {
                PostModelResolver.PreparePostPrevBottom(PostCollection);

                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.PrevBottomPostPart,
                    Id = long.Parse((int)PostModelType.PrevBottomPostPart + PostCollection.Id),

                    PostData = PostCollection,
                    IsDefaultFeedPost = true
                };

                PostList.Add(item);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void AddPostFooter()
        {
            try
            {
                bool isSharing = false;
                var collection = PostCollection.SharedInfo.SharedInfoClass;
                if (collection != null)
                {
                    isSharing = true;
                }

                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.BottomPostPart,
                    Id = long.Parse((int)PostModelType.BottomPostPart + PostCollection.Id),
                    IsSharingPost = isSharing,
                    PostData = PostCollection,
                    IsDefaultFeedPost = true 
                };

                PostList.Add(item);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void AddPostDivider(int index = -1)
        {

            try
            {
                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.Divider,
                    Id = long.Parse((int)PostModelType.Divider + PostCollection?.Id),

                    PostData = PostCollection,
                    IsDefaultFeedPost = true 
                };

                switch (index)
                {
                    case -1:
                        PostList.Add(item);
                        break;
                    default:
                        PostList.Insert(index, item);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void AddPostCommentAbility()
        {
            try
            {
                if (PostCollection == null || PostCollection.PostComments == "0" || PostCollection?.GetPostComments?.Count == 0)
                    return;

                var check = PostCollection?.GetPostComments?.FirstOrDefault(banjo => string.IsNullOrEmpty(banjo.CFile) && string.IsNullOrEmpty(banjo.Record) && !string.IsNullOrEmpty(banjo.Text));
                switch (check)
                {
                    case null:
                        return;
                }

                var item1 = new AdapterModelsClass
                {
                    TypeView = PostModelType.CommentSection,
                    Id = long.Parse((int)PostModelType.CommentSection + PostCollection?.Id),
                    PostData = PostCollection,
                    IsDefaultFeedPost = true 
                };
                PostList.Add(item1);

                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.AddCommentSection,
                    Id = long.Parse((int)PostModelType.AddCommentSection + PostCollection?.Id),
                    PostData = PostCollection,
                    IsDefaultFeedPost = true 
                };
                PostList.Add(item);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void AddSharedPost()
        {
            try
            {
                if (PostCollection.SharedInfo.SharedInfoClass != null)
                {
                    AddPostPromote(true);
                    AddPostHeader(true);
                    AddPostTextSection(true);
                    AddAutoSection(true);
                    AddPollsPostView(true);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void AddPollsPostView(bool isSharing = false)
        {
            try
            {
                switch (isSharing)
                {
                    case true:
                    {
                        var collection = PostCollection.SharedInfo.SharedInfoClass;

                        if (collection.Options != null)
                        {
                            var count = collection.Options.Count;
                            switch (count)
                            {
                                case > 0:
                                {
                                    foreach (var poll in collection.Options)
                                    {
                                        PostModelResolver.PreparePoll(poll);

                                        poll.PostId = collection.Id;
                                        poll.RelatedToPollsCount = count;

                                        var i = new AdapterModelsClass
                                        {
                                            TypeView = PostModelType.PollPost,
                                            Id = long.Parse((int)PostModelType.PollPost + collection.Id),

                                            PostData = collection,
                                            IsDefaultFeedPost = true,
                                            PollId = poll.Id,
                                            PollsOption = poll,
                                            PollOwnerUserId = collection.Publisher?.UserId
                                        };
                                        PostList.Add(i);
                                    }

                                    break;
                                }
                            }
                        }

                        break;
                    }
                    default:
                    {
                        if (PostCollection.Options != null)
                        {
                            var count = PostCollection.Options.Count;
                            switch (count)
                            {
                                case > 0:
                                {
                                    foreach (var poll in PostCollection.Options)
                                    {
                                        PostModelResolver.PreparePoll(poll);

                                        poll.PostId = PostCollection.Id;
                                        poll.RelatedToPollsCount = count;

                                        var i = new AdapterModelsClass
                                        {
                                            TypeView = PostModelType.PollPost,
                                            Id = long.Parse((int)PostModelType.PollPost + PostCollection.Id),

                                            PostData = PostCollection,
                                            IsDefaultFeedPost = true,
                                            PollId = poll.Id,
                                            PollsOption = poll,
                                            PollOwnerUserId = PostCollection.Publisher?.UserId
                                        };
                                        PostList.Add(i);
                                    }

                                    break;
                                }
                            }
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

        #endregion

        #region Extra Post

        public void AddAdsPost(string type = "Add")
        {
            try
            {
                if (PostCollection.UserData != null)
                {
                    PostModelResolver.PrepareAds(PostCollection);

                    var item = new AdapterModelsClass
                    {
                        TypeView = PostModelType.AdsPost,
                        Id = long.Parse((int)PostModelType.AdsPost + PostCollection.Id),
                        PostData = PostCollection,
                        PostDataDecoratedContent = TextDecorator.SetupStrings(PostCollection, MainContext),
                        IsDefaultFeedPost = true
                    };

                    switch (type)
                    {
                        case "Add":
                            PostList.Add(item);
                            AddPostDivider();
                            break;
                        default:
                        {
                            CountIndex = 0;
                            var model1 = PostList.FirstOrDefault(a => a.TypeView == PostModelType.Story);
                            var model2 = PostList.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                            var model3 = PostList.FirstOrDefault(a => a.TypeView == PostModelType.FilterSection);
                            var model4 = PostList.FirstOrDefault(a => a.TypeView == PostModelType.AlertBox);
                            var model5 = PostList.FirstOrDefault(a => a.TypeView == PostModelType.SearchForPosts);

                            if (model5 != null)
                                CountIndex += PostList.IndexOf(model5) + 1;
                            else if (model4 != null)
                                CountIndex += PostList.IndexOf(model4) + 1;
                            else if (model3 != null)
                                CountIndex += PostList.IndexOf(model3) + 1;
                            else if (model2 != null)
                                CountIndex += PostList.IndexOf(model2) + 1;
                            else if (model1 != null)
                                CountIndex += PostList.IndexOf(model1) + 1;
                            else
                                CountIndex = 0;

                            CountIndex++;
                            PostList.Insert(CountIndex, item);

                            InsertOnTopPostDivider();
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void AddJobPost()
        {
            try
            {
                if (PostCollection.Job != null)
                {
                    PostModelResolver.PrepareJob(PostCollection);

                    var i = new AdapterModelsClass
                    {
                        TypeView = PostModelType.JobPostSection1,
                        Id = long.Parse((int)PostModelType.JobPostSection1 + PostCollection.Id),

                        PostData = PostCollection,
                        IsDefaultFeedPost = true
                    };
                    var i2 = new AdapterModelsClass
                    {
                        TypeView = PostModelType.JobPostSection2,
                        Id = long.Parse((int)PostModelType.JobPostSection2 + PostCollection.Id),

                        PostData = PostCollection,
                        IsDefaultFeedPost = true
                    };

                    PostList.Add(i);
                    PostList.Add(i2);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void AddStoryPostView(string typePost, int index, PostDataObject postData = null)
        {
            try
            {
                switch (AppSettings.ShowStory)
                {
                    case true:
                    {
                        var story = new AdapterModelsClass
                        {
                            TypeView = PostModelType.Story,
                            StoryList = new ObservableCollection<StoryDataObject>(),
                            Id = 545454545
                        };

                        PostList.Add(story);
                        AddPostDivider();
                        break;
                    }
                }

                //if (typePost != "feed") return;
                //var check = PostList.FirstOrDefault(a => a.TypeView == PostModelType.FilterSection);
                //switch (check)
                //{
                //    case null:
                //    {
                //        string filter = WRecyclerView.GetInstance()?.GetFilter() ?? "";
                //        string titleHead = filter switch
                //        {
                //            "0" => MainContext.GetString(Resource.String.Lbl_All),
                //            "1" => MainContext.GetString(Resource.String.Lbl_People_i_Follow),
                //            _ => MainContext.GetString(Resource.String.Lbl_All)
                //        };

                //        var modelsClass = new AdapterModelsClass
                //        {
                //            TypeView = PostModelType.FilterSection,
                //            Id = 521,
                //            AboutModel = new AboutModelClass { TitleHead = titleHead },
                //        };

                //        PostList.Add(modelsClass);
                //        AddPostDivider();
                //        break;
                //    }
                //}
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void GroupsBoxPostView(GroupsModelClass modelClass, int index)
        {
            try
            {
                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.GroupsBox,
                    Id = 222222222,
                    GroupsModel = modelClass
                };

                switch (index)
                {
                    case -1:
                        PostList.Add(item);
                        AddPostDivider();
                        break;
                    default:
                        PostList.Insert(index, item);
                        AddPostDivider(PostList.IndexOf(item) + 1);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }
        
        public void InfoGroupBox(GroupPrivacyModelClass modelClass, int index)
        {
            try
            {
                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.InfoGroupBox,
                    Id = 7447453,
                    PrivacyModelClass = modelClass
                };

                switch (index)
                {
                    case -1:
                        PostList.Add(item);
                        AddPostDivider();
                        break;
                    default:
                        PostList.Insert(index, item);
                        AddPostDivider(PostList.IndexOf(item) + 1);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void PagesBoxPostView(PagesModelClass modelClass, int index)
        {
            try
            {
                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.PagesBox,
                    Id = 333333333,
                    PagesModel = modelClass
                };

                switch (index)
                {
                    case -1:
                        PostList.Add(item);
                        AddPostDivider();
                        break;
                    default:
                        PostList.Insert(index, item);
                        AddPostDivider(PostList.IndexOf(item) + 1);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }
        
        public void InfoPageBox(PageInfoModelClass modelClass, int index)
        {
            try
            {
                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.InfoPageBox,
                    Id = 7447453,
                    PageInfoModelClass = modelClass
                };

                switch (index)
                {
                    case -1:
                        PostList.Add(item);
                        AddPostDivider();
                        break;
                    default:
                        PostList.Insert(index, item);
                        AddPostDivider(PostList.IndexOf(item) + 1);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void ImagesBoxPostView(ImagesModelClass modelClass, int index)
        {
            try
            {
                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.ImagesBox,
                    Id = 444444444,
                    ImagesModel = modelClass
                };

                switch (index)
                {
                    case -1:
                        PostList.Add(item);
                        AddPostDivider();
                        break;
                    default:
                        PostList.Insert(index, item);
                        AddPostDivider(PostList.IndexOf(item) + 1);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void FollowersBoxPostView(FollowersModelClass modelClass, int index)
        {
            try
            {
                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.FollowersBox,
                    Id = 1111111111,
                    FollowersModel = modelClass
                };

                switch (index)
                {
                    case -1:
                        PostList.Add(item);
                        AddPostDivider();
                        break;
                    default:
                        PostList.Insert(index, item);
                        AddPostDivider(PostList.IndexOf(item) + 1);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void SocialLinksBoxPostView(SocialLinksModelClass modelClass, int index)
        {
            try
            {
                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.SocialLinks,
                    Id = 0023,
                    SocialLinksModel = modelClass,
                };

                switch (index)
                {
                    case -1:
                        PostList.Add(item);
                        AddPostDivider();
                        break;
                    default:
                        PostList.Insert(index, item);
                        AddPostDivider(PostList.IndexOf(item) + 1);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void AboutBoxPostView(string description, int index)
        {
            try
            {
                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.AboutBox,
                    Id = 0000,
                    AboutModel = new AboutModelClass { Description = description },
                };

                switch (index)
                {
                    case -1:
                        PostList.Add(item);
                        AddPostDivider();
                        break;
                    default:
                        PostList.Insert(index, item);
                        AddPostDivider(PostList.IndexOf(item) + 1);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }
        
        public void InfoUserBoxPostView(UserDataObject userData, int index)
        {
            try
            {
                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.InfoUserBox,
                    Id = 0000,
                    InfoUserModel = new InfoUserModelClass {UserData = userData}
                };

                switch (index)
                {
                    case -1:
                        PostList.Add(item);
                        AddPostDivider();
                        break;
                    default:
                        PostList.Insert(index, item);
                        AddPostDivider(PostList.IndexOf(item) + 1);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void AddPostBoxPostView(string typePost, int index, PostDataObject postData = null)
        {
            try
            {
                var checkAddPostBox = PostList.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                switch (checkAddPostBox)
                {
                    case null:
                    {
                        var item = new AdapterModelsClass
                        {
                            TypeView = PostModelType.AddPostBox,
                            TypePost = typePost,
                            Id = 959595959,
                            PostData = postData
                        };

                        switch (index)
                        {
                            case -1:
                                PostList.Add(item);
                                AddPostDivider();
                                break;
                            default:
                                PostList.Insert(index, item);
                                AddPostDivider(index);
                                break;
                        }

                        break;
                    }
                }

                /*if (typePost != "feed") return;
                var check = PostList.FirstOrDefault(a => a.TypeView == PostModelType.FilterSection);
                if (check == null)
                { 
                    string filter = WRecyclerView.GetInstance()?.GetFilter() ?? "";
                    string titleHead = filter switch
                    {
                        "0" => MainContext.GetString(Resource.String.Lbl_All),
                        "1" => MainContext.GetString(Resource.String.Lbl_People_i_Follow),
                        _ => MainContext.GetString(Resource.String.Lbl_All)
                    };

                    var modelsClass = new AdapterModelsClass
                    {
                        TypeView = PostModelType.FilterSection,
                        Id = 521,
                        AboutModel = new AboutModelClass { TitleHead = titleHead },
                    };
                         
                    PostList.Add(modelsClass);
                    AddPostDivider();
                }*/
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void SearchForPostsView(string type, PostDataObject postData = null)
        {
            try
            {
                var check = PostList.FirstOrDefault(a => a.TypeView == PostModelType.SearchForPosts);
                switch (check)
                {
                    case null:
                    {
                        var item = new AdapterModelsClass
                        {
                            TypeView = PostModelType.SearchForPosts,
                            TypePost = type,
                            Id = 2321,
                            PostData = postData
                        };

                        PostList.Add(item);
                        AddPostDivider();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void SetAnnouncementAlertView(string subText, string color, int? resource = null)
        {
            try
            {
                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.AlertBoxAnnouncement,
                    Id = 66655666,
                    AlertModel = new AlertModelClass
                    {
                        TitleHead = MainContext.GetText(Resource.String.Lbl_Announcement),
                        SubText = subText,
                        LinerColor = color
                    }
                };

                if (resource.HasValue)
                    item.AlertModel.ImageDrawable = resource.Value;

                PostList.Insert(0, item);
                AddPostDivider(PostList.IndexOf(item) + 1);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void AddGreetingAlertPostView()
        {
            try
            {
                var item = PostModelResolver.PrepareGreetingAlert();
                switch (item)
                {
                    case null:
                        return;
                    default:
                        PostList.Add(item);
                        AddPostDivider();
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void AddFindMoreAlertPostView(string type)
        {
            try
            {
                var item = PostModelResolver.SetFindMoreAlert(type);
                switch (item)
                {
                    case null:
                        return;
                    default:
                        PostList.Add(item);
                        AddPostDivider();
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public static string TypePost = TemplateView.MediumTemplate;
        public void AddAdsPostView(PostModelType modelType)
        {
            try
            {
                switch (modelType)
                {
                    case PostModelType.AdMob1:
                    {
                        if (TypePost == TemplateView.MediumTemplate)
                        {
                            TypePost = TemplateView.MediumTemplate;
                        }
                        else if (TypePost == TemplateView.NativeContentAd)
                        {
                            TypePost = TemplateView.NativeContentAd;
                        }

                        if (TypePost == TemplateView.MediumTemplate)
                        {
                            var adMobBox = new AdapterModelsClass
                            {
                                TypeView = PostModelType.AdMob1,
                                Id = 2222019,
                            };
                            PostList.Add(adMobBox);
                        }
                        else
                        {
                            var adMobBox = new AdapterModelsClass
                            {
                                TypeView = PostModelType.AdMob2,
                                Id = 2222019,
                            };
                            PostList.Add(adMobBox);
                        }

                        break;
                    }
                    case PostModelType.AdMob2:
                    {
                        var adMobBox = new AdapterModelsClass
                        {
                            TypeView = PostModelType.AdMob2,
                            Id = 2222019,
                        };
                        PostList.Add(adMobBox);
                        break;
                    }
                    case PostModelType.AdMob3:
                    {
                        var adMobBox = new AdapterModelsClass
                        {
                            TypeView = PostModelType.AdMob3,
                            Id = 2222019,
                        };
                        PostList.Add(adMobBox);
                        break;
                    }
                    default:
                    {
                        var adMobBox = new AdapterModelsClass
                        {
                            TypeView = PostModelType.FbAdNative,
                            Id = 2222220
                        };
                        PostList.Add(adMobBox);
                        break;
                    }
                }

                AddPostDivider();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void AddSuggestedBoxPostView(PostModelType modelType)
        {
            try
            {
                switch (modelType)
                {
                    case PostModelType.SuggestedGroupsBox:
                        PostList.Add(new AdapterModelsClass
                        {
                            TypeView = PostModelType.SuggestedGroupsBox,
                            Id = 3216545,
                        });
                        break;
                    default:
                        PostList.Add(new AdapterModelsClass
                        {
                            TypeView = PostModelType.SuggestedUsersBox,
                            Id = 3228546,
                        });
                        break;
                }

                AddPostDivider();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        #endregion

        // Insert Post
        //=====================================
        private void InsertOnTopAutoSection(bool isSharing = false)
        {
            try
            {
                switch (isSharing)
                {
                    case true:
                    {
                        var getSharedPostType = PostFunctions.GetAdapterType(PostCollection.SharedInfo.SharedInfoClass);

                        var collection = PostCollection.SharedInfo.SharedInfoClass;

                        switch (getSharedPostType)
                        {
                            case PostModelType.BlogPost:
                                PostModelResolver.PrepareBlog(collection);
                                break;
                            case PostModelType.EventPost:
                                PostModelResolver.PrepareEvent(collection);
                                switch (long.Parse(collection.EventId))
                                {
                                    case > 0:
                                        return;
                                }
                                break;
                            case PostModelType.ColorPost:
                                PostModelResolver.PrepareColorBox(collection);
                                break;
                            case PostModelType.LinkPost:
                                PostModelResolver.PrepareLink(collection);
                                break;
                            case PostModelType.ProductPost:
                                PostModelResolver.PrepareProduct(collection);
                                break;
                            case PostModelType.FundingPost:
                                PostModelResolver.PrepareFunding(collection);
                                break;
                            case PostModelType.PurpleFundPost:
                                PostModelResolver.PreparePurpleFundPost(collection);
                                break;
                            case PostModelType.OfferPost:
                                PostModelResolver.PrepareOffer(collection);
                                break;
                            case PostModelType.MapPost:
                                PostModelResolver.PrepareMapPost(collection);
                                break;
                            case PostModelType.PollPost:
                            case PostModelType.AdsPost:
                            case PostModelType.AdMob1:
                            case PostModelType.AdMob2:
                            case PostModelType.AdMob3:
                            case PostModelType.FbAdNative:
                                return;
                            case PostModelType.VideoPost:
                                WRecyclerView.GetInstance()?.CacheVideosFiles(Uri.Parse(collection.PostFileFull));
                                break;
                            case PostModelType.JobPost:
                                AddJobPost();
                                return;
                            case PostModelType.VimeoPost:
                            {
                                PostModelResolver.PreparVimeoVideo(collection);
                                switch (AppSettings.EmbedVimeoVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        getSharedPostType = PostModelType.VimeoPost;
                                        break;
                                    case VideoPostTypeSystem.Link:
                                        getSharedPostType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                    default:
                                        return;
                                }
                            }
                                break;
                            case PostModelType.FacebookPost:
                            {
                                PostModelResolver.PrepareFacebookVideo(collection);
                                switch (AppSettings.EmbedFacebookVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        getSharedPostType = PostModelType.FacebookPost;
                                        break;
                                    case VideoPostTypeSystem.Link:
                                        getSharedPostType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                    default:
                                        return;
                                }
                            }
                                break;
                            case PostModelType.PlayTubePost:
                            {
                                PostModelResolver.PreparePlayTubeVideo(collection);

                                switch (AppSettings.EmbedPlayTubeVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        getSharedPostType = PostModelType.PlayTubePost;
                                        break;
                                    case VideoPostTypeSystem.Link:
                                        getSharedPostType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                    default:
                                        return;
                                }
                            }
                                break;
                            case PostModelType.TikTokPost:
                            {
                                PostModelResolver.PrepareTikTokVideo(collection); 
                                switch (AppSettings.EmbedTikTokVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        getSharedPostType = PostModelType.TikTokPost;
                                        break;
                                    case VideoPostTypeSystem.Link:
                                        getSharedPostType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                    default:
                                        return;
                                }
                            }  break;
                        }

                        var item = new AdapterModelsClass
                        {
                            TypeView = getSharedPostType,
                            Id = long.Parse((int)getSharedPostType + collection.Id),
                            PostData = collection,
                            IsDefaultFeedPost = true,
                        };

                        CountIndex++;
                        PostList.Insert(CountIndex, item);
                        break;
                    }
                    default:
                    {
                        switch (PostFeedType)
                        {
                            case PostModelType.BlogPost:
                                PostModelResolver.PrepareBlog(PostCollection);
                                break;
                            case PostModelType.EventPost:
                                PostModelResolver.PrepareEvent(PostCollection);
                                switch (long.Parse(PostCollection.EventId))
                                {
                                    case > 0:
                                        return;
                                }
                                break;
                            case PostModelType.ColorPost:
                                PostModelResolver.PrepareColorBox(PostCollection);
                                break;
                            case PostModelType.LinkPost:
                                PostModelResolver.PrepareLink(PostCollection);
                                break;
                            case PostModelType.ProductPost:
                                PostModelResolver.PrepareProduct(PostCollection);
                                break;
                            case PostModelType.FundingPost:
                                PostModelResolver.PrepareFunding(PostCollection);
                                break;
                            case PostModelType.PurpleFundPost:
                                PostModelResolver.PreparePurpleFundPost(PostCollection);
                                break;
                            case PostModelType.OfferPost:
                                PostModelResolver.PrepareOffer(PostCollection);
                                break;
                            case PostModelType.MapPost:
                                PostModelResolver.PrepareMapPost(PostCollection);
                                break;
                            case PostModelType.PollPost:
                            case PostModelType.AdsPost:
                            case PostModelType.AdMob1:
                            case PostModelType.AdMob2:
                            case PostModelType.AdMob3:
                            case PostModelType.FbAdNative:
                                return;
                            case PostModelType.VideoPost:
                                WRecyclerView.GetInstance()?.CacheVideosFiles(Uri.Parse(PostCollection.PostFileFull));
                                break;
                            case PostModelType.JobPost:
                                AddJobPost();
                                return;
                            case PostModelType.SharedPost:
                                InsertOnTopSharedPost();
                                return;
                            case PostModelType.VimeoPost:
                            {
                                PostModelResolver.PreparVimeoVideo(PostCollection);
                                switch (AppSettings.EmbedVimeoVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        PostFeedType = PostModelType.VimeoPost;
                                        break;
                                    case VideoPostTypeSystem.Link:
                                        PostFeedType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                    default:
                                        return;
                                }
                            }
                                break;
                            case PostModelType.FacebookPost:
                            {
                                PostModelResolver.PrepareFacebookVideo(PostCollection);
                                switch (AppSettings.EmbedFacebookVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        PostFeedType = PostModelType.FacebookPost;
                                        break;
                                    case VideoPostTypeSystem.Link:
                                        PostFeedType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                    default:
                                        return;
                                }
                            }
                                break;
                            case PostModelType.PlayTubePost:
                            {
                                PostModelResolver.PreparePlayTubeVideo(PostCollection);

                                switch (AppSettings.EmbedPlayTubeVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        PostFeedType = PostModelType.PlayTubePost;
                                        break;
                                    case VideoPostTypeSystem.Link:
                                        PostFeedType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                    default:
                                        return;
                                }
                            }
                                break;
                            case PostModelType.TikTokPost:
                            {
                                PostModelResolver.PrepareTikTokVideo(PostCollection);

                                switch (AppSettings.EmbedTikTokVideoPostType)
                                {
                                    case VideoPostTypeSystem.EmbedVideo:
                                        PostFeedType = PostModelType.TikTokPost;
                                        break;
                                    case VideoPostTypeSystem.Link:
                                        PostFeedType = PostModelType.LinkPost;
                                        break;
                                    case VideoPostTypeSystem.None:
                                        return;
                                    default:
                                        return;
                                }  
                            }  break; 
                        }

                        var item = new AdapterModelsClass
                        {
                            TypeView = PostFeedType,
                            Id = long.Parse((int)PostFeedType + PostCollection.Id),
                            PostData = PostCollection,
                            IsDefaultFeedPost = true,
                            PostDataDecoratedContent = TextDecorator.SetupStrings(PostCollection, MainContext),
                        };

                        CountIndex++;
                        PostList.Insert(CountIndex, item);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        #region Insert Post

        public void InsertOnTopPostPromote(bool isSharing = false)
        {
            try
            {
                bool isPromoted = false;
                switch (isSharing)
                {
                    case true:
                    {
                        var collection = PostCollection.SharedInfo.SharedInfoClass;

                        if (collection.IsPostBoosted == "1" || collection.SharedInfo.SharedInfoClass != null && collection.SharedInfo.SharedInfoClass?.IsPostBoosted == "1")
                            isPromoted = true;

                        switch (isPromoted)
                        {
                            case true:
                            {
                                var item = new AdapterModelsClass
                                {
                                    TypeView = PostModelType.PromotePost,
                                    Id = long.Parse((int)PostModelType.PromotePost + collection.Id),
                                    PostData = collection,
                                    IsDefaultFeedPost = true,
                                };

                                CountIndex++;
                                PostList.Insert(CountIndex, item);
                                break;
                            }
                        }

                        break;
                    }
                    default:
                    {
                        if (PostCollection.IsPostBoosted == "1" || PostCollection.SharedInfo.SharedInfoClass != null && PostCollection.SharedInfo.SharedInfoClass?.IsPostBoosted == "1")
                            isPromoted = true;

                        switch (isPromoted)
                        {
                            case true:
                            {
                                var item = new AdapterModelsClass
                                {
                                    TypeView = PostModelType.PromotePost,
                                    Id = long.Parse((int)PostModelType.PromotePost + PostCollection.Id),
                                    PostData = PostCollection,
                                    IsDefaultFeedPost = true,
                                    PostDataDecoratedContent = TextDecorator.SetupStrings(PostCollection, MainContext),
                                };
                                CountIndex++;
                                PostList.Insert(CountIndex, item);
                                break;
                            }
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

        public void InsertOnTopPostHeader(bool isSharing = false)
        {
            try
            {
                switch (isSharing)
                {
                    case true:
                    {
                        var collection = PostCollection.SharedInfo.SharedInfoClass;
                        PostModelResolver.PrepareHeader(collection);
                        var item = new AdapterModelsClass
                        {
                            TypeView = PostModelType.SharedHeaderPost,
                            Id = long.Parse((int)PostModelType.SharedHeaderPost + collection.Id),
                            PostData = collection,
                            IsDefaultFeedPost = true,
                        };

                        CountIndex++;
                        PostList.Insert(CountIndex, item);
                        break;
                    }
                    default:
                    {
                        PostModelResolver.PrepareHeader(PostCollection);
                        var item = new AdapterModelsClass
                        {
                            TypeView = PostModelType.HeaderPost,
                            Id = long.Parse((int)PostModelType.HeaderPost + PostCollection.Id),
                            PostData = PostCollection,
                            IsDefaultFeedPost = true,
                            PostDataDecoratedContent = TextDecorator.SetupStrings(PostCollection, MainContext),
                        };
                        CountIndex++;
                        PostList.Insert(CountIndex, item);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        private void InsertOnTopPostTextSection(bool isSharing = false)
        {
            try
            {
                switch (isSharing)
                {
                    case true:
                    {
                        var collection = PostCollection.SharedInfo.SharedInfoClass;

                        var getSharedPostType = PostFunctions.GetAdapterType(collection);
                        switch (getSharedPostType)
                        {
                            case PostModelType.ColorPost:
                                return;
                        }


                        if (string.IsNullOrEmpty(collection.Orginaltext))
                            return;

                        PostModelResolver.PrepareTextSection(collection);

                        var item = new AdapterModelsClass
                        {
                            TypeView = PostModelType.TextSectionPostPart,
                            Id = long.Parse((int)PostModelType.TextSectionPostPart + collection.Id),
                            PostData = collection,
                            IsDefaultFeedPost = true,
                        };

                        CountIndex++;
                        PostList.Insert(CountIndex, item);
                        break;
                    }
                    default:
                    {
                        switch (PostFeedType)
                        {
                            case PostModelType.ColorPost:
                                return;
                        }

                        if (string.IsNullOrEmpty(PostCollection.Orginaltext))
                            return;

                        PostModelResolver.PrepareTextSection(PostCollection);

                        var item = new AdapterModelsClass
                        {
                            TypeView = PostModelType.TextSectionPostPart,
                            Id = long.Parse((int)PostModelType.TextSectionPostPart + PostCollection.Id),
                            PostData = PostCollection,
                            IsDefaultFeedPost = true,
                        };
                        CountIndex++;
                        PostList.Insert(CountIndex, item);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        private void InsertOnTopPollsPostView(bool isSharing = false)
        {
            try
            {
                switch (isSharing)
                {
                    case true:
                    {
                        var collection = PostCollection.SharedInfo.SharedInfoClass;

                        if (collection.Options != null)
                        {
                            var count = collection.Options.Count;
                            switch (count)
                            {
                                case > 0:
                                {
                                    foreach (var poll in collection.Options)
                                    {
                                        PostModelResolver.PreparePoll(poll);

                                        poll.PostId = collection.Id;
                                        poll.RelatedToPollsCount = count;

                                        var i = new AdapterModelsClass
                                        {
                                            TypeView = PostModelType.PollPost,
                                            Id = long.Parse((int)PostModelType.PollPost + collection.Id),
                                            PostData = collection,
                                            IsDefaultFeedPost = true,
                                            PollId = poll.Id,
                                            PollsOption = poll,
                                            PollOwnerUserId = collection.Publisher?.UserId,
                                        };
                                        CountIndex++;
                                        PostList.Insert(CountIndex, i);
                                    }

                                    break;
                                }
                            }
                        }

                        break;
                    }
                    default:
                    {
                        if (PostCollection.Options != null)
                        {
                            var count = PostCollection.Options.Count;
                            switch (count)
                            {
                                case > 0:
                                {
                                    foreach (var poll in PostCollection.Options)
                                    {
                                        PostModelResolver.PreparePoll(poll);

                                        poll.PostId = PostCollection.Id;
                                        poll.RelatedToPollsCount = count;

                                        var i = new AdapterModelsClass
                                        {
                                            TypeView = PostModelType.PollPost,
                                            Id = long.Parse((int)PostModelType.PollPost + PostCollection.Id),
                                            PostData = PostCollection,
                                            IsDefaultFeedPost = true,
                                            PollId = poll.Id,
                                            PollsOption = poll,
                                            PollOwnerUserId = PostCollection.Publisher?.UserId,
                                        };
                                        CountIndex++;
                                        PostList.Insert(CountIndex, i);
                                    }

                                    break;
                                }
                            }
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

        private void InsertOnTopPostPrevBottom()
        {
            try
            {
                PostModelResolver.PreparePostPrevBottom(PostCollection);

                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.PrevBottomPostPart,
                    Id = long.Parse((int)PostModelType.PrevBottomPostPart + PostCollection.Id),
                    PostData = PostCollection,
                    IsDefaultFeedPost = true,
                };

                CountIndex++;
                PostList.Insert(CountIndex, item);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        private void InsertOnTopPostFooter()
        {
            try
            {
                bool isSharing = false;
                var collection = PostCollection.SharedInfo.SharedInfoClass;
                if (collection != null)
                {
                    isSharing = true;
                }
                 
                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.BottomPostPart,
                    Id = long.Parse((int)PostModelType.BottomPostPart + PostCollection.Id),
                    PostData = PostCollection,
                    IsDefaultFeedPost = true,
                    IsSharingPost = isSharing,
                };

                CountIndex++;
                PostList.Insert(CountIndex, item);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        private void InsertOnTopPostDivider()
        {
            try
            {
                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.Divider,
                    Id = long.Parse((int)PostModelType.Divider + PostCollection.Id),

                    PostData = PostCollection,
                    IsDefaultFeedPost = true
                };

                CountIndex++;
                PostList.Insert(CountIndex, item);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        private void InsertOnTopSharedPost()
        {
            try
            {
                if (PostCollection.SharedInfo.SharedInfoClass != null)
                {
                    InsertOnTopPostPromote(true);
                    InsertOnTopPostHeader(true);
                    InsertOnTopPostTextSection(true);
                    InsertOnTopAutoSection(true);
                    InsertOnTopPollsPostView(true);
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        #endregion

        private int CountIndex = 1;
        public void CombineDefaultPostSections(string type = "Add")
        {
            try
            {
                switch (type)
                {
                    case "Add":
                        AddPostPromote();
                        AddPostHeader();
                        AddPostTextSection();
                        AddAutoSection();
                        AddPollsPostView();
                        AddPostPrevBottom();
                        AddPostFooter();
                        AddPostCommentAbility();
                        AddPostDivider();
                        break;
                    default:
                    {
                        CountIndex = 0;
                        var model1 = PostList.FirstOrDefault(a => a.TypeView == PostModelType.Story);
                        var model2 = PostList.FirstOrDefault(a => a.TypeView == PostModelType.AddPostBox);
                        var model3 = PostList.FirstOrDefault(a => a.TypeView == PostModelType.FilterSection);
                        var model4 = PostList.FirstOrDefault(a => a.TypeView == PostModelType.AlertBox);
                        var model5 = PostList.FirstOrDefault(a => a.TypeView == PostModelType.SearchForPosts);

                        if (model5 != null)
                            CountIndex += PostList.IndexOf(model5) + 1;
                        else if (model4 != null)
                            CountIndex += PostList.IndexOf(model4) + 1;
                        else if (model3 != null)
                            CountIndex += PostList.IndexOf(model3) + 1;
                        else if (model2 != null)
                            CountIndex += PostList.IndexOf(model2) + 1;
                        else if (model1 != null)
                            CountIndex += PostList.IndexOf(model1) + 1;
                        else
                            CountIndex = 0;

                        InsertOnTopPostPromote();
                        InsertOnTopPostHeader();
                        InsertOnTopPostTextSection();
                        InsertOnTopAutoSection();
                        InsertOnTopPollsPostView();
                        InsertOnTopPostPrevBottom();
                        InsertOnTopPostFooter();
                        InsertOnTopPostDivider();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void ProfileHeaderView(UserDataObject userData, int index)
        {
            try
            {
                var item = new AdapterModelsClass
                {
                    TypeView = PostModelType.ProfileHeaderSection,
                    Id = 0000,
                    headerSectionClass = new InfoUserModelClass { UserData = userData }
                };

                switch (index)
                {
                    case -1:
                        PostList.Add(item);
                        AddPostDivider();
                        break;
                    default:
                        PostList.Insert(index, item);
                        AddPostDivider(PostList.IndexOf(item) + 1);
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

    }
}
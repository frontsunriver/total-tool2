using System;
using System.Collections.Generic;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;

namespace WoWonder.Library.Anjo
{
    public static class XReactions
    {
        //private static readonly Reaction DefaultReact = new Reaction(ReactConstants.Like,ReactConstants.Default, AppSettings.SetTabDarkTheme ? "#ffffff" : "#E02020", Resource.Drawable.icon_post_like_vector);  //ic_action_like
        private static readonly Reaction DefaultReact = new Reaction(ReactConstants.Like, ReactConstants.Default, AppSettings.SetTabDarkTheme ? "#ffffff" : "#888888", Resource.Drawable.icon_post_like_vector);  //ic_action_like
        private static List<Reaction> Reactions = new List<Reaction>();
        
        public static Reaction GetDefaultReact()
        {
            return DefaultReact;
        }

        public static List<Reaction> GetReactions()
        {
            try
            {
                Reactions = AppSettings.PostButton switch
                {
                    PostButtonSystem.ReactionDefault => new List<Reaction>
                    {
                        new Reaction(ReactConstants.Like, ReactConstants.Like, ReactConstants.Blue, Resource.Drawable.emoji_like),
                        new Reaction(ReactConstants.Love, ReactConstants.Love, ReactConstants.RedLove, Resource.Drawable.emoji_love),
                        new Reaction(ReactConstants.HaHa, ReactConstants.HaHa, ReactConstants.YellowWow, Resource.Drawable.emoji_haha),
                        new Reaction(ReactConstants.Wow, ReactConstants.Wow, ReactConstants.YellowWow, Resource.Drawable.emoji_wow),
                        new Reaction(ReactConstants.Sad, ReactConstants.Sad, ReactConstants.YellowHaHa, Resource.Drawable.emoji_sad),
                        new Reaction(ReactConstants.Angry, ReactConstants.Angry, ReactConstants.RedAngry, Resource.Drawable.emoji_angry)
                    },
                    PostButtonSystem.ReactionSubShine => new List<Reaction>
                    {
                        new Reaction(ReactConstants.Like, ReactConstants.Like, ReactConstants.Blue, Resource.Drawable.like),
                        new Reaction(ReactConstants.Love, ReactConstants.Love, ReactConstants.RedLove, Resource.Drawable.love),
                        new Reaction(ReactConstants.HaHa, ReactConstants.HaHa, ReactConstants.YellowWow, Resource.Drawable.haha),
                        new Reaction(ReactConstants.Wow, ReactConstants.Wow, ReactConstants.YellowWow, Resource.Drawable.wow),
                        new Reaction(ReactConstants.Sad, ReactConstants.Sad, ReactConstants.YellowHaHa, Resource.Drawable.sad),
                        new Reaction(ReactConstants.Angry, ReactConstants.Angry, ReactConstants.RedAngry, Resource.Drawable.angry)
                    },
                    PostButtonSystem.Wonder => new List<Reaction>
                    {
                        new Reaction(ReactConstants.Like, ReactConstants.Like, ReactConstants.Blue, Resource.Drawable.emoji_like)
                    },
                    PostButtonSystem.DisLike => new List<Reaction>
                    {
                        new Reaction(ReactConstants.Like, ReactConstants.Like, ReactConstants.Blue, Resource.Drawable.emoji_like)
                    },
                    PostButtonSystem.Like => new List<Reaction>
                    {
                        new Reaction(ReactConstants.Like, ReactConstants.Like, ReactConstants.Blue, Resource.Drawable.emoji_like)
                    },
                    _ => new List<Reaction>
                    {
                        new Reaction(ReactConstants.Like, ReactConstants.Like, ReactConstants.Blue, Resource.Drawable.emoji_like)
                    }
                };

                return Reactions;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return new List<Reaction>();
            }
        }

    }
}
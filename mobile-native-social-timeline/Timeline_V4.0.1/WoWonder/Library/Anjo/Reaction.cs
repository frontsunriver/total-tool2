using System;
using WoWonder.Helpers.Utils;

namespace WoWonder.Library.Anjo
{
    public class Reaction
    {
        //ReactButton Text for this Reaction
        private readonly string ReactText; 
        //ReactButton Type for this Reaction
        private readonly string ReactType; 
        //ReactButton TextColor value for this Reaction
        private readonly string ReactTextColor; 
        //ReactButton Icon id value for this Reaction
        private readonly int ReactIconId;

       //This Constructor for default state because React type not equal react Text for example in library default state text is 'like' but type is 'default'
        public Reaction(string reactText, string reactType, string reactTextColor, int reactIconId)
        {
            ReactText = reactText;
            ReactType = reactType;
            ReactTextColor = reactTextColor;
            ReactIconId = reactIconId;
        }
         
        /// <summary>
        ///  Constructor for all Reaction that text is equal type for example in like state text is 'like' and type is 'like' also
        /// </summary>
        /// <param name="reactText"></param>
        /// <param name="reactTextColor"></param>
        /// <param name="reactIconId"></param>
        public Reaction(string reactText, string reactTextColor, int reactIconId)
        {
            ReactText = reactText;
            ReactType = reactText;
            ReactTextColor = reactTextColor;
            ReactIconId = reactIconId;
        }

        public string GetReactText()
        {
            return ReactText;
        }

        public string GetReactType()
        {
            return ReactType;
        }

        public string GetReactTextColor()
        {
            return ReactTextColor;
        }

        public int GetReactIconId()
        {
            return ReactIconId;
        }

        public override bool Equals(object obj)
        {
            try
            {
                switch (obj)
                {
                    //Assert that obj type is Reaction
                    case Reaction reaction:
                    {
                        //Cast Object to Reaction
                        Reaction react = reaction;
                        //if react type equal current Reaction type
                        return react.GetReactType().Equals(ReactType);
                    }
                    default:
                        return false;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return false;
            }
        }

        protected bool Equals(Reaction other)
        {
            return ReactText == other.ReactText && ReactType == other.ReactType && ReactTextColor == other.ReactTextColor && ReactIconId == other.ReactIconId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ReactText, ReactType, ReactTextColor, ReactIconId);
        }
    }
}
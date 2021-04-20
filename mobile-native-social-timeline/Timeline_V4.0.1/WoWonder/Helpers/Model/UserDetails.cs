using System;
using Android.App;
using WoWonder.Helpers.Utils;
using WoWonderClient;

namespace WoWonder.Helpers.Model
{
    public static class UserDetails
    {
        public static string AccessToken = string.Empty;
        public static string UserId = string.Empty;
        public static string Username = string.Empty;
        public static string FullName = string.Empty;
        public static string Password = string.Empty;
        public static string Email = string.Empty;
        public static string Cookie = string.Empty;
        public static string Status = string.Empty;
        public static string Avatar = "no_profile_image";
        public static string Cover = string.Empty;
        public static string DeviceId = string.Empty;
        //public static string DeviceMsgId = string.Empty;
        public static string LangName = string.Empty;
        public static string IsPro = string.Empty;
        public static string Url = string.Empty; 
        public static string Lat = string.Empty;
        public static string Lng = string.Empty;
        public static string Country = string.Empty;
        public static string City = string.Empty;
       
        public static int CountNotificationsStatic = 0;
        public static string OffsetLastChat = "0";

        public static bool NotificationPopup { get; set; } = true;
        public static bool SoundControl = true;
        public static bool ChatHead = true;
        public static bool OnlineUsers = true;

        public static long UnixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public static string Time = UnixTimestamp.ToString();
         
        public static string MarketDistanceCount = "";

        public static string NearbyShopsDistanceCount = "";
        public static string NearbyBusinessDistanceCount = "";
        
        public static string NearByDistanceCount = "0";
        public static string NearByGender = "all"; 
        public static string NearByStatus = "2";
        public static string NearByRelationship = "5";

        public static string SearchGender = "all";
        public static string SearchCountry = "all";
        public static string SearchStatus = "all";
        public static string SearchVerified = "all";
        public static string SearchProfilePicture = "all";
        public static string SearchFilterByAge = "off";
        public static string SearchAgeFrom = "10";
        public static string SearchAgeTo = "70";

        public static string FilterJobType = "";
        public static string FilterJobLocation = "";
        public static string FilterJobCategories = "";
          
        public static readonly string AndroidId = Android.Provider.Settings.Secure.GetString(Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);

        public static void ClearAllValueUserDetails()
        {
            try
            {
                AccessToken = string.Empty;
                UserId = string.Empty;
                Username = string.Empty;
                FullName = string.Empty;
                Password = string.Empty;
                Email = string.Empty;
                Cookie = string.Empty;
                Status = string.Empty;
                Avatar = string.Empty;
                Cover = string.Empty;
                DeviceId = string.Empty;
                //DeviceMsgId = string.Empty;
                LangName = string.Empty;
                Lat = string.Empty;
                Lng = string.Empty;

                Current.AccessToken = string.Empty; 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }
}
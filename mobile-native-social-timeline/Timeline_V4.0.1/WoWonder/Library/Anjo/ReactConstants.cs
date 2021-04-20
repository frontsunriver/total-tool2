using Android.App;

namespace WoWonder.Library.Anjo
{
    public static class ReactConstants
    {
        //Color Constants
        public const string Blue = "#0366d6";
        public const string RedLove = "#f0716b";
        public const string RedAngry = "#f15268";
        public const string YellowHaHa = "#fde99c";
        public const string YellowWow = "#f0ba15";

        //Text Constants
        public const string Default = "Default";
        public static string Like = Application.Context.GetString(Resource.String.Btn_Like);
        public static string Love = Application.Context.GetString(Resource.String.Btn_Love);
        public static string HaHa = Application.Context.GetString(Resource.String.Btn_Haha);
        public static string Wow = Application.Context.GetString(Resource.String.Btn_Wow);
        public static string Sad = Application.Context.GetString(Resource.String.Btn_Sad);
        public static string Angry = Application.Context.GetString(Resource.String.Btn_Angry);
    }
}
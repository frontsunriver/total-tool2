using DT.Xamarin.Agora.Video;

namespace WoWonder.Activities.Live.Page 
{
    public class Constants
    {
        private static readonly int BeautyEffectDefaultContrast = BeautyOptions.LighteningContrastNormal;
        private static readonly float BeautyEffectDefaultLightness = 0.7f;
        private static readonly float BeautyEffectDefaultSmoothness = 0.5f;
        private static readonly float BeautyEffectDefaultRedness = 0.1f;

        public static BeautyOptions DefaultBeautyOptions = new BeautyOptions(
            BeautyEffectDefaultContrast,
            BeautyEffectDefaultLightness,
            BeautyEffectDefaultSmoothness,
            BeautyEffectDefaultRedness);

        public static VideoEncoderConfiguration.VideoDimensions[] VideoDimensions = new VideoEncoderConfiguration.VideoDimensions[]{
            VideoEncoderConfiguration.VD320x240,
            VideoEncoderConfiguration.VD480x360,
            VideoEncoderConfiguration.VD640x360,
            VideoEncoderConfiguration.VD640x480,
            new VideoEncoderConfiguration.VideoDimensions(960, 540),
            VideoEncoderConfiguration.VD1280x720
        };

        public static int[] VideoMirrorModes = new int[]{
            DT.Xamarin.Agora.Constants.VideoMirrorModeAuto,
            DT.Xamarin.Agora.Constants.VideoMirrorModeEnabled,
            DT.Xamarin.Agora.Constants.VideoMirrorModeDisabled,
        };

        public static string PrefName = "Demo_Live";
        public static int DefaultProfileIdx = 2;
        public static string PrefResolutionIdx = "pref_profile_index";
        public static string PrefEnableStats = "pref_enable_stats";
        public static string PrefMirrorLocal = "pref_mirror_local";
        public static string PrefMirrorRemote = "pref_mirror_remote";
        public static string PrefMirrorEncode = "pref_mirror_encode";
        public static string KeyClientRole = "key_client_role";
    }

}
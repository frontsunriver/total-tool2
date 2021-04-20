using Uri = Android.Net.Uri;

namespace WoWonder.MediaPlayers
{
    public static class PlayerSettings
    {
        public static bool EnableOfflineMode = false;
        public static bool ShowInteractiveMediaAds = false;
        public static Uri ImAdsUri = Uri.Parse("https://pubads.g.doubleclick.net/gampad/ads?sz=640x480&iu=/124319096/external/ad_rule_samples&ciu_szs=300x250&ad_rule=1&impl=s&gdfp_req=1&env=vp&output=vmap&unviewed_position_start=1&cust_params=deployment%3Ddevsite%26sample_ar%3Dpremidpost&cmsid=496&vid=short_onecue&correlator=");
    }
}
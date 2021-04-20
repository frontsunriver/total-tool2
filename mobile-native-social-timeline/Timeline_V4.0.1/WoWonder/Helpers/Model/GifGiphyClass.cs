using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWonder.Helpers.Model
{
    public class GifGiphyClass
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public List<Datum> Data { get; set; }

        [JsonProperty("pagination", NullValueHandling = NullValueHandling.Ignore)]
        public Pagination DataPagination { get; set; }

        [JsonProperty("meta", NullValueHandling = NullValueHandling.Ignore)]
        public Meta DataMeta { get; set; }

        public class FixedHeightStill
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }
        }

        public class OriginalStill
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }
        }

        public class FixedWidth
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }

            [JsonProperty("mp4", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4 { get; set; }

            [JsonProperty("mp4_size", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4Size { get; set; }

            [JsonProperty("webp", NullValueHandling = NullValueHandling.Ignore)]
            public string Webp { get; set; }

            [JsonProperty("webp_size", NullValueHandling = NullValueHandling.Ignore)]
            public string WebpSize { get; set; }
        }

        public class FixedHeightSmallStill
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }
        }

        public class FixedHeightDownsampled
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }

            [JsonProperty("webp", NullValueHandling = NullValueHandling.Ignore)]
            public string Webp { get; set; }

            [JsonProperty("webp_size", NullValueHandling = NullValueHandling.Ignore)]
            public string WebpSize { get; set; }
        }

        public class Preview
        {

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("mp4", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4 { get; set; }

            [JsonProperty("mp4_size", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4Size { get; set; }
        }

        public class FixedHeightSmall
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }

            [JsonProperty("mp4", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4 { get; set; }

            [JsonProperty("mp4_size", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4Size { get; set; }

            [JsonProperty("webp", NullValueHandling = NullValueHandling.Ignore)]
            public string Webp { get; set; }

            [JsonProperty("webp_size", NullValueHandling = NullValueHandling.Ignore)]
            public string WebpSize { get; set; }
        }

        public class DownsizedStill
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }
        }

        public class Downsized
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }
        }

        public class DownsizedLarge
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }
        }

        public class FixedWidthSmallStill
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }
        }

        public class PreviewWebp
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }
        }

        public class FixedWidthStill
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }
        }

        public class FixedWidthSmall
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }

            [JsonProperty("mp4", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4 { get; set; }

            [JsonProperty("mp4_size", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4Size { get; set; }

            [JsonProperty("webp", NullValueHandling = NullValueHandling.Ignore)]
            public string Webp { get; set; }

            [JsonProperty("webp_size", NullValueHandling = NullValueHandling.Ignore)]
            public string WebpSize { get; set; }
        }

        public class DownsizedSmall
        {

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("mp4", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4 { get; set; }

            [JsonProperty("mp4_size", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4Size { get; set; }
        }

        public class FixedWidthDownsampled
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }

            [JsonProperty("webp", NullValueHandling = NullValueHandling.Ignore)]
            public string Webp { get; set; }

            [JsonProperty("webp_size", NullValueHandling = NullValueHandling.Ignore)]
            public string WebpSize { get; set; }
        }

        public class DownsizedMedium
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }
        }

        public class Original
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }

            [JsonProperty("frames", NullValueHandling = NullValueHandling.Ignore)]
            public string Frames { get; set; }

            [JsonProperty("mp4", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4 { get; set; }

            [JsonProperty("mp4_size", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4Size { get; set; }

            [JsonProperty("webp", NullValueHandling = NullValueHandling.Ignore)]
            public string Webp { get; set; }

            [JsonProperty("webp_size", NullValueHandling = NullValueHandling.Ignore)]
            public string WebpSize { get; set; }

            [JsonProperty("hash", NullValueHandling = NullValueHandling.Ignore)]
            public string Hash { get; set; }
        }

        public class FixedHeight
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }

            [JsonProperty("mp4", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4 { get; set; }

            [JsonProperty("mp4_size", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4Size { get; set; }

            [JsonProperty("webp", NullValueHandling = NullValueHandling.Ignore)]
            public string Webp { get; set; }

            [JsonProperty("webp_size", NullValueHandling = NullValueHandling.Ignore)]
            public string WebpSize { get; set; }
        }

        public class Looping
        {

            [JsonProperty("mp4", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4 { get; set; }

            [JsonProperty("mp4_size", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4Size { get; set; }
        }

        public class OriginalMp4
        {

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("mp4", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4 { get; set; }

            [JsonProperty("mp4_size", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4Size { get; set; }
        }

        public class PreviewGif
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }
        }

        public class WStill
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
            public string Size { get; set; }
        }

        public class Hd
        {

            [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
            public string Width { get; set; }

            [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
            public string Height { get; set; }

            [JsonProperty("mp4", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4 { get; set; }

            [JsonProperty("mp4_size", NullValueHandling = NullValueHandling.Ignore)]
            public string Mp4Size { get; set; }
        }


        public class Images
        {

            [JsonProperty("fixed_height_still", NullValueHandling = NullValueHandling.Ignore)]
            public FixedHeightStill FixedHeightStill { get; set; }

            [JsonProperty("original_still", NullValueHandling = NullValueHandling.Ignore)]
            public OriginalStill OriginalStill { get; set; }

            [JsonProperty("fixed_width", NullValueHandling = NullValueHandling.Ignore)]
            public FixedWidth FixedWidth { get; set; }

            [JsonProperty("fixed_height_small_still", NullValueHandling = NullValueHandling.Ignore)]
            public FixedHeightSmallStill FixedHeightSmallStill { get; set; }

            [JsonProperty("fixed_height_downsampled", NullValueHandling = NullValueHandling.Ignore)]
            public FixedHeightDownsampled FixedHeightDownsampled { get; set; }

            [JsonProperty("preview", NullValueHandling = NullValueHandling.Ignore)]
            public Preview Preview { get; set; }

            [JsonProperty("fixed_height_small", NullValueHandling = NullValueHandling.Ignore)]
            public FixedHeightSmall FixedHeightSmall { get; set; }

            [JsonProperty("downsized_still", NullValueHandling = NullValueHandling.Ignore)]
            public DownsizedStill DownsizedStill { get; set; }

            [JsonProperty("downsized", NullValueHandling = NullValueHandling.Ignore)]
            public Downsized Downsized { get; set; }

            [JsonProperty("downsized_large", NullValueHandling = NullValueHandling.Ignore)]
            public DownsizedLarge DownsizedLarge { get; set; }

            [JsonProperty("fixed_width_small_still", NullValueHandling = NullValueHandling.Ignore)]
            public FixedWidthSmallStill FixedWidthSmallStill { get; set; }

            [JsonProperty("preview_webp", NullValueHandling = NullValueHandling.Ignore)]
            public PreviewWebp PreviewWebp { get; set; }

            [JsonProperty("fixed_width_still", NullValueHandling = NullValueHandling.Ignore)]
            public FixedWidthStill FixedWidthStill { get; set; }

            [JsonProperty("fixed_width_small", NullValueHandling = NullValueHandling.Ignore)]
            public FixedWidthSmall FixedWidthSmall { get; set; }

            [JsonProperty("downsized_small", NullValueHandling = NullValueHandling.Ignore)]
            public DownsizedSmall DownsizedSmall { get; set; }

            [JsonProperty("fixed_width_downsampled", NullValueHandling = NullValueHandling.Ignore)]
            public FixedWidthDownsampled FixedWidthDownsampled { get; set; }

            [JsonProperty("downsized_medium", NullValueHandling = NullValueHandling.Ignore)]
            public DownsizedMedium DownsizedMedium { get; set; }

            [JsonProperty("original", NullValueHandling = NullValueHandling.Ignore)]
            public Original Original { get; set; }

            [JsonProperty("fixed_height", NullValueHandling = NullValueHandling.Ignore)]
            public FixedHeight FixedHeight { get; set; }

            [JsonProperty("looping", NullValueHandling = NullValueHandling.Ignore)]
            public Looping Looping { get; set; }

            [JsonProperty("original_mp4", NullValueHandling = NullValueHandling.Ignore)]
            public OriginalMp4 OriginalMp4 { get; set; }

            [JsonProperty("preview_gif", NullValueHandling = NullValueHandling.Ignore)]
            public PreviewGif PreviewGif { get; set; }

            [JsonProperty("480w_still", NullValueHandling = NullValueHandling.Ignore)]
            public WStill WStill { get; set; }

            [JsonProperty("hd", NullValueHandling = NullValueHandling.Ignore)]
            public Hd Hd { get; set; }
        }

        public class Onload
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }
        }

        public class Onclick
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }
        }

        public class Onsent
        {

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }
        }

        public class Analytics
        {

            [JsonProperty("onload", NullValueHandling = NullValueHandling.Ignore)]
            public Onload Onload { get; set; }

            [JsonProperty("onclick", NullValueHandling = NullValueHandling.Ignore)]
            public Onclick Onclick { get; set; }

            [JsonProperty("onsent", NullValueHandling = NullValueHandling.Ignore)]
            public Onsent Onsent { get; set; }
        }

        public class User
        {

            [JsonProperty("avatar_url", NullValueHandling = NullValueHandling.Ignore)]
            public string AvatarUrl { get; set; }

            [JsonProperty("banner_url", NullValueHandling = NullValueHandling.Ignore)]
            public string BannerUrl { get; set; }

            [JsonProperty("banner_image", NullValueHandling = NullValueHandling.Ignore)]
            public string BannerImage { get; set; }

            [JsonProperty("profile_url", NullValueHandling = NullValueHandling.Ignore)]
            public string ProfileUrl { get; set; }

            [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
            public string Username { get; set; }

            [JsonProperty("display_name", NullValueHandling = NullValueHandling.Ignore)]
            public string DisplayName { get; set; }

            [JsonProperty("is_verified", NullValueHandling = NullValueHandling.Ignore)]
            public bool IsVerified { get; set; }
        }

        public class Datum
        {

            [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
            public string Type { get; set; }

            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }

            [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
            public string Slug { get; set; }

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }

            [JsonProperty("bitly_gif_url", NullValueHandling = NullValueHandling.Ignore)]
            public string BitlyGifUrl { get; set; }

            [JsonProperty("bitly_url", NullValueHandling = NullValueHandling.Ignore)]
            public string BitlyUrl { get; set; }

            [JsonProperty("embed_url", NullValueHandling = NullValueHandling.Ignore)]
            public string EmbedUrl { get; set; }

            [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
            public string Username { get; set; }

            [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
            public string Source { get; set; }

            [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
            public string Rating { get; set; }

            [JsonProperty("content_url", NullValueHandling = NullValueHandling.Ignore)]
            public string ContentUrl { get; set; }

            [JsonProperty("source_tld", NullValueHandling = NullValueHandling.Ignore)]
            public string SourceTld { get; set; }

            [JsonProperty("source_post_url", NullValueHandling = NullValueHandling.Ignore)]
            public string SourcePostUrl { get; set; }

            [JsonProperty("is_sticker", NullValueHandling = NullValueHandling.Ignore)]
            public int IsSticker { get; set; }

            [JsonProperty("import_datetime", NullValueHandling = NullValueHandling.Ignore)]
            public string ImportDatetime { get; set; }

            [JsonProperty("trending_datetime", NullValueHandling = NullValueHandling.Ignore)]
            public string TrendingDatetime { get; set; }

            [JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
            public Images Images { get; set; }

            [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
            public string Title { get; set; }

            [JsonProperty("analytics", NullValueHandling = NullValueHandling.Ignore)]
            public Analytics Analytics { get; set; }

            [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
            public User User { get; set; }
        }

        public class Pagination
        {

            [JsonProperty("total_count", NullValueHandling = NullValueHandling.Ignore)]
            public int TotalCount { get; set; }

            [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
            public int Count { get; set; }

            [JsonProperty("offset", NullValueHandling = NullValueHandling.Ignore)]
            public int Offset { get; set; }
        }

        public class Meta
        {

            [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
            public int Status { get; set; }

            [JsonProperty("msg", NullValueHandling = NullValueHandling.Ignore)]
            public string Msg { get; set; }

            [JsonProperty("response_id", NullValueHandling = NullValueHandling.Ignore)]
            public string ResponseId { get; set; }
        }

    }
}
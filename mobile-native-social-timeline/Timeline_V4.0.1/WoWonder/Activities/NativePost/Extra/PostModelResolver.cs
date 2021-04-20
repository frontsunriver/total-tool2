using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Android.Gms.Maps.Model;
using Android.Icu.Util;
using Android.Locations;
using Android.OS;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Fonts;
using WoWonder.Helpers.Model;
using WoWonder.Helpers.Utils;
using WoWonderClient;
using WoWonderClient.Classes.Posts;
using Calendar = Android.Icu.Util.Calendar;
using Exception = System.Exception;

namespace WoWonder.Activities.NativePost.Extra
{
    public class PostModelResolver
    {
        private readonly Context MainContext;
        private readonly PostModelType PostFeedType;

        public PostModelResolver(Context mainContext, PostModelType postFeedType)
        {
            try
            {
                MainContext = mainContext;
                PostFeedType = postFeedType;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void PrepareHeader(PostDataObject item)
        {
            try
            {
                var time = item.Time;
                bool success = int.TryParse(time, out var number);
                item.Time = success ? Methods.Time.TimeAgo(number, false) : time;

                item.Name = Methods.FunString.DecodeString(item.Name);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void PrepareTextSection(PostDataObject item)
        {
            try
            {
                 item.Orginaltext = Methods.FunString.DecodeString(item.Orginaltext);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void PreparePostPrevBottom(PostDataObject item)
        {
            try
            {
                switch (AppSettings.PostButton)
                {
                    case PostButtonSystem.ReactionDefault:
                    case PostButtonSystem.ReactionSubShine:
                        item.PostLikes = item.Reaction?.Count == null ? "0" : item.Reaction?.Count.ToString();
                        break;
                    default:
                        item.PostLikes = Methods.FunString.FormatPriceValue(Convert.ToInt32(item.PostLikes));
                        break;
                }

                item.PrevButtonViewText = PostFeedType switch
                {
                    PostModelType.VideoPost => item.VideoViews + " " + MainContext.GetString(Resource.String.Lbl_Views),
                    PostModelType.PollPost => item.Options[0]?.All + " " + MainContext.GetString(Resource.String.Lbl_votesPost),
                    _ => item.PrevButtonViewText
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void PrepareBlog(PostDataObject item)
        {
            try
            {
                switch (string.IsNullOrEmpty(item.Blog?.Title))
                {
                    case false:
                    {
                        var prepareTitle = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.Blog?.Title, 60));
                        item.Blog.Title = prepareTitle;
                        break;
                    }
                }

                switch (string.IsNullOrEmpty(item.Blog?.Description))
                {
                    case false:
                    {
                        var prepareDescription = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.Blog?.Title, 120));
                        item.Blog.Description = prepareDescription;
                        break;
                    }
                }

                item.Blog.CategoryName = string.IsNullOrEmpty(item.Blog?.CategoryName) switch
                {
                    false => item.Blog?.CategoryName,
                    _ => item.Blog.CategoryName
                };
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void PrepareColorBox(PostDataObject item)
        {
            try
            {
                if (ListUtils.SettingsSiteList?.PostColors != null && ListUtils.SettingsSiteList?.PostColors.Value.PostColorsList != null)
                { 
                    var getColorObject = ListUtils.SettingsSiteList.PostColors.Value.PostColorsList.FirstOrDefault(a => a.Key == item.ColorId);

                    item.Orginaltext = Methods.FunString.DecodeString(item.Orginaltext);

                    if (getColorObject.Value != null)
                    {
                        switch (string.IsNullOrEmpty(getColorObject.Value.Image))
                        {
                            case false:
                                item.ColorBoxImageUrl = getColorObject.Value.Image;
                                break;
                            default:
                            {
                                var colorsList = new List<int>();

                                switch (string.IsNullOrEmpty(getColorObject.Value.Color1))
                                {
                                    case false:
                                        colorsList.Add(Color.ParseColor(getColorObject.Value.Color1));
                                        break;
                                }

                                switch (string.IsNullOrEmpty(getColorObject.Value.Color2))
                                {
                                    case false:
                                        colorsList.Add(Color.ParseColor(getColorObject.Value.Color2));
                                        break;
                                }

                                item.ColorBoxGradientDrawable = new GradientDrawable(GradientDrawable.Orientation.TopBottom, colorsList.ToArray());
                                item.ColorBoxGradientDrawable.SetCornerRadius(0f);
                                break;
                            }
                        }

                        item.ColorBoxTextColor = Color.ParseColor(getColorObject.Value.TextColor);
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void PrepareEvent(PostDataObject item)
        {
            try
            {
                switch (string.IsNullOrEmpty(item.Event?.EventClass?.Name))
                {
                    case false:
                    {
                        var prepareTitle = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.Event?.EventClass?.Name, 100));
                        item.Event.Value.EventClass.Name = prepareTitle;
                        break;
                    }
                }
                switch (string.IsNullOrEmpty(item.Event?.EventClass?.Description))
                {
                    case false:
                    {
                        var prepareDescription = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.Event?.EventClass?.Description, 100));
                        item.Event.Value.EventClass.Description = prepareDescription;
                        break;
                    }
                }

                switch (string.IsNullOrEmpty(item.Event?.EventClass?.Location))
                {
                    case false:
                    {
                        var prepareLocation = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.Event?.EventClass?.Location, 60));
                        item.Event.Value.EventClass.Location = prepareLocation;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void PrepareLink(PostDataObject item)
        {
            try
            { 
                switch (string.IsNullOrEmpty(item.PostLink))
                {
                    case false:
                    {
                        var prepareUrl = item.PostLink.Replace("https://", "").Replace("http://", "").Split('/').FirstOrDefault();
                        item.PostLink = prepareUrl;
                        break;
                    }
                }
                switch (string.IsNullOrEmpty(item.PostLinkContent))
                {
                    case false:
                    {
                        var prepareDescription = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.PostLinkContent, 100));
                        item.PostLinkContent = prepareDescription;
                        break;
                    }
                }
                switch (string.IsNullOrEmpty(item.PostLinkTitle))
                {
                    case false:
                    {
                        var prepareTitle = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.PostLinkTitle, 100));
                        item.PostLinkTitle = prepareTitle;
                        break;
                    }
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }
          
        public void PrepareTikTokVideo(PostDataObject item)
        {
            try
            { 
                switch (string.IsNullOrEmpty(item.PostLink))
                {
                    case false:
                    {
                        item.PostTikTok = item.PostLink;

                        var prepareUrl = item.PostLink.Replace("https://", "").Replace("http://", "").Split('/').FirstOrDefault();
                        item.PostLink = prepareUrl;
                        break;
                    }
                }

                item.PostLinkContent = string.Empty;

                switch (string.IsNullOrEmpty(item.PostLinkTitle))
                {
                    case false:
                    {
                        var prepareTitle = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.PostLinkTitle, 100));
                        item.PostLinkTitle = prepareTitle;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(item.PostLinkImage))
                {
                    item.PostLinkImage = "default_video_thumbnail.png";
                } 
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }
          
        public void PreparVimeoVideo(PostDataObject item)
        {
            try
            {
                switch (AppSettings.EmbedVimeoVideoPostType)
                {
                    case VideoPostTypeSystem.EmbedVideo:
                        item.PostLink = "https://player.vimeo.com/video/" + item.PostVimeo;
                        break;
                    case VideoPostTypeSystem.Link:
                        item.PostLink = "https://player.vimeo.com/video/" + item.PostVimeo;
                        item.PostLinkTitle = "vimeo.com";
                        item.PostLinkContent = "Vimeo" + " "  + MainContext.GetText(Resource.String.Lbl_PostLinkContentVideo);
                        item.PostLinkImage = "vimeo.png";
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }
         
        public void PrepareFacebookVideo(PostDataObject item)
        {
            try
            {
                switch (AppSettings.EmbedFacebookVideoPostType)
                {
                    case VideoPostTypeSystem.EmbedVideo:
                        item.PostLink = "https://www.facebook.com/video/embed?video_id=" + item.PostFacebook.Split("/videos/").Last();
                        break;
                    case VideoPostTypeSystem.Link:
                        item.PostLink = "https://www.facebook.com/video/embed?video_id=" + item.PostFacebook.Split("/videos/").Last();
                        item.PostLinkTitle = "facebook.com";
                        item.PostLinkContent = MainContext.GetText(Resource.String.Lbl_PostLinkContentFacebook);
                        item.PostLinkImage = "facebook.png";
                        break;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void PreparePlayTubeVideo(PostDataObject item)
        {
            try
            {
                switch (AppSettings.EmbedPlayTubeVideoPostType)
                {
                    case VideoPostTypeSystem.EmbedVideo:
                    {
                        var playTubeUrl = ListUtils.SettingsSiteList?.PlaytubeUrl; 
                        item.PostLink = playTubeUrl + "/embed/" + item.PostPlaytube;
                        break;
                    }
                    case VideoPostTypeSystem.Link:
                    {
                        var playTubeUrl = ListUtils.SettingsSiteList?.PlaytubeUrl;
                        item.PostLink = playTubeUrl + "/embed/" + item.PostPlaytube;
                     
                        item.PostLinkTitle = playTubeUrl.Replace("https://" , "").Replace("http://" , "");
                        item.PostLinkContent = item.PostLinkTitle + " "  + MainContext.GetText(Resource.String.Lbl_PostLinkContentVideo);
                        item.PostLinkImage = "default_video_thumbnail.png";
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        } 

        public void PrepareFunding(PostDataObject item)
        {
            try
            {
                if (item.FundData != null)
                {
                    switch (string.IsNullOrEmpty(item.FundData.Value.FundDataClass?.Title))
                    {
                        case false:
                        {
                            var prepareTitle = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.FundData.Value.FundDataClass?.Title, 100));
                            item.FundData.Value.FundDataClass.Title = prepareTitle;
                            break;
                        }
                    }

                    bool success = int.TryParse(item.FundData.Value.FundDataClass.Time, out var number);
                    switch (success)
                    {
                        case true:
                            Console.WriteLine("Converted '{0}' to {1}.", item.FundData.Value.FundDataClass.Time, number);
                            item.FundData.Value.FundDataClass.Time = Methods.Time.TimeAgo(number, false);
                            break;
                        default:
                            Console.WriteLine("Attempted conversion of '{0}' failed.", item.FundData.Value.FundDataClass.Time ?? "<null>");
                            item.FundData.Value.FundDataClass.Time = Methods.Time.ReplaceTime(item.FundData.Value.FundDataClass.Time);
                            break;
                    }

                    switch (string.IsNullOrEmpty(item.FundData.Value.FundDataClass?.Description))
                    {
                        case false:
                        {
                            var prepareDescription = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.FundData.Value.FundDataClass?.Description, 100));
                            item.FundData.Value.FundDataClass.Description = prepareDescription;
                            break;
                        }
                        default:
                        {
                            if (item.FundData.Value.FundDataClass != null)
                                item.FundData.Value.FundDataClass.Description = MainContext.GetText(Resource.String.Lbl_NoAnyDescription);
                            break;
                        }
                    }

                    try
                    {
                        item.FundData.Value.FundDataClass.Raised = item.FundData.Value.FundDataClass.Raised.Replace(AppSettings.CurrencyFundingPriceStatic, "");
                        item.FundData.Value.FundDataClass.Amount = item.FundData.Value.FundDataClass.Amount.Replace(AppSettings.CurrencyFundingPriceStatic, "");

                        decimal d = decimal.Parse(item.FundData.Value.FundDataClass.Raised, CultureInfo.InvariantCulture);
                        item.FundData.Value.FundDataClass.Raised = AppSettings.CurrencyFundingPriceStatic + d.ToString("0.00");

                        decimal amount = decimal.Parse(item.FundData.Value.FundDataClass.Amount, CultureInfo.InvariantCulture);
                        item.FundData.Value.FundDataClass.Amount = AppSettings.CurrencyFundingPriceStatic + amount.ToString("0.00");
                    }
                    catch (Exception exception)
                    {
                        item.FundData.Value.FundDataClass.Raised = AppSettings.CurrencyFundingPriceStatic + item.FundData.Value.FundDataClass.Raised;
                        item.FundData.Value.FundDataClass.Amount = AppSettings.CurrencyFundingPriceStatic + item.FundData.Value.FundDataClass.Amount;
                        Methods.DisplayReportResultTrack(exception);
                    }

                    if (item.FundData.Value.FundDataClass.Bar != null && item.FundData.Value.FundDataClass.Bar.Value > 0)
                    {
                        item.FundData.Value.FundDataClass.Bar = item.FundData.Value.FundDataClass.Bar;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }
        
        public void PreparePurpleFundPost(PostDataObject item)
        {
            try
            {
                if (item?.Fund?.PurpleFund?.Fund != null)
                {
                    switch (string.IsNullOrEmpty(item?.Fund?.PurpleFund?.Fund?.Title))
                    {
                        case false:
                        {
                            var prepareTitle = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.Fund.Value.PurpleFund.Fund?.Title, 100));
                            item.Fund.Value.PurpleFund.Fund.Title = prepareTitle;
                            break;
                        }
                    }
                     
                    bool success = int.TryParse(item?.Fund?.PurpleFund?.Fund.Time, out var number);
                    switch (success)
                    {
                        case true:
                            Console.WriteLine("Converted '{0}' to {1}.", item?.Fund?.PurpleFund?.Fund.Time, number);
                            item.Fund.Value.PurpleFund.Fund.Time = Methods.Time.TimeAgo(number, false);
                            break;
                        default:
                            Console.WriteLine("Attempted conversion of '{0}' failed.", item?.Fund?.PurpleFund?.Fund.Time ?? "<null>");
                            item.Fund.Value.PurpleFund.Fund.Time = Methods.Time.ReplaceTime(item?.Fund?.PurpleFund?.Fund.Time);
                            break;
                    }
                     
                    switch (string.IsNullOrEmpty(item?.Fund?.PurpleFund?.Fund?.Description))
                    {
                        case false:
                        {
                            var prepareDescription = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item?.Fund?.PurpleFund?.Fund?.Description, 100));
                            item.Fund.Value.PurpleFund.Fund.Description = prepareDescription;
                            break;
                        }
                        default:
                        {
                            if (item?.Fund?.PurpleFund?.Fund != null)
                                item.Fund.Value.PurpleFund.Fund.Description = MainContext.GetText(Resource.String.Lbl_NoAnyDescription);
                            break;
                        }
                    }

                    try
                    {
                        item.Fund.Value.PurpleFund.Fund.Raised = item.Fund.Value.PurpleFund.Fund.Raised.Replace(AppSettings.CurrencyFundingPriceStatic, "");
                        item.Fund.Value.PurpleFund.Fund.Amount = item.Fund.Value.PurpleFund.Fund.Amount.Replace(AppSettings.CurrencyFundingPriceStatic, "");

                        decimal d = decimal.Parse(item.Fund.Value.PurpleFund.Fund.Raised, CultureInfo.InvariantCulture);
                        item.Fund.Value.PurpleFund.Fund.Raised = AppSettings.CurrencyFundingPriceStatic + d.ToString("0.00");

                        decimal amount = decimal.Parse(item.Fund.Value.PurpleFund.Fund.Amount, CultureInfo.InvariantCulture);
                        item.Fund.Value.PurpleFund.Fund.Amount = AppSettings.CurrencyFundingPriceStatic + amount.ToString("0.00");
                    }
                    catch (Exception exception)
                    {
                        item.Fund.Value.PurpleFund.Fund.Raised = AppSettings.CurrencyFundingPriceStatic + item.Fund.Value.PurpleFund.Fund.Raised;
                        item.Fund.Value.PurpleFund.Fund.Amount = AppSettings.CurrencyFundingPriceStatic + item.Fund.Value.PurpleFund.Fund.Amount;
                        Methods.DisplayReportResultTrack(exception);
                    }

                    if (item.Fund.Value.PurpleFund.Fund.Bar != null && item.Fund.Value.PurpleFund.Fund.Bar.Value > 0)
                    {
                        item.Fund.Value.PurpleFund.Fund.Bar = item.Fund.Value.PurpleFund.Fund.Bar.Value;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void PrepareProduct(PostDataObject item)
        { 
            try
            {
                switch (item.Product?.ProductClass?.Seller)
                {
                    case null:
                    {
                        if (item.Product != null)
                            item.Product.Value.ProductClass.Seller = item.Publisher;
                        break;
                    }
                }

                if (item.Product?.ProductClass != null)
                {
                    switch (string.IsNullOrEmpty(item.Product.Value.ProductClass?.Location))
                    {
                        case false:
                        {
                            item.Product.Value.ProductClass.LocationDecodedText = item.Product.Value.ProductClass?.Location;
                            var prepareLocation = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.Product.Value.ProductClass?.Location, 100));
                            item.Product.Value.ProductClass.LocationDecodedText = prepareLocation;
                            break;
                        }
                    }

                    switch (string.IsNullOrEmpty(item.Product.Value.ProductClass?.Name))
                    {
                        case false:
                        {
                            var prepareTitle = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.Product.Value.ProductClass?.Name, 100));
                            item.Product.Value.ProductClass.Name = prepareTitle;
                            break;
                        }
                    }

                    switch (string.IsNullOrEmpty(item.Product.Value.ProductClass?.Description))
                    {
                        case false:
                        {
                            var prepareDescription = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.Product.Value.ProductClass?.Description, 100));
                            item.Product.Value.ProductClass.Description = prepareDescription;
                            break;
                        }
                    }

                    var (currency, currencyIcon) = WoWonderTools.GetCurrency(item.Product.Value.ProductClass?.Currency);
                    Console.WriteLine(currency);
                    if (item.Product.Value.ProductClass != null)
                    {
                        item.Product.Value.ProductClass.CurrencyText = item.Product.Value.ProductClass?.Price + " " + currencyIcon;
                        item.Product.Value.ProductClass.TypeDecodedText = MainContext.GetString(item.Product.Value.ProductClass?.Type == "0" ? Resource.String.Radio_New : Resource.String.Radio_Used); 
                        item.Product.Value.ProductClass.StatusDecodedText = item.Product.Value.ProductClass?.Status == "0" ? MainContext.GetString(Resource.String.Lbl_In_Stock) : " "; 
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void PrepareOffer(PostDataObject item)
        {
            try
            {
                switch (string.IsNullOrEmpty(item.Offer?.OfferClass?.OfferText))
                {
                    case false:
                    {
                        var prepareOfferText = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.Offer?.OfferClass?.OfferText, 100));
                        item.Offer.Value.OfferClass.OfferText = prepareOfferText;
                        break;
                    }
                }

                switch (string.IsNullOrEmpty(item.Offer?.OfferClass?.Description))
                {
                    case false:
                    {
                        var prepareDescription = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.Offer?.OfferClass?.Description, 100));
                        item.Offer.Value.OfferClass.Description = prepareDescription;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public async void PrepareMapPost(PostDataObject item)
        {
            try
            {
                switch (item.PostMap.Contains("https://maps.googleapis.com/maps/api/staticmap?"))
                {
                    case false:
                    {
                        string imageUrlMap = "https://maps.googleapis.com/maps/api/staticmap?";
                        //imageUrlMap += "center=" + item.CurrentLatitude + "," + item.CurrentLongitude;
                        imageUrlMap += "center=" + item.PostMap.Replace("/", "");
                        imageUrlMap += "&zoom=10";
                        imageUrlMap += "&scale=1";
                        imageUrlMap += "&size=300x300";
                        imageUrlMap += "&maptype=roadmap";
                        imageUrlMap += "&key=" + MainContext.GetText(Resource.String.google_maps_key);
                        imageUrlMap += "&format=png";
                        imageUrlMap += "&visual_refresh=true";
                        imageUrlMap += "&markers=size:small|color:0xff0000|label:1|" + item.PostMap.Replace("/", "");

                        item.ImageUrlMap = imageUrlMap;
                        break;
                    }
                    default:
                        item.ImageUrlMap = item.PostMap;
                        break;
                }

                var latLng = await GetLocationFromAddress(item.PostMap).ConfigureAwait(false);
                if (latLng != null)
                {
                    item.CurrentLatitude = latLng.Latitude;
                    item.CurrentLongitude = latLng.Longitude;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        #region Location >> BindMapPost

        private async Task<LatLng> GetLocationFromAddress(string strAddress)
        {
            #pragma warning disable 618
            var locale = (int)Build.VERSION.SdkInt < 25 ? MainContext.Resources?.Configuration?.Locale : MainContext.Resources?.Configuration?.Locales.Get(0) ?? MainContext.Resources?.Configuration?.Locale;
            #pragma warning restore 618
            Geocoder coder = new Geocoder(MainContext, locale);

            try
            {
                var address = await coder.GetFromLocationNameAsync(strAddress, 2);
                switch (address?.Count)
                {
                    case > 0:
                        return null!;
                }

                Address location = address[0];
                var lat = location.Latitude;
                var lng = location.Longitude;

                LatLng p1 = new LatLng(lat, lng);

                return p1;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
                return null!;
            }
        }

        #endregion

        public void PrepareAds(PostDataObject item)
        {
            try
            {
                if (item.UserData != null)
                {
                    item.Name = Methods.FunString.DecodeString(item.Name);
                    item.Posted = Methods.Time.TimeAgo(Convert.ToInt32(item.Posted), false);

                    item.Location = Methods.FunString.DecodeString(item.Location);
                    item.Headline = Methods.FunString.DecodeString(item.Headline);
                    item.Url = item.Url?.Replace("https://", "")?.Replace("http://", "")?.Split('/')?.FirstOrDefault() ?? "";
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void PrepareJob(PostDataObject item)
        {
            try
            {
                if (item.Job != null)
                {
                    item.Job.Value.JobInfoClass.Image =
                        item.Job.Value.JobInfoClass.Image.Contains(Client.WebsiteUrl) switch
                        {
                            false => WoWonderTools.GetTheFinalLink(item.Job.Value.JobInfoClass.Image),
                            _ => item.Job.Value.JobInfoClass.Image
                        };

                    item.Job.Value.JobInfoClass.IsOwner = item.Job.Value.JobInfoClass.UserId == UserDetails.UserId;
                     
                    switch (string.IsNullOrEmpty(item.Job.Value.JobInfoClass.Title))
                    {
                        case false:
                        {
                            var prepareTitle = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.Job.Value.JobInfoClass.Title, 100));
                            item.Job.Value.JobInfoClass.Title = prepareTitle;
                            break;
                        }
                    }
                    switch (string.IsNullOrEmpty(item.Job?.JobInfoClass.Description))
                    {
                        case false:
                        {
                            var prepareDescription = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.Job?.JobInfoClass.Description, 200));
                            item.Job.Value.JobInfoClass.Description = prepareDescription;
                            break;
                        }
                    }

                    if (item.Job.Value.JobInfoClass.Page != null)
                    {
                        var prepareName = "@" + Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.Job.Value.JobInfoClass.Page.PageName, 100));
                        item.Job.Value.JobInfoClass.Page.PageName = prepareName;

                        if (item.Job.Value.JobInfoClass.Page.IsPageOnwer != null && item.Job.Value.JobInfoClass.Page.IsPageOnwer.Value)
                            item.Job.Value.JobInfoClass.ButtonText = MainContext.GetString(Resource.String.Lbl_show_applies) + " (" + item.Job.Value.JobInfoClass.ApplyCount + ")";
                    }

                    switch (item.Job.Value.JobInfoClass.Apply)
                    {
                        case "true":
                        {
                            var prepare = MainContext.GetString(Resource.String.Lbl_already_applied);
                            item.Job.Value.JobInfoClass.ButtonText = prepare;
                            break;
                        }
                        default:
                        {
                            if (item.Job.Value.JobInfoClass.Apply != "true" && item.Job.Value.JobInfoClass.Page.IsPageOnwer != null && !item.Job.Value.JobInfoClass.Page.IsPageOnwer.Value)
                            {
                                var prepare = MainContext.GetString(Resource.String.Lbl_apply_now);
                                item.Job.Value.JobInfoClass.ButtonText = prepare; 
                            }

                            break;
                        }
                    }

                    switch (string.IsNullOrEmpty(item.Job.Value.JobInfoClass.Time))
                    {
                        case false:
                        {
                            var prepareTime = Methods.Time.TimeAgo(Convert.ToInt32(item.Job.Value.JobInfoClass.Time), false);
                            item.Job.Value.JobInfoClass.Time = prepareTime;
                            break;
                        }
                    }

                    switch (string.IsNullOrEmpty(item.Job.Value.JobInfoClass.Location))
                    {
                        case false:
                        {
                            item.Job.Value.JobInfoClass.LocationDecodedText = item.Product?.ProductClass?.Location;
                            var prepareLocation = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(item.Product?.ProductClass?.Location, 100));
                            item.Job.Value.JobInfoClass.LocationDecodedText = prepareLocation;
                            break;
                        }
                    }

                    switch (string.IsNullOrEmpty(item.Job.Value.JobInfoClass.SalaryDate))
                    {
                        //Set Salary Date
                        case false:
                        {
                            var salaryDate = item.Job.Value.JobInfoClass.SalaryDate switch
                            {
                                "per_hour" => MainContext.GetString(Resource.String.Lbl_per_hour),
                                "per_day" => MainContext.GetString(Resource.String.Lbl_per_day),
                                "per_week" => MainContext.GetString(Resource.String.Lbl_per_week),
                                "per_month" => MainContext.GetString(Resource.String.Lbl_per_month),
                                "per_year" => MainContext.GetString(Resource.String.Lbl_per_year),
                                _ => MainContext.GetString(Resource.String.Lbl_Unknown)
                            };

                            item.Job.Value.JobInfoClass.SalaryDate = salaryDate;
                            break;
                        }
                    }

                    switch (string.IsNullOrEmpty(item.Job.Value.JobInfoClass.JobType))
                    {
                        case false:
                        {
                            var jobInfo = item.Job.Value.JobInfoClass.JobType switch
                            {
                                //Set job type
                                "full_time" => IonIconsFonts.IosBriefcase + " " + MainContext.GetString(Resource.String.Lbl_full_time),
                                "part_time" => IonIconsFonts.IosBriefcase + " " + MainContext.GetString(Resource.String.Lbl_part_time),
                                "internship" => IonIconsFonts.IosBriefcase + " " + MainContext.GetString(Resource.String.Lbl_internship),
                                "volunteer" => IonIconsFonts.IosBriefcase + " " + MainContext.GetString(Resource.String.Lbl_volunteer),
                                "contract" => IonIconsFonts.IosBriefcase + " " + MainContext.GetString(Resource.String.Lbl_contract),
                                _ => IonIconsFonts.IosBriefcase + " " + MainContext.GetString(Resource.String.Lbl_Unknown)
                            };

                            item.Job.Value.JobInfoClass.JobType = jobInfo;
                            break;
                        }
                    }


                    var categoryName = CategoriesController.ListCategoriesJob.FirstOrDefault(categories => categories.CategoriesId == item.Job.Value.JobInfoClass.Category)?.CategoriesName;
                    item.Job.Value.JobInfoClass.Category += " " + " " + IonIconsFonts.Pricetag + " " + categoryName;

                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public void PreparePoll(PollsOptionObject poll)
        {
            try
            {
                switch (string.IsNullOrEmpty(poll.Text))
                {
                    case false:
                    {
                        var prepareText = Methods.FunString.DecodeString(Methods.FunString.SubStringCutOf(poll.Text, 100));
                        poll.Text = prepareText;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
            }
        }

        public AdapterModelsClass PrepareGreetingAlert()
        {
            try
            {
                //afternoon_system
                var afternoonSystem = ListUtils.SettingsSiteList?.AfternoonSystem;
                switch (afternoonSystem)
                {
                    case "1":
                    {
                        var data = ListUtils.MyProfileList?.FirstOrDefault();
                        string name = data != null ? WoWonderTools.GetNameFinal(data) : UserDetails.Username;

                        var alertModel = new AlertModelClass();
                     
                        switch ((int)Build.VERSION.SdkInt)
                        {
                            case >= 24:
                            {
#pragma warning disable 618
                                var locale = (int)Build.VERSION.SdkInt < 25 ? MainContext?.Resources?.Configuration?.Locale : MainContext?.Resources?.Configuration?.Locales?.Get(0) ?? MainContext?.Resources?.Configuration?.Locale;
#pragma warning restore 618

                                var c = Calendar.GetInstance(locale);
                                var timeOfDay = c?.Get(CalendarField.HourOfDay);

                                switch (timeOfDay)
                                {
                                    case >= 0 and < 12:
                                        alertModel = new AlertModelClass
                                        {
                                            TitleHead = MainContext.GetString(Resource.String.Lbl_GoodMorning) + ", " + name,
                                            SubText = MainContext.GetString(Resource.String.Lbl_GoodMorning_Text),
                                            LinerColor = "#ffc107",
                                            ImageDrawable = Resource.Drawable.ic_post_park
                                        };
                                        break;
                                    case >= 12 and < 16:
                                        alertModel = new AlertModelClass
                                        {
                                            TitleHead = MainContext.GetString(Resource.String.Lbl_GoodAfternoon) + ", " + name,
                                            SubText = MainContext.GetString(Resource.String.Lbl_GoodAfternoon_Text),
                                            LinerColor = "#ffc107",
                                            ImageDrawable = Resource.Drawable.ic_post_desert
                                        };
                                        break;
                                    case >= 16 and < 21:
                                    case >= 21 and < 24:
                                        alertModel = new AlertModelClass
                                        {
                                            TitleHead = MainContext.GetString(Resource.String.Lbl_GoodEvening) + ", " + name,
                                            SubText = MainContext.GetString(Resource.String.Lbl_GoodEvening_Text),
                                            LinerColor = "#ffc107",
                                            ImageDrawable = Resource.Drawable.ic_post_sea
                                        };
                                        break;
                                }

                                var alertBox = new AdapterModelsClass
                                {
                                    TypeView = PostModelType.AlertBox,
                                    AlertModel = alertModel,
                                    Id = 333333333
                                };
                                return alertBox;
                            }
                        }

                        break;
                    }
                }
                return null!;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
                return null!;
            }
        }

        public AdapterModelsClass SetFindMoreAlert(string type)
        {
            try
            {
                var alertModel1 = type switch
                {
                    "Groups" => new AlertModelClass
                    {
                        TitleHead = MainContext.GetString(Resource.String.Lbl_Groups),
                        SubText = MainContext.GetString(Resource.String.Lbl_FindMoreAler_TextGroups),
                        TypeAlert = "Groups",
                        ImageDrawable = Resource.Drawable.image2,
                        IconImage = Resource.Drawable.shareGroup,
                    },
                    "Pages" => new AlertModelClass
                    {
                        TitleHead = MainContext.GetString(Resource.String.Lbl_Pages),
                        SubText = MainContext.GetString(Resource.String.Lbl_FindMoreAler_TextPages),
                        TypeAlert = "Pages",
                        ImageDrawable = Resource.Drawable.image1,
                        IconImage = Resource.Drawable.sharePage,
                    },
                    _ => new AlertModelClass()
                };

                var addAlertJoinBox = new AdapterModelsClass
                {
                    TypeView = PostModelType.AlertJoinBox,
                    Id = DateTime.Now.Millisecond,
                    AlertModel = alertModel1
                };

                return addAlertJoinBox;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e); 
                return null!;
            }
        }
    }
}
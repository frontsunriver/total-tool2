using System;
using System.Linq;
using Android.Content;
using Android.Graphics; 
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using WoWonder.Activities.NativePost.Post;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Global;
using WoWonderClient.Classes.Posts;
using Exception = System.Exception;

namespace WoWonder.Helpers.Fonts
{
    public class WoTextDecorator
    {
        public string Content;
        public SpannableString DecoratedContent;

        #region old
          
        //public SpannableString SetupStrings(PostDataObject item, Context mainContext)
        //{
        //    try
        //    { 
        //        if (item == null)
        //            return null!;

        //        UserDataObject publisher = item.Publisher ?? item.UserData;

        //        var username = Methods.FunString.DecodeString(WoWonderTools.GetNameFinal(publisher));
        //        var textHighLighter = username;
        //        var textIsPro = string.Empty;
        //        var textFeelings = string.Empty;
        //        var textTraveling = string.Empty;
        //        var textWatching = string.Empty;
        //        var textPlaying = string.Empty;
        //        var textListening = string.Empty;
        //        var textProduct = string.Empty;
        //        var textArticle = string.Empty;
        //        var textEvent = string.Empty;
        //        var textPurpleFund = string.Empty;
        //        var textOffer = string.Empty;
        //        var textFundData = string.Empty;
        //        var textLocation = string.Empty;
        //        var textAlbumName = string.Empty;
        //        var textImageChange = string.Empty;
        //        var textShare = string.Empty;
        //        var textGroupRecipient = string.Empty;
        //        var textUserRecipient = string.Empty;

        //        if (publisher.Verified == "1")
        //            textHighLighter += " " + IonIconsFonts.CheckmarkCircle;

        //        if (publisher.IsPro == "1")
        //        {
        //            textIsPro = " " + IonIconsFonts.Flash;
        //            textHighLighter += textIsPro;
        //        }

        //        if (!string.IsNullOrEmpty(item.PostFeeling) && item.SharedInfo.SharedInfoClass == null)
        //        {
        //            textFeelings = " " + mainContext.GetString(Resource.String.Lbl_IsFeeling) + " " + PostFunctions.GetFeelingTypeIcon(item.PostFeeling) + " " + PostFunctions.GetFeelingTypeTextString(item.PostFeeling, mainContext);
        //            textHighLighter += textFeelings;
        //        }

        //        if (!string.IsNullOrEmpty(item.PostTraveling))
        //        {
        //            textTraveling = "  " + IonIconsFonts.Plane + " " + mainContext.GetText(Resource.String.Lbl_IsTravelingTo) + " " + item.PostTraveling;
        //            textHighLighter += textTraveling;
        //        }

        //        if (!string.IsNullOrEmpty(item.PostWatching))
        //        {
        //            textWatching = "  " + IonIconsFonts.Eye + " " + mainContext.GetText(Resource.String.Lbl_IsWatching) + " " + item.PostWatching;
        //            textHighLighter += textWatching;
        //        }

        //        if (!string.IsNullOrEmpty(item.PostPlaying))
        //        {
        //            textPlaying = "  " + IonIconsFonts.LogoGameControllerB + " " + mainContext.GetText(Resource.String.Lbl_IsPlaying) + " " + item.PostPlaying;
        //            textHighLighter += textPlaying;
        //        }

        //        if (!string.IsNullOrEmpty(item.PostListening))
        //        {
        //            textListening = "  " + IonIconsFonts.Headphone + " " + mainContext.GetText(Resource.String.Lbl_IsListeningTo) + " " + item.PostListening;
        //            textHighLighter += textListening;
        //        }

        //        if (!string.IsNullOrEmpty(item.PostMap))
        //        {
        //            textLocation = "  " + IonIconsFonts.Pin + " " + item.PostMap.Replace("/", "");
        //            textHighLighter += textLocation;
        //        }

        //        if (item.PostType == "profile_cover_picture" && item.SharedInfo.SharedInfoClass == null)
        //        {
        //            textImageChange += " " + mainContext.GetText(Resource.String.Lbl_ChangedProfileCover) + " ";
        //            textHighLighter += textImageChange;
        //        }

        //        if (item.PostType == "profile_picture" && item.SharedInfo.SharedInfoClass == null)
        //        {
        //            textImageChange += " " + mainContext.GetText(Resource.String.Lbl_ChangedProfilePicture) + " ";
        //            textHighLighter += textImageChange;
        //        }

        //        if (!string.IsNullOrEmpty(item.PostType) && item.PostType == "live" && !string.IsNullOrEmpty(item.StreamName))
        //        {
        //            textImageChange += " " + mainContext.GetText(Resource.String.Lbl_WasLive) + " ";
        //            textHighLighter += textImageChange;
        //        }

        //        if (item.Product != null && item.SharedInfo.SharedInfoClass == null)
        //        {
        //            textProduct = " " + mainContext.GetText(Resource.String.Lbl_AddedNewProductForSell) + " ";
        //            textHighLighter += textProduct;
        //        }

        //        if (item.Blog != null && item.SharedInfo.SharedInfoClass == null)
        //        {
        //            textArticle = " " + mainContext.GetText(Resource.String.Lbl_CreatedNewArticle) + " ";
        //            textHighLighter += textArticle;
        //        }

        //        if (item.Event?.EventClass != null && item.SharedInfo.SharedInfoClass == null)
        //        {
        //            textEvent = "  " + IonIconsFonts.ArrowRightC + " " + mainContext.GetText(Resource.String.Lbl_CreatedNewEvent) + " "; 
        //            textHighLighter += textEvent;
        //        }

        //        if (!string.IsNullOrEmpty(item.AlbumName) && item.AlbumName != null && item.SharedInfo.SharedInfoClass == null)
        //        {
        //            textAlbumName = " " + mainContext.GetText(Resource.String.Lbl_AddedNewPhotosTo) + " " + Methods.FunString.DecodeString(item.AlbumName);
        //            textHighLighter += textAlbumName;
        //        }

        //        if (item.FundData?.FundDataClass != null && item.SharedInfo.SharedInfoClass == null)
        //        {
        //            textFundData = " " + mainContext.GetText(Resource.String.Lbl_CreatedNewFund) + " ";
        //            textHighLighter += textFundData;
        //        }

        //        if (item.Fund?.PurpleFund != null && item.SharedInfo.SharedInfoClass == null)
        //        {
        //            textPurpleFund = " " + mainContext.GetText(Resource.String.Lbl_DonatedRequestFund) + " ";
        //            textHighLighter += textPurpleFund;
        //        }

        //        if (item.Offer != null && item.PostType == "offer")
        //        {
        //            textOffer = " " + mainContext.GetText(Resource.String.Lbl_OfferPostAdded) + " ";
        //            textHighLighter += textOffer;
        //        }

        //        if (item.ParentId != "0")
        //        {
        //            textShare += " " + mainContext.GetText(Resource.String.Lbl_SharedPost) + " ";

        //            //if (item.SharedInfo.SharedInfoClass?.Publisher != null)
        //            //    textShare += WoWonderTools.GetNameFinal(item.SharedInfo.SharedInfoClass.Publisher);

        //            textHighLighter += textShare;
        //        }

        //        if (item.GroupRecipientExists != null && item.GroupRecipientExists.Value && !string.IsNullOrEmpty(item.GroupRecipient.GroupName))
        //        {
        //            textUserRecipient += " " + IonIconsFonts.ArrowRightC + " " + Methods.FunString.DecodeString(item.GroupRecipient.GroupName);
        //            textHighLighter += textUserRecipient;
        //        }
        //        else if (item.RecipientExists != null && item.RecipientExists.Value && item.Recipient.RecipientClass != null) 
        //        {
        //            textUserRecipient += " " + IonIconsFonts.ArrowRightC + " " + Methods.FunString.DecodeString(WoWonderTools.GetNameFinal(item.Recipient.RecipientClass));
        //            textHighLighter += textUserRecipient;
        //        }

        //        Content = textHighLighter;
        //        DecoratedContent = new SpannableString(textHighLighter);

        //        SetTextStyle(username, TypefaceStyle.Bold);

        //        if (publisher.Verified == "1")
        //            SetTextColor(IonIconsFonts.CheckmarkCircle, "#55acee");

        //        if (publisher.IsPro == "1")
        //            SetTextColor(textIsPro, "#888888");

        //        if (!string.IsNullOrEmpty(item.PostFeeling))
        //            SetTextColor(textFeelings, "#888888");

        //        if (!string.IsNullOrEmpty(item.PostTraveling))
        //        {
        //            SetTextColor(textTraveling, "#888888");
        //            SetTextColor(IonIconsFonts.Plane, "#3F51B5");
        //            SetRelativeSize(IonIconsFonts.Plane, 1.3f);
        //        }

        //        if (!string.IsNullOrEmpty(item.PostWatching))
        //        {
        //            SetTextColor(textWatching, "#888888");
        //            SetTextColor(IonIconsFonts.Eye, "#E91E63");
        //            SetRelativeSize(IonIconsFonts.Eye, 1.3f);
        //        }

        //        if (!string.IsNullOrEmpty(item.PostPlaying))
        //        {
        //            SetTextColor(textPlaying, "#888888");
        //            SetTextColor(IonIconsFonts.LogoGameControllerB, "#FF9800");
        //            SetRelativeSize(IonIconsFonts.LogoGameControllerB, 1.3f);
        //        }

        //        if (!string.IsNullOrEmpty(item.PostListening))
        //        {
        //            SetTextColor(textListening, "#888888");
        //            SetTextColor(IonIconsFonts.Headphone, "#03A9F4");
        //            SetRelativeSize(IonIconsFonts.Headphone, 1.3f);
        //        }

        //        if (!string.IsNullOrEmpty(item.PostMap))
        //        {
        //            SetTextColor(textLocation, "#888888");
        //            SetTextColor(IonIconsFonts.Pin, "#E91E63");
        //            SetRelativeSize(IonIconsFonts.Pin, 1.3f);
        //        }

        //        if (item.PostType == "profile_cover_picture" || item.PostType == "profile_picture")
        //            SetTextColor(textImageChange, "#888888");

        //        if (item.Product != null)
        //            SetTextColor(textProduct, "#888888");

        //        if (item.Blog != null)
        //            SetTextColor(textArticle, "#888888");

        //        if (item.Event?.EventClass != null)
        //        {
        //            SetTextColor(textEvent, "#888888");
        //            SetRelativeSize(IonIconsFonts.ArrowRightC, 1.0f);
        //        }

        //        if (item.FundData?.FundDataClass != null)
        //            SetTextColor(textFundData, "#888888");

        //        if (item.Fund?.PurpleFund != null)
        //            SetTextColor(textPurpleFund, "#888888");

        //        if (item.Offer.HasValue && item.PostType == "offer")
        //            SetTextColor(textOffer, "#888888");

        //        if (item.ParentId != "0")
        //            SetTextColor(textShare, "#888888");

        //        if (item.GroupRecipientExists != null && item.GroupRecipientExists.Value)
        //        {
        //            SetTextColor(textShare, "#888888");
        //            SetTextStyle(textGroupRecipient, TypefaceStyle.Bold);
        //            SetRelativeSize(IonIconsFonts.ArrowRightC, 1.0f);
        //        }  
        //        else if (item.RecipientExists != null && item.RecipientExists.Value)
        //        {
        //            SetTextStyle(textUserRecipient, TypefaceStyle.Bold);
        //            SetRelativeSize(IonIconsFonts.ArrowRightC, 1.0f);
        //        }

        //        if (!string.IsNullOrEmpty(item.AlbumName) && item.AlbumName != null)
        //            SetTextColor(textAlbumName, "#888888");

        //        return DecoratedContent;
        //    }
        //    catch (Exception e)
        //    {
        //        Methods.DisplayReportResultTrack(e);
        //        return DecoratedContent;
        //    }
        //}

        #endregion

        public SpannableString SetupStrings(PostDataObject item, Context mainContext)
        {
            try
            {
                switch (item)
                {
                    case null:
                        return null!;
                }

                UserDataObject publisher = item.Publisher ?? item.UserData;

                var username = Methods.FunString.DecodeString(WoWonderTools.GetNameFinal(publisher));
                var textHighLighter = username;
                string textEvent;
                string textImageChange = string.Empty, textShare = string.Empty;

                switch (publisher.Verified)
                {
                    case "1":
                        textHighLighter += " " + "[img src=icon_checkmark_small_vector/]";
                        break;
                } 

                switch (publisher.IsPro)
                {
                    case "1":
                    {
                        var textIsPro = " " + "[img src=post_icon_flash/]";
                        textHighLighter += textIsPro;
                        break;
                    }
                }

                switch (string.IsNullOrEmpty(item.PostFeeling))
                {
                    case false when item.SharedInfo.SharedInfoClass == null:
                    {
                        var textFeelings = " " + mainContext.GetString(Resource.String.Lbl_IsFeeling) + " " + PostFunctions.GetFeelingTypeIcon(item.PostFeeling) + " " + PostFunctions.GetFeelingTypeTextString(item.PostFeeling, mainContext);
                        textHighLighter += textFeelings;
                        break;
                    }
                }
                 
                switch (string.IsNullOrEmpty(item.PostTraveling))
                {
                    case false:
                    {
                        var textTraveling = "  " + "[img src=post_icon_airplane/]" + " " + mainContext.GetText(Resource.String.Lbl_IsTravelingTo) + " " + item.PostTraveling;
                        textHighLighter += textTraveling;
                        break;
                    }
                }

                switch (string.IsNullOrEmpty(item.PostWatching))
                {
                    case false:
                    {
                        var textWatching = "  " + "[img src=post_icon_eye/]" + " " + mainContext.GetText(Resource.String.Lbl_IsWatching) + " " + item.PostWatching;
                        textHighLighter += textWatching;
                        break;
                    }
                }
                 
                switch (string.IsNullOrEmpty(item.PostPlaying))
                {
                    case false:
                    {
                        var textPlaying = "  " + "[img src=post_icon_game_controller_b/]" + " " + mainContext.GetText(Resource.String.Lbl_IsPlaying) + " " + item.PostPlaying;
                        textHighLighter += textPlaying;
                        break;
                    }
                }

                switch (string.IsNullOrEmpty(item.PostListening))
                {
                    case false:
                    {
                        var textListening = "  " + "[img src=post_icon_musical_notes/]" + " " + mainContext.GetText(Resource.String.Lbl_IsListeningTo) + " " + item.PostListening;
                        textHighLighter += textListening;
                        break;
                    }
                }

                switch (string.IsNullOrEmpty(item.PostMap))
                {
                    case false:
                    {
                        var textLocation = "  " + "[img src=post_icon_pin/]" + " " + item.PostMap.Replace("/", "");
                        textHighLighter += textLocation;
                        break;
                    }
                }

                switch (item.PostType)
                {
                    case "profile_cover_picture" when item.SharedInfo.SharedInfoClass == null:
                        textImageChange += " " + mainContext.GetText(Resource.String.Lbl_ChangedProfileCover) + " ";
                        textHighLighter += textImageChange;
                        break;
                    case "profile_picture" when item.SharedInfo.SharedInfoClass == null:
                        textImageChange += " " + mainContext.GetText(Resource.String.Lbl_ChangedProfilePicture) + " ";
                        textHighLighter += textImageChange;
                        break;
                }

                if (!string.IsNullOrEmpty(item.PostType) && item.PostType == "live" && !string.IsNullOrEmpty(item.StreamName))
                {
                    if (ListUtils.SettingsSiteList?.AgoraLiveVideo is 1 && !string.IsNullOrEmpty(ListUtils.SettingsSiteList?.AgoraAppId))
                    {
                        if (item?.LiveTime != null && item?.LiveTime.Value > 0 && item.IsStillLive != null && item.IsStillLive.Value && string.IsNullOrEmpty(item?.AgoraResourceId) && string.IsNullOrEmpty(item?.PostFile)) //Live
                        {
                            textImageChange += " " + mainContext.GetText(Resource.String.Lbl_IsLiveNow) + " ";
                        }
                        else if (item?.LiveTime != null && item?.LiveTime.Value > 0 && !string.IsNullOrEmpty(item?.AgoraResourceId) && !string.IsNullOrEmpty(item?.PostFile)) //Saved
                        {
                            textImageChange += " " + mainContext.GetText(Resource.String.Lbl_WasLive) + " ";
                        }
                        else //End
                        {
                            textImageChange += " " + mainContext.GetText(Resource.String.Lbl_WasLive) + " ";
                        }
                        textHighLighter += textImageChange;
                    }
                }

                if (item.Product != null && item.SharedInfo.SharedInfoClass == null)
                {
                    var textProduct = " " + mainContext.GetText(Resource.String.Lbl_AddedNewProductForSell) + " ";
                    textHighLighter += textProduct;
                }

                if (item.Blog != null && item.SharedInfo.SharedInfoClass == null)
                {
                    var textArticle = " " + mainContext.GetText(Resource.String.Lbl_CreatedNewArticle) + " ";
                    textHighLighter += textArticle;
                }

                if (item.Event?.EventClass != null && item.SharedInfo.SharedInfoClass == null && Convert.ToInt32(item.PageEventId) > 0)
                {
                    textEvent = " " + "[img src=post_icon_arrow_forward/]" + " " + mainContext.GetText(Resource.String.Lbl_CreatedNewEvent) + " "; //IonIconsFonts.ArrowRightC
                    textHighLighter += textEvent;
                }
                 
                if (item.Event?.EventClass != null && item.SharedInfo.SharedInfoClass == null && Convert.ToInt32(item.EventId) > 0)
                {
                    textEvent = " " + "[img src=post_icon_arrow_forward/]" + " " + Methods.FunString.DecodeString(item.Event?.EventClass.Name); //IonIconsFonts.ArrowRightC
                    textHighLighter += textEvent;
                }

                switch (string.IsNullOrEmpty(item.AlbumName))
                {
                    case false when item.AlbumName != null && item.SharedInfo.SharedInfoClass == null:
                    {
                        var textAlbumName = " " + mainContext.GetText(Resource.String.Lbl_AddedNewPhotosTo) + " " + Methods.FunString.DecodeString(item.AlbumName);
                        textHighLighter += textAlbumName;
                        break;
                    }
                }

                if (item.FundData?.FundDataClass != null && item.SharedInfo.SharedInfoClass == null)
                {
                    var textFundData = " " + mainContext.GetText(Resource.String.Lbl_CreatedNewFund) + " ";
                    textHighLighter += textFundData;
                }

                if (item.Fund?.PurpleFund != null && item.SharedInfo.SharedInfoClass == null)
                {
                    var textPurpleFund = " " + mainContext.GetText(Resource.String.Lbl_DonatedRequestFund) + " ";
                    textHighLighter += textPurpleFund;
                }

                if (item.Offer != null && item.PostType == "offer")
                {
                    var textOffer = " " + mainContext.GetText(Resource.String.Lbl_OfferPostAdded) + " ";
                    textHighLighter += textOffer;
                }

                if (item.ParentId != "0")
                {
                    if (item.GroupRecipientExists != null && item.GroupRecipientExists.Value && !string.IsNullOrEmpty(item.GroupRecipient.Name))
                    {
                        textShare += " " + "[img src=post_icon_arrow_forward/]" + " " + Methods.FunString.DecodeString(item.GroupRecipient.Name); //IonIconsFonts.ArrowRightC
                        textHighLighter += textShare;
                    }
                    else if (item.RecipientExists != null && item.RecipientExists.Value && item.Recipient.RecipientClass != null)
                    {
                        textShare += " " + "[img src=post_icon_arrow_forward/]" + " " + WoWonderTools.GetNameFinal(item.Recipient.RecipientClass); //IonIconsFonts.ArrowRightC
                        textHighLighter += textShare;
                    }
                    else
                    {
                        textShare += " " + mainContext.GetText(Resource.String.Lbl_SharedPost) + " ";

                        //if (item.SharedInfo.SharedInfoClass?.Publisher != null)
                        //    textShare += WoWonderTools.GetNameFinal(item.SharedInfo.SharedInfoClass.Publisher);

                        textHighLighter += textShare; 
                    } 
                }
                else
                {
                    if (item.GroupRecipientExists != null && item.GroupRecipientExists.Value && !string.IsNullOrEmpty(item.GroupRecipient.Name))
                    {
                        textShare += " " + "[img src=post_icon_arrow_forward/]" + " " + Methods.FunString.DecodeString(item.GroupRecipient.Name); //IonIconsFonts.ArrowRightC
                        textHighLighter += textShare;
                    }
                    else if (item.RecipientExists != null && item.RecipientExists.Value && item.Recipient.RecipientClass != null)
                    {
                        textShare += " " + "[img src=post_icon_arrow_forward/]" + " " + WoWonderTools.GetNameFinal(item.Recipient.RecipientClass); //IonIconsFonts.ArrowRightC
                        textHighLighter += textShare;
                    }
                }
                 
                Content = textHighLighter;
                DecoratedContent = new SpannableString(textHighLighter);

                //SetTextStyle(username, TypefaceStyle.Bold);
                TextViewWithImages.Publisher = publisher;
                DecoratedContent = TextViewWithImages.GetTextWithImages(item ,mainContext, new Java.Lang.String(textHighLighter.ToArray(), 0, textHighLighter.Length));

                return DecoratedContent;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return null!;
            }
        }

        public void Build(AppCompatTextView textHolder, SpannableString decoratedContent)
        {
            try
            {
                //textHolder.SetTextFuture(PrecomputedTextCompat.GetTextFuture(decoratedContent, TextViewCompat.GetTextMetricsParams(textHolder), null));
                textHolder.SetText(decoratedContent, TextView.BufferType.Spannable);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            } 
        }

        public void SetTextColor(string content, string color)
        {
            try
            { 
                if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
                    return;

                var indexFrom = content.IndexOf(content, StringComparison.Ordinal);
                indexFrom = indexFrom switch
                {
                    <= -1 => 0,
                    _ => indexFrom
                };

                var indexLast = indexFrom + content.Length;
                indexLast = indexLast switch
                {
                    <= -1 => 0,
                    _ => indexLast
                };

                DecoratedContent.SetSpan(new ForegroundColorSpan(Color.ParseColor(color)), indexFrom, indexLast, SpanTypes.ExclusiveExclusive);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }

        }

        public void SetRelativeSize(string texts, float proportion)
        {
            try
            {
                DecoratedContent.SetSpan(new RelativeSizeSpan(proportion), Content.IndexOf(texts, StringComparison.Ordinal), Content.IndexOf(texts, StringComparison.Ordinal) + texts.Length, SpanTypes.ExclusiveExclusive);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }

        }

        public void SetTextStyle(string texts, TypefaceStyle style)
        {
            try
            {
                DecoratedContent.SetSpan(new StyleSpan(style), Content.IndexOf(texts, StringComparison.Ordinal), Content.IndexOf(texts, StringComparison.Ordinal) + texts.Length, SpanTypes.ExclusiveExclusive);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void MakeTextClickable(string texts)
        {
            foreach (var text in texts.Where(text => Content.Contains(text)).ToList())
            {
                var index = Content.IndexOf(text);
                DecoratedContent.SetSpan(new ClickSpanClass(), index, index + text.ToString().Length, SpanTypes.ExclusiveExclusive);
            }

            //textView.setMovementMethod(LinkMovementMethod.getInstance());
        }

        public class ClickSpanClass : ClickableSpan
        {
            public override void OnClick(View widget)
            {

            }

            //public override void UpdateDrawState(TextPaint ds)
            //{
            //    base.UpdateDrawState(ds);
            //}
        }
    }
}
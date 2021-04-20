using System;
using Android.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using WoWonder.Helpers.Fonts;

namespace WoWonder.Helpers.Utils
{
   public class EmptyStateInflater
   {
        public Button EmptyStateButton;
        private TextView EmptyStateIcon;
        private TextView DescriptionText;
        private TextView TitleText;
        private LottieAnimationView AnimationView;

        public enum Type
        {
            NoConnection,
            NoSearchResult,
            SomThingWentWrong,
            NoPost,
            NoComments,
            NoNotifications,
            NoUsers,
            NoUsersReaction,
            NoFollow,
            NoAlbum,
            NoArticle,
            NoMovies,
            NoNearBy,
            NoEvent,
            NoProduct,
            NoGroup,
            NoPage,
            NoPhoto,
            NoFunding,
            NoJob,
            NoJobApply,
            NoCommonThings,
            NoReviews,
            NoVideo,
            NoGames,
            NoSessions,
            Gify,
            NoActivities,
            NoMemories,
            NoOffers,
            NoShop,
            NoBusiness,
        }

        public void InflateLayout(View inflated , Type type)
        {
            try
            {          
                EmptyStateIcon = (TextView)inflated.FindViewById(Resource.Id.emtyicon);
                TitleText = (TextView)inflated.FindViewById(Resource.Id.headText);
                DescriptionText = (TextView)inflated.FindViewById(Resource.Id.seconderyText);
                EmptyStateButton = (Button)inflated.FindViewById(Resource.Id.button);
                AnimationView = inflated.FindViewById<LottieAnimationView>(Resource.Id.animation_view);
               
               
                switch (type)
                {
                    case Type.NoConnection:
                        AnimationView.SetAnimation("NoInterntconnection.json");
                        AnimationView.Visibility = ViewStates.Visible; 
                        EmptyStateIcon.Visibility = ViewStates.Gone; 
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoConnection_TitleText);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_NoConnection_DescriptionText);
                        EmptyStateButton.Text = Application.Context.GetText(Resource.String.Lbl_NoConnection_Button);
                        break;
                    case Type.NoSearchResult:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.Search);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoSearchResult_TitleText);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_NoSearchResult_DescriptionText);
                        EmptyStateButton.Text = Application.Context.GetText(Resource.String.Lbl_NoSearchResult_Button);
                        break;
                    case Type.SomThingWentWrong:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.Close);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_SomThingWentWrong_TitleText);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_SomThingWentWrong_DescriptionText);
                        EmptyStateButton.Text = Application.Context.GetText(Resource.String.Lbl_SomThingWentWrong_Button);
                        break;
                    //else if (type == Type.NoComments)
                    //{
                    //    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.Chatbubbles);
                    //    EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                    //    TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoComments_TitleText);
                    //    DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_NoComments_DescriptionText);
                    //    EmptyStateButton.Visibility = ViewStates.Gone;
                    //}
                    case Type.NoPost:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.Frown);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoPost_TitleText);
                        DescriptionText.Text = " ";
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoNotifications:
                        AnimationView.SetAnimation("EmptyStateAnim4.json");
                        AnimationView.Visibility = ViewStates.Visible;
                        EmptyStateIcon.Visibility = ViewStates.Gone; 
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_notifications);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_NoNotifcationsDescriptions); 
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoUsers:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.Person);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoUsers_TitleText);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_NoUsers_DescriptionText);
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoUsersReaction:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.Person);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoUsers_TitleText);
                        DescriptionText.Text = " ";
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoFollow:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.Person);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoFollow_TitleText);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_NoFollow_DescriptionText);
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoAlbum:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.Images);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Albums);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_Start_Albums);
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoArticle:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon,FontAwesomeIcon.FileAlt);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Article);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_Start_Article);
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoMovies:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon,FontAwesomeIcon.Video);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Movies);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_Start_Movies);
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoNearBy:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.Person);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoUsers_TitleText);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_Start_NearBy);
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoEvent:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.CalendarAlt);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Events);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_Start_Events);
                        EmptyStateButton.Text = Application.Context.GetText(Resource.String.Btn_Create_Events);
                        break;
                    case Type.NoProduct:
                        AnimationView.SetAnimation("EmptyStateAnim1.json");
                        AnimationView.Visibility = ViewStates.Visible;
                        EmptyStateIcon.Visibility = ViewStates.Gone;
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Market);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_Start_Market);
                        EmptyStateButton.Text = Application.Context.GetText(Resource.String.Btn_AddProduct);
                        break;
                    case Type.NoGroup:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.CalendarAlt);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_JoinedGroup);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_Start_JoinedGroup);
                        EmptyStateButton.Text = Application.Context.GetText(Resource.String.Lbl_Search);
                        break;
                    case Type.NoPage:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.CalendarAlt);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_LikedPages);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_Start_LikedPages);
                        EmptyStateButton.Text = Application.Context.GetText(Resource.String.Lbl_Search);
                        break;
                    case Type.NoPhoto:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.Images);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Albums);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_Start_Albums);
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoFunding:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.HandHoldingUsd);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NotHaveAnyFundingRequest);
                        DescriptionText.Text = " ";
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoJob:
                        AnimationView.SetAnimation("EmptyStateAnim3.json");
                        AnimationView.Visibility = ViewStates.Visible;
                        EmptyStateIcon.Visibility = ViewStates.Gone;
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NotHaveAnyJobs);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_NoJobsDescriptions); 
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoJobApply:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.Briefcase);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoUsers_TitleText);
                        DescriptionText.Text = " ";
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoCommonThings:
                        AnimationView.SetAnimation("EmptyStateAnim7.json");
                        AnimationView.Visibility = ViewStates.Visible;
                        EmptyStateIcon.Visibility = ViewStates.Gone;
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoUsers_TitleText);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_NoCommentThingsDescriptions);
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoReviews:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.Star);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoReviews);
                        DescriptionText.Text = " ";
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoVideo:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.Images);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Video);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_Start_Video);
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoGames:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.Gamepad);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Games);
                        DescriptionText.Text = "";
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoSessions:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.Fingerprint);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Sessions);
                        DescriptionText.Text = "";
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.Gify:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.Gift);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Gif);
                        DescriptionText.Text = "";
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoActivities:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.ChartLine);
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Activities);
                        DescriptionText.Text = "";
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoMemories:
                        AnimationView.SetAnimation("EmptyStateAnim6.json");
                        AnimationView.Visibility = ViewStates.Visible;
                        EmptyStateIcon.Visibility = ViewStates.Gone;
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Memories);
                        DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_NoMemoriesDescriptions);
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoOffers:
                        AnimationView.Visibility = ViewStates.Gone;
                        FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.Box);  
                        EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Offers);
                        DescriptionText.Text = "";
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoShop:
                        AnimationView.SetAnimation("EmptyStateAnim1.json");
                        AnimationView.Visibility = ViewStates.Visible;
                        EmptyStateIcon.Visibility = ViewStates.Gone;  
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Shop);
                        DescriptionText.Text = "";
                        EmptyStateButton.Visibility = ViewStates.Gone;
                        break;
                    case Type.NoBusiness:
                        AnimationView.SetAnimation("EmptyStateAnim3.json");
                        AnimationView.Visibility = ViewStates.Visible;
                        EmptyStateIcon.Visibility = ViewStates.Gone;
                        TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Business);
                        DescriptionText.Text = "";
                        EmptyStateButton.Visibility = ViewStates.Gone;
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
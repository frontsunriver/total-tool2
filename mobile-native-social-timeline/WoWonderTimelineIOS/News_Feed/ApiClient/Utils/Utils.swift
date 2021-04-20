//
//  Utils.swift
//  News_Feed
//
//  Created by MBE For You on 2019-10-22.
//  Copyright © 2019 clines329. All rights reserved.
//

import Foundation

public struct APIClient {
    
//    public static let baseURl = "https://Facecjoc.com/api"
    public static let baseURl = "https://demo.wowonder.com/api"
//
    public struct Get_News_Feed {
        public static let get_News_Feed_Posts = "\(baseURl)/posts"
    }
    
    public struct Login_Authentication {
        public static let  loginAuthApi = "\(baseURl)/auth"
    }
    public struct TwoFactorAuthentication {
           public static let  TwoFactorAuthApi = "\(baseURl)/two-factor"
       }
    public struct SignUp_Authentication {
        public static let  signupAuthApi = "\(baseURl)/create-account"
    }
    public struct SocialLogin {
        public static let socailLoginApi = "\(baseURl)/social-login"
    }
    
    public struct ForgetPassword {
        public static let forgetPasswordApi = "\(baseURl)/send-reset-password-email"
    }
    
    public struct DeleteUser{
        public static let deleteUserApi = "\(baseURl)/delete-user"
    }
    public struct Stie_Setting{
        public static let siteSettingApi = "\(baseURl)/get-site-settings"
    }
    
    public struct User_Data {
        public static let getUserDataApi = "\(baseURl)/get-user-data"
    }
    
    public struct User_Posts_Data{
        public static let getUserPostsApi = "\(baseURl)/posts"
    }
    
    public struct User_Images {
        public static let getUserImagesApi = "\(baseURl)/get-user-albums"
    }
    
    public struct Follow_Request {
        public static let followRequestApi = "\(baseURl)/follow-user"
    }
    
    public struct Block_User {
        public static let bockUserApi = "\(baseURl)/block-user"
        public static let getBlockUserApi = "\(baseURl)/get-blocked-users"
        
    }
    public struct GetReactions {
        public static let getPostReactionApi = "\(baseURl)/get-reactions"
        
    }
    public struct AddReactions{
        public static let addReactionApi = "\(baseURl)/post-actions"
        
    }
    public struct BlogComments{
        public static let blogCommentApi = "\(baseURl)/blogs"
    }
    public struct CreateComment{
        public static let createComment = "\(baseURl)/comments"
    }
    public struct FetchComment {
        public static let fetchComment = "\(baseURl)/comments"
    }
    
    public struct CreateCommentReply{
        public static let createCommentReply = "\(baseURl)/comments"
    }
    public struct LikeComment{
        public static let likeComment = "\(baseURl)/comments"
    }
    
    public struct GetGroupPost{
        public static let getGroupPostApi = "\(baseURl)/posts"
    }
    
    public struct GetPagePost{
        public static let getPagePostApi = "\(baseURl)/posts"
    }
    public struct GetPageData{
        public static let getPageDataApi = "\(baseURl)/get-page-data"
    }
    public struct LikePage {
        public static let likePageApi = "\(baseURl)/like-page"
    }
    
    public struct GetPageInfo {
        public static let getPageInfoApi = "\(baseURl)/get-page-data"
    }
    
    public struct GetNotPageMemebers {
        public static let getNotPageMemebrsApi = "\(baseURl)/not_in_page_member"
    }
    public struct GetNotGroupMemebers {
        public static let getNotGroupMemebrsApi = "\(baseURl)/not_in_group_member"
    }
    
    public struct JoinGroup{
        public static let joinGroupAPi = "\(baseURl)/join-group"
    }
    
    public struct GetGroupMember {
        public static let getGroupMemberApi = "\(baseURl)/get_group_members"
    }
    
    public struct AddMembertoPage {
        public static let addMmembertoPageApi = "\(baseURl)/page_add"
    }
    
    public struct AddMembertoGroup {
        public static let addMmembertoGroupApi = "\(baseURl)/group_add"
    }
    
    public struct UpdateProfile {
        public static let updateProfileApi = "\(baseURl)/update-user-data"
    }
    
    public struct GetMyGroups {
        public static let getMyGroupsApi =  "\(baseURl)/get-my-groups"
    }
    public struct GetGroupData {
        public static let getGroupsDataApi =  "\(baseURl)/get-group-data"
    }
    
    public struct GetLikedPages{
        public static let getMyLikedPagesApi = "\(baseURl)/get-my-pages"
    }
    
    public struct CreateGroup{
        public static let createGroupApi = "\(baseURl)/create-group"
    }
    
    public struct CreatePage{
        public static let createPageApi = "\(baseURl)/create-page"
    }
    
    public struct GetMyPages{
        public static let getMyPagesApi = "\(baseURl)/get-my-pages"
    }
    
    public struct GetCommunity{
        public static let getcummunityApi = "\(baseURl)/get-community"
    }
    
    public struct UpdateGroupData {
        public static let updateGroupDataApi = "\(baseURl)/update-group-data"
    }
    
    public struct UpdatePageData {
        public static let updatePageDataApi = "\(baseURl)/update-page-data"
    }
    
    public struct DeleteGroup{
        public static let deleteGroupApi = "\(baseURl)/delete_group"
    }
    public struct DeletePage{
        public static let deletePageAPi = "\(baseURl)/delete_page"
    }
    public struct Job{
        public static let jobApi = "\(baseURl)/job"
    }
    public struct DeleteJob{
        public static let deleteJobApi = "\(baseURl)/post-actions"
    }
    public struct Album{
        public static let albumApi = "\(baseURl)/albums"
    }
    public struct Pokes{
        public static let PokesApi = "\(baseURl)/poke"
    }
    public struct Articles{
        public static let getBlogsApi = "\(baseURl)/get-articles"
    }
    public struct Products{
        public static let getProductApi = "\(baseURl)/get-products"
        public static let createProductApi = "\(baseURl)/create-product"
    }
    public struct Events{
        public static let getEventsApi = "\(baseURl)/get-events"
        public static let createEventApi = "\(baseURl)/create-event"
        public static let gotoEventApi = "\(baseURl)/go-to-event"
        public static let interestEventApi = "\(baseURl)/interest-event"
    }
    public struct Share{
        public static let sharePosts = "\(baseURl)/posts"
    }
    public struct PopularPost{
        public static let getPopularPostApi = "\(baseURl)/most_liked"
    }
    public struct Search{
        public static let getSearchDataApi = "\(baseURl)/search"
    }
    public struct GeneralData{
        public static let getGeneralDataApi = "\(baseURl)/get-general-data"
    }
    public struct GetHashtagPost{
        public static let getHashtagPostApi = "\(baseURl)/posts"
    }
    public struct GetFriends{
        public static let getFriendsApi = "\(baseURl)/get-friends"
    }
    public struct GetSavedPost{
        public static let getSavedPostApi = "\(baseURl)/posts"
    }
    public struct SavePost{
        public static let savePostApi = "\(baseURl)/post-actions"
    }
    public struct GetPostById{
          public static let getPostApi = "\(baseURl)/get-post-data"
      }
    public struct ReportPost{
        public static let reportPostApi = "\(baseURl)/post-actions"
    }
    public struct DeletePost{
        public static let deletePostApi = "\(baseURl)/post-actions"
    }
    public struct RatePage{
        public static  let ratePageApi = "\(baseURl)/rate_page"
    }
    public struct GetPageReview{
        public static let getPageReviewApi = "\(baseURl)/page_reviews"
    }
    public struct Wallet{
        public static let walletApi = "\(baseURl)/wallet"
    }
    public struct Gift{
          public static let giftApi = "\(baseURl)/gift"
    }
    public struct Upgrade{
             public static let upgradeApi = "\(baseURl)/upgrade"
    }
    public struct GoogleMap{
        public static let googleMapApi = "https://maps.googleapis.com/maps/api/geocode/json"
    }
    public struct Stories{
        public static let getUserStories = "\(baseURl)/get-user-stories"
        public static let createStories =  "\(baseURl)/create-story"
        public static let deleteStory =  "\(baseURl)/delete-story"
    }
    public struct Activities{
        public static let getActivities = "\(baseURl)/get-activities"
        
    }
    public struct Session{
        public static let getSession = "\(baseURl)/sessions"
        
    }
    
    public struct AddMoney{
        public static let addMoney = "\(baseURl)/wallet"
        
    }
    public struct userList{
        public static let userlIst = "\(baseURl)/get-friends"
        
    }
    public struct AddPost{
        public static let AddPostApi = "https://demo.wowonder.com/app_api.php?application=phone&type=new_post"
        
    }
    public struct GIFs{
        public static let GIFsApi = "https://api.giphy.com/v1/gifs/search"
        
    }
    public struct VIDEOS{
        public static let getUserVideos = "\(baseURl)/get-user-albums"
        
    }
    public struct UpdateTwoFactor{
          public static let updateTwoFactorApi = "\(baseURl)/update_two_factor"
      }
    public struct VoteUp{
        public static let voteUpApi = "\(baseURl)/vote_up"
    }
      
    public struct Params {
        public static let serverKey = "server_key"
        public static let type = "type"
        public static let limit = "limit"
        public static let userName = "username"
        public static let password = "password"
        public static let email = "email"
        public static let confirmPassword = "confirm_password"
        public static let accessToken = "access_token"
        public static let provider = "provider"
        public static let googleKey = "google_key"
        public static let offset = "after_post_id"
        public static let fetch = "fetch"
        public static let userId = "user_id"
        public static let id = "id"
        public static let blockUser = "block_action"
        public static let reactions = "reaction"
        public static let action = "action"
        public static let postId = "post_id"
        public static let off_set = "offset"
        public static let myOffset = "my_offset"
        public static let afterPostId = "after_post_id"
        public static let text = "text"
        public static let commentId = "comment_id"
        public static let pageId = "page_id"
        public static let groupId = "group_id"
        public static let phoneNumber = "phone_number"
        public static let newPassword = "new_password"
        public static let currentPassword = "current_password"
        public static let gender = "gender"
        public static let avatar = "avatar"
        public static let cover = "cover"
        public static let address = "address"
        public static let website = "website"
        public static let working = "working"
        public static let firstName = "first_name"
        public static let lastName = "last_name"
        public static let school = "school"
        public static let groupName = "group_name"
        public static let groupTitle = "group_title"
        public static let about = "about"
        public static let category = "category"
        public static let groupPrivacy = "privacy"
        public static let pageName = "page_name"
        public static let pageTitle = "page_title"
        public static let pageCategory = "page_category"
        public static let company = "company"
        public static let pagePhone = "phone"
        public static let pageDecription = "page_description"
        public static let callActionType = "call_action_type"
        public static let callActionUrl = "call_action_type_url"
        public static let facebook = "facebook"
        public static let twitter = "twitter"
        public static let instgram = "instgram"
        public static let linkedin = "linkedin"
        public static let youtube = "youtube"
        public static let vk = "vk"
        public static let jobTitle = "job_title"
        public static let location = "location"
        public static let jobType = "job_type"
        public static let currentLat = "lat"
        public static let currentLng = "lng"
        public static let minimumSalary = "minimum"
        public static let maximumSalary = "maximum"
        public static let salaryDate = "salary_date"
        public static let currency = "currency"
        public static let jobDescription = "description"
        public static let jobCategory = "category"
        public static let jobC_id = "c_id"
        public static let imageType = "image_type"
        public static let thumbnail = "thumbnail"
        public static let jobId = "job_id"
        public static let albumName = "album_name"
        public static let postPhotos = "postPhotos"
        public static let headerKey = "Content-Type"
        public static let distance = "distance"
        public static let keyword = "keyword"
        public static let categoryId = "category_id"
        public static let productTitle = "product_title"
        public static let productPrice = "product_price"
        public static let productLocation = "product_location"
        public static let productType = "product_type"
        public static let productCategory = "product_category"
        public static let productDescription = "product_description"
        public static let eventName = "event_name"
        public static let eventLocation = "event_location"
        public static let eventDescription = "event_description"
        public static let eventStartDate = "event_start_date"
        public static let eventEndDate = "event_end_date"
        public static let eventStarttime = "event_start_time"
        public static let eventEndtime = "event_end_time"
        public static let eventId = "event_id"
        public static let googleMapKey = "key"
        public static let lastTotal = "lasttotal"
        public static let dt = "dt"
        public static let country = "country"
        public static let status = "status"
        public static let verified = "verified"
        public static let filterbyage = "filterbyage"
        public static let age_from = "age_from"
        public static let age_to = "age_to"
        public static let hash = "hash"
        public static let followingOffset = "following_offset"
        public static let followersOffset = "followers_offset"
        public static let story_id = "story_id"
        public static var FileType = "file_type"
        public static let two_factor = "two_factor"
        public static let val = "val"
        public static let blogId = "blog_id"
        public static let reactionType = "reaction_type"
        public static let usr_birthday = "birthday"
        public static let workspace = "workspace"
        public static let mobile = "mobile"
        public static let s = "s"
        public static let amount = "amount"
        public static let postText = "postText"
        public static let post_color = "post_color"
        public static let postPrivacy = "postPrivacy"
        public static let postSticker = "postSticker"
        public static let feeling_type = "feeling_type"
        public static let feeling = "feeling"
        public static var api_key = "api_key"
        public static var q = "q"
         public static var device_id = "device_id"
         public static var group_id = "group_id"
        public static var page_id = "page_id"
        public static var event_id = "event_id"
         public static var code = "code"
        
    }
    public struct LOCAL {
        public  static var siteSetting = "siteSetting"
        public static let amount = "amount"
    }
    
    
    
    
    public struct SERVER_KEY {
//        public  static var Server_Key = "0d50145a4192b799c2df5f0dbc0204c55501dbc0"
        //wowonder 
        public  static var Server_Key = "131c471c8b4edf662dd0ebf7adf3c3d7365838b9"

//        131c471c8b4edf662dd0ebf7adf3c3d7365838b9
        
        
    }
    public struct Google_Key{
        public static var google_Key = "AIzaSyCdzU_y3YKo12pjsa3HBSCwqeLjbqf4zjc"
    }
}






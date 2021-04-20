

import Foundation
class LastActivitiesModel{
    
    struct LastActivitiesSuccessModel{
          var api_status: Int
          var activities: [[String:Any]]
    }
    
//    struct LastActivitiesSuccessModel: Codable {
//        var apiStatus: Int?
//        var activities: [Activity]?
//
//        enum CodingKeys: String, CodingKey {
//            case apiStatus = "api_status"
//            case activities
//        }
//    }
    struct LastActivitiesErrorModel: Codable {
              var apiStatus: String?
              var errors: Errors?

              enum CodingKeys: String, CodingKey {
                  case apiStatus = "api_status"
                  case errors
              }
          }

          // MARK: - Errors
          struct Errors: Codable {
              var errorID, errorText: String?

              enum CodingKeys: String, CodingKey {
                  case errorID = "error_id"
                  case errorText = "error_text"
              }
          }

    // MARK: - Activity
    struct Activity: Codable {
        var id, userID, postID, replyID: String?
        var commentID, followID: String?
        var activityType: ActivityType?
        var time: String?
        var postData: PostData?
        var activator: Activator?
        var activityText: String?

        enum CodingKeys: String, CodingKey {
            case id
            case userID = "user_id"
            case postID = "post_id"
            case replyID = "reply_id"
            case commentID = "comment_id"
            case followID = "follow_id"
            case activityType = "activity_type"
            case time, postData, activator
            case activityText = "activity_text"
        }
    }

    // MARK: - Activator
    struct Activator: Codable {
        var userID: String?
        var username: Username?
        var email: Email?
        var firstName: FirstName?
        var lastName: LastName?
        var avatar, cover: String?
        var backgroundImage, relationshipID: String?
        var address: Address?
        var working: ActivatorWorking?
        var workingLink, about: String?
        var school: ActivatorSchool?
        var gender: Gender?
        var birthday, countryID, website, facebook: String?
        var google, twitter, linkedin, youtube: String?
        var vk, instagram: String?
        var language: ActivatorLanguage?
        var ipAddress: IPAddress?
        var followPrivacy, friendPrivacy: String?
        var postPrivacy: ActivatorPostPrivacy?
        var messagePrivacy, confirmFollowers, showActivitiesPrivacy, birthPrivacy: String?
        var visitPrivacy, verified, lastseen, emailNotification: String?
        var eLiked, eWondered, eShared, eFollowed: String?
        var eCommented, eVisited, eLikedPage, eMentioned: String?
        var eJoinedGroup, eAccepted, eProfileWallPost, eSentmeMsg: String?
        var eLastNotif, notificationSettings, status, active: String?
        var admin: String?
        var registered: ActivatorRegistered?
        var phoneNumber: ActivatorPhoneNumber?
        var isPro, proType: String?
        var timezone: Timezone?
        var referrer, refUserID, balance, paypalEmail: String?
        var notificationsSound, orderPostsBy, androidMDeviceID, iosMDeviceID: String?
        var androidNDeviceID, iosNDeviceID, webDeviceID, wallet: String?
        var lat, lng, lastLocationUpdate, shareMyLocation: String?
        var lastDataUpdate: String?
        var details: ActivatorDetails?
        var lastAvatarMod, lastCoverMod, points, dailyPoints: String?
        var pointDayExpire, lastFollowID, shareMyData: String?
        var lastLoginData: String?
        var twoFactor, newEmail, twoFactorVerified, newPhone: String?
        var infoFile, city, state, zip: String?
        var schoolCompleted: String?
        var weatherUnit: WeatherUnit?
        var avatarFull: String?
        var url: String?
        var name: Name?
        var mutualFriendsData: [String]?
        var lastseenUnixTime: String?
        var lastseenStatus: LastseenStatus?
        var password, backgroundImageStatus, emailCode, src: String?
        var showlastseen, type, startUp, startUpInfo: String?
        var startupFollow, startupImage, lastEmailSent, smsCode: String?
        var proTime, joined, cssFile, socialLogin: String?
        var sidebarData, avatarOrg, coverOrg, coverFull: String?
        var id: String?
        var followingData, followersData: [String]?
        var likesData, groupsData, albumData: String?

        enum CodingKeys: String, CodingKey {
            case userID = "user_id"
            case username, email
            case firstName = "first_name"
            case lastName = "last_name"
            case avatar, cover
            case backgroundImage = "background_image"
            case relationshipID = "relationship_id"
            case address, working
            case workingLink = "working_link"
            case about, school, gender, birthday
            case countryID = "country_id"
            case website, facebook, google, twitter, linkedin, youtube, vk, instagram, language
            case ipAddress = "ip_address"
            case followPrivacy = "follow_privacy"
            case friendPrivacy = "friend_privacy"
            case postPrivacy = "post_privacy"
            case messagePrivacy = "message_privacy"
            case confirmFollowers = "confirm_followers"
            case showActivitiesPrivacy = "show_activities_privacy"
            case birthPrivacy = "birth_privacy"
            case visitPrivacy = "visit_privacy"
            case verified, lastseen, emailNotification
            case eLiked = "e_liked"
            case eWondered = "e_wondered"
            case eShared = "e_shared"
            case eFollowed = "e_followed"
            case eCommented = "e_commented"
            case eVisited = "e_visited"
            case eLikedPage = "e_liked_page"
            case eMentioned = "e_mentioned"
            case eJoinedGroup = "e_joined_group"
            case eAccepted = "e_accepted"
            case eProfileWallPost = "e_profile_wall_post"
            case eSentmeMsg = "e_sentme_msg"
            case eLastNotif = "e_last_notif"
            case notificationSettings = "notification_settings"
            case status, active, admin, registered
            case phoneNumber = "phone_number"
            case isPro = "is_pro"
            case proType = "pro_type"
            case timezone, referrer
            case refUserID = "ref_user_id"
            case balance
            case paypalEmail = "paypal_email"
            case notificationsSound = "notifications_sound"
            case orderPostsBy = "order_posts_by"
            case androidMDeviceID = "android_m_device_id"
            case iosMDeviceID = "ios_m_device_id"
            case androidNDeviceID = "android_n_device_id"
            case iosNDeviceID = "ios_n_device_id"
            case webDeviceID = "web_device_id"
            case wallet, lat, lng
            case lastLocationUpdate = "last_location_update"
            case shareMyLocation = "share_my_location"
            case lastDataUpdate = "last_data_update"
            case details
            case lastAvatarMod = "last_avatar_mod"
            case lastCoverMod = "last_cover_mod"
            case points
            case dailyPoints = "daily_points"
            case pointDayExpire = "point_day_expire"
            case lastFollowID = "last_follow_id"
            case shareMyData = "share_my_data"
            case lastLoginData = "last_login_data"
            case twoFactor = "two_factor"
            case newEmail = "new_email"
            case twoFactorVerified = "two_factor_verified"
            case newPhone = "new_phone"
            case infoFile = "info_file"
            case city, state, zip
            case schoolCompleted = "school_completed"
            case weatherUnit = "weather_unit"
            case avatarFull = "avatar_full"
            case url, name
            case mutualFriendsData = "mutual_friends_data"
            case lastseenUnixTime = "lastseen_unix_time"
            case lastseenStatus = "lastseen_status"
            case password
            case backgroundImageStatus = "background_image_status"
            case emailCode = "email_code"
            case src, showlastseen, type
            case startUp = "start_up"
            case startUpInfo = "start_up_info"
            case startupFollow = "startup_follow"
            case startupImage = "startup_image"
            case lastEmailSent = "last_email_sent"
            case smsCode = "sms_code"
            case proTime = "pro_time"
            case joined
            case cssFile = "css_file"
            case socialLogin = "social_login"
            case sidebarData = "sidebar_data"
            case avatarOrg = "avatar_org"
            case coverOrg = "cover_org"
            case coverFull = "cover_full"
            case id
            case followingData = "following_data"
            case followersData = "followers_data"
            case likesData = "likes_data"
            case groupsData = "groups_data"
            case albumData = "album_data"
        }
    }

    enum Address: String, Codable {
        case ankaraTürkiye = "Ankara/Türkiye"
        case empty = ""
    }

    // MARK: - ActivatorDetails
    struct ActivatorDetails: Codable {
        var postCount, albumCount, followingCount, followersCount: String?
        var groupsCount, likesCount: String?
        var mutualFriendsCount: Int?

        enum CodingKeys: String, CodingKey {
            case postCount = "post_count"
            case albumCount = "album_count"
            case followingCount = "following_count"
            case followersCount = "followers_count"
            case groupsCount = "groups_count"
            case likesCount = "likes_count"
            case mutualFriendsCount = "mutual_friends_count"
        }
    }

    enum Email: String, Codable {
        case bulutmedya1GmailCOM = "bulutmedya1@gmail.com"
        case eratwqriyGmailCOM = "eratwqriy@gmail.com"
        case infoOlomu1COM = "info@olomu1.com"
    }

    enum FirstName: String, Codable {
        case fast = "fast"
        case güven = "Güven"
        case ollaArray = "Olla Array"
    }

    enum Gender: String, Codable {
        case female = "female"
        case male = "male"
    }

    enum IPAddress: String, Codable {
        case the231061611 = "23.106.161.1"
        case the7030106134 = "70.30.106.134"
        case the781795137 = "78.179.5.137"
    }

    enum ActivatorLanguage: String, Codable {
        case english = "english"
        case turkish = "turkish"
    }

    enum LastName: String, Codable {
        case empty = ""
        case gamegold = "gamegold"
        case özdemir = "Özdemir"
    }

    enum LastseenStatus: String, Codable {
        case off = "off"
        case on = "on"
    }

    enum Name: String, Codable {
        case fastGamegold = "fast gamegold"
        case güvenÖzdemir = "Güven Özdemir"
        case ollaArray = "Olla Array"
    }

    enum ActivatorPhoneNumber: String, Codable {
        case empty = ""
        case the905331438062 = "+905331438062"
    }

    enum ActivatorPostPrivacy: String, Codable {
        case everyone = "everyone"
        case ifollow = "ifollow"
    }

    enum ActivatorRegistered: String, Codable {
        case the102019 = "10/2019"
        case the22020 = "2/2020"
    }

    enum ActivatorSchool: String, Codable {
        case empty = ""
        case girneAmerikanÜniversitesi = "Girne Amerikan Üniversitesi"
    }

    enum Timezone: String, Codable {
        case africaLagos = "Africa/Lagos"
        case africaTunis = "Africa/Tunis"
        case asiaTehran = "Asia/Tehran"
        case utc = "UTC"
    }

    enum Username: String, Codable {
        case bulutmedya = "bulutmedya"
        case gamegoldfast = "gamegoldfast"
        case ollaArray = "OllaArray"
    }

    enum WeatherUnit: String, Codable {
        case us = "us"
    }

    enum ActivatorWorking: String, Codable {
        case danışman = "Danışman"
        case empty = ""
    }

    enum ActivityType: String, Codable {
        case commentedPost = "commented_post"
        case following = "following"
        case friend = "friend"
        case reactionPostLike = "reaction|post|Like"
    }

    // MARK: - PostData
    struct PostData: Codable {
        var id, postID, userID, recipientID: String?
        var postText, pageID, groupID, eventID: String?
        var pageEventID: String?
        var postLink: String?
        var postLinkTitle: PostLinkTitle?
        var postLinkImage: String?
        var postLinkContent: PostLinkContent?
        var postVimeo, postDailymotion, postFacebook, postFile: String?
        var postRecord: String?
        var postSticker, sharedFrom, postURL: JSONNull?
        var parentID, cache, commentsStatus, blur: String?
        var colorID, jobID, fundRaiseID, fundID: String?
        var active: String?
        var postFileName: PostFileName?
        var postFileThumb: String?
        var postYoutube: PostYoutube?
        var postVine, postSoundCloud, postPlaytube: String?
        var postDeepsound: PostDeepsound?
        var postMap: PostMap?
        var postShare, postPrivacy: String?
        var postType: PostType?
        var postFeeling, postListening, postTraveling, postWatching: String?
        var postPlaying, postPhoto, time: String?
        var registered: PostDataRegistered?
        var albumName, multiImage, multiImagePost, boosted: String?
        var productID, pollID, blogID, videoViews: String?
        var publisher: Publisher?
        var limitComments: Int?
        var limitedComments, isGroupPost, groupRecipientExists, groupAdmin: Bool?
        var postIsPromoted: Int?
        var postTextAPI: String?
        var orginaltext: String?
        var postTime: String?
        var page: Int?
        var url: String?
        var viaType: String?
        var recipientExists: Bool?
        var recipient: String?
        var admin: Bool?
        var postDataPostShare: String?
        var isPostSaved, isPostReported: Bool?
        var isPostBoosted: Int?
        var isLiked, isWondered: Bool?
        var postComments, postShares, postLikes, postWonders: String?
        var isPostPinned: Bool?
        var photoAlbum: [JSONAny]?
        var options: [Option]?
        var votedID: Int?
        var postFileFull: String?
        var reaction: Reaction?
        var job, fund, fundData: [JSONAny]?
        var event: Event?
        var blog: Blog?

        enum CodingKeys: String, CodingKey {
            case id
            case postID = "post_id"
            case userID = "user_id"
            case recipientID = "recipient_id"
            case postText
            case pageID = "page_id"
            case groupID = "group_id"
            case eventID = "event_id"
            case pageEventID = "page_event_id"
            case postLink, postLinkTitle, postLinkImage, postLinkContent, postVimeo, postDailymotion, postFacebook, postFile, postRecord, postSticker
            case sharedFrom = "shared_from"
            case postURL = "post_url"
            case parentID = "parent_id"
            case cache
            case commentsStatus = "comments_status"
            case blur
            case colorID = "color_id"
            case jobID = "job_id"
            case fundRaiseID = "fund_raise_id"
            case fundID = "fund_id"
            case active, postFileName, postFileThumb, postYoutube, postVine, postSoundCloud, postPlaytube, postDeepsound, postMap, postShare, postPrivacy, postType, postFeeling, postListening, postTraveling, postWatching, postPlaying, postPhoto, time, registered
            case albumName = "album_name"
            case multiImage = "multi_image"
            case multiImagePost = "multi_image_post"
            case boosted
            case productID = "product_id"
            case pollID = "poll_id"
            case blogID = "blog_id"
            case videoViews, publisher
            case limitComments = "limit_comments"
            case limitedComments = "limited_comments"
            case isGroupPost = "is_group_post"
            case groupRecipientExists = "group_recipient_exists"
            case groupAdmin = "group_admin"
            case postIsPromoted = "post_is_promoted"
            case postTextAPI = "postText_API"
            case orginaltext = "Orginaltext"
            case postTime = "post_time"
            case page, url
            case viaType = "via_type"
            case recipientExists = "recipient_exists"
            case recipient, admin
            case postDataPostShare = "post_share"
            case isPostSaved = "is_post_saved"
            case isPostReported = "is_post_reported"
            case isPostBoosted = "is_post_boosted"
            case isLiked = "is_liked"
            case isWondered = "is_wondered"
            case postComments = "post_comments"
            case postShares = "post_shares"
            case postLikes = "post_likes"
            case postWonders = "post_wonders"
            case isPostPinned = "is_post_pinned"
            case photoAlbum = "photo_album"
            case options
            case votedID = "voted_id"
            case postFileFull = "postFile_full"
            case reaction, job, fund
            case fundData = "fund_data"
            case event, blog
        }
    }

    // MARK: - Blog
    struct Blog: Codable {
        var id, user, title, content: String?
        var blogDescription, posted, category: String?
        var thumbnail: String?
        var view, shared, tags: String?
        var author: Activator?
        var tagsArray: [String]?
        var url: String?
        var categoryLink: String?
        var categoryName: String?
        var isPostAdmin: Bool?

        enum CodingKeys: String, CodingKey {
            case id, user, title, content
            case blogDescription = "description"
            case posted, category, thumbnail, view, shared, tags, author
            case tagsArray = "tags_array"
            case url
            case categoryLink = "category_link"
            case categoryName = "category_name"
            case isPostAdmin = "is_post_admin"
        }
    }

    // MARK: - Event
    struct Event: Codable {
        var id, name, location, eventDescription: String?
        var startDate, startTime, endDate, endTime: String?
        var posterID: String?
        var cover: String?
        var userData: UserData?
        var isOwner: Bool?
        var startEditDate, startDateJS, endEditDate: String?
        var url: String?

        enum CodingKeys: String, CodingKey {
            case id, name, location
            case eventDescription = "description"
            case startDate = "start_date"
            case startTime = "start_time"
            case endDate = "end_date"
            case endTime = "end_time"
            case posterID = "poster_id"
            case cover
            case userData = "user_data"
            case isOwner = "is_owner"
            case startEditDate = "start_edit_date"
            case startDateJS = "start_date_js"
            case endEditDate = "end_edit_date"
            case url
        }
    }

    // MARK: - UserData
    struct UserData: Codable {
        var userID, username, email, password: String?
        var firstName, lastName: String?
        var avatar, cover: String?
        var backgroundImage, backgroundImageStatus, relationshipID, address: String?
        var working: UserDataWorking?
        var workingLink: String?
        var about: String?
        var school: UserDataSchool?
        var gender: Gender?
        var birthday, countryID: String?
        var website: String?
        var facebook: Facebook?
        var google, twitter, linkedin: String?
        var youtube: Facebook?
        var vk: String?
        var instagram: Facebook?
        var language: ActivatorLanguage?
        var emailCode, src, ipAddress, followPrivacy: String?
        var friendPrivacy: String?
        var postPrivacy: ActivatorPostPrivacy?
        var messagePrivacy, confirmFollowers, showActivitiesPrivacy, birthPrivacy: String?
        var visitPrivacy, verified, lastseen, showlastseen: String?
        var emailNotification, eLiked, eWondered, eShared: String?
        var eFollowed, eCommented, eVisited, eLikedPage: String?
        var eMentioned, eJoinedGroup, eAccepted, eProfileWallPost: String?
        var eSentmeMsg, eLastNotif, notificationSettings, status: String?
        var active, admin, type: String?
        var registered: ActivatorRegistered?
        var startUp, startUpInfo, startupFollow, startupImage: String?
        var lastEmailSent: String?
        var phoneNumber: UserDataPhoneNumber?
        var smsCode, isPro, proTime, proType: String?
        var joined, cssFile: String?
        var timezone: Timezone?
        var referrer, refUserID, balance, paypalEmail: String?
        var notificationsSound, orderPostsBy, socialLogin, androidMDeviceID: String?
        var iosMDeviceID, androidNDeviceID, iosNDeviceID, webDeviceID: String?
        var wallet, lat, lng, lastLocationUpdate: String?
        var shareMyLocation, lastDataUpdate: String?
        var details: ActivatorDetails?
        var sidebarData, lastAvatarMod, lastCoverMod, points: String?
        var dailyPoints, pointDayExpire, lastFollowID, shareMyData: String?
        var lastLoginData: JSONNull?
        var twoFactor, newEmail, twoFactorVerified, newPhone: String?
        var infoFile, city, state, zip: String?
        var schoolCompleted: String?
        var weatherUnit: WeatherUnit?
        var avatarOrg, coverOrg, coverFull, avatarFull: String?
        var id: String?
        var url: String?
        var name: String?
        var followingData, followersData: [String]?
        var mutualFriendsData: String?
        var likesData, groupsData: [String]?
        var albumData, lastseenUnixTime: String?
        var lastseenStatus: LastseenStatus?

        enum CodingKeys: String, CodingKey {
            case userID = "user_id"
            case username, email, password
            case firstName = "first_name"
            case lastName = "last_name"
            case avatar, cover
            case backgroundImage = "background_image"
            case backgroundImageStatus = "background_image_status"
            case relationshipID = "relationship_id"
            case address, working
            case workingLink = "working_link"
            case about, school, gender, birthday
            case countryID = "country_id"
            case website, facebook, google, twitter, linkedin, youtube, vk, instagram, language
            case emailCode = "email_code"
            case src
            case ipAddress = "ip_address"
            case followPrivacy = "follow_privacy"
            case friendPrivacy = "friend_privacy"
            case postPrivacy = "post_privacy"
            case messagePrivacy = "message_privacy"
            case confirmFollowers = "confirm_followers"
            case showActivitiesPrivacy = "show_activities_privacy"
            case birthPrivacy = "birth_privacy"
            case visitPrivacy = "visit_privacy"
            case verified, lastseen, showlastseen, emailNotification
            case eLiked = "e_liked"
            case eWondered = "e_wondered"
            case eShared = "e_shared"
            case eFollowed = "e_followed"
            case eCommented = "e_commented"
            case eVisited = "e_visited"
            case eLikedPage = "e_liked_page"
            case eMentioned = "e_mentioned"
            case eJoinedGroup = "e_joined_group"
            case eAccepted = "e_accepted"
            case eProfileWallPost = "e_profile_wall_post"
            case eSentmeMsg = "e_sentme_msg"
            case eLastNotif = "e_last_notif"
            case notificationSettings = "notification_settings"
            case status, active, admin, type, registered
            case startUp = "start_up"
            case startUpInfo = "start_up_info"
            case startupFollow = "startup_follow"
            case startupImage = "startup_image"
            case lastEmailSent = "last_email_sent"
            case phoneNumber = "phone_number"
            case smsCode = "sms_code"
            case isPro = "is_pro"
            case proTime = "pro_time"
            case proType = "pro_type"
            case joined
            case cssFile = "css_file"
            case timezone, referrer
            case refUserID = "ref_user_id"
            case balance
            case paypalEmail = "paypal_email"
            case notificationsSound = "notifications_sound"
            case orderPostsBy = "order_posts_by"
            case socialLogin = "social_login"
            case androidMDeviceID = "android_m_device_id"
            case iosMDeviceID = "ios_m_device_id"
            case androidNDeviceID = "android_n_device_id"
            case iosNDeviceID = "ios_n_device_id"
            case webDeviceID = "web_device_id"
            case wallet, lat, lng
            case lastLocationUpdate = "last_location_update"
            case shareMyLocation = "share_my_location"
            case lastDataUpdate = "last_data_update"
            case details
            case sidebarData = "sidebar_data"
            case lastAvatarMod = "last_avatar_mod"
            case lastCoverMod = "last_cover_mod"
            case points
            case dailyPoints = "daily_points"
            case pointDayExpire = "point_day_expire"
            case lastFollowID = "last_follow_id"
            case shareMyData = "share_my_data"
            case lastLoginData = "last_login_data"
            case twoFactor = "two_factor"
            case newEmail = "new_email"
            case twoFactorVerified = "two_factor_verified"
            case newPhone = "new_phone"
            case infoFile = "info_file"
            case city, state, zip
            case schoolCompleted = "school_completed"
            case weatherUnit = "weather_unit"
            case avatarOrg = "avatar_org"
            case coverOrg = "cover_org"
            case coverFull = "cover_full"
            case avatarFull = "avatar_full"
            case id, url, name
            case followingData = "following_data"
            case followersData = "followers_data"
            case mutualFriendsData = "mutual_friends_data"
            case likesData = "likes_data"
            case groupsData = "groups_data"
            case albumData = "album_data"
            case lastseenUnixTime = "lastseen_unix_time"
            case lastseenStatus = "lastseen_status"
        }
    }

    enum Facebook: String, Codable {
        case donyadadrasan = "donyadadrasan"
        case empty = ""
    }

    enum UserDataPhoneNumber: String, Codable {
        case empty = ""
        case the0989109693365 = "0989109693365"
        case the12121212112 = "12121212112"
    }

    enum UserDataSchool: String, Codable {
        case anonymousHackSchool = "Anonymous Hack School"
        case empty = ""
        case yadgarEmam = "Yadgar Emam"
    }

    enum UserDataWorking: String, Codable {
        case empty = ""
        case hackTheBox = "HackTheBox"
        case pmbConsole = "PMB CONSOLE"
        case wowonder = "wowonder"
    }

    // MARK: - Option
    struct Option: Codable {
        var id, postID, text, time: String?
        var optionVotes, percentage, percentageNum: String?
        var all: Int?

        enum CodingKeys: String, CodingKey {
            case id
            case postID = "post_id"
            case text, time
            case optionVotes = "option_votes"
            case percentage
            case percentageNum = "percentage_num"
            case all
        }
    }

    enum PostDeepsound: String, Codable {
        case empty = ""
        case wpNeN56Fp7FosHy = "WPNeN56fp7FosHy"
    }

    enum PostFileName: String, Codable {
        case empty = ""
        case logoJpg = "logo.jpg"
        case lokPNG = "lok.png"
    }

    enum PostLinkContent: String, Codable {
        case byDynatonic = "By @dynatonic"
        case empty = ""
    }

    enum PostLinkTitle: String, Codable {
        case empty = ""
        case tehranMazeratiDeepSound = "Tehran mazerati - DeepSound"
    }

    enum PostMap: String, Codable {
        case belgique = "Belgique"
        case empty = ""
    }

    enum PostType: String, Codable {
        case post = "post"
        case profileCoverPicture = "profile_cover_picture"
        case profilePicture = "profile_picture"
    }

    enum PostYoutube: String, Codable {
        case c14IcbK14YI = "C14icbK14yI"
        case empty = ""
    }

    // MARK: - Publisher
    struct Publisher: Codable {
        var userID, username, email, firstName: String?
        var lastName: String?
        var avatar: String?
        var cover: String?
        var backgroundImage, relationshipID, address: String?
        var working: UserDataWorking?
        var workingLink: String?
        var about: String?
        var school: UserDataSchool?
        var gender: Gender?
        var birthday, countryID: String?
        var website: String?
        var facebook: Facebook?
        var google, twitter, linkedin: String?
        var youtube: Facebook?
        var vk: String?
        var instagram: Facebook?
        var language: PublisherLanguage?
        var ipAddress, followPrivacy, friendPrivacy: String?
        var postPrivacy: PublisherPostPrivacy?
        var messagePrivacy, confirmFollowers, showActivitiesPrivacy, birthPrivacy: String?
        var visitPrivacy, verified, lastseen, emailNotification: String?
        var eLiked, eWondered, eShared, eFollowed: String?
        var eCommented, eVisited, eLikedPage, eMentioned: String?
        var eJoinedGroup, eAccepted, eProfileWallPost, eSentmeMsg: String?
        var eLastNotif, notificationSettings, status, active: String?
        var admin: String?
        var registered: PublisherRegistered?
        var phoneNumber: UserDataPhoneNumber?
        var isPro, proType: String?
        var timezone: Timezone?
        var referrer, refUserID, balance, paypalEmail: String?
        var notificationsSound, orderPostsBy, androidMDeviceID, iosMDeviceID: String?
        var androidNDeviceID, iosNDeviceID, webDeviceID, wallet: String?
        var lat, lng, lastLocationUpdate, shareMyLocation: String?
        var lastDataUpdate: String?
        var details: PublisherDetails?
        var lastAvatarMod, lastCoverMod, points, dailyPoints: String?
        var pointDayExpire, lastFollowID, shareMyData: String?
        var lastLoginData: String?
        var twoFactor, newEmail, twoFactorVerified, newPhone: String?
        var infoFile, city, state, zip: String?
        var schoolCompleted: String?
        var weatherUnit: WeatherUnit?
        var avatarFull: String?
        var url: String?
        var name: String?
        var mutualFriendsData: MutualFriendsData?
        var lastseenUnixTime: String?
        var lastseenStatus: LastseenStatus?

        enum CodingKeys: String, CodingKey {
            case userID = "user_id"
            case username, email
            case firstName = "first_name"
            case lastName = "last_name"
            case avatar, cover
            case backgroundImage = "background_image"
            case relationshipID = "relationship_id"
            case address, working
            case workingLink = "working_link"
            case about, school, gender, birthday
            case countryID = "country_id"
            case website, facebook, google, twitter, linkedin, youtube, vk, instagram, language
            case ipAddress = "ip_address"
            case followPrivacy = "follow_privacy"
            case friendPrivacy = "friend_privacy"
            case postPrivacy = "post_privacy"
            case messagePrivacy = "message_privacy"
            case confirmFollowers = "confirm_followers"
            case showActivitiesPrivacy = "show_activities_privacy"
            case birthPrivacy = "birth_privacy"
            case visitPrivacy = "visit_privacy"
            case verified, lastseen, emailNotification
            case eLiked = "e_liked"
            case eWondered = "e_wondered"
            case eShared = "e_shared"
            case eFollowed = "e_followed"
            case eCommented = "e_commented"
            case eVisited = "e_visited"
            case eLikedPage = "e_liked_page"
            case eMentioned = "e_mentioned"
            case eJoinedGroup = "e_joined_group"
            case eAccepted = "e_accepted"
            case eProfileWallPost = "e_profile_wall_post"
            case eSentmeMsg = "e_sentme_msg"
            case eLastNotif = "e_last_notif"
            case notificationSettings = "notification_settings"
            case status, active, admin, registered
            case phoneNumber = "phone_number"
            case isPro = "is_pro"
            case proType = "pro_type"
            case timezone, referrer
            case refUserID = "ref_user_id"
            case balance
            case paypalEmail = "paypal_email"
            case notificationsSound = "notifications_sound"
            case orderPostsBy = "order_posts_by"
            case androidMDeviceID = "android_m_device_id"
            case iosMDeviceID = "ios_m_device_id"
            case androidNDeviceID = "android_n_device_id"
            case iosNDeviceID = "ios_n_device_id"
            case webDeviceID = "web_device_id"
            case wallet, lat, lng
            case lastLocationUpdate = "last_location_update"
            case shareMyLocation = "share_my_location"
            case lastDataUpdate = "last_data_update"
            case details
            case lastAvatarMod = "last_avatar_mod"
            case lastCoverMod = "last_cover_mod"
            case points
            case dailyPoints = "daily_points"
            case pointDayExpire = "point_day_expire"
            case lastFollowID = "last_follow_id"
            case shareMyData = "share_my_data"
            case lastLoginData = "last_login_data"
            case twoFactor = "two_factor"
            case newEmail = "new_email"
            case twoFactorVerified = "two_factor_verified"
            case newPhone = "new_phone"
            case infoFile = "info_file"
            case city, state, zip
            case schoolCompleted = "school_completed"
            case weatherUnit = "weather_unit"
            case avatarFull = "avatar_full"
            case url, name
            case mutualFriendsData = "mutual_friends_data"
            case lastseenUnixTime = "lastseen_unix_time"
            case lastseenStatus = "lastseen_status"
        }
    }

    // MARK: - PublisherDetails
    struct PublisherDetails: Codable {
        var postCount, albumCount, followingCount, followersCount: String?
        var groupsCount, likesCount: String?
        var mutualFriendsCount: MutualFriendsCount?

        enum CodingKeys: String, CodingKey {
            case postCount = "post_count"
            case albumCount = "album_count"
            case followingCount = "following_count"
            case followersCount = "followers_count"
            case groupsCount = "groups_count"
            case likesCount = "likes_count"
            case mutualFriendsCount = "mutual_friends_count"
        }
    }

    enum MutualFriendsCount: Codable {
        case bool(Bool)
        case integer(Int)

        init(from decoder: Decoder) throws {
            let container = try decoder.singleValueContainer()
            if let x = try? container.decode(Bool.self) {
                self = .bool(x)
                return
            }
            if let x = try? container.decode(Int.self) {
                self = .integer(x)
                return
            }
            throw DecodingError.typeMismatch(MutualFriendsCount.self, DecodingError.Context(codingPath: decoder.codingPath, debugDescription: "Wrong type for MutualFriendsCount"))
        }

        func encode(to encoder: Encoder) throws {
            var container = encoder.singleValueContainer()
            switch self {
            case .bool(let x):
                try container.encode(x)
            case .integer(let x):
                try container.encode(x)
            }
        }
    }

    enum PublisherLanguage: String, Codable {
        case arabic = "arabic"
        case english = "english"
        case french = "french"
    }

    enum MutualFriendsData: Codable {
        case string(String)
        case stringArray([String])

        init(from decoder: Decoder) throws {
            let container = try decoder.singleValueContainer()
            if let x = try? container.decode([String].self) {
                self = .stringArray(x)
                return
            }
            if let x = try? container.decode(String.self) {
                self = .string(x)
                return
            }
            throw DecodingError.typeMismatch(MutualFriendsData.self, DecodingError.Context(codingPath: decoder.codingPath, debugDescription: "Wrong type for MutualFriendsData"))
        }

        func encode(to encoder: Encoder) throws {
            var container = encoder.singleValueContainer()
            switch self {
            case .string(let x):
                try container.encode(x)
            case .stringArray(let x):
                try container.encode(x)
            }
        }
    }

    enum PublisherPostPrivacy: String, Codable {
        case ifollow = "ifollow"
        case nobody = "nobody"
    }

    enum PublisherRegistered: String, Codable {
        case the22020 = "2/2020"
        case the32020 = "3/2020"
        case the42018 = "4/2018"
    }

    // MARK: - Reaction
    struct Reaction: Codable {
        var like, love, haHa, wow: Int?
        var sad, angry: Int?
        var isReacted: Bool?
        var type: TypeEnum?
        var count: Int?

        enum CodingKeys: String, CodingKey {
            case like = "Like"
            case love = "Love"
            case haHa = "HaHa"
            case wow = "Wow"
            case sad = "Sad"
            case angry = "Angry"
            case isReacted = "is_reacted"
            case type, count
        }
    }

    enum TypeEnum: String, Codable {
        case empty = ""
        case like = "Like"
        case love = "Love"
    }

    enum PostDataRegistered: String, Codable {
        case the32019 = "3/2019"
        case the32020 = "3/2020"
    }

    // MARK: - Encode/decode helpers

    class JSONNull: Codable, Hashable {

        public static func == (lhs: JSONNull, rhs: JSONNull) -> Bool {
            return true
        }

        public var hashValue: Int {
            return 0
        }

        public init() {}

        public required init(from decoder: Decoder) throws {
            let container = try decoder.singleValueContainer()
            if !container.decodeNil() {
                throw DecodingError.typeMismatch(JSONNull.self, DecodingError.Context(codingPath: decoder.codingPath, debugDescription: "Wrong type for JSONNull"))
            }
        }

        public func encode(to encoder: Encoder) throws {
            var container = encoder.singleValueContainer()
            try container.encodeNil()
        }
    }

    class JSONCodingKey: CodingKey {
        let key: String

        required init?(intValue: Int) {
            return nil
        }

        required init?(stringValue: String) {
            key = stringValue
        }

        var intValue: Int? {
            return nil
        }

        var stringValue: String {
            return key
        }
    }

    class JSONAny: Codable {

        let value: Any

        static func decodingError(forCodingPath codingPath: [CodingKey]) -> DecodingError {
            let context = DecodingError.Context(codingPath: codingPath, debugDescription: "Cannot decode JSONAny")
            return DecodingError.typeMismatch(JSONAny.self, context)
        }

        static func encodingError(forValue value: Any, codingPath: [CodingKey]) -> EncodingError {
            let context = EncodingError.Context(codingPath: codingPath, debugDescription: "Cannot encode JSONAny")
            return EncodingError.invalidValue(value, context)
        }

        static func decode(from container: SingleValueDecodingContainer) throws -> Any {
            if let value = try? container.decode(Bool.self) {
                return value
            }
            if let value = try? container.decode(Int64.self) {
                return value
            }
            if let value = try? container.decode(Double.self) {
                return value
            }
            if let value = try? container.decode(String.self) {
                return value
            }
            if container.decodeNil() {
                return JSONNull()
            }
            throw decodingError(forCodingPath: container.codingPath)
        }

        static func decode(from container: inout UnkeyedDecodingContainer) throws -> Any {
            if let value = try? container.decode(Bool.self) {
                return value
            }
            if let value = try? container.decode(Int64.self) {
                return value
            }
            if let value = try? container.decode(Double.self) {
                return value
            }
            if let value = try? container.decode(String.self) {
                return value
            }
            if let value = try? container.decodeNil() {
                if value {
                    return JSONNull()
                }
            }
            if var container = try? container.nestedUnkeyedContainer() {
                return try decodeArray(from: &container)
            }
            if var container = try? container.nestedContainer(keyedBy: JSONCodingKey.self) {
                return try decodeDictionary(from: &container)
            }
            throw decodingError(forCodingPath: container.codingPath)
        }

        static func decode(from container: inout KeyedDecodingContainer<JSONCodingKey>, forKey key: JSONCodingKey) throws -> Any {
            if let value = try? container.decode(Bool.self, forKey: key) {
                return value
            }
            if let value = try? container.decode(Int64.self, forKey: key) {
                return value
            }
            if let value = try? container.decode(Double.self, forKey: key) {
                return value
            }
            if let value = try? container.decode(String.self, forKey: key) {
                return value
            }
            if let value = try? container.decodeNil(forKey: key) {
                if value {
                    return JSONNull()
                }
            }
            if var container = try? container.nestedUnkeyedContainer(forKey: key) {
                return try decodeArray(from: &container)
            }
            if var container = try? container.nestedContainer(keyedBy: JSONCodingKey.self, forKey: key) {
                return try decodeDictionary(from: &container)
            }
            throw decodingError(forCodingPath: container.codingPath)
        }

        static func decodeArray(from container: inout UnkeyedDecodingContainer) throws -> [Any] {
            var arr: [Any] = []
            while !container.isAtEnd {
                let value = try decode(from: &container)
                arr.append(value)
            }
            return arr
        }

        static func decodeDictionary(from container: inout KeyedDecodingContainer<JSONCodingKey>) throws -> [String: Any] {
            var dict = [String: Any]()
            for key in container.allKeys {
                let value = try decode(from: &container, forKey: key)
                dict[key.stringValue] = value
            }
            return dict
        }

        static func encode(to container: inout UnkeyedEncodingContainer, array: [Any]) throws {
            for value in array {
                if let value = value as? Bool {
                    try container.encode(value)
                } else if let value = value as? Int64 {
                    try container.encode(value)
                } else if let value = value as? Double {
                    try container.encode(value)
                } else if let value = value as? String {
                    try container.encode(value)
                } else if value is JSONNull {
                    try container.encodeNil()
                } else if let value = value as? [Any] {
                    var container = container.nestedUnkeyedContainer()
                    try encode(to: &container, array: value)
                } else if let value = value as? [String: Any] {
                    var container = container.nestedContainer(keyedBy: JSONCodingKey.self)
                    try encode(to: &container, dictionary: value)
                } else {
                    throw encodingError(forValue: value, codingPath: container.codingPath)
                }
            }
        }

        static func encode(to container: inout KeyedEncodingContainer<JSONCodingKey>, dictionary: [String: Any]) throws {
            for (key, value) in dictionary {
                let key = JSONCodingKey(stringValue: key)!
                if let value = value as? Bool {
                    try container.encode(value, forKey: key)
                } else if let value = value as? Int64 {
                    try container.encode(value, forKey: key)
                } else if let value = value as? Double {
                    try container.encode(value, forKey: key)
                } else if let value = value as? String {
                    try container.encode(value, forKey: key)
                } else if value is JSONNull {
                    try container.encodeNil(forKey: key)
                } else if let value = value as? [Any] {
                    var container = container.nestedUnkeyedContainer(forKey: key)
                    try encode(to: &container, array: value)
                } else if let value = value as? [String: Any] {
                    var container = container.nestedContainer(keyedBy: JSONCodingKey.self, forKey: key)
                    try encode(to: &container, dictionary: value)
                } else {
                    throw encodingError(forValue: value, codingPath: container.codingPath)
                }
            }
        }

        static func encode(to container: inout SingleValueEncodingContainer, value: Any) throws {
            if let value = value as? Bool {
                try container.encode(value)
            } else if let value = value as? Int64 {
                try container.encode(value)
            } else if let value = value as? Double {
                try container.encode(value)
            } else if let value = value as? String {
                try container.encode(value)
            } else if value is JSONNull {
                try container.encodeNil()
            } else {
                throw encodingError(forValue: value, codingPath: container.codingPath)
            }
        }

        public required init(from decoder: Decoder) throws {
            if var arrayContainer = try? decoder.unkeyedContainer() {
                self.value = try JSONAny.decodeArray(from: &arrayContainer)
            } else if var container = try? decoder.container(keyedBy: JSONCodingKey.self) {
                self.value = try JSONAny.decodeDictionary(from: &container)
            } else {
                let container = try decoder.singleValueContainer()
                self.value = try JSONAny.decode(from: container)
            }
        }

        public func encode(to encoder: Encoder) throws {
            if let arr = self.value as? [Any] {
                var container = encoder.unkeyedContainer()
                try JSONAny.encode(to: &container, array: arr)
            } else if let dict = self.value as? [String: Any] {
                var container = encoder.container(keyedBy: JSONCodingKey.self)
                try JSONAny.encode(to: &container, dictionary: dict)
            } else {
                var container = encoder.singleValueContainer()
                try JSONAny.encode(to: &container, value: self.value)
            }
        }
    }
}
extension LastActivitiesModel.LastActivitiesSuccessModel{
   init(json : [String:Any]) {
    let api_status = json["api_status"] as? Int
    let activities = json["activities"] as? [[String:Any]]
    self.api_status = api_status ?? 0
    self.activities = activities ?? [["PostType" : "Profile_Pic"]]
    }
}

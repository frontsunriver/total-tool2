

import Foundation
class FetchUserModel{
    struct FetchUserSuccessModel: Codable {
        
        var apiStatus: Int?
        var userData: UserData?

        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case userData = "user_data"
        }
    }
    struct FetchUserErrorModel: Codable {
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
    // MARK: - UserData
    struct UserData: Codable {
        var userID, username, email, firstName: String?
        var lastName: String?
        var avatar, cover: String?
        var backgroundImage, relationshipID, address, working: String?
        var workingLink, about, school, gender: String?
        var birthday, countryID, website, facebook: String?
        var google, twitter, linkedin, youtube: String?
        var vk, instagram, language, ipAddress: String?
        var followPrivacy, friendPrivacy, postPrivacy, messagePrivacy: String?
        var confirmFollowers, showActivitiesPrivacy, birthPrivacy, visitPrivacy: String?
        var verified, lastseen, emailNotification, eLiked: String?
        var eWondered, eShared, eFollowed, eCommented: String?
        var eVisited, eLikedPage, eMentioned, eJoinedGroup: String?
        var eAccepted, eProfileWallPost, eSentmeMsg, eLastNotif: String?
        var notificationSettings, status, active, admin: String?
        var registered, phoneNumber, isPro, proType: String?
        var timezone, referrer, refUserID, balance: String?
        var paypalEmail, notificationsSound, orderPostsBy, androidMDeviceID: String?
        var iosMDeviceID, androidNDeviceID, iosNDeviceID, webDeviceID: String?
        var wallet, lat, lng, lastLocationUpdate: String?
        var shareMyLocation, lastDataUpdate: String?
        var details: Details?
        var lastAvatarMod, lastCoverMod, points, dailyPoints: String?
        var pointDayExpire, lastFollowID, shareMyData, lastLoginData: String?
        var twoFactor, newEmail, twoFactorVerified, newPhone: String?
        var infoFile, city, state, zip: String?
        var schoolCompleted, weatherUnit, avatarFull: String?
        var url: String?
        var name: String?
        var mutualFriendsData: [String]?
        var lastseenUnixTime, lastseenStatus: String?
        var isFollowing, canFollow, isFollowingMe: Int?
        var genderText, lastseenTimeText: String?
        var isBlocked: Bool?

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
            case isFollowing = "is_following"
            case canFollow = "can_follow"
            case isFollowingMe = "is_following_me"
            case genderText = "gender_text"
            case lastseenTimeText = "lastseen_time_text"
            case isBlocked = "is_blocked"
        }
    }

    // MARK: - Details
    struct Details: Codable {
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

}

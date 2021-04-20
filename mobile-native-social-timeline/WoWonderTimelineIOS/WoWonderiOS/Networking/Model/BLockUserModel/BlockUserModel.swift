

import Foundation
class GetBlockuserModel{
    
    struct GetBlockuserSuccessModel {
           var api_status: Int
           var blocked_users: [[String:Any]]
       }
    struct GetBlockuserErrorModel: Codable {
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

    // MARK: - BlockedUser
    struct BlockedUser: Codable {
        var userID, username, email, firstName: String?
        var lastName: String?
        var avatar: String?
        var cover: String?
        var backgroundImage, relationshipID, address, working: String?
        var workingLink: String?
        var about, school: String?
        var gender: Gender?
        var birthday, countryID: String?
        var website: String?
        var facebook, google, twitter: Facebook?
        var linkedin: String?
        var youtube, vk, instagram: Facebook?
        var language, ipAddress, followPrivacy, friendPrivacy: String?
        var postPrivacy: PostPrivacy?
        var messagePrivacy, confirmFollowers, showActivitiesPrivacy, birthPrivacy: String?
        var visitPrivacy, verified, lastseen, emailNotification: String?
        var eLiked, eWondered, eShared, eFollowed: String?
        var eCommented, eVisited, eLikedPage, eMentioned: String?
        var eJoinedGroup, eAccepted, eProfileWallPost, eSentmeMsg: String?
        var eLastNotif, notificationSettings, status, active: String?
        var admin, registered: String?
        var phoneNumber: PhoneNumber?
        var isPro, proType: String?
        var timezone: Timezone?
        var referrer, refUserID, balance: String?
        var paypalEmail: PaypalEmail?
        var notificationsSound, orderPostsBy, androidMDeviceID, iosMDeviceID: String?
        var androidNDeviceID, iosNDeviceID, webDeviceID, wallet: String?
        var lat, lng, lastLocationUpdate, shareMyLocation: String?
        var lastDataUpdate: String?
        var details: Details?
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
        var genderText: GenderText?
        var lastseenTimeText: String?

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
            case genderText = "gender_text"
            case lastseenTimeText = "lastseen_time_text"
        }
    }

    // MARK: - Details
    struct Details: Codable {
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

    enum Facebook: String, Codable {
        case empty = ""
        case facebookEmpty = "Empty"
        case yxNPZADPBWEhoMmpwY2PPaHI1Sl9UcG04ClNodi1ZAZAEJKNHI = "YXNpZADpBWEhoMmpwY2pPaHI1Sl9ucG04clNodi1ZAZAEJKNHI"
    }

    enum Gender: String, Codable {
        case female = "female"
        case male = "male"
    }

    enum GenderText: String, Codable {
        case female = "Female"
        case male = "Male"
    }

    enum LastseenStatus: String, Codable {
        case off = "off"
        case on = "on"
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

    enum PaypalEmail: String, Codable {
        case abdalkneamarGmailCOM = "abdalkneamar@gmail.com"
        case deepakhawxGmailCOM = "deepakhawx@gmail.com"
        case empty = ""
    }

    enum PhoneNumber: String, Codable {
        case empty = ""
        case the60192711515 = "+60192711515"
        case the6588874652526 = "6588874652526"
    }

    enum PostPrivacy: String, Codable {
        case everyone = "everyone"
        case ifollow = "ifollow"
    }

    enum Timezone: String, Codable {
        case asiaBeirut = "Asia/Beirut"
        case utc = "UTC"
    }

    enum WeatherUnit: String, Codable {
        case uk = "uk"
        case us = "us"
    }
}
extension GetBlockuserModel.GetBlockuserSuccessModel{
    init(json : [String:Any]) {
    let api_status = json["api_status"] as? Int
    let data = json["blocked_users"] as? [[String:Any]]
    self.api_status = api_status ?? 0
    self.blocked_users = data ?? [["PostType" : "Profile_Pic"]]
    }
}

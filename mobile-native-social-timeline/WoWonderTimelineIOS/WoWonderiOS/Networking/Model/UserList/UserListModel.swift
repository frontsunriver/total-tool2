

import Foundation

class UserListModel{
    
     
    struct UserListSuccessModel: Codable {
        var apiStatus: Int?
        var data: DataClass?

        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case data
        }
    }
    struct UserListErrorModel: Codable {
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


    
    // MARK: - DataClass
    struct DataClass: Codable {
        var following, followers: [Follow]?
    }

    // MARK: - Follow
    struct Follow: Codable {
        var userID, username, email, firstName: String?
        var lastName: String?
        var avatar: String?
        var cover: String?
        var backgroundImage, relationshipID: String?
        var address: String?
        var working: String?
        var workingLink: String?
        var about: String?
        var school: String?
        var gender: String?
        var birthday, countryID: String?
        var website: String?
        var facebook: String?
        var google, twitter: String?
        var linkedin: String?
        var youtube: String?
        var vk, instagram: String?
        var language, ipAddress, followPrivacy, friendPrivacy: String?
        var postPrivacy: String?
        var messagePrivacy, confirmFollowers, showActivitiesPrivacy, birthPrivacy: String?
        var visitPrivacy, verified: String?
        var lastseen: String?
        var emailNotification, eLiked, eWondered, eShared: String?
        var eFollowed, eCommented, eVisited, eLikedPage: String?
        var eMentioned, eJoinedGroup, eAccepted, eProfileWallPost: String?
        var eSentmeMsg, eLastNotif, notificationSettings, status: String?
        var active, admin, registered, phoneNumber: String?
        var isPro, proType: String?
        var timezone: String?
        var referrer, refUserID, balance, paypalEmail: String?
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
        var weatherUnit: String?
        var avatarFull: String?
        var url: String?
        var name: String?
        var mutualFriendsData: MutualFriendsData?
        var lastseenUnixTime: String?
        var lastseenStatus: String?
        var familyMember, lastseenTimeText: String?
        var userPlatform: UserPlatform?
        var isFollowing: Int?

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
            case familyMember = "family_member"
            case lastseenTimeText = "lastseen_time_text"
            case userPlatform = "user_platform"
            case isFollowing = "is_following"
        }
    }

    enum Address: String, Codable {
        case ankaraTürkiye = "Ankara/Türkiye"
        case avcılarİstanbulTurkey = "Avcılar/İstanbul, Turkey"
        case empty = ""
        case i̇stanbulTürkiye = "İstanbul, Türkiye"
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
        case deendoughouz = "deendoughouz"
        case empty = ""
        case waelwael = "waelwael"
    }

    enum Gender: String, Codable {
        case female = "female"
        case male = "male"
    }

    enum Google: String, Codable {
        case empty = ""
        case mhwaelanjo = "mhwaelanjo"
    }

    enum Lastseen: String, Codable {
        case off = "off"
    }

    enum Linkedin: String, Codable {
        case empty = ""
        case wael = "wael"
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

    enum PostPrivacy: String, Codable {
        case everyone = "everyone"
        case ifollow = "ifollow"
    }

    enum School: String, Codable {
        case empty = ""
        case girneAmerikanÜniversitesi = "Girne Amerikan Üniversitesi"
        case php = "PHP"
        case schoolEmpty = "Empty"
    }

    enum Timezone: String, Codable {
        case europeIstanbul = "Europe/Istanbul"
        case utc = "UTC"
    }

    enum UserPlatform: String, Codable {
        case phone = "phone"
        case web = "web"
    }

    enum WeatherUnit: String, Codable {
        case uk = "uk"
        case us = "us"
    }

    enum Working: String, Codable {
        case danışman = "Danışman"
        case empty = ""
        case scriptsun = "Scriptsun"
        case woWonder = "WoWonder"
    }

    enum Youtube: String, Codable {
        case channelUCMJYhKckKQYUgiefU9MA = "channel/UCm_jYhKckK_QY-ugiefU9mA"
        case empty = ""
        case youtubeEmpty = "Empty"
    }
      
        
}

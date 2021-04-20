
import Foundation
class GetNotificationModel{
   


    // MARK: - Welcome
//    struct GetNotificationSuccessModel: Codable {
//        var apiStatus: Int?
//        var notifications: [Notification]?
//        var newNotificationsCount: String?
//
//        enum CodingKeys: String, CodingKey {
//            case apiStatus = "api_status"
//            case notifications
//            case newNotificationsCount = "new_notifications_count"
//        }
//    }
    struct GetNotificationSuccessModel{
        var apiStatus: Int
        var notifications: [[String:Any]]
        var new_notifications_count: String
    }
    struct GetNotificationErrorModel: Codable {
           let apiStatus: String?
           let errors: Errors?
           
           enum CodingKeys: String, CodingKey {
               case apiStatus = "api_status"
               case errors
           }
       }
       
       struct Errors: Codable {
           let errorID: Int?
           let errorText: String?
           
           enum CodingKeys: String, CodingKey {
               case errorID = "error_id"
               case errorText = "error_text"
           }
       }

    // MARK: - Notification
    struct Notification: Codable {
        var id, notifierID, recipientID, postID: String?
        var replyID, commentID, pageID, groupID: String?
        var groupChatID, eventID, threadID, blogID: String?
        var storyID, seenPop, type: String?
        var type2: Type2?
        var text: Text?
        var url: String?
        var fullLink, seen, sentPush, time: String?
        var notifier: Notifier?
        var ajaxURL, typeText, icon, timeTextString: String?
        var timeText: String?

        enum CodingKeys: String, CodingKey {
            case id
            case notifierID = "notifier_id"
            case recipientID = "recipient_id"
            case postID = "post_id"
            case replyID = "reply_id"
            case commentID = "comment_id"
            case pageID = "page_id"
            case groupID = "group_id"
            case groupChatID = "group_chat_id"
            case eventID = "event_id"
            case threadID = "thread_id"
            case blogID = "blog_id"
            case storyID = "story_id"
            case seenPop = "seen_pop"
            case type, type2, text, url
            case fullLink = "full_link"
            case seen
            case sentPush = "sent_push"
            case time, notifier
            case ajaxURL = "ajax_url"
            case typeText = "type_text"
            case icon
            case timeTextString = "time_text_string"
            case timeText = "time_text"
        }
    }

    // MARK: - Notifier
    struct Notifier: Codable {
        var userID, username, email, firstName: String?
        var lastName: String?
        var avatar: String?
        var cover: String?
        var backgroundImage, relationshipID: String?
        var address: Address?
        var working: Working?
        var workingLink, about: String?
        var school: School?
        var gender: Gender?
        var birthday, countryID, website, facebook: String?
        var google, twitter, linkedin, youtube: String?
        var vk, instagram: String?
        var language: Language?
        var ipAddress, followPrivacy, friendPrivacy: String?
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
        var weatherUnit: WeatherUnit?
        var url: String?
        var name: String?
        var mutualFriendsData: MutualFriendsData?
        var lastseenUnixTime: String?
        var lastseenStatus: LastseenStatus?
        var avatarFull: String?

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
            case url, name
            case mutualFriendsData = "mutual_friends_data"
            case lastseenUnixTime = "lastseen_unix_time"
            case lastseenStatus = "lastseen_status"
            case avatarFull = "avatar_full"
        }
    }

    enum Address: String, Codable {
        case ankaraTürkiye = "Ankara/Türkiye"
        case empty = ""
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

    enum Gender: String, Codable {
        case male = "male"
    }

    enum Language: String, Codable {
        case arabic = "arabic"
        case english = "english"
        case turkish = "turkish"
    }

    enum LastseenStatus: String, Codable {
        case off = "off"
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

    enum PhoneNumber: String, Codable {
        case empty = ""
        case the0460206684 = "0460206684"
        case the905331438062 = "+905331438062"
    }

    enum PostPrivacy: String, Codable {
        case everyone = "everyone"
        case ifollow = "ifollow"
    }

    enum School: String, Codable {
        case empty = ""
        case girneAmerikanÜniversitesi = "Girne Amerikan Üniversitesi"
    }

    enum Timezone: String, Codable {
        case africaCairo = "Africa/Cairo"
        case asiaKolkata = "Asia/Kolkata"
        case utc = "UTC"
    }

    enum WeatherUnit: String, Codable {
        case us = "us"
    }

    enum Working: String, Codable {
        case danışman = "Danışman"
        case empty = ""
    }

    enum Text: String, Codable {
        case comment = "comment"
        case empty = ""
        case post = "post"
    }

    enum Type2: String, Codable {
        case empty = ""
        case like = "Like"
        case postAvatar = "post_avatar"
    }

}

extension GetNotificationModel.GetNotificationSuccessModel{
   init(json : [String:Any]) {
    let api_status = json["api_status"] as? Int
    let notifications = json["notifications"] as? [[String:Any]]
    let noiti_count = json["new_notifications_count"] as? String
    self.apiStatus = api_status ?? 0
    self.notifications = notifications ?? [["PostType" : "Profile_Pic"]]
    self.new_notifications_count = noiti_count ?? ""
//    self.api_status = api_status ?? 0
//    self.data = data ?? [["PostType" : "Profile_Pic"]]
    }
}

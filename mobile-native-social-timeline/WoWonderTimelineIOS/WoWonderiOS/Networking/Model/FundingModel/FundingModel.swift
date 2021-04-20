

import Foundation
class GetFundingModel{
    struct GetFundingSuccessModel: Codable {
        var apiStatus: Int?
        var data: [Datum]?

        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case data
        }
    }
    struct GetFundingErrorModel:Codable{
        let apiStatus: String?
        let errors: Errors?
        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case errors
        }
    }
    
    // MARK: - Errors
    struct Errors: Codable {
        let errorID: Int?
        let errorText: String?
        
        enum CodingKeys: String, CodingKey {
            case errorID = "error_id"
            case errorText = "error_text"
        }
    }

    // MARK: - Datum
    struct Datum: Codable {
        var id: Int?
        var hashedID, title, datumDescription, amount: String?
        var userID: Int?
        var image: String?
        var time: String?
        var raised, bar: Int?
        var userData: UserData?

        enum CodingKeys: String, CodingKey {
            case id
            case hashedID = "hashed_id"
            case title
            case datumDescription = "description"
            case amount
            case userID = "user_id"
            case image, time, raised, bar
            case userData = "user_data"
        }
    }

    // MARK: - UserData
    struct UserData: Codable {
        var userID, username, email, firstName: String?
        var lastName: String?
        var avatar, cover: String?
        
       
        
       

        enum CodingKeys: String, CodingKey {
            case userID = "user_id"
            case username, email
            case firstName = "first_name"
            case lastName = "last_name"
            case avatar, cover
            
         
          
           
        }
    }

    // MARK: - Details
    struct Details: Codable {
        var postCount, albumCount, followingCount, followersCount: String?
        var groupsCount, likesCount: String?
        var mutualFriendsCount: Bool?

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

}
class CreateFundingModel{
    struct CreateFundingSuccessModel: Codable {
        var apiStatus: Int?
        var data: DataClass?

        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case data
        }
    }

    struct CreateFundingErrorModel:Codable{
        let apiStatus: String?
        let errors: Errors?
        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case errors
        }
    }
    
    // MARK: - Errors
    struct Errors: Codable {
        let errorID: Int?
        let errorText: String?
        
        enum CodingKeys: String, CodingKey {
            case errorID = "error_id"
            case errorText = "error_text"
        }
    }
    // MARK: - DataClass
    struct DataClass: Codable {
        var id, postID, userID, recipientID: String?
        var postText, pageID, groupID, eventID: String?
        var pageEventID, postLink: String?
        var postLinkTitle: JSONNull?
        var postLinkImage, postLinkContent, postVimeo, postDailymotion: String?
        var postFacebook, postFile, postRecord: String?
        var postSticker: JSONNull?
        var sharedFrom: Bool?
        var postURL: JSONNull?
        var parentID, cache, commentsStatus, blur: String?
        var colorID, jobID, offerID, fundRaiseID: String?
        var fundID, active, streamName, liveTime: String?
        var liveEnded, postFileName, postFileThumb, postYoutube: String?
        var postVine, postSoundCloud, postPlaytube, postDeepsound: String?
        var postMap, postShare, postPrivacy, postType: String?
        var postFeeling, postListening, postTraveling, postWatching: String?
        var postPlaying, postPhoto, time, registered: String?
        var albumName, multiImage, multiImagePost, boosted: String?
        var productID, pollID, blogID, forumID: String?
        var threadID, videoViews: String?
        var publisher: Publisher?
        var limitComments: Int?
        var limitedComments, isGroupPost, groupRecipientExists, groupAdmin: Bool?
        var postIsPromoted: Int?
        var postTextAPI, orginaltext, postTime: String?
        var page: Int?
        var url: String?
        var viaType: String?
        var recipientExists: Bool?
        var recipient: String?
        var admin: Bool?
        var dataPostShare: String?
        var isPostSaved, isPostReported: Bool?
        var isPostBoosted: Int?
        var isLiked, isWondered: Bool?
        var postComments, postShares, postLikes, postWonders: String?
        var isPostPinned: Bool?
        var getPostComments, photoAlbum, options: [JSONAny]?
        var votedID: Int?
        var postFileFull: String?
        var reaction: Reaction?
        var job, offer, fund: [JSONAny]?
        var fundData: FundData?
        var forum, thread: [JSONAny]?
        var isStillLive: Bool?
        var liveSubUsers: Int?

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
            case offerID = "offer_id"
            case fundRaiseID = "fund_raise_id"
            case fundID = "fund_id"
            case active
            case streamName = "stream_name"
            case liveTime = "live_time"
            case liveEnded = "live_ended"
            case postFileName, postFileThumb, postYoutube, postVine, postSoundCloud, postPlaytube, postDeepsound, postMap, postShare, postPrivacy, postType, postFeeling, postListening, postTraveling, postWatching, postPlaying, postPhoto, time, registered
            case albumName = "album_name"
            case multiImage = "multi_image"
            case multiImagePost = "multi_image_post"
            case boosted
            case productID = "product_id"
            case pollID = "poll_id"
            case blogID = "blog_id"
            case forumID = "forum_id"
            case threadID = "thread_id"
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
            case dataPostShare = "post_share"
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
            case getPostComments = "get_post_comments"
            case photoAlbum = "photo_album"
            case options
            case votedID = "voted_id"
            case postFileFull = "postFile_full"
            case reaction, job, offer, fund
            case fundData = "fund_data"
            case forum, thread
            case isStillLive = "is_still_live"
            case liveSubUsers = "live_sub_users"
        }
    }

    // MARK: - FundData
    struct FundData: Codable {
        var id: Int?
        var hashedID, title, fundDataDescription, amount: String?
        var userID: Int?
        var image: String?
        var time: String?
        var raised, allDonation, bar: Int?

        enum CodingKeys: String, CodingKey {
            case id
            case hashedID = "hashed_id"
            case title
            case fundDataDescription = "description"
            case amount
            case userID = "user_id"
            case image, time, raised
            case allDonation = "all_donation"
            case bar
        }
    }

    // MARK: - Publisher
    struct Publisher: Codable {
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
        var schoolCompleted, weatherUnit, paystackRef, userPlatform: String?
        var url: String?
        var name: String?
        var mutualFriendsData: [String]?
        var lastseenUnixTime, lastseenStatus: String?

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
            case paystackRef = "paystack_ref"
            case userPlatform = "user_platform"
            case url, name
            case mutualFriendsData = "mutual_friends_data"
            case lastseenUnixTime = "lastseen_unix_time"
            case lastseenStatus = "lastseen_status"
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

    // MARK: - Reaction
    struct Reaction: Codable {
        var like, love, haHa, wow: Int?
        var sad, angry: Int?
        var isReacted: Bool?
        var type: String?
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


import Foundation


class Get_Site_SettingModel {
  
    
    struct siteSetting_SuccessModal{
        let apiStatus: Int
        let config: [String:Any]
        init(json: [String:Any]) {
            let apiStatus = json["apiStatus"] as? Int
            let config = json["config"] as? [String:Any]
            self.apiStatus = apiStatus ?? 0
            self.config = config ?? ["":""]
        }
        
    }
    
    // MARK: - Welcome
    struct Site_Setting_SuccessModel: Codable {
        let apiStatus: Int?
        let config: Config?

        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case config
        }    }
    
    // MARK: - Config
    struct Config: Codable {
        let siteName, siteTitle, siteKeywords, siteDesc: String?
        let siteEmail, defualtLang, emailValidation, emailNotification: String?
        let fileSharing, seoLink, cacheSystem, chatSystem: String?
        let useSEOFrindly, reCAPTCHA, reCAPTCHAKey, userLastseen: String?
        let age, deleteAccount, connectivitySystem, profileVisit: String?
        let maxUpload, maxCharacters, messageSeen, messageTyping: String?
        let googleMapAPI, allowedExtenstion, censoredWords, googleAnalytics: String?
        let allLogin, googleLogin, facebookLogin, twitterLogin: String?
        let linkedinLogin, vkontakteLogin, facebookAppID, facebookAppKey: String?
        let googleAppID, googleAppKey, twitterAppID, twitterAppKey: String?
        let linkedinAppID, linkedinAppKey, vkontakteAppID, vkontakteAppKey: String?
        let theme, secondPostButton, instagramAppID, instagramAppkey: String?
        let instagramLogin, headerBackground, headerHoverBorder, headerColor: String?
        let bodyBackground, btnColor, btnBackgroundColor, btnHoverColor: String?
        let btnHoverBackgroundColor, settingHeaderColor, settingHeaderBackground, settingActiveSidebarColor: String?
        let settingActiveSidebarBackground, settingSidebarBackground, settingSidebarColor, logoExtension: String?
        let onlineSidebar, backgroundExtension, profilePrivacy, videoUpload: String?
        let audioUpload, smtpOrMail, smtpUsername, smtpHost: String?
        let smtpPassword, smtpPort, smtpEncryption, smsOrEmail: String?
        let smsUsername, smsPassword, smsPhoneNumber, isOk: String?
        let pro, paypalID, paypalSecret, paypalMode: String?
        let weeklyPrice, monthlyPrice, yearlyPrice, lifetimePrice: String?
        let postLimit, userLimit, cssUpload, smoothLoading: String?
        let headerSearchColor, headerButtonShadow, currency, games: String?
        let lastBackup, pages, groups, orderPostsBy: String?
        let btnDisabled, developersPage, userRegistration, maintenanceMode: String?
        let videoChat, videoAccountSid, videoAPIKeySid, videoAPIKeySecret: String?
        let videoConfigurationProfileSid, eapi, faviconExtension, monthlyBoosts: String?
        let yearlyBoosts, lifetimeBoosts, chatOutgoingBackground, windowsAppVersion: String?
        let widnowsAppAPIID, widnowsAppAPIKey, stripeID, stripeSecret: String?
        let creditCard, bitcoin, mWithdrawal, amountRef: String?
        let affiliateType, affiliateSystem, classified, amazoneS3: String?
        let bucketName, amazoneS3Key, amazoneS3SKey, region: String?
        let footerBackground, isUtf8, alipay, smsTPhoneNumber: String?
        let audioChat, smsTwilioUsername, smsTwilioPassword, smsProvider: String?
        let footerTextColor, updatedLatest, classifiedCurrency, classifiedCurrencyS: String?
        let mimeTypes, footerBackground2, footerBackgroundN, blogs: String?
        let canBlogs, push, androidMPushID, androidMPushKey: String?
        let events, forum, lastUpdate, movies: String?
        let yandexTranslationAPI, updateDB15, adVPrice, adCPrice: String?
        let emoCDN: String?
        let userAds, userStatus, dateStyle, stickers: String?
        let giphyAPI, findFriends, updateDB152, scriptVersion: String?
        let androidPushNative, androidPushMessages, updateDB153, adsCurrency: String?
        let webPush: String?
        let playtubeURL: String?
        let connectivitySystemLimit, videoAdSkip, updateUserProfile, cacheSidebar: String?
        let androidNPushID, androidNPushKey, ftpHost, ftpPort: String?
        let ftpUsername, ftpPassword, ftpUpload, ftpEndpoint: String?
        let ftpPath, transactionLog, coinpaymentsSecret, coinpaymentsID: String?
        let infobipUsername, infobipPassword, updatev2, amountPercentRef: String?
        let giftSystem, socialShareTwitter, socialShareGoogle, socialShareFacebook: String?
        let socialShareWhatsup, socialSharePinterest, socialShareLinkedin, socialShareTelegram: String?
        let stickersSystem, dollarToPointCost, commentsPoint, likesPoint: String?
        let dislikesPoint, wondersPoint, reactionPoint, createpostPoint: String?
        let pointAllowWithdrawal, stickyVideoPlayer, pointLevelSystem, commentReports: String?
        let popularPosts, autoFriendUsers, spacesKey, spacesSecret: String?
        let spaceName, spaceRegion, spaces, watermark: String?
        let googleMap, loginAuth, twoFactor, twoFactorType: String?
        let lastNotificationDeleteRun, iosPushMessages, iosMPushID, iosMPushKey: String?
        let iosPushNative, iosNPushID, iosNPushKey, webPushID: String?
        let webPushKey, profileBack, freeDayLimit, adultImages: String?
        let adultImagesAction, adultImagesFile, proDayLimit, visionAPIKey: String?
        let recaptchaSecretKey, bankPayment, bankTransferNote, bankDescription: String?
        let createblogPoint: String?
        let deepsoundURL: String?
        let english, arabic, dutch, french: String?
        let german, italian, portuguese, russian: String?
        let spanish, turkish: String?
        let currencyArray: [String]?
        let currencySymbolArray: String?
        let paypalCurrency, stripeCurrency, the2CheckoutCurrency, version: String?
        let forumVisibility, eventsVisibility, productVisibility, paypal: String?
        let pokeSystem, afternoonSystem, providersArray, coloredPostsSystem: String?
        let checkoutPayment, checkoutMode, checkoutSellerID, checkoutPublishableKey: String?
        let checkoutPrivateKey, jobSystem, weatherWidget, commonThings: String?
        let fundingSystem, fundingRequest, donatePercentage, weatherKey: String?
        let postApproval, autoPageLike, autoGroupJoin, memoriesSystem: String?
        let membershipSystem, recurringPayment, whoUpload, whoCall: String?
        let blogApproval, refundSystem, paystackPayment, paystackSecretKey: String?
        let cashfreePayment, cashfreeClientKey, cashfreeSecretKey, msg91AuthKey: String?
        let offerSystem, nearbyShopSystem, nearbyBusinessSystem, preventSystem: String?
        let badLoginLimit, lockTime, passwordComplexitySystem, inviteLinksSystem: String?
        let userLinksLimit, expireUserLinks, shoutBoxSystem, bankWithdrawalSystem: String?
        let liveVideo, liveToken, liveAccountID, razorpayPayment: String?
        let razorpayKeyID, razorpayKeySecret, payseraPayment, payseraProjectID: String?
        let payseraSignPassword, payseraMode, cloudUpload, cloudFilePath: String?
        let cloudBucketName, liveVideoSave, notifyNewPost, agoraAppID: String?
        let agoraLiveVideo, millicastLiveVideo, agoraCustomerID, agoraCustomerCertificate: String?
        let yahooConsumerKey, yahooConsumerSecret, cashfreeMode, amazoneS32: String?
        let bucketName2, amazoneS3Key2, amazoneS3SKey2, region2: String?
        let logoURL: String
        let pageCategories, groupCategories, blogCategories, productsCategories: [String: String]?
        let jobCategories: [String: String]?
        let genders: String?
        let family: [String: String]?
        let fields: [Field]?
        let movieCategory: MovieCategory?
        let postColors: [String: PostColor]?
        let pageSubCategories, groupSubCategories: SubCategories?
        let productsSubCategories, pageCustomFields, groupCustomFields, productCustomFields: [JSONAny]
        let postReactionsTypes: [String: PostReactionsType]
        let proPackages: ProPackages
        let proPackagesTypes: [String: String]?

        enum CodingKeys: String, CodingKey {
            case siteName, siteTitle, siteKeywords, siteDesc, siteEmail, defualtLang, emailValidation, emailNotification, fileSharing, seoLink, cacheSystem, chatSystem
            case useSEOFrindly = "useSeoFrindly"
            case reCAPTCHA = "reCaptcha"
            case reCAPTCHAKey = "reCaptchaKey"
            case userLastseen = "user_lastseen"
            case age, deleteAccount, connectivitySystem, profileVisit, maxUpload, maxCharacters
            case messageSeen = "message_seen"
            case messageTyping = "message_typing"
            case googleMapAPI = "google_map_api"
            case allowedExtenstion
            case censoredWords = "censored_words"
            case googleAnalytics
            case allLogin = "AllLogin"
            case googleLogin, facebookLogin, twitterLogin, linkedinLogin
            case vkontakteLogin = "VkontakteLogin"
            case facebookAppID = "facebookAppId"
            case facebookAppKey
            case googleAppID = "googleAppId"
            case googleAppKey
            case twitterAppID = "twitterAppId"
            case twitterAppKey
            case linkedinAppID = "linkedinAppId"
            case linkedinAppKey
            case vkontakteAppID = "VkontakteAppId"
            case vkontakteAppKey = "VkontakteAppKey"
            case theme
            case secondPostButton = "second_post_button"
            case instagramAppID = "instagramAppId"
            case instagramAppkey, instagramLogin
            case headerBackground = "header_background"
            case headerHoverBorder = "header_hover_border"
            case headerColor = "header_color"
            case bodyBackground = "body_background"
            case btnColor = "btn_color"
            case btnBackgroundColor = "btn_background_color"
            case btnHoverColor = "btn_hover_color"
            case btnHoverBackgroundColor = "btn_hover_background_color"
            case settingHeaderColor = "setting_header_color"
            case settingHeaderBackground = "setting_header_background"
            case settingActiveSidebarColor = "setting_active_sidebar_color"
            case settingActiveSidebarBackground = "setting_active_sidebar_background"
            case settingSidebarBackground = "setting_sidebar_background"
            case settingSidebarColor = "setting_sidebar_color"
            case logoExtension = "logo_extension"
            case onlineSidebar = "online_sidebar"
            case backgroundExtension = "background_extension"
            case profilePrivacy = "profile_privacy"
            case videoUpload = "video_upload"
            case audioUpload = "audio_upload"
            case smtpOrMail = "smtp_or_mail"
            case smtpUsername = "smtp_username"
            case smtpHost = "smtp_host"
            case smtpPassword = "smtp_password"
            case smtpPort = "smtp_port"
            case smtpEncryption = "smtp_encryption"
            case smsOrEmail = "sms_or_email"
            case smsUsername = "sms_username"
            case smsPassword = "sms_password"
            case smsPhoneNumber = "sms_phone_number"
            case isOk = "is_ok"
            case pro
            case paypalID = "paypal_id"
            case paypalSecret = "paypal_secret"
            case paypalMode = "paypal_mode"
            case weeklyPrice = "weekly_price"
            case monthlyPrice = "monthly_price"
            case yearlyPrice = "yearly_price"
            case lifetimePrice = "lifetime_price"
            case postLimit = "post_limit"
            case userLimit = "user_limit"
            case cssUpload = "css_upload"
            case smoothLoading = "smooth_loading"
            case headerSearchColor = "header_search_color"
            case headerButtonShadow = "header_button_shadow"
            case currency, games
            case lastBackup = "last_backup"
            case pages, groups
            case orderPostsBy = "order_posts_by"
            case btnDisabled = "btn_disabled"
            case developersPage = "developers_page"
            case userRegistration = "user_registration"
            case maintenanceMode = "maintenance_mode"
            case videoChat = "video_chat"
            case videoAccountSid = "video_accountSid"
            case videoAPIKeySid = "video_apiKeySid"
            case videoAPIKeySecret = "video_apiKeySecret"
            case videoConfigurationProfileSid = "video_configurationProfileSid"
            case eapi
            case faviconExtension = "favicon_extension"
            case monthlyBoosts = "monthly_boosts"
            case yearlyBoosts = "yearly_boosts"
            case lifetimeBoosts = "lifetime_boosts"
            case chatOutgoingBackground = "chat_outgoing_background"
            case windowsAppVersion = "windows_app_version"
            case widnowsAppAPIID = "widnows_app_api_id"
            case widnowsAppAPIKey = "widnows_app_api_key"
            case stripeID = "stripe_id"
            case stripeSecret = "stripe_secret"
            case creditCard = "credit_card"
            case bitcoin
            case mWithdrawal = "m_withdrawal"
            case amountRef = "amount_ref"
            case affiliateType = "affiliate_type"
            case affiliateSystem = "affiliate_system"
            case classified
            case amazoneS3 = "amazone_s3"
            case bucketName = "bucket_name"
            case amazoneS3Key = "amazone_s3_key"
            case amazoneS3SKey = "amazone_s3_s_key"
            case region
            case footerBackground = "footer_background"
            case isUtf8 = "is_utf8"
            case alipay
            case smsTPhoneNumber = "sms_t_phone_number"
            case audioChat = "audio_chat"
            case smsTwilioUsername = "sms_twilio_username"
            case smsTwilioPassword = "sms_twilio_password"
            case smsProvider = "sms_provider"
            case footerTextColor = "footer_text_color"
            case updatedLatest = "updated_latest"
            case classifiedCurrency = "classified_currency"
            case classifiedCurrencyS = "classified_currency_s"
            case mimeTypes = "mime_types"
            case footerBackground2 = "footer_background_2"
            case footerBackgroundN = "footer_background_n"
            case blogs
            case canBlogs = "can_blogs"
            case push
            case androidMPushID = "android_m_push_id"
            case androidMPushKey = "android_m_push_key"
            case events, forum
            case lastUpdate = "last_update"
            case movies
            case yandexTranslationAPI = "yandex_translation_api"
            case updateDB15 = "update_db_15"
            case adVPrice = "ad_v_price"
            case adCPrice = "ad_c_price"
            case emoCDN = "emo_cdn"
            case userAds = "user_ads"
            case userStatus = "user_status"
            case dateStyle = "date_style"
            case stickers
            case giphyAPI = "giphy_api"
            case findFriends = "find_friends"
            case updateDB152 = "update_db_152"
            case scriptVersion = "script_version"
            case androidPushNative = "android_push_native"
            case androidPushMessages = "android_push_messages"
            case updateDB153 = "update_db_153"
            case adsCurrency = "ads_currency"
            case webPush = "web_push"
            case playtubeURL = "playtube_url"
            case connectivitySystemLimit
            case videoAdSkip = "video_ad_skip"
            case updateUserProfile = "update_user_profile"
            case cacheSidebar = "cache_sidebar"
            case androidNPushID = "android_n_push_id"
            case androidNPushKey = "android_n_push_key"
            case ftpHost = "ftp_host"
            case ftpPort = "ftp_port"
            case ftpUsername = "ftp_username"
            case ftpPassword = "ftp_password"
            case ftpUpload = "ftp_upload"
            case ftpEndpoint = "ftp_endpoint"
            case ftpPath = "ftp_path"
            case transactionLog = "transaction_log"
            case coinpaymentsSecret = "coinpayments_secret"
            case coinpaymentsID = "coinpayments_id"
            case infobipUsername = "infobip_username"
            case infobipPassword = "infobip_password"
            case updatev2
            case amountPercentRef = "amount_percent_ref"
            case giftSystem = "gift_system"
            case socialShareTwitter = "social_share_twitter"
            case socialShareGoogle = "social_share_google"
            case socialShareFacebook = "social_share_facebook"
            case socialShareWhatsup = "social_share_whatsup"
            case socialSharePinterest = "social_share_pinterest"
            case socialShareLinkedin = "social_share_linkedin"
            case socialShareTelegram = "social_share_telegram"
            case stickersSystem = "stickers_system"
            case dollarToPointCost = "dollar_to_point_cost"
            case commentsPoint = "comments_point"
            case likesPoint = "likes_point"
            case dislikesPoint = "dislikes_point"
            case wondersPoint = "wonders_point"
            case reactionPoint = "reaction_point"
            case createpostPoint = "createpost_point"
            case pointAllowWithdrawal = "point_allow_withdrawal"
            case stickyVideoPlayer = "sticky_video_player"
            case pointLevelSystem = "point_level_system"
            case commentReports = "comment_reports"
            case popularPosts = "popular_posts"
            case autoFriendUsers = "auto_friend_users"
            case spacesKey = "spaces_key"
            case spacesSecret = "spaces_secret"
            case spaceName = "space_name"
            case spaceRegion = "space_region"
            case spaces, watermark
            case googleMap = "google_map"
            case loginAuth = "login_auth"
            case twoFactor = "two_factor"
            case twoFactorType = "two_factor_type"
            case lastNotificationDeleteRun = "last_notification_delete_run"
            case iosPushMessages = "ios_push_messages"
            case iosMPushID = "ios_m_push_id"
            case iosMPushKey = "ios_m_push_key"
            case iosPushNative = "ios_push_native"
            case iosNPushID = "ios_n_push_id"
            case iosNPushKey = "ios_n_push_key"
            case webPushID = "web_push_id"
            case webPushKey = "web_push_key"
            case profileBack = "profile_back"
            case freeDayLimit = "free_day_limit"
            case adultImages = "adult_images"
            case adultImagesAction = "adult_images_action"
            case adultImagesFile = "adult_images_file"
            case proDayLimit = "pro_day_limit"
            case visionAPIKey = "vision_api_key"
            case recaptchaSecretKey = "recaptcha_secret_key"
            case bankPayment = "bank_payment"
            case bankTransferNote = "bank_transfer_note"
            case bankDescription = "bank_description"
            case createblogPoint = "createblog_point"
            case deepsoundURL = "deepsound_url"
            case english, arabic, dutch, french, german, italian, portuguese, russian, spanish, turkish
            case currencyArray = "currency_array"
            case currencySymbolArray = "currency_symbol_array"
            case paypalCurrency = "paypal_currency"
            case stripeCurrency = "stripe_currency"
            case the2CheckoutCurrency = "2checkout_currency"
            case version
            case forumVisibility = "forum_visibility"
            case eventsVisibility = "events_visibility"
            case productVisibility = "product_visibility"
            case paypal
            case pokeSystem = "poke_system"
            case afternoonSystem = "afternoon_system"
            case providersArray = "providers_array"
            case coloredPostsSystem = "colored_posts_system"
            case checkoutPayment = "checkout_payment"
            case checkoutMode = "checkout_mode"
            case checkoutSellerID = "checkout_seller_id"
            case checkoutPublishableKey = "checkout_publishable_key"
            case checkoutPrivateKey = "checkout_private_key"
            case jobSystem = "job_system"
            case weatherWidget = "weather_widget"
            case commonThings = "common_things"
            case fundingSystem = "funding_system"
            case fundingRequest = "funding_request"
            case donatePercentage = "donate_percentage"
            case weatherKey = "weather_key"
            case postApproval = "post_approval"
            case autoPageLike = "auto_page_like"
            case autoGroupJoin = "auto_group_join"
            case memoriesSystem = "memories_system"
            case membershipSystem = "membership_system"
            case recurringPayment = "recurring_payment"
            case whoUpload = "who_upload"
            case whoCall = "Who_call"
            case blogApproval = "blog_approval"
            case refundSystem = "refund_system"
            case paystackPayment = "paystack_payment"
            case paystackSecretKey = "paystack_secret_key"
            case cashfreePayment = "cashfree_payment"
            case cashfreeClientKey = "cashfree_client_key"
            case cashfreeSecretKey = "cashfree_secret_key"
            case msg91AuthKey = "msg91_authKey"
            case offerSystem = "offer_system"
            case nearbyShopSystem = "nearby_shop_system"
            case nearbyBusinessSystem = "nearby_business_system"
            case preventSystem = "prevent_system"
            case badLoginLimit = "bad_login_limit"
            case lockTime = "lock_time"
            case passwordComplexitySystem = "password_complexity_system"
            case inviteLinksSystem = "invite_links_system"
            case userLinksLimit = "user_links_limit"
            case expireUserLinks = "expire_user_links"
            case shoutBoxSystem = "shout_box_system"
            case bankWithdrawalSystem = "bank_withdrawal_system"
            case liveVideo = "live_video"
            case liveToken = "live_token"
            case liveAccountID = "live_account_id"
            case razorpayPayment = "razorpay_payment"
            case razorpayKeyID = "razorpay_key_id"
            case razorpayKeySecret = "razorpay_key_secret"
            case payseraPayment = "paysera_payment"
            case payseraProjectID = "paysera_project_id"
            case payseraSignPassword = "paysera_sign_password"
            case payseraMode = "paysera_mode"
            case cloudUpload = "cloud_upload"
            case cloudFilePath = "cloud_file_path"
            case cloudBucketName = "cloud_bucket_name"
            case liveVideoSave = "live_video_save"
            case notifyNewPost = "notify_new_post"
            case agoraAppID = "agora_app_id"
            case agoraLiveVideo = "agora_live_video"
            case millicastLiveVideo = "millicast_live_video"
            case agoraCustomerID = "agora_customer_id"
            case agoraCustomerCertificate = "agora_customer_certificate"
            case yahooConsumerKey = "yahoo_consumer_key"
            case yahooConsumerSecret = "yahoo_consumer_secret"
            case cashfreeMode = "cashfree_mode"
            case amazoneS32 = "amazone_s3_2"
            case bucketName2 = "bucket_name_2"
            case amazoneS3Key2 = "amazone_s3_key_2"
            case amazoneS3SKey2 = "amazone_s3_s_key_2"
            case region2 = "region_2"
            case logoURL = "logo_url"
            case pageCategories = "page_categories"
            case groupCategories = "group_categories"
            case blogCategories = "blog_categories"
            case productsCategories = "products_categories"
            case jobCategories = "job_categories"
            case genders, family, fields
            case movieCategory = "movie_category"
            case postColors = "post_colors"
            case pageSubCategories = "page_sub_categories"
            case groupSubCategories = "group_sub_categories"
            case productsSubCategories = "products_sub_categories"
            case pageCustomFields = "page_custom_fields"
            case groupCustomFields = "group_custom_fields"
            case productCustomFields = "product_custom_fields"
            case postReactionsTypes = "post_reactions_types"
            case proPackages = "pro_packages"
            case proPackagesTypes = "pro_packages_types"
        }
    }
    
//    // MARK: - Config
//    struct Config: Codable {
//        let siteName, siteTitle, siteKeywords, siteDesc: String?
//        let defualtLang, fileSharing, chatSystem, userLastseen: String?
//        let age, deleteAccount, connectivitySystem, maxUpload: String?
//        let maxCharacters, messageSeen, messageTyping, allowedExtenstion: String?
//        let theme, headerBackground, headerHoverBorder, headerColor: String?
//        let bodyBackground, btnColor, btnBackgroundColor, btnHoverColor: String?
//        let btnHoverBackgroundColor, settingHeaderColor, settingHeaderBackground, settingActiveSidebarColor: String?
//        let settingActiveSidebarBackground, settingSidebarBackground, settingSidebarColor, logoExtension: String?
//        let backgroundExtension, videoUpload, audioUpload, headerSearchColor: String?
//        let headerButtonShadow, btnDisabled, userRegistration, faviconExtension: String?
//        let chatOutgoingBackground, windowsAppVersion, widnowsAppAPIID, widnowsAppAPIKey: String?
//        let creditCard, bitcoin, mWithdrawal, affiliateType: String?
//        let affiliateSystem, classified, bucketName, region: String?
//        let footerBackground, isUtf8, alipay, audioChat: String?
//        let smsProvider, footerTextColor, updatedLatest, footerBackground2: String?
//        let footerBackgroundN, blogs, canBlogs, push: String?
//        let androidMPushID, androidMPushKey, events, forum: String?
//        let lastUpdate, movies, yandexTranslationAPI, updateDB15: String?
//        let adVPrice, adCPrice: String?
//        let emoCDN: String?
//        let userAds, userStatus, dateStyle, stickers: String?
//        let giphyAPI, findFriends, updateDB152, scriptVersion: String?
//        let androidPushNative, androidPushMessages, updateDB153, adsCurrency: String?
//        let webPush: String?
//        let playtubeURL: String?
//        let connectivitySystemLimit, videoAdSkip, updateUserProfile, cacheSidebar: String?
//        let androidNPushID, androidNPushKey, ftpHost, ftpPort: String?
//        let ftpUsername, ftpPassword, ftpUpload, ftpEndpoint: String?
//        let ftpPath, transactionLog, coinpaymentsSecret, coinpaymentsID: String?
//        let infobipUsername, infobipPassword, updatev2, giftSystem: String?
//        let socialShareTwitter, socialShareGoogle, socialShareFacebook, socialShareWhatsup: String?
//        let socialSharePinterest, socialShareLinkedin, socialShareTelegram, stickersSystem: String?
//        let dollarToPointCost, commentsPoint, likesPoint, dislikesPoint: String?
//        let wondersPoint, reactionPoint, createpostPoint, pointAllowWithdrawal: String?
//        let stickyVideoPlayer, pointLevelSystem, commentReports, popularPosts: String?
//        let autoFriendUsers, spacesKey, spacesSecret, spaceName: String?
//        let spaceRegion, spaces, watermark, googleMap: String?
//        let loginAuth, twoFactor, twoFactorType, lastNotificationDeleteRun: String?
//        let iosPushMessages, iosMPushID, iosMPushKey, iosPushNative: String?
//        let iosNPushID, iosNPushKey, webPushID, webPushKey: String?
//        let profileBack, freeDayLimit, adultImages, adultImagesAction: String?
//        let adultImagesFile, proDayLimit, visionAPIKey, recaptchaSecretKey: String?
//        let bankPayment, bankTransferNote, bankDescription, createblogPoint: String?
//        let deepsoundURL: String?
//        let english, arabic, dutch, french: String?
//        let german, italian, portuguese, russian: String?
//        let spanish, turkish: String?
//        let currencyArray: [String]?
//        let paypalCurrency, stripeCurrency, the2CheckoutCurrency, version: String?
//        let forumVisibility, eventsVisibility, productVisibility, paypal: String?
//        let pokeSystem, afternoonSystem, providersArray, coloredPostsSystem: String?
//        let checkoutPayment, checkoutMode, checkoutSellerID, checkoutPublishableKey: String?
//        let checkoutPrivateKey, jobSystem, weatherWidget, commonThings: String?
//        let fundingSystem, fundingRequest, donatePercentage, weatherKey: String?
//        let logoURL: String?
//        let pageCategories, groupCategories, blogCategories, productsCategories: [String: String]?
//        let jobCategories: [String: String]?
//        let family: [String: String]?
//        let postColors: [String: PostColor]?
//        let postReactionsTypes: [String]? = nil
//
//        enum CodingKeys: String, CodingKey {
//            case siteName, siteTitle, siteKeywords, siteDesc, defualtLang, fileSharing, chatSystem
//            case userLastseen = "user_lastseen"
//            case age, deleteAccount, connectivitySystem, maxUpload, maxCharacters
//            case messageSeen = "message_seen"
//            case messageTyping = "message_typing"
//            case allowedExtenstion, theme
//            case headerBackground = "header_background"
//            case headerHoverBorder = "header_hover_border"
//            case headerColor = "header_color"
//            case bodyBackground = "body_background"
//            case btnColor = "btn_color"
//            case btnBackgroundColor = "btn_background_color"
//            case btnHoverColor = "btn_hover_color"
//            case btnHoverBackgroundColor = "btn_hover_background_color"
//            case settingHeaderColor = "setting_header_color"
//            case settingHeaderBackground = "setting_header_background"
//            case settingActiveSidebarColor = "setting_active_sidebar_color"
//            case settingActiveSidebarBackground = "setting_active_sidebar_background"
//            case settingSidebarBackground = "setting_sidebar_background"
//            case settingSidebarColor = "setting_sidebar_color"
//            case logoExtension = "logo_extension"
//            case backgroundExtension = "background_extension"
//            case videoUpload = "video_upload"
//            case audioUpload = "audio_upload"
//            case headerSearchColor = "header_search_color"
//            case headerButtonShadow = "header_button_shadow"
//            case btnDisabled = "btn_disabled"
//            case userRegistration = "user_registration"
//            case faviconExtension = "favicon_extension"
//            case chatOutgoingBackground = "chat_outgoing_background"
//            case windowsAppVersion = "windows_app_version"
//            case widnowsAppAPIID = "widnows_app_api_id"
//            case widnowsAppAPIKey = "widnows_app_api_key"
//            case creditCard = "credit_card"
//            case bitcoin
//            case mWithdrawal = "m_withdrawal"
//            case affiliateType = "affiliate_type"
//            case affiliateSystem = "affiliate_system"
//            case classified
//            case bucketName = "bucket_name"
//            case region
//            case footerBackground = "footer_background"
//            case isUtf8 = "is_utf8"
//            case alipay
//            case audioChat = "audio_chat"
//            case smsProvider = "sms_provider"
//            case footerTextColor = "footer_text_color"
//            case updatedLatest = "updated_latest"
//            case footerBackground2 = "footer_background_2"
//            case footerBackgroundN = "footer_background_n"
//            case blogs
//            case canBlogs = "can_blogs"
//            case push
//            case androidMPushID = "android_m_push_id"
//            case androidMPushKey = "android_m_push_key"
//            case events, forum
//            case lastUpdate = "last_update"
//            case movies
//            case yandexTranslationAPI = "yandex_translation_api"
//            case updateDB15 = "update_db_15"
//            case adVPrice = "ad_v_price"
//            case adCPrice = "ad_c_price"
//            case emoCDN = "emo_cdn"
//            case userAds = "user_ads"
//            case userStatus = "user_status"
//            case dateStyle = "date_style"
//            case stickers
//            case giphyAPI = "giphy_api"
//            case findFriends = "find_friends"
//            case updateDB152 = "update_db_152"
//            case scriptVersion = "script_version"
//            case androidPushNative = "android_push_native"
//            case androidPushMessages = "android_push_messages"
//            case updateDB153 = "update_db_153"
//            case adsCurrency = "ads_currency"
//            case webPush = "web_push"
//            case playtubeURL = "playtube_url"
//            case connectivitySystemLimit
//            case videoAdSkip = "video_ad_skip"
//            case updateUserProfile = "update_user_profile"
//            case cacheSidebar = "cache_sidebar"
//            case androidNPushID = "android_n_push_id"
//            case androidNPushKey = "android_n_push_key"
//            case ftpHost = "ftp_host"
//            case ftpPort = "ftp_port"
//            case ftpUsername = "ftp_username"
//            case ftpPassword = "ftp_password"
//            case ftpUpload = "ftp_upload"
//            case ftpEndpoint = "ftp_endpoint"
//            case ftpPath = "ftp_path"
//            case transactionLog = "transaction_log"
//            case coinpaymentsSecret = "coinpayments_secret"
//            case coinpaymentsID = "coinpayments_id"
//            case infobipUsername = "infobip_username"
//            case infobipPassword = "infobip_password"
//            case updatev2
//            case giftSystem = "gift_system"
//            case socialShareTwitter = "social_share_twitter"
//            case socialShareGoogle = "social_share_google"
//            case socialShareFacebook = "social_share_facebook"
//            case socialShareWhatsup = "social_share_whatsup"
//            case socialSharePinterest = "social_share_pinterest"
//            case socialShareLinkedin = "social_share_linkedin"
//            case socialShareTelegram = "social_share_telegram"
//            case stickersSystem = "stickers_system"
//            case dollarToPointCost = "dollar_to_point_cost"
//            case commentsPoint = "comments_point"
//            case likesPoint = "likes_point"
//            case dislikesPoint = "dislikes_point"
//            case wondersPoint = "wonders_point"
//            case reactionPoint = "reaction_point"
//            case createpostPoint = "createpost_point"
//            case pointAllowWithdrawal = "point_allow_withdrawal"
//            case stickyVideoPlayer = "sticky_video_player"
//            case pointLevelSystem = "point_level_system"
//            case commentReports = "comment_reports"
//            case popularPosts = "popular_posts"
//            case autoFriendUsers = "auto_friend_users"
//            case spacesKey = "spaces_key"
//            case spacesSecret = "spaces_secret"
//            case spaceName = "space_name"
//            case spaceRegion = "space_region"
//            case spaces, watermark
//            case googleMap = "google_map"
//            case loginAuth = "login_auth"
//            case twoFactor = "two_factor"
//            case twoFactorType = "two_factor_type"
//            case lastNotificationDeleteRun = "last_notification_delete_run"
//            case iosPushMessages = "ios_push_messages"
//            case iosMPushID = "ios_m_push_id"
//            case iosMPushKey = "ios_m_push_key"
//            case iosPushNative = "ios_push_native"
//            case iosNPushID = "ios_n_push_id"
//            case iosNPushKey = "ios_n_push_key"
//            case webPushID = "web_push_id"
//            case webPushKey = "web_push_key"
//            case profileBack = "profile_back"
//            case freeDayLimit = "free_day_limit"
//            case adultImages = "adult_images"
//            case adultImagesAction = "adult_images_action"
//            case adultImagesFile = "adult_images_file"
//            case proDayLimit = "pro_day_limit"
//            case visionAPIKey = "vision_api_key"
//            case recaptchaSecretKey = "recaptcha_secret_key"
//            case bankPayment = "bank_payment"
//            case bankTransferNote = "bank_transfer_note"
//            case bankDescription = "bank_description"
//            case createblogPoint = "createblog_point"
//            case deepsoundURL = "deepsound_url"
//            case english, arabic, dutch, french, german, italian, portuguese, russian, spanish, turkish
//            case currencyArray = "currency_array"
//            case paypalCurrency = "paypal_currency"
//            case stripeCurrency = "stripe_currency"
//            case the2CheckoutCurrency = "2checkout_currency"
//            case version
//            case forumVisibility = "forum_visibility"
//            case eventsVisibility = "events_visibility"
//            case productVisibility = "product_visibility"
//            case paypal
//            case pokeSystem = "poke_system"
//            case afternoonSystem = "afternoon_system"
//            case providersArray = "providers_array"
//            case coloredPostsSystem = "colored_posts_system"
//            case checkoutPayment = "checkout_payment"
//            case checkoutMode = "checkout_mode"
//            case checkoutSellerID = "checkout_seller_id"
//            case checkoutPublishableKey = "checkout_publishable_key"
//            case checkoutPrivateKey = "checkout_private_key"
//            case jobSystem = "job_system"
//            case weatherWidget = "weather_widget"
//            case commonThings = "common_things"
//            case fundingSystem = "funding_system"
//            case fundingRequest = "funding_request"
//            case donatePercentage = "donate_percentage"
//            case weatherKey = "weather_key"
//            case logoURL = "logo_url"
//            case pageCategories = "page_categories"
//            case groupCategories = "group_categories"
//            case blogCategories = "blog_categories"
//            case productsCategories = "products_categories"
//            case jobCategories = "job_categories"
//            case  family
//            case postColors = "post_colors"
//            case postReactionsTypes = "post_reactions_types"
//        }
//    }
    
    // MARK: - CurrencySymbolArray
    struct CurrencySymbolArray: Codable {
        let usd, eur, currencySymbolArrayTRY, gbp: String
        let rub, pln, ils, brl: String
        let inr: String
        
        enum CodingKeys: String, CodingKey {
            case usd = "USD"
            case eur = "EUR"
            case currencySymbolArrayTRY = "TRY"
            case gbp = "GBP"
            case rub = "RUB"
            case pln = "PLN"
            case ils = "ILS"
            case brl = "BRL"
            case inr = "INR"
        }
    }
    
    // MARK: - Genders
    struct Genders: Codable {
        let female, male: String
    }
    
    // MARK: - PostColor
//    struct PostColor: Codable {
//        let id: Int
//        let color1, color2, textColor: String
//        let image: String
//        let time: String
//
//        enum CodingKeys: String, CodingKey {
//            case id
//            case color1 = "color_1"
//            case color2 = "color_2"
//            case textColor = "text_color"
//            case image, time
//        }
//    }
    
    // MARK: - PostColor
    struct PostColor: Codable {
        let id: Int
        let color1, color2, textColor: String
        let image: String
        let time: String

        enum CodingKeys: String, CodingKey {
            case id
            case color1 = "color_1"
            case color2 = "color_2"
            case textColor = "text_color"
            case image, time
        }
    }

    // MARK: - Field
    struct Field: Codable {
        let id, name, fieldDescription, type: String
        let length, placement, registrationPage, profilePage: String
        let selectType, active, fid: String

        enum CodingKeys: String, CodingKey {
            case id, name
            case fieldDescription = "description"
            case type, length, placement
            case registrationPage = "registration_page"
            case profilePage = "profile_page"
            case selectType = "select_type"
            case active, fid
        }
    }
    
    // MARK: - MovieCategory
    struct MovieCategory: Codable {
        let action, comedy, drama, horror: String
        let mythological, war, adventure, family: String
        let sport, animation, crime, fantasy: String
        let musical, romance, thriller, history: String
        let documentary, tvshow: String
    }
    

    // MARK: - SubCategories
    struct SubCategories: Codable {
        let the2: [The2]

        enum CodingKeys: String, CodingKey {
            case the2 = "2"
        }
    }
    
    // MARK: - The2
    struct The2: Codable {
        let id, categoryID, langKey, type: String
        let lang: String

        enum CodingKeys: String, CodingKey {
            case id
            case categoryID = "category_id"
            case langKey = "lang_key"
            case type, lang
        }
    }
    

    // MARK: - PostReactionsType
    struct PostReactionsType: Codable {
        let id, name, wowonderIcon, sunshineIcon: String
        let status, wowonderSmallIcon: String
        let isHTML: Int

        enum CodingKeys: String, CodingKey {
            case id, name
            case wowonderIcon = "wowonder_icon"
            case sunshineIcon = "sunshine_icon"
            case status
            case wowonderSmallIcon = "wowonder_small_icon"
            case isHTML = "is_html"
        }
    }
    
    struct ProPackages: Codable {
        let star, hot, ultima, vip: Hot
    }

    // MARK: - Hot
    struct Hot: Codable {
        let id, type, price, featuredMember: String
        let profileVisitors, lastSeen, verifiedBadge, postsPromotion: String
        let pagesPromotion, discount, image, nightImage: String
        let status, time: String

        enum CodingKeys: String, CodingKey {
            case id, type, price
            case featuredMember = "featured_member"
            case profileVisitors = "profile_visitors"
            case lastSeen = "last_seen"
            case verifiedBadge = "verified_badge"
            case postsPromotion = "posts_promotion"
            case pagesPromotion = "pages_promotion"
            case discount, image
            case nightImage = "night_image"
            case status, time
        }
    }
    
    

   ///////////////////////////////////////////////////
    struct Get_Site_SettingErrorModel: Codable {
        let apiStatus: String
        let errors: Errors
        
        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case errors
        }
    }
    
    // MARK: - Errors
    struct Errors: Codable {
        let errorID, errorText: String
        
        enum CodingKeys: String, CodingKey {
            case errorID = "error_id"
            case errorText = "error_text"
        }
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



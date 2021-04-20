
import Foundation
class GetOffersModel{
    struct GetOffersSuccessModel: Codable {
        var apiStatus: Int?
        var data: [Datum]?

        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case data
        }
    }
    struct GetOffersErrorModel:Codable{
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
        var id, pageID, userID: Int?
        var discountType: String?
        var discountPercent, discountAmount: Int?
        var discountedItems: String?
        var buy, getPrice, spend, amountOff: Int?
        var datumDescription: String?
        var expireDate: String?
        var expireTime: Int?
        var image: String?
        var currency: String?
        var time: Int?
        var offerText: String?
        var page: Page?
        var url: String?

        enum CodingKeys: String, CodingKey {
            case id
            case pageID = "page_id"
            case userID = "user_id"
            case discountType = "discount_type"
            case discountPercent = "discount_percent"
            case discountAmount = "discount_amount"
            case discountedItems = "discounted_items"
            case buy
            case getPrice = "get_price"
            case spend
            case amountOff = "amount_off"
            case datumDescription = "description"
            case expireDate = "expire_date"
            case expireTime = "expire_time"
            case image, currency, time
            case offerText = "offer_text"
            case page, url
        }
    }

    enum Currency: String, Codable {
        case empty = "$"
    }

    enum DiscountType: String, Codable {
        case buyGetDiscount = "buy_get_discount"
        case discountPercent = "discount_percent"
        case freeShipping = "free_shipping"
        case spendGetOff = "spend_get_off"
    }

    enum ExpireDate: String, Codable {
        case the230420 = "23-04-20"
    }

    // MARK: - Page
    struct Page: Codable {
        var pageID, userID, pageName, pageTitle: String?
        var pageDescription: String?
        var avatar: String?
        var cover: String?
        var usersPost, pageCategory, subCategory: String?
        var website: String?
        var facebook: String?
        var google: String?
        var vk, twitter, linkedin, company: String?
        var phone, address, callActionType: String?
        var callActionTypeURL: String?
        var backgroundImage, backgroundImageStatus, instgram, youtube: String?
        var verified, active, registered, boosted: String?
        var about, id: String?
        var type: String?
        var url: String?
        var name: String?
        var rating: Int?
        var category, pageSubCategory: String?
        var isPageOnwer: Bool?
        var username: String?

        enum CodingKeys: String, CodingKey {
            case pageID = "page_id"
            case userID = "user_id"
            case pageName = "page_name"
            case pageTitle = "page_title"
            case pageDescription = "page_description"
            case avatar, cover
            case usersPost = "users_post"
            case pageCategory = "page_category"
            case subCategory = "sub_category"
            case website, facebook, google, vk, twitter, linkedin, company, phone, address
            case callActionType = "call_action_type"
            case callActionTypeURL = "call_action_type_url"
            case backgroundImage = "background_image"
            case backgroundImageStatus = "background_image_status"
            case instgram, youtube, verified, active, registered, boosted, about, id, type, url, name, rating, category
            case pageSubCategory = "page_sub_category"
            case isPageOnwer = "is_page_onwer"
            case username
        }
    }

    enum Google: String, Codable {
        case booksart = "booksart"
        case empty = ""
    }

    enum TypeEnum: String, Codable {
        case page = "page"
    }

}

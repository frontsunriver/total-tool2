

import Foundation
class TwoFactorModel{
    // MARK: - Welcome
    struct TwoFactorSuccessModel: Codable {
        var apiStatus: Int?
        var timezone, accessToken, userID: String?

        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case timezone
            case accessToken = "access_token"
            case userID = "user_id"
        }
    }
    struct TwoFactorErrorModel:Codable{
          let apiStatus: String
          let errors: Errors
          enum CodingKeys: String, CodingKey {
              case apiStatus = "api_status"
              case errors
          }
      }
      
      // MARK: - Errors
      struct Errors: Codable {
          let errorID: Int
          let errorText: String
          
          enum CodingKeys: String, CodingKey {
              case errorID = "error_id"
              case errorText = "error_text"
          }
      }

}
class UpdateTwoFactorModel{
    struct UpdateTwoFactorSuccessModel: Codable {
        var apiStatus: Int?
        var message: String?

        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case message
        }
    }
    struct UpdateTwoFactorErrorModel:Codable{
             let apiStatus: String
             let errors: Errors
             enum CodingKeys: String, CodingKey {
                 case apiStatus = "api_status"
                 case errors
             }
         }
         
         // MARK: - Errors
         struct Errors: Codable {
             let errorID: Int
             let errorText: String
             
             enum CodingKeys: String, CodingKey {
                 case errorID = "error_id"
                 case errorText = "error_text"
             }
         }
}

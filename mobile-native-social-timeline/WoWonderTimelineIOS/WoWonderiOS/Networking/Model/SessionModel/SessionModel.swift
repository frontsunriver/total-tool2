
import Foundation
class SessionModel{
    struct SessionSuccessModel: Codable {
        var apiStatus: Int?
        var data: [Datum]?

        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case data
        }
    }
    struct SessionErrorModel: Codable {
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

    // MARK: - Datum
    struct Datum: Codable {
        var id, userID, sessionID: String?
        var platform: String?
        var platformDetails, time: String?
        var browser: String?
        var ipAddress: String?

        enum CodingKeys: String, CodingKey {
            case id
            case userID = "user_id"
            case sessionID = "session_id"
            case platform
            case platformDetails = "platform_details"
            case time, browser
            case ipAddress = "ip_address"
        }
    }

    enum Browser: String, Codable {
        case mobile = "Mobile"
    }

    enum Platform: String, Codable {
        case empty = ""
        case phone = "Phone"
    }

}
class DeleteSessionModel{
    struct DeleteSessionSuccessModel: Codable {
        var apiStatus: Int?
        var message: String?

        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case message
        }
    }
    struct DeleteSessionErrorModel: Codable {
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

}

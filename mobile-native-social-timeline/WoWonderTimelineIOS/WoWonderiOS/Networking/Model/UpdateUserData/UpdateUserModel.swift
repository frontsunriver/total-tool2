

import Foundation
class UpdateUserModel{
    struct UpdateUserDataSuccessModel: Codable {
        var apiStatus: Int?
        var message: String?
        
        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case message
        }
    }
    struct UpdateUserDataErrorModel: Codable {
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



import Foundation

class Follow_RequestModel {
    
struct followRequest_SuccessModel : Codable{
        var api_status : Int
        var follow_status : String
    
    }
    
    struct follow_RequestErrorModel : Codable {
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


import Foundation

class Block_UserModel {
    
    struct BlockUser_SuccessModel : Codable {
        var api_status : Int
        var block_status : String
    }
    
    struct BlockUser_ErrorModel : Codable {
        let apiStatus: String
        let errors: Errors
        
        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case errors
        }
    }
    
   
    struct Errors: Codable {
        let errorID, errorText: String
        
        enum CodingKeys: String, CodingKey {
            case errorID = "error_id"
            case errorText = "error_text"
        }
    }
    
}



import Foundation

class DeleteGroupModel {
    
  struct DeleteGroup_SuccessModel : Codable{
        var api_status : Int
        var message : String
    }
    
    struct  DeleteGroup_ErrorModel : Codable {
     
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



import Foundation

class GetNotPageMembersModel{
    
    struct GetNotPageMember_SuccessModel {
        var api_status : Int
        var users : [[String:Any]]
    }
    
    struct GetNotPageMember_ErrorModel : Codable{
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

extension GetNotPageMembersModel.GetNotPageMember_SuccessModel{
    
    init (json : [String:Any]){
        
        let api_status = json["api_status"] as? Int
        let users = json["users"] as? [[String:Any]]
        self.api_status = api_status ?? 0
        self.users = users ?? [["PostType" : "Profile_Pic"]]
        
    }
    
}

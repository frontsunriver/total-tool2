

import Foundation

class CreateGroupModel{
    
    struct createGroup_SuccessModel{
        var api_status : Int
        var group_data : [String:Any]
    }
    
    struct createGroup_ErrorModel : Codable{
        let apiStatus: String
        let errors: Errors
        
        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case errors
        }
    }
    
    struct Errors: Codable {
        let errorID: Int
        let errorText: String
        
        enum CodingKeys: String, CodingKey {
            case errorID = "error_id"
            case errorText = "error_text"
        }
    }
    
    
}

extension CreateGroupModel.createGroup_SuccessModel{
    
    init (json:[String:Any]){
        let api_status = json["api_status"] as? Int
        let groupdata =  json["group_data"] as? [String:Any]
        self.api_status = api_status ?? 0
        self.group_data = groupdata ?? ["id" : "12345"]
    }
    
}

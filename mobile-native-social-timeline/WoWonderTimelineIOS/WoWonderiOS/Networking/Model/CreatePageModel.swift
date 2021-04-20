

import Foundation


class CreatePageModel{
    
    struct createPage_SuccessModel{
        var api_status : Int
        var page_data : [String:Any]
    }
    
    struct createPage_ErrorModel : Codable{
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

extension CreatePageModel.createPage_SuccessModel{
    
    init (json:[String:Any]){
        let api_status = json["api_status"] as? Int
        let pageData =  json["group_data"] as? [String:Any]
        self.api_status = api_status ?? 0
        self.page_data = pageData ?? ["id" : "12345"]
    }
    
}






import Foundation

class GetPageInfoModel{
    
    struct GetPageInfo_SuccessModel {
        var api_status : Int
        var page_data : [String:Any]
    }
    
    
    struct GetPageInfo_ErrorModel : Codable{
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

extension GetPageInfoModel.GetPageInfo_SuccessModel{
    
    init(json:[String:Any]){
        let api_status = json["api_status"] as? Int
        let page_data = json["page_data"] as? [String:Any]
        self.api_status = api_status ?? 0
        self.page_data = page_data ?? ["PostType" : "Profile_Pic"]
        
    }
}

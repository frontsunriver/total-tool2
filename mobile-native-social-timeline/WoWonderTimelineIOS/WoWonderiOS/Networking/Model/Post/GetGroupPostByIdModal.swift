
import Foundation

class GetGroupDataModal{
    
    struct GetGroupData_SuccessModal{
        let api_status: Int
        let group_data: [String:Any]
    }
    
      struct GetGroupData_ErrorModal:Codable{
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

extension GetGroupDataModal.GetGroupData_SuccessModal{
    init(json : [String:Any]) {
        let api_status = json["api_status"] as? Int
        let data = json["group_data"] as? [String:Any]
        self.api_status = api_status ?? 0
        self.group_data = data ?? ["PostType" : "Profile_Pic"]
    }
}



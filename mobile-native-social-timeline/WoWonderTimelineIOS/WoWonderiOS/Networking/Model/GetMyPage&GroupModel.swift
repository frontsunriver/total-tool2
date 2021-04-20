

import Foundation

class GetMyGroupsandPages_Modal{
    
  struct GetMyGroups_SuccessModal{
    var api_status: Int
    var groups: [[String:Any]]
    init(json: [String:Any]) {
        let apiStatus = json["api_status"] as? Int
        let groups = json["groups"] as? [[String:Any]]
        self.api_status = apiStatus ?? 0
        self.groups = groups ?? [["id" : "12345"]]
       }
    }
    
    struct GetMyPages_SuccessModal{
        var api_status: Int
        var pages: [[String:Any]]
        init(json: [String:Any]) {
            let apiStatus = json["api_status"] as? Int
            let pages = json["pages"] as? [[String:Any]]
            self.api_status = apiStatus ?? 0
            self.pages = pages ?? [["id" : "12345"]]
        }
    }
    
    struct GetMyGroupandPage_ErrorModal :Codable{
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

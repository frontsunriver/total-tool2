
import Foundation
 
class  GetSeacrhDataModal {
    
    struct getSearchData_SuccessModal{
        var api_status: Int
        var users: [[String:Any]]
        var pages: [[String:Any]]
        var groups: [[String:Any]]
    }
    
    struct getSearchData_ErrorModal:Codable{
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
extension GetSeacrhDataModal.getSearchData_SuccessModal{
    init(json: [String:Any]) {
        let apiStatus = json["api_status"] as? Int
        let users = json["users"] as? [[String:Any]]
        let pages = json["pages"] as? [[String:Any]]
        let groups = json["groups"] as? [[String:Any]]
        self.api_status = apiStatus ?? 0
        self.users = users ?? [["user_name":"abc"]]
        self.pages = pages ?? [["user_name":"abc"]]
        self.groups = groups ?? [["user_name":"abc"]]
    }
}

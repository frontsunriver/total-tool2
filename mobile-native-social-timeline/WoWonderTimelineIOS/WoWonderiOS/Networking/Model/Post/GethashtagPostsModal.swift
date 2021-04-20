

import Foundation

class GethashtagPostsModal{
    
    struct getHashTagPost_SuccessModal{
        let api_status: Int
        let data: [[String:Any]]
    }
    
      struct getHashTagPost_ErrorModal:Codable{
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
extension GethashtagPostsModal.getHashTagPost_SuccessModal{
init(json : [String:Any]) {
    let api_status = json["api_status"] as? Int
    let data = json["data"] as? [[String:Any]]
    self.api_status = api_status ?? 0
    self.data = data ?? [["PostType" : "Profile_Pic"]]
    }
}

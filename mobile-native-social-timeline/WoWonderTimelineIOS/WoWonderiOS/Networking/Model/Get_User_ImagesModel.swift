
import Foundation

class Get_Users_ImagesModel {
    
    struct get_UserImages_SuccessModel {
        var api_status : Int
        var data : [[String:Any]]
        
    }
    
    struct get_UserImages_ErrorModel : Codable {
        let apiStatus: String
        let errors: Errors
        
        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case errors
        }
    }
    
  
    // MARK: - Errors
    struct Errors: Codable {
        let errorID, errorText: String
        
        enum CodingKeys: String, CodingKey {
            case errorID = "error_id"
            case errorText = "error_text"
        }
    }
    
}

extension Get_Users_ImagesModel.get_UserImages_SuccessModel{
    init(json : [String:Any]) {
        let api_status = json["api_status"] as? Int
        let albums = json["data"] as? [[String:Any]]
        self.api_status = api_status ?? 0
        self.data = albums ?? [["PostType" : "Profile_Pic"]]
    }
    
}

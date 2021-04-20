
import Foundation

class SharePostOnGroupandPageModal {
    
    struct SharePostOnGroupandPageModal_SuccessModal {
        var api_status :Int
        var data :[String:Any]
    }
    
    struct SharePostOnGroupandPageModal_ErrorModal :Codable{
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
extension SharePostOnGroupandPageModal.SharePostOnGroupandPageModal_SuccessModal{
    init(json :[String:Any]) {
        let apiStatus = json["api_status"] as? Int
        let data = json["data"] as? [String:Any]
        self.api_status = apiStatus ?? 0
        self.data = data ?? ["id" : "1234"]
    }
}



import Foundation

class GetBlogModel{
    
    struct getBlog_SuccessModel {
    var api_status : Int
    var articles : [[String:Any]]
    }
    
    struct getBlog_ErrorModel : Codable {
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

extension GetBlogModel.getBlog_SuccessModel{
    init(json :[String:Any]) {
        let apiStatus = json["api_status"] as? Int
        let articles = json["articles"] as? [[String:Any]]
        self.articles = articles ?? [["id" : "12345"]]
        self.api_status = apiStatus ?? 0
    }
}

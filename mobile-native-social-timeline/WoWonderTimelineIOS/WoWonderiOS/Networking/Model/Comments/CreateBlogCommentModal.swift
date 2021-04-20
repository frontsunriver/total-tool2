
import Foundation

class CreateBlogCommentModal{
    
    struct createBlogComment_SuccessModal{
           var api_status: Int
           var data: [[String:Any]]
       }
    
      struct createBlogComment_ErrorModal:Codable{
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

extension CreateBlogCommentModal.createBlogComment_SuccessModal{
init(json : [String:Any]) {
    let api_status = json["api_status"] as? Int
    let data = json["data"] as? [[String:Any]]
    self.api_status = api_status ?? 0
    self.data = data ?? [["PostType" : "Profile_Pic"]]
    }
}

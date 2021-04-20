

import Foundation

class LikeBlogCommentReplyModal {
    
    struct likeBlogCommentReply_SuccessModal:Codable{
        var  api_status: Int
        var  code: Int
        var  type: String
    }
    
    struct likeBlogCommentReply_ErrorModal:Codable{
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

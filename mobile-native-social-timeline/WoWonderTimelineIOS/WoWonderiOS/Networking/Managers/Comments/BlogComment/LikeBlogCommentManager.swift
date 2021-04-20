

import Foundation
import Alamofire
import WoWonderTimelineSDK


class LikeBlogCommentManager{
    
    func likeComment(type: String, comment_id: String, blogId: String,reactionType: String,completionBlock :@escaping (_ Success: LikeBlogCommentModal.likeBlogComment_SuccessModal?, _ AuthError: LikeBlogCommentModal.getBlogComments_ErrorModal?, Error?)->()){
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.commentId:comment_id,APIClient.Params.blogId:blogId,APIClient.Params.type:type,APIClient.Params.reactionType:reactionType]
      let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.BlogComments.blogCommentApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(LikeBlogCommentModal.likeBlogComment_SuccessModal.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(LikeBlogCommentModal.getBlogComments_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else{
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = LikeBlogCommentManager()
    private init() {}
}

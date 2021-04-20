

import Foundation
import Alamofire
import WoWonderTimelineSDK

class CreateReplyBlogCommentManager{
      
    func createReply(text: String, commentId: String, blog_id: String,completionBlock :@escaping (_ Success: CreateBlogCommentReplayModal.createBlogCommentReplay_SuccessModal?, _ AuthError: CreateBlogCommentReplayModal.createBlogCommentReplay_ErrorModal?, Error?)->()){
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.text:text, APIClient.Params.commentId:commentId,APIClient.Params.blogId:blog_id, APIClient.Params.type: "add_reply"]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.BlogComments.blogCommentApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
                let result = CreateBlogCommentReplayModal.createBlogCommentReplay_SuccessModal.init(json: res)
                 completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(CreateBlogCommentReplayModal.createBlogCommentReplay_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else{
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = CreateReplyBlogCommentManager()
    private init() {}
}

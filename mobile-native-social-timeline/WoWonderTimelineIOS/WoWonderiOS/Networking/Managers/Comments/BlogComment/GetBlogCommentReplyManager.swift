

import Foundation
import Alamofire
import WoWonderTimelineSDK


class GetBlogCommentReplyManager{
    
    func getCommentReply(commentId: String, offset: String,completionBlock :@escaping (_ Success: GetBlogCommentReplyModal.getBlogCommentReply_SuccessModal?, _ AuthError: GetBlogCommentReplyModal.getBlogCommentReply_ErrorModal?, Error?)->()){
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.commentId:commentId,APIClient.Params.offset:offset,APIClient.Params.type:"reply_fetch",APIClient.Params.limit:10] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.BlogComments.blogCommentApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
                    let result = GetBlogCommentReplyModal.getBlogCommentReply_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetBlogCommentReplyModal.getBlogCommentReply_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else{
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    static let sharedInstance = GetBlogCommentReplyManager()
    private init() {}
}


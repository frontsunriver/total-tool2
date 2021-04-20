
import Foundation
import Alamofire
import WoWonderTimelineSDK


class FetchCommentReplyManager{
    
    func fetchCommentReply(comment_Id: String,offset: String,completionBlock : @escaping (_ Success:FetchCommentModel.fetchComment_SuccessModel?, _ AuthError : FetchCommentModel.fetchComment_ErrorModel? , Error?)->()){
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.limit : 15, APIClient.Params.type : "fetch_comments_reply",APIClient.Params.commentId: comment_Id, APIClient.Params.offset : offset] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.FetchComment.fetchComment + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = FetchCommentModel.fetchComment_SuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(FetchCommentModel.fetchComment_ErrorModel.self, from: data)else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
        
    }
    
    static let sharedInstance = FetchCommentReplyManager()
    private init () {}
}

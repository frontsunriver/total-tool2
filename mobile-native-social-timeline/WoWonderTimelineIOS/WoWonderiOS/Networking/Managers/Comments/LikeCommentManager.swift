
import Foundation
import Alamofire
import WoWonderTimelineSDK


class LikeCommentManager{
    
    func likeComment (commentId: String, type: String, completionBlock : @escaping (_ Success:
        LikeCommentModal.likeComment_SuccessModal?, _ AuthError : LikeCommentModal.likeComment_ErrorModal? , Error?)->()){
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.commentId:commentId, APIClient.Params.type:type]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
    
        Alamofire.request(APIClient.LikeComment.likeComment + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                   guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                   guard let result = try? JSONDecoder().decode(LikeCommentModal.likeComment_SuccessModal.self, from: data) else {return}
                   completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(LikeCommentModal.likeComment_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                  print(response.error?.localizedDescription)
                  completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedIntsance = LikeCommentManager()
    private init() {}
}


import Foundation
import Alamofire
import WoWonderTimelineSDK


class FetchCommentManager {
    func fetchComment (postId : String, offset : String,completionBlock : @escaping (_ Success:FetchCommentModel.fetchComment_SuccessModel?, _ AuthError : FetchCommentModel.fetchComment_ErrorModel? , Error?)->()){
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.limit : 20, APIClient.Params.type : "fetch_comments",APIClient.Params.postId :postId,APIClient.Params.offset : offset] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.FetchComment.fetchComment + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
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
    
    static let sharedInstance = FetchCommentManager()
    private init() {}
    
}

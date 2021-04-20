

import Foundation
import WoWonderTimelineSDK

import Alamofire

class GetHashTagPostManager{
    
    func getHashtagPost(hash: String,afterPostId: String, completionBlock :@escaping (_ Success: GethashtagPostsModal.getHashTagPost_SuccessModal?, _ AuthError: GethashtagPostsModal.getHashTagPost_ErrorModal?, Error?)->()){
        let params = [APIClient.Params.serverKey: APIClient.SERVER_KEY.Server_Key, APIClient.Params.limit: 10, APIClient.Params.afterPostId:afterPostId,APIClient.Params.type:"hashtag",APIClient.Params.hash: hash] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.GetHashtagPost.getHashtagPostApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
                    let result = GethashtagPostsModal.getHashTagPost_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GethashtagPostsModal.getHashTagPost_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = GetHashTagPostManager()
    private init() {}
}

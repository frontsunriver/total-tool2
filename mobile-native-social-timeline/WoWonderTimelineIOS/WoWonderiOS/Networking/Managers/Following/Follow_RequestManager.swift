

import Foundation
import Alamofire
import WoWonderTimelineSDK


class Follow_RequestManager{
    
    func sendFollowRequest(userId : String,completionBlock : @escaping (_ Success:Follow_RequestModel.followRequest_SuccessModel?, _ AuthError : Follow_RequestModel.follow_RequestErrorModel? , Error?)->()){
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.userId : userId]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
Alamofire.request(APIClient.Follow_Request.followRequestApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
    if response.result.value != nil {
    guard let res = response.result.value as? [String:Any] else {return}
    guard let apiStatusCode = res["api_status"]  as? Any else {return}
    let apiCode = apiStatusCode as? Int
    if apiCode == 200 {
    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
        guard let result = try? JSONDecoder().decode(Follow_RequestModel.followRequest_SuccessModel.self, from: data) else {return}
    completionBlock(result,nil,nil)
        }
        
    else {
    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
    guard let result = try? JSONDecoder().decode(Follow_RequestModel.follow_RequestErrorModel.self, from: data) else {return}
     completionBlock(nil,result,nil)
        }
            }
    else {
        print(response.error?.localizedDescription)
        completionBlock(nil,nil,response.error)
    }
    
        }
    
    }
    
  static let sharedInstance = Follow_RequestManager()
    private init() {}
    
    
}



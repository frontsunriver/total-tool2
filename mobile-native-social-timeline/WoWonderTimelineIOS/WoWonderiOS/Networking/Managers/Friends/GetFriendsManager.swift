

import Foundation
import Alamofire
import WoWonderTimelineSDK


class GetFriendsManager{
    
    func getFriends(type: String, user_id: String, following_offset: String, followers_offset: String,completionBlock :@escaping (_ Success: GetFriendsModal.getFriends_SuccessModal?, _ AuthError: GetFriendsModal.getFriends_ErrorModal?, Error?)->()){
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.limit:15, APIClient.Params.type:type, APIClient.Params.followersOffset:followers_offset, APIClient.Params.followingOffset:following_offset,APIClient.Params.userId:user_id] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.GetFriends.getFriendsApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = GetFriendsModal.getFriends_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetFriendsModal.getFriends_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = GetFriendsManager()
    private init() {}
}


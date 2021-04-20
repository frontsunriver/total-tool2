//
//  GetFriendRequestManager.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/4/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK
class GetFriendRequestManager{
  
    func getFriendRequest(completionBlock : @escaping (_ Success:GetFriendRequestModal.getFirend_RequestSuccessModal?, _ AuthError :GetFriendRequestModal.getFirend_Request_ErrorModel?, Error?)->()){
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.fetch:"friend_requests"]
//        let params = [API.Params.ServerKey:API.SERVER_KEY.Server_Key,API.Params.FetchType:API.Params.friendRequest]
//        API.Following_Methods.GetFollow_Request + "\(AppInstance.instance.sessionId ?? "")"
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.GeneralData.getGeneralDataApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            print(response.result.value)
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = GetFriendRequestModal.getFirend_RequestSuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetFriendRequestModal.getFirend_Request_ErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = GetFriendRequestManager()
    private init() {}
}

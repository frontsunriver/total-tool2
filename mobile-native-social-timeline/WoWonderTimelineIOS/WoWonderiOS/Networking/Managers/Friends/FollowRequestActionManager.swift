//
//  FollowRequestActionManager.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/4/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK

class FollowRequestActionManager{
    
    func followRequestAction(user_Id: String,action: String,completionBlock : @escaping (_ Success:FollowRequestActionModal.FollowRequest_SuccessModal?, _ AuthError :FollowRequestActionModal.FollowRequest_ErrorModel?, Error?)->()){
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,"request_action":action,APIClient.Params.userId:user_Id]
//        API.Following_Methods.Accept_DeclineRequest + "\(AppInstance.instance.sessionId ?? "")"
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.FollowRequest.followRequestApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(FollowRequestActionModal.FollowRequest_SuccessModal.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(FollowRequestActionModal.FollowRequest_ErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    static let sharedInstance = FollowRequestActionManager()
    private init() {}
}


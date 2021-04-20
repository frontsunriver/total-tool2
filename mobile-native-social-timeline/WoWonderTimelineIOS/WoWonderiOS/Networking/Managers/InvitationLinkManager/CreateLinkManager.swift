//
//  CreateLinkManager.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/6/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK


class CreateLinkManager{
    
    func createLink(completionBlock :@escaping (_ Success: CreateInvitationLinkModal.createLink_SuccessModal?, _ AuthError: CreateInvitationLinkModal.createLink_ErrorModal?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.type:"create"
            ] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"

        Alamofire.request(APIClient.InvtationLink.invitationLinkApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if (response.result.value != nil){
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if  apiStatusCode as? Int == 200{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(CreateInvitationLinkModal.createLink_SuccessModal.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(CreateInvitationLinkModal.createLink_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
        }
    }
    
    static let sharedInstance = CreateLinkManager()
    private init () {}
}

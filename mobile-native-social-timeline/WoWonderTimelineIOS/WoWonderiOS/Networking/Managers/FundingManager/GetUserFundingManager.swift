//
//  GetUserFundingManager.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/21/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK

class GetUserFundingManager{
    
    
    func getFunding(userId:String,offset:String,completionBlock :@escaping (_ Success: GetUserFundingModal.getUserFunding_SuccessModal?, _ AuthError: GetUserFundingModal.getUserFunding_ErrorModal?, Error?)->()){
        
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.type:"user_funding",APIClient.Params.limit:10,APIClient.Params.userId:userId,APIClient.Params.off_set:offset] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"

        Alamofire.request(APIClient.Funding.getfundingApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if (response.result.value != nil){
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = GetUserFundingModal.getUserFunding_SuccessModal.init(json: res)
                completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetUserFundingModal.getUserFunding_ErrorModal.self, from: data) else {return}
                     completionBlock(nil,result,nil)
                }
            }
            else{
                completionBlock(nil,nil,response.error)
            }
        }
        
    }
    
    static let sharedInstance = GetUserFundingManager()
    private init() {}
    
    
}

//
//  GetFundingDonationManager.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/12/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK

class GetFundingDonationsManager{
    
    func getDonations(fundId: Int,completionBlock :@escaping (_ Success: GetFundingDonationsModal.getDonation_SuccessModal?, _ AuthError: GetFundingDonationsModal.getDonation_ErrorModal?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.type:"get_recent_donations",APIClient.Params.limit:10,"fund_id":fundId] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.Funding.getfundingApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if (response.result.value != nil){
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = GetFundingDonationsModal.getDonation_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetFundingDonationsModal.getDonation_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else{
                completionBlock(nil,nil,response.error)
            }
        }
        
    }
    
    static let sharedInstance = GetFundingDonationsManager()
    private init () {}
    
}

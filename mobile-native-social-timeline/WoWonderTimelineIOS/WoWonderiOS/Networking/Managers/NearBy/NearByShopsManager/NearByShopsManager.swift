//
//  NearByShopsManager.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/10/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK

class NearByShopsManager{
    
    func nearByShops (offset: String, name: String,distance:Int,completionBlock :@escaping (_ Success: NearByShopsModal.nearByShops_SuccessModal?, _ AuthError: NearByShopsModal.nearByShops_ErrorModal?, Error?)->()){
        
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.limit:10,
            APIClient.Params.offset:offset,
            APIClient.Params.type : "shops",
            "name":name,
            APIClient.Params.distance:distance,
            ] as [String : Any]
        
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.NearByBusiness.nearByBusiness + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                   guard let apiStatusCode = res["api_status"] as? Any else {return}
                   if apiStatusCode as? Int == 200 {
                    let result = NearByShopsModal.nearByShops_SuccessModal.init(json: res)
                      completionBlock(result,nil,nil)
                  }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(NearByShopsModal.nearByShops_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else{
                completionBlock(nil,nil,response.error)
            }

        }
    }
    
    static let sharedInstacne = NearByShopsManager()
    private init() {}
    
}

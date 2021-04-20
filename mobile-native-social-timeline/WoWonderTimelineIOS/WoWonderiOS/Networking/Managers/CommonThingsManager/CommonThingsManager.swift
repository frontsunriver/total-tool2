//
//  CommonThingsManager.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/17/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK


class CommonThingsManager{
    
    func getCommonThings(limit:Int, offset: Int,completionBlock :@escaping (_ Success: CommonThingsModel.CommonThingsSuccessModel?, _ AuthError: CommonThingsModel.CommonThingsErrorModel?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            
            ] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.CommonThings.getCommonThings + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = CommonThingsModel.CommonThingsSuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(CommonThingsModel.CommonThingsErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = CommonThingsManager()
    private init() {}
}


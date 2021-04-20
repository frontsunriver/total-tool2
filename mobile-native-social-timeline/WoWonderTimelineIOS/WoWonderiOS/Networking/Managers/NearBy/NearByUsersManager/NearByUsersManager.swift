//
//  NearByUsersManager.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/13/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK


class NearByUsersManager{
    
    func getNearByUsers(limit:Int, offset: Int, gender: String, keyword: String,status:String,distance:Int,relationship:String,completionBlock :@escaping (_ Success: GetNearByUsersModel.GetNearByUsersSuccessModel?, _ AuthError: GetNearByUsersModel.GetNearByUsersErrorModel?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.limit:limit,
            APIClient.Params.offset:offset,
            APIClient.Params.gender:gender,
            APIClient.Params.keyword:keyword,
            APIClient.Params.status:status,
            APIClient.Params.distance:distance,
            APIClient.Params.relship:relationship,
            
            
            ] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.NearbyFriends.getNearByFriends + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = GetNearByUsersModel.GetNearByUsersSuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetNearByUsersModel.GetNearByUsersErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = NearByUsersManager()
    private init() {}
}


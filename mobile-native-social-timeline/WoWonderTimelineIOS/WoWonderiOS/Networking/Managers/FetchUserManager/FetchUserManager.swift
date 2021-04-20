//
//  FetchUserManager.swift
//  News_Feed
//
//  Created by Muhammad Haris Butt on 4/1/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK

class FetchUserManager{

static let instance = FetchUserManager()

    func fetchProfile(completionBlock :@escaping (_ Success: FetchUserModel.FetchUserSuccessModel?, _ AuthError: FetchUserModel.FetchUserErrorModel?, Error?)->()){
    let params = [
        APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
        APIClient.Params.userId:UserData.getUSER_ID() ?? "",
        APIClient.Params.fetch:"user_data",
        
        ] as [String : Any]
    let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.User_Data.getUserDataApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
        if response.result.value != nil{
            guard let res = response.result.value as? [String:Any] else {return}
            guard let apiStatusCode = res["api_status"] as? Any else {return}
            if apiStatusCode as? Int == 200{
                guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                guard let result = try? JSONDecoder().decode(FetchUserModel.FetchUserSuccessModel.self, from: data) else {return}
                completionBlock(result,nil,nil)
            }
                
            else {
                guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                guard let result = try? JSONDecoder().decode(FetchUserModel.FetchUserErrorModel.self, from: data) else {return}
                completionBlock(nil,result,nil)
            }
        }
        else {
            print(response.error?.localizedDescription)
            completionBlock(nil,nil,response.error)
        }
    }
}
}

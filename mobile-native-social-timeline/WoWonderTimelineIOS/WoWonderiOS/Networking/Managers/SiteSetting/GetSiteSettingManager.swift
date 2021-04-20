//
//  GetSiteSettingManager.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/2/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK

class GetSiteSettingManager{
    
    func getSiteSetting(completionBlock : @escaping (_ Success:Get_Site_SettingModel.siteSetting_SuccessModal?, _ AuthError : Get_Site_SettingModel.Get_Site_SettingErrorModel?, Error?)->()){
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key]
        Alamofire.request(APIClient.Stie_Setting.siteSettingApi, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if (response.result.value != nil){
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                if apiStatusCode as? Int == 200 {
                    let result = Get_Site_SettingModel.siteSetting_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else{
                    guard let allData = try? JSONSerialization.data(withJSONObject: response.value, options: [])
                        else {return}
                    guard let result = try? JSONDecoder().decode(Get_Site_SettingModel.Get_Site_SettingErrorModel.self, from: allData) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else{
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = GetSiteSettingManager()
    private init() {}
    
}

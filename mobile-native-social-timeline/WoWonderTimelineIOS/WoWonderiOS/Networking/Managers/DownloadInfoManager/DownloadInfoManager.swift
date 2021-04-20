//
//  DownloadInfoManager.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/10/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK

class DownloadInfoManager{
    
    func download_Info(data:String,completionBlock :@escaping (_ Success: DownloadInfoModal.downloadInfo_SuccessModal?, _ AuthError: DownloadInfoModal.downloadInfo_ErrorModal?, Error?)->()){
        
    let params = [
        APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,"data":data] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"

        Alamofire.request(APIClient.UserInfo.userInfoApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if (response.result.value != nil){
                guard let res = response.result.value as? [String:Any] else {return}
                 guard let apiStatusCode = res["api_status"] as? Any else {return}
                 if apiStatusCode as? Int == 200 {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(DownloadInfoModal.downloadInfo_SuccessModal.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(DownloadInfoModal.downloadInfo_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else{
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = DownloadInfoManager()
    private init() {}
    
}

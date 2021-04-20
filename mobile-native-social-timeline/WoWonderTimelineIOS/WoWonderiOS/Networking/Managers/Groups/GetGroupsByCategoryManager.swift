//
//  GetGroupsByCategoryManager.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/6/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK

class GetGroupsByCategoryManager{
    
    func getGroups(offSet:String,cate_id:Int,completionBlock :@escaping (_ Success: GetGroupsByCategory.getGroups_SuccessModal?, _ AuthError: GetGroupsByCategory.getGroups_ErrorModal?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.type:APIClient.Params.category,APIClient.Params.category:cate_id,APIClient.Params.limit:20,APIClient.Params.off_set:offSet
            ] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.GetMyGroups.getMyGroupsApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if (response.result.value != nil){
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
                    let result = GetGroupsByCategory.getGroups_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetGroupsByCategory.getGroups_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else{
                completionBlock(nil,nil,response.error)
            }
        }
        
    }
    
    static let sharedInstance = GetGroupsByCategoryManager()
    private init() {}
    
}

//
//  VoteUpManager.swift
//  News_Feed
//
//  Created by Ubaid Javaid on 4/25/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire

class VoteUpManager{
    
    func voteUp(id: Int,completionBlock :@escaping (_ Success: VoteUPModal.voteUp_SuccessModal?, _ AuthError: VoteUPModal.voteUp_ErrorModal?, Error?)->()){
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.id:id] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"

        Alamofire.request(APIClient.VoteUp.voteUpApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                let result = VoteUPModal.voteUp_SuccessModal.init(json: res)
                completionBlock(result,nil,nil)
                }
                else{
                guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                guard let result = try? JSONDecoder().decode(VoteUPModal.voteUp_ErrorModal.self, from: data) else {return}
                completionBlock(nil,result,nil)
                }
            }
        }
    }
    
    static let sharedInstance = VoteUpManager()
}


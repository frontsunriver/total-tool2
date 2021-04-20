//
//  CommentReactionsManager.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/15/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK

class GetCommentReactionsManager{
    
    func getReactions(commentId:Int,offset:Int,completionBlock : @escaping (_ Success:GetCommentReactionsModal.getReactions_SuccessModal?, _ AuthError : GetCommentReactionsModal.getReactions_ErrorModal? , Error?)->()){
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.type:"comment",APIClient.Params.id:commentId,APIClient.Params.reactions:"1,2,3,4,5,6",APIClient.Params.limit:15,"1_offset":offset] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"

        Alamofire.request(APIClient.FetchComment.fetchComment, method: .post, parameters: params, encoding:  URLEncoding.default, headers: nil).responseJSON { (response) in
            if (response.result.value != nil){
                guard let res = response.result.value as? [String:Any] else {return}
                 guard let apiStatusCode = res["api_status"]  as? Any else {return}
                 let apiCode = apiStatusCode as? Int
                 if apiCode == 200 {
            let result = GetCommentReactionsModal.getReactions_SuccessModal.init(json: res)
            completionBlock(result,nil,nil)
                }
                 else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetCommentReactionsModal.getReactions_ErrorModal.self, from: data)else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = GetCommentReactionsManager()
    private init() {}
    
}

//
//  DeleteEventManager.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/16/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK

class DeleteEventManager{
    
    func deleteEvent(eventId: Int,completionBlock : @escaping (_ Success :DeleteEventModal.deleteEvent_SuccessModal?, _ AuthError : DeleteEventModal.deleteEvent_ErrorModal?, Error?)->()){
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.type:"delete",APIClient.Params.event_id:eventId] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request("", method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if (response.result.value != nil){
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(DeleteEventModal.deleteEvent_SuccessModal.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(DeleteEventModal.deleteEvent_ErrorModal.self, from: data) else {return}
                      completionBlock(nil,result,nil)
                  }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
            
        }

    }
    
    static let sharedInstance = DeleteEventManager()
    private init() {}
    
    
}

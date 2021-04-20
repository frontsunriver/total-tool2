

import Foundation
import Alamofire

import WoWonderTimelineSDK

class SendGiftManager{
    
    func sendGiftManager(user_id: String,id: String,completionBlock :@escaping (_ Success: SendGiftModal.sendGift_SuccessModal?, _ AuthError: SendGiftModal.sendGift_ErrorModal?, Error?)->()){
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.userId:user_id,APIClient.Params.id:id,APIClient.Params.type: "send"]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.Gift.giftApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: [])else{return}
                    guard let result = try? JSONDecoder().decode(SendGiftModal.sendGift_SuccessModal.self, from: data)else {return}
                    
                    completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: [])else{return}
                    guard let result = try? JSONDecoder().decode(SendGiftModal.sendGift_ErrorModal.self, from: data)else {return}
                    
                    completionBlock(nil,result,nil)
                }
            }
        }
    }
    
    static let sharedInstance = SendGiftManager()
    private init() {}
    
}

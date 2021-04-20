

import Foundation
import WoWonderTimelineSDK

import Alamofire

class RatePageManager{
    
    func ratepage(page_id: String, val: String, text: String,completionBlock : @escaping (_ Success: RatePageModal.ratePage_SuccessModal?, _ AuthError :RatePageModal.ratePage_ErrorModal?, Error?)->()){
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.pageId:page_id, APIClient.Params.text:text, APIClient.Params.val:val]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.RatePage.ratePageApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            
                if response.result.value != nil{
                    guard let res = response.result.value as? [String:Any] else {return}
                    guard let apiStatusCode = res["api_status"] as? Any else {return}
                    if apiStatusCode as? Int == 200{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else{return}
                    guard let result = try? JSONDecoder().decode(RatePageModal.ratePage_SuccessModal.self, from: data) else{return}
                    completionBlock(result,nil,nil)
                    }
                    else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else{return}
                    guard let result = try? JSONDecoder().decode(RatePageModal.ratePage_ErrorModal.self, from: data) else{return}
                    completionBlock(nil,result,nil)
                    }
                }
                else{
                    print(response.error?.localizedDescription)
                    completionBlock(nil,nil,response.error)
                    
                }
        }
        
    }
   
    static let sharedInstance = RatePageManager()
    private init() {}
    
}

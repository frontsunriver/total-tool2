

import Foundation
import Alamofire
import WoWonderTimelineSDK


class UpgardeUserManager{
   
    func upgradeUser(type: Int,completionBlock :@escaping (_ Success: upgradeUserModal.upgradeUser_SuccessModal?, _ AuthError: upgradeUserModal.upgradeUser_ErrorModal?, Error?)->()){
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.type:type] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.Upgrade.upgradeApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: [])else{return}
                    guard let result = try? JSONDecoder().decode(upgradeUserModal.upgradeUser_SuccessModal.self, from: data)else {return}
                    
                    completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: [])else{return}
                    guard let result = try? JSONDecoder().decode(upgradeUserModal.upgradeUser_ErrorModal.self, from: data)else {return}
                    
                    completionBlock(nil,result,nil)
                }
            }
        }
    }
    
    
    static let sharedInstance = UpgardeUserManager()
    private init() {}
    
}

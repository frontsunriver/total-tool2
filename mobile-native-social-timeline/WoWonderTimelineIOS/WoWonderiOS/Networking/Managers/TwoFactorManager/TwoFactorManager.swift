

import Foundation
import Alamofire
import WoWonderTimelineSDK


class TwoFactorManager {
    
 static let instance = TwoFactorManager()
    func verifyCode (code : String, UserID : String, completionBlock : @escaping (_ Success:TwoFactorModel.TwoFactorSuccessModel?, _ AuthError : TwoFactorModel.TwoFactorErrorModel?, Error?)->()) {
        
        let params  = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.code : code, APIClient.Params.userId :UserID]
        
        Alamofire.request(APIClient.TwoFactorAuthentication.TwoFactorAuthApi, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                    guard let allData = try? JSONSerialization.data(withJSONObject: response.value, options: [])else {return}
                    guard let result = try? JSONDecoder().decode(TwoFactorModel.TwoFactorSuccessModel.self, from: allData) else {return}
                    
                    completionBlock(result,nil,nil)
                    
                }
                    
                else {
                    guard let allData = try? JSONSerialization.data(withJSONObject: response.value, options: [])else {return}
                    guard let result = try? JSONDecoder().decode(TwoFactorModel.TwoFactorErrorModel.self, from: allData) else {return}
                    
                    completionBlock(nil,result,nil)
                    
                }
                
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
            
            
        }
        
    }
    func updateTwoFactor ( completionBlock : @escaping (_ Success:UpdateTwoFactorModel.UpdateTwoFactorSuccessModel?, _ AuthError : UpdateTwoFactorModel.UpdateTwoFactorErrorModel?, Error?)->()) {
        
        let params  = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key]
             let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.UpdateTwoFactor.updateTwoFactorApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                    guard let allData = try? JSONSerialization.data(withJSONObject: response.value, options: [])else {return}
                    guard let result = try? JSONDecoder().decode(UpdateTwoFactorModel.UpdateTwoFactorSuccessModel.self, from: allData) else {return}
                    
                    completionBlock(result,nil,nil)
                    
                }
                    
                else {
                    guard let allData = try? JSONSerialization.data(withJSONObject: response.value, options: [])else {return}
                    guard let result = try? JSONDecoder().decode(UpdateTwoFactorModel.UpdateTwoFactorErrorModel.self, from: allData) else {return}
                    
                    completionBlock(nil,result,nil)
                    
                }
                
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    func updateVerifyTwoFactor ( code:String,Type:String,completionBlock : @escaping (_ Success:UpdateTwoFactorModel.UpdateTwoFactorSuccessModel?, _ AuthError : UpdateTwoFactorModel.UpdateTwoFactorErrorModel?, Error?)->()) {
        
        let params  = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,
                       APIClient.Params.code:code,
                       
                       APIClient.Params.type:Type,
        ]
       let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.UpdateTwoFactor.updateTwoFactorApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                    guard let allData = try? JSONSerialization.data(withJSONObject: response.value, options: [])else {return}
                    guard let result = try? JSONDecoder().decode(UpdateTwoFactorModel.UpdateTwoFactorSuccessModel.self, from: allData) else {return}
                    
                    completionBlock(result,nil,nil)
                    
                }
                    
                else {
                    guard let allData = try? JSONSerialization.data(withJSONObject: response.value, options: [])else {return}
                    guard let result = try? JSONDecoder().decode(UpdateTwoFactorModel.UpdateTwoFactorErrorModel.self, from: allData) else {return}
                    
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



import Foundation
import Alamofire
import WoWonderTimelineSDK

class ForgetPasswordManager {
    
    func forgetPassword(email: String,completionBlock : @escaping (_ Success:ForgetPasswordModel.forgetPasswordSuccessModel?, _ AuthError : ForgetPasswordModel.forgetPasswordErrorModel?, Error?)->()){
        
        let params = [APIClient.Params.email : email,APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key]
        
        Alamofire.request(APIClient.ForgetPassword.forgetPasswordApi, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                    
                    guard let allData = try? JSONSerialization.data(withJSONObject: response.value, options: [])
                        else {return}
                    guard let result = try? JSONDecoder().decode(ForgetPasswordModel.forgetPasswordSuccessModel.self, from: allData) else {return}
                    completionBlock(result,nil,nil)
                    
                }
                else {
                    guard let allData = try? JSONSerialization.data(withJSONObject: response.value, options: [])
                        else {return}
                    guard let result = try? JSONDecoder().decode(ForgetPasswordModel.forgetPasswordErrorModel.self, from: allData) else {return}
                    completionBlock(nil,result,nil)
                    
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
                
            }
            
            
            
        }
        
        
        
        
        
    }
    
    
    static let sharedInstance = ForgetPasswordManager()
    private init() {}
    
    
    
}


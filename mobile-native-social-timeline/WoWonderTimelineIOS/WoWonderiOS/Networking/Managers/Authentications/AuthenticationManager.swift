
import Foundation
import Alamofire
import WoWonderTimelineSDK


class AuthenticationManager {
    
    func loginAuthentication (userName : String, password : String,deviceId:String, completionBlock : @escaping (_ Success:AuthenticationModel.LoginAuth_SuccessModel?, _ AuthError : AuthenticationModel.LoginAuth_ErrorModel?, Error?)->()) {
        
        let params  = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.userName : userName, APIClient.Params.password :password,APIClient.Params.ios_n_device_id : deviceId]
        
        Alamofire.request(APIClient.Login_Authentication.loginAuthApi, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                    guard let allData = try? JSONSerialization.data(withJSONObject: response.value, options: [])else {return}
                    guard let result = try? JSONDecoder().decode(AuthenticationModel.LoginAuth_SuccessModel.self, from: allData) else {return}
                    
                    completionBlock(result,nil,nil)
                    
                }
                    
                else {
                    guard let allData = try? JSONSerialization.data(withJSONObject: response.value, options: [])else {return}
                    guard let result = try? JSONDecoder().decode(AuthenticationModel.LoginAuth_ErrorModel.self, from: allData) else {return}
                    
                    completionBlock(nil,result,nil)
                    
                }
                
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
            
            
        }
        
        
        
        
    }
    
    func signUPAuthentication(userName : String, password : String, email : String,confirmPassword : String ,deviceId:String,completionBlock : @escaping (_ Success:AuthenticationModel.SignUp_SuccessModel?, _ AuthError : AuthenticationModel.SignUp_ErrorModel?, Error?)->()) {
        
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key, APIClient.Params.userName : userName, APIClient.Params.password : password, APIClient.Params.email : email, APIClient.Params.confirmPassword : confirmPassword,APIClient.Params.ios_n_device_id : deviceId]
        print("API = \(APIClient.SignUp_Authentication.signupAuthApi)")
        Alamofire.request(APIClient.SignUp_Authentication.signupAuthApi, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                    guard let allData = try? JSONSerialization.data(withJSONObject: response.value, options: [])else {return}
                    guard let result = try? JSONDecoder().decode(AuthenticationModel.SignUp_SuccessModel.self, from: allData) else {return}
                    
                    completionBlock(result,nil,nil)
                    
                }
                    
                else {
                    guard let allData = try? JSONSerialization.data(withJSONObject: response.value, options: [])else {return}
                    guard let result = try? JSONDecoder().decode(AuthenticationModel.SignUp_ErrorModel.self, from: allData) else {return}
                    
                    completionBlock(nil,result,nil)
                    
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
            
            
        }
        
        
    }
    
    
    
    
    static let sharedInstance = AuthenticationManager()
    private init () {}
    
    
    
    
}

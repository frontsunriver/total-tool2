

import Foundation
import Alamofire
import WoWonderTimelineSDK


class UpdateUserManager{
    
    static let instance = UpdateUserManager()
    
    func updateTwoStepVerification( type: String,completionBlock :@escaping (_ Success: UpdateUserModel.UpdateUserDataSuccessModel?, _ AuthError: UpdateUserModel.UpdateUserDataErrorModel?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.two_factor:type
            
            ] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.UpdateProfile.updateProfileApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataSuccessModel.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                    
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    func updatePassword( currentPass: String,newPass:String,completionBlock :@escaping (_ Success: UpdateUserModel.UpdateUserDataSuccessModel?, _ AuthError: UpdateUserModel.UpdateUserDataErrorModel?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.currentPassword:currentPass,
            APIClient.Params.newPassword:newPass
            
            ] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.UpdateProfile.updateProfileApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataSuccessModel.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                    
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    func updateSocialLinks(paramType: String,Link:String,completionBlock :@escaping (_ Success: UpdateUserModel.UpdateUserDataSuccessModel?, _ AuthError: UpdateUserModel.UpdateUserDataErrorModel?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            paramType:Link
            
            ] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.UpdateProfile.updateProfileApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataSuccessModel.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                    
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    func updateMyAccount(username: String,email:String,birthday:String,gender:String,completionBlock :@escaping (_ Success: UpdateUserModel.UpdateUserDataSuccessModel?, _ AuthError: UpdateUserModel.UpdateUserDataErrorModel?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.userName:username,
            APIClient.Params.email:email,
            APIClient.Params.usr_birthday:birthday,
            APIClient.Params.gender:gender,
            ] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.UpdateProfile.updateProfileApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataSuccessModel.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                    
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    func updateAboutMe(aboutMe: String,completionBlock :@escaping (_ Success: UpdateUserModel.UpdateUserDataSuccessModel?, _ AuthError: UpdateUserModel.UpdateUserDataErrorModel?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.about:aboutMe,
            
            ] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.UpdateProfile.updateProfileApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataSuccessModel.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                    
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    func updateProfile(firstName: String,lastName:String,mobile:String,website:String,workSpace:String,school:String,location:String,completionBlock :@escaping (_ Success: UpdateUserModel.UpdateUserDataSuccessModel?, _ AuthError: UpdateUserModel.UpdateUserDataErrorModel?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.firstName:firstName,
            APIClient.Params.lastName:lastName,
            APIClient.Params.address:location,
            APIClient.Params.phoneNumber:mobile,
            APIClient.Params.website:website,
            APIClient.Params.working:workSpace,
            APIClient.Params.school:school
            
            
            ] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.UpdateProfile.updateProfileApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataSuccessModel.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                    
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    print("")
                    guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
     func deleteAccount(currentPassword: String,completionBlock :@escaping (_ Success: UpdateUserModel.UpdateUserDataSuccessModel?, _ AuthError: UpdateUserModel.UpdateUserDataErrorModel?, Error?)->()){
            let params = [
                APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                APIClient.Params.password:currentPassword,
                
                
                
                ] as [String : Any]
            let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.DeleteUser.deleteUserApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
                if response.result.value != nil{
                    guard let res = response.result.value as? [String:Any] else {return}
                    guard let apiStatusCode = res["api_status"] as? Any else {return}
                    if apiStatusCode as? Int == 200{
                        guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                        guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataSuccessModel.self, from: data) else {return}
                        completionBlock(result,nil,nil)
                    }
                        
                    else {
                        guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                        print("")
                        guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataErrorModel.self, from: data) else {return}
                        completionBlock(nil,result,nil)
                    }
                }
                else {
                    print(response.error?.localizedDescription)
                    completionBlock(nil,nil,response.error)
                }
            }
        }
    func updatePrivacy(key: String,value:Int,completionBlock :@escaping (_ Success: UpdateUserModel.UpdateUserDataSuccessModel?, _ AuthError: UpdateUserModel.UpdateUserDataErrorModel?, Error?)->()){
               let params = [
                   APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                    key:value
                   
                   ] as [String : Any]
               let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
           Alamofire.request(APIClient.UpdateProfile.updateProfileApi  + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
                   if response.result.value != nil{
                       guard let res = response.result.value as? [String:Any] else {return}
                       guard let apiStatusCode = res["api_status"] as? Any else {return}
                       if apiStatusCode as? Int == 200{
                           guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                           guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataSuccessModel.self, from: data) else {return}
                           completionBlock(result,nil,nil)
                       }
                           
                       else {
                           guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                           print("")
                           guard let result = try? JSONDecoder().decode(UpdateUserModel.UpdateUserDataErrorModel.self, from: data) else {return}
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

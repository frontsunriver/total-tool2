

import Foundation
import Alamofire
import WoWonderTimelineSDK

class UpdateUserDataManager {
    
    let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
    
    func updateUserData(firstName : String, lastName : String ,phoneNumber : String, website : String, address : String, workPlace : String, school : String, gender: String, completionBlock : @escaping (_ Success: UpdateUserDataModel.updateUserData_SuccessModel?, _ AuthError : UpdateUserDataModel.updateUserData_ErrorModel? , Error?)->()){
        
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.firstName : firstName, APIClient.Params.phoneNumber : phoneNumber, APIClient.Params.lastName : lastName, APIClient.Params.working : workPlace, APIClient.Params.address : address, APIClient.Params.website : website, APIClient.Params.school : school,APIClient.Params.gender: gender]
        
        Alamofire.request(APIClient.UpdateProfile.updateProfileApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            print(response.value)
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(UpdateUserDataModel.updateUserData_SuccessModel.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(UpdateUserDataModel.updateUserData_ErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
                
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
        
        
    }
    
    func uploadUserImage(imageType : String ,data : Data?, completionBlock : @escaping (_ Success:  UpdateUserDataModel.updateUserData_SuccessModel?, _ AuthError : UpdateUserDataModel.updateUserData_ErrorModel? , Error?)->()){
        
        let headers: HTTPHeaders = ["Content-type": "multipart/form-data"]
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key]
        
        Alamofire.upload(multipartFormData: { (multipartFormData) in
            for (key, value) in params {
                multipartFormData.append("\(value)".data(using: String.Encoding.utf8)!, withName: key as String)
            }
            if let data = data{
                multipartFormData.append(data, withName: imageType, fileName: "file.jpg", mimeType: "image/png")
            }
            
        },usingThreshold: UInt64.init(),to: APIClient.UpdateProfile.updateProfileApi + self.access_token,method : .post,headers: headers) { (result) in
            switch result{
            case .success(let upload, _, _):
                upload.responseJSON { (response) in
                    if (response.result.value != nil){
                        guard let res = response.result.value as? [String:Any] else {return}
                        guard let apiStatusCode = res["api_status"] as? Any else {return}
                        if apiStatusCode as? Int == 200{
                            guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                            guard let result = try? JSONDecoder().decode(UpdateUserDataModel.updateUserData_SuccessModel.self, from: data) else {return}
                            completionBlock(result,nil,nil)
                        }
                        else {
                            guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                            guard let result = try? JSONDecoder().decode(UpdateUserDataModel.updateUserData_ErrorModel.self, from: data) else {return}
                            completionBlock(nil,result,nil)
                        }
                    }
                    else{
                        print(response.error?.localizedDescription)
                        completionBlock(nil,nil,response.error)
                    }
                }
            case .failure(let error):
                print("Error in upload: \(error.localizedDescription)")
                completionBlock(nil,nil,error)
                
            }
        }
    }
    
    
    
    
    
    
    static let sharedInstance = UpdateUserDataManager()
    private init() {}
    
}

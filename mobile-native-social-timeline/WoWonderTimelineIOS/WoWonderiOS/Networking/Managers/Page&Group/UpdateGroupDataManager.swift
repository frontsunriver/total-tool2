

import Foundation
import Alamofire
import WoWonderTimelineSDK


class UpdateGroupDataManager {
    
    let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
    
    func updateGroupData(params : [String:Any], completionBlock : @escaping (_ Success: UpdateGroupDataModel.updateData_successModel?, _ AuthError : UpdateGroupDataModel.updateData_ErrorModel? , Error?)->()) {
        
        Alamofire.request(APIClient.UpdateGroupData.updateGroupDataApi + self.access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(UpdateGroupDataModel.updateData_successModel.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                    
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(UpdateGroupDataModel.updateData_ErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                    
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    

    func uploadImage(groupId : String, imageType : String ,data : Data?, completionBlock : @escaping (_ Success: UpdateGroupDataModel.updateData_successModel?, _ AuthError : UpdateGroupDataModel.updateData_ErrorModel? , Error?)->()){
            
            let headers: HTTPHeaders = ["Content-type": "multipart/form-data"]
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.groupId : groupId]
            
            Alamofire.upload(multipartFormData: { (multipartFormData) in

                for (key, value) in params {
                    multipartFormData.append("\(value)".data(using: String.Encoding.utf8)!, withName: key as String)
                }
                if let data = data{
                multipartFormData.append(data, withName: imageType, fileName: "file.jpg", mimeType: "image/png")
                        }

            },usingThreshold: UInt64.init(),to: APIClient.UpdateGroupData.updateGroupDataApi + self.access_token,method : .post,headers: headers) { (result) in
                switch result{
                case .success(let upload, _, _):
                    upload.responseJSON { (response) in
                        if (response.result.value != nil){
                    guard let res = response.result.value as? [String:Any] else {return}
                    guard let apiStatusCode = res["api_status"] as? Any else {return}
                    if apiStatusCode as? Int == 200{
            guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
            guard let result = try? JSONDecoder().decode(UpdateGroupDataModel.updateData_successModel.self, from: data) else {return}
                completionBlock(result,nil,nil)
                    }
                    else {
            guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
            guard let result = try? JSONDecoder().decode(UpdateGroupDataModel.updateData_ErrorModel.self, from: data) else {return}
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
    
    
    
    static var sharedInstance = UpdateGroupDataManager()
    private init () {}
}

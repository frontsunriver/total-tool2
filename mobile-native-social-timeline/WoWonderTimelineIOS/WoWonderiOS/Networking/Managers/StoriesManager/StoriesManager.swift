

import Foundation
import Alamofire
import WoWonderTimelineSDK


class StoriesManager{
    
    func getUserStories(offset: Int, limit: Int,completionBlock :@escaping (_ Success: GetStoriesModel.StoriesScuccessModel?, _ AuthError: GetStoriesModel.StoriesErrorModel?, Error?)->()){
        let params = [
            
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.offset:offset,
            APIClient.Params.limit:limit,
            ] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.Stories.getUserStories + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetStoriesModel.StoriesScuccessModel.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetStoriesModel.StoriesErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    func createStory(story_data:Data?,mimeType:String,type:String,text:String, completionBlock: @escaping (_ Success:CreateStoryModel.CreateStorySuccessModel?,_ AuthError:CreateStoryModel.CreateStoryErrorModel?, Error?) ->()){
        
        let params = [
            APIClient.Params.FileType : type,
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.text:text,
           
            ] as [String : Any]
        
        let jsonData = try! JSONSerialization.data(withJSONObject: params, options: [])
        let decoded = String(data: jsonData, encoding: .utf8)!
        print("Decoded String = \(decoded)")
        let headers: HTTPHeaders = [
            "Content-type": "multipart/form-data"
        ]
        
        Alamofire.upload(multipartFormData: { (multipartFormData) in
            for (key, value) in params {
                multipartFormData.append("\(value)".data(using: String.Encoding.utf8)!, withName: key as String)
            }
            if type == "video"{
                if let data = story_data{
                    multipartFormData.append(data, withName: "file", fileName: "file.mp4", mimeType: mimeType)
                }
            }else if type == "image"{
                if let data = story_data{
                     multipartFormData.append(data, withName: "file", fileName: "image.jpg", mimeType: mimeType)
                }
            }
           
        }, usingThreshold: UInt64.init(), to: APIClient.Stories.createStories +  "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)", method: .post, headers: headers) { (result) in
            switch result{
            case .success(let upload, _, _):
                upload.responseJSON { response in
                    print("Succesfully uploaded")
                    if (response.result.value != nil){
                        guard let res = response.result.value as? [String:Any] else {return}
                        print("Response = \(res)")
                        guard let apiStatus = res["api_status"]  as? Any else {return}
                        if apiStatus as? Int == 200{
                            let data = try! JSONSerialization.data(withJSONObject: response.value, options: [])
                            let result = try! JSONDecoder().decode(CreateStoryModel.CreateStorySuccessModel.self, from: data)
                            print("Success = \(result.storyID ?? 0)")
                            completionBlock(result,nil,nil)
                        }else{
                            print("apiStatus String = \(apiStatus)")
                            let data = try! JSONSerialization.data(withJSONObject: response.value, options: [])
                            let result = try! JSONDecoder().decode(CreateStoryModel.CreateStoryErrorModel.self, from: data)
                            print("AuthError = \(result.errors!.errorText)")
                            completionBlock(nil,result,nil)
                            
                        }
                        
                    }else{
                        print("error = \(response.error?.localizedDescription)")
                        completionBlock(nil,nil,response.error)
                    }
                    
                }
            case .failure(let error):
                print("Error in upload: \(error.localizedDescription)")
                completionBlock(nil,nil,error)
                
            }
        }
    }
    func deleteStory(storyId:Int, completionBlock: @escaping (_ Success:DeleteStoryModel.DeleteStorySuccessModel?,_ SessionError:DeleteStoryModel.DeleteStoryErrorModel?, Error?) ->()){
        
       let params = [
                  APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                  APIClient.Params.story_id:storyId,
                  ] as [String : Any]
              let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.Stories.deleteStory + access_token, method: .post, parameters: params, encoding:URLEncoding.default, headers: nil).responseJSON { (response) in
            
            if (response.result.value != nil){
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatus = res["code"]  as? Any else {return}
                if apiStatus as? Int == 200{
                    print("apiStatus Int = \(apiStatus)")
                    let data = try! JSONSerialization.data(withJSONObject: response.value!, options: [])
                    let result = try! JSONDecoder().decode(DeleteStoryModel.DeleteStorySuccessModel.self, from: data)
                    completionBlock(result,nil,nil)
                }else{
                    print("apiStatus String = \(apiStatus)")
                    let data = try! JSONSerialization.data(withJSONObject: response.value as Any, options: [])
                    let result = try! JSONDecoder().decode(DeleteStoryModel.DeleteStoryErrorModel.self, from: data)
                    print("AuthError = \(result.errors?.errorText ?? "")")
                    completionBlock(nil,result,nil)
                }
            }else{
                print("error = \(response.error?.localizedDescription ?? "")")
                completionBlock(nil,nil,response.error)
            }
        }
    }
    static let sharedInstance = StoriesManager()
    private init() {}
}

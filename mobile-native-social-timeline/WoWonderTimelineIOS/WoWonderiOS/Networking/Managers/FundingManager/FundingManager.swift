

import Foundation
import Alamofire
import WoWonderTimelineSDK


class FundingManager{
    
    static let instance = FundingManager()
    
    func getFundings(type:String,limit:Int,offset:Int,completionBlock :@escaping (_ Success: [[String:Any]]?, _ AuthError: GetFundingModel.GetFundingErrorModel?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
//            APIClient.Params.limit:limit,
//             APIClient.Params.offset:offset,
             APIClient.Params.type:type
            ] as [String : Any]
        
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.Funding.getfundingApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    guard let datamine = res["data"] as? [[String:Any]] else{return }
//                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
//                     let result = try? JSONDecoder().decode(GetFundingModel.GetFundingSuccessModel.self, from: data)
                    completionBlock(datamine,nil,nil)
                }
                    
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetFundingModel.GetFundingErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    func addDunding(type:String,title:String,description:String,amount:String,imageData:Data?, completionBlock: @escaping (_ Success:CreateFundingModel.CreateFundingSuccessModel?,_ AuthError:CreateFundingModel.CreateFundingErrorModel?, Error?) ->()){
        
       var param = [
            
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.description:description,
            APIClient.Params.title:title,
            APIClient.Params.amount:amount,
                APIClient.Params.type:type
            
            ]
        
        
        
        let jsonData = try! JSONSerialization.data(withJSONObject: param, options: [])
        let decoded = String(data: jsonData, encoding: .utf8)!
        print("Decoded String = \(decoded)")
        let headers: HTTPHeaders = [
            "Content-type": "multipart/form-data"
        ]
        
        Alamofire.upload(multipartFormData: { (multipartFormData) in
            for (key, value) in param {
                multipartFormData.append("\(value)".data(using: String.Encoding.utf8)!, withName: key as String)
            }
           if let data = imageData{
                              multipartFormData.append(data, withName: "image", fileName: "file.jpg", mimeType: "image/png")
                          }
           
        }, usingThreshold: UInt64.init(), to: APIClient.Funding.createFundingApi + "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)", method: .post, headers: headers) { (result) in
            switch result{
            case .success(let upload, _, _):
                upload.responseJSON { response in
                    print("Succesfully uploaded")
                    print("response = \(response.result.value)")
                    if (response.result.value != nil){
                        guard let res = response.result.value as? [String:Any] else {return}
                        print("Response = \(res)")
                        guard let apiStatusCode = res["api_status"] as? Any else {return}
                        if apiStatusCode as? Int     == 200{
                            print("apiStatus Int = \(apiStatusCode)")
                            let data = try! JSONSerialization.data(withJSONObject: response.value, options: [])
                            let result = try! JSONDecoder().decode(CreateFundingModel.CreateFundingSuccessModel.self, from: data)
                            print("Success = \(result.apiStatus ?? 0)")
                            completionBlock(result,nil,nil)
                        }else{
                            print("apiStatus String = \(apiStatusCode)")
                            let data = try! JSONSerialization.data(withJSONObject: response.value, options: [])
                            let result = try! JSONDecoder().decode(CreateFundingModel.CreateFundingErrorModel.self, from: data)
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
}

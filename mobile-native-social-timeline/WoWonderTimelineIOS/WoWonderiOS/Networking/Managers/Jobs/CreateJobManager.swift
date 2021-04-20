import WoWonderTimelineSDK

import Foundation
import Alamofire

class CreateJobManager {
    
    func createJob (jobTitle:String,jobDescription:String,location:String,jobCategory:String,jobType:String,minimumSalary:String,maximumSalary:String,salaryDate:String,currency:String,pageId:String,cuurentLat:Double,currentLng:Double,data:Data?, completionBlock : @escaping (_ Success: CreateJobModel.createJob_SuccessModel?, _ AuthError : CreateJobModel.createJob_ErrorModel? , Error?)->()){
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.jobTitle:jobTitle, APIClient.Params.jobDescription:jobDescription, APIClient.Params.location:location,APIClient.Params.jobCategory:jobCategory, APIClient.Params.jobType:jobType,APIClient.Params.minimumSalary:minimumSalary, APIClient.Params.maximumSalary:maximumSalary,APIClient.Params.type:"create", APIClient.Params.salaryDate:salaryDate,APIClient.Params.currency:currency,APIClient.Params.pageId:pageId,APIClient.Params.currentLat:cuurentLat, APIClient.Params.currentLng:currentLng, APIClient.Params.imageType:"upload"] as [String : Any]
        let headers: HTTPHeaders = ["Content-type": "multipart/form-data"]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.upload(multipartFormData: { (multipartFormData) in
            for (key, value) in params {
                multipartFormData.append("\(value)".data(using: String.Encoding.utf8)!, withName: key as String)
            }
            if let data = data{
            multipartFormData.append(data, withName: "thumbnail", fileName: "file.jpg", mimeType: "image/png")
            }
        },usingThreshold: UInt64.init(), to: APIClient.Job.jobApi + access_token,method: .post,headers: headers) { (result) in
         switch result{
            case .success(let upload, _, _):
             upload.responseJSON { (response) in
                print(response.value)
                if response.result.value != nil {
                    guard let res = response.result.value as? [String:Any] else {return}
                    guard let apiStatusCode = res["api_status"] as? Any else {return}
                   if apiStatusCode as? Int == 200{
                    let result = CreateJobModel.createJob_SuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                     }
                   else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(CreateJobModel.createJob_ErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                    }
                  }
                else {
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
    
    static let sharedInstance = CreateJobManager()
    private init() {}
    
}


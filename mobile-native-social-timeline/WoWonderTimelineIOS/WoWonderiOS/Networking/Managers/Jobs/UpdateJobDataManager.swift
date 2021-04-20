
import Foundation
import Alamofire
import WoWonderTimelineSDK

class UpdateJobDataManager{
    
    func updateJobData (jobId:Int,jobTitle:String,jobDescription:String,location:String,jobCategory:String,jobType:String,minimumSalary:String,maximumSalary:String,salaryDate:String, completionBlock : @escaping (_ Success: UpdateJobDataModal.updateJobData_SuccessModel?, _ AuthError : UpdateJobDataModal.updateJobData_ErrorModel?, Error?)->()){
        let params =  [APIClient.Params.serverKey :APIClient.SERVER_KEY.Server_Key, APIClient.Params.jobTitle: jobTitle, APIClient.Params.jobDescription : jobDescription, APIClient.Params.jobCategory :jobCategory, APIClient.Params.jobType :jobType, APIClient.Params.minimumSalary :minimumSalary, APIClient.Params.maximumSalary :maximumSalary, APIClient.Params.jobId :jobId, APIClient.Params.salaryDate :salaryDate, APIClient.Params.location :location, APIClient.Params.type : "edit"] as [String : Any]
    let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.Job.jobApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            print(response.value)
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
            guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
            guard let result = try? JSONDecoder().decode(UpdateJobDataModal.updateJobData_SuccessModel.self, from: data) else {return}
            completionBlock(result,nil,nil)
                }
                else {
            guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
            guard let result = try? JSONDecoder().decode(UpdateJobDataModal.updateJobData_ErrorModel.self, from: data) else {return}
                completionBlock(nil,result,nil)
                }
            }
            else{
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sahredInstance = UpdateJobDataManager()
    private init() {}
}

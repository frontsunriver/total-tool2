
import Foundation
import Alamofire
import WoWonderTimelineSDK


class GetAllJobsManager{
    func getAllJobs(offset: String,type: String,distance: Int,categoryId: String,jobType: String,keyword: String, completionBlock :@escaping (_ Success: GetAllJobsModal.getAllJobs_SuccessModal?, _ AuthError: GetAllJobsModal.getAllJobs_ErrorModal?, Error?)->()){
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.limit:15,APIClient.Params.keyword:keyword,APIClient.Params.off_set:offset,APIClient.Params.jobC_id:categoryId,APIClient.Params.jobType:jobType,APIClient.Params.type:type] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.Job.jobApi + access_token
            , method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = GetAllJobsModal.getAllJobs_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetAllJobsModal.getAllJobs_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = GetAllJobsManager()
    private init() {}
    
}

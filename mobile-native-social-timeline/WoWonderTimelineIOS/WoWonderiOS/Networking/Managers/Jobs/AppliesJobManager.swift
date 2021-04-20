

import Foundation
import Alamofire
import WoWonderTimelineSDK

class AppliesJobManager{
    
    func appliesJob (jobId :Int, offset :Int, completionBlock : @escaping (_ Success:
        AppliesJobModel.appliesJob_SuccessModel?, _ AuthError : AppliesJobModel.appliesJob_ErrorModel?, Error?)->()){
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.jobId:jobId, APIClient.Params.off_set:offset, APIClient.Params.limit:10,APIClient.Params.type:"get_apply"] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.Job.jobApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
                    let result = AppliesJobModel.appliesJob_SuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    print(data)
                guard let result = try? JSONDecoder().decode(AppliesJobModel.appliesJob_ErrorModel.self, from: data) else {return}
                    print(result)
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static var sharedInstance = AppliesJobManager()
    private init() {}
}



import Foundation
import Alamofire
import WoWonderTimelineSDK

class DeleteJobManager {
    
    func deleteJob(postId : String,completionBlock : @escaping (_ Success:
        DeleteJobModel.deleteJob_SuccessModel?, _ AuthError : DeleteJobModel.deleteJob_ErrorModel?, Error?)->()){
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.action:"delete", APIClient.Params.postId:postId]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.DeleteJob.deleteJobApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
              guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
              guard let result = try? JSONDecoder().decode(DeleteJobModel.deleteJob_SuccessModel.self, from: data) else {return}
              completionBlock(result,nil,nil)
                }
                else {
                guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                guard let result = try? JSONDecoder().decode(DeleteJobModel.deleteJob_ErrorModel.self, from: data) else {return}
                completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    public static var sharedInstance = DeleteJobManager()
    private init() {}
}


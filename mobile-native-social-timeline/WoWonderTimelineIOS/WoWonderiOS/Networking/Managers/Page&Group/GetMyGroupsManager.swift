

import Foundation
import Alamofire
import WoWonderTimelineSDK


class GetMyGroupsManager{
    
    func getMyGroups(userId : String,offset: String,completionBlock : @escaping (_ Success: GetMyGroupsModel.getMyGroup_SuccessModel?, _ AuthError : GetMyGroupsModel.getMyGroup_ErrorModel? , Error?)->()){
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.type : "my_groups",APIClient.Params.off_set : offset,APIClient.Params.userId:userId,APIClient.Params.limit : 25] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.GetMyGroups.getMyGroupsApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            print(response.value)
            if response.result.value != nil {
            guard let res = response.result.value as? [String:Any] else {return}
            guard let apiStatusCode = res["api_status"] as? Int else {return}
                if apiStatusCode == 200 {
                let result = GetMyGroupsModel.getMyGroup_SuccessModel.init(json: res)
                completionBlock(result,nil,nil)
                }
                else {
                guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    print(data)
                guard let result = try? JSONDecoder().decode(GetMyGroupsModel.getMyGroup_ErrorModel.self, from: data) else {return}
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
    
 static let sharedInstance = GetMyGroupsManager()
    private init() {}
}

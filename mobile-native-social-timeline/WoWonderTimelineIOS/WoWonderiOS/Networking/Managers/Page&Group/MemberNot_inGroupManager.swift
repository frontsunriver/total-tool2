

import Foundation
import Alamofire
import WoWonderTimelineSDK

class MemberNot_inGroupManager {
    
  
    func getNotGroupMember(groupId : Int,completionBlock : @escaping (_ Success:GetMemberNot_inGroupModel.GetMemberNot_inGroup_SuccessModel?, _ AuthError : GetMemberNot_inGroupModel.GetMemberNot_inGroup_ErrorModel? , Error?)->()){
        
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key, APIClient.Params.groupId:groupId] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.GetNotGroupMemebers.getNotGroupMemebrsApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                let result = GetMemberNot_inGroupModel.GetMemberNot_inGroup_SuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetMemberNot_inGroupModel.GetMemberNot_inGroup_ErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
                
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
        
    }
    static let sharedInstance = MemberNot_inGroupManager()
    private init() {}
}

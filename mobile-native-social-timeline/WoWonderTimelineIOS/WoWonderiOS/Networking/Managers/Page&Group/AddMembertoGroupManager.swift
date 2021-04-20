

import Foundation
import Alamofire
import WoWonderTimelineSDK


class AddMembertoGroupManager{
    
    
    func addMembertoGroup(groupId : String,userId : String,completionBlock : @escaping (_ Success: AddMembertoGroupModel.AddMembertoGroup_SuccessModel?, _ AuthError : AddMembertoGroupModel.AddMembertoGroup_ErrorModel? , Error?)->()){
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.groupId : groupId,APIClient.Params.userId:userId]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.AddMembertoGroup.addMmembertoGroupApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(AddMembertoGroupModel.AddMembertoGroup_SuccessModel.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(AddMembertoGroupModel.AddMembertoGroup_ErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
                
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = AddMembertoGroupManager()
    private init() {}
    
}

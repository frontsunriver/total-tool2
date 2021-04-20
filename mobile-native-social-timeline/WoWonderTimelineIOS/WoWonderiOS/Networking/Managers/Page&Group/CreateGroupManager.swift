

import Foundation
import Alamofire
import WoWonderTimelineSDK

class CreateGroupManager{
    
    func createGroup (groupName:String,groupTitle:String,aboutGroup:String,category:Int,Privacy:Int,completionBlock : @escaping (_ Success: CreateGroupModel.createGroup_SuccessModel?, _ AuthError : CreateGroupModel.createGroup_ErrorModel? , Error?)->()){
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.about : aboutGroup, APIClient.Params.groupName : groupName, APIClient.Params.groupTitle : groupTitle, APIClient.Params.category : category, APIClient.Params.groupPrivacy : Privacy] as [String : Any]
       let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.CreateGroup.createGroupApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            print(response.value)
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any]  else {return}
                guard let apiCodeString = res["api_status"] as? Any else {return}
                if apiCodeString as? Int == 200 {
                let result = CreateGroupModel.createGroup_SuccessModel.init(json: res)
                completionBlock(result,nil,nil)
                }
                else  {
        guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    print(data)
        guard let result = try? JSONDecoder().decode(CreateGroupModel.createGroup_ErrorModel.self, from: data) else {return}
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
    
    static let sharedInstance = CreateGroupManager()
    private init() {}
    
}

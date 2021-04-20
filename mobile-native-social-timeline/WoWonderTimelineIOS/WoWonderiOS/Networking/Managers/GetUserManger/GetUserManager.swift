

import Foundation
import Alamofire
import WoWonderTimelineSDK

class GetUserManager{

static let instance = GetUserManager()

    func getUsers(limit:Int,type:String,completionBlock :@escaping (_ Success: UserListModel.UserListSuccessModel?, _ AuthError: UserListModel.UserListErrorModel?, Error?)->()){
    let params = [
        APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
         APIClient.Params.type:type,
        APIClient.Params.limit:limit,
        APIClient.Params.userId:UserData.getUSER_ID() ?? "",
        
        ] as [String : Any]
    let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.userList.userlIst + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
        if response.result.value != nil{
            guard let res = response.result.value as? [String:Any] else {return}
            guard let apiStatusCode = res["api_status"] as? Any else {return}
            if apiStatusCode as? Int == 200{
                guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: .prettyPrinted) else {return}
                 let result = try! JSONDecoder().decode(UserListModel.UserListSuccessModel.self, from: data)
                completionBlock(result,nil,nil)
            }
                
            else {
                guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                guard let result = try? JSONDecoder().decode(UserListModel.UserListErrorModel.self, from: data) else {return}
                completionBlock(nil,result,nil)
            }
        }
        else {
            print(response.error?.localizedDescription)
            completionBlock(nil,nil,response.error)
        }
    }
}
}

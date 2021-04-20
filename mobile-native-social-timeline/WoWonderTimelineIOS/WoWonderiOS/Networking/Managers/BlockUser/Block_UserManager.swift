
import Foundation
import Alamofire
import WoWonderTimelineSDK


class Block_UserManager{
    
    func blockUser (user_Id : String, blockUser : String,completionBlock : @escaping (_ Success:Block_UserModel.BlockUser_SuccessModel?, _ AuthError : Block_UserModel.BlockUser_ErrorModel? , Error?)->()){
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.userId : user_Id, APIClient.Params.blockUser : blockUser]
        let accessToken = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.Block_User.bockUserApi + accessToken, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                guard let result = try? JSONDecoder().decode(Block_UserModel.BlockUser_SuccessModel.self, from: data) else {return}
                    
                 completionBlock(result,nil,nil)
                }
                else {
        guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
        guard let result = try? JSONDecoder().decode(Block_UserModel.BlockUser_ErrorModel.self,from: data) else {return}
            completionBlock(nil,result,nil)
              }
                
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = Block_UserManager()
    private init () {}
    
}


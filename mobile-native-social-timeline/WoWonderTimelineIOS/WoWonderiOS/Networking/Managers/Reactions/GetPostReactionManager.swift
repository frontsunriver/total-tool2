

import Foundation
import Alamofire
import WoWonderTimelineSDK


class GetPostReactionManager {
    
    func getReactions(type: String,postID: String,completionBlock : @escaping (_ Success:GetPost_ReactionsModel.GetPostReactionSuccessModel?, _ AuthError : GetPost_ReactionsModel.GEtPostReactionErrorModel? , Error?)->()) {
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key, APIClient.Params.id : postID,APIClient.Params.type: type ,APIClient.Params.reactions : "1,2,3,4,5,6"]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.GetReactions.getPostReactionApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200{
                let result = GetPost_ReactionsModel.GetPostReactionSuccessModel.init(json: res)
                 completionBlock(result,nil,nil)
                    
                }
                else {
            guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
            guard let result = try? JSONDecoder().decode(GetPost_ReactionsModel.GEtPostReactionErrorModel.self, from: data) else {return}
                    
                  completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
                
            }
            
        }
    }
    
    
 static var sharedInstance = GetPostReactionManager()
    private init() {}
    
    
    
}

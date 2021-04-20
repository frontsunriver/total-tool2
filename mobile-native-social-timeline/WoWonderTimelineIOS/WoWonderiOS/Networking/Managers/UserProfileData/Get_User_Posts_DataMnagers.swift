

import Foundation
import Alamofire
import WoWonderTimelineSDK

class Get_Users_Posts_DataManager {
    
    
    func get_User_PostsData(access_token : String, limit : Int, id : String, off_set : String ,completionBlock : @escaping (_ Success:Get_User_Post_DataModel.get_User_PostSuccessModel?, _ AuthError : Get_User_Post_DataModel.get_User_PostErrorModel? , Error?)->()){
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key, APIClient.Params.type : "get_user_posts", APIClient.Params.limit : limit, APIClient.Params.offset : off_set, APIClient.Params.id : id] as [String : Any]
        
        Alamofire.request(APIClient.User_Posts_Data.getUserPostsApi + access_token,method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            print(response.value)
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode != nil {
                    let result = Get_User_Post_DataModel.get_User_PostSuccessModel(json: res)
                    print(result)
                    
                    completionBlock(result,nil,nil)
                }
                else {
                    
                    guard let alldata = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    
                    guard let result = try? JSONDecoder().decode(Get_User_Post_DataModel.get_User_PostErrorModel.self, from: alldata) else {return}
                    completionBlock(nil,result,nil)
                    
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
                
            }
            
            
        }
    }
    
    static let sharedInstance = Get_Users_Posts_DataManager()
    private init() {}
    
    
}



import Foundation
import Alamofire
import WoWonderTimelineSDK


class Get_User_DataManagers {
    
    func get_User_Data (userId : String, access_token : String, completionBlock : @escaping (_ Success:Get_User_DataModel.get_Uers_DataSuccessModel?, _ AuthError : Get_User_DataModel.get_Uers_DataErrorModel? , Error?)->()){
        
    let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key, APIClient.Params.userId : userId, APIClient.Params.fetch : "followers,user_data,followers,following,liked_pages,joined_groups"] as [String : Any]
        
        Alamofire.request(APIClient.User_Data.getUserDataApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                    let result = Get_User_DataModel.get_Uers_DataSuccessModel(json: res)
                    print(result)
                    completionBlock(result,nil,nil)
                }
                    
                else {
                    guard let alldata = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    
                    guard let result = try? JSONDecoder().decode(Get_User_DataModel.get_Uers_DataErrorModel.self, from: alldata) else {return}
                    completionBlock(nil,result,nil)
                    
                }
            }
                
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
                
            }
        }
        
    }
    
    
 static let sharedInstance = Get_User_DataManagers()
    private init() {}
    
    
    
    
}



import Foundation
import Alamofire
import WoWonderTimelineSDK


class Get_Site_Setting_Manager {
    
    func get_Site_Setting (completionBlock : @escaping (_ Success:Get_Site_SettingModel.Site_Setting_SuccessModel?, _ AuthError : Get_Site_SettingModel.Get_Site_SettingErrorModel?, Error?)->()) {
        
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key]
        print("Params",params)
        Alamofire.request(APIClient.Stie_Setting.siteSettingApi, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
          print("API",APIClient.Stie_Setting.siteSettingApi)
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                print(response.value)
                if apiCode == 200{
                    
                    let allData = try? JSONSerialization.data(withJSONObject: response.value, options: .prettyPrinted)
                       
                    let result = try? JSONDecoder().decode(Get_Site_SettingModel.Site_Setting_SuccessModel.self, from: allData!)
                    print(result)
                    completionBlock(result,nil,nil)
                }
                    
                else {
                    guard let allData = try? JSONSerialization.data(withJSONObject: response.value, options: [])
                        else {return}
                    guard let result = try? JSONDecoder().decode(Get_Site_SettingModel.Get_Site_SettingErrorModel.self, from: allData) else {return}
                    completionBlock(nil,result,nil)
                }
            }
                
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
  static let sharedInstance = Get_Site_Setting_Manager()
    private init() {}
}

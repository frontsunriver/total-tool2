

import Foundation
import Alamofire
import WoWonderTimelineSDK

class Get_User_ImageManager {
    
    func getUserImages (user_id : String, param : String, completionBlock : @escaping (_ Success: Get_Users_ImagesModel.get_UserImages_SuccessModel?, _ AuthError :Get_Users_ImagesModel.get_UserImages_ErrorModel? , Error?)->()){
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.type : param, APIClient.Params.userId : user_id] as [String : Any]
        let access_token =  "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.User_Images.getUserImagesApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            print(response.value)
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                    let result = Get_Users_ImagesModel.get_UserImages_SuccessModel.init(json: res)
                    print(result)
                    completionBlock(result,nil,nil)
                }
                    
                else {
                    guard  let allData = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(Get_Users_ImagesModel.get_UserImages_ErrorModel.self ,from: allData) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        
        
    }

}
static let sharedInstance = Get_User_ImageManager()
    private init() {}

}

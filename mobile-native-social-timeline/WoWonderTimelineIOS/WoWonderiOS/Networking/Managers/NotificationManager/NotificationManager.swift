
import Foundation
import Alamofire
import WoWonderTimelineSDK


class NotificationManager{
    
    static let instance = NotificationManager()
    private init() {}
    
    func getNotification(offset: Int, limit: Int,type:String,completionBlock :@escaping (_ Success: GetNotificationModel.GetNotificationSuccessModel?, _ AuthError: GetNotificationModel.GetNotificationErrorModel?, Error?)->()){
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                      APIClient.Params.fetch:type] as [String : Any]
        //            APIClient.Params.offset:offset,
        //            APIClient.Params.limit:limit,
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.GeneralData.getGeneralDataApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
//                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
//        let result = try? JSONDecoder().decode(GetNotificationModel.GetNotificationSuccessModel.self, from: data)
            let result = GetNotificationModel.GetNotificationSuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetNotificationModel.GetNotificationErrorModel.self, from: data) else {return}
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


import Foundation
import Alamofire
import WoWonderTimelineSDK


class GetEventManager {
    
    func getEvents(fetch :String,offset :String, myoffset :String,completionBlock : @escaping (_ Success :GetEventModel.getEvent_SuccessModel?, _ AuthError : GetEventModel.getEvent_ErrorModel?, Error?)->()) {
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.limit:15 ,APIClient.Params.off_set:offset, APIClient.Params.myOffset:myoffset, APIClient.Params.fetch:fetch] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.Events.getEventsApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = GetEventModel.getEvent_SuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetEventModel.getEvent_ErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = GetEventManager()
    private init() {}
}

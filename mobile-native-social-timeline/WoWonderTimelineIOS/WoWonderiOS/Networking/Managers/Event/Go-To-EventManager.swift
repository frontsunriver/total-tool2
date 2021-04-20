

import Foundation
import Alamofire
import WoWonderTimelineSDK

class GoToEventManager {
    
    func gotoEvent (eventId: String, completionBlock : @escaping (_ Success: GoToEventModal.gotoEvent_SuccessModel?, _ AuthError: GoToEventModal.gotoEvent_ErrorModal?, Error?)->()) {
        let params  = [APIClient.Params.serverKey :APIClient.SERVER_KEY.Server_Key, APIClient.Params.eventId :eventId]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.Events.gotoEventApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            print(response.value)
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GoToEventModal.gotoEvent_SuccessModel.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GoToEventModal.gotoEvent_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = GoToEventManager()
    private init() {}
    
}

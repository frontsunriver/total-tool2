

import Foundation
import Alamofire
import WoWonderTimelineSDK

class GetEventPostManager{
    
    func getEventPost(eventId :String,offset :String, completionBlock :@escaping (_ Success: GetEventPostModal.getEventPost_SuccessModal?, _ AuthError: GetEventPostModal.getEventPost_ErrorModel?, Error?)->()){
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.type: "get_event_posts", APIClient.Params.limit:15,APIClient.Params.id:eventId,APIClient.Params.offset:offset] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.Get_News_Feed.get_News_Feed_Posts + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = GetEventPostModal.getEventPost_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetEventPostModal.getEventPost_ErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = GetEventPostManager()
    private init() {}
    
}

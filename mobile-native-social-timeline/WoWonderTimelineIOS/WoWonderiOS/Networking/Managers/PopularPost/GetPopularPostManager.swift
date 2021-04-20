import WoWonderTimelineSDK
import Foundation
import Alamofire

class GetPostPopularManager{
    
    func getPopularPost(offset: String, completionBlock :@escaping (_ Success: GetPopularPostModal.getPopularPost_SuccessModal?, _ AuthError: GetPopularPostModal.getPopularPost_ErrorModal?, Error?)->()) {
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.offset:offset,APIClient.Params.limit:15] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.PopularPost.getPopularPostApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
                    let result = GetPopularPostModal.getPopularPost_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetPopularPostModal.getPopularPost_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
        }
        
    }
    
    static let sharedInstance = GetPostPopularManager()
    private init() {}
    
}

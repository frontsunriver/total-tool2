

import Foundation
import Alamofire
import WoWonderTimelineSDK

class GetPagePostManager{
    
 
    func getGroupPost(pageId : String,afterPostId : String, completionBlock : @escaping (_ Success:GetPagePostModel.getPagePost_SuccessModel?, _ AuthError : GetPagePostModel.getPagePost_ErrorModel? , Error?)->()){
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.type : "get_page_posts", APIClient.Params.limit : 10, APIClient.Params.id : pageId, APIClient.Params.afterPostId : afterPostId] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.GetPagePost.getPagePostApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                    let result = GetPagePostModel.getPagePost_SuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject:response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetPagePostModel.getPagePost_ErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
                
            }
                
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
   
    static let sharedInstance = GetPagePostManager()
    private init() {}
    
}

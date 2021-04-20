
import Foundation
import Alamofire
import WoWonderTimelineSDK


class GetBlogManager{
    
    func getBlog (categoryId : Int, offset :String, completionBlock : @escaping (_ Success:GetBlogModel.getBlog_SuccessModel?, _ AuthError :GetBlogModel.getBlog_ErrorModel?, Error?)->()){
    
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.category:categoryId, APIClient.Params.off_set : offset, APIClient.Params.limit:15] as [String : Any]
       let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.Articles.getBlogsApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = GetBlogModel.getBlog_SuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetBlogModel.getBlog_ErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            
            else {
            print(response.error?.localizedDescription)
            completionBlock(nil,nil,response.error)
          }
      }
  }
    
    static let sharedInstance = GetBlogManager()
    private init() {}

}

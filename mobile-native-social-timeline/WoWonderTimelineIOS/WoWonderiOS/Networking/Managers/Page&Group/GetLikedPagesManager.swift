
import Foundation
import Alamofire
import WoWonderTimelineSDK


class GetLikedPagesManager {
    
    func getLikedPages (userId : String,offset: String,completionBlock : @escaping (_ Success: GetLikedPageModel.getLikedPage_SuccessModel?, _ AuthError : GetLikedPageModel.getLikedPage_ErrorModel? , Error?)->()){
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.type : "liked_pages",APIClient.Params.off_set : offset,APIClient.Params.userId:userId,APIClient.Params.limit : 10] as [String : Any]
      let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.GetLikedPages.getMyLikedPagesApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
                   print(response.value)
                   if response.result.value != nil {
                   guard let res = response.result.value as? [String:Any] else {return}
                   guard let apiStatusCode = res["api_status"] as? Int else {return}
                       if apiStatusCode == 200 {
                    let result =  GetLikedPageModel.getLikedPage_SuccessModel.init(json: res)
                       completionBlock(result,nil,nil)
                       }
                       else {
                       guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                           print(data)
                        guard let result = try? JSONDecoder().decode(GetLikedPageModel.getLikedPage_ErrorModel.self, from: data) else {return}
                           print(result)
                       completionBlock(nil,result,nil)
                       }
                   }
                   else {
                       print(response.error?.localizedDescription)
                       completionBlock(nil,nil,response.error)
          }
        }
    }
  static let sharedInstance = GetLikedPagesManager()
    private init() {}
    
}



import Foundation
import Alamofire
import WoWonderTimelineSDK


class GetMyPagesManager {
    
    func getLikedPages (userId : String,offset: String,completionBlock : @escaping (_ Success: GetMyPageModel.getMyPages_SuccessModel?, _ AuthError : GetMyPageModel.getMyPages_ErrorModel? , Error?)->()){
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.type : "my_pages",APIClient.Params.off_set : offset,APIClient.Params.userId:userId,APIClient.Params.limit : 20] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.GetMyPages.getMyPagesApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            print(response.value)
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
                    let result =  GetMyPageModel.getMyPages_SuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    print(data)
                    guard let result = try? JSONDecoder().decode(GetMyPageModel.getMyPages_ErrorModel.self, from: data) else {return}
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
    static let sharedInstance = GetMyPagesManager()
    private init() {}
    
}

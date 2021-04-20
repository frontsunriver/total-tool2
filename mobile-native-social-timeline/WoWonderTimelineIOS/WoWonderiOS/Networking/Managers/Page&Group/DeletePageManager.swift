
import Foundation
import Alamofire
import WoWonderTimelineSDK


class DeletePageManager {
    
    func deletePage (pageId : String, password : String, completionBlock : @escaping (_ Success: DeletePageModel.DeletePage_SuccessModel?, _ AuthError : DeletePageModel.DeletePage_ErrorModel? , Error?)->()){
    let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key ,APIClient.Params.pageId : pageId, APIClient.Params.password : password]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.DeletePage.deletePageAPi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(DeletePageModel.DeletePage_SuccessModel.self, from: data) else {return}
                    completionBlock(result,nil,nil)
                }
                else {
    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options : []) else {return}
    guard let result = try? JSONDecoder().decode(DeletePageModel.DeletePage_ErrorModel.self, from: data) else {return}
        completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = DeletePageManager()
    private init () {}
}

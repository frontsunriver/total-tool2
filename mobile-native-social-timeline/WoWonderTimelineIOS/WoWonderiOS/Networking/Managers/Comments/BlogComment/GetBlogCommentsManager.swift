import WoWonderTimelineSDK

import Foundation
import Alamofire

class GetBlogCommentsManager{
    func getBlogComments(blogId: String,completionBlock :@escaping (_ Success: GetBlogCommentsModal.getBlogComments_SuccessModal?, _ AuthError: GetBlogCommentsModal.getBlogComments_ErrorModal?, Error?)->()){
    let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.blogId:blogId,APIClient.Params.type:"get_comments"]
    let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.BlogComments.blogCommentApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default,headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
            let result = GetBlogCommentsModal.getBlogComments_SuccessModal.init(json: res)
            completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetBlogCommentsModal.getBlogComments_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else{
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    static let sharedInstance = GetBlogCommentsManager()
    private init() {}
}

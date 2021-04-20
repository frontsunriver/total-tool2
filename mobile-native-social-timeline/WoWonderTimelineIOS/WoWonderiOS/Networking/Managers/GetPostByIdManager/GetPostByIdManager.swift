
import Foundation
import Alamofire
import WoWonderTimelineSDK


class GetPostByIdManager{
    
    func getPost(post_id: String,completionBlock :@escaping (_ Success: GetPostByIdModal.GetPost_SuccessModal?, _ AuthError: GetPostByIdModal.GetPost_ErrorModal?, Error?)->()){
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.fetch:"post_data",APIClient.Params.postId:post_id]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.GetPostById.getPostApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
                    let result = GetPostByIdModal.GetPost_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetPostByIdModal.GetPost_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else{
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    static let sharedInstance = GetPostByIdManager()
    private init() {}
}

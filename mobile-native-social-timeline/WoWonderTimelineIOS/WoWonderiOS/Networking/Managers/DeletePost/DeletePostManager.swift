
import Foundation
import Alamofire
import WoWonderTimelineSDK


class DeletePostManager{
   
    func deletePost(postId: String,completionBlock :@escaping (_ Success: DeletePostModal.deletePost_SuccessModal?, _ AuthError: DeletePostModal.deletePost_ErrorModal?, Error?)->()){
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.action: "delete",APIClient.Params.postId:postId]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.DeletePost.deletePostApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(DeletePostModal.deletePost_SuccessModal.self, from: data) else{return}
                     completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(DeletePostModal.deletePost_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else{
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    func postDelete(targetController: UIViewController, postId: String,completion:@escaping (Bool) -> ()) {
        performUIUpdatesOnMain {
            self.deletePost(postId: postId) { (success, authError, error) in
                if success != nil {
                    targetController.view.makeToast("Post Deleted")
                   completion(true)
                }
                else if authError != nil{
                    targetController.view.makeToast(authError?.errors.errorText)
                }
                else{
                    targetController.view.makeToast(error?.localizedDescription)
                }
            }
        }
    }
    
    static let sharedInstance = DeletePostManager()
    private init () {}
}


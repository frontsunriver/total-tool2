

import Foundation
import WoWonderTimelineSDK

import Alamofire

class SavePostManager{
    
    func savePost(action: String,postId: String, completionBlock :@escaping (_ Success: SavePostModal.savePost_SuccessModal?, _ AuthError: SavePostModal.savePost_ErrorModal?, Error?)->()){
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.action: action,APIClient.Params.postId:postId]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.SavePost.savePostApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
            guard let res = response.result.value as? [String:Any] else {return}
            guard let apiStatusCode = res["api_status"] as? Any else {return}
            if apiStatusCode as? Int == 200 {
                guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                guard let result = try? JSONDecoder().decode(SavePostModal.savePost_SuccessModal.self, from: data) else{return}
              completionBlock(result,nil,nil)
                }
            else{
                guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                guard let result = try? JSONDecoder().decode(SavePostModal.savePost_ErrorModal.self, from: data) else {return}
                completionBlock(nil,result,nil)
                }
            }
            else{
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    
    func savedPost(targetController: UIViewController, postId: String,action: String){
        performUIUpdatesOnMain {
            self.savePost(action: action, postId: postId) { (success, authError, error) in
                 if success != nil{
                    targetController.view.makeToast(success?.action)
                 }
                 else if authError != nil{
                     targetController.view.makeToast(authError?.errors.errorText)
                 }
                 else if error != nil{
                     targetController.view.makeToast(error?.localizedDescription)
                 }
             }
         }
    }
    static let sharedInstance = SavePostManager()
    private init() {}
}

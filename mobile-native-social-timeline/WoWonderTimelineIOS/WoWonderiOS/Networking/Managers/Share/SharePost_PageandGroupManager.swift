

import Foundation
import Alamofire
import WoWonderTimelineSDK


class SharePost_PageandGroupManager{
   
    func sharePostonPageandGroup(type: String, text: String, postId: String, pageId: String, groupId : String, completionBlock :@escaping (_ Success: SharePostOnGroupandPageModal.SharePostOnGroupandPageModal_SuccessModal?, _ AuthError: SharePostOnGroupandPageModal.SharePostOnGroupandPageModal_ErrorModal?, Error?)->()){
        
        let params = [APIClient.Params.serverKey: APIClient.SERVER_KEY.Server_Key, APIClient.Params.type: type, APIClient.Params.id: postId, APIClient.Params.text: text, APIClient.Params.groupId: groupId, APIClient.Params.pageId: pageId]
       let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.Get_News_Feed.get_News_Feed_Posts + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = SharePostOnGroupandPageModal.SharePostOnGroupandPageModal_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(SharePostOnGroupandPageModal.SharePostOnGroupandPageModal_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
            
        }
    }
    
    
    static let sharedInstance = SharePost_PageandGroupManager()
    private init() {}
    
    
}

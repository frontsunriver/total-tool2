
import Foundation
import Alamofire
import WoWonderTimelineSDK


class GetMyGroups_PagesManager{
    
func getMyPages (completionBlock : @escaping (_ Success: GetMyGroupsandPages_Modal.GetMyPages_SuccessModal?, _ AuthError :GetMyGroupsandPages_Modal.GetMyGroupandPage_ErrorModal? , Error?)->()){
    let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.fetch : "pages",APIClient.Params.userId:UserData.getUSER_ID()!]
    let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
    Alamofire.request(APIClient.GetCommunity.getcummunityApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
        if response.result.value != nil {
            guard let res = response.result.value as? [String:Any] else {return}
            guard let apiStatusCode = res["api_status"] as? Any else {return}
            if apiStatusCode as? Int == 200{
                let result = GetMyGroupsandPages_Modal.GetMyPages_SuccessModal.init(json: res)
                completionBlock(result,nil,nil)
            }
            else {
                guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                guard let result = try? JSONDecoder().decode(GetMyGroupsandPages_Modal.GetMyGroupandPage_ErrorModal.self, from: data) else {return}
                completionBlock(nil,result,nil)
            }
        }
        
        else {
            print(response.error?.localizedDescription)
            completionBlock(nil,nil,response.error)
        }
        
      }
    
    }
    
    func getMyGroups (completionBlock : @escaping (_ Success: GetMyGroupsandPages_Modal.GetMyGroups_SuccessModal?, _ AuthError :GetMyGroupsandPages_Modal.GetMyGroupandPage_ErrorModal? , Error?)->()){
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.fetch : "groups",APIClient.Params.userId:UserData.getUSER_ID()!]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.GetCommunity.getcummunityApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = GetMyGroupsandPages_Modal.GetMyGroups_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetMyGroupsandPages_Modal.GetMyGroupandPage_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
            
        }
        
    }
    static let sharedInstance = GetMyGroups_PagesManager()
    private init() {}
    
}

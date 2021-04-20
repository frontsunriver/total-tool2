

import Foundation
import Alamofire
import WoWonderTimelineSDK


class CretaPageManager {
    
    func cretaPage (pageName:String,pageTitle:String,aboutPage:String,category:Int, completionBlock : @escaping (_ Success: CreatePageModel.createPage_SuccessModel?, _ AuthError : CreatePageModel.createPage_ErrorModel? , Error?)->() ) {
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.pageDecription : aboutPage, APIClient.Params.pageTitle : pageTitle, APIClient.Params.pageName : pageName, APIClient.Params.pageCategory : category] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.CreatePage.createPageApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any]  else {return}
                guard let apiCodeString = res["api_status"] as? Any else {return}
                if (apiCodeString as? Int == 200) {
                    let result = CreatePageModel.createPage_SuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
               
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    print(data)
                    guard let result = try? JSONDecoder().decode(CreatePageModel.createPage_ErrorModel.self, from: data) else {return}
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

    static let sharedInstance = CretaPageManager()
    private init() {}
    
}

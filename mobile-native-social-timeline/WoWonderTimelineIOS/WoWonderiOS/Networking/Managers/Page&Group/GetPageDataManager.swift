
import Foundation
import Alamofire
import WoWonderTimelineSDK


class GetPageDataManager{
    
    func getData(page_id: String,completionBlock :@escaping (_ Success: GetPageDataModal.GetPageData_SuccessModal?, _ AuthError: GetPageDataModal.GetPageData_ErrorModal?, Error?)->()){
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,APIClient.Params.pageId:page_id]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.GetPageData.getPageDataApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
                if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
                    let result = GetPageDataModal.GetPageData_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetPageDataModal.GetPageData_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
        }
    }
    
    static let sharedInstance = GetPageDataManager()
    private init() {}

}

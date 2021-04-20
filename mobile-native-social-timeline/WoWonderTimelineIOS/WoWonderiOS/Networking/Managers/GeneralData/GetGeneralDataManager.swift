
import Foundation
import Alamofire
import WoWonderTimelineSDK

class GetGeneralDataManager {
    
    func getGeneralDataManager(fetch: String, offset: String, completionBlock :@escaping (_ Success: GetGeneralDataModal.getGeneralData_SuccessModal?, _ AuthError: GetGeneralDataModal.getGeneralData_ErrorModal?, Error?)->()) {
        
        let params = [APIClient.Params.serverKey: APIClient.SERVER_KEY.Server_Key, APIClient.Params.fetch: fetch, APIClient.Params.offset: offset]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.GeneralData.getGeneralDataApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200 {
                    let result = GetGeneralDataModal.getGeneralData_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetGeneralDataModal.getGeneralData_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }        
    }
    
   static let sharedInstance = GetGeneralDataManager()
    private init() {}
}

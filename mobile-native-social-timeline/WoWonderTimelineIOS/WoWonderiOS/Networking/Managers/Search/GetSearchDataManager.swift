

import Foundation
import Alamofire
import WoWonderTimelineSDK

class GetSearchDataManager {
    func getSearchData (search_keyword: String,country: String, status: String, verified: String, gender: String, filterbyage: String, age_from: String, age_to: String, completionBlock :@escaping (_ Success: GetSeacrhDataModal.getSearchData_SuccessModal?, _ AuthError: GetSeacrhDataModal.getSearchData_ErrorModal?, Error?)->()){
        let params = [APIClient.Params.serverKey: APIClient.SERVER_KEY.Server_Key, APIClient.Params.country: country, APIClient.Params.status: status, APIClient.Params.verified: verified, APIClient.Params.gender: gender, APIClient.Params.filterbyage: filterbyage, APIClient.Params.age_from: age_from, APIClient.Params.age_to: age_to,APIClient.Params.search_keyword:search_keyword]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.Search.getSearchDataApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = GetSeacrhDataModal.getSearchData_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetSeacrhDataModal.getSearchData_ErrorModal.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = GetSearchDataManager()
    private init() {}
}

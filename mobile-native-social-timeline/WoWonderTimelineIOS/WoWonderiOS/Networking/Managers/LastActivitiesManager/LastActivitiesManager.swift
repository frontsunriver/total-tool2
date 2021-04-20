
import Foundation
import Alamofire
import WoWonderTimelineSDK


class LastActivitiesManager{
    
    static let instance = LastActivitiesManager()
    private init() {}
    
    func getLastActivites(offset: Int, limit: Int,completionBlock :@escaping (_ Success: LastActivitiesModel.LastActivitiesSuccessModel?, _ AuthError: LastActivitiesModel.LastActivitiesErrorModel?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.offset:offset,
            APIClient.Params.limit:limit,
            ] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.Activities.getActivities + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            print(response.result.value)
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
//                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
//                    guard let result = try? JSONDecoder().decode(LastActivitiesModel.LastActivitiesSuccessModel.self, from: data) else {return}
            let result = LastActivitiesModel.LastActivitiesSuccessModel.init(json: res)
            completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(LastActivitiesModel.LastActivitiesErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
}

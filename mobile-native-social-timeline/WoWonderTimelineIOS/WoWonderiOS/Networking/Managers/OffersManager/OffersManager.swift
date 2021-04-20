
import Foundation
import Alamofire
import WoWonderTimelineSDK

class OffersManager{
    
    static let instance = OffersManager()
    
    func getOffers(type:String,limit:Int,offset:Int,completionBlock :@escaping (_ Success: GetOffersModel.GetOffersSuccessModel?, _ AuthError: GetOffersModel.GetOffersErrorModel?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
//            APIClient.Params.limit:limit,
//             APIClient.Params.offset:offset,
             APIClient.Params.type:type
            ] as [String : Any]
        
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.Offers.getOffersApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    guard let datamine = res["data"] as? [[String:Any]] else{return }
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    let result = try? JSONDecoder().decode(GetOffersModel.GetOffersSuccessModel.self, from: data)
                    completionBlock(result,nil,nil)
                }
                    
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetOffersModel.GetOffersErrorModel.self, from: data) else {return}
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

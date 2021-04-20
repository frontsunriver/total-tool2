
import Foundation
import Alamofire
import WoWonderTimelineSDK

class AddMoneyManager{

static let instance = AddMoneyManager()

    func addMoney(amount:Int,userID:String,type:String,completionBlock :@escaping (_ Success: AddMoneyModel.AddMoneySuccessModel?, _ AuthError: AddMoneyModel.AddMoneyErrorModel?, Error?)->()){
    let params = [
        APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
        APIClient.Params.type:type,
        APIClient.Params.userId:userID,
               
        APIClient.Params.amount:amount,
        
        ] as [String : Any]
    let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.AddMoney.addMoney + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
        if response.result.value != nil{
            guard let res = response.result.value as? [String:Any] else {return}
            guard let apiStatusCode = res["api_status"] as? Any else {return}
            if apiStatusCode as? Int == 200{
                guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                guard let result = try? JSONDecoder().decode(AddMoneyModel.AddMoneySuccessModel.self, from: data) else {return}
                completionBlock(result,nil,nil)
            }
                
            else {
                guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                guard let result = try? JSONDecoder().decode(AddMoneyModel.AddMoneyErrorModel.self, from: data) else {return}
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

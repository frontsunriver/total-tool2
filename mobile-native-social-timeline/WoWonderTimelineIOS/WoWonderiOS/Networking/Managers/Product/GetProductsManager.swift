


import Foundation
import Alamofire
import WoWonderTimelineSDK

class GetProductsManager{
    
    func getProducts(userId :String, categoryId :String, distance : Int, offset :String, keyword :String, completionBlock : @escaping (_ Success: GetProductsModel.getProducts_SuccessModel?, _ AuthError : GetProductsModel.getProducts_ErrorModel?, Error?)->()){
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.distance:distance, APIClient.Params.categoryId:categoryId, APIClient.Params.limit:15, APIClient.Params.userId:userId, APIClient.Params.off_set:offset,APIClient.Params.keyword:keyword] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.request(APIClient.Products.getProductApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if (apiStatusCode as? Int == 200){
                let result = GetProductsModel.getProducts_SuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(GetProductsModel.getProducts_ErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
                
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = GetProductsManager()
    private init() {}
}

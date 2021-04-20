

import Foundation
import Alamofire
import WoWonderTimelineSDK


class CreateProductManager {
    
    func createProduct(producName :String, price :Int, currency :String, location :String, categoryId :String, description :String, type :String, data :[Data]?,completionBlock : @escaping (_ Success :CreateProductModel.createProduct_SuccessModel?, _ AuthError : CreateProductModel.createProduct_ErrorModel?, Error?)->()){
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.productTitle:producName, APIClient.Params.productPrice:price, APIClient.Params.currency:currency, APIClient.Params.productCategory:categoryId, APIClient.Params.productLocation:location, APIClient.Params.productType:type, APIClient.Params.productDescription:description] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.upload(multipartFormData: { (multipartFormData) in
            for (key, value) in params {
                multipartFormData.append("\(value)".data(using: String.Encoding.utf8)!, withName: key as String)
            }
            for (index,data) in (data?.enumerated())!{
                multipartFormData.append(data, withName: "images[]", fileName: "file.jpg", mimeType: "image/png")
            }
        },usingThreshold: UInt64.init(), to: APIClient.Products.createProductApi + access_token,method: .post,headers: nil) { (result) in
            switch result{
            case .success(let upload, _, _):
                upload.responseJSON { (response) in
                    print(response.value)
                    if response.result.value != nil {
                        guard let res = response.result.value as? [String:Any] else {return}
                        guard let apiStatusCode = res["api_status"] as? Any else {return}
                        if apiStatusCode as? Int == 200{
                            guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                            guard let result = try? JSONDecoder().decode(CreateProductModel.createProduct_SuccessModel.self, from: data) else {return}
                            completionBlock(result,nil,nil)
                        }
                        else {
                            guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                            guard let result = try? JSONDecoder().decode(CreateProductModel.createProduct_ErrorModel.self, from: data) else {return}
                            completionBlock(nil,result,nil)
                        }
                    }
                    else {
                        print(response.error?.localizedDescription)
                        completionBlock(nil,nil,response.error)
                    }
                }
            case .failure(let error):
                print("Error in upload: \(error.localizedDescription)")
                completionBlock(nil,nil,error)
            }
        }
    }
    
    static var sharedInstance = CreateProductManager()
    private init() {}
    
}

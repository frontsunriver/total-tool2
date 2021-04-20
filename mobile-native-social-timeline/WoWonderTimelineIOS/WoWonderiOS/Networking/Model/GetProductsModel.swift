

import Foundation

class GetProductsModel{
    
    struct getProducts_SuccessModel{
        var api_status : Int
        var products : [[String:Any]]
    }
    
    
    struct getProducts_ErrorModel : Codable {
        let apiStatus: String
        let errors: Errors
        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case errors
        }
        
    }
    
    // MARK: - Errors
    struct Errors: Codable {
        let errorID: Int
        let errorText: String
        
        enum CodingKeys: String, CodingKey {
            case errorID = "error_id"
            case errorText = "error_text"
        }
    }
}

extension GetProductsModel.getProducts_SuccessModel{
    init(json :[String:Any]) {
        let apiStatus = json["api_status"] as? Int
        let products = json["products"] as? [[String:Any]]
        self.products = products ?? [["id" : "12345"]]
        self.api_status = apiStatus ?? 0
    }
}

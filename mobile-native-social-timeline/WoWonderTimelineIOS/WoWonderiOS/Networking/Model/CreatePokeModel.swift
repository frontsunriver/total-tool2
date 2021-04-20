
import Foundation

class CreatePokeModel {
    
   struct createPoke_SuccessModel{
       var api_status : Int
       var message_data : String
       var data : [[String:Any]]
       }
    
    struct createPoke_ErrorModel : Codable {
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

extension CreatePokeModel.createPoke_SuccessModel{
    init(json :[String:Any]) {
       let apiStatus = json["api_status"] as? Int
       let data = json["data"] as? [[String:Any]]
        let message = json["message_data"] as? String
       self.api_status = apiStatus ?? 0
       self.data = data ?? [["id" : "12345"]]
        self.message_data = message ?? "message Successfull"
    
    }
    
}

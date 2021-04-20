
import Foundation

class IntrestEventModal {
    
    struct intrestEvent_SuccessModal :Codable{
       var api_status: Int
       var interest_status: String
    }
    
    struct intrestEvent_ErrorModal :Codable {
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

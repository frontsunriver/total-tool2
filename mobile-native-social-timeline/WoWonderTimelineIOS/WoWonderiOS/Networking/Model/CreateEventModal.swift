
import Foundation


class CreateEventModal {
    
    struct createEvent_SuccessModal:Codable{
        var api_status :Int
        var event_id :Int
    }
    
    struct createEvent_ErrorModal :Codable {
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

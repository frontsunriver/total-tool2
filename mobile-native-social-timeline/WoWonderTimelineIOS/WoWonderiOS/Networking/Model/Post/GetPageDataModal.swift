
import Foundation

class GetPageDataModal{
    
    struct GetPageData_SuccessModal{
        let api_status: Int
        let page_data: [String:Any]
    }
    
   struct GetPageData_ErrorModal:Codable{
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

extension GetPageDataModal.GetPageData_SuccessModal{
    init(json : [String:Any]) {
        let api_status = json["api_status"] as? Int
        let data = json["page_data"] as? [String:Any]
        self.api_status = api_status ?? 0
        self.page_data = data ?? ["PostType" : "Profile_Pic"]
    }
}

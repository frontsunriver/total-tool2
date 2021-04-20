

import Foundation

class Get_News_FeedModel {

    // MARK: - Welcome
    struct get_News_Feed_SuccessModel  {
        let api_status: Int
        let data: [[String:Any]]
    }
    
    struct Datum {
        let api_Status : Int
    public static var postdata  : [String:Any] = ["PostType" : "Profile_Picture"]
       
    }
    
    struct get_News_Feed_ErrorModel: Codable {
        let apiStatus: String
        let errors: Errors

        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case errors
        }
    }

    // MARK: - Errors
    struct Errors: Codable {
        let errorID, errorText: String

        enum CodingKeys: String, CodingKey {
            case errorID = "error_id"
            case errorText = "error_text"
        }
    }
      

}


extension Get_News_FeedModel.get_News_Feed_SuccessModel{
    init(json : [String:Any]) {
        let api_status = json["api_status"] as? Int
        let data = json["data"] as? [[String:Any]]
        self.api_status = api_status ?? 0
        self.data = data ?? [["PostType" : "Profile_Pic"]]
    
    }
   
}

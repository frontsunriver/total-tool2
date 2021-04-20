
import Foundation

class Get_User_DataModel{

struct get_Uers_DataSuccessModel  {
    
    let api_status: Int
    let  user_data : [String:Any]
    let followers : [[String:Any]]
    let following : [[String:Any]]
    let liked_pages : [[String:Any]]
    let joined_groups : [[String:Any]]
    }
 
 
    struct get_Uers_DataErrorModel: Codable {
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


extension Get_User_DataModel.get_Uers_DataSuccessModel{
    init(json : [String:Any]) {
        let api_status = json["api_status"] as? Int
        let user_data = json["user_data"] as? [String:Any]
        let followers = json["followers"] as? [[String:Any]]
        let following = json["following"] as? [[String:Any]]
        let liked_pages = json["liked_pages"] as? [[String:Any]]
        let joined_groups = json["joined_groups"] as? [[String:Any]]
        self.api_status = api_status ?? 0
        self.user_data = user_data ?? ["PostType" : "Profile_Pic"]
        self.followers = followers ?? [["username": "jmichaelb"]]
        self.following = following ?? [["username": "jmichaelb"]]
        self.liked_pages = liked_pages ?? [[ "page_name": "wowonder"]]
        self.joined_groups = joined_groups ?? [["group_name": "wowonder"]]
    }
    
}


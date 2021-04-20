

import Foundation

class GetGeneralDataModal{
    
    struct getGeneralData_SuccessModal{
        var api_status: Int
        var notifications: [[String:Any]]
        var new_notifications_count: String
        var friend_requests: [[String:Any]]
        var new_friend_requests_count: String
        var group_chat_requests: [[String:Any]]
        var new_group_chat_requests_count: Int
        var pro_users: [[String:Any]]
        var promoted_pages: [[String:Any]]
        var trending_hashtag: [[String:Any]]
        var count_new_messages: String
        var announcement: [String:Any]
    }
    
    struct getGeneralData_ErrorModal:Codable{
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
extension GetGeneralDataModal.getGeneralData_SuccessModal{
    init(json: [String:Any]) {
        let api_status = json["api_status"] as? Int
        let notifications = json["notifications"] as? [[String:Any]]
        let new_notifications_count = json["new_notifications_count"] as? String
        let friend_requests = json["friend_requests"] as? [[String:Any]]
        let new_friend_requests_count = json["new_friend_requests_count"] as? String
        let group_chat_requests = json ["group_chat_requests"] as? [[String:Any]]
        let new_group_chat_requests_count = json["new_group_chat_requests_count"] as? Int
        let pro_users = json["pro_users"] as? [[String:Any]]
        let promoted_pages = json["promoted_pages"] as? [[String:Any]]
        let trending_hashtag = json["trending_hashtag"] as? [[String:Any]]
        let count_new_messages = json["count_new_messages"] as? String
        let announcement = json["announcement"] as? [String:Any]
        self.api_status = api_status ?? 0
        self.notifications = notifications ?? [["id" : "12345"]]
        self.new_notifications_count = new_notifications_count ?? "0"
        self.friend_requests = friend_requests ?? [["id" : "12345"]]
        self.new_friend_requests_count = new_friend_requests_count ?? "0"
        self.group_chat_requests = group_chat_requests ?? [["id" : "12345"]]
        self.new_group_chat_requests_count = new_group_chat_requests_count ?? 0
        self.pro_users = pro_users ?? [["id" : "12345"]]
        self.promoted_pages = promoted_pages ?? [["id" : "12345"]]
        self.trending_hashtag = trending_hashtag ?? [["id" : "12345"]]
        self.count_new_messages = count_new_messages ?? "0"
        self.announcement = announcement ?? ["id" : "12345"]
   }
}

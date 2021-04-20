

import Foundation

class GetEventModel {
    
    struct getEvent_SuccessModel {
        var api_status :Int
        var my_events :[[String:Any]]
        var events :[[String:Any]]
        var going :[[String:Any]]
        var interested :[[String:Any]]
        var invited :[[String:Any]]
        var past :[[String:Any]]
    }
    
    struct getEvent_ErrorModel :Codable {
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

extension GetEventModel.getEvent_SuccessModel{
    
    init(json :[String:Any]) {
        let apiStatus = json["api_status"] as? Int
        let events = json["events"] as? [[String:Any]]
        let myEvents = json["my_events"] as? [[String:Any]]
        let intrested = json["intrested"] as? [[String:Any]]
        let going = json["going"] as? [[String:Any]]
        let past =  json["past"] as? [[String:Any]]
        let invited = json["invited"] as? [[String:Any]]
        
        self.api_status = apiStatus ?? 0
        self.events = events ?? [["id" : "1234"]]
        self.my_events = myEvents ?? [["id" : "1234"]]
        self.interested = intrested ?? [["id" : "1234"]]
        self.going = going ?? [["id" : "1234"]]
        self.invited = invited ?? [["id" : "1234"]]
        self.past = past ?? [["id" : "1234"]]
    }
}

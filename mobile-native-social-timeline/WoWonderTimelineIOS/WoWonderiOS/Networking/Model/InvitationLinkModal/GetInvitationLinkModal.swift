//
//  GetInvitationLinkModal.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/6/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation

class GetInvitationLinkModal{
    
    struct getLink_SuccessModal {
        var api_status: Int
        var available_links: Int
        var generated_links: Int
        var used_links: String
        var data: [[String:Any]]
    }
    
      struct getLink_ErrorModal:Codable{
        let apiStatus: String?
        let errors: Errors?
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
extension GetInvitationLinkModal.getLink_SuccessModal{
    init(json:[String:Any]) {
        let api_status = json["api_status"] as? Int
        let available_links = json["available_links"] as? Int
        let generated_links = json["generated_links"] as? Int
        let used_links = json["used_links"] as? String
        let data = json["data"] as? [[String:Any]]
        self.api_status = api_status ?? 0
        self.available_links = available_links ?? 0
        self.generated_links = generated_links ?? 0
        self.used_links = used_links ?? ""
        self.data = data ?? [["" : ""]]
     }
}

//
//  VoteUpModal.swift
//  News_Feed
//
//  Created by Ubaid Javaid on 4/25/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation

class VoteUPModal{
    
    struct voteUp_SuccessModal{
        var api_status: Int
        var votes: [[String:Any]]
    }
    
    struct voteUp_ErrorModal: Codable {
        var apiStatus: String?
        var errors: Errors?
        
        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case errors
        }
    }

    // MARK: - Errors
    struct Errors: Codable {
        var errorID, errorText: String?
        
        enum CodingKeys: String, CodingKey {
            case errorID = "error_id"
            case errorText = "error_text"
        }
    }
}

extension VoteUPModal.voteUp_SuccessModal{
init(json : [String:Any]) {
    let api_status = json["api_status"] as? Int
    let data = json["votes"] as? [[String:Any]]
    self.api_status = api_status ?? 0
    self.votes = data ?? [["PostType" : "Profile_Pic"]]
    }
}


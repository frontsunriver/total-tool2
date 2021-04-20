//
//  GetNearByUsersModel.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/13/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
class GetNearByUsersModel{
    
    struct GetNearByUsersSuccessModel{
        var api_status: Int?
        var data: [[String:Any]]?
    }
    
      struct GetNearByUsersErrorModel:Codable{
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
extension GetNearByUsersModel.GetNearByUsersSuccessModel{
init(json : [String:Any]) {
    let api_status = json["api_status"] as? Int
    let data = json["nearby_users"] as? [[String:Any]]
    self.api_status = api_status ?? 0
    self.data = data ?? [["PostType" : "Profile_Pic"]]
    }
}

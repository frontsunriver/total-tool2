//
//  GamingModel.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/15/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
class GamingModel{
    
    struct GamingSuccessModel{
        var api_status: Int?
        var data: [[String:Any]]?
    }
    
      struct GamingErrorModel:Codable{
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
extension GamingModel.GamingSuccessModel{
init(json : [String:Any]) {
    let api_status = json["api_status"] as? Int
    let data = json["data"] as? [[String:Any]]
    self.api_status = api_status ?? 0
    self.data = data ?? [["PostType" : "Profile_Pic"]]
    }
}

//
//  JobsModel.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/22/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Foundation
class jobsModel{
    
    struct jobsSuccessModel{
        var api_status: Int?
        var data: [[String:Any]]?
    }
    
      struct jobsErrorModel:Codable{
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
extension jobsModel.jobsSuccessModel{
init(json : [String:Any]) {
    let api_status = json["api_status"] as? Int
    let data = json["data"] as? [[String:Any]]
    self.api_status = api_status ?? 0
    self.data = data ?? [["PostType" : "Profile_Pic"]]
    }
}
class ApplyJobModel{
    struct ApplyJobSuccessModel: Codable {
        let apiStatus: Int?
        let messageData: String?

        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case messageData = "message_data"
        }
    }
    struct ApplyJobErrorModel:Codable{
           let apiStatus: String?
           let errors: Errors?
           enum CodingKeys: String, CodingKey {
               case apiStatus = "api_status"
               case errors
           }
       }
       
       struct Errors: Codable {
           let errorID: Int
           let errorText: String
           
           enum CodingKeys: String, CodingKey {
               case errorID = "error_id"
               case errorText = "error_text"
           }
       }

}

//
//  EditEventModal.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/15/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation


class EditEventModal{
    
    struct EditEvent_SuccessModal:Codable{
        let apiStatus: Int
        let message: String
      
        enum CodingKeys: String, CodingKey {
            case apiStatus = "api_status"
            case message = "message_data"
        }
    }
    
    struct EditEvent_ErrorModal:Codable {
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

//
//  GetRegisterInfo.swift
//  driver
//
//  Created by Manly Man on 11/23/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import Foundation


class GetRegisterInfo: HTTPRequest {
    var path: String = "driver/get"
    typealias ResponseType = RegistrationInfo
    var params: [String : Any]?
    
    init(jwtToken: String) {
        self.params = [
            "token": jwtToken
        ]
    }
}

struct RegistrationInfo: Codable {
    var driver: Driver
    var services: [Service] = []
}

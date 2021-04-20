//
//  Login.swift
//  driver
//
//  Created by Manly Man on 11/22/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import Foundation


class Login: HTTPRequest {
    var params: [String : Any]?
    var path: String = "driver/login"
    
    typealias ResponseType = LoginResult
    
    init(firebaseToken: String) {
        self.params = [
            "token": firebaseToken
        ]
    }
}

struct LoginResult: Codable {
    var token: String
    var user: Driver
}

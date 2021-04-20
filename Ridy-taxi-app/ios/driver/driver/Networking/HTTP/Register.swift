//
//  Register.swift
//  driver
//
//  Created by Manly Man on 11/22/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import Foundation


class Register: HTTPRequest {
    var params: [String : Any]?
    var path: String = "driver/register"
    typealias ResponseType = EmptyClass
    
    init(jwtToken: String, driver: Driver) {
        self.params = [
            "token": jwtToken,
            "driver": try! driver.asDictionary()
        ]
    }
}

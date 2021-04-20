//
//  UserDefaultsConfig.swift
//  driver
//
//  Created by Manly Man on 11/22/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import Foundation

public struct UserDefaultsConfig {
    @UserDefault("jwtToken", defaultValue: nil)
    public static var jwtToken: String?
    
    @UserDefault("user", defaultValue: nil)
    public static var user: [String : Any]?
    
    @UserDefault("services", defaultValue: [])
    public static var services: [Any]?
}

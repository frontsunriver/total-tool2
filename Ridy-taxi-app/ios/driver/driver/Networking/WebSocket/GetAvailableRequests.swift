//
//  GetAvailableRequests.swift
//  driver
//
//  Created by Manly Man on 11/23/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import Foundation


class GetAvailableRequests: SocketRequest {
    typealias ResponseType = [Request]
    
    required public init() {}
}

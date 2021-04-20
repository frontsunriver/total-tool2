//
//  AcceptOrder.swift
//  driver
//
//  Created by Manly Man on 11/23/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import Foundation


class AcceptOrder: SocketRequest {
    typealias ResponseType = Request
    var params: [Any]?
    
    init(requestId: Int) {
        self.params = [requestId]
    }
}

//
//  UpdateStatus.swift
//  driver
//
//  Created by Manly Man on 11/23/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import Foundation


class UpdateStatus: SocketRequest {
    typealias ResponseType = EmptyClass
    var params: [Any]?
    
    init(turnOnline: Bool) {
        self.params = [turnOnline]
    }
}

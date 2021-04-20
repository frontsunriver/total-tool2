//
//  UpsertAddress.swift
//  rider
//
//  Created by Manly Man on 11/26/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import UIKit


class UpsertAddress: SocketRequest {
    typealias ResponseType = EmptyClass
    var params: [Any]?
    
    init(address: Address) {
        self.params = [try! address.asDictionary()]
    }
}

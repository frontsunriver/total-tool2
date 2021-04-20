//
//  DeleteAddress.swift
//  rider
//
//  Created by Manly Man on 11/26/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import UIKit


class DeleteAddress: SocketRequest {
    typealias ResponseType = EmptyClass
    var params: [Any]?
    
    init(id: Int) {
        self.params = [id]
    }
}

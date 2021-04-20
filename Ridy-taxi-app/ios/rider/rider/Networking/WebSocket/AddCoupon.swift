//
//  AddCoupon.swift
//  rider
//
//  Created by Manly Man on 11/26/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import UIKit


class AddCoupon: SocketRequest {
    typealias ResponseType = EmptyClass
    var params: [Any]?
    
    init(code: String) {
        self.params = [code]
    }

}

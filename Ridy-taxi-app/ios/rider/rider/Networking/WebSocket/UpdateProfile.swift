//
//  EditProfile.swift
//  rider
//
//  Created by Manly Man on 11/26/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import UIKit


class UpdateProfile: SocketRequest {
    typealias ResponseType = EmptyClass
    var params: [Any]?
    
    init(user: Rider) {
        self.params = [try! user.asDictionary()]
    }
}

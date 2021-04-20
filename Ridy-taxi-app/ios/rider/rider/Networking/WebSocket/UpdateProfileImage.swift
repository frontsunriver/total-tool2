//
//  UpdateProfileImage.swift
//  rider
//
//  Created by Manly Man on 11/26/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import UIKit


class UpdateProfileImage: SocketRequest {
    typealias ResponseType = Media
    var params: [Any]?
    
    init(data: Data) {
        self.params = [data]
    }

}

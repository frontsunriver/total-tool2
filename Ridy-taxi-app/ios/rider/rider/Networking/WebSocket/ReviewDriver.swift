//
//  ReviewDriver.swift
//  rider
//
//  Created by Manly Man on 11/26/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import UIKit


class ReviewDriver: SocketRequest {
    typealias ResponseType = EmptyClass
    var params: [Any]?
    
    init(review: Review) {
        self.params = [try! review.asDictionary()]
    }
}

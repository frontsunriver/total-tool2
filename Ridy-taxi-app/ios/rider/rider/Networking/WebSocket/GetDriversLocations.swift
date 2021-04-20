//
//  GetDriversLocations.swift
//  rider
//
//  Created by Manly Man on 11/26/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import UIKit
import MapKit


class GetDriversLocations: SocketRequest {
    typealias ResponseType = [CLLocationCoordinate2D]
    var params: [Any]?
    
    init(location: CLLocationCoordinate2D) {
        self.params = [try! location.asDictionary()]
    }
}

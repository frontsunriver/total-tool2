//
//  CalculateFare.swift
//  rider
//
//  Created by Manly Man on 11/26/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import UIKit
import MapKit

class CalculateFare: SocketRequest {
    typealias ResponseType = CalculateFareResult
    var params: [Any]?
    
    init(locations: [CLLocationCoordinate2D]) {
        self.params = [locations.map() { loc in
            return [
                "x": loc.longitude,
                "y": loc.latitude
            ]
        }]
    }

}

struct CalculateFareResult: Codable {
    var categories: [ServiceCategory]
    var distance: Int
    var duration: Int
    var currency: String
}

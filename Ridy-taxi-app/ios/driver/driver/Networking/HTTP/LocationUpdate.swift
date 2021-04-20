//
//  LocationUpdate.swift
//  driver
//
//  Created by Manly Man on 11/25/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import Foundation

import MapKit

public class LocationUpdate: HTTPRequest {
    public var path: String = "driver/update_location"
    
    public typealias ResponseType = EmptyClass
    public var params: [String : Any]?
    
    init(jwtToken: String, location: CLLocationCoordinate2D, inTravel: Bool = false) {
        self.params = [
            "token": jwtToken,
            "location": try! location.asDictionary(),
            "inTravel": inTravel
        ]
    }
}

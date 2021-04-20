//
//  CLLocationCoordinate2D.swift
//  rider
//
//  Created by Manly Man on 7/21/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import Foundation
import CoreLocation

extension CLLocationCoordinate2D: Codable {
    public enum CodingKeys: String, CodingKey {
        case x
        case y
    }
    
    public func encode(to encoder: Encoder) throws {
        var container = encoder.container(keyedBy: CodingKeys.self)
        try container.encode(latitude, forKey: .y)
        try container.encode(longitude, forKey: .x)
    }
    
    public init(from decoder: Decoder) throws {
        let values = try decoder.container(keyedBy: CodingKeys.self)
        self.init()
        latitude = try values.decode(Double.self, forKey: .y)
        longitude = try values.decode(Double.self, forKey: .x)
    }
}

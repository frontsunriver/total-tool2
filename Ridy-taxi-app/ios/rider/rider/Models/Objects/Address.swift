//
//  Address.swift
//
//  Copyright (c) Minimalistic apps. All rights reserved.
//

import Foundation
import CoreLocation

public final class Address: Codable {
    
    public static var lastDownloaded = [Address]()
    // MARK: Declaration for string constants to be used to decode and also serialize.
    enum CodingKeys: String, CodingKey {
        case title = "title"
        case location = "location"
        case id = "id"
        case riderId = "rider_id"
        case address = "address"
    }
    
    // MARK: Properties
    public var title: String?
    public var location: CLLocationCoordinate2D?
    public var id: Int?
    public var riderId: Int?
    public var address: String?
}

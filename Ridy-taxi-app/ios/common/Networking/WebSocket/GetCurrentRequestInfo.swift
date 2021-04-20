//
//  GetCurrentRequestInfo.swift
//  Shared
//
//  Created by Manly Man on 11/23/19.
//  Copyright © 2019 Innomalist. All rights reserved.
//

import Foundation
import MapKit

public class GetCurrentRequestInfo: SocketRequest {
    public typealias ResponseType = CurrentRequestInfo
    
    required public init() {}
}

public struct CurrentRequestInfo: Codable {
    var request: Request
    var driverLocation: CLLocationCoordinate2D?
    
}

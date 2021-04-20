//
//  GetTravels.swift
//  Shared
//
//  Created by Manly Man on 11/23/19.
//  Copyright © 2019 Innomalist. All rights reserved.
//

import Foundation

public class GetRequestHistory: SocketRequest {
    public typealias ResponseType = [Request]
    required public init() {}
}

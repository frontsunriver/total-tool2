//
//  ServiceCategory.swift
//
//  Copyright (c) Minimalistic apps. All rights reserved.
//

import Foundation

public final class ServiceCategory: Codable {
    enum CodingKeys: String, CodingKey {
        case id = "id"
        case title = "title"
        case services  = "services"
    }
    
    // MARK: Properties
    public var id: Int
    public var title: String = ""
    public var services: [Service] = []
}

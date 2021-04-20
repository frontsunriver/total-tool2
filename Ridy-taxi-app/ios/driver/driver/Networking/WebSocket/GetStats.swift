//
//  GetStats.swift
//  driver
//
//  Created by Manly Man on 1/20/20.
//  Copyright © 2020 minimal. All rights reserved.
//

import UIKit

class GetStats: SocketRequest {
    typealias ResponseType = StatisticsResult
    public var params: [Any]?
    
    init(query: QueryType) {
        self.params = [query.rawValue]
    }
}

struct StatisticsResult: Codable {
    var currency: String
    var dataset: [DataPoint]
}

struct DataPoint: Codable {
    var name: String
    var current: String
    var earning: Double
    var count: String
    var distance: String
    var time: String
}

enum QueryType: String, Codable {
    case Daily = "daily"
    case Weekly = "weekly"
    case Monthly = "monthly"
}

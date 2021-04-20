//
//  Finish.swift
//  driver
//
//  Created by Manly Man on 11/23/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import Foundation


class Finish: SocketRequest {
    typealias ResponseType = FinishResult
    
    var params: [Any]?
    
    required public init(confirmationCode: Int?, distance: Int, log: String) {
        self.params = [[
            "confirmationCode": confirmationCode ?? 0,
            "distance": distance,
            "log": log
        ]]
    }
}

struct FinishResult: Codable {
    public var status: Bool
}

class FinishService: Codable {
    public var log: String?
    public var cost: Double?
    public var distance: Int?
    public var confirmationCode: Int?
    
    public init(cost: Double, log: String? = "", distance: Int, confirmationCode: Int) {
        self.log = log
        self.cost = cost
        self.confirmationCode = confirmationCode
        self.distance = distance
    }
    
    public init(cost: Double, log: String? = "", distance: Int) {
        self.log = log
        self.cost = cost
        self.distance = distance
    }
}

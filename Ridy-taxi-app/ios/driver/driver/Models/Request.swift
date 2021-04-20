//
//  Request.swift
//  Driver
//
//  Copyright © 2018 minimalistic apps. All rights reserved.
//

import Foundation

class Request:Hashable {
    var hashValue: Int {
        return (travel?.id!)!.hashValue
    }
    
    static func ==(lhs: Request, rhs: Request) -> Bool {
        if lhs.travel?.id == rhs.travel?.id {
            return true
        }else{
            return false
        }
    }
    
    public var travel: Travel?
    public var fromDriver: String?
    public static var selected : Request?
    init(travel:Travel, fromDriver: Int) {
        self.travel = travel
        self.fromDriver = distanceIntToString(value: fromDriver)
    }
    
    func distanceIntToString(value:Int) -> String {
        if value < 1000 {
            return String(value) + " m"
        }
        else {
            return String(value / 1000) + " km"
        }
    }
    
}

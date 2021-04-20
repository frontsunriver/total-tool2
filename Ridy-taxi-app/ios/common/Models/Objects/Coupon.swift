//
//  Coupon.swift
//
//  Created by Minimalistic apps on 11/15/18
//  Copyright (c) . All rights reserved.
//

import Foundation

public final class Coupon: Codable {
    
    // MARK: Properties
    
    public var id: Int?
    public var title: String?
    public var description: String?
    public var startTimestamp: Double?
    public var expirationTimestamp: Double?
    public var isEnabled: Bool?
    public var manyTimesUserCanUse: Int?
    public var manyUsersCanUse: Int?
    public var isFirstTravelOnly: Bool?
    public var maximumCost: Int?
    public var code: String?
    public var creditGift: Int?
    public var discountFlat: Int?
    public var minimumCost: Int?
    public var discountPercent: Int?
}

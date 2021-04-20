//
//  Wallet.swift
//  Shared
//
//  Created by Manly Man on 12/12/19.
//  Copyright © 2019 Innomalist. All rights reserved.
//

import UIKit

public class Wallet: Codable, Equatable {
    public static func == (lhs: Wallet, rhs: Wallet) -> Bool {
        return lhs.id == rhs.id
    }
    
    public var id: Int?
    public var amount: Double?
    public var currency: String?
    
    init(id: Int, amount: Double, currency: String) {
        self.id = id
        self.amount = amount
        self.currency = currency
    }
}

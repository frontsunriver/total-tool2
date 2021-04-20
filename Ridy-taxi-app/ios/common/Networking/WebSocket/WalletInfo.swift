//
//  WalletInfo.swift
//  driver
//
//  Created by Manly Man on 12/21/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import Foundation

class WalletInfo: SocketRequest {
    typealias ResponseType = WalletInfoResult
    
    required public init() {}
}

struct WalletInfoResult: Codable {
    var gateways: [PaymentGateway]
    var wallet: [Wallet]
}


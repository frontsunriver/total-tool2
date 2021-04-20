//
//  WalletTopUp.swift
//  Shared
//
//  Created by Manly Man on 11/23/19.
//  Copyright © 2019 Innomalist. All rights reserved.
//

import Foundation

public final class WalletTopUp: SocketRequest {
    public typealias ResponseType = EmptyClass
    public var params: [Any]?
    
    public init(dto: WalletTopUpDTO) {
        self.params = [try! dto.asDictionary()]
    }
    
    required public init() {}
}

public struct WalletTopUpDTO: Codable {
    var gatewayId: Int
    var amount: Double
    var currency: String
    var token: String
    var pin: Int?
    var otp: Int?
    var transactionId: String?
}

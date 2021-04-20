//
//  GetStripeClientSecret.swift
//  driver
//
//  Created by Manly Man on 6/2/20.
//  Copyright © 2020 minimal. All rights reserved.
//

import Foundation

public class GetStripeClientSecret: HTTPRequest {
    public var path: String = "stripe_client_secret"
    
    public typealias ResponseType = StripeClientSecretResult
    public var params: [String : Any]?
    
    public init(gatewayId: Int, amount: Int, currency: String) {
        self.params = [
            "gatewayId": gatewayId,
            "amount": amount,
            "currency": currency
        ]
    }
}

public struct StripeClientSecretResult: Codable {
    var clientSecret: String
}

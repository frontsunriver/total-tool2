//
//  GetPaymentLink.swift
//  rider
//
//  Created by Manly Man on 9/10/20.
//  Copyright © 2020 minimal. All rights reserved.
//

import Foundation

class GetPaymentLink: SocketRequest {
    typealias ResponseType = GetPaymentLinkResult
    var params: [Any]?
    
    init(gatewayId: Int, amount: Double, currency: String) {
        let dto = GetPaymentLinkDTO(gatewayId: gatewayId, amount: amount, currency: currency, serverUrl: Config.Backend)
        let dic = try! dto.asDictionary()
        print(dic)
        self.params = [dic]
    }
}

struct GetPaymentLinkDTO: Codable {
    var gatewayId: Int
    var amount: Double
    var currency: String
    var serverUrl: String
}

struct GetPaymentLinkResult: Codable {
    var url: String
}

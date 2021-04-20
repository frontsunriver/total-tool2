//
//  AvailablePaymentMethods.swift
//  Shared
//
//  Created by Manly Man on 12/11/19.
//  Copyright © 2019 Innomalist. All rights reserved.
//

import UIKit

public class AvailablePaymentMethods: SocketRequest {
    public typealias ResponseType = [PaymentGateway]
    
    required public init() {}
}

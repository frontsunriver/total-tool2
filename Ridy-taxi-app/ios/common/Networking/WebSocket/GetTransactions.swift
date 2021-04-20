//
//  GetTransactions.swift
//  Shared
//
//  Created by Manly Man on 11/23/19.
//  Copyright © 2019 Innomalist. All rights reserved.
//

import Foundation

public class GetTransactions: SocketRequest {
    public typealias ResponseType = [Transaction]
    
    required public init() {}
}

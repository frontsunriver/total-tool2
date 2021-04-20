//
//  HideHistoryItem.swift
//  Shared
//
//  Created by Manly Man on 11/23/19.
//  Copyright © 2019 Innomalist. All rights reserved.
//

import Foundation

public class HideHistoryItem: SocketRequest {
    public typealias ResponseType = EmptyClass
    public var params: [Any]?
    
    init(requestId: Int) {
        self.params = [requestId]
    }
}

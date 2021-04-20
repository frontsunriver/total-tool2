//
//  SendMessage.swift
//  Shared
//
//  Created by Manly Man on 11/23/19.
//  Copyright © 2019 Innomalist. All rights reserved.
//

import Foundation

public class SendMessage: SocketRequest {
    public typealias ResponseType = ChatMessage
    public var params: [Any]?
    
    public init(content: String) {
        self.params = [content]
    }
    
    required public init() {}
}

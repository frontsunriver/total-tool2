//
//  GetMessages.swift
//  Shared
//
//  Created by Manly Man on 11/23/19.
//  Copyright © 2019 Innomalist. All rights reserved.
//

import Foundation

public class GetMessages: SocketRequest {
    public typealias ResponseType = [ChatMessage]
    
    required public init() {}
}

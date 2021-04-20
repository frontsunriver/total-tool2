//
//  WriteComplaint.swift
//  Shared
//
//  Created by Manly Man on 11/23/19.
//  Copyright © 2019 Innomalist. All rights reserved.
//

import Foundation

public class WriteComplaint: SocketRequest {
    public typealias ResponseType = EmptyClass
    public var params: [Any]?
    
    public init(requestId: Int, subject: String, content: String) {
        self.params = [
        requestId,
        subject,
        content
        ]
    }
    
    required public init() {}
}

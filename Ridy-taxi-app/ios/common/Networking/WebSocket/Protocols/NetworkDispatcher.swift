//
//  NetworkDispatcher.swift
//
//  Copyright © 2019 Minimal. All rights reserved.
//

import Foundation

public protocol NetworkDispatcher {
    func dispatch(event: String, params: [Any]?, completionHandler: @escaping (Result<Any, SocketClientError>) -> Void)
}

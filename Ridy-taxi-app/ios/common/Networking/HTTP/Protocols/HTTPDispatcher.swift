//
//  Copyright © 2019 Minimal. All rights reserved.
//
import Foundation

public protocol HTTPDispatcher {
    func dispatch(path: String, method: HTTPMethod, params: [String: Any]?, completionHandler: @escaping (Result<Data, HTTPStatusCode>) -> Void)
}

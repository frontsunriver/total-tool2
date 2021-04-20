//
//  RequestType.swift
//
//  Copyright © 2019 Minimal. All rights reserved.
//

import Foundation

public protocol SocketRequest {
    associatedtype ResponseType: Codable
    var event: String { get }
    var params: [Any]? { get set }
}

public extension SocketRequest {
    var event: String {
        get {
            return String(String(describing: self).split(separator: ".")[1])
        }
    }
    var params: [Any]? {
        get {
            return nil
        }
        set {
        }
    }
    
    func execute(dispatcher: NetworkDispatcher = SocketNetworkDispatcher.instance, completionHandler: @escaping (Result<ResponseType, ServerError>) -> Void) {
        DispatchQueue.main.async {
            dispatcher.dispatch(event: self.event, params: self.params) { result in
                switch result {
                case .success(let data):
                    if let obj = data as? [String: Any], obj["status"] != nil, obj["message"] != nil {
                        if let error = try? ServerError(from: obj) {
                            completionHandler(.failure(error))
                            print("Socket event \(self.event) Failed status: \(error.status)")
                        } else {
                            if let message = obj["message"] as? String {
                                completionHandler(.failure(ServerError(status: .Unknown, message: message)))
                            } else {
                                completionHandler(.failure(ServerError(status: .Unknown, message: nil)))
                            }
                        }
                        return
                    }
                        if let decoded = try? ResponseType(from: data) {
                            completionHandler(.success(decoded))
                        } else {
                            if let e = EmptyClass() as? ResponseType {
                                completionHandler(.success(e))
                            } else {
                                do {
                                    let _ = try ResponseType(from: data)
                                } catch let _e {
                                    print(_e)
                                    completionHandler(.failure(ServerError(status: .FailedEncoding, message: _e.localizedDescription)))
                                }
                                
                            }
                        }
                    
                case .failure(_):
                    DispatchQueue.main.async {
                        completionHandler(.failure(ServerError(status: .Networking, message: nil)))
                    }
                }
            }
        }
    }
}

public class EmptyClass: Codable {

}

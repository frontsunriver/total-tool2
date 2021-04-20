//
//  SocketNetworkDispatcher.swift
//
//  Copyright © 2019 Minimal. All rights reserved.
//

import Foundation
import SocketIO
import MapKit

public struct SocketNetworkDispatcher: NetworkDispatcher {
    public func dispatch(event: String, params: [Any]?, completionHandler: @escaping (Result<Any, SocketClientError>) -> Void) {
        if let _params = params {
            socket!.emitWithAck(event, with: _params).timingOut(after: 20) { response in
                if(response.count > 1) {
                    completionHandler(.failure(SocketClientError.InvalidAckParamCount))
                    return
                }
                if(response.count == 0) {
                    completionHandler(.success(try! EmptyClass().asDictionary()))
                    return
                }
                if let str = response[0] as? String, str == "NO ACK" {
                    completionHandler(.failure(.RequestTimeout))
                    return
                }
                completionHandler(.success((response[0])))
            }
        } else {
            socket!.emitWithAck(event).timingOut(after: 15) { response in
                if(response.count > 1) {
                    completionHandler(.failure(SocketClientError.InvalidAckParamCount))
                    return
                }
                if(response.count == 0) {
                    completionHandler(.success(try! EmptyClass().asDictionary()))
                    return
                }
                if let str = response[0] as? String, str == "NO ACK" {
                    completionHandler(.failure(.RequestTimeout))
                    return
                }
                completionHandler(.success((response[0])))
            }
        }
    }
    
    public static var instance = SocketNetworkDispatcher()
    var socket : SocketIOClient?
    var manager: SocketManager?
    public var userType: SocketNamespace?
    
    private init() {
        
    }
    
    public mutating func connect(namespace: SocketNamespace, token: String, notificationId: String, completionHandler: @escaping (Result<Bool, ConnectionError>) -> Void) {
        userType = namespace
        manager = SocketManager(socketURL: URL(string: Config.Backend)!,config:[.connectParams([
            "token" : token,
            "os" : "ios",
            "ver" : Config.Version,
            "not": notificationId
        ])])
        socket = manager!.socket(forNamespace: "/\(namespace.rawValue)")
        socket!.on(clientEvent: .connect) {data, ack in
            completionHandler(.success(true))
        }
        
        socket!.on(clientEvent: .disconnect) {data, ack in
            
        }
        
        socket!.on(clientEvent: .error) { data, ack in
            if data.count < 1 {
                completionHandler(.failure(.ErrorWithoutData))
            } else if let obj = data[0] as? [String: Any] {
                print(obj["message"]!)
                completionHandler(.failure(.TokenVerificationError))
            } else if let string = data[0] as? String {
                let knownError = ConnectionError(rawValue: string)
                if knownError != nil {
                    completionHandler(.failure(knownError!))
                } else {
                    completionHandler(.failure(.Unknown))
                }
            } else {
                completionHandler(.failure(.NotDecodableError))
            }
            
        }
        // Driver Events
        socket!.on("requestReceived") { data, ack in
            let travel = try! Request(from: data[0] as Any)
            NotificationCenter.default.post(name: .requestReceived, object: travel)
        }
        socket!.on("cancelRequest") { data, ack in
            NotificationCenter.default.post(name: .requestCanceled, object: nil)
        }
        socket!.on("messageReceived") { data, ack in
            NotificationCenter.default.post(name: .messageReceived, object: try! ChatMessage(from: data[0]))
        }
        socket!.on("driverInfoChanged") { data, ack in
            UserDefaultsConfig.user = try! (Driver(from: data[0] as Any).asDictionary())
        }
        socket!.on("cancelTravel") { data, ack in
            NotificationCenter.default.post(name: .cancelTravel, object: nil)
        }
        
        socket!.on("paid") { data, ack in
            NotificationCenter.default.post(name: .paid, object: nil)
        }
        
        //Rider Events
        socket!.on("arrived") { data, ack in
            let request = try! Request(from: data[0])
            NotificationCenter.default.post(name: .arrived, object: request)
        }
        socket!.on("started") { data, ack in
            let request = try! Request(from: data[0])
            NotificationCenter.default.post(name: .serviceStarted, object: request)
        }
        socket!.on("cancelTravel") { data, ack in
            NotificationCenter.default.post(name: .serviceCanceled, object: nil)
        }
        socket!.on("riderInfoChanged") { data, ack in
            UserDefaultsConfig.user = try! (Rider(from: data[0] as Any).asDictionary())
        }
        socket!.on("travelInfoReceived") { data, ack in
            let location = try! CLLocationCoordinate2D(from: data[0])
            NotificationCenter.default.post(name: .travelInfoReceived, object: location)
        }
        socket!.on("Finished") { data, ack in
            NotificationCenter.default.post(name: .serviceFinished, object: data)
        }
        socket!.on("driverAccepted") { data, ack in
            let travel = try! Request(from: data[0] as Any)
            NotificationCenter.default.post(name: .newDriverAccepted, object: travel)
        }
        socket!.connect()
    }
    
    public func disconnect() {
        if let s = socket {
            s.disconnect()
        }
    }
}

public enum SocketNamespace: String {
    case Driver = "drivers"
    case Rider = "riders"
}

public enum ConnectionError: String, Error, Codable {
    case VersionOutdated = "VersionOutdated"
    case NotFound = "NotFound"
    case Blocked = "Blocked"
    case RegistrationIncomplete = "RegistrationIncomplete"
    case TokenVerificationError = "TokenVerificationError"
    case NotDecodableError = "NotDecodableError"
    case Unknown = "Unknown"
    case ErrorWithoutData = "ErrorWithoutData"
}

public enum SocketClientError: String, Error, Codable, LocalizedError {
    case InvalidAckParamCount = "InvalidAckParamCount"
    case RequestTimeout = "RequestTimeout"
    
    var localizedDescription: String {
        get {
            switch self {
            case .InvalidAckParamCount:
                return "Result parameter couunt is more than one. It's unexpected."
                
            case .RequestTimeout:
                return "Request Timeout"
            }
        }
    }
}

public extension Notification.Name {
    static let menuClicked = Notification.Name("menuClicked")
    static let socketError = Notification.Name("socketError")
    static let newDriverAccepted = Notification.Name("newDriverAccepted")
    static let arrived = Notification.Name("arrived")
    static let serviceStarted = Notification.Name("serviceStarted")
    static let serviceCanceled = Notification.Name("serviceCanceled")
    static let riderInfoChanged = Notification.Name("riderInfoChanged")
    static let travelInfoReceived = Notification.Name("travelInfoReceived")
    static let serviceFinished = Notification.Name("serviceFinished")
    static let messageReceived = Notification.Name("messageReceived")
    static let driverInfoChanged = Notification.Name("driverInfoChanged")
    static let cancelTravel = Notification.Name("cancelTravel")
    static let paid = Notification.Name("paid")
    static let requestReceived = Notification.Name("requestReceived")
    static let requestCanceled = Notification.Name("requestCanceled")
    static let statusReceived = Notification.Name("statusReceived")
    static let connectedAfterForeground = Notification.Name("connectedAfterForeground")
    static let connectionError = Notification.Name("connectionError")
}

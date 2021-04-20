//
//  Copyright © 2019 Minimal. All rights reserved.
//
import Foundation

public struct URLSessionNetworkDispatcher: HTTPDispatcher {
    public func dispatch(path: String, method: HTTPMethod, params: [String : Any]?, completionHandler: @escaping (Result<Data, HTTPStatusCode>) -> Void) {
        guard let url = URL(string: "\(Config.Backend)\(path)") else {
            completionHandler(.failure(HTTPStatusCode.InvalidURL))
            return
        }
        
        var urlRequest = URLRequest(url: url)
        urlRequest.httpMethod = method.rawValue
        
        guard params != nil, let body = try? JSONSerialization.data(withJSONObject: params!) else {
            completionHandler(.failure(HTTPStatusCode.FailedToDecode))
            return
        }
        urlRequest.httpBody = body
        urlRequest.addValue("application/json", forHTTPHeaderField: "Content-Type")
        urlRequest.addValue("application/json", forHTTPHeaderField: "Accept")
        URLSession.shared.dataTask(with: urlRequest) { (data, response, error) in
            if error != nil {
                completionHandler(.failure(HTTPStatusCode.Networking))
                return
            }
            if let httpStatus = response as? HTTPURLResponse, httpStatus.statusCode != 200 {
                guard let error = HTTPStatusCode(rawValue: httpStatus.statusCode) else {
                    completionHandler(.failure(HTTPStatusCode.Unknown))
                    return
                }
                completionHandler(.failure(error))
                return
            }
            guard let _data = data else {
                completionHandler(.failure(HTTPStatusCode.NoData))
                return
            }
            completionHandler(.success(_data))
        }.resume()
    }
    
    public static let instance = URLSessionNetworkDispatcher()
    private init() {}
}

public enum HTTPStatusCode: Int, Error {
    case InvalidCredentials = 403
    case HardReject = 411
    case NotFound = 404
    case Unknown = 666
    case InvalidURL = 701
    case NoData = 702
    case FailedToDecode = 703
    case Networking = 704
    
    var localizedDescription: String {
        get {
            switch self {
            case .FailedToDecode:
                return "Failed to decode."
                
            case .Networking:
                return "Networking Error."
                
            case .NoData:
                return "No Data Received."
                
            case .InvalidURL:
                return "Invalid URL."
                
            case .Unknown:
                return "Unknown."
                
            case .NotFound:
                return "Not Found."
                
            case .HardReject:
                return "Access has been disabled by admin."
                
            case .InvalidCredentials:
                return "Invalid Credentials."
            }
        }
    }
}

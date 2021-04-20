//
//  Rider.swift
//
//  Copyright (c) Minimalistic Apps. All rights reserved.
//

import Foundation
import MessageKit

public final class Rider: Codable, SenderType {
    public var senderId: String { get {
        return "r\(String(self.id!))"
        }
    }
    
    public var displayName: String { get {
        return "\(self.firstName ?? "") \(self.lastName ?? "")"
        }
    }
    
    // MARK: Properties
    public var email: String?
    public var mobileNumber: Int64?
    public var address: String?
    public var gender: String?
    public var balance: Wallet?
    public var lastName: String?
    public var id: Int?
    public var firstName: String?
    public var media: Media?
}


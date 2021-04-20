//
//  Driver.swift
//
//  Copyright (c) Minimalistic Apps. All rights reserved.
//

import Foundation
import MessageKit

public final class Driver: Codable, SenderType {
    public var senderId: String { get {
            return "d\(String(self.id!))"
        }
    }
    
    public var displayName: String { get {
         return "\(self.firstName ?? "") \(self.lastName ?? "")"
        }
    }
    
    // MARK: Properties
    public var id: Int?
    public var firstName: String?
    public var lastName: String?
    public var certificateNumber: String?
    public var mobileNumber: Int64?
    public var email: String?
    public var balance: Wallet?
    public var car: Car?
    public var carColor: String?
    public var carProductionYear: Int?
    public var carPlate: String?
    public var carMedia: Media?
    public var status: Status?
    public var rating: Int?
    public var reviewCount: Int?
    public var media: Media?
    public var gender: String?
    public var accountNumber: String?
    public var bankName: String?
    public var bankRoutingNumber: String?
    public var bankSwift: String?
    public var address: String?
    public var infoChanged: Int?
    public var documentsNote: String?
    public var services: [Service]?
    public var documents: [Media]?
    
    public enum Status: String, Codable {
        case Offline = "offline"
        case Online = "online"
        case InService = "in service"
        case Blocked = "blocked"
        case Disabled = "disabled"
        case PendingApproval = "pending approval"
        case WaitingDocuments = "waiting documents"
        case SoftReject = "soft reject"
        case HardReject = "hard rejet"
    }
}


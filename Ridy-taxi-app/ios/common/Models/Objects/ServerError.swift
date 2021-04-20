//
//  ServerError.swift
//  Shared
//
//  Created by Manly Man on 11/22/19.
//  Copyright © 2019 Innomalist. All rights reserved.
//

import Foundation
import SPAlert

public struct ServerError: Error, Codable {
    public var status: ErrorStatus
    public var message: String?
    
    func getMessage() -> String {
        if self.status != .Unknown {
            return self.status.localizedDescription
        } else if let _message = message {
            return _message
        } else {
            return NSLocalizedString("An unknown error happend", comment: "Error Status")
        }
    }
    
    public func showAlert() {
        SPAlert.present(title: "Errpr", message: getMessage(), preset: .error)
    }
}

public enum ErrorStatus: String, Codable {
    case DistanceCalculationFailed = "DistanceCalculationFailed"
    case DriversUnavailable = "DriversUnavailable"
    case ConfirmationCodeRequired = "ConfirmationCodeRequired"
    case ConfirmationCodeInvalid = "ConfirmationCodeInvalid"
    case OrderAlreadyTaken = "OrderAlreadyTaken"
    case CreditInsufficient = "CreditInsufficient"
    case CouponUsed = "CouponUsed"
    case CouponExpired = "CouponExpired"
    case CouponInvalid = "CouponInvalid"
    case Unknown = "Unknown"
    case Networking = "Networking"
    case FailedEncoding = "FailedEncoding"
    case FailedToVerify = "FailedToVerify"
    case RegionUnsupported = "RegionUnsupported"
    case NoServiceInRegion = "NoServiceInRegion"
    case PINCodeRequired = "PINCodeRequired"
    case OTPCodeRequired = "OTPCodeRequired"

    var localizedDescription: String {
        switch self {
        case .DistanceCalculationFailed:
            return NSLocalizedString("Distance Calculation Failed", comment: "Error Status")
        case .DriversUnavailable:
            return NSLocalizedString("No Driver Available", comment: "Error Status")
        case .ConfirmationCodeRequired:
            return NSLocalizedString("Confirmation Code Required", comment: "Error Status")
        case .ConfirmationCodeInvalid:
            return NSLocalizedString("Confirmation Code is not valid", comment: "Error Status")
        case .OrderAlreadyTaken:
            return NSLocalizedString("Order is taken", comment: "Error Status")
        case .Unknown:
            return NSLocalizedString("Unkown error", comment: "Error Status")
        case .Networking:
            return NSLocalizedString("Networking Error", comment: "Error Status")
        case .FailedEncoding:
            return NSLocalizedString("Failed to Encode/Decode", comment: "Error Status")
        case .FailedToVerify:
            return NSLocalizedString("Failed to verify", comment: "Error Status")
        case .RegionUnsupported:
            return NSLocalizedString("Pickup location Region is not supported", comment: "Error Status")
        case .NoServiceInRegion:
            return NSLocalizedString("No service is currently available at this region", comment: "Error Status")
        case .CreditInsufficient:
            return NSLocalizedString("Credit Not sufficient for this action", comment: "Error Status")
        case .CouponUsed:
            return NSLocalizedString("Coupon is already used", comment: "Error Status")
        case .CouponExpired:
            return NSLocalizedString("Coupon is expired", comment: "Error Status")
        case .CouponInvalid:
            return NSLocalizedString("Coupon is invalid", comment: "Error Status")
        case .PINCodeRequired:
            return NSLocalizedString("PIN Code Required", comment: "Error Status")
        case .OTPCodeRequired:
            return NSLocalizedString("OTP Code Required", comment: "Error Status")
        }
    }
}

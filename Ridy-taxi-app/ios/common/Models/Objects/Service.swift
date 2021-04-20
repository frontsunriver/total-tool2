//
//  Service.swift
//
//  Copyright (c) Minimalistic apps. All rights reserved.
//

import Foundation

public final class Service: Codable, Hashable, CustomStringConvertible {
    public static func == (lhs: Service, rhs: Service) -> Bool {
        return lhs.id == rhs.id
    }
    
    public var description: String {
        return title!
    }
    
    // MARK: Properties
    public var id: Int?
    public var title: String?
    public var baseFare: Double = 0
    public var distanceFeeMode: DistanceFee = .PickupToDestination
    public var perHundredMeters: Double = 0
    public var perMinuteWait: Double = 0
    public var perMinuteDrive: Double = 0
    public var feeEstimationMode = FeeEstimationMode.Static
    public var canEnableVerificationCode: Bool = false
    public var paymentMethod: PaymentMethod = .CashCredit
    public var paymentTime: PaymentTime = .PostPay
    public var prePayPercent: Int = 0
    public var rangePlusPercent: Int = 0
    public var rangeMinusPercent: Int = 0
    public var quantityMode: QuantityMode = .Singular
    public var eachQuantityFee: Double = 0
    public var maxQuantity: Int = 0
    public var minimumFee: Double?
    public var cost: Double? = 0
    public var bookingMode: BookingMode
    public var media: Media?
    
    public var category: ServiceCategory?
    
    public func hash(into hasher: inout Hasher) {
        hasher.combine(id)
    }
    
    public enum DistanceFee: String, Codable {
        case None = "None"
        case PickupToDestination = "PickupToDestination"
    }
    
    public enum FeeEstimationMode: String, Codable {
        case Static = "Static"
        case Dynamic = "Dynamic"
        case Ranged = "Ranged"
        case RangedStrict = "RangedStrict"
        case Disabled = "Disabled"
    }

    public enum PaymentMethod: String, Codable {
        case CashCredit = "CashCredit"
        case OnlyCredit = "OnlyCredit"
        case OnlyCash = "OnlyCash"
    }
    
    public enum BookingMode: String, Codable {
        case OnlyNow = "OnlyNow"
        case Time = "Time"
        case DateTime = "DateTime"
        case DateTimeAbosoluteHour = "DateTimeAbosoluteHour"
    }

    public enum PaymentTime: String, Codable {
        case PrePay = "PrePay"
        case PostPay = "PostPay"
    }

    public enum QuantityMode: String, Codable {
        case Singular = "Singular"
        case Multiple = "Multiple"
    }
    
}

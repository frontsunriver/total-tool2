//
//  NSLocalizedString+Extensions.swift
//  rider
//
//  Created by Manly Man on 9/4/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import Foundation

public class MyLocale {
    
    static var currencyFormatter: NumberFormatter {
        get {
            let formatter = NumberFormatter()
            formatter.locale = Locale.current
            formatter.numberStyle = .currency
            return formatter
        }
    }
    
    public static func formattedCurrency(amount: Double, currency: String) -> String {
        let formatter = self.currencyFormatter
        formatter.currencyCode = currency
        return formatter.string(from: NSNumber(value: amount))!
    }
}

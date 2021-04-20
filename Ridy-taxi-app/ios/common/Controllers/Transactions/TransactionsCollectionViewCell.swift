//
//  TripHistoryCollectionViewCell.swift
//  rider
//
//  Copyright © 2018 minimal. All rights reserved.
//

import UIKit


class TransactionsCollectionViewCell: UICollectionViewCell {
    public var transaction: Transaction?
    @IBOutlet weak var title: UILabel!
    @IBOutlet weak var textdescription: UILabel!
    @IBOutlet weak var background: UIView!
    
    override func layoutSubviews() {
        super.layoutSubviews()
        if let tr = transaction {
            title.text = tr.transactionType?.capitalizingFirstLetter()
            let dateFormatter = DateFormatter()
            dateFormatter.dateStyle = .medium
            dateFormatter.timeStyle = .medium
            
            if let startTimestamp = tr.transactionTime {
                textdescription.text = "\(MyLocale.formattedCurrency(amount: tr.amount!, currency: tr.currency!)) at \(dateFormatter.string(from: Date(timeIntervalSince1970: startTimestamp / 1000)))"
            }
        }
    }
}

extension String {
    func capitalizingFirstLetter() -> String {
        return prefix(1).uppercased() + self.lowercased().dropFirst()
    }
    
    mutating func capitalizeFirstLetter() {
        self = self.capitalizingFirstLetter()
    }
}

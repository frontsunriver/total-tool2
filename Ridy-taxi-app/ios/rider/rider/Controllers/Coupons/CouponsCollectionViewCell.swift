//
//  TripHistoryCollectionViewCell.swift
//  rider
//
//  Copyright © 2018 minimal. All rights reserved.
//

import UIKit


class CouponsCollectionViewCell: UICollectionViewCell {
    public var coupon: Coupon?
    @IBOutlet weak var title: UILabel!
    @IBOutlet weak var textdescription: UILabel!
    @IBOutlet weak var background: UIView!
    @IBOutlet weak var textDaysLeft: UILabel!
    
    override func layoutSubviews() {
        super.layoutSubviews()
        if coupon != nil {
            title.text = coupon?.title
            textdescription.text = coupon?.description
            let days = Calendar.current.dateComponents([.day], from: Date(), to: Date(timeIntervalSince1970: coupon!.expirationTimestamp! / 1000)).day
            textDaysLeft.text = "\(days ?? 0)"
        }
    }
}

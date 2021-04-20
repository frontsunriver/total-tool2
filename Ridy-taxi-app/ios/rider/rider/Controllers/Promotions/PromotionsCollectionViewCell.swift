//
//  TripHistoryCollectionViewCell.swift
//  rider
//
//  Copyright © 2018 minimal. All rights reserved.
//

import UIKit


class PromotionsCollectionViewCell: UICollectionViewCell {
    public var promotion: Promotion?
    @IBOutlet weak var title: UILabel!
    @IBOutlet weak var textdescription: UILabel!
    @IBOutlet weak var background: UIView!
    
    override func layoutSubviews() {
        super.layoutSubviews()
        if let pr = promotion {
            title.text = pr.title
            textdescription.text = pr.description
        }
    }
}

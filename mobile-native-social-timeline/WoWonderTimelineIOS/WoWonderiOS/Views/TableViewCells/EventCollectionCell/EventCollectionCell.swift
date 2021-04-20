//
//  EventCollectionCell.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/9/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class EventCollectionCell: UICollectionViewCell {
    
    @IBOutlet weak var eventImage: Roundimage!
    @IBOutlet weak var dateLabel: RoundLabel!
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var descLabel: UILabel!
    @IBOutlet weak var locationLabel: UILabel!
    
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

}

//
//  SettingsTableItem.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/13/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class SettingsTableItem: UITableViewCell {

    @IBOutlet weak var iconImage: UIImageView!
    @IBOutlet weak var titleLabel: UILabel!
    override func awakeFromNib() {
        super.awakeFromNib()
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    
}

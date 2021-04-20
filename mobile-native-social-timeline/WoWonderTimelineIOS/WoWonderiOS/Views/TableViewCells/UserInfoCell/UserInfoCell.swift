//
//  UserInfoCell.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/24/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class UserInfoCell: UITableViewCell {
    
    
    @IBOutlet weak var imageSet
    : UIImageView!
    @IBOutlet weak var infoLabel: UILabel!
    
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
}

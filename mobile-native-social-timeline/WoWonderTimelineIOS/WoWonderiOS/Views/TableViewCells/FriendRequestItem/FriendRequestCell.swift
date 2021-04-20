//
//  FriendRequestCell.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/4/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class FriendRequestCell: UITableViewCell {
    
  

    @IBOutlet weak var image1: Roundimage!
    @IBOutlet weak var image2: Roundimage!
    @IBOutlet weak var image3: Roundimage!
    @IBOutlet weak var followLabel: UILabel!
    @IBOutlet weak var viewallLabel: UILabel!
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.followLabel.text = NSLocalizedString("Follow Request", comment: "Follow Request")
        self.viewallLabel.text = NSLocalizedString("View all Follow Request", comment: "View all Follow Request")
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
}

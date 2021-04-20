//
//  UserCell.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/14/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class UserCell: UITableViewCell {
    
    @IBOutlet weak var userImage: Roundimage!
    @IBOutlet weak var userName: UILabel!
    @IBOutlet weak var lastSeen: UILabel!
    @IBOutlet weak var followBtn: RoundButton!
    
    @IBOutlet weak var onlineView: DesignView!
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
    @IBAction func Follow(_ sender: Any) {
    }
}

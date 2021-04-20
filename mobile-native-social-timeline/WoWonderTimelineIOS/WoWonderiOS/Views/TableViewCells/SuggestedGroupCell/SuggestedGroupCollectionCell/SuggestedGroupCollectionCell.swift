//
//  SuggestedGroupCollectionCell.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/8/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class SuggestedGroupCollectionCell: UICollectionViewCell {
    
    @IBOutlet weak var groupImage: Roundimage!
    @IBOutlet weak var groupName: UILabel!
    @IBOutlet weak var members: UILabel!
    @IBOutlet weak var joinBtn: RoundButton!
    
    var vc: GroupsDiscoverController?
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.joinBtn.setTitle(NSLocalizedString("Join Group", comment: "Join Group"), for: .normal)
    }
    
    
    @IBAction func Join(_ sender: Any) {
    }
    
}

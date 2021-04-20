

import UIKit
import WoWonderTimelineSDK


class UserContactCell: UITableViewCell {
    
    
    @IBOutlet weak var userImage: Roundimage!
    @IBOutlet weak var username: UILabel!
    @IBOutlet weak var postedLabel: UILabel!
    @IBOutlet weak var contatBtn: RoundButton!
    
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.contatBtn.setTitle(NSLocalizedString("Contact", comment: "Contact"), for: .normal)
    }


    // Configure the view for the selected state
    }



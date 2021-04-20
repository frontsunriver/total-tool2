

import WoWonderTimelineSDK
import UIKit

class JoinedGroupCell: UITableViewCell {
    
    
    @IBOutlet weak var groupIcon: Roundimage!
    @IBOutlet weak var groupName: UILabel!
    @IBOutlet weak var joinedBtn: UIButton!
    
    var isJoined = true
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.joinedBtn.setTitle(NSLocalizedString("JOINED", comment: "JOINED"), for: .normal)
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }

}

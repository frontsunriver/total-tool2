

import UIKit
import WoWonderTimelineSDK


class GroupCoverCell: UITableViewCell {
    
    @IBOutlet weak var cover: UIImageView!
    
    @IBOutlet weak var profile: UIImageView!
    
    @IBOutlet weak var titleLabel: UILabel!
    
    @IBOutlet weak var subtitleLabel: UILabel!
    
    @IBOutlet weak var categoryBtn: UIButton!
    
    @IBOutlet weak var joinGroupBtn: UIButton!
    
    @IBOutlet weak var moreBtn: UIButton!
    
    @IBOutlet weak var memberBtn: UIButton!
    
    @IBOutlet weak var publicBtn: UIButton!
    
    @IBOutlet weak var addMembersBtn: UIButton!
    
    @IBOutlet weak var backBtn: UIButton!
    
    @IBOutlet weak var editCoverBtn: UIButton!
    @IBOutlet weak var editIconBtn: UIButton!
    @IBOutlet weak var editView: UIView!
    var isJoined = false
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.editIconBtn.setTitle(NSLocalizedString("Edit", comment: "Edit"), for: .normal)
        self.editCoverBtn.setTitle(NSLocalizedString("Edit", comment: "Edit"), for: .normal)
        
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
}

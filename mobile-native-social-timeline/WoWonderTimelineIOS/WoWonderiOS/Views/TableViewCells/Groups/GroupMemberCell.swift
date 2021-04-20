
import UIKit
import WoWonderTimelineSDK


class GroupMemberCell: UITableViewCell {
    
    
    @IBOutlet weak var memberProfile: Roundimage!
    
    @IBOutlet weak var memberName: UILabel!
    
    @IBOutlet weak var lastseen: UILabel!
    
    @IBOutlet weak var moreBtn: UIButton!
    
    
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }

}

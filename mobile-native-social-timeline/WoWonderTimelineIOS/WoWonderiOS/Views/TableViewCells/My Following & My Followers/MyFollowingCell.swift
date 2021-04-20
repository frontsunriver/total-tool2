

import UIKit
import WoWonderTimelineSDK


class MyFollowing_MyFollowerCell: UITableViewCell {
    
    @IBOutlet weak var profileImage: Roundimage!
    @IBOutlet weak var userName: UILabel!
    @IBOutlet weak var followingBtn: RoundButton!
    var isfollowing = true
    override func awakeFromNib() {
        super.awakeFromNib()
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
}



import UIKit
import WoWonderTimelineSDK


class UserCoverView: UITableViewCell {
    
    
    @IBOutlet weak var profileNAme: UILabel!
    @IBOutlet weak var coverImage: UIImageView!
    @IBOutlet weak var profileImage: Roundimage!
    @IBOutlet weak var addButton: RoundButton!
    @IBOutlet weak var messageButton: RoundButton!
    @IBOutlet weak var moreButton: RoundButton!
    @IBOutlet weak var followingBtn: UIButton!
    @IBOutlet weak var followersBtn: UIButton!
    @IBOutlet weak var pointsBtn: UIButton!
    @IBOutlet weak var pageLikesBtn: UIButton!
    @IBOutlet weak var followersLabel: UILabel!
    @IBOutlet weak var followingLabel: UILabel!
    @IBOutlet weak var likesLabel: UILabel!
    @IBOutlet weak var pointsLabel: UILabel!
    @IBOutlet weak var backButton: UIButton!
    @IBOutlet weak var aboutLbl: UILabel!
    @IBOutlet weak var aboutText: UILabel!
    var request = "unfollow"
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.followersLabel.text = NSLocalizedString("Followers", comment: "Followers")
        self.followingLabel.text = NSLocalizedString("Following", comment: "Following")
        self.likesLabel.text = NSLocalizedString("Likes", comment: "Likes")
        self.pointsLabel.text = NSLocalizedString("Points", comment: "Points")
        self.aboutLbl.text = NSLocalizedString("About", comment: "About")
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
}

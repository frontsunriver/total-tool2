

import UIKit
import WoWonderTimelineSDK

class MyProfileCell: UITableViewCell {
    
    
    @IBOutlet weak var coverImage: UIImageView!
    @IBOutlet weak var profileImage: Roundimage!
    @IBOutlet weak var followerBtn: UIButton!
    @IBOutlet weak var followingBtn: UIButton!
    @IBOutlet weak var likeBtn: UIButton!
    @IBOutlet weak var pointBtn: UIButton!
    @IBOutlet weak var editProfileBtn: RoundButton!
    @IBOutlet weak var changeImageBtn: RoundButton!
    @IBOutlet weak var moreBtn: RoundButton!
    @IBOutlet weak var backBtn: UIButton!
    @IBOutlet weak var nameLabel: UILabel!
    @IBOutlet weak var walletBtn: RoundButton!
    
    @IBOutlet weak var followersLbl: UILabel!
    @IBOutlet weak var followingLbl: UILabel!
    @IBOutlet weak var likesLbl: UILabel!
    @IBOutlet weak var pointsLbl: UILabel!
    @IBOutlet weak var aboutMeLbl: UILabel!
    @IBOutlet weak var statusTextLbl: UILabel!
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.followersLbl.text = NSLocalizedString("Followers", comment: "Followers")
        self.followingLbl.text = NSLocalizedString("Following", comment: "Following")
        self.likesLbl.text = NSLocalizedString("Likes", comment: "Likes")
        self.pointsLbl.text = NSLocalizedString("Points", comment: "Points")
        self.aboutMeLbl.text = NSLocalizedString("About me", comment: "About me")
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
}

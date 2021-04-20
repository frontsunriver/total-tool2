

import UIKit
import ActiveLabel
import WoWonderTimelineSDK


class EventCell: UITableViewCell {
    
    @IBOutlet weak var timeLabel: UILabel!
    @IBOutlet weak var profileName: UILabel!
    @IBOutlet weak var statusLabel: ActiveLabel!
    @IBOutlet weak var heightConstraint: NSLayoutConstraint!
    @IBOutlet weak var eventImage: UIImageView!
    @IBOutlet weak var startDate: UILabel!
    @IBOutlet weak var eventName: UILabel!
    @IBOutlet weak var eventDescription: UILabel!
    @IBOutlet weak var location: UILabel!
    @IBOutlet weak var profileImage: Roundimage!
    
    @IBOutlet weak var moreBtn: UIButton!
    @IBOutlet weak var LikeBtn: UIButton!
    @IBOutlet weak var CommentBtn: UIButton!
    @IBOutlet weak var ShareBtn: UIButton!
    @IBOutlet weak var likesCountBtn: UIButton!
    @IBOutlet weak var commentsCountBtn: UIButton!
    @IBOutlet weak var likeandcommentViewHeight: NSLayoutConstraint!
    @IBOutlet weak var stackViewHeight: NSLayoutConstraint!
    @IBOutlet weak var tapEventbtn: UIButton!
    
     var btnAction : (() -> ())?
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.tapEventbtn.addTarget(self, action: #selector(self.TappedBtn(sender:)), for: .touchUpInside)
        self.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
         self.CommentBtn.setTitle("\(" ")\(NSLocalizedString("Comment", comment: "Comment"))", for: .normal)
         self.ShareBtn.setTitle("\(" ")\(NSLocalizedString("Share", comment: "Share"))", for: .normal)
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
    @IBAction func TappedBtn(sender:UIButton){
        btnAction?()
    }
    
}

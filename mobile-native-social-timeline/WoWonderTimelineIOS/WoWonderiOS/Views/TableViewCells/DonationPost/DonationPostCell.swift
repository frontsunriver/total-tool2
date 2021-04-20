

import UIKit
import WoWonderTimelineSDK


class DonationPostCell: UITableViewCell {
    
    @IBOutlet weak var profileName: UILabel!
    @IBOutlet weak var profileImage: Roundimage!
    @IBOutlet weak var timeLabel: UILabel!
    @IBOutlet weak var donationimage: UIImageView!
    @IBOutlet weak var donationTitle: UILabel!
    @IBOutlet weak var donationDescription: UILabel!
    @IBOutlet weak var amountRaised: UILabel!
    @IBOutlet weak var amount: UILabel!
    @IBOutlet weak var donationUsername: UILabel!
    @IBOutlet weak var progressView: UIProgressView!
    @IBOutlet weak var donationPostTime: UILabel!
    @IBOutlet weak var moreBtn: UIButton!
    @IBOutlet weak var likeAndcommentView: UIView!
    @IBOutlet weak var stackView: UIStackView!
    @IBOutlet weak var LikeBtn: UIButton!
    @IBOutlet weak var commentBtn: UIButton!
    @IBOutlet weak var shareBtn: UIButton!
    @IBOutlet weak var likesCountBtn: UIButton!
    @IBOutlet weak var commentsCountBtn: UIButton!
    @IBOutlet weak var likeandcommentViewHeight: NSLayoutConstraint!
    @IBOutlet weak var stackViewHeight: NSLayoutConstraint!
    @IBOutlet weak var tapDonationBtn: UIButton!
    
    var btnAction : (() -> ())?
    override func awakeFromNib() {
        super.awakeFromNib()
        self.progressView.transform = self.progressView.transform.scaledBy(x: 1, y: 0.5)
        self.tapDonationBtn.addTarget(self, action: #selector(self.BtnTapped(sender:)), for: .touchUpInside)
        self.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
        self.commentBtn.setTitle("\(" ")\(NSLocalizedString("Comment", comment: "Comment"))", for: .normal)
        self.shareBtn.setTitle("\(" ")\(NSLocalizedString("Share", comment: "Share"))", for: .normal)
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
    @IBAction func BtnTapped(sender: UIButton){
       btnAction?()
    }
    
}

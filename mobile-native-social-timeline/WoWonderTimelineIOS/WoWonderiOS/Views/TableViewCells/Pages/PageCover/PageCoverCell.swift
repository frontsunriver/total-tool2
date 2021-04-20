
import UIKit
import Cosmos
import WoWonderTimelineSDK


class PageCoverCell: UITableViewCell {
    
    @IBOutlet weak var coverImage: UIImageView!

    @IBOutlet weak var pageIcon: UIImageView!

    @IBOutlet weak var ratingLabel: UILabel!
    @IBOutlet weak var pageName: UILabel!
    @IBOutlet weak var pageTitle: UILabel!
    @IBOutlet weak var ratingView: CosmosView!
    @IBOutlet weak var likeBtn: UIButton!
    @IBOutlet weak var messageBtn: UIButton!
    @IBOutlet weak var moreBtn: UIButton!
    @IBOutlet weak var callActionBtn: UIButton!
    @IBOutlet weak var ratingBtn: UIButton!
    @IBOutlet weak var likeCountBtn: UIButton!
    @IBOutlet weak var invteBtn: UIButton!
    @IBOutlet weak var descriptionLabel: UILabel!
    @IBOutlet weak var categoryBtn: UIButton!
    @IBOutlet weak var backBtn: UIButton!
    @IBOutlet weak var editView: UIView!
    @IBOutlet weak var EditCover: UIButton!
    @IBOutlet weak var EditIcon: UIButton!
    
    var isLiked = false
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.EditIcon.setTitle("\(" ")\(NSLocalizedString("Edit", comment: "Edit"))", for: .normal)
        self.EditCover.setTitle("\(" ")\(NSLocalizedString("Edit", comment: "Edit"))", for: .normal)
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
}

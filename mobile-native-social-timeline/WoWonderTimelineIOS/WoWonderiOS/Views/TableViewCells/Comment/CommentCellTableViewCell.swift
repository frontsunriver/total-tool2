
import UIKit
import WoWonderTimelineSDK


class CommentCellTableViewCell: UITableViewCell {
    
    
    @IBOutlet weak var profileImage: Roundimage!
    @IBOutlet weak var userName: UILabel!
    @IBOutlet weak var commentText: UILabel!
    @IBOutlet weak var commentTime: UILabel!
    @IBOutlet weak var viewLeadingContraint: NSLayoutConstraint!
    @IBOutlet weak var likeBtn: UIButton!
    @IBOutlet weak var replyBtn: UIButton!
    
    @IBOutlet weak var commentImage: UIImageView!
    @IBOutlet weak var imageHeight: NSLayoutConstraint!
    @IBOutlet weak var designView: DesignView!
    @IBOutlet var imageWidth: NSLayoutConstraint!
    @IBOutlet weak var noCommentsLAbel: UILabel!
    @IBOutlet weak var noCommentView: UIView!
    @IBOutlet weak var reactionImage: UIImageView!
    @IBOutlet weak var reactionCount: UILabel!
    @IBOutlet weak var reactionBtn: UIButton!
    
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.noCommentsLAbel.text = NSLocalizedString("No Comments to be displayed", comment: "No Comments to be displayed")
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
}

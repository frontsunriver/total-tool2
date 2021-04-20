

import UIKit
import SDWebImage
import Kingfisher
import ActiveLabel
import WoWonderTimelineSDK

class NewsFeedCell: UITableViewCell {
        
    @IBOutlet weak var profileName: UILabel!
    @IBOutlet weak var profileImage: Roundimage!
    @IBOutlet weak var timeLabel: UILabel!
    @IBOutlet weak var statusLabel: ActiveLabel!
    @IBOutlet weak var heigthConstraint: NSLayoutConstraint!
    @IBOutlet weak var stausimage: UIImageView!
    @IBOutlet weak var moreBtn: UIButton!
    @IBOutlet weak var videoView: UIView!
    @IBOutlet weak var likeAndcommentView: UIView!
    @IBOutlet weak var stackView: UIStackView!
    @IBOutlet weak var LikeBtn: UIButton!
    @IBOutlet weak var commentBtn: UIButton!
    @IBOutlet weak var shareBtn: UIButton!
    @IBOutlet weak var likesCountBtn: UIButton!
    @IBOutlet weak var commentsCountBtn: UIButton!
    @IBOutlet weak var likeandcommentViewHeight: NSLayoutConstraint!
    @IBOutlet weak var stackViewHeight: NSLayoutConstraint!
    
    var imageAction : (() -> ())?
    var normalAction : (() -> ())?
    var longAction : (() -> ())?
    var commentAction : (() -> ())?
    var likeCountAction: (() -> ())?
    var commentCountAction: (() -> ())?
    var moreAction: (() -> ())?
    var addREact: (() -> ())?
    var shareAction: (() -> ())?
    var share_timeLine: (() -> ())?
    var share_link: (() -> ())?
    var share_page_group: (() -> ())?
    var share_postTo: (() -> ())?
    override func awakeFromNib() {
        super.awakeFromNib()
   let tapGesture = UITapGestureRecognizer(target: self, action: #selector(self.imageTapped(gesture:)))
    self.stausimage.addGestureRecognizer(tapGesture)
    self.stausimage.isUserInteractionEnabled = true
        let normalTapGesture = UITapGestureRecognizer(target: self, action: #selector(self.NormalTapped(gesture:)))
        let longGesture = UILongPressGestureRecognizer(target: self, action: #selector(self.LongTapped(gesture:)))
        normalTapGesture.numberOfTapsRequired = 1
        longGesture.minimumPressDuration = 0.30
        self.LikeBtn.addGestureRecognizer(normalTapGesture)
        self.LikeBtn.addGestureRecognizer(longGesture)
        self.commentBtn.addTarget(self, action: #selector(self.CommentTapped(_:)), for: .touchUpInside)
        self.likesCountBtn.addTarget(self, action: #selector(LikeCountTapped(_:)), for: .touchUpInside)
        self.moreBtn.addTarget(self, action: #selector(MoreTapped(_:)), for: .touchUpInside)
        self.shareBtn.addTarget(self, action: #selector(ShareTapped(_:)), for: .touchUpInside)
        self.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
        self.commentBtn.setTitle("\(" ")\(NSLocalizedString("Comment", comment: "Comment"))", for: .normal)
        self.shareBtn.setTitle("\(" ")\(NSLocalizedString("Share", comment: "Share"))", for: .normal)
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
    }
    
    override func prepareForReuse() {
        super.prepareForReuse()
        self.stausimage.kf.cancelDownloadTask()
        self.stausimage.image = nil
    }
    
    internal var aspectConstraint : NSLayoutConstraint? {
        didSet {
            if oldValue != nil {
                stausimage.removeConstraint(oldValue!)
            }
            if aspectConstraint != nil {
                stausimage.addConstraint(aspectConstraint!)
            }
        }
    }

    func setPostedImage(image : UIImage) {

        let aspect = image.size.width / image.size.height

        aspectConstraint = NSLayoutConstraint(item: stausimage, attribute: NSLayoutConstraint.Attribute.width, relatedBy: NSLayoutConstraint.Relation.equal, toItem: stausimage, attribute: NSLayoutConstraint.Attribute.height, multiplier: aspect, constant: 0.0)

        stausimage.image = image
    }

    
    @IBAction func imageTapped(gesture: UIGestureRecognizer){
       imageAction?()
     }
    @IBAction func NormalTapped(gesture: UIGestureRecognizer){
        normalAction?()
    }
    @IBAction func LongTapped(gesture: UIGestureRecognizer){
        longAction?()
    }
    @IBAction func CommentTapped(_ sender: UIButton){
     commentAction?()
    }
    @IBAction func LikeCountTapped(_ sender: UIButton){
        likeCountAction?()
    }
    @IBAction func CommentCountTapped(_ sender: UIButton){
        commentCountAction?()
    }
    @IBAction func MoreTapped(_ sender: UIButton){
        moreAction?()
    }
    @IBAction func ShareTapped(_ sender: UIButton){
        shareAction?()
    }
    
   
    
    
}

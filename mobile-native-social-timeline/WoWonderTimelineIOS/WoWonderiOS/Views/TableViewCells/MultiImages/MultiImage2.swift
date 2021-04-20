
import UIKit
import ActiveLabel
import WoWonderTimelineSDK


class MultiImage2: UITableViewCell {
    
    @IBOutlet weak var timeLabel: UILabel!
    @IBOutlet weak var profileImage: Roundimage!
    @IBOutlet weak var profileName: UILabel!
    @IBOutlet weak var statusLabel: ActiveLabel!
    @IBOutlet weak var imageView1: UIImageView!
    @IBOutlet weak var imageView2: UIImageView!
    @IBOutlet weak var moreBtn: UIButton!
    @IBOutlet weak var likesCountBtn: UIButton!
    @IBOutlet weak var commentsCountBtn: UIButton!
    @IBOutlet weak var LikeBtn: UIButton!
    @IBOutlet weak var commentBtn: UIButton!
    @IBOutlet weak var shareBtn: UIButton!
    @IBOutlet weak var stackViewHeight: NSLayoutConstraint!
    @IBOutlet weak var likeandcommentViewHeight: NSLayoutConstraint!
    var imageAction : (() -> ())?
    var normalAction : (() -> ())?
    var longAction : (() -> ())?
    var commentAction : (() -> ())?
    var likeCountAction: (() -> ())?
    var commentCountAction: (() -> ())?
    var moreAction: (() -> ())?
    var addREact: (() -> ())?
    override func awakeFromNib() {
        super.awakeFromNib()
        let Imagegesture1 = UITapGestureRecognizer(target: self, action: #selector(self.imageTapped(gesture:)))
        let Imagegesture2 = UITapGestureRecognizer(target: self, action: #selector(self.imageTapped(gesture:)))
        self.imageView1.addGestureRecognizer(Imagegesture1)
        self.imageView1.isUserInteractionEnabled = true
        self.imageView2.addGestureRecognizer(Imagegesture2)
        self.imageView2.isUserInteractionEnabled = true
        let normalTapGesture = UITapGestureRecognizer(target: self, action: #selector(self.NormalTapped(gesture:)))
        let longGesture = UILongPressGestureRecognizer(target: self, action: #selector(self.LongTapped(gesture:)))
        normalTapGesture.numberOfTapsRequired = 1
        longGesture.minimumPressDuration = 0.30
        self.LikeBtn.addGestureRecognizer(normalTapGesture)
        self.LikeBtn.addGestureRecognizer(longGesture)
        self.commentBtn.addTarget(self, action: #selector(self.CommentTapped(_:)), for: .touchUpInside)
        self.likesCountBtn.addTarget(self, action: #selector(LikeCountTapped(_:)), for: .touchUpInside)
        self.moreBtn.addTarget(self, action: #selector(MoreTapped(_:)), for: .touchUpInside)
        
        self.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
        self.commentBtn.setTitle("\(" ")\(NSLocalizedString("Comment", comment: "Comment"))", for: .normal)
        self.shareBtn.setTitle("\(" ")\(NSLocalizedString("Share", comment: "Share"))", for: .normal)
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
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
    
}



import UIKit
import ActiveLabel
import WoWonderTimelineSDK


class PostOptionCell: UITableViewCell {

    @IBOutlet weak var profileName: UILabel!
    @IBOutlet weak var timeLabel: UILabel!
    @IBOutlet weak var profileImage: Roundimage!
    @IBOutlet weak var statusLabel: ActiveLabel!
    @IBOutlet weak var voteLabel: UILabel!
    @IBOutlet weak var view1: DesignView!
    @IBOutlet weak var view2: DesignView!
    @IBOutlet weak var view3: DesignView!
    @IBOutlet weak var view4: DesignView!
    @IBOutlet weak var view5: DesignView!
    @IBOutlet weak var view6: DesignView!
    @IBOutlet weak var view7: DesignView!
    @IBOutlet weak var view8: DesignView!
    @IBOutlet weak var view9: DesignView!
    @IBOutlet weak var view10: DesignView!
    @IBOutlet weak var label1: UILabel!
    @IBOutlet weak var label2: UILabel!
    @IBOutlet weak var label3: UILabel!
    @IBOutlet weak var label4: UILabel!
    @IBOutlet weak var label5: UILabel!
    @IBOutlet weak var label6: UILabel!
    @IBOutlet weak var label7: UILabel!
    @IBOutlet weak var label8: UILabel!
    @IBOutlet weak var label9: UILabel!
    @IBOutlet weak var label10: UILabel!
    
    @IBOutlet weak var checkBtn1: RoundButton!
    @IBOutlet weak var checkBtn2: RoundButton!
    @IBOutlet weak var checkBtn3: RoundButton!
    @IBOutlet weak var checkBtn4: RoundButton!
    @IBOutlet weak var checkBtn5: RoundButton!
    @IBOutlet weak var checkBtn6: RoundButton!
    @IBOutlet weak var checkBtn7: RoundButton!
    @IBOutlet weak var checkBtn8: UIButton!
    @IBOutlet weak var checkBtn9: RoundButton!
    @IBOutlet weak var checkBtn10: RoundButton!
    
    @IBOutlet weak var Percent1: UILabel!
    @IBOutlet weak var Percent2: UILabel!
    @IBOutlet weak var Percent3: UILabel!
    @IBOutlet weak var Percent4: UILabel!
    @IBOutlet weak var Percent5: UILabel!
    @IBOutlet weak var Percent6: UILabel!
    @IBOutlet weak var Percent7: UILabel!
    @IBOutlet weak var Percent8: UILabel!
    @IBOutlet weak var Percent9: UILabel!
    @IBOutlet weak var Percent10: UILabel!
    
    @IBOutlet weak var ProgressView1: UIProgressView!
    @IBOutlet weak var ProgressView2: UIProgressView!
    @IBOutlet weak var ProgressView3: UIProgressView!
    @IBOutlet weak var ProgressView4: UIProgressView!
    @IBOutlet weak var ProgressView5: UIProgressView!
    @IBOutlet weak var ProgressView6: UIProgressView!
    @IBOutlet weak var ProgressView7: UIProgressView!
    @IBOutlet weak var ProgressView8: UIProgressView!
    @IBOutlet weak var ProgressView9: UIProgressView!
    @IBOutlet weak var ProgressView10: UIProgressView!
    
    @IBOutlet weak var VoteBtn: UIButton!
    @IBOutlet weak var VoteBtn1: UIButton!
    @IBOutlet weak var VoteBtn2: UIButton!
    @IBOutlet weak var VoteBtn3: UIButton!
    @IBOutlet weak var VoteBtn4: UIButton!
    @IBOutlet weak var VoteBtn5: UIButton!
    @IBOutlet weak var VoteBtn6: UIButton!
    @IBOutlet weak var VoteBtn7: UIButton!
    @IBOutlet weak var VoteBtn8: UIButton!
    @IBOutlet weak var VoteBtn9: UIButton!

    @IBOutlet weak var moreBtn: UIButton!
    @IBOutlet weak var LikeBtn: UIButton!
    @IBOutlet weak var CommentBtn: UIButton!
    @IBOutlet weak var ShareBtn: UIButton!
    @IBOutlet weak var likesCountBtn: UIButton!
    @IBOutlet weak var commentsCountBtn: UIButton!
    @IBOutlet weak var likeandcommentViewHeight: NSLayoutConstraint!
    @IBOutlet weak var stackViewHeight: NSLayoutConstraint!
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.ProgressView1.transform = self.ProgressView1.transform.scaledBy(x: 1, y: 1.8)
        self.ProgressView2.transform = self.ProgressView2.transform.scaledBy(x: 1, y: 1.8)
        self.ProgressView3.transform = self.ProgressView3.transform.scaledBy(x: 1, y: 1.8)
        self.ProgressView4.transform = self.ProgressView4.transform.scaledBy(x: 1, y: 1.8)
        self.ProgressView5.transform = self.ProgressView5.transform.scaledBy(x: 1, y: 1.8)
        self.ProgressView6.transform = self.ProgressView6.transform.scaledBy(x: 1, y: 1.8)
        self.ProgressView7.transform = self.ProgressView7.transform.scaledBy(x: 1, y: 1.8)
        self.ProgressView8.transform = self.ProgressView8.transform.scaledBy(x: 1, y: 1.8)
        self.ProgressView9.transform = self.ProgressView9.transform.scaledBy(x: 1, y: 1.8)
        self.ProgressView10.transform = self.ProgressView10.transform.scaledBy(x: 1, y: 1.8)
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
}

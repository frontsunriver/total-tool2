//
//  PostOptionCell.swift
//  News_Feed
//
//  Created by clines329 on 11/27/19.
//  Copyright © 2019 clines329. All rights reserved.
//

import UIKit
import ActiveLabel

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
    
    @IBOutlet weak var moreBtn: UIButton!
    @IBOutlet weak var LikeBtn: UIButton!
    @IBOutlet weak var CommentBtn: UIButton!
    @IBOutlet weak var ShareBtn: UIButton!
    @IBOutlet weak var likesCountBtn: UIButton!
    @IBOutlet weak var commentsCountBtn: UIButton!
    
    @IBOutlet weak var percent1: UILabel!
    @IBOutlet weak var percent2: UILabel!
    @IBOutlet weak var percent3: UILabel!
    @IBOutlet weak var percent4: UILabel!
    @IBOutlet weak var percent5: UILabel!
    @IBOutlet weak var percent6: UILabel!
    @IBOutlet weak var percent7: UILabel!
    @IBOutlet weak var percent8: UILabel!
    @IBOutlet weak var percent9: UILabel!
    @IBOutlet weak var percent10: UILabel!
    
    @IBOutlet weak var checkBtn1: UIButton!
    @IBOutlet weak var checkBtn2: UIButton!
    @IBOutlet weak var checkBtn3: UIButton!
    @IBOutlet weak var checkBtn4: UIButton!
    @IBOutlet weak var checkBtn5: UIButton!
    @IBOutlet weak var checkBtn6: UIButton!
    @IBOutlet weak var checkBtn7: UIButton!
    @IBOutlet weak var checkBtn8: UIButton!
    @IBOutlet weak var checkBtn9: UIButton!
    @IBOutlet weak var checkBtn10: UIButton!

    @IBOutlet weak var progressView1: UIProgressView!
    @IBOutlet weak var progressView2: UIProgressView!
    @IBOutlet weak var progressView3: UIProgressView!
    @IBOutlet weak var progressView4: UIProgressView!
    @IBOutlet weak var progressView5: UIProgressView!
    @IBOutlet weak var progressView6: UIProgressView!
    @IBOutlet weak var progressView7: UIProgressView!
    @IBOutlet weak var progressView8: UIProgressView!
    @IBOutlet weak var progressView9: UIProgressView!
    @IBOutlet weak var progressView10: UIProgressView!

    @IBOutlet weak var voteBtn :UIButton!
    @IBOutlet weak var voteBtn1 :UIButton!
    @IBOutlet weak var voteBtn2 :UIButton!
    @IBOutlet weak var voteBtn3 :UIButton!
    @IBOutlet weak var voteBtn4 :UIButton!
    @IBOutlet weak var voteBtn5 :UIButton!
    @IBOutlet weak var voteBtn6 :UIButton!
    @IBOutlet weak var voteBtn7 :UIButton!
    @IBOutlet weak var voteBtn8 :UIButton!
    @IBOutlet weak var voteBtn9 :UIButton!

    
    @IBOutlet weak var likeandcommentViewHeight: NSLayoutConstraint!
    @IBOutlet weak var stackViewHeight: NSLayoutConstraint!
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.progressView1.transform = self.progressView1.transform.scaledBy(x: 1, y: 1.5)
        self.progressView2.transform = self.progressView2.transform.scaledBy(x: 1, y: 1.5)
        self.progressView3.transform = self.progressView3.transform.scaledBy(x: 1, y: 1.5)
        self.progressView4.transform = self.progressView4.transform.scaledBy(x: 1, y: 1.5)
        self.progressView5.transform = self.progressView5.transform.scaledBy(x: 1, y: 1.5)
        self.progressView6.transform = self.progressView6.transform.scaledBy(x: 1, y: 1.5)
        self.progressView7.transform = self.progressView7.transform.scaledBy(x: 1, y: 1.5)
        self.progressView8.transform = self.progressView8.transform.scaledBy(x: 1, y: 1.5)
        self.progressView9.transform = self.progressView9.transform.scaledBy(x: 1, y: 1.5)
    self.progressView10.transform = self.progressView10.transform.scaledBy(x: 1, y: 1.5)
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
}

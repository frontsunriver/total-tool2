
import UIKit
import AVKit
import AVFoundation
import MobilePlayer
import ActiveLabel
import WoWonderTimelineSDK

import VersaPlayer
class VideoCell: UITableViewCell {
    
    
    @IBOutlet weak var controls: VersaPlayerControls!
    @IBOutlet weak var playerView: VersaPlayerView!
    @IBOutlet weak var profileName: UILabel!
    @IBOutlet weak var profileImage: Roundimage!
    @IBOutlet weak var timeLabel: UILabel!
    @IBOutlet weak var statusLabel: ActiveLabel!
    @IBOutlet weak var videoView: VideoView!
    @IBOutlet weak var videoPlayButton: UIButton!
    //    VideoView
    @IBOutlet weak var sliderView: UIView!
    @IBOutlet weak var startTimer: UILabel!
    @IBOutlet weak var totalTimer: UILabel!
    @IBOutlet weak var seekSlider: UISlider!
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
        self.playerView.autoplay = false
        self.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
        self.CommentBtn.setTitle("\(" ")\(NSLocalizedString("Comment", comment: "Comment"))", for: .normal)
        self.ShareBtn.setTitle("\(" ")\(NSLocalizedString("Share", comment: "Share"))", for: .normal)
      
    }
    
    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
    }
    
    
    func load(url: String) {
        //let html = "<video playsinline controls width=\"100%\" height=\"100%\" src=\"\(url)\"> </video>"
        print("postFile = \(url)")
        DispatchQueue.main.async {
            self.playerView.layer.backgroundColor = UIColor.black.cgColor
            self.playerView.use(controls: self.controls)
            if let url = URL.init(string: url) {
                let item = VersaPlayerItem(url: url)
                self.playerView.set(item: item)
            }
        }
    }
    
}
//    @objc func updateSlider(){
////
////        self.seekSlider.value = Float(CMTimeGetSeconds((self.videoView.player?.currentTime())!))
////        if self.seekSlider.value >= self.seekSlider.maximumValue {
////            self.videoView.stop()
////            self.seekSlider.value = 0.0
////            self.videoView.isLoop = false
////            self.videoPlayButton.isHidden = false
////        }
//
//
//    }





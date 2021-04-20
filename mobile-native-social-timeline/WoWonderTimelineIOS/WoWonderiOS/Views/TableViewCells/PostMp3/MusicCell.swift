

import UIKit
import AVFoundation
import ActiveLabel
import NVActivityIndicatorView
import WoWonderTimelineSDK



class MusicCell: UITableViewCell {

    @IBOutlet weak var nameLabel: UILabel!
    @IBOutlet weak var timeLabel: UILabel!
    @IBOutlet weak var statusLabel: ActiveLabel!
    @IBOutlet weak var playBtn: UIButton!
    @IBOutlet weak var indicatorView: NVActivityIndicatorView!
    @IBOutlet weak var slider: UISlider!
    @IBOutlet weak var avatarImage: Roundimage!
    var isPlay = 0
    
    @IBOutlet weak var moreBtn: UIButton!
    @IBOutlet weak var LikeBtn: UIButton!
    @IBOutlet weak var CommentBtn: UIButton!
    @IBOutlet weak var ShareBtn: UIButton!
    @IBOutlet weak var likesCountBtn: UIButton!
    @IBOutlet weak var commentsCountBtn: UIButton!
    @IBOutlet weak var likeandcommentViewHeight: NSLayoutConstraint!
    @IBOutlet weak var stackViewHeight: NSLayoutConstraint!
    
    
//    let audioSession = AVAudioSession.sharedInstance()
    
    
//
//    func BtnPlay(){
//        if self.isPlay == 0 {
//                self.players.play()
//                 self.plarBtn.setImage(UIImage(named: "pause-button"), for: .normal)
//                self.isPlay = 1
//             }
//             else {
//            self.players.pause()
//        self.plarBtn.setImage(UIImage(named: "play-button"), for: .normal)
//         self.isPlay = 0
//             }
//    }
    
//    func loadMp3 (url : String){
//   let url  = URL.init(string:url)
////        "https://demo.wowonder.com/upload/sounds/2019/11/AT2wCvBOZr9Mk7hLdM6p_02_76a1633cc3719e55d4d0cc642c4b8210_soundFile.mp3"
//
//         let playerItem: AVPlayerItem = AVPlayerItem(url: url!)
//         players = AVPlayer(playerItem: playerItem)
//
//         let playerLayer = AVPlayerLayer(player: players!)
//
//         playerLayer.frame = CGRect(x: 0, y: 0, width: 10, height: 50)
//        self.contentView.layer.addSublayer(playerLayer)
//         let audioAsset = AVURLAsset.init(url: url!, options: nil)
//         let duration = audioAsset.duration
//         let durationInSeconds = CMTimeGetSeconds(duration)
//         print(durationInSeconds)
//         slider.maximumValue = Float(durationInSeconds)
//        slider.minimumValue = 0.0
//         let convertToMinute = durationInSeconds / 60
//         print(convertToMinute)
//
//         do {
//             try audioSession.setCategory(AVAudioSession.Category.playAndRecord, mode: .spokenAudio, options: .defaultToSpeaker)
//             try audioSession.setActive(true, options: .notifyOthersOnDeactivation)
//         } catch {
//             print("error.")
//         }
//
//
//
//
//    }
    
    
    
    override func awakeFromNib() {
        super.awakeFromNib()
        
        self.slider.setThumbImage(UIImage(named: "circular-shape-silhouettet") , for: .normal)
        self.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
        self.CommentBtn.setTitle("\(" ")\(NSLocalizedString("Comment", comment: "Comment"))", for: .normal)
        self.ShareBtn.setTitle("\(" ")\(NSLocalizedString("Share", comment: "Share"))", for: .normal)
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
}

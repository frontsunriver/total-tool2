

import UIKit
import NotificationCenter
import WoWonderTimelineSDK

class LikeReactionsController: UIViewController {
    
    @IBOutlet weak var likeReaction: UIImageView!
    @IBOutlet weak var loveReaction: UIImageView!
    @IBOutlet weak var hahaReaction: UIImageView!
    @IBOutlet weak var wowReaction: UIImageView!
    @IBOutlet weak var sadReaction: UIImageView!
    @IBOutlet weak var angryReaction: UIImageView!
    
    var delegate : AddReactionDelegate!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        
        
        self.likeReaction.loadGif(name: "like1")
        self.loveReaction.loadGif(name: "love1")
        self.hahaReaction.loadGif(name: "haha1")
        self.wowReaction.loadGif(name: "wow1")
        self.sadReaction.loadGif(name: "sad1")
        self.angryReaction.loadGif(name: "angry1")
    }
    
    ///Network Connectivity.
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
            
        }
        
    }
    
    override func viewDidDisappear(_ animated: Bool) {
        NotificationCenter.default.removeObserver(self)
    }
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        self.dismiss(animated: true, completion: nil)
    }
    
    @IBAction func LikeReact(_ sender: UIButton) {
        let status = Reach().connectionStatus()
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            switch sender.tag{
            case 0:
                print("Like")
                self.dismiss(animated: true) {
                    self.delegate.addReaction(reation: "1")
                }
            case 1:
                print("Love")
                self.dismiss(animated: true) {
                    self.delegate.addReaction(reation: "2")
                }
            case 2:
                print("haha")
                self.dismiss(animated: true) {
                    self.delegate.addReaction(reation: "3")
                }
            case 3:
                self.dismiss(animated: true) {
                    self.delegate.addReaction(reation: "4")
                }
            case 4:
                print("sad")
                self.dismiss(animated: true) {
                    self.delegate.addReaction(reation: "5")
                }
            case 5:
                print("Angry")
                self.dismiss(animated: true) {
                    self.delegate.addReaction(reation: "6")
                }
            default:
                print("Nothing")
            }
        }
    }
    
}

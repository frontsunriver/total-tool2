

import UIKit
import WoWonderTimelineSDK
class MoreVC: UIViewController {
    
    
    @IBOutlet weak var moreLabel: UILabel!
    @IBOutlet weak var copyLinkBtn: UIButton!
    @IBOutlet weak var shareBtn: UIButton!
    @IBOutlet weak var privacyBtn: UIButton!
    @IBOutlet weak var settingBtn: UIButton!
    @IBOutlet weak var upgradeBtn: UIButton!
    @IBOutlet weak var closeBtn: UIButton!
    
    var delegate : ProfileMoreDelegate?

    override func viewDidLoad() {
        super.viewDidLoad()
        self.moreLabel.text = NSLocalizedString("More", comment: "More")
        self.copyLinkBtn.setTitle(NSLocalizedString("Copy Link", comment: "Copy Link"), for: .normal)
        self.shareBtn.setTitle(NSLocalizedString("Share", comment: "Share"), for: .normal)
        self.privacyBtn.setTitle(NSLocalizedString("View Privacy Shortcut", comment: "View Privacy Shortcut"), for: .normal)
        self.settingBtn.setTitle(NSLocalizedString("Setting Account", comment: "Setting Account"), for: .normal)
        self.upgradeBtn.setTitle(NSLocalizedString("Upgrade Now", comment: "Upgrade Now"), for: .normal)
        self.closeBtn.setTitle(NSLocalizedString("Close", comment: "Close"), for: .normal)

    }
    
    @IBAction func Close(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    
    @IBAction func MoreAction(_ sender: UIButton) {
        switch sender.tag{
        case 0:
            self.dismiss(animated: true) {
                self.delegate?.profileMore(tag: 0)
            }
        case 1:
            self.dismiss(animated: true) {
                self.delegate?.profileMore(tag: 1)
            }
        case 2:
            self.dismiss(animated: true) {
                self.delegate?.profileMore(tag: 2)
            }
        case 3:
            self.dismiss(animated: true) {
                self.delegate?.profileMore(tag: 3)
            }
        case 4:
            self.dismiss(animated: true) {
                self.delegate?.profileMore(tag: 4)
            }
        default:
            print("print")
        }
    }
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        self.dismiss(animated: true, completion: nil)
    }
    

}

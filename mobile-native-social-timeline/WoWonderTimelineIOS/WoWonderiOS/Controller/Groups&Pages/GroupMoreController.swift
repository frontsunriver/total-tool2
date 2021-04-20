

import UIKit
import WoWonderTimelineSDK

class GroupMoreController: UIViewController {
    
    
    @IBOutlet weak var moreLabel: UILabel!
    @IBOutlet weak var copyLinkBtn: UIButton!
    @IBOutlet weak var shareBtn: UIButton!
    @IBOutlet weak var settingBtn: UIButton!
    @IBOutlet weak var closeBtn: UIButton!
    
    
    var groupUrl: String? = nil
    
    var delegate: GroupMoreDelegate?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.copyLinkBtn.setTitle(NSLocalizedString("Copy Link", comment: "Copy Link"), for: .normal)
        self.shareBtn.setTitle(NSLocalizedString("Share", comment: "Share"), for: .normal)
        self.settingBtn.setTitle(NSLocalizedString("Setting", comment: "Setting"), for: .normal)
        self.closeBtn.setTitle(NSLocalizedString("Close", comment: "Close"), for: .normal)
        
        
    }
    
    @IBAction func Close(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    @IBAction func More(_ sender: UIButton) {
        switch sender.tag{
        case 0:
            self.dismiss(animated: true) {
                self.delegate?.gotoSetting(type: "copy")
            }
            
        case 1:
            self.dismiss(animated: true) {
               self.delegate?.gotoSetting(type: "share")
            }
        case 2:
            self.dismiss(animated: true) {
                self.delegate?.gotoSetting(type: "setting")
            }
            
        default:
            print("noting")
        }
    }
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        self.dismiss(animated: true, completion: nil)
    }
}

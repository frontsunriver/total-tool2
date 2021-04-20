

import UIKit
import WoWonderTimelineSDK
class UpdateTwoFactorVC: UIViewController {

    var delegate:TwoFactorAuthDelegate?
    
    
    @IBOutlet weak var selectLabel: UILabel!
    @IBOutlet weak var cancelBtn: UIButton!
    @IBOutlet weak var enableBtn: UIButton!
    @IBOutlet weak var disableBtn: UIButton!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.selectLabel.text = NSLocalizedString("Select", comment: "Select")
        self.enableBtn.setTitle(NSLocalizedString("Enable", comment: "Enable"), for: .normal)
        self.disableBtn.setTitle(NSLocalizedString("Enable", comment: "Enable"), for: .normal)
        self.cancelBtn.setTitle(NSLocalizedString("CANCEL", comment: "CANCEL"), for: .normal)
       
    }
    
    @IBAction func cancelPressed(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    
    @IBAction func disablePressed(_ sender: Any) {
        self.dismiss(animated: true) {
            self.delegate?.getTwoFactorUpdateString(type: "off")
        }
    }
    @IBAction func enablePressed(_ sender: Any) {
        self.dismiss(animated: true) {
            self.delegate?.getTwoFactorUpdateString(type: "on")
        }
    }
}

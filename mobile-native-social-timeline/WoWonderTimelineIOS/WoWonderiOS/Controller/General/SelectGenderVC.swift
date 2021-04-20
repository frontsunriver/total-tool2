

import UIKit
import WoWonderTimelineSDK
class SelectGenderVC: UIViewController {

    var delegate:TwoFactorAuthDelegate?
    
    
    @IBOutlet weak var selectLbl: UILabel!
    @IBOutlet weak var maleBtn: UIButton!
    @IBOutlet weak var femaleBtn: UIButton!
    @IBOutlet weak var cancelBtn: UIButton!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.selectLbl.text = NSLocalizedString("Select Gender", comment: "Select Gender")
        self.maleBtn.setTitle(NSLocalizedString("Male", comment: "Male"), for: .normal)
        self.femaleBtn.setTitle(NSLocalizedString("Female", comment: "Female"), for: .normal)
        self.cancelBtn.setTitle(NSLocalizedString("CANCEL", comment: "CANCEL"), for: .normal)
    }
    
    @IBAction func cancelPressed(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    @IBAction func disablePressed(_ sender: Any) {
        self.dismiss(animated: true) {
            self.delegate?.getTwoFactorUpdateString(type: "female")
        }
    }
    
    @IBAction func enablePressed(_ sender: Any) {
        self.dismiss(animated: true) {
            self.delegate?.getTwoFactorUpdateString(type: "male")
        }
    }
}



import UIKit
import WoWonderTimelineSDK

class SecurityController: UIViewController {
    
    
    
    
    @IBOutlet weak var errorLabel: UILabel!
    @IBOutlet weak var securityLabel: UILabel!
    @IBOutlet weak var okBtn: UIButton!
    
    var error = ""
    override func viewDidLoad() {
        super.viewDidLoad()
        self.errorLabel.text! = error
        self.securityLabel.text = NSLocalizedString("Security", comment: "Security")
        self.okBtn.setTitle("OK", for: .normal)
        
    }
    
    
    
    @IBAction func OK(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
}

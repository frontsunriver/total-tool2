
import UIKit
import WoWonderTimelineSDK

class SharePopUpController: UIViewController {

    var delegate : SharePostDelegate!
    
    override func viewDidLoad() {
        super.viewDidLoad()

    }
    

    @IBAction func Yes(_ sender: Any) {
        self.dismiss(animated: true) {
            self.delegate.sharePost()
        }
        
    }
    
    @IBAction func No(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        self.dismiss(animated: true, completion: nil)
    }
    
}


import UIKit
import WoWonderTimelineSDK

class ShareController: UIViewController {
    
    var delegate :SharePostDelegate!
    

    override func viewDidLoad() {
        super.viewDidLoad()

        // Do any additional setup after loading the view.
    }
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
    self.dismiss(animated: true, completion: nil)
    }
 
    @IBAction func SharePost(_ sender: UIButton) {
        if sender.tag == 0{
            print("Share to Timeline")
            self.dismiss(animated: true) {
                self.delegate.sharePostTo(type: "timeline")
            }
        }
        else if sender.tag == 1{
            print("Share to Group")
            self.dismiss(animated: true) {
                self.delegate.sharePostTo(type: "group")
            }
            
        }
        else if sender.tag == 2{
            print("Activity")
            self.dismiss(animated: true) {
               self.delegate.sharePostLink()
            }
        }
        else {
            self.dismiss(animated: true) {
              self.delegate.sharePostTo(type: "page")
            }
            self.delegate.sharePostTo(type: "page")
            print("Share to Page")

        }
        
    }
    
}



import UIKit
import WoWonderTimelineSDK

class SetActionController: UIViewController {

    
    @IBOutlet weak var actionLabel: UILabel!
    @IBOutlet weak var closeBtn: UIButton!
    
    var delegate : CallActionDelegate!
    var callActionId = ""
    var callACtionNAme = ""
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.actionLabel.text = NSLocalizedString("Call to action", comment: "Call to action")
        self.closeBtn.setTitle(NSLocalizedString("Close", comment: "Close"), for: .normal)

    }
    


    @IBAction func ActionBtn(_ sender: UIButton) {
        switch sender.tag{
        case 0:
            self.delegate.sendCallAction(callActionId: "1", callActionName: "Read more")
            self.dismiss(animated: true, completion: nil)
        case 1:
            self.delegate.sendCallAction(callActionId: "2", callActionName: "Shop now")
            self.dismiss(animated: true, completion: nil)
        case 2:
            self.delegate.sendCallAction(callActionId: "3", callActionName: "View now")
            self.dismiss(animated: true, completion: nil)
        case 3:
            self.delegate.sendCallAction(callActionId: "4", callActionName: "visit now")
            self.dismiss(animated: true, completion: nil)
        case 4:
            self.delegate.sendCallAction(callActionId: "5", callActionName: "Book now")
            self.dismiss(animated: true, completion: nil)
        case 5:
            self.delegate.sendCallAction(callActionId: "6", callActionName: "Learn more")
            self.dismiss(animated: true, completion: nil)
        case 6:
            self.delegate.sendCallAction(callActionId: "7", callActionName: "Play now")
            self.dismiss(animated: true, completion: nil)
        case 7:
            self.delegate.sendCallAction(callActionId: "8", callActionName: "Bet now")
            self.dismiss(animated: true, completion: nil)
        case 8:
            self.delegate.sendCallAction(callActionId: "9", callActionName: "Donate")
            self.dismiss(animated: true, completion: nil)
        case 9:
            self.delegate.sendCallAction(callActionId: "10", callActionName: "Apply here")
            self.dismiss(animated: true, completion: nil)
        case 10:
            self.delegate.sendCallAction(callActionId: "11", callActionName: "Quote here")
            self.dismiss(animated: true, completion: nil)
        case 11:
            self.delegate.sendCallAction(callActionId: "12", callActionName: "Order now")
            self.dismiss(animated: true, completion: nil)
        case 12:
            self.delegate.sendCallAction(callActionId: "13", callActionName: "Book tickets")
            self.dismiss(animated: true, completion: nil)
        case 13:
            self.delegate.sendCallAction(callActionId: "14", callActionName: "Enroll now")
            self.dismiss(animated: true, completion: nil)
        case 14:
            self.delegate.sendCallAction(callActionId: "15", callActionName: "Find a card")
            self.dismiss(animated: true, completion: nil)
        case 15:
            self.delegate.sendCallAction(callActionId: "16", callActionName: "Get a quote")
            self.dismiss(animated: true, completion: nil)
        case 16:
            self.delegate.sendCallAction(callActionId: "17", callActionName: "Get tickets")
            self.dismiss(animated: true, completion: nil)
        case 17:
            self.delegate.sendCallAction(callActionId: "18", callActionName: "Locate a dealer")
            self.dismiss(animated: true, completion: nil)
        case 18:
            self.delegate.sendCallAction(callActionId: "19", callActionName: "Order online")
            self.dismiss(animated: true, completion: nil)
        case 19:
            self.delegate.sendCallAction(callActionId: "20", callActionName: "Preorder now")
            self.dismiss(animated: true, completion: nil)
        case 20:
            self.delegate.sendCallAction(callActionId: "21", callActionName: "Schedule now")
            self.dismiss(animated: true, completion: nil)
        case 21:
            self.delegate.sendCallAction(callActionId: "22", callActionName: "Sgin up now")
            self.dismiss(animated: true, completion: nil)
        case 22:
            self.delegate.sendCallAction(callActionId: "23", callActionName: "Subscribe")
            self.dismiss(animated: true, completion: nil)
            
        case 23:
            self.delegate.sendCallAction(callActionId: "24", callActionName: "Register now")
            self.dismiss(animated: true, completion: nil)
            
            
            
        default:
            print("Nothing")
        }
        
        
    }
    
    @IBAction func Close(_ sender: Any) {
    }
    
}



import UIKit
import WoWonderTimelineSDK
protocol didSelectingFeelingTypeDelegate {
    func didselectFeelingType(type:String,feelingString:String)
}
class FeelingTypeVC: UIViewController {

    @IBOutlet weak var feelingTextField: UITextField!
    @IBOutlet weak var typeLabel: UILabel!
    
    var delegate:didSelectingFeelingTypeDelegate?
    
    var feelingTypeString:String? = ""
    override func viewDidLoad() {
        super.viewDidLoad()
        self.typeLabel.text = self.feelingTypeString ?? ""
        if self.feelingTypeString == "Listening"{
            self.feelingTextField.placeholder = "What are you listening to ?"
        }else if self.feelingTypeString == "Playing"{
            self.feelingTextField.placeholder = "What are you playing?"
        }
        else if self.feelingTypeString == "Watching"{
            self.feelingTextField.placeholder = "What are you watching?"
        }else if self.feelingTypeString == "Traveling"{
            self.feelingTextField.placeholder = "Where are you traveling?"
        }
    }
    

   
    @IBAction func cancelPressed(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    @IBAction func SubmitPressed(_ sender: Any) {
        if self.feelingTextField.text!.isEmpty{
            
        }else{
            self.dismiss(animated: true) {
                self.delegate?.didselectFeelingType(type: self.feelingTypeString ?? "", feelingString: self.feelingTextField.text ?? "")
            }
        }
    }
}

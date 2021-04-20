

import UIKit
import WoWonderTimelineSDK

class EditAndDeleteController: UIViewController {
    
    
    @IBOutlet weak var editBtn: UIButton!
    
    @IBOutlet weak var DeleteBtn: UIButton!
    
    @IBOutlet weak var closeBtn: UIButton!
    
    let Storyboard = UIStoryboard(name: "Jobs", bundle: nil)
    var delegate : EditJobDelegate!
    var deleteDelegate : DeleteJobDelegate!

    override func viewDidLoad() {
        super.viewDidLoad()
        self.closeBtn.setTitle(NSLocalizedString("Close", comment: "Close"), for: .normal)
        self.editBtn.setTitle(NSLocalizedString("Edit", comment: "Edit"), for: .normal)
        self.DeleteBtn.setTitle(NSLocalizedString("Delete", comment: "Delete"), for: .normal)

    }
    
    
    @IBAction func EditandDelete(_ sender: UIButton) {
        switch sender.tag {
        case 0:
        self.delegate.editJob()
        self.dismiss(animated: true, completion: nil)
        case 1:
            self.deleteDelegate.deleteJob(jobId: 0)
            self.dismiss(animated: true, completion: nil)
        default:
            print("Default")
        }
    }
    

  
    @IBAction func Close(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
}

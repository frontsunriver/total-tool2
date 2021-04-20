

import UIKit
import WoWonderTimelineSDK

class JobCategoryController: UIViewController {

    
    
    @IBOutlet weak var cateLabel: UILabel!
    
    @IBOutlet weak var closeBtn: UIButton!
    var delegate : JobCategoryDelegate!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.cateLabel.text = NSLocalizedString("Select A Category", comment: "Select A Category")
        self.closeBtn.setTitle(NSLocalizedString("Close", comment: "Close"), for: .normal)

    }
    


    @IBAction func SelectCategory(_ sender: UIButton) {
        switch sender.tag{
        case 0:
            self.delegate.category(category: "Other", categoryId: "1")
            self.dismiss(animated: true, completion: nil)
        case 1:
            self.delegate.category(category: "Admin & Office", categoryId: "2")
            self.dismiss(animated: true, completion: nil)
        case 2:
            self.delegate.category(category: "Art & Design", categoryId: "3")
            self.dismiss(animated: true, completion: nil)
         case 3:
            self.delegate.category(category: "Business Operations", categoryId: "4")
            self.dismiss(animated: true, completion: nil)
        case 4:
            self.delegate.category(category: "Cleaning & Facilites", categoryId: "5")
            self.dismiss(animated: true, completion: nil)
            
        case 5:
            self.delegate.category(category: "Community & Social Services", categoryId: "6")
            self.dismiss(animated: true, completion: nil)
        case 6:
            self.delegate.category(category: "Computer & Data", categoryId: "7")
            self.dismiss(animated: true, completion: nil)
        case 7:
            self.delegate.category(category: "Constraction & Mining", categoryId: "8")
            self.dismiss(animated: true, completion: nil)
        case 8:
            self.delegate.category(category: "Education", categoryId: "9")
            self.dismiss(animated: true, completion: nil)
        case 9:
            self.delegate.category(category: "Farming & Foresty", categoryId: "10")
            self.dismiss(animated: true, completion: nil)
        case 10:
            self.delegate.category(category: "Health Care", categoryId: "11")
            self.dismiss(animated: true, completion: nil)
        case 11:
            self.delegate.category(category: "Intsallation,Maintenance & Repair", categoryId: "12")
            self.dismiss(animated: true, completion: nil)
        case 12:
            self.delegate.category(category: "Legal", categoryId: "13")
            self.dismiss(animated: true, completion: nil)
        case 13:
            self.delegate.category(category: "Management", categoryId: "14")
            self.dismiss(animated: true, completion: nil)
        case 14:
            self.delegate.category(category: "Manufacturing", categoryId: "15")
            self.dismiss(animated: true, completion: nil)
        case 15:
            self.delegate.category(category: "Media & Communication", categoryId: "16")
            self.dismiss(animated: true, completion: nil)
        case 16:
            self.delegate.category(category: "Personal Care", categoryId: "17")
            self.dismiss(animated: true, completion: nil)
        case 17:
            self.delegate.category(category: "Protective Services", categoryId: "18")
            self.dismiss(animated: true, completion: nil)
        case 18:
            self.delegate.category(category: "Restaurant & Hospitality", categoryId: "19")
            self.dismiss(animated: true, completion: nil)
        case 19:
            self.delegate.category(category: "Retail & Sales", categoryId: "20")
            self.dismiss(animated: true, completion: nil)
        case 20:
            self.delegate.category(category: "Science & Engineering", categoryId: "21")
            self.dismiss(animated: true, completion: nil)
        case 21:
            self.delegate.category(category: "Sports & Entertainment", categoryId: "22")
            self.dismiss(animated: true, completion: nil)
        case 22:
            self.delegate.category(category: "Transportation", categoryId: "23")
            self.dismiss(animated: true, completion: nil)
        default:
            print("Default")
        }
       
        
    }
    
    
    @IBAction func Close(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
}

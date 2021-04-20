

import UIKit
import WoWonderTimelineSDK

class ProductCategoryController: UIViewController {
    
    
    @IBOutlet weak var cateLabel: UILabel!
    @IBOutlet weak var closeBtn: UIButton!
    
    var delegate : ProductCategoryDelegate!

    override func viewDidLoad() {
        super.viewDidLoad()
        self.cateLabel.text = NSLocalizedString("Select A Category", comment: "Select A Category")
        self.closeBtn.setTitle(NSLocalizedString("Close", comment: "Close"), for: .normal)

    }
    

  
    @IBAction func Category(_ sender: UIButton) {
        switch sender.tag {
        case 0:
            self.delegate.category(categoryName: "Others", categoryId: "1")
            self.dismiss(animated: true, completion: nil)
        case 1:
           self.delegate.category(categoryName: "Autos & Vehicles", categoryId: "2")
            self.dismiss(animated: true, completion: nil)
        case 2:
            self.delegate.category(categoryName: "Baby & Childer's Products", categoryId: "3")
            self.dismiss(animated: true, completion: nil)
        case 3:
            self.delegate.category(categoryName: "Beauty Products & Services", categoryId: "4")
            self.dismiss(animated: true, completion: nil)
        case 4:
            self.delegate.category(categoryName: "Computers & Peripherals", categoryId: "5")
            self.dismiss(animated: true, completion: nil)
        case 5:
            self.delegate.category(categoryName: "Consumer Electronics", categoryId: "6")
            self.dismiss(animated: true, completion: nil)
        case 6:
            self.delegate.category(categoryName: "Dating Services", categoryId: "7")
            self.dismiss(animated: true, completion: nil)
        case 7:
            self.delegate.category(categoryName: "Financial Services", categoryId: "8")
            self.dismiss(animated: true, completion: nil)
        case 8:
            self.delegate.category(categoryName: "Gifts & Services", categoryId: "9")
            self.dismiss(animated: true, completion: nil)
        case 9:
            self.delegate.category(categoryName: "Home & Garden", categoryId: "10")
            self.dismiss(animated: true, completion: nil)
        default:
            print("Nothing")
        }
    }
    
    
    @IBAction func Close(_ sender: Any) {
        self.delegate.category(categoryName: "", categoryId: "")
        self.dismiss(animated: true, completion: nil)
    }
    
}


import UIKit
import WoWonderTimelineSDK


class GroupCategoryController: UIViewController {
    
    
  
    @IBOutlet weak var closeBtn: UIButton!
    @IBOutlet weak var selectLabel: UILabel!
    
    @IBOutlet var btn1: UIButton!
    @IBOutlet var btn2: UIButton!
    @IBOutlet var btn3: UIButton!
    @IBOutlet var btn4: UIButton!
    @IBOutlet var btn5: UIButton!
    @IBOutlet var btn6: UIButton!
    @IBOutlet var btn7: UIButton!
    @IBOutlet var btn8: UIButton!
    @IBOutlet var btn9: UIButton!
    @IBOutlet var btn10: UIButton!
    @IBOutlet var btn11: UIButton!
    @IBOutlet var btn12: UIButton!
    @IBOutlet var btn13: UIButton!
    @IBOutlet var btn14: UIButton!
    @IBOutlet var btn15: UIButton!
    @IBOutlet var btn16: UIButton!
    @IBOutlet var btn17: UIButton!
    @IBOutlet var btn18: UIButton!
    
    var delegate : selectCategoryDelegate!

    override func viewDidLoad() {
        super.viewDidLoad()
        self.selectLabel.text = NSLocalizedString("Select a Category", comment: "Select a Category")
        self.closeBtn.setTitle(NSLocalizedString("Close", comment: "Close"), for: .normal)
    }
    
    override func viewWillAppear(_ animated: Bool) {
        self.btn1.setTitle(NSLocalizedString("Cars and Vehicles", comment: "Cars and Vehicles"), for: .normal)
        self.btn2.setTitle(NSLocalizedString("Comedy", comment: "Comedy"), for: .normal)
        self.btn3.setTitle(NSLocalizedString("Economics & Trade", comment: "Economics & Trade"), for: .normal)
        self.btn4.setTitle(NSLocalizedString("Education", comment: "Education"), for: .normal)
        self.btn5.setTitle(NSLocalizedString("Entertainment", comment: "Entertainment"), for: .normal)
        self.btn6.setTitle(NSLocalizedString("Movies & Animation", comment: "Movies & Animation"), for: .normal)
        self.btn7.setTitle(NSLocalizedString("Gaming", comment: "Gaming"), for: .normal)
        self.btn8.setTitle(NSLocalizedString("History and Facts", comment: "History and Facts"), for: .normal)
        self.btn9.setTitle(NSLocalizedString("Live Style", comment: "Live Style"), for: .normal)
        self.btn10.setTitle(NSLocalizedString("Natural", comment: "Natural"), for: .normal)
        self.btn11.setTitle(NSLocalizedString("News and Politics", comment: "News and Politics"), for: .normal)
        self.btn12.setTitle(NSLocalizedString("People and Nations", comment: "People and Nations"), for: .normal)
        self.btn13.setTitle(NSLocalizedString("Pets & Animals", comment: "Pets & Animals"), for: .normal)
        self.btn14.setTitle(NSLocalizedString("Places & Regions", comment: "Places & Regions"), for: .normal)
        self.btn15.setTitle(NSLocalizedString("Science and Technology", comment: "Science and Technology"), for: .normal)
        self.btn16.setTitle(NSLocalizedString("Sports", comment: "Sports"), for: .normal)
        self.btn17.setTitle(NSLocalizedString("Traval and Events", comment: "Traval and Events"), for: .normal)
        self.btn18.setTitle(NSLocalizedString("Other", comment: "Other"), for: .normal)

    }
    
    @IBAction func Close(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }

    @IBAction func Category(_ sender: UIButton) {
       switch sender.tag {
        case 0:
        print("Category ID 1")
        self.delegate.selectCategory(categoryID: 1, categoryName: NSLocalizedString("Cars and Vehicles", comment: "Cars and Vehicles"))
        self.dismiss(animated: true, completion: nil)
        case 1:
        print("Category ID 2")
        self.delegate.selectCategory(categoryID: 2, categoryName: NSLocalizedString("Comedy", comment: "Comedy"))
        self.dismiss(animated: true, completion: nil)
        case 2:
        print("Category ID 3")
        self.delegate.selectCategory(categoryID: 3, categoryName: NSLocalizedString("Economics & Trade", comment: "Economics & Trade"))
        self.dismiss(animated: true, completion: nil)
        case 3:
        print("Category ID 4")
        self.delegate.selectCategory(categoryID: 4, categoryName: NSLocalizedString("Education", comment: "Education"))
        self.dismiss(animated: true, completion: nil)
        case 4:
        print("Category ID 5")
        self.delegate.selectCategory(categoryID: 5, categoryName: NSLocalizedString("Entertainment", comment: "Entertainment"))
        self.dismiss(animated: true, completion: nil)
        case 5:
        print("Category ID 6")
        self.delegate.selectCategory(categoryID: 6, categoryName: NSLocalizedString("Movies & Animation", comment: "Movies & Animation"))
        self.dismiss(animated: true, completion: nil)
        case 6:
        print("Category ID 7")
        self.delegate.selectCategory(categoryID: 7, categoryName: NSLocalizedString("Gaming", comment: "Gaming"))
        self.dismiss(animated: true, completion: nil)
        case 7:
        print("Category ID 8")
        self.delegate.selectCategory(categoryID: 8, categoryName: NSLocalizedString("History and Facts", comment: "History and Facts"))
        self.dismiss(animated: true, completion: nil)
        case 8:
        print("Category ID 9")
            self.delegate.selectCategory(categoryID: 9, categoryName: NSLocalizedString("Live Style", comment: "Live Style"))
        self.dismiss(animated: true, completion: nil)
        case 9:
        print("Category ID 10")
        self.delegate.selectCategory(categoryID: 10, categoryName: NSLocalizedString("Natural", comment: "Natural"))
        self.dismiss(animated: true, completion: nil)
        case 10:
        print("Category ID 11")
            self.delegate.selectCategory(categoryID: 11, categoryName: NSLocalizedString("News and Politics", comment: "News and Politics"))
        self.dismiss(animated: true, completion: nil)
        case 11:
        print("Category ID 12")
        self.delegate.selectCategory(categoryID: 12, categoryName: NSLocalizedString("People and Nations", comment: "People and Nations"))
        self.dismiss(animated: true, completion: nil)
        case 12:
        print("Category ID 13")
        self.delegate.selectCategory(categoryID: 13, categoryName: NSLocalizedString("Pets & Animals", comment: "Pets & Animals"))
        self.dismiss(animated: true, completion: nil)
       case 13:
        print("Category ID 14")
        self.delegate.selectCategory(categoryID: 14, categoryName: NSLocalizedString("Places & Regions", comment: "Places & Regions"))
        self.dismiss(animated: true, completion: nil)
        case 14:
        print("Category ID 15")
        self.delegate.selectCategory(categoryID: 15, categoryName: NSLocalizedString("Science and Technology", comment: "Science and Technology"))
        self.dismiss(animated: true, completion: nil)
       case 15:
       print("Category ID 16")
        self.delegate.selectCategory(categoryID: 16, categoryName: NSLocalizedString("Sports", comment: "Sports"))
        self.dismiss(animated: true, completion: nil)
        case 16:
        print("Category ID 17")
        self.delegate.selectCategory(categoryID: 17, categoryName: NSLocalizedString("Traval and Events", comment: "Traval and Events"))
        self.dismiss(animated: true, completion: nil)
        case 17:
        print("Category ID 0")
        self.delegate.selectCategory(categoryID: 0, categoryName: NSLocalizedString("Other", comment: "Other"))
        self.dismiss(animated: true, completion: nil)
        default:
        print("Default")
        }
        
    }
    
  

    
}

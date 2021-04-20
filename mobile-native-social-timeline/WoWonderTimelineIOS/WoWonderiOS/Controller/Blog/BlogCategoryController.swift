

import UIKit
import WoWonderTimelineSDK

class BlogCategoryController: UIViewController {
    
    
    @IBOutlet weak var cateLabel: UILabel!
    @IBOutlet weak var closeBtn: UIButton!
    
    var delegate : BlogCategoryDelegate!

    override func viewDidLoad() {
        super.viewDidLoad()
        self.cateLabel.text = NSLocalizedString("Select A Category", comment: "Select A Category")
        self.closeBtn.setTitle(NSLocalizedString("Close", comment: "Close"), for: .normal)
    }
    
    @IBAction func CategoryBtn(_ sender: UIButton) {
        switch sender.tag {
        case 0:
            self.delegate.blogCategory(categoryName: "Default", categoryId: 0)
            self.dismiss(animated: true, completion: nil)
        case 1:
            self.delegate.blogCategory(categoryName: "Cars and Vehicles", categoryId: 2)
            self.dismiss(animated: true, completion: nil)
        case 2:
            self.delegate.blogCategory(categoryName: "Comedy", categoryId: 3)
            self.dismiss(animated: true, completion: nil)
        case 3:
            self.delegate.blogCategory(categoryName: "Economics and Trade", categoryId: 4)
            self.dismiss(animated: true, completion: nil)
        case 4:
            self.delegate.blogCategory(categoryName: "Education", categoryId: 5)
            self.dismiss(animated: true, completion: nil)
        case 5:
            self.delegate.blogCategory(categoryName: "Entertainment", categoryId: 6)
            self.dismiss(animated: true, completion: nil)
        case 6:
            self.delegate.blogCategory(categoryName: "Movies and Animation", categoryId: 7)
            self.dismiss(animated: true, completion: nil)
        case 7:
            self.delegate.blogCategory(categoryName: "Gaming", categoryId: 8)
            self.dismiss(animated: true, completion: nil)
        case 8:
            self.delegate.blogCategory(categoryName: "History and Facts", categoryId: 9)
            self.dismiss(animated: true, completion: nil)
        case 9:
            self.delegate.blogCategory(categoryName: "Live Style", categoryId: 10)
            self.dismiss(animated: true, completion: nil)
        case 10:
            self.delegate.blogCategory(categoryName: "Natural", categoryId: 11)
            self.dismiss(animated: true, completion: nil)
        case 11:
            self.delegate.blogCategory(categoryName: "News and Politics", categoryId: 12)
            self.dismiss(animated: true, completion: nil)
        case 12:
            self.delegate.blogCategory(categoryName: "People and Nations", categoryId: 13)
            self.dismiss(animated: true, completion: nil)
        case 13:
            self.delegate.blogCategory(categoryName: "Pets and Animals", categoryId: 14)
            self.dismiss(animated: true, completion: nil)
        case 14:
            self.delegate.blogCategory(categoryName: "Places and Regions", categoryId: 15)
            self.dismiss(animated: true, completion: nil)
        case 15:
            self.delegate.blogCategory(categoryName: "Science and Technology", categoryId: 16)
            self.dismiss(animated: true, completion: nil)
        case 16:
            self.delegate.blogCategory(categoryName: "Sport", categoryId: 17)
            self.dismiss(animated: true, completion: nil)
        case 17:
            self.delegate.blogCategory(categoryName: "Travel and Events", categoryId: 18)
            self.dismiss(animated: true, completion: nil)
        case 18:
            self.delegate.blogCategory(categoryName: "Others", categoryId: 1)
            self.dismiss(animated: true, completion: nil)
        default:
        print("Nothing")
        }
        
    }
    

    @IBAction func Close(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
}

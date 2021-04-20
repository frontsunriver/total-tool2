
import UIKit
import WoWonderTimelineSDK
class MyAffiliatesVC: UIViewController {

    @IBOutlet weak var profileIamge: UIImageView!
    @IBOutlet weak var hypLinkLabel: UILabel!
    @IBOutlet weak var earnLabel: UILabel!
    @IBOutlet weak var shareBtn: RoundButton!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
            navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("My Affiliates", comment: "My Affiliates")
        self.setupUI()
        self.earnLabel.text = NSLocalizedString("Earn up to 10$ for each user you refer to us!", comment: "Earn up to 10$ for each user you refer to us!")
        self.shareBtn.setTitle(NSLocalizedString("Share to", comment: "Share to"), for: .normal)

    }
    private func setupUI(){
        let url = URL(string: AppInstance.instance.profile?.userData?.avatar ?? "")
                          self.profileIamge.kf.setImage(with: url)
    }
    

    @IBAction func sharePressed(_ sender: Any) {
        let firstActivityItem = "Text you want"
        let secondActivityItem : NSURL = NSURL(string:"https://demo.wowonder.com/@\(AppInstance.instance.profile?.userData?.username ?? "")" )!
        // If you want to put an image
//        let image : UIImage = UIImage(named: "image .jpg")!

        let activityViewController : UIActivityViewController = UIActivityViewController(
            activityItems: [firstActivityItem, secondActivityItem], applicationActivities: nil)

        // This lines is for the popover you need to show in iPad
        activityViewController.popoverPresentationController?.sourceView = (sender as! UIButton)

        // This line remove the arrow of the popover to show in iPad
        activityViewController.popoverPresentationController?.permittedArrowDirections = .up
        activityViewController.popoverPresentationController?.sourceRect = CGRect(x: 150, y: 150, width: 0, height: 0)

        // Anything you want to exclude
        activityViewController.excludedActivityTypes = [
            UIActivity.ActivityType.postToWeibo,
            UIActivity.ActivityType.print,
            UIActivity.ActivityType.assignToContact,
            UIActivity.ActivityType.saveToCameraRoll,
            UIActivity.ActivityType.addToReadingList,
            UIActivity.ActivityType.postToFlickr,
            UIActivity.ActivityType.postToVimeo,
            UIActivity.ActivityType.postToTencentWeibo
        ]

        self.present(activityViewController, animated: true, completion: nil)
    }
    
}

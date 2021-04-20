

import UIKit
import WoWonderTimelineSDK

class EventMoreController: UIViewController {
    
    
    
    @IBOutlet weak var moreLabel: UILabel!
    @IBOutlet weak var copyBtn: UIButton!
    @IBOutlet weak var shareBtn: UIButton!
    @IBOutlet weak var closeBtn: UIButton!
    @IBOutlet weak var editBtn: UIButton!
    var copyurl = ""
    var isOwner = false
    var event_details = [String:Any]()
    var isMyEventVC = false
    var delegate: EditEventBtnDelegate!
    let Stroyboard =  UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
   
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.moreLabel.text = NSLocalizedString("More", comment: "More")
        self.copyBtn.setTitle(NSLocalizedString("Copy", comment: "Copy"), for: .normal)
        self.shareBtn.setTitle(NSLocalizedString("Share", comment: "Share"), for: .normal)
        self.closeBtn.setTitle(NSLocalizedString("Close", comment: "Close"), for: .normal)
        self.editBtn.setTitle(NSLocalizedString("Edit", comment: "Edit"), for: .normal)
        if (self.isOwner == true) && (self.isMyEventVC == true){
            self.editBtn.isHidden = false
        }
        else{
            self.editBtn.isHidden = true
        }
    }
    
    @IBAction func Copy(_ sender: Any) {
        UIPasteboard.general.string = self.copyurl
        self.view.makeToast(NSLocalizedString("Copied", comment: "Copied"))
        DispatchQueue.main.asyncAfter(deadline: .now() + 2.0) { // Change `2.0` to the desired number of seconds.
            self.dismiss(animated: true, completion: nil)
            
        }
        
    }
    
    @IBAction func Share(_ sender: Any) {
        
        // text to share
        let text = copyurl
        
        // set up activity view controller
        let textToShare = [ text ]
        let activityViewController = UIActivityViewController(activityItems: textToShare, applicationActivities: nil)
        activityViewController.popoverPresentationController?.sourceView = self.view // so that iPads won't crash
        
        // exclude some activity types from the list (optional,)
        activityViewController.excludedActivityTypes = [ UIActivity.ActivityType.airDrop, UIActivity.ActivityType.postToFacebook, UIActivity.ActivityType.assignToContact,UIActivity.ActivityType.mail,UIActivity.ActivityType.postToTwitter,UIActivity.ActivityType.message,UIActivity.ActivityType.postToFlickr,UIActivity.ActivityType.postToVimeo,UIActivity.ActivityType.init(rawValue: "net.whatsapp.WhatsApp.ShareExtension"),UIActivity.ActivityType.init(rawValue: "com.google.Gmail.ShareExtension"),UIActivity.ActivityType.init(rawValue: "com.toyopagroup.picaboo.share"),UIActivity.ActivityType.init(rawValue: "com.tinyspeck.chatlyio.share")]
        
        // present the view controller
        self.present(activityViewController, animated: true, completion: nil)
    }
    
    
    @IBAction func Edit(_ sender: Any) {
        self.dismiss(animated: true) {
            self.delegate.EditButton(id: 0)
        }
    }
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        self.dismiss(animated: true, completion: nil)
    }
    
    @IBAction func Close(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    

}

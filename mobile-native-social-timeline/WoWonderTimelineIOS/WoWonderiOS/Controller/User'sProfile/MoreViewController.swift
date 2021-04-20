

import UIKit
import WoWonderTimelineSDK
class MoreViewController: UIViewController {
 
    @IBOutlet weak var moreLabel: UILabel!
    @IBOutlet weak var blockBtn: UIButton!
    @IBOutlet weak var copyBtn: UIButton!
    @IBOutlet weak var share: UIButton!
    @IBOutlet weak var poke: UIButton!
    @IBOutlet weak var sendGift: UIButton!


    
    var delegate : blockUserDelegate?
    var delegate1: ProfileMoreDelegate?
    let status = Reach().connectionStatus()
    var userId = ""
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        NotificationCenter.default.addObserver(self, selector: #selector(MoreViewController.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        
        self.moreLabel.text = NSLocalizedString("More", comment: "More")
        self.blockBtn.setTitle(NSLocalizedString("Block", comment: "Block"), for: .normal)
         self.copyBtn.setTitle(NSLocalizedString("Copy Link", comment: "Copy Link"), for: .normal)
         self.share.setTitle(NSLocalizedString("Share", comment: "Share"), for: .normal)
         self.poke.setTitle(NSLocalizedString("Pokes", comment: "Pokes"), for: .normal)
         self.sendGift.setTitle(NSLocalizedString("Send Gift", comment: "Send Gift"), for: .normal)
        
        
        
    }
    /////////////////////////NetWork Connection//////////////////////////
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
            
        }
    }
    
    @IBAction func Close(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    @IBAction func BlockUser(_ sender: Any) {
        self.blockUser(userId : self.userId)
    }
    
    @IBAction func CopyLink(_ sender: Any) {
        self.dismiss(animated: true) {
            self.delegate1?.profileMore(tag: 1)
        }
    }
    
    @IBAction func Share(_ sender: Any) {
        self.dismiss(animated: true) {
            self.delegate1?.profileMore(tag: 2)
        }
    }
    
    @IBAction func Poke(_ sender: Any) {
        self.dismiss(animated: true) {
            self.delegate1?.profileMore(tag: 3)
        }
    }
    
    @IBAction func AddtoFamily(_ sender: Any) {
        self.dismiss(animated: true) {
            self.delegate1?.profileMore(tag: 4)
        }
    }
    
    @IBAction func Gift(_ sender: Any) {
        self.dismiss(animated: true) {
            self.delegate1?.profileMore(tag: 5)
        }
    }
    
    private func blockUser(userId : String){
        switch status {
        case .unknown, .offline:
            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan), .online(.wiFi):
            Block_UserManager.sharedInstance.blockUser(user_Id: userId, blockUser: "block") { (success, authError, error) in
                if success != nil {
                    self.dismiss(animated: true, completion: nil)
                    self.delegate?.block()
                    print(success?.block_status)
                }
                else if authError != nil {
                    self.showAlert(title: "", message: (authError?.errors.errorText)!)
                }
                else if error != nil {
                    print("error")
                    
                }
            }
        }
    }
}

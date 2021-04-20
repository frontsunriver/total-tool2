

import UIKit
import Toast_Swift
import WoWonderTimelineSDK
class PageDeleteController: UIViewController {
    
    
    @IBOutlet weak var passwordField: RoundTextField!
    @IBOutlet weak var checkView: UIView!
    @IBOutlet weak var check: RoundButton!
    @IBOutlet weak var deleteLabel: UILabel!
    @IBOutlet weak var sureLabel: UILabel!
    @IBOutlet weak var deleteBtn: RoundButton!
    var ischeck = 0
    var pageId = ""
    var delegate : DeletePageDelegate!
    let status = Reach().connectionStatus()


    override func viewDidLoad() {
        super.viewDidLoad()
        
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        
        self.deleteLabel.text = NSLocalizedString("Delete", comment: "Delete")
        self.passwordField.placeholder = NSLocalizedString("Password", comment: "Password")
        self.sureLabel.text = NSLocalizedString("Are you sure want to delete ?", comment: "Are you sure want to delete ?")
        self.deleteBtn.setTitle(NSLocalizedString("Delete", comment: "Delete"), for: .normal)
        
   
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
        
    }
    
    
    @IBAction func CheckBtn(_ sender: Any) {
        if ischeck == 0 {
            self.checkView.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
            self.check.setImage(UIImage(named: "tick"), for: .normal)
            self.ischeck = 1
        }
        else {
            self.checkView.backgroundColor = .white
            self.check.setImage(UIImage(named: ""), for: .normal)
            self.ischeck = 0
        }
    }
    
    
    @IBAction func Delete(_ sender: Any) {
        if self.passwordField.text?.isEmpty == true {
            self.view.makeToast("Please Enter Password")
        }
        else if self.checkView.backgroundColor != UIColor.hexStringToUIColor(hex: "#984243"){
            self.view.makeToast("Are you sure want to delete group please check")
        }
        else {
            self.deletePage()
        }
    }
    
    private func deletePage() {
        
        switch status {
        case .unknown, .offline:
            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            DeletePageManager.sharedInstance.deletePage(pageId: self.pageId, password: self.passwordField.text!) { (success, authError, error) in
                performUIUpdatesOnMain {
                    if success != nil {
                        self.view.makeToast(success!.message)
                        self.dismiss(animated: true, completion: {
                            self.delegate.deletePage(pageId: self.pageId)
                        })
                    }
                        
                    else if authError != nil {
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        print(error?.localizedDescription)
                    }
                }
            }
        }
    }
}

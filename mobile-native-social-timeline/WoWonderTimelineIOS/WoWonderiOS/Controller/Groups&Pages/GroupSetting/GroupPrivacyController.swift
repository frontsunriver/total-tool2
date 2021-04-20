

import UIKit
import WoWonderTimelineSDK


class GroupPrivacyController: UIViewController {
    
    
    @IBOutlet weak var publicView: DesignView!
    @IBOutlet weak var privateView: DesignView!
    @IBOutlet weak var privacyLabel: UILabel!
    @IBOutlet weak var saveBtn: UIButton!
    @IBOutlet weak var confirmLbl: UILabel!
    
    let status = Reach().connectionStatus()
      
    var privacy = "0"
    var groupId = ""
    

    override func viewDidLoad() {
        super.viewDidLoad()
        self.publicView.backgroundColor = .white
        self.privateView.backgroundColor = .white
        
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.privacyLabel.text = NSLocalizedString("Privacy", comment: "Privacy")
        self.saveBtn.setTitle(NSLocalizedString("Save", comment: "Save"), for: .normal)
        self.confirmLbl.text = NSLocalizedString("Confirm when someone joining this group ?", comment: "Confirm when someone joining this group ?")
    }
    
    /// Network Connectivity
     @objc func networkStatusChanged(_ notification: Notification) {
         if let userInfo = notification.userInfo {
             let status = userInfo["Status"] as! String
             print("Status",status)
         }
     }
    
    
    @IBAction func Save(_ sender: Any) {
        self.updateData()
    }
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)

    }
    
    @IBAction func Public(_ sender: Any) {
        self.publicView.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
       self.privateView.backgroundColor = .white
        self.privacy = "1"

        
    }
    
    
    @IBAction func Private(_ sender: Any) {
        self.publicView.backgroundColor = .white
        self.privateView.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
      self.privacy = "2"

    }
    
    
    private func updateData(){
        switch status {
        case .unknown, .offline:
        self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
//            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                UpdateGroupDataManager.sharedInstance.updateGroupData(params: [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key, APIClient.Params.groupId : self.groupId, APIClient.Params.groupPrivacy : self.privacy]) { (success, authError, error) in
                    if success != nil {
                        self.view.makeToast(success?.message)
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

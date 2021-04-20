//
//  DeleteController.swift
//  News_Feed
//
//

import UIKit
import Toast_Swift

class DeleteGroupController: UIViewController {
    
    
    @IBOutlet weak var passwordField: RoundTextField!
    @IBOutlet weak var sureLabel: UILabel!
    @IBOutlet weak var checkView: UIView!
    @IBOutlet weak var check: RoundButton!
    @IBOutlet weak var deleteBtn: RoundButton!
    @IBOutlet weak var deleteLabel: UILabel!
    var groupId = ""
    var ischeck = 0
    
    let status = Reach().connectionStatus()
    var delegate : DeleteGroupDelegate!

    
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
            self.view.makeToast(NSLocalizedString("Enter password", comment: "Enter password"))
        }
        else if self.checkView.backgroundColor != UIColor.hexStringToUIColor(hex: "#984243"){
            self.view.makeToast(NSLocalizedString("Are you sure want to delete group please check", comment: "Are you sure want to delete group please check"))
        }
        else {
            self.deleteGroup()
        }
    }
    

    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    private func deleteGroup(){
        switch status {
             case .unknown, .offline:
                self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
//                 showAlert(title: "", message: "")
             case .online(.wwan),.online(.wiFi):
                performUIUpdatesOnMain {
                    DeleteGroupManager.sharedInstance.deleteGroup(group_id: self.groupId, password: self.passwordField.text!) { (success, authError, error) in
                        if success != nil {
                        self.view.makeToast(success!.message)
                        self.dismiss(animated: true, completion: {
                        self.delegate.deleteGroup(groupId: self.groupId)
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

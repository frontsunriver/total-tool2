

import UIKit
import Toast_Swift
import WoWonderTimelineSDK


class GeneralController: UIViewController,selectCategoryDelegate {
    
    
    
    @IBOutlet weak var generalLbl: UILabel!
    
    @IBOutlet weak var saveBtn: UIButton!
    
    func selectCategory(categoryID: Int, categoryName: String) {
        self.categoryId = "\(categoryID)"
        print(categoryId)
        self.groupCategory.text = categoryName
    }
    
    
    
    @IBOutlet weak var groupTitle: RoundTextField!
    @IBOutlet weak var groupName: RoundTextField!
    @IBOutlet weak var groupDesc: RoundTextField!
    @IBOutlet weak var groupCategory: RoundTextField!
    
    
    let status = Reach().connectionStatus()
    var delegate : EditGroupDataDelegate!
    
    var group_Id = ""
    var group_Title = ""
    var group_Name = ""
    var categoryId = ""
    var categoryName = ""
    var privacy = "0"
    var about = ""
    
    override func viewDidLoad() {
        super.viewDidLoad()
        print(self.group_Id)
        self.groupTitle.text = self.group_Title
        self.groupName.text = self.group_Name
        self.groupDesc.text = self.about
        self.groupCategory.text = self.categoryName
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.generalLbl.text = NSLocalizedString("General", comment: "General")
        self.saveBtn.setTitle(NSLocalizedString("Save", comment: "Save"), for: .normal)
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    
    
    @IBAction func Category(_ sender: Any) {
        self.groupCategory.resignFirstResponder()
        let Storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = Storyboard.instantiateViewController(withIdentifier: "CategoryVC") as! GroupCategoryController
        vc.delegate = self
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    
    
    @IBAction func Save(_ sender: Any) {
        self.updateData()
    }
    
    
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil
        )
    }
    
    private func updateData(){
        switch status {
        case .unknown, .offline:
            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                UpdateGroupDataManager.sharedInstance.updateGroupData(params: [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key, APIClient.Params.groupId : self.group_Id,APIClient.Params.about : self.groupDesc.text!, APIClient.Params.groupName : self.groupName.text!, APIClient.Params.groupTitle : self.groupTitle.text!,APIClient.Params.category : self.categoryId]) { (success, authError, error) in
                    if success != nil {
                        self.view.makeToast(success?.message)
                        self.delegate.editGroup(groupName: self.groupName.text!, groupTitle: self.groupTitle.text!, about: self.groupDesc.text!, Category: self.groupCategory.text!, CategoryId: self.categoryId)
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

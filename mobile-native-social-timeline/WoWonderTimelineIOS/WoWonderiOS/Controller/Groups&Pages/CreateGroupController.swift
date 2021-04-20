

import UIKit
import Toast_Swift
import ZKProgressHUD
import WoWonderTimelineSDK


class CreateGroupController: UIViewController,UITextFieldDelegate {
  
    @IBOutlet weak var groupNameField: RoundTextField!
    @IBOutlet weak var groupTitleField: RoundTextField!
    @IBOutlet weak var groupURLField: RoundTextField!
    @IBOutlet weak var aboutField: UITextView!
    @IBOutlet weak var categoryField: RoundTextField!
    @IBOutlet weak var publicBtn: UIButton!
    @IBOutlet weak var privateBtn: UIButton!
    @IBOutlet weak var publicView: DesignView!
    @IBOutlet weak var privateView: DesignView!
    @IBOutlet weak var createLabel: UILabel!
    @IBOutlet weak var saveBtn: UIButton!
    @IBOutlet weak var describeLbl: UILabel!
    @IBOutlet weak var groupDescLbl: UILabel!
    @IBOutlet weak var privacyLabel: UILabel!
    
    let status = Reach().connectionStatus()
    var delegate : CreateGroupDelegate!
    var category = 0
    var privacy = 0
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationItem.hidesBackButton = true
        self.navigationController?.navigationBar.isHidden = true
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.publicView.backgroundColor = .white
        self.privateView.backgroundColor = .white
        self.categoryField.inputView = UIView()
        self.createLabel.text = NSLocalizedString("Create New Group", comment: "Create New Group")
        self.saveBtn.setTitle(NSLocalizedString("Save", comment: "Save"), for: .normal)
        self.describeLbl.text = NSLocalizedString("Describe Your Group", comment: "Describe Your Group")
        self.groupDescLbl.text = NSLocalizedString("Tell pontentials members what your groups about to help them know whether its relevant to them", comment: "Tell pontentials members what your groups about to help them know whether its relevant to them")
        self.privacyLabel.text = NSLocalizedString("Privacy", comment: "Privacy")
        self.groupTitleField.placeholder = NSLocalizedString("Group Title", comment: "Group Title")
        self.groupURLField.placeholder = NSLocalizedString("Group URL", comment: "Group URL")
        self.aboutField.text = NSLocalizedString("About Group", comment: "About Group")
        self.categoryField.placeholder = NSLocalizedString("Select A Category", comment: "Select A Category")

    }
    override func viewWillAppear(_ animated: Bool) {
//        self.categoryField.inputView = UIView()
    }
    
    
    /// Network Connectivity
      @objc func networkStatusChanged(_ notification: Notification) {
          if let userInfo = notification.userInfo {
              let status = userInfo["Status"] as! String
              print("Status",status)
          }
      }
    
    private func createGroup() {
        switch status {
        case .unknown, .offline:
            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            ZKProgressHUD.show()
       performUIUpdatesOnMain {
                
        CreateGroupManager.sharedInstance.createGroup(groupName: self.groupURLField.text!, groupTitle: self.groupTitleField.text!, aboutGroup: self.aboutField.text!, category: self.category, Privacy: self.privacy) { (success, authError, error) in
        if success != nil {
                print(success!.group_data)
            let groupData = (success!.group_data)
            print(groupData)
              ZKProgressHUD.dismiss()
            self.delegate.sendGroupData(groupData: groupData)
             self.view.makeToast("Group Created")
             self.dismiss(animated: true, completion: nil)
                
            }
            
        else if authError != nil {
            ZKProgressHUD.dismiss()
            if authError?.errors.errorText == "Invalid group name characters"{
                self.view.makeToast("Invalid group URL")
            }
            else{
                self.view.makeToast(authError?.errors.errorText)
            }
                }
            else {
                ZKProgressHUD.dismiss()
                print(error?.localizedDescription)
                }
            }
        }
    }
        
        
    }
    
    
    
    @IBAction func SelectCategory(_ sender: UITextField) {
      self.categoryField.resignFirstResponder()
      self.performSegue(withIdentifier: "gotoCategory", sender: self)
        
    }
    
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        if segue.identifier == "gotoCategory" {
            let vc = segue.destination as! GroupCategoryController
            vc.delegate = self
        }
    }
    
    
    @IBAction func Public(_ sender: Any) {
        self.publicView.backgroundColor = UIColor.hexStringToUIColor(hex:"#984243")
        self.privateView.backgroundColor = .white
        self.privacy = 1
        
    }
    
    @IBAction func Private(_ sender: Any) {
        self.publicView.backgroundColor = .white
        self.privateView.backgroundColor = UIColor.hexStringToUIColor(hex:"#984243")
        self.privacy = 2
    }
    
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    
    @IBAction func Save(_ sender: Any) {
        if (self.groupTitleField.text?.isEmpty == true) {
            self.view.makeToast(NSLocalizedString("Enter Group Name", comment: "Enter Group Name"))
        }
        else if (self.groupURLField.text?.isEmpty == true) {
            self.view.makeToast(NSLocalizedString("Enter Group Url", comment: "Enter Group Url"))
        }
        else if (self.aboutField.text?.isEmpty == true) {
            self.view.makeToast(NSLocalizedString("Enter Group About", comment: "Enter Group About"))
        }
        else if (self.categoryField.text?.isEmpty == true) {
            self.view.makeToast(NSLocalizedString("Enter Category", comment: "Enter Category"))
        }
        else if (self.privacy == 0) {
            self.view.makeToast(NSLocalizedString("select Group Privacy", comment: "select Group Privacy"))
        }
        else {
            self.createGroup()
        }
        
    }
    
}

extension CreateGroupController : selectCategoryDelegate {
    func selectCategory(categoryID: Int, categoryName: String) {
        self.categoryField.text = categoryName
        self.category = categoryID
    }
    
    
    
    
}



import UIKit
import ZKProgressHUD
import WoWonderTimelineSDK

class EditProfileVC: UIViewController {
    @IBOutlet weak var lastNameTextField: UITextField!
    @IBOutlet weak var schoolTextField: UITextField!
    @IBOutlet weak var workSpaceTextFeild: UITextField!
    @IBOutlet weak var websiteTextField: UITextField!
    @IBOutlet weak var mobileTextField: UITextField!
    @IBOutlet weak var locationTextField: UITextField!
    @IBOutlet weak var firstNameTextFiled: UITextField!
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.setupUI()
    }
    private func setupUI(){
//        self.title = "Edit Profile"
        
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
            navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("Edit Profile", comment: "Edit Profile")
        let save = UIBarButtonItem(title: NSLocalizedString("Save", comment: "Save"), style: .done, target: self, action: #selector(Save))
        self.navigationItem.rightBarButtonItem  = save
        self.firstNameTextFiled.text = AppInstance.instance.profile?.userData?.firstName ?? ""
        self.lastNameTextField.text = AppInstance.instance.profile?.userData?.lastName ?? ""
        self.locationTextField.text = AppInstance.instance.profile?.userData?.address ?? ""
    
        self.mobileTextField.text = AppInstance.instance.profile?.userData?.phoneNumber ?? ""
        self.websiteTextField.text = AppInstance.instance.profile?.userData?.website ?? ""
        self.workSpaceTextFeild.text = AppInstance.instance.profile?.userData?.working ?? ""
        self.schoolTextField.text = AppInstance.instance.profile?.userData?.school ?? ""
        self.firstNameTextFiled.placeholder = NSLocalizedString("First Name", comment: "First Name")
        self.lastNameTextField.placeholder = NSLocalizedString("Last Name", comment: "Last Name")
        self.locationTextField.placeholder = NSLocalizedString("Location", comment: "Location")
        self.mobileTextField.placeholder = NSLocalizedString("Mobile", comment: "Mobile")
        self.websiteTextField.placeholder = NSLocalizedString("Website", comment: "Website")
        self.schoolTextField.placeholder = NSLocalizedString("School", comment: "School")
    }
    
    
    @objc func Save(){
        self.updateMyProfile()
    }
    
    
    private func updateMyProfile(){
        ZKProgressHUD.show()
        let firstName = self.firstNameTextFiled.text ?? ""
        let lastName = self.lastNameTextField.text ?? ""
        let location = self.locationTextField.text ?? ""
        let mobile = self.mobileTextField.text ?? ""
        let website = self.websiteTextField.text ?? ""
        let workspace = self.workSpaceTextFeild.text ?? ""
        let school = self.schoolTextField.text ?? ""
        performUIUpdatesOnMain {
            UpdateUserManager.instance.updateProfile(firstName: firstName, lastName: lastName, mobile: mobile, website: website, workSpace: workspace, school: school,location: location) { (success, authError, error) in
                if success != nil {
                    AppInstance.instance.getProfile()
                    self.view.makeToast(success?.message ?? "")
                    ZKProgressHUD.dismiss()
                }
                else if authError != nil {
                    ZKProgressHUD.dismiss()
                    self.view.makeToast(authError?.errors?.errorText)
                    self.showAlert(title: "", message: (authError?.errors?.errorText)!)
                }
                else if error  != nil {
                    ZKProgressHUD.dismiss()
                    print(error?.localizedDescription)
                    
                }
            }
        }
        
    }
    
}

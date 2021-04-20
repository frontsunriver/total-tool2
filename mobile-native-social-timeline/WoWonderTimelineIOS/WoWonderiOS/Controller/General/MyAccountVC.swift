
import UIKit
import WoWonderTimelineSDK
import ZKProgressHUD
class MyAccountVC: UIViewController {
    
    
    @IBOutlet weak var genderBtn: UIButton!
    @IBOutlet weak var birthdayTextField: UITextField!
    @IBOutlet weak var emailTextField: UITextField!
    @IBOutlet weak var usernameTextField: UITextField!
    
    var genderString:String? = ""
     let datePicker = UIDatePicker()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.setupUI()
    }
    private func setupUI(){
        
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
            navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("My Account", comment: "My Account")
        
           let save = UIBarButtonItem(title: NSLocalizedString("Save", comment: "Save"), style: .done, target: self, action: #selector(Save))
           self.navigationItem.rightBarButtonItem  = save
        datePicker.datePickerMode = .date

               //ToolBar
              let toolbar = UIToolbar();
              toolbar.sizeToFit()

              //done button & cancel button
        let doneButton = UIBarButtonItem(title: NSLocalizedString("Done", comment: "Done"), style: UIBarButtonItem.Style.bordered, target: self, action: "donedatePicker")
        let spaceButton = UIBarButtonItem(barButtonSystemItem: UIBarButtonItem.SystemItem.flexibleSpace, target: nil, action: nil)
        let cancelButton = UIBarButtonItem(title: NSLocalizedString("Cancel", comment: "Cancel"), style: UIBarButtonItem.Style.bordered, target: self, action: "cancelDatePicker")
              toolbar.setItems([doneButton,spaceButton,cancelButton], animated: false)

           // add toolbar to textField
           birthdayTextField.inputAccessoryView = toolbar
            // add datepicker to textField
           birthdayTextField.inputView = datePicker
        
        self.usernameTextField.text = AppInstance.instance.profile?.userData?.username ?? ""
        self.emailTextField.text = AppInstance.instance.profile?.userData?.email ?? ""
         self.birthdayTextField.text = AppInstance.instance.profile?.userData?.birthday ?? ""
        self.genderBtn.setTitle(AppInstance.instance.profile?.userData?.genderText ?? "", for: .normal)
        self.genderString = AppInstance.instance.profile?.userData?.genderText ?? ""
        self.usernameTextField.placeholder = NSLocalizedString("User Name", comment: "User Name")
        self.emailTextField.placeholder = NSLocalizedString("Email", comment: "Email")
        self.birthdayTextField.placeholder = NSLocalizedString("Birthday", comment: "Birthday")
    
    }
    @objc func donedatePicker(){

      let formatter = DateFormatter()
      formatter.dateFormat = "dd/MM/yyyy"
      birthdayTextField.text = formatter.string(from: datePicker.date)
        self.genderString = birthdayTextField.text ?? ""
      self.view.endEditing(true)
    }

    @objc func cancelDatePicker(){
       self.view.endEditing(true)
     }
    
   
    @objc func Save(){
        self.updateMyAccount()
       }
    
    @IBAction func genderPressed(_ sender: Any) {
        let storyboard = UIStoryboard(name: "General", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "SelectGenderVC") as! SelectGenderVC
        vc.delegate = self
        self.present(vc, animated: true, completion: nil)
    }
    private func updateMyAccount(){
        ZKProgressHUD.show()
        let username = usernameTextField.text ?? ""
        let email = emailTextField.text ?? ""
        let birthday = birthdayTextField.text ?? ""
        let gender = genderString ?? ""
        performUIUpdatesOnMain {
            UpdateUserManager.instance.updateMyAccount(username: username, email: email, birthday: birthday, gender: gender) { (success, authError, error) in
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
extension MyAccountVC:TwoFactorAuthDelegate{
    func getTwoFactorUpdateString(type: String) {
        
        self.genderBtn.setTitle(type, for: .normal)
        self.genderString = type
    }
    
    
}

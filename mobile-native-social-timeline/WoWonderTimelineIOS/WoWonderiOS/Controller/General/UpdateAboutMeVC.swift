

import UIKit
import WoWonderTimelineSDK
import ZKProgressHUD
class UpdateAboutMeVC: UIViewController {
    
    @IBOutlet weak var aboutTextFIeld: UITextField!
    @IBOutlet weak var aboutLabel: UILabel!
    @IBOutlet weak var cancelBtn: UIButton!
    @IBOutlet weak var saveBtn: UIButton!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.setupUI()
        self.aboutLabel.text = NSLocalizedString("About", comment: "About")
        self.saveBtn.setTitle(NSLocalizedString("SAVE", comment: "SAVE"), for: .normal)
        self.cancelBtn.setTitle(NSLocalizedString("CANCEL", comment: "CANCEL"), for: .normal)
    }
    
    @IBAction func cancelPressed(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    private func setupUI(){
        self.aboutTextFIeld.text = AppInstance.instance.profile?.userData?.about ?? ""
    }
    
    @IBAction func savePressed(_ sender: Any) {
        self.updateAboutMe()
        
    }
    private func updateAboutMe(){
        ZKProgressHUD.show()
        
        let aboutMe = aboutTextFIeld.text ?? ""
        performUIUpdatesOnMain {
            UpdateUserManager.instance.updateAboutMe(aboutMe: aboutMe) { (success, authError, error) in
                if success != nil {
                     AppInstance.instance.getProfile()
                    self.view.makeToast(success?.message ?? "")
                    ZKProgressHUD.dismiss()
                    
                    self.dismiss(animated: true, completion: nil)

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


import UIKit
import ZKProgressHUD
import WoWonderTimelineSDK

class ConfirmationCodeVC: UIViewController {
    
    @IBOutlet weak var confirmLbl: UILabel!
    @IBOutlet weak var codeTextField: UITextField!
    @IBOutlet weak var cancelBtn: UIButton!
    @IBOutlet weak var confirmBtn: UIButton!
    override func viewDidLoad() {
        super.viewDidLoad()
        self.confirmLbl.text = NSLocalizedString("A confirmation email has been sent", comment: "A confirmation email has been sent")
        self.cancelBtn.setTitle(NSLocalizedString("CANCEL", comment: "CANCEL"), for: .normal)
        self.confirmBtn.setTitle(NSLocalizedString("Confirm", comment: "Confirm"), for: .normal)
        self.codeTextField.placeholder = NSLocalizedString("Confirmation Code", comment: "Confirmation Code")
    }
    
    @IBAction func cancelPressed(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    @IBAction func sendPressed(_ sender: Any) {
        if self.codeTextField.text!.isEmpty{
            self.view.makeToast("Please enter code")
        }else{
            self.verifyCode(code: self.codeTextField.text ?? "", Type: "verify")
        }
    }
    private func verifyCode(code:String,Type:String){
        ZKProgressHUD.show()
        performUIUpdatesOnMain {
            TwoFactorManager.instance.updateVerifyTwoFactor(code: code, Type: Type) { (success, authError, error) in
                if success != nil {
                    self.view.makeToast(success?.message ?? "")
                    self.dismiss(animated: true) {
                        AppInstance.instance.getProfile()
                        ZKProgressHUD.dismiss()
                    }
                }
                else if authError != nil {
                    ZKProgressHUD.dismiss()
                    self.view.makeToast(authError?.errors.errorText)
                    self.showAlert(title: "", message: (authError?.errors.errorText)!)
                }
                else if error  != nil {
                    ZKProgressHUD.dismiss()
                    print(error?.localizedDescription)
                    
                }
            }
            
        }
    }
}

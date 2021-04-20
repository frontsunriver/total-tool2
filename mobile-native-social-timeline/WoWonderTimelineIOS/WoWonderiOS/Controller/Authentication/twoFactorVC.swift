import WoWonderTimelineSDK
import UIKit
import ZKProgressHUD
class twoFactorVC: UIViewController {
    @IBOutlet weak var verifyCodeTextField: UITextField!
    @IBOutlet weak var toLogLabel: UILabel!
    @IBOutlet weak var sentLabel: UILabel!
    @IBOutlet weak var verifyBtn: UIButton!
    
    
    
    var code:String? = ""
    var userID : String? = ""
    var error = ""
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(LoginController.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.verifyBtn.setTitle(NSLocalizedString("VERIFY", comment: "VERIFY"), for: .normal)
        self.sentLabel.text = NSLocalizedString("We have sent you the confirmation code to your email address.", comment: "We have sent you the confirmation code to your email address.")
        self.toLogLabel.text = NSLocalizedString("To log in, you need to verify  your identity.", comment: "To log in, you need to verify  your identity.")
        self.verifyCodeTextField.placeholder = NSLocalizedString("Add code number", comment: "Add code number")
    }
    
    
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
            
        }
        
    }
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        self.navigationController?.isNavigationBarHidden = false
    }
    override func viewWillDisappear(_ animated: Bool) {
        super.viewWillDisappear(animated)
    }
    
    
    @IBAction func verifyPressed(_ sender: Any) {
        if self.verifyCodeTextField.text!.isEmpty{
            self.view.makeToast(NSLocalizedString("Please enter Code", comment: "Please enter Code"))
        }else{
            self.verifyCode(code: self.verifyCodeTextField.text ?? "", userid: userID ?? "")
        }
        
    }
    
    
    private func verifyCode (code:String, userid : String) {
        ZKProgressHUD.show()
        let status = Reach().connectionStatus()
        switch status {
        case .unknown, .offline:
            self.error = NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed")
            self.performSegue(withIdentifier: "ErrorVC", sender: self)
            
        case .online(.wwan), .online(.wiFi):
            ZKProgressHUD.show()

            TwoFactorManager.instance.verifyCode(code: code, UserID: userid) { (success, authError, error) in
                if success != nil {
                    ZKProgressHUD.dismiss()
                    print("Login Succesfull =\(success?.accessToken ?? "")")
                    
                    UserData.setaccess_token(success?.accessToken)
                    UserData.setUSER_ID(success?.userID)
                    AppInstance.instance.getProfile()
                    let vc = UIStoryboard(name: "Authentication", bundle: nil).instantiateViewController(identifier: "IntroController") as? IntroController
                    
                    self.navigationController?.pushViewController(vc!, animated: true)
                    
                    
                    
                    
                }
                    
                else if authError != nil {
                    ZKProgressHUD.dismiss()
                    self.error = authError?.errors.errorText ?? ""
                    self.performSegue(withIdentifier: "ErrorVC", sender: self)
                    print(authError?.errors.errorText)
                    
                }
                    
                else if error != nil{
                    ZKProgressHUD.dismiss()
                    print("error - \(error?.localizedDescription)")
                    
                    
                }
            }
            
        }
    }
}

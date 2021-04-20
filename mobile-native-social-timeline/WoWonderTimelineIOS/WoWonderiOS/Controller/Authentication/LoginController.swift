
import UIKit
import ZKProgressHUD
import FBSDKLoginKit
import GoogleSignIn
import WoWonderTimelineSDK
import AuthenticationServices

@available(iOS 13.0, *)
class LoginController: BaseVC {
    
    @IBOutlet weak var appleBtn: ASAuthorizationAppleIDButton!
    @IBOutlet weak var userNameField: RoundTextField!
    @IBOutlet weak var passwordField: RoundTextField!
    @IBOutlet weak var fbLoginBtn: FBButton!
    @IBOutlet weak var googleBtn: GIDSignInButton!
    @IBOutlet weak var signBtn: RoundButton!
    @IBOutlet weak var forgetPasswordBtn: UIButton!
    @IBOutlet weak var createAccountBtn: UIButton!
    
    var error = ""
    
    override func viewDidLoad() {
        super.viewDidLoad()
        print("OneSignal device id = \(self.oneSignalID ?? "")")
        self.userNameField.placeholder = NSLocalizedString("Username", comment: "Username")
        self.passwordField.placeholder = NSLocalizedString("Password", comment: "Password")
        self.signBtn.setTitle(NSLocalizedString("Sign in", comment: "Sign in"), for: .normal)
        self.forgetPasswordBtn.setTitle(NSLocalizedString("Forget Password ?", comment: "Forget Password ?"), for: .normal)
        self.createAccountBtn.setTitle(NSLocalizedString("Create Account Sign Up", comment: "Create Account Sign Up"), for: .normal)
        NotificationCenter.default.addObserver(self, selector: #selector(LoginController.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.fbLoginBtn.titleLabel?.textAlignment = .center
        self.fbLoginBtn.setTitle(NSLocalizedString("Continue with Facebook", comment: "Continue with Facebook"), for: .normal)
        self.fbLoginBtn.titleLabel?.font = UIFont(name: "Arial Rounded MT Bold", size: 15.0)
        GIDSignIn.sharedInstance().presentingViewController = self
        if ControlSettings.isShowSocicalLogin{
            self.googleBtn.isHidden = false
            self.fbLoginBtn.isHidden = false
             self.appleBtn.isHidden = false
        }else{
            self.googleBtn.isHidden = true
                     self.fbLoginBtn.isHidden = true
                      self.appleBtn.isHidden = true
        }
        
        
    }
    
    override func viewWillAppear(_ animated: Bool) {
        self.navigationController?.navigationBar.isHidden = true
    }
    
    
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
            
        }
        
    }
    @IBAction func loginWithApplePressed(_ sender: Any) {
        let appleIDProvider = ASAuthorizationAppleIDProvider()
        let request = appleIDProvider.createRequest()
        request.requestedScopes = [.fullName, .email]
        let authorizationController = ASAuthorizationController(authorizationRequests: [request])
        authorizationController.delegate = self
        authorizationController.performRequests()

    }
    func setUpSignInAppleButton() {

      
      //Add button on some view or stack
    }
    
    @IBAction func SignIn(_ sender: Any) {
        
        if self.userNameField.text?.isEmpty == true {
            self.error = NSLocalizedString("Error, Required Username", comment: "Error, Required Username")
            self.performSegue(withIdentifier: "ErrorVC", sender: self)
            //        self.showAlert(title: "Required Field", message: "Required UserName")
        }
        else if self.passwordField.text?.isEmpty == true {
            self.error = NSLocalizedString("Error, Required Password", comment: "Error, Required Password")
            self.performSegue(withIdentifier: "ErrorVC", sender: self)
            
            //        self.showAlert(title: "Required Field", message: "Required Password")
        }
        else {
            ZKProgressHUD.show(NSLocalizedString("Loading", comment: "Loading"))
            self.loginAuthentication(userName: self.userNameField.text!, password: self.passwordField.text!, deviceID: self.oneSignalID ?? "")
        }
        
    }
    
    
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        if segue.identifier == "ErrorVC"{
            let destinationVc = segue.destination as! SecurityController
            destinationVc.error = error
        }
    }
    
    
    
    @IBAction func FBLogin(_ sender: Any) {
        self.facebookLogin() 
    }
    
    
    @IBAction func GoogleSignin(_ sender : Any){
        GIDSignIn.sharedInstance().delegate = self
        GIDSignIn.sharedInstance().signIn()
        
    }
    
    
    
    @IBAction func ForgetPassword(_ sender: Any) {
        self.performSegue(withIdentifier: "ForgetPasswordVC", sender: self)
    }
    
    
    
    
    private func loginAuthentication (userName:String, password : String,deviceID:String) {
        
        let status = Reach().connectionStatus()
        switch status {
        case .unknown, .offline:
            self.error = NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed")
            self.performSegue(withIdentifier: "ErrorVC", sender: self)
            
        case .online(.wwan), .online(.wiFi):
            
            AuthenticationManager.sharedInstance.loginAuthentication(userName: userName, password: password,deviceId:deviceID) { (success, authError, error) in
                
                if success != nil {
                    ZKProgressHUD.dismiss()
                    print("Login Succesfull =\(success?.message)")
                    if success?.message == "Please enter your confirmation code"{
                        let vc = UIStoryboard(name: "Authentication", bundle: nil).instantiateViewController(identifier: "twoFactorVC") as? twoFactorVC
                        vc?.userID = success?.userID ?? ""
                        self.navigationController?.pushViewController(vc!, animated: true)
                    }else{
                        UserData.setaccess_token(success?.accessToken)
                                         UserData.setUSER_ID(success?.userID)
                                         AppInstance.instance.getProfile()
                                         self.performSegue(withIdentifier: "imageSlider", sender: self)
                    }
                 
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
    
    
    private func facebookLogin () {
        
        let status = Reach().connectionStatus()
        switch status {
        case .unknown, .offline:
            self.error = NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed")
            self.performSegue(withIdentifier: "ErrorVC", sender: self)
            
        case .online(.wwan), .online(.wiFi):
            //            ZKProgressHUD.show("Loading")
            let fbLoginManager : LoginManager = LoginManager()
            
            fbLoginManager.logIn(permissions: ["email"], from: self) { (result, error) in
                if (error == nil) {
                    ZKProgressHUD.dismiss()
                    let fbloginresult : LoginManagerLoginResult = result!
                    if(fbloginresult.isCancelled) {
                        //Show Cancel alert
                        ZKProgressHUD.dismiss()
                        print("cancelResult",fbloginresult.isCancelled)
                    }
                    else if (fbloginresult.grantedPermissions != nil){
                        ZKProgressHUD.dismiss()
                        if fbloginresult.grantedPermissions.contains("email"){
                            if AccessToken.current != nil {
                                ZKProgressHUD.dismiss()
                                GraphRequest(graphPath: "me", parameters: ["fields":"email,name,id,picture.type(large),first_name,last_name"])
                                    .start(completionHandler: { (connection, result, error) -> Void in
                                        if error == nil{
                                            let dic = result as! [String:Any]
                                            print("Dic result",dic)
                                            
                                            guard let firstName = dic["first_name"] as? String else {return}
                                            guard let lastName = dic["last_name"] as? String else {return}
                                            guard let email = dic["email"] as? String else {return}
                                            let token = AccessToken.current!.tokenString
                                            print("Token",token)
                                            
                                            ZKProgressHUD.show(NSLocalizedString("Loading", comment: "Loading"))
                                            self.socialLogin(accesstoken: token ?? "", provider: "facebook", googleKey: "")
                                            
                                        }
                                        
                                    })
                            }
                        }
                        
                    }
                }
            }
        }
        
    }
    
    
    
    private func socialLogin(accesstoken : String, provider:String, googleKey : String){
        
        SocialLoginManager.sharedInstance.socailLogin(access_token: accesstoken, provider: provider, google_key: googleKey) { (success, authError, error) in
            if success != nil {
                ZKProgressHUD.dismiss()
                print("Login Succesfull")
                UserData.setaccess_token(success?.accessToken)
                UserData.setUSER_ID(success?.userID)
                AppInstance.instance.getProfile()
                self.performSegue(withIdentifier: "imageSlider", sender: self)
            }
            else if authError != nil {
                ZKProgressHUD.dismiss()
                self.error = authError?.errors?.errorText ?? ""
                self.performSegue(withIdentifier: "ErrorVC", sender: self)
                print(authError?.errors?.errorText)
            }
            else if error != nil {
                ZKProgressHUD.dismiss()
                print("error")
            }
        }
    }
}

@available(iOS 13.0, *)
extension LoginController : GIDSignInDelegate{
    
    func sign(_ signIn: GIDSignIn!, didSignInFor user: GIDGoogleUser!, withError error: Error!) {
        if error == nil {
            let userId = user.userID
            let idToken = user.authentication.accessToken
            print("user auth " ,idToken)
            let token = user.authentication.accessToken ?? ""
            socialLogin(accesstoken: token, provider: "google", googleKey: "AIzaSyDo-tKjkOFkb5yl2n_dxPNJngDdFWNrFMk")
        }
        else {
            ZKProgressHUD.dismiss()
            print(error.localizedDescription)
        }
        
    }
}

@available(iOS 13.0, *)
extension LoginController:ASAuthorizationControllerDelegate{
    func authorizationController(controller: ASAuthorizationController, didCompleteWithAuthorization authorization: ASAuthorization) {
    if let appleIDCredential = authorization.credential as?  ASAuthorizationAppleIDCredential {
        let userIdentifier = appleIDCredential.authorizationCode
    let fullName = appleIDCredential.fullName
    let email = appleIDCredential.email
        let authorizationCode = String(data: appleIDCredential.identityToken!, encoding: .utf8)!
        print("authorizationCode: \(authorizationCode)")
        self.socialLogin(accesstoken: authorizationCode, provider: "apple", googleKey: "")
    
        
    print("User id is \(userIdentifier) \n Full Name is \(String(describing: fullName)) \n Email id is \(String(describing: email))") }
    }
}

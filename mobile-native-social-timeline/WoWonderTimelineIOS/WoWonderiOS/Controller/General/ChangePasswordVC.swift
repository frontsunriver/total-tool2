

import UIKit
import WoWonderTimelineSDK
import ZKProgressHUD
import GoogleMobileAds

class ChangePasswordVC: UIViewController {
    
    @IBOutlet weak var repeatPasswordTextField: UITextField!
    @IBOutlet weak var newPasswordTextFiled: UITextField!
    @IBOutlet weak var currnentPasswordTextFIeld: UITextField!
    @IBOutlet weak var textLbl: UIButton!
    
    var bannerView: GADBannerView!
    override func viewDidLoad() {
        super.viewDidLoad()
        self.setupUI()
        self.currnentPasswordTextFIeld.placeholder = NSLocalizedString("Current Password", comment: "Current Password")
        self.newPasswordTextFiled.placeholder = NSLocalizedString("New Password", comment: "New Password")
        self.repeatPasswordTextField.placeholder = NSLocalizedString("Repeat Password", comment: "Repeat Password")
        self.textLbl.setTitle(NSLocalizedString("If  you forgot your password, you can reset it from here.", comment: "If  you forgot your password, you can reset it from here."), for: .normal)
        
    }
    
    private func setupUI(){

        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
            navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("Change Password", comment: "Change Password")
        
        let save = UIBarButtonItem(title: NSLocalizedString("Save", comment: "Save"), style: .done, target: self, action: #selector(logoutUser))
        self.navigationItem.rightBarButtonItem  = save
        if ControlSettings.shouldShowAddMobBanner{
                         
                         bannerView = GADBannerView(adSize: kGADAdSizeBanner)
                         addBannerViewToView(bannerView)
                         bannerView.adUnitID = ControlSettings.addUnitId
                         bannerView.rootViewController = self
                         bannerView.load(GADRequest())
                        
                     }
    }
    func addBannerViewToView(_ bannerView: GADBannerView) {
            bannerView.translatesAutoresizingMaskIntoConstraints = false
            view.addSubview(bannerView)
            view.addConstraints(
                [NSLayoutConstraint(item: bannerView,
                                    attribute: .bottom,
                                    relatedBy: .equal,
                                    toItem: bottomLayoutGuide,
                                    attribute: .top,
                                    multiplier: 1,
                                    constant: 0),
                 NSLayoutConstraint(item: bannerView,
                                    attribute: .centerX,
                                    relatedBy: .equal,
                                    toItem: view,
                                    attribute: .centerX,
                                    multiplier: 1,
                                    constant: 0)
                ])
        }
    @objc func logoutUser(){
       if self.currnentPasswordTextFIeld.text!.isEmpty {
                 self.view.makeToast("Please enter current password.")
             }else  if self.newPasswordTextFiled.text!.isEmpty {
                 self.view.makeToast("Please enter new password.")
             }else if self.repeatPasswordTextField.text!.isEmpty {
                 self.view.makeToast("Please enter repeat password.")
             }else if self.newPasswordTextFiled.text !=  self.repeatPasswordTextField.text{
                 self.view.makeToast("Password does not match.")
             }else{
                 self.updatePassword(newPass: self.newPasswordTextFiled.text ?? "", currentPass: self.currnentPasswordTextFIeld.text ?? "")
             }
        
    }
    @IBAction func forgotPasswordBtnPressed(_ sender: Any) {
    }
    private func updatePassword(newPass:String,currentPass:String){
        ZKProgressHUD.show()
        
        performUIUpdatesOnMain {
            UpdateUserManager.instance.updatePassword(currentPass: currentPass, newPass: newPass) { (success, authError, error) in
                if success != nil {
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

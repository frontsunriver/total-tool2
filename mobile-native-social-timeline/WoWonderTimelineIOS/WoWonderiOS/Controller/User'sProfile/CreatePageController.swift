

import UIKit
import Toast_Swift
import ZKProgressHUD
import WoWonderTimelineSDK
import GoogleMobileAds


class CreatePageController: UIViewController {

    
    @IBOutlet weak var pageTitleField : RoundTextField!
    @IBOutlet weak var pageURL: RoundTextField!
    @IBOutlet weak var pageCategory: RoundTextField!
    @IBOutlet weak var aboutPage: UITextView!
    
    var delegate : CreatePageDelegate!
    
    let status = Reach().connectionStatus()
    var categoryId = 0
    var categoryName = ""
    var interstitial: GADInterstitial!
    
    override func viewDidLoad() {
        super.viewDidLoad()
//        self.navigationItem.hidesBackButton = true
//        self.navigationController?.navigationBar.isHidden = true
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.pageCategory.inputView = UIView()
        if ControlSettings.shouldShowAddMobBanner{
                        
                        interstitial = GADInterstitial(adUnitID:  ControlSettings.interestialAddUnitId)
                        let request = GADRequest()
                        interstitial.load(request)
                    }

    }
    func CreateAd() -> GADInterstitial {
             let interstitial = GADInterstitial(adUnitID:  ControlSettings.interestialAddUnitId)
             interstitial.load(GADRequest())
             return interstitial
         }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    private func createPage(){
        switch status {
        case .unknown, .offline:
            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            ZKProgressHUD.show()
            CretaPageManager.sharedInstance.cretaPage(pageName: self.pageURL.text!, pageTitle: self.pageTitleField.text!, aboutPage: self.aboutPage.text!, category: self.categoryId) { (success, authError, error) in
                    if success != nil {
                          ZKProgressHUD.dismiss()
                        self.view.makeToast("Page Created")
                        self.delegate.sendPageData(pageData : success!.page_data)
                        self.dismiss(animated: true, completion: nil)
                    }
                    else if authError != nil {
                         ZKProgressHUD.dismiss()
                        if authError?.errors.errorText == "Invalid Page name characters"{
                            self.view.makeToast("Invalid Page URL")
                        }
                        else{
                            self.view.makeToast(authError?.errors.errorText)
                        }
                    }
                    else if error  != nil {
                               ZKProgressHUD.dismiss()
                        print(error?.localizedDescription)

                    }
                }
        }
        
    }
    
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    @IBAction func Save(_ sender: Any) {
         if (self.pageTitleField.text?.isEmpty == true) {
             self.view.makeToast("Enter Page Title")
         }
         else if (self.pageURL.text?.isEmpty == true) {
            self.view.makeToast("Enter Page Url")
         }
        else if (self.aboutPage.text?.isEmpty == true) {
             self.view.makeToast("Enter Page About")
         }
        else if (self.pageCategory.text?.isEmpty == true) {
             self.view.makeToast("Enter Category")
         }
        else {
            self.createPage()
        }
        
    }
    
    @IBAction func SelectCategory(_ sender: Any) {
        self.pageCategory.resignFirstResponder()
    let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
    let vc = storyboard.instantiateViewController(withIdentifier: "CategoryVC") as! GroupCategoryController
        vc.delegate = self
        vc.modalTransitionStyle = .crossDissolve
        vc.modalPresentationStyle = .overCurrentContext
        self.present(vc, animated: true, completion: nil)
        
    }
    
    
}
extension CreatePageController : selectCategoryDelegate {
    func selectCategory(categoryID: Int, categoryName: String) {
        self.categoryId = categoryID
        self.pageCategory.text = categoryName
    }
    
    
}



import UIKit
import ZKProgressHUD
 import WoWonderTimelineSDK


class PageInformationController: UIViewController {
    
    @IBOutlet weak var companyField: RoundTextField!
    @IBOutlet weak var phoneField: RoundTextField!
    @IBOutlet weak var locationField: RoundTextField!
    @IBOutlet weak var aboutField: RoundTextField!
    @IBOutlet weak var websiteField: RoundTextField!
    @IBOutlet weak var pageLabel: UILabel!
    @IBOutlet weak var saveBtn: UIButton!
    
    let status = Reach().connectionStatus()
    
    var pageData : ForwardPageData!
    var page_data = [String:Any]()
    var delegate : EditPageDelegete!
    
    var page_id: String? = nil
    
    override func viewDidLoad() {
       super.viewDidLoad()
        
        
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.pageLabel.text = NSLocalizedString("Update Page Data", comment: "Update Page Data")
        self.saveBtn.setTitle(NSLocalizedString("Save", comment: "Save"), for: .normal)
        
        if let pageId = self.page_data["page_id"] as? String{
            self.page_id = pageId
        }
        if let company = self.page_data["company"] as? String{
            self.companyField.text = company
        }
        if let phone = self.page_data["phone"] as? String{
            self.phoneField.text = phone
        }
        if let location = self.page_data["address"] as? String{
            self.locationField.text = location
        }
        if let webSite = self.page_data["website"] as? String{
            self.websiteField.text = webSite
        }
        if let about = self.page_data["about"] as? String{
            self.aboutField.text = about
        }
        
        self.companyField.placeholder = NSLocalizedString("Company", comment: "Company")
        self.phoneField.placeholder = NSLocalizedString("Phone", comment: "Phone")
        self.locationField.placeholder = NSLocalizedString("Location", comment: "Location")
        self.aboutField.placeholder = NSLocalizedString("About", comment: "About")
        self.websiteField.placeholder = NSLocalizedString("Website", comment: "Website")
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    @IBAction func Save(_ sender: Any) {
        if (self.companyField.text?.isEmpty == true) && (self.phoneField.text?.isEmpty == true) && (self.locationField.text?.isEmpty == true) && (self.websiteField.text?.isEmpty == true) && (self.aboutField.text?.isEmpty == true) {
            self.view.makeToast("Please Enter Data")
        }
        else {
            self.updateDate()
        }
    }
    
    
    
    
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    
//
    private func updateDate() {
        switch status {
         case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
         case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                ZKProgressHUD.show()
                UpdatePageDataManager.sharedInstance.updatePageData(params: [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.pageId : self.page_id ?? "",APIClient.Params.company : self.companyField.text!,APIClient.Params.pagePhone : self.phoneField.text!,APIClient.Params.address : self.locationField.text!,APIClient.Params.website : self.websiteField.text!, APIClient.Params.pageDecription : self.aboutField.text!]) { (success, authError, error) in
                    if success != nil {
                        ZKProgressHUD.dismiss()
                        self.view.makeToast(success?.message)
                        self.page_data["company"] = self.companyField.text!
                        self.page_data["phone"] = self.phoneField.text!
                        self.page_data["address"] = self.locationField.text!
                        self.page_data["website"] = self.websiteField.text!
                        self.page_data["about"] = self.aboutField.text!
                        self.delegate.editPage(pageData: self.page_data)
                    }
                    
                    else if authError != nil {
                        ZKProgressHUD.dismiss()
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        ZKProgressHUD.dismiss()
                        print(error?.localizedDescription)
                    }
                    
                }
            }
            
        }
    }
    

}

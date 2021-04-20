

import UIKit
import Toast_Swift
import ZKProgressHUD
import WoWonderTimelineSDK

class PageGeneralController: UIViewController,selectCategoryDelegate {

    
    @IBOutlet weak var pageTitle: RoundTextField!
    @IBOutlet weak var pageName: RoundTextField!
    @IBOutlet weak var generalLabel: UILabel!
    @IBOutlet weak var saveBtn: UIButton!
    
    var pageData  : ForwardPageData!
    var delegate : EditPageDelegete!
    var page_data = [String:Any]()
    let status = Reach().connectionStatus()

    var page_id: String? = nil
    
    @IBOutlet weak var pageCategory: RoundTextField!
    override func viewDidLoad() {
        super.viewDidLoad()
        
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.generalLabel.text = NSLocalizedString("General", comment: "General")
        self.saveBtn.setTitle(NSLocalizedString("Save", comment: "Save"), for: .normal)
        
        if let pageid = self.page_data["page_id"] as? String{
            self.page_id = pageid
        }
        if let title = self.page_data["page_title"] as? String{
           self.pageTitle.text = title
        }
        if let pageName = self.page_data["page_name"] as? String{
            self.pageName.text = pageName
        }
        if let category = self.page_data["category"] as? String{
            self.pageCategory.text = category
        }
        
        self.pageTitle.placeholder = NSLocalizedString("Page title", comment: "Page title")
        self.pageName.placeholder = NSLocalizedString("Page name", comment: "Page name")
        self.pageCategory.placeholder = NSLocalizedString("Category", comment: "Category")
    }
    
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }

    
    @IBAction func Category(_ sender: UITextField) {
      self.pageCategory.inputView = UIView()
      self.pageCategory.resignFirstResponder()
//         self.actionField.inputView = UIView()
       let Storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
       let vc = Storyboard.instantiateViewController(withIdentifier: "CategoryVC") as! GroupCategoryController
       vc.delegate = self
       vc.modalPresentationStyle = .overCurrentContext
    vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    
    @IBAction func Save(_ sender: Any) {
        if (self.pageTitle.text?.isEmpty == true) && (self.pageName.text?.isEmpty == true) &&  (self.pageCategory.text?.isEmpty == true)  {
        self.view.makeToast("Please Enter Data")
        }
        else {
           
            self.updateDate()
        }
    }
    
    func selectCategory(categoryID: Int, categoryName: String) {
//        self.pageData.cateforyId = "\(categoryID)"
//        print(self.pageData.cateforyId)
        self.page_data["page_category"] = "\(categoryID)"
        self.pageCategory.text = categoryName
        self.pageCategory.resignFirstResponder()
    }
    
    
    private func updateDate() {
        switch status {
         case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
         case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
          ZKProgressHUD.show()
                UpdatePageDataManager.sharedInstance.updatePageData(params: [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.pageId : self.page_id ?? "", APIClient.Params.pageTitle : self.pageTitle.text!, APIClient.Params.pageName : self.pageName.text!, APIClient.Params.pageCategory : self.page_data["page_category"] as? String]) { (success, authError, error) in
                    if success != nil {
                        ZKProgressHUD.dismiss()
                        self.view.makeToast(success?.message)
//                        self.pageData.page_Title = self.pageTitle.text!
//                        self.pageData.page_Name = self.pageName.text!
//                        self.pageData.categoryName = self.pageCategory.text!
//                        print(self.pageData.categoryName)
//                        self.delegate.editPage(pageData: self.pageData)
                        self.page_data["page_title"] = self.pageTitle.text!
                        self.page_data["page_name"] = self.pageName.text!
                        self.page_data["category"] = self.pageCategory.text!
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

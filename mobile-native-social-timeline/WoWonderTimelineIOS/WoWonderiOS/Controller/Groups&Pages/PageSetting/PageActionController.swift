
import UIKit
import Toast_Swift
import ZKProgressHUD
import WoWonderTimelineSDK


class PageActionController: UIViewController,UITextFieldDelegate,CallActionDelegate{

    @IBOutlet weak var actionField: RoundTextField!
    @IBOutlet weak var actionUrlField: RoundTextField!
    @IBOutlet weak var actionLabel: UILabel!
    @IBOutlet weak var saveBtn: UIButton!
    
    
    let Storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
    let status = Reach().connectionStatus()
    
    var pageData : ForwardPageData!
    var delegate : EditPageDelegete!
    var page_data = [String:Any]()

    var pageId: String? = nil
//    var callActionType = ""
//    var callActionUrl = ""
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.actionLabel.text = NSLocalizedString("Action Buttons", comment: "Action Buttons")
        self.saveBtn.setTitle(NSLocalizedString("Save", comment: "Save"), for: .normal)
        
        self.actionField.placeholder = NSLocalizedString("Call to action", comment: "Call to action")
        self.actionUrlField.placeholder = NSLocalizedString("Call to target url", comment: "Call to target url")
        
        
        if let pageid = self.page_data["page_id"] as? String{
            self.pageId = pageid
        }
        if let actionUrl = self.page_data["call_action_type_url"] as? String{
            self.actionUrlField.text = actionUrl
        }
        
        if let action_name = self.page_data["call_action_type_text"] as? String{
            self.actionField.text = action_name
        }
        
        self.actionUrlField.text = self.pageData.callActionUrl
//        self.callActionName()
        self.actionField.inputView = UIView()
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    @IBAction func CallAction(_ sender: Any) {
        self.actionField.inputView = UIView()
        self.actionField.resignFirstResponder()
        let vc = Storyboard.instantiateViewController(withIdentifier: "SetActionVC") as! SetActionController
        vc.modalTransitionStyle = .crossDissolve
        vc.modalPresentationStyle = .overCurrentContext
        vc.delegate = self
        self.present(vc, animated: true, completion: nil)
    }
    
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    @IBAction func Save(_ sender: Any) {
    if (self.actionField.text?.isEmpty == true) && (self.actionUrlField.text?.isEmpty == true){
            self.view.makeToast("Please Enter Data")
        }
        else {
        self.updateDate()
        }
    }
    
    func sendCallAction(callActionId: String, callActionName: String) {
        self.actionField.resignFirstResponder()
        self.page_data["call_action_type"] = callActionId
        self.actionField.text = callActionName
    }
    
    

    //
        private func updateDate() {
            switch status {
             case .unknown, .offline:
                self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
             case .online(.wwan),.online(.wiFi):
                performUIUpdatesOnMain {
                    ZKProgressHUD.show()
                    UpdatePageDataManager.sharedInstance.updatePageData(params: [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.pageId : self.pageId, APIClient.Params.callActionUrl : self.actionUrlField.text!, APIClient.Params.callActionType :  self.page_data["call_action_type"]]) { (success, authError, error) in
                        if success != nil {
                            ZKProgressHUD.dismiss()
                            self.view.makeToast(success?.message)
                            self.page_data["call_action_type_url"] = self.actionField.text!
//                            self.pageData.callActionUrl = self.actionField.text!
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
    
    private func callActionName() {
        if (self.pageData.callActionType) == "1"{
            self.actionField.text = "Read more"
         }
         else if (self.pageData.callActionType) == "2"{
            self.actionField.text = "Shop now"
         }
         else if (self.pageData.callActionType) == "3"{
            self.actionField.text = "View now"
            
        }
         else if (self.pageData.callActionType) == "4"{
            self.actionField.text = "Visit now"
       }
         else if (self.pageData.callActionType) == "5"{
            self.actionField.text = "Book now"
         }
         else if (self.pageData.callActionType) == "6"{
            self.actionField.text = "Learn more"
         }
         else if (self.pageData.callActionType) == "7"{
            self.actionField.text = "Play now"
         }
         else if (self.pageData.callActionType) == "8"{
            self.actionField.text = "Bet now"
         }
         else if (self.pageData.callActionType) == "9"{
            self.actionField.text = "Donate"

         }
        
         else if (self.pageData.callActionType) == "10"{
            self.actionField.text = "Apply here"

         }
         else if (self.pageData.callActionType) == "11"{
            self.actionField.text = "Quote here"
         }
         else if (self.pageData.callActionType) == "12"{
            self.actionField.text = "Order now"
         }
         else if (self.pageData.callActionType) == "13"{
            self.actionField.text = "Book tickets"
         }
         else if (self.pageData.callActionType) == "14"{
            self.actionField.text = "Enroll now"
         }
         else if (self.pageData.callActionType) == "15"{
            self.actionField.text = "Find a card"
         }
         else if (self.pageData.callActionType) == "16"{
            self.actionField.text = "Get a quote"

         }
         else if (self.pageData.callActionType) == "17"{
            self.actionField.text = "Get tickets"
         }
         else if (self.pageData.callActionType) == "18"{
            self.actionField.text = "Locate a dealer"
         }
         else if (self.pageData.callActionType) == "19"{
            self.actionField.text = "Order online"
         }
         else if (self.pageData.callActionType) == "20"{
            self.actionField.text = "Preorder now"
         }
         else if (self.pageData.callActionType) == "21"{
            self.actionField.text = "Schedule now"
         }
         else if (self.pageData.callActionType) == "22"{
            self.actionField.text = "Signup now"
         }
         else if (self.pageData.callActionType) == "23"{
            self.actionField.text = "Subscribe now"
         }
         else if (self.pageData.callActionType) == "24"{
            self.actionField.text = "Register now"
         }
    }
}

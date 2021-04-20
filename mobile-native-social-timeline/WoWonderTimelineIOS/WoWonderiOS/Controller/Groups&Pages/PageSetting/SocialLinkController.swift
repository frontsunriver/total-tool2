

import UIKit
import Toast_Swift
import WoWonderTimelineSDK
import ZKProgressHUD

class SocialLinkController: UIViewController {
    
    
    
    @IBOutlet weak var linkLabel: UILabel!
    @IBOutlet weak var facebookLink: RoundTextField!
    @IBOutlet weak var twitterLink: RoundTextField!
    @IBOutlet weak var instagramLink: RoundTextField!
    @IBOutlet weak var vkLink: RoundTextField!
    @IBOutlet weak var linkedinLink: RoundTextField!
    @IBOutlet weak var youtubeLink: RoundTextField!

    
    var pageData : ForwardPageData!
    var delegate : EditPageDelegete!
    var page_data = [String:Any]()
    
    let status = Reach().connectionStatus()

    var pageId: String? = nil
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.linkLabel.text = NSLocalizedString("Social Links", comment: "Social Links")
        
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        
        if let pageid = self.page_data["page_id"] as? String{
            self.pageId = pageid
        }
        if let facebook = self.page_data["facebook"] as? String{
            self.facebookLink.text = facebook
        }
        if let twitter = self.page_data["twitter"] as? String{
            self.twitterLink.text = twitter
        }
        if let instagram = self.page_data["instgram"] as? String{
            self.instagramLink.text = instagram
        }
        if let vk = self.page_data["vk"] as? String{
            self.vkLink.text = vk
        }
        if let linkedin = self.page_data["linkedin"] as? String{
            self.linkedinLink.text = linkedin
        }
        if let youtube = self.page_data["youtube"] as? String{
           self.youtubeLink.text = youtube
        }

    }
    
    /// Network Connectivity
     @objc func networkStatusChanged(_ notification: Notification) {
         if let userInfo = notification.userInfo {
             let status = userInfo["Status"] as! String
             print("Status",status)
         }
     }
    

    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)

    }
    

    @IBAction func Save(_ sender: Any) {
        if (self.facebookLink.text?.isEmpty == true) && (self.twitterLink.text?.isEmpty == true) && (self.linkedinLink.text?.isEmpty == true) && (self.instagramLink.text?.isEmpty == true) && (self.vkLink.text?.isEmpty == true) && (self.youtubeLink.text?.isEmpty == true){
            self.view.makeToast("Please Enter Data")
        }
        else {
            updateDate()
        }
    
    }
    
    
    //
    private func updateDate() {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                ZKProgressHUD.show()
                UpdatePageDataManager.sharedInstance.updatePageData(params: [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.pageId : self.pageData.pageId, APIClient.Params.facebook : self.facebookLink.text!, APIClient.Params.youtube : self.youtubeLink.text!, APIClient.Params.twitter : self.twitterLink.text!, APIClient.Params.instgram : self.instagramLink.text!, APIClient.Params.linkedin : self.linkedinLink.text!,  APIClient.Params.vk : self.vkLink.text!]) { (success, authError, error) in
                if success != nil {
                    ZKProgressHUD.dismiss()
                    self.view.makeToast(success?.message)
//                    self.pageData.facebook = self.facebookLink.text!
//                    self.pageData.youtube = self.youtubeLink.text!
//                    self.pageData.linkdin = self.linkedinLink.text!
//                    self.pageData.vk = self.vkLink.text!
//                    self.pageData.twitter = self.twitterLink.text!
//                    self.pageData.instagrm = self.instagramLink.text!
//                    self.delegate.editPage(pageData: self.pageData)
                    self.page_data["facebook"] = self.facebookLink.text!
                    self.page_data["youtube"] = self.youtubeLink.text!
                    self.page_data["linkedin"] = self.linkedinLink.text!
                    self.page_data["vk"] = self.vkLink.text!
                    self.page_data["twitter"] = self.twitterLink.text!
                    self.page_data["instgram"] = self.instagramLink.text!
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

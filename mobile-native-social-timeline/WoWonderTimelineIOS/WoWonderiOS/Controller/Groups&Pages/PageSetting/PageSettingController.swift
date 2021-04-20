

import UIKit
import WoWonderTimelineSDK

class PageSettingController: UIViewController,EditPageDelegete,DeletePageDelegate {
    
    
    @IBOutlet weak var generalBtn: UIButton!
    @IBOutlet weak var pageInfoBtn: UIButton!
    @IBOutlet weak var actionButton: UIButton!
    @IBOutlet weak var linkBtn: UIButton!
    @IBOutlet weak var offerBtn: UIButton!
    @IBOutlet weak var deleteBtn: UIButton!
    
    var pageData : ForwardPageData!
    var delegate : EditPageDelegete!
    var deleteDelegate : DeletePageDelegate!
    var page_Data = [String:Any]()
    
    var page_id: String? = nil
    
    let Storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)

    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.navigationItem.title = NSLocalizedString("Setting", comment: "Setting")
        
         self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        
        if let id = self.page_Data["page_id"] as? String{
            self.page_id = id
        }
        


    }
    override func viewWillAppear(_ animated: Bool) {
        self.generalBtn.setTitle("\(" ")\(NSLocalizedString("General", comment: "General"))", for: .normal)
        self.pageInfoBtn.setTitle("\(" ")\(NSLocalizedString("Page Information", comment: "Page Information"))", for: .normal)
        self.deleteBtn.setTitle("\(" ")\(NSLocalizedString("Delete a Page", comment: "Delete a Page"))", for: .normal)
        self.offerBtn.setTitle("\(" ")\(NSLocalizedString("Offer a Job", comment: "Offer a Job"))", for: .normal)
        self.linkBtn.setTitle("\(" ")\(NSLocalizedString("Social Links", comment: "Social Links"))", for: .normal)
        self.actionButton.setTitle("\(" ")\(NSLocalizedString("Actions Buttons", comment: "Actions Buttons"))", for: .normal)
    }
    
    @IBAction func SettingBtn(_ sender: UIButton) {
        switch sender.tag {
        case 0:
        let vc = Storyboard.instantiateViewController(withIdentifier: "PageGeneralVC") as! PageGeneralController
        vc.pageData = self.pageData
        vc.page_data = self.page_Data
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        vc.delegate = self
        self.present(vc, animated: true, completion: nil)
        case 1:
        let vc = Storyboard.instantiateViewController(withIdentifier: "PageInfoVC") as! PageInformationController
        vc.pageData = self.pageData
        vc.page_data = self.page_Data
        vc.delegate = self
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.present(vc, animated: true, completion: nil)
        case 2:
        let vc = Storyboard.instantiateViewController(withIdentifier: "PageActionVC") as! PageActionController
        vc.pageData = self.pageData
        vc.delegate = self
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.present(vc, animated: true, completion: nil)
        case 3:
        let vc = Storyboard.instantiateViewController(withIdentifier: "SocialLinkVC") as! SocialLinkController
        vc.pageData = self.pageData
        vc.page_data = self.page_Data
        vc.delegate = self
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.present(vc, animated: true, completion: nil)
        case 4:
        let Storyboard = UIStoryboard(name: "Jobs", bundle: nil)
        let vc = Storyboard.instantiateViewController(withIdentifier: "JobsVC") as! JobsController
        vc.pageId = self.page_id ?? ""
        self.navigationController?.pushViewController(vc, animated: true)

        case 5:
        let vc = Storyboard.instantiateViewController(withIdentifier: "PageDeleteVC") as! PageDeleteController
        vc.pageId = self.page_id ?? ""
        vc.delegate = self
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.present(vc, animated: true, completion: nil)
            
        default:
            print("Nothing")
        }
        
    }
    
    @IBAction func Back(_ sender: Any) {
        self.navigationController?.popViewController(animated: true)
    }
    
      func editPage(pageData: [String:Any]) {
//        self.pageData = pageData
        self.page_Data = pageData
        self.delegate.editPage(pageData: pageData)
    }
    func deletePage(pageId: String) {
        self.deleteDelegate.deletePage(pageId: pageId)
        self.navigationController?.popViewController(animated: true)
    }
    
    
    
  

}

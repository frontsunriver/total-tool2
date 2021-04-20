

import UIKit
import WoWonderTimelineSDK


class GroupSettingController: UIViewController,DeleteGroupDelegate,EditGroupDataDelegate {
  
    
    @IBOutlet weak var settingLabel: UILabel!
    
    @IBOutlet weak var generalBtn: UIButton!
    @IBOutlet weak var privacyBtn: UIButton!
    @IBOutlet weak var membersBtn: UIButton!
    @IBOutlet weak var deleteBtn: UIButton!
    
    var group_Id = ""
    var groupTitle = ""
    var groupName = ""
    var categoryId = ""
    var categoryName = ""
    var privacy = "0"
    var about = ""
    
    var pageData : ForwardPageData!
    
    var delegate : DeleteGroupDelegate!

    override func viewDidLoad() {
        super.viewDidLoad()
       print(self.group_Id)
        print (self.categoryName)
        self.navigationItem.hidesBackButton = true
        self.navigationController?.navigationBar.isHidden = true
        self.navigationItem.largeTitleDisplayMode = .never
        self.settingLabel.text = NSLocalizedString("Settings", comment: "Settings")
        self.privacyBtn.setTitle("\("   ")\(NSLocalizedString("Privacy", comment: "Privacy"))", for: .normal)
        self.generalBtn.setTitle("\("   ")\(NSLocalizedString("General", comment: "General"))", for: .normal)
        self.membersBtn.setTitle("\("   ")\(NSLocalizedString("Memebers", comment: "Memebers"))", for: .normal)
        self.deleteBtn.setTitle("\("   ")\(NSLocalizedString("Delete Group", comment: "Delete Group"))", for: .normal)
       
    }
    
    let Storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)

    
    @IBAction func Settings(_ sender: UIButton) {
        
        switch sender.tag{
        case 0:
            let vc = Storyboard.instantiateViewController(withIdentifier: "GeneralVC") as! GeneralController
            vc.group_Name = self.groupName
            vc.group_Title = self.groupTitle
            vc.categoryId = self.categoryId
            vc.categoryName = self.categoryName
            vc.about = self.about
            vc.group_Id = self.group_Id
            vc.delegate = self
            vc.modalPresentationStyle = .fullScreen
            vc.modalTransitionStyle = .coverVertical
            
            self.present(vc, animated: true, completion: nil)
        case 1:
            let vc = Storyboard.instantiateViewController(withIdentifier: "PrivacyVC") as! GroupPrivacyController
            vc.groupId = self.group_Id
            vc.privacy = self.privacy
            vc.modalPresentationStyle = .fullScreen
            vc.modalTransitionStyle = .coverVertical
            self.present(vc, animated: true, completion: nil)
        case 2:
            let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "GroupMemberVC") as! GroupMembersController
            vc.groupId = self.group_Id
            self.navigationController?.pushViewController(vc, animated: true)
        
        
        case 3:
            let vc = Storyboard.instantiateViewController(withIdentifier: "DeleteVC") as! DeleteGroupController
            vc.groupId = self.group_Id
            vc.modalPresentationStyle = .fullScreen
            vc.modalTransitionStyle = .coverVertical
            vc.delegate = self
            self.present(vc, animated: true, completion: nil)
        default:
            print("Nothing")
        }
        
    }
    
    func deleteGroup(groupId: String) {
        self.delegate.deleteGroup(groupId: groupId)
        self.navigationController?.popViewController(animated: true)
    
       }
    
    func editGroup(groupName: String, groupTitle: String, about: String, Category: String, CategoryId: String) {
        self.groupName = groupName
        self.groupTitle = groupTitle
        self.categoryName = Category
        self.categoryId = CategoryId
        self.about = about
     }
     
    
    @IBAction func Back(_ sender: Any) {
         self.navigationController?.popViewController(animated: true)
    }


}

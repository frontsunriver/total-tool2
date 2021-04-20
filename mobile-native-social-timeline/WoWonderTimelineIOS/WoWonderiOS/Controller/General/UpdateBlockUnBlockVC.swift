
import UIKit
import ZKProgressHUD
import WoWonderTimelineSDK
class UpdateBlockUnBlockVC: UIViewController {
    
    @IBOutlet weak var changeIcon: UIImageView!
    @IBOutlet weak var blockTitleLabel: UILabel!
    @IBOutlet weak var blockView: Roundimage!
    @IBOutlet weak var userName: UILabel!
    @IBOutlet weak var profileImage: Roundimage!
    var object = [String:Any]()
    var delegate : block_unblockDelegate?
    
    var status:String? = "un-block"
    override func viewDidLoad() {
        super.viewDidLoad()
        self.setupUI()
        
        
    }
    private func setupUI(){
        if let name = object["name"] as? String{
            self.userName.text = name
        }
        if let avatar = object["avatar"] as? String{
            let url = URL(string: avatar)
            self.profileImage.kf.setImage(with: url)
        }
        let gesture = UITapGestureRecognizer(target: self, action:  #selector(self.checkAction))
        self.blockView.addGestureRecognizer(gesture)
        
        let gesture1 = UITapGestureRecognizer(target: self, action:  #selector(self.dismissTapped))
        self.view.addGestureRecognizer(gesture1)
    }
    
    @objc func checkAction(sender : UITapGestureRecognizer) {
        self.unblockUser(type: status ?? "")
        
        if self.status == "un-block"{
              self.blockView.backgroundColor = UIColor.hexStringToUIColor(hex: "984243")
              self.blockTitleLabel.text = NSLocalizedString("Block", comment: "Block")
              self.status = "block"
         }else{
             self.blockView.backgroundColor = UIColor.hexStringToUIColor(hex: "AAAAAA")
             self.blockTitleLabel.text = NSLocalizedString("Unblock", comment: "Unblock")
             self.status = "un-block"
         }
    }
    @objc func dismissTapped(sender : UITapGestureRecognizer) {
        self.dismiss(animated: true, completion: nil)
    }
    
    
    private func unblockUser(type:String){
        var userID = ""
        if let userId = self.object["user_id"] as? String{
            userID = userId
        }
        print(userID)
        print(type)
        performUIUpdatesOnMain {
            Block_UserManager.sharedInstance.blockUser(user_Id: userID, blockUser: type) { (success, authError, error) in
                if success != nil {
                    self.view.makeToast(success?.block_status)
                    self.dismiss(animated: true) {
                        self.delegate?.unblock(user_id: userID)
                    }
                }
                else if authError != nil {
                    ZKProgressHUD.dismiss()
                    self.view.makeToast(authError?.errors.errorText)
                    self.showAlert(title: "", message: (authError?.errors.errorText)!)
                }
                else if error  != nil {
                    ZKProgressHUD.dismiss()
                    print(error?.localizedDescription)
                    
                }
            }
        }
        
    }
    
}

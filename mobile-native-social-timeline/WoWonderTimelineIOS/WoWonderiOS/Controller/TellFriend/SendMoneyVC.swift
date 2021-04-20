

import UIKit
import XLPagerTabStrip
import ZKProgressHUD
import WoWonderTimelineSDK
class SendMoneyVC: UIViewController {
    
    @IBOutlet weak var amountLabel: UILabel!
    @IBOutlet weak var amountTextfield: UITextField!
    @IBOutlet weak var addUserBtn: UIButton!
    @IBOutlet weak var sendMoneyLbl: UILabel!
    @IBOutlet weak var moneyDescLbl: UILabel!
    @IBOutlet weak var currentBalanceLbl: UILabel!
    @IBOutlet weak var continueBtn: RoundButton!
    
    
    var userId:String? = ""
    override func viewDidLoad() {
        super.viewDidLoad()
        self.sendMoneyLbl.text = NSLocalizedString("Send money to friends", comment: "Send money to friends")
        self.moneyDescLbl.text = NSLocalizedString("you can send money to your friends, acquaintances or anyone", comment: "you can send money to your friends, acquaintances or anyone")
        self.currentBalanceLbl.text = NSLocalizedString("Current Balance", comment: "Current Balance")
        self.continueBtn.setTitle(NSLocalizedString("CONTINUE", comment: "CONTINUE"), for: .normal)
        self.amountTextfield.placeholder = NSLocalizedString("Amount", comment: "Amount")
        self.addUserBtn.setTitle(NSLocalizedString("Email or Username", comment: "Email or Username"), for: .normal)
        
    }
    
    @IBAction func continuePressed(_ sender: Any) {
        if self.amountTextfield.text!.isEmpty {
                        self.view.makeToast("Please enter some amount.")
        }else if self.userId == "" {
                        self.view.makeToast("Please Select one atleast one user.")
        }else{
            self.continuePress(userID: self.userId ?? "", type: "send")
        }
    }
    @IBAction func addUserPressed(_ sender: Any) {
                let storyboard = UIStoryboard(name: "TellFriend", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "SelectUserVC") as! SelectUserVC
        vc.delegate = self
        self.navigationController?.pushViewController(vc, animated: true)
                
    }
    
    private func continuePress(userID:String,type:String){
        let amount = Int(self.amountTextfield.text ?? "")
        
        ZKProgressHUD.show()
        
        performUIUpdatesOnMain {
            AddMoneyManager.instance.addMoney(amount: amount!, userID: userID, type: type) { (success, authError, error) in
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
extension SendMoneyVC:IndicatorInfoProvider{
    func indicatorInfo(for pagerTabStripController: PagerTabStripViewController) -> IndicatorInfo {
        return IndicatorInfo(title: "Send Money")
    }
}
extension SendMoneyVC:didSelectUserDelegate{
    func didSelectUser(userID: String, username: String, index: Int) {
        self.userId = userID
        self.addUserBtn.setTitle(username, for: .normal)
    }
    
    
}

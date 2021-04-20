
import UIKit
import Toast_Swift
import WoWonderTimelineSDK
import ZKProgressHUD

class SendMoneyController: UIViewController,selecteUSerDelegate{


    @IBOutlet weak var balance: UILabel!
    @IBOutlet weak var amountField: RoundTextField!
    @IBOutlet weak var emailField: RoundTextField!
    @IBOutlet weak var sendMoneyLbl: UILabel!
    @IBOutlet weak var moneyDescLbl: UILabel!
    @IBOutlet weak var currentBalanceLbl: UILabel!
    @IBOutlet weak var continueBtn: RoundButton!
    
    
    var myBalance: String? = nil
    var userId: String? = nil
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.sendMoneyLbl.text = NSLocalizedString("Send money to friends", comment: "Send money to friends")
        self.moneyDescLbl.text = NSLocalizedString("you can send money to your friends, acquaintances or anyone", comment: "you can send money to your friends, acquaintances or anyone")
        self.currentBalanceLbl.text = NSLocalizedString("Current Balance", comment: "Current Balance")
        self.continueBtn.setTitle(NSLocalizedString("CONTINUE", comment: "CONTINUE"), for: .normal)
        self.amountField.placeholder = NSLocalizedString("Amount", comment: "Amount")
        self.emailField.placeholder = NSLocalizedString("Email or Username", comment: "Email or Username")
    }
    
    override func viewWillAppear(_ animated: Bool) {
        self.balance.text = UserData.getWallet()
    }
    
    
    @IBAction func didTapTextField(_ sender: Any) {
        self.emailField.resignFirstResponder()
        self.emailField.inputView = UIView()
        let storyBoard = UIStoryboard(name: "Main", bundle: nil)
        let vc = storyBoard.instantiateViewController(withIdentifier: "SelectUserVC") as! SelectUserController
        vc.modalPresentationStyle = .fullScreen
        vc.modalTransitionStyle = .coverVertical
        vc.delegate = self
        self.present(vc, animated: true, completion: nil)
    }
    
    private func sendMoney(user_id: String){
        WalletManager.sharedInstance.sendMoney(user_id: user_id, amount: self.amountField.text!) { (success, authError, error) in
            if success != nil {
                ZKProgressHUD.dismiss()
                self.view.makeToast(success?.message)
            }
            else if authError != nil{
                ZKProgressHUD.dismiss()
                self.view.makeToast(authError?.errors.errorText)
            }
            else if error != nil {
                ZKProgressHUD.dismiss()
                self.view.makeToast(error?.localizedDescription)
            }
        }
    }

    @IBAction func Continue(_ sender: Any) {
        if self.amountField.text?.isEmpty == true{
            self.view.makeToast("Please Enter Amount")
        }
        else if (self.emailField.text?.isEmpty == true){
            self.view.makeToast("Please Enter name or email")
        }
        else{
            ZKProgressHUD.show(NSLocalizedString("Loading", comment: "Loading"))
            self.sendMoney(user_id: self.userId ?? "")
        }
    }
    
    func selectUser(name: String, user_id: String) {
        self.emailField.text = name
        self.userId = user_id
    }
}

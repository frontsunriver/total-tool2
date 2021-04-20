

import UIKit
import WoWonderTimelineSDK
import Braintree
import ZKProgressHUD
class AddFundController: UIViewController {

    @IBOutlet weak var balance: UILabel!
    @IBOutlet weak var amountField: RoundTextField!
    @IBOutlet weak var sendMoneyLbl: UILabel!
    @IBOutlet weak var moneyDescLbl: UILabel!
    @IBOutlet weak var currentBalanceLbl: UILabel!
    @IBOutlet weak var continueBtn: RoundButton!
    
    var braintree: BTAPIClient?
    var braintreeClient: BTAPIClient?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.balance.text = UserData.getWallet()
        self.sendMoneyLbl.text = NSLocalizedString("Replenish my balance", comment: "Replenish my balance")

        self.currentBalanceLbl.text = NSLocalizedString("Current Balance", comment: "Current Balance")
        self.continueBtn.setTitle(NSLocalizedString("CONTINUE", comment: "CONTINUE"), for: .normal)
        self.amountField.placeholder = NSLocalizedString("Amount", comment: "Amount")
      

    }
    override func viewWillAppear(_ animated: Bool) {
           super.viewWillAppear(animated)

       }
    
    
    
    private func topUpMoney(user_id: String){
        WalletManager.sharedInstance.topUpMoney(user_id: user_id, amount: self.amountField.text!) { (success, authError, error) in
            if success != nil {
//                ZKProgressHUD.dismiss()
                self.view.makeToast(success?.message)
            }
            else if authError != nil{
//                ZKProgressHUD.dismiss()
                self.view.makeToast(authError?.errors.errorText)
            }
            else if error != nil {
//                ZKProgressHUD.dismiss()
                self.view.makeToast(error?.localizedDescription)
            }
        }
    }
    
    
    @IBAction func Contnue(_ sender: Any) {
        if self.amountField.text?.isEmpty == true{
            self.view.makeToast(NSLocalizedString("Enter Amount", comment: "Enter Amount"))
        }
        else{
            self.startCheckout()
        }
    }
    
    
    func startCheckout() {
              // Example: Initialize BTAPIClient, if you haven't already
              braintreeClient = BTAPIClient(authorization: ControlSettings.paypalAuthorizationToken)!
              let payPalDriver = BTPayPalDriver(apiClient: braintreeClient!)
              payPalDriver.viewControllerPresentingDelegate = self
              payPalDriver.appSwitchDelegate = self // Optional
              
              // Specify the transaction amount here. "2.32" is used in this example.
           let request = BTPayPalRequest(amount: self.amountField.text ?? "")
              request.currencyCode = "USD" // Optional; see BTPayPalRequest.h for more options
              
              payPalDriver.requestOneTimePayment(request) { (tokenizedPayPalAccount, error) in
                  if let tokenizedPayPalAccount = tokenizedPayPalAccount {
                      print("Got a nonce: \(tokenizedPayPalAccount.nonce)")
                      
                      let email = tokenizedPayPalAccount.email
                      let firstName = tokenizedPayPalAccount.firstName
                      let lastName = tokenizedPayPalAccount.lastName
                      let phone = tokenizedPayPalAccount.phone
                      let billingAddress = tokenizedPayPalAccount.billingAddress
                      let shippingAddress = tokenizedPayPalAccount.shippingAddress
                     self.topUpMoney(user_id: UserData.getUSER_ID()!)
                  } else if let error = error {
                      print("error = \(error.localizedDescription ?? "")")
                  } else {
                      print("error = \(error?.localizedDescription ?? "")")
                      
                  }
              }
          }
}

extension AddFundController:BTAppSwitchDelegate, BTViewControllerPresentingDelegate{
    func appSwitcherWillPerformAppSwitch(_ appSwitcher: Any) {
        ZKProgressHUD.show()
    }
    
    func appSwitcher(_ appSwitcher: Any, didPerformSwitchTo target: BTAppSwitchTarget) {
        print("Switched")
        
    }
    
    func appSwitcherWillProcessPaymentInfo(_ appSwitcher: Any) {
        ZKProgressHUD.dismiss()
      print("Switched")
    }
    
    func paymentDriver(_ driver: Any, requestsPresentationOf viewController: UIViewController) {
        viewController.present(viewController, animated: true, completion: nil)
        
        
    }
    
    func paymentDriver(_ driver: Any, requestsDismissalOf viewController: UIViewController) {
        viewController.dismiss(animated: true, completion: nil)
        
    }
    
}

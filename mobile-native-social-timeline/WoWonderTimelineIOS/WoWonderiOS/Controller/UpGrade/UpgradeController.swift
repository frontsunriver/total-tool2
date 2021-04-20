
import UIKit
import WoWonderTimelineSDK
import Braintree
import ZKProgressHUD
class UpgradeController: UIViewController {
    
    
    private var upgardeArray = [[String:Any]]()
    var selectedindex: Int? = nil
    var braintree: BTAPIClient?
          var braintreeClient: BTAPIClient?
   
    
    @IBOutlet weak var collectionView: UICollectionView!
    @IBOutlet weak var proLabel: UILabel!
    @IBOutlet weak var cancelBtn: UIButton!
    @IBOutlet weak var textLabel: UILabel!
    @IBOutlet weak var featuredLabel: UILabel!
    @IBOutlet weak var seeProfileLbl:  UILabel!
    @IBOutlet weak var pagePromoLbl:  UILabel!
    @IBOutlet weak var showHideLbl:  UILabel!
    @IBOutlet weak var verifiedLbl:  UILabel!
    @IBOutlet weak var postPromotionLbl:  UILabel!
    @IBOutlet weak var planeLabel: UILabel!
    override func viewDidLoad() {
        super.viewDidLoad()
        let pro1 = ["main_image":"star","price":"3","validity":"\(NSLocalizedString("Per Week", comment: "Per Week"))","pro_type":NSLocalizedString("STAR", comment: "STAR"),"feature_member":NSLocalizedString("Featured member", comment: "Featured member"),"profile_visitor":NSLocalizedString("See profile visitors", comment: "See profile visitors"),"last_seen":NSLocalizedString("Show / Hide last seen", comment: "Show / Hide last seen"),"badge":NSLocalizedString("Verified badge", comment: "Verified badge"),"boost_Post":NSLocalizedString("Posts promotion", comment: "Posts promotion"),"boost_Page":NSLocalizedString("Pages promotions", comment: "Pages promotions"),"discount":NSLocalizedString("Discount", comment: "Discount"),"BtnColor":"#4D7737","BtnTxt":"","pagePro_image":"cross","discount_image":"cross"]
        let pro2 = ["main_image":"flame","price":"8","validity":NSLocalizedString("Per Month", comment: "Per Month"),"pro_type":"HOT","feature_member":NSLocalizedString("Featured member", comment: "Featured member"),"profile_visitor":NSLocalizedString("See profile visitors", comment: "See profile visitors"),"last_seen":NSLocalizedString("Show / Hide last seen", comment: "Show / Hide last seen"),"badge":NSLocalizedString("Verified badge", comment: "Verified badge"),"boost_Post":NSLocalizedString("Boost upto 5 posts", comment: "Boost upto 5 posts"),"boost_Page":NSLocalizedString("Boost up to 5 Pages", comment: "Boost up to 5 Pages"),"discount":"10%","BtnColor":"#F9B340","BtnTxt":"","pagePro_image":"check","discount_image":"check"]
        let pro3 = ["main_image":"thunder","price":"89","validity":NSLocalizedString("Per Year", comment: "Per Year"),"pro_type":"ULTIMA","feature_member":NSLocalizedString("Featured member", comment: "Featured member"),"profile_visitor":NSLocalizedString("See profile visitors", comment: "See profile visitors"),"last_seen":NSLocalizedString("Show / Hide last seen", comment: "Show / Hide last seen"),"badge":NSLocalizedString("Verified badge", comment: "Verified badge"),"boost_Post":NSLocalizedString("Boost upto 20 posts", comment: "Boost upto 20 posts"),"boost_Page":NSLocalizedString("Boost up to 20 Pages", comment: "Boost up to 20 Pages"),"discount":"20%","BtnColor":"#E13C4B","BtnTxt":"","pagePro_image":"check","discount_image":"check"]
        let pro4 = ["main_image":"rocket","price":"259","validity":NSLocalizedString("life time", comment: "life time"),"pro_type":"VIP","feature_member":NSLocalizedString("Featured member", comment: "Featured member"),"profile_visitor":NSLocalizedString("See profile visitors", comment: "See profile visitors"),"last_seen":NSLocalizedString("Show / Hide last seen", comment: "Show / Hide last seen"),"badge":NSLocalizedString("Verified badge", comment: "Verified badge"),"boost_Post":NSLocalizedString("Boost upto 60 posts", comment: "Boost upto 60 posts"),"boost_Page":NSLocalizedString("Boost up to 60 Pages", comment: "Boost up to 60 Pages"),"discount":"60%","BtnColor":"#3F4BBA","BtnTxt":"","pagePro_image":"check","discount_image":"check"]
        
        self.upgardeArray.append(pro1)
        self.upgardeArray.append(pro2)
        self.upgardeArray.append(pro3)
        self.upgardeArray.append(pro4)
   
    }
    
    override func viewWillAppear(_ animated: Bool) {
        self.proLabel.text = NSLocalizedString("GoPro", comment: "GoPro")
        self.cancelBtn.setTitle(NSLocalizedString("Cancel", comment: "Cancel"), for: .normal)
        self.textLabel.text = NSLocalizedString("Pro Features give you Complete control over your Profile", comment: "Pro Features give you Complete control over your Profile")
        self.featuredLabel.text = NSLocalizedString("Featured member", comment: "Featured member")
        self.seeProfileLbl.text = NSLocalizedString("See profile visitors", comment: "See profile visitors")
        self.pagePromoLbl.text = NSLocalizedString("Pages promotions", comment: "Pages promotions")
        self.showHideLbl.text = NSLocalizedString("Show / Hide last seen", comment: "Show / Hide last seen")
        self.verifiedLbl.text = NSLocalizedString("Verified badge", comment: "Verified badge")
        self.postPromotionLbl.text = NSLocalizedString("Posts promotion", comment: "Posts promotion")
        self.planeLabel.text = NSLocalizedString("Pick Your Plan", comment: "Pick Your Plan")
    }
    
    private func userUpGrade(){
        UpgardeUserManager.sharedInstance.upgradeUser(type: self.selectedindex ?? 0) { (success, authError, error) in
            if success != nil{
//                self.view.makeToast(success?.message_data)
                self.dismiss(animated: true, completion: nil)
            }
            else if authError != nil{
                self.view.makeToast(authError?.errors.errorText)
            }
            else if error != nil {
                self.view.makeToast(error?.localizedDescription)
            }
        }
    }
    

    @IBAction func Cancel(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
}

extension UpgradeController : UICollectionViewDelegate,UICollectionViewDataSource,UICollectionViewDelegateFlowLayout{
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        self.upgardeArray.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "UpgradeUser", for: indexPath) as! UpgradeUserCell
        let index = self.upgardeArray[indexPath.row]
        if let price = index["price"] as? String{
            cell.priceLabel.text = "\("$")\(price)"
        }
        if let validity = index["validity"] as? String{
            cell.validityLabel.text = validity
        }
        if let type = index["pro_type"] as? String{
            cell.proType.text = type
        }
        if let post = index["boost_Post"] as? String{
            cell.postPromotionLabel.text = post
        }
        if let page = index["boost_Page"] as? String{
            cell.pagePromotion.text = page
        }
        if let color = index["BtnColor"] as? String{
            cell.upgradeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: color)
            cell.priceLabel.textColor = UIColor.hexStringToUIColor(hex: color)
            cell.proType.textColor = UIColor.hexStringToUIColor(hex: color)
        }
        if let image = index["main_image"] as? String{
            if image == "star"{
                cell.mainImage.image = #imageLiteral(resourceName: "StarGreen")
            }
            else if image == "flame"{
                cell.mainImage.image = #imageLiteral(resourceName: "fire-symbol")
            }
            else if image == "thunder"{
                cell.mainImage.image = #imageLiteral(resourceName: "Shape-4")
            }
            else if image == "rocket"{
                cell.mainImage.image = #imageLiteral(resourceName: "small-rocket-ship-silhouette")
            }
        }
        
        if let discountImage = index["discount_image"] as? String{
            if discountImage == "cross"{
                cell.pageImage.image = #imageLiteral(resourceName: "cancel")
                cell.promotionImage.image = #imageLiteral(resourceName: "cancel")
                cell.discountImage.image = #imageLiteral(resourceName: "cancel")
            }
            else{
                cell.pageImage.image = #imageLiteral(resourceName: "checkmark")
                cell.promotionImage.image = #imageLiteral(resourceName: "checkmark")
                cell.discountImage.image = #imageLiteral(resourceName: "checkmark")
            }
        }
        cell.upgradeBtn.setTitle(NSLocalizedString("Upgrade Now", comment: "Upgrade Now"), for: .normal)
        cell.upgradeBtn.tag = indexPath.row
        if ControlSettings.showPaymentVC == true{
        cell.upgradeBtn.addTarget(self, action: #selector(self.startCheckout), for: .touchUpInside)
        }
        
        return cell
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        return CGSize(width: 200.0, height: 420.0)
     }
    @objc func startCheckout(sender: UIButton) {
         // Example: Initialize BTAPIClient, if you haven't already
         braintreeClient = BTAPIClient(authorization: ControlSettings.paypalAuthorizationToken)!
         let payPalDriver = BTPayPalDriver(apiClient: braintreeClient!)
         payPalDriver.viewControllerPresentingDelegate = self
         payPalDriver.appSwitchDelegate = self // Optional
         
         // Specify the transaction amount here. "2.32" is used in this example.
        let request = BTPayPalRequest(amount:  (self.upgardeArray[sender.tag]["price"] as? String)!)
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
              self.userUpGrade()
             } else if let error = error {
                 print("error = \(error.localizedDescription ?? "")")
             } else {
                 print("error = \(error?.localizedDescription ?? "")")
                 
             }
         }
     }
}

extension UpgradeController:BTAppSwitchDelegate, BTViewControllerPresentingDelegate{
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

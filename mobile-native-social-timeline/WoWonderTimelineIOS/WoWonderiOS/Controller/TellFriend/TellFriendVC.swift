
import UIKit
import WoWonderTimelineSDK
import GoogleMobileAds

class TellFriendVC: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    var bannerView: GADBannerView!
       var interstitial: GADInterstitial!
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.setupUI()
    }
    
    private func setupUI(){
        self.tableView.separatorStyle = .none
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
            navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("Earnings", comment: "Earnings")
        
        tableView.register(UINib(nibName: "TellFriendTableItem", bundle: nil), forCellReuseIdentifier: "TellFriendTableItem")
        tableView.register(UINib(nibName: "HelpSupportTableItem", bundle: nil), forCellReuseIdentifier: "HelpSupportTableItem")
        
        if ControlSettings.shouldShowAddMobBanner{
            
            bannerView = GADBannerView(adSize: kGADAdSizeBanner)
            addBannerViewToView(bannerView)
            bannerView.adUnitID = ControlSettings.addUnitId
            bannerView.rootViewController = self
            bannerView.load(GADRequest())
            interstitial = GADInterstitial(adUnitID:  ControlSettings.interestialAddUnitId)
            let request = GADRequest()
            interstitial.load(request)
        }
        
    }
    func CreateAd() -> GADInterstitial {
             let interstitial = GADInterstitial(adUnitID:  ControlSettings.interestialAddUnitId)
             interstitial.load(GADRequest())
             return interstitial
         }
         func addBannerViewToView(_ bannerView: GADBannerView) {
             bannerView.translatesAutoresizingMaskIntoConstraints = false
             view.addSubview(bannerView)
             view.addConstraints(
                 [NSLayoutConstraint(item: bannerView,
                                     attribute: .bottom,
                                     relatedBy: .equal,
                                     toItem: bottomLayoutGuide,
                                     attribute: .top,
                                     multiplier: 1,
                                     constant: 0),
                  NSLayoutConstraint(item: bannerView,
                                     attribute: .centerX,
                                     relatedBy: .equal,
                                     toItem: view,
                                     attribute: .centerX,
                                     multiplier: 1,
                                     constant: 0)
                 ])
         }
    private func share(){
          let firstActivityItem = "Text you want"
                let secondActivityItem : NSURL = NSURL(string: "https://play.typeracer.com")!
                // If you want to put an image
        //        let image : UIImage = UIImage(named: "image .jpg")!

                let activityViewController : UIActivityViewController = UIActivityViewController(
                    activityItems: [firstActivityItem, secondActivityItem], applicationActivities: nil)

                // This lines is for the popover you need to show in iPad
        activityViewController.popoverPresentationController?.sourceView = self.view

                // This line remove the arrow of the popover to show in iPad
                activityViewController.popoverPresentationController?.permittedArrowDirections = .up
                activityViewController.popoverPresentationController?.sourceRect = CGRect(x: 150, y: 150, width: 0, height: 0)

                // Anything you want to exclude
                activityViewController.excludedActivityTypes = [
                    UIActivity.ActivityType.postToWeibo,
                    UIActivity.ActivityType.print,
                    UIActivity.ActivityType.assignToContact,
                    UIActivity.ActivityType.saveToCameraRoll,
                    UIActivity.ActivityType.addToReadingList,
                    UIActivity.ActivityType.postToFlickr,
                    UIActivity.ActivityType.postToVimeo,
                    UIActivity.ActivityType.postToTencentWeibo
                ]

                self.present(activityViewController, animated: true, completion: nil)
    }
    
}

extension TellFriendVC: UITableViewDelegate {
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        
        return 50.0
        
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        if AppInstance.instance.addCount == ControlSettings.interestialCount {
                                     if interstitial.isReady {
                                         interstitial.present(fromRootViewController: self)
                                         interstitial = CreateAd()
                                         AppInstance.instance.addCount = 0
                                     } else {
                                         
                                         print("Ad wasn't ready")
                                     }
                                 }
        AppInstance.instance.addCount = AppInstance.instance.addCount! + 1
        tableView.deselectRow(at: indexPath, animated: true)
        switch indexPath.section {
        case 0:
            switch indexPath.row {
            case 0:
                let storyboard = UIStoryboard(name: "TellFriend", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "MyAffiliatesVC") as! MyAffiliatesVC
                self.navigationController?.pushViewController(vc, animated: true)
            case 1:
                let storyBoard = UIStoryboard(name: "Main", bundle: nil)
                let vc = storyBoard.instantiateViewController(withIdentifier: "WalletVC") as! WalletMainController
                
                vc.mybalance = AppInstance.instance.profile?.userData?.wallet ?? ""
                if (ControlSettings.showPaymentVC == true){
                self.navigationController?.pushViewController(vc, animated: true)
                }
            case 2:
                let storyboard = UIStoryboard(name: "TellFriend", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "MyPointsVC") as! MyPointsVC
                self.navigationController?.pushViewController(vc, animated: true)
            default:
                break
                
            }
        case 1:
            switch indexPath.row{
            case 0:
                self.share()
                
            default:
                break
            }
            
        default:
            break
        }
        
        
    }
    
    func tableView(_ tableView: UITableView, viewForHeaderInSection section: Int) -> UIView? {
        
        let view = UIView(frame: CGRect(x: 0, y: 0, width: tableView.frame.size.width, height: 56))
        view.backgroundColor = UIColor.white
        
        let separatorView = UIView(frame: CGRect(x: 0, y: 0, width: view.frame.size.width, height: 8))
        separatorView.backgroundColor = UIColor(red: 220/255, green: 220/255, blue: 220/255, alpha: 1.0)
        
        let label = UILabel(frame: CGRect(x: 16, y: 8, width: view.frame.size.width, height: 48))
        label.textColor = UIColor.hexStringToUIColor(hex: "984243")
        label.font = UIFont(name: "Arrial", size: 17)
        if section == 0{
            label.text = NSLocalizedString("Affliates", comment: "Affliates")
            
        } else if section == 1 {
            label.text = NSLocalizedString("Share", comment: "Share")
        }
        view.addSubview(separatorView)
        view.addSubview(label)
        return view
        
    }
    
    func tableView(_ tableView: UITableView, heightForHeaderInSection section: Int) -> CGFloat {
        return 56
    }
}

extension TellFriendVC: UITableViewDataSource {
    
    func numberOfSections(in tableView: UITableView) -> Int {
        
        return 2
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        switch section {
        case 0: return 3
        case 1: return 1
        default: return 0
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        
        switch indexPath.section {
        case 0:
            switch indexPath.row {
            case 0:
                let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
                cell.titleLabel.text = NSLocalizedString("My Affiliates", comment: "My Affiliates")
                
                return cell
            case 1:
                let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
                cell.titleLabel.text = NSLocalizedString("My Balance", comment: "My Balance")
                return cell
                
            case 2:
                let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
                cell.titleLabel.text = NSLocalizedString("My Points", comment: "My Points")
                return cell
                
            default:
                return UITableViewCell()
            }
        case 1:
            switch indexPath.row{
            case 0:
                let cell = tableView.dequeueReusableCell(withIdentifier: "TellFriendTableItem") as! TellFriendTableItem
                return cell
                
            default:
                return UITableViewCell()
            }
            
        default:
            return UITableViewCell()
        }
    }
}



import UIKit
import OneSignal
import WoWonderTimelineSDK
import GoogleMobileAds

class NotificationCheckVC: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    var bannerView: GADBannerView!
     var interstitial: GADInterstitial!
    override func viewDidLoad() {
        super.viewDidLoad()
        self.title = NSLocalizedString("Notification", comment: "Notification")
        self.navigationItem.largeTitleDisplayMode = .never
        self.setupUI()
    }
    
    private func setupUI(){
        self.tableView.separatorStyle = .none
        tableView.register(UINib(nibName: "NotificationTwoTableItem", bundle: nil), forCellReuseIdentifier: "NotificationTwoTableItem")
        tableView.register(UINib(nibName: "NotificationOneTableItem", bundle: nil), forCellReuseIdentifier: "NotificationOneTableItem")
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
    
}

extension NotificationCheckVC: UITableViewDelegate {
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 100.0
        
    }
}

extension NotificationCheckVC: UITableViewDataSource {
    
    func numberOfSections(in tableView: UITableView) -> Int {
        
        return 1
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        switch section {
        case 0: return 1
//        case 1: return 1
        default: return 0
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        
        switch indexPath.section {
        case 0:
            switch indexPath.row {
            case 0:
                let cell = tableView.dequeueReusableCell(withIdentifier: "NotificationOneTableItem") as! NotificationOneTableItem
                cell.Switchlabel.isHidden = true
              
                 let status = UserDefaults.standard.getNotificationStatus(Key: "notificationStatus")
                           cell.isSelected = status
                           if cell.isSelected
                           {
                               cell.isSelected = true
                               if cell.accessoryType == UITableViewCell.AccessoryType.none{
                               cell.accessoryType = UITableViewCell.AccessoryType.checkmark
                                   
                               }else{
                                   cell.isSelected = false
                                   cell.accessoryType = UITableViewCell.AccessoryType.none
                               }
                           }
                return cell
            default:
                return UITableViewCell()
            }
        case 1:
            let cell = tableView.dequeueReusableCell(withIdentifier: "NotificationTwoTableItem") as! NotificationTwoTableItem
            let status = UserDefaults.standard.getConservationToneStatus(Key: "conversationToneStatus")
            cell.isSelected = status
            if cell.isSelected
            {
                cell.isSelected = true
                if cell.accessoryType == UITableViewCell.AccessoryType.none{
                cell.accessoryType = UITableViewCell.AccessoryType.checkmark
                    
                }else{
                    cell.isSelected = false
                    cell.accessoryType = UITableViewCell.AccessoryType.none
                }
                
            }
            return cell
        default:
            return UITableViewCell()
        }
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
        //        tableView.cellForRow(at: indexPath)?.accessoryType = .checkmark
        //    self.delegate?.didSelectUser(userID: self.filteredData[indexPath.row].userID ?? "", username: self.filteredData[indexPath.row].username ?? "", index: indexPath.row)
        if indexPath.section == 0{
            
            let cell = tableView.cellForRow(at: indexPath)
                              
                              if cell!.isSelected
                              {
                                  cell!.isSelected = false
                                  if cell!.accessoryType == UITableViewCell.AccessoryType.none
                                  {
                                        UserDefaults.standard.setNotificationStatus(value: true, ForKey: "notificationStatus")
                                     OneSignal.setSubscription(true)
                                      cell!.accessoryType = UITableViewCell.AccessoryType.checkmark
                                  }
                                  else
                                  {
                                       UserDefaults.standard.setNotificationStatus(value: false, ForKey: "notificationStatus")
                                     OneSignal.setSubscription(false)
                                      cell!.accessoryType = UITableViewCell.AccessoryType.none
                                      
                                  }
                              }
        }else{
            let cell = tableView.cellForRow(at: indexPath)
                   
                   if cell!.isSelected
                   {
                       cell!.isSelected = false
                       if cell!.accessoryType == UITableViewCell.AccessoryType.none
                       {
                           UserDefaults.standard.setConversationToneStatus(value: true, ForKey: "conversationToneStatus")
                       
                           cell!.accessoryType = UITableViewCell.AccessoryType.checkmark
                       }
                       else
                       {
                           UserDefaults.standard.setConversationToneStatus(value: false, ForKey: "conversationToneStatus")
                           cell!.accessoryType = UITableViewCell.AccessoryType.none
                           
                       }
                   }
        }

       
    }
}

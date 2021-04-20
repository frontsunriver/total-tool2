
import UIKit
import WoWonderTimelineSDK
import GoogleMobileAds

class LastActivitesController: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    var activites = [[String:Any]]()
    
    let Storyboard = UIStoryboard(name: "Main", bundle: nil)
    let status = Reach().connectionStatus()
    let pulltoRefresh = UIRefreshControl()
    var interstitial: GADInterstitial!

    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("Last Activites", comment: "Last Activites")
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        tableView.register(UINib(nibName: "ActivitiesSectionTwoTableItem", bundle: nil), forCellReuseIdentifier: "ActivitiesSectionTwoTableItem")
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.tableView.addSubview(pulltoRefresh)
        self.activityIndicator.startAnimating()
        self.tableView.tableFooterView = UIView()
        self.loadActivities()
        if ControlSettings.shouldShowAddMobBanner{
                                
                              
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
    
    ///Network Connectivity.
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
        }
    }
   
    
    @objc func refresh(){
        self.activites.removeAll()
        self.tableView.reloadData()
        self.loadActivities()
    }
    
    
    private func loadActivities(){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            performUIUpdatesOnMain {
                LastActivitiesManager.instance.getLastActivites(offset: 0, limit: 25) { (success, authError, error) in
                    if success != nil {
                        for i in success!.activities{
                            self.activites.append(i)
                        }
                        self.tableView.reloadData()
                        self.activityIndicator.stopAnimating()
                    }
                    else if authError != nil {
                        self.view.makeToast(authError?.errors?.errorText)
                    }
                    else if error  != nil {
                        print(error?.localizedDescription)
                        
                    }
                }
            }
        }
    }
    
}
extension LastActivitesController: UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        self.activites.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "ActivitiesSectionTwoTableItem") as! ActivitiesSectionTwoTableItem
        cell.selectionStyle = .none
        let index = self.activites[indexPath.row]
        
        if let activitor = index["activator"] as? [String:Any]{
            if let image = activitor["avatar"] as? String{
                let url = URL(string: image)
                cell.profileImage.kf.setImage(with: url)
            }
            
        }
        if let descrip = index["activity_text"] as? String{
            cell.titleLabel.text = descrip
        }
        if let type = index["activity_type"] as? String{
            if type == "commented_post"{
                cell.changeIcon.image = UIImage(named: "commentss")
            }
            else if type == "following"{
                cell.changeIcon.image = #imageLiteral(resourceName: "Shape")
            }
            else  {
                cell.changeIcon.image = #imageLiteral(resourceName: "like-2")
            }
        }
        
        return cell
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 80.0
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
             let index = self.activites[indexPath.row]
             if let Type = index["activity_type"] as? String{
                 if (Type == "following" || Type == "friend"){
                     if let activator = index["activator"] as? [String:Any]{
                         let storyBoard = UIStoryboard(name: "Main", bundle: nil)
                         let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
                         vc.userData = activator
                         self.navigationController?.pushViewController(vc, animated: true)
                     }
                 }
                 else{
                     var post_id: String? = nil
                     var post_data: [String:Any]? = nil
                     if let id = index["post_id"] as? String{
                         post_id = id
                 }
                     if let postData = index["postData"] as? [String:Any]{
                         post_data = postData
                     }
                     let storyBoard = UIStoryboard(name: "Main", bundle: nil)
                     let vc = storyBoard.instantiateViewController(withIdentifier: "ShowPostVC") as! ShowPostController
                     vc.postId = post_id
                     self.navigationController?.pushViewController(vc, animated: true)

                 }
             }
     }
}

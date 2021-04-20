

import UIKit
import AlamofireImage
import WoWonderTimelineSDK
import Kingfisher
import SDWebImage
import PaginatedTableView
import Toast_Swift
import ZKProgressHUD
import GoogleMobileAds


class ManageSessionVC: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    
    let spinner = UIActivityIndicatorView(style: .gray)
    let pulltoRefresh = UIRefreshControl()
    var sessionArray = [SessionModel.Datum]()
    let status = Reach().connectionStatus()
    var interstitial: GADInterstitial!

    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.setupUI()
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
    private func setupUI(){

        self.tableView.separatorStyle = .none
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
            navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("Manage Session", comment: "Manage Session")
        tableView.register(UINib(nibName: "ManageSessionTableItem", bundle: nil), forCellReuseIdentifier: "ManageSessionTableItem")
        
        self.tableView.dataSource = self
        self.tableView.backgroundColor = .white
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.tableView.addSubview(pulltoRefresh)
        ZKProgressHUD.show()
        self.loadSessions()
    }
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
            
        }
        
    }
    @objc func refresh(){
        self.sessionArray.removeAll()
        self.tableView.reloadData()
        loadSessions()
        pulltoRefresh.endRefreshing()
        
    }
    private func loadSessions(){
        switch status {
        case .unknown, .offline:
            ZKProgressHUD.dismiss()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            performUIUpdatesOnMain {
                SessionManager.instance.getSession(type: "get") { (success, authError, error) in
                    if success != nil {
                        self.sessionArray = success?.data ?? []
                        self.tableView.reloadData()
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
    
}
extension ManageSessionVC: UITableViewDelegate {
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 80.0
    }
}


extension ManageSessionVC: UITableViewDataSource {
    
    func numberOfSections(in tableView: UITableView) -> Int {
        
        return 1
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.sessionArray.count ?? 0
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "ManageSessionTableItem") as! ManageSessionTableItem
        cell.vc = self
        let object = self.sessionArray[indexPath.row]
        cell.bind(object, index: indexPath.row)
        
        return cell
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
    }
}

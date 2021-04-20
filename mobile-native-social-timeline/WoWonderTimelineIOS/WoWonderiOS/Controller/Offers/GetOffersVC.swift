
import UIKit
import WoWonderTimelineSDK
import AlamofireImage
import Kingfisher
import SDWebImage
import PaginatedTableView
import Toast_Swift
import ZKProgressHUD
import GoogleMobileAds

class GetOffersVC: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    let spinner = UIActivityIndicatorView(style: .gray)
    let pulltoRefresh = UIRefreshControl()
    var offersArray = [GetOffersModel.Datum]()
    let status = Reach().connectionStatus()
    var interstitial: GADInterstitial!
   
    
    override func viewDidLoad() {
        super.viewDidLoad()
        if ControlSettings.shouldShowAddMobBanner{
            interstitial = GADInterstitial(adUnitID:  ControlSettings.interestialAddUnitId)
            let request = GADRequest()
            interstitial.load(request)
        }
          self.setupUI()
        
    }
    func CreateAd() -> GADInterstitial {
        let interstitial = GADInterstitial(adUnitID:  ControlSettings.interestialAddUnitId)
        interstitial.load(GADRequest())
        return interstitial
    }
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
      
    }
    
    @IBAction func addFundingPressed(_ sender: Any) {
        let storyboard = UIStoryboard(name: "Offers", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "AddUpdateFundingVC") as! AddUpdateFundingVC
        self.navigationController?.pushViewController(vc, animated: true)
    }
    private func setupUI(){
        
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("Offers", comment: "Offers")
        
        self.tableView.separatorStyle = .none
        tableView.register(UINib(nibName: "GetOffersTableItem", bundle: nil), forCellReuseIdentifier: "GetOffersTableItem")
        
        self.tableView.dataSource = self
        self.tableView.backgroundColor = .white
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.tableView.addSubview(pulltoRefresh)
//        ZKProgressHUD.show()
        self.activityIndicator.startAnimating()
        self.offersArray.removeAll()
        self.tableView.reloadData()
        self.loadFundings()
    }
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
            
        }
        
    }
    @objc func refresh(){
        self.offersArray.removeAll()
        self.tableView.reloadData()
        loadFundings()
        pulltoRefresh.endRefreshing()
        
    }
    private func loadFundings(){
        switch status {
        case .unknown, .offline:
            ZKProgressHUD.dismiss()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            performUIUpdatesOnMain {
                OffersManager.instance.getOffers(type: "get", limit: 10, offset: 0) { (success,authError , error) in
                    if success != nil {
                        self.offersArray = success?.data ?? []
                        self.tableView.reloadData()
                        self.activityIndicator.stopAnimating()
                        
                    }
                    else if authError != nil {
                        self.activityIndicator.stopAnimating()
                        self.view.makeToast(authError?.errors?.errorText)
                        self.showAlert(title: "", message: (authError?.errors?.errorText)!)
                    }
                    else if error  != nil {
                        print(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
}
extension GetOffersVC: UITableViewDelegate {
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return UITableView.automaticDimension
    }
}


extension GetOffersVC: UITableViewDataSource {
    
    func numberOfSections(in tableView: UITableView) -> Int {
        return 1
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.offersArray.count ?? 0
        
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "GetOffersTableItem") as! GetOffersTableItem
        let object = self.offersArray[indexPath.row]
        let randomInt = Int.random(in: 0...6    )
        cell.bind(object,index:randomInt)
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
        let storyboard = UIStoryboard(name: "Offers", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "DetailOfferVC") as! DetailOfferVC
        vc.object = self.offersArray[indexPath.row]
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
}

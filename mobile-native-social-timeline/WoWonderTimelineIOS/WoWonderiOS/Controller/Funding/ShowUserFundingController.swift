//
//  ShowUserFundingController.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/21/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import AlamofireImage
import Kingfisher
import SDWebImage
import PaginatedTableView
import Toast_Swift
import ZKProgressHUD
import WoWonderTimelineSDK
import GoogleMobileAds
import XLPagerTabStrip

class ShowUserFundingController: UIViewController,IndicatorInfoProvider,CreateFundDelegate {
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    var myFunding = [[String:Any]]()
    let spinner = UIActivityIndicatorView(style: .medium)
    let pulltoRefresh = UIRefreshControl()
    let status = Reach().connectionStatus()
    var interstitial: GADInterstitial!
    var offset = ""
    
    override func viewDidLoad() {
        super.viewDidLoad()

        self.setupUI()
    }
    
    func CreateAd() -> GADInterstitial {
        let interstitial = GADInterstitial(adUnitID:  ControlSettings.interestialAddUnitId)
        interstitial.load(GADRequest())
        return interstitial
    }
    
    
    private func setupUI(){
        
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("MyFundings", comment: "MyFundings")
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(self.CreateFunds(_:)), name: Notification.Name(rawValue: "Funds"), object: nil)
        self.tableView.separatorStyle = .none
        tableView.register(UINib(nibName: "GetFundingTableItem", bundle: nil), forCellReuseIdentifier: "GetFundingTableItem")
        self.tableView.dataSource = self
        self.tableView.delegate = self
        self.tableView.backgroundColor = .white
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.tableView.addSubview(pulltoRefresh)
//                ZKProgressHUD.show()
//        self.myFunding.removeAll()
//        self.tableView.reloadData()
        self.loadMyFunding(offset: Int(self.offset) ?? 0)
    }
    
    
    
    
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
            
        }
        
    }
    @objc func CreateFunds(_ notification: Notification) {
        self.loadMyFunding(offset: Int(self.offset) ?? 0)
    }
    @objc func refresh(){
        self.myFunding.removeAll()
        self.tableView.reloadData()
        self.loadMyFunding(offset: Int(self.offset) ?? 0)

    }
    
    private func loadMyFunding(offset: Int){
        GetUserFundingManager.sharedInstance.getFunding(userId: UserData.getUSER_ID() ?? "", offset: "\(offset)") { (success, authError, error) in
            if (success != nil){
                self.myFunding.removeAll()
                for i in success!.data{
                    self.myFunding.append(i)
                }
                let off_set = self.myFunding.last!["id"] as? Int
                self.offset = "\(off_set)"
                print(offset)
                self.pulltoRefresh.endRefreshing()
                self.activityIndicator.stopAnimating()
                self.tableView.reloadData()
            }
            else if (authError != nil){
                self.pulltoRefresh.endRefreshing()
                self.activityIndicator.stopAnimating()
                self.view.makeToast(authError?.errors?.errorText)
            }
            else if (error != nil){
                self.pulltoRefresh.endRefreshing()
                self.activityIndicator.stopAnimating()
                self.view.makeToast(error?.localizedDescription)
            }
        }
    }
    
    @IBAction func CreateFunding(_ sender: Any) {
        let storyboard = UIStoryboard(name: "Funding", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "AddUpdateFundingVC") as! AddUpdateFundingVC
        vc.delegate = self
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
    func indicatorInfo(for pagerTabStripController: PagerTabStripViewController) -> IndicatorInfo {
         return IndicatorInfo(title: NSLocalizedString("My Funding", comment: "My Funding"))
     }
    
    func createFund() {
        print("Create Funds")
//        self.loadMyFunding(offset: Int(self.offset) ?? 0)
    }
    
}
extension ShowUserFundingController:UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.myFunding.count ?? 0
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "GetFundingTableItem") as! GetFundingTableItem
        let index = self.myFunding[indexPath.row]
        if let userData = index["user_data"] as? [String:Any]{
            if let userName = userData["username"] as? String{
                cell.usernameLabel.text = userName
            }
    
        }
        if let title = index["title"] as? String{
            cell.titleLabel.text = title
        }
        if let desc = index["description"] as? String{
            cell.descriptionLabel.text  = desc
        }
        if let total_amount = index["amount"] as? String{
            cell.totalAmountLabel.text = total_amount
        }
        if let amountCollect = index["raised"] as? String{
            cell.amountLabel.text = amountCollect
        }
        if let time = index["time"] as? String{
            let epocTime = TimeInterval(Int(time) ?? 1601815559)
            let myDate = NSDate(timeIntervalSince1970: epocTime)
            let formate = DateFormatter()
            formate.dateFormat = "yyyy-MM-dd"
            let dat = formate.string(from: myDate as Date)
            print("Date",dat)
            print("Converted Time \(myDate)")
            cell.timeLabel.text = "\(dat)"
        }
        if let bar = index["bar"] as? Int{
            cell.progressBar.progressValue = CGFloat(bar)
        }
        if let userImage = index["image"] as? String{
            let url = URL(string: userImage)
            cell.profileImage.kf.setImage(with: url)
        }
        
//        cell.bind(object,index:indexPath.row)
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
        
        let storyboard = UIStoryboard(name: "Funding", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "ShowFundingDetailsVC") as! ShowFundingDetailsVC
        let index = self.myFunding[indexPath.row]
        var user_data = [String:Any]()
        if let userInfo = index["user_data"] as? [String:Any]{
            user_data = userInfo
        }
        let userData = GetFundingModel.UserData(userID: user_data["user_id"] as? String, username: user_data["username"] as? String, email: user_data["email"] as? String, firstName: user_data["first_name"] as? String, lastName: user_data["last_name"] as? String, avatar: user_data["avatar"] as? String, cover: user_data["cover"] as? String)
        let data = GetFundingModel.Datum(id: index["id"] as? Int, hashedID: index["hashed_id"] as? String, title: index["title"] as? String, datumDescription: index["description"] as? String, amount: index["amount"] as? String, userID: index["user_id"] as? Int, image: index["image"] as? String, time: index["time"] as? String, raised: index["raised"] as? Int, bar: index["bar"] as? Int, userData: userData)
        vc.object = data
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
        func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
            return 490.0
        }
    
    
    func tableView(_ tableView: UITableView, willDisplay cell: UITableViewCell, forRowAt indexPath: IndexPath) {
        
        if self.myFunding.count >= 10 {
            let count = self.myFunding.count
            let lastElement = count - 1
            
            if indexPath.row == lastElement {
                spinner.startAnimating()
                spinner.frame = CGRect(x: CGFloat(0), y: CGFloat(0), width: tableView.bounds.width, height: CGFloat(44))
                self.tableView.tableFooterView = spinner
                self.tableView.tableFooterView?.isHidden = false
                self.loadMyFunding(offset: Int(self.offset) ?? 0)
            }
        }
    }
    
    }


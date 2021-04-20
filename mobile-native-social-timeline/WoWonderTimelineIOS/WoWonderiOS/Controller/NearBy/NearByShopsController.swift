//
//  NearByShopsController.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/10/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class NearByShopsController: UIViewController {
    
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var noShopsView: UIView!
    @IBOutlet weak var noShopLbl: UILabel!
    
    let status = Reach().connectionStatus()
    let pulltoRefresh = UIRefreshControl()
    let Stroyboard =  UIStoryboard(name: "MoreSection2", bundle: nil)
    
    var shops = [[String:Any]]()
    var offset = ""
    var name = ""
    var distance = 0
    var selectedIndex = 0
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.title = "NearBy Shops"
        self.navigationItem.largeTitleDisplayMode = .never
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.tableView.delegate = self
        self.tableView.dataSource = self
        self.tableView.tableFooterView = UIView()
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        self.noShopsView.isHidden = true
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.tableView.addSubview(pulltoRefresh)
        Reach().monitorReachabilityChanges()
        self.activityIndicator.startAnimating()
    }
    
    @objc func refresh(){
        self.offset = ""
        self.shops.removeAll()
        self.tableView.reloadData()
        self.getShops()
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    private func getShops(){
        
        NearByShopsManager.sharedInstacne.nearByShops(offset: self.offset, name: self.name, distance: self.distance) { (success, authError, error) in
            if (success != nil){
                for i in success!.data{
                    self.shops.append(i)
                }
                if (self.shops.count == 0){
                    self.noShopsView.isHidden = false
                    self.tableView.isHidden = true
                }
                self.offset = self.shops.last?["id"] as? String ?? ""
                self.activityIndicator.stopAnimating()
                self.pulltoRefresh.endRefreshing()
                self.tableView.reloadData()
            }
            else if (authError != nil){
                self.activityIndicator.stopAnimating()
                self.pulltoRefresh.endRefreshing()
                self.view.makeToast(authError?.errors?.errorText)
            }
            else if (error != nil){
                self.activityIndicator.stopAnimating()
                self.pulltoRefresh.endRefreshing()
                print(error?.localizedDescription)
            }
            
        }
        
    }
    
    @IBAction func Filter(_ sender: Any) {
    }
    
    
}

extension NearByShopsController: UITableViewDelegate, UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.shops.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
         let cell = UITableViewCell()
//            collectionView.dequeueReusableCell(withReuseIdentifier: "JobVCCollectionCell", for: indexPath) as! JobVCCollectionCell
//        let object = self.shops[indexPath.row]
//        let title = object["title"] as? String
//        let description = object["description"] as? String
//        let minimum = object["minimum"] as? String
//        let maxium = object["maximum"] as? String
//        let category = object["category"] as? String
//        var imageString = object["image"] as? String
//        
//        self.titleLabel.text = title?.htmlToString ?? ""
//        self.descriptionLabel.text = description?.htmlToString ?? ""
//        let siteSetting = AppInstance.instance.siteSettings
//        if let cat = siteSetting["job_categories"] as? [String:Any]{
//            if let cate_name = cat[category ?? ""] as? String{
//                self.MoneyLabel.text = "$\(minimum ?? "") - $\(maxium ?? "").\(cate_name)"
//            }
//        }
//        let url = URL(string: imageString ?? "")
//        self.thumbnailImage.kf.indicatorType = .activity
//        self.thumbnailImage.kf.setImage(with: url)
        return cell
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 180.0
    }
    
}

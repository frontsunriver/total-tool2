//
//  NearByBusiness.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/10/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class NearByBusiness: UIViewController {
    
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    let status = Reach().connectionStatus()
    let pulltoRefresh = UIRefreshControl()
    let Stroyboard =  UIStoryboard(name: "Jobs", bundle: nil)
    var offset = ""
    
    var businesses = [[String:Any]]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.title = NSLocalizedString("NearByBusiness", comment: "NearByBusiness")
        self.navigationItem.largeTitleDisplayMode = .never
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.tableView.tableFooterView = UIView()
        self.activityIndicator.startAnimating()
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.tableView.addSubview(pulltoRefresh)
//        self.noEventView.isHidden = true
//        self.getMyEvents(myOffset: self.myoffSet)

    }
    
    @objc func refresh(){
        self.offset = ""
        self.businesses.removeAll()
        self.tableView.reloadData()
//        self.getMyEvents(myOffset: self.myoffSet)
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    private func getBusinesses() {
        
    }
    
    



}

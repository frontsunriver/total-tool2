//
//  PageReviewController.swift
//  News_Feed
//
//  Created by Ubaid Javaid on 3/25/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class PageReviewController: UIViewController {

    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    @IBOutlet weak var noReviewView: UIView!
    var reviews = [[String:Any]]()
    
    let status = Reach().connectionStatus()
    let spinner = UIActivityIndicatorView(style: .gray)
    let pulltoRefresh = UIRefreshControl()

    var offset: String? = nil
    var pageId: String? = nil

    override func viewDidLoad() {
        super.viewDidLoad()
        
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.tableView.register(UINib(nibName: "PageReviewCell", bundle: nil), forCellReuseIdentifier: "ReviewCell")
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.title = NSLocalizedString("Reviews", comment: "Reviews")
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.activityIndicator.startAnimating()
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.tableView.addSubview(pulltoRefresh)
        self.tableView.tableFooterView = UIView()
        self.getPageReview(pageId: self.pageId ?? "", offset: "")
    }
    /// Network Connectivity
     @objc func networkStatusChanged(_ notification: Notification) {
         if let userInfo = notification.userInfo {
             let status = userInfo["Status"] as! String
             print("Status",status)
         }
     }
    
    //Pull To Refresh
    
    @objc func refresh(){
        self.reviews.removeAll()
        self.spinner.stopAnimating()
        self.tableView.reloadData()
        self.getPageReview(pageId: self.pageId ?? "", offset: "")
    }
    
    private func getPageReview(pageId: String, offset: String){
        switch status {
         case .unknown, .offline:
             showAlert(title: "", message: "Internet Connection Failed")
         case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetPageReviewManager.sahredInstance.getPageReview(pageid: pageId, offset: offset) { (success, authError, error) in
                    if success != nil{
                        for i in success!.data{
                            self.reviews.append(i)
                        }
                        if self.reviews.isEmpty == true{
                            self.tableView.isHidden = true
                            self.noReviewView.isHidden = false
                        }
                        self.offset = self.reviews.last?["user_id"] as? String
                        self.activityIndicator.stopAnimating()
                        self.tableView.reloadData()
                    }
                    else if authError != nil{
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil{
                        self.view.makeToast(error?.localizedDescription)
                    }
                }
            }
        }
    }
}

extension PageReviewController: UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        self.reviews.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "ReviewCell") as! PageReviewCell
        let index = self.reviews[indexPath.row]
        if let val = index["valuation"] as? String{
            cell.ratingLabel.text = val
        }
        if let review = index["review"] as? String{
            cell.ReviewLabel.text = review
        }
        if let userdata = index["user_data"] as? [String:Any]{
            if let image = userdata["avatar"] as? String{
                let url = URL(string: image)
                cell.userImage.kf.setImage(with: url)
            }
            if let username = userdata["name"] as? String{
                cell.userName.text = username
            }
        }
        return cell
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return UITableView.automaticDimension
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
         let storyBoard = UIStoryboard(name: "Main", bundle: nil)
         let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
        if let user_Data = self.reviews[indexPath.row]["user_data"] as? [String:Any]{
            vc.userData = user_Data
         }
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
    func tableView(_ tableView: UITableView, willDisplay cell: UITableViewCell, forRowAt indexPath: IndexPath) {
        
        if self.reviews.count >= 15 {
            let count = self.reviews.count
            let lastElement = count - 1
            
            if indexPath.row == lastElement {
                spinner.startAnimating()
                spinner.frame = CGRect(x: CGFloat(0), y: CGFloat(0), width: tableView.bounds.width, height: CGFloat(44))
                self.tableView.tableFooterView = spinner
                self.tableView.tableFooterView?.isHidden = false
                self.getPageReview(pageId: self.pageId ?? "", offset: self.offset ?? "")
            }
        }
    }
    
}

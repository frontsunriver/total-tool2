//
//  ShowAllSuggestedGroups.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/13/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class ShowAllSuggestedGroups: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    var suggestedGroups = [[String:Any]]()
    
    let status = Reach().connectionStatus()
    var selectedIndex = 0

    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.title = NSLocalizedString("Suggested Groups", comment: "Suggested Groups")
        self.navigationItem.largeTitleDisplayMode = .never
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.tableView.register(UINib(nibName: "LikePagesCell", bundle: nil), forCellReuseIdentifier: "LikePage")
        self.tableView.tableFooterView = UIView()
        self.tableView.delegate = self
        self.tableView.dataSource = self
        self.tableView.showsVerticalScrollIndicator = false
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.activityIndicator.startAnimating()
        self.getSuggestedGroups()

    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    
    private func getSuggestedGroups(){
        switch status {
        case .unknown, .offline:
            self.activityIndicator.stopAnimating()
            self.view.makeToast("Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            GetSuggestedGroupManager.sharedInstance.getGroups(limit: 25) { (success, authError, error) in
                if (success != nil){
                    for i in success!.data{
                        self.suggestedGroups.append(i)
                    }
                    self.activityIndicator.stopAnimating()
                    self.tableView.reloadData()
                    
                }
                else if (authError != nil){
                    self.activityIndicator.stopAnimating()
                    self.view.makeToast(authError?.errors?.errorText)
                }
                else if (error != nil){
                    self.activityIndicator.stopAnimating()
                    self.view.makeToast(error?.localizedDescription)
                }
            }
        }
    }
    
    private func JoinGroup(groupId: String){
        switch self.status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            JoinGroupManager.sharedInstance.joinGroup(groupId: Int(groupId) ?? 0) { (success, authError, error) in
                if success != nil {
                    self.view.makeToast(success?.join_status)
                    
                }
                else if authError != nil {
                    self.view.makeToast(authError?.errors.errorText)
                }
                else if error != nil {
                    self.view.makeToast(error?.localizedDescription)
                }
            }
            
        }
    }
    
    @IBAction func Groupjoin(sender: UIButton){
        switch self.status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
        let cell = self.tableView.cellForRow(at: IndexPath(row: sender.tag, section: 0)) as! LikePagesCell
            let index = self.suggestedGroups[sender.tag]
            var group_id: String? = nil
            if let groupid = index["id"] as? String{
                group_id = groupid
            }
            if let isLike = index["is_joined"] as? Bool{
                if isLike == true{
                    cell.likeBtn.setTitle(NSLocalizedString("Join", comment: "Join"), for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                    cell.likeBtn.backgroundColor = .white
                    self.JoinGroup(groupId: group_id ?? "")
                    self.suggestedGroups[sender.tag]["is_joined"] = false
                }
                else{
                    cell.likeBtn.setTitle(NSLocalizedString("Joined", comment: "Joined"), for: .normal)
                    cell.likeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                    cell.likeBtn.setTitleColor(.white, for: .normal)
                    self.JoinGroup(groupId: group_id ?? "")
                    self.suggestedGroups[sender.tag]["is_joined"] = true
                }
            }
            
        }
    }
}

extension ShowAllSuggestedGroups: UITableViewDelegate, UITableViewDataSource,JoinGroupDelegate,DeleteGroupDelegate{

    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.suggestedGroups.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let index = self.suggestedGroups[indexPath.row]
        let cell = tableView.dequeueReusableCell(withIdentifier: "LikePage") as! LikePagesCell
        if let image = index["avatar"] as? String{
            let url = URL(string: image.trimmingCharacters(in: .whitespaces))
            cell.pageicon.kf.setImage(with: url)
        }
        if let name = index["group_name"] as? String{
            cell.pageName.text = name
        }
        if let category = index["category"] as? String{
            cell.pageCategory.text = category
        }
        if let isOwner = index["is_owner"] as? Bool{
             if (isOwner == true){
                cell.likeBtn.isHidden = true
             }
             else{
                 cell.likeBtn.isHidden = false
             }
         }
    
        if let isJoined = index["is_joined"] as? Bool{
            if isJoined == false{
                cell.likeBtn.setTitle(NSLocalizedString("Join", comment: "Join"), for: .normal)
                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                cell.likeBtn.backgroundColor = .white
            }
            else{
                cell.likeBtn.setTitle(NSLocalizedString("Joined", comment: "Joined"), for: .normal)
                cell.likeBtn.setTitleColor(.white, for: .normal)
                cell.likeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
            }
        }
        cell.likeBtn.tag = indexPath.row
        cell.likeBtn.addTarget(self, action: #selector(self.Groupjoin(sender:)), for: .touchUpInside)
        return cell
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 70.0
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
             let vc = storyboard.instantiateViewController(withIdentifier: "GroupVC") as! GroupController
             let index = self.suggestedGroups[indexPath.row]
             self.selectedIndex = indexPath.row
             if let groupId = index["group_id"] as? String{
                 vc.groupId = groupId
             }
             if let groupName = index["group_name"] as? String{
                 vc.groupName = groupName
             }
             if let groupTitle = index["group_title"] as? String{
                 vc.groupTitle = groupTitle
             }
             if let groupIcon = index["avatar"] as? String{
                 vc.groupIcon = groupIcon
             }
             if let groupCover = index["cover"] as? String{
                 vc.groupCover = groupCover
             }
             if let groupcategory = index["category"] as? String{
                 vc.category = groupcategory
             }
             if let privacy = index["privacy"] as? String{
                 vc.privacy = privacy
             }
             if let categoryId = index["category_id"] as? String{
                 print(categoryId)
                 vc.categoryId = categoryId
             }
             
             if let about  = index["about"] as? String{
                 print(about)
                 vc.aboutGroup = about
             }
             if let isJoined = index["is_joined"] as? Bool{
                vc.isJoined = isJoined
             }
             vc.delegte1 = self
             vc.delegate = self
             vc.groupData = index
             self.navigationController?.pushViewController(vc, animated: true)
    }
    
    func joinGroup(isJoin: Bool) {
        let cell = self.tableView.cellForRow(at: IndexPath(row: self.selectedIndex, section:0)) as! LikePagesCell
        if isJoin == false{
            cell.likeBtn.setTitle(NSLocalizedString("Join", comment: "Join"), for: .normal)
            cell.likeBtn.backgroundColor = .white
            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
            self.suggestedGroups[self.selectedIndex]["is_joined"] = false
            
        }
        else{
            cell.likeBtn.setTitle(NSLocalizedString("Joined", comment: "Joined"), for: .normal)
            cell.likeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
            cell.likeBtn.setTitleColor(.white, for: .normal)
            self.suggestedGroups[self.selectedIndex]["is_joined"] = true
        }
        
    }
    
    func deleteGroup(groupId: String) {
        print("Nothing")
    }
    
}

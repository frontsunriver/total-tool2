//
//  GroupsDiscoverController.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/7/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class GroupsDiscoverController: UIViewController {
    
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    @IBOutlet weak var tableView: UITableView!
    
    var navTitle = ""
    var countryId = ""
    var _status = ""
    var verified = ""
    var gender = ""
    var filterage = ""
    var ageFrom = ""
    var ageTo = ""
    var keyword = ""
    var selectedIndex = 0
    
    var suggestedGroups = [[String:Any]]()
    var randomGroups = [[String:Any]]()
    var groupsCategories = [String:String]()
    
    let status = Reach().connectionStatus()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.largeTitleDisplayMode = .never
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.title = NSLocalizedString("Discover", comment: "Discover")
        self.tableView.tableFooterView = UIView()
        self.tableView.delegate = self
        self.tableView.dataSource = self
        self.tableView.separatorStyle = .none
        self.tableView.showsVerticalScrollIndicator = false
        self.tableView.register(UINib(nibName: "LikePagesCell", bundle: nil), forCellReuseIdentifier: "LikePage")
        self.tableView.register(UINib(nibName: "SuggestedGroupTableCell", bundle: nil), forCellReuseIdentifier: "suggestedTableCell")
        self.tableView.register(UINib(nibName: "GroupCategoryTableCell", bundle: nil), forCellReuseIdentifier: "GroupCateTableCell")
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.activityIndicator.startAnimating()
        self.getGroupCategory()
        self.getSuggestedGroups()
        self.getRandomGroups()
    }
    override func viewWillAppear(_ animated: Bool) {
        self.navigationController?.navigationBar.isHidden = false
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
            GetSuggestedGroupManager.sharedInstance.getGroups(limit: 10) { (success, authError, error) in
                if (success != nil){
                    for i in success!.data{
                        self.suggestedGroups.append(i)
                    }
                    self.activityIndicator.stopAnimating()
                    self.tableView.reloadData()
                    
                }
                else if (authError != nil){
                    self.view.makeToast(authError?.errors?.errorText)
                }
                else if (error != nil){
                    self.view.makeToast(error?.localizedDescription)
                }
            }
        }
    }
    
    private func getRandomGroups(){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetSearchDataManager.sharedInstance.getSearchData(search_keyword: self.keyword, country: self.countryId, status: self._status, verified: self.verified, gender: self.gender, filterbyage: self.filterage, age_from: self.ageFrom, age_to: self.ageTo) { (success, authError, error) in
                    if success != nil{
                        for i in success!.groups{
                            self.randomGroups.append(i)
                        }
                        self.tableView.reloadData()
                    }
                    else if authError != nil{
                        self.view.makeToast(authError?.errors.errorText)
                        self.tableView.isHidden = false
                    }
                    else if error != nil{
                        self.view.makeToast(error?.localizedDescription)
                        self.tableView.isHidden = false
                    }
                }
            }
        }
    }
    private func getGroupCategory(){
        let config = AppInstance.instance.siteSettings
        if let groupCat = config["group_categories"] as? [String:String]{
//            for (key,value) in groupCat{
//                self.groupsCategories.append(value)
//            }
            self.groupsCategories = groupCat
         
            self.tableView.reloadData()
            
        }
    }
    
    private func JoinGroup(groupId: String){
        switch status {
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
        let cell = self.tableView.cellForRow(at: IndexPath(row: sender.tag, section: 2)) as! LikePagesCell
        let index = self.randomGroups[sender.tag]
        var group_id: String? = nil
        if let groupid = index["id"] as? String{
            group_id = groupid
        }
        if let isLike = index["is_joined"] as? String{
            if isLike == "no"{
                cell.likeBtn.setTitle(NSLocalizedString("Joined", comment: "Joined"), for: .normal)
                cell.likeBtn.setTitleColor(.white, for: .normal)
                cell.likeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                self.JoinGroup(groupId: group_id ?? "")
                self.randomGroups[sender.tag]["is_joined"] = "yes"
            }
            else{
                cell.likeBtn.setTitle(NSLocalizedString("Join", comment: "Join"), for: .normal)
                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                cell.likeBtn.backgroundColor = .white
                self.JoinGroup(groupId: group_id ?? "")
                self.randomGroups[sender.tag]["is_joined"] = "no"
            }
        }
    }
}

extension GroupsDiscoverController: UITableViewDelegate,UITableViewDataSource,DeleteGroupDelegate,JoinGroupDelegate{
    
    func numberOfSections(in tableView: UITableView) -> Int {
        return 3
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if (section == 0){
            return 1
        }
        else if (section == 1){
            return 1
        }
        else {
            return self.randomGroups.count
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        if (indexPath.section == 0){
            let cell = tableView.dequeueReusableCell(withIdentifier: "suggestedTableCell") as! SuggestedGroupTableCell
            cell.vc = self
            cell.suggestedGroups = self.suggestedGroups
            cell.collectionView.reloadData()
            return cell
        }
        else if (indexPath.section == 1){
            let cell = tableView.dequeueReusableCell(withIdentifier: "GroupCateTableCell") as! GroupCategoryTableCell
            cell.vc = self
            cell.categorylist = self.groupsCategories
            cell.collectionView.reloadData()
            return cell
        }
        else {
            let index = self.randomGroups[indexPath.row]
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
            if let isJoined = index["is_joined"] as? String{
                if isJoined == "no"{
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
    }
    
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        if indexPath.section == 2{
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
            if let isJoined = index["is_joined"] as? String{
                if isJoined == "no"{
                    vc.isJoined = false
                }
                else{
                    vc.isJoined = true
                }
            }
            vc.delegte1 = self
            vc.delegate = self
            vc.groupData = index
            self.navigationController?.pushViewController(vc, animated: true)
        }
    
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        if (indexPath.section == 0){
            return 270.0
        }
        if (indexPath.section == 1){
            return 246.0
        }
        else{
            return 80.0
        }
        
    }
    func deleteGroup(groupId: String) {
        print("Nothing")
    }
    
    func joinGroup(isJoin: Bool) {
        let cell = self.tableView.cellForRow(at: IndexPath(row: self.selectedIndex, section:2)) as! LikePagesCell
        if isJoin == false{
            cell.likeBtn.setTitle(NSLocalizedString("Join Group", comment: "Join Group"), for: .normal)
            cell.likeBtn.backgroundColor = .white
            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
            self.randomGroups[self.selectedIndex]["is_joined"] = "no"
            
        }
        else{
            cell.likeBtn.setTitle(NSLocalizedString("Joined", comment: "Joined"), for: .normal)
            cell.likeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
            cell.likeBtn.setTitleColor(.white, for: .normal)
            self.randomGroups[self.selectedIndex]["is_joined"] = "yes"
        }
    }
    
}

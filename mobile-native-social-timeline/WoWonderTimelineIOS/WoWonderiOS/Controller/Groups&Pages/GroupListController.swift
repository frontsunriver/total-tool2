
import UIKit
import WoWonderTimelineSDK

class GroupListController: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    @IBOutlet weak var exploreLabel: UILabel!
    @IBOutlet weak var suggestedBtn: UIButton!

    var groupList = [[String:Any]]()
    var myGroups  = [[String:Any]]()
    let status = Reach().connectionStatus()
    
    var offset = "0"
    var isJoined = true
    var isAdmin = false
    var selctedIndex = 0
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationItem.hidesBackButton = true
        self.navigationController?.navigationBar.isHidden = true
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.suggestedBtn.setTitle(NSLocalizedString("Suggested Groups", comment: "Suggested Groups"), for: .normal)
        self.exploreLabel.text = NSLocalizedString("Explore Groups", comment: "Explore Groups")
        self.tableView.tableFooterView = UIView()
        self.activityIndicator.startAnimating()
        //        self.getGroups()
        self.getJoinedGroups(user_id: UserData.getUSER_ID()!)
    }
    
    override func viewWillAppear(_ animated: Bool) {
        self.navigationItem.hidesBackButton = true
        self.navigationController?.navigationBar.isHidden = true
    }
    
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    private func getmyGroups() {
        switch status {
        case .unknown, .offline:
            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetMyGroupsManager.sharedInstance.getMyGroups(userId: UserData.getUSER_ID()!, offset: self.offset) { (success, authError, error) in
                    if success != nil {
                        print(success!.data)
                        for i in success!.data{
                            self.myGroups.append(i)
                        }
                        //        self.myGroups = self.groupList.filter({$0["user_id"] as? String == UserData.getUSER_ID()!})
                        //            print(self.myGroups.count)
                        //        self.groupList = self.groupList.filter({$0["user_id"] as? String != UserData.getUSER_ID()!})
                        //                     print(self.groupList.count)
                        
                        self.tableView.reloadData()
                    }
                    else if authError != nil {
                        self.showAlert(title: "", message: (authError?.errors.errorText)!)
                    }
                    else if error != nil {
                        print(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    
    private func getJoinedGroups(user_id: String){
        switch status {
        case .unknown, .offline:
            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            DispatchQueue.main.async {
                Get_User_DataManagers.sharedInstance.get_User_Data(userId: user_id, access_token: "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)") { [weak self] (success, authError, error) in
                    if success != nil {
                        for i in success!.joined_groups{
                            self?.groupList.append(i)
                        }
                        self?.myGroups = (self?.groupList.filter({$0["user_id"] as? String == UserData.getUSER_ID()!})) ?? []
                        self?.groupList = self?.groupList.filter({$0["user_id"] as? String != UserData.getUSER_ID()!}) ?? []
                        self?.tableView.reloadData()
                        self?.activityIndicator.stopAnimating()
                    }
                    else if authError != nil {
                        self?.showAlert(title: "", message: (authError?.errors.errorText)!)
                    }
                    else if error != nil {
                        print(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    private func sendJoinRequest (groupId : String) {
        performUIUpdatesOnMain {
            JoinGroupManager.sharedInstance.joinGroup(groupId: Int(groupId)!) { (success, authError, error) in
                if success != nil {
                    print(success?.join_status)
                }
                else if authError != nil {
                    print(authError?.errors.errorText)
                }
                else if error != nil {
                    print(error?.localizedDescription)
                }
            }
        }
    }
    
    
    
    
    @IBAction func SuggestedGroups(_ sender: Any) {
        let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "GroupsDiscoverVC") as! GroupsDiscoverController
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
    @IBAction func Create(_ sender: Any) {
        let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "CreateGroupVC") as! CreateGroupController
        vc.modalPresentationStyle = .fullScreen
        vc.modalTransitionStyle = .coverVertical
        vc.delegate = self
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func Back(_ sender: Any) {
        self.navigationController?.popViewController(animated: true)
    }
    
}
extension GroupListController:UITableViewDelegate,UITableViewDataSource,CreateGroupDelegate,DeleteGroupDelegate,JoinGroupDelegate{
    
    func numberOfSections(in tableView: UITableView) -> Int {
        return 2
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if section == 0 {
            return 1
        }
        else {
            return self.groupList.count
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        if indexPath.section == 0 {
            let cell = tableView.dequeueReusableCell(withIdentifier: "MyGroup") as! GetUserGroup
            cell.groupArray = self.myGroups
            cell.didSelectItemAction = {[weak self] indexPath in
                self?.gotoGroupController(indexPath: indexPath)
            }
            cell.groupCollectionView.reloadData()
            return cell
        }
            
        else {
            let cell = tableView.dequeueReusableCell(withIdentifier: "JoinGroup") as! JoinedGroupCell
            let index =  self.groupList[indexPath.row]
            if let image = index["avatar"] as? String {
                let trimmedString = image.trimmingCharacters(in: .whitespaces)
                print(trimmedString)
                let url = URL(string: trimmedString)
                cell.groupIcon.kf.setImage(with: url)
            }
            if let groupname = index["group_title"] as? String {
                cell.groupName.text = groupname
            }
           
            cell.joinedBtn.tag = indexPath.row
            cell.joinedBtn.addTarget(self, action: #selector(self.JoinGroup(sender:)), for: .touchUpInside)
            return cell
        }
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        if indexPath.section == 0 {
            if self.myGroups.count == 0 {
                return 0
            }
            else {
                return 130.0
            }
        }
        else {
            return 100.0
        }
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        if indexPath.section == 1{
            let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "GroupVC") as! GroupController
            let index = self.groupList[indexPath.row]
            self.selctedIndex = indexPath.row
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
            vc.isJoined = self.isJoined
            vc.delegte1 = self
            vc.delegate = self
            vc.groupData = index
            self.navigationController?.pushViewController(vc, animated: true)
        }
    }
    
    func tableView(_ tableView: UITableView, viewForHeaderInSection section: Int) -> UIView? {
        if section == 0 {
            
            let headerView = UIView.init(frame: CGRect.init(x: 0, y: 0, width: tableView.frame.width, height: 50))
            let label = UILabel()
            label.frame = CGRect.init(x: 10, y: 5, width: headerView.frame.width-10, height: headerView.frame.height-10)
            label.text = "\("  ")\(NSLocalizedString("Manage Group", comment: "Manage Group"))"
            label.textColor = .black
            label.backgroundColor = UIColor.hexStringToUIColor(hex: "#E4E6E8")
            headerView.addSubview(label)
            return headerView
        }
        else {
            
            
            let headerView = UIView.init(frame: CGRect.init(x: 0, y: 0, width: tableView.frame.width, height: 50))
            
            let label = UILabel()
            label.frame = CGRect.init(x: 10, y: 5, width: headerView.frame.width-10, height: headerView.frame.height-10)
            label.text = "\("  ")\(NSLocalizedString("Joined Group", comment: "Joined Group"))"
            label.textColor = .black
            label.backgroundColor = UIColor.hexStringToUIColor(hex: "#E4E6E8")
            headerView.addSubview(label)
            return headerView
        }
    }
    func tableView(_ tableView: UITableView, heightForHeaderInSection section: Int) -> CGFloat {
        if section == 0{
            if self.groupList.count == 0{
                return 0
            }
            else {
                return 50
            }
        }
        else {
            if self.groupList.count == 0{
                return 0
            }
            else {
                return 50
            }
        }
    }
    
    func sendGroupData(groupData: [String : Any]) {
        print("delegate")
        self.myGroups.insert(groupData, at: 0)
        self.tableView.reloadData()
    }
    func deleteGroup(groupId: String) {
        self.myGroups = self.myGroups.filter({$0["group_id"] as? String != groupId})
        self.tableView.reloadData()
    }
    func joinGroup(isJoin: Bool) {
        
        let indexPath = IndexPath(row: self.selctedIndex, section: 1)
        let cell = tableView.cellForRow(at: indexPath) as! JoinedGroupCell
        if isJoin == true{
            cell.joinedBtn.setTitle(NSLocalizedString("JOINED", comment: "JOINED"), for: .normal)
            cell.joinedBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
            self.groupList[self.selctedIndex]["is_joined"] = true
            self.isJoined = true
        }
        else {
            cell.joinedBtn.setTitle(NSLocalizedString("JOIN GROUP", comment: "JOIN GROUP"), for: .normal)
            cell.joinedBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#000000"), for: .normal)
            self.groupList[self.selctedIndex]["is_joined"] = false
            self.isJoined = false
        }
    }
    
    
    func gotoGroupController(indexPath:IndexPath){
        let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "GroupVC") as! GroupController
        let index = self.myGroups[indexPath.row]
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
        if let about = index["about"] as? String{
            vc.aboutGroup = about
        }
        if let categoryId = index["category_id"] as? String{
            vc.categoryId = categoryId
        }
        vc.isAdmin = true
        vc.delegate = self
        vc.isFromList = true
        vc.groupData = index
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
    
    @IBAction func JoinGroup(sender : UIButton){
        let cell = self.tableView.cellForRow(at: IndexPath(row: sender.tag, section:1)) as! JoinedGroupCell
        let index = self.groupList[sender.tag]
        var group_id: String? = nil
        if let groupId = index["group_id"] as? String{
            group_id = groupId
        }
        if let is_Joined = index["is_joined"] as? Bool{
            if is_Joined == true{
                cell.joinedBtn.setTitle(NSLocalizedString("JOIN GROUP", comment: "JOIN GROUP"), for: .normal)
                cell.joinedBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#000000"), for: .normal)
                self.isJoined = false
                self.sendJoinRequest(groupId: group_id ?? "")
                self.groupList[sender.tag]["is_joined"] = false
            }
            else{
                cell.joinedBtn.setTitle(NSLocalizedString("JOINED", comment: "JOINED"), for: .normal)
                cell.joinedBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                self.isJoined = true
                self.sendJoinRequest(groupId: group_id ?? "")
                self.groupList[sender.tag]["is_joined"] = true
            }
        }
    }
}

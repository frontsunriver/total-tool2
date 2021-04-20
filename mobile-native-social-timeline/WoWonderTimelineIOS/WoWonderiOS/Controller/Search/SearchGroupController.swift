
import UIKit
import XLPagerTabStrip
import WoWonderTimelineSDK
class SearchGroupController: UIViewController,IndicatorInfoProvider,SearchDelegate,JoinGroupDelegate{
    func filterSearch(gender: String, countryId: String, ageTo: String, ageFrom: String, verified: String, status: String, profilePic: String) {
    print("abc")
    }
    
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var noContentView: UIView!
    
    @IBOutlet weak var noResultLbl: UILabel!
    @IBOutlet weak var descLbl: UILabel!
    @IBOutlet weak var searchBtn: RoundButton!
    
    
    
    let status = Reach().connectionStatus()
    var groups = [[String:Any]()]
    var selctedIndex = 0

    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(self.LoadGroups(notification:)), name: NSNotification.Name(rawValue: "loadGroup"), object: nil)
        self.tableView.register(UINib(nibName: "LikePagesCell", bundle: nil), forCellReuseIdentifier: "LikePage")
        self.noContentView.isHidden = false
        self.tableView.isHidden = true
        self.tableView.tableFooterView = UIView()
        self.noResultLbl.text = NSLocalizedString("Sad no result!", comment: "Sad no result!")
        self.descLbl.text = NSLocalizedString("We cannot find the keyword  you are searching from maybe a little spelling mistake ?", comment: "We cannot find the keyword  you are searching from maybe a little spelling mistake ?")
        self.searchBtn.setTitle(NSLocalizedString("Search Random", comment: "Search Random"), for: .normal)
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
     func indicatorInfo(for pagerTabStripController: PagerTabStripViewController) -> IndicatorInfo {
        return IndicatorInfo(title: NSLocalizedString("GROUPS", comment: "GROUPS"))
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
    
       @IBAction func LoadGroups(notification: NSNotification){
        self.groups.removeAll()
        self.tableView.reloadData()
            if let data = notification.userInfo?["groupData"] as? [[String:Any]] {
                self.groups = data
            }
    
            if self.groups.count == 0{
                self.noContentView.isHidden = false
                self.tableView.isHidden = true
            }
            else {
                self.noContentView.isHidden = true
                self.tableView.isHidden = false
            }
            self.tableView.reloadData()
        }
    
    
    
    @IBAction func RandomSearch(_ sender: Any) {
        let userInfo =  ["gender": "","country":  "","verified":  "","status": "","profilePic": "","filterbyage":  "","age_from": "", "age_to": "","keyword": ""] as [String : Any]
    NotificationCenter.default.post(name: NSNotification.Name(rawValue: "loadFilterData"), object: nil, userInfo: userInfo)
    let parentViewController = self.parent as! SearchController
        parentViewController.moveToViewController(at: 0)
    }
    
    
    func joinGroup(isJoin: Bool) {
        
        let indexPath = IndexPath(row: self.selctedIndex, section: 0)
        let cell = tableView.cellForRow(at: indexPath) as! LikePagesCell
        if isJoin == true{
            cell.likeBtn.setTitle(NSLocalizedString("JOINED", comment: "JOINED"), for: .normal)
            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#FFFFFF"), for: .normal)
            cell.likeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
            self.groups[self.selctedIndex]["is_joined"] = "yes"
//            self.isJoined = true
        }
        else {
            cell.likeBtn.setTitle(NSLocalizedString("Join", comment: "Join"), for: .normal)
            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
            cell.likeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#FFFFFF")
            self.groups[self.selctedIndex]["is_joined"] = "no"
//            self.isJoined = false
        }
    }
    
}

extension SearchGroupController: UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.groups.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let index = self.groups[indexPath.row]
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
        
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "GroupVC") as! GroupController
        let index = self.groups[indexPath.row]
        self.selctedIndex = indexPath.row
        vc.delegte1 = self
        vc.groupData = index
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 80.0
    }
    
    @IBAction func Groupjoin(sender: UIButton){
        let cell = self.tableView.cellForRow(at: IndexPath(row: sender.tag, section: 0)) as! LikePagesCell
        let index = self.groups[sender.tag]
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
                self.groups[sender.tag]["is_joined"] = "yes"
            }
            else{
                cell.likeBtn.setTitle(NSLocalizedString("Join", comment: "Join"), for: .normal)
                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                cell.likeBtn.backgroundColor = .white
                self.JoinGroup(groupId: group_id ?? "")
                self.groups[sender.tag]["is_joined"] = "no"
            }
        }
    }
    
    func locationSearch(location: String, countryId: String) {
        print("")
    }
    func sendSearchData(data: [[String : Any]]) {
        self.groups = data
        if self.groups.count == 0{
            self.noContentView.isHidden = false
            self.tableView.isHidden = true
        }
        else {
            self.noContentView.isHidden = true
            self.tableView.isHidden = false
        }
        self.tableView.reloadData()
    }
    
    
}

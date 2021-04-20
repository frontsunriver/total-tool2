

import UIKit
import XLPagerTabStrip
import WoWonderTimelineSDK
class SearchUserController: UIViewController,IndicatorInfoProvider{
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var noContentView: UIView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    @IBOutlet weak var noResultLbl: UILabel!
    @IBOutlet weak var descLbl: UILabel!
    @IBOutlet weak var searchBtn: RoundButton!
    
    let status = Reach().connectionStatus()
    var delegate: SearchDelegate?
    
    var users = [[String:Any]]()
    var countryId = ""
    var _status = ""
    var verified = ""
    var gender = ""
    var filterage = ""
    var ageFrom = ""
    var ageTo = ""
    var keyword = ""
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(self.LoadFilterData(notification:)), name: NSNotification.Name(rawValue: "loadFilterData"), object: nil)
        self.noContentView.isHidden = false
        self.tableView.isHidden = true
        self.tableView.tableFooterView = UIView()
        self.activityIndicator.stopAnimating()
        self.tableView.register(UINib(nibName: "MyFollowingCell", bundle: nil), forCellReuseIdentifier: "MyFollower&FollowingCell")
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
    @IBAction func LoadFilterData(notification: NSNotification){
        self.users.removeAll()
        self.tableView.reloadData()
        self.activityIndicator.startAnimating()
        if let gender = notification.userInfo?["gender"] as? String{
            self.gender = gender
        }
        if let country = notification.userInfo?["country"] as? String{
            self.countryId = country
        }
        if let verified = notification.userInfo?["verified"] as? String{
            self.verified = verified
        }
        if let status = notification.userInfo?["status"] as? String{
            self._status = status
        }
        if let profilePic = notification.userInfo?["profilePic"] as? String{
            print("profilepic")
        }
        if let filterbyage = notification.userInfo?["filterbyage"] as? String{
            print(filterbyage)
            self.filterage = filterbyage
        }
        if let keyword = notification.userInfo?["keyword"] as? String{
            self.keyword = keyword
        }
        if let age_from = notification.userInfo?["age_from"] as? String{
            print(age_from)
            self.ageFrom = age_from
        }
        if let age_to = notification.userInfo?["age_to"] as? String{
            print(age_to)
            self.ageTo = age_to
        }
        self.getSearchData()
    }
    private func getSearchData(){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetSearchDataManager.sharedInstance.getSearchData(search_keyword: self.keyword, country: self.countryId, status: self._status, verified: self.verified, gender: self.gender, filterbyage: self.filterage, age_from: self.ageFrom, age_to: self.ageTo) { (success, authError, error) in
                    if success != nil{
                        for i in success!.users{
                            self.users.append(i)
                        }
                        let userInfo =  ["pageData" : success!.pages]
                        let usersInfo = ["groupData" : success!.groups]
                        NotificationCenter.default.post(name: NSNotification.Name(rawValue: "loadpage"), object: nil, userInfo: userInfo)
                         NotificationCenter.default.post(name: NSNotification.Name(rawValue: "loadGroup"), object: nil, userInfo: usersInfo)
                        if self.users.count == 0{
                            self.tableView.isHidden = true
                            self.noContentView.isHidden = false
                            self.activityIndicator.stopAnimating()
                        }
                        else {
                            self.tableView.isHidden = false
                            self.noContentView.isHidden = true
                            self.activityIndicator.stopAnimating()
                        }
                        print(self.users)
                        self.tableView.reloadData()
                    }
                    else if authError != nil{
                        self.view.makeToast(authError?.errors.errorText)
                        self.tableView.isHidden = false
                        self.noContentView.isHidden = true
                    }
                    else if error != nil{
                        self.view.makeToast(error?.localizedDescription)
                        self.tableView.isHidden = false
                        self.noContentView.isHidden = true
                    }
                }
            }
        }
    }
      private func follow_unfollowRequest(userId : String){
        switch status {
         case .unknown, .offline:
             self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
         case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                Follow_RequestManager.sharedInstance.sendFollowRequest(userId: userId) { (success, authError, error) in
                    if success != nil {
                      print(success?.follow_status)
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
      }
    
    @IBAction func SearchRandom(_ sender: Any) {
        self.activityIndicator.startAnimating()
        self.noContentView.isHidden = true
        self.getSearchData()
    }
    
    func indicatorInfo(for pagerTabStripController: PagerTabStripViewController) -> IndicatorInfo {
        return IndicatorInfo(title: NSLocalizedString("USERS", comment: "USERS"))
    }

}
extension SearchUserController: UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        var index = self.users[indexPath.row]
        let cell = tableView.dequeueReusableCell(withIdentifier: "MyFollower&FollowingCell") as! MyFollowing_MyFollowerCell
        if let image = index["avatar"] as? String{
            let url = URL(string: image)
            cell.profileImage.kf.setImage(with: url)
        }
        if let name = index["username"] as? String{
            cell.userName.text = name
        }
        if let isFollowing = index["is_following"] as? Int{
            if isFollowing == 0{
                if (AppInstance.instance.connectivity_setting == "1"){
                cell.followingBtn.setTitle(NSLocalizedString("AddFriend", comment: "AddFriend"), for: .normal)
                }
                else{
                cell.followingBtn.setTitle(NSLocalizedString("Follow", comment: "Follow"), for: .normal)
                }
                cell.followingBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                cell.followingBtn.backgroundColor = .white
            }
            else if (isFollowing == 2){
               if (AppInstance.instance.connectivity_setting == "1"){
                    cell.followingBtn.setTitle(NSLocalizedString("Requested", comment: "Requested"), for: .normal)
                   cell.followingBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                   cell.followingBtn.backgroundColor = .white
                }
               else{
                  cell.followingBtn.setTitle(NSLocalizedString("following", comment: "following"), for: .normal)
                      cell.followingBtn.setTitleColor(.white, for: .normal)
                           cell.followingBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                }
            }
                
            else {
                if (AppInstance.instance.connectivity_setting == "0"){
                    cell.followingBtn.setTitle(NSLocalizedString("following", comment: "following"), for: .normal)
                }
                else{
                cell.followingBtn.setTitle(NSLocalizedString("Friends", comment: "Friends"), for: .normal)
            }
        cell.followingBtn.setTitleColor(.white, for: .normal)
        cell.followingBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
        }
        }
        cell.followingBtn.tag = indexPath.row
        cell.followingBtn.addTarget(self, action: #selector(self.sendRequest(sender:)), for: .touchUpInside)
        return cell
    }
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.users.count
    }
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 80.0
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
            let index = self.users[indexPath.row]
              let storyBoard = UIStoryboard(name: "Main", bundle: nil)
              let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
              vc.userData = index
              self.navigationController?.pushViewController(vc, animated: true)
    }
    
    @IBAction func sendRequest(sender: UIButton){
        let cell = self.tableView.cellForRow(at: IndexPath(row: sender.tag, section: 0)) as! MyFollowing_MyFollowerCell
        let index = self.users[sender.tag]
        var user_id: String? = nil
        if let userId = index["user_id"] as? String{
            user_id = userId
        }
        if let isFollowing = index["is_following"] as? Int{
            if isFollowing == 0{
                if (AppInstance.instance.connectivity_setting == "0"){
                    cell.followingBtn.setTitle(NSLocalizedString("Following", comment: "Following"), for: .normal)
                    cell.followingBtn.setTitleColor(.white, for: .normal)
                    cell.followingBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                    self.follow_unfollowRequest(userId: user_id ?? "0")
                    self.users[sender.tag]["is_following"] = 1
                }
                else{
                    if (isFollowing == 0){
                        cell.followingBtn.setTitle(NSLocalizedString("Requested", comment: "Requested"), for: .normal)
                        cell.followingBtn.setTitleColor( UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                        cell.followingBtn.backgroundColor = .white
                        self.users[sender.tag]["is_following"] = 2
                    }
                    else{
                        self.users[sender.tag]["is_following"] = 1
                        cell.followingBtn.setTitle(NSLocalizedString("MyFriend", comment: "MyFriend"), for: .normal)
                        cell.followingBtn.setTitleColor(.white, for: .normal)
                        cell.followingBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                    }
                }
                self.follow_unfollowRequest(userId: user_id ?? "0")
                
            }
            else{
                if (AppInstance.instance.connectivity_setting == "0"){
                    cell.followingBtn.setTitle(NSLocalizedString("Follow", comment: "Follow"), for: .normal)

                }
                else{
                    cell.followingBtn.setTitle(NSLocalizedString("Add Friend", comment: "Add Friend"), for: .normal)
                }
                cell.followingBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                cell.followingBtn.backgroundColor = .white
                self.follow_unfollowRequest(userId: user_id ?? "0")
                self.users[sender.tag]["is_following"] = 0
            }
        }
    }
    
}

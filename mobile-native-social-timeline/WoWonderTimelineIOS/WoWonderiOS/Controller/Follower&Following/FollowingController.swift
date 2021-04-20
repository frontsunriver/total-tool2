
import WoWonderTimelineSDK
import UIKit
import Kingfisher
import Toast_Swift
import GoogleMobileAds


class FollowingController: UIViewController {
    
    var followingArray = [[String:Any]]()
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    var following_offset: String? = nil
    var follower_offset:String? = nil
    var userId: String? = nil
    var type: String? = nil
    var navTitle: String? = nil
    let status = Reach().connectionStatus()
    let spinner = UIActivityIndicatorView(style: .gray)
    var interstitial: GADInterstitial!

    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.title = navTitle
        self.navigationItem.largeTitleDisplayMode = .never
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.tableView.tableFooterView = UIView()
        self.tableView.register(UINib(nibName: "MyFollowingCell", bundle: nil), forCellReuseIdentifier: "MyFollower&FollowingCell")
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.activityIndicator.startAnimating()
//        if self.userId == UserData.getUSER_ID(){
//            self.getMyFriends(user_id: self.userId ?? "", type: "following")
//        }
//        else{
            self.getMyFriends(user_id: self.userId ?? "", type: self.type ?? "")
//        }
        if ControlSettings.shouldShowAddMobBanner{
                                
                              
                                interstitial = GADInterstitial(adUnitID:  ControlSettings.interestialAddUnitId)
                                let request = GADRequest()
                                interstitial.load(request)
                            }
        
    }
    func CreateAd() -> GADInterstitial {
              let interstitial = GADInterstitial(adUnitID:  ControlSettings.interestialAddUnitId)
              interstitial.load(GADRequest())
              return interstitial
          }
    
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    
    private func getMyFollowing(){
        switch status {
        case .unknown, .offline:
            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            Get_User_DataManagers.sharedInstance.get_User_Data(userId: UserData.getUSER_ID()!, access_token: "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)") { (success, authError, error) in
                if success != nil {
                    self.followingArray = (success!.following.map({$0}))
                    self.tableView.separatorStyle = .singleLine
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
    
    private func getMyFriends(user_id: String,type: String){
        switch status {
        case .unknown, .offline:
            self.view.makeToast("Internet Connection Faield")
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetFriendsManager.sharedInstance.getFriends(type: type, user_id: user_id, following_offset: self.following_offset ?? "", followers_offset: self.follower_offset ?? "") { (success, authError, error) in
                    if success != nil{
                        if let data = success?.data{
                            if self.type == "followers"{
                            if let followersArray = data["followers"] as? [[String:Any]]{
                                for i in followersArray{
                                    self.followingArray.append(i)
                                }
                            }
                            }else{
                            if let followingArray = data["following"] as? [[String:Any]]{
                                for i in followingArray{
                                    self.followingArray.append(i)
                                }
                            }
                          }
                        }
                        self.follower_offset = self.followingArray.last?["user_id"] as? String ?? "0"
                        self.following_offset = self.followingArray.last?["user_id"] as? String ?? "0"
                        self.spinner.stopAnimating()
                        self.activityIndicator.stopAnimating()
                        self.tableView.reloadData()
                    }
                    else if authError != nil {
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil{
                        self.view.makeToast(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    
    private func follow_unfollowRequest(userId : String){
        switch status {
        case .unknown, .offline:
            self.view.makeToast("Internet Connection Faield")
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                Follow_RequestManager.sharedInstance.sendFollowRequest(userId: userId) { (success, authError, error) in
                    if success != nil {
                        print(success?.follow_status)
//                        self.view.makeToast(success?.follow_status)
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
    
}
extension  FollowingController : UITableViewDelegate,UITableViewDataSource{
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.followingArray.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "MyFollower&FollowingCell") as! MyFollowing_MyFollowerCell
        let index = self.followingArray[indexPath.row]
        if let image = index["avatar"] as? String{
            let url = URL(string: image)
            cell.profileImage.kf.setImage(with: url)
        }
        if let name = index["name"] as? String{
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
//        ///////////////////////
//        if let isFollowing = index["is_following"] as? Int{
//            if isFollowing == 0{
//                cell.followingBtn.setTitle(NSLocalizedString("Follow", comment: "Follow"), for: .normal)
//                cell.followingBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
//                cell.followingBtn.backgroundColor = .white
//            }
//            else {
//                cell.followingBtn.setTitle(NSLocalizedString("Following", comment: "Following"), for: .normal)
//                cell.followingBtn.setTitleColor(.white, for: .normal)
//                cell.followingBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
//            }
//
//
//
//        }
        cell.followingBtn.tag = indexPath.row
        cell.followingBtn.addTarget(self, action: #selector(self.sendRequest(sender:)), for: .touchUpInside)
        return cell
    }
    
    
    func tableView(_ tableView: UITableView, willDisplay cell: UITableViewCell, forRowAt indexPath: IndexPath) {
        
        if self.followingArray.count >= 15 {
            let count = self.followingArray.count
            let lastElement = count - 1
            
            if indexPath.row == lastElement {
                spinner.startAnimating()
                spinner.frame = CGRect(x: CGFloat(0), y: CGFloat(0), width: tableView.bounds.width, height: CGFloat(44))
                self.tableView.tableFooterView = spinner
                self.tableView.tableFooterView?.isHidden = false
                self.getMyFriends(user_id: self.userId ?? "", type: self.type ?? "")
            }
        }
    }
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 80.0
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
        let index = self.followingArray[indexPath.row]
        let storyBoard = UIStoryboard(name: "Main", bundle: nil)
        let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
        vc.userData = index
        //         if let data = notification.userInfo?["userData"] as? [String:Any]{
        //             print(data)
        //             vc.userData = data
        //         }
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
    
    @IBAction func sendRequest(sender: UIButton){
        let cell = self.tableView.cellForRow(at: IndexPath(row: sender.tag, section: 0)) as! MyFollowing_MyFollowerCell
        let index = self.followingArray[sender.tag]
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
                    self.followingArray[sender.tag]["is_following"] = 1
                }
                else{
                    if (isFollowing == 0){
                        cell.followingBtn.setTitle(NSLocalizedString("Requested", comment: "Requested"), for: .normal)
                        cell.followingBtn.setTitleColor( UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                        cell.followingBtn.backgroundColor = .white
                        self.followingArray[sender.tag]["is_following"] = 2
                    }
                    else{
                        self.followingArray[sender.tag]["is_following"] = 1
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
                self.followingArray[sender.tag]["is_following"] = 0
            }
        }
//        if let isFollowing = index["is_following"] as? Int{
//            if isFollowing == 0{
//                cell.followingBtn.setTitle(NSLocalizedString("Following", comment: "Following"), for: .normal)
//                cell.followingBtn.setTitleColor(.white, for: .normal)
//                cell.followingBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
//                self.follow_unfollowRequest(userId: user_id ?? "0")
//                self.followingArray[sender.tag]["is_following"] = 1
//            }
//            else{
//                cell.followingBtn.setTitle(NSLocalizedString("Follow", comment: "Follow"), for: .normal)
//                cell.followingBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
//                cell.followingBtn.backgroundColor = .white
//                self.follow_unfollowRequest(userId: user_id ?? "0")
//                self.followingArray[sender.tag]["is_following"] = 0
//            }
//        }
    }
    
}

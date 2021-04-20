

import UIKit
import WoWonderTimelineSDK
class SelectUserController: UIViewController,UISearchBarDelegate{
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var searchBar: UISearchBar!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    var delegate: selecteUSerDelegate?
    
    let status = Reach().connectionStatus()
    let spinner = UIActivityIndicatorView(style: .gray)
    var followingArray = [[String:Any]]()
    var searchList = [[String:Any]]()
    
    var following_offset: String? = nil
    var type = "following"
    var searching = false
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.tableView.register(UINib(nibName: "MyFollowingCell", bundle: nil), forCellReuseIdentifier: "MyFollower&FollowingCell")
        self.activityIndicator.startAnimating()
        self.tableView.tableFooterView = UIView()
        self.searchBar.delegate = self
        self.setUPSearchField()
        self.getMyFriends(user_id: UserData.getUSER_ID()!, type: "following")
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    
    private func setUPSearchField(){
        if let textfield = self.searchBar.value(forKey: "searchField") as? UITextField {
            textfield.clearButtonMode = .never
            textfield.backgroundColor = .clear
            //                UIColor.hexStringToUIColor(hex: "#984243")
            textfield.attributedPlaceholder = NSAttributedString(string:" Search...", attributes:[NSAttributedString.Key.foregroundColor: UIColor.yellow])
            textfield.textColor = .white
            if let leftView = textfield.leftView as? UIImageView {
                leftView.image = leftView.image?.withRenderingMode(.alwaysTemplate)
                leftView.tintColor = UIColor.white
            }
        }
    }
    
    
    private func getMyFriends(user_id: String,type: String){
        switch status {
        case .unknown, .offline:
            self.view.makeToast("Internet Connection Faield")
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetFriendsManager.sharedInstance.getFriends(type: type, user_id: user_id, following_offset: self.following_offset ?? "", followers_offset: "") { (success, authError, error) in
                    if success != nil{
                        if let data = success?.data as? [String:Any]{
                            if let followingArray = data["following"] as? [[String:Any]]{
                                for i in followingArray{
                                    self.followingArray.append(i)
                                }
                            }
                        }
                        
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
    
    
    @IBAction func Cancel(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
}
extension SelectUserController: UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if searching{
            return self.searchList.count
        }
        else{
            return self.followingArray.count
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "MyFollower&FollowingCell") as! MyFollowing_MyFollowerCell
        if self.searching  {
            
            let index = self.searchList[indexPath.row]
            if let image = index["avatar"] as? String{
                let url = URL(string: image)
                cell.profileImage.kf.setImage(with: url)
            }
            if let name = index["name"] as? String{
                cell.userName.text = name
            }
            cell.followingBtn.isHidden = true
            
            return cell
        }
        else{
            let index = self.followingArray[indexPath.row]
            if let image = index["avatar"] as? String{
                let url = URL(string: image)
                cell.profileImage.kf.setImage(with: url)
            }
            if let name = index["name"] as? String{
                cell.userName.text = name
            }
            cell.followingBtn.isHidden = true
            
            return cell
        }
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
      var username: String? = nil
      var id: String? = nil
        if searching{
            let index  = self.searchList[indexPath.row]
          
            if let name = index["name"] as? String{
                username = name
               
            }
            if let userid = index["user_id"] as? String{
                id = userid
            }
            self.dismiss(animated: true) {
                self.delegate?.selectUser(name: username ?? "", user_id: id ?? "")
            }
        }
        else {
            let index  = self.followingArray[indexPath.row]
            if let name = index["name"] as? String{
                username = name
               
            }
            if let userid = index["user_id"] as? String{
                id = userid
            }
            self.dismiss(animated: true) {
                self.delegate?.selectUser(name: username ?? "", user_id: id ?? "")
            }
        }
        
    }
    
    func searchBar(_ searchBar: UISearchBar, textDidChange searchText: String) {
        self.searchList = self.followingArray.filter({($0["name"] as! String).prefix(searchText.count) == searchText})
        print(self.searchList)
        self.searching = true
        self.tableView.reloadData()
    }
    func searchBarSearchButtonClicked(_ searchBar: UISearchBar) {
        self.searchBar.resignFirstResponder()
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
                self.getMyFriends(user_id: UserData.getUSER_ID()!, type: "following")
            }
        }
    }
    
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 80.0
    }
    
    
    
    
    
}


import UIKit
import Kingfisher
import Toast_Swift
import WoWonderTimelineSDK


class GroupMembersController: UIViewController {

    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    @IBOutlet weak var memebrLabel: UILabel!
    
    var usersArray = [[String:Any]]()
    var groupId: String? = nil
    var offset: String? = nil
    let spinner = UIActivityIndicatorView(style: .gray)
    let pulltoRefresh = UIRefreshControl()
    let status = Reach().connectionStatus()

    
    override func viewDidLoad() {
        super.viewDidLoad()
        print(self.groupId)
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        self.activityIndicator.startAnimating()
        self.tableView.tableFooterView = UIView()
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.tableView.addSubview(pulltoRefresh)
       self.getGroupMembers(groupId: self.groupId ?? "", offset: self.offset ?? "")
        self.memebrLabel.text = NSLocalizedString("Group Members", comment: "Group Members")
    }
    ///Network Connectivity.
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
            
        }
    }
    //Pull To Refresh
    @objc func refresh(){
        self.offset = ""
        self.spinner.stopAnimating()
        self.usersArray.removeAll()
        self.tableView.reloadData()
        self.getGroupMembers(groupId: self.groupId ?? "", offset: self.offset ?? "")
    }

    @IBAction func Back(_ sender: Any) {
        self.navigationController?.popViewController(animated: true)
    }
    
    @IBAction func AddMember(_ sender: Any) {
        let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "InviteFriendVC") as! InviteFriendsController
        vc.groupId = self.groupId ?? ""
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
  
    @IBAction func More(sender: UIButton){
        let alert = UIAlertController(title: "", message: NSLocalizedString("More", comment: "More"), preferredStyle: .actionSheet)

        alert.setValue(NSAttributedString(string: alert.message ?? "", attributes: [NSAttributedString.Key.font : UIFont.systemFont(ofSize: 20, weight: UIFont.Weight.medium), NSAttributedString.Key.foregroundColor : UIColor.black]), forKey: "attributedMessage")
        
        alert.addAction(UIAlertAction(title: NSLocalizedString("Block Member", comment: "Block Member"), style: .default, handler: { (_) in
            switch self.status {
            case .unknown, .offline:
                self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
            case .online(.wwan), .online(.wiFi):
                var user_id: String? = nil
                if let  userId = self.usersArray[sender.tag]["user_id"] as? String{
                    user_id = userId
                }
                self.blockUser(userId: user_id ?? "")
                self.usersArray.remove(at: sender.tag)
                self.tableView.reloadData()
            }
        }))

        alert.addAction(UIAlertAction(title: NSLocalizedString("Profile", comment: "Profile"), style: .default, handler: { (_) in
            let storyBoard = UIStoryboard(name: "Main", bundle: nil)
            let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController

            vc.userData = self.usersArray[sender.tag]
            self.navigationController?.pushViewController(vc, animated: true)
        }))
        
        alert.addAction(UIAlertAction(title: NSLocalizedString("Close", comment: "Close"), style: .cancel, handler: { (_) in
            print("User click Dismiss button")
        }))
        if let popoverController = alert.popoverPresentationController {
            popoverController.sourceView = self.view
            popoverController.sourceRect = CGRect(x: self.view.bounds.midX, y: self.view.bounds.midY, width: 0, height: 0)
            popoverController.permittedArrowDirections = []
        }
        self.present(alert, animated: true, completion: {
            print("completion block")
        })
    }
    
    private func getGroupMembers(groupId:String,offset :String){
  switch status {
   case .unknown, .offline:
       self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
   case .online(.wwan), .online(.wiFi):
    performUIUpdatesOnMain {
        GetGroupMemberManager.sharedInstance.getGroupMember(groupId: groupId, offset: offset) { (success, authError, error) in
            if success != nil {
                for i in success!.users{
                self.usersArray.append(i)
                }
                self.offset = self.usersArray.last?["group_id"] as? String ?? "0"
                self.activityIndicator.stopAnimating()
                self.pulltoRefresh.endRefreshing()
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
    private func blockUser(userId: String){
        Block_UserManager.sharedInstance.blockUser(user_Id: userId, blockUser: "block") { (success, authError, error) in
            if success != nil{
                self.view.makeToast(success?.block_status)
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



extension GroupMembersController : UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
       return self.usersArray.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "MemberCell") as! GroupMemberCell
        let index = self.usersArray[indexPath.row]
        if let name = index["name"] as? String {
            cell.memberName.text = name
        }
       if let image = index["avatar"] as? String{
            let url = URL(string: image)
        cell.memberProfile.kf.setImage(with: url)
        }
        cell.moreBtn.tag = indexPath.row
        cell.moreBtn.addTarget(self, action: #selector(self.More(sender:)), for: .touchUpInside)
        
        return cell
    }
    func tableView(_ tableView: UITableView, willDisplay cell: UITableViewCell, forRowAt indexPath: IndexPath) {
        if self.usersArray.count >= 10 {
            let count = self.usersArray.count
            let lastElement = count - 1
            
            if indexPath.row == lastElement {
                spinner.startAnimating()
                spinner.frame = CGRect(x: CGFloat(0), y: CGFloat(0), width: tableView.bounds.width, height: CGFloat(44))
                self.tableView.tableFooterView = spinner
                
                self.tableView.tableFooterView?.isHidden = false
                self.getGroupMembers(groupId: self.groupId ?? "", offset: self.offset ?? "")

            }
        }
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 80.0
    }
    
    
    
}


import UIKit
import AlamofireImage
import Kingfisher
import SDWebImage
import PaginatedTableView
import Toast_Swift
import WoWonderTimelineSDK
import ZKProgressHUD

class BlockedUsersVC: UIViewController,block_unblockDelegate{
    
    func unblock(user_id: String) {
        self.blockedUserArray.remove(at: self.selectedIndex)
        self.tableView.reloadData()
    }
    
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    let spinner = UIActivityIndicatorView(style: .gray)
    let pulltoRefresh = UIRefreshControl()
    var blockedUserArray = [[String:Any]]()
    let status = Reach().connectionStatus()
    var selectedIndex = 0
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        
    }
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        self.setupUI()
    }
    private func setupUI(){
   
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: " ", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
            navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("Blocked Users", comment: "Blocked Users")
        
        self.tableView.separatorStyle = .none
        tableView.register(UINib(nibName: "BlockedUsersTableItem", bundle: nil), forCellReuseIdentifier: "BlockedUsersTableItem")
        
        self.tableView.dataSource = self
        self.tableView.backgroundColor = .white
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.tableView.addSubview(pulltoRefresh)
//        ZKProgressHUD.show()
        self.activityIndicator.startAnimating()
        self.blockedUserArray.removeAll()
        self.tableView.reloadData()
        self.loadBlockedUsers()
    }
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
        }
    }
    @objc func refresh(){
        self.blockedUserArray.removeAll()
        self.tableView.reloadData()
        loadBlockedUsers()
        pulltoRefresh.endRefreshing()
        
    }
    private func loadBlockedUsers(){
        switch status {
        case .unknown, .offline:
//            ZKProgressHUD.dismiss()
//            self.activityIndicator.stopAnimating()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            performUIUpdatesOnMain {
                BLockUserManager.instance.getBlockUsers { (success, authError, error) in
                  
                    if success != nil {
                        for i in success!.blocked_users{
                            self.blockedUserArray.append(i)
                        }
                        self.tableView.reloadData()
                        self.activityIndicator.stopAnimating()
//                        ZKProgressHUD.dismiss()
                        
                    }
                    else if authError != nil {
//                        ZKProgressHUD.dismiss()
                        self.view.makeToast(authError?.errors?.errorText)
                        self.showAlert(title: "", message: (authError?.errors?.errorText)!)
                    }
                    else if error  != nil {
//                        ZKProgressHUD.dismiss()
                         self.activityIndicator.stopAnimating()
                        print(error?.localizedDescription)
                        
                    }
                }
            }
        }
    }
    
}
extension BlockedUsersVC: UITableViewDelegate {
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        
        return 80.0
        
    }
}


extension BlockedUsersVC: UITableViewDataSource {
    
    func numberOfSections(in tableView: UITableView) -> Int {
        
        return 1
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.blockedUserArray.count ?? 0
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "BlockedUsersTableItem") as! BlockedUsersTableItem
        let object = self.blockedUserArray[indexPath.row]
        cell.bind(object,index:indexPath.row)
        
        return cell
    }
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        self.selectedIndex = indexPath.row
        let storyboard = UIStoryboard(name: "General", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "UpdateBlockUnBlockVC") as! UpdateBlockUnBlockVC
        vc.object = self.blockedUserArray[indexPath.row]
        vc.delegate = self
        self.present(vc, animated: true, completion: nil)
    }
}

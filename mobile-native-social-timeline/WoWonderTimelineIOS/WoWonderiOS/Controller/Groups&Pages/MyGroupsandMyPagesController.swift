

import WoWonderTimelineSDK
import UIKit

class MyGroupsandMyPagesController: UIViewController {
    
    @IBOutlet weak var viewHeightConstraint: NSLayoutConstraint!
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var shareLabel: UILabel!
    
    var delegate: SharePostDelegate?
    
    var groupsOrpages = [[String:Any]]()
    var type = ""
    
    let status = Reach().connectionStatus()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.viewHeightConstraint.constant = 100.0
        if self.type == "group"{
            self.shareLabel.text = "Share to a Group"
            self.getGroups()
        }
        else {
            self.shareLabel.text = "Share to a Page"
            self.getPages()
        }
        
    }
    
    ///Network Connectivity.
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
            
        }
    }
    
    
    
    @IBAction func Create(_ sender: Any) {
        if (self.type == "group"){
            let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "CreatePageVC") as! CreatePageController
            vc.modalPresentationStyle = .fullScreen
            vc.modalTransitionStyle = .coverVertical
            vc.delegate = self
            self.present(vc, animated: true, completion: nil)
        }
        else {
            let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "CreateGroupVC") as! CreateGroupController
            vc.modalPresentationStyle = .fullScreen
            vc.modalTransitionStyle = .coverVertical
            vc.delegate = self
            self.present(vc, animated: true, completion: nil)
        }
        
    }
    @IBAction func Close(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        self.dismiss(animated: true, completion: nil)
    }
    
    private func getPages(){
        switch status {
        case .unknown, .offline:
            self.tableView.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            GetMyGroups_PagesManager.sharedInstance.getMyPages { (success, authError, error) in
                if success != nil {
                    for i in success!.pages{
                        self.groupsOrpages.append(i)
                    }
                    if (self.groupsOrpages.count == 1) || (self.groupsOrpages.count == 2) {
                        self.viewHeightConstraint.constant = 240.0
                    }
                    else if (self.groupsOrpages.count == 3){
                        self.viewHeightConstraint.constant = 290.0
                    }
                    else if (self.groupsOrpages.count == 4){
                        self.viewHeightConstraint.constant = 330.0
                    }
                    else if (self.groupsOrpages.count == 5){
                        self.viewHeightConstraint.constant = 370.0
                    }
                    else if (self.groupsOrpages.count == 6){
                        self.viewHeightConstraint.constant = 460.0
                    }
                    else if (self.groupsOrpages.count == 7){
                        self.viewHeightConstraint.constant = 500.0
                    }
                    else {
                        self.viewHeightConstraint.constant = 510.0
                    }
                    print(self.groupsOrpages)
                    self.tableView.reloadData()
                }
                else if authError != nil {
                    self.view.makeToast(authError?.errors.errorText)
                }
                else if error != nil {
                    print(error?.localizedDescription)
                }
            }
            
        }
        
    }
    
    private func getGroups(){
        switch status {
        case .unknown, .offline:
            self.tableView.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            GetMyGroups_PagesManager.sharedInstance.getMyGroups { (success, authError, error) in
                if success != nil {
                    for i in success!.groups{
                        self.groupsOrpages.append(i)
                    }
                    if (self.groupsOrpages.count == 1) || (self.groupsOrpages.count == 2) {
                        self.viewHeightConstraint.constant = 240.0
                    }
                    else if (self.groupsOrpages.count == 3){
                        self.viewHeightConstraint.constant = 290.0
                    }
                    else if (self.groupsOrpages.count == 4){
                        self.viewHeightConstraint.constant = 330.0
                    }
                    else if (self.groupsOrpages.count == 5){
                        self.viewHeightConstraint.constant = 370.0
                    }
                    else if (self.groupsOrpages.count == 6){
                        self.viewHeightConstraint.constant = 460.0
                    }
                    else if (self.groupsOrpages.count == 7){
                        self.viewHeightConstraint.constant = 500.0
                    }
                    else {
                        self.viewHeightConstraint.constant = 510.0
                    }
                    self.tableView.reloadData()
                }
                else if authError != nil {
                    self.view.makeToast(authError?.errors.errorText)
                }
                else if error != nil {
                    print(error?.localizedDescription)
                }
            }
            
        }
        
    }
    
}

extension MyGroupsandMyPagesController : UITableViewDelegate,UITableViewDataSource,CreateGroupDelegate,CreatePageDelegate{
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.groupsOrpages.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = UITableViewCell()
        cell.textLabel?.backgroundColor = .white
        cell.textLabel?.textColor = .lightGray
        cell.backgroundColor = .white
        cell.selectionStyle = .gray
        let index = self.groupsOrpages[indexPath.row]
        if self.type == "group"{
            if let groupName = index["group_name"] as? String{
                cell.textLabel?.text = groupName
            }
        }
        else {
            if let pageName = index["page_title"] as? String{
                cell.textLabel?.text = pageName
            }
        }
        
        return cell
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        let index = self.groupsOrpages[indexPath.row]
        if self.type == "group"{
            self.dismiss(animated: true) {
                self.delegate?.selectPageandGroup(data: index, type: "group")
            }
        }
        else {
            self.dismiss(animated: true) {
                self.delegate?.selectPageandGroup(data: index, type: "page")
            }
        }
        
    }
    
    func sendPageData(pageData: [String : Any]) {
        print("Create Page")
    }
    
    func sendGroupData(groupData: [String : Any]) {
        print("Create Group")
    }
    
    
    
}

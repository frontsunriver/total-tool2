
import UIKit
import Kingfisher
import WoWonderTimelineSDK

class InviteFriendsController: UIViewController {

    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var inviteLabel: UILabel!
    
    var usersArray = [[String:Any]]()
    
    var pageId = ""
    var groupId = ""
    override func viewDidLoad() {
        super.viewDidLoad()
        self.tableView.tableFooterView = UIView()
        self.inviteLabel.text = NSLocalizedString("Invite Friends", comment: "Invite Friends")
        if self.pageId != "" {
           self.getNotPageMembers(pageId: Int(self.pageId) ?? 10)
        }
        else {
           self.getNotGroupMembers(groupId: Int(self.groupId) ?? 10)
        }
    }
    
    @IBAction func Back(_ sender: Any) {
        self.navigationController?.popViewController(animated: true)
    }
    
    private func getNotPageMembers (pageId : Int) {
        
        GetNotPageMemberManager.sharedInstance.getNotPageMember(pageId: pageId) { (success, authError, error) in
            if success != nil {
                for i in success!.users{
                    self.usersArray.append(i)
                }
                self.tableView.reloadData()
            }
            
            else if authError != nil {
                self.showAlert(title: "", message: (authError?.errors.errorText)!)
            }
            else if error != nil {
                print("InternalError")
            }
        }
        
    }
    
    
    private func getNotGroupMembers (groupId : Int) {
        
        MemberNot_inGroupManager.sharedInstance.getNotGroupMember(groupId: groupId, completionBlock: { (success, authError, error) in
            
            if success != nil {
                for i in success!.users{
                    self.usersArray.append(i)
                }
                self.tableView.reloadData()
            }
           
            else if authError != nil {
                self.showAlert(title: "", message: (authError?.errors.errorText)!)
            }
            
            else if error != nil {
                print("InternalError")
            }
            
        })
        
        }
    
    private func AddMembertoPage(user_id : String){
        AddMembertoPageManager.sharedInstance.addMembertoPage(pageId: self.pageId, userId: user_id) { (success, authError, error) in
            if success != nil {
                print(success?.message)
            }
            else if authError  != nil {
                print(authError?.errors.errorText)
            }
            else if error != nil {
                print(error?.localizedDescription)
            }
        }
    }
    
    
    private func AddMembertoGroup(user_id : String){
        AddMembertoGroupManager.sharedInstance.addMembertoGroup(groupId: self.groupId, userId: user_id) { (success, authError, error) in
            if success != nil {
                print(success?.message)
            }
            else if authError  != nil {
                print(authError?.errors.errorText)
            }
            else if error != nil {
                print(error?.localizedDescription)
            }
        }
    }
    
    @IBAction func addMember (sender:UIButton) {
        let position = (sender as AnyObject).convert(CGPoint.zero, to: self.tableView)
        let indexPath = self.tableView.indexPathForRow(at: position)!
        let cell = tableView!.cellForRow(at: IndexPath(row: indexPath.row, section: 0)) as! InviteFriendCell
        let index = self.usersArray[indexPath.row]
        var userId = ""
        if let userid = index["user_id"] as? String{
            userId = userid
        }
        if self.pageId != "" {
        self.AddMembertoPage(user_id: userId)
        }
        else {
            self.AddMembertoGroup(user_id: userId)
        }
        self.usersArray.remove(at: indexPath.row)
        self.tableView.reloadData()
    }
}

extension InviteFriendsController : UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.usersArray.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "inviteFriend") as! InviteFriendCell
        let index = usersArray[indexPath.row]
        if let name = index["username"] as? String {
            cell.nameLabel.text = name
        }
        if let profileImage = index["avatar"] as? String{
        let url = URL(string: profileImage)
        cell.profileIcon.kf.setImage(with: url)
        }
    cell.addBtn.addTarget(self, action: #selector(self.addMember(sender:)), for: .touchUpInside)
        return cell

    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 80.0
    }
    
    
}


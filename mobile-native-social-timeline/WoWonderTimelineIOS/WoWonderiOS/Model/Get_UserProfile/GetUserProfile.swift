
import Foundation
import UIKit


class GetUserProfile {
    
    var navigationController : UINavigationController?
    var controller : UIViewController?
    var user_id : String?
    var tableView : UITableView?
    func getUserProfile (tableView: UITableView,indexpath:IndexPath,followersArray : [[String:Any]],followingArray : [[String:Any]],followPrivacy : String, messagePrivacy: String, navigationController : UINavigationController,username : String,follower : String, following : String, points : String,likes : String, profileImage : String, coverImage : String,controller: UIViewController,user_id : String) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "Cover") as! UserCoverView
        tableView.rowHeight = 275.0
        
        self.navigationController = navigationController
        self.controller = controller
        self.tableView = tableView
        self.user_id = user_id
        cell.profileNAme.text = username
        cell.followersLabel.text = follower
        cell.followingLabel.text = following
        cell.pointsLabel.text = points
        cell.likesLabel.text =  likes
        let profileUrl = URL(string:  profileImage)
        cell.profileImage.kf.setImage(with: profileUrl)
        let coverUrl = URL(string:  coverImage)
        cell.coverImage.kf.setImage(with: coverUrl)
    cell.backButton.addTarget(self, action: #selector(self.popViewController), for: .touchUpInside)
    cell.moreButton.addTarget(self, action: #selector(self.gotoMore), for: .touchUpInside)
    cell.addButton.addTarget(self, action: #selector(self.sendfollowRequest(sender:)), for: .touchUpInside)
    cell.messageButton.addTarget(self, action: #selector(self.gotoMassengerApp(sender:)), for: .touchUpInside)
        if (followPrivacy == "1"){
            for i in followingArray {
                if let userId = i["user_id"] as? String {
                    if userId == UserData.getUSER_ID(){
                        cell.addButton.isHidden = false
                        break
                    }
                    else {
                        cell.addButton.isHidden = true
                    }
                }
            }
        }
            
        else {
            cell.addButton.isHidden = false
        }
        
        for i in followersArray{
            if let userId = i["user_id"] as? String {
                if userId == UserData.getUSER_ID(){
                    cell.addButton.backgroundColor = .white
                    cell.addButton.setImage(#imageLiteral(resourceName: "check"), for: .normal)
                    cell.request = "follow"
                    break
                }
            }
        }
        
        
        if (messagePrivacy == "1"){
            for i in followingArray {
                if let userId = i["user_id"] as? String {
                    if userId == UserData.getUSER_ID(){
                        cell.messageButton.isHidden = false
                        break
                    }
                    else {
                        cell.messageButton.isHidden = true
                    }
                }
            }
            
        }
            
        else if (messagePrivacy == "2") {
            for i in followersArray {
                if let userId = i["user_id"] as? String {
                    if userId == UserData.getUSER_ID(){
                        cell.messageButton.isHidden = false
                        break
                    }
                    else {
                        cell.messageButton.isHidden = true
                    }
                }
            }
            
            
        }
        else {
            cell.messageButton.isHidden = false
            
        }
        
       return cell
    }
  
    @objc func popViewController() {
        self.navigationController?.popViewController(animated: true)
    }
    @objc func gotoMore() {
        let storyBoard = UIStoryboard(name: "Main", bundle: nil)
        let vc = storyBoard.instantiateViewController(withIdentifier: "GotoMore") as! MoreViewController
        vc.modalTransitionStyle = .crossDissolve
        vc.modalPresentationStyle = .overCurrentContext
        vc.delegate = self.controller!.self as! blockUserDelegate
        vc.userId = self.user_id!
        self.controller?.present(vc, animated: true, completion: nil)
    }
    
    @objc func sendfollowRequest(sender:UIButton){
        let position = (sender as AnyObject).convert(CGPoint.zero, to: tableView)
        let indexPath = tableView!.indexPathForRow(at: position)!
        let cell = tableView!.cellForRow(at: IndexPath(row: indexPath.row, section: 0)) as! UserCoverView
        if (cell.request == "unfollow"){
            cell.addButton.backgroundColor = .white
            cell.addButton.setImage(#imageLiteral(resourceName: "check"), for: .normal)
            cell.request = "follow"
            self.sendRequest()
        }
            
        else {
            cell.addButton.backgroundColor = .blue
            cell.addButton.setImage(#imageLiteral(resourceName: "Shape"), for: .normal)
            cell.request = "unfollow"
            self.sendRequest()
            
        }
        
        
    }
    
    
    private func sendRequest(){
        Follow_RequestManager.sharedInstance.sendFollowRequest(userId: self.user_id!) { (success, authError, error) in
            if success != nil {
                
                print(success?.follow_status)
            }
            else if authError != nil {
                print("InternalError")
//                self.showAlert(title: "", message: (authError?.errors.errorText)!)
            }
            else if error != nil {
                print("InternalError")
            }
            
        }
        
    }
    
    @objc func gotoMassengerApp(sender: UIButton) {
        UIApplication.shared.openURL(NSURL(string: "itms://itunes.apple.com/de/app/loungemates-messenger/id1475911448")! as URL)
    }
    
    static let sharedInstance = GetUserProfile()
    private init() {}
    
}

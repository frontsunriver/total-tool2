
import Foundation
import UIKit
import Kingfisher
import SDWebImage
import NotificationCenter
import WoWonderTimelineSDK


class GetPostWithImage: AddReactionDelegate,SharePostDelegate{
    
    
    static let sharedInstance = GetPostWithImage()
    private init () {
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
    }
    
    ///Network Connectivity.
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
            
        }
    }
    
    var targetController : UIViewController!
    var image : UIImage!
    var tableView : UITableView!
    var caption = ""
    var imageUrl = ""
    var selectedIndex = 0
    var section = 0
    var selection = [Int]()
    var selectIndex : Int? = nil
    var postArray = [[String:Any]]()
    var selectedIndexs = [[String:Any]]()
    var reaction: String? = nil
    var share_type: String? = nil
    var groupPageData = [String:Any]()
    var groupPageType: String? = nil
    let Storyboard = UIStoryboard(name: "Main", bundle: nil)
    
    func getPostImage(targetController : UIViewController ,tableView : UITableView, indexpath:IndexPath, postFile : String, array : [[String:Any]], url : URL,stackViewHeight: CGFloat,viewHeight: CGFloat, isHidden: Bool, viewColor :UIColor) -> UITableViewCell {
        var index = array[indexpath.row]
        self.targetController = targetController
        self.tableView = tableView
        self.postArray = array
        //        let index = postArray[indexpath.row]
        let cell = tableView.dequeueReusableCell(withIdentifier: "NewsFeedCell") as! NewsFeedCell
        tableView.rowHeight = UITableView.automaticDimension
        cell.likeandcommentViewHeight.constant = viewHeight
        cell.stackViewHeight.constant = stackViewHeight
        cell.likesCountBtn.isHidden = isHidden
        cell.commentsCountBtn.isHidden = isHidden
        cell.LikeBtn.isHidden = isHidden
        cell.commentBtn.isHidden = isHidden
        cell.shareBtn.isHidden = isHidden
        cell.contentView.backgroundColor = viewColor
        tableView.estimatedRowHeight = 400.0
        cell.heigthConstraint.isActive = true
        cell.heigthConstraint.constant = 220.0
        cell.stausimage.translatesAutoresizingMaskIntoConstraints = false
        var postTypes = ""
        var names = ""
        var fileURL = ""
        var isPro = ""
        var isVerified = ""
        
        if let time = index["post_time"] as? String{
            cell.timeLabel.text! = time
        }
        
        if let textStatus = index["postText"] as? String {
            cell.statusLabel.text! = textStatus.htmlToString
            self.caption = cell.statusLabel.text ?? ""
        }
        if let postFile = index["postFile_full"] as? String {
            fileURL = postFile
        }
        
        if let postType = index["postType"] as? String {
            if postType == "profile_picture" {
                postTypes = NSLocalizedString("changed profile picture", comment: "changed profile picture")
            }
            else if (postType == "profile_cover_picture"){
                postTypes = NSLocalizedString("changed cover picture", comment: "changed cover picture")
            }
            else {
                postTypes = ""
            }
        }
        
        if let publisher = index["publisher"] as? [String:Any] {
            if let profilename = publisher["name"] as? String{
                print(profilename)
                names = profilename
            }
            if let avatarUrl =  publisher["avatar"] as? String {
                let url = URL(string: avatarUrl)
                cell.profileImage.kf.setImage(with: url)
            }
            if let is_pro = publisher["is_pro"] as? String{
                isPro = is_pro
            }
            if let is_verify = publisher["verified"] as? String{
                isVerified = is_verify
            }
         }
        
        cell.statusLabel.handleURLTap { (URL) in
            print("Tap URL")
            UIApplication.shared.open(URL, options: [:], completionHandler: nil)
        }
        cell.statusLabel.handleHashtagTap { (hash) in
            print(hash)
            let Storyboard = UIStoryboard(name: "Search", bundle: nil)
            let vc = Storyboard.instantiateViewController(withIdentifier: "PostHashTagVC") as! PostHashTagController
            vc.hashtag = hash
            vc.modalTransitionStyle = .coverVertical
            vc.modalPresentationStyle = .fullScreen
            let appDelegate = UIApplication.shared.delegate as! AppDelegate
            appDelegate.window?.rootViewController?.present(vc, animated: true, completion: nil)
        }
        if let reactions = index["reaction"] as? [String:Any]{
            if let count = reactions["count"] as? Int{
                cell.likesCountBtn.setTitle("\(count)\(" ")\(NSLocalizedString("Likes", comment: "Likes"))", for: .normal)
            }
            if let isreact  = reactions["is_reacted"] as? Bool {
                if isreact == true{
                    if let type = reactions["type"] as? String{
                        if type == "6"{
                            cell.LikeBtn.setImage(UIImage(named: "angry"), for: .normal)
                            cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Angry", comment: "Angry"))", for: .normal)
                            cell.LikeBtn.setTitleColor(.red, for: .normal)
                        }
                        else if type == "1"{
                            cell.LikeBtn.setImage(UIImage(named: "like-2"), for: .normal)
                            cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
                            cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "3D5898"), for: .normal)
                        }
                        else if type == "2"{
                            cell.LikeBtn.setImage(UIImage(named: "love"), for: .normal)
                            cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Love", comment: "Love"))", for: .normal)
                            cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FB1002"), for: .normal)
                        }
                        else if type == "4"{
                            cell.LikeBtn.setImage(UIImage(named: "wow"), for: .normal)
                            cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                            cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Wow", comment: "Wow"))", for: .normal)
                        }
                        else if type == "5"{
                            cell.LikeBtn.setImage(UIImage(named: "sad"), for: .normal)
                            cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                            cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Sad", comment: "Sad"))", for: .normal)
                        }
                        else if type == "3"{
                            cell.LikeBtn.setImage(UIImage(named: "haha"), for: .normal)
                            cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                            cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Haha", comment: "Haha"))", for: .normal)
                        }
                    }
                }
                else{
                    cell.LikeBtn.setTitleColor(.lightGray, for: .normal)
                    cell.LikeBtn.setImage(UIImage(named:"like"), for: .normal)
                    cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
                }
            }
        }
        
        for i in self.selectedIndexs{
            if i["index"] as? Int == indexpath.row{
                if let reaction = i["reaction"] as? String{
                    if reaction == "6"{
                        cell.LikeBtn.setImage(UIImage(named: "angry"), for: .normal)
                        cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Angry", comment: "Angry"))", for: .normal)
                        cell.LikeBtn.setTitleColor(.red, for: .normal)
                    }
                    else if reaction == "1"{
                        cell.LikeBtn.setImage(UIImage(named: "like-2"), for: .normal)
                        cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
                        cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "3D5898"), for: .normal)
                    }
                    else if reaction == "2"{
                        cell.LikeBtn.setImage(UIImage(named: "love"), for: .normal)
                        cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Love", comment: "Love"))", for: .normal)
                        cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FB1002"), for: .normal)
                    }
                    else if reaction == "4"{
                        cell.LikeBtn.setImage(UIImage(named: "wow"), for: .normal)
                        cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                        cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Wow", comment: "Wow"))", for: .normal)
                    }
                    else if reaction == "5"{
                        cell.LikeBtn.setImage(UIImage(named: "sad"), for: .normal)
                        cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                        cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Sad", comment: "Sad"))", for: .normal)
                    }
                    else if reaction == "3"{
                        cell.LikeBtn.setImage(UIImage(named: "haha"), for: .normal)
                        cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                        cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Haha", comment: "Haha"))", for: .normal)
                    }
                        
                    else if reaction == ""{
                        cell.LikeBtn.setTitleColor(.lightGray, for: .normal)
                        cell.LikeBtn.setImage(UIImage(named:"like"), for: .normal)
                         cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
                    }
                }
                if let count = i["count"] as? Int{
                    cell.likesCountBtn.setTitle("\(count)\(" ")\(NSLocalizedString("Likes", comment: "Likes"))", for: .normal)
                }
            }
        }
        if let commentsCount = index["post_comments"] as? String{
            cell.commentsCountBtn.setTitle("\(NSLocalizedString("Comments", comment: "Comments"))\(" ")\(commentsCount)", for: .normal)
        }
        
        cell.imageAction = { [unowned self] in
            let vc = self.Storyboard.instantiateViewController(withIdentifier: "ShowImageVC") as! ShowImageController
            if let image = index["postFile"] as? String{
                vc.imageUrl = image
            }
            vc.posts.append(index)
            vc.modalPresentationStyle = .overFullScreen
            vc.modalTransitionStyle = .coverVertical
            self.targetController.present(vc, animated: true, completion: nil)
        }
        
        cell.commentAction = {[unowned self] in
            let vc = self.Storyboard.instantiateViewController(withIdentifier: "CommentVC") as! CommentController
            if let postId = index["post_id"] as? String{
                vc.postId = postId
            }
            if let reaction = index["reaction"] as? [String:Any]{
                if let count = reaction["count"] as? Int{
                    vc.likes = count
                }
            }
//            if let comments = index["get_post_comments"] as? [[String:Any]]{
//                vc.comments = comments
//            }
            vc.modalPresentationStyle = .fullScreen
            vc.modalTransitionStyle = .coverVertical
            targetController.present(vc, animated: true, completion: nil)
        }
        cell.likeCountAction = {[unowned self] in
            if let reaction = index["reaction"] as? [String:Any]{
                if let count = reaction["count"] as? Int{
                    if count > 0 {
                        let vc = self.Storyboard.instantiateViewController(withIdentifier: "PostReactionVC") as! PostReactionController
                        if let postId = index["post_id"] as? String{
                            vc.postId = postId
                        }
                        if let reactions = index["reaction"] as? [String:Any]{
                            vc.reaction = reactions
                        }
                        targetController.present(vc, animated: true, completion: nil)
                    }
                }
            }
        }
        cell.shareAction = {[unowned self] in
            self.selectedIndex = indexpath.row
            let vc = self.Storyboard.instantiateViewController(withIdentifier: "ShareVC") as! ShareController
            vc.delegate = self
            vc.modalPresentationStyle = .overFullScreen
            vc.modalTransitionStyle = .crossDissolve
            targetController.present(vc, animated: true, completion: nil)
        }
        cell.share_link = {[unowned self] in
            var text = ""
            if let postUrl =  index["url"] as? String{
                text = postUrl
            }
            let textToShare = [ text ]
            let activityViewController = UIActivityViewController(activityItems: textToShare, applicationActivities: nil)
            activityViewController.popoverPresentationController?.sourceView = self.targetController.view
            activityViewController.excludedActivityTypes = [ UIActivity.ActivityType.airDrop, UIActivity.ActivityType.postToFacebook, UIActivity.ActivityType.assignToContact,UIActivity.ActivityType.mail,UIActivity.ActivityType.postToTwitter,UIActivity.ActivityType.message,UIActivity.ActivityType.postToFlickr,UIActivity.ActivityType.postToVimeo,UIActivity.ActivityType.init(rawValue: "net.whatsapp.WhatsApp.ShareExtension"),UIActivity.ActivityType.init(rawValue: "com.google.Gmail.ShareExtension"),UIActivity.ActivityType.init(rawValue: "com.toyopagroup.picaboo.share"),UIActivity.ActivityType.init(rawValue: "com.tinyspeck.chatlyio.share")]
            self.targetController.present(activityViewController, animated: true, completion: nil)
        }
        cell.share_timeLine = {[unowned self] in
            let vc = self.Storyboard.instantiateViewController(withIdentifier : "SharePostVC") as! SharePostController
        vc.posts.append(index)
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.targetController.present(vc, animated: true, completion: nil)
        }
        cell.share_postTo = {[unowned self] in
            if (self.share_type == "group") || (self.share_type == "page"){
                let Storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
                let vc = Storyboard.instantiateViewController(withIdentifier : "MyGroups&PagesVC") as! MyGroupsandMyPagesController
                vc.type = self.share_type ?? ""
                vc.delegate = self
                vc.modalPresentationStyle = .overFullScreen
                vc.modalTransitionStyle = .crossDissolve
                self.targetController.present(vc, animated: true, completion: nil)
            }
            else {
                let vc = self.Storyboard.instantiateViewController(withIdentifier : "SharePopUpVC") as! SharePopUpController
                vc.delegate = self
                vc.modalPresentationStyle = .overFullScreen
                vc.modalTransitionStyle = .crossDissolve
                self.targetController.present(vc, animated: true, completion: nil)
            }
        }
        cell.share_page_group = {[unowned self] in
            let vc = self.Storyboard.instantiateViewController(withIdentifier : "SharePostVC") as! SharePostController
            vc.posts.append(index)
            if self.groupPageType == "group"{
                if let groupName = self.groupPageData["group_name"] as? String{
                    vc.groupName = groupName
                }
                if let groupId = self.groupPageData["id"] as? String{
                    vc.groupId = groupId
                }
                if let image  =  self.groupPageData["avatar"] as? String{
                    let trimmedString = image.trimmingCharacters(in: .whitespaces)
                    vc.imageUrl = trimmedString
                }
                vc.isGroup = true
            }
            else {
                if let pageName =  self.groupPageData["page_title"] as? String{
                    vc.pageName = pageName
                }
                if let pageId =  self.groupPageData["id"] as? String{
                    vc.pageId = pageId
                }
                if let image  =  self.groupPageData["avatar"] as? String{
                    vc.imageUrl = image
                }
                vc.isPage = true
            }
            vc.modalTransitionStyle = .coverVertical
            vc.modalPresentationStyle = .fullScreen
            self.targetController.present(vc, animated: true, completion: nil)
        }
        cell.moreAction = { [unowned self] in
            var post_id: String? = nil
            if let postId = index["post_id"] as? String{
                post_id = postId
            }
            let alert = UIAlertController(title: "", message: NSLocalizedString("More", comment: "More"), preferredStyle: .actionSheet)
            
            alert.setValue(NSAttributedString(string: alert.message ?? "", attributes: [NSAttributedString.Key.font : UIFont.systemFont(ofSize: 20, weight: UIFont.Weight.medium), NSAttributedString.Key.foregroundColor : UIColor.black]), forKey: "attributedMessage")
            
            if let is_saved = index["is_post_saved"] as? Bool{
                if !is_saved{
                    alert.addAction(UIAlertAction(title: NSLocalizedString("Save Post", comment: "Save Post"), style: .default, handler: { (_) in
                        let status = Reach().connectionStatus()
                        switch status {
                        case .unknown, .offline:
                            self.tableView.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
                        case .online(.wwan), .online(.wiFi):
                            index["is_post_saved"] = true
                            SavePostManager.sharedInstance.savedPost(targetController: self.targetController, postId: post_id ?? "", action: "save")
                        }
                    }))
                }
                else{
                    alert.addAction(UIAlertAction(title: NSLocalizedString("Unsave Post", comment: "Unsave Post"), style: .default, handler: { (_) in
                        let status = Reach().connectionStatus()
                        switch status {
                        case .unknown, .offline:
                            self.tableView.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
                        case .online(.wwan), .online(.wiFi):
                            index["is_post_saved"] = false
                            SavePostManager.sharedInstance.savedPost(targetController: self.targetController, postId: post_id ?? "", action: "save")
                        }
                    }))
                }
            }
            if let copyText = index["postText"] as? String{
                if copyText != ""{
                    alert.addAction(UIAlertAction(title: NSLocalizedString("Copy Text", comment: "Copy Text"), style: .default, handler: { (_) in
                        UIPasteboard.general.string = copyText
                        self.targetController.view.makeToast(NSLocalizedString("Copied", comment: "Copied"))
                    }))
                }
            }
            if let copyLink = index["url"] as? String{
                alert.addAction(UIAlertAction(title: NSLocalizedString("Copy Link", comment: "Copy Link"), style: .default, handler: { (_) in
                    UIPasteboard.general.string = copyLink
                    self.targetController.view.makeToast(NSLocalizedString("Copied", comment: "Copied"))
                }))
            }
            if let publisher = index["publisher"] as? [String:Any]{
                if let is_myPost = publisher["user_id"] as? String{
                    if is_myPost != UserData.getUSER_ID(){
                        if let is_report = index["is_post_reported"] as? Bool{
                            if !is_report{
                                alert.addAction(UIAlertAction(title: "\(NSLocalizedString("Report Post", comment: "Report Post"))", style: .default, handler: { (_) in
                                    let status = Reach().connectionStatus()
                                    switch status {
                                    case .unknown, .offline:
                                        self.tableView.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
                                    case .online(.wwan), .online(.wiFi):
                                        ReportPostManager.sharedInstance.reportedPost(targetController: self.targetController, postId: post_id ?? "")
                                    }
                                }))
                            }
                        }
                    }
                }
            }
            if let publisher = index["publisher"] as? [String:Any]{
                if let is_myPost = publisher["user_id"] as? String{
                    if is_myPost == UserData.getUSER_ID(){
                        //                alert.addAction(UIAlertAction(title: "Edit Post", style: .default, handler: { (_) in
                        //                }))
                        //                alert.addAction(UIAlertAction(title: "Boost Post", style: .default, handler: { (_) in
                        //                }))
                        alert.addAction(UIAlertAction(title: NSLocalizedString("Delete Post", comment: "Delete Post"), style: .default, handler: { (_) in
                            let status = Reach().connectionStatus()
                            switch status {
                            case .unknown, .offline:
                                self.tableView.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
                            case .online(.wwan), .online(.wiFi):
                                //            DeletePostManager.sharedInstance.postDelete(targetController: self.targetController, postId: post_id ?? "")
                                DeletePostManager.sharedInstance.postDelete(targetController: self.targetController,   postId: post_id ?? "") { (success) in
                                    if AppInstance.instance.vc == "myProfile"{
                                        let userInfo = ["userData":indexpath.row,"type":"delete"] as [String : Any]
                                        NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
                                    }
                                    else if AppInstance.instance.vc == "newsFeedVC"{
                                        let userInfo = ["userData":indexpath.row,"type":"delete"] as [String : Any]
                                        NotificationCenter.default.post(name: NSNotification.Name(rawValue: "performSegue"), object: nil, userInfo: userInfo)
                                    }
                                    else if AppInstance.instance.vc == "popularPostVC"{
                                        let userInfo = ["userData":indexpath.row,"type":"delete"] as [String : Any]
                                        NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
                                    }
                                    else if AppInstance.instance.vc == "hasTagPostVC"{
                                        let userInfo = ["userData":indexpath.row,"type":"delete"] as [String : Any]
                                        NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
                                    }
                                    else if AppInstance.instance.vc == "savedPostVC"{
                                        let userInfo = ["userData":indexpath.row,"type":"delete"] as [String : Any]
                                        NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
                                    }
                                    else if AppInstance.instance.vc == "showPostVC"{
                                        let userInfo = ["userData":indexpath.row,"type":"delete"] as [String : Any]
                                        NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
                                    }
                                    else if AppInstance.instance.vc == "eventDetailVC"{
                                        let userInfo = ["userData":indexpath.row,"type":"delete"] as [String : Any]
                                        NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
                                    }
                                    else if AppInstance.instance.vc == "pageVC"{
                                        let userInfo = ["userData":indexpath.row,"type":"delete"] as [String : Any]
                                        NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
                                    }
                                    else if AppInstance.instance.vc == "groupVC"{
                                        let userInfo = ["userData":indexpath.row,"type":"delete"] as [String : Any]
                                        NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
                                    }
                                }
                            }
                        }))
                    }
                }
            }
            alert.addAction(UIAlertAction(title: NSLocalizedString("Close", comment: "Close"), style: .cancel, handler: { (_) in
                print("User click Dismiss button")
            }))
            if let popoverController = alert.popoverPresentationController {
                popoverController.sourceView = self.targetController.view
                popoverController.sourceRect = CGRect(x: self.targetController.view.bounds.midX, y: self.targetController.view.bounds.midY, width: 0, height: 0)
                popoverController.permittedArrowDirections = []
            }
            self.targetController.present(alert, animated: true, completion: {
                print("completion block")
            })
        }
        cell.longAction = {[unowned self] in
            self.selectedIndex = indexpath.row
            let vc = self.Storyboard.instantiateViewController(withIdentifier: "LikeReactionsVC") as! LikeReactionsController
        vc.delegate = self
        vc.modalPresentationStyle = .overFullScreen
        vc.modalTransitionStyle = .crossDissolve
        self.targetController.present(vc, animated: true, completion: nil)
        }
        
        cell.addREact = {[unowned self] in
            self.reactions(index: self.selectedIndex, reaction: self.reaction ?? "")
            var localPostArray = index["reaction"] as! [String:Any]
            var totalCount = 0
            if let reactions = index["reaction"] as? [String:Any]{
                if let is_react = reactions["is_reacted"] as? Bool{
                    if !is_react {
                        if let count = reactions["count"] as? Int{
                            totalCount = count
                        }
                        localPostArray["count"] = totalCount + 1
                        totalCount =  localPostArray["count"] as? Int ?? 0
                        cell.likesCountBtn.setTitle("\(totalCount)\(" ")\(NSLocalizedString("Likes", comment: "Likes"))", for: .normal)
                        
                    }
                    else{
                        if let count = reactions["count"] as? Int{
                            totalCount = count
                        }
                    }
                }
            }
            let action = ["count": totalCount, "reaction": self.reaction ?? "","index": self.selectedIndex] as [String : Any]
            var count = 0
            print(self.selectedIndexs.count)
            if self.selectedIndexs.count == 0 {
                self.selectedIndexs.append(action)
            }
            else{
                for i in self.selectedIndexs{
                    count += 1
                    if i["index"] as? Int == self.selectedIndex{
                        print((count) - 1)
                        self.selectedIndexs[(count) - 1] = action
                    }
                    else{
                        self.selectedIndexs.append(action)
                    }
                }
            }
            
            localPostArray["is_reacted"] = true
            localPostArray["type"]  = self.reaction ?? ""
            if self.reaction == "1"{
                localPostArray["Like"] = 1
                localPostArray["1"]  = 1
                index["reaction"] = localPostArray
                cell.LikeBtn.setImage(UIImage(named: "like-2"), for: .normal)
                cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
                cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "3D5898"), for: .normal)
            }
            else if self.reaction == "2"{
                localPostArray["Love"] = 1
                localPostArray["2"]  = 1
                index["reaction"] = localPostArray
                cell.LikeBtn.setImage(UIImage(named: "love"), for: .normal)
                cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Love", comment: "Love"))", for: .normal)
                cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FB1002"), for: .normal)
            }
            else if self.reaction == "3"{
                localPostArray["HaHa"] = 1
                localPostArray["3"]  = 1
                index["reaction"] = localPostArray
                cell.LikeBtn.setImage(UIImage(named: "haha"), for: .normal)
                cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Haha", comment: "Haha"))", for: .normal)
            }
            else if self.reaction == "4"{
                localPostArray["Wow"] = 1
                localPostArray["4"]  = 1
                index["reaction"] = localPostArray
                cell.LikeBtn.setImage(UIImage(named: "wow"), for: .normal)
                cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Wow", comment: "Wow"))", for: .normal)
            }
            else if self.reaction == "5"{
                localPostArray["Sad"] = 1
                localPostArray["5"]  = 1
                index["reaction"] = localPostArray
                cell.LikeBtn.setImage(UIImage(named: "sad"), for: .normal)
                cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Sad", comment: "Sad"))", for: .normal)
            }
            else {
                localPostArray["Angry"] = 1
                localPostArray["6"]  = 1
                index["reaction"] = localPostArray
                cell.LikeBtn.setImage(UIImage(named: "angry"), for: .normal)
                cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Angry", comment: "Angry"))", for: .normal)
                cell.LikeBtn.setTitleColor(.red, for: .normal)
            }
        }
        
        cell.normalAction = {[unowned self] in
            let status = Reach().connectionStatus()
              switch status {
              case .unknown, .offline:
                  self.tableView.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
              case .online(.wwan), .online(.wiFi):
                let cell = self.tableView.cellForRow(at: IndexPath(row: indexpath.row, section: 6)) as! NewsFeedCell
                  if let reactions = index["reaction"] as? [String:Any]{
                      var totalCount = 0
                      if let count = reactions["count"] as? Int{
                          totalCount = count
                      }
                      if let is_react = reactions["is_reacted"] as? Bool{
                          print(is_react)
                          if is_react == true{
                            self.reactions(index: indexpath.row, reaction: "")
                            var localPostArray = index["reaction"] as! [String:Any]
                              localPostArray["is_reacted"] = false
                              localPostArray["type"]  = ""
                              localPostArray["count"] = totalCount - 1
                              totalCount =  localPostArray["count"] as? Int ?? 0
                              index["reaction"] = localPostArray
                              cell.likesCountBtn.setTitle("\(totalCount)\(" Likes")", for: .normal)
                              cell.LikeBtn.setImage(UIImage(named: "like"), for: .normal)
                              cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
                              cell.LikeBtn.setTitleColor(.lightGray, for: .normal)
                            let action = ["count": totalCount, "reaction": "","index":indexpath.row ?? 0] as [String : Any]
                              var count = 0
                              if self.selectedIndexs.count == 0{
                                  self.selectedIndexs.append(action)
                              }
                              else{
                                  for i in self.selectedIndexs{
                                      count += 1
                                    if i["index"] as? Int == indexpath.row{
                                          print((count) - 1)
                                          self.selectedIndexs[(count) - 1] = action
                                      }
                                      else{
                                          self.selectedIndexs.append(action)
                                      }
                                  }
                              }
                              
                          }
                          else{
                              var localPostArray = index["reaction"] as! [String:Any]
                              localPostArray["is_reacted"] = true
                              localPostArray["type"]  = "1"
                              localPostArray["count"] = totalCount + 1
                              localPostArray["Like"] = 1
                              localPostArray["1"]  = 1
                              totalCount =  localPostArray["count"] as? Int ?? 0
                              index["reaction"] = localPostArray
                            self.reactions(index: indexpath.row, reaction: "1")
                              cell.likesCountBtn.setTitle("\(totalCount)\(" ")\(NSLocalizedString("Likes", comment: "Likes"))", for: .normal)
                              cell.LikeBtn.setImage(UIImage(named: "like-2"), for: .normal)
                              cell.LikeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
                              cell.LikeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "3D5898"), for: .normal)
                            let action = ["count": totalCount, "reaction": "1","index":indexpath.row] as [String : Any]
                              var count = 0
                              print(self.selectedIndexs.count)
                              if self.selectedIndexs.count == 0 {
                                  self.selectedIndexs.append(action)
                              }
                              else{
                                  for i in self.selectedIndexs{
                                      count += 1
                                      if i["index"] as? Int == indexpath.row{
                                          print((count ?? 0) - 1)
                                          self.selectedIndexs[(count ?? 0) - 1] = action
                                      }
                                      else{
                                          self.selectedIndexs.append(action)
                                      }
                                  }
                              }
                          }
                      }
                  }
              }
        }
        
        cell.profileImage.tag = indexpath.row
        cell.profileName.tag = indexpath.row
        
        let imageAttachment =  NSTextAttachment()
        let imageAttachment1 =  NSTextAttachment()
        imageAttachment.image = UIImage(named:"veirfied")
        imageAttachment1.image = UIImage(named: "flash-1")
        let imageOffsetY: CGFloat = -2.0
        imageAttachment.bounds = CGRect(x: 0, y: imageOffsetY, width: imageAttachment.image!.size.width, height: imageAttachment.image!.size.height)
        imageAttachment1.bounds = CGRect(x: 0, y: imageOffsetY, width: 11.0, height: 14.0)
        
        let attechmentString = NSAttributedString(attachment: imageAttachment)
        let attechmentString1 = NSAttributedString(attachment: imageAttachment1)
        let attrs1 = [NSAttributedString.Key.foregroundColor : UIColor.black]
        let attrs2 = [NSAttributedString.Key.foregroundColor : UIColor.white]
        let attrs3 = [NSAttributedString.Key.foregroundColor : UIColor.lightGray]
        let attributedString1 = NSMutableAttributedString(string: names, attributes:attrs1)
        let attributedString2 = NSMutableAttributedString(string: " ", attributes:attrs2)
        let attributedString3 = NSMutableAttributedString(attributedString: attechmentString)
        let attributedString4 = NSMutableAttributedString(string: " ", attributes:attrs2)
        let attributedString5 = NSMutableAttributedString(attributedString: attechmentString1)
        let attributedString6 = NSMutableAttributedString(string: "\(" ")\(postTypes)", attributes:attrs3)
        
        attributedString1.append(attributedString2)
        if (isVerified == "1") && (isPro == "1"){
            attributedString1.append(attributedString3)
            attributedString1.append(attributedString4)
            attributedString1.append(attributedString5)
        }
        else if (isVerified == "1"){
           attributedString1.append(attributedString3)
           attributedString1.append(attributedString4)
        }
        else if (isPro == "1"){
             attributedString1.append(attributedString5)
        }
        attributedString1.append(attributedString6)
        cell.profileName.attributedText = attributedString1
        var cellFrame = cell.frame.size
        cellFrame.height =  cellFrame.height - 15
        cellFrame.width =  cellFrame.width - 15
        
        cell.stausimage.kf.setImage(with: URL(string:fileURL)) { result in
            // `result` is either a `.success(RetrieveImageResult)` or a `.failure(KingfisherError)`
            switch result {
            case .success(let value):
                // The image was set to image view:
                print(value.image)
                print("Print Size",value.image.size.height)
                let cellFrame = cell.frame.size
                let widthOffset = value.image.size.width - cellFrame.width
                let widthOffsetPercentage = (widthOffset*100)/value.image.size.width
                let heightOffset = (widthOffsetPercentage * value.image.size.height)/100
                let effectiveHeight = value.image.size.height - heightOffset
                cell.heigthConstraint.constant = effectiveHeight
                cell.stausimage?.clipsToBounds = true
                print(value.cacheType)
//                 if value.cacheType == .none || value.cacheType == .disk{
//                    UIView.performWithoutAnimation {
//                    cell.layoutIfNeeded()
//                    tableView.beginUpdates()
//                    cell.setNeedsLayout()
//                    tableView.endUpdates()
//                    tableView.setContentOffset(CGPoint(x: 0, y: tableView.contentOffset.y), animated: false)
//                    }
//                }
            case .failure(let error):
                print(error)
                cell.heigthConstraint.constant = 220.0
            }
        }
        
        let gesture = UITapGestureRecognizer(target: self, action: #selector(self.gotoUserProfile(gesture:)))
        let gestureonLabel = UITapGestureRecognizer(target: self, action: #selector(self.gotoUserProfile(gesture:)))
        cell.profileImage.addGestureRecognizer(gesture)
        cell.profileImage.isUserInteractionEnabled = true
        cell.profileName.isUserInteractionEnabled = true
        cell.profileName.addGestureRecognizer(gestureonLabel)
        cell.layoutIfNeeded()
        cell.setNeedsLayout()
        cell.clipsToBounds = true
        return cell
    }
    
    @IBAction func gotoUserProfile(gesture: UIGestureRecognizer){
        if AppInstance.instance.vc == "myProfile"{
            if (AppInstance.instance.index != nil) && (gesture.view?.tag == 0){
                print(AppInstance.instance.index)
                let userInfo = ["userData":AppInstance.instance.index,"tag":gesture.view?.tag ?? 0,"type":"share"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
            }
            else{
                let userInfo = ["userData":gesture.view!.tag,"type":"profile"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
            }
        }
        else if (AppInstance.instance.vc == "newsFeedVC"){
            if (AppInstance.instance.index != nil) && (gesture.view?.tag == 0){
                print(AppInstance.instance.index)
                let userInfo = ["userData":AppInstance.instance.index,"tag":gesture.view?.tag ?? 0,"type":"share"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "performSegue"), object: nil, userInfo: userInfo)
            }
            else{
                let userInfo = ["userData":gesture.view!.tag,"type":"profile"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "performSegue"), object: nil, userInfo: userInfo)
            }
        }
        else if AppInstance.instance.vc == "popularPostVC"{
            if (AppInstance.instance.index != nil) && (gesture.view?.tag == 0){
                print(AppInstance.instance.index)
                let userInfo = ["userData":AppInstance.instance.index,"tag":gesture.view?.tag ?? 0,"type":"share"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
            }
            else{
                let userInfo = ["userData":gesture.view!.tag,"type":"profile"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
            }
        }
        else if AppInstance.instance.vc == "hasTagPostVC"{
            if (AppInstance.instance.index != nil) && (gesture.view?.tag == 0){
                print(AppInstance.instance.index)
                let userInfo = ["userData":AppInstance.instance.index,"tag":gesture.view?.tag ?? 0,"type":"share"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
            }
            else{
                let userInfo = ["userData":gesture.view!.tag,"type":"profile"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
            }
        }
        else if AppInstance.instance.vc == "savedPostVC"{
            if (AppInstance.instance.index != nil) && (gesture.view?.tag == 0){
                print(AppInstance.instance.index)
                let userInfo = ["userData":AppInstance.instance.index,"tag":gesture.view?.tag ?? 0,"type":"share"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
            }
            else{
                let userInfo = ["userData":gesture.view!.tag,"type":"profile"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
            }
        }
        else if AppInstance.instance.vc == "showPostVC"{
            if (AppInstance.instance.index != nil) && (gesture.view?.tag == 0){
                print(AppInstance.instance.index)
                let userInfo = ["userData":AppInstance.instance.index,"tag":gesture.view?.tag ?? 0,"type":"share"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
            }
            else{
                let userInfo = ["userData":gesture.view!.tag,"type":"profile"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
            }
        }
        else if AppInstance.instance.vc == "eventDetailVC"{
            if (AppInstance.instance.index != nil) && (gesture.view?.tag == 0){
                print(AppInstance.instance.index)
                let userInfo = ["userData":AppInstance.instance.index,"tag":gesture.view?.tag ?? 0,"type":"share"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
            }
            else{
                let userInfo = ["userData":gesture.view!.tag,"type":"profile"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
            }
        }
        else if AppInstance.instance.vc == "pageVC"{

        }
        else if AppInstance.instance.vc == "groupVC"{
            if (AppInstance.instance.index != nil) && (gesture.view?.tag == 0){
                print(AppInstance.instance.index)
                let userInfo = ["userData":AppInstance.instance.index,"tag":gesture.view?.tag ?? 0,"type":"share"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
            }
            else{
                let userInfo = ["userData":gesture.view!.tag,"type":"profile"] as [String : Any]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Notifire"), object: nil, userInfo: userInfo)
            }
        }
    }
    
    private func reactions(index :Int, reaction: String){
        performUIUpdatesOnMain {
            print(reaction)
            var postID = ""
            if let postId = self.postArray[index]["post_id"] as? String{
                postID = postId
            }
            AddReactionManager.sharedInstance.addReaction(postId: postID, reaction:reaction) { (success, authError, error) in
                
                if success != nil{
                    print(success?.action)
                }
                else if authError != nil{
                    print(authError?.errors.errorText)
                }
                else {
                    print(error?.localizedDescription)
                }
            }
        }
    }
    func addReaction(reation: String) {
          let cell = self.tableView.cellForRow(at: IndexPath(row: self.selectedIndex, section: 6)) as! NewsFeedCell
          self.reaction = reation
          cell.addREact?()
      }
    
    func sharePost() {
    let cell = self.tableView.cellForRow(at: IndexPath(row: self.selectedIndex, section: 6)) as! NewsFeedCell
        cell.share_timeLine?()
    }
    
    func sharePostTo(type:String) {
    let cell = self.tableView.cellForRow(at: IndexPath(row: self.selectedIndex, section: 6)) as! NewsFeedCell
        self.share_type = type
        cell.share_postTo?()
    }
    
    func selectPageandGroup(data: [String : Any],type : String) {
        let cell = self.tableView.cellForRow(at: IndexPath(row: self.selectedIndex, section: 6)) as! NewsFeedCell
        self.groupPageData = data
        self.groupPageType = type
        cell.share_page_group?()
    }
    
    func sharePostLink() {
let cell = self.tableView.cellForRow(at: IndexPath(row: self.selectedIndex, section: 6)) as! NewsFeedCell
    cell.share_link?()

    }
}

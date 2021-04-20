

import UIKit
import Kingfisher
import WoWonderTimelineSDK
import GoogleMobileAds


class GroupController: UIViewController,GroupMoreDelegate{

    @IBOutlet weak var tableView: UITableView!
    
    let Storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
    
    var groupPostsArray = [[String:Any]]()
    var groupData = [String:Any]()
    var groupCover = ""
    var groupIcon = ""
    var groupTitle = ""
    var groupName = ""
    var about = ""
    var category = ""
    var afterpostid  = "0"
    var groupId = ""
    var isJoined = false
    var isAdmin = false
    var privacy = "0"
    var categoryId = ""
    var aboutGroup = ""
    
    var id: String? = nil
    var isFromList = false
    var isData_nil: Bool = false

    
    var delegate : DeleteGroupDelegate!
    var delegte1 : JoinGroupDelegate!
    
    let status = Reach().connectionStatus()
    let spinner = UIActivityIndicatorView(style: .gray)

    var interstitial: GADInterstitial!

    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        NotificationCenter.default.addObserver(self, selector: #selector(loadList), name: NSNotification.Name(rawValue: "load"), object: nil)
        self.navigationItem.hidesBackButton = true
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationController?.navigationBar.isHidden = true
        tableView.register(UINib(nibName: "GroupCoverCell", bundle: nil), forCellReuseIdentifier: "GroupCover")
        self.tableView.tableFooterView = UIView()
        if let group_id = self.groupData["id"] as? String{
            self.id = group_id
        }
    self.getGroupData(groupId: self.id ?? "")
        self.getGroupPosts(groupId: self.id ?? "", offset: self.afterpostid)
        SetUpcells.setupCells(tableView: self.tableView)
        print(self.isJoined)
        print(self.category,self.categoryId)
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
     
//    override func viewWillDisappear(_ animated: Bool) {
//        super.viewWillDisappear(animated)
//        self.navigationController?.isNavigationBarHidden = true
//    }
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        self.navigationController?.navigationBar.isHidden = true
        AppInstance.instance.vc = "groupVC"
        NotificationCenter.default.addObserver(self, selector: #selector(self.Notifire(notification:)), name: NSNotification.Name(rawValue: "Notifire"), object: nil)
        self.navigationController?.navigationBar.isHidden = true
    }
    override func viewWillDisappear(_ animated: Bool) {
        NotificationCenter.default.removeObserver(self, name: NSNotification.Name(rawValue: "Notifire"), object: nil)
        self.navigationController?.navigationBar.isHidden = false
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    @objc func loadList(notification: NSNotification){
        var post_id = ""
        if let data = notification.userInfo?["data"] as? [String:Any] {
            if let id = data["post_id"] as? String{
                post_id = id
            }
            switch status {
            case .unknown, .offline:
                self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
            case .online(.wwan), .online(.wiFi):
                performUIUpdatesOnMain {
                    GetGroupPostsManager.sharedInstance.getGroupPost(groupId: self.id ?? "", afterPostId: "") {[weak self] (success, authError, error) in
                        if success != nil {
                            for i in success!.data{
                                if i["post_id"] as? String == post_id{
                                    self?.groupPostsArray.insert(i, at: 0)
                                }
                            }
                            if success?.data.count == 0 {
                                self?.isData_nil = true
                            }
                            else{
                                self?.isData_nil = false
                            }
                             self?.spinner.stopAnimating()
                              self?.tableView.reloadData()
                          }
                          else if authError != nil {
                              self?.showAlert(title: "", message: (authError?.errors.errorText)!)
                          }
                          else if error != nil {
                              print(error?.localizedDescription)
                          }
                      }
                }
            }
        }
    }
    
    
    @objc func Notifire(notification: NSNotification){
        if let type = notification.userInfo?["type"] as? String{
            if type == "delete"{
                if let data = notification.userInfo?["userData"] as? Int{
                    print(data)
                    self.groupPostsArray.remove(at: data)
                    self.tableView.reloadData()
                }
            }
            if type == "profile"{
                let storyBoard = UIStoryboard(name: "Main", bundle: nil)
                let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
                var groupId: String? = nil
                var pageId: String? = nil
                var user_data: [String:Any]? = nil
                if let data = notification.userInfo?["userData"] as? Int{
                    print(data)
                    if let groupid = self.groupPostsArray[data]["group_id"] as? String{
                        groupId = groupid
                    }
                    if let page_Id = self.groupPostsArray[data]["page_id"] as? String{
                        pageId = page_Id
                    }
                    if let userData = self.groupPostsArray[data]["publisher"] as? [String:Any]{
                        user_data = userData
                    }
                }
                if pageId != "0"{
                    let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "PageVC") as! PageController
                    
                    vc.page_id = pageId
                    self.navigationController?.pushViewController(vc, animated: true)
                }
                else if groupId != "0"{
                    let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "GroupVC") as! GroupController
                    vc.id = groupId
                    self.navigationController?.pushViewController(vc, animated: true)
                }
                else{
                    if let id = user_data?["user_id"] as? String{
                        if id == UserData.getUSER_ID(){
                            let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
                            let vc = storyboard.instantiateViewController(withIdentifier: "MyProfileVC") as! MyProfileController
                            self.navigationController?.pushViewController(vc, animated: true)
                        }
                        else{
                            vc.userData = user_data
                            self.navigationController?.pushViewController(vc, animated: true)
                        }
                    }
                }
            }
                
            else if (type == "share"){
                let storyBoard = UIStoryboard(name: "Main", bundle: nil)
                let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
                var groupId: String? = nil
                var pageId: String? = nil
                var user_data: [String:Any]? = nil
                if let data = notification.userInfo?["userData"] as? Int{
                    if let shared_info = self.groupPostsArray[data]["shared_info"] as? [String:Any]{
                        if shared_info != nil{
                            if let groupid = self.groupPostsArray[data]["group_id"] as? String{
                                groupId = groupid
                            }
                            if let page_Id = self.groupPostsArray[data]["page_id"] as? String{
                                pageId = page_Id
                            }
                            if let publisher = shared_info["publisher"] as? [String:Any]{
                                user_data = publisher
                            }
                            if pageId != "0"{
                                let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
                                let vc = storyboard.instantiateViewController(withIdentifier: "PageVC") as! PageController
                                
                                vc.page_id = pageId
                                self.navigationController?.pushViewController(vc, animated: true)
                            }
                            else if groupId != "0"{
                                let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
                                let vc = storyboard.instantiateViewController(withIdentifier: "GroupVC") as! GroupController
                                vc.id = groupId
                                self.navigationController?.pushViewController(vc, animated: true)
                            }
                            else{
                                if let id = user_data?["user_id"] as? String{
                                    if id == UserData.getUSER_ID(){
                                        
                                    }
                                    else{
                                        vc.userData = user_data
                                        self.navigationController?.pushViewController(vc, animated: true)
                                    }
                                }
                            }
                        }
                        else{
                            if let tag = notification.userInfo?["tag"] as? Int{
                                if let groupid = self.groupPostsArray[tag]["group_id"] as? String{
                                    groupId = groupid
                                }
                                if let page_Id = self.groupPostsArray[tag]["page_id"] as? String{
                                    pageId = page_Id
                                }
                                if let userData = self.groupPostsArray[tag]["publisher"] as? [String:Any]{
                                    user_data = userData
                                }
                            }
                            if pageId != "0"{
                                let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
                                let vc = storyboard.instantiateViewController(withIdentifier: "PageVC") as! PageController
                                
                                vc.page_id = pageId
                                self.navigationController?.pushViewController(vc, animated: true)
                            }
                            else if groupId != "0"{
                                let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
                                let vc = storyboard.instantiateViewController(withIdentifier: "GroupVC") as! GroupController
                                vc.id = groupId
                                self.navigationController?.pushViewController(vc, animated: true)
                            }
                            else{
                                if let id = user_data?["user_id"] as? String{
                                    if id == UserData.getUSER_ID(){
                                        
                                    }
                                    else{
                                        vc.userData = user_data
                                        self.navigationController?.pushViewController(vc, animated: true)
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    private func getGroupData(groupId: String){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetGroupDataManager.sharedInstance.getData(groupId: groupId) { (success, authError, error) in
                    if success != nil{
                    self.groupData = success!.group_data
                        self.isAdmin = self.groupData["is_owner"] as? Bool ?? true
                        self.isJoined = self.groupData["is_joined"] as? Bool ?? true
                    self.tableView.reloadData()
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
    }

    private func getGroupPosts(groupId:String, offset:String){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetGroupPostsManager.sharedInstance.getGroupPost(groupId: groupId, afterPostId: offset) {[weak self] (success, authError, error) in
                      if success != nil {
                          for i in success!.data{
                          self?.groupPostsArray.append(i)
                          }
                        self?.afterpostid = self?.groupPostsArray.last?["post_id"] as? String ?? "0"
                        if success?.data.count == 0 {
                            self?.isData_nil = true
                        }
                        else{
                            self?.isData_nil = false
                        }
                         self?.spinner.stopAnimating()
                          self?.tableView.reloadData()
                      }
                      else if authError != nil {
                          self?.showAlert(title: "", message: (authError?.errors.errorText)!)
                      }
                      else if error != nil {
                          print(error?.localizedDescription)
                      }
                  }
            }
        }
    }
    
    private func JoinGroup(){
        switch status {
         case .unknown, .offline:
             self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
         case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                JoinGroupManager.sharedInstance.joinGroup(groupId: Int(self.id ?? "0") ?? 0) { (success, authError, error) in
                    if success != nil {
                        print(success?.join_status)
                    }
                    else if authError != nil {
                        print(authError?.errors.errorText)
                    }
                    else if error != nil {
                        print(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
private func uploadImage(imageType:String, data:Data){
    var group_id: String? = nil
    if let groupId = self.groupData["id"] as? String{
        group_id = groupId
    }
    UpdateGroupDataManager.sharedInstance.uploadImage(groupId: group_id ?? "", imageType:
        imageType, data: data) { (success, authError, error) in
        performUIUpdatesOnMain {
            if success != nil {
                self.view.makeToast(success?.message)
                print(success!.message)
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
    
    func gotoSetting(type: String) {
        var group_url: String? = nil
        if let groupUrl = self.groupData["url"] as? String{
            group_url = groupUrl
        }
        if type == "setting"{
        let Storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = Storyboard.instantiateViewController(withIdentifier: "GroupSettingVC") as! GroupSettingController
        vc.groupName = self.groupName
        vc.groupTitle = self.groupTitle
        vc.privacy = self.privacy
        vc.categoryName = self.category
        vc.categoryId = self.categoryId
        vc.about = self.aboutGroup
        vc.group_Id = self.groupId
        vc.categoryName = self.category
        vc.delegate = self
        self.navigationController?.pushViewController(vc, animated: true)
        }
        else if type == "copy"{
            UIPasteboard.general.string = group_url
            self.view.makeToast(NSLocalizedString("Copied", comment: "Copied"))
        }
        else {
            let text = group_url
            
            // set up activity view controller
            let textToShare = [ text ]
            let activityViewController = UIActivityViewController(activityItems: textToShare as [Any], applicationActivities: nil)
            activityViewController.popoverPresentationController?.sourceView = self.view // so that iPads won't crash
            
            // exclude some activity types from the list (optional,)
            activityViewController.excludedActivityTypes = [ UIActivity.ActivityType.airDrop, UIActivity.ActivityType.postToFacebook, UIActivity.ActivityType.assignToContact,UIActivity.ActivityType.mail,UIActivity.ActivityType.postToTwitter,UIActivity.ActivityType.message,UIActivity.ActivityType.postToFlickr,UIActivity.ActivityType.postToVimeo,UIActivity.ActivityType.init(rawValue: "net.whatsapp.WhatsApp.ShareExtension"),UIActivity.ActivityType.init(rawValue: "com.google.Gmail.ShareExtension"),UIActivity.ActivityType.init(rawValue: "com.toyopagroup.picaboo.share"),UIActivity.ActivityType.init(rawValue: "com.tinyspeck.chatlyio.share")]
            
            // present the view controller
            self.present(activityViewController, animated: true, completion: nil)
            
        }
    }
    
    @objc func GotoAddPost(sender: UIButton){
        let storyboard = UIStoryboard(name: "AddPost", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "AddPostVC") as! AddPostVC
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
}

extension GroupController : UITableViewDataSource,UITableViewDelegate,DeleteGroupDelegate,uploadImageDelegate{
     func numberOfSections(in tableView: UITableView) -> Int {
        return 7
    }
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if section == 0 {
            return 1
        }
        else if section == 1{
            return 1
        }
        else if (section == 2){
            return 1
        }
        else if (section == 3){
            return 1
        }
        else if (section == 4){
            return 1
        }
        else if (section == 5){
            return 1
        }
        else {
           return self.groupPostsArray.count
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
    if indexPath.section == 0 {
        
    let cell = tableView.dequeueReusableCell(withIdentifier: "GroupCover") as! GroupCoverCell
     self.tableView.rowHeight = 362.0
        if let avatar = self.groupData["avatar"] as? String{
            let image = avatar.trimmingCharacters(in: .whitespaces)
            let url = URL(string: image)
            cell.profile.kf.setImage(with: url)
        }
        if let cover = self.groupData["cover"] as? String{
            let image = cover.trimmingCharacters(in: .whitespaces)
            let url = URL(string: image)
            cell.cover.kf.setImage(with: url)
        }
        if let title = self.groupData["group_title"] as? String{
            cell.titleLabel.text = title
        }
        if let subTitle = self.groupData["group_name"] as? String{
            cell.subtitleLabel.text = "\("@")\(subTitle)"
        }
        if let category = self.groupData["category"] as? String{
            cell.categoryBtn.setTitle("\("   ")\(category)", for: .normal)
        }
        if let user_id = self.groupData["user_id"] as? String{
            print(user_id)
            if user_id == UserData.getUSER_ID(){
                cell.joinGroupBtn.setTitle(NSLocalizedString("Edit", comment: "Edit"), for: .normal)
                cell.joinGroupBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "984243")
                cell.joinGroupBtn.setTitleColor(.white, for: .normal)
            }
            else{
                cell.editIconBtn.isHidden = true
                cell.editCoverBtn.isHidden = true
                cell.editView.isHidden = true
                if let is_Joined = self.groupData["is_joined"] as? Bool{
                    if is_Joined{
                cell.joinGroupBtn.setTitle(NSLocalizedString("Joined", comment: "Joined"), for: .normal)
                cell.joinGroupBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#e5e5e5")
                        cell.joinGroupBtn.setTitleColor(.black, for: .normal)
                    }
                    else{
                        cell.joinGroupBtn.setTitle(NSLocalizedString("Join Group", comment: "Join Group"), for: .normal)
                        cell.joinGroupBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "984243")
                        cell.joinGroupBtn.setTitleColor(.white, for: .normal)
                    }
                }
            }
        }
     cell.backBtn.addTarget(self, action: #selector(self.Back), for: .touchUpInside)
     cell.joinGroupBtn.addTarget(self, action: #selector(self.GroupJoin), for: .touchUpInside)
     cell.memberBtn.addTarget(self, action: #selector(self.gotoGroupMembers), for: .touchUpInside)
     cell.addMembersBtn.addTarget(self, action: #selector(self.inviteFriend), for: .touchUpInside)
     cell.moreBtn.addTarget(self, action: #selector(self.gotoMoreVC), for: .touchUpInside)
     cell.editCoverBtn.addTarget(self, action: #selector(self.EditGroupCover(sender:)), for: .touchUpInside)
    cell.editIconBtn.addTarget(self, action: #selector(self.EditGroupIcon(sender:)), for: .touchUpInside)
     return cell
    }
    else if (indexPath.section == 1){
        
        let cell = tableView.dequeueReusableCell(withIdentifier: "AddPostCells") as! AddPostCell
        cell.moreBtn.addTarget(self, action: #selector(self.GotoAddPost(sender:)), for: .touchUpInside)
        cell.photoBtn.addTarget(self, action: #selector(self.GotoAddPost(sender:)), for: .touchUpInside)
        cell.bind(page_data: self.groupData)
        self.tableView.rowHeight = 60.0
        return cell
        
//        let cell = tableView.dequeueReusableCell(withIdentifier: "AddPostCells") as! AddPostCell
//        cell.moreBtn.addTarget(self, action: #selector(self.GotoAddPost(sender:)), for: .touchUpInside)
//        cell.photoBtn.addTarget(self, action: #selector(self.GotoAddPost(sender:)), for: .touchUpInside)
//        cell.is_group = true
//        cell.bind(page_data: self.groupData)
//        self.tableView.rowHeight = 60.0
//        return cell
    }
    else if (indexPath.section == 2){
        let cell = UITableViewCell()
        self.tableView.rowHeight = 0
        return cell
    }
    else if (indexPath.section == 3){
        let cell = UITableViewCell()
        self.tableView.rowHeight = 0
        return cell
    }
    else if (indexPath.section == 4){
        let cell = UITableViewCell()
        self.tableView.rowHeight = 0
        return cell
    }
    else if (indexPath.section == 5){
        let cell = UITableViewCell()
        self.tableView.rowHeight = 0
        return cell
    }
    else {
        var cell = UITableViewCell()
        let index = self.groupPostsArray[indexPath.row]
        let postfile = index["postFile"] as? String ?? ""
        let postLink = index["postLink"] as? String ?? ""
        let postYoutube = index["postYoutube"] as? String ?? ""
        let blog = index["blog_id"] as? String ?? "0"
        let group = index["group_recipient_exists"] as? Bool ??  false
        let product = index["product_id"] as? String ?? "0"
        let event = index["page_event_id"] as? String ?? "0"
        let postSticker = index["postSticker"] as? String ?? ""
        let colorId = index["color_id"] as? String ?? "0"
        let multi_image = index["multi_image"] as? String ?? "0"
        let photoAlbum = index["album_name"] as? String ?? ""
        let postOptions = index["poll_id"] as? String ?? "0"
        let postRecord = index["postRecord"] as? String ?? "0"
        
        
        if (postfile != "")  {
            let url = URL(string: postfile)
            let urlExtension: String? = url?.pathExtension
            if (urlExtension == "jpg" || urlExtension == "png" || urlExtension == "jpeg" || urlExtension == "JPG" || urlExtension == "PNG"){
                cell = GetPostWithImage.sharedInstance.getPostImage(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.groupPostsArray, url: url!, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if(urlExtension == "wav" ||  urlExtension == "mp3" || urlExtension == "MP3"){
                cell = GetPostMp3.sharedInstance.getMP3(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.groupPostsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
            else if (urlExtension == "pdf") {
                cell = GetPostPDF.sharedInstance.getPostPDF(targetControler: self, tableView: tableView, indexpath: indexPath, postfile: postfile, array: self.groupPostsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
                
            else {
                cell = GetPostVideo.sharedInstance.getVideo(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.groupPostsArray, url: url!, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
            
            
        }
            
        else if (postLink != "") {
            cell = GetPostWithLink.sharedInstance.getPostLink(targetController: self, tableView: tableView, indexpath: indexPath, postLink: postLink, array: self.groupPostsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
            
        else if (postYoutube != "") {
            cell = GetPostYoutube.sharedInstance.getPostYoutub(targetController: self, tableView: tableView, indexpath: indexPath, postLink: postYoutube, array: self.groupPostsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            
        }
        else if (blog != "0") {
            cell = GetPostBlog.sharedInstance.GetBlog(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.groupPostsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
            
        else if (group != false){
            cell = GetPostGroup.sharedInstance.GetGroupRecipient(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.groupPostsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
            
        else if (product != "0") {
            cell = GetPostProduct.sharedInstance.GetProduct(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.groupPostsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        else if (event != "0") {
            cell = GetPostEvent.sharedInstance.getEvent(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array:  self.groupPostsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            
        }
        else if (postSticker != "") {
            cell = GetPostSticker.sharedInstance.getPostSticker(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.groupPostsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            
        }
            
        else if (colorId != "0"){
            cell = GetPostWithBg_Image.sharedInstance.postWithBg_Image(targetController: self, tableView: tableView, indexpath: indexPath, array: self.groupPostsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
            
        else if (multi_image != "0") {
            cell = GetPostMultiImage.sharedInstance.getMultiImage(targetController: self, tableView: tableView, indexpath: indexPath, array: self.groupPostsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            
        }
            
        else if photoAlbum != "" {
            cell = getPhotoAlbum.sharedInstance.getPhoto_Album(targetController: self, tableView: tableView, indexpath: indexPath, array: self.groupPostsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
            
        else if postOptions != "0" {
            cell = GetPostOptions.sharedInstance.getPostOptions(targertController: self, tableView: tableView, indexpath: indexPath, array: self.groupPostsArray,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
            
        else if postRecord != ""{
            cell = GetPostRecord.sharedInstance.getPostRecord(targetController: self, tableView: tableView, indexpath: indexPath, array: self.groupPostsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            
        }
            
        else {
            cell = GetNormalPost.sharedInstance.getPostText(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.groupPostsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
        return cell
    }
   
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
        if indexPath.section == 1{
            let storyboard = UIStoryboard(name: "AddPost", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "AddPostVC") as! AddPostVC
            vc.postType = "group"
            vc.groupId = self.id ?? ""
            self.navigationController?.pushViewController(vc, animated: true)
        }
    }
    

        func tableView(_ tableView: UITableView, willDisplay cell: UITableViewCell, forRowAt indexPath: IndexPath) {
            if self.isData_nil == false{
                if self.groupPostsArray.count >= 10 {
                let count = self.groupPostsArray.count
                let lastElement = count - 1
                
                if indexPath.row == lastElement {
                    spinner.startAnimating()
                    spinner.frame = CGRect(x: CGFloat(0), y: CGFloat(0), width: tableView.bounds.width, height: CGFloat(44))
                    self.tableView.tableFooterView = spinner
                    self.tableView.tableFooterView?.isHidden = false
                    self.getGroupPosts(groupId: self.id ?? "", offset: self.afterpostid)
                    self.isData_nil = true
                }
            }
        }
    }
    
    @IBAction func Back(){
       NotificationCenter.default.removeObserver(self, name: NSNotification.Name(rawValue: "load"), object: nil)
         self.navigationController?.popViewController(animated: true)
     }
     @IBAction func gotoGroupMembers(){
         
         let vc = Storyboard.instantiateViewController(withIdentifier: "GroupMemberVC") as! GroupMembersController
         vc.groupId = self.groupId
         self.navigationController?.pushViewController(vc, animated: true)
     }
     
     @IBAction func inviteFriend(){
         let vc = Storyboard.instantiateViewController(withIdentifier: "InviteFriendVC") as! InviteFriendsController
         vc.groupId = self.groupId
         self.navigationController?.pushViewController(vc, animated: true)
         
     }
    
     
     @IBAction func gotoMoreVC() {
     let vc = Storyboard.instantiateViewController(withIdentifier: "GroupMoreVC") as! GroupMoreController
        if let groupUrl = self.groupData["url"] as? String{
            vc.groupUrl = groupUrl
        }
        vc.delegate = self
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    
    @IBAction func GroupJoin(sender:UIButton) {
       let position = (sender as AnyObject).convert(CGPoint.zero, to: self.tableView)
        let indexPath = self.tableView.indexPathForRow(at: position)!
        let cell = self.tableView.cellForRow(at: IndexPath(row: indexPath.row, section: 0)) as! GroupCoverCell
        if self.isAdmin == true {
        let Storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = Storyboard.instantiateViewController(withIdentifier: "GroupSettingVC") as! GroupSettingController
            vc.groupName = self.groupName
            vc.groupTitle = self.groupTitle
            vc.privacy = self.privacy
            vc.categoryName = self.category
            vc.categoryId = self.categoryId
            vc.about = self.aboutGroup
            vc.group_Id = self.groupId
            vc.categoryName = self.category
            vc.delegate = self
           self.navigationController?.pushViewController(vc, animated: true)
        }
        else {
        if self.isJoined == false {
            self.isJoined = true
            cell.joinGroupBtn.setTitle(NSLocalizedString("Joined", comment: "Joined"), for: .normal)
            cell.joinGroupBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#000000"), for:.normal)
            cell.joinGroupBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#e5e5e5")
            JoinGroup()
            self.delegte1.joinGroup(isJoin: true)
        }
            
       else {
        self.isJoined = false
        cell.joinGroupBtn.setTitle(NSLocalizedString("Join Group", comment: "Join Group"), for: .normal)
        cell.joinGroupBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#f8f8f8"), for:.normal)
        cell.joinGroupBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
        JoinGroup()
        self.delegte1.joinGroup(isJoin: false)
        }
    }
}
    
    
    @IBAction func EditGroupIcon(sender: UIButton) {
    let Storyboard = UIStoryboard(name: "Main", bundle: nil)
    let vc = Storyboard.instantiateViewController(withIdentifier: "CropImageVC") as! CropImageController
            vc.delegate = self
            vc.imageType = "avatar"
            vc.modalTransitionStyle = .coverVertical
            vc.modalPresentationStyle = .fullScreen
            self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func EditGroupCover (sender: UIButton) {
        let Storyboard = UIStoryboard(name: "Main", bundle: nil)
        let vc = Storyboard.instantiateViewController(withIdentifier: "CropImageVC") as! CropImageController
        vc.delegate = self
        vc.imageType = "cover"
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.present(vc, animated: true, completion: nil)
    }
    
    func deleteGroup(groupId: String) {
        if self.isFromList == true{
        self.delegate.deleteGroup(groupId: groupId)
        self.navigationController?.popViewController(animated: true)
        }
    }
    
    func uploadImage(imageType: String, image: UIImage) {
        let indexPath = IndexPath(row: 0, section: 0)
        let cell = self.tableView.cellForRow(at: indexPath) as! GroupCoverCell
        switch status {
        case .unknown, .offline:
            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            let imageData = image.jpegData(compressionQuality: 0.1)
            if imageType == "avatar" {
                self.uploadImage(imageType: "avatar", data: (imageData ?? nil)!)
                cell.profile.image = image
            }
            else{
                self.uploadImage(imageType: "cover", data: (imageData ?? nil)!)
                cell.cover.image = image

            }
        }
    }
    
}

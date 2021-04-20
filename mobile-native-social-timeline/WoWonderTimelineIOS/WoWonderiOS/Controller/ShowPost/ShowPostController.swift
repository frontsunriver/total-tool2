
import UIKit
import Kingfisher
import WoWonderTimelineSDK
import Toast_Swift
class ShowPostController: UIViewController {
    

    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    let Storyboard = UIStoryboard(name: "Main", bundle: nil)
    let status = Reach().connectionStatus()
    var isVideo:Bool = false
    
    var comments = [[String:Any]]()
   private var postsArray = [[String:Any]]()
    
    var postId: String? = nil
    var offset: String? = nil
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
            navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("Post", comment: "Post")
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        self.activityIndicator.startAnimating()
        self.tableView.tableFooterView = UIView()
        SetUpcells.setupCells(tableView: self.tableView)
        self.tableView.register(UINib(nibName: "CommentCellTableViewCell", bundle: nil), forCellReuseIdentifier: "CommentsCell")
        self.getPost(postId: self.postId ?? "")
        self.getPostComments()
    }
    
    override func viewWillAppear(_ animated: Bool) {
    AppInstance.instance.vc = "showPostVC"
    NotificationCenter.default.addObserver(self, selector: #selector(self.Notifire(notification:)), name: NSNotification.Name(rawValue: "Notifire"), object: nil)
    }
    override func viewWillDisappear(_ animated: Bool) {
    NotificationCenter.default.removeObserver(self, name: NSNotification.Name(rawValue: "Notifire"), object: nil)
       
        if isVideo{
            let indexpathforTextView = IndexPath(row: 0, section: 6)
                                let cell = tableView.cellForRow(at: indexpathforTextView)! as! VideoCell
            cell.playerView.pause()
        }
    }
   
    
    ///Network Connectivity.
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
        }
    }
  
    @objc func Notifire(notification: NSNotification){
        if let type = notification.userInfo?["type"] as? String{
            if type == "delete"{
                if let data = notification.userInfo?["userData"] as? Int{
                    print(data)
                    self.postsArray.remove(at: data)
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
                    if let groupid = self.postsArray[data]["group_id"] as? String{
                        groupId = groupid
                    }
                    if let page_Id = self.postsArray[data]["page_id"] as? String{
                        pageId = page_Id
                    }
                    if let userData = self.postsArray[data]["publisher"] as? [String:Any]{
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
                    if let shared_info = self.postsArray[data]["shared_info"] as? [String:Any]{
                        if shared_info != nil{
                            if let groupid = self.postsArray[data]["group_id"] as? String{
                                groupId = groupid
                            }
                            if let page_Id = self.postsArray[data]["page_id"] as? String{
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
                        else{
                            if let tag = notification.userInfo?["tag"] as? Int{
                                if let groupid = self.postsArray[tag]["group_id"] as? String{
                                    groupId = groupid
                                }
                                if let page_Id = self.postsArray[tag]["page_id"] as? String{
                                    pageId = page_Id
                                }
                                if let userData = self.postsArray[tag]["publisher"] as? [String:Any]{
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
                    }
                }
            }
        }
    }
    
    
    private func getPost(postId: String){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetPostByIdManager.sharedInstance.getPost(post_id: postId) { (success, authError, error) in
                    if success != nil{
                        self.postsArray.append(success!.post_data)
                        let delayTime = DispatchTime.now() + 2
                        DispatchQueue.main.asyncAfter(deadline: delayTime) {
                            self.activityIndicator.stopAnimating()
                            self.tableView.reloadData()
                        }
                        
                        print(self.postsArray)
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
    
    
    private func getPostComments(){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                FetchCommentManager.sharedInstance.fetchComment(postId: self.postId ?? "", offset: self.offset ?? "") { (success, authError, error) in
                    if success != nil {
                        self.comments.removeAll()
                        for i in success!.data {
                            self.comments.append(i)
                        }
                        
                        print(self.comments.count)
                        let delayTime = DispatchTime.now() + 3
                        DispatchQueue.main.asyncAfter(deadline: delayTime) {
                            self.tableView.reloadData()
                        }
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
    
    private func likeComment(commentId: String,type: String) {
        LikeCommentManager.sharedIntsance.likeComment(commentId: commentId, type: type) { (success, authError, error) in
            if success != nil {
                print(success?.api_status)
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

extension ShowPostController: UITableViewDelegate,UITableViewDataSource{
    
    func numberOfSections(in tableView: UITableView) -> Int {
        return 8
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if section == 0 {
            return 1
        }
        else if section == 1{
            return 1
        }
        else if section == 2{
            return 1
        }
        else if section == 3{
            return 1
        }
        else if section == 4{
            return 1
        }
        else if section == 5{
            return 1
        }
        else if section == 6{
            return self.postsArray.count
        }
        else {
            
            if self.comments.isEmpty == true || self.comments.count == 0{
                  return 1
            }
            else{
                 return self.comments.count
            }
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        if (indexPath.section == 0){
            let cell = UITableViewCell()
             self.tableView.rowHeight = 0
             return cell
        }
        else if (indexPath.section == 1){
            let cell = UITableViewCell()
             self.tableView.rowHeight = 0
             return cell
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
        else if (indexPath.section == 6){
    
        let index = self.postsArray[indexPath.row]
        var cell = UITableViewCell()
        var shared_info : [String:Any]? = nil
        var fundDonation: [String:Any]? = nil
        
        let postfile = index["postFile"] as? String ?? ""
              let postFile_full = index["postFile_full"] as? String ?? ""
            
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
        if let sharedInfo = index["shared_info"] as? [String:Any] {
            shared_info = sharedInfo
        }
        if let fund = index["fund_data"] as? [String:Any]{
            fundDonation = fund
        }
        if (shared_info != nil){
            cell = GetPostShare.sharedInstance.getsharePost(targetController: self, tableView: self.tableView, indexpath: indexPath, postFile: postfile, array: self.postsArray)
        }
        
       else if (postfile != "")  {
            let url = URL(string: postfile)
            let urlExtension: String? = url?.pathExtension
            if (urlExtension == "jpg" || urlExtension == "png" || urlExtension == "jpeg" || urlExtension == "JPG" || urlExtension == "PNG"){
                cell = GetPostWithImage.sharedInstance.getPostImage(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.postsArray, url: url!, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if(urlExtension == "wav" ||  urlExtension == "mp3" || urlExtension == "MP3"){
                cell = GetPostMp3.sharedInstance.getMP3(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.postsArray,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
            else if (urlExtension == "pdf") {
                cell = GetPostPDF.sharedInstance.getPostPDF(targetControler: self, tableView: tableView, indexpath: indexPath, postfile: postfile, array: self.postsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
                
            else {
                cell = GetPostVideo.sharedInstance.getVideo(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postFile_full, array: self.postsArray, url: url!, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
        }
        else if (postLink != "") {
            cell = GetPostWithLink.sharedInstance.getPostLink(targetController: self, tableView: tableView, indexpath: indexPath, postLink: postLink, array: self.postsArray,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
            
        else if (postYoutube != "") {
            cell = GetPostYoutube.sharedInstance.getPostYoutub(targetController: self, tableView: tableView, indexpath: indexPath, postLink: postYoutube, array: self.postsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            
        }
        else if (blog != "0") {
            cell = GetPostBlog.sharedInstance.GetBlog(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.postsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
            
        else if (group != false){
            cell = GetPostGroup.sharedInstance.GetGroupRecipient(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.postsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
            
        else if (product != "0") {
            cell = GetPostProduct.sharedInstance.GetProduct(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.postsArray,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        else if (event != "0") {
            cell = GetPostEvent.sharedInstance.getEvent(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array:  self.postsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            
        }
        else if (postSticker != "") {
            cell = GetPostSticker.sharedInstance.getPostSticker(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.postsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            
        }
            
        else if (colorId != "0"){
            cell = GetPostWithBg_Image.sharedInstance.postWithBg_Image(targetController: self, tableView: tableView, indexpath: indexPath, array: self.postsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
            
        else if (multi_image != "0") {
            cell = GetPostMultiImage.sharedInstance.getMultiImage(targetController: self, tableView: tableView, indexpath: indexPath, array: self.postsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            
        }
            
        else if photoAlbum != "" {
            cell = getPhotoAlbum.sharedInstance.getPhoto_Album(targetController: self, tableView: tableView, indexpath: indexPath, array: self.postsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
            
        else if postOptions != "0" {
            cell = GetPostOptions.sharedInstance.getPostOptions(targertController: self, tableView: tableView, indexpath: indexPath, array: self.postsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
            
        else if postRecord != ""{
            cell = GetPostRecord.sharedInstance.getPostRecord(targetController: self, tableView: tableView, indexpath: indexPath, array: self.postsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
            
        else if fundDonation != nil{
            cell = GetDonationPost.sharedInstance.getDonationpost(targetController: self, tableView: tableView, indexpath: indexPath, array: self.postsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        else {
            cell = GetNormalPost.sharedInstance.getPostText(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.postsArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        return cell
        }
        else  {
          
            if (self.comments.count == 0 || self.comments.isEmpty == true){
                 let cell = tableView.dequeueReusableCell(withIdentifier: "CommentsCell") as! CommentCellTableViewCell
                cell.noCommentView.isHidden = true
                self.tableView.rowHeight = 230.0
                return cell
            }
            else{
            let index = self.comments[indexPath.row]
            let cell = tableView.dequeueReusableCell(withIdentifier: "CommentsCell") as! CommentCellTableViewCell
            cell.noCommentView.isHidden = false
            self.tableView.rowHeight = UITableView.automaticDimension
            cell.imageHeight.constant = 0.0
            cell.imageWidth.isActive = false
            if let publisher = index["publisher"] as? [String:Any]{
                if let name = publisher["username"] as? String{
                    cell.userName.text = name
                }
                
                if let image = publisher["avatar"] as? String{
                    let url = URL(string: image)
                    cell.profileImage.kf.setImage(with: url)
                }
            }
            if let text = index["text"] as? String{
                cell.commentText?.text = text.htmlToString
            }
            if let image = index["c_file"] as? String{
                if image != ""{
                    let img = "\("https://wowonder.fra1.digitaloceanspaces.com/")\(image)"
                    let width = cell.designView.frame.size.width
                    print("Width",width)
                    cell.imageWidth.isActive = true
                    cell.imageWidth.constant = width
                    cell.imageHeight.constant = width
                    let url = URL(string: img)
                    cell.commentImage.kf.setImage(with: url)
                }
                else {
                    cell.imageWidth.isActive = false
                    cell.imageHeight.constant = 0.0
                }
            }
            //    }
            if let replies = index["replies"] as? String{
                if replies == "0"{
                    cell.replyBtn.setTitle("\("Reply")", for: .normal)
                }
                else {
                    cell.replyBtn.setTitle("\("Reply ")\("(\(replies))")", for: .normal)
                }
            }
            if let isLiked = index["is_comment_liked"] as? Bool{
                if isLiked{
                    cell.likeBtn.setTitle("Liked", for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                }
            }
            cell.replyBtn.tag = indexPath.row
            cell.likeBtn.tag = indexPath.row
            cell.replyBtn.addTarget(self, action: #selector(self.GotoCommentReply(sender:)), for: .touchUpInside)
            cell.likeBtn.addTarget(self, action: #selector(self.LikeComment(sender:)), for: .touchUpInside)
            cell.viewLeadingContraint.constant = 8.0
            return cell
            }
        }
    }
    
    @IBAction func GotoCommentReply(sender: UIButton){
        let vc = Storyboard.instantiateViewController(withIdentifier : "CommentReplyVC") as! CommentReplyController
        vc.comment = self.comments[sender.tag]
        vc.modalPresentationStyle = .fullScreen
        vc.modalTransitionStyle = .coverVertical
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func LikeComment(sender: UIButton){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            let cell = tableView.cellForRow(at: IndexPath(row: sender.tag, section: 2)) as! CommentCellTableViewCell
            var commentId :String? = nil
            if let id = self.comments[sender.tag]["id"] as? String{
                commentId = id
            }
            if let isLiked = self.comments[sender.tag]["is_comment_liked"] as? Bool{
                if isLiked{
                    cell.likeBtn.setTitle("Like", for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#333333"), for: .normal)
                    self.likeComment(commentId: commentId ?? "", type: "comment_dislike")
                }
                else {
                    cell.likeBtn.setTitle("Liked", for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                    self.likeComment(commentId: commentId ?? "", type: "comment_like")
                }
            }
        }
    }
}

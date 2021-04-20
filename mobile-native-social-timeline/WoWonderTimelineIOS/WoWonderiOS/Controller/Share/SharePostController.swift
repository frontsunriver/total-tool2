
import UIKit
import NotificationCenter
import Kingfisher
import SDWebImage
import WoWonderTimelineSDK
import ZKProgressHUD

class SharePostController: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    
    var posts = [[String:Any]]()
    
    var isGroup = false
    var isPage = false
    var groupId = ""
    var groupName = ""
    var pageId = ""
    var pageName = ""
    var text = ""
    var imageUrl = ""
    
    override func viewDidLoad() {
        super.viewDidLoad()
        SetUpcells.setupCells(tableView: self.tableView)
        self.tableView.backgroundColor = .white
        
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
    
    let status = Reach().connectionStatus()
    private func sharePostonTimeline(){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            ZKProgressHUD.show()
            SharePostOnTimelineManager.sharedInstance.sharePostOnTimeline(userId: UserData.getUSER_ID()!, postId: (posts.first!["post_id"] as? String)!) { (success, authError, error) in
                if success != nil {
                    ZKProgressHUD.dismiss()
                    self.view.makeToast("Post shared successfully")
                    let userInfo = ["data" : success?.data]
                    NotificationCenter.default.post(name: NSNotification.Name(rawValue: "load"), object: nil, userInfo: userInfo)
                    self.dismiss(animated: true, completion: nil)
                }
                else if authError != nil{
                    ZKProgressHUD.dismiss()
                    self.view.makeToast(authError?.errors.errorText)
                }
                else if error != nil {
                    ZKProgressHUD.dismiss()
                    self.view.makeToast(error?.localizedDescription)
                }
            }
        }
    }
    
    private func sharePostOnGroupandPage() {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            if self.isGroup == true{
                ZKProgressHUD.show()
                SharePost_PageandGroupManager.sharedInstance.sharePostonPageandGroup(type: "share_post_on_group", text: self.text, postId: (self.posts.first!["post_id"] as? String)!, pageId: "", groupId: self.groupId) { (success, authError, error) in
                    if success != nil {
                        ZKProgressHUD.dismiss()
                        self.view.makeToast("Post shared successfully")
                            let userInfo = ["data" : success?.data]
                            NotificationCenter.default.post(name: NSNotification.Name(rawValue: "load"), object: nil, userInfo: userInfo)
                        self.dismiss(animated: true, completion: nil)
                    }
                    else if authError != nil{
                        ZKProgressHUD.dismiss()
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        ZKProgressHUD.dismiss()
                        self.view.makeToast(error?.localizedDescription)
                    }
                }
            }
            else {
                ZKProgressHUD.show()
                SharePost_PageandGroupManager.sharedInstance.sharePostonPageandGroup(type: "share_post_on_page", text: self.text, postId: (self.posts.first!["post_id"] as? String)!, pageId: self.pageId, groupId: "") { (success, authError, error) in
                    if success != nil {
                        ZKProgressHUD.dismiss()
                        self.view.makeToast("Post shared successfully")
                        let userInfo = ["data" : success?.data]
                        NotificationCenter.default.post(name: NSNotification.Name(rawValue: "load"), object: nil, userInfo: userInfo)
                        self.dismiss(animated: true, completion: nil)
                    }
                    else if authError != nil{
                        ZKProgressHUD.dismiss()
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        ZKProgressHUD.dismiss()
                        self.view.makeToast(error?.localizedDescription)
                    }
                }
                
            }
        }
    }
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    
    @IBAction func Share(_ sender: Any) {
        if self.isGroup == true || self.isPage == true {
            self.tableView.reloadData()
            self.sharePostOnGroupandPage()
        }
        else {
            self.tableView.reloadData()
            self.sharePostonTimeline()
        }
        
    }
    
    deinit {
        NotificationCenter.default.removeObserver(self)
    }
}
extension SharePostController : UITableViewDelegate,UITableViewDataSource{
    
    func numberOfSections(in tableView: UITableView) -> Int {
        return 6
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
        else {
            return 1
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        if indexPath.section == 0{
            let cell = tableView.dequeueReusableCell(withIdentifier: "sharePost") as! SharePostCell
            if self.groupName != ""{
                cell.nameLabel.text! = self.groupName
            }
            else if self.pageName != "" {
                cell.nameLabel.text! = self.pageName
            }
            else {
                cell.nameLabel.text! = UserData.getUSER_NAME() ?? "Ali2233"
            }
            let url = URL(string: UserData.getImage() ?? "")
            cell.profileImage.sd_setImage(with: url, placeholderImage: #imageLiteral(resourceName: "no-avatar"), options: [], completed: nil)
            cell.textView.backgroundColor = .white
            self.tableView.rowHeight = 230.0
            if cell.textView.text! == "" || cell.textView.text! == "What's going on ? #Hashtag.. @Mention."{
                self.text = ""
            }
            else {
                self.text = cell.textView.text!
            }
            return cell
        }
        else if indexPath.section == 1{
            self.tableView.rowHeight = 02.0
            let cell = UITableViewCell()
            cell.backgroundColor = .white
            return cell
        }
        else if indexPath.section == 2{
            self.tableView.rowHeight = 0
            let cell = UITableViewCell()
            cell.backgroundColor = .white
            return cell
        }
        else if indexPath.section == 3{
            self.tableView.rowHeight = 0
            let cell = UITableViewCell()
            cell.backgroundColor = .white
            return cell
        }
        else if indexPath.section == 4{
            self.tableView.rowHeight = 0
            let cell = UITableViewCell()
            cell.backgroundColor = .white
            return cell
        }

        else  {
            let index = self.posts[indexPath.row]
            var tableViewCells = UITableViewCell()
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
                    print("NewsFeed",indexPath.row)
                    tableViewCells = GetPostWithImage.sharedInstance.getPostImage(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.posts, url: url!, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                }
                    
                else if(urlExtension == "wav" ||  urlExtension == "mp3" || urlExtension == "MP3"){
                    tableViewCells = GetPostMp3.sharedInstance.getMP3(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.posts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                }
                else if (urlExtension == "pdf") {
                    tableViewCells = GetPostPDF.sharedInstance.getPostPDF(targetControler: self, tableView: self.tableView, indexpath: indexPath, postfile: postfile, array: self.posts,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                    
                }
                    
                else {
                    tableViewCells = GetPostVideo.sharedInstance.getVideo(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.posts, url: url!, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                }
                
                
            }
                
            else if (postLink != "") {
                tableViewCells = GetPostWithLink.sharedInstance.getPostLink(targetController: self, tableView: tableView, indexpath: indexPath, postLink: postLink, array: self.posts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if (postYoutube != "") {
                tableViewCells = GetPostYoutube.sharedInstance.getPostYoutub(targetController: self, tableView: tableView, indexpath: indexPath, postLink: postYoutube, array: self.posts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
            else if (blog != "0") {
                tableViewCells = GetPostBlog.sharedInstance.GetBlog(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.posts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if (group != false){
                tableViewCells = GetPostGroup.sharedInstance.GetGroupRecipient(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.posts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if (product != "0") {
                tableViewCells = GetPostProduct.sharedInstance.GetProduct(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.posts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
            else if (event != "0") {
                tableViewCells = GetPostEvent.sharedInstance.getEvent(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array:  self.posts,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
            else if (postSticker != "") {
                tableViewCells = GetPostSticker.sharedInstance.getPostSticker(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.posts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
                
            else if (colorId != "0"){
                tableViewCells = GetPostWithBg_Image.sharedInstance.postWithBg_Image(targetController: self, tableView: tableView, indexpath: indexPath, array: self.posts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if (multi_image != "0") {
                tableViewCells = GetPostMultiImage.sharedInstance.getMultiImage(targetController: self, tableView: tableView, indexpath: indexPath, array: self.posts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
                
            else if photoAlbum != "" {
                tableViewCells = getPhotoAlbum.sharedInstance.getPhoto_Album(targetController: self, tableView: tableView, indexpath: indexPath, array: self.posts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if postOptions != "0" {
                tableViewCells = GetPostOptions.sharedInstance.getPostOptions(targertController: self, tableView: tableView, indexpath: indexPath, array: self.posts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if postRecord != ""{
                tableViewCells = GetPostRecord.sharedInstance.getPostRecord(targetController: self, tableView: tableView, indexpath: indexPath, array: self.posts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
                
            else {
                tableViewCells = GetNormalPost.sharedInstance.getPostText(targetController: self, tableView: self.tableView, indexpath: indexPath, postFile: "", array: self.posts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
            return tableViewCells
            
        }
        
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        let index = self.posts[indexPath.row]
        
    }
    
    
}

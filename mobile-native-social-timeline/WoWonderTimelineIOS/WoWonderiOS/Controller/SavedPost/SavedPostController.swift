

import UIKit
import Toast_Swift
import Kingfisher
import WoWonderTimelineSDK
class SavedPostController: UIViewController {

    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    var posts = [[String:Any]]()
    
    var offset: String? = nil
    let status = Reach().connectionStatus()
    let spinner = UIActivityIndicatorView(style: .gray)
    let pulltoRefresh = UIRefreshControl()
    
    override func viewDidLoad() {
        super.viewDidLoad()
    NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("Saved Post", comment: "Saved Post")
        self.tableView.backgroundColor = .white
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        SetUpcells.setupCells(tableView: self.tableView)
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.tableView.addSubview(pulltoRefresh)
        self.activityIndicator.startAnimating()
        self.tableView.tableFooterView = UIView()
        self.getSavedPost(offset: "")
    }
    
    override func viewWillAppear(_ animated: Bool) {
        AppInstance.instance.vc = "savedPostVC"
        NotificationCenter.default.addObserver(self, selector: #selector(self.Notifire(notification:)), name: NSNotification.Name(rawValue: "Notifire"), object: nil)
    }
    override func viewWillDisappear(_ animated: Bool) {
    NotificationCenter.default.removeObserver(self, name: NSNotification.Name(rawValue: "Notifire"), object: nil)
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
        self.posts.removeAll()
        self.tableView.reloadData()
        self.getSavedPost(offset: self.offset ?? "")
    }
    
    @objc func Notifire(notification: NSNotification){
        if let type = notification.userInfo?["type"] as? String{
            if type == "delete"{
                if let data = notification.userInfo?["userData"] as? Int{
                    print(data)
                    self.posts.remove(at: data)
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
                    if let groupid = self.posts[data]["group_id"] as? String{
                        groupId = groupid
                    }
                    if let page_Id = self.posts[data]["page_id"] as? String{
                        pageId = page_Id
                    }
                    if let userData = self.posts[data]["publisher"] as? [String:Any]{
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
                var shared: [String:Any]? = nil
                if let data = notification.userInfo?["userData"] as? Int{
                    if let shared_info = self.posts[data]["shared_info"] as? [String:Any]{
                        shared = shared_info
                    }
                    if shared != nil{
                        if let groupid = self.posts[data]["group_id"] as? String{
                            groupId = groupid
                        }
                        if let page_Id = self.posts[data]["page_id"] as? String{
                            pageId = page_Id
                        }
                        if let publisher = shared?["publisher"] as? [String:Any]{
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
                            if let groupid = self.posts[tag]["group_id"] as? String{
                                groupId = groupid
                            }
                            if let page_Id = self.posts[tag]["page_id"] as? String{
                                pageId = page_Id
                            }
                            if let userData = self.posts[tag]["publisher"] as? [String:Any]{
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
    
    private func getSavedPost(offset: String){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetSavedPostManager.sharedInstance.getSavedPost(afterPostId: offset) { (success, authError, error) in
                    if success != nil{
                        for i in success!.data{
                            self.posts.append(i)
                        }
                        self.offset = self.posts.last?["post_id"] as? String ?? ""
                        self.pulltoRefresh.endRefreshing()
                        self.activityIndicator.stopAnimating()
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
}
extension SavedPostController: UITableViewDelegate,UITableViewDataSource{
    func numberOfSections(in tableView: UITableView) -> Int {
        return 7
    }
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if (section == 0){
            return 1
        }
        else if (section == 1){
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
        else{
        return self.posts.count
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
       
        else {
            let index = self.posts[indexPath.row]
            var tableViewCells = UITableViewCell()
            var shared_info : [String:Any]? = nil
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
            if let sharedInfo = index["shared_info"] as? [String:Any] {
                shared_info = sharedInfo
            }
            
            if (shared_info != nil){
                tableViewCells = GetPostShare.sharedInstance.getsharePost(targetController: self, tableView: self.tableView, indexpath: indexPath, postFile: postfile, array: self.posts)
            }
                
            else if (postfile != "")  {
                let url = URL(string: postfile)
                let urlExtension: String? = url?.pathExtension
                if (urlExtension == "jpg" || urlExtension == "png" || urlExtension == "jpeg" || urlExtension == "JPG" || urlExtension == "PNG"){
                    print("NewsFeed",indexPath.row)
                    tableViewCells = GetPostWithImage.sharedInstance.getPostImage(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.posts, url: url!, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                }
                    
                else if(urlExtension == "wav" ||  urlExtension == "mp3" || urlExtension == "MP3"){
                    tableViewCells = GetPostMp3.sharedInstance.getMP3(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.posts,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
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
                tableViewCells = GetPostYoutube.sharedInstance.getPostYoutub(targetController: self, tableView: tableView, indexpath: indexPath, postLink: postYoutube, array: self.posts,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
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
    
    
    func tableView(_ tableView: UITableView, willDisplay cell: UITableViewCell, forRowAt indexPath: IndexPath) {
        
        if self.posts.count >= 10 {
            let count = self.posts.count
            let lastElement = count - 1
            
            if indexPath.row == lastElement {
                spinner.startAnimating()
                spinner.frame = CGRect(x: CGFloat(0), y: CGFloat(0), width: tableView.bounds.width, height: CGFloat(44))
                self.tableView.tableFooterView = spinner
                self.tableView.tableFooterView?.isHidden = false
                self.getSavedPost(offset: self.offset ?? "")
            }
        }
    }
    
    
    
}

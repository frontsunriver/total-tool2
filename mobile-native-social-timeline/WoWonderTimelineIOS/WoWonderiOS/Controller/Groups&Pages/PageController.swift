
import UIKit
import Kingfisher
import SDWebImage
import WoWonderTimelineSDK
import GoogleMobileAds

class PageController: UIViewController,EditPageDelegete,DeletePageDelegate,PageMoreDelegate,PageRatingDelegate{
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var messageLabel: UILabel!
    
    let Storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
    let spinner = UIActivityIndicatorView(style: .gray)
    
    var pagePostArray = [[String:Any]]()
    var pagelikes = "", afterpostid  = "0"
    var isLike  = false
    var isPageOwner = false
    var pageData : ForwardPageData!
    var delegate : EditPageDelegete!
    var deleteDelegate : DeletePageDelegate!
    var likeDelegate: PageLikeDelegate?
    var page_data = [String:Any]()
    var page_id: String? = nil
    var isFromList: Bool = false
    var isData_nil: Bool = false
    let status = Reach().connectionStatus()
      var interstitial: GADInterstitial!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationItem.hidesBackButton = true
        self.navigationController?.navigationBar.isHidden = true
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        NotificationCenter.default.addObserver(self, selector: #selector(loadList), name: NSNotification.Name(rawValue: "load"), object: nil)
        self.messageLabel.isHidden = true
        tableView.register(UINib(nibName: "PageCoverCell", bundle: nil), forCellReuseIdentifier: "PageCover")
        self.tableView.tableFooterView = UIView()
        SetUpcells.setupCells(tableView: self.tableView)
        self.getPagePosts(pageId: self.page_id ?? "", afterPostId: self.afterpostid)
        self.getPageInfo(pageId: Int(self.page_id ?? "") ?? 0)
        self.getPageData(pageId: self.page_id ?? "")
        self.tableView.reloadData()
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
    
    override func viewWillAppear(_ animated: Bool) {
        AppInstance.instance.vc = "pageVC"
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
                    GetPagePostManager.sharedInstance.getGroupPost(pageId: self.page_id ?? "", afterPostId: "") {[weak self] (success, authError, error) in
                        if success != nil {
                            for i in success!.data{
                                if i["post_id"] as? String == post_id{
                                    self?.pagePostArray.insert(i, at: 0)
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
                        else if authError != nil{
                            self?.spinner.stopAnimating()
                            self?.view.makeToast(authError?.errors.errorText)
                        }
                        else if error != nil {
                            self?.spinner.stopAnimating()
                            self?.view.makeToast(error?.localizedDescription)
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
                    self.pagePostArray.remove(at: data)
                    self.tableView.reloadData()
                }
            }
            else if (type == "profile"){
                let storyBoard = UIStoryboard(name: "Main", bundle: nil)
                let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
                if let data = notification.userInfo?["userData"] as? Int{
                    print(data)
                    if let userData = self.pagePostArray[data]["publisher"] as? [String:Any]{
                        vc.userData = userData
                    }
                }
                self.navigationController?.pushViewController(vc, animated: true)
            }
            else if (type == "share"){
                let storyBoard = UIStoryboard(name: "Main", bundle: nil)
                let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
                var groupId: String? = nil
                var pageId: String? = nil
                var user_data: [String:Any]? = nil
                if let data = notification.userInfo?["userData"] as? Int{
                    if let shared_info = self.pagePostArray[data]["shared_info"] as? [String:Any]{
                        if shared_info != nil{
                            if let groupid = self.pagePostArray[data]["group_id"] as? String{
                                groupId = groupid
                            }
                            if let page_Id = self.pagePostArray[data]["page_id"] as? String{
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
                                if let groupid = self.pagePostArray[tag]["group_id"] as? String{
                                    groupId = groupid
                                }
                                if let page_Id = self.pagePostArray[tag]["page_id"] as? String{
                                    pageId = page_Id
                                }
                                if let userData = self.pagePostArray[tag]["publisher"] as? [String:Any]{
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
    
    private func getPageData(pageId: String){
        switch status {
        case .unknown, .offline:
            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetPageDataManager.sharedInstance.getData(page_id: pageId) { (success, authError, error) in
                    if success != nil{
                        self.page_data = success?.page_data ?? [:]
                        self.isPageOwner = self.page_data["is_page_onwer"] as? Bool ?? false
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
    
    private func getPagePosts(pageId : String, afterPostId : String) {
        switch status {
        case .unknown, .offline:
            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetPagePostManager.sharedInstance.getGroupPost(pageId: pageId, afterPostId: afterPostId) { (success, authError, error) in
                    if success != nil {
                        for i in success!.data{
                            print(success!.data)
                            self.pagePostArray.append(i)
                        }
                        self.afterpostid = self.pagePostArray.last?["post_id"] as? String ?? "0"
                        print(self.pagePostArray.count)
                        if success?.data.count == 0 {
                            //self.messageLabel.isHidden = false
                            self.isData_nil = true
                        }
                        else{
                            self.isData_nil = false
//                            self.messageLabel.isHidden = true
                        }
                        self.spinner.stopAnimating()
                        self.tableView.reloadData()
                    }
                    else if authError != nil{
                        self.spinner.stopAnimating()
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        self.spinner.stopAnimating()
                        self.view.makeToast(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    private func getPageInfo(pageId : Int){
        GetPageInfoManager.sharedInstance.getPageInfo(pageId: pageId) { (success, authError, error) in
            if success != nil {
                let data = success?.page_data
                if let totalLikes = data!["likes_count"] as? String{
                    self.pagelikes = totalLikes
                }
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
    
    private func likePage(){
        LikePageManager.sharedInstance.likePage(pageId: Int(self.page_id ?? "")!) { (success, authError, error) in
            if success != nil {
                print(success?.like_status)
            }
                
            else if authError != nil {
                self.showAlert(title: "", message: (authError?.errors.errorText)!)
            }
                
            else if error != nil {
                print(error?.localizedDescription)
                
            }
            
        }
    }
    
    private func uploadImage(imageType:String,data:Data){
        UpdatePageDataManager.sharedInstance.uploadImage(pageId: self.page_id ?? "", imageType: imageType , data: data) { (success, authError, error) in
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
    
    
    @IBAction func backtoPreviousVC() {
    NotificationCenter.default.removeObserver(self, name: NSNotification.Name(rawValue: "load"), object: nil)
    self.navigationController?.popViewController(animated: true)
    }
    
    @IBAction func inviteFriend(){
        let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "InviteFriendVC") as! InviteFriendsController
        vc.pageId = self.page_id ?? ""
        self.navigationController?.pushViewController(vc, animated: true)
        
    }
    
    @IBAction func pageLike(sender:UIButton) {
        let position = (sender as AnyObject).convert(CGPoint.zero, to: self.tableView)
        let indexPath = self.tableView.indexPathForRow(at: position)!
        let cell = tableView!.cellForRow(at: IndexPath(row: indexPath.row, section: 0)) as! PageCoverCell
        if cell.likeBtn.title(for: .normal) == NSLocalizedString("Edit", comment: "Edit"){
            let vc = Storyboard.instantiateViewController(withIdentifier: "PageSettingVC") as! PageSettingController
            vc.pageData = self.pageData
            vc.page_Data = self.page_data
            vc.delegate = self
            vc.deleteDelegate = self
            self.navigationController?.pushViewController(vc, animated: true)
        }
        else {
            if let isLiked = self.page_data["is_liked"] as? Bool{
                if isLiked == false{
                    cell.likeBtn.setTitle(NSLocalizedString("Liked", comment: "Liked"), for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#000000"), for:.normal)
                    cell.likeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#e5e5e5")
                    self.likePage()
                    self.page_data["is_liked"] = true
                    self.likeDelegate?.pageLiked(isLike: true)
                }
                else {
                    cell.likeBtn.setTitle(NSLocalizedString("Like", comment: "Like"), for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#f8f8f8"), for:.normal)
                    cell.likeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                    self.likePage()
                    self.page_data["is_liked"] = true
                    self.likeDelegate?.pageLiked(isLike: false)
                }
            }
        }
    }
    
    @IBAction func Rating(sender: UIButton){
        let vc = Storyboard.instantiateViewController(withIdentifier: "PageRatingVC") as! PageRatingController
        vc.delegate = self
        vc.pageId = self.page_id ?? ""
        if let pageName = self.page_data["page_name"] as? String{
             vc.page_name = pageName
        }
        vc.modalTransitionStyle = .crossDissolve
        vc.modalPresentationStyle = .overCurrentContext
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func openLink(sender: UIButton) {
        let position = (sender as AnyObject).convert(CGPoint.zero, to: self.tableView)
        let indexPath = self.tableView.indexPathForRow(at: position)!
        let cell = self.tableView.cellForRow(at: IndexPath(row: indexPath.row, section: 0)) as! PageCoverCell
        if cell.callActionBtn.title(for: .normal) == NSLocalizedString("Edit", comment: "Edit") {
            let vc = Storyboard.instantiateViewController(withIdentifier: "PageSettingVC") as! PageSettingController
            vc.pageData = self.pageData
            vc.page_Data = self.page_data
            vc.delegate = self
            vc.deleteDelegate = self
            self.navigationController?.pushViewController(vc, animated: true)
        }
            
        else {
            if let action_type = self.page_data["call_action_type"] as? String{
                if action_type == "0"{
                    if let is_liked = self.page_data["is_liked"] as? Bool{
                        if is_liked == false{
                            cell.callActionBtn.setTitle(NSLocalizedString("Liked", comment: "Liked"), for: .normal)
                            cell.callActionBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#000000"), for:.normal)
                            cell.callActionBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#e5e5e5")
                            self.likePage()
                            self.page_data["is_liked"] = true
                            self.likeDelegate?.pageLiked(isLike: true)
                        }
                        else {
                            cell.callActionBtn.setTitle(NSLocalizedString("Like", comment: "Like"), for: .normal)
                            cell.callActionBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#ffffff"), for:.normal)
                            cell.callActionBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                            self.likePage()
                            self.page_data["is_liked"] = false
                            self.likeDelegate?.pageLiked(isLike: false)
                        }
                    }
                }
                else {
                    if let action_url = self.page_data["call_action_type_url"] as? String{
                        guard let url = URL(string: action_url) else {
                            return //be safe
                        }
                        UIApplication.shared.open(url, options: [:], completionHandler: nil)
                    }
                }
            }
  
            
        }
    }
    
    func pageRating(rating: Double) {
        self.view.makeToast("Done")
        self.page_data["rating"] = rating
        self.tableView.reloadData()
    }
    
    func gotoSetting(type: String) {
        var url: String? = nil
        if type == "copy"{
            if let page_url = self.page_data["url"] as? String{
                url = page_url
                UIPasteboard.general.string = url
                self.view.makeToast(NSLocalizedString("Copied", comment: "Copied"))

            }
        }
        else if type == "share"{
            // text to share
            let text = url
            
            // set up activity view controller
            let textToShare = [ text ]
            let activityViewController = UIActivityViewController(activityItems: textToShare, applicationActivities: nil)
            activityViewController.popoverPresentationController?.sourceView = self.view // so that iPads won't crash
            
            // exclude some activity types from the list (optional,)
            activityViewController.excludedActivityTypes = [ UIActivity.ActivityType.airDrop, UIActivity.ActivityType.postToFacebook, UIActivity.ActivityType.assignToContact,UIActivity.ActivityType.mail,UIActivity.ActivityType.postToTwitter,UIActivity.ActivityType.message,UIActivity.ActivityType.postToFlickr,UIActivity.ActivityType.postToVimeo,UIActivity.ActivityType.init(rawValue: "net.whatsapp.WhatsApp.ShareExtension"),UIActivity.ActivityType.init(rawValue: "com.google.Gmail.ShareExtension"),UIActivity.ActivityType.init(rawValue: "com.toyopagroup.picaboo.share"),UIActivity.ActivityType.init(rawValue: "com.tinyspeck.chatlyio.share")]
            
            // present the view controller
            self.present(activityViewController, animated: true, completion: nil)
            
        }
        else if type == "reviews"{
            let vc = Storyboard.instantiateViewController(withIdentifier: "PageReviewVC") as! PageReviewController
            vc.pageId = self.page_id ?? ""
            self.navigationController?.pushViewController(vc, animated: true)
        }
        else if type == "BoostPage"{
            print("Boost Page")
        }
        else if type == "setting"{
            let vc = Storyboard.instantiateViewController(withIdentifier: "PageSettingVC") as! PageSettingController
            vc.pageData = self.pageData
            vc.page_Data = self.page_data
            vc.delegate = self
            vc.deleteDelegate = self
            self.navigationController?.pushViewController(vc, animated: true)
        }
    }
    @objc func GotoAddPost(sender: UIButton){
        let storyboard = UIStoryboard(name: "AddPost", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "AddPostVC") as! AddPostVC
        self.navigationController?.pushViewController(vc, animated: true)
    }
}

extension PageController : UITableViewDataSource,UITableViewDelegate,uploadImageDelegate {
    
    func numberOfSections(in tableView: UITableView) -> Int {
        return 6
    }
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if section == 0 {
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
        else {
            return self.pagePostArray.count
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        if indexPath.section == 0 {
            let cell = tableView.dequeueReusableCell(withIdentifier: "PageCover") as! PageCoverCell
            self.tableView.rowHeight = UITableView.automaticDimension
            self.tableView.estimatedRowHeight = 485.0
            cell.likeBtn.isHidden = true
            if let cover = self.page_data["cover"] as? String{
                let url = URL(string: cover)
                cell.coverImage.sd_setImage(with: url, placeholderImage: #imageLiteral(resourceName: "Cover_image"), options: [], completed: nil)
            }
            if let avatar = self.page_data["avatar"] as? String{
                let url = URL(string: avatar)
                cell.pageIcon.sd_setImage(with: url, placeholderImage: #imageLiteral(resourceName: "PagesIcon"), options: [], completed: nil)
            }
            if let title = self.page_data["page_title"] as? String{
                cell.pageTitle.text = title
            }
            if let name = self.page_data["page_name"] as? String{
                cell.pageName.text = "\("@")\(name)"
            }
            if let rating = self.page_data["rating"] as? Double{
                cell.ratingView.rating = rating
            }
            if let category = self.page_data["category"] as? String{
                cell.categoryBtn.setTitle("\("       ")\(category)", for: .normal)
            }
            cell.ratingView.settings.updateOnTouch = false
            cell.backBtn.addTarget(self, action: #selector(self.backtoPreviousVC), for: .touchUpInside)
            cell.likeCountBtn.setTitle("\("       ")\(self.pagelikes)", for: .normal)
            cell.likeBtn.addTarget(self, action: #selector(self.pageLike(sender:)), for: .touchUpInside)
            cell.invteBtn.addTarget(self, action: #selector(self.inviteFriend), for: .touchUpInside)
            cell.callActionBtn.addTarget(self, action: #selector(self.openLink(sender:)), for: .touchUpInside)
            cell.moreBtn.addTarget(self, action: #selector(self.gotoMore), for: .touchUpInside)
            cell.EditIcon.addTarget(self, action: #selector(self.EditPageIcon), for: .touchUpInside)
            cell.EditCover.addTarget(self, action: #selector(self.EditPageCover), for: .touchUpInside)
            cell.ratingBtn.addTarget(self, action: #selector(self.Rating(sender:)), for: .touchUpInside)
            if let about = self.page_data["about"] as? String{
                if about == "" {
                    cell.descriptionLabel.text = "No Any Description of this page"
                }
                else {
                    if let descrip = self.page_data["page_description"] as? String{
                        cell.descriptionLabel.text = descrip.htmlToString
                    }
            }
         
            }
            if self.isPageOwner == true {
                cell.likeBtn.setTitle(NSLocalizedString("Edit", comment: "Edit"), for: .normal)
                cell.ratingBtn.isEnabled = false
                cell.ratingView.isHidden = true
                cell.ratingLabel.isHidden = true
            }
            else {
                cell.editView.isHidden = true
                cell.EditCover.isHidden = true
                if let is_liked = self.page_data["is_liked"] as? Bool{
                    if is_liked == false{
                        cell.likeBtn.setTitle(NSLocalizedString("Like", comment: "Like"), for: .normal)
                        cell.likeBtn.setTitleColor(.white, for: .normal)
                        cell.likeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                    }
                    else {
                        cell.likeBtn.setTitle(NSLocalizedString("Liked", comment: "Liked"), for: .normal)
                        cell.likeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#e5e5e5")
                        cell.likeBtn.setTitleColor(.black, for: .normal)
                    }
                }
            }
            if let call_action = self.page_data["call_action_type"] as? String{
                if call_action != "0"{
                    cell.likeBtn.isHidden = false
                    if let call_actionText = self.page_data["call_action_type_text"] as? String{
                        cell.callActionBtn.setTitle(call_actionText, for: .normal)
                    }
                }
                else {
                    cell.likeBtn.isHidden = true
                    if self.isPageOwner == true {
                        cell.callActionBtn.setTitle(NSLocalizedString("Edit", comment: "Edit"), for: .normal)
                        cell.editView.isHidden = false
                        cell.EditCover.isHidden = false
                    }
                    else {
                        cell.editView.isHidden = true
                        cell.EditCover.isHidden = true
                        if let is_liked = self.page_data["is_liked"] as? Bool{
                            if is_liked == false{
                                cell.callActionBtn.setTitle(NSLocalizedString("Like", comment: "Like"), for: .normal)
                                cell.callActionBtn.setTitleColor(.white, for: .normal)
                                cell.callActionBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                            }
                            else{
                                cell.callActionBtn.setTitle(NSLocalizedString("Liked", comment: "Liked"), for: .normal)
                                cell.callActionBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#e5e5e5")
                                cell.callActionBtn.setTitleColor(.black, for: .normal)
                            }
                        }
                    }
                }
            }
            
            return cell
        }
        else if (indexPath.section) == 1{
            if self.isPageOwner == true {
                let cell = tableView.dequeueReusableCell(withIdentifier: "AddPostCells") as! AddPostCell
                cell.moreBtn.addTarget(self, action: #selector(self.GotoAddPost(sender:)), for: .touchUpInside)
                cell.photoBtn.addTarget(self, action: #selector(self.GotoAddPost(sender:)), for: .touchUpInside)
                cell.bind(page_data: self.page_data)
                self.tableView.rowHeight = 60.0
                return cell
            }
            else{
                let cell = UITableViewCell()
                self.tableView.rowHeight = 0
                return cell
            }
            
        }
        else if (indexPath.section) == 2{
            let cell = UITableViewCell()
            self.tableView.rowHeight = 0
            return cell
        }
        else if (indexPath.section) == 3{
            let cell = UITableViewCell()
            self.tableView.rowHeight = 0
            return cell
        }
        else if (indexPath.section) == 4{
            let cell = UITableViewCell()
            self.tableView.rowHeight = 0
            return cell
        }
            
        else {
            var cell = UITableViewCell()
            let index = self.pagePostArray[indexPath.row]
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
                    cell = GetPostWithImage.sharedInstance.getPostImage(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.pagePostArray, url: url!, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                }
                    
                else if(urlExtension == "wav" ||  urlExtension == "mp3" || urlExtension == "MP3"){
                    cell = GetPostMp3.sharedInstance.getMP3(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.pagePostArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                }
                else if (urlExtension == "pdf") {
                    cell = GetPostPDF.sharedInstance.getPostPDF(targetControler: self, tableView: tableView, indexpath: indexPath, postfile: postfile, array: self.pagePostArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                    
                }
                    
                else {
                    cell = GetPostVideo.sharedInstance.getVideo(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.pagePostArray, url: url!, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                }
                
                
            }
                
            else if (postLink != "") {
                cell = GetPostWithLink.sharedInstance.getPostLink(targetController: self, tableView: tableView, indexpath: indexPath, postLink: postLink, array: self.pagePostArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if (postYoutube != "") {
                cell = GetPostYoutube.sharedInstance.getPostYoutub(targetController: self, tableView: tableView, indexpath: indexPath, postLink: postYoutube, array: self.pagePostArray,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
            else if (blog != "0") {
                cell = GetPostBlog.sharedInstance.GetBlog(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.pagePostArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if (group != false){
                cell = GetPostGroup.sharedInstance.GetGroupRecipient(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.pagePostArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if (product != "0") {
                cell = GetPostProduct.sharedInstance.GetProduct(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.pagePostArray,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
            else if (event != "0") {
                cell = GetPostEvent.sharedInstance.getEvent(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array:  self.pagePostArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
            else if (postSticker != "") {
                cell = GetPostSticker.sharedInstance.getPostSticker(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.pagePostArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
                
            else if (colorId != "0"){
                cell = GetPostWithBg_Image.sharedInstance.postWithBg_Image(targetController: self, tableView: tableView, indexpath: indexPath, array: self.pagePostArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if (multi_image != "0") {
                cell = GetPostMultiImage.sharedInstance.getMultiImage(targetController: self, tableView: tableView, indexpath: indexPath, array: self.pagePostArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
                
            else if photoAlbum != "" {
                cell = getPhotoAlbum.sharedInstance.getPhoto_Album(targetController: self, tableView: tableView, indexpath: indexPath, array: self.pagePostArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if postOptions != "0" {
                cell = GetPostOptions.sharedInstance.getPostOptions(targertController: self, tableView: tableView, indexpath: indexPath, array: self.pagePostArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if postRecord != ""{
                cell = GetPostRecord.sharedInstance.getPostRecord(targetController: self, tableView: tableView, indexpath: indexPath, array: self.pagePostArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
            else {
                cell = GetNormalPost.sharedInstance.getPostText(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.pagePostArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
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
            vc.postType = "page"
            if let page_id = self.page_data["page_id"] as? String{
                vc.pageid = page_id
            }
            self.navigationController?.pushViewController(vc, animated: true)
        }
    }
    
    func tableView(_ tableView: UITableView, willDisplay cell: UITableViewCell, forRowAt indexPath: IndexPath) {
        if self.isData_nil == false{
        if self.pagePostArray.count >= 10 {
            let count = self.pagePostArray.count
            let lastElement = count - 1
            
            if indexPath.row == lastElement {
                spinner.startAnimating()
                spinner.frame = CGRect(x: CGFloat(0), y: CGFloat(0), width: tableView.bounds.width, height: CGFloat(44))
                self.tableView.tableFooterView = spinner
                self.tableView.tableFooterView?.isHidden = false
                self.getPagePosts(pageId: self.page_id ?? "", afterPostId: self.afterpostid)
                self.isData_nil = true
            }
        }
    }
}
    
    func tableView(_ tableView: UITableView, didEndDisplaying cell: UITableViewCell, forRowAt indexPath: IndexPath) {
        GetPostRecord.sharedInstance.stopSound()
    }

    
    @IBAction func EditPageIcon (){
        let Storyboard = UIStoryboard(name: "Main", bundle: nil)
        let vc = Storyboard.instantiateViewController(withIdentifier: "CropImageVC") as! CropImageController
        vc.delegate = self
        vc.imageType = "avatar"
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func EditPageCover (){
        let Storyboard = UIStoryboard(name: "Main", bundle: nil)
        let vc = Storyboard.instantiateViewController(withIdentifier: "CropImageVC") as! CropImageController
        vc.delegate = self
        vc.imageType = "cover"
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.present(vc, animated: true, completion: nil)
    }
    
    
    @IBAction func gotoMore(){
        let vc = Storyboard.instantiateViewController(withIdentifier: "More") as! PageandGroupMoreController
        vc.delegate1 = self
        vc.isPageowner = self.isPageOwner
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.navigationController?.present(vc, animated: true, completion: nil)
    }
    
    
    func editPage(pageData: [String:Any]) {
//        self.pageData = pageData
          self.page_data = pageData
//        vc.page_Data = self.page_data
        self.tableView.reloadData()
        if self.isFromList{
        self.delegate.editPage(pageData: pageData)
        }
    }
    
    func deletePage(pageId: String) {
        if self.isFromList{
        self.deleteDelegate.deletePage(pageId: self.page_id ?? "")
        self.navigationController?.popViewController(animated: true)
        }
    }
    func uploadImage(imageType: String, image: UIImage) {
        let indexPath = IndexPath(row: 0, section: 0)
        let cell = self.tableView.cellForRow(at: indexPath) as! PageCoverCell
        switch status {
        case .unknown, .offline:
            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            print(imageType)
            let imageData = image.jpegData(compressionQuality: 0.1)
            if imageType == "avatar"{
                self.uploadImage(imageType: "avatar", data: (imageData ?? nil)!)
                cell.pageIcon.image = image
            }
            else{
                self.uploadImage(imageType: "cover", data: (imageData ?? nil)!)
                cell.coverImage.image = image
            }
        }
    }
}

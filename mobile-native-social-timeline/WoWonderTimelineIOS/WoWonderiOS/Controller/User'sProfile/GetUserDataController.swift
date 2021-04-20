

import UIKit
import ZKProgressHUD
import Kingfisher
import Toast_Swift
import WoWonderTimelineSDK
//import GiphyUISDK
//import GiphyCoreSDK
import GoogleMobileAds



protocol sendGroupData{
    func groupData(groupArray : [[String:Any]])
    
}

class GetUserDataController: UIViewController,blockUserDelegate,ProfileMoreDelegate,didSelectGIFDelegate{

    var delegate : FilterBlockUser?
    var bannerView: GADBannerView!
    var interstitial: GADInterstitial!
    
    @IBOutlet weak var tableView: UITableView!
    var postsArray = [[String : Any]]()
    var pageArray = [[String:Any]]()
    var groupArray = [[String:Any]]()
    var imagesArray = [[String:Any]]()
    var followersArray = [[String:Any]]()
    var followingArray = [[String:Any]]()
    let status = Reach().connectionStatus()
    let spinner = UIActivityIndicatorView(style: .gray)
    var userProfileData : UserProfileData!
    var userData: [String:Any]? = nil
    var userInfo = [[String:Any]]()
    private var off_set: String? = nil
    var user_id: String? = nil
    var profileImageURL = ""
    var coverImageURL = ""
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.isHidden = true
        self.navigationItem.hidesBackButton = true
        tableView.register(UINib(nibName: "UserCoverView", bundle: nil), forCellReuseIdentifier: "Cover")
        NotificationCenter.default.addObserver(self, selector: #selector(GetUserDataController.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
     
        self.tableView.delegate = self
        self.tableView.dataSource = self
        self.tableView.register(UINib(nibName: "UserInfoCell", bundle: nil), forCellReuseIdentifier: "USerInfoCells")
        SetUpcells.setupCells(tableView: self.tableView)
        if let userId = self.userData? ["user_id"] as? String{
            self.user_id = userId
        }
        var web = ""
        var school = ""
        var gender = ""
        var lastseen = ""
        var birthDay = ""
        var working = ""
        var living = ""
        
        if let webp = self.userData?["website"] as? String{
            web = webp
        }
        if let schools = self.userData?["school"] as? String{
            school = schools
        }
        if let gendr = self.userData?["gender_text"] as? String{
            gender = gendr
        }
        if let lastSeen = self.userData?["lastseen_time_text"] as? String{
            lastseen = lastSeen
        }
        if let birt = self.userData?["birthday"] as? String{
             birthDay = birt
        }
        if let work = self.userData?["working"] as? String{
            working = work
        }
        if let liv = self.userData?["address"] as? String{
            living  = liv
        }
        
        let website = ["website":web]
        let schol = ["school":school]
        let gendr = ["gender_text":gender]
        let lastsen = ["lastseen_time_text":lastseen]
        let birth = ["birthday":birthDay]
        let works = ["working":working]
        let live = ["address":living]
        
        if (lastseen != ""){
            self.userInfo.append(lastsen)
        }
         if (web != ""){
            self.userInfo.append(website)
        }
         if (gender != ""){
           self.userInfo.append(gendr)
        }
         if (birthDay != "" && birthDay != "00-00-0000" && birthDay != "0000-00-00"){
            self.userInfo.append(birth)
        }
         if (working != ""){
            self.userInfo.append(works)
        }
         if (living != ""){
            self.userInfo.append(live)
        }
         if (school != ""){
            self.userInfo.append(schol)
        }

        self.getUserData(userId: self.user_id ?? "" , access_token: "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)")
        if ControlSettings.shouldShowAddMobBanner{
                          
                          bannerView = GADBannerView(adSize: kGADAdSizeBanner)
                          addBannerViewToView(bannerView)
                          bannerView.adUnitID = ControlSettings.addUnitId
                          bannerView.rootViewController = self
                          bannerView.load(GADRequest())
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
         func addBannerViewToView(_ bannerView: GADBannerView) {
             bannerView.translatesAutoresizingMaskIntoConstraints = false
             view.addSubview(bannerView)
             view.addConstraints(
                 [NSLayoutConstraint(item: bannerView,
                                     attribute: .bottom,
                                     relatedBy: .equal,
                                     toItem: bottomLayoutGuide,
                                     attribute: .top,
                                     multiplier: 1,
                                     constant: 0),
                  NSLayoutConstraint(item: bannerView,
                                     attribute: .centerX,
                                     relatedBy: .equal,
                                     toItem: view,
                                     attribute: .centerX,
                                     multiplier: 1,
                                     constant: 0)
                 ])
         }
    override func viewWillAppear(_ animated: Bool) {
//         Giphy.configure(apiKey: "4ZD626dqZCchkf4GhiXlHEtTP496ypwp")
        AppInstance.instance.vc = "userProfile"
        self.navigationController?.navigationBar.isHidden = true
    }
    override func viewWillDisappear(_ animated: Bool) {
        self.navigationController?.navigationBar.isHidden = false
    }
    
    
    /////////////////////////NetWork Connection//////////////////////////
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
            
        }
        
    }
    
    private func  getUserPostsData (access_token : String, limit : Int, offset : String, ids : String) {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            performUIUpdatesOnMain {
                Get_Users_Posts_DataManager.sharedInstance.get_User_PostsData(access_token: access_token, limit: limit, id: ids, off_set: offset) { [weak self] (success, authError, error) in
                    if (success != nil) {
                        for i in success!.data {
                            self?.postsArray.append(i)
                        }
                        self?.off_set = self?.postsArray.last?["post_id"] as? String ?? "0"
                        self?.spinner.stopAnimating()
                        self?.tableView.reloadData()
                        ZKProgressHUD.dismiss()
                    }
                    else if (authError != nil) {
                        ZKProgressHUD.dismiss()
                        self?.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        ZKProgressHUD.dismiss()
                        print("InternalError")
                        
                    }
                    
                }
            }
        }
    }
    
    private func getUserData (userId : String, access_token : String) {
        switch status {
        case .unknown, .offline:
            self.view.makeToast("InternetConnectionFialed")
        case .online(.wwan), .online(.wiFi):
            performUIUpdatesOnMain {
                Get_User_DataManagers.sharedInstance.get_User_Data(userId: userId, access_token : access_token) {[weak self] (success, authError, error) in
                    if success != nil {
                        for i in success!.followers{
                            self?.followersArray.append(i)
                        }
                        for j in success!.joined_groups{
                            self?.groupArray.append(j)
                        }
                        for k in success!.liked_pages{
                            self?.pageArray.append(k)
                        }
                        for l in success!.following{
                            self?.followingArray.append(l)
                        }
                        self?.userData = success?.user_data
                        print(self?.userData)
                        self?.tableView.reloadData()
                        self?.getImages(user_Id: self?.user_id ?? "")
                    }
                        
                    else if (authError != nil) {
                        ZKProgressHUD.dismiss()
                        self?.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        ZKProgressHUD.dismiss()
                        print("InternalError")
                        self?.view.makeToast(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    private func getImages (user_Id : String) {
        switch status {
        case .unknown, .offline:
            self.view.makeToast("InternetConnection Failed")
        case .online(.wwan), .online(.wiFi):
            performUIUpdatesOnMain {
                Get_User_ImageManager.sharedInstance.getUserImages(user_id: user_Id, param: "photos") {[weak self] (success, authError, error) in
                    if success != nil {
                        for i in success!.data {
                            self?.imagesArray.append(i)
                        }
                        self?.tableView.reloadData()
                        self?.getUserPostsData(access_token: "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)", limit:10, offset: self?.off_set ?? "", ids: self?.user_id ?? "")
                    }
                        
                    else if authError != nil {
                        self?.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        print("InternalError")
                    }
                }
            }
        }
    }
    
    private func sendRequest(user_id: String){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            performUIUpdatesOnMain {
                Follow_RequestManager.sharedInstance.sendFollowRequest(userId: user_id) { (success, authError, error) in
                    if success != nil {
                        self.view.makeToast(success?.follow_status)
                        print(success?.follow_status)
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
    func profileMore(tag: Int) {
        var profileUrl: String? = nil
        if let url = self.userData?["url"] as? String{
            profileUrl = url
        }
        
        if tag == 1{
            UIPasteboard.general.string = profileUrl ?? ""
            self.view.makeToast(NSLocalizedString("Copied", comment: "Copied"))
        }
        else if tag == 2{
            let textToShare = [ profileUrl ]
            let activityViewController = UIActivityViewController(activityItems: textToShare, applicationActivities: nil)
            activityViewController.popoverPresentationController?.sourceView = self.view
            activityViewController.excludedActivityTypes = [ UIActivity.ActivityType.airDrop, UIActivity.ActivityType.postToFacebook, UIActivity.ActivityType.assignToContact,UIActivity.ActivityType.mail,UIActivity.ActivityType.postToTwitter,UIActivity.ActivityType.message,UIActivity.ActivityType.postToFlickr,UIActivity.ActivityType.postToVimeo,UIActivity.ActivityType.init(rawValue: "net.whatsapp.WhatsApp.ShareExtension"),UIActivity.ActivityType.init(rawValue: "com.google.Gmail.ShareExtension"),UIActivity.ActivityType.init(rawValue: "com.toyopagroup.picaboo.share"),UIActivity.ActivityType.init(rawValue: "com.tinyspeck.chatlyio.share")]
            self.present(activityViewController, animated: true, completion: nil)
        }
        else if tag == 3{
            print("Poke")
            switch status {
            case .unknown, .offline:
                self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
            case .online(.wwan),.online(.wiFi):
                CreatePokeManager.sharedInstance.createPokes(user_Id: self.userData?["user_id"] as! String) { (success, authError, error) in
                    if success != nil {
                        self.view.makeToast("Poked")
                    }
                    else if authError != nil  {
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        print(error?.localizedDescription)
                    }
                    
                }
            }
        }
        else if tag == 4{
            print("Add to Family")
        }
        else if tag == 5{
            let storyboard = UIStoryboard(name: "AddPost", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "SelectGIFVC") as! SelectGIFVC
            vc.delegate = self
            self.present(vc, animated: true, completion: nil)
           
        }
    }
    
    func didSelectGIF(GIFUrl: String,id: String) {
        print(GIFUrl)
        print(id)
        SendGiftManager.sharedInstance.sendGiftManager(user_id:self.user_id ?? "" , id: "1" ?? "1") { (success, authError, error) in
            if success != nil{
                self.view.makeToast("Done")
            }
            else if (authError != nil){
                self.view.makeToast(authError?.errors.errorText)
            }
            else if (error != nil){
                self.view.makeToast(error?.localizedDescription)
            }
        }
    }
}
extension GetUserDataController : UITableViewDelegate,UITableViewDataSource,JoinGroupDelegate{
    
    func numberOfSections(in tableView: UITableView) -> Int {
        return  7
    }
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if section == 0 {
            return 1
        }
        else if section == 1{
            return self.userInfo.count
        }
    
        else if section == 2 {
            if (self.imagesArray.count == 0) {
                return 0
            }
            else {
                return 1
            }
        }
        else if section == 3 {
            if (self.followersArray.count == 0) {
                return 0
            }
            else {
                return 1
            }
        }
        else if section == 4 {
            if (self.groupArray.count == 0) {
                return 0
            }
            else {
                return 1
            }
        }
        else if section == 5 {
            if self.pageArray.count == 0 {
                return 0
            }
            else {
                return 1
            }
        }
            
        else {
            return  self.postsArray.count
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        if (indexPath.section == 0) {
            var isPro: String? = nil
            var isVerified: String? = nil
            var userName: String? = nil
            let cell = tableView.dequeueReusableCell(withIdentifier: "Cover") as! UserCoverView
            self.tableView.rowHeight = 320.0
            if let name = self.userData?["name"] as? String{
                userName = name
            }
            if let image = self.userData?["avatar"] as? String{
                print(image)
                let url = URL(string: image)
                self.profileImageURL = image
                cell.profileImage.kf.setImage(with: url)
            }
            if let cover = self.userData?["cover"] as? String{
                print(cover)
                let url = URL(string: cover)
                self.coverImageURL = cover
                cell.coverImage.kf.setImage(with: url)
            }
            if let about = self.userData?["about"] as? String{
                cell.aboutText.text = about
            }
            if let details = self.userData?["details"] as? [String:Any]{
                if let followerCount = details["followers_count"] as? String{
                    cell.followersBtn.setTitle(followerCount, for: .normal)
                }
                if let followingCount = details["following_count"] as? String{
                    cell.followingBtn.setTitle(followingCount, for: .normal
                    )
                }
                if let likesCount = details["likes_count"] as? String{
                    cell.pageLikesBtn.setTitle(likesCount, for: .normal)
                }
            }
            if let points = self.userData?["points"] as? String{
                cell.pointsBtn.setTitle(points, for: .normal)
            }
            if let is_Pro = self.userData?["is_pro"] as? String{
                isPro = is_Pro
            }
            if let is_Verified = self.userData?["verified"] as? String{
                isVerified = is_Verified
            }
            let imageAttachment =  NSTextAttachment()
            let imageAttachment1 =  NSTextAttachment()
            imageAttachment.image = UIImage(named:"veirfied")
            imageAttachment1.image = UIImage(named: "pros")
            let imageOffsetY: CGFloat = -2.0
        imageAttachment.bounds = CGRect(x: 0, y: imageOffsetY, width: imageAttachment.image!.size.width, height: imageAttachment.image!.size.height)
        imageAttachment1.bounds = CGRect(x: 0, y: imageOffsetY, width: imageAttachment1.image!.size.width, height: imageAttachment1.image!.size.height)
            
            let attechmentString = NSAttributedString(attachment: imageAttachment)
            let attechmentString1 = NSAttributedString(attachment: imageAttachment1)
            let attrs1 = [NSAttributedString.Key.foregroundColor : UIColor.white]
            let attrs2 = [NSAttributedString.Key.foregroundColor : UIColor.white]
            if isPro == "1"{
            let attributedString1 = NSMutableAttributedString(string: "\(userName ?? "")\("  ")", attributes:attrs1)
             let attributedString2 = NSMutableAttributedString(attributedString: attechmentString)
            let attributedString3 = NSMutableAttributedString(string: " ", attributes:attrs2)
             let attributedString4 = NSMutableAttributedString(attributedString: attechmentString1)
            attributedString1.append(attributedString2)
            attributedString1.append(attributedString3)
            attributedString1.append(attributedString4)
                cell.profileNAme.attributedText = attributedString1
            }
            else{
                cell.profileNAme.text = userName
            }

            if let isFollwoing = self.userData?["is_following"] as? Int{
                if isFollwoing == 0{
                    cell.addButton.setImage(#imageLiteral(resourceName: "Shape"), for: .normal)
                    cell.addButton.backgroundColor = UIColor.hexStringToUIColor(hex: "#6665FC")
                }
                else{
                    cell.addButton.setImage(#imageLiteral(resourceName: "check"), for: .normal)
                    cell.addButton.backgroundColor = .white
                }
            }
            let gesture = UITapGestureRecognizer(target: self, action: #selector(self.showProfileImage(gesture:)))
            let gesture1 = UITapGestureRecognizer(target: self, action: #selector(self.showCoverImage(gesture:)))
            cell.addButton.tag = indexPath.row
            cell.followingBtn.tag = indexPath.row
            cell.followersBtn.tag = indexPath.row
            cell.pageLikesBtn.tag = indexPath.row
            cell.pointsBtn.tag = indexPath.row
            cell.profileImage.addGestureRecognizer(gesture)
            cell.profileImage.isUserInteractionEnabled = true
            cell.coverImage.addGestureRecognizer(gesture1)
            cell.coverImage.isUserInteractionEnabled = true
            cell.backButton.addTarget(self, action: #selector(self.popViewController), for: .touchUpInside)
            cell.moreButton.addTarget(self, action: #selector(self.gotoMore), for: .touchUpInside)
            cell.addButton.addTarget(self, action: #selector(self.sendfollowRequest(sender:)), for: .touchUpInside)
            cell.messageButton.addTarget(self, action: #selector(self.gotoMassengerApp(sender:)), for: .touchUpInside)
            cell.followingBtn.addTarget(self, action: #selector(self.GotoFollowingController(sender:)), for: .touchUpInside)
            cell.followersBtn.addTarget(self, action: #selector(self.GotoFollowerController(sender:)), for: .touchUpInside)
            cell.pageLikesBtn.addTarget(self, action: #selector(self.GotoPageController(sender:)), for: .touchUpInside)
             cell.messageButton.addTarget(self, action: #selector(self.messageButton(sender:)), for: .touchUpInside)
            return cell
        }
        else if (indexPath.section == 1){
            let cell = tableView.dequeueReusableCell(withIdentifier: "USerInfoCells") as! UserInfoCell
            cell.selectionStyle = .none
             tableView.rowHeight = 40.0
            let index = self.userInfo[indexPath.row]
            if let lastSenn = index["lastseen_time_text"] as? String{
                cell.imageSet.image = #imageLiteral(resourceName: "clockBlack")
                cell.infoLabel.text = lastSenn
            }
            if let website = index["website"] as? String{
                cell.imageSet.image = UIImage(named: "internet")
                cell.infoLabel.text = website
            }
            if let gender = index["gender_text"] as? String{
                cell.imageSet.image = UIImage(named: "genderi")
                cell.infoLabel.text = gender
            }
            if let birth = index["birthday"] as? String{
                cell.imageSet.image = #imageLiteral(resourceName: "birthday-cake")
                cell.infoLabel.text = birth
            }
            if let work = index["working"] as? String{
                cell.imageSet.image = UIImage(named: "portfolioess")
                cell.infoLabel.text = "\("Working at ")\(work)"
            }
            if let living = index["address"] as? String{
                cell.imageSet.image = UIImage(named: "home")
                cell.infoLabel.text = "\("Living in ")\(living)"
            }
            if let school = index["school"] as? String{
                cell.imageSet.image = UIImage(named: "high-school")
                cell.infoLabel.text = "\("Studying at ")\(school)"
            }
            
            return cell
        }
        else if (indexPath.section == 2) {
            let cell = tableView.dequeueReusableCell(withIdentifier: "ProfileCell") as! GetUserPofile
            tableView.rowHeight = 173.0
            cell.profileArray = self.imagesArray
            cell.didSelectItemAction = {[weak self] indexPath in
                self?.gotoImageVC(indexPath: indexPath)
            }
            cell.profileCollectionView.reloadData()
            return cell
        }
            
        else if (indexPath.section == 3) {
            let cell = tableView.dequeueReusableCell(withIdentifier: "FollowerCell") as! GetUserFollowers
            tableView.rowHeight = 108.0
            let index = self.followersArray[indexPath.row]
            cell.followersArray = self.followersArray
            cell.didSelectItemAction = {[weak self] indexPath in
                self?.gotoFollowerProfile(indexPath: indexPath)
            }
            cell.followersCollectionView.reloadData()
            return cell
        }
            
        else if (indexPath.section == 4) {
            let cell = tableView.dequeueReusableCell(withIdentifier: "Group") as! GetUserGroup
            tableView.rowHeight = 163.0
            cell.groupArray = self.groupArray
            cell.didSelectItemAction = {[weak self] indexPath in
                self?.gotoGroupVC(indexPath: indexPath)
            }
            cell.groupCollectionView.reloadData()
            
            return cell
        }
        else if (indexPath.section == 5)  {
            let cell = tableView.dequeueReusableCell(withIdentifier: "PageCell") as! GetUserLikePages
            tableView.rowHeight = 68.0
            for i in pageArray{
                if let image = i["avatar"] as? String{
                    let url = URL(string: image)
                    cell.pageiconImage.kf.setImage(with: url)
                    break
                }
            }
            return cell
        }
            
        else {
            var cell = UITableViewCell()
            let index = self.postsArray[indexPath.row]
            var shared_info : [String:Any]? = nil
            var fundDonation: [String:Any]? = nil
            
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
                    cell = GetPostVideo.sharedInstance.getVideo(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.postsArray, url: url!, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
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
    }
    
    
    @IBAction func showProfileImage (gesture: UIGestureRecognizer){
        let storyboard = UIStoryboard(name: "Main", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "ShowImageVC") as! ShowImageController
        vc.imageUrl = self.profileImageURL
        vc.posts.append(self.userData!)
         vc.modalPresentationStyle = .overFullScreen
         vc.modalTransitionStyle = .coverVertical
         self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func showCoverImage (gesture: UIGestureRecognizer){
        let storyboard = UIStoryboard(name: "Main", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "ShowImageVC") as! ShowImageController
         vc.imageUrl = self.coverImageURL
         vc.posts.append(self.userData!)
         vc.modalPresentationStyle = .overFullScreen
         vc.modalTransitionStyle = .coverVertical
         self.present(vc, animated: true, completion: nil)
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
        if indexPath.section == 2 {
            let storyBoard = UIStoryboard(name: "Main", bundle: nil)
            let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
            self.navigationController?.pushViewController(vc, animated: true)
        }
            
        else if indexPath.section == 4{
            
        }
        else if indexPath.section == 4 {
            let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "PageListVC") as! PageListsController
            vc.likePageLists = self.pageArray
            vc.pageLabeltxt = "Pages"
            vc.btnHidden = true
            vc.isOwner = false
            self.navigationController?.pushViewController(vc, animated: true)
        }
    }
    func tableView(_ tableView: UITableView, willDisplay cell: UITableViewCell, forRowAt indexPath: IndexPath) {
        let count = self.postsArray.count
        let lastElement = count - 1
    }
    
    func gotoFollowerProfile(indexPath : IndexPath) {
        let index = self.followersArray[indexPath.row]
        let storyBoard = UIStoryboard(name: "Main", bundle: nil)
        let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
        if let follower_profile = self.followersArray[indexPath.row] as? [String:Any]{
            vc.userData = follower_profile
        }
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
    func gotoImageVC(indexPath:IndexPath){
        let index = self.imagesArray[indexPath.row]
        let storyboard = UIStoryboard(name: "Main", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "ShowImageVC") as! ShowImageController
        vc.modalPresentationStyle = .overFullScreen
        vc.modalTransitionStyle = .coverVertical
        if let imageUrl = index["postFile_full"] as? String{
            vc.imageUrl = imageUrl
        }
        vc.posts.append(index)
        self.present(vc, animated: true, completion: nil)
    }
    
    func gotoGroupVC (indexPath : IndexPath) {
        let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "GroupVC") as! GroupController
        let index = self.groupArray[indexPath.row]
        if let groupId = index["group_id"] as? String{
            vc.groupId = groupId
        }
        if let groupName = index["group_name"] as? String{
            vc.groupName = groupName
        }
        if let groupTitle = index["group_title"] as? String{
            vc.groupTitle = groupTitle
        }
        if let groupIcon = index["avatar"] as? String{
            vc.groupIcon = groupIcon
        }
        if let groupCover = index["cover"] as? String{
            vc.groupCover = groupCover
        }
        if let groupcategory = index["category"] as? String{
            vc.category = groupcategory
        }
        if let isJoined = index["is_joined"] as? Bool{
            vc.isJoined = isJoined
        }
        vc.groupData = index
        vc.delegte1 = self
        self.navigationController?.pushViewController(vc, animated: true)
        
    }
    func block() {
        self.navigationController?.popViewController(animated: true)
        //        self.delegate?.filterBlockUser(userId: self.userProfileData.user_id)
    }
    func joinGroup(isJoin: Bool) {
        print("")
    }
    
    @objc func sendfollowRequest(sender:UIButton){
        let cell = self.tableView.cellForRow(at: IndexPath(row: sender.tag, section: 0)) as! UserCoverView
        var user_id: String? = nil
        if let userId = self.userData?["user_id"] as? String{
            user_id = userId
        }
        if let is_following = self.userData?["is_following"] as? Int{
            if is_following == 0{
                cell.addButton.setImage(#imageLiteral(resourceName: "check"), for: .normal)
                cell.addButton.backgroundColor = .white
                self.sendRequest(user_id: user_id ?? "")
                self.userData?["is_following"] = 1
            }
            else {
                cell.addButton.setImage(#imageLiteral(resourceName: "Shape"), for: .normal)
                cell.addButton.backgroundColor = UIColor.hexStringToUIColor(hex: "#6665FC")
                self.sendRequest(user_id: user_id ?? "")
                self.userData?["is_following"] = 0
                
            }
        }
    }
    @objc func popViewController() {
        self.navigationController?.popViewController(animated: true)
    }
    @objc func gotoMore() {
        let storyBoard = UIStoryboard(name: "Main", bundle: nil)
        let vc = storyBoard.instantiateViewController(withIdentifier: "GotoMore") as! MoreViewController
        vc.modalTransitionStyle = .crossDissolve
        vc.modalPresentationStyle = .overCurrentContext
        vc.delegate = self
        vc.delegate1 = self
        if let user_Id = self.userData?["user_id"] as? String{
            vc.userId = user_Id
        }
        self.present(vc, animated: true, completion: nil)
    }
    
    @objc func gotoMassengerApp(sender: UIButton) {
        UIApplication.shared.openURL(NSURL(string: "itms://itunes.apple.com/de/app/loungemates-messenger/id1475911448")! as URL)
    }
    
    @objc func GotoFollowingController(sender: UIButton){
        if let details = self.userData?["details"] as? [String:Any]{
            if let followingCount = details["following_count"] as? String{
                if followingCount != "0"{
                    let storyBoard = UIStoryboard(name: "MoreSection", bundle: nil)
                    let vc = storyBoard.instantiateViewController(withIdentifier: "FollowingVC") as! FollowingController
                    if let user_Id = self.userData?["user_id"] as? String{
                        vc.userId = user_Id
                    }
                    vc.type = "following"
                    vc.navTitle = NSLocalizedString("Following", comment: "Following")
                    self.navigationController?.pushViewController(vc, animated: true)
                }
            }
        }
    }
    @objc func GotoFollowerController(sender: UIButton){
        if let details = self.userData?["details"] as? [String:Any]{
            if let followersCount = details["followers_count"] as? String{
                if followersCount != "0"{
                    let storyBoard = UIStoryboard(name: "MoreSection", bundle: nil)
                    let vc = storyBoard.instantiateViewController(withIdentifier: "FollowingVC") as! FollowingController
                    if let user_Id = self.userData?["user_id"] as? String{
                        vc.userId = user_Id
                    }
                    vc.type = "followers"
                    vc.navTitle = NSLocalizedString("Followers", comment: "Followers")
                    self.navigationController?.pushViewController(vc, animated: true)
                }
            }
        }
    }
    
    @objc func GotoPageController(sender: UIButton){
        if let details = self.userData?["details"] as? [String:Any]{
            if let pageLike = details["likes_count"] as? String{
                if pageLike != "0"{
                    let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "PageListVC") as! PageListsController
                    if let user_Id = self.userData?["user_id"] as? String{
                        vc.user_id = user_Id
                    }
                    vc.pageLabeltxt = "Pages"
                    vc.btnHidden = true
                    vc.isOwner = false
                    self.navigationController?.pushViewController(vc, animated: true)
                }
            }
            
        }
    }
    @objc func messageButton(sender: UIButton){
         let appURLScheme = "AppToOpen://"
         guard let appURL = URL(string: appURLScheme) else {
             return
         }

         if UIApplication.shared.canOpenURL(appURL) {

             if #available(iOS 10.0, *) {
                 UIApplication.shared.open(appURL)
             }
             else {
                 UIApplication.shared.openURL(appURL)
             }
         }
         else {
             self.view.makeToast("Please install WoWonder Messenger App")
         }
       }
    
}


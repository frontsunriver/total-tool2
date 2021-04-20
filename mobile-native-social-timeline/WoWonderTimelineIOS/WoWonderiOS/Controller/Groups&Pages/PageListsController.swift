

import UIKit
import WoWonderTimelineSDK
import GoogleMobileAds

class PageListsController: UIViewController,EditPageDelegete,DeletePageDelegate,PageLikeDelegate{

    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    let status = Reach().connectionStatus()
    var likePageLists = [[String:Any]]()
    var myPages = [[String:Any]]()
    var pageLabeltxt = NSLocalizedString("Explore Pages", comment: "Explore Pages")
    var btnHidden = false
    var offset = "0"
    var isOwner =  false
    var editingIndex = 0
    var selectedIndex: Int? = nil
    var user_id: String? = nil
    var interstitial: GADInterstitial!


    var pageCover = "",pageIcon = "",pageTitle = "",pageName = "",rating = 0.0,category = "",categoryId = "",company = "",about = "",phone = "",address = "",website = "",pagelikes = "",callActionType = "",callActionUrl = "",pageId = "",facebook = "",twitter = "",instagram = "",linkdin = "",youtube = "",vk = "",pageUrl = "",isLike = false
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = self.pageLabeltxt
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        
        self.tableView.register(UINib(nibName: "LikePagesCell", bundle: nil), forCellReuseIdentifier: "LikePage")
        self.tableView.reloadData()
        self.tableView.tableFooterView = UIView()
        self.activityIndicator.startAnimating()
        if (self.user_id == UserData.getUSER_ID()!) {
       self.navigationItem.rightBarButtonItem = UIBarButtonItem(title: NSLocalizedString("Create", comment: "Create"), style: .done, target: self, action: #selector(self.CreatePage(sender:)))
        }
        self.getLikedPages(user_id: self.user_id ?? "")
        if self.isOwner == true{
            self.getMyPages()
        }
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
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    
    
    @IBAction func Back(_ sender: Any) {
        self.navigationController?.popViewController(animated: true)
    }
    
    private func likePage(pageId :Int){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                LikePageManager.sharedInstance.likePage(pageId:pageId) { (success, authError, error) in
                    if success != nil {
                        self.view.makeToast(success?.like_status)
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
    

    private func getLikedPages(user_id: String){
        switch status {
        case .unknown, .offline:
            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            DispatchQueue.main.async {
                Get_User_DataManagers.sharedInstance.get_User_Data(userId: user_id, access_token: "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)") { [weak self] (success, authError, error) in
                    if success != nil {
                        self?.likePageLists = (success!.liked_pages.map({$0}))
                        self?.tableView.reloadData()
                        self?.activityIndicator.stopAnimating()
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
    
    private func getMyPages() {
        switch status {
        case .unknown, .offline:
            print("Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetMyPagesManager.sharedInstance.getLikedPages(userId: UserData.getUSER_ID()!, offset: "0") { (success, authError, error) in
                    if success != nil {
                        for i in success!.data{
                            self.myPages.append(i)
                        }
                        self.tableView.reloadData()
                        if let off_set = self.likePageLists.last?["page_id"] as?String{
                            print("offset")
                        }
//                        self.activityIndicator.stopAnimating()
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
        }
    }
    
    
    @IBAction func CreatePage(sender: UIButton) {
        let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "CreatePageVC") as! CreatePageController
        vc.modalPresentationStyle = .fullScreen
        vc.modalTransitionStyle = .coverVertical
        vc.delegate = self
        self.present(vc, animated: true, completion: nil)
    }
    
    func pageLiked(isLike: Bool) {
        if isLike == true{
            self.likePageLists[self.selectedIndex ?? 0]["is_liked"] = true
        }
        else{
            self.likePageLists[self.selectedIndex ?? 0]["is_liked"] = false
        }
        self.tableView.reloadData()
    }
    
}

extension PageListsController : UITableViewDelegate,UITableViewDataSource,CreatePageDelegate{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if section == 0 {
            if self.isOwner == false {
                return 0
            }
            else {
                return 1
            }
        }
        else {
            return self.likePageLists.count
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        if indexPath.section == 0 {
            let cell = tableView.dequeueReusableCell(withIdentifier: "MyPages") as! MyPagesCell
            cell.myPagesArray = self.myPages
            cell.collectionView.reloadData()
            cell.didSelectItemAction  = {[weak self] indexPath in
                self?.gotoPageController(indexPath: indexPath)
            }
            return cell
        }
        else {
            
            let cell = tableView.dequeueReusableCell(withIdentifier: "LikePage") as! LikePagesCell
            let index = likePageLists[indexPath.row]
            if let image = index["avatar"] as? String{
                let url = URL(string: image)
                cell.pageicon.kf.setImage(with: url)
            }
            if let pageName = index["page_name"] as? String{
                cell.pageName.text = pageName
            }
            if let pageCategory = index["category"] as? String{
                cell.pageCategory.text = pageCategory
            }
            if let isLike = index["is_liked"] as? Bool{
                if isLike == true{
                    cell.likeBtn.setTitle(NSLocalizedString("Unlike", comment: "Unlike"), for: .normal)
                    cell.likeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                    cell.likeBtn.setTitleColor(.white, for: .normal)
                }
                else {
                    cell.likeBtn.setTitle(NSLocalizedString("Like", comment: "Like"), for: .normal)
                    cell.likeBtn.backgroundColor = .white
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                }
            }
            cell.likeBtn.tag = indexPath.row
            cell.likeBtn.addTarget(self, action: #selector(self.pageLike(sender:)), for: .touchUpInside)
            return cell
        }
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        if indexPath.section == 0 {
            return 100.0
        }
        else {
            return 75.0
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
        let index = self.likePageLists[indexPath.row]
        self.selectedIndex = indexPath.row
        let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "PageVC") as! PageController
        if let page_Cover = index["cover"] as? String{
            pageCover = page_Cover
        }
        if let page_Icon = index ["avatar"] as? String {
            pageIcon = page_Icon
        }
        if let page_Description = index["page_description"] as? String{
            about = page_Description
        }
        if let page_Title = index["page_title"] as? String {
            pageTitle = page_Title
        }
        
        if let page_Name = index["page_name"] as? String{
            pageName = page_Name
        }
        
        if let pagecategory = index["category"] as? String{
            category = pagecategory
        }
        
        if let pageRating = index["rating"] as? Double{
            rating = pageRating
        }
        
        if let callAction_Type = index["call_action_type"] as? String{
            callActionType = callAction_Type
        }
        
        if let callAction_Url = index["call_action_type_url"] as? String{
            callActionUrl = callAction_Url
            
        }
        if let page_Id = index["page_id"] as? String{
            pageId = page_Id
            vc.page_id = pageId
        }
        if let category_Id = index["page_category"] as? String{
            categoryId = category_Id
        }
        if let company_name = index["company"] as? String{
            company = company_name
        }
        if let phone_num = index["phone"] as? String{
            phone = phone_num
        }
        if let location = index["address"] as? String{
            address = location
        }
        if let website_link = index["website"] as? String{
            website = website_link
        }
        
        if let facebook_link = index["facebook"] as? String{
            facebook = facebook_link
        }
        if let youtube_link = index["youtube"] as? String{
            youtube = youtube_link
        }
        if let instagrm_link = index["instgram"] as? String{
            instagram = instagrm_link
        }
        if let linkdin_link = index["linkedin"] as? String{
            linkdin = linkdin_link
        }
        if let vk_link = index["vk"] as? String{
            vk = vk_link
        }
        if let twitter_link = index["twitter"] as? String{
            twitter = twitter_link
        }
        if let page_Url = index["url"] as? String{
            pageUrl = page_Url
        }
        if let is_Like = index["is_liked"] as? Bool{
            isLike = is_Like
        }
        
        let pageData = ForwardPageData(page_Name: pageName, page_Title: pageTitle, cateforyId: categoryId, categoryName: category, callActionType: callActionType, callActionUrl: callActionUrl, company: company, about: about, phone: phone, address: address, website: website, pageId: pageId, facebook: facebook, twitter: twitter, instagrm: instagram, linkdin: linkdin, youtube: youtube, vk: vk, pageCover: pageCover, pageIcon: pageIcon, rating: rating, pageurl: pageUrl, isLike: isLike)
        
        vc.pageData = pageData
        vc.isPageOwner = false
        vc.likeDelegate = self
        vc.isFromList = true
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
    
    @IBAction func pageLike(sender : UIButton){
        let cell = tableView!.cellForRow(at: IndexPath(row: sender.tag, section: 1)) as! LikePagesCell
        let index = self.likePageLists[sender.tag]
        var page_id: String? = nil
        if let pageId = index["page_id"] as? String {
            page_id = pageId
        }
        if let isLike = index["is_liked"] as? Bool{
            if isLike == true{
                cell.likeBtn.setTitle(NSLocalizedString("Like", comment: "Like"), for: .normal)
                cell.likeBtn.backgroundColor = .white
                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                self.likePage(pageId: Int(page_id ?? "") ?? 0)
                self.likePageLists[sender.tag]["is_liked"]  = false
            }
            else {
                cell.likeBtn.setTitle(NSLocalizedString("Unlike", comment: "Unlike"), for: .normal)
                cell.likeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                cell.likeBtn.setTitleColor(.white, for: .normal)
                self.likePage(pageId: Int(page_id ?? "") ?? 0)
                self.likePageLists[sender.tag]["is_liked"]  = true
            }
        }
    }
    
    //
    
    func tableView(_ tableView: UITableView, viewForHeaderInSection section: Int) -> UIView? {
        if section == 0 {
            
            let headerView = UIView.init(frame: CGRect.init(x: 0, y: 0, width: tableView.frame.width, height: 50))
            
            let label = UILabel()
            label.frame = CGRect.init(x: 5, y: 5, width: headerView.frame.width-10, height: headerView.frame.height-10)
            label.text = "\("  ")\(NSLocalizedString("Manage Page", comment: "Manage Page"))"
            label.textColor = .black
            label.backgroundColor = UIColor.hexStringToUIColor(hex: "#E4E6E8")
            headerView.addSubview(label)
            return headerView
        }
        else {
         
            let headerView = UIView.init(frame: CGRect.init(x: 0, y: 0, width: tableView.frame.width, height: 50))
            let label = UILabel()
            label.frame = CGRect.init(x: 5, y: 5, width: headerView.frame.width-10, height: headerView.frame.height-10)
            label.text = "\("  ")\(NSLocalizedString("Liked Pages", comment: "Liked Pages"))"
            label.textColor = .black
            label.backgroundColor = UIColor.hexStringToUIColor(hex: "#E4E6E8")
            headerView.addSubview(label)
            return headerView
        }
    }
    func tableView(_ tableView: UITableView, heightForHeaderInSection section: Int) -> CGFloat {
        if section == 0{
            if self.isOwner == false {
                return 0
            }
            else {
                if self.myPages.isEmpty == true{
                    return 0
                }
                else {
                    return 50
                }
            }
        }
        else {
            if self.isOwner == false {
                return 0
            }
            else {
                if self.likePageLists.isEmpty == true{
                    return 0
                }
                else {
                   return 50
                }
            }
        }
    }
    
    func numberOfSections(in tableView: UITableView) -> Int {
        return 2
    }
    
    func sendPageData(pageData: [String : Any]) {
        self.myPages.append(pageData)
        //        insert(pageData, at: 0)
        self.tableView.reloadData()
    }
    
    func gotoPageController (indexPath : IndexPath) {
        let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "PageVC") as! PageController
        let index = self.myPages[indexPath.row]
        self.editingIndex = indexPath.row
        vc.isPageOwner = true
        if let page_Cover = index["cover"] as? String{
            pageCover = page_Cover
        }
        if let page_Icon = index ["avatar"] as? String {
            pageIcon = page_Icon
        }
        if let page_Description = index["page_description"] as? String{
            about = page_Description
        }
        if let page_Title = index["page_title"] as? String {
            pageTitle = page_Title
        }
        
        if let page_Name = index["page_name"] as? String{
            pageName = page_Name
        }
        
        if let pagecategory = index["category"] as? String{
            category = pagecategory
        }
        
        if let pageRating = index["rating"] as? Double{
            rating = pageRating
        }
        
        if let callAction_Type = index["call_action_type"] as? String{
            callActionType = callAction_Type
        }
        
        if let callAction_Url = index["call_action_type_url"] as? String{
            callActionUrl = callAction_Url
            
        }
        if let page_Id = index["page_id"] as? String{
            pageId = page_Id
            vc.page_id = page_Id
        }
        if let category_Id = index["page_category"] as? String{
            categoryId = category_Id
        }
        if let company_name = index["company"] as? String{
            company = company_name
        }
        if let phone_num = index["phone"] as? String{
            phone = phone_num
        }
        if let location = index["address"] as? String{
            address = location
        }
        if let website_link = index["website"] as? String{
            website = website_link
        }
        
        if let facebook_link = index["facebook"] as? String{
            facebook = facebook_link
        }
        if let youtube_link = index["youtube"] as? String{
            youtube = youtube_link
        }
        if let instagrm_link = index["instgram"] as? String{
            instagram = instagrm_link
        }
        if let linkdin_link = index["linkedin"] as? String{
            linkdin = linkdin_link
        }
        if let vk_link = index["vk"] as? String{
            vk = vk_link
        }
        if let twitter_link = index["twitter"] as? String{
            twitter = twitter_link
        }
        if let page_Url = index["url"] as? String{
            pageUrl = page_Url
        }
        
        let pageData = ForwardPageData(page_Name: pageName, page_Title: pageTitle, cateforyId: categoryId, categoryName: category, callActionType: callActionType, callActionUrl: callActionUrl, company: company, about: about, phone: phone, address: address, website: website, pageId: pageId, facebook: facebook, twitter: twitter, instagrm: instagram, linkdin: linkdin, youtube: youtube, vk: vk, pageCover: pageCover, pageIcon: pageIcon, rating: rating, pageurl: pageUrl, isLike: false)
        
        vc.pageData = pageData
        vc.delegate = self
        vc.deleteDelegate = self
        vc.isPageOwner = true
        vc.isFromList = true
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
    func editPage(pageData: [String:Any]) {
        self.myPages[self.editingIndex] = pageData
        
        //        self.myPages[self.editingIndex]["page_id"]  = pageData.pageId
//        self.myPages[self.editingIndex]["page_name"] = pageData.page_Name
//        self.myPages[self.editingIndex]["page_title"] = pageData.page_Title
//        self.myPages[self.editingIndex]["cover"] = pageData.pageCover
//        self.myPages[self.editingIndex]["page_description"] = pageData.about
//        self.myPages[self.editingIndex]["category"] = pageData.categoryName
//        self.myPages[self.editingIndex]["page_category"] = pageData.cateforyId
//        self.myPages[self.editingIndex]["call_action_type"] = pageData.callActionType
//        self.myPages[self.editingIndex]["call_action_type_url"] = pageData.callActionUrl
//        self.myPages[self.editingIndex]["company"] = pageData.company
//        self.myPages[self.editingIndex]["phone"] = pageData.phone
//        self.myPages[self.editingIndex]["website"] = pageData.website
//        self.myPages[self.editingIndex]["address"] = pageData.address
//        self.myPages[self.editingIndex]["facebook"] = pageData.facebook
//        self.myPages[self.editingIndex]["twitter"] = pageData.twitter
//        self.myPages[self.editingIndex]["vk"] = pageData.vk
//        self.myPages[self.editingIndex]["youtube"] = pageData.youtube
//        self.myPages[self.editingIndex]["linkedin"] = pageData.linkdin
//        self.myPages[self.editingIndex]["instgram"] = pageData.instagrm
    }
    
    func deletePage(pageId: String) {
        self.myPages = self.myPages.filter({$0["page_id"] as? String != pageId})
        self.tableView.delegate = self
    }
}

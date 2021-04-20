//
//  ViewController.swift
//  News_Feed
//
//  Created by clines329 on 10/19/19.
//  Copyright © 2019 clines329. All rights reserved.
//

import UIKit
import AlamofireImage
import Kingfisher
import SDWebImage
import PaginatedTableView
import Toast_Swift
import ZKProgressHUD
import NVActivityIndicatorView
import WoWonderTimelineSDK
import GoogleMobileAds


struct datas{
    let status : String
    let image : UIImage?
}

class ViewController: UIViewController,FilterBlockUser,UISearchBarDelegate {
    
    func filterBlockUser(userId: String) {
        self.newsFeedArray = self.newsFeedArray.filter({$0["user_id"] as? String != userId})
        self.tableView.reloadData()
    }
    
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    var newsFeedArray = [[String:Any]]()
    var filterFeedArray = [[String:Any]]()
    var isVideo:Bool? = false
    
    
    let spinner = UIActivityIndicatorView(style: .gray)
    let pulltoRefresh = UIRefreshControl()
    var storiesArray = [GetStoriesModel.UserDataElement]()
    let status = Reach().connectionStatus()
    var interstitial: GADInterstitial!
        
    lazy var searchBar:UISearchBar = UISearchBar(frame: CGRect(x: 0.0, y: 0.0, width: 250, height: 40))
    let placeholder = NSAttributedString(string: "Search", attributes: [.foregroundColor: UIColor.white])
    
    var off_set = "0"
    
    override func viewDidLoad() {
        super.viewDidLoad()
        print(UserData.getUSER_ID()!)
        print(UserData.getAccess_Token()!)
        if let textfield = self.searchBar.value(forKey: "searchField") as? UITextField {
            textfield.clearButtonMode = .never
            textfield.textColor = .white
            textfield.backgroundColor = UIColor.clear
            textfield.attributedPlaceholder = NSAttributedString(string: "\(" ")\(NSLocalizedString("Search...", comment: "Search..."))", attributes:[NSAttributedString.Key.foregroundColor: UIColor.white])
           
            if let leftView = textfield.leftView as? UIImageView {
                leftView.image = leftView.image?.withRenderingMode(.alwaysTemplate)
                leftView.tintColor = UIColor.white
            }
        }
        self.searchBar.delegate = self
        self.searchBar.tintColor = .white
        let leftNavBarButton = UIBarButtonItem(customView:searchBar)
        self.navigationItem.leftBarButtonItem = leftNavBarButton
        self.tableView.delegate = self
        self.tableView.dataSource = self
        self.tableView.backgroundColor = .white
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.tableView.addSubview(pulltoRefresh)
        
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(loadList), name: NSNotification.Name(rawValue: "load"), object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(self.Segue(notification:)), name: NSNotification.Name(rawValue: "performSegue"), object: nil)
        Reach().monitorReachabilityChanges()
        self.activityIndicator.startAnimating()
        SetUpcells.setupCells(tableView: self.tableView)
        self.getNewsFeed(access_token: "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)", limit:15, offset: "0")
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
        self.navigationController?.navigationBar.isHidden = false
        self.tableView.reloadData()
        AppInstance.instance.vc = "newsFeedVC"
        NotificationCenter.default.addObserver(self, selector: #selector(self.Segue(notification:)), name: NSNotification.Name(rawValue: "performSegue"), object: nil)
       if AppInstance.instance.commingBackFromAddPost{
        newsFeedArray.removeLast()
        self.storiesArray.removeAll()
         self.tableView.reloadData()
        self.getNewsFeed(access_token: "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)", limit:15, offset: "0")
        self.tableView.reloadData()
        AppInstance.instance.commingBackFromAddPost = false
       }else{
        
        }

    }
    
    override func viewWillDisappear(_ animated: Bool) {
    NotificationCenter.default.removeObserver(self, name: NSNotification.Name(rawValue: "performSegue"), object: nil)
    }
    
    @IBAction func addStoriesPressed(_ sender: Any) {
        self.showStoriesLog()
    }
    func showStoriesLog(){
        let alert = UIAlertController(title: NSLocalizedString("source", comment: "source"), message: NSLocalizedString("Add new Story", comment: "Add new Story"), preferredStyle: .actionSheet)
        let camera = UIAlertAction(title: NSLocalizedString("Camera", comment: "Camera"), style: .default) { (action) in
            if(UIImagePickerController .isSourceTypeAvailable(UIImagePickerController.SourceType.camera)){
                self.isVideo = false
                let imagePickerController = UIImagePickerController()
                imagePickerController.sourceType = UIImagePickerController.SourceType.camera
                imagePickerController.allowsEditing = false
                imagePickerController.delegate = self
                self.present(imagePickerController, animated: true, completion: nil)
                
            }else{
                let alert  = UIAlertController(title: NSLocalizedString("Warning", comment: "Warning"), message: NSLocalizedString("You don't have camera", comment: "You don't have camera"), preferredStyle: .alert)
                alert.addAction(UIAlertAction(title: NSLocalizedString("OK", comment: "OK"), style: .default, handler: nil))
                self.present(alert, animated: true, completion: nil)
            }
        }
        let videos = UIAlertAction(title: NSLocalizedString("Videos", comment: "Videos"), style: .default) { (UIAlertAction) in
            self.isVideo = true
            let imagePickerController = UIImagePickerController()
            imagePickerController.sourceType = .photoLibrary
            imagePickerController.mediaTypes = ["public.movie"]
            imagePickerController.delegate = self
            self.present(imagePickerController, animated: true, completion: nil)
        }
        let image = UIAlertAction(title: NSLocalizedString("image", comment: "image"), style: .default) { (action) in
            self.isVideo = false
            let imagePickerController = UIImagePickerController()
            imagePickerController.sourceType = UIImagePickerController.SourceType.photoLibrary
            imagePickerController.mediaTypes = ["public.image"]
            imagePickerController.delegate = self
            self.present(imagePickerController, animated: true, completion: nil)
        }
        let cancel = UIAlertAction(title: NSLocalizedString("Cancel", comment: "Cancel"), style: .cancel) { (action) in
            print("cancel")
        }
        alert.addAction(image)
        alert.addAction(videos)
        alert.addAction(camera)
        alert.addAction(cancel)
        if let popoverController = alert.popoverPresentationController {
            popoverController.sourceView = self.view
            popoverController.sourceRect = CGRect(x: self.view.bounds.midX, y: self.view.bounds.midY, width: 0, height: 0)
            popoverController.permittedArrowDirections = []
        }
        self.present(alert, animated: true, completion: nil)
    }
    ///Network Connectivity.
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
            
        }
        
    }
    
    @objc func Segue(notification: NSNotification){
        if let type = notification.userInfo?["type"] as? String{
            if type == "profile"{
                let storyBoard = UIStoryboard(name: "Main", bundle: nil)
                let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
                var groupId: String? = nil
                var pageId: String? = nil
                var user_data: [String:Any]? = nil
                if let data = notification.userInfo?["userData"] as? Int{
                    print(data)
                    if let groupid = self.newsFeedArray[data]["group_id"] as? String{
                        groupId = groupid
                    }
                    if let page_Id = self.newsFeedArray[data]["page_id"] as? String{
                        pageId = page_Id
                    }
                    if let userData = self.newsFeedArray[data]["publisher"] as? [String:Any]{
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
                    if let shared_info = self.newsFeedArray[data]["shared_info"] as? [String:Any]{
                        if shared_info != nil{
                            if let groupid = self.newsFeedArray[data]["group_id"] as? String{
                                groupId = groupid
                            }
                            if let page_Id = self.newsFeedArray[data]["page_id"] as? String{
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
                                if let groupid = self.newsFeedArray[tag]["group_id"] as? String{
                                    groupId = groupid
                                }
                                if let page_Id = self.newsFeedArray[tag]["page_id"] as? String{
                                    pageId = page_Id
                                }
                                if let userData = self.newsFeedArray[tag]["publisher"] as? [String:Any]{
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
            else if type == "product"{
                let Storyboard = UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
                let vc = Storyboard.instantiateViewController(withIdentifier: "ProductDetailsVC") as! ProductDetailController
                if let data = notification.userInfo?["userData"] as? Int{
                    let index =  self.newsFeedArray[data]
                    var seller = [String:Any]()
                    if let publisher = index["publisher"] as? [String:Any]{
                        seller = publisher
                    }
                    if var product = index["product"] as? [String:Any]{
                        product.updateValue(seller, forKey: "seller")
                        vc.productDetails = product
                    }
                }
                self.navigationController?.pushViewController(vc, animated: true)
            }
            else if type == "delete"{
                if let data = notification.userInfo?["userData"] as? Int{
                    print(data)
            self.newsFeedArray.remove(at: data)
            self.tableView.reloadData()
            }
          }
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
                ZKProgressHUD.dismiss()
                self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
            case .online(.wwan), .online(.wiFi):
                performUIUpdatesOnMain {
                    GetNewsFeedManagers.sharedInstance.get_News_Feed(access_token: "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)", limit: 10, off_set: "") {[weak self] (success, authError, error) in
                        if success != nil {
                            for i in success!.data{
                                if i["post_id"] as? String == post_id{
                                    self?.newsFeedArray.insert(i, at: 0)
                                }
                            }
                            self?.spinner.stopAnimating()
                            self?.pulltoRefresh.endRefreshing()
                            self?.tableView.reloadData()
                            ZKProgressHUD.dismiss()
                        }
                        else if authError != nil {
                            ZKProgressHUD.dismiss()
                            self?.view.makeToast(authError?.errors.errorText)
                            self?.showAlert(title: "", message: (authError?.errors.errorText)!)
                        }
                        else if error  != nil {
                            ZKProgressHUD.dismiss()
                            print(error?.localizedDescription)
                            
                        }
                    }
                }
            }
        }
    }
    private func loadStories(){
        switch status {
        case .unknown, .offline:
            ZKProgressHUD.dismiss()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            performUIUpdatesOnMain {
                StoriesManager.sharedInstance.getUserStories(offset: 0, limit: 10) {[weak self] (success, authError, error) in
                    if success != nil {
                        self!.storiesArray = success?.stories ?? []
                        self?.tableView.reloadData()
                        
                    }
                    else if authError != nil {
                        ZKProgressHUD.dismiss()
                        self!.view.makeToast(authError?.errors?.errorText)
                        self!.showAlert(title: "", message: (authError?.errors?.errorText)!)
                    }
                    else if error  != nil {
                        ZKProgressHUD.dismiss()
                        print(error?.localizedDescription)
                        
                    }
                } 
            }
        }
    }
    
    
    
    //Pull To Refresh
    
    @objc func refresh(){
        self.off_set = ""
        self.newsFeedArray.removeAll()
        self.tableView.reloadData()
        self.getNewsFeed(access_token: "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)", limit:10, offset: "0")
    }
    
    func searchBarTextDidBeginEditing(_ searchBar: UISearchBar) {
        self.searchBar.resignFirstResponder()
        let Storyboard = UIStoryboard(name: "Search", bundle: nil)
        let vc = Storyboard.instantiateViewController(withIdentifier: "SearchVC") as! UINavigationController
        vc.modalPresentationStyle = .fullScreen
        vc.modalTransitionStyle = .coverVertical
        self.present(vc, animated: true, completion: nil)
    }
    
    private func getNewsFeed (access_token : String, limit : Int, offset : String) {
        switch status {
        case .unknown, .offline:
            ZKProgressHUD.dismiss()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            performUIUpdatesOnMain {
                GetNewsFeedManagers.sharedInstance.get_News_Feed(access_token: access_token, limit: limit, off_set: offset) {[weak self] (success, authError, error) in
                    if success != nil {
                        for i in success!.data{
                            self?.newsFeedArray.append(i)
                        }
                        self?.off_set = self?.newsFeedArray.last?["post_id"] as? String ?? "0"
                        for it in self!.newsFeedArray{
                            let boosted = it["is_post_boosted"] as? Int ?? 0
                            self?.newsFeedArray.sorted(by: { _,_ in boosted == 1 })
                        }
//                        let boosted = self?.newsFeedArray["is_post_boosted"] as? Int ?? 0
//                        self?.newsFeedArray.sorted(by: { _,_ in boosted == 1 })
                        self?.spinner.stopAnimating()
                        self?.pulltoRefresh.endRefreshing()
                        self?.tableView.reloadData()
                        self?.loadStories()
                        self?.activityIndicator.stopAnimating()
                        ZKProgressHUD.dismiss()                        
                    }
                    else if authError != nil {
                        ZKProgressHUD.dismiss()
                        self?.view.makeToast(authError?.errors.errorText)
                    }
                    else if error  != nil {
                        ZKProgressHUD.dismiss()
                        print(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    @objc func GotoAddPost(sender: UIButton){
        let storyboard = UIStoryboard(name: "AddPost", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "AddPostVC") as! AddPostVC
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
}

extension ViewController : UITableViewDelegate,UITableViewDataSource{
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        if indexPath.section == 0{
            let cell = tableView.dequeueReusableCell(withIdentifier: "StroiesCell") as! StoriesCell
            self.tableView.rowHeight = 100
            cell.bind(self.storiesArray)
            cell.vc = self
            return cell
            
        }
        else if indexPath.section == 1 {
    let cell = tableView.dequeueReusableCell(withIdentifier: "PostStatusCell") as! PostStatusCell
            cell.bind()
            self.tableView.rowHeight = 80.0
            cell.moreBtn.addTarget(self, action: #selector(self.GotoAddPost(sender:)), for: .touchUpInside)
            cell.photoBtn.addTarget(self, action: #selector(self.GotoAddPost(sender:)), for: .touchUpInside)
            return cell
        }
        if (indexPath.section == 2){
            let cell = UITableViewCell()
            self.tableView.rowHeight = 0
            return cell
        }
        if (indexPath.section == 3){
            let cell = UITableViewCell()
            self.tableView.rowHeight = 0
            return cell
        }
        if (indexPath.section == 4){
            let cell = UITableViewCell()
            self.tableView.rowHeight = 0
            return cell
        }
        if (indexPath.section == 5){
            let cell = UITableViewCell()
            self.tableView.rowHeight = 0
            return cell
        }
        else  {
            let index = self.newsFeedArray[indexPath.row]
            var tableViewCells = UITableViewCell()
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
                tableViewCells = GetPostShare.sharedInstance.getsharePost(targetController: self, tableView: self.tableView, indexpath: indexPath, postFile: postfile, array: self.newsFeedArray)
            }
                
            else if (postfile != "")  {
                let url = URL(string: postfile)
                let urlExtension: String? = url?.pathExtension
                if (urlExtension == "jpg" || urlExtension == "png" || urlExtension == "jpeg" || urlExtension == "JPG" || urlExtension == "PNG"){
                    print("NewsFeed",indexPath.row)
                    tableViewCells = GetPostWithImage.sharedInstance.getPostImage(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.newsFeedArray, url: url!, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                }
                    
                else if(urlExtension == "wav" ||  urlExtension == "mp3" || urlExtension == "MP3"){
                    tableViewCells = GetPostMp3.sharedInstance.getMP3(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.newsFeedArray,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                }
                else if (urlExtension == "pdf") {
                    tableViewCells = GetPostPDF.sharedInstance.getPostPDF(targetControler: self, tableView: self.tableView, indexpath: indexPath, postfile: postfile, array: self.newsFeedArray,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                    
                }
                    
                else {
                    tableViewCells = GetPostVideo.sharedInstance.getVideo(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.newsFeedArray, url: url!, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                }
            }
                
            else if (postLink != "") {
                tableViewCells = GetPostWithLink.sharedInstance.getPostLink(targetController: self, tableView: tableView, indexpath: indexPath, postLink: postLink, array: self.newsFeedArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if (postYoutube != "") {
                tableViewCells = GetPostYoutube.sharedInstance.getPostYoutub(targetController: self, tableView: tableView, indexpath: indexPath, postLink: postYoutube, array: self.newsFeedArray,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
            else if (blog != "0") {
                tableViewCells = GetPostBlog.sharedInstance.GetBlog(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.newsFeedArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if (group != false){
                tableViewCells = GetPostGroup.sharedInstance.GetGroupRecipient(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.newsFeedArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if (product != "0") {
                tableViewCells = GetPostProduct.sharedInstance.GetProduct(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.newsFeedArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
            else if (event != "0") {
                tableViewCells = GetPostEvent.sharedInstance.getEvent(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array:  self.newsFeedArray,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
            else if (postSticker != "") {
                tableViewCells = GetPostSticker.sharedInstance.getPostSticker(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.newsFeedArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if (colorId != "0"){
                tableViewCells = GetPostWithBg_Image.sharedInstance.postWithBg_Image(targetController: self, tableView: tableView, indexpath: indexPath, array: self.newsFeedArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if (multi_image != "0") {
                tableViewCells = GetPostMultiImage.sharedInstance.getMultiImage(targetController: self, tableView: tableView, indexpath: indexPath, array: self.newsFeedArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
                
            else if photoAlbum != "" {
                tableViewCells = getPhotoAlbum.sharedInstance.getPhoto_Album(targetController: self, tableView: tableView, indexpath: indexPath, array: self.newsFeedArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if postOptions != "0" {
                tableViewCells = GetPostOptions.sharedInstance.getPostOptions(targertController: self, tableView: tableView, indexpath: indexPath, array: self.newsFeedArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else if postRecord != ""{
                tableViewCells = GetPostRecord.sharedInstance.getPostRecord(targetController: self, tableView: tableView, indexpath: indexPath, array: self.newsFeedArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
            else if fundDonation != nil{
                tableViewCells = GetDonationPost.sharedInstance.getDonationpost(targetController: self, tableView: tableView, indexpath: indexPath, array: self.newsFeedArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
                
            else {
                tableViewCells = GetNormalPost.sharedInstance.getPostText(targetController: self, tableView: self.tableView, indexpath: indexPath, postFile: "", array: self.newsFeedArray, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
                
            }
            return tableViewCells
            
        }
        
        
    }
    
    func tableView(_ tableView: UITableView, willDisplay cell: UITableViewCell, forRowAt indexPath: IndexPath) {
     
        if self.newsFeedArray.count >= 10 {
            let count = self.newsFeedArray.count
            let lastElement = count - 1
            
            if indexPath.row == lastElement {
                spinner.startAnimating()
                spinner.frame = CGRect(x: CGFloat(0), y: CGFloat(0), width: tableView.bounds.width, height: CGFloat(44))
                self.tableView.tableFooterView = spinner
                
                self.tableView.tableFooterView?.isHidden = false
                self.getNewsFeed(access_token: "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)", limit: 10, offset: off_set)
            }
        }
    }
    
    func tableView(_ tableView: UITableView, didEndDisplaying cell: UITableViewCell, forRowAt indexPath: IndexPath) {
        GetPostRecord.sharedInstance.stopSound()
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if section == 0 {
            return 1
        }
        else if (section == 1) {
            return 1
        }
        else if (section == 2) {
            return 1
        }
        else if (section == 3) {
            return 1
        }
        else if (section == 4) {
            return 1
        }
        else if (section == 5) {
            return 1
        }
        else {
            return self.newsFeedArray.count
        }
    }
    func numberOfSections(in tableView: UITableView) -> Int {
        return 7
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
        if indexPath.section == 0 {
            print("Nothing")
            
        }
        else if indexPath.section == 1 {
            let storyboard = UIStoryboard(name: "AddPost", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "AddPostVC") as! AddPostVC
            self.navigationController?.pushViewController(vc, animated: true)
            
        }
        else {
            print("Didtap")
        }
    }
}
extension ViewController : UIImagePickerControllerDelegate , UINavigationControllerDelegate {
    
    public func imagePickerController(_ picker: UIImagePickerController, didFinishPickingMediaWithInfo info: [UIImagePickerController.InfoKey : Any]) {
        
        picker.dismiss(animated: true) {
            if self.isVideo! {
                
                let vidURL = info[UIImagePickerController.InfoKey.mediaURL] as! URL
                var CreateVideoStoryVC = UIStoryboard(name: "Stories", bundle: nil).instantiateViewController(withIdentifier: "CreateVideoStoryVC") as! CreateVideoStoryVC
                CreateVideoStoryVC.videoLinkString  = vidURL.absoluteString
                self.navigationController?.pushViewController(CreateVideoStoryVC, animated: true)
                
            }else{
                let img = info[UIImagePickerController.InfoKey.originalImage] as? UIImage
                
                var CreateImageStoryVC = UIStoryboard(name: "Stories", bundle: nil).instantiateViewController(withIdentifier: "CreateImageStoryVC") as! CreateImageStoryVC
                CreateImageStoryVC.imageLInkString  = FileManager().savePostImage(image: img!)
                CreateImageStoryVC.iamge = img
                CreateImageStoryVC.isVideo = self.isVideo
                self.navigationController?.pushViewController(CreateImageStoryVC, animated: true)
            }
        }
    }
    
    public func imagePickerControllerDidCancel(_ picker: UIImagePickerController) {
        picker.dismiss(animated: true)
    }
}

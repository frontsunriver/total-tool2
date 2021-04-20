//
//  NotificationVC.swift
//  News_Feed
//
//  Created by Muhammad Haris Butt on 3/25/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import AlamofireImage
import Kingfisher
import SDWebImage
import PaginatedTableView
import Toast_Swift
import ZKProgressHUD
import WoWonderTimelineSDK
import GoogleMobileAds



class NotificationVC: UIViewController {
    
  @IBOutlet weak var tableView: UITableView!
  @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    let spinner = UIActivityIndicatorView(style: .gray)
    let pulltoRefresh = UIRefreshControl()
    var notificationArray = [[String:Any]]()
    let status = Reach().connectionStatus()
    var shouldRefreshStories = false
    var interstitial: GADInterstitial!
    
    
    var isVideo:Bool? = false
    lazy var searchBar:UISearchBar = UISearchBar(frame: CGRect(x: 0.0, y: 0.0, width: 250, height: 40))
    let placeholder = NSAttributedString(string: "Search", attributes: [.foregroundColor: UIColor.white])
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.prefersLargeTitles = true
        navigationController?.view.backgroundColor = UIColor.hexStringToUIColor(hex: "994141")
        self.navigationController?.navigationBar.isTranslucent = false
       self.navigationController?.navigationItem.largeTitleDisplayMode = .never
        self.setupUI()
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
        self.navigationItem.title  = NSLocalizedString("Notifications", comment: "Notifications")

    }
    
    @IBAction func cameraPressed(_ sender: Any) {
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
        if let popoverController = alert.popoverPresentationController {
            popoverController.sourceView = self.view
            popoverController.sourceRect = CGRect(x: self.view.bounds.midX, y: self.view.bounds.midY, width: 0, height: 0)
            popoverController.permittedArrowDirections = []
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
    
    private func setupUI(){
        self.tableView.separatorStyle = .none
          tableView.register(UINib(nibName: "NotificationsTableItem", bundle: nil), forCellReuseIdentifier: "NotificationsTableItem")
        
        print(UserData.getUSER_ID()!)
        print(UserData.getAccess_Token()!)
//        if let textfield = self.searchBar.value(forKey: "searchField") as? UITextField {
//            textfield.clearButtonMode = .never
//            textfield.backgroundColor = UIColor.clear
//            textfield.attributedPlaceholder = NSAttributedString(string:" Search...", attributes:[NSAttributedString.Key.foregroundColor: UIColor.yellow])
//            textfield.textColor = .white
//            if let leftView = textfield.leftView as? UIImageView {
//                leftView.image = leftView.image?.withRenderingMode(.alwaysTemplate)
//                leftView.tintColor = UIColor.white
//            }
//        }
//        self.searchBar.delegate = self
//        self.searchBar.tintColor = .white
//        let leftNavBarButton = UIBarButtonItem(customView:searchBar)
//        self.navigationItem.leftBarButtonItem = leftNavBarButton
        self.tableView.delegate = self
        self.tableView.dataSource = self
        self.tableView.backgroundColor = .white
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.tableView.addSubview(pulltoRefresh)
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
//        NotificationCenter.default.addObserver(self, selector: #selector(loadList), name: NSNotification.Name(rawValue: "load"), object: nil)
//
        Reach().monitorReachabilityChanges()
       // ZKProgressHUD.show("Loading")
        self.activityIndicator.startAnimating()
        SetUpcells.setupCells(tableView: self.tableView)
        self.loadNotification()
        
    }
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
            
        }
    }
    @objc func refresh(){
        self.notificationArray.removeAll()
        self.tableView.reloadData()
        loadNotification()
        pulltoRefresh.endRefreshing()
        
    }
    private func loadNotification(){
        switch status {
        case .unknown, .offline:
//            ZKProgressHUD.dismiss()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            performUIUpdatesOnMain {
                NotificationManager.instance.getNotification(offset: 0, limit: 20, type: "notifications") { (success, authError, error) in
                    print(success?.notifications)
                    if success != nil {
                        self.notificationArray = success?.notifications ?? []
                        self.tableView.reloadData()
//                         ZKProgressHUD.dismiss()
                        self.activityIndicator.stopAnimating()
                        print(self.notificationArray)
                    }
                    else if authError != nil {
//                        ZKProgressHUD.dismiss()
                        self.view.makeToast(authError?.errors?.errorText)
                        self.showAlert(title: "", message: (authError?.errors?.errorText)!)
                    }
                    else if error  != nil {
//                        ZKProgressHUD.dismiss()
                        print(error?.localizedDescription)
                        
                    }
                }
            }
        }
    }
    
}
extension NotificationVC:UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
         return self.notificationArray.count
    }
    
   
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "NotificationsTableItem") as! NotificationsTableItem
        let index = self.notificationArray[indexPath.row]
        if let notifier = index["notifier"] as? [String:Any]{
            if let image = notifier["avatar"] as? String{
                let url = URL(string: image)
                cell.profileImage.kf.setImage(with: url)
            }
            if let name = notifier["name"] as? String{
                cell.titleLabel.text = name
            }
        }
        if let type = index["type"] as? String{
            if type == "joined_group"{
                cell.changeBg.backgroundColor = UIColor.hexStringToUIColor(hex: "3F30FF")
                cell.changeIcon.image = UIImage(named: "tick")
            }
            else if type == "poke"{
                cell.changeBg.backgroundColor = UIColor.hexStringToUIColor(hex: "1F2124")
                cell.changeIcon.image = UIImage(named: "notification")
            }
            else if type == "shared_your_post"{
                cell.changeBg.backgroundColor = UIColor.hexStringToUIColor(hex: "1F2124")
                cell.changeIcon.image = #imageLiteral(resourceName: "Share")
            }
            else{
               cell.changeBg.backgroundColor = UIColor.hexStringToUIColor(hex: "994141")
               cell.changeIcon.image = UIImage(named: "like-2")
            }
        }
        if let type_text = index["type_text"] as? String{
            cell.descriptionLabel.text = type_text
        }
        return cell
     
    }
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 80.0
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
        let index = self.notificationArray[indexPath.row]
        if let Type = index["type"] as? String{
            if (Type == "following" || Type == "visited_profile" || Type == "accepted_request"){
                if let notifier = index["notifier"] as? [String:Any]{
                let storyBoard = UIStoryboard(name: "Main", bundle: nil)
                let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
                vc.userData = notifier
                self.navigationController?.pushViewController(vc, animated: true)
                }
            }
            else if (Type == "liked_page" || Type == "invited_page" || Type == "accepted_invite"){
                
            }
            else if (Type == "joined_group" || Type == "accepted_join_request" || Type == "added_you_to_group"){
                if let id  = index["group_id"] as? String{
                    let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "GroupVC") as! GroupController
                    vc.id = id
                    self.navigationController?.pushViewController(vc, animated: true)
                }
            }
            else if (Type == "going_event"){
                self.shouldRefreshStories = true
                             PPStoriesItemsViewControllerVC = UIStoryboard(name: "Stories", bundle: nil).instantiateViewController(withIdentifier: "StoryItemVC") as! StoryItemVC
                             let vc = PPStoriesItemsViewControllerVC
                             vc.refreshStories = {
                                 //                self.viewModel?.refreshStories.accept(true)
                             }
                             vc.modalPresentationStyle = .overFullScreen
//                             vc.pages = (self.storiesArray)
                             vc.currentIndex = indexPath.row
                          self.present(vc, animated: true, completion: nil)
                
            }
            else if (Type == "viewed_story"){
                
                
            }
            else if (Type == "requested_to_join_group"){
                
            }
            else{
                if let id = index["post_id"] as? String{
                let storyBoard = UIStoryboard(name: "Main", bundle: nil)
                let vc = storyBoard.instantiateViewController(withIdentifier: "ShowPostVC") as! ShowPostController
                    vc.postId = id
                self.navigationController?.pushViewController(vc, animated: true)
                }
            }
        }
    }
}
extension NotificationVC:UISearchBarDelegate{
    func searchBarTextDidBeginEditing(_ searchBar: UISearchBar) {
        self.searchBar.resignFirstResponder()
        let Storyboard = UIStoryboard(name: "Search", bundle: nil)
        let vc = Storyboard.instantiateViewController(withIdentifier: "SearchVC") as! SearchController
        vc.modalPresentationStyle = .fullScreen
        vc.modalTransitionStyle = .coverVertical
        self.present(vc, animated: true, completion: nil)
    }
}
extension NotificationVC : UIImagePickerControllerDelegate , UINavigationControllerDelegate {
    
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

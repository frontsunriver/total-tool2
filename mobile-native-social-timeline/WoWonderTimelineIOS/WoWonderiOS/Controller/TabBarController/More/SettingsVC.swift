//
//  SettingsVC.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/13/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

struct DataSet{
    var title:String?
    var image:UIImage?
}

class SettingsVC: BaseVC {
    @IBOutlet weak var tableView: UITableView!
    
    @IBOutlet weak var collectionView: UICollectionView!
    
    let appdelegate = UIApplication.shared.delegate as? AppDelegate
    
    
    var data = [DataSet]()
    var data1 = [DataSet]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.setupUI()
        self.navigationController?.navigationBar.prefersLargeTitles = true
        self.navigationController?.navigationItem.largeTitleDisplayMode = .never
        self.navigationController?.view.backgroundColor = UIColor.hexStringToUIColor(hex: "994141")
        self.collectionView.register(UINib(nibName: "MoreItemCell", bundle: nil), forCellWithReuseIdentifier: "MoreItemCells")
         self.collectionView.register(UINib(nibName: "MoreItemCell2", bundle: nil), forCellWithReuseIdentifier: "MoreImageCells2")
        //        self.collectionView.re
        self.collectionView.delegate = self
        self.collectionView.dataSource = self
    }
    
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        self.navigationController?.navigationBar.isHidden = false
        self.tabBarController?.tabBar.isHidden = false
        self.navigationItem.title  = NSLocalizedString("More", comment: "More")
    }
    
    private func setupUI(){
//        self.tableView.separatorStyle = .none
//        tableView.register(UINib(nibName: "SettingsTableItem", bundle: nil), forCellReuseIdentifier: "SettingsTableItem")
        self.data = [
            DataSet(title: NSLocalizedString("My Profile", comment: "My Profile"), image: UIImage(named: "Profiles")),
            DataSet(title: NSLocalizedString("Messages", comment: "Messages"), image: UIImage(named: "speech-bubble")),
            DataSet(title: NSLocalizedString("Following", comment: "Following"), image: UIImage(named: "teamwork")),
            DataSet(title: NSLocalizedString("Pokes", comment: "Pokes"), image: UIImage(named: "tap")),
            DataSet(title: NSLocalizedString("Albums", comment: "Albums"), image: UIImage(named: "albums-1")),
            DataSet(title: NSLocalizedString("My Images", comment: "My Images"), image: UIImage(named: "picture")),
            DataSet(title: NSLocalizedString("My Videos", comment: "My Videos"), image: UIImage(named: "play-buttons")),
            DataSet(title: NSLocalizedString("Saved Posts", comment: "Saved Posts"), image: UIImage(named: "bookmark")),
            DataSet(title: NSLocalizedString("Groups", comment: "Groups"), image: UIImage(named: "groupes")),
            DataSet(title: NSLocalizedString("Pages", comment: "Pages"), image: UIImage(named: "goal")),
            DataSet(title: NSLocalizedString("Blogs", comment: "Blogs"), image: UIImage(named: "paid-articles")),
            DataSet(title: NSLocalizedString("MarketPlace", comment: "MarketPlace"), image: UIImage(named: "bags")),
            DataSet(title: NSLocalizedString("Popular Posts", comment: "Popular Posts"), image: UIImage(named: "search-engine")),
            DataSet(title: NSLocalizedString("Events", comment: "Events"), image: UIImage(named: "calendarss")),
            DataSet(title: NSLocalizedString("Find Friends", comment: "Find Friends"), image: UIImage(named: "location")),
            DataSet(title: NSLocalizedString("Offers", comment: "Offers"), image: UIImage(named: "sale")),
            DataSet(title: NSLocalizedString("Movies", comment: "Movies"), image: UIImage(named: "movie_video")),
            DataSet(title: NSLocalizedString("Jobs", comment: "Jobs"), image: UIImage(named: "briefcase")),
            DataSet(title: NSLocalizedString("Common Things", comment: "Common Things"), image: UIImage(named: "Archery")),
            DataSet(title: NSLocalizedString("Funding", comment: "Funding"), image: UIImage(named: "bank")),
            DataSet(title: NSLocalizedString("Gaming", comment: "Gaming"), image: UIImage(named: "game-controllers"))
        ]
        self.data1 = [DataSet(title: NSLocalizedString("General Account", comment: "General Account"), image: UIImage(named: "tools-cross")),
        DataSet(title: NSLocalizedString("Privacy", comment: "Privacy"), image: UIImage(named: "eye")),
        DataSet(title: NSLocalizedString("Notifications", comment: "Notifications"), image: UIImage(named: "notification")),
        DataSet(title: NSLocalizedString("Invitation Links", comment: "Invitation Links"), image: UIImage(named: "chain")),
        DataSet(title: NSLocalizedString("My Information", comment: "My Information"), image: UIImage(named: "clipboardes")),
        DataSet(title: NSLocalizedString("Earnings", comment: "Earnings"), image: UIImage(named: "home-page")),
        DataSet(title: NSLocalizedString("Help & Support", comment: "Help & Support"), image: UIImage(named: "question-mark")),
        DataSet(title: NSLocalizedString("Logout", comment: "Logout"), image: UIImage(named: "logout"))]
    }
    
    
    
}


extension SettingsVC: UICollectionViewDelegate,UICollectionViewDataSource,UICollectionViewDelegateFlowLayout{
    
    
    func numberOfSections(in collectionView: UICollectionView) -> Int {
        return 2
    }
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        if (section == 0){
        return self.data.count
        }
        else{
            return self.data1.count
        }
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        if (indexPath.section == 0){
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "MoreItemCells", for: indexPath) as! MoreItemCell
        cell.nameLabel.text = self.data[indexPath.row].title ?? ""
        cell.itemImage.image =  self.data[indexPath.row].image ?? UIImage()
            return cell
        }
        else{
            let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "MoreImageCells2", for: indexPath) as! MoreItemCell2
            cell.nameLabel.text = self.data1[indexPath.row].title ?? ""
            cell.imageItem.image =  self.data1[indexPath.row].image ?? UIImage()
                return cell
        }
    }
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        if (indexPath.section == 0){
            switch indexPath.item {
            case 0:
                let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "MyProfileVC") as! MyProfileController
                vc.is_Profile = 1
                self.navigationController?.pushViewController(vc, animated: true)
            case 1:
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
            case 2:
                let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "FollowingVC") as! FollowingController
                vc.userId = UserData.getUSER_ID()!
                vc.type = "following"
                vc.navTitle = NSLocalizedString("Following", comment: "Following")
                self.navigationController?.pushViewController(vc, animated: true)
            case 3:
                let storyboard = UIStoryboard(name: "Poke-MyVideos-Albums", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "PokeVC") as! PokeController
                self.navigationController?.pushViewController(vc, animated: true)
                
            case 4:
                let storyboard = UIStoryboard(name: "Poke-MyVideos-Albums", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "AlbumVC") as! AlbumController
                self.navigationController?.pushViewController(vc, animated: true)
                
            case 5:
                let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "ImagesVC") as! MyImagesController
                self.navigationController?.pushViewController(vc, animated: true)
                
            case 6:
                let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "MyVideosController") as! MyVideosController
                self.navigationController?.pushViewController(vc, animated: true)
                
            case 7:
                let storyboard = UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "SavedPostVC") as! SavedPostController
                self.navigationController?.pushViewController(vc, animated: true)
            case 8:
                let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "MyGroupsVC") as! GroupListController
                self.navigationController?.pushViewController(vc, animated: true)
                
            case 9:
                let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "PageListVC") as! PageListsController
                vc.user_id = UserData.getUSER_ID()!
                vc.isOwner = true
                self.navigationController?.pushViewController(vc, animated: true)
                
            case 10:
                let storyboard = UIStoryboard(name: "Poke-MyVideos-Albums", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "ArticleVC") as! BlogController
                self.navigationController?.pushViewController(vc, animated: true)
                
            case 11:
                let storyboard = UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "GetProductsVC") as! GetProductsController
                self.navigationController?.pushViewController(vc, animated: true)
                
            case 12:
                let storyboard = UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "PopularPostVC") as! PopularPostController
                self.navigationController?.pushViewController(vc, animated: true)
                
            case 13:
                let storyboard = UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "EventVC") as! EventsParentController
                self.navigationController?.pushViewController(vc, animated: true)
            case 14:
                print("find friends")
                let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "FindFriendVC") as! FindFriendVC
                self.navigationController?.pushViewController(vc, animated: true)
            case 15:
                
                let storyboard = UIStoryboard(name: "Offers", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "GetOffersVC") as! GetOffersVC
                self.navigationController?.pushViewController(vc, animated: true)
            case 16:
                print("Movies")
                let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "MoviesVC") as! MoviesVC
                self.navigationController?.pushViewController(vc, animated: true)
            case 17:
                print("Jobs")
                
                let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
                //            let storyboard = UIStoryboard(name: "Jobs", bundle: nil)
                //            let vc = storyboard.instantiateViewController(withIdentifier: "AllJobsVC") as! AllJobsController
                //            self.navigationController?.pushViewController(vc, animated: true)
                let vc = storyboard.instantiateViewController(withIdentifier: "JobsVC") as! JobsVC
                self.navigationController?.pushViewController(vc, animated: true)
            case 18:
                print("Common Things")
                let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "CommonThingsVC") as! CommonThingsVC
                self.navigationController?.pushViewController(vc, animated: true)
            case 19:
                let storyboard = UIStoryboard(name: "Funding", bundle: nil)
                //            let vc = storyboard.instantiateViewController(withIdentifier: "ShowFundingsVC") as! ShowFundingsVC
                let vc = storyboard.instantiateViewController(withIdentifier: "FundingsParentVc") as! FundingParentVC
                
                self.navigationController?.pushViewController(vc, animated: true)
            case 20:
                print("Gaming")
                let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "GamesParentVC") as! GamesParentVC
                self.navigationController?.pushViewController(vc, animated: true)
            default:
                
                print("Nothing to navigate")
                
            }
        }
        else{
            switch indexPath.item {
                case 0:
                    print("General Account")
                    let storyboard = UIStoryboard(name: "General", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "GeneralVC") as! GeneralVC
                    self.navigationController?.pushViewController(vc, animated: true)
                case 1:
                    print("Privacy")
                    let storyboard = UIStoryboard(name: "Privacy", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "PrivacyVC") as! PrivacyVC
                    self.navigationController?.pushViewController(vc, animated: true)
                case 2:
                    print("Notifications")
                    let storyboard = UIStoryboard(name: "Notification", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "NotificationCheckVC") as! NotificationCheckVC
                    self.navigationController?.pushViewController(vc, animated: true)
                case 3:
                    print("Invitation Link")
                    let storyboard = UIStoryboard(name: "MoreSection2", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "InvitationLinkVC") as! InvitationLinkController
                    self.navigationController?.pushViewController(vc, animated: true)
                case 4:
                    let storyboard = UIStoryboard(name: "MoreSection2", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "MyInfoVC") as! MyInformationController
                    self.navigationController?.pushViewController(vc, animated: true)
                case 5:
                    print("Tell a Friends")
                    let storyboard = UIStoryboard(name: "TellFriend", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "TellFriendVC") as! TellFriendVC
                    self.navigationController?.pushViewController(vc, animated: true)
                case 6:
                    print("Help & Support")
                    let storyboard = UIStoryboard(name: "HelpSupport", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "HelpAndSupportVC") as! HelpAndSupportVC
                    self.navigationController?.pushViewController(vc, animated: true)
                case 7:
                    print("Logout")
                    let alert = UIAlertController(title: NSLocalizedString("Logout", comment: "Logout"), message: NSLocalizedString("Are you sure you want to logout", comment: "Are you sure you want to logout"), preferredStyle: .alert)
                    
                    let cancel = UIAlertAction(title: NSLocalizedString("Cancel", comment: "Cancel"), style: .cancel, handler: nil)
                    let logout = UIAlertAction(title: NSLocalizedString("Logout", comment: "Logout"), style: .destructive) { (aciton) in
                        UserData.clear()
                        if #available(iOS 13.0, *) {
                            let storyboard = UIStoryboard(name: "Authentication", bundle: nil).instantiateViewController(identifier: "FirstVc")
                            self.appdelegate?.window?.rootViewController = storyboard
                        } else {
                            
                        }
                    }
                    alert.addAction(logout)
                    alert.addAction(cancel)
                    if let popoverController = alert.popoverPresentationController {
                        popoverController.sourceView = self.view
                        popoverController.sourceRect = CGRect(x: self.view.bounds.midX, y: self.view.bounds.midY, width: 0, height: 0)
                        popoverController.permittedArrowDirections = []
                    }
                    self.present(alert, animated: true, completion: nil)
                
            default:
                print("Nothing")
            }
        }
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        if (indexPath.section == 0){
            let padding: CGFloat = 10
            //        let collectionViewSize = collectionView.frame.size.width - padding
            let collectionViewSize = collectionView.frame.size.width
            print(collectionViewSize)
            print(collectionViewSize/4)
            return CGSize(width: collectionViewSize/4, height: 108.0)
        }
        else{
            let collectionViewSize = collectionView.frame.size.width
               print(collectionViewSize)
               print(collectionViewSize/4)
            return CGSize(width: collectionViewSize - 10, height: 45.0)
        }
 
    }
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, minimumLineSpacingForSectionAt section: Int) -> CGFloat {
        return 5.0
    }
    
//    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, minimumInteritemSpacingForSectionAt section: Int) -> CGFloat {
//        return -15.0
//    }
    
    
}

//
//extension SettingsVC: UITableViewDataSource,UITableViewDelegate {
//
//    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
//
//        return self.data.count
//    }
//
//    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
//
//        let cell = tableView.dequeueReusableCell(withIdentifier: "SettingsTableItem") as! SettingsTableItem
//        cell.titleLabel.text = self.data[indexPath.row].title ?? ""
//        cell.iconImage.image = self.data[indexPath.row].image ?? UIImage()
//        return cell
//    }
//    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
//        switch indexPath.row {
//        case 0:
//            let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "MyProfileVC") as! MyProfileController
//            vc.is_Profile = 1
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 1:
//            let appURLScheme = "AppToOpen://"
//            guard let appURL = URL(string: appURLScheme) else {
//                return
//            }
//
//            if UIApplication.shared.canOpenURL(appURL) {
//
//                if #available(iOS 10.0, *) {
//                    UIApplication.shared.open(appURL)
//                }
//                else {
//                    UIApplication.shared.openURL(appURL)
//                }
//            }
//            else {
//                self.view.makeToast("Please install WoWonder Messenger App")
//            }
//        case 2:
//            let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "FollowingVC") as! FollowingController
//            vc.userId = UserData.getUSER_ID()!
//            vc.type = "following"
//            vc.navTitle = NSLocalizedString("Following", comment: "Following")
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 3:
//            let storyboard = UIStoryboard(name: "Poke-MyVideos-Albums", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "PokeVC") as! PokeController
//            self.navigationController?.pushViewController(vc, animated: true)
//
//        case 4:
//            let storyboard = UIStoryboard(name: "Poke-MyVideos-Albums", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "AlbumVC") as! AlbumController
//            self.navigationController?.pushViewController(vc, animated: true)
//
//        case 5:
//            let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "ImagesVC") as! MyImagesController
//            self.navigationController?.pushViewController(vc, animated: true)
//
//        case 6:
//            let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "MyVideosController") as! MyVideosController
//            self.navigationController?.pushViewController(vc, animated: true)
//
//        case 7:
//            let storyboard = UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "SavedPostVC") as! SavedPostController
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 8:
//            let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "MyGroupsVC") as! GroupListController
//            self.navigationController?.pushViewController(vc, animated: true)
//
//        case 9:
//            let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "PageListVC") as! PageListsController
//            vc.user_id = UserData.getUSER_ID()!
//            vc.isOwner = true
//            self.navigationController?.pushViewController(vc, animated: true)
//
//        case 10:
//            let storyboard = UIStoryboard(name: "Poke-MyVideos-Albums", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "ArticleVC") as! BlogController
//            self.navigationController?.pushViewController(vc, animated: true)
//
//        case 11:
//            let storyboard = UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "GetProductsVC") as! GetProductsController
//            self.navigationController?.pushViewController(vc, animated: true)
//
//        case 12:
//            let storyboard = UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "PopularPostVC") as! PopularPostController
//            self.navigationController?.pushViewController(vc, animated: true)
//
//        case 13:
//            let storyboard = UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "EventVC") as! EventsParentController
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 14:
//            print("find friends")
//            let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "FindFriendVC") as! FindFriendVC
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 15:
//
//            let storyboard = UIStoryboard(name: "Offers", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "GetOffersVC") as! GetOffersVC
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 16:
//            print("Movies")
//            let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "MoviesVC") as! MoviesVC
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 17:
//            print("Jobs")
//
//            let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
//            //            let storyboard = UIStoryboard(name: "Jobs", bundle: nil)
//            //            let vc = storyboard.instantiateViewController(withIdentifier: "AllJobsVC") as! AllJobsController
//            //            self.navigationController?.pushViewController(vc, animated: true)
//            let vc = storyboard.instantiateViewController(withIdentifier: "JobsVC") as! JobsVC
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 18:
//            print("Common Things")
//            let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "CommonThingsVC") as! CommonThingsVC
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 19:
//            let storyboard = UIStoryboard(name: "Funding", bundle: nil)
//            //            let vc = storyboard.instantiateViewController(withIdentifier: "ShowFundingsVC") as! ShowFundingsVC
//            let vc = storyboard.instantiateViewController(withIdentifier: "FundingsParentVc") as! FundingParentVC
//
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 20:
//            print("Gaming")
//            let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "GamesParentVC") as! GamesParentVC
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 21:
//            print("Invitation Link")
//            let storyboard = UIStoryboard(name: "MoreSection2", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "InvitationLinkVC") as! InvitationLinkController
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 22:
//            let storyboard = UIStoryboard(name: "MoreSection2", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "MyInfoVC") as! MyInformationController
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 23:
//            print("General Account")
//            let storyboard = UIStoryboard(name: "General", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "GeneralVC") as! GeneralVC
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 24:
//            print("Privacy")
//            let storyboard = UIStoryboard(name: "Privacy", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "PrivacyVC") as! PrivacyVC
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 25:
//            print("Notifications")
//            let storyboard = UIStoryboard(name: "Notification", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "NotificationCheckVC") as! NotificationCheckVC
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 26:
//            print("Tell a Friends")
//            let storyboard = UIStoryboard(name: "TellFriend", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "TellFriendVC") as! TellFriendVC
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 27:
//            print("Help & Support")
//            let storyboard = UIStoryboard(name: "HelpSupport", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "HelpAndSupportVC") as! HelpAndSupportVC
//            self.navigationController?.pushViewController(vc, animated: true)
//        case 28:
//            print("Logout")
//            let alert = UIAlertController(title: NSLocalizedString("Logout", comment: "Logout"), message: NSLocalizedString("Are you sure you want to logout", comment: "Are you sure you want to logout"), preferredStyle: .alert)
//
//            let cancel = UIAlertAction(title: NSLocalizedString("Cancel", comment: "Cancel"), style: .cancel, handler: nil)
//            let logout = UIAlertAction(title: NSLocalizedString("Logout", comment: "Logout"), style: .destructive) { (aciton) in
//                UserData.clear()
//                if #available(iOS 13.0, *) {
//                    let storyboard = UIStoryboard(name: "Authentication", bundle: nil).instantiateViewController(identifier: "FirstVc")
//                    self.appdelegate?.window?.rootViewController = storyboard
//                } else {
//
//                }
//            }
//            alert.addAction(logout)
//            alert.addAction(cancel)
//            if let popoverController = alert.popoverPresentationController {
//                popoverController.sourceView = self.view
//                popoverController.sourceRect = CGRect(x: self.view.bounds.midX, y: self.view.bounds.midY, width: 0, height: 0)
//                popoverController.permittedArrowDirections = []
//            }
//            self.present(alert, animated: true, completion: nil)
//        default:
//
//            print("Nothing to navigate")
//
//        }
//    }
//    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
//
//        return 50.0
//
//    }
//}

//
//  MoreController.swift
//  News_Feed
//
//  Created by Ubaid Javaid on 12/24/19.
//  Copyright © 2019 clines329. All rights reserved.
//

import UIKit
import WoWonderTimelineSDK

class MoreController: UIViewController {
    
    @IBOutlet weak var profile: UIButton!
    @IBOutlet weak var message: UIButton!
    @IBOutlet weak var following: UIButton!
    @IBOutlet weak var pokes: UIButton!
    @IBOutlet weak var album: UIButton!
    @IBOutlet weak var myImages: UIButton!
    @IBOutlet weak var myVideos: UIButton!
    @IBOutlet weak var savedPost: UIButton!
    @IBOutlet weak var groups: UIButton!
    @IBOutlet weak var pages: UIButton!
    @IBOutlet weak var blogs: UIButton!
    @IBOutlet weak var marketPlace: UIButton!
    @IBOutlet weak var popularPost: UIButton!
    @IBOutlet weak var events: UIButton!
    @IBOutlet weak var funding: UIButton!
    @IBOutlet weak var offers: UIButton!
    @IBOutlet weak var general: UIButton!
    @IBOutlet weak var privacy: UIButton!
    @IBOutlet weak var notification: UIButton!
    @IBOutlet weak var tellFriend: UIButton!
    @IBOutlet weak var helpSupport: UIButton!
    @IBOutlet weak var logout: UIButton!
    
    
    let appdelegate = UIApplication.shared.delegate as? AppDelegate
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.prefersLargeTitles = true
        self.navigationController?.navigationItem.largeTitleDisplayMode = .never
        self.navigationController?.view.backgroundColor = UIColor.hexStringToUIColor(hex: "994141")
        self.profile.setTitle(NSLocalizedString("My Profile", comment: "My Profile"), for: .normal)
        self.message.setTitle(NSLocalizedString("Message", comment: "Message"), for: .normal)
        self.following.setTitle(NSLocalizedString("Following", comment: "Following"), for: .normal)
        self.pokes.setTitle(NSLocalizedString("Pokes", comment: "Pokes"), for: .normal)
        self.album.setTitle(NSLocalizedString("Albums", comment: "Albums"), for: .normal)
        self.myImages.setTitle(NSLocalizedString("My Images", comment: "My Images"), for: .normal)
        self.myVideos.setTitle(NSLocalizedString("My Videos", comment: "My Videos"), for: .normal)
        self.savedPost.setTitle(NSLocalizedString("Saved Posts", comment: "Saved Posts"), for: .normal)
        self.groups.setTitle(NSLocalizedString("Groups", comment: "Groups"), for: .normal)
        self.pages.setTitle(NSLocalizedString("Pages", comment: "Pages"), for: .normal)
        self.blogs.setTitle(NSLocalizedString("Blogs", comment: "Blogs"), for: .normal)
        self.marketPlace.setTitle(NSLocalizedString("MarketPlace", comment: "MarketPlace"), for: .normal)
        self.popularPost.setTitle(NSLocalizedString("Popular Posts", comment: "Popular Posts"), for: .normal)
        self.events.setTitle(NSLocalizedString("Events", comment: "Events"), for: .normal)
        self.funding.setTitle(NSLocalizedString("Funding", comment: "Funding"), for: .normal)
        self.offers.setTitle(NSLocalizedString("Offers", comment: "Offers"), for: .normal)
        self.general.setTitle(NSLocalizedString("General Account", comment: "General Account"), for: .normal)
        self.privacy.setTitle(NSLocalizedString("Privacy", comment: "Privacy"), for: .normal)
        self.notification.setTitle(NSLocalizedString("Notifications", comment: "     Notifications"), for: .normal)
        self.tellFriend.setTitle(NSLocalizedString("Tell a Friends", comment: "Tell a Friends"), for: .normal)
        self.helpSupport.setTitle(NSLocalizedString("Help & Support", comment: "Help & Support"), for: .normal)
        self.logout.setTitle(NSLocalizedString("Logout", comment: "Logout"), for: .normal)



        





        


        




        
        
        
        
    }
    
    override func viewWillAppear(_ animated: Bool) {
        self.navigationController?.navigationBar.isHidden = false
    }
    @IBAction func Buttons(_ sender: UIButton) {
        switch sender.tag {
        case 0:
            let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "MyProfileVC") as! MyProfileController
            vc.is_Profile = 1
            self.navigationController?.pushViewController(vc, animated: true)
            
        case 1:
            print("Messages")
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
            print("Find Friends")
        case 15:
            print("Job")
            let Storyboard = UIStoryboard(name: "Jobs", bundle: nil)
            let vc = Storyboard.instantiateViewController(withIdentifier: "AllJobsVC") as! AllJobsController
//            self.navigationController?.pushViewController(vc, animated: true)
            
        case 16:
            let storyboard = UIStoryboard(name: "Funding", bundle: nil)
                      let vc = storyboard.instantiateViewController(withIdentifier: "ShowFundingsVC") as! ShowFundingsVC
                      self.navigationController?.pushViewController(vc, animated: true)
             print("Funding")
            print("Common Things")
        case 17:
            let storyboard = UIStoryboard(name: "Offers", bundle: nil)
                                let vc = storyboard.instantiateViewController(withIdentifier: "GetOffersVC") as! GetOffersVC
                                self.navigationController?.pushViewController(vc, animated: true)
            print("Funding")
        case 18:
            print("Games")
        case 19:
            print("General Account")
            let storyboard = UIStoryboard(name: "General", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "GeneralVC") as! GeneralVC
            self.navigationController?.pushViewController(vc, animated: true)
        case 20:
            print("Privacy")
            let storyboard = UIStoryboard(name: "Privacy", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "PrivacyVC") as! PrivacyVC
            self.navigationController?.pushViewController(vc, animated: true)
        case 21:
            print("Notifications")
            let storyboard = UIStoryboard(name: "Notification", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "NotificationCheckVC") as! NotificationCheckVC
            self.navigationController?.pushViewController(vc, animated: true)
        case 22:
            print("Tell a Friends")
            let storyboard = UIStoryboard(name: "TellFriend", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "TellFriendVC") as! TellFriendVC
            self.navigationController?.pushViewController(vc, animated: true)
        case 23:
            print("Help & Support")
            let storyboard = UIStoryboard(name: "HelpSupport", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "HelpAndSupportVC") as! HelpAndSupportVC
            self.navigationController?.pushViewController(vc, animated: true)
        case 24:
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
            print("Unknown Button")
            return
        }
    }

    
    
    
    
}

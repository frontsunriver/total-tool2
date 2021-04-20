//
//  MyInformationController.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/11/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import ZKProgressHUD

class MyInformationController: UIViewController {
    
    @IBOutlet weak var userImage: Roundimage!
    @IBOutlet weak var userName: UILabel!
    @IBOutlet weak var textLbl: UILabel!
    @IBOutlet weak var myInfoLbl: UILabel!
    @IBOutlet weak var postLbl: UILabel!
    @IBOutlet weak var groupsLbl: UILabel!
    @IBOutlet weak var pageLbl: UILabel!
    @IBOutlet weak var followingLbl: UILabel!
    @IBOutlet weak var followersLbl: UILabel!
    @IBOutlet weak var linkViewBtn: RoundButton!
    
    let status = Reach().connectionStatus()

    var link = ""
    var title_text = ""
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.linkViewBtn.isHidden = true
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.title = "Download My Information"
        self.navigationItem.largeTitleDisplayMode = .never
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
         NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        let imageUrl = AppInstance.instance.profile?.userData?.avatar ?? ""
        let url = URL(string: imageUrl)
        self.userImage.kf.setImage(with: url)
        if let user_name = AppInstance.instance.profile?.userData?.name {
            self.userName.text = user_name
        }
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    private func downloadLink(data: String){
        switch status {
        case .unknown, .offline:
            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            ZKProgressHUD.show()
            DownloadInfoManager.sharedInstance.download_Info(data: data) { (success, authError, error) in
                if (success != nil){
                    ZKProgressHUD.dismiss()
                    self.view.makeToast(success!.message)
                    self.linkViewBtn.isHidden = false
                    self.link = success!.link
                    print(self.link)
                }
                else if (authError != nil){
                    ZKProgressHUD.dismiss()
                    self.view.makeToast(authError?.errors?.errorText)
                }
                else if (error != nil){
                    ZKProgressHUD.dismiss()
                    self.view.makeToast(error?.localizedDescription)
                }
            }
            
        }
    }
    
    @IBAction func DownloadBtn(_ sender: UIButton) {
        switch sender.tag{
        case 0:
            self.downloadLink(data: "my_information")
            self.title_text = "My Information"
        case 1:
            self.downloadLink(data: "posts")
            self.title_text = "posts"
        case 2:
            self.downloadLink(data: "pages")
            self.title_text = "Pages"
        case 3:
            self.downloadLink(data: "groups")
            self.title_text = "Groups"
        case 4:
            self.downloadLink(data: "following")
            self.title_text = "Folllowing"
        case 5:
            self.downloadLink(data: "followers")
            self.title_text = "Followers"


        default:
            print("Nothing")
        }

        
    }
    
    @IBAction func DownloadViewBtn(_ sender: Any) {
        
        let StoryBoard = UIStoryboard(name: "MoreSection2", bundle: nil)
        let vc = StoryBoard.instantiateViewController(withIdentifier: "ShowInfoVC") as! ShowMyInformation
        
        vc.navTitle = self.title_text
        vc.urlString = self.link
        print(vc.navTitle)
        print(vc.urlString)

        self.navigationController?.pushViewController(vc, animated: true)
        
    }
    
}

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

import ZKProgressHUD

struct datas{
    let status : String
    let image : UIImage?
}

class ViewController: UIViewController,UITableViewDelegate,UITableViewDataSource {
    
    @IBOutlet weak var tableView: UITableView!
    
    var array = [datas]()
    
    
    var newsFeedArray = [[String:Any]]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        
NotificationCenter.default.addObserver(self, selector: #selector(ViewController.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
              Reach().monitorReachabilityChanges()
       // ZKProgressHUD.show("Loading")
        
      
        
        
        tableView.register(UINib(nibName: "NewsFeedCell", bundle: nil), forCellReuseIdentifier: "NewsFeedCell")
        tableView.register(UINib(nibName: "MusicCell", bundle: nil), forCellReuseIdentifier: "musicCell")
    //self.getNewsFeed(access_token: "?access_token=f86e0a1580afed8fba872189158964c62cd358812f0033500b23126db4ea2be3bf7c42e54305342331f81674a348511b990af268ca3a8391")
    }
    
    
    /////////////////////////NetWork Connection//////////////////////////
       @objc func networkStatusChanged(_ notification: Notification) {
           if let userInfo = notification.userInfo {
               let status = userInfo["Status"] as! String
               print(status)
               
           }
           
       }
    
    
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if section == 0 {
            return 1
        }
        else {
            return self.newsFeedArray.count
        }
    }
    func numberOfSections(in tableView: UITableView) -> Int {
        return 2
    }
    
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
           if indexPath.section == 0{
    let cell = tableView.dequeueReusableCell(withIdentifier: "StroiesCell") as! StoriesCell
            self.tableView.rowHeight = 100
            return cell
       
        }
           else  {
            let index = self.newsFeedArray[indexPath.row]
//
            var cellIdentefier = ""
            var tableViewCells = UITableViewCell()
            if let postfile = index["postFile"] as? String  {
                
                if postfile != ""{
                    let url = URL(string: postfile)
                    let urlExtension: String? = url?.pathExtension
                    if (urlExtension == "jpg" || urlExtension == "png" || urlExtension == "jpeg"){
                    let cell = tableView.dequeueReusableCell(withIdentifier: "NewsFeedCell") as! NewsFeedCell
                    self.tableView.rowHeight = UITableView.automaticDimension
                    self.tableView.estimatedRowHeight = UITableView.automaticDimension
                        cell.videoView.isHidden = true
                    if let time = index["postTime"] as? String{
                    cell.timeLabel.text! = time
                        }
                    if let textStatus = index["postText"] as? String {
                        cell.statusLabel.text! = textStatus.htmlToString
                        }
                        if let name = index["publisher"] as? [String:Any] {
                            if let profilename = name["name"] as? String{
                                cell.profileName.text! = profilename
                            }
                            if let avatarUrl =  name["avatar"] as? String {
                                let url = URL(string: avatarUrl)
                                 cell.profileImage.kf.setImage(with: url)
                                
                            }
                        }
                  

                cell.stausimage.sd_setImage(with: url, placeholderImage: nil, options: []) {[weak self] (image, error, casheh, url) in
                    if image != nil {
                    DispatchQueue.main.async {
                    let ratio = image!.size.width / image!.size.height
                    let newHeight = cell.stausimage.frame.width / ratio
                    cell.heigthConstraint.constant = newHeight
                                           
                cell.stausimage.frame.size = CGSize(width: cell.contentView.frame.width, height:cell.heigthConstraint.constant)
                    }
                                            
                    }
                        
                    else {
                        cell.heigthConstraint.constant = 0
                    }
                    
                }
                        cellIdentefier = "NewsFeedCell"
                        tableViewCells = cell
                   }
    else if( urlExtension == ".wav" ||  urlExtension == ".mp3"){
let cell = tableView.dequeueReusableCell(withIdentifier: "musicCell") as! MusicCell

                if let name = index["publisher"] as? [String:Any] {
                        if let profilename = name["name"] as? String{
                            cell.nameLabel.text! = profilename
                        }
                        if let avatarUrl =  name["avatar"] as? String {
                        let url = URL(string: avatarUrl)
                        cell.avatarImage.kf.setImage(with: url)
                            
                        }
                    }
                    if let time = index["postTime"] as? String{
                        cell.timeLabel.text! = time
                        }
                        
                    if let textStatus = index["postText"] as? String {
                    cell.statusLabel.text! = textStatus.htmlToString
                        }
                       
                    cell.loadMp3(url: postfile)
                    self.tableView.rowHeight = 200
                    cellIdentefier = "musicCell"
                    tableViewCells = cell
                    }
                    
                  
                }
                else {
        let cell = tableView.dequeueReusableCell(withIdentifier: "NewsFeedCell") as! NewsFeedCell

            if let name = index["publisher"] as? [String:Any] {
                if let profilename = name["name"] as? String{
                    cell.profileName.text! = profilename
                }
                
            if let avatarUrl =  name["avatar"] as? String {
                let url = URL(string: avatarUrl)
                cell.profileImage.kf.setImage(with: url)
                            
                }
                   }
               if let time = index["postTime"] as? String{
                cell.timeLabel.text! = time
                  }
                if let textStatus = index["postText"] as? String {
                cell.statusLabel.text! = textStatus.htmlToString
                        
                  }
                    
                    cell.videoView.isHidden = true
                    cell.heigthConstraint.constant = 0
                    self.tableView.rowHeight = UITableView.automaticDimension
                    self.tableView.estimatedRowHeight = UITableView.automaticDimension
                    cellIdentefier = "NewsFeedCell"
                    tableViewCells = cell
                }
            
                
            }
            
            

            return tableViewCells
                 
        }
        
     
       }

//    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
//        if indexPath.section  == 0 {
//            return 100
//        }
//        else{
//            return 414
////           return UITableView.automaticDimension
////            self.tableView.estimatedRowHeight = UITableView.automaticDimension
//
//
//
//        }
//
//    }
//
//    func tableView(_ tableView: UITableView, estimatedHeightForRowAt indexPath: IndexPath) -> CGFloat {
//        if indexPath.section  == 0 {
//            return 100
//        }
//        else{
////           self.tableView.rowHeight = UITableView.automaticDimension
//
////            UITableView.automaticDimension
//
//
//
//            }
//    }
    

    private func getNewsFeed (access_token : String) {
    
    let status = Reach().connectionStatus()
           switch status {
           case .unknown, .offline:
       
            showAlert(title: "", message: "Internet Connection Failed")
               
           case .online(.wwan), .online(.wiFi):
            GetNewsFeedManagers.sharedInstance.get_News_Feed(access_token: access_token) {[weak self] (success, authError, error) in
                if success != nil {
                    for i in success!.data{
                        self?.newsFeedArray.append(i)
                        
                    }

                    self?.tableView.reloadData()
                    ZKProgressHUD.dismiss()
                    print(self?.newsFeedArray.count)
                    
                }
                else if authError != nil {
                    ZKProgressHUD.dismiss()
                    self?.showAlert(title: "", message: (authError?.errors.errorText)!)

                }
                
                
                
                else if error  != nil {
                    ZKProgressHUD.dismiss()
                    self?.showAlert(title: "", message: "InternalError")
                }
            }
           
    
    
    
    
    }
    
    
    
    
}

  }

extension String {
    var htmlToAttributedString: NSAttributedString? {
        guard let data = data(using: .utf8) else { return NSAttributedString() }
        do {
            return try NSAttributedString(data: data, options: [.documentType: NSAttributedString.DocumentType.html, .characterEncoding:String.Encoding.utf8.rawValue], documentAttributes: nil)
        } catch {
            return NSAttributedString()
        }
    }
    var htmlToString: String {
        return htmlToAttributedString?.string ?? ""
    }
}

//
//  CommonThingsVC.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/17/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import AVFoundation
import WoWonderTimelineSDK
import XLPagerTabStrip
class CommonThingsVC: UIViewController {
    
    @IBOutlet weak var collectionView: UICollectionView!
    @IBOutlet weak var noVideoView: UIView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    var commonThingsArray = [[String:Any]]()
    let status = Reach().connectionStatus()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("Common Things", comment: "Common Things")
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        //        self.collectionView.register(UINib(nibName: "MyVideosCell", bundle: nil), forCellWithReuseIdentifier: "MyVideosCell")
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
        //        self.activityIndicator.startAnimating()
        
        
        self.getCommonThings()
    }
 
    
    private func getCommonThings() {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                CommonThingsManager.sharedInstance.getCommonThings(limit: 0, offset: 0) { (success, sessionError, error) in
                    if success != nil {
                        //
                        //                        for i in success!.data{
                        //                                                self.myVideos.append(i)
                        //                                            }
                        self.commonThingsArray = success?.data ?? []
                        //                        self.myVideos = success!.data.map({$0})
                        if self.commonThingsArray.count != 0{
                            self.collectionView.isHidden = false
                            self.noVideoView.isHidden = true
                            
                        }
                        else{
                            self.collectionView.isHidden = true
                            self.noVideoView.isHidden = false
                        }
                        self.activityIndicator.stopAnimating()
                        self.collectionView.reloadData()
                    }
                    else if sessionError != nil {
                        self.view.makeToast(sessionError?.errors?.errorText)
                    }
                    else if error != nil {
                        print(error?.localizedDescription)
                    }
                }
                
            }
        }
    }
    
    @IBAction func Back(_ sender: Any) {
        self.navigationController?.popViewController(animated: true)
    }
    
}

extension CommonThingsVC : UICollectionViewDelegate,UICollectionViewDataSource{
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.commonThingsArray.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "CommonThingsCollectionCell", for: indexPath) as! CommonThingsCollectionCell
        let object = self.commonThingsArray[indexPath.row]
        cell.bind(object: object)
        return cell
    }
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
           let storyBoard = UIStoryboard(name: "Main", bundle: nil)
             let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
             let index = self.commonThingsArray[indexPath.row]
        let value = index["user_data"] as? [String:Any]
             vc.userData = ["user_id":index["user_id"] as? String,"name":value!["username"] as? String,"avatar":value!["avatar"] as? String,"cover":value!["cover"] as? String,"points":value!["points"] as? Int,"verified":value!["verified"] as? String ,"is_pro":value!["is_pro"] as? String ]
             self.navigationController?.pushViewController(vc, animated: true)
    }
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        let padding: CGFloat =  50
        let collectionViewSize = collectionView.frame.size.width - padding
        return CGSize(width: collectionViewSize/2, height: collectionViewSize/2)
    }
    
    
}

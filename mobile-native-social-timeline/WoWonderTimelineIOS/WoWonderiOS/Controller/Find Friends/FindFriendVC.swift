//
//  FindFriendVC.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/13/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import Toast_Swift
import AVFoundation
import WoWonderTimelineSDK

class FindFriendVC: UIViewController {
    
    @IBOutlet weak var collectionView: UICollectionView!
    @IBOutlet weak var noVideoView: UIView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    var nearByFriends = [[String:Any]]()
    let status = Reach().connectionStatus()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.largeTitleDisplayMode = .never
//        self.navigationController?.navigationItem.title = "My Videos"
        self.navigationItem.title = NSLocalizedString("Find Friends", comment: "Find Friends")
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        //        self.collectionView.register(UINib(nibName: "MyVideosCell", bundle: nil), forCellWithReuseIdentifier: "MyVideosCell")
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
        //        self.activityIndicator.startAnimating()
        
        
        self.getNearByUsers(gender: "All", statusRe: "All", distance: 0, relationship: "All")
    }
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        self.tabBarController?.tabBar.isHidden = true
    }

    override func viewDidDisappear(_ animated: Bool) {
        super.viewDidDisappear(animated)
         self.tabBarController?.tabBar.isHidden = false
    }
    @IBAction func filterPressed(_ sender: Any) {
           let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "FilterFriendsVC") as! FilterFriendsVC
           vc.modalTransitionStyle = .crossDissolve
           vc.modalPresentationStyle = .overCurrentContext
        vc.delegate = self
           self.present(vc, animated: true, completion: nil)
    }
    
    private func getNearByUsers(gender:String,statusRe:String,distance:Int,relationship:String) {
        self.nearByFriends.removeAll()
        self.collectionView.reloadData()
        self.activityIndicator.startAnimating()
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                NearByUsersManager.sharedInstance.getNearByUsers(limit: 10, offset: 0, gender: gender, keyword: "", status: statusRe, distance: distance, relationship: relationship) { (success, sessionError, error) in
                    if success != nil {
                        //
                        //                        for i in success!.data{
                        //                                                self.myVideos.append(i)
                        //                                            }
                        self.nearByFriends = success?.data ?? []
                        //                        self.myVideos = success!.data.map({$0})
                        if self.nearByFriends.count != 0{
                            self.collectionView.isHidden = false
                        }
                        else{
                            self.collectionView.isHidden = true
                            //                            self.noVideoView.isHidden = false
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

extension FindFriendVC : UICollectionViewDelegate,UICollectionViewDataSource,UICollectionViewDelegateFlowLayout{
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.nearByFriends.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "FindFriendCollectionCell", for: indexPath) as! FindFriendCollectionCell
        let object = self.nearByFriends[indexPath.row]
        cell.vc = self
        cell.bind(object: object)
        return cell
    }
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        let collectionViewSize = collectionView.frame.size.width - 10
        return CGSize(width: collectionViewSize/2, height: 220.0)
    }
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, minimumLineSpacingForSectionAt section: Int) -> CGFloat {
        return 0
    }
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, minimumInteritemSpacingForSectionAt section: Int) -> CGFloat {
        return 0
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        let index = self.nearByFriends[indexPath.item]
        let storyBoard = UIStoryboard(name: "Main", bundle: nil)
        let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
        vc.userData = index
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
}

extension FindFriendVC : filterFriendsDelegate{
    func filterFriends(gender: String, distenace: Int, status: String, relationship: String) {
        self.getNearByUsers(gender: gender, statusRe: status, distance: distenace, relationship: relationship)
    }
    
   
}

//
//  MyGamesVC.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/15/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import XLPagerTabStrip
import AVFoundation
import WoWonderTimelineSDK
import XLPagerTabStrip
class MyGamesVC: UIViewController {
    
    @IBOutlet weak var collectionView: UICollectionView!
    @IBOutlet weak var noVideoView: UIView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    var gamesArray = [[String:Any]]()
    let status = Reach().connectionStatus()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.largeTitleDisplayMode = .never
//        self.navigationController?.navigationItem.title = "My Videos"
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        //        self.collectionView.register(UINib(nibName: "MyVideosCell", bundle: nil), forCellWithReuseIdentifier: "MyVideosCell")
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
        //        self.activityIndicator.startAnimating()
        
        
        self.getNearByUsers()
    }
    
    
    @IBAction func filterPressed(_ sender: Any) {
        //        let vc = Storyboard.instantiateViewController(withIdentifier: "SearchFilterVC") as! SearchFilterController
        //           vc.modalTransitionStyle = .crossDissolve
        //           vc.modalPresentationStyle = .overCurrentContext
        //           self.present(vc, animated: true, completion: nil)
    }
    
    private func getNearByUsers() {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GamesManager.sharedInstance.getGames(limit: 0, offset: 0, type: "get_my") { (success, sessionError, error) in
                    if success != nil {
                        //
                        //                        for i in success!.data{
                        //                                                self.myVideos.append(i)
                        //                                            }
                        self.gamesArray = success?.data ?? []
                        //                        self.myVideos = success!.data.map({$0})
                        if self.gamesArray.count != 0{
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

extension MyGamesVC : UICollectionViewDelegate,UICollectionViewDataSource,UICollectionViewDelegateFlowLayout{
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.gamesArray.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "GamesCollectionCell", for: indexPath) as! GamesCollectionCell
        let object = self.gamesArray[indexPath.row]
        cell.myGamesVc = self
        cell.bind(object: object)
        return cell
    }

    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        let collectionViewSize = collectionView.frame.size.width - 10
        return CGSize(width: collectionViewSize/2, height: 255.0)
    }
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, minimumLineSpacingForSectionAt section: Int) -> CGFloat {
        return 0
    }
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, minimumInteritemSpacingForSectionAt section: Int) -> CGFloat {
        return 0
    }
   
    
}
extension MyGamesVC:IndicatorInfoProvider{
    func indicatorInfo(for pagerTabStripController: PagerTabStripViewController) -> IndicatorInfo {
        return IndicatorInfo(title: NSLocalizedString("MY GAMES", comment: "MY GAMES"))
    }
}

//
//  JobsVC.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/21/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import AVFoundation
import WoWonderTimelineSDK
import XLPagerTabStrip
class JobsVC: UIViewController {
    
    @IBOutlet weak var collectionView: UICollectionView!
    @IBOutlet weak var noVideoView: UIView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    var jobsArray = [[String:Any]]()
    let status = Reach().connectionStatus()
    lazy   var searchBar:UISearchBar = UISearchBar(frame: CGRect(x: 0, y: 0, width: 200, height: 20))
    var filteredData =  [[String:Any]]()
    var distance:Int? = 0


    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.largeTitleDisplayMode = .never
     
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        //        self.collectionView.register(UINib(nibName: "MyVideosCell", bundle: nil), forCellWithReuseIdentifier: "MyVideosCell")
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
        //        self.activityIndicator.startAnimating()
       let btn1 = UIButton(type: .custom)
           btn1.setImage(UIImage(named: "levels"), for: .normal)
           btn1.frame = CGRect(x: 0, y: 0, width: 30, height: 30)
           btn1.addTarget(self, action: #selector(self.filter), for: .touchUpInside)
           let item1 = UIBarButtonItem(customView: btn1)

          

           self.navigationItem.setRightBarButtonItems([item1], animated: true)
        searchBar.placeholder = "Search"
        searchBar.delegate = self
        var leftNavBarButton = UIBarButtonItem(customView:searchBar)
        
        self.navigationItem.titleView =  searchBar
        self.getCommonThings()
    }
    override func viewWillAppear(_ animated: Bool) {
         super.viewWillAppear(animated)
         self.tabBarController?.tabBar.isHidden = true
     }
    
    @IBAction func NearByBusinessPressed(_ sender: Any) {
        let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
                           let vc = storyboard.instantiateViewController(withIdentifier: "NearByBusinessVC") as! NearByBusinessVC
                           self.navigationController?.pushViewController(vc, animated: true)
    }
    
    @objc func filter(){
      
        let vc = UIStoryboard(name: "MoreSection", bundle: nil).instantiateViewController(withIdentifier: "FilterJobsVC") as! FilterJobsVC
        
           vc.delegate = self
//           vc.distance = "\(self.distance)"
           vc.modalPresentationStyle = .overCurrentContext
           vc.modalTransitionStyle = .crossDissolve
           self.present(vc, animated: true, completion: nil)
    }
    
    private func getCommonThings() {
        self.jobsArray.removeAll()
        self.collectionView.reloadData()
         self.activityIndicator.startAnimating()
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                JobsManager.sharedInstance.getJobs(Type: "search", limit: 10, offset: 0,distance:self.distance ?? 0) { (success, sessionError, error) in
                    if success != nil {
                        //
                        //                        for i in success!.data{
                        //                                                self.myVideos.append(i)
                        //                                            }
                        self.jobsArray = success?.data ?? []
                        self.filteredData = self.jobsArray
                        //                        self.myVideos = success!.data.map({$0})
                        if self.jobsArray.count != 0{
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

extension JobsVC : UICollectionViewDelegate,UICollectionViewDataSource,UICollectionViewDelegateFlowLayout{
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.filteredData.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "JobVCCollectionCell", for: indexPath) as! JobVCCollectionCell
        if self.filteredData.count == 0{
            return UICollectionViewCell()
        }else{
            let object = self.filteredData[indexPath.row]
                   cell.vc = self
                   cell.bind(object: object)
                   return cell
        }
       
    }
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        let storyboard = UIStoryboard(name: "Jobs", bundle: nil)
//        let vc = storyboard.instantiateViewController(withIdentifier: "JobDetailsVC") as! JobDetailsVC
//        vc.object = self.jobsArray[indexPath.row]
//        self.navigationController?.pushViewController(vc, animated: true)
        
          let index = jobsArray[indexPath.row]
//          self.selectedIndex = indexPath.row
          let vc = storyboard.instantiateViewController(withIdentifier: "JobDetailVC") as! JobDetailController
          if let postId = self.jobsArray[indexPath.row]["post_id"] as? String {
          vc.postId = postId
          }
          vc.jobData = index
//          vc.delegate = self
//          vc.deleteDelegate = self
          self.navigationController?.pushViewController(vc, animated: true)
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        let padding: CGFloat =  50
        let collectionViewSize = collectionView.frame.size.width - 10
        return CGSize(width: collectionViewSize, height: 320.0)
    }
    
    
}
extension JobsVC:UISearchBarDelegate{
    func searchBarCancelButtonClicked(_ searchBar: UISearchBar) {
      
    }
    func searchBar(_ searchBar: UISearchBar, textDidChange searchText: String) {
   
        filteredData = searchText.isEmpty ? jobsArray : jobsArray.filter({(dataString: [String:Any]) -> Bool in
            let string = dataString["title"] as? String
            return string?.range(of: searchText, options: .caseInsensitive) != nil
        })
        collectionView.reloadData()
    }
}
extension JobsVC:ProductDistanceDelegate{
    func productDistance(distance: Int) {
        print("Distance \(distance)")
        self.distance = distance
        self.getCommonThings()
    }
    
    
   
}



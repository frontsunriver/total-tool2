//
//  MoviesVC.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/23/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

import AVFoundation
import WoWonderTimelineSDK
import XLPagerTabStrip
class MoviesVC: UIViewController {
    
    @IBOutlet weak var tablView: UITableView!
    @IBOutlet weak var noVideoView: UIView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    var moviesArray = [[String:Any]]()
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
        self.navigationItem.title = NSLocalizedString("Movies", comment: "Movies")
        //        self.collectionView.register(UINib(nibName: "MyVideosCell", bundle: nil), forCellWithReuseIdentifier: "MyVideosCell")
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
                self.activityIndicator.startAnimating()
        
        self.getMovies(genre: "Action")
        self.tablView.separatorStyle = .none
        self.tablView.register(UINib(nibName: "MoviesTableItem", bundle: nil), forCellReuseIdentifier: "MoviesTableItem")
    }
    private func getMovies(genre:String) {
        self.moviesArray.removeAll()
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                MoviesManager.sharedInstance.getMovies(limit: 10, offset: 0, genre: genre) { (success, sessionError, error) in
                    if success != nil {
                        //
                        //                        for i in success!.data{
                        //                                                self.myVideos.append(i)
                        //                                            }
                        self.moviesArray = success?.data ?? []
                        //                        self.myVideos = success!.data.map({$0})
                        if self.moviesArray.count != 0{
                            self.tablView.isHidden = false
                            self.noVideoView.isHidden = true
                            
                        }
                        else{
                            self.tablView.isHidden = true
                            self.noVideoView.isHidden = false
                        }
                        self.activityIndicator.stopAnimating()
                        self.tablView.reloadData()
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
    
    
    @IBAction func filterMoviesPressed(_ sender: Any) {
        let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
              let vc = storyboard.instantiateViewController(withIdentifier: "SelectDateVC") as! SelectDateVC
        
              vc.delegate = self
              vc.type = 3
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func Back(_ sender: Any) {
        self.navigationController?.popViewController(animated: true)
    }
    
}

extension MoviesVC : UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.moviesArray.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "MoviesTableItem") as! MoviesTableItem
        if self.moviesArray.count == 0{
            return UITableViewCell()
        }else{
            let object = self.moviesArray[indexPath.row]
            cell.bind(object: object)
            cell.vc = self
            return cell
        }
    }
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
         let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
                             let vc = storyboard.instantiateViewController(withIdentifier: "VideoDetailsVC") as! VideoDetailsVC
        vc.url = self.moviesArray[indexPath.row]["source"] as? String
        vc.object = self.moviesArray[indexPath.row]
         self.navigationController?.pushViewController(vc, animated: true)
    }
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 120.0
    }
    
}
extension MoviesVC:SelectYearDelegate{
    func selectYear(year: String, index: Int, type: Int) {
         self.getMovies(genre: year)
       
    }
    
}

//
//  IntrestedEventController.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/9/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import XLPagerTabStrip
import WoWonderTimelineSDK

class IntrestedEventController: UIViewController,IndicatorInfoProvider {

    @IBOutlet weak var noEventView: UIView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    @IBOutlet weak var collectionView: UICollectionView!
    
    let status = Reach().connectionStatus()
    let pulltoRefresh = UIRefreshControl()
    let Stroyboard =  UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
    
    var intrestedEvents = [[String:Any]]()
    var offSet = ""
    var selectedIndex = 0
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
        self.activityIndicator.startAnimating()
        self.collectionView.delegate = self
        self.collectionView.dataSource = self
        self.collectionView.register(UINib(nibName: "EventCollectionCell", bundle: nil), forCellWithReuseIdentifier: "EventCollectionCells")
        self.collectionView.showsVerticalScrollIndicator = false
        self.collectionView.showsHorizontalScrollIndicator = false
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.collectionView.addSubview(pulltoRefresh)
        self.noEventView.isHidden = true
        self.getIntrestedEvents(myOffset: self.offSet)
        
    }
    
    
    
    @objc func refresh(){
        self.offSet = ""
        self.intrestedEvents.removeAll()
        self.collectionView.reloadData()
        self.getIntrestedEvents(myOffset: self.offSet)
    }
    
    private func getIntrestedEvents (myOffset :String){
        switch status {
        case .unknown, .offline:
            self.activityIndicator.stopAnimating()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetMyEventManager.sharedInstance.getMyEvents(fetch: "interested", myoffset: self.offSet) { (success, authError, error) in
                    if success != nil {
                        for i in success!.interested{
                            self.intrestedEvents.append(i)
                        }
                        print(self.intrestedEvents.count)
                        if self.intrestedEvents.count == 0 {
                            self.noEventView.isHidden = false
                            self.collectionView.isHidden = true
                        }
                        self.pulltoRefresh.endRefreshing()
                        self.offSet = self.intrestedEvents.last?["id"] as? String ?? ""
                        self.activityIndicator.stopAnimating()
                        self.collectionView.reloadData()
                    }
                    else if authError != nil {
                        self.activityIndicator.stopAnimating()
                        self.pulltoRefresh.endRefreshing()
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        self.activityIndicator.stopAnimating()
                        self.pulltoRefresh.endRefreshing()
                        print(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    @IBAction func CreateOwnEvent(_ sender: Any) {
        let vc = self.Stroyboard.instantiateViewController(withIdentifier: "CreateEventVC") as! CreateEventController
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.present(vc, animated: true, completion: nil)
    }
    
    
    @IBAction func AddEvent(_ sender: Any) {
        let vc = self.Stroyboard.instantiateViewController(withIdentifier: "CreateEventVC") as! CreateEventController
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.present(vc, animated: true, completion: nil)
    }
    
    func indicatorInfo(for pagerTabStripController: PagerTabStripViewController) -> IndicatorInfo {
        return IndicatorInfo(title: NSLocalizedString("INTRESTED", comment: "INTRESTED"))
    }
    
    
    
}

extension IntrestedEventController: UICollectionViewDataSource,UICollectionViewDelegate,UICollectionViewDelegateFlowLayout,GoingEventDelegate,InterestedEventDelegate{
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.intrestedEvents.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "EventCollectionCells", for: indexPath) as! EventCollectionCell
        let index = self.intrestedEvents[indexPath.row]
        if let image = index["cover"] as? String{
            let url = URL(string: image)
            cell.eventImage.kf.setImage(with: url)
        }
        if let name = index["name"] as? String{
            cell.titleLabel.text = name
        }
        if let date = index["end_date"] as? String{
            cell.dateLabel.text  = date
        }
        if let desc = index["description"] as? String{
            cell.descLabel.text = desc
        }
        if let location = index["location"] as? String{
            cell.locationLabel.text = location
        }
        return cell
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        var index = self.intrestedEvents[indexPath.row]
        index["is_interested"] = true
        
        self.selectedIndex = indexPath.row
        let StoryBoard = UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
        let vc = StoryBoard.instantiateViewController(withIdentifier: "EventDetailVC") as! EventDetailController
        vc.delegate = self
        vc.isEventVC = true
        vc.interestEventDelegate = self
        vc.eventDetail = index
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
         return CGSize(width: self.collectionView.frame.size.width, height: 240.0)
    }
    func goingEvent(isGoing: Bool) {
    self.intrestedEvents[self.selectedIndex]["is_going"]  = isGoing
    self.collectionView.reloadData()
    }
    
    func interestedEvent(isInterested: Bool) {
        self.intrestedEvents[self.selectedIndex]["is_interested"] = isInterested
        self.collectionView.reloadData()
    }
}

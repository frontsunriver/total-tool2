
import UIKit
import XLPagerTabStrip
import Toast_Swift
import NotificationCenter
import WoWonderTimelineSDK


class AllEventsController: UIViewController,IndicatorInfoProvider {


    
    @IBOutlet weak var collectionView: UICollectionView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    @IBOutlet weak var noEventsView: UIView!
    
    let status = Reach().connectionStatus()
    let pulltoRefresh = UIRefreshControl()
    let Stroyboard =  UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
   


    
    var events = [[String:Any]]()
    var offset = ""
    var selectedIndex = 0
    
    override func viewDidLoad() {
        super.viewDidLoad()
    NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
//    NotificationCenter.default.addObserver(self,selector:#selector(self.createEvent(_:)),name: NSNotification.Name("createEvent"),object: nil)
    self.collectionView.register(UINib(nibName: "EventCollectionCell", bundle: nil), forCellWithReuseIdentifier: "EventCollectionCells")
        self.activityIndicator.startAnimating()
        self.getEvent(offset: self.offset)
        self.noEventsView.isHidden = true
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.collectionView.addSubview(pulltoRefresh)
    }
    
    @objc func refresh(){
        self.offset = ""
        self.events.removeAll()
        self.collectionView.reloadData()
        self.getEvent(offset: self.offset)
    }
    
//    @objc func createEvent (_ notification: Notification){
//        print("createEvent")
//        if let id = notification.userInfo?["EventId"] as? Int{
//            print(id)
//            self.getEventbyId(id: id)
//        }
      
//      let  id = notification.userInfo?["EventId"] as? [String: Int] ?? [:]
//        print(id)
//        self.getEventbyId(id: 0)
//        if let event_id  = id["EventId"] as? Int {
//            print(event_id)
//        }
//    }
    
    override func viewWillAppear(_ animated: Bool) {
    }
    
    private func getEventbyId(id: Int) {
        performUIUpdatesOnMain {
            GetEventManager.sharedInstance.getEvents(fetch: "events", offset: "", myoffset: "") { (success, authError, error) in
                if success != nil {
                    self.events.insert(success!.events.first!, at: 0)
                    self.activityIndicator.stopAnimating()
                    self.pulltoRefresh.endRefreshing()
                    self.collectionView.reloadData()
                }
                else if authError != nil  {
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
    
    
    private func getEvent(offset :String){
        switch status {
        case .unknown, .offline:
            self.activityIndicator.stopAnimating()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetEventManager.sharedInstance.getEvents(fetch: "events", offset: self.offset, myoffset: "") { (success, authError, error) in
                    if success != nil {
                        for i in success!.events{
                            self.events.append(i)
                        }
                        if (self.events.count == 0){
                            self.noEventsView.isHidden = false
                            self.collectionView.isHidden = true
                        }
                        self.offset = self.events.last?["id"] as? String ?? ""
                        self.activityIndicator.stopAnimating()
                        self.pulltoRefresh.endRefreshing()
                        self.collectionView.reloadData()
                    }
                    else if authError != nil  {
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
    
    private func GoingEvent(eventId: String){
        switch status {
        case .unknown, .offline:
            self.activityIndicator.stopAnimating()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GoToEventManager.sharedInstance.gotoEvent(eventId: eventId) { (success, authError, error) in
                    if success != nil {
                        print(success!.go_status)
                        if success?.go_status == "going"{
                            self.events[self.selectedIndex]["is_going"]  = true
                        }
                        else {
                            self.events[self.selectedIndex]["is_going"]  = false
                        }
                    }
                    else if authError != nil{
                        print(authError?.errors.errorText)
                    }
                    else if error != nil {
                        print(error?.localizedDescription)
                    }
                }
            }
        }
    }
    func indicatorInfo(for pagerTabStripController: PagerTabStripViewController) -> IndicatorInfo {
        return IndicatorInfo(title: "ALL EVENTS")
    }
    
    
    @IBAction func AddEvent(_ sender: Any) {
        let vc = self.Stroyboard.instantiateViewController(withIdentifier: "CreateEventVC") as! CreateEventController
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func CreateEvent(_ sender: Any) {
        let vc = self.Stroyboard.instantiateViewController(withIdentifier: "CreateEventVC") as! CreateEventController
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.present(vc, animated: true, completion: nil)
    }
    
    
}
extension AllEventsController :UICollectionViewDelegate,UICollectionViewDataSource,UICollectionViewDelegateFlowLayout,GoingEventDelegate,InterestedEventDelegate{

    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.events.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "EventCollectionCells", for: indexPath) as! EventCollectionCell
        let index = self.events[indexPath.row]
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
//     let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "AllEventCell", for: indexPath) as! GetEventCell
//        let index = self.events[indexPath.row]
//        if let image = index["cover"] as? String{
//            let url = URL(string: image)
//            cell.eventImage.kf.setImage(with: url)
//        }
//        if let name = index["name"] as? String{
//            cell.EventTitle.text! = name
//        }
//        if let startData = index["start_date"] as? String{
//            cell.eventDate.text! = startData
//        }
//        if let descrip = index["description"] as? String{
//            cell.eventDescription.text! = descrip.htmlToString
//        }
//        if let location = index["location"] as? String{
//            cell.location.text! = location
//        }
//        if let isGoing = index["is_going"] as? Bool {
//            if isGoing == false{
//                cell.isGoingBtn.setImage(#imageLiteral(resourceName: "Star"), for: .normal)
//            }
//            else {
//                cell.isGoingBtn.setImage(#imageLiteral(resourceName: "Fillsstar"), for: .normal)
//            }
//        }
//        cell.isGoingBtn.tag = indexPath.row
//        cell.isGoingBtn.addTarget(self, action: #selector(self.GoingEvent(sender:)), for: .touchUpInside)
//        return cell
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        let index = self.events[indexPath.row]
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
//        if (indexPath.item == 0 || indexPath.item % 7==0){
//            return CGSize(width: collectionView.frame.size.width, height: 305.0)
//        }
//        else{
//            let padding: CGFloat = 10
//            let collectionViewSize = collectionView.frame.size.width - padding
//            return CGSize(width: collectionViewSize/2, height: 305.0)
//        }
    }
    
    func goingEvent(isGoing: Bool) {
    self.events[self.selectedIndex]["is_going"]  = isGoing
    self.collectionView.reloadData()
    }
    
    func interestedEvent(isInterested: Bool) {
        self.events[self.selectedIndex]["is_interested"] = isInterested
        self.collectionView.reloadData()
    }
    
    @IBAction func GoingEvent(sender:UIButton){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
        let index = sender.tag
        self.selectedIndex = index
        if let isGoing = self.events[index]["is_going"] as? Bool {
            if isGoing == false{
                sender.setImage(#imageLiteral(resourceName: "Fillsstar"), for: .normal)
                self.events[self.selectedIndex]["is_going"]  = true
            }
            else {
                sender.setImage(#imageLiteral(resourceName: "Star"), for: .normal)
                self.events[self.selectedIndex]["is_going"]  = false
            }
        }
        
        if let eventId = self.events[index]["id"] as? String {
            self.GoingEvent(eventId: eventId)
        }
        
        self.collectionView.reloadData()
    }
  }
}

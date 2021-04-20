
import UIKit
import XLPagerTabStrip
import WoWonderTimelineSDK


class MyEventsController: UIViewController,IndicatorInfoProvider {
    
    @IBOutlet weak var collectionView: UICollectionView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    @IBOutlet weak var noEventView: UIView!
    
    
    let status = Reach().connectionStatus()
    let pulltoRefresh = UIRefreshControl()
    let Stroyboard =  UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
    
    var myEvents = [[String:Any]]()
    var myoffSet = ""
    var selectedIndex = 0
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
        NotificationCenter.default.addObserver(self,selector:#selector(self.createEvent(_:)),name: NSNotification.Name("createEvent"),object: nil)
        NotificationCenter.default.addObserver(self,selector:#selector(self.EditEvent(_:)),name: NSNotification.Name("EditEvent"),object: nil)
    self.collectionView.register(UINib(nibName: "EventCollectionCell", bundle: nil), forCellWithReuseIdentifier: "EventCollectionCells")
        self.activityIndicator.startAnimating()
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.collectionView.addSubview(pulltoRefresh)
        self.noEventView.isHidden = true
        self.getMyEvents(myOffset: self.myoffSet)
        
    }

    @objc func EditEvent (_ notification: Notification){
        print("createEventMy")
        let cell = self.collectionView.cellForItem(at: IndexPath(item: self.selectedIndex, section: 0)) as! EventCollectionCell
        if let data = notification.userInfo?["event_data"] as? [String:Any]{
            print(data)
            if let name = data["event_name"] as? String{
                cell.titleLabel.text = name
            }
            if let desc = data["description"] as? String{
                cell.descLabel.text = desc
            }
            if let endDate = data["end_date"] as? String{
                cell.dateLabel.text = endDate
            }
            if let location = data["location"] as? String{
                cell.locationLabel.text = location
            }
            if let image = data["image"] as? UIImage{
                cell.eventImage.image = image
            }
        }
    }
    @objc func createEvent (_ notification: Notification){
        print("createEventMy")
        if let id = notification.userInfo?["EventId"] as? Int{
            print(id)
            self.getEventbyId(id: id)
        }
//        self.getEventbyId(id: 0)
//        let id = notification.userInfo?["userInfo"] as? [String: Int] ?? [:]
//        if let event_id  = id["EventId"] {
//            print(event_id)
//        }
    }
    
    @objc func refresh(){
        self.myoffSet = ""
        self.myEvents.removeAll()
        self.collectionView.reloadData()
        self.getMyEvents(myOffset: self.myoffSet)
    }
    
    func indicatorInfo(for pagerTabStripController: PagerTabStripViewController) -> IndicatorInfo {
        return IndicatorInfo(title: NSLocalizedString("MY EVENTS", comment: "MY EVENTS"))
    }
    
    
    
    private func getEventbyId(id: Int) {
        performUIUpdatesOnMain {
            GetMyEventManager.sharedInstance.getMyEvents(fetch: "my_events", myoffset: "") { (success, authError, error) in
                if success != nil {
                    self.myEvents.insert((success?.my_events.first)!, at: 0)
                    self.activityIndicator.stopAnimating()
                    self.collectionView.reloadData()
                }
                else if authError != nil {
                    self.activityIndicator.stopAnimating()
                    self.view.makeToast(authError?.errors.errorText)
                }
                else if error != nil {
                    self.activityIndicator.stopAnimating()
                    print(error?.localizedDescription)
                }
            }
        }
    }
    
    private func getMyEvents (myOffset :String){
        switch status {
        case .unknown, .offline:
            self.activityIndicator.stopAnimating()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetMyEventManager.sharedInstance.getMyEvents(fetch: "my_events", myoffset: self.myoffSet) { (success, authError, error) in
                    if success != nil {
                        for i in success!.my_events{
                            self.myEvents.append(i)
                        }
                        if self.myEvents.count == 0 {
                            self.noEventView.isHidden = false
                            self.collectionView.isHidden = true
                        }
                        self.myoffSet = self.myEvents.last?["id"] as? String ?? ""
                        self.activityIndicator.stopAnimating()
                        self.collectionView.reloadData()
                    }
                    else if authError != nil {
                        self.activityIndicator.stopAnimating()
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        self.activityIndicator.stopAnimating()
                        print(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    
    private func GoingEvent(eventId: String){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GoToEventManager.sharedInstance.gotoEvent(eventId: eventId) { (success, authError, error) in
                    if success != nil {
                        print(success!.go_status)
                        if success?.go_status == "going"{
                            self.myEvents[self.selectedIndex]["is_going"]  = true
                        }
                        else {
                            self.myEvents[self.selectedIndex]["is_going"]  = false
                        }
                    }
                    else if authError != nil{
                        print(authError!.errors.errorText)
                    }
                    else if error != nil {
                        print(error!.localizedDescription)
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
    
    
    
    
}
extension MyEventsController :UICollectionViewDelegate,UICollectionViewDataSource,UICollectionViewDelegateFlowLayout,GoingEventDelegate,InterestedEventDelegate{
    
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.myEvents.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "EventCollectionCells", for: indexPath) as! EventCollectionCell
        let index = self.myEvents[indexPath.row]
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

//        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "MyEventCell", for: indexPath) as! GetEventCell
//        let index = self.myEvents[indexPath.row]
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
//            cell.eventDescription.text! = descrip
//        }
//        if let location = index["location"] as? String{
//            cell.location.text! = location
//        }
//
//        if let isGoing = index["is_going"] as? Bool {
//            if isGoing == false{
//                   cell.isGoingBtn.setImage(#imageLiteral(resourceName: "Star"), for: .normal)
//            }
//            else {
//             cell.isGoingBtn.setImage(#imageLiteral(resourceName: "Fillsstar"), for: .normal)
//            }
//        }
//        cell.isGoingBtn.tag = indexPath.row
//        cell.isGoingBtn.addTarget(self, action: #selector(self.GoingEvent(sender:)), for: .touchUpInside)
//
//        return cell
    }

    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        let index = self.myEvents[indexPath.row]
        self.selectedIndex = indexPath.row
        let StoryBoard = UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
        let vc = StoryBoard.instantiateViewController(withIdentifier: "EventDetailVC") as! EventDetailController
        vc.isEventVC = true
        vc.delegate = self
        vc.interestEventDelegate = self
        vc.eventDetail = index
        vc.isMyEventVC = true
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
    
    func interestedEvent(isInterested: Bool) {
        self.myEvents[self.selectedIndex]["is_interested"] = isInterested
        self.collectionView.reloadData()
    }
    

    
    func goingEvent(isGoing: Bool) {
        self.myEvents[self.selectedIndex]["is_going"]  = isGoing
        self.collectionView.reloadData()
    }
    
    @IBAction func GoingEvent(sender:UIButton){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            let index = sender.tag
            self.selectedIndex = index
            if let isGoing = self.myEvents[index]["is_going"] as? Bool {
                if isGoing == false{
                    sender.setImage(#imageLiteral(resourceName: "Fillsstar"), for: .normal)
                    self.myEvents[self.selectedIndex]["is_going"]  = true
                }
                else {
                    sender.setImage(#imageLiteral(resourceName: "Star"), for: .normal)
                    self.myEvents[self.selectedIndex]["is_going"]  = false
                }
            }
            if let eventId = self.myEvents[index]["id"] as? String {
                self.GoingEvent(eventId: eventId)
            }
            
            self.collectionView.reloadData()
            
        }
    }
    
}

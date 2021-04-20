
//

import UIKit
import Kingfisher
import GoogleMaps
import GooglePlaces
import WoWonderTimelineSDK


class EventDetailController: UIViewController,GMSMapViewDelegate,CLLocationManagerDelegate,UIScrollViewDelegate{
    
    @IBOutlet weak var eventImage: UIImageView!
    @IBOutlet weak var eventStartDate: UILabel!
    @IBOutlet weak var eventEndDate: UILabel!
    @IBOutlet weak var goingPeopleCount: UILabel!
    @IBOutlet weak var intrestedPeopleCount: UILabel!
    @IBOutlet weak var eventLocation: UILabel!
    @IBOutlet weak var mapView:GMSMapView!
    @IBOutlet weak var eventDescription: UILabel!
    @IBOutlet weak var goBtn: RoundButton!
    @IBOutlet weak var moreBtn: RoundButton!
    @IBOutlet weak var interestedBtn: RoundButton!
    @IBOutlet weak var eventName: UILabel!
    @IBOutlet weak var scrollView: UIScrollView!
    @IBOutlet weak var contentView: UIView!
    @IBOutlet weak var heightConstraint: NSLayoutConstraint!
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var eventname: UILabel!
    @IBOutlet weak var startLabel: UILabel!
    @IBOutlet weak var endLabel: UILabel!
    @IBOutlet weak var descLbl: UILabel!
    
    var eventDetail = [String:Any]()
    var eventPosts = [[String:Any]]()
    let status = Reach().connectionStatus()
    var eventId = ""
    var offset = ""
    var addressLat = 0.0
    var addressLng = 0.0
    var isEventVC = false
    var isMyEventVC = false
    
    var delegate : GoingEventDelegate!
    var interestEventDelegate : InterestedEventDelegate!
    var locationManager = CLLocationManager()
    let marker = GMSMarker()
    let spinner = UIActivityIndicatorView(style: .gray)
    let Stroyboard =  UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)

    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.mapView.delegate = self
        self.locationManager.delegate = self
        self.scrollView.delegate = self
//        self.navigationController?.navigationBar.isHidden = true
        self.tableView.backgroundColor = .white
//        self.navigationController?.interactivePopGestureRecognizer?.isEnabled = false
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
      NotificationCenter.default.addObserver(self,selector:#selector(self.EditEvent(_:)),name: NSNotification.Name("EditEvent"),object: nil)
        Reach().monitorReachabilityChanges()
        self.tableView.isHidden = true
        //        self.heightConstraint.constant = 1220.0
        self.loadData()
        SetUpcells.setupCells(tableView: self.tableView)
        
        
    }
    
        @objc func EditEvent (_ notification: Notification){
            print("createEventMy")
            if let data = notification.userInfo?["event_data"] as? [String:Any]{
                print(data)
                if let name = data["event_name"] as? String{
                    self.eventName.text = name
                }
                if let desc = data["description"] as? String{
                    self.eventDescription.text = desc
                }
                if let startDate = data["start_date"] as? String{
                    self.eventStartDate.text = startDate
                }
                if let endDate = data["end_date"] as? String{
                    self.eventEndDate.text = endDate
                }
                if let location = data["location"] as? String{
                    self.eventLocation.text = location
                    self.forwardGeoCode(address: location)
                }
                if let image = data["image"] as? UIImage{
                    self.eventImage.image = image
                }
                
            }
        }

    override func viewWillAppear(_ animated: Bool) {
        self.navigationController?.interactivePopGestureRecognizer?.isEnabled = false
        self.navigationController?.navigationBar.isHidden = true
    AppInstance.instance.vc = "eventDetailVC"
    NotificationCenter.default.addObserver(self, selector: #selector(self.Notifire(notification:)), name: NSNotification.Name(rawValue: "Notifire"), object: nil)
//        self.goBtn.setTitle(NSLocalizedString("Go", comment: "Go"), for: .normal)
        self.interestedBtn.setTitle(NSLocalizedString("Intrested", comment: "Intrested"), for: .normal)
        self.moreBtn.setTitle(NSLocalizedString("More", comment: "More"), for: .normal)
        self.startLabel.text = NSLocalizedString("Start Date", comment: "Start Date")
        self.endLabel.text = NSLocalizedString("End Date", comment: "End Date")
        self.descLbl.text = NSLocalizedString("Description", comment: "Description")
         self.goingPeopleCount.text = "\("0")\(" ")\(NSLocalizedString("Going People", comment: "Going People"))"
        self.intrestedPeopleCount.text = "\("0")\(" ")\(NSLocalizedString("Intrested People", comment: "Intrested People"))"
    }
    
    override func viewWillDisappear(_ animated: Bool) {
        self.navigationController?.navigationBar.isHidden = false
NotificationCenter.default.removeObserver(self, name: NSNotification.Name(rawValue: "Notifire"), object: nil)
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    @objc func Notifire(notification: NSNotification){
        if let type = notification.userInfo?["type"] as? String{
            if type == "delete"{
                if let data = notification.userInfo?["userData"] as? Int{
                    print(data)
                    self.eventPosts.remove(at: data)
                    self.tableView.reloadData()
                }
            }
            if type == "profile"{
                let storyBoard = UIStoryboard(name: "Main", bundle: nil)
                let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
                var groupId: String? = nil
                var pageId: String? = nil
                var user_data: [String:Any]? = nil
                if let data = notification.userInfo?["userData"] as? Int{
                    print(data)
                    if let groupid = self.eventPosts[data]["group_id"] as? String{
                        groupId = groupid
                    }
                    if let page_Id = self.eventPosts[data]["page_id"] as? String{
                        pageId = page_Id
                    }
                    if let userData = self.eventPosts[data]["publisher"] as? [String:Any]{
                        user_data = userData
                    }
                }
                if pageId != "0"{
                    let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "PageVC") as! PageController
                    
                    vc.page_id = pageId
                    self.navigationController?.pushViewController(vc, animated: true)
                }
                else if groupId != "0"{
                    let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "GroupVC") as! GroupController
                    vc.id = groupId
                    self.navigationController?.pushViewController(vc, animated: true)
                }
                else{
                    if let id = user_data?["user_id"] as? String{
                        if id == UserData.getUSER_ID(){
                            let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
                            let vc = storyboard.instantiateViewController(withIdentifier: "MyProfileVC") as! MyProfileController
                            self.navigationController?.pushViewController(vc, animated: true)
                        }
                        else{
                            vc.userData = user_data
                            self.navigationController?.pushViewController(vc, animated: true)
                        }
                    }
                }
            }
        }
    }
    
    private func loadData(){
        
        if let eventId = self.eventDetail["id"] as? String{
            self.eventId = eventId
            self.getEventPosts(offset: self.offset, eventId: eventId)
        }
        if let eventName = self.eventDetail["name"] as? String{
            self.eventName.text = eventName
            self.eventname.text = eventName
        }
        if let eventImage = self.eventDetail["cover"] as? String{
            let url = URL(string: eventImage)
            self.eventImage.kf.setImage(with: url)
        }
        if let startDate = self.eventDetail["start_date"] as? String{
            self.eventStartDate.text! = startDate
        }
        if let endDate = self.eventDetail["end_date"] as? String{
            self.eventEndDate.text! = endDate
        }
        if let goingPeopleCount = self.eventDetail["going_count"] as? String{
            self.goingPeopleCount.text = "\(goingPeopleCount)\(" ")\(NSLocalizedString("Going People", comment: "Going People"))"
        }
        if let intrestedPeopleCount = self.eventDetail["interested_count"] as? String{
            self.intrestedPeopleCount.text = "\(intrestedPeopleCount)\(" ")\(NSLocalizedString("Intrested People", comment: "Intrested People"))"
        }
        if let location = self.eventDetail["location"] as? String{
            self.eventLocation.text = location
            self.forwardGeoCode(address: location)
            
        }
        if let eventDescription = self.eventDetail["description"] as? String{
            self.eventDescription.text = eventDescription.htmlToString
        }
        if let isGoing = self.eventDetail["is_going"] as? Bool {
            if isGoing == false {
                self.goBtn.backgroundColor = .white
                self.goBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "984243"), for: .normal)
                self.goBtn.setTitle(NSLocalizedString("Go", comment: "Go"), for: .normal)
            }
            else {
                self.goBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "984243")
                self.goBtn.setTitleColor(UIColor.white, for: .normal)
                self.goBtn.setTitle(NSLocalizedString("Going", comment: "Going"), for: .normal)
            }
            
        }
        
        if let isInterested = self.eventDetail["is_interested"] as? Bool{
            if isInterested == false {
                self.interestedBtn.backgroundColor = .white
                self.interestedBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "984243"), for: .normal)
                
            }
            else {
                self.interestedBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "984243")
                self.interestedBtn.setTitleColor(UIColor.white, for: .normal)
            }
            
        }
    }
    
    private func forwardGeoCode (address : String){
        GoogleGeoCodeManager.sharedInstance.geoCode(address: address) { (success, authError, error) in
            if success != nil {
                for i in success!.results{
                    self.addressLat = i.geometry.location.lat
                    self.addressLng = i.geometry.location.lng
                }
                print(self.addressLat,self.addressLng)
                let camera = GMSCameraPosition.camera(withTarget: CLLocationCoordinate2D(latitude:self.addressLat, longitude:self.addressLng), zoom: 9.0)
                self.mapView.camera = camera
                self.marker.position = CLLocationCoordinate2D(latitude: self.addressLat, longitude: self.addressLng)
                self.marker.title = ""
                self.marker.snippet = ""
                self.marker.map = self.mapView
            }
            else if authError != nil {
                print(authError?.errorMessage)
            }
            else if error != nil {
                print(error?.localizedDescription)
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
                        if success!.go_status == "going"{
                            self.delegate.goingEvent(isGoing: true)
                     }
                        else{
                            self.delegate.goingEvent(isGoing: false)
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
    
    
    private func interestedinEvent(eventId :String){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                InterestEventManager.sharedInstance.interestEvent(eventId: eventId) { (success, authError, error) in
                    if success != nil {
                        print(success?.interest_status)
                        if success?.interest_status == "interested"{
                            self.eventDetail["is_interested"] = true
                            if self.isEventVC == true{
                            self.interestEventDelegate.interestedEvent(isInterested: true)
                            }
                        }
                        else {
                            self.eventDetail["is_interested"] = false
                            if self.isEventVC == true{
                            self.interestEventDelegate.interestedEvent(isInterested: false)
                            }
                        }
                    }
                    else if authError != nil {
                        print(authError?.errors.errorText)
                    }
                    else if error != nil {
                        print(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    private func getEventPosts(offset: String, eventId :String){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetEventPostManager.sharedInstance.getEventPost(eventId: eventId, offset: offset) { (success, authError, error) in
                    if success != nil {
                        for i in success!.data{
                            self.eventPosts.append(i)
                        }
                        if self.eventPosts.count != 0{
                            self.tableView.isHidden = false
                        }

                        self.spinner.stopAnimating()
                        self.offset = self.eventPosts.last?["post_id"] as? String ?? ""
                        self.tableView.reloadData()
                    }
                    else if authError != nil {
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        print(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    
    
    
    @IBAction func Go(_ sender: Any) {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            if self.goBtn.backgroundColor == UIColor.white{
                self.goBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "984243")
                self.goBtn.setTitleColor(UIColor.white, for: .normal)
                self.goBtn.setTitle("Going", for: .normal)
                self.GoingEvent(eventId: self.eventId)
            }
            else {
                self.goBtn.backgroundColor = .white
                self.goBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "984243"), for: .normal)
                self.goBtn.setTitle("Go", for: .normal)
                self.GoingEvent(eventId: self.eventId)
                
            }
        }
        
    }
    
    @IBAction func Interested(_ sender: Any) {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            if self.eventDetail["is_interested"] as? Bool == false {
                self.interestedBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "984243")
                self.interestedBtn.setTitleColor(.white, for: .normal)
                self.eventDetail["is_interested"] = true
                
            }
            else {
                self.interestedBtn.backgroundColor = .white
                self.interestedBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "984243"), for: .normal)
                self.eventDetail["is_interested"] = false
            }
            
            if let eventId = self.eventDetail["id"] as? String {
                self.interestedinEvent(eventId: eventId)
            }
        }
    }
    
    @IBAction func More(_ sender: Any) {
        let StoryBoard = UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
        let vc = StoryBoard.instantiateViewController(withIdentifier: "EventMoreVC") as! EventMoreController
        if let url = self.eventDetail["url"] as? String{
            vc.copyurl = url
        }
        if let is_owner = self.eventDetail["is_owner"] as? Bool{
            vc.isOwner = is_owner
        }
        vc.event_details = self.eventDetail
        vc.isMyEventVC = self.isMyEventVC
        vc.delegate = self
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    func scrollViewDidScroll(_ scrollView: UIScrollView) {
        if scrollView == self.scrollView{
        print(scrollView.contentOffset.y)
        print(self.eventDescription.frame.minY)
        }
        else {
            print("TableView Scroll")
        }
    }
    
    @IBAction func Back(_ sender: Any) {
        self.navigationController?.popViewController(animated: true)
    }
    
}
extension EventDetailController: UITableViewDelegate,UITableViewDataSource,EditEventBtnDelegate{
    
    
    func EditButton(id: Int) {
        let vc = self.Stroyboard.instantiateViewController(withIdentifier: "CreateEventVC") as! CreateEventController
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        vc.isEdit = 1
        vc.eventDetails = self.eventDetail
        self.present(vc, animated: true, completion: nil)
    }
    
    func numberOfSections(in tableView: UITableView) -> Int {
        return 7
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if section == 0{
            return 1
        }
        else if section == 1{
            return 1
        }
        else if section == 2{
            return 1
        }
        else if section == 3{
            return 1
        }
        else if section == 4{
            return 1
        }
        else if section == 5{
            return 1
        }
        else{
        return  self.eventPosts.count
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        if (indexPath.section == 0){
            let cell = UITableViewCell()
            self.tableView.rowHeight = 0
            return cell
        }
        else if (indexPath.section == 1){
            let cell = UITableViewCell()
            self.tableView.rowHeight = 0
            return cell
        }
        else if (indexPath.section == 2){
            let cell = UITableViewCell()
            self.tableView.rowHeight = 0
            return cell
        }
        else if (indexPath.section == 3){
            let cell = UITableViewCell()
            self.tableView.rowHeight = 0
            return cell
        }
        else if (indexPath.section == 4){
            let cell = UITableViewCell()
            self.tableView.rowHeight = 0
            return cell
        }
        else if (indexPath.section == 5){
            let cell = UITableViewCell()
            self.tableView.rowHeight = 0
            return cell
        }
        else{
        let index = self.eventPosts[indexPath.row]
        var tableViewCells = UITableViewCell()
        
        let postfile = index["postFile"] as? String ?? ""
        let postLink = index["postLink"] as? String ?? ""
        let postYoutube = index["postYoutube"] as? String ?? ""
        let blog = index["blog_id"] as? String ?? "0"
        let group = index["group_recipient_exists"] as? Bool ??  false
        let product = index["product_id"] as? String ?? "0"
        let event = index["page_event_id"] as? String ?? "0"
        let postSticker = index["postSticker"] as? String ?? ""
        let colorId = index["color_id"] as? String ?? "0"
        let multi_image = index["multi_image"] as? String ?? "0"
        let photoAlbum = index["album_name"] as? String ?? ""
        let postOptions = index["poll_id"] as? String ?? "0"
        let postRecord = index["postRecord"] as? String ?? "0"
        
        if (postfile != "")  {
            let url = URL(string: postfile)
            let urlExtension: String? = url?.pathExtension
            if (urlExtension == "jpg" || urlExtension == "png" || urlExtension == "jpeg" || urlExtension == "JPG" || urlExtension == "PNG"){
        tableViewCells = GetPostWithImage.sharedInstance.getPostImage(targetController: self, tableView: self.tableView, indexpath: indexPath, postFile: postfile, array: self.eventPosts, url: url!, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
            else if(urlExtension == "wav" ||  urlExtension == "mp3" || urlExtension == "MP3"){
                tableViewCells = GetPostMp3.sharedInstance.getMP3(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.eventPosts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
            else if (urlExtension == "pdf") {
                tableViewCells = GetPostPDF.sharedInstance.getPostPDF(targetControler: self, tableView: tableView, indexpath: indexPath, postfile: postfile, array: self.eventPosts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
        else {
                tableViewCells = GetPostVideo.sharedInstance.getVideo(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.eventPosts, url: url!, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
            }
        }
        else if (postLink != "") {
            tableViewCells = GetPostWithLink.sharedInstance.getPostLink(targetController: self, tableView: tableView, indexpath: indexPath, postLink: postLink, array: self.eventPosts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        else if (postYoutube != "") {
            tableViewCells = GetPostYoutube.sharedInstance.getPostYoutub(targetController: self, tableView: tableView, indexpath: indexPath, postLink: postYoutube, array: self.eventPosts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        else if (blog != "0") {
            tableViewCells = GetPostBlog.sharedInstance.GetBlog(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.eventPosts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        else if (group != false){
            tableViewCells = GetPostGroup.sharedInstance.GetGroupRecipient(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.eventPosts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        else if (product != "0") {
            tableViewCells = GetPostProduct.sharedInstance.GetProduct(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.eventPosts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        else if (event != "0") {
            tableViewCells = GetPostEvent.sharedInstance.getEvent(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array:  self.eventPosts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        else if (postSticker != "") {
            tableViewCells = GetPostSticker.sharedInstance.getPostSticker(targetController: self, tableView: tableView, indexpath: indexPath, postFile: postfile, array: self.eventPosts,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        else if (colorId != "0"){
            tableViewCells = GetPostWithBg_Image.sharedInstance.postWithBg_Image(targetController: self, tableView: tableView, indexpath: indexPath, array: self.eventPosts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        else if (multi_image != "0") {
            tableViewCells = GetPostMultiImage.sharedInstance.getMultiImage(targetController: self, tableView: tableView, indexpath: indexPath, array: self.eventPosts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        else if photoAlbum != "" {
            tableViewCells = getPhotoAlbum.sharedInstance.getPhoto_Album(targetController: self, tableView: tableView, indexpath: indexPath, array: self.eventPosts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        else if postOptions != "0" {
            tableViewCells = GetPostOptions.sharedInstance.getPostOptions(targertController: self, tableView: tableView, indexpath: indexPath, array: self.eventPosts,stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        else if postRecord != ""{
            tableViewCells = GetPostRecord.sharedInstance.getPostRecord(targetController: self, tableView: tableView, indexpath: indexPath, array: self.eventPosts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        else {
            tableViewCells = GetNormalPost.sharedInstance.getPostText(targetController: self, tableView: tableView, indexpath: indexPath, postFile: "", array: self.eventPosts, stackViewHeight: 50.0, viewHeight: 22.0, isHidden: false, viewColor: .lightGray)
        }
        return tableViewCells
      }
    }
    
    func tableView(_ tableView: UITableView, willDisplay cell: UITableViewCell, forRowAt indexPath: IndexPath) {
        if self.eventPosts.count >= 15{
            let count = self.eventPosts.count
            let lastElement = count - 1
            
            if indexPath.row == lastElement {
                spinner.startAnimating()
                spinner.frame = CGRect(x: CGFloat(0), y: CGFloat(0), width: tableView.bounds.width, height: CGFloat(44))
                self.tableView.tableFooterView = spinner
                self.tableView.tableFooterView?.isHidden = false
                self.getEventPosts(offset: self.offset, eventId: eventId)
            }
        }
    }
}

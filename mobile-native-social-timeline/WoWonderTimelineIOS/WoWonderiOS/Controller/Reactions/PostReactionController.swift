

import UIKit
import Toast_Swift
import WoWonderTimelineSDK

class PostReactionController: UIViewController {
    
    @IBOutlet weak var collectionView: UICollectionView!
    @IBOutlet weak var reactionBtn1: UIButton!
    @IBOutlet weak var reactionBtn2: UIButton!
    @IBOutlet weak var reactionBtn3: UIButton!
    @IBOutlet weak var reactionBtn4: UIButton!
    @IBOutlet weak var reactionBtn5: UIButton!
    @IBOutlet weak var reactionBtn6: UIButton!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    let status = Reach().connectionStatus()
    
    var reaction = [String:Any]()
    var Likes = [[String:Any]]()
    var Angry = [[String:Any]]()
    var HaHa = [[String:Any]]()
    var Sad = [[String:Any]]()
    var Wow = [[String:Any]]()
    var Love = [[String:Any]]()
    
    
    var totalCount = 0
    var likeCount = 0
    var angryCount = 0
    var hahaCount = 0
    var sadCount = 0
    var wowCount = 0
    var loveCount = 0
    var postId = ""
    var is_Comment = 0
    var reaction_Type = ""
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        if self.is_Comment == 1{
            self.reaction_Type = "comment"
        }
        else{
            self.reaction_Type = "post"
        }
        activityIndicator.startAnimating()
        print(self.postId)
        
        self.getPostReaction()
        self.loadReaction()
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    
    private func getPostReaction() {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            GetPostReactionManager.sharedInstance.getReactions(type: self.reaction_Type, postID: self.postId) { (success, authError, error) in
                if success != nil {
                    if let like = success?.data["1"] as? [[String:Any]]{
                        self.Likes = like
                    }
                    if let love = success?.data["2"] as? [[String:Any]]{
                        self.Love = love
                    }
                    if let sad = success?.data["5"] as? [[String:Any]]{
                        print(sad)
                        self.Sad = sad
                    }
                    if let haha = success?.data["3"] as? [[String:Any]]{
                        self.HaHa = haha
                    }
                    if let angry = success?.data["6"] as? [[String:Any]]{
                        self.Angry = angry
                    }
                    if let wow = success?.data["4"] as? [[String:Any]]{
                        self.Wow = wow
                    }
                    print("Likes",self.Likes)
                    print("Love",self.Love)
                    print("Sad",self.Sad)
                    print("Haha", self.HaHa)
                    print("Angry",self.Angry)
                    print("Wow", self.Wow)
                    self.activityIndicator.stopAnimating()
                    self.collectionView.reloadData()
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
    
    private func loadReaction(){
        if let like = self.reaction["1"] as? Int{
            self.likeCount = like
            self.totalCount = self.totalCount + like
        }
        if let love = self.reaction["2"] as? Int{
            self.loveCount = love
            self.totalCount = self.totalCount + love
        }
        if let haha = self.reaction["3"] as? Int{
            self.hahaCount = haha
            self.totalCount = self.totalCount + haha
        }
        if let wow = self.reaction["4"] as? Int{
            self.wowCount = wow
            self.totalCount = self.totalCount + wow
        }
        if let sad = self.reaction["5"] as? Int{
            self.sadCount = sad
            self.totalCount = self.totalCount + sad
        }
        if let angry = self.reaction["6"] as? Int{
            self.angryCount = angry
            self.totalCount = self.totalCount + angry
        }
        if totalCount == 1{
            self.reactionBtn1.isEnabled = true
            self.reactionBtn2.isEnabled = false
            self.reactionBtn3.isEnabled = false
            self.reactionBtn4.isEnabled = false
            self.reactionBtn5.isEnabled = false
            self.reactionBtn6.isEnabled = false
        }
        else if totalCount == 2{
            self.reactionBtn1.isEnabled = true
            self.reactionBtn2.isEnabled = true
            self.reactionBtn3.isEnabled = false
            self.reactionBtn4.isEnabled = false
            self.reactionBtn5.isEnabled = false
            self.reactionBtn6.isEnabled = false
        }
        else if totalCount == 3{
            self.reactionBtn1.isEnabled = true
            self.reactionBtn2.isEnabled = true
            self.reactionBtn3.isEnabled = true
            self.reactionBtn4.isEnabled = false
            self.reactionBtn5.isEnabled = false
            self.reactionBtn6.isEnabled = false
        }
        else if totalCount == 4{
            self.reactionBtn1.isEnabled = true
            self.reactionBtn2.isEnabled = true
            self.reactionBtn3.isEnabled = true
            self.reactionBtn4.isEnabled = true
            self.reactionBtn5.isEnabled = false
            self.reactionBtn6.isEnabled = false
        }
        else if totalCount == 5{
            self.reactionBtn1.isEnabled = true
            self.reactionBtn2.isEnabled = true
            self.reactionBtn3.isEnabled = true
            self.reactionBtn4.isEnabled = true
            self.reactionBtn5.isEnabled = true
            self.reactionBtn6.isEnabled = false
        }
        else {
            self.reactionBtn1.isEnabled = true
            self.reactionBtn2.isEnabled = true
            self.reactionBtn3.isEnabled = true
            self.reactionBtn4.isEnabled = true
            self.reactionBtn5.isEnabled = true
            self.reactionBtn6.isEnabled = true
        }
        
        if self.likeCount != 0{
            self.reactionBtn1.setTitle("LIKE", for: .normal)
        }
        else if self.loveCount != 0{
            self.reactionBtn1.setTitle("LOVE", for: .normal)
        }
        else if self.hahaCount != 0{
            self.reactionBtn1.setTitle("HAHA", for: .normal)
        }
        else if self.wowCount != 0{
            self.reactionBtn1.setTitle("WOW", for: .normal)
        }
        else if self.sadCount != 0 {
            self.reactionBtn1.setTitle("SAD", for: .normal)
        }
        else if self.angryCount != 0{
            self.reactionBtn1.setTitle("ANGRY", for: .normal)
        }
        
        if self.loveCount != 0 && self.reactionBtn1.title(for: .normal) != "LOVE"{
            self.reactionBtn2.setTitle("LOVE", for: .normal)
        }
        else if self.hahaCount != 0 && self.reactionBtn1.title(for: .normal) != "HAHA"{
            self.reactionBtn2.setTitle("HAHA", for: .normal)
        }
        else if self.wowCount != 0 && self.reactionBtn1.title(for: .normal) != "WOW"{
            self.reactionBtn2.setTitle("WOW", for: .normal)
        }
        else if self.sadCount != 0 && self.reactionBtn1.title(for: .normal) != "SAD" {
            self.reactionBtn2.setTitle("SAD", for: .normal)
        }
        else if self.angryCount != 0 && self.reactionBtn1.title(for: .normal) != "ANGRY"{
            self.reactionBtn2.setTitle("ANGRY", for: .normal)
        }
        if (self.hahaCount != 0 && self.reactionBtn1.title(for: .normal) != "HAHA") && (self.reactionBtn2.title(for: .normal) != "HAHA"){
            self.reactionBtn3.setTitle("HAHA", for: .normal)
        }
        else if (self.wowCount != 0 && self.reactionBtn1.title(for: .normal) != "WOW") && (self.reactionBtn2.title(for: .normal) != "WOW"){
            self.reactionBtn3.setTitle("WOW", for: .normal)
        }
        else if (self.sadCount != 0 && self.reactionBtn1.title(for: .normal) != "SAD") && (self.reactionBtn2.title(for: .normal) != "SAD") {
            self.reactionBtn3.setTitle("SAD", for: .normal)
        }
        else if (self.angryCount != 0 && self.reactionBtn1.title(for: .normal) != "ANGRY") && (self.reactionBtn2.title(for: .normal) != "ANGRY"){
            self.reactionBtn3.setTitle("ANGRY", for: .normal)
        }
        if (self.wowCount != 0 && self.reactionBtn1.title(for: .normal) != "WOW") && (self.reactionBtn2.title(for: .normal) != "WOW") && self.reactionBtn3.title(for: .normal) != "WOW"{
            self.reactionBtn4.setTitle("WOW", for: .normal)
        }
        else if (self.sadCount != 0 && self.reactionBtn1.title(for: .normal) != "SAD") && (self.reactionBtn2.title(for: .normal) != "SAD") && self.reactionBtn3.title(for: .normal) != "SAD" {
            self.reactionBtn4.setTitle("SAD", for: .normal)
        }
        else if (self.angryCount != 0 && self.reactionBtn1.title(for: .normal) != "ANGRY") && (self.reactionBtn2.title(for: .normal) != "ANGRY") && (self.reactionBtn3.title(for: .normal) != "ANGRY"){
            self.reactionBtn4.setTitle("ANGRY", for: .normal)
        }
        
        if (self.sadCount != 0 && self.reactionBtn1.title(for: .normal) != "SAD") && (self.reactionBtn2.title(for: .normal) != "SAD") && (self.reactionBtn3.title(for: .normal) != "SAD") && (self.reactionBtn4.title(for: .normal) != "SAD") {
            self.reactionBtn5.setTitle("SAD", for: .normal)
        }
        else if (self.angryCount != 0 && self.reactionBtn1.title(for: .normal) != "ANGRY") && (self.reactionBtn2.title(for: .normal) != "ANGRY") && (self.reactionBtn3.title(for: .normal) != "ANGRY") && (self.reactionBtn4.title(for: .normal) != "ANGRY"){
            self.reactionBtn5.setTitle("ANGRY", for: .normal)
        }
        
        if (self.angryCount != 0 && self.reactionBtn1.title(for: .normal) != "ANGRY") && (self.reactionBtn2.title(for: .normal) != "ANGRY") && (self.reactionBtn3.title(for: .normal) != "ANGRY") && (self.reactionBtn4.title(for: .normal) != "ANGRY") && (self.reactionBtn5.title(for: .normal) != "ANGRY"){
            self.reactionBtn6.setTitle("ANGRY", for: .normal)
        }
        self.collectionView.reloadData()
    }
    
    var currentTag = 0
    
    @IBAction func ReactionBtn(_ sender: UIButton) {
        if sender.tag == 0{
            self.currentTag = 0
            self.collectionView.scrollToItem(at: IndexPath.init(item: 0, section: 0), at: .left, animated: true)
            self.reactionBtn2.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn3.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn4.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn5.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn6.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn1.setTitleColor(.white, for: .normal)
        }
        else if sender.tag == 1{
            if currentTag >= 1{
             self.collectionView.scrollToItem(at: IndexPath.init(item: 1, section: 0), at: .left, animated: true)
            }
            else {
                self.collectionView.scrollToItem(at: IndexPath.init(item: 1, section: 0), at: .right, animated: true)
            }
            self.currentTag = 1
            self.reactionBtn2.setTitleColor(.white, for: .normal)
            self.reactionBtn1.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn3.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn4.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn5.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn6.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
        }
        else if sender.tag == 2{
            if currentTag >= 2{
                self.collectionView.scrollToItem(at: IndexPath.init(item: 2, section: 0), at: .left, animated: true)
            }
            else {
                self.collectionView.scrollToItem(at: IndexPath.init(item: 2, section: 0), at: .right, animated: true)
            }
            self.currentTag = 2
            self.reactionBtn3.setTitleColor(.white, for: .normal)
            self.reactionBtn1.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn2.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn4.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn5.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn6.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
        }
        else if sender.tag == 3{
            if currentTag >= 3{
                self.collectionView.scrollToItem(at: IndexPath.init(item: 3, section: 0), at: .left, animated: true)
            }
            else {
                self.collectionView.scrollToItem(at: IndexPath.init(item: 3, section: 0), at: .right, animated: true)
            }
            self.currentTag = 3
            self.reactionBtn4.setTitleColor(.white, for: .normal)
            self.reactionBtn1.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn2.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn3.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn5.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn6.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            
        }
        else if sender.tag == 4{
            if currentTag >= 4{
                self.collectionView.scrollToItem(at: IndexPath.init(item: 4, section: 0), at: .left, animated: true)
            }
            else {
                self.collectionView.scrollToItem(at: IndexPath.init(item: 4, section: 0), at: .right, animated: true)
            }
            self.currentTag = 4
            self.reactionBtn5.setTitleColor(.white, for: .normal)
            self.reactionBtn1.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn2.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn3.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn4.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn6.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            
        }
        else if sender.tag == 5{
            if currentTag >= 5{
                self.collectionView.scrollToItem(at: IndexPath.init(item: 5, section: 0), at: .left, animated: true)
            }
            else {
                self.collectionView.scrollToItem(at: IndexPath.init(item: 5, section: 0), at: .right, animated: true)
            }
            self.currentTag = 5
            self.reactionBtn6.setTitleColor(.white, for: .normal)
            self.reactionBtn1.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn3.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn4.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn5.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            self.reactionBtn2.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
            
        }
    }
    
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    deinit {
        NotificationCenter.default.removeObserver(self)
    }
    
    
    
}
extension PostReactionController :UICollectionViewDelegate,UICollectionViewDataSource,UICollectionViewDelegateFlowLayout{
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.totalCount
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        if indexPath.item == 0{
            if self.reactionBtn1.title(for: .normal) == "LIKE"{
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Likes
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn1.title(for: .normal) == "HAHA"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.HaHa
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn1.title(for: .normal) == "WOW"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Wow
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn1.title(for: .normal) == "SAD") {
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Sad
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn1.title(for: .normal) == "LOVE"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Love
                cell.tableView.reloadData()
                return cell
            }
            else {
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Angry
                cell.tableView.reloadData()
                return cell
            }
        }
        else if indexPath.item == 1{
            if self.reactionBtn2.title(for: .normal) == "LIKE"{
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Likes
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn2.title(for: .normal) == "HAHA"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.HaHa
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn2.title(for: .normal) == "WOW"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Wow
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn2.title(for: .normal) == "SAD") {
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Sad
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn2.title(for: .normal) == "LOVE"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Love
                cell.tableView.reloadData()
                return cell
            }
            else {
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Angry
                cell.tableView.reloadData()
                return cell
            }
        }
        else if indexPath.item == 2{
            if self.reactionBtn3.title(for: .normal) == "LIKE"{
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Likes
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn3.title(for: .normal) == "HAHA"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.HaHa
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn3.title(for: .normal) == "WOW"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Wow
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn3.title(for: .normal) == "SAD") {
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Sad
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn3.title(for: .normal) == "LOVE"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Love
                cell.tableView.reloadData()
                return cell
            }
            else {
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Angry
                cell.tableView.reloadData()
                return cell
            }
        }
        else if indexPath.item == 3{
            if self.reactionBtn4.title(for: .normal) == "LIKE"{
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Likes
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn4.title(for: .normal) == "HAHA"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.HaHa
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn4.title(for: .normal) == "WOW"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Wow
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn4.title(for: .normal) == "SAD") {
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Sad
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn4.title(for: .normal) == "LOVE"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Love
                cell.tableView.reloadData()
                return cell
            }
            else {
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Angry
                cell.tableView.reloadData()
                return cell
            }
        }
        else if indexPath.item == 4{
            if self.reactionBtn5.title(for: .normal) == "LIKE"{
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Likes
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn5.title(for: .normal) == "HAHA"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.HaHa
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn5.title(for: .normal) == "WOW"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Wow
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn5.title(for: .normal) == "SAD") {
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Sad
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn5.title(for: .normal) == "LOVE"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Love
                cell.tableView.reloadData()
                return cell
            }
            else {
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Angry
                cell.tableView.reloadData()
                return cell
            }
        }
        else {
            if self.reactionBtn6.title(for: .normal) == "LIKE"{
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Likes
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn6.title(for: .normal) == "HAHA"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.HaHa
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn6.title(for: .normal) == "WOW"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Wow
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn6.title(for: .normal) == "SAD") {
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Sad
                cell.tableView.reloadData()
                return cell
            }
            else if (self.reactionBtn6.title(for: .normal) == "LOVE"){
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Love
                cell.tableView.reloadData()
                return cell
            }
            else {
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "PostReactioncell", for: indexPath) as! ReactionCells
                cell.reactions = self.Angry
                cell.tableView.reloadData()
                return cell
            }
        }
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        
        let padding: CGFloat = 0
        let collectionViewSize = self.collectionView.frame.size.width - padding
        return CGSize(width: collectionViewSize, height:self.collectionView.frame.size.height )
    }
}

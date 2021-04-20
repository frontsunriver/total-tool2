

import UIKit
import NotificationCenter
import WoWonderTimelineSDK
class CommentReplyController: UIViewController,UITextViewDelegate,uploadImageDelegate,AddReactionDelegate{
    
    @IBOutlet weak var replyLabel: UILabel!
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var imageBtn: UIButton!
    @IBOutlet weak var sendBtn: UIButton!
    @IBOutlet weak var commentText: UITextView!
    
    let status = Reach().connectionStatus()
    let Storyboard = UIStoryboard(name: "Main", bundle: nil)
    
    
    var commentReplies = [[String:Any]]()
    var selectedIndexs = [[String:Any]]()
    var comment = [String:Any]()
    var commentId: String? = nil
    var offset: String? = nil
    var isImage = false
    var selectedIndex = 0
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.tableView.register(UINib(nibName: "CommentCellTableViewCell", bundle: nil), forCellReuseIdentifier: "CommentsCell")
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.commentText.delegate = self
        self.getCommentReplies()
        self.tableView.tableFooterView = UIView()
        if let replies = self.comment["replies"] as? String{
            if replies == "0"{
                self.replyLabel.text = " Replies"
            }
            else {
                self.replyLabel.text = "\(replies)\(" Replies")"
            }
        }
        if let commentId = self.comment["id"] as? String{
            self.commentId = commentId
        }
        print(self.commentId)
        self.tableView.reloadData()
        self.getCommentReplies()
        self.commentText.delegate = self
        self.sendBtn.isEnabled = false
        self.tableView.separatorStyle = .singleLine
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    func textViewDidBeginEditing(_ textView: UITextView) {
        if self.commentText.text == "Add a comment here"{
            self.commentText.text = nil
        }
    }
    
    func textViewDidEndEditing(_ textView: UITextView) {
        if self.commentText.text == "" || self.commentText.text.isEmpty == true{
            self.commentText.text = "Add a comment here"
            self.sendBtn.isEnabled = false
            self.sendBtn.setImage(#imageLiteral(resourceName: "right-arrow"), for: .normal)
        }
    }
    func textViewDidChangeSelection(_ textView: UITextView) {
        //        print(self.commentText.text)
        if self.commentText.text.count > 0{
            self.sendBtn.isEnabled = true
            self.sendBtn.setImage(#imageLiteral(resourceName: "send"), for: .normal)
        }
        else {
            self.sendBtn.isEnabled = false
            self.sendBtn.setImage(#imageLiteral(resourceName: "right-arrow"), for: .normal)
        }
    }
    
    private func getCommentReplies(){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                FetchCommentReplyManager.sharedInstance.fetchCommentReply(comment_Id: self.commentId ?? "00", offset: self.offset ?? "1") { (success, authError, error) in
                    if success != nil {
                        self.commentReplies.removeAll()
                        for i in success!.data{
                            self.commentReplies.append(i)
                        }
                        self.tableView.reloadData()
                    }
                    else if authError != nil {
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil{
                        self.view.makeToast(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    private func createCommentReply(data :Data?) {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            var text: String? = nil
            if (self.commentText.text) == "Add a comment here" || ((self.commentText.text) == "") {
                text = ""
            }
            else{
                text = self.commentText.text
            }
            CreateComment_ReplyManager.sharedInstance.createCommentReply(data: data, commentId: self.commentId ?? "1", text: text ?? "") { (success, authError, error) in
                if success != nil {
                    self.commentText.text = ""
                    self.sendBtn.isEnabled = false
                    self.sendBtn.setImage(#imageLiteral(resourceName: "right-arrow"), for: .normal)
                    self.commentText.resignFirstResponder()
                    self.commentReplies.append(success!.data)
                    self.isImage = false
                    self.tableView.reloadData()
                }
                else if (authError != nil) {
                    self.view.makeToast(authError?.errors.errorText)
                }
                    
                else if (error != nil){
                    self.view.makeToast(error?.localizedDescription)
                }
                
            }
        }
        
    }
    
    
    @IBAction func Back(_ sender: Any) {
        NotificationCenter.default.removeObserver(self)
        self.dismiss(animated: true, completion: nil)
    }
    
    private func likeComment(commentId: String,type: String) {
        LikeCommentManager.sharedIntsance.likeComment(commentId: commentId, type: type) { (success, authError, error) in
            if success != nil {
                print(success?.api_status)
            }
            else if authError != nil {
                print(authError?.errors.errorText)
            }
            else if error != nil {
                print(error?.localizedDescription)
            }
        }
    }
    
    @IBAction func AddImage(_ sender: Any) {
        let Storyboard = UIStoryboard(name: "Main", bundle: nil)
        let vc = Storyboard.instantiateViewController(withIdentifier: "CropImageVC") as! CropImageController
        vc.delegate = self
        vc.imageType = "upload"
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func Send(_ sender: Any) {
        if !self.isImage{
            self.createCommentReply(data: nil)
        }
        else{
            let imageData = self.imageBtn.image(for: .normal)?.jpegData(compressionQuality: 0.1)
            self.createCommentReply(data: imageData)
            
        }
    }
    
    
    @IBAction func NormalTapped(gesture: UIGestureRecognizer){
        let status = Reach().connectionStatus()
        switch status {
        case .unknown, .offline:
            self.tableView.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            let cell = self.tableView.cellForRow(at: IndexPath(row: gesture.view?.tag ?? 0, section: 1)) as! CommentCellTableViewCell
            if let reactions = self.commentReplies[gesture.view?.tag ?? 0]["reaction"] as? [String:Any]{
                var totalCount = 0
                if let count = reactions["count"] as? Int{
                    totalCount = count
                }
                if let isReacted = reactions["is_reacted"] as? Bool{
                    if (isReacted == true){
                        self.reactions(index: gesture.view?.tag ?? 0, reaction: "")
                        var localPostArray = self.commentReplies[gesture.view?.tag ?? 0]["reaction"] as! [String:Any]
                        localPostArray["is_reacted"] = false
                        localPostArray["type"]  = ""
                        localPostArray["count"] = totalCount - 1
                        totalCount =  localPostArray["count"] as? Int ?? 0
                        if totalCount == 0{
                            cell.reactionImage.isHidden = true
                            cell.reactionCount.isHidden = true
                        }
                        if let reaction_type = reactions["type"] as? String{
                            if reaction_type == "1"{
                                if let likecount = reactions["1"] as? Int{
                                    localPostArray["1"] = likecount - 1
                                }
                            }
                            else if reaction_type == "2"{
                                if let lovecount = reactions["2"] as? Int{
                                    localPostArray["2"] = lovecount - 1
                                }
                            }
                            else if reaction_type == "3"{
                                if let hahacount = reactions["3"] as? Int{
                                    localPostArray["3"] = hahacount - 1
                                }
                            }
                            else if reaction_type == "4"{
                                if let wowcount = reactions["4"] as? Int{
                                    localPostArray["4"] = wowcount - 1
                                }
                            }
                            else if reaction_type == "5"{
                                if let sadcount = reactions["5"] as? Int{
                                    localPostArray["5"] = sadcount - 1
                                }
                            }
                            else if reaction_type == "6"{
                                if let angryCount = reactions["6"] as? Int{
                                    localPostArray["6"] = angryCount - 1
                                }
                            }
                            
                        }
                        self.commentReplies[gesture.view?.tag ?? 0]["reaction"] = localPostArray
                        cell.likeBtn.setTitle(NSLocalizedString("Like", comment: "Like"), for: .normal)
                        cell.likeBtn.setTitleColor(.black, for: .normal)
                        cell.reactionCount.text = "\(totalCount)"
                        if let reacts = self.commentReplies[gesture.view?.tag ?? 0]["reaction"] as? [String:Any]{
                            if let checkLike = reacts["1"] as? Int{
                                if checkLike != 0{
                                    cell.reactionImage.image = UIImage(named: "like-2")
                                    break;
                                }
                            }
                            if let checkLove = reacts["2"] as? Int{
                                if checkLove != 0{
                                    cell.reactionImage.image = UIImage(named: "love")
                                    break;
                                }
                            }
                            if let checkHaha = reacts["3"] as? Int{
                                if checkHaha != 0{
                                    cell.reactionImage.image = UIImage(named: "haha")
                                    break;
                                }
                            }
                            if let checkWow = reacts["4"] as? Int{
                                if checkWow != 0{
                                    cell.reactionImage.image = UIImage(named: "wow")
                                    break;
                                }
                                
                            }
                            if let checkSad = reacts["5"] as? Int{
                                if checkSad != 0{
                                    cell.reactionImage.image = UIImage(named: "sad")
                                    break;
                                }
                            }
                            if let checkSad = reacts["6"] as? Int{
                                if checkSad != 0{
                                    cell.reactionImage.image = UIImage(named: "angry")
                                    break;
                                }
                                
                            }
                            
                        }
                        
                    }
                    else{
                        self.selectedIndex = gesture.view!.tag
                        let storyboard = UIStoryboard(name: "Main", bundle: nil)
                        let vc = storyboard.instantiateViewController(withIdentifier: "LikeReactionsVC") as! LikeReactionsController
                        vc.delegate = self
                        vc.modalPresentationStyle = .overFullScreen
                        vc.modalTransitionStyle = .crossDissolve
                        self.present(vc, animated: true, completion: nil)
                    }
                }
            }
            
        }
    }
    
    
    @IBAction func LongTapped(gesture: UILongPressGestureRecognizer){
        self.selectedIndex = gesture.view!.tag
        let storyboard = UIStoryboard(name: "Main", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "LikeReactionsVC") as! LikeReactionsController
        vc.delegate = self
        vc.modalPresentationStyle = .overFullScreen
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    func addReaction(reation: String) {
        print(reation)
        let cell = self.tableView.cellForRow(at: IndexPath(row: self.selectedIndex ?? 0, section: 1)) as! CommentCellTableViewCell
        print(self.selectedIndex)
        self.reactions(index: self.selectedIndex, reaction: reation)
        var localPostArray = self.commentReplies[self.selectedIndex]["reaction"] as! [String:Any]
        var totalCount = 0
        if let reactions = self.commentReplies[self.selectedIndex]["reaction"] as? [String:Any]{
            if let is_react = reactions["is_reacted"] as? Bool{
                if !is_react {
                    if let count = reactions["count"] as? Int{
                        totalCount = count
                    }
                    localPostArray["count"] = totalCount + 1
                    totalCount =  localPostArray["count"] as? Int ?? 0
                    print(totalCount)
                    cell.reactionCount.text = "\(totalCount)"
                }
                else{
                    if let count = reactions["count"] as? Int{
                        totalCount = count
                    }
                }
            }
        }
        let action = ["count": totalCount, "reaction": reation,"index": self.selectedIndex] as [String : Any]
        var count = 0
        print(self.selectedIndexs.count)
        if self.selectedIndexs.count == 0 {
            self.selectedIndexs.append(action)
        }
        else{
            for i in self.selectedIndexs{
                count += 1
                if i["index"] as? Int == self.selectedIndex{
                    print((count) - 1)
                    self.selectedIndexs[(count) - 1] = action
                }
                else{
                    self.selectedIndexs.append(action)
                }
            }
        }
        
        localPostArray["is_reacted"] = true
        localPostArray["type"]  = reation
        
        if reation == "1"{
            localPostArray["Like"] = 1
            localPostArray["1"] = 1
            self.commentReplies[self.selectedIndex]["reaction"] = localPostArray
            cell.reactionImage.image = UIImage(named: "like-2")
            cell.likeBtn.setTitle("\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "3D5898"), for: .normal)
        }
        else if reation == "2"{
            localPostArray["Love"] = 1
            localPostArray["2"] = 1
            self.commentReplies[self.selectedIndex]["reaction"] = localPostArray
            cell.likeBtn.setTitle("\(NSLocalizedString("Love", comment: "Love"))", for: .normal)
            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FB1002"), for: .normal)
            cell.reactionImage.image = UIImage(named: "love")
        }
        else if reation == "3"{
            localPostArray["HaHa"] = 1
            localPostArray["3"] = 1
            self.commentReplies[self.selectedIndex]["reaction"] = localPostArray
            cell.reactionImage.image = UIImage(named: "haha")
            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
            cell.likeBtn.setTitle("\(NSLocalizedString("Haha", comment: "Haha"))", for: .normal)
        }
        else if reation == "4"{
            localPostArray["Wow"] = 1
            localPostArray["4"] = 1
            self.commentReplies[self.selectedIndex]["reaction"] = localPostArray
            cell.reactionImage.image = UIImage(named: "wow")
            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
            cell.likeBtn.setTitle("\(NSLocalizedString("Wow", comment: "Wow"))", for: .normal)
        }
        else if reation == "5"{
            localPostArray["Sad"] = 1
            localPostArray["5"] = 1
            self.commentReplies[self.selectedIndex]["reaction"] = localPostArray
            cell.reactionImage.image = UIImage(named: "sad")
            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
            cell.likeBtn.setTitle("\(NSLocalizedString("Sad", comment: "Sad"))", for: .normal)
        }
        else {
            localPostArray["Angry"] = 1
            localPostArray["6"] = 1
            self.commentReplies[self.selectedIndex]["reaction"] = localPostArray
            cell.reactionImage.image = UIImage(named: "angry")
            cell.likeBtn.setTitle("\(NSLocalizedString("Angry", comment: "Angry"))", for: .normal)
            cell.likeBtn.setTitleColor(.red, for: .normal)
        }
    }
    
    
    
    private func reactions(index :Int, reaction: String) {
        let status = Reach().connectionStatus()
        switch status {
        case .unknown, .offline:
            self.tableView.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan), .online(.wiFi):
            performUIUpdatesOnMain {
                var comment_id = ""
                if let commentId = self.commentReplies[index]["id"] as? String{
                    comment_id = commentId
                }
                AddCommentReactionManager.sharedInstacne.AddComment(commentId: Int(comment_id) ?? 0, reaction: reaction) { (success, authError, error) in
                    if (success != nil){
                        print(success?.message)
                    }
                    else if (authError != nil){
                        print(authError?.errors?.errorText)
                    }
                    else if (error != nil){
                        print(error?.localizedDescription)
                    }
                }
                
            }
        }
    }
    
    func uploadImage(imageType: String, image: UIImage) {
        self.imageBtn.setImage(image, for: .normal)
        self.isImage = true
        self.sendBtn.isEnabled = true
        self.sendBtn.setImage(#imageLiteral(resourceName: "send"), for: .normal)
    }
    
    
    @IBAction func GotoPostReaction(sender :UIButton){
        if let reaction = self.commentReplies[sender.tag]["reaction"] as? [String:Any]{
            if let count = reaction["count"] as? Int{
                if count > 0 {
                    let storyboard = UIStoryboard(name: "Main", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "PostReactionVC") as! PostReactionController
                    if let postId = self.commentReplies[sender.tag]["id"] as? String{
                        print(postId)
                        vc.postId = postId
                    }
                    if let reactions = self.commentReplies[sender.tag]["reaction"] as? [String:Any]{
                        vc.reaction = reactions
                    }
                    vc.is_Comment = 1
                    self.present(vc, animated: true, completion: nil)
                }
            }
        }
    }
    
}
extension CommentReplyController : UITableViewDelegate,UITableViewDataSource{
    func numberOfSections(in tableView: UITableView) -> Int {
        return 2
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if section == 0 {
            return 1
        }
        else {
            return self.commentReplies.count
        }
        
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        if indexPath.section == 0 {
            let cell = tableView.dequeueReusableCell(withIdentifier: "CommentsCell") as! CommentCellTableViewCell
            cell.noCommentView.isHidden = false
            if let publisher = self.comment["publisher"] as? [String:Any]{
                if let name = publisher["username"] as? String{
                    cell.userName.text = name
                }
                
                if let image = publisher["avatar"] as? String{
                    let url = URL(string: image)
                    cell.profileImage.kf.setImage(with: url)
                }
            }
            if let text = self.comment["text"] as? String{
                cell.commentText?.text = text.htmlToString
            }
            
            if let image = self.comment["c_file"] as? String{
                if image != ""{
                    let prefix = image.prefix(6)
                    var img = ""
                    if prefix == "upload"{
                        img = "\("https://wowonder.fra1.digitaloceanspaces.com/")\(image)"
                    }
                    else{
                        img = image
                    }
                    let width = cell.designView.frame.size.width
                    print("Width",width)
                    cell.imageWidth.isActive = true
                    cell.imageWidth.constant = width
                    cell.imageHeight.constant = width
                    let url = URL(string: img)
                    cell.commentImage.kf.setImage(with: url)
                }
                else {
                    cell.imageWidth.isActive = false
                    cell.imageHeight.constant = 0.0
                }
            }
            
            if let is_react =  self.comment["reaction"] as? [String:Any]{
                if let isLiked = is_react["is_reacted"] as? Bool{
                    if isLiked == true{
                        if let type = is_react["type"] as? String{
                            if type == "1"{
                                cell.likeBtn.setTitle(NSLocalizedString("Like", comment: "Like"), for: .normal)
                                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "3D5898"), for: .normal)
                                cell.reactionImage.image = UIImage(named: "like-2")
                            }
                            else if type == "2"{
                                cell.likeBtn.setTitle(NSLocalizedString("Love", comment: "Love"), for: .normal)
                                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FB1002"), for: .normal)
                                cell.reactionImage.image = UIImage(named: "love")
                                
                            }
                            else if type == "3"{
                                cell.likeBtn.setTitle(NSLocalizedString("Haha", comment: "Haha"), for: .normal)
                                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                                cell.reactionImage.image = UIImage(named: "haha")
                                
                            }
                            else if type == "4"{
                                cell.likeBtn.setTitle(NSLocalizedString("Wow", comment: "Wow"), for: .normal)
                                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                                cell.reactionImage.image = UIImage(named: "wow")
                                
                            }
                            else if type == "5"{
                                cell.likeBtn.setTitle(NSLocalizedString("Sad", comment: "Sad"), for: .normal)
                                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                                cell.reactionImage.image = UIImage(named: "sad")
                            }
                            else if type == "6"{
                                cell.likeBtn.setTitle(NSLocalizedString("Angry", comment: "Angry"), for: .normal)
                                cell.likeBtn.setTitleColor(.red, for: .normal)
                                cell.reactionImage.image = UIImage(named: "angry")
                            }
                        }
                        
                        
                        if let checkLike = is_react["1"] as? Int{
                            if checkLike != 0{
                                cell.reactionImage.image = UIImage(named: "like-2")
                            }
                        }
                        if let checkLove = is_react["2"] as? Int{
                            if checkLove != 0{
                                cell.reactionImage.image = UIImage(named: "love")
                            }
                        }
                        if let checkHaha = is_react["3"] as? Int{
                            if checkHaha != 0{
                                cell.reactionImage.image = UIImage(named: "haha")
                            }
                        }
                        if let checkWow = is_react["4"] as? Int{
                            if checkWow != 0{
                                cell.reactionImage.image = UIImage(named: "wow")
                            }
                            
                        }
                        if let checkSad = is_react["5"] as? Int{
                            if checkSad != 0{
                                cell.reactionImage.image = UIImage(named: "sad")
                            }
                        }
                        if let checkSad = is_react["6"] as? Int{
                            if checkSad != 0{
                                cell.reactionImage.image = UIImage(named: "angry")
                            }
                        }
                        
                        
                        
                    }
                }
                if let count = is_react["count"] as? Int{
                    if count == 0{
                        cell.reactionCount.text = nil
                        cell.reactionImage.image = nil
                        cell.likeBtn.setTitle(NSLocalizedString("Like", comment: "Like"), for: .normal)
                        cell.likeBtn.setTitleColor(.black, for: .normal)
                    }
                    else{
                        cell.reactionCount.text = "\(count)"
                    }
                }
            }
            
            for i in self.selectedIndexs{
                if i["index"] as? Int == indexPath.row{
                    if let reaction = i["reaction"] as? String{
                        if reaction == "6"{
                            cell.reactionImage.image = UIImage(named: "angry")
                            cell.likeBtn.setTitle("\(" ")\(NSLocalizedString("Angry", comment: "Angry"))", for: .normal)
                            cell.likeBtn.setTitleColor(.red, for: .normal)
                        }
                        else if reaction == "1"{
                            cell.reactionImage.image = UIImage(named: "like-2")
                            cell.likeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
                            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "3D5898"), for: .normal)
                        }
                        else if reaction == "2"{
                            cell.reactionImage.image = UIImage(named: "love")
                            cell.likeBtn.setTitle("\(" ")\(NSLocalizedString("Love", comment: "Love"))", for: .normal)
                            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FB1002"), for: .normal)
                        }
                        else if reaction == "4"{
                            cell.reactionImage.image = UIImage(named: "wow")
                            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                            cell.likeBtn.setTitle("\(" ")\(NSLocalizedString("Wow", comment: "Wow"))", for: .normal)
                        }
                        else if reaction == "5"{
                            cell.reactionImage.image = UIImage(named: "sad")
                            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                            cell.likeBtn.setTitle("\(" ")\(NSLocalizedString("Sad", comment: "Sad"))", for: .normal)
                        }
                        else if reaction == "3"{
                            cell.reactionImage.image = UIImage(named: "haha")
                            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                            cell.likeBtn.setTitle("\(" ")\(NSLocalizedString("Haha", comment: "Haha"))", for: .normal)
                        }
                        else if reaction == ""{
                            //                        cell.likeBtn.setTitleColor(.lightGray, for: .normal)
                            //                        cell.likeBtn.setImage(UIImage(named:"like"), for: .normal)
                            cell.likeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
                        }
                    }
                    if let count = i["count"] as? Int{
                        cell.reactionCount.text = "\(count)"
                    }
                }
            }
            
            let normalTapGesture = UITapGestureRecognizer(target: self, action: #selector(self.NormalTapped(gesture:)))
            let longGesture = UILongPressGestureRecognizer(target: self, action: #selector(self.LongTapped(gesture:)))
            normalTapGesture.numberOfTapsRequired = 1
            longGesture.minimumPressDuration = 0.30
            
//            cell.likeBtn.addGestureRecognizer(normalTapGesture)
//            cell.likeBtn.addGestureRecognizer(longGesture)
            cell.likeBtn.tag = indexPath.row
            //            cell.likeBtn.addTarget(self, action: #selector(self.LikeComments(sender:)), for: .touchUpInside)
            cell.replyBtn.isHidden = true
            cell.viewLeadingContraint.constant = 8.0
            return cell
        }
        else {
            let cell = tableView.dequeueReusableCell(withIdentifier: "CommentsCell") as! CommentCellTableViewCell
            cell.noCommentView.isHidden = false
            self.tableView.tableFooterView = UIView()
            self.tableView.separatorStyle = .none
            let index = self.commentReplies[indexPath.row]
            if let publisher = index["publisher"] as? [String:Any]{
                if let name = publisher["username"] as? String{
                    cell.userName.text = name
                }
                
                if let image = publisher["avatar"] as? String{
                    let url = URL(string: image)
                    cell.profileImage.kf.setImage(with: url)
                }
            }
            if let text = index["text"] as? String{
                cell.commentText?.text = text.htmlToString
            }
            
            if let image = index["c_file"] as? String{
                if image != ""{
                    let prefix = image.prefix(6)
                    var img = ""
                    if prefix == "upload"{
                        img = "\("https://wowonder.fra1.digitaloceanspaces.com/")\(image)"
                    }
                    else{
                        img = image
                    }
                    let width = cell.designView.frame.size.width
                    print("Width",width)
                    cell.imageWidth.isActive = true
                    cell.imageWidth.constant = width
                    cell.imageHeight.constant = width
                    let url = URL(string: img)
                    cell.commentImage.kf.setImage(with: url)
                }
                else {
                    cell.imageWidth.isActive = false
                    cell.imageHeight.constant = 0.0
                }
            }
            
            if let is_react = index["reaction"] as? [String:Any]{
                if let isLiked = is_react["is_reacted"] as? Bool{
                    if isLiked == true{
                        if let type = is_react["type"] as? String{
                            if type == "1"{
                                cell.likeBtn.setTitle(NSLocalizedString("Like", comment: "Like"), for: .normal)
                                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "3D5898"), for: .normal)
                                cell.reactionImage.image = UIImage(named: "like-2")
                            }
                            else if type == "2"{
                                cell.likeBtn.setTitle(NSLocalizedString("Love", comment: "Love"), for: .normal)
                                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FB1002"), for: .normal)
                                cell.reactionImage.image = UIImage(named: "love")
                                
                            }
                            else if type == "3"{
                                cell.likeBtn.setTitle(NSLocalizedString("Haha", comment: "Haha"), for: .normal)
                                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                                cell.reactionImage.image = UIImage(named: "haha")
                                
                            }
                            else if type == "4"{
                                cell.likeBtn.setTitle(NSLocalizedString("Wow", comment: "Wow"), for: .normal)
                                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                                cell.reactionImage.image = UIImage(named: "wow")
                                
                            }
                            else if type == "5"{
                                cell.likeBtn.setTitle(NSLocalizedString("Sad", comment: "Sad"), for: .normal)
                                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                                cell.reactionImage.image = UIImage(named: "sad")
                            }
                            else if type == "6"{
                                cell.likeBtn.setTitle(NSLocalizedString("Angry", comment: "Angry"), for: .normal)
                                cell.likeBtn.setTitleColor(.red, for: .normal)
                                cell.reactionImage.image = UIImage(named: "angry")
                            }
                        }
                        
                        
                        if let checkLike = is_react["1"] as? Int{
                            if checkLike != 0{
                                cell.reactionImage.image = UIImage(named: "like-2")
                            }
                        }
                        if let checkLove = is_react["2"] as? Int{
                            if checkLove != 0{
                                cell.reactionImage.image = UIImage(named: "love")
                            }
                        }
                        if let checkHaha = is_react["3"] as? Int{
                            if checkHaha != 0{
                                cell.reactionImage.image = UIImage(named: "haha")
                            }
                        }
                        if let checkWow = is_react["4"] as? Int{
                            if checkWow != 0{
                                cell.reactionImage.image = UIImage(named: "wow")
                            }
                            
                        }
                        if let checkSad = is_react["5"] as? Int{
                            if checkSad != 0{
                                cell.reactionImage.image = UIImage(named: "sad")
                            }
                        }
                        if let checkSad = is_react["6"] as? Int{
                            if checkSad != 0{
                                cell.reactionImage.image = UIImage(named: "angry")
                            }
                        }
                        
                        
                        
                    }
                }
                if let count = is_react["count"] as? Int{
                    if count == 0{
                        cell.reactionCount.text = nil
                        cell.reactionImage.image = nil
                        cell.likeBtn.setTitle(NSLocalizedString("Like", comment: "Like"), for: .normal)
                        cell.likeBtn.setTitleColor(.black, for: .normal)
                    }
                    else{
                        cell.reactionCount.text = "\(count)"
                    }
                }
            }
            
            for i in self.selectedIndexs{
                if i["index"] as? Int == indexPath.row{
                    if let reaction = i["reaction"] as? String{
                        if reaction == "6"{
                            cell.reactionImage.image = UIImage(named: "angry")
                            cell.likeBtn.setTitle("\(" ")\(NSLocalizedString("Angry", comment: "Angry"))", for: .normal)
                            cell.likeBtn.setTitleColor(.red, for: .normal)
                        }
                        else if reaction == "1"{
                            cell.reactionImage.image = UIImage(named: "like-2")
                            cell.likeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
                            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "3D5898"), for: .normal)
                        }
                        else if reaction == "2"{
                            cell.reactionImage.image = UIImage(named: "love")
                            cell.likeBtn.setTitle("\(" ")\(NSLocalizedString("Love", comment: "Love"))", for: .normal)
                            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FB1002"), for: .normal)
                        }
                        else if reaction == "4"{
                            cell.reactionImage.image = UIImage(named: "wow")
                            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                            cell.likeBtn.setTitle("\(" ")\(NSLocalizedString("Wow", comment: "Wow"))", for: .normal)
                        }
                        else if reaction == "5"{
                            cell.reactionImage.image = UIImage(named: "sad")
                            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                            cell.likeBtn.setTitle("\(" ")\(NSLocalizedString("Sad", comment: "Sad"))", for: .normal)
                        }
                        else if reaction == "3"{
                            cell.reactionImage.image = UIImage(named: "haha")
                            cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "FECD30"), for: .normal)
                            cell.likeBtn.setTitle("\(" ")\(NSLocalizedString("Haha", comment: "Haha"))", for: .normal)
                        }
                        else if reaction == ""{
                            //                        cell.likeBtn.setTitleColor(.lightGray, for: .normal)
                            //                        cell.likeBtn.setImage(UIImage(named:"like"), for: .normal)
                            cell.likeBtn.setTitle("\(" ")\(NSLocalizedString("Like", comment: "Like"))", for: .normal)
                        }
                    }
                    if let count = i["count"] as? Int{
                        cell.reactionCount.text = "\(count)"
                    }
                }
            }
            
            if let replies = index["replies"] as? String{
                if replies == "0"{
                    cell.replyBtn.setTitle("\("Replay")", for: .normal)
                }
                else {
                    cell.replyBtn.setTitle("\("Replay ")\("(\(replies))")", for: .normal)
                }
            }
            let normalTapGesture = UITapGestureRecognizer(target: self, action: #selector(self.NormalTapped(gesture:)))
            let longGesture = UILongPressGestureRecognizer(target: self, action: #selector(self.LongTapped(gesture:)))
            normalTapGesture.numberOfTapsRequired = 1
            longGesture.minimumPressDuration = 0.30
            
            cell.likeBtn.addGestureRecognizer(normalTapGesture)
            cell.likeBtn.addGestureRecognizer(longGesture)
            cell.replyBtn.tag = indexPath.row
            cell.likeBtn.tag = indexPath.row
            cell.reactionBtn.tag = indexPath.row
            cell.reactionBtn.addTarget(self, action: #selector(self.GotoPostReaction(sender:)), for: .touchUpInside)
//            cell.replyBtn.isHidden = true
//            cell.likeBtn.addTarget(self, action: #selector(self.LikeComment(sender:)), for: .touchUpInside)
            cell.viewLeadingContraint.constant = 56.0
            return cell
        }
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        if indexPath.section == 0{
            return UITableView.automaticDimension
        }
        else {
            return UITableView.automaticDimension
        }
    }
    func tableView(_ tableView: UITableView, estimatedHeightForRowAt indexPath: IndexPath) -> CGFloat {
        if indexPath.section == 0{
            return 300.0
        }
        else {
            return 300.0
        }
    }
    
    @IBAction func GotoCommentReply(sender: UIButton){
        let vc = Storyboard.instantiateViewController(withIdentifier : "CommentReplyVC") as! CommentReplyController
        vc.comment = self.commentReplies[sender.tag]
        vc.modalPresentationStyle = .fullScreen
        vc.modalTransitionStyle = .coverVertical
        self.present(vc, animated: true, completion: nil)
    }
    
    
    @IBAction func LikeComment(sender: UIButton){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            let cell = tableView.cellForRow(at: IndexPath(row: sender.tag, section: 1)) as! CommentCellTableViewCell
            var commentId :String? = nil
            if let id = self.commentReplies[sender.tag]["id"] as? String{
                commentId = id
            }
            if let isLiked = self.commentReplies[sender.tag]["is_comment_liked"] as? Bool{
                if isLiked{
                    cell.likeBtn.setTitle("Like", for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#333333"), for: .normal)
                    self.likeComment(commentId: commentId ?? "", type: "comment_dislike")
                }
                else {
                    cell.likeBtn.setTitle("Liked", for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                    self.likeComment(commentId: commentId ?? "", type: "comment_like")
                }
            }
        }
    }
    
    
    ///////////////
    
    
    @IBAction func LikeComments(sender: UIButton){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            let cell = tableView.cellForRow(at: IndexPath(row: sender.tag, section: 0)) as! CommentCellTableViewCell
            var commentId :String? = nil
            if let id = self.comment["id"] as? String{
                commentId = id
            }
            if let isLiked = self.comment["is_comment_liked"] as? Bool{
                if isLiked{
                    cell.likeBtn.setTitle("Like", for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#333333"), for: .normal)
                    self.likeComment(commentId: commentId ?? "", type: "comment_dislike")
                }
                else {
                    cell.likeBtn.setTitle("Liked", for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                    self.likeComment(commentId: commentId ?? "", type: "comment_like")
                }
            }
        }
    }
    
    
    
}

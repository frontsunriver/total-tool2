//
//  CommentReplyVC.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/25/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import NotificationCenter
import WoWonderTimelineSDK
class CommentReplyVC: UIViewController,UITextViewDelegate,uploadImageDelegate{
    
    @IBOutlet weak var replyLabel: UILabel!
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var imageBtn: UIButton!
    @IBOutlet weak var sendBtn: UIButton!
    @IBOutlet weak var commentText: UITextView!
    
    let status = Reach().connectionStatus()
    let Storyboard = UIStoryboard(name: "Main", bundle: nil)

    
    var commentReplies = [[String:Any]]()
    var comment = [String:Any]()
    var commentId: String? = nil
    var offset: String? = nil
    var isImage = false
    var movieID:String? = ""
    var likeStatus:Bool? = false

    
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
                MoviesCommentManager.sharedInstance.fetchCommentReply(commentId: self.commentId ?? "00", offset: self.offset ?? "1") { (success, authError, error) in
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
            MoviesCommentManager.sharedInstance.addCommentReply(MovieId: self.movieID ?? "", commentID: self.commentId ?? "" , type: "add_reply", text: text ?? "") { (success, sessionError, error) in
                if (success != nil) {
                    self.commentText.text = ""
                    self.sendBtn.isEnabled = false
                    self.sendBtn.setImage(#imageLiteral(resourceName: "right-arrow"), for: .normal)
                    self.commentText.resignFirstResponder()
                    print(success?.data)
                    self.commentReplies.append(success!.data)
                    self.isImage = false
                    self.tableView.reloadData()
                }
                else if (sessionError != nil) {
                    self.view.makeToast(sessionError?.errors.errorText)
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

    private func likeComment(commentId: String,type: String,reaction:String) {
        MoviesCommentManager.sharedInstance.commentReplyLikeDislike(MovieId: self.movieID ?? "", commentId: commentId, type: type, reaction: reaction) { (success, sessionError, error) in
            if success != nil {
                print(success?.api_status)
            }
            else if sessionError != nil {
                print(sessionError?.errors.errorText)
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
    
    func uploadImage(imageType: String, image: UIImage) {
        self.imageBtn.setImage(image, for: .normal)
        self.isImage = true
        self.sendBtn.isEnabled = true
        self.sendBtn.setImage(#imageLiteral(resourceName: "send"), for: .normal)
    }
    
}
extension CommentReplyVC : UITableViewDelegate,UITableViewDataSource{
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
            
            if let isLiked = self.comment["is_comment_liked"] as? Bool{
                   self.likeStatus = isLiked
                if isLiked{
                    cell.likeBtn.setTitle("Liked", for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                }
            }
            cell.likeBtn.tag = indexPath.row
            cell.likeBtn.addTarget(self, action: #selector(self.LikeComments(sender:)), for: .touchUpInside)
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
            if let isLiked = index["is_comment_liked"] as? Bool{
                self.likeStatus = isLiked
                if isLiked{
                    cell.likeBtn.setTitle("Liked", for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
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
            cell.replyBtn.tag = indexPath.row
            cell.likeBtn.tag = indexPath.row
          //  cell.replyBtn.addTarget(self, action: #selector(self.GotoCommentReply(sender:)), for: .touchUpInside)
            cell.likeBtn.addTarget(self, action: #selector(self.LikeComment(sender:)), for: .touchUpInside)
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
        self.likeStatus  = !self.likeStatus!
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
                if self.likeStatus!{
                    cell.likeBtn.setTitle("Like", for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#333333"), for: .normal)
                    self.likeComment(commentId: commentId ?? "", type: "reply_like", reaction: "dislike")
                }
                else {
                    cell.likeBtn.setTitle("Liked", for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                    self.likeComment(commentId: commentId ?? "", type: "reply_like", reaction: "like")
                }
            }
        }
    }
    
    
    ///////////////
    
    
    @IBAction func LikeComments(sender: UIButton){
        self.likeStatus  = !self.likeStatus!
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
                if self.likeStatus!{
                    cell.likeBtn.setTitle("Like", for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#333333"), for: .normal)
                    self.likeComment(commentId: commentId ?? "", type: "like", reaction: "dislike")
                }
                else {
                    cell.likeBtn.setTitle("Liked", for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                    self.likeComment(commentId: commentId ?? "", type: "like", reaction: "like")
                }
            }
        }
    }
    
    
    
}

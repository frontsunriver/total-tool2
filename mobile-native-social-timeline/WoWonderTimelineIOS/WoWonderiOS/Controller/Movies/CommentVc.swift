//
//  CommentVc.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/24/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import XLPagerTabStrip
import Kingfisher
import Toast_Swift
import WoWonderTimelineSDK

import FittedSheets
class CommentVc: UIViewController,UITextViewDelegate,uploadImageDelegate {
    
    @IBOutlet weak var likesCount: UILabel!
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var commentText: UITextView!
    @IBOutlet weak var sendBtn: UIButton!
    @IBOutlet weak var noCommentView: UIView!
    @IBOutlet weak var imageBtn: UIButton!
    
    var comments = [[String:Any]]()
    let status = Reach().connectionStatus()
    var likeStatus:Bool? = false
    
    var moviesID: String? = nil
    var offset = ""
    var likes = 0
    var isImage = false
    let Storyboard = UIStoryboard(name: "Main", bundle: nil)
    override func viewDidLoad() {
        super.viewDidLoad()
        print("commt",comments)
        self.tableView.register(UINib(nibName: "CommentCellTableViewCell", bundle: nil), forCellReuseIdentifier: "CommentsCell")
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.noCommentView.isHidden = true
        self.tableView.tableFooterView = UIView()
        //        self.likesCount.text = "\(self.likes ?? 0)\(" Likes")"
        self.tableView.reloadData()
        self.getComments()
        self.commentText.delegate = self
        self.sendBtn.isEnabled = false
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    @IBAction func Back(_ sender: Any) {
        NotificationCenter.default.removeObserver(self)
        self.dismiss(animated: true, completion: nil)
    }
    
    @IBAction func Send(_ sender: Any) {
        if !self.isImage{
            self.createComment(data: nil)
        }
        else{
            let imageData = self.imageBtn.image(for: .normal)?.jpegData(compressionQuality: 0.1)
            self.createComment(data: imageData)
            
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
    
    func uploadImage(imageType: String, image: UIImage) {
        self.imageBtn.setImage(image, for: .normal)
        self.isImage = true
        self.sendBtn.isEnabled = true
        self.sendBtn.setImage(#imageLiteral(resourceName: "send"), for: .normal)
        self.commentText.resignFirstResponder()
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
    
    private func createComment(data :Data?){
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
            MoviesCommentManager.sharedInstance.addComment(MovieId: self.moviesID ?? "" , type: "add_comment", text: text ?? "") { (success, sessionError, error) in
                if (success != nil) {
                    self.commentText.text = ""
                    self.sendBtn.isEnabled = false
                    self.sendBtn.setImage(#imageLiteral(resourceName: "right-arrow"), for: .normal)
                    self.commentText.resignFirstResponder()
                    if self.comments.isEmpty == true {
                        self.tableView.isHidden = false
                        self.noCommentView.isHidden = false
                    }
                    print(success?.data)
                    self.comments.append(success!.data)
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
    
    private func getComments(){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                MoviesCommentManager.sharedInstance.fetchComment(movieId: self.moviesID ?? "12" , offset: self.offset) { (success, authError, error) in
                    if success != nil {
                        self.comments.removeAll()
                        for i in success!.data {
                            self.comments.append(i)
                        }
                        print(self.comments)
                        if (self.comments.isEmpty == true){
                            self.tableView.isHidden = true
                            self.noCommentView.isHidden = false
                        }
                        self.tableView.reloadData()
                    }
                    else if authError != nil {
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        self.view.makeToast(error?.localizedDescription)
                    }
                }
            }
            
        }
    }
    
    private func likeComment(commentId: String,type: String,reaction:String) {
        MoviesCommentManager.sharedInstance.likeComment(MovieId: self.moviesID ?? "", commentId: commentId, type: type, reaction: reaction) { (success, sessionError, error) in
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
    
}
extension CommentVc : UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        self.comments.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let index = self.comments[indexPath.row]
        let cell = tableView.dequeueReusableCell(withIdentifier: "CommentsCell") as! CommentCellTableViewCell
        cell.noCommentView.isHidden = false
        cell.imageHeight.constant = 0.0
        cell.imageWidth.isActive = false
        //        cell.imageWidth.constant = 100.0
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
        
        if let replies = index["replies"] as? String{
            if replies == "0"{
                cell.replyBtn.setTitle("\("Reply")", for: .normal)
            }
            else {
                cell.replyBtn.setTitle("\("Reply ")\("(\(replies))")", for: .normal)
            }
        }
        if let isLiked = index["is_comment_liked"] as? Bool{
            self.likeStatus = isLiked
            if isLiked{
                cell.likeBtn.setTitle("Liked", for: .normal)
                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
            }
        }
        cell.replyBtn.tag = indexPath.row
        cell.likeBtn.tag = indexPath.row
        cell.replyBtn.addTarget(self, action: #selector(self.GotoCommentReply(sender:)), for: .touchUpInside)
        cell.likeBtn.addTarget(self, action: #selector(self.LikeComment(sender:)), for: .touchUpInside)
        cell.viewLeadingContraint.constant = 8.0
        return cell
    }
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return UITableView.automaticDimension
        //                 return 400.0
    }
    func tableView(_ tableView: UITableView, estimatedHeightForRowAt indexPath: IndexPath) -> CGFloat {
        return 300.0
    }
    
    @IBAction func GotoCommentReply(sender: UIButton){

        let vc =  UIStoryboard(name: "MoreSection", bundle: nil).instantiateViewController(withIdentifier: "CommentReplyVC") as? CommentReplyVC
        vc!.comment = self.comments[sender.tag]
        vc?.movieID = self.comments[sender.tag]["id"] as? String
                  let controller = SheetViewController(controller:vc!)
                  
                  controller.blurBottomSafeArea = false
                  self.present(controller, animated: false, completion: nil)
    }
    
    @IBAction func LikeComment(sender: UIButton){
        self.likeStatus  = !self.likeStatus!
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            let cell = tableView.cellForRow(at: IndexPath(row: sender.tag, section: 0)) as! CommentCellTableViewCell
            var commentId :String? = nil
            if let id = self.comments[sender.tag]["id"] as? String{
                commentId = id
            }
            
            if let isLiked = self.comments[sender.tag]["is_comment_liked"] as? Bool{
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
extension CommentVc:IndicatorInfoProvider{
    func indicatorInfo(for pagerTabStripController: PagerTabStripViewController) -> IndicatorInfo {
        return IndicatorInfo(title: "COMMENT")
    }
}

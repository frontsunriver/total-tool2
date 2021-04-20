

import UIKit
import WoWonderTimelineSDK

class BlogCommentReplyController: UIViewController,UITextViewDelegate {
    
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var textView: UITextView!
    
    let status = Reach().connectionStatus()
    let Storyboard = UIStoryboard(name: "Main", bundle: nil)
    
    var commentReplies = [[String:Any]]()
    var comment = [String:Any]()
    var commentId: String? = nil
    var offset: String? = nil
    var blod_id: String? = nil
    
    @IBOutlet weak var sendBtn: UIButton!
    override func viewDidLoad() {
        super.viewDidLoad()
        print(self.commentId)
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.tableView.register(UINib(nibName: "CommentCellTableViewCell", bundle: nil), forCellReuseIdentifier: "CommentsCell")
        self.textView.delegate = self
        self.tableView.tableFooterView = UIView()
        self.getCommentReplies()
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    func textViewDidBeginEditing(_ textView: UITextView) {
        if self.textView.text == "Add a comment here"{
            self.textView.text = nil
        }
    }
    
    func textViewDidEndEditing(_ textView: UITextView) {
        if self.textView.text == "" || self.textView.text.isEmpty == true{
            self.textView.text = "Add a comment here"
            self.sendBtn.isEnabled = false
            self.sendBtn.setImage(#imageLiteral(resourceName: "right-arrow"), for: .normal)
        }
    }
    func textViewDidChangeSelection(_ textView: UITextView) {
        //        print(self.commentText.text)
        if self.textView.text.count > 0{
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
                GetBlogCommentReplyManager.sharedInstance.getCommentReply(commentId: self.commentId ?? "", offset: self.offset ?? "") { (success, authError, error) in
                    if success != nil{
                        for i in success!.data{
                            self.commentReplies.append(i)
                        }
                        self.tableView.reloadData()
                    }
                    else if authError != nil{
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil{
                        self.view.makeToast(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    private func createCommentReply(){
        CreateReplyBlogCommentManager.sharedInstance.createReply(text: self.textView.text, commentId: self.commentId ?? "", blog_id: self.blod_id ?? "") { (success, authError, error) in
            if success != nil{
                self.textView.text = ""
                self.sendBtn.isEnabled = false
                self.sendBtn.setImage(#imageLiteral(resourceName: "right-arrow"), for: .normal)
                self.textView.resignFirstResponder()
                for i in success!.data{
                    self.commentReplies.append(i)
                }
                self.tableView.reloadData()
            }
        }
    }
    
    
    private func likeComment(commentId: String,reaction_type: String){
        LikeBlogCommentManager.sharedInstance.likeComment(type: "reply_like", comment_id: commentId, blogId: self.blod_id ?? "", reactionType: reaction_type) { (success, authError, error) in
            if success != nil{
                print(success?.type)
            }
            else if authError != nil{
                self.view.makeToast(authError?.errors.errorText)
            }
            else if error != nil{
                self.view.makeToast(error?.localizedDescription)
            }
        }
    }
    
    
    @IBAction func Send(_ sender: Any) {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            self.createCommentReply()
        }
    }
    
    
    @IBAction func Back(_ sender: Any) {
        NotificationCenter.default.removeObserver(self)
        self.dismiss(animated: true, completion: nil)
    }
    
}
extension BlogCommentReplyController: UITableViewDelegate,UITableViewDataSource{
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
            if let publisher = self.comment["user_data"] as? [String:Any]{
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
            if let isLiked = self.comment["is_comment_liked"] as? Bool{
                if isLiked{
                    cell.likeBtn.setTitle("Liked", for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                }
            }
            cell.likeBtn.tag = indexPath.row
            //             cell.likeBtn.addTarget(self, action: #selector(self.LikeComments(sender:)), for: .touchUpInside)
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
            if let publisher = index["user_data"] as? [String:Any]{
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
            if let isLiked = index["is_comment_liked"] as? Bool{
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
    
    
    @IBAction func LikeComment(sender:UIButton){
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
                    self.likeComment(commentId: commentId ?? "", reaction_type: "dislike")
                    self.commentReplies[sender.tag]["is_comment_liked"] = false
                }
                else {
                    cell.likeBtn.setTitle("Liked", for: .normal)
                    cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                    self.likeComment(commentId: commentId ?? "", reaction_type: "like")
                    self.commentReplies[sender.tag]["is_comment_liked"] = true
                }
            }
            
        }
    }
}

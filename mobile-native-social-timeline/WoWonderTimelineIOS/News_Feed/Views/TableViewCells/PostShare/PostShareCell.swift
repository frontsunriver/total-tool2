//
//  PostShareCell.swift
//  News_Feed
//
//  Created by Ubaid Javaid on 2/25/20.
//

import UIKit
import ActiveLabel

class PostShareCell: UITableViewCell {

    var targetController : UIViewController!
    
    @IBOutlet weak var shareUserName: UILabel!
    @IBOutlet weak var sharedTimeLabel: UILabel!
    @IBOutlet weak var sharedMoreBtn: UIButton!
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var likesCountBtn: UIButton!
    @IBOutlet weak var commentsCountBtn: UIButton!
    @IBOutlet weak var LikeBtn: UIButton!
    @IBOutlet weak var CommentBtn: UIButton!
    @IBOutlet weak var sharedPostText: ActiveLabel!
    @IBOutlet weak var tableViewHeight: NSLayoutConstraint!
    @IBOutlet weak var sharedProfileImage: Roundimage!
    
    var sharePostArray = [[String:Any]]()
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.tableView.delegate = self
        self.tableView.dataSource = self
        self.tableView.backgroundColor = .white
//        self.tableViewHeight.constant = 400.0
        SetUpcells.setupCells(tableView: self.tableView)
    }
    
    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
        
        // Configure the view for the selected state
    }
}

extension PostShareCell: UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return 1
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        var tableViewCells = UITableViewCell()
        let index = self.sharePostArray[indexPath.row]
        var shared_info : [String:Any]? = nil
        var fundDonation: [String:Any]? = nil
        
//        var postFile = ""
//        if let postfile = index["postFile"] as? String{
//            postFile = "\("https://wowonder.fra1.digitaloceanspaces.com/")\(postfile))"
//        }
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
        if let sharedInfo = index["shared_info"] as? [String:Any] {
            shared_info = sharedInfo
        }
        if let fund = index["fund_data"] as? [String:Any]{
            fundDonation = fund
        }
        
        if (postfile != "")  {
            var postFile = "\("https://wowonder.fra1.digitaloceanspaces.com/")\(postfile))"
            print("postFile",postFile)
            postFile = postFile.replacingOccurrences(of: ")", with: "", options: NSString.CompareOptions.literal, range: nil)

            let url = URL(string: postFile)
            let urlExtension: String? = url?.pathExtension
            print(urlExtension)
            if (urlExtension == "jpg" || urlExtension == "png" || urlExtension == "jpeg" || urlExtension == "JPG" || urlExtension == "PNG" || urlExtension == "jpeg)"){
                print("NewsFeed",indexPath.row)
                tableViewCells = GetPostWithImage.sharedInstance.getPostImage(targetController: targetController, tableView: tableView, indexpath: indexPath, postFile: postFile, array: self.sharePostArray, url: url!, stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
            }
                
            else if(urlExtension == "wav" ||  urlExtension == "mp3" || urlExtension == "MP3"){
                tableViewCells = GetPostMp3.sharedInstance.getMP3(targetController: targetController, tableView: tableView, indexpath: indexPath, postFile: postFile, array: self.sharePostArray, stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
            }
            else if (urlExtension == "pdf") {
                tableViewCells = GetPostPDF.sharedInstance.getPostPDF(targetControler: targetController, tableView: self.tableView, indexpath: indexPath, postfile: postFile, array: self.sharePostArray,stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
                
            }
                
            else {
                tableViewCells = GetPostVideo.sharedInstance.getVideo(targetController: targetController, tableView: tableView, indexpath: indexPath, postFile: postFile, array: self.sharePostArray, url: url!, stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
            }
            
            
        }
            
        else if (postLink != "") {
            tableViewCells = GetPostWithLink.sharedInstance.getPostLink(targetController: targetController, tableView: tableView, indexpath: indexPath, postLink: postLink, array: self.sharePostArray,stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
        }
            
        else if (postYoutube != "") {
            tableViewCells = GetPostYoutube.sharedInstance.getPostYoutub(targetController: targetController, tableView: tableView, indexpath: indexPath, postLink: postYoutube, array: self.sharePostArray,stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
            
        }
        else if (blog != "0") {
            tableViewCells = GetPostBlog.sharedInstance.GetBlog(targetController: targetController, tableView: tableView, indexpath: indexPath, postFile: "", array: self.sharePostArray, stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
        }
            
        else if (group != false){
            tableViewCells = GetPostGroup.sharedInstance.GetGroupRecipient(targetController: targetController, tableView: tableView, indexpath: indexPath, postFile: "", array: self.sharePostArray, stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
        }
            
        else if (product != "0") {
            tableViewCells = GetPostProduct.sharedInstance.GetProduct(targetController: targetController, tableView: tableView, indexpath: indexPath, postFile: "", array: self.sharePostArray, stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
        }
        else if (event != "0") {
            tableViewCells = GetPostEvent.sharedInstance.getEvent(targetController: targetController, tableView: tableView, indexpath: indexPath, postFile: "", array:  self.sharePostArray, stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
            
        }
        else if (postSticker != "") {
            tableViewCells = GetPostSticker.sharedInstance.getPostSticker(targetController: targetController, tableView: tableView, indexpath: indexPath, postFile: "", array: self.sharePostArray, stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
            
        }
            
        else if (colorId != "0"){
            tableViewCells = GetPostWithBg_Image.sharedInstance.postWithBg_Image(targetController: targetController, tableView: tableView, indexpath: indexPath, array: self.sharePostArray, stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
        }
            
        else if (multi_image != "0") {
            tableViewCells = GetPostMultiImage.sharedInstance.getMultiImage(tableView: tableView, indexpath: indexPath, array: self.sharePostArray)
            
        }
            
        else if photoAlbum != "" {
            tableViewCells = getPhotoAlbum.sharedInstance.getPhoto_Album(targetController: targetController, tableView: tableView, indexpath: indexPath, array: self.sharePostArray, stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
        }
            
        else if postOptions != "0" {
            tableViewCells = GetPostOptions.sharedInstance.getPostOptions(targertController: targetController, tableView: tableView, indexpath: indexPath, array: self.sharePostArray, stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
        }
            
        else if postRecord != ""{
            tableViewCells = GetPostRecord.sharedInstance.getPostRecord(targetController: targetController, tableView: tableView, indexpath: indexPath, array: self.sharePostArray, stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
            
        }
        else if fundDonation != nil{
        tableViewCells = GetDonationPost.sharedInstance.getDonationpost(targetController: targetController, tableView: tableView, indexpath: indexPath, array: self.sharePostArray, stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
        }
        else {
            tableViewCells = GetNormalPost.sharedInstance.getPostText(targetController: targetController, tableView: self.tableView, indexpath: indexPath, postFile: "", array: self.sharePostArray, stackViewHeight: 0.0, viewHeight: 0.0, isHidden: true, viewColor: .white)
        }
        print("Row Height", tableViewCells.bounds.height)
        self.tableViewHeight.constant = tableViewCells.bounds.height
        print("Height",self.tableViewHeight.constant)
        self.sharePostArray.removeAll()
        tableViewCells.layoutIfNeeded()
        tableViewCells.clipsToBounds = true
        return tableViewCells
    }
}

//
//  UserDonationCell.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/14/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class UserDonationCell: UITableViewCell {
    
    
    @IBOutlet weak var userImage: Roundimage!
    @IBOutlet weak var amountLabel: UILabel!
    @IBOutlet weak var monthLabel: UILabel!
    @IBOutlet weak var followingLabel: RoundButton!
    @IBOutlet weak var onlineView: DesignView!
    @IBOutlet weak var nameLabel: UILabel!
    
    var vc: ShowFundingDetailsVC?
    let status = Reach().connectionStatus()
    var user_id: String? = nil
    override func awakeFromNib() {
        super.awakeFromNib()
        self.followingLabel.setTitle(NSLocalizedString("Follow", comment: "Follow"), for: .normal)
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }

  func  bind(index: [String:Any]){
    var names = ""
    var isPro = ""
    var isVerified = ""
    
    if let userData = index["user_data"] as? [String:Any]{
        if let userId = index["user_id"] as? String{
            self.user_id = userId
        }
        if let name = userData["name"] as? String{
            names = name
        }
        if let image = userData["avatar"] as? String{
            let url = URL(string: image)
            self.userImage.kf.setImage(with: url)
        }
        if let is_pro = userData["is_pro"] as? String{
            isPro = is_pro
        }
        if let is_verify = userData["verified"] as? String{
            isVerified = is_verify
        }
        if let lastseen = userData["lastseen_status"] as? String{
            if lastseen == "off"{
                self.onlineView.backgroundColor = .lightGray
            }
            else{
                self.onlineView.backgroundColor = .systemGreen
            }
        }
//        if let time = userData[""]
    }
    if let amount = index["amount"] as? String{
        self.amountLabel.text = "\("$")\(amount)"
    }

    let imageAttachment =  NSTextAttachment()
    let imageAttachment1 =  NSTextAttachment()
    imageAttachment.image = UIImage(named:"veirfied")
    imageAttachment1.image = UIImage(named: "flash-1")
    let imageOffsetY: CGFloat = -2.0
    imageAttachment.bounds = CGRect(x: 0, y: imageOffsetY, width: imageAttachment.image!.size.width, height: imageAttachment.image!.size.height)
    imageAttachment1.bounds = CGRect(x: 0, y: imageOffsetY, width: 11.0, height: 14.0)
    let attechmentString = NSAttributedString(attachment: imageAttachment)
    let attechmentString1 = NSAttributedString(attachment: imageAttachment1)
    let attrs1 = [NSAttributedString.Key.foregroundColor : UIColor.black]
    let attrs2 = [NSAttributedString.Key.foregroundColor : UIColor.white]
    let attributedString1 = NSMutableAttributedString(string: names, attributes:attrs1)
    let attributedString2 = NSMutableAttributedString(string: " ", attributes:attrs2)
    let attributedString3 = NSMutableAttributedString(attributedString: attechmentString)
    let attributedString4 = NSMutableAttributedString(string: " ", attributes:attrs2)
    let attributedString5 = NSMutableAttributedString(attributedString: attechmentString1)
    attributedString1.append(attributedString2)
    if (isVerified == "1") && (isPro == "1"){
        attributedString1.append(attributedString3)
        attributedString1.append(attributedString4)
        attributedString1.append(attributedString5)
    }
    else if (isVerified == "1"){
        attributedString1.append(attributedString3)
        attributedString1.append(attributedString4)
    }
    else if (isPro == "1"){
        attributedString1.append(attributedString5)
    }
    self.nameLabel.attributedText = attributedString1
    }
    
    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
    
    private func follow_unfollowRequest(userId : String){
        switch status {
        case .unknown, .offline:
            self.vc?.view.makeToast("Internet Connection Faield")
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                Follow_RequestManager.sharedInstance.sendFollowRequest(userId: userId) { (success, authError, error) in
                    if success != nil {
                        self.vc?.view.makeToast(success?.follow_status)
                    }
                    else if authError != nil {
                        self.vc?.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        self.vc?.view.makeToast(error?.localizedDescription)
                    }
                    
                }
            }
        }
    }
    
    @IBAction func Follow(_ sender: Any) {

        
//        if let userId = self.userData?["user_id"] as? String{
//            user_id = userId
//        }
//        if let is_following = self.userData?["is_following"] as? Int{
//            if is_following == 0{
//                cell.addButton.setImage(#imageLiteral(resourceName: "check"), for: .normal)
//                cell.addButton.backgroundColor = .white
//                self.sendRequest(user_id: user_id ?? "")
//                self.userData?["is_following"] = 1
//            }
//            else {
//                cell.addButton.setImage(#imageLiteral(resourceName: "Shape"), for: .normal)
//                cell.addButton.backgroundColor = UIColor.hexStringToUIColor(hex: "#6665FC")
//                self.sendRequest(user_id: user_id ?? "")
//                self.userData?["is_following"] = 0
//
//            }
//
    }
    
}

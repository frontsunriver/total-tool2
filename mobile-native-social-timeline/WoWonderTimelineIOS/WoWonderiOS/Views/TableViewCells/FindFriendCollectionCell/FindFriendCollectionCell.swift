//
//  FindFriendCollectionCell.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/13/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class FindFriendCollectionCell: UICollectionViewCell {
    @IBOutlet weak var profileImage: Roundimage!
    @IBOutlet weak var userNameLabel: UILabel!
     @IBOutlet weak var timeLabel: UILabel!
    @IBOutlet weak var followBtn: RoundButton!
    var vc:FindFriendVC?
    var data = [String:Any]()

    var userId:String? = ""
    
    override func awakeFromNib() {
      if (AppInstance.instance.connectivity_setting == "1"){
        self.followBtn.setTitle(NSLocalizedString("AddFriend", comment: "AddFriend"), for: .normal)
        }
      else{
           self.followBtn.setTitle(NSLocalizedString("Follow", comment: "Follow"), for: .normal)
        }
    }
    
    @IBAction func followPressed(_ sender: Any) {
        self.follow_unfollowRequest(userId: self.userId ?? "")

    }
    func bind(object:[String:Any]){
        self.data = object
        self.userNameLabel.text = object["username"] as? String
       var imageString = object["avatar"] as? String
        let url = URL(string: imageString ?? "")
        self.profileImage.kf.indicatorType = .activity
        self.profileImage.kf.setImage(with: url)
        self.userId = object["user_id"] as? String
        
        if let isFollowing = object["is_following"] as? String{
            if isFollowing == "no"{
                if (AppInstance.instance.connectivity_setting == "1"){
                    self.followBtn.setTitle(NSLocalizedString("AddFriend", comment: "AddFriend"), for: .normal)
                }
                else{
                 self.followBtn.setTitle(NSLocalizedString("Follow", comment: "Follow"), for: .normal)
                }
                 self.followBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                 self.followBtn.backgroundColor = .white
            }
            else if (isFollowing == "yes"){
               if (AppInstance.instance.connectivity_setting == "1"){
                     self.followBtn.setTitle(NSLocalizedString("Requested", comment: "Requested"), for: .normal)
                    self.followBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                    self.followBtn.backgroundColor = .white
                }
               else{
                   self.followBtn.setTitle(NSLocalizedString("following", comment: "following"), for: .normal)
                       self.followBtn.setTitleColor(.white, for: .normal)
                        self.followBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                }
            }
                
            else {
                if (AppInstance.instance.connectivity_setting == "0"){
                     self.followBtn.setTitle(NSLocalizedString("following", comment: "following"), for: .normal)
                }
                else{
                 self.followBtn.setTitle(NSLocalizedString("Friends", comment: "Friends"), for: .normal)
            }
         self.followBtn.setTitleColor(.white, for: .normal)
         self.followBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
        }
        }
        
        
    }
    private func follow_unfollowRequest(userId : String){
               performUIUpdatesOnMain {
                Follow_RequestManager.sharedInstance.sendFollowRequest(userId: userId) { (success, authError, error) in
                    if success != nil {
                        if success?.follow_status ?? "" == "followed"{
                            if (AppInstance.instance.connectivity_setting == "0"){
                                self.data["is_following"] = "yes"
                                self.followBtn.setTitle(NSLocalizedString("following", comment: "following"), for: .normal)
                                self.followBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                                self.followBtn.setTitleColor(UIColor.white, for: .normal)

                            }
                            else{
                                if (self.data["is_following"] as? String == "no"){
                                    self.followBtn.backgroundColor = UIColor.white
                                    self.followBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                                    self.data["is_following"] = "yes"
                                    self.followBtn.setTitle(NSLocalizedString("Requested", comment: "Requested"), for: .normal)
                                }
                                else{
                                    self.data["is_following"] = "yes"
                                    self.followBtn.setTitle(NSLocalizedString("MyFriend", comment: "MyFriend"), for: .normal)
                                    self.followBtn.setTitleColor(UIColor.white, for: .normal)
                                     self.followBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")

                                }
                            }

                            
                        }
                        else{
                            if (AppInstance.instance.connectivity_setting == "0"){
                                 self.data["is_following"] = 0
                                self.followBtn.setTitle(NSLocalizedString("follow", comment: "follow"), for: .normal)
                                self.followBtn.backgroundColor = .white
                                self.followBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                                
                            }
                            else{
                                self.data["is_following"] = 0
                                self.followBtn.setTitle(NSLocalizedString("AddFriend", comment: "AddFriend"), for: .normal)
                                self.followBtn.backgroundColor = .white
                                self.followBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                            }
                        }
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

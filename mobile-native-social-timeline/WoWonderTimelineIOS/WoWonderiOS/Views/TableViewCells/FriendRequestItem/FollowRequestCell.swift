//
//  FollowRequestCell.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/4/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class FollowRequestCell: UITableViewCell {
    
    @IBOutlet weak var lastSeen: UILabel!
    @IBOutlet weak var userName: UILabel!
    @IBOutlet weak var profileImage: Roundimage!
    @IBOutlet weak var acceptBtn: RoundButton!
    @IBOutlet weak var declineBtn: RoundButton!
    
    var vc : FollowRequestController?

    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    
    private func accept_declineRequest(user_id: String,action:String){
//        if Connectivity.isConnectedToNetwork(){
            FollowRequestActionManager.sharedInstance.followRequestAction(user_Id: user_id, action: action) { (success, authError, error) in
                if success != nil {
                    print(success?.api_status)
                }
                else if (authError != nil){
                    self.vc?.view.makeToast(authError?.errors.errorText)
                }
                else if (error != nil){
                    self.vc?.view.makeToast(error?.localizedDescription)
                }
            }
//        }

    }
    
    @IBAction func Accept(_ sender: UIButton) {
        let user_Id =  self.vc?.friend_Requests[sender.tag]["user_id"] as? String
         print(user_Id)
         self.accept_declineRequest(user_id: user_Id ?? "", action: "accept")
         self.vc?.friend_Requests.remove(at: sender.tag)
         self.vc?.delegate?.follow_request(index: sender.tag)
         if self.vc?.friend_Requests.count == 0{
             self.vc?.tableView.isHidden = true
         }
         else{
             self.vc?.tableView.isHidden = false
         }
         self.vc?.tableView.reloadData()
    }
    
    
    @IBAction func Reject(_ sender: UIButton) {
            let user_Id =  self.vc?.friend_Requests[sender.tag]["user_id"] as? String
            print(user_Id)
            self.accept_declineRequest(user_id: user_Id ?? "", action: "decline")
            self.vc?.friend_Requests.remove(at: sender.tag)
            self.vc?.delegate?.follow_request(index: sender.tag)
            if self.vc?.friend_Requests.count == 0{
                self.vc?.tableView.isHidden = true
            }
            else{
                self.vc?.tableView.isHidden = false
            }
            self.vc?.tableView.reloadData()
        }
    }
    


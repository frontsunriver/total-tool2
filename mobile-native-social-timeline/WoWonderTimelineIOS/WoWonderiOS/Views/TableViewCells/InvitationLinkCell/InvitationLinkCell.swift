//
//  InvitationLinkCell.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/7/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class InvitationLinkCell: UITableViewCell {
    
    @IBOutlet weak var linkBtn: UIButton!
    @IBOutlet weak var userName: UILabel!
    @IBOutlet weak var dateLbl: UILabel!
    
    var link = ""
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }
    
    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
        
        // Configure the view for the selected state
    }
    
    func bind(index: [String:Any]){
        
        if let name = index["user_name"] as? String{
            self.userName.text = name
        }
        if let date = index["time"] as? String{
            print("UTC Time",date)
            let epocTime = TimeInterval(Int(date) ?? 1601815559)
            let myDate = NSDate(timeIntervalSince1970: epocTime)
            let formate = DateFormatter()
            formate.dateFormat = "yyyy-MM-dd"
            let dat = formate.string(from: myDate as Date)
            print("Date",dat)
            print("Converted Time \(myDate)")
            self.dateLbl.text = "\(dat)"
            
        }
        if let link_url = index["link"] as? String{
            self.link = link_url
        }
        
    }
    
    @IBAction func CopyLink(_ sender: Any) {
        print("Link",self.link)
        UIPasteboard.general.string = self.link
    }
    
}

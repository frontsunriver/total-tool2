//
//  CommonThingsCollectionCell.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/17/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class CommonThingsCollectionCell: UICollectionViewCell {
    
    @IBOutlet weak var thumbnailImage: Roundimage!
      @IBOutlet weak var usernameLabel: UILabel!
     @IBOutlet weak var commonThingCountLabel: UILabel!

      func bind(object:[String:Any]){
        let value = object["user_data"] as? [String:Any]
        let firsName = value!["first_name"] as? String
        let lastName = value!["last_name"] as? String
        let username = value!["username"] as? String
        if firsName ?? "" == "" && lastName ?? "" == ""{
            self.usernameLabel.text = username
            
        }else{
            self.usernameLabel.text = "\(firsName ?? "") \(lastName ?? "")"
        }
        var commonThingCount = object["common_things"] as? Int
        self.commonThingCountLabel.text = "\(commonThingCount ?? 0 ) common"
         var imageString = value!["avatar"] as? String
          let url = URL(string: imageString ?? "")
          self.thumbnailImage.kf.indicatorType = .activity
          self.thumbnailImage.kf.setImage(with: url)
          
      }
     
    
}

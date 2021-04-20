//
//  GamesCollectionCell.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/15/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class GamesCollectionCell: UICollectionViewCell {
    @IBOutlet weak var thumbnailImage: Roundimage!
    @IBOutlet weak var titleLabel: UILabel!
    var object:[String:Any] = [:]
    
    var myGamesVc:MyGamesVC?
    var gamesVc:GsmesVC?

    func bind(object:[String:Any]){
        self.object = object
        self.titleLabel.text = object["game_name"] as? String
       var imageString = object["game_avatar"] as? String
        let url = URL(string: imageString ?? "")
        self.thumbnailImage.kf.indicatorType = .activity
        self.thumbnailImage.kf.setImage(with: url)
    }
   
    @IBAction func playPressed(_ sender: Any) {
        let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
                        let vc = storyboard.instantiateViewController(withIdentifier: "ShowGameVC") as! ShowGameVC
              vc.gameLink = self.object["game_link"] as? String
        if self.gamesVc != nil{
             self.gamesVc?.navigationController?.pushViewController(vc, animated: true)
        }else if self.myGamesVc != nil{
            self.myGamesVc?.navigationController?.pushViewController(vc, animated: true)
        }
       
        
    }
}

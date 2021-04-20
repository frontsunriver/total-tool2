//
//  JobVCCollectionCell.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/21/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class JobVCCollectionCell: UICollectionViewCell {
    @IBOutlet weak var thumbnailImage: Roundimage!
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var descriptionLabel: UILabel!
    @IBOutlet weak var MoneyLabel: UILabel!
    
    var vc:JobsVC?
    var object:[String:Any] = [:]
    
    func bind(object:[String:Any]){
        self.object = object
        let title = object["title"] as? String
        let description = object["description"] as? String
        let minimum = object["minimum"] as? String
        let maxium = object["maximum"] as? String
        let category = object["category"] as? String
        var imageString = object["image"] as? String
        
        self.titleLabel.text = title?.htmlToString ?? ""
        self.descriptionLabel.text = description?.htmlToString ?? ""
        let siteSetting = AppInstance.instance.siteSettings
        if let cat = siteSetting["job_categories"] as? [String:Any]{
            if let cate_name = cat[category ?? ""] as? String{
                self.MoneyLabel.text = "$\(minimum ?? "") - $\(maxium ?? "").\(cate_name)"
            }
        }
        let url = URL(string: imageString ?? "")
        self.thumbnailImage.kf.indicatorType = .activity
        self.thumbnailImage.kf.setImage(with: url)
        
    }
    @IBAction func ApplyNowPressed(_ sender: Any) {
        let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "ApplyJobVC") as! ApplyJobVC
        vc.jobId = self.object["id"] as? String
        vc.titleString = self.object["title"] as? String
        self.vc?.navigationController?.pushViewController(vc, animated: true)
    }
    
}

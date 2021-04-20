//
//  NearByCollectionCell.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/22/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class NearByCollectionCell  : UICollectionViewCell {
    @IBOutlet weak var thumbnailImage: Roundimage!
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var descriptionLabel: UILabel!
    @IBOutlet weak var MoneyLabel: UILabel!
    
    var vc:NearByBusinessVC?
    var object:[String:Any] = [:]
    var urlImage:String? = ""

    
    func bind(object:[String:Any]){
        self.object = object
        let job = object["job"] as? [String:Any]
        let title = job!["title"] as? String
        let description = job!["description"] as? String
        let minimum = job!["minimum"] as? String
        let maxium = job!["maximum"] as? String
        let category = job!["category"] as? String
        var imageString = job!["image"] as? String
        var imageType = job!["image_type"] as? String
        if imageType ?? "" == "upload"{
            self.urlImage = "https://wowonder.fra1.digitaloceanspaces.com/\(imageString ?? "")"
        }else {
            self.urlImage = imageString ?? ""
        }
        
        
        self.titleLabel.text = title?.htmlToString ?? ""
        self.descriptionLabel.text = description?.htmlToString ?? ""
        let siteSetting = AppInstance.instance.siteSettings
        if let cat = siteSetting["job_categories"] as? [String:Any]{
            if let cate_name = cat[category ?? ""] as? String{
                self.MoneyLabel.text = "$\(minimum ?? "") - $\(maxium ?? "").\(cate_name)"
            }
        }
        let url = URL(string: urlImage ?? "")
        self.thumbnailImage.kf.indicatorType = .activity
        self.thumbnailImage.kf.setImage(with: url)
        
    }
    @IBAction func ApplyNowPressed(_ sender: Any) {
        let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "ApplyJobVC") as! ApplyJobVC
            vc.jobId = self.object["job_id"] as? String
               let jobObject = object["job"] as! [String:Any]
               vc.titleString = jobObject["title"] as? String
        self.vc?.navigationController?.pushViewController(vc, animated: true)
    }
    
}

//
//  JobDetailsVC.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/22/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class JobDetailsVC: UIViewController {
    
    @IBOutlet weak var pageImage: Roundimage!
    @IBOutlet weak var thumbnailImage: UIImageView!
    @IBOutlet weak var descriptionLabel: UILabel!
    @IBOutlet weak var maximumLabel: UILabel!
    @IBOutlet weak var minimumLabel: UILabel!
    @IBOutlet weak var categorylabel: UILabel!
    @IBOutlet weak var timeLabel: UILabel!
    @IBOutlet weak var locationLabel: UILabel!
    @IBOutlet weak var typeLabel: UILabel!
    @IBOutlet weak var pageNameLabel: UILabel!
    @IBOutlet weak var titleLabel: UILabel!
    
    var object: [String:Any]  = [:]
    var type:String? = ""
    override func viewDidLoad() {
        super.viewDidLoad()
        self.setupUI()
    }
    
    
    private func setupUI(){
        if type == "business"{
            let pageObject = object["page_data"] as? [String:Any]
            let jobObject = object["job"] as! [String:Any]
            let pageImage = pageObject!["avatar"] as? String
            let ThumnailImage = jobObject["image"] as? String
            
            
            let pageURL = URL(string: pageImage ?? "")
            self.pageImage.kf.indicatorType = .activity
            self.pageImage.kf.setImage(with: pageURL)
            
            let thumbImage = URL(string: ThumnailImage ?? "")
            self.thumbnailImage.kf.indicatorType = .activity
            self.thumbnailImage.kf.setImage(with: thumbImage)
            
            let title = jobObject["title"] as? String
            let description  = jobObject["description"] as? String
            let minumum = jobObject["minimum"] as? String
            let maximum = jobObject["maximum"] as? String
            let location = jobObject["location"] as? String
            let category = jobObject["category"] as? String
            let jobType = jobObject["job_type"] as? String
            let pageName = pageObject!["page_name"] as? String
            
            self.titleLabel.text = title ?? ""
            self.descriptionLabel.text = description ?? ""
            self.minimumLabel.text = "\(minumum ?? "" ) PER HOUR"
            self.maximumLabel.text = "\(maximum ?? "" ) PER HOUR"
            self.pageNameLabel.text = "@\(pageName ?? "")"
            self.locationLabel.text = location ?? ""
            let siteSetting = AppInstance.instance.siteSettings
            if let cat = siteSetting["job_categories"] as? [String:Any]{
                if let cate_name = cat[category ?? ""] as? String{
                    self.categorylabel.text = cate_name
                }
            }
//            self.categorylabel.text =  AppInstance.instance.siteSettings?.config?.jobCategories![category ?? ""] ?? ""
            self.typeLabel.text = jobType ?? ""
        }else{
            let pageObject = object["page"] as? [String:Any]
            let pageImage = pageObject!["avatar"] as? String
            let ThumnailImage = object["image"] as? String
            
            
            let pageURL = URL(string: pageImage ?? "")
            self.pageImage.kf.indicatorType = .activity
            self.pageImage.kf.setImage(with: pageURL)
            
            let thumbImage = URL(string: ThumnailImage ?? "")
            self.thumbnailImage.kf.indicatorType = .activity
            self.thumbnailImage.kf.setImage(with: thumbImage)
            
            let title = object["title"] as? String
            let description  = object["description"] as? String
            let minumum = object["minimum"] as? String
            let maximum = object["maximum"] as? String
            let location = object["location"] as? String
            let category = object["category"] as? String
            let jobType = object["job_type"] as? String
            let pageName = pageObject!["page_name"] as? String
            
            self.titleLabel.text = title ?? ""
            self.descriptionLabel.text = description ?? ""
            self.minimumLabel.text = "\(minumum ?? "" ) PER HOUR"
            self.maximumLabel.text = "\(maximum ?? "" ) PER HOUR"
            self.pageNameLabel.text = "@\(pageName ?? "")"
            self.locationLabel.text = location ?? ""
            let siteSetting = AppInstance.instance.siteSettings
            if let cat = siteSetting["job_categories"] as? [String:Any]{
                if let cate_name = cat[category ?? ""] as? String{
                    self.categorylabel.text = cate_name
                }
            }
            self.typeLabel.text = jobType ?? ""
        }
        
        
        
    }
    
    @IBAction func applyNowPressed(_ sender: Any) {
        let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "ApplyJobVC") as! ApplyJobVC
        if  type == "business"{
            vc.jobId = self.object["job_id"] as? String
            let jobObject = object["job"] as! [String:Any]
            vc.titleString = jobObject["title"] as? String
        }else{
            vc.jobId = self.object["id"] as? String
            vc.titleString = self.object["title"] as? String
        }
        
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
}

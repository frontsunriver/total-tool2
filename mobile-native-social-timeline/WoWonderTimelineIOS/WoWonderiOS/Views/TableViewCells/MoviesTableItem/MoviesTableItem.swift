//
//  MoviesTableItem.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/23/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class MoviesTableItem: UITableViewCell {
    @IBOutlet weak var descriptionLabel: UILabel!
    
    @IBOutlet weak var timeLabel: UILabel!
    @IBOutlet weak var viewsLabel: UILabel!
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var thumbnailImage: Roundimage!
    
    
    
    var vc: MoviesVC?
    var object: [String:Any] = [:]
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    func bind(object:[String:Any]){
        self.object = object
           let name = object["name"] as? String
           let description = object["description"] as? String
           let views = object["views"] as? String
        let timeDuration = object["duration"] as? String
        
        let image = object["cover"] as? String
           self.titleLabel.text = name?.htmlToString ?? ""
           self.descriptionLabel.text = description?.htmlToString ?? ""
           self.viewsLabel.text = "\(views ?? "") Views"
        self.timeLabel.text = Double(timeDuration ?? "0")?.asString(style: .positional)
           let url = URL(string: image ?? "")
           self.thumbnailImage.kf.indicatorType = .activity
           self.thumbnailImage.kf.setImage(with: url)
       }
    
    @IBAction func morePressed(_ sender: Any) {
        let alert = UIAlertController(title: nil, message: nil, preferredStyle: .actionSheet)
        let copyLink = UIAlertAction(title: NSLocalizedString("Copy Link", comment: "Copy Link"), style: .default) { (action) in
            UIPasteboard.general.string = self.object["url"] as? String
            self.vc?.view.makeToast("Copy to Clipboard")

        }
        let share = UIAlertAction(title: NSLocalizedString("Share", comment: "Share"), style: .default) { (action) in
            self.shareAcitvity(url: (self.object["url"] as? String)!)
            
        }
        
        let cancel = UIAlertAction(title: NSLocalizedString("Cancel", comment: "Cancel"), style: .default, handler: nil)
        alert.addAction(copyLink)
        alert.addAction(share)
        alert.addAction(cancel)
        self.vc?.present(alert, animated: true, completion: nil)
    }
    func shareAcitvity(url:String){
        let myWebsite = NSURL(string:url)
        let shareAll = [ myWebsite]
        let activityViewController = UIActivityViewController(activityItems: shareAll, applicationActivities: nil)
        activityViewController.popoverPresentationController?.sourceView = self
        self.vc?.present(activityViewController, animated: true, completion: nil)
    }
}

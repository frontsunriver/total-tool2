//
//  AboutTableItem.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/24/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class AboutTableItem: UITableViewCell {
    
    @IBOutlet weak var viewsLabel: UILabel!
    @IBOutlet weak var moreStack: UIStackView!
        @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var tagLabel: UILabel!
    @IBOutlet weak var starLabel: UILabel!
    @IBOutlet weak var categoryLabel: UILabel!
    @IBOutlet weak var descriptionLabel: UILabel!
    @IBOutlet weak var publishLabel: UILabel!
    @IBOutlet weak var shareStack: UIStackView!
    @IBOutlet weak var catGoryLabel: UILabel!
    
    var object:[String:Any] = [:]
    var vc:AboutVC?
    
    override func awakeFromNib() {
        super.awakeFromNib()
        let moreTap = UITapGestureRecognizer(target: self, action: #selector(self.moreStackPressed(_:)))
        moreStack.addGestureRecognizer(moreTap)
        
        let shareTap = UITapGestureRecognizer(target: self, action: #selector(self.shareStackPressed(_:)))
               shareStack.addGestureRecognizer(shareTap)
        
    }

    @objc func moreStackPressed(_ sender: UITapGestureRecognizer? = nil) {
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
    @objc func shareStackPressed(_ sender: UITapGestureRecognizer? = nil) {
        self.shareAcitvity(url: (self.object["url"] as? String)!)
        
        
      }
    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    func bind(object:[String:Any]){
        self.object = object
        self.titleLabel.text = object["name"] as? String
        let descriptionText =  object["description"] as? String
        self.descriptionLabel.text = descriptionText?.htmlToString
        self.tagLabel.text = object["producer"] as? String
        self.starLabel.text = object["stars"] as? String
        self.catGoryLabel.text = object["genre"] as? String
        self.categoryLabel.text = object["quality"] as? String
        self.viewsLabel.text = "\(object["views"] as? String ?? "") Views"
        self.publishLabel.text = "Published on \(object["release"] as? String ?? "")"
    }
    
    
    
}

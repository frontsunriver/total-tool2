//
//  VideoDetailsVC.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/24/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import MobilePlayer
import VersaPlayer
import XLPagerTabStrip
class VideoDetailsVC: ButtonBarPagerTabStripViewController {
    
    @IBOutlet weak var controls: VersaPlayerControls!
    @IBOutlet weak var playerView: VersaPlayerView!
    var url: String? = ""
    var object:[String:Any] = [:]
    
    override func viewDidLoad() {
        self.setupUI()
        super.viewDidLoad()
        self.playerView.autoplay = false
        self.load(url: self.url ?? "")
    }
    
    func load(url: String) {
        //let html = "<video playsinline controls width=\"100%\" height=\"100%\" src=\"\(url)\"> </video>"
        print("postFile = \(url)")
        DispatchQueue.main.async {
            self.playerView.layer.backgroundColor = UIColor.black.cgColor
            self.playerView.use(controls: self.controls)
            if let url = URL.init(string: url) {
                let item = VersaPlayerItem(url: url)
                self.playerView.set(item: item)
            }
        }
    }
    private func setupUI(){
        
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title =  self.object["name"] as? String
        
        let lineColor = UIColor.black
        settings.style.buttonBarItemBackgroundColor = .clear
        settings.style.selectedBarBackgroundColor = lineColor
        settings.style.buttonBarItemFont =  UIFont.systemFont(ofSize: 15.0)
        settings.style.selectedBarHeight = 2
        settings.style.buttonBarMinimumLineSpacing = 0
        settings.style.buttonBarItemTitleColor = .black
        settings.style.buttonBarItemsShouldFillAvailableWidth = true
        settings.style.buttonBarLeftContentInset = 0
        settings.style.buttonBarRightContentInset = 0
        let color = UIColor(red:26/255, green: 34/255, blue: 78/255, alpha: 0.4)
        let newCellColor = UIColor.black
        changeCurrentIndexProgressive = { [weak self] (oldCell: ButtonBarViewCell?, newCell: ButtonBarViewCell?, progressPercentage: CGFloat, changeCurrentIndex: Bool, animated: Bool) -> Void in
            guard changeCurrentIndex == true else { return }
            oldCell?.label.textColor = color
            newCell?.label.textColor = newCellColor
            print("OldCell",oldCell)
            print("NewCell",newCell)
        }
    }
    override func viewControllers(for pagerTabStripController: PagerTabStripViewController) -> [UIViewController] {
        let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
        var AboutVC =  storyboard.instantiateViewController(withIdentifier: "AboutVC") as! AboutVC
        AboutVC.object = self.object
        var CommentVc =  storyboard.instantiateViewController(withIdentifier: "CommentVc") as! CommentVc
        CommentVc.moviesID = self.object["id"] as? String
        return [AboutVC,CommentVc]
        
    }
}

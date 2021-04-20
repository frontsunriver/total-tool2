//
//  GamesParentVC.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/15/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import XLPagerTabStrip
class GamesParentVC: ButtonBarPagerTabStripViewController {

    override func viewDidLoad() {
        self.setupUI()
        super.viewDidLoad()

        // Do any additional setup after loading the view.
    }
    

    private func setupUI(){
           
           self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
           
           let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
               navigationController?.navigationBar.titleTextAttributes = textAttributes
           self.navigationItem.largeTitleDisplayMode = .never
           self.navigationItem.title = NSLocalizedString("Games", comment: "Games")
           
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
           
           let SendMoneyVC =  storyboard.instantiateViewController(withIdentifier: "GsmesVC") as! GsmesVC
           let AddFundsVC =  storyboard.instantiateViewController(withIdentifier: "MyGamesVC") as! MyGamesVC
           return [SendMoneyVC,AddFundsVC]
           
       }
}

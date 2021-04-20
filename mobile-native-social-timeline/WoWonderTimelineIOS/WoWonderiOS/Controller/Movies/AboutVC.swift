//
//  AboutVC.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/24/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import XLPagerTabStrip
class AboutVC: UIViewController {

    @IBOutlet weak var tableView: UITableView!
    
    
    var object : [String:Any] = [:]
    override func viewDidLoad() {
        super.viewDidLoad()
        self.setupUI()
    }
    private func setupUI(){
        self.tableView.separatorStyle = .none
             self.tableView.register(UINib(nibName: "AboutTableItem", bundle: nil), forCellReuseIdentifier: "AboutTableItem")
    }
}
extension AboutVC : UITableViewDelegate,UITableViewDataSource{
func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
    return 1
}

func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
    let cell = tableView.dequeueReusableCell(withIdentifier: "AboutTableItem") as! AboutTableItem
    let object = self.object
    cell.vc = self
    cell.selectionStyle = .none
    cell.bind(object: self.object)
          return cell
}
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return UITableView.automaticDimension
    }
}
extension AboutVC:IndicatorInfoProvider{
    func indicatorInfo(for pagerTabStripController: PagerTabStripViewController) -> IndicatorInfo {
        return IndicatorInfo(title: "ABOUT")
    }
}

//
//  FollowRequestController.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/4/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class FollowRequestController: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var noUserView: UIView!
    
    var friend_Requests =  [[String:Any]]()
    
    var delegate: FollowRequestDelegate?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.tableView.delegate = self
        self.tableView.dataSource = self
        self.tableView.register(UINib(nibName: "FollowRequestCell", bundle: nil), forCellReuseIdentifier: "RequestCell")
        self.tableView.tableFooterView = UIView()
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("Follow Requests", comment: "Follow Requests")
    }
    
    override func viewWillAppear(_ animated: Bool) {
        if self.friend_Requests.count > 0 {
            self.tableView.isHidden = false
            
            self.tableView.reloadData()
        }
        else{
            self.tableView.isHidden = true
        }
        
    }
}


extension FollowRequestController:UITableViewDataSource,UITableViewDelegate{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.friend_Requests.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "RequestCell") as! FollowRequestCell
        let index = self.friend_Requests[indexPath.row]
        if let pro_url = index["avatar"] as? String{
            let url = URL(string: pro_url)
            cell.profileImage.sd_setImage(with: url, completed: nil)
        }
        if let lastseen = index["lastseen"] as? String{
            cell.lastSeen.text = lastseen
        }
        if let userName  = index["name"] as? String{
            cell.userName.text = userName
        }
        cell.acceptBtn.tag = indexPath.row
        cell.declineBtn.tag = indexPath.row
        cell.vc = self
        return cell
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 65.0
    }
    
}



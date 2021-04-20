//
//  InvitationLinkController.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/7/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class InvitationLinkController: UIViewController {
    
    
    @IBOutlet weak var availableLinkLbl: UILabel!
    @IBOutlet weak var generateLinkLbl: UILabel!
    @IBOutlet weak var usedLinkLbl: UILabel!
    @IBOutlet weak var availableLink: UILabel!
    @IBOutlet weak var generateLink: UILabel!
    @IBOutlet weak var usedLink: UILabel!
    @IBOutlet weak var generateBtn: RoundButton!
    @IBOutlet weak var urlLbl: UILabel!
    @IBOutlet weak var invitedLbl: UILabel!
    @IBOutlet weak var dateLbl: UILabel!
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    let status = Reach().connectionStatus()
    
    var invitationLinks = [[String:Any]]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.largeTitleDisplayMode = .never
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.title = "Invitation Links"
        self.tableView.tableFooterView = UIView()
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.tableView.register((UINib(nibName: "InvitationLinkCell", bundle: nil)), forCellReuseIdentifier: "invitationCell")
        self.tableView.delegate = self
        self.tableView.dataSource = self
        self.activityIndicator.startAnimating()
        self.getLinks()
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    private func getLinks(){
        switch status {
        case .unknown, .offline:
            self.activityIndicator.stopAnimating()
            self.view.makeToast("Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            GetInvitationLinkManager.sharedInstance.getLink { (success, authError, error) in
                if (success != nil){
                    self.invitationLinks.removeAll()
                    self.availableLink.text = "\(success?.available_links ?? 0)"
                    self.usedLink.text = success?.used_links ?? ""
                    self.generateLink.text = "\(success?.generated_links ?? 0)"
                    for i in success!.data{
                        self.invitationLinks.append(i)
                    }
                    self.activityIndicator.stopAnimating()
                    self.tableView.reloadData()
                }
                else if (authError != nil){
                     self.activityIndicator.stopAnimating()
                    self.view.makeToast(authError?.errors?.errorText)
                }
                else if (error != nil){
                    self.activityIndicator.stopAnimating()
                    self.view.makeToast(error?.localizedDescription)
                }
            }
        }
    }
    
    private func createLink(){
        switch status {
         case .unknown, .offline:
            self.activityIndicator.stopAnimating()
             self.view.makeToast("Internet Connection Failed")
         case .online(.wwan),.online(.wiFi):
            
            CreateLinkManager.sharedInstance.createLink { (success,authError, error) in
                if (success != nil){
                    self.getLinks()
                }
                else if (authError != nil){
                    self.view.makeToast(authError?.errors?.errorText)
                }
                else if (error != nil){
                    self.view.makeToast(error?.localizedDescription)
                }
            }
            
        }
    }
    
    
    
    @IBAction func GenerateLink(_ sender: Any) {
        self.createLink()
    }
    
}
extension InvitationLinkController: UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.invitationLinks.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "invitationCell") as! InvitationLinkCell
        let index = self.invitationLinks[indexPath.row]
        cell.bind(index: index)
        return cell
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 50.0
    }
    
}

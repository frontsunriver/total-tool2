//
//  MemoriesController.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/11/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class MemoriesController: UIViewController {
    
    
    var postsMemories = [[String:Any]]()
    var friendsMemories = [[String:Any]]()

    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.title = NSLocalizedString("Memories", comment: "Memories")
        self.navigationItem.largeTitleDisplayMode = .never
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    private func getMemories(){
        
        GetMemoriesManager.sharedInstance.getMemories(type: "all") { (success, authError, error) in
            if (success != nil){
                if let posts = success?.data["posts"] as? [[String:Any]]{
                    for i in posts{
                        self.postsMemories.append(i)
                    }
                }
                if let friends = success?.data["friends"] as? [[String:Any]]{
                    for i in friends{
                        self.friendsMemories.append(i)
                    }
                }
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
